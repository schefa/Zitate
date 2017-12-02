using System.ComponentModel;

namespace Zitate.ViewModel
{
    public class ZViewModel : INotifyPropertyChanged
    {

        protected MainPage rootPage = MainPage.Current;
        protected string JSONdata = "[]";

        public event PropertyChangedEventHandler PropertyChanged;
        protected bool _isLoading = false;
        public bool IsLoading
        {
            get { return _isLoading; }
            set
            {
                _isLoading = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("IsLoading"));
            }
        }

    }
}
