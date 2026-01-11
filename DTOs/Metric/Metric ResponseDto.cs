namespace Net9RestApi.DTOs.Metric
{
    public class MetricResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public double Value { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
