namespace ProvisionPadel.Api.Shared.Notifications;

public class Notifier : INotifier
{
    private List<Notification> _notifications;
    public Notifier()
    {
        _notifications = new List<Notification>();
    }

    public List<Notification> GetNotifications()
    {
        return _notifications;
    }

    public void Add(string message)
    {
        var notification = new Notification(message);

        _notifications.Add(notification);
    }

    public bool HasNotifications()
    {
        return _notifications.Any();
    }
}
