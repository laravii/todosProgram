using Bogus;
using Todo.Domain.Requests;

namespace Todo.Tests.Builders
{
    public class InsertTodoRequestBuilder : BaseBuilder<InsertTodoRequest>
    {
        private readonly static Faker _faker = new("pt_BR");
        private string _title = _faker.Random.Word();

        public override InsertTodoRequest Build() =>
            Faker
            .RuleFor(p => p.Title, _title)
            .RuleFor(p => p.Description, f => f.Lorem.Lines(1))
            .RuleFor(p => p.DaysToFinish, f => f.Random.Int(1, 10));

        public InsertTodoRequestBuilder WithTitle(string title)
        {
            _title = title;
            return this;
        }
    }
}