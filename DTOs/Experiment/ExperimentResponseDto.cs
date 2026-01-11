namespace Net9RestApi.DTOs.Experiment
{
    public class ExperimentResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; }=null!;
        public string ModelName { get; set; }=null!;
        public string Status { get; set; }=null!;
        public int AIProjectId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}