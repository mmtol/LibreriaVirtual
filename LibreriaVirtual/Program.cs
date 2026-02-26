using LibreriaVirtual.Data;
using LibreriaVirtual.Helpers;
using LibreriaVirtual.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();

builder.Services.AddTransient<IRepositoryLibreria, RepositoryLibreria>();
builder.Services.AddSingleton<HelperPathProvider>();
string connString = builder.Configuration.GetConnectionString("SqlConnection")!;
builder.Services.AddDbContext<LibreriaVirtualContext>(options => options.UseSqlServer(connString));

var app = builder.Build();

app.UseStaticFiles();

app.UseSession();

app.MapControllerRoute
    (
        name: "default",
        pattern: "{controller=Account}/{action=Login}"
    );

app.Run();