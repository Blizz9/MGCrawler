using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Newtonsoft.Json;

namespace MGCrawler
{
    internal class Platform
    {
        private const string URL_PREFIX = "/v1/games";

        private string _url;
        private string _pathPrefix;

        internal int ID;
        internal string Name;
        internal bool IsDownloaded;
        internal List<APIGame> APIGames;
        internal List<Game> Games;

        internal Platform(APIPlatform apiPlatform)
        {
            ID = apiPlatform.platform_id;
            Name = apiPlatform.platform_name;
            _url = string.Format("{0}?platform={1}", URL_PREFIX, apiPlatform.platform_id);
            _pathPrefix = Path.Combine(Program.MG_SITE_PATH, Name, "Page ");

            if (File.Exists(_pathPrefix + "1" + Program.EXTENSION))
            {
                IsDownloaded = true;
                load();
            }
            else
            {
                Console.WriteLine(Name + ": JSON does not exist");
            }
        }

        private void load()
        {
            APIGames = new List<APIGame>();
            Games = new List<Game>();

            string directoryPath = _pathPrefix.Substring(0, _pathPrefix.LastIndexOf('\\'));
            foreach (string path in Directory.GetFiles(directoryPath))
                APIGames.AddRange(JsonConvert.DeserializeObject<APIGames>(File.ReadAllText(path)).games);

            Console.WriteLine(Name + ": Loaded");

            foreach (APIGame apiGame in APIGames)
            {
                Game game = new Game(apiGame, ID, Name);

                if (!game.IsDownloaded)
                    game.Download();

                Games.Add(game);
            }
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
                    string path = string.Format("{0}{1}{2}", _pathPrefix, (pageIndex + 1), Program.EXTENSION);
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
