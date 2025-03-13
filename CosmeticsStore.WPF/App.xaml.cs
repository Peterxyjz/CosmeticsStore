using System.Windows;

namespace CosmeticsStore.WPF
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Start with login window
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();
        }
    }
}