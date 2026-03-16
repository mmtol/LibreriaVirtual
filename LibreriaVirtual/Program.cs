using LibreriaVirtual.Data;
using LibreriaVirtual.Helpers;
using LibreriaVirtual.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
}).AddCookie();

builder.Services.AddControllersWithViews
(options => options.EnableEndpointRouting = false)
.AddSessionStateTempDataProvider();

builder.Services.AddTransient<IRepositoryLibreria, RepositoryLibreria>();
builder.Services.AddSingleton<HelperPathProvider>();

string connString = builder.Configuration.GetConnectionString("SqlConnection")!;
builder.Services.AddDbContext<LibreriaVirtualContext>
(options => options.UseSqlServer(connString));

var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

app.UseMvc(routes =>
{
    routes.MapRoute(
        name: "default",
        template: "{controller=Home}/{action=Index}/{id?}");
});

app.Run();