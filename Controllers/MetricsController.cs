using Microsoft.AspNetCore.Mvc;
using Net9RestApi.DTOs;
using Net9RestApi.DTOs.Metric;
using Net9RestApi.Services;

namespace Net9RestApi.Controllers
{
    [ApiController]
    [Route("api")]
    public class MetricsController : ControllerBase
    {
        private readonly MetricService _metricService;

        public MetricsController(MetricService metricService)
        {
            _metricService = metricService;
        }

        // Bir experiment'e ait metric'leri getir
        [HttpGet("experiments/{experimentId:int}/metrics")]
        public async Task<IActionResult> GetByExperimentId(int experimentId)
        {
            var metrics = await _metricService.GetByExperimentIdAsync(experimentId);
            return Ok(ApiResponse<List<MetricResponseDto>>.SuccessResponse(metrics));
        }

        // Yeni metric ekle
        [HttpPost("experiments/{experimentId:int}/metrics")]
        public async Task<IActionResult> Create(int experimentId, MetricCreateDto dto)
        {
            var metric = await _metricService.CreateAsync(experimentId, dto);

            if (metric == null)
                return NotFound(ApiResponse<string>.Fail("Experiment not found"));

            return Ok(ApiResponse<MetricResponseDto>.SuccessResponse(metric, "Metric created successfully"));
        }

        // Metric sil
        [HttpDelete("metrics/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _metricService.DeleteAsync(id);

            if (!deleted)
                return NotFound(ApiResponse<string>.Fail("Metric not found"));

            return NoContent();
        }
    }
}
