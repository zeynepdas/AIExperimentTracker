using Microsoft.AspNetCore.Mvc;
using Net9RestApi.DTOs;
using Net9RestApi.Services;
using Net9RestApi.DTOs.Project;

namespace Net9RestApi.Controllers
{
    [ApiController]
    [Route("api")]
    public class ProjectsController : ControllerBase
    {
        private readonly ProjectService _projectService;

        public ProjectsController(ProjectService projectService)
        {
            _projectService = projectService;
        }

        //Kullanıcıy ait tüm projelerini getirir
        [HttpGet("users/{userId:int}/projects")]
        public async Task<IActionResult> GetByUserId(int userId)
        {
            var projects = await _projectService.GetProjectsByUserIdAsync(userId);
            return Ok(ApiResponse<List<ProjectResponseDto>>.SuccessResponse(projects));
        }

        //ID ile proje getirir
        [HttpGet("projects/{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var project = await _projectService.GetByIdAsync(id);

            if (project == null)
                return NotFound(ApiResponse<string>.Fail("Project not found"));

            return Ok(ApiResponse<ProjectResponseDto>.SuccessResponse(project));
        }
        
        //Kullanıcıya yeni proje oluşturur
        [HttpPost("users/{userId:int}/projects")]
        public async Task<IActionResult> Create(int userId, ProjectCreateDto dto)
        {
            var project = await _projectService.CreateAsync(userId, dto);

            if (project == null)
                return NotFound(ApiResponse<string>.Fail("User not found"));

            return CreatedAtAction(
                nameof(GetById),
                new { id = project.Id },
                ApiResponse<ProjectResponseDto>.SuccessResponse(project, "Project created successfully")
            );
        }

        //Proje günceller
        [HttpPut("projects/{id:int}")]
        public async Task<IActionResult> Update(int id, ProjectUpdateDto dto)
        {
            var updatedProject = await _projectService.UpdateAsync(id, dto);

            if (!updatedProject)
                return NotFound(ApiResponse<string>.Fail("Project not found"));

            return Ok(ApiResponse<string>.SuccessResponse("Project updated successfully"));
        }

        //Proje siler
        [HttpDelete("projects/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deletedProject = await _projectService.DeleteAsync(id);

            if (!deletedProject)
                return NotFound(ApiResponse<string>.Fail("Project not found"));

            return NoContent();
        }
    }
}