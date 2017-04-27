using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Xml.Serialization;

namespace MGCrawler
{
    internal class Program
    {
        internal static string MG_SITE_PATH = "MGSite";
        internal static string EXTENSION = ".json";
        internal static string API_KEY = "qHEc3ysh5xGCM9m0BtT8Zg==";

        private static void Main(string[] args)
        {
            //MySerializableClass myObject;
            //// Construct an instance of the XmlSerializer with the type  
            //// of object that is being deserialized.  
            XmlSerializer mySerializer = new XmlSerializer(typeof(LBPlatform));
            //// To read the file, create a FileStream.  
            FileStream myFileStream = new FileStream(@"test.xml", FileMode.Open);
            //// Call the Deserialize method and cast to the object type.  
            LBPlatform myObject = (LBPlatform)mySerializer.Deserialize(myFileStream);

            Site site = new Site();
            List<Platform> platforms = new List<Platform>();
            //List<Game> games = new List<Game>();

            if (!site.IsDownloaded)
                site.Download();

            APIPlatform n64APIPlatform = site.APIPlatforms.Where(p => p.platform_name == "Nintendo 64").First();
            platforms.Add(new Platform(n64APIPlatform));

            if (!platforms.First().IsDownloaded)
                platforms.First().Download();

            //APIPlatform dreamcastAPIPlatform = site.APIPlatforms.Where(p => p.platform_name == "Dreamcast").First();
            //platforms.Add(new PlatformNew(dreamcastAPIPlatform));

            //if (!platforms.Last().IsDownloaded)
            //    platforms.Last().Download();

            Console.ReadKey();

            //APIGames temp = JsonConvert.DeserializeObject<APIGames>(QueryMobyGames("v1/games?format=brief"));
            //APIPlatforms temp = JsonConvert.DeserializeObject<APIPlatforms>(QueryAPI("/v1/platforms"));
        }

        internal static string QueryAPI(string uri)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri("https://api.mobygames.com");
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                string separator = uri.Contains("?") ? "&" : "?";
                string fullURI = string.Format("{0}{1}api_key={2}", uri, separator, API_KEY);

                HttpResponseMessage response = httpClient.GetAsync(fullURI).Result;
                if (response.IsSuccessStatusCode)
                    return (response.Content.ReadAsStringAsync().Result);
                else
                    return ("ERROR");
            }
        }
    }
}
