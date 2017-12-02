using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Zitate.Helpers;
using Zitate.Model;
using Zitate.ViewModel;

namespace Zitate
{
    public sealed partial class ZViewAuthor : Page
    {
        public ZViewModelAuthor ViewModelAuthor { get; set; }
        public ZModelAuthor author { get; set; }

        public ZViewAuthor()
        {
            InitializeComponent();
            ViewModelAuthor = new ZViewModelAuthor();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            int n;
            bool isNumeric = int.TryParse(e.Parameter.ToString(), out n);

            if(isNumeric)
            {
                ViewModelAuthor.loadData(e.Parameter.ToString());
                author = ViewModelAuthor.Author;
            }
            else
            {
                author = (ZModelAuthor) e.Parameter;
                ViewModelAuthor.Author = author;
                ViewModelAuthor.loadData(author.id.ToString());
            }

            MainPage.Current.checkAdditionButtons();
        }

        private void goToItem(object sender, ItemClickEventArgs e)
        {
            SuspensionManager.SessionState["ItemsList"] = e.OriginalSource;
            Frame.Navigate(typeof(ZViewItem), e.ClickedItem as ZModelItem);
            MainPage.Current.checkAdditionButtons();
        }
    }
}
