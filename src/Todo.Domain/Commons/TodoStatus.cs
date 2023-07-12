namespace Todo.Domain.Commons
{
    public class TodoStatus : IEquatable<TodoStatus>
    {
        public int Status { get; private set; }
        public string StatusDescription { get; private set; }

        private TodoStatus(int status, string statusDescription)
        {
            Status = status;
            StatusDescription = statusDescription;
        }

        public bool Equals(TodoStatus? other)
        {
            return other is not null && Status == other.Status;
        }

        public static readonly TodoStatus Todo = new(1, "todo");
        public static readonly TodoStatus Doing = new(2, "doing");
        public static readonly TodoStatus Done = new(3, "done");

        public static TodoStatus GetTodoStatus(string statusDescription) =>
            statusDescription.ToLower() switch
            {
                "todo" => Todo,
                "doing" => Doing,
                "done" => Done,
                _ => Todo
            };

        public static TodoStatus GetTodoStatus(int status) =>
            status switch
            {
                1 => Todo,
                2 => Doing,
                3 => Done,
                _ => Todo
            };

        public static TodoStatus UpdateStatus(int actual, string toUpdate)
        {
            var todoCurrentStatus = GetTodoStatus(actual);

            if (string.IsNullOrEmpty(toUpdate)){ return todoCurrentStatus; }

            var todoNextStatus = GetTodoStatus(toUpdate);
            var isSameStatus = todoNextStatus.Equals(todoCurrentStatus);

            return isSameStatus ? todoCurrentStatus : todoNextStatus ;
        }
    }
}
