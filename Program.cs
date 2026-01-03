using Microsoft.EntityFrameworkCore;
using Net9RestApi.Data;

//.NET 8/9 ile gelen Minimal API yaklaşımı kullanılmıştır
// Controller yerine endpoint mapping tercih edilmiştir

var builder = WebApplication.CreateBuilder(args);

//SQLite + EF Core DbContext bağlantısı
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"))
);
// Swagger services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Swagger middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.Run();
