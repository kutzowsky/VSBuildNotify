using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Threading;
using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.Shell;
using VSBuildNotify.Notifiers;
using VSBuildNotify.Notifiers.DTO;
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
    public sealed class BuildNotifyCommandPackage : AsyncPackage
    {
        public const string PackageGuidString = "d2a9f380-f42f-43bd-855c-5b8f08bb8c08";

        public BuildEvents BuildEvents { get; private set; }

        public bool CommandExecuted { get; set; }

        private bool _overallBuildSuccess = true;

        protected override async Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
        {
            var dte = GetGlobalService(typeof(DTE)) as DTE2;

            await JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);

            BuildEvents = dte.Events.BuildEvents;

            BuildEvents.OnBuildDone += OnSolutionBuildDone;
            BuildEvents.OnBuildProjConfigDone += OnProjectBuildDone;

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
                var optionsPage = (OptionsPage)GetDialogPage(typeof(OptionsPage));
                var options = optionsPage.GetGeneralOptions();

                string messageTitle = options.Common.NotificationTitle;
                string messageBody = _overallBuildSuccess ? options.Common.SucessText : options.Common.FailureText;
                var notification = new Notification(messageTitle, messageBody);

                //TODO: maybe some logging?
                //TODO: error handling

                var notifierFactory = new NotifierFactory(this, options);

                var notifier = notifierFactory.GetNotifier(options.Common.NotifierType);
                notifier.Send(notification);

                _overallBuildSuccess = true;
                CommandExecuted = false;
            }
        }
    }
}
