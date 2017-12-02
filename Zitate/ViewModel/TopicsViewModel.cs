using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Zitate.Helpers;
using Zitate.Model;

namespace Zitate.ViewModel
{

    public class ZViewModelTopics : ZViewModel  
    {

        public ZModelTopic SearchedTopic { get; set; }
        public ObservableCollection<ZModelItem> TopicItems { get; set; }
        public ObservableCollection<ZModelTopic> PopularTopics { get; set; }
         
        public async void loadData(string searchValue = "0", string view = "tags")
        {
            TopicItems = new ObservableCollection<ZModelItem>();
            PopularTopics = new ObservableCollection<ZModelTopic>();
            
            IsLoading = true;
            rootPage.NotifyUser(NotifyType.PrepareMessage);

            try
            {
                ZHelpersREST rest = new ZHelpersREST(view, "quotes", searchValue);
                JSONdata = await rest.getResponse();

                doData(JSONdata, searchValue);
                rootPage.NotifyUser( NotifyType.StatusMessage );
            }
            catch (Exception ex)
            {
                rootPage.NotifyUser(NotifyType.ErrorMessage, ex.Message);
            }

            IsLoading = false;
        }

        private void doData(string JSON, string searchValue)
        {
            int n;
            bool isNumeric = int.TryParse(searchValue, out n);

            if (searchValue == "0" || searchValue == null)
            {
                var popular = JsonConvert.DeserializeObject<ZModelTopicsLandingpage>(JSON).PopularTopics;
                foreach (var item in popular)
                {
                    PopularTopics.Add(item);
                }
            }
            else if ( searchValue != null && !isNumeric )
            { 
                var popular = JsonConvert.DeserializeObject<List<ZModelTopic>>(JSON);
                foreach (var item in popular)
                {
                    PopularTopics.Add(item);
                }

                if (PopularTopics.Count < 1)
                    PopularTopics.Add(new ZModelTopic() { name = "Keine Themen gefunden." });
            }
            else 
            {
                SearchedTopic = JsonConvert.DeserializeObject<ZModelTopicItems>(JSONdata).Topic;
                var items = JsonConvert.DeserializeObject<ZModelTopicItems>(JSONdata).Items;
                foreach (var item in items)
                {
                    TopicItems.Add(item);
                }
            } 
            
        }

    }
}
