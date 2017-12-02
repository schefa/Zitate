using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Windows.UI.Xaml.Media.Imaging;

namespace Zitate.Model
{
    public class ZModelAuthor 
    {
        public int id { get; set; }
        public string created_by { get; set; }
        public string bday { get; set; }
        public string dday { get; set; }
        public string username { get; set; }
        public string fullname { get; set; }
        public string nachname { get; set; }
        public string vorname { get; set; }
        public string profession { get; set; }
        public string image { get; set; }
        public BitmapImage imageSrc { get; set; }

        public int item_hits { get; set; }
        public int anzahl { get; set; }
        public string nationname { get; set; }

        public static BitmapImage getImage(string src)
        { 
            if (src == null || src.Length <= 1)
                return new BitmapImage(new Uri("http://www.denkschatz.de/images/autoren/user.png"));
            else
                return new BitmapImage(new Uri("http://www.denkschatz.de/images/autoren/original/" + src));
        }

    }

    public class ZModelAuthors 
    {
        [JsonProperty("popular")]
        public List<ZModelAuthor> Popular { get; set; }

        [JsonProperty("random")]
        public List<ZModelAuthor> Random { get; set; }
    }


    public class ZModelAuthorItems 
    {
        [JsonProperty("author")]
        public ZModelAuthor Author { get; set; }

        [JsonProperty("items")]
        public List<ZModelItem> Items { get; set; }
    }

}
