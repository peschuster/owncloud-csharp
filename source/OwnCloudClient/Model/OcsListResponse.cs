using System.Xml.Serialization;

namespace OwnCloud.Model
{
    [XmlRoot("ocs")]
    public class OcsListResponse<TElement> : OcsBaseResponse
    {
        [XmlArray("data")]
        [XmlArrayItem("element")]
        public TElement[] Data { get; set; }
    }
}
