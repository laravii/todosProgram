using FluentValidation;
using Todo.Domain.Requests;

namespace Todo.Domain.Validators
{
    public class InsertTodoRequestValidator : AbstractValidator<InsertTodoRequest>
    {
        public InsertTodoRequestValidator()
        {
            RuleFor(req => req.Title)
                .NotNull()
                .NotEmpty()
                .MaximumLength(20)
                .MinimumLength(3);

            RuleFor(req => req.Description)
                .NotEmpty()
                .NotNull()
                .MaximumLength(100)
                .MinimumLength(10);

            RuleFor(req => req.DaysToFinish)
                .NotEmpty()
                .NotNull();
        }
    }
}
