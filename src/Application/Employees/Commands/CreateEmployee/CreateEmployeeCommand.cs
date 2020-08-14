using LeaveManagement.Application.Common.Interfaces;
using LeaveManagement.Domain.Entities;
using LeaveManagement.Application.Common.Models;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using System;

namespace LeaveManagement.Application.Employees.Commands.CreateEmployee
{
    public class CreateEmployeeCommand : IRequest<Result>
    {
        public string Name { get; set; }
        public Nullable<int> EmployeeManagerId { get; set; }
    }

    public class CommandHandler : IRequestHandler<CreateEmployeeCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly ILeaveService _leaveService;


        public CommandHandler(IApplicationDbContext context, ILeaveService leaveService)
        {
            _context = context;
            _leaveService = leaveService;
        }

        public async Task<Result> Handle(CreateEmployeeCommand request, CancellationToken cancellationToken)
        {

            var entity = new Employee
            {
                Name = request.Name,
                EmployeeManagerId = request.EmployeeManagerId.ValueOrNull()
            };

            _context.Employees.Add(entity);

            _leaveService.AddDefaultLeave(entity.Id);

            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success(entity.Id);
        }

    }
}

