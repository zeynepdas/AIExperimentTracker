//Kullanıcıya ait yapay zeka projesini temsil eder

namespace Net9RestApi.Entities
{
    public class AIProject
    {
        public int Id{ get; set; }

        public string Name{ get; set; } = null!;
        public string? Description{ get; set; } 

        public int UserId{ get; set; }
        public User User{ get; set; } = null!;
        public DateTime CreatedAt{ get; set; }
        public DateTime UpdatedAt{ get; set; }
    
        //Projeye ait deneyler 
        public ICollection<Experiment> Experiments{ get; set; } = new List<Experiment>();
    }
}