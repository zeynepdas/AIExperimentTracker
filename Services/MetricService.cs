using Microsoft.EntityFrameworkCore;
using Net9RestApi.Data;
using Net9RestApi.DTOs.Metric;
using Net9RestApi.Entities;

namespace Net9RestApi.Services
{
    public class MetricService
    {
        private readonly AppDbContext _context;

        public MetricService(AppDbContext context)
        {
            _context = context;
        }

        // Bir experiment'e ait tüm metric'leri getir
        public async Task<List<MetricResponseDto>> GetByExperimentIdAsync(int experimentId)
        {
            return await _context.Metrics
                .Where(m => m.ExperimentId == experimentId)
                .Select(m => new MetricResponseDto
                {
                    Id = m.Id,
                    Name = m.Name,
                    Value = m.Value,
                    CreatedAt = m.CreatedAt
                })
                .ToListAsync();
        }

        // Yeni metric ekle
        public async Task<MetricResponseDto?> CreateAsync(int experimentId, MetricCreateDto dto)
        {
            var experimentExists = await _context.Experiments.AnyAsync(e => e.Id == experimentId);
            if (!experimentExists) return null;

            var metric = new Metric
            {
                Name = dto.Name,
                Value = dto.Value,
                ExperimentId = experimentId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Metrics.Add(metric);
            await _context.SaveChangesAsync();

            return new MetricResponseDto
            {
                Id = metric.Id,
                Name = metric.Name,
                Value = metric.Value,
                CreatedAt = metric.CreatedAt
            };
        }

        // Metric sil
        public async Task<bool> DeleteAsync(int id)
        {
            var metric = await _context.Metrics.FindAsync(id);
            if (metric == null) return false;

            _context.Metrics.Remove(metric);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
