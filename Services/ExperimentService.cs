using Microsoft.EntityFrameworkCore;
using Net9RestApi.Data;
using Net9RestApi.DTOs.Experiment;
using Net9RestApi.Entities;

namespace Net9RestApi.Services
{
    public class ExperimentService
    {
        private readonly AppDbContext _context;
        private readonly ILogger<ExperimentService> _logger;

        public ExperimentService(AppDbContext context, ILogger<ExperimentService> logger)
        {
            _context = context;
            _logger = logger;
        }

        // Projeye ait experiment'leri getir
        public async Task<List<ExperimentResponseDto>> GetByProjectIdAsync(int projectId)
        {
            _logger.LogInformation(
                "Fetching experiments for ProjectId: {ProjectId}",
                projectId
            );

            return await _context.Experiments
                .Where(e => e.AIProjectId == projectId && !e.IsDeleted)
                .Select(e => new ExperimentResponseDto
                {
                    Id = e.Id,
                    Name = e.Name,
                    ModelName = e.ModelName,
                    Status = e.Status,
                    CreatedAt = e.CreatedAt
                })
                .ToListAsync();
        }

        // ID ile experiment getir
        public async Task<ExperimentResponseDto?> GetByIdAsync(int id)
        {
            var experiment = await _context.Experiments
                .FirstOrDefaultAsync(e => e.Id == id && !e.IsDeleted);

            if (experiment == null)
            {
                _logger.LogWarning(
                    "Experiment not found. ExperimentId: {ExperimentId}",
                    id
                );
                return null;
            }

            return new ExperimentResponseDto
            {
                Id = experiment.Id,
                Name = experiment.Name,
                ModelName = experiment.ModelName,
                Status = experiment.Status,
                CreatedAt = experiment.CreatedAt
            };
        }

        // Yeni experiment oluştur
        public async Task<ExperimentResponseDto?> CreateAsync(int projectId, ExperimentCreateDto dto)
        {
            var projectExists = await _context.AIProjects
                .AnyAsync(p => p.Id == projectId && !p.IsDeleted);

            if (!projectExists)
            {
                _logger.LogWarning(
                    "Experiment creation failed. Project not found. ProjectId: {ProjectId}",
                    projectId
                );
                return null;
            }

            var experiment = new Experiment
            {
                Name = dto.Name,
                Notes = dto.Notes,
                ModelName = dto.ModelName,
                Status = dto.Status,
                AIProjectId = projectId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsDeleted = false
            };

            _context.Experiments.Add(experiment);
            await _context.SaveChangesAsync();

            _logger.LogInformation(
                "Experiment created successfully. ExperimentId: {ExperimentId}, ProjectId: {ProjectId}",
                experiment.Id,
                projectId
            );

            return new ExperimentResponseDto
            {
                Id = experiment.Id,
                Name = experiment.Name,
                ModelName = experiment.ModelName,
                Status = experiment.Status,
                CreatedAt = experiment.CreatedAt
            };
        }

        // Experiment güncelle
        public async Task<bool> UpdateAsync(int id, ExperimentUpdateDto dto)
        {
            var experiment = await _context.Experiments
                .FirstOrDefaultAsync(e => e.Id == id && !e.IsDeleted);

            if (experiment == null)
            {
                _logger.LogWarning(
                    "Experiment update failed. Experiment not found. ExperimentId: {ExperimentId}",
                    id
                );
                return false;
            }

            experiment.Name = dto.Name;
            experiment.Notes = dto.Notes;
            experiment.ModelName = dto.ModelName;
            experiment.Status = dto.Status;
            experiment.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            _logger.LogInformation(
                "Experiment updated successfully. ExperimentId: {ExperimentId}",
                id
            );

            return true;
        }

        // Experiment sil (Soft Delete)
        public async Task<bool> DeleteAsync(int id)
        {
            var experiment = await _context.Experiments
                .FirstOrDefaultAsync(e => e.Id == id && !e.IsDeleted);

            if (experiment == null)
            {
                _logger.LogWarning(
                    "Experiment delete failed. Experiment not found. ExperimentId: {ExperimentId}",
                    id
                );
                return false;
            }

            experiment.IsDeleted = true;
            experiment.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            _logger.LogInformation(
                "Experiment soft deleted successfully. ExperimentId: {ExperimentId}",
                id
            );

            return true;
        }
    }
}
