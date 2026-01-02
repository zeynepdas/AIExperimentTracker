//Kullanıcıya ait yapay zeka projesini temsil eder
public class AIProject
{
    public int Id{ get; set; }

    public string Name{ get; set; } = null!;
    public string? Description{ get; set; } 

    public User User{ get; set; }
    public DateTime CreatedAt{ get; set; }
    public DateTime UpdatedAt{ get; set; }
    
    //Projeye ait deneyler 
    public ICollection<Experiment> Experiments{ get; set; } = new List<Experiment>();
}