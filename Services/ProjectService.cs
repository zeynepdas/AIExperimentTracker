using Microsoft.EntityFrameworkCore;
using Net9RestApi.Data;
using Net9RestApi.DTOs.Project;
using Net9RestApi.Entities;

namespace Net9RestApi.Services
{
    public class ProjectService
    {
        //AI project ile ilgili tüm iş mantığı burada tutuluyor
        private readonly AppDbContext _context;

        public ProjectService(AppDbContext context)
        {
            _context = context;
        }

        //Kullanıcıya ait tüm projeleri getir
        public async Task<List<ProjectResponseDto>> GetProjectsByUserIdAsync(int userId)
        {

            return await _context.AIProjects
                .Where(p => p.UserId == userId)
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

        //ID ile proje getir
        public async Task<ProjectResponseDto?> GetByIdAsync(int id)
        {
            var project = await _context.AIProjects.FindAsync(id);
            if (project == null) return null;

            return new ProjectResponseDto
            {
                Id = project.Id,
                Name = project.Name,
                Description = project.Description,
                UserId = project.UserId,
                CreatedAt = project.CreatedAt
            };
        }

        //Kullanıcıya ait yeni proje ekler
        public async Task<ProjectResponseDto?> CreateAsync(int userId, ProjectCreateDto dto)
        {
            var userExists = await _context.Users.AnyAsync(u => u.Id == userId);
            if (!userExists) return null;

            var project = new AIProject
            {
                Name = dto.Name,
                Description = dto.Description,
                UserId = userId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.AIProjects.Add(project);
            await _context.SaveChangesAsync();

            return new ProjectResponseDto
            {
                Id = project.Id,
                Name = project.Name,
                Description = project.Description,
                UserId = project.UserId,
                CreatedAt = project.CreatedAt
            };
        }

        //Projeyi günceller
        public async Task<bool> UpdateAsync(int id, ProjectUpdateDto dto)
        {
            var project = await _context.AIProjects.FindAsync(id);
            if (project == null) return false;

            project.Name = dto.Name;
            project.Description = dto.Description;
            project.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        //Projeyi siler
        public async Task<bool> DeleteAsync(int id)
        {
            var project = await _context.AIProjects.FindAsync(id);
            if (project == null) return false;

            _context.AIProjects.Remove(project);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}