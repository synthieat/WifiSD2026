
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using SD.Application.Extensions;
using SD.Persistence.Extensions;
using SD.Persistence.Repositories.DBContext;

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

            builder.Services.AddSwaggerGen(g =>
            {
                g.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Wifi SW-Developer 2025-2026 API",
                    Version = "v1",
                    Contact = new OpenApiContact { Email = "horst.schneider@hotmail.com", 
                                                   Url = new Uri("http://www.syntpop.at"), Name = "Horst Schneider"}

                    });
            });

            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();


            /* DBContext registrieren */
            var connectionString = builder.Configuration.GetConnectionString("MovieDbContext");
            builder.Services.AddDbContext<MovieDbContext>(options => options.UseSqlServer(connectionString));

            builder.Services.RegisterRepositories();
            builder.Services.RegisterApplicationServices();
            builder.Services.AddMediator(cfg => cfg.ServiceLifetime = ServiceLifetime.Scoped);



            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
