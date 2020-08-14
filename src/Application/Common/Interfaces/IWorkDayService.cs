using System;
using System.Threading.Tasks;

namespace LeaveManagement.Application.Common.Interfaces
{
    public interface IWorkDayService
    {
         int CalculateBusinessDays(DateTime startDate , DateTime endDate);
    }
}