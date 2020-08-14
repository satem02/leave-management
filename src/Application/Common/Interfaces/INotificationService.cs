namespace LeaveManagement.Application.Common.Interfaces
{
    public interface INotificationService
    {
         void SendNotification(int userId , object content);
    }
}