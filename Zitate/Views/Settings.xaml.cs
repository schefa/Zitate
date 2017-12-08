using Windows.Storage;
using Windows.UI.Xaml.Controls;

namespace Zitate
{
    public sealed partial class ZViewSettings : Page
    {
        private ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;

        public ZViewSettings()
        {
            InitializeComponent(); 

            if (localSettings.Values["email"] != null)
                tb_email.Text = localSettings.Values["email"].ToString();

        }

        private void btn_save_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        { 
            localSettings.Values["email"] = tb_email.Text;
        }
    }
}
