using System;
using System.ComponentModel;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Zitate.Helpers;
using Zitate.Model;

namespace Zitate
{

    public sealed partial class MainPage : Page
    {
        private static MainPage current = null;
        public static MainPage Current { get { return current; } }
        
        public MainPage()
        {
            InitializeComponent();
            
            if (current == null) 
            {
                current = this;

                ScenarioFrame.Navigate(typeof(ZViewHomepage));

                searchCombobox.DataContext = ZNavigation.NavigationCombobox;
                searchCombobox.SelectedIndex = 0;

                scenarioListBox.DataContext = ZNavigation.NavigationItems.Values.ToList();
                scenarioListBox.SelectedIndex = 0;

            }
        }
        
        private void HamburgerRadioButton_Click(object sender, RoutedEventArgs e)
        {
            SplitView.IsPaneOpen = !SplitView.IsPaneOpen;
        }

        #region Navigation

        #region Suche
        private string searchValue;
        public string SearchValue
        {
            get {
                return searchValue;
            }

            private set {
                searchValue = value;
            }
        }

        private void searchButton_Click(object sender, RoutedEventArgs e)
        {
            ZNavigationItem s = searchCombobox.SelectedItem as ZNavigationItem;
            if (s != null)
            {
                ZNavigationArgs args = new ZNavigationArgs() { SearchValue = searchBox.Text, Refresh = true };
                _navigate(s, args);
            }
        }

        private void searchBox_KeyDown(object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            if (e.Key.ToString() == "Enter")
                searchButton_Click(sender, e);
        }


        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        { 
            ListBox scenarioListBox = sender as ListBox;
            ZNavigationItem s = scenarioListBox.SelectedItem as ZNavigationItem;
            if (s != null)
            {
                _navigate(s, null);
            }
        }
        #endregion

        private void _navigate(ZNavigationItem s, object args = null)
        {
            if (SuspensionManager.SessionState.ContainsKey("Next"))
                SuspensionManager.SessionState["Prev"] = SuspensionManager.SessionState["Next"];
            SuspensionManager.SessionState["Next"] = scenarioListBox.SelectedIndex;

            ScenarioFrame.Navigate(s.ClassType, args);

            checkAdditionButtons();
        }
        
        private void refresh_click(object sender, RoutedEventArgs e)
        {
            ZNavigationItem navItem = _frameToNavigationItem();
            ZNavigationArgs args = new ZNavigationArgs() { SearchValue = SearchValue, Refresh = true };

            if (navItem.ClassType != null)
                _navigate(navItem, args);
           
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            if(ScenarioFrame.CanGoBack)
                ScenarioFrame.GoBack();
            checkAdditionButtons();
        }
        #endregion

        #region additional navigation

        public void checkAdditionButtons()
        {
            ZNavigationItem frameToNavItem = _frameToNavigationItem();

            backButton.Visibility = (ScenarioFrame.CanGoBack) ? Visibility.Visible : Visibility.Collapsed;
            refreshButton.Visibility = ((new string[] { "Start", "Autoren", "Zitate", "Thema", "Sammlung" }).Contains(frameToNavItem.Title)) ? Visibility.Visible : Visibility.Collapsed;
            
            if (scenarioListBox.Items.Contains(frameToNavItem))
                scenarioListBox.SelectedItem = frameToNavItem;

        }

        private ZNavigationItem _frameToNavigationItem()
        {
            Type type = ScenarioFrame.Content.GetType();
            string view = type.Name.ToLower();
            if (ZNavigation.NavigationItems.ContainsKey(view))
                return ZNavigation.NavigationItems[type.Name.ToLower()];
            else
                return new ZNavigationItem();
        }

        #endregion

        #region Status

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }

        // Used to display messages to the user
        public void NotifyUser(NotifyType type, string strMessage = "" )
        {
            switch (type)
            {
                case NotifyType.StatusMessage:
                    statusToolbar.Visibility = Visibility.Collapsed;
                    break;
                case NotifyType.PrepareMessage:
                    statusToolbar.Visibility = Visibility.Visible;
                    progressBar.Visibility = Visibility.Visible;
                    break;
                case NotifyType.ErrorMessage:
                    StatusBlock.Foreground = new SolidColorBrush(Windows.UI.Colors.Red);
                    progressBar.Visibility = Visibility.Collapsed;
                    break;
            }

            ScenarioFrame.Visibility = (type == NotifyType.ErrorMessage) ? Visibility.Collapsed : Visibility.Visible;
            StatusBlock.Text = strMessage;

            if (StatusBlock.Text != String.Empty)
                StatusBlock.Visibility = Visibility.Visible;
            else
                StatusBlock.Visibility = Visibility.Collapsed;

        }

        #endregion

    }

    public enum NotifyType
    {
        StatusMessage,
        PrepareMessage,
        ErrorMessage
    }
    
}
