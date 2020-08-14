using System;
using System.Linq;
using System.Threading.Tasks;
using LeaveManagement.Application.Common.Interfaces;

namespace LeaveManagement.Infrastructure.Services
{
    public class WorkDayService : IWorkDayService
    {
        private readonly IApplicationDbContext _context;

        public WorkDayService(IApplicationDbContext context)
        {
            _context = context;
        }

        public int CalculateBusinessDays(DateTime startDate, DateTime endDate)
        {
            var workDayCount = CalculateWorkDay(startDate,endDate);
            var holidayDayCount = CalculateHolidayDay(startDate,endDate);
            
            var result = workDayCount - holidayDayCount;
            
            return result;
        }

        private int CalculateWorkDay(DateTime startDate, DateTime endDate)
        {
            var totalDays = 0;
                for (var date = startDate; date < endDate; date = date.AddDays(1))
                {
                    if (date.DayOfWeek != DayOfWeek.Saturday
                        && date.DayOfWeek != DayOfWeek.Sunday)
                        totalDays++;
                }

                return totalDays;
        }

        
        private int CalculateHolidayDay(DateTime startDate, DateTime endDate)
        {
            var result = _context.PublicHolidays.Where(x=>(x.ActiveDate.CompareTo(startDate) == 1) && (x.ActiveDate.CompareTo(endDate) == 1)).Count();
            return result;
        }

    }
}