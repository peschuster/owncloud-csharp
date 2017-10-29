using System.Xml.Serialization;

namespace OwnCloudClient.Model
{
    [XmlRoot("ocs")]
    public class OcsListResponse<TElement> : OcsBaseResponse
    {
        [XmlArray("data")]
        [XmlArrayItem("element")]
        public TElement[] Data { get; set; }
    }
}
