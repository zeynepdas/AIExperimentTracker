using Microsoft.EntityFrameworkCore;
using Net9RestApi.Data;
using Net9RestApi.DTOs.Experiment;
using Net9RestApi.Entities;

namespace Net9RestApi.Services
{
    public class ExperimentService
    {
        //Experiment ile ilgili tüm iş mantığı burada tutuluyor
        private readonly AppDbContext _context;

        public ExperimentService(AppDbContext context)
        {
            _context = context;
        }

        //ProjeId'ye göre tüm experiment'leri getirir
        public async Task<List<ExperimentResponseDto>> GetByProjectIdAsync(int projectId)
        {
            return await _context.Experiments
                .Where(e => e.AIProjectId == projectId)
                .Select(e => new ExperimentResponseDto
                {
                    Id = e.Id,
                    Name = e.Name,
                    ModelName = e.ModelName,
                    Status = e.Status,
                    AIProjectId = e.AIProjectId,
                    CreatedAt = e.CreatedAt
                })
                .ToListAsync();
        }

        //ID ile experiment getirir
        public async Task<ExperimentResponseDto?> GetByIdAsync(int id)  
        {
            var experiment = await _context.Experiments.FindAsync(id);
            if (experiment == null) return null;

            return new ExperimentResponseDto
            {
                Id = experiment.Id,
                Name = experiment.Name,
                ModelName = experiment.ModelName,
                Status = experiment.Status,
                AIProjectId = experiment.AIProjectId,
                CreatedAt = experiment.CreatedAt
            };
        }

        //Yeni experiment oluşturur
        public async Task<ExperimentResponseDto?> CreateAsync(int projectId, ExperimentCreateDto dto)
        {
            var projectExists = await _context.AIProjects.AnyAsync(p => p.Id == projectId);
            if (!projectExists) return null;

            var experiment = new Experiment
            {
                Name = dto.Name,
                Notes = dto.Notes,
                ModelName = dto.ModelName,
                Status = dto.Status,
                AIProjectId = projectId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Experiments.Add(experiment);
            await _context.SaveChangesAsync();

            return new ExperimentResponseDto
            {
                Id = experiment.Id,
                Name = experiment.Name,
                ModelName = experiment.ModelName,
                Status = experiment.Status,
                AIProjectId = experiment.AIProjectId,
                CreatedAt = experiment.CreatedAt
            };
        }

        //Experiment günceller
        public async Task<bool> UpdateAsync(int id, ExperimentUpdateDto dto)    
        {
            var experiment = await _context.Experiments.FindAsync(id);
            if (experiment == null) return false;

            experiment.Name = dto.Name;
            experiment.Notes = dto.Notes;
            experiment.Status = dto.Status;
            experiment.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        //Experiment siler
        public async Task<bool> DeleteAsync(int id)
        {
            var experiment = await _context.Experiments.FindAsync(id);
            if (experiment == null) return false;

            _context.Experiments.Remove(experiment);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}