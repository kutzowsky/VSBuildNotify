using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Threading;
using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.Shell;
using VSBuildNotify.Helpers;
using VSBuildNotify.Notifiers;
using VSBuildNotify.Notifiers.DTO;
using VSBuildNotify.Options.DTO;
using VSBuildNotify.Page.Options;
using Task = System.Threading.Tasks.Task;

namespace VSBuildNotify
{
    [PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)] // Info on this package for Help/About
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [Guid(PackageGuidString)]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "pkgdef, VS and vsixmanifest are valid VS terms")]
    [ProvideOptionPage(typeof(OptionsPage), "Build Notify", "Basic settings", 0, 0, true)]
    public sealed class BuildNotifyCommandPackage : AsyncPackage, INotificationService
    {
        public const string PackageGuidString = "d2a9f380-f42f-43bd-855c-5b8f08bb8c08";

        public BuildEvents BuildEvents { get; private set; }

        public bool CommandExecuted { get; set; }

        private bool _overallBuildSuccess = true;

        private Logger _logger;
        private ExceptionHandler _exceptionHandler;

        protected override async Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
        {
            var dte = GetGlobalService(typeof(DTE)) as DTE2;

            await JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);

            BuildEvents = dte.Events.BuildEvents;

            BuildEvents.OnBuildDone += OnSolutionBuildDone;
            BuildEvents.OnBuildProjConfigDone += OnProjectBuildDone;

            _logger = new Logger(this);
            _exceptionHandler = new ExceptionHandler(this, _logger);

            await BuildNotifyCommand.InitializeAsync(this);
        }

        private void OnProjectBuildDone(string Project, string ProjectConfig, string Platform, string SolutionConfig, bool Success)
        {
            if (CommandExecuted)
            {
                _overallBuildSuccess = _overallBuildSuccess && Success;
            }
        }

        private void OnSolutionBuildDone(vsBuildScope Scope, vsBuildAction Action)
        {
            if (CommandExecuted)
            {
                _logger.Info("Solution build done");

                var optionsPage = (OptionsPage)GetDialogPage(typeof(OptionsPage));
                var options = optionsPage.GetGeneralOptions();

                string messageTitle = options.Common.NotificationTitle;
                string messageBody = _overallBuildSuccess ? options.Common.SucessText : options.Common.FailureText;
                var notification = new Notification(messageTitle, messageBody);

                SendNotification(notification, options);

                _overallBuildSuccess = true;
                CommandExecuted = false;
            }
        }

        public void SendNotification(Notification notification, GeneralOptions options)
        {
            var notifierFactory = new NotifierFactory(this, options);
            var notifierType = options.Common.NotifierType;

            var notifier = notifierFactory.GetNotifier(notifierType);

            try
            {
                _logger.Info($"Sending notification using {notifierType.ToString()} notifier");
                notifier.Send(notification);
                _logger.Info("Notification sent");
            }
            catch (Exception exception)
            {
                _exceptionHandler.Handle(exception);
            }
        }
    }
}