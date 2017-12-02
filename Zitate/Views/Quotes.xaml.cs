using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Zitate.Helpers;
using Zitate.Model;
using Zitate.ViewModel;

namespace Zitate
{

    public sealed partial class ZViewQuotes : Page
    {

        MainPage rootPage = MainPage.Current;
        public ZViewModelItems ViewModelItems { get; set; }
        
        public ZViewQuotes()
        {
            InitializeComponent();
            ViewModelItems = new ZViewModelItems();
        }

        private void goToItem(object sender, ItemClickEventArgs e)
        {
            SuspensionManager.SessionState["ItemsList"] = e.OriginalSource; 
            Frame.Navigate(typeof(ZViewItem), e.ClickedItem as ZModelItem);
            rootPage.checkAdditionButtons();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {  
            ZNavigationArgs args = (e.Parameter != null) ? (ZNavigationArgs)e.Parameter : new ZNavigationArgs();
            _getData(args.SearchValue, args.Refresh);
        }
        
        private void _getData(string searchValue = "0", bool refresh = false)
        {
            if (searchValue == null || searchValue.Length <= 0)
            {
                // Landing Page
                if (refresh || !SuspensionManager.SessionState.ContainsKey("QuotesLandingPagePopular"))
                {
                    ViewModelItems.loadData();
                    SuspensionManager.SessionState["QuotesLandingPagePopular"] = ViewModelItems.PopularItems;
                    SuspensionManager.SessionState["QuotesLandingPageRandom"] = ViewModelItems.RandomItems;
                }
                cvs2.Source = SuspensionManager.SessionState["QuotesLandingPagePopular"];
                cvs3.Source = SuspensionManager.SessionState["QuotesLandingPageRandom"];
                quotesTabs.Items.Remove(hub1);
            }
            else
            {
                // Search
                if (refresh || (SuspensionManager.SessionState.ContainsKey("QuotesSearchValue") && searchValue != (string)SuspensionManager.SessionState["QuotesSearchValue"])
                    || !SuspensionManager.SessionState.ContainsKey("QuotesSearchItems"))
                {
                    ViewModelItems.loadData(searchValue);
                    SuspensionManager.SessionState["QuotesSearchValue"] = searchValue;
                    SuspensionManager.SessionState["QuotesSearchItems"] = ViewModelItems.Items;
                }
                cvs1.Source = SuspensionManager.SessionState["QuotesSearchItems"];
                quotesTabs.Items.Remove(hub2);
                quotesTabs.Items.Remove(hub3);
            }
        }
        
    }

}
