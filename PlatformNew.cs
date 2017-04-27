using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Newtonsoft.Json;

namespace MGCrawler
{
    internal class PlatformNew
    {
        private const string URL_PREFIX = "/v1/games";

        private string _url;
        private string _pathPrefix;

        internal string Name;
        internal bool IsDownloaded;
        internal List<APIGame> Games;

        internal PlatformNew(APIPlatform apiPlatform)
        {
            Name = apiPlatform.platform_name;
            _url = string.Format("{0}?platform={1}", URL_PREFIX, apiPlatform.platform_id);
            _pathPrefix = Path.Combine(Program.MG_SITE_PATH, _url.Replace('?', '&').Trim('/'));

            if (File.Exists(_pathPrefix + "&offset=0" + Program.EXTENSION_NEW))
            {
                IsDownloaded = true;
                load();
            }
            else
            {
                Console.WriteLine(Name + ": HTML does not exist");
            }
        }

        private void load()
        {
            Games = new List<APIGame>();

            string directoryPath = _pathPrefix.Substring(0, _pathPrefix.LastIndexOf('/'));
            string searchPattern = "*" + _pathPrefix.Substring(_pathPrefix.IndexOf('&')) + "*";
            foreach (string path in Directory.GetFiles(directoryPath, searchPattern))
                Games.AddRange(JsonConvert.DeserializeObject<APIGames>(File.ReadAllText(path)).games);

            Console.WriteLine(Name + ": Loaded");
        }

        internal void Download()
        {
            int pageIndex = 0;
            int gameCount = 0;

            do
            {
                Console.Write(string.Format("{0} (page {1}): Downloading...", Name, (pageIndex + 1)));

                string offsetSuffix = "&offset=" + (pageIndex * 100);
                string contents = Program.QueryAPI(_url + offsetSuffix);
                gameCount = JsonConvert.DeserializeObject<APIGames>(contents).games.Length;

                if (gameCount > 0)
                {
                    string path = _pathPrefix + offsetSuffix + Program.EXTENSION_NEW;
                    Directory.CreateDirectory(Path.GetDirectoryName(path));
                    File.WriteAllText((path), contents);

                    pageIndex++;
                }

                Console.WriteLine("done");

                Thread.Sleep(1250);
            }
            while (gameCount == 100);

            IsDownloaded = true;

            load();
        }
    }
}
