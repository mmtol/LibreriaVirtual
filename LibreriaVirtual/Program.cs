using LibreriaVirtual.Repositories;
using Microsoft.EntityFrameworkCore;
using LibreriaVirtual.Repositories;
using LibreriaVirtual.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTransient<RepositoryUsuario>();
builder.Services.AddTransient<RepositoryContenido>();
string connString = builder.Configuration.GetConnectionString("SqlConnection");
builder.Services.AddDbContext<LibreriaVirtualContext>(options => options.UseSqlServer(connString));

var app = builder.Build();

app.UseStaticFiles();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}");

app.Run();
