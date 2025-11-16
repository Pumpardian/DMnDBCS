namespace DMnDBCS.Domain.Entities
{
    public class Task
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public int Status { get; set; }
        public string? StatusName { get; set; }
        public DateOnly CreationDate { get; set; }
        public DateOnly? CompletionDate { get; set; }
        public int ProjectId { get; set; }
        public int ExecutorId { get; set; }
        public bool? CanDelete { get; set; }
    }
}
