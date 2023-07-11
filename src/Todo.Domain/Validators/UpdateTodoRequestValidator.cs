using FluentValidation;
using Todo.Domain.Requests;

namespace Todo.Domain.Validators
{
    public class UpdateTodoRequestValidator : AbstractValidator<UpdateTodoRequest>
    {
        public UpdateTodoRequestValidator()
        {
            RuleFor(req => req.Title)
                .MaximumLength(20)
                .MinimumLength(3);

            RuleFor(req => req.Description)
                .MaximumLength(100)
                .MinimumLength(10);
        }
    }
}
