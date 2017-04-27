using System.Xml.Serialization;

namespace MGCrawler
{
    [XmlRoot("LaunchBox")]
    public class LBPlatform
    {
        [XmlElement("Game")]
        public LBPlatformGame[] Games;
    }
}
