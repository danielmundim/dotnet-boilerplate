namespace DotnetBoilerplate.Infrastructure.Mediator.Notifications
{
    public class Notification
    {
        public NotificationType Type { get; protected set; }
        public string Value { get; protected set; }

        public Notification(NotificationType type, string value)
        {
            Type = type;
            Value = value;
        }
    }
}
