using Microsoft.EntityFrameworkCore;
using Net9RestApi.Data;
using Net9RestApi.DTOs.Project;
using Net9RestApi.Entities;

namespace Net9RestApi.Services
{
    public class ProjectService
    {
        private readonly AppDbContext _context;
        private readonly ILogger<ProjectService> _logger;

        public ProjectService(AppDbContext context, ILogger<ProjectService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<ProjectResponseDto>> GetProjectsByUserIdAsync(int userId)
        {
            _logger.LogInformation("Fetching projects for UserId: {UserId}", userId);

            return await _context.AIProjects
                .Where(p => p.UserId == userId && !p.IsDeleted)
                .Select(p => new ProjectResponseDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    UserId = p.UserId,
                    CreatedAt = p.CreatedAt
                })
                .ToListAsync();
        }

        public async Task<ProjectResponseDto?> GetByIdAsync(int id)
        {
            var project = await _context.AIProjects
                .FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);

            if (project == null)
            {
                _logger.LogWarning("Project not found. ProjectId: {ProjectId}", id);
                return null;
            }

            return new ProjectResponseDto
            {
                Id = project.Id,
                Name = project.Name,
                Description = project.Description,
                UserId = project.UserId,
                CreatedAt = project.CreatedAt
            };
        }

        public async Task<ProjectResponseDto?> CreateAsync(int userId, ProjectCreateDto dto)
        {
            var userExists = await _context.Users
                .AnyAsync(u => u.Id == userId && !u.IsDeleted);

            if (!userExists)
            {
                _logger.LogWarning("Project creation failed. User not found. UserId: {UserId}", userId);
                return null;
            }

            var project = new AIProject
            {
                Name = dto.Name,
                Description = dto.Description,
                UserId = userId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsDeleted = false
            };

            _context.AIProjects.Add(project);
            await _context.SaveChangesAsync();

            _logger.LogInformation(
                "Project created successfully. ProjectId: {ProjectId}, UserId: {UserId}",
                project.Id,
                userId
            );

            return new ProjectResponseDto
            {
                Id = project.Id,
                Name = project.Name,
                Description = project.Description,
                UserId = project.UserId,
                CreatedAt = project.CreatedAt
            };
        }

        public async Task<bool> UpdateAsync(int id, ProjectUpdateDto dto)
        {
            var project = await _context.AIProjects
                .FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);

            if (project == null)
            {
                _logger.LogWarning("Project update failed. Project not found. ProjectId: {ProjectId}", id);
                return false;
            }

            project.Name = dto.Name;
            project.Description = dto.Description;
            project.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            _logger.LogInformation("Project updated successfully. ProjectId: {ProjectId}", id);
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var project = await _context.AIProjects
                .FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);

            if (project == null)
            {
                _logger.LogWarning("Project delete failed. Project not found. ProjectId: {ProjectId}", id);
                return false;
            }

            project.IsDeleted = true;
            project.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            _logger.LogInformation("Project soft deleted successfully. ProjectId: {ProjectId}", id);
            return true;
        }
    }
}
