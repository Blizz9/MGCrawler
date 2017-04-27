using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;

namespace MGCrawler
{
    internal class Program
    {
        internal static string MG_HOST = "http://www.mobygames.com";
        internal static string MG_SITE_PATH = "MGSite";
        internal static string EXTENSION = ".html";

        internal static string EXTENSION_NEW = ".json";
        internal static string API_KEY = "qHEc3ysh5xGCM9m0BtT8Zg==";

        //private static Random _random;

        /*
        private static void Main(string[] args)
        {
            _random = new Random();

            Site site = new Site();
            List<Platform> platforms = new List<Platform>();
            List<Game> games = new List<Game>();

            if (!site.IsDownloaded)
                site.Download();

            //NUPair platformInfo = (from p in site.Platforms where p.Name == "Virtual Boy" select p).First();
            //platforms.Add(new Platform(platformInfo.Name, platformInfo.URL));

            //if (!platform.IsDownloaded)
            //    platform.Download();

            //NUPair gameInfo = (from g in platform.Games where g.Name == "Mario's Tennis" select g).First();
            //Game game = new Game(gameInfo.Name, gameInfo.URL);

            //if (!game.IsDownloaded)
            //    game.Download();

            //platformInfo = (from p in site.Platforms where p.Name == "Nintendo 64" select p).First();
            //Platform platform2 = new Platform(platformInfo.Name, platformInfo.URL);

            //if (!platform2.IsDownloaded)
            //    platform2.Download();

            //while (!platform2.ArePagesDownloaded)
            //{
            //    Thread.Sleep(_random.Next(30000, 90000));
            //    platform2.DownloadRandomPage();
            //}

            //NUPair platformInfo = (from p in site.Platforms where p.Name == "NES" select p).First();
            //Platform platformNES = new Platform(platformInfo.Name, platformInfo.URL);
            //platforms.Add(platformNES);

            //if (!platformNES.IsDownloaded)
            //    platformNES.Download();

            //while (!platformNES.ArePagesDownloaded)
            //{
            //    Thread.Sleep(_random.Next(30000, 90000));
            //    platformNES.DownloadRandomPage();
            //}

            //platformInfo = (from p in site.Platforms where p.Name == "SNES" select p).First();
            //Platform platformSNES = new Platform(platformInfo.Name, platformInfo.URL);
            //platforms.Add(platformSNES);

            //if (!platformSNES.IsDownloaded)
            //    platformSNES.Download();

            //while (!platformSNES.ArePagesDownloaded)
            //{
            //    Thread.Sleep(_random.Next(20000, 40000));
            //    platformSNES.DownloadRandomPage();
            //}

            //NUPair platformInfo = (from p in site.Platforms where p.Name == "DOS" select p).First();
            //Platform platformDOS = new Platform(platformInfo.Name, platformInfo.URL);
            //platforms.Add(platformDOS);

            //if (!platformDOS.IsDownloaded)
            //    platformDOS.Download();

            //while (!platformDOS.ArePagesDownloaded)
            //{
            //    Thread.Sleep(_random.Next(20000, 40000));
            //    platformDOS.DownloadRandomPage();
            //}

            //NUPair platformInfo = (from p in site.Platforms where p.Name == "Windows 3.x" select p).First();
            //Platform platformWindows3X = new Platform(platformInfo.Name, platformInfo.URL);
            //platforms.Add(platformWindows3X);

            //if (!platformWindows3X.IsDownloaded)
            //    platformWindows3X.Download();

            //while (!platformWindows3X.ArePagesDownloaded)
            //{
            //    Thread.Sleep(_random.Next(20000, 40000));
            //    platformWindows3X.DownloadRandomPage();
            //}

            NUPair platformInfo = (from p in site.Platforms where p.Name == "Windows" select p).First();
            Platform platformWindows = new Platform(platformInfo.Name, platformInfo.URL);
            platforms.Add(platformWindows);

            if (!platformWindows.IsDownloaded)
                platformWindows.Download();

            while (!platformWindows.ArePagesDownloaded)
            {
                Thread.Sleep(_random.Next(20000, 40000));
                platformWindows.DownloadRandomPage();
            }

            foreach (var gameExtInfo in platforms.First().GamesEXT)
                Console.WriteLine(gameExtInfo.Item1 + "\t" + gameExtInfo.Item2 + "\t" + gameExtInfo.Item3);

            //foreach (NUPair gameInfo in platforms.First().Games)
            //    games.Add(new Game(gameInfo.Name, gameInfo.URL));

            //foreach (NUPair gameInfo in platforms.Last().Games)
            //    games.Add(new Game(gameInfo.Name, gameInfo.URL));

            int count = games.Where(g => g.IsDownloaded).Count();

            //while (games.Where(g => !g.IsDownloaded).Any())
            //{
            //    Thread.Sleep(_random.Next(30000, 90000));

            //    List<Game> gamesToDownload = games.Where(g => !g.IsDownloaded).ToList();
            //    Game gameToDownload = gamesToDownload[_random.Next(gamesToDownload.Count)];
            //    gameToDownload.DownloadRandomPage();
            //}

            //games.First().Download();

            //Console.ReadKey();
        }
        */

        private static void Main(string[] args)
        {
            SiteNew site = new SiteNew();
            List<PlatformNew> platforms = new List<PlatformNew>();
            //List<Game> games = new List<Game>();

            if (!site.IsDownloaded)
                site.Download();

            APIPlatform n64APIPlatform = site.Platforms.Where(p => p.platform_name == "Nintendo 64").First();
            platforms.Add(new PlatformNew(n64APIPlatform));

            if (!platforms.First().IsDownloaded)
                platforms.First().Download();

            APIPlatform dreamcastAPIPlatform = site.Platforms.Where(p => p.platform_name == "Dreamcast").First();
            platforms.Add(new PlatformNew(dreamcastAPIPlatform));

            if (!platforms.Last().IsDownloaded)
                platforms.Last().Download();

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
