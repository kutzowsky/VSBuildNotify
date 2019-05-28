using VSBuildNotify.Notifiers;

namespace VSBuildNotify.Options.DTO
{
    public class CommonOptions
    {
        public string NotificationTitle { get; set; }

        public string SucessText { get; set; }
        public string FailureText { get; set; }

        public NotifierType NotifierType { get; set; }
    }
}
