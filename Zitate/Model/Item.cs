﻿using Newtonsoft.Json;
using System.Collections.Generic;
using Zitate.Helpers;

namespace Zitate.Model
{
    public class ZModelItem 
    {
        public int id { get; set; }
        public string title { get; set; }

        private string text;
        public string Text {
            get {
                string str = HtmlRemoval.StripTagsRegex(text);
                return str;
            }
            set { text = value; }
        }

        public string textoriginal { get; set; }
        public string nachweis { get; set; }
        public double rating { get; set; }
        public double width { get; set; }

        public int created_by { get; set; }
        public string username { get; set; }
    }

    public class ZModelItems
    {
        [JsonProperty("best")]
        public List<ZModelItem> Popular { get; set; }

        [JsonProperty("random")]
        public List<ZModelItem> Random { get; set; }
    }
}
