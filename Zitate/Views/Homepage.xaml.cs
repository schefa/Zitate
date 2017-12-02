using System;
using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Zitate.Helpers;
using Zitate.Model;
using Zitate.ViewModel;

namespace Zitate
{

    public sealed partial class ZViewHomepage : Page
    {

        private MainPage rootPage = MainPage.Current;
        private DateTime date = DateTime.Today;
        
        public ZViewModelHomepage ViewModelHomepage { get; set; }
        
        public ZViewHomepage()
        {
            InitializeComponent();
            ViewModelHomepage = new ZViewModelHomepage();

            var today = date.ToString("dd. MMMM");
            birthers.Text = "Geboren am " + today;
            diers.Text = "Gestorben am " + today;
            
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ZNavigationArgs args = (e.Parameter != null) ? (ZNavigationArgs)e.Parameter : new ZNavigationArgs();
            _getData(args.Refresh);
        }

        private void _getData(bool refresh = false)
        {

            if (!SuspensionManager.SessionState.ContainsKey("myCollection"))
            {
                // Meine Sammlung laden um bei bestehenden Einträge das Hinzufügen auszuschalten
                ZViewModelCollection zwmCollection = new ZViewModelCollection();
                zwmCollection.loadData();
                SuspensionManager.SessionState["myCollection"] = zwmCollection.Items;
            }
            
            if (refresh || !SuspensionManager.SessionState.ContainsKey("RandomQuotes"))
            {
                ViewModelHomepage.loadData();
                SuspensionManager.SessionState["RandomQuotes"] = ViewModelHomepage.RandomQuotes;
                SuspensionManager.SessionState["PopularAuthors"] = ViewModelHomepage.PopularAuthors;
                SuspensionManager.SessionState["Birthdays"] = ViewModelHomepage.Birthdays;
                SuspensionManager.SessionState["Deathdays"] = ViewModelHomepage.Deathdays;
            }

            randomQuotes.Source = SuspensionManager.SessionState["RandomQuotes"];
            cvs1.Source = SuspensionManager.SessionState["PopularAuthors"];
            cvs2.Source = SuspensionManager.SessionState["Birthdays"];
            cvs3.Source = SuspensionManager.SessionState["Deathdays"];
            
        }

        private void goToAuthor(object sender, ItemClickEventArgs e)
        {
            Frame.Navigate(typeof(ZViewAuthor), e.ClickedItem as ZModelAuthor);
        }

        private void goToItem(object sender, ItemClickEventArgs e)
        {
            SuspensionManager.SessionState["ItemsList"] = e.OriginalSource;
            Frame.Navigate(typeof(ZViewItem), e.ClickedItem as ZModelItem);
            MainPage.Current.checkAdditionButtons();
        }

        private void goToItem(object sender, RoutedEventArgs e)
        {
            ListView listView = new ListView();
            listView.ItemsSource = cvs1.Source;

            SuspensionManager.SessionState["ItemsList"] = cvs1 ;
            Button clickedItem = e.OriginalSource as Button;
            Frame.Navigate(typeof(ZViewItem), clickedItem.DataContext as ZModelItem);
            MainPage.Current.checkAdditionButtons();
        }

    }

}
