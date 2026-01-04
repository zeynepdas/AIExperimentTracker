
using Microsoft.EntityFrameworkCore;
using Net9RestApi.Data;
using Net9RestApi.DTOs;
using Net9RestApi.DTOs.User;    
using Net9RestApi.Services;
using Net9RestApi.DTOs.Project;
using Net9RestApi.DTOs.Experiment;

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
builder.Services.AddScoped<ProjectService>();
builder.Services.AddScoped<ExperimentService>();

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

//--------------- Proje Endpoints ------------------

//Kullanıcıya ait tüm projeleri getir
app.MapGet("/users/{userId:int}/projects", async (int userId, ProjectService service) =>
{
    var projects = await service.GetProjectsByUserIdAsync(userId);
    return Results.Ok(ApiResponse<List<ProjectResponseDto>>.SuccessResponse(projects));
});

//Kullanıcıya ait yeni proje ekler
app.MapPost("/users/{userId:int}/projects", async (int userId, ProjectCreateDto dto, ProjectService service) =>
{
    var project = await service.CreateAsync(userId, dto);

    if (project == null)
        return Results.NotFound(ApiResponse<string>.Fail("User not found"));

    return Results.Created($"/projects/{project.Id}", ApiResponse<ProjectResponseDto>.SuccessResponse(project, "Project created successfully"));
});

//ID ile proje getir
app.MapGet("/projects/{id:int}", async (int id, ProjectService service) =>
{
    var project = await service.GetByIdAsync(id);
    if (project == null)
        return Results.NotFound(ApiResponse<string>.Fail("Project not found"));

    return Results.Ok(ApiResponse<ProjectResponseDto>.SuccessResponse(project));
});

//Proje güncelle
app.MapPut("/projects/{id:int}", async (int id, ProjectUpdateDto dto, ProjectService service) =>
{
    var updatedProject = await service.UpdateAsync(id, dto);

    if (!updatedProject)
        return Results.NotFound(ApiResponse<string>.Fail("Project not found"));

    return Results.Ok(ApiResponse<string>.SuccessResponse("Project updated successfully"));
});

//Proje sil
app.MapDelete("/projects/{id:int}", async (int id, ProjectService service) =>
{
    var deletedProject = await service.DeleteAsync(id);

    if (!deletedProject)
        return Results.NotFound(ApiResponse<string>.Fail("Project not found"));

    return Results.NoContent();
});

//--------------- Experiment Endpoints ------------------

//Bir projeye ait tüm experiment'leri getirir
app.MapGet("/projects/{projectId:int}/experiments", async (int projectId, ExperimentService service) =>
{
    var experiments = await service.GetByProjectIdAsync(projectId);
    return Results.Ok(ApiResponse<List<ExperimentResponseDto>>.SuccessResponse(experiments));
});

// ID ile experiment getirir
app.MapGet("/experiments/{id:int}", async (int id, ExperimentService service) =>
{
    var experiment = await service.GetByIdAsync(id);
    if (experiment == null)
        return Results.NotFound(ApiResponse<string>.Fail("Experiment not found"));

    return Results.Ok(ApiResponse<ExperimentResponseDto>.SuccessResponse(experiment));
});

//Bir projeye yeni experiment ekle
app.MapPost("/projects/{projectId:int}/experiments", async (int projectId, ExperimentCreateDto dto, ExperimentService service) =>
{
    var experiment = await service.CreateAsync(projectId, dto);

    if (experiment == null)
        return Results.NotFound(ApiResponse<string>.Fail("Project not found"));

    return Results.Created($"/experiments/{experiment.Id}", ApiResponse<ExperimentResponseDto>.SuccessResponse(experiment, "Experiment created successfully"));
});

//Experiment güncelle
app.MapPut("/experiments/{id:int}", async (int id, ExperimentUpdateDto dto, ExperimentService service) =>
{
    var updatedExperiment = await service.UpdateAsync(id, dto);

    if (!updatedExperiment)
        return Results.NotFound(ApiResponse<string>.Fail("Experiment not found"));

    return Results.Ok(ApiResponse<string>.SuccessResponse("Experiment updated successfully"));
});

//Experiment sil
app.MapDelete("/experiments/{id:int}", async (int id, ExperimentService service) =>
{
    var deletedExperiment = await service.DeleteAsync(id);

    if (!deletedExperiment)
        return Results.NotFound(ApiResponse<string>.Fail("Experiment not found"));

    return Results.NoContent();
});

app.Run();
