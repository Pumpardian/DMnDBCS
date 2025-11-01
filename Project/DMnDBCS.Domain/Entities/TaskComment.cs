namespace DMnDBCS.Domain.Entities
{
    public class TaskComment
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateOnly CreationDate { get; set; }
        public int TaskId { get; set; }
        public int AuthorId { get; set; }
    }
}
