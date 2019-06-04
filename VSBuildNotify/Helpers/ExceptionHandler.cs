using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Text;

namespace VSBuildNotify.Helpers
{
    class ExceptionHandler
    {
        private AsyncPackage _package;
        private Logger _logger;

        public ExceptionHandler(AsyncPackage package, Logger logger)
        {
            _package = package;
            _logger = logger;
        }

        public void Handle(Exception exception)
        {
            var title = $"Error: {exception.GetType().Name}";
            var message = $"Details:\n{exception.Message}";

            if (exception is AggregateException)
            {
                var aggregateException = exception as AggregateException;
                aggregateException = aggregateException.Flatten();
                var messageBuilder = new StringBuilder(message);

                foreach (Exception innerException in aggregateException.InnerExceptions)
                {
                    var baseException = innerException.GetBaseException();

                    messageBuilder.AppendLine(baseException.GetType().Name);
                    messageBuilder.AppendLine(baseException.Message);
                    messageBuilder.AppendLine();
                }

                message = messageBuilder.ToString();
            }

            VsShellUtilities.ShowMessageBox(
                _package,
                message,
                title,
                OLEMSGICON.OLEMSGICON_WARNING,
                OLEMSGBUTTON.OLEMSGBUTTON_OK,
                OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST);

            _logger.Info($"Error occured, {message}");
        }
    }
}
