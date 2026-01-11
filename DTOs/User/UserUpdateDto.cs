using System.ComponentModel.DataAnnotations;

namespace Net9RestApi.DTOs.User
{
    // Kullanıcı güncellerken dışarıdan alınacak veriler
    public class UserUpdateDto
    {
        //Required]
        //[EmailAddress]
        public string Email { get; set; } = string.Empty;

        //[Required]
        //[MinLength(3)]
        public string Username { get; set; } = string.Empty;
    }
}
