//Bir AI projesi adı altında yapılan deneyi(experiment) temsil eder

namespace Net9RestApi.Entities
{
    public class Experiment
    {  
        public int Id{ get; set; }

        public string Name{ get; set; } = null!;
        public string? Notes{ get; set; } 

        public string ModelName{ get; set; } = null!;
        //Deneyin durumu (örneğin: "Running", "Completed", "Failed")
        public string Status{ get; set; } = null!;

        public int AIProjectId{ get; set; }
        public AIProject AIProject{ get; set; } = null!;
        public DateTime CreatedAt{ get; set; }
        public DateTime UpdatedAt{ get; set; }
        //Deneye ait metrikler
        public ICollection<Metric> Metrics{ get; set; } = new List<Metric>();
    }
}