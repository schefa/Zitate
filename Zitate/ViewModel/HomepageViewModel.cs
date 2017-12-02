using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using Zitate.Helpers;
using Zitate.Model;

namespace Zitate.ViewModel
{

    public class ZViewModelHomepage : ZViewModel  
    {

        public ObservableCollection<ZModelItem> RandomQuotes { get; set; }
        public ObservableCollection<ZModelAuthor> Birthdays { get; set; }
        public ObservableCollection<ZModelAuthor> Deathdays { get; set; }
        public ObservableCollection<ZModelAuthor> PopularAuthors { get; set; }
        
        private void resetItems()
        {
            RandomQuotes = new ObservableCollection<ZModelItem>();
            Birthdays = new ObservableCollection<ZModelAuthor>();
            Deathdays = new ObservableCollection<ZModelAuthor>();
            PopularAuthors = new ObservableCollection<ZModelAuthor>();
        }

        public async void loadData(string searchValue = "0")
        {
            resetItems();
            IsLoading = true;
            rootPage.NotifyUser(NotifyType.PrepareMessage);

            try
            {
                ZHelpersREST rest = new ZHelpersREST("homepage", "quotes", "0");
                JSONdata = await rest.getResponse();

                doData(JSONdata);
                rootPage.NotifyUser(NotifyType.StatusMessage);
            }
            catch (Exception ex)
            {
                rootPage.NotifyUser(NotifyType.ErrorMessage, ex.Message);
            }

            IsLoading = false;
        }

        private void doData(string JSON)
        { 
            var random = JsonConvert.DeserializeObject<ZModelHomepage>(JSON).RandomQuotes;
            if(random != null) { 
                foreach (var item in random)
                { 
                    item.Text = HtmlRemoval.StripTagsRegex(item.Text);
                    RandomQuotes.Add(item);
                    break;
                }
            }

            var birthers = JsonConvert.DeserializeObject<ZModelHomepage>(JSON).Births;
            if (birthers != null)
            {
                foreach (var item in birthers)
                {
                item.imageSrc = ZModelAuthor.getImage(item.image);
                Birthdays.Add(item);
                }
            }

            var diers = JsonConvert.DeserializeObject<ZModelHomepage>(JSON).Deaths;
            if (diers != null)
            {
                foreach (var item in diers)
                {
                    item.imageSrc = ZModelAuthor.getImage(item.image);
                    Deathdays.Add(item);
                }
            }

            var authors = JsonConvert.DeserializeObject<ZModelHomepage>(JSON).PopularAuthors;
            if (authors != null)
            {
                foreach (var item in authors)
                {
                item.imageSrc = ZModelAuthor.getImage(item.image);
                PopularAuthors.Add(item);
                }
            }
        }

    }
}
