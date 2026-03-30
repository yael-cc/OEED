using Microsoft.EntityFrameworkCore;
using OEED_ITT.Models;
using OEED_ITT.Helpers;
using DotNetEnv; 

var builder = WebApplication.CreateBuilder(args);

// 1. Cargar las variables del archivo .env
Env.Load();

// 2. Obtener la cadena de conexión desde el entorno
// Asegúrate de que en tu .env la variable se llame exactamente DB_CONNECTION_STRING
var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSingleton<UsuarioE>();

// 3. Configurar el DbContext usando la variable obtenida
builder.Services.AddDbContext<EventosInstitucionalesContext>(options =>
    options.UseSqlServer(connectionString));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Index}/{id?}");

app.Run();