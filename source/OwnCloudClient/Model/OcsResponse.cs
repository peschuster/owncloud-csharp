using System.Xml.Serialization;

namespace OwnCloudClient.Model
{
    [XmlRoot("ocs")]
    public class OcsResponse<TElement> : OcsBaseResponse
    {
        [XmlElement("data")]
        public TElement Data { get; set; }
    }
}
