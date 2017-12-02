using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Zitate.Helpers;
using Zitate.Model;
using Zitate.ViewModel;

namespace Zitate
{

    public sealed partial class ZViewTopics : Page
    {

        MainPage rootPage = MainPage.Current;
        public ZViewModelTopics ViewModelTopics { get; set; }

        private ZModelTopic selectedTopic;
        public string SelectedTopic
        {
            get
            {
                return "Zitate über: "+ selectedTopic.ToString();
            }
        }

        public ZViewTopics()
        {
            InitializeComponent();
            ViewModelTopics = new ZViewModelTopics();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ZNavigationArgs args = (e.Parameter == null) ? new ZNavigationArgs() : (ZNavigationArgs) e.Parameter;
            _getData(args.SearchValue, args.Refresh);
        }
        
        private void goToItem(object sender, ItemClickEventArgs e)
        {
            SuspensionManager.SessionState["TopicsSearchItems"] = e.OriginalSource;
            SuspensionManager.SessionState["ItemsList"] = e.OriginalSource;
            Frame.Navigate(typeof(ZViewItem), e.ClickedItem as ZModelItem);
            rootPage.checkAdditionButtons();
        }

        private void goToTag(object sender, ItemClickEventArgs e)
        {
            var topic = e.ClickedItem as ZModelTopic;
            ZNavigationArgs args = new ZNavigationArgs() { SearchValue = topic.id.ToString() };
            Frame.Navigate(typeof(ZViewTopics), args);
        }

        private void _getData(string searchValue = "0", bool refresh = false)
        {
            int n;
            bool isNumeric = int.TryParse(searchValue, out n);

            if (isNumeric)
            {
                // Tag Items
                if (refresh || !SuspensionManager.SessionState.ContainsKey("TopicsItems"))
                {
                    ViewModelTopics.loadData(searchValue, "topic");
                    SuspensionManager.SessionState["TopicsItems"] = ViewModelTopics.TopicItems;
                }
                selectedTopic = ViewModelTopics.SearchedTopic;
               
                cvs3.Source = SuspensionManager.SessionState["TopicsItems"];
                _showTabs(false, false, true);
                if(selectedTopic != null)
                    pageTitle.Text = "Zitate über: " + selectedTopic.name;
            }
            else if (searchValue == null || searchValue.Length <= 0)
            {
                // Landing Page 
                if (refresh || !SuspensionManager.SessionState.ContainsKey("PopularTopics"))
                {
                    ViewModelTopics.loadData();
                    SuspensionManager.SessionState["PopularTopics"] = ViewModelTopics.PopularTopics;
                }
                cvs2.Source = SuspensionManager.SessionState["PopularTopics"];
                _showTabs(false, true, false);
                pageTitle.Text = "Populäre Themen";
            }
            else
            {
                // Search
                if (refresh || (SuspensionManager.SessionState.ContainsKey("TopicsSearchValue")
                    && searchValue != (string)SuspensionManager.SessionState["TopicsSearchValue"])
                    || !SuspensionManager.SessionState.ContainsKey("TopicsSearchItems"))
                {
                    ViewModelTopics.loadData(searchValue, "tags");
                    SuspensionManager.SessionState["TopicsSearchValue"] = searchValue;
                    SuspensionManager.SessionState["TopicsSearchItems"] = ViewModelTopics.PopularTopics;
                }
                cvs1.Source = SuspensionManager.SessionState["TopicsSearchItems"];
                _showTabs(true, false, false);
                pageTitle.Text = "Suchergebnisse";
            }

        }

        private void _showTabs(bool first, bool second, bool third = false)
        {
            hub1.Visibility = (first) ? Windows.UI.Xaml.Visibility.Visible : Windows.UI.Xaml.Visibility.Collapsed;
            hub2.Visibility = (second) ? Windows.UI.Xaml.Visibility.Visible : Windows.UI.Xaml.Visibility.Collapsed;
            hub3.Visibility = (third) ? Windows.UI.Xaml.Visibility.Visible : Windows.UI.Xaml.Visibility.Collapsed;
        }
    }
}
