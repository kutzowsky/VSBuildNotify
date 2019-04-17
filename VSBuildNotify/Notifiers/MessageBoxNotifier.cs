using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using VSBuildNotify.DTO;

namespace VSBuildNotify.Notifiers
{
    public class MessageBoxNotifier : INotifier
    {
        private IServiceProvider _serviceProvider;

        public MessageBoxNotifier(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void Send(Notification notification)
        {
            VsShellUtilities.ShowMessageBox(
                _serviceProvider,
                notification.Body,
                notification.Title,
                OLEMSGICON.OLEMSGICON_INFO,
                OLEMSGBUTTON.OLEMSGBUTTON_OK,
                OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST);
        }
    }
}
