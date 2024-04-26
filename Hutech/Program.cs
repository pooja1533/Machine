using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Hutech.Data;
using Hutech.Models;
using Serilog;
using FluentValidation.AspNetCore;
using Hutech;
using Microsoft.AspNetCore.Authentication.Cookies;
using Elfie.Serialization;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using Microsoft.AspNetCore.Localization;
using System.Globalization;
using System.Reflection;
using Hutech.Services;
using Microsoft.Extensions.Options;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

//Resource 
builder.Services.AddSingleton<LanguageService>();
builder.Services.AddLocalization(o => o.ResourcesPath = "Resources");
builder.Services.AddMvc()
.AddViewLocalization()
    .AddDataAnnotationsLocalization(options =>
    {
        options.DataAnnotationLocalizerProvider = (type, factory) =>
        {
            var assemblyName = new AssemblyName(typeof(SharedResource).GetTypeInfo().Assembly.FullName);
            return factory.Create("SharedResource", assemblyName.Name);
        };
    });

builder.Services.Configure<RequestLocalizationOptions>(
    options =>
    {
        var supportedCultures = new[]
   {
                new CultureInfo("en-US"),
    };

        options.DefaultRequestCulture = new RequestCulture("en-US", "en-US");
        options.SupportedCultures = supportedCultures;
        options.SupportedUICultures = supportedCultures;

        options.RequestCultureProviders.Insert(0, new QueryStringRequestCultureProvider());
    });
//end reosurce

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllers(opt=> { opt.Filters.Add<LogActionAttribute>(); }).AddFluentValidation(x => x.RegisterValidatorsFromAssemblyContaining<Program>());
// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(x => x.LoginPath = "/Identity/Account/Login");

builder.Services.AddScoped<LogActionAttribute, LogActionAttribute>();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(80);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.Services.AddControllersWithViews().AddRazorPagesOptions(options => {
    options.Conventions.AddAreaPageRoute("Identity", "/Account/Login", "");
});

var env = builder.Environment;
if (env.IsDevelopment())
{

    builder.Host.UseSerilog((hostContext, services, configuration) => {
        configuration
            //.MinimumLevel.Debug()
            .WriteTo.File("serilog-file.txt")
            .WriteTo.Console();
    });
}
else
{
    builder.Host.UseSerilog((hostContext, services, configuration) => {
        configuration
            .WriteTo.File("serilog-file.txt")
            .WriteTo.Console();
    });
}
builder.Services.AddAuthorization();

var app = builder.Build();
app.UseRequestLocalization(app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value);

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
if (env.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");

}
app.UseExceptionHandler("/Error");
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession();
app.UseRouting();

app.UseStatusCodePages(async context =>
{
    if (context.HttpContext.Response.StatusCode == 400)
    {
        context.HttpContext.Response.Redirect("/Home/Error");
    }
    else if (context.HttpContext.Response.StatusCode == 404)
    {
        context.HttpContext.Response.Redirect("/Home/Error");
    }
    else if (context.HttpContext.Response.StatusCode == 500)
    {
        context.HttpContext.Response.Redirect("/Home/Error");
    }
});

app.UseAuthorization(); 
app.UseSession();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();
app.Run();
