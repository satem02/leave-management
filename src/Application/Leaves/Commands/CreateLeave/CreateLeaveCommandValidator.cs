using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using LeaveManagement.Application.Common.Interfaces;

namespace LeaveManagement.Application.Leaves.Commands.CreateLeave
{
    public class CreateLeaveCommandValidator : AbstractValidator<CreateLeaveCommand>
    {
        private readonly IWorkDayService _workDayService;
        public CreateLeaveCommandValidator(IWorkDayService workDayService)
        {
            _workDayService = workDayService;
        }
        public CreateLeaveCommandValidator()
        {
            RuleFor(v => v.EmployeeId)
                .NotEmpty();
            
            // RuleFor(m => new {m.StartDate, m.EndDate})
            //     .Must(x => BeSuccessDate(x.StartDate, x.EndDate))
            //     .WithMessage("Hatali tarih girisi");

        }


        public bool BeSuccessDate(DateTime startDate, DateTime endDate)
        {
            var daysCount = _workDayService.CalculateBusinessDays(startDate,endDate);
            return daysCount > 0;
        }
    }
}
