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
        private string? _title = "test tittle";
        private int _daysToFinish = 1;

        public override UpdateTodoRequest Build() =>
            Faker
            .RuleFor(p => p.Title, _title)
            .RuleFor(p => p.Description, f => f.Lorem.Lines(1))
            .RuleFor(p => p.DaysToFinish, _daysToFinish);

        public UpdateTodoRequestBuilder WithTitle(string? title = null)
        {
            _title = title;
            return this;
        }

        public UpdateTodoRequestBuilder WithDaysToFinish(int daysToFinish)
        {
            _daysToFinish = daysToFinish;
            return this;
        }
    }
}
