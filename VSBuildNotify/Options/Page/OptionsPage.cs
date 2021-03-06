﻿using Microsoft.VisualStudio.Shell;
using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using VSBuildNotify.Helpers;
using VSBuildNotify.Notifiers;
using VSBuildNotify.Notifiers.DTO;
using VSBuildNotify.Options.DTO;
using VSBuildNotify.Options.Page;

namespace VSBuildNotify.Page.Options
{
    [Guid("1D9ECCF3-5D2F-4112-9B25-264596873DC9")]
    public class OptionsPage : UIElementDialogPage, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public string NotificationTitle { get { return _notificationTitle; } set { _notificationTitle = value; NotifyPropertyChanged(nameof(NotificationTitle)); } }
        private string _notificationTitle = "Build status";

        public string SucessText { get { return _sucessText; } set { _sucessText = value; NotifyPropertyChanged(nameof(SucessText)); } }
        private string _sucessText = "Build completed successfully!";

        public string FailureText { get { return _failureText; } set { _failureText = value; NotifyPropertyChanged(nameof(FailureText)); } }
        private string _failureText = "Build failed :(";

        public NotifierType NotifierType { get { return _notifierType; } set { _notifierType = value; NotifyPropertyChanged(nameof(NotifierType)); } }
        private NotifierType _notifierType = NotifierType.MESSAGE_BOX;

        public string PushbulletAuthToken { get { return _pushbulletAuthToken; } set { _pushbulletAuthToken = value; NotifyPropertyChanged(nameof(PushbulletAuthToken)); } }
        private string _pushbulletAuthToken = "AUTH TOKEN";

        public string PushbulletTargetDeviceId { get { return _pushbulletTargetDeviceId; } set { _pushbulletTargetDeviceId = value; NotifyPropertyChanged(nameof(PushbulletTargetDeviceId)); } }
        private string _pushbulletTargetDeviceId = "AUTH TOKEN";

        public ICommand SendTestNotificationCommand { get; }

        public OptionsPage() : base()
        {
            SendTestNotificationCommand = new CommandHandler(SendTestNotification, () => true);
        }

        public GeneralOptions GetGeneralOptions()
        {
            var commonOptions = new CommonOptions
            {
                NotificationTitle = _notificationTitle,
                SucessText = _sucessText,
                FailureText = _failureText,
                NotifierType = _notifierType
            };

            var pushbulletOptions = new PushbulletOptions
            {
                AuthToken = _pushbulletAuthToken,
                TargetDeviceId = _pushbulletTargetDeviceId
            };

            return new GeneralOptions
            {
                Common = commonOptions,
                Pushbullet = pushbulletOptions
            };
        }

        public void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void SendTestNotification()
        {
            INotificationService buildNotifyPackage = (BuildNotifyCommandPackage)GetService(typeof(BuildNotifyCommandPackage));

            string messageTitle = "Test notification";
            string messageBody = "Everything works well. Enjoy!";
            var notification = new Notification(messageTitle, messageBody);

            buildNotifyPackage.SendNotification(notification, GetGeneralOptions());
        }

        protected override UIElement Child
        {
            get { return new OptionsPageControl(this); }
        }
    }
}
