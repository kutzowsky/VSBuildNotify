using Microsoft.VisualStudio.Shell;
using System.ComponentModel;
using System.Runtime.InteropServices;
using VSBuildNotify.Notifiers;

namespace VSBuildNotify.Options
{
    [Guid("1D9ECCF3-5D2F-4112-9B25-264596873DC9")]
    public class OptionsPage : UIElementDialogPage, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public string NotificationTitle { get { return _notificationTitle; } set { _notificationTitle = value; NotifyPropertyChanged("NotificationTitle"); } }
        private string _notificationTitle = "Build status";

        public string SucessText { get { return _sucessText; } set { _sucessText = value; NotifyPropertyChanged("SucessText"); } }
        private string _sucessText = "Build completed successfully!";

        public string FailureText { get { return _failureText; } set { _failureText = value; NotifyPropertyChanged("FailureText"); } }
        private string _failureText = "Build failed :(";

        public NotifierType NotifierType { get { return _notifierType; } set { _notifierType = value; NotifyPropertyChanged("NotifierType"); } }
        private NotifierType _notifierType = NotifierType.MESSAGE_BOX;

        public string PushbulletAuthToken { get { return _pushbulletAuthToken; } set { _pushbulletAuthToken = value; NotifyPropertyChanged("PushbulletAuthToken"); } }
        private string _pushbulletAuthToken = "AUTH TOKEN";

        public string PushbulletTargetDeviceId { get { return _pushbulletTargetDeviceId; } set { _pushbulletTargetDeviceId = value; NotifyPropertyChanged("PushbulletTargetDeviceId"); } }
        private string _pushbulletTargetDeviceId = "AUTH TOKEN";

        public void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected override System.Windows.UIElement Child
        {
            get { return new OptionsPageControl(this); }
        }
    }
}
