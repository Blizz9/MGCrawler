using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace MGCrawler
{
    internal class Site
    {
        private const string URL = "/browse/games/full,1/";

        private string _path;
        private string _webURL;

        internal bool IsDownloaded;
        internal List<NUPair> Platforms;

        internal Site()
        {
            _path = Path.Combine(Program.MG_SITE_PATH, URL.Trim('/') + Program.EXTENSION);
            _webURL = Program.MG_HOST + URL;

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
            Platforms = new List<NUPair>();

            string contents = File.ReadAllText(_path);
            contents = contents.Replace("& ", "&amp; ");

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
                Platforms.Add(new NUPair() { Name = contentNode.InnerText, URL = contentNode.Attributes[0].Value });
            }

            Console.WriteLine("Platforms: Loaded");
        }

        internal void Download()
        {
            Console.Write("Platforms: Downloading...");

            string contents = CURLWrapper.ReadMGURL(_webURL);

            Directory.CreateDirectory(Path.GetDirectoryName(_path));
            File.WriteAllText(_path, contents);

            Console.WriteLine("done");

            IsDownloaded = true;

            load();
        }
    }
}
