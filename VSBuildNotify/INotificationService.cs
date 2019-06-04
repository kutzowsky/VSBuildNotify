using VSBuildNotify.Notifiers.DTO;
using VSBuildNotify.Options.DTO;

namespace VSBuildNotify
{
    public interface INotificationService
    {
        void SendNotification(Notification notification, GeneralOptions options);
    }
}
