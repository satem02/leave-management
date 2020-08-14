using LeaveManagement.Application.Common.Models;
using LeaveManagement.Application.Leaves.Commands.CreateLeave;
using LeaveManagement.Application.Leaves.Commands.UpdateLeave;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LeaveManagement.WebAPI.Controllers
{

    public class LeavesController : ApiController
    {
        [HttpPost]
        public async Task<ActionResult<Result>> Create(CreateLeaveCommand command)
        {
            return await Mediator.Send(command);
        }

        [HttpPut("{leaveId}")]
        public async Task<ActionResult> Update(int leaveId, UpdateLeaveCommand command)
        {
            if (leaveId != command.LeaveId)
            {
                return BadRequest();
            }

            await Mediator.Send(command);

            return NoContent();
        }
    }
}
