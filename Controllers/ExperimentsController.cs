using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Net9RestApi.DTOs;
using Net9RestApi.DTOs.Experiment;
using Net9RestApi.Services;


namespace Net9RestApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api")]
    public class ExperimentsController : ControllerBase
    {
        private readonly ExperimentService _experimentService;

        public ExperimentsController(ExperimentService experimentService)
        {
            _experimentService = experimentService;
        }

        // Bir projeye ait tüm experiment'leri getirir
        [HttpGet("projects/{projectId:int}/experiments")]
        public async Task<IActionResult> GetByProjectId(int projectId)
        {
            var experiments = await _experimentService.GetByProjectIdAsync(projectId);
            return Ok(ApiResponse<List<ExperimentResponseDto>>.SuccessResponse(experiments));
        }

        // ID ile experiment getirir
        [HttpGet("experiments/{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var experiment = await _experimentService.GetByIdAsync(id);

            if (experiment == null)
                return NotFound(ApiResponse<string>.Fail("Experiment not found"));

            return Ok(ApiResponse<ExperimentResponseDto>.SuccessResponse(experiment));
        }

        // Projeye yeni experiment ekler
        [HttpPost("projects/{projectId:int}/experiments")]
        public async Task<IActionResult> Create(int projectId, ExperimentCreateDto dto)
        {
            var experiment = await _experimentService.CreateAsync(projectId, dto);

            if (experiment == null)
                return NotFound(ApiResponse<string>.Fail("Project not found"));

            return CreatedAtAction(
                nameof(GetById),
                new { id = experiment.Id },
                ApiResponse<ExperimentResponseDto>.SuccessResponse(experiment, "Experiment created successfully")
            );
        }

        // Experiment günceller
        [HttpPut("experiments/{id:int}")]
        public async Task<IActionResult> Update(int id, ExperimentUpdateDto dto)
        {
            var updated = await _experimentService.UpdateAsync(id, dto);

            if (!updated)
                return NotFound(ApiResponse<string>.Fail("Experiment not found"));

            return Ok(ApiResponse<string>.SuccessResponse("Experiment updated successfully"));
        }

        // Experiment siler
        [HttpDelete("experiments/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _experimentService.DeleteAsync(id);

            if (!deleted)
                return NotFound(ApiResponse<string>.Fail("Experiment not found"));

            return NoContent();
        }
    }
}
