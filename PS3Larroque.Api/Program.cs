using Microsoft.EntityFrameworkCore;
using PS3Larroque.Infrastructure;
using PS3Larroque.Application.Interfaces;
using PS3Larroque.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

// 🔹 Si Render define el PORT, nos ligamos a 0.0.0.0:PORT
var port = Environment.GetEnvironmentVariable("PORT");
if (!string.IsNullOrEmpty(port))
{
    builder.WebHost.UseUrls($"http://0.0.0.0:{port}");
}

// 🔹 Connection string
// 1) Intenta leerla desde ConnectionStrings:DefaultConnection (appsettings o env)
// 2) Si no encuentra nada, intenta DATABASE_URL
// 3) Si nada de eso existe, usa la local de desarrollo
var connectionString =
    builder.Configuration.GetConnectionString("DefaultConnection") ??
    builder.Configuration["DATABASE_URL"];

if (string.IsNullOrWhiteSpace(connectionString))
{
    // 👉 Local (tu PC)
    connectionString = "Host=localhost;Port=5432;Database=ps3larroque;Username=ps3admin;Password=ps3pass";
}

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));

// 🔹 CORS: habilitamos Netlify + localhost (para pruebas)
const string CorsPolicyName = "AllowedOrigins";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: CorsPolicyName, policy =>
    {
        policy
            .WithOrigins(
                "https://lustrous-florentine-2899fd.netlify.app", // tu front en Netlify
                "http://localhost:3000",                         // front local
                "http://localhost:5000",                         // otra variante local
                "http://localhost:5173"                          // por si usás Vite u otro
            )
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

// 🔹 Servicios de aplicación
builder.Services.AddScoped<IStockService, StockService>();
builder.Services.AddScoped<IPreventaService, PreventaService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// 🔹 Swagger (lo dejamos siempre activado, si querés lo podés limitar a Development)
app.UseSwagger();
app.UseSwaggerUI();

// 🔹 Redirección a HTTPS (en Render ya viene con proxy HTTPS)
app.UseHttpsRedirection();

// 🔹 Activar CORS (IMPORTANTE: antes de MapControllers)
app.UseCors(CorsPolicyName);

app.MapControllers();

app.Run();
