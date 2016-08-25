using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace MGCrawler
{
    internal class Program
    {
        internal static string MG_HOST = "http://www.mobygames.com";
        internal static string MG_SITE_PATH = "MGSite";
        internal static string EXTENSION = ".html";

        private static Random _random;

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

            NUPair platformInfo = (from p in site.Platforms where p.Name == "NES" select p).First();
            Platform platformNES = new Platform(platformInfo.Name, platformInfo.URL);
            platforms.Add(platformNES);

            //if (!platformNES.IsDownloaded)
            //    platformNES.Download();

            //while (!platformNES.ArePagesDownloaded)
            //{
            //    Thread.Sleep(_random.Next(30000, 90000));
            //    platformNES.DownloadRandomPage();
            //}

            platformInfo = (from p in site.Platforms where p.Name == "SNES" select p).First();
            Platform platformSNES = new Platform(platformInfo.Name, platformInfo.URL);
            platforms.Add(platformSNES);

            //if (!platformSNES.IsDownloaded)
            //    platformSNES.Download();

            //while (!platformSNES.ArePagesDownloaded)
            //{
            //    Thread.Sleep(_random.Next(20000, 40000));
            //    platformSNES.DownloadRandomPage();
            //}

            foreach (NUPair gameInfo in platforms.First().Games)
                games.Add(new Game(gameInfo.Name, gameInfo.URL));

            foreach (NUPair gameInfo in platforms.Last().Games)
                games.Add(new Game(gameInfo.Name, gameInfo.URL));

            int count = games.Where(g => g.IsDownloaded).Count();

            while (games.Where(g => !g.IsDownloaded).Any())
            {
                Thread.Sleep(_random.Next(30000, 90000));

                List<Game> gamesToDownload = games.Where(g => !g.IsDownloaded).ToList();
                Game gameToDownload = gamesToDownload[_random.Next(gamesToDownload.Count)];
                gameToDownload.DownloadRandomPage();
            }

            //games.First().Download();

            Console.ReadKey();
        }
    }
}
