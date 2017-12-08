using System;
using System.Net.Http;
using System.Threading.Tasks;
using Windows.Storage;

namespace Zitate.Helpers
{
    public class ZHelpersREST
    {

        private ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;

        public static string myEmail;
        public string url = "";

        public ZHelpersREST(string view, string category, string searchValue = "0", string ordering = "best")
        {
            if (localSettings.Values["email"] != null)
                myEmail = localSettings.Values["email"].ToString();
            
            url = "http://www.denkschatz.de/api/" + view + "/" + category + "/" + HtmlRemoval.StripTagsRegex( searchValue ) + "?c=" + myEmail + "&o=" + ordering;
        }

        public async Task<string> getResponse()
        {
            HttpClient client = new HttpClient();
           
            Task<string> json = client.GetStringAsync(new Uri(url));

            var result = await json;

            client.Dispose();
            return result;

            //  throw new Exception( "Verbindung zum Server fehlgeschlagen" );
           
        }

    }
}
