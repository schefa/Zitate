using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.Serialization;
using Windows.Storage;
using Zitate.Helpers;
using Zitate.Model;

namespace Zitate.ViewModel
{

    public class ZViewModelCollection : ZViewModel
    {

        public ObservableCollection<ZModelItem> Items { get; set; }
        
        private static StorageFolder folder = ApplicationData.Current.LocalFolder; 

        public async void loadData()
        {
            IsLoading = true;
            rootPage.NotifyUser(NotifyType.PrepareMessage);

            Items = new ObservableCollection<ZModelItem>();

            try
            {

                if (await ZHelpersLocalData.FileExists("Data.xml"))
                {
                    StorageFile file = await folder.GetFileAsync("Data.xml");
                    using (Stream stream = await file.OpenStreamForReadAsync())
                    {
                        DataContractSerializer serializer = new DataContractSerializer(Items.GetType());

                        ObservableCollection<ZModelItem> list = (ObservableCollection<ZModelItem>)serializer.ReadObject(stream);
                        foreach (var item in list)
                        {
                            Items.Add(item);
                        }
                    }
                }

                rootPage.NotifyUser(NotifyType.StatusMessage);

            }
            catch (Exception ex)
            {
                rootPage.NotifyUser(NotifyType.ErrorMessage, ex.Message);
            }

            IsLoading = false;
        }

        
    }
}
