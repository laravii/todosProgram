using Bogus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Todo.Domain.Requests;

namespace Todo.Tests.Builders
{
    public class UpdateTodoRequestBuilder : BaseBuilder<UpdateTodoRequest>
    {
        private readonly static Faker _faker = new("pt_BR");
        private string? _title = "test tittle";

        public override UpdateTodoRequest Build() =>
            Faker
            .RuleFor(p => p.Title, _title)
            .RuleFor(p => p.Description, f => f.Lorem.Lines(1))
            .RuleFor(p => p.DaysToFinish, f => f.Random.Int(1, 10));

        public UpdateTodoRequestBuilder WithTitle(string? title = null)
        {
            _title = title;
            return this;
        }
    }
}
