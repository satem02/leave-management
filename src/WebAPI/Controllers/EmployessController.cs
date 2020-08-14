using LeaveManagement.Application.Common.Models;
using LeaveManagement.Application.Employees.Commands.CreateEmployee;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LeaveManagement.WebAPI.Controllers
{

    public class EmployessController : ApiController
    {
        [HttpPost]
        public async Task<ActionResult<Result>> Create(CreateEmployeeCommand command)
        {
            return await Mediator.Send(command);
        }
    }
}
