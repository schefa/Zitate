using System.Collections.Generic;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Zitate.Helpers;
using Zitate.Model;
using Windows.UI.Xaml;
using Zitate.ViewModel;
using System.Collections.ObjectModel;

namespace Zitate
{

    public sealed partial class ZViewItem : Page
    {

        public ZViewModelItem ViewModelItem { get; set; }
        public ZModelItem item { get; set; }
        public ListView itemsList { get; set; }

        public ZModelAuthor Author { get; set; }

        public ZViewItem()
        {
            InitializeComponent();

            if (!SuspensionManager.SessionState.ContainsKey("ItemsList"))
                return;

            itemsList = SuspensionManager.SessionState["ItemsList"] as ListView;
            
            if(itemsList.Items.Count > 1 )
            {
                prevContainer.Visibility = Visibility.Visible;
                nextContainer.Visibility = Visibility.Visible;
            } else
            {
                prevContainer.Visibility = Visibility.Collapsed;
                nextContainer.Visibility = Visibility.Collapsed;
            }
            
        }
        
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            item = (ZModelItem)e.Parameter;
            DataContext = item;
            checkIfCollected();
        }

        private void NextClick(object sender, RoutedEventArgs e)
        {
            if (itemsList.Items == null)
                return;

            int nextID = itemsList.SelectedIndex + 1;
            if(nextID < itemsList.Items.Count)
            {
                _navigateItem(nextID);
            }
        }

        private void PrevClick(object sender, RoutedEventArgs e)
        {
            if (itemsList.Items == null)
                return;

            int prevID = itemsList.SelectedIndex - 1;
            if (prevID >= 0)
            {
                _navigateItem(prevID);
            }
        }

        private void _navigateItem(int id)
        {
            itemsList.SelectedIndex = id;
            SuspensionManager.SessionState["ItemsList"] = itemsList;
            Frame.Navigate(typeof(ZViewItem), itemsList.Items[id] as ZModelItem);
            MainPage.Current.checkAdditionButtons();
        }

        private void goToAuthor(object sender, RoutedEventArgs e)
        {
            SuspensionManager.SessionState["Prev"] = SuspensionManager.SessionState["Next"];
            Frame.Navigate(typeof(ZViewAuthor), item.created_by);
            MainPage.Current.checkAdditionButtons();
        }

        private void checkIfCollected()
        {
            if (SuspensionManager.SessionState.ContainsKey("myCollection"))
            {
                ObservableCollection<ZModelItem> myCollection = SuspensionManager.SessionState["myCollection"] as ObservableCollection<ZModelItem>;
                if (myCollection == null)
                    return;

                for (int i = 0; i < myCollection.Count; i++)
                {
                    if (item.id == myCollection[i].id)
                    {
                        merkenButton.IsEnabled = false;
                        merkenButtonText.Text = "Gemerkt";
                        return;
                    }
                }
            }
        }

        private void Merken_Click(object sender, RoutedEventArgs e)
        {
            merkenButtonText.Text = "Gemerkt";
            merkenButton.IsEnabled = false;
            ZHelpersLocalData.Save(item);
        }

        /* Später
        private void nextKeyDown(object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            if (e.Key.ToString() == "right")
                NextClick(sender, e);
        }

        private void prevKeyDown(object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            if (e.Key.ToString() == "left")
                PrevClick(sender, e);
        }
        */

    }
}
