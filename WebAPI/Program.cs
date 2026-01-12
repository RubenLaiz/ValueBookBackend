using EBGBackend.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Reflection;
using System.Text;
using WebAPI.Extensions;
using WebAPI.Funciones;
using WebAPI.Mapping;

var builder = WebApplication.CreateBuilder(args);

// Configuración de Kestrel para aumentar el tamaño máximo del cuerpo de la solicitud
builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.Limits.MaxRequestBodySize = 21474836480; // Límite de 20 GB
});

// Configuración para las opciones de formularios
builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 21474836480; // Límite de 20 GB
});

// -----------------------------
// Configuración de Serilog
// -----------------------------
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()          // Nivel mínimo global
    .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Warning)
    .WriteTo.File(
        builder.Configuration["Logging:File"],
        rollingInterval: RollingInterval.Day,
        retainedFileCountLimit: 7,           // Mantener últimos 7 días
        fileSizeLimitBytes: 10_000_000,      // Máx 10 MB por archivo
        rollOnFileSizeLimit: true)           // Rota si se llena
    .CreateLogger();

// Reemplazar logger por defecto de .NET con Serilog
builder.Host.UseSerilog();

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnectionPostgreSql")));

// TOKEN
var tokenManager = new TokenManager(builder.Configuration);
var key = Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]);
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });


builder.Services.AddAuthorization();

// Clases a añadir
builder.Services.AddScoped<TextoMyExtension>();

// Registrar AutoMapper y los perfiles
builder.Services.AddAutoMapper(typeof(MapperProfile));


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddServicesFromAssembly(Assembly.GetExecutingAssembly());

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyHeader()
                   .AllowAnyMethod();
        });
});


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAllOrigins");
app.UseAuthorization();
app.MapControllers();


// Importante: asegura liberar logs antes de cerrar la app
try
{
    app.Run();
}
finally
{
    Log.CloseAndFlush();
}

