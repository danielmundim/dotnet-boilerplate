using DotnetBoilerplate.Infrastructure.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace DotnetBoilerplate.Infrastructure.Mediator.Notifications
{
    public class NotificationContext : INotificationContext
    {
        private readonly List<Notification> _notifications;

        public NotificationContext()
        {
            _notifications = new List<Notification>();
        }

        public bool HasErrorNotifications
            => _notifications.Any();

        public void NotifySuccess(string message)
            => Notify(message, NotificationType.Success);

        public void NotifyError(string message)
            => Notify(message, NotificationType.Error);

        private void Notify(string message, NotificationType type)
            => _notifications.Add(new Notification(type, message));

        public List<Notification> GetErrorNotifications()
            => _notifications.Where(a => a.Type == NotificationType.Error).ToList();

        public List<Notification> GetSuccessNotifications()
            => _notifications.Where(a => a.Type == NotificationType.Success).ToList();
    }
}
