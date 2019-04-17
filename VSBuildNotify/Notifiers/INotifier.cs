using VSBuildNotify.DTO;

namespace VSBuildNotify.Notifiers
{
    interface INotifier
    {
        void Send(Notification notification);
    }
}
