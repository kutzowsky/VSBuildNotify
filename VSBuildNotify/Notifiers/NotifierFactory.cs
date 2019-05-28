using System;
using VSBuildNotify.Notifiers.Pushbullet;
using VSBuildNotify.Options.DTO;

namespace VSBuildNotify.Notifiers
{
    class NotifierFactory
    {
        private IServiceProvider _serviceProvider;
        private GeneralOptions _options;

        public NotifierFactory(IServiceProvider serviceProvider, GeneralOptions options)
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
                    return new PushbulletNotifier(_options.Pushbullet);
            }
        }
    }
}
