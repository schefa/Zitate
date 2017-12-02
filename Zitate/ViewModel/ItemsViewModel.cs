using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Zitate.Helpers;
using Zitate.Model;

namespace Zitate.ViewModel
{

    public class ZViewModelItems : ZViewModel
    {
        
        public ObservableCollection<ZModelItem> Items { get; set; }
        public ObservableCollection<ZModelItem> PopularItems { get; set; }
        public ObservableCollection<ZModelItem> RandomItems { get; set; }
        
        private void resetItems()
        {
            Items = new ObservableCollection<ZModelItem>();
            PopularItems = new ObservableCollection<ZModelItem>();
            RandomItems = new ObservableCollection<ZModelItem>();
        }

        public async void loadData(string search = "0")
        {
            IsLoading = true;
            rootPage.NotifyUser(NotifyType.PrepareMessage);
            resetItems();

            try
            { 
                ZHelpersREST rest = new ZHelpersREST("items", "quotes", search, "best");
                JSONdata = await rest.getResponse();

                if (search == "0" || search == null)
                    doItemsLandingPage(JSONdata);
                else
                    doItemsSearch(JSONdata);

                rootPage.NotifyUser( NotifyType.StatusMessage);
            }
            catch (Exception ex)
            {
                rootPage.NotifyUser(NotifyType.ErrorMessage, ex.Message);
            }

            IsLoading = false;
        }

        private void doItemsSearch(string jSONdata)
        {
            var json = JsonConvert.DeserializeObject<List<ZModelItem>>(JSONdata);
            foreach (var item in json)
            {
                item.Text = HtmlRemoval.StripTagsRegex(item.Text);
                Items.Add(item);
            }

            if (Items.Count < 1)
                Items.Add(new ZModelItem() { Text = "Leider konnten keine Zitate gefunden werden." });
        }

        private void doItemsLandingPage(string jSONdata)
        {
            var random = JsonConvert.DeserializeObject<ZModelItems>(JSONdata).Random;
            if(random != null)
            {
                foreach (var item in random)
                {
                    item.Text = HtmlRemoval.StripTagsRegex(item.Text);
                    RandomItems.Add(item);
                }
            }
            
            var popular = JsonConvert.DeserializeObject<ZModelItems>(JSONdata).Popular;
            if(popular != null)
            {
                foreach (var item in popular)
                {
                    item.Text = HtmlRemoval.StripTagsRegex(item.Text);
                    PopularItems.Add(item);
                }
            }

        }
    }
}
