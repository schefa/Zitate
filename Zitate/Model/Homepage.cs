using Newtonsoft.Json;
using System.Collections.Generic;

namespace Zitate.Model
{
    public class ZModelHomepage 
    {
        [JsonProperty("randomQuote")]
        public List<ZModelItem> RandomQuotes { get; set; }

        [JsonProperty("birthDay")]
        public List<ZModelAuthor> Births { get; set; }

        [JsonProperty("deathDay")]
        public List<ZModelAuthor> Deaths { get; set; }

        [JsonProperty("popularAuthors")]
        public List<ZModelAuthor> PopularAuthors { get; set; }
        
        [JsonProperty("gedenktage")]
        public List<ZModelItem> Gedenktage { get; set; }
    }
    
}
