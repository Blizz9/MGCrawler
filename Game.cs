using System;
using System.IO;
using System.Linq;
using System.Threading;
using Newtonsoft.Json;

namespace MGCrawler
{
    internal class Game
    {
        private const string URL_PREFIX = "/v1/games";
        private const string PATH = "Games";

        private string _url;
        private string _path;

        internal bool IsDownloaded;

        internal string Name;
        internal string Country;
        internal string Developer;
        internal string Publisher;
        internal string ReleaseDate;
        internal double MobyScore;

        internal Game(APIGame apiGame, int platformID, string platformName)
        {
            Name = apiGame.title;
            MobyScore = apiGame.moby_score;

            _url = string.Format("{0}/{1}/platforms/{2}", URL_PREFIX, apiGame.game_id, platformID);
            string _gameURL = apiGame.moby_url.Substring(apiGame.moby_url.LastIndexOf('/') + 1);
            _path = Path.Combine(Program.MG_SITE_PATH, platformName, PATH, _gameURL + Program.EXTENSION);

            if (File.Exists(_path))
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
            APIGame apiGame = JsonConvert.DeserializeObject<APIGame>(File.ReadAllText(_path));

            APIRelease apiRelease = apiGame.releases.Where(r => r.countries.Contains("United States")).FirstOrDefault();
            if (apiRelease == null)
            {
                apiRelease = apiGame.releases.First();
                Country = apiRelease.countries.First();
            }
            else
            {
                Country = "United States";
            }

            Developer = (from c in apiRelease.companies where c.role == "Developed by" select c.company_name).FirstOrDefault();
            Publisher = (from c in apiRelease.companies where c.role == "Published by" select c.company_name).FirstOrDefault();

            ReleaseDate = apiRelease.release_date;

            Console.WriteLine(Name + ": Loaded");
        }

        internal void Download()
        {
            Console.Write(string.Format("{0}: Downloading...", Name));

            string contents = Program.QueryAPI(_url);
            APIGame temp = JsonConvert.DeserializeObject<APIGame>(contents);

            Directory.CreateDirectory(Path.GetDirectoryName(_path));
            File.WriteAllText((_path), contents);

            Console.WriteLine("done");

            Thread.Sleep(11000);

            IsDownloaded = true;

            load();
        }
    }
}
