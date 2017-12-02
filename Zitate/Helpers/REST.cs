using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Zitate.Helpers
{
    public class ZHelpersREST
    {

        public static string myEmail = "ich@vonfio.de";
        public string url = "";

        public ZHelpersREST(string view, string category, string searchValue = "0", string ordering = "best")
        {
            url = "http://localhost/denkschatz/index.php/api/" + view + "/" + category + "/" + HtmlRemoval.StripTagsRegex( searchValue ) + "?c=" + myEmail + "&o=" + ordering;
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
