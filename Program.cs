
using Microsoft.EntityFrameworkCore;
using Net9RestApi.Data;   
using Net9RestApi.Services;

// ASP.NET Core (.NET 9) Controller tabanlı Web API mimarisi kullanılmıştır
// Endpoint’ler Controller sınıfları üzerinden tanımlanmıştır

var builder = WebApplication.CreateBuilder(args);

//SQLite + EF Core DbContext bağlantısı
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder.Services.AddControllers();

// Swagger services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<ProjectService>();
builder.Services.AddScoped<ExperimentService>();
builder.Services.AddScoped<MetricService>();

var app = builder.Build();

// Swagger middleware
app.UseSwagger();
app.UseSwaggerUI();


app.UseHttpsRedirection();

app.MapControllers();

app.Run();
