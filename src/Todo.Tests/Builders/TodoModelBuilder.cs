using Bogus;
using Todo.Domain.Commons;
using Todo.Domain.Models;

namespace Todo.Tests.Builders
{
    public class TodoModelBuilder : BaseBuilder<TodoModel>
    {
        private readonly static Faker _faker = new("pt_BR");
        private string _title = _faker.Random.Word();
        private int _daysToFinish = 2;
        private int _status = TodoStatus.Todo.Status;
        private DateTime _createdDate = DateTime.UtcNow;
        private int _id = _faker.Random.Int(1, 10);

        public override TodoModel Build() =>
            Faker
            .RuleFor(p => p.Title, _title)
            .RuleFor(p => p.Description, f => f.Lorem.Lines(1))
            .RuleFor(p => p.CreatedDate, _createdDate)
            .RuleFor(p => p.Id, _id)
            .RuleFor(p => p.DaysToFinish, _daysToFinish)
            .RuleFor(p => p.DueDate, _createdDate.AddDays(_daysToFinish))
            .RuleFor(p => p.Status, _status);

        public TodoModelBuilder WithId(int id)
        {
            _id = id;
            return this;
        }

        public TodoModelBuilder WithTitle(string title)
        {
            _title = title;
            return this;
        }

        public TodoModelBuilder WithCreatedDate(DateTime createdDate)
        {
            _createdDate = createdDate;
            return this;
        }

        public TodoModelBuilder WithStatus(TodoStatus status)
        {
            _status = status.Status;
            return this;
        }

        public TodoModelBuilder WithDaysToFinish(int daysToFinish)
        {
            _daysToFinish = daysToFinish;
            return this;
        }
    }
}
