using System.Xml.Serialization;

namespace OwnCloud.Model
{
    [XmlRoot("ocs")]
    public abstract class OcsBaseResponse
    {
        [XmlElement("meta")]
        public OcsMeta Meta { get; set; }
    }
}
