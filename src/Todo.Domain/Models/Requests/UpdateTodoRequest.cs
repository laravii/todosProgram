namespace Todo.Domain.Requests
{
    public class UpdateTodoRequest
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public int DaysToFinish { get; set; }
    }
}
