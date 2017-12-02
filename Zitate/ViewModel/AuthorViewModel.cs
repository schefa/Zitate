using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using Zitate.Helpers;
using Zitate.Model;

namespace Zitate.ViewModel
{

    public class ZViewModelAuthor : ZViewModel
    {
        
        public ZModelAuthor Author { get; set; }
        public ObservableCollection<ZModelItem> Items { get; set; }
        
        public async void loadData(string search = "0" )
        {
            IsLoading = true;
            rootPage.NotifyUser(NotifyType.PrepareMessage);
            Items = new ObservableCollection<ZModelItem>();

            try
            {

                ZHelpersREST rest = new ZHelpersREST("author", "quotes", search);
                JSONdata = await rest.getResponse();

                var authorItems = JsonConvert.DeserializeObject<ZModelAuthorItems>(JSONdata).Items;
                if(Author == null)
                {
                    Author = JsonConvert.DeserializeObject<ZModelAuthorItems>(JSONdata).Author;

                }

                if (authorItems != null)
                {
                   // authorItems.ForEach(x => Items.Add(x));
                    foreach (var item in authorItems)
                    {
                        item.username = Author.username;
                        item.Text = HtmlRemoval.SetLineBreaks(item.Text);
                        Items.Add(item);
                    }

                    rootPage.NotifyUser( NotifyType.StatusMessage);
                }
                else
                {
                    rootPage.NotifyUser(NotifyType.StatusMessage, "No items found.");
                }
            }
            catch (Exception ex)
            {
                rootPage.NotifyUser(NotifyType.ErrorMessage, ex.Message );
            }

            IsLoading = false;
        }
        
    }
}
