using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace MGCrawler
{
    internal class Site
    {
        private const string URL = "/v1/platforms";
        private const string PATH = "Platforms";

        private string _path;

        internal bool IsDownloaded;
        internal List<APIPlatform> APIPlatforms;

        internal Site()
        {
            _path = Path.Combine(Program.MG_SITE_PATH, PATH + Program.EXTENSION);

            if (File.Exists(_path))
            {
                IsDownloaded = true;
                load();
            }
            else
            {
                Console.WriteLine("API Platforms: JSON does not exist");
            }
        }

        private void load()
        {
            APIPlatforms = new List<APIPlatform>();
            APIPlatforms.AddRange(JsonConvert.DeserializeObject<APIPlatforms>(File.ReadAllText(_path)).platforms);

            Console.WriteLine("API Platforms: Loaded");
        }

        internal void Download()
        {
            Console.Write("API Platforms: Downloading...");

            string contents = Program.QueryAPI(URL);

            Directory.CreateDirectory(Path.GetDirectoryName(_path));
            File.WriteAllText(_path, contents);

            Console.WriteLine("done");

            IsDownloaded = true;

            load();
        }
    }
}
