// Sistemde AI projelerine sahip olan kullanıcıyı temsil eder

namespace Net9RestApi.Entities
{
    public class User
    {
        public int Id{ get; set; }

        public string Email{ get; set; } = null!;
        public string Username{ get; set; } = null!;
     public string PasswordHash{ get; set; } = null!;

        public DateTime CreatedAt{ get; set; }
        public DateTime UpdatedAt{ get; set; }

        public ICollection<AIProject> Projects{ get; set; } = new List<AIProject>();
    }
}