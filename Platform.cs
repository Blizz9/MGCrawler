using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace MGCrawler
{
    internal class Platform
    {
        private static string CONTENTS_FILENAME = "gamelist.html";

        private string _name;
        private string _path;
        private string _fullURL;

        internal List<NUPair> Games;

        internal Platform(string name, string url)
        {
            _name = name;
            _path = Path.Combine(Program.MG_SITE_PATH, url.TrimStart('/'), CONTENTS_FILENAME);
            _fullURL = Program.MG_HOST + url;

            if (File.Exists(_path))
                load();
            else
                Console.WriteLine(_name + ": HTML does not exist");
        }

        private void load()
        {
            Games = new List<NUPair>();

            string contents = File.ReadAllText(_path);

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

            Console.WriteLine(_name + ": Loaded");
        }

        internal void Download()
        {
            Console.Write(_name + ": Downloading...");

            string contents = CURLWrapper.ReadMGURL(_fullURL);
            File.WriteAllText(_path, contents);

            Console.WriteLine("done");

            load();
        }
    }
}
