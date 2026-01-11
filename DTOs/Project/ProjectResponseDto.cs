namespace Net9RestApi.DTOs.Project
{
    public class ProjectResponseDto
    {
        //API'dan dışarı dönen AI proje modeli
        public int Id { get; set; }
        public string Name { get; set; }=null!;
        public string? Description { get; set; }
        public int UserId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}