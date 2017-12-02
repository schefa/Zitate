using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Zitate.Helpers;
using Zitate.Model;

namespace Zitate.ViewModel
{

    public class ZViewModelItem : ZViewModel
    {
        
        public ObservableCollection<ZModelItem> Items { get; set; }
        
        public async void loadData(string search = "0")
        {
            IsLoading = true;
            rootPage.NotifyUser(NotifyType.PrepareMessage);

            Items = new ObservableCollection<ZModelItem>();

            try
            { 
                ZHelpersREST rest = new ZHelpersREST("items", "quotes", search, "best");
                JSONdata = await rest.getResponse();

                var json = JsonConvert.DeserializeObject<List<ZModelItem>>(JSONdata);
                json.ForEach(x => Items.Add(x));

                rootPage.NotifyUser( NotifyType.StatusMessage);
            }
            catch (Exception ex)
            {
                rootPage.NotifyUser(NotifyType.ErrorMessage, ex.Message);
            }

            IsLoading = false;
        }

    }
}
