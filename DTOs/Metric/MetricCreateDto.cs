namespace Net9RestApi.DTOs.Metric
{
    public class MetricCreateDto
    {
        public string Name { get; set; } = null!;
        public double Value { get; set; }
    }
}
