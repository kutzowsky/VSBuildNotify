using VSBuildNotify.Notifiers.DTO;

namespace VSBuildNotify.Notifiers
{
    interface INotifier
    {
        void Send(Notification notification);
    }
}
