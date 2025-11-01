namespace DMnDBCS.Domain.Entities
{
    public class ProjectResource
    {
        public int Id { get; set; }
        public string? Description { get; set; }
        public string Type { get; set; }
        public int ProjectId { get; set; }
    }
}
