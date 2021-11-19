using DotnetBoilerplate.Infrastructure.Mediator.Notifications;
using System.Collections.Generic;

namespace DotnetBoilerplate.Infrastructure.Interfaces
{
    public interface INotificationContext
    {
        bool HasErrorNotifications { get; }
        void NotifyError(string message);
        void NotifySuccess(string message);
        List<Notification> GetErrorNotifications();
    }
}
