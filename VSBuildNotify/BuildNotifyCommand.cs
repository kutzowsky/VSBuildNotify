using System;
using System.ComponentModel.Design;
using EnvDTE;
using Microsoft.VisualStudio.Shell;
using Task = System.Threading.Tasks.Task;

namespace VSBuildNotify
{
    internal sealed class BuildNotifyCommand
    {
        public const int CommandId = 0x0100;

        public static readonly Guid CommandSet = new Guid("b5efc83a-1c15-4603-86c1-282e30c07825");

        private readonly AsyncPackage package;

        private BuildNotifyCommand(AsyncPackage package, OleMenuCommandService commandService)
        {
            this.package = package ?? throw new ArgumentNullException(nameof(package));
            commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));

            var menuCommandID = new CommandID(CommandSet, CommandId);
            var menuItem = new MenuCommand(this.Execute, menuCommandID);
            commandService.AddCommand(menuItem);
        }

        public static BuildNotifyCommand Instance
        {
            get;
            private set;
        }

        private Microsoft.VisualStudio.Shell.IAsyncServiceProvider ServiceProvider
        {
            get
            {
                return package;
            }
        }

        public static async Task InitializeAsync(AsyncPackage package)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(package.DisposalToken);

            OleMenuCommandService commandService = await package.GetServiceAsync((typeof(IMenuCommandService))) as OleMenuCommandService;
            Instance = new BuildNotifyCommand(package, commandService);
        }

        private void Execute(object sender, EventArgs e)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var commandPackage = (BuildNotifyCommandPackage)package;
            commandPackage.CommandExecuted = true;

            var dte = Package.GetGlobalService(typeof(DTE)) as EnvDTE80.DTE2;
            dte.Solution.SolutionBuild.Build();
        }
    }
}
