namespace Net9RestApi.DTOs.Experiment
{
    public class ExperimentUpdateDto
    {
        public string Name { get; set; }=null!;
        public string? Notes { get; set; }
        public string? ModelName { get; set; }=null!;
        public string Status { get; set; }=null!;
    }
}