using FluentValidation;
using Todo.Domain.Models.Requests;

namespace Todo.Domain.Validators
{
    public class UpdateStatusRequestValidator : AbstractValidator<UpdateStatusRequest>
    {
        public UpdateStatusRequestValidator()
        {
            var conditions = new List<string>() { "todo", "done", "doing" };
            RuleFor(req => req.Status)
              .Must(req => conditions.Contains(req))
              .WithMessage("Please only use: " + String.Join(",", conditions));
        }
    }
}
