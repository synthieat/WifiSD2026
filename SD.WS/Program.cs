
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using SD.Application.Extensions;
using SD.Persistence.Extensions;
using SD.Persistence.Repositories.DBContext;
using Microsoft.AspNetCore.OpenApi;
using Scalar.AspNetCore;
using SD.Common.Services;
using SD.Application.Authentication;
using Microsoft.AspNetCore.Authentication;

namespace SD.WS
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();

            builder.Services.AddEndpointsApiExplorer();

            var openApiSecuritySchema = new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.Http, /* Protokoll */
                Scheme = "basic", /* Authentication Scheme, basic, bearer */
                In = ParameterLocation.Header, /* In den Http-Header einfügen */
                Description = "Basic Authentication header using basic scheme" /* Beschreibung */
            };

            builder.Services.AddSwaggerGen(g =>
            {
                g.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Wifi SW-Developer 2025-2026 API",
                    Version = "v1",
                    Contact = new OpenApiContact { Email = "horst.schneider@hotmail.com", 
                                                   Url = new Uri("http://www.syntpop.at"), Name = "Horst Schneider"}

                });
                             

                g.AddSecurityDefinition("basic", openApiSecuritySchema);

                g.AddSecurityRequirement(document => new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecuritySchemeReference("basic", document, null),
                        new List<string>()
                    }
                });

            });

            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi(options =>
            {
                options.AddDocumentTransformer((document, context, cancellationToken) =>
                {
                    document.Info = new()
                    {
                        Title = "Wifi SW-Developer 2025-2026 API",
                        Version = "v1",
                        Contact = new()
                        {
                            Name = "Horst Schneider",
                            Email = "horst.schneider@hotmail.com",
                            Url = new Uri("http://www.syntpop.at")
                        }
                    };

                    // Basic Authentication für Scalar hinzufügen
                    document.Components ??= new();
                    document.Components.SecuritySchemes ??= new Dictionary<string, IOpenApiSecurityScheme>();
                    document.Components.SecuritySchemes["basic"] = openApiSecuritySchema;
                  
                    return Task.CompletedTask;
                });

                options.AddOperationTransformer((operation, context, cancellationToken) =>
                {
                    operation.Security ??= new List<OpenApiSecurityRequirement>();

                    var securityRequirement = new OpenApiSecurityRequirement();
                    securityRequirement.Add(new OpenApiSecuritySchemeReference("basic"), new List<string>());
                    operation.Security.Add(securityRequirement);

                    return Task.CompletedTask;
                });
            });


            /* DBContext registrieren */
            var connectionString = builder.Configuration.GetConnectionString("MovieDbContext");
            builder.Services.AddDbContext<MovieDbContext>(options => options.UseSqlServer(connectionString));

            /* User Service zur Service-Collection hinzugefügt. */
            builder.Services.AddScoped<IUserService, UserService>();

            /* BasicAuthentication Handler registrieren */
            builder.Services.AddAuthentication(nameof(BasicAuthenticationHandler))
                            .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>(nameof(BasicAuthenticationHandler), null);

            builder.Services.AddAuthorization();

            builder.Services.RegisterRepositories();
            builder.Services.RegisterApplicationServices();
            builder.Services.AddMediator(cfg => cfg.ServiceLifetime = ServiceLifetime.Scoped);



            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                /* Swagger */
                app.UseSwagger();
                app.UseSwaggerUI();

                /* Microsoft Scalar */
                app.MapOpenApi();          // /openapi/v1.json
                app.MapScalarApiReference(options =>
                {
                    options
                        .WithTitle("Wifi SW-Developer 2025-2026 API")
                        .WithTheme(ScalarTheme.Purple)
                        .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
                }); // /scalar
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();
            app.UseRouting();

            /* Uses für Authentifizierung und Autorisierung aufrufen
             * UseAuthentication immer vor UseAuthorization anführen */
            app.UseAuthentication();
            app.UseAuthorization();
            


            app.MapControllers();

            app.Run();
        }
    }
}
