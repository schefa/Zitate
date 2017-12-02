using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Zitate.Helpers;
using Zitate.Model;

namespace Zitate.ViewModel
{

    public class ZViewModelAuthors : ZViewModel
    {

        public ObservableCollection<ZModelAuthor> Items { get; set; }
        public ObservableCollection<ZModelAuthor> RandomAuthors { get; set; }
        public ObservableCollection<ZModelAuthor> PopularAuthors { get; set; }
        
        private void resetItems()
        {
            Items = new ObservableCollection<ZModelAuthor>();
            RandomAuthors = new ObservableCollection<ZModelAuthor>();
            PopularAuthors = new ObservableCollection<ZModelAuthor>();
        }

        public async void loadData(string search = "0" )
        {

            IsLoading = true;
            rootPage.NotifyUser(NotifyType.PrepareMessage);
            resetItems();

            try
            {
                ZHelpersREST rest = new ZHelpersREST("authors", "quotes", search);
                JSONdata = await rest.getResponse();

                if(search == "0")
                    doAuthorsLandingPage(JSONdata);
                else
                    doAuthorsSearch(JSONdata);

                rootPage.NotifyUser(NotifyType.StatusMessage );
            }
            catch (Exception ex)
            {
                rootPage.NotifyUser(NotifyType.ErrorMessage, ex.Message);
            }

            IsLoading = false;
        }

        private void doAuthorsSearch(string JSONdata)
        {
            List<ZModelAuthor> authors = JsonConvert.DeserializeObject<List<ZModelAuthor>>(JSONdata);
            foreach (var item in authors)
            {
                item.imageSrc = ZModelAuthor.getImage(item.image);
                Items.Add(item);
            }
            if (Items.Count < 1)
                Items.Add(new ZModelAuthor() { username = "Leider konnten keine Autoren gefunden werden." });
        }

        private void doAuthorsLandingPage(string JSONdata)
        {
            List<ZModelAuthor> randomAuthors = JsonConvert.DeserializeObject<ZModelAuthors>(JSONdata).Random;
            foreach (var item in randomAuthors)
            {
                item.imageSrc = ZModelAuthor.getImage(item.image);
                RandomAuthors.Add(item);
            }

            List<ZModelAuthor> popularAuthors = JsonConvert.DeserializeObject<ZModelAuthors>(JSONdata).Popular;
            foreach (var item in popularAuthors)
            {
                item.imageSrc = ZModelAuthor.getImage(item.image);
                PopularAuthors.Add(item);
            }
        }
    }
}
