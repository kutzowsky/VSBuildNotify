using System.Windows.Forms;
using VSBuildNotify.Notifiers;

namespace VSBuildNotify.Options
{
    public partial class OptionsPageControl : UserControl
    {
        internal OptionsPage optionsPage;
        public OptionsPageControl()
        {
            InitializeComponent();
        }

        internal void Initialize()
        {
            txtTitle.Text = optionsPage.GeneralOptions.NotificationTitle;
            txtSuccessText.Text = optionsPage.GeneralOptions.SucessText;
            txtFailureText.Text = optionsPage.GeneralOptions.FailureText;

            cmbNotificationType.SelectedIndex = (int)NotifierType.MESSAGE_BOX;

            txtPushbulletAuthToken.Text = optionsPage.PushbulletOptions.AuthToken;
            txtPushbulletDeviceId.Text = optionsPage.PushbulletOptions.TargetDeviceId;
        }

        private void txtTitle_Leave(object sender, System.EventArgs e)
        {
            optionsPage.GeneralOptions.NotificationTitle = ((TextBox)sender).Text;
        }
    }
}
