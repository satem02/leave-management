using FluentValidation;

namespace LeaveManagement.Application.Employees.Commands.CreateEmployee
{
    public class CreateLeaveCommandValidator : AbstractValidator<CreateEmployeeCommand>
    {
        public CreateLeaveCommandValidator()
        {
            RuleFor(v => v.Name)
                .NotEmpty();
        }
    }
}
