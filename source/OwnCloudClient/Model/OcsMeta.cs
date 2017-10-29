using System.Xml.Serialization;

namespace OwnCloud.Model
{
    public class OcsMeta
    {
        [XmlElement("status")]
        public string Status { get; set; }

        [XmlElement("statuscode")]
        public int StatusCode { get; set; }

        [XmlElement("message")]
        public string Message { get; set; }
    }
}
