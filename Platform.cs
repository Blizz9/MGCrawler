using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;

namespace MGCrawler
{
    internal class Platform
    {
        private Random _random;

        private string _url;
        private string _path;
        private string _webURL;
        private List<PlatformPage> _platformPages;

        internal string Name;
        internal bool IsMultiPaged;
        internal bool IsDownloaded;
        internal List<NUPair> Games;
        internal List<Tuple<string, string, string>> GamesEXT; // this is temporary

        internal Platform(string name, string url)
        {
            _random = new Random();

            Name = name;
            _url = url;
            _path = Path.Combine(Program.MG_SITE_PATH, url.Trim('/') + Program.EXTENSION);
            _webURL = Program.MG_HOST + url;

            if (File.Exists(_path))
            {
                IsDownloaded = true;
                load();
            }
            else
            {
                Console.WriteLine(Name + ": HTML does not exist");
            }
        }

        internal bool ArePagesDownloaded
        {
            get { return (!_platformPages.Any(pp => !pp.IsDownloaded)); }
        }

        private void load()
        {
            Games = new List<NUPair>();
            GamesEXT = new List<Tuple<string, string, string>>(); // this is temporary

            string contents = File.ReadAllText(_path);
            contents = contents.Replace("& ", "&amp; ");

            if (contents.Contains("The list below has been reduced"))
            {
                IsMultiPaged = true;
                _platformPages = new List<PlatformPage>();

                int gameCountBegin = contents.IndexOf("</sup>");
                gameCountBegin = contents.IndexOf('(', gameCountBegin);
                int gameCountEnd = contents.IndexOf(" games", gameCountBegin);

                int gameCount = Convert.ToInt32(contents.Substring((gameCountBegin + 1), (gameCountEnd - gameCountBegin - 1)));
                int pageCount = (int)Math.Ceiling((double)gameCount / 25);

                for (int pageNumber = 0; pageNumber < pageCount; pageNumber++)
                {
                    string pageName = Name + " (page " + (pageNumber + 1) + ")";
                    string pageURL = _url + "offset," + (pageNumber * 25) + "/so,0a/list-games/";
                    _platformPages.Add(new PlatformPage(pageName, pageURL));
                }

                if (!_platformPages.Any(pp => !pp.IsDownloaded))
                {
                    foreach (PlatformPage platformPage in _platformPages)
                    {
                        foreach (NUPair game in platformPage.Games)
                            Games.Add(game);

                        foreach (var gameExt in platformPage.GamesEXT) // this is temporary
                            GamesEXT.Add(gameExt); // this is temporary
                    }

                    Console.WriteLine(Name + ": Loaded");
                }
            }
            else
            {
                int platformsBegin = contents.IndexOf("<h4>Game List</h4>");
                platformsBegin = contents.IndexOf("<ul>", platformsBegin);
                int platformsEnd = contents.IndexOf("</ul>", platformsBegin);

                contents = contents.Substring(platformsBegin, (platformsEnd - platformsBegin + 5));

                XmlDocument xml = new XmlDocument();
                xml.LoadXml(contents);

                foreach (XmlNode gameNode in xml.FirstChild.ChildNodes)
                {
                    XmlNode contentNode = gameNode.FirstChild;
                    Games.Add(new NUPair() { Name = contentNode.FirstChild.InnerText, URL = contentNode.Attributes[0].Value });
                }

                Console.WriteLine(Name + ": Loaded");
            }
        }

        internal void Download()
        {
            Console.Write(Name + ": Downloading...");

            string contents = CURLWrapper.ReadMGURL(_webURL);

            Directory.CreateDirectory(Path.GetDirectoryName(_path));
            File.WriteAllText(_path, contents);

            Console.WriteLine("done");

            IsDownloaded = true;

            load();
        }

        internal void DownloadRandomPage()
        {
            List<PlatformPage> pagesToDownload = _platformPages.Where(pp => !pp.IsDownloaded).ToList();
            PlatformPage pageToDownload = pagesToDownload[_random.Next(pagesToDownload.Count)];
            pageToDownload.Download();

            if (pagesToDownload.Count == 1)
                load();
        }
    }
}
