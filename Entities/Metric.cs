//Bir deneye ait ddeğerlendirme metriklerini temsil eder

namespace Net9RestApi.Entities
{
    public class Metric
    {
        public int Id{ get; set; }

        public string Name{ get; set; } = null!;
        public double Value{ get; set; } 

        public int ExperimentId{ get; set; }
        public Experiment Experiment{ get; set; } = null!;
        public DateTime CreatedAt{ get; set; }
        public DateTime UpdatedAt{ get; set; }


    }
}