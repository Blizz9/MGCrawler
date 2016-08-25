using System;
using System.Collections.Generic;
using System.IO;

namespace MGCrawler
{
    internal class Game
    {
        private const string CREDITS_PATH = "credits";
        private const string SCREENSHOTS_PATH = "screenshots";
        private const string COVER_ART_PATH = "cover-art";
        private const string RELEASE_INFO_PATH = "release-info";

        private Random _random;

        private string _path;
        private string _creditsPath;
        private string _screenshotsPath;
        private string _coverArtPath;
        private string _releaseInfoPath;
        private string _webURL;
        private string _creditsWebURL;
        private string _screenshotsWebURL;
        private string _coverArtWebURL;
        private string _releaseInfoWebURL;

        internal string Name;
        internal bool IsDownloaded;

        internal Game(string name, string url)
        {
            _random = new Random();

            Name = name;

            _path = Path.Combine(Program.MG_SITE_PATH, url.Trim('/') + Program.EXTENSION);
            _creditsPath = Path.Combine(Program.MG_SITE_PATH, url.Trim('/'), CREDITS_PATH + Program.EXTENSION);
            _screenshotsPath = Path.Combine(Program.MG_SITE_PATH, url.Trim('/'), SCREENSHOTS_PATH + Program.EXTENSION);
            _coverArtPath = Path.Combine(Program.MG_SITE_PATH, url.Trim('/'), COVER_ART_PATH + Program.EXTENSION);
            _releaseInfoPath = Path.Combine(Program.MG_SITE_PATH, url.Trim('/'), RELEASE_INFO_PATH + Program.EXTENSION);
            _webURL = Program.MG_HOST + url;
            _creditsWebURL = Program.MG_HOST + url + "/" + CREDITS_PATH;
            _screenshotsWebURL = Program.MG_HOST + url + "/" + SCREENSHOTS_PATH;
            _coverArtWebURL = Program.MG_HOST + url + "/" + COVER_ART_PATH;
            _releaseInfoWebURL = Program.MG_HOST + url + "/" + RELEASE_INFO_PATH;

            if (File.Exists(_path) && File.Exists(_creditsPath) && File.Exists(_screenshotsPath) && File.Exists(_coverArtPath) && File.Exists(_releaseInfoPath))
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
            string contents = File.ReadAllText(_path);
            //contents = contents.Replace("& ", "&amp; ");
            //contents = contents.Replace("&D", "&amp;D");
            //contents = contents.Replace("&E", "&amp;E");
            //contents = contents.Replace("&nbsp;", " ");

            //int begin = contents.IndexOf("<div id=\"coreGameRelease\">");
            //int end = contents.IndexOf("</td>", begin);

            //contents = contents.Substring(begin, (end - begin));

            //XmlDocument xml = new XmlDocument();
            //xml.LoadXml(contents);

            //string publisher = xml.FirstChild.ChildNodes[1].FirstChild.InnerText;
            //string developer = xml.FirstChild.ChildNodes[3].FirstChild.InnerText;
            //string releaseDate = xml.FirstChild.ChildNodes[5].FirstChild.InnerText;

            //foreach (XmlNode attributeNode in xml.FirstChild.ChildNodes)
            //{
            //    XmlNode contentNode = gameNode.FirstChild;
            //    Games.Add(new NUPair() { Name = contentNode.FirstChild.InnerText, URL = contentNode.Attributes[0].Value });
            //}

            Console.WriteLine(Name + ": Loaded");
        }

        internal void DownloadRandomPage()
        {
            List<Tuple<string, string, string>> pagesToDownload = new List<Tuple<string, string, string>>();
            if (!File.Exists(_path))
                pagesToDownload.Add(new Tuple<string, string, string>(string.Empty, _path, _webURL));
            if (!File.Exists(_creditsPath))
                pagesToDownload.Add(new Tuple<string, string, string>(" (credits)", _creditsPath, _creditsWebURL));
            if (!File.Exists(_screenshotsPath))
                pagesToDownload.Add(new Tuple<string, string, string>(" (screenshots)", _screenshotsPath, _screenshotsWebURL));
            if (!File.Exists(_coverArtPath))
                pagesToDownload.Add(new Tuple<string, string, string>(" (cover art)", _coverArtPath, _coverArtWebURL));
            if (!File.Exists(_releaseInfoPath))
                pagesToDownload.Add(new Tuple<string, string, string>(" (release info)", _releaseInfoPath, _releaseInfoWebURL));

            Tuple<string, string, string> pageToDownload = pagesToDownload[_random.Next(pagesToDownload.Count)];

            Console.Write(Name + pageToDownload.Item1 + ": Downloading...");

            string contents = CURLWrapper.ReadMGURL(pageToDownload.Item3);

            Directory.CreateDirectory(Path.GetDirectoryName(pageToDownload.Item2));
            File.WriteAllText(pageToDownload.Item2, contents);

            Console.WriteLine("done");

            if (File.Exists(_path) && File.Exists(_creditsPath) && File.Exists(_screenshotsPath) && File.Exists(_coverArtPath) && File.Exists(_releaseInfoPath))
            {
                IsDownloaded = true;
                load();
            }
        }
    }
}
