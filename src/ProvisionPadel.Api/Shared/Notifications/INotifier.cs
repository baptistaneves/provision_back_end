using MediatR;

namespace ProvisionPadel.Api.Shared.Notifications;

public interface INotifier
{
    bool HasNotifications();
    List<Notification> GetNotifications();
    void Add(string message);
}