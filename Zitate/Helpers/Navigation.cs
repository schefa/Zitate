using System;
using System.Collections.Generic;

namespace Zitate.Helpers
{
    public class ZNavigation
    {
        private static Dictionary<string, ZNavigationItem> navigationItems = new Dictionary<string, ZNavigationItem> ()
                {
                    {  "zviewhomepage" , new ZNavigationItem() { Title = "Start", Symbol = "Home", ClassType = typeof(ZViewHomepage) }},
                    {  "zviewauthors" , new ZNavigationItem() { Title = "Autoren", Symbol = "People", ClassType = typeof(ZViewAuthors) }},
                    {  "zviewquotes" , new ZNavigationItem() { Title = "Zitate", Symbol = "Comment", ClassType = typeof(ZViewQuotes) }},
                    {  "zviewtopics" ,new ZNavigationItem() { Title = "Themen", Symbol = "Tag", ClassType = typeof(ZViewTopics) }},
                    {  "zviewcollection" ,new ZNavigationItem() { Title = "Sammlung", Symbol = "OutlineStar", ClassType = typeof(ZViewCollection) }}
                };

        public static Dictionary<string, ZNavigationItem> NavigationItems
        {
            get
            {
                return navigationItems;
            }

            private set
            {
                navigationItems = value;
            }
        }


        private static List<ZNavigationItem> navigationCombobox = new List<ZNavigationItem>
            {
                new ZNavigationItem() { Title = "Zitate", ClassType = typeof(ZViewQuotes) },
                new ZNavigationItem() { Title = "Autoren", ClassType = typeof(ZViewAuthors) },
                new ZNavigationItem() { Title = "Themen", ClassType = typeof(ZViewTopics) }
            };

        public static List<ZNavigationItem> NavigationCombobox
        {
            get
            {
                return navigationCombobox;
            }

            private set
            {
                navigationCombobox = value;
            }
        }
    }

    public class ZNavigationItem
    {
        public string Title { get; set; }
        public string Symbol { get; set; }

        public Type ClassType { get; set; }

        public override string ToString()
        {
            return Title;
        }
    }

    public class ZNavigationArgs
    {
        public ZNavigationArgs() { }

        public ZNavigationArgs(object args)
        {
            searchValue = args.ToString();
        }

        private string searchValue = null;
        public string SearchValue
        {
            get
            {
                return searchValue;
            }

            set
            {
                searchValue = value;
            }
        }

        private bool refresh = false;
        public bool Refresh
        {
            get
            {
                return refresh;
            }

            set
            {
                refresh = value;
            }
        }

    } 

}
