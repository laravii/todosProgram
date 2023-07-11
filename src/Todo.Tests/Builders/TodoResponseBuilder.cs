using Bogus;
using Todo.Domain.Commons;
using Todo.Domain.Responses;

namespace Todo.Tests.Builders
{
    public class TodoResponseBuilder : BaseBuilder<TodoResponse>
    {
        private readonly static Faker _faker = new("pt_BR");
        private string _title = _faker.Random.Word();
        private int _daysToFinish = 2;
        private string _status = TodoStatus.Todo.StatusDescription;
        private DateTime _createdDate = DateTime.UtcNow;
        private int _id = _faker.Random.Int(1, 10);

        public override TodoResponse Build() =>
            Faker
            .RuleFor(p => p.Title, _title)
            .RuleFor(p => p.Description, f => f.Lorem.Lines(1))
            .RuleFor(p => p.CreatedDate, _createdDate)
            .RuleFor(p => p.Id, _id)
            .RuleFor(p => p.DueDate, _createdDate.AddDays(_daysToFinish))
            .RuleFor(p => p.TodoStatus, _status);

        public TodoResponseBuilder WithId(int id)
        {
            _id = id;
            return this;
        }

        public TodoResponseBuilder WithTitle(string title)
        {
            _title = title;
            return this;
        }

        public TodoResponseBuilder WithCreatedDate(DateTime createdDate)
        {
            _createdDate = createdDate;
            return this;
        }

        public TodoResponseBuilder WithStatus(TodoStatus status)
        {
            _status = status.StatusDescription;
            return this;
        }

        public TodoResponseBuilder WithDaysToFinish(int daysToFinish)
        {
            _daysToFinish = daysToFinish;
            return this;
        }
    }
}
//public int Id { get; set; }
//public string Title { get; set; }
//public string Description { get; set; }
//public DateTime CreatedDate { get; set; }
//public string TodoStatus { get; set; }
//public bool IsCompleted { get { return Commons.TodoStatus.GetTodoStatus(TodoStatus).Equals(Commons.TodoStatus.Done); } }
//public DateTime DueDate { get; set; }