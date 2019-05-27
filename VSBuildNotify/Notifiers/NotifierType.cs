using System.ComponentModel;

namespace VSBuildNotify.Notifiers
{
    public enum NotifierType
    {
        [Description("Message box")]
        MESSAGE_BOX = 0,

        [Description("Pushbullet")]
        PUSHBULLET = 1
    }
}
