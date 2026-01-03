namespace Net9RestApi.DTOs.User
{
    //API'dan dışarı dönen kullanıcı modeli 
    public class UserResponseDto
    {
        public int Id{ get; set; }
        public string Email{ get; set; } = string.Empty;
        public string Username{ get; set; } = string.Empty;
        public DateTime CreatedAt{ get; set; }
    }
}