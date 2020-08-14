using LeaveManagement.Application.Common.Interfaces;
using System;

namespace LeaveManagement.Infrastructure.Services
{
    public class DateTimeService : IDateTime
    {
        public DateTime Now => DateTime.Now;
    }
}
