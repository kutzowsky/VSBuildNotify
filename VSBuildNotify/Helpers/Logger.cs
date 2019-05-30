using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Threading;

namespace VSBuildNotify.Helpers
{
    public class Logger
    {
        private AsyncPackage _package;

        public Logger(AsyncPackage package)
        {
            _package = package;
        }

        public void Info(string message)
        {
            WriteEntry(__ACTIVITYLOG_ENTRYTYPE.ALE_INFORMATION, message);
        }

        public void Warning(string message)
        {
            WriteEntry(__ACTIVITYLOG_ENTRYTYPE.ALE_WARNING, message);
        }

        public void Error(string message)
        {
            WriteEntry(__ACTIVITYLOG_ENTRYTYPE.ALE_ERROR, message);
        }

        private void WriteEntry(__ACTIVITYLOG_ENTRYTYPE entryType, string description)
        {
            ThreadHelper.JoinableTaskFactory.Run(async delegate {
                await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
                if (!(await _package.GetServiceAsync(typeof(SVsActivityLog)) is IVsActivityLog logger)) return;

                logger.LogEntry((uint)__ACTIVITYLOG_ENTRYTYPE.ALE_ERROR, _package.ToString(), description);
            });
        }
    }
}
