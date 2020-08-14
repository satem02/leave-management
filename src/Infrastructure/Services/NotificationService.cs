using LeaveManagement.Application.Common.Interfaces;
using Microsoft.Extensions.Logging;

namespace LeaveManagement.Infrastructure.Services
{
    public class NotificationService : INotificationService
    {
        private readonly ILogger<NotificationService> _logger;
        public NotificationService(ILogger<NotificationService> logger)
        {
            _logger = logger;
        }
        public void SendNotification(int userId, object content)
        {
            _logger.LogInformation(" UserId : {@userId}  Message: {@content}" , userId,content);
        }
    }
}