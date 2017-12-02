using System.Collections.Generic;
using System.Collections.ObjectModel;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Zitate.Helpers;
using Zitate.Model;
using Zitate.ViewModel;

namespace Zitate
{
    public sealed partial class ZViewAuthors : Page
    {
        MainPage rootPage = MainPage.Current;
        public ZViewModelAuthors ViewModelAuthors { get; set; }

        public ZViewAuthors()
        {
            InitializeComponent();
            ViewModelAuthors = new ZViewModelAuthors();
        }

        private void _getData(string searchValue = "0", bool refresh = false)
        {
            if (searchValue == null || searchValue.Length <= 0)
            {
                // Landing Page
                if (refresh || !SuspensionManager.SessionState.ContainsKey("PopularAuthors") 
                    || !SuspensionManager.SessionState.ContainsKey("RandomAuthors"))
                {
                    ViewModelAuthors.loadData();
                    SuspensionManager.SessionState["PopularAuthors"] = ViewModelAuthors.PopularAuthors;
                    SuspensionManager.SessionState["RandomAuthors"] = ViewModelAuthors.RandomAuthors;
                }
                cvs2.Source = SuspensionManager.SessionState["PopularAuthors"];
                cvs3.Source = SuspensionManager.SessionState["RandomAuthors"];
                quotesTabs.Items.Remove(hub1);
            }
            else
            {
                // Search
                if (refresh || (SuspensionManager.SessionState.ContainsKey("AuthorsSearchValue") 
                    && searchValue != (string) SuspensionManager.SessionState["AuthorsSearchValue"])
                    || !SuspensionManager.SessionState.ContainsKey("AuthorsSearchItems"))
                {
                    ViewModelAuthors.loadData(searchValue);
                    SuspensionManager.SessionState["AuthorsSearchValue"] = searchValue;
                    SuspensionManager.SessionState["AuthorsSearchItems"] = ViewModelAuthors.Items;
                }
                cvs1.Source = SuspensionManager.SessionState["AuthorsSearchItems"];
                quotesTabs.Items.Remove(hub2);
                quotesTabs.Items.Remove(hub3);
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ZNavigationArgs args = (e.Parameter != null) ? (ZNavigationArgs) e.Parameter : new ZNavigationArgs();
            _getData(args.SearchValue, args.Refresh);
        }
        
        private void goToAuthor(object sender, ItemClickEventArgs e)
        { 
            SuspensionManager.SessionState["Prev"] = SuspensionManager.SessionState["Next"];
            Frame.Navigate(typeof(ZViewAuthor), e.ClickedItem as ZModelAuthor);
            rootPage.checkAdditionButtons();
        }

    }
}
