using System;
using VSBuildNotify.Notifiers.Pushbullet;
using VSBuildNotify.Page.Options;

namespace VSBuildNotify.Notifiers
{
    class NotifierFactory
    {
        private IServiceProvider _serviceProvider;
        private OptionsPage _options;

        public NotifierFactory(IServiceProvider serviceProvider, OptionsPage options)
        {
            _serviceProvider = serviceProvider;
            _options = options;
        }

        public INotifier GetNotifier(NotifierType notifierType)
        {
            switch(notifierType)
            {
                case NotifierType.MESSAGE_BOX:
                default: 
                    return new MessageBoxNotifier(_serviceProvider);
                case NotifierType.PUSHBULLET:
                    var pushbulletClient = new PushbulletClient(_options.PushbulletAuthToken);
                    return new PushbulletNotifier(pushbulletClient, _options.PushbulletTargetDeviceId);
            }
        }
    }
}
