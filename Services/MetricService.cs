using Microsoft.EntityFrameworkCore;
using Net9RestApi.Data;
using Net9RestApi.DTOs.Metric;
using Net9RestApi.Entities;

namespace Net9RestApi.Services
{
    public class MetricService
    {
        private readonly AppDbContext _context;
        private readonly ILogger<MetricService> _logger;

        public MetricService(AppDbContext context, ILogger<MetricService> logger)
        {
            _context = context;
            _logger = logger;
        }

        // Bir experiment'e ait tüm metric'leri getir (Soft Delete filtreli)
        public async Task<List<MetricResponseDto>> GetByExperimentIdAsync(int experimentId)
        {
            _logger.LogInformation(
                "Fetching metrics for ExperimentId: {ExperimentId}",
                experimentId
            );

            return await _context.Metrics
                .Where(m => m.ExperimentId == experimentId && !m.IsDeleted)
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
            var experimentExists = await _context.Experiments
                .AnyAsync(e => e.Id == experimentId && !e.IsDeleted);

            if (!experimentExists)
            {
                _logger.LogWarning(
                    "Metric creation failed. Experiment not found or deleted. ExperimentId: {ExperimentId}",
                    experimentId
                );
                return null;
            }

            var metric = new Metric
            {
                Name = dto.Name,
                Value = dto.Value,
                ExperimentId = experimentId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsDeleted = false
            };

            _context.Metrics.Add(metric);
            await _context.SaveChangesAsync();

            _logger.LogInformation(
                "Metric created successfully. MetricId: {MetricId}, ExperimentId: {ExperimentId}",
                metric.Id,
                experimentId
            );

            return new MetricResponseDto
            {
                Id = metric.Id,
                Name = metric.Name,
                Value = metric.Value,
                CreatedAt = metric.CreatedAt
            };
        }

        // Metric güncelle
        public async Task<bool> UpdateAsync(int id, MetricUpdateDto dto)
        {
            var metric = await _context.Metrics
                .FirstOrDefaultAsync(m => m.Id == id && !m.IsDeleted);

            if (metric == null)
            {
                _logger.LogWarning(
                    "Metric update failed. Metric not found or deleted. MetricId: {MetricId}",
                    id
                );
                return false;
            }

            metric.Name = dto.Name;
            metric.Value = dto.Value;
            metric.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            _logger.LogInformation(
                "Metric updated successfully. MetricId: {MetricId}",
                id
            );

            return true;
        }

        // Metric sil (Soft Delete)
        public async Task<bool> DeleteAsync(int id)
        {
            var metric = await _context.Metrics
                .FirstOrDefaultAsync(m => m.Id == id && !m.IsDeleted);

            if (metric == null)
            {
                _logger.LogWarning(
                    "Metric soft delete failed. Metric not found or already deleted. MetricId: {MetricId}",
                    id
                );
                return false;
            }

            metric.IsDeleted = true;
            metric.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            _logger.LogInformation(
                "Metric soft deleted successfully. MetricId: {MetricId}",
                id
            );

            return true;
        }
    }
}
