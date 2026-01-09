// Sistemde AI projelerine sahip olan kullanıcıyı temsil eder

namespace Net9RestApi.Entities
{
    public class User
    {
        public int Id{ get; set; }

        public string Email{ get; set; } = string.Empty;
        public string Username{ get; set; } = string.Empty;
        public string PasswordHash{ get; set; } = string.Empty;
        public string Role { get; set; } = "User";

        public bool IsDeleted{ get; set; } = false;
        public DateTime CreatedAt{ get; set; }
        public DateTime UpdatedAt{ get; set; }

        public ICollection<AIProject> Projects{ get; set; } = new List<AIProject>();
    }
}