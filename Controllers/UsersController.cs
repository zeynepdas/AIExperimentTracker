using Microsoft.AspNetCore.Mvc;
using Net9RestApi.DTOs;
using Net9RestApi.DTOs.User;
using Net9RestApi.Services;

namespace Net9RestApi.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UsersController : ControllerBase
    {
        private readonly UserService _userService;

        public UsersController(UserService userService)
        {
            _userService = userService;
        }

        //Tüm kullanıcıları getirir 
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userService.GetAllAsync();
            return Ok(ApiResponse<List<UserResponseDto>>.SuccessResponse(users));
        }

        //ID'ye göre kullanıcı getirir
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _userService.GetByIdAsync(id);

            if (user == null)
                return NotFound(ApiResponse<string>.Fail("User not found"));

            return Ok(ApiResponse<UserResponseDto>.SuccessResponse(user));
        }

        //Yeni kullanıcı oluşturur
        [HttpPost]
        public async Task<IActionResult> Create(UserCreateDto dto)
        {
            var createdUser = await _userService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = createdUser.Id }, ApiResponse<UserResponseDto>.SuccessResponse(createdUser, "User created successfully"));
        }

        //Kullanıcı updateler
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, UserUpdateDto dto)
        {
            var updated = await _userService.UpdateAsync(id, dto);

            if (!updated)
                return NotFound(ApiResponse<string>.Fail("User not found"));

            return Ok(ApiResponse<string>.SuccessResponse("User updated"));
        }

        //Kaullanıcı siler
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _userService.DeleteAsync(id);

            if (!deleted)
                return NotFound(ApiResponse<string>.Fail("User not found"));

            return NoContent();
        }
    }
}