using Todo.Domain.Commons;

namespace Todo.Domain.Requests
{
    public class InsertTodoRequest
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int DaysToFinish { get; set; }
    }
}
