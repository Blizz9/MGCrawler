using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace MGCrawler
{
    internal class Platforms
    {
        private const string URL = "/browse/games/full,1/";
        private static string CONTENTS_FILENAME = "platformlist.html";

        private string _path;
        private string _fullURL;

        internal List<NUPair> PlatformList;

        internal Platforms()
        {
            _path = Path.Combine(Program.MG_SITE_PATH, URL.TrimStart('/'), CONTENTS_FILENAME);
            _fullURL = Program.MG_HOST + URL;

            if (File.Exists(_path))
                load();
            else
                Console.WriteLine("Platforms: HTML does not exist");
        }

        private void load()
        {
            PlatformList = new List<NUPair>();

            string contents = File.ReadAllText(_path);

            int platformsBegin = contents.IndexOf("<h4>Platform</h4>");
            int platformsEnd = contents.IndexOf("</div></div></div></td>", platformsBegin);
            platformsBegin = contents.IndexOf("<div><div><a href=", platformsBegin);

            contents = contents.Substring(platformsBegin, (platformsEnd - platformsBegin));
            contents = "<platforms>" + contents + "</platforms>";

            XmlDocument xml = new XmlDocument();
            xml.LoadXml(contents);

            foreach (XmlNode platformNode in xml.FirstChild.ChildNodes)
            {
                XmlNode contentNode = platformNode.FirstChild.FirstChild;
                PlatformList.Add(new NUPair() { Name = contentNode.InnerText, URL = contentNode.Attributes[0].Value });
            }

            Console.WriteLine("Platforms: Loaded");
        }

        internal void Download()
        {
            Console.Write("Platforms: Downloading...");

            string contents = CURLWrapper.ReadMGURL(_fullURL);
            File.WriteAllText(_path, contents);

            Console.WriteLine("done");

            load();
        }
    }
}
