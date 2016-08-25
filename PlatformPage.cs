using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace MGCrawler
{
    internal class PlatformPage
    {
        private string _name;
        private string _path;
        private string _webURL;

        internal bool IsDownloaded;
        internal List<NUPair> Games;

        internal PlatformPage(string name, string url)
        {
            _name = name;
            _path = Path.Combine(Program.MG_SITE_PATH, url.Trim('/') + Program.EXTENSION);
            _webURL = Program.MG_HOST + url;

            if (File.Exists(_path))
            {
                IsDownloaded = true;
                load();
            }
            else
            {
                Console.WriteLine(_name + ": HTML does not exist");
            }
        }

        private void load()
        {
            Games = new List<NUPair>();

            string contents = File.ReadAllText(_path);
            contents = contents.Replace("& ", "&amp; ");
            contents = contents.Replace("&E", "&amp;E");
            contents = contents.Replace("&M", "&amp;M");

            int platformsBegin = contents.IndexOf("<tbody>");
            int platformsEnd = contents.IndexOf("</tbody>", platformsBegin);

            contents = contents.Substring(platformsBegin, (platformsEnd - platformsBegin + 8));

            XmlDocument xml = new XmlDocument();
            xml.LoadXml(contents);

            foreach (XmlNode gameNode in xml.FirstChild.ChildNodes)
            {
                XmlNode contentNode = gameNode.FirstChild.FirstChild;
                Games.Add(new NUPair() { Name = contentNode.InnerText, URL = contentNode.Attributes[0].Value });
            }

            Console.WriteLine(_name + ": Loaded");
        }

        internal void Download()
        {
            Console.Write(_name + ": Downloading...");

            string contents = CURLWrapper.ReadMGURL(_webURL);

            Directory.CreateDirectory(Path.GetDirectoryName(_path));
            File.WriteAllText(_path, contents);

            Console.WriteLine("done");

            IsDownloaded = true;

            load();
        }
    }
}
