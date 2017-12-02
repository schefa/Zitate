using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Zitate.Helpers;
using Zitate.Model;
using Zitate.ViewModel;

namespace Zitate
{

    public sealed partial class ZViewCollection : Page
    { 

        private MainPage rootPage = MainPage.Current;
        public ZViewModelCollection ViewModelCollection { get; set; }

        public ZViewCollection()
        {
            InitializeComponent();
            ViewModelCollection = new ZViewModelCollection();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ZNavigationArgs args = (e.Parameter != null) ? (ZNavigationArgs)e.Parameter : new ZNavigationArgs();
            _getData(args.Refresh);
        }

        private void _getData(bool refresh = false)
        {
            if (refresh || !SuspensionManager.SessionState.ContainsKey("myCollection"))
            {
                ViewModelCollection.loadData();
                SuspensionManager.SessionState["myCollection"] = ViewModelCollection.Items; 
            }

            myCollection.Source = SuspensionManager.SessionState["myCollection"];
        }

        private void goToAuthor(object sender, ItemClickEventArgs e)
        {
            Frame.Navigate(typeof(ZViewAuthor), e.ClickedItem as ZModelAuthor);
        }

        private void goToItem(object sender, ItemClickEventArgs e)
        {
            SuspensionManager.SessionState["ItemsList"] = e.OriginalSource;
            Frame.Navigate(typeof(ZViewItem), e.ClickedItem as ZModelItem);
            rootPage.checkAdditionButtons();
        }

    }
}
