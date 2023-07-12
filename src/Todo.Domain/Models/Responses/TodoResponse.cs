namespace Todo.Domain.Responses
{
    public class TodoResponse
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public string TodoStatus { get; set; }
        public bool IsCompleted { get { return Commons.TodoStatus.GetTodoStatus(TodoStatus).Equals(Commons.TodoStatus.Done); } }
        public DateTime DueDate { get; set; }
        public string Observation
        {
            get
            {
                var isLater = !IsCompleted && DueDate.Date.CompareTo(DateTime.Today.Date) < 0;
                return isLater ? "The task is later" : "";
            }
        }
    }
}
