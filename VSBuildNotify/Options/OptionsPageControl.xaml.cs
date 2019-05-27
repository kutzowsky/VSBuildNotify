using System.Windows.Controls;
using System.Windows.Input;
using VSBuildNotify.Helpers;

namespace VSBuildNotify.Options
{
    public partial class OptionsPageControl : UserControl
    {
        private OptionsPage _options;

        public OptionsPageControl(OptionsPage options)
        {
            InitializeComponent();

            _options = options;
            DataContext = _options;
        }

        private void UserControl_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            // http://blog.danskingdom.com/adding-a-wpf-settings-page-to-the-tools-options-dialog-window-for-your-visual-studio-extension/
            foreach (var textBox in ChildrenFinder.FindVisualChildren<TextBox>(sender as UserControl))
            {
                var bindingExpression = textBox.GetBindingExpression(TextBox.TextProperty);
                if (bindingExpression != null) bindingExpression.UpdateSource();
            }
        }


    }
}
