namespace WindowsBusinessAppXamlTemplate3.Views
{
    using Microsoft.Practices.Prism.StoreApps.Interfaces;
    using Windows.UI.Xaml.Controls;

    public sealed partial class AboutFlyout : SettingsFlyout
    {
        public AboutFlyout()
        {
            this.InitializeComponent();

            var viewModel = this.DataContext as IFlyoutViewModel;
            viewModel.CloseFlyout = () => this.Hide();
        }
    }
}
