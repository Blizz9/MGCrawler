using System;
using System.Linq;

namespace MGCrawler
{
    internal class Program
    {
        internal static string MG_HOST = "http://www.mobygames.com";
        internal static string MG_SITE_PATH = "MGSite";

        private static void Main(string[] args)
        {
            Platforms platforms = new Platforms();

            if (platforms.PlatformList == null)
                platforms.Download();

            //Console.WriteLine((from p in platforms.List where p.Name == "Virtual Boy" select p.URL).First());
            //platforms.List.Where(p => p.Name == "Virtual Boy").First().sel

            NUPair platformInfo = (from p in platforms.PlatformList where p.Name == "Virtual Boy" select p).First();
            Platform platform = new Platform(platformInfo.Name, platformInfo.URL);

            if (platform.Games == null)
                platform.Download();

            Console.ReadKey();
        }
    }
}
