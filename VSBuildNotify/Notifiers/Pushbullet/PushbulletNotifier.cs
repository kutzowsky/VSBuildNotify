using VSBuildNotify.Notifiers.DTO;

namespace VSBuildNotify.Notifiers.Pushbullet
{
    public class PushbulletNotifier : INotifier
    {
        public string TargetDeviceId;
        private PushbulletClient _pushbulletClient;

        public PushbulletNotifier(PushbulletClient pushbulletClient, string targetDeviceId) //TODO: use name of a device instead of internal Pushbullet ID
        {
            _pushbulletClient = pushbulletClient;
            TargetDeviceId = targetDeviceId;
        }

        public void Send(Notification notification)
        {
            _pushbulletClient.PushTo(TargetDeviceId, notification);
        }
    }
}
