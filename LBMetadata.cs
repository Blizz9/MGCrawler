using System.Xml.Serialization;

namespace MGCrawler
{
    [XmlRoot("LaunchBox")]
    public class LBMetadata
    {
        [XmlElement("Game")]
        public LBMetadataGame[] Games;
    }
}
