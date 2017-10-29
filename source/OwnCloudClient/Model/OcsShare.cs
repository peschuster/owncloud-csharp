using System;
using System.Xml.Serialization;

namespace OwnCloudClient.Model
{
    public class OcsShare
    {
        [XmlElement("id")]
        public int Id { get; set; }

        [XmlElement("share_type")]
        public int XmlShareType { get; set; }

        [XmlIgnore]
        public OcsShareType ShareType
        {
            get { return (OcsShareType)this.XmlShareType; }
            set { this.XmlShareType = (int)value; }
        }

        [XmlElement("uid_owner")]
        public string UidOwner { get; set; }

        [XmlElement("displayname_owner")]
        public string DisplaynameOwner { get; set; }

        [XmlElement("permissions")]
        public int XmlPermissions { get; set; }

        [XmlIgnore]
        public OcsSharePermission Permissions
        {
            get { return (OcsSharePermission)this.XmlPermissions; }
            set { this.XmlPermissions = (int)value; }
        }

        [XmlElement("stime")]
        public int STime { get; set; }

        [XmlElement("parent")]
        public string XmlParent { get; set; }

        [XmlIgnore]
        public int? Parent
        {
            get { return string.IsNullOrWhiteSpace(this.XmlParent) ? default(int?) : Convert.ToInt32(this.XmlParent); }
            set { this.XmlParent = string.Format("{0}", value); }
        }

        [XmlElement("expiration")]
        public string Expiration { get; set; }

        [XmlElement("token")]
        public string Token { get; set; }

        [XmlElement("uid_file_owner")]
        public string UidFileOwner { get; set; }

        [XmlElement("displayname_file_owner")]
        public string DisplaynameFileOwner { get; set; }

        [XmlElement("path")]
        public string Path { get; set; }

        [XmlElement("item_type")]
        public string ItemType { get; set; }

        [XmlElement("mimetype")]
        public string MimeType { get; set; }

        [XmlElement("storage_id")]
        public string StorageId { get; set; }

        [XmlElement("storage")]
        public int Storage { get; set; }

        [XmlElement("item_source")]
        public int ItemSource { get; set; }

        [XmlElement("file_source")]
        public int FileSource { get; set; }

        [XmlElement("file_parent")]
        public int FileParent { get; set; }

        [XmlElement("file_target")]
        public string FileTarget { get; set; }

        [XmlElement("share_with")]
        public string ShareWith { get; set; }

        [XmlElement("share_with_displayname")]
        public string ShareWithDisplayname { get; set; }

        [XmlElement("url")]
        public string Url { get; set; }

        [XmlElement("mail_send")]
        public int MailSend { get; set; }
    }
}
