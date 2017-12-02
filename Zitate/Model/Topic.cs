using Newtonsoft.Json;
using System.Collections.Generic;

namespace Zitate.Model
{
    public class ZModelTopic
    {
        public int id { get; set; }
        public string name { get; set; }
        public int sum { get; set; }
        public int size { get; set; }
    }
    
    public class ZModelTopicsLandingpage
    {
        [JsonProperty("popular")]
        public List<ZModelTopic> PopularTopics { get; set; }
    }

    public class ZModelTopicItems
    {
        [JsonProperty("thema")]
        public ZModelTopic Topic { get; set; }

        [JsonProperty("items")]
        public List<ZModelItem> Items { get; set; }
    }

}
