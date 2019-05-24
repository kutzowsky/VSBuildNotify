using VSBuildNotify.Notifiers;

namespace VSBuildNotify.Options
{
    public class GeneralOptions
    {
        public string NotificationTitle { get; set; } = "Build status";

        public string SucessText { get; set; } = "Build completed successfully!";
        public string FailureText { get; set; } = "Build failed :(";

        public NotifierType NotifierType { get; set; } = NotifierType.MESSAGE_BOX;
    }
}
