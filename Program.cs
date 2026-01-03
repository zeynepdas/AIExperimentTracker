
using Microsoft.EntityFrameworkCore;
using Net9RestApi.Data;
using Net9RestApi.DTOs;
using Net9RestApi.DTOs.User;    
using Net9RestApi.Services;

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

builder.Services.AddScoped<UserService>();

var app = builder.Build();

// Swagger middleware
app.UseDeveloperExceptionPage();

app.UseSwagger();
app.UseSwaggerUI();


app.UseHttpsRedirection();

//--------------- Kullanıcı Endpoints ------------------

//Tüm kullanıcıları getirir
app.MapGet("/users", async (UserService service) =>
{
    var users = await service.GetAllAsync();
    return Results.Ok(ApiResponse<List<UserResponseDto>>.SuccessResponse(users));
});

//ID'ye göre kullanıcı getirir
app.MapGet("/users/{id:int}", async (int id, UserService service) =>
{
    var user = await service.GetByIdAsync(id);

    if (user == null)
        return Results.NotFound(ApiResponse<UserResponseDto>.Fail("User not found"));

    return Results.Ok(ApiResponse<UserResponseDto>.SuccessResponse(user));
});

//Yeni kullanıcı oluşturur
app.MapPost("/users", async (UserCreateDto dto, UserService service) =>
{
    var user = await service.CreateAsync(dto);

    return Results.Created($"/users/{user.Id}", ApiResponse<UserResponseDto>.SuccessResponse(user, "User created successfully"));
});

//Kullanıcı güncelle
app.MapPut("/users/{id:int}", async (int id, UserUpdateDto dto, UserService service) =>
{
    var updatedUser = await service.UpdateAsync(id, dto);

    if (!updatedUser)
        return Results.NotFound(ApiResponse<string>.Fail("User not found"));

    return Results.Ok(ApiResponse<string>.SuccessResponse("User updated successfully"));
});

//Kullanıcı sil
app.MapDelete("/users/{id:int}", async (int id, UserService service) =>
{
    var deletedUser = await service.DeleteAsync(id);

    if (!deletedUser)
        return Results.NotFound(ApiResponse<string>.Fail("User not found"));

    return Results.NoContent();
});

app.Run();
