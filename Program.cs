using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;

namespace MGCrawler
{
    internal class Program
    {
        internal static string MG_SITE_PATH = "MGSite";
        internal static string EXTENSION = ".json";
        internal static string API_KEY = "qHEc3ysh5xGCM9m0BtT8Zg==";

        private static void Main(string[] args)
        {
            //LBPlatform lbPlatform = (LBPlatform)new XmlSerializer(typeof(LBPlatform)).Deserialize(new FileStream(@"D:\Emulation\Emulators\LaunchBox\Data\Platforms\Nintendo Entertainment System.xml", FileMode.Open));
            //LBMetadata lbMetadata = (LBMetadata)new XmlSerializer(typeof(LBMetadata)).Deserialize(new FileStream(@"D:\Emulation\Emulators\LaunchBox\Metadata\Metadata.xml", FileMode.Open));

            Site site = new Site();
            List<Platform> platforms = new List<Platform>();
            List<Game> games = new List<Game>();

            if (!site.IsDownloaded)
                site.Download();

            //APIPlatform n64APIPlatform = site.APIPlatforms.Where(p => p.platform_name == "Nintendo 64").First();
            //platforms.Add(new Platform(n64APIPlatform));

            //if (!platforms.First().IsDownloaded)
            //    platforms.First().Download();

            //APIPlatform dreamcastAPIPlatform = site.APIPlatforms.Where(p => p.platform_name == "Dreamcast").First();
            //platforms.Add(new Platform(dreamcastAPIPlatform));

            //if (!platforms.Last().IsDownloaded)
            //    platforms.Last().Download();

            //APIPlatform nesAPIPlatform = site.APIPlatforms.Where(p => p.platform_name == "NES").First();
            //platforms.Add(new Platform(nesAPIPlatform));

            //if (!platforms.Last().IsDownloaded)
            //    platforms.Last().Download();

            APIPlatform dsAPIPlatform = site.APIPlatforms.Where(p => p.platform_name == "Nintendo DS").First();
            platforms.Add(new Platform(dsAPIPlatform));

            if (!platforms.Last().IsDownloaded)
                platforms.Last().Download();

            foreach (Game game in platforms.Last().Games.OrderBy(g => g.ReleaseDate))
            {
                Console.Write(game.Name + "\t");
                Console.Write(game.ReleaseDate + "\t");
                Console.Write(game.Developer + "\t");
                Console.Write(game.Publisher + "\t");
                Console.Write(game.MobyScore + "\t");
                Console.Write(game.Country + "\n");
            }

            //APIGame awdsAPIGame = platforms.Last().APIGames.Where(g => g.title == "Picross 3D").First();
            //games.Add(new Game(awdsAPIGame, dsAPIPlatform));

            //if (!games.Last().IsDownloaded)
            //    games.Last().Download();

            //List<string> nesGamesInMetadata = (from g in lbMetadata.Games where g.Platform == "Nintendo Entertainment System" orderby g.Name select g.Name).ToList();
            //List<string> nesGamesInJSON = (from g in platforms.Last().APIGames orderby g.title select g.title).ToList();
            //List<string> rawNESGamesInFolder = new DirectoryInfo(@"D:\Emulation\ROMs\Nintendo\Nintendo Entertainment System").GetFiles("*", SearchOption.TopDirectoryOnly).Select(f => f.Name.Substring(0, (f.Name.Length - 4))).ToList();

            //List<string> nesGamesInFolder = new List<string>();
            //foreach (string rawNESGame in rawNESGamesInFolder)
            //{
            //    string nesGame = rawNESGame;

            //    if (nesGame.Contains("(Prototype)"))
            //        nesGame = nesGame.Substring(0, nesGame.IndexOf("(Prototype)") - 1);

            //    if (nesGame.Contains("(Translation)"))
            //        nesGame = nesGame.Substring(0, nesGame.IndexOf("(Translation)") - 1);

            //    if (nesGame.Contains("(Import)"))
            //        nesGame = nesGame.Substring(0, nesGame.IndexOf("(Import)") - 1);

            //    if (nesGame.Contains("(Beta)"))
            //        nesGame = nesGame.Substring(0, nesGame.IndexOf("(Beta)") - 1);

            //    if (nesGame.Contains("(Namco)"))
            //        nesGame = nesGame.Substring(0, nesGame.IndexOf("(Namco)") - 1);

            //    if (nesGame.Contains("(Tengen)"))
            //        nesGame = nesGame.Substring(0, nesGame.IndexOf("(Tengen)") - 1);

            //    if (nesGame.Contains("(Sunsoft)"))
            //        nesGame = nesGame.Substring(0, nesGame.IndexOf("(Sunsoft)") - 1);

            //    if (nesGame.Contains("(Nintendo)"))
            //        nesGame = nesGame.Substring(0, nesGame.IndexOf("(Nintendo)") - 1);

            //    if (nesGame.Contains("(Kemco)"))
            //        nesGame = nesGame.Substring(0, nesGame.IndexOf("(Kemco)") - 1);

            //    if (nesGame.Contains("(Taito)"))
            //        nesGame = nesGame.Substring(0, nesGame.IndexOf("(Taito)") - 1);

            //    if (nesGame.Contains("(UBI Soft)"))
            //        nesGame = nesGame.Substring(0, nesGame.IndexOf("(UBI Soft)") - 1);

            //    if (nesGame.Contains(" - "))
            //        nesGame = nesGame.Replace(" - ", ": ");

            //    if (nesGame.Contains(", The"))
            //        nesGame = "The " + nesGame.Replace(", The", "");

            //    nesGamesInFolder.Add(nesGame);
            //}

            //foreach (string nesGame in nesGamesInFolder)
            //{
            //    if (nesGame.Contains('('))
            //        Console.WriteLine(nesGame);
            //}

            //List<string> duplicates = (from a in nesGamesInJSON group a by a into g where g.Count() > 1 select g.Key).ToList();

            //List<string> nesGamesInMetadataAndJSON = nesGamesInMetadata.Intersect(nesGamesInJSON).ToList();
            //List<string> nesGamesOnlyInMetadata = nesGamesInMetadata.Except(nesGamesInJSON).ToList();
            //List<string> nesGamesOnlyInJSON = nesGamesInJSON.Except(nesGamesInMetadata).ToList();

            //List<string> nesGamesOnlyInFolder = nesGamesInFolder.Except(nesGamesInMetadata).Except(nesGamesInJSON).ToList();
            //List<string> nesGamesOnlyInMetadata = nesGamesInMetadata.Except(nesGamesInFolder).ToList();
            //List<string> nesGamesOnlyInJSON = nesGamesInJSON.Except(nesGamesInFolder).ToList();

            //string a = String.Join("\t", nesGamesOnlyInFolder.ToArray());
            //string b = String.Join("\t", nesGamesOnlyInMetadata.ToArray());
            //string c = String.Join("\t", nesGamesOnlyInJSON.ToArray());

            //Console.ReadKey();

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
