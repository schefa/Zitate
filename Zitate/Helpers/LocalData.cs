using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Zitate.Model;

namespace Zitate.Helpers
{

    public static class ZHelpersLocalData
    {

        private static StorageFolder folder = ApplicationData.Current.LocalFolder; 
        
        private static ObservableCollection<ZModelItem> sammlung = new ObservableCollection<ZModelItem>();
        public static ObservableCollection<ZModelItem> Sammlung
        {
            get
            {
                return sammlung;
            }

            private set
            {
                sammlung = value;
            }
        }

        public static async void GetCollection()
        {
            Sammlung = await getCollection(); 
        }

        public static async Task<ObservableCollection<ZModelItem>> getCollection()
        {
            ObservableCollection<ZModelItem> temp = new ObservableCollection<ZModelItem>();

            if (await FileExists("Data.xml"))
            {
                StorageFile file = await folder.GetFileAsync("Data.xml");
                using (Stream stream = await file.OpenStreamForReadAsync())
                {
                    DataContractSerializer serializer = new DataContractSerializer(sammlung.GetType());

                    ObservableCollection<ZModelItem> list = (ObservableCollection<ZModelItem>) serializer.ReadObject(stream);
                    foreach (var item in list)
                    {
                        temp.Add(item);
                    }
                }
            }
            return temp;
        }

        public static async void Save(ZModelItem item)
        {
            ObservableCollection<ZModelItem> temp = new ObservableCollection<ZModelItem>();
            bool exists = await FileExists("Data");

            // saving existing temporary, then delete file
            if (exists == true)
            {
                StorageFile file = await folder.GetFileAsync("Data.xml");
                using (Stream stream = await file.OpenStreamForReadAsync())
                {
                    DataContractSerializer serializer = new DataContractSerializer(sammlung.GetType());

                    ObservableCollection<ZModelItem> list = (ObservableCollection<ZModelItem>)serializer.ReadObject(stream);
                    temp.Add(item);
                    for (int i = 0; i < list.Count; i++)
                    {
                        if(item.id != list[i].id)
                            temp.Add(list[i]);
                        if (i == 99) break; // die leute sollen nicht die ganze Datenbank herunterladen
                    }
                }
                await file.DeleteAsync(StorageDeleteOption.PermanentDelete);
            }


            StorageFile file1 = await folder.CreateFileAsync("Data.xml", CreationCollisionOption.ReplaceExisting);
            
            using (Stream stream = await file1.OpenStreamForWriteAsync())
            {
                DataContractSerializer serializer = new DataContractSerializer(sammlung.GetType());
                serializer.WriteObject(stream, temp);
                await stream.FlushAsync(); 
            }
            
        }


        public static async Task<bool> FileExists(string filename)
        {
            IReadOnlyList<StorageFile> list_of_files = await folder.GetFilesAsync();

            foreach (StorageFile file in list_of_files)
            {
                if (file.DisplayName == "Data")
                    return true;
            }
            return false;
        }

    }
}
