using System;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.Win32;
using VSBuildNotify.DTO;
using VSBuildNotify.Notifiers;
using VSBuildNotify.Notifiers.Pushbullet;
using VSBuildNotify.Options;
using Task = System.Threading.Tasks.Task;

namespace VSBuildNotify
{
    /// <summary>
    /// This is the class that implements the package exposed by this assembly.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The minimum requirement for a class to be considered a valid package for Visual Studio
    /// is to implement the IVsPackage interface and register itself with the shell.
    /// This package uses the helper classes defined inside the Managed Package Framework (MPF)
    /// to do it: it derives from the Package class that provides the implementation of the
    /// IVsPackage interface and uses the registration attributes defined in the framework to
    /// register itself and its components with the shell. These attributes tell the pkgdef creation
    /// utility what data to put into .pkgdef file.
    /// </para>
    /// <para>
    /// To get loaded into VS, the package must be referred by &lt;Asset Type="Microsoft.VisualStudio.VsPackage" ...&gt; in .vsixmanifest file.
    /// </para>
    /// </remarks>
    [PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)] // Info on this package for Help/About
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [Guid(BuildNotifyCommandPackage.PackageGuidString)]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "pkgdef, VS and vsixmanifest are valid VS terms")]
    [ProvideOptionPage(typeof(OptionsPage), "Build Notify", "Basic settings", 0, 0, true)]
    public sealed class BuildNotifyCommandPackage : AsyncPackage
    {
        /// <summary>
        /// BuildNotifyCommandPackage GUID string.
        /// </summary>
        public const string PackageGuidString = "d2a9f380-f42f-43bd-855c-5b8f08bb8c08";

        public BuildEvents BuildEvents { get; private set; }

        public bool CommandExecuted { get; set; }

        private bool _overallBuildSuccess = true;

        /// <summary>
        /// Initializes a new instance of the <see cref="BuildNotifyCommandPackage"/> class.
        /// </summary>
        public BuildNotifyCommandPackage()
        {
            // Inside this method you can place any initialization code that does not require
            // any Visual Studio service because at this point the package object is created but
            // not sited yet inside Visual Studio environment. The place to do all the other
            // initialization is the Initialize method.
        }

        #region Package Members

        /// <summary>
        /// Initialization of the package; this method is called right after the package is sited, so this is the place
        /// where you can put all the initialization code that rely on services provided by VisualStudio.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token to monitor for initialization cancellation, which can occur when VS is shutting down.</param>
        /// <param name="progress">A provider for progress updates.</param>
        /// <returns>A task representing the async work of package initialization, or an already completed task if there is none. Do not return null from this method.</returns>
        protected override async Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
        {
            var dte = GetGlobalService(typeof(DTE)) as DTE2;

            // When initialized asynchronously, the current thread may be a background thread at this point.
            // Do any initialization that requires the UI thread after switching to the UI thread.
            await this.JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);

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
                // We could hardcode it all
                // So that is what wee did... (add some configuration somehow)

                string messageTitle = "Build completed";    
                string messageBody = _overallBuildSuccess ? "Success!!!" : "Error :(";
                var notification = new Notification(messageTitle, messageBody);

                const string PUSHBULLET_AUTH_TOKEN = "TOP SECRET";
                const string TARGET_DEVICE_ID = "SOME GUID";

                //TODO: maybe some logging?
                //TODO: error handling

                var notifier = new PushbulletNotifier(new PushbulletClient(PUSHBULLET_AUTH_TOKEN), TARGET_DEVICE_ID); //TODO: use name of a device instead of internal Pushbullet ID

                notifier.Send(notification);

                _overallBuildSuccess = true;
                CommandExecuted = false;
            }
        }
        #endregion
    }
}
