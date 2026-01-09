using Microsoft.EntityFrameworkCore;
using Net9RestApi.Data;
using Net9RestApi.DTOs.User;
using Net9RestApi.Entities;

namespace Net9RestApi.Services
{
    public class UserService
    {
        private readonly AppDbContext _context;
        private readonly ILogger<UserService> _logger;

        public UserService(AppDbContext context, ILogger<UserService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<UserResponseDto>> GetAllAsync()
        {
            _logger.LogInformation("Fetching all users");

            return await _context.Users
                .Where(u => !u.IsDeleted)
                .Select(u => new UserResponseDto
                {
                    Id = u.Id,
                    Email = u.Email,
                    Username = u.Username,
                    CreatedAt = u.CreatedAt
                })
                .ToListAsync();
        }

        public async Task<UserResponseDto?> GetByIdAsync(int id)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == id && !u.IsDeleted);

            if (user == null)
            {
                _logger.LogWarning("User not found. UserId: {UserId}", id);
                return null;
            }

            return new UserResponseDto
            {
                Id = user.Id,
                Email = user.Email,
                Username = user.Username,
                CreatedAt = user.CreatedAt
            };
        }

        public async Task<UserResponseDto> CreateAsync(UserCreateDto dto)
        {
            var user = new User
            {
                Email = dto.Email,
                Username = dto.Username,
                PasswordHash = dto.Password,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsDeleted = false
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            _logger.LogInformation("User created successfully. UserId: {UserId}", user.Id);

            return new UserResponseDto
            {
                Id = user.Id,
                Email = user.Email,
                Username = user.Username,
                CreatedAt = user.CreatedAt
            };
        }

        public async Task<bool> UpdateAsync(int id, UserUpdateDto dto)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == id && !u.IsDeleted);

            if (user == null)
            {
                _logger.LogWarning("User update failed. User not found. UserId: {UserId}", id);
                return false;
            }

            user.Email = dto.Email;
            user.Username = dto.Username;
            user.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            _logger.LogInformation("User updated successfully. UserId: {UserId}", id);
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == id && !u.IsDeleted);

            if (user == null)
            {
                _logger.LogWarning("User delete failed. User not found. UserId: {UserId}", id);
                return false;
            }

            user.IsDeleted = true;
            user.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            _logger.LogInformation("User soft deleted successfully. UserId: {UserId}", id);
            return true;
        }
    }
}
