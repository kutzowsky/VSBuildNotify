using Microsoft.VisualStudio.Shell;
using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace VSBuildNotify.Options
{
    [Guid("CE1251D9-F1CB-4268-A58A-73D4C859EC61")]
    class OptionsPage : DialogPage
    {
        public GeneralOptions GeneralOptions { get; set; } = new GeneralOptions();
        public PushbulletOptions PushbulletOptions { get; set; } = new PushbulletOptions();

        protected override IWin32Window Window
        {
            get
            {
                OptionsPageControl page = new OptionsPageControl();
                page.optionsPage = this;
                page.Initialize();
                return page;
            }
        }
    }
}
