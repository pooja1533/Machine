using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Hutech.Data;
using Hutech.Models;
using Serilog;
using FluentValidation.AspNetCore;
using Hutech;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllers(opt=> { opt.Filters.Add<LogActionAttribute>(); }).AddFluentValidation(x => x.RegisterValidatorsFromAssemblyContaining<RoleValidator>());

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

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession();
app.UseRouting();
app.UseAuthorization(); 
app.UseSession();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();
app.Run();
