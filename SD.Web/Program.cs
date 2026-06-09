using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SD.Application.Extensions;
using SD.Persistence.Extensions;
using SD.Persistence.Repositories.DBContext;
using SD.Web.Data;
using System.Globalization;



var builder = WebApplication.CreateBuilder(args);

/* Add services to the container. */
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();


/* Auslesen der Sprachen des Clients (Browser Request) */
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCultures = new List<CultureInfo>
    {
        new("de"),
        new("en")
    };

    options.DefaultRequestCulture = new Microsoft.AspNetCore.Localization.RequestCulture("en");
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;

});


/* DBContext registrieren */
var movieDbConnectionString = builder.Configuration.GetConnectionString("MovieDbContext");
builder.Services.AddDbContext<MovieDbContext>(options => options.UseSqlServer(movieDbConnectionString));


/* Registrierung der Repository und Handler Klassen */
builder.Services.RegisterRepositories();
builder.Services.RegisterApplicationServices();

/* Registrierung Mediator -> Extension erforder Referenz auf Nuget Package */
builder.Services.AddMediator(cfg => cfg.ServiceLifetime = ServiceLifetime.Scoped);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapRazorPages()
   .WithStaticAssets();

/* Browser-Culture Ermittlung aktivieren */
var options = app.Services.GetService<IOptions<RequestLocalizationOptions>>();
app.UseRequestLocalization(options.Value);

app.Run();
