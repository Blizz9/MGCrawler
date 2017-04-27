using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace MGCrawler
{
    internal class SiteNew
    {
        private const string URL = "/v1/platforms";

        private string _path;

        internal bool IsDownloaded;
        internal List<APIPlatform> Platforms;

        internal SiteNew()
        {
            _path = Path.Combine(Program.MG_SITE_PATH, URL.Trim('/') + Program.EXTENSION_NEW);

            if (File.Exists(_path))
            {
                IsDownloaded = true;
                load();
            }
            else
            {
                Console.WriteLine("Platforms: HTML does not exist");
            }
        }

        private void load()
        {
            Platforms = new List<APIPlatform>();
            Platforms.AddRange(JsonConvert.DeserializeObject<APIPlatforms>(File.ReadAllText(_path)).platforms);

            Console.WriteLine("Platforms: Loaded");
        }

        internal void Download()
        {
            Console.Write("Platforms: Downloading...");

            string contents = Program.QueryAPI(URL);

            Directory.CreateDirectory(Path.GetDirectoryName(_path));
            File.WriteAllText(_path, contents);

            Console.WriteLine("done");

            IsDownloaded = true;

            load();
        }
    }
}
