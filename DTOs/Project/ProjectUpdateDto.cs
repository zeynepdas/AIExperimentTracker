namespace Net9RestApi.DTOs.Project
{
    //AI projesi güncellenirken alınacak veriler
    public class ProjectUpdateDto
    {
        public string Name { get; set; }=null!;
        public string? Description { get; set; }
    }
}