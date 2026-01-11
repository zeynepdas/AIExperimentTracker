namespace Net9RestApi.DTOs.Project
{
    //AI projesi oluştururken alınacak veriler 
    public class ProjectCreateDto
    {
        public string Name { get; set; }= null!;
        public string? Description { get; set; }
    }
}