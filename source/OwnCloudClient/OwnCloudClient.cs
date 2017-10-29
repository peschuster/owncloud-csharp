using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Serialization;
using OwnCloud.Model;
using WebDav;

namespace OwnCloud
{
    public class OwnCloudClient
    {
        private readonly HttpClient client;

        private readonly WebDavClient webDav;

        public OwnCloudClient(string ownCloudHost, string user, string password)
        {
            this.client = new HttpClient
            {
                BaseAddress = new Uri($"{ownCloudHost?.TrimEnd('/')}/ocs/v1.php/apps/files_sharing/api/v1/"),
            };

            var byteArray = Encoding.ASCII.GetBytes($"{user}:{password}");
            this.client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                "Basic", 
                Convert.ToBase64String(byteArray));

            var webDavParams = new WebDavClientParams
            {
                BaseAddress = new Uri($"{ownCloudHost?.TrimEnd('/')}/remote.php/webdav/"),
            };

            webDavParams.DefaultRequestHeaders.Add("Authorization", new AuthenticationHeaderValue(
                "Basic",
                Convert.ToBase64String(byteArray)).ToString());

            this.webDav = new WebDavClient(webDavParams);
        }

        public async Task<OcsShare[]> GetShares()
        {
            HttpResponseMessage response = await this.client.GetAsync("shares")
                .ConfigureAwait(false);

            response.EnsureSuccessStatusCode();
            
            return (await this.Deserialize<OcsListResponse<OcsShare>>(response.Content))?.Data;
        }

        public async Task<OcsShare> CreateShare(string path, string userGroup, OcsShareType shareType, OcsSharePermission permissions)
        {
            path = "/" + path?.TrimStart('/');

            var data = new Dictionary<string, string>
                {
                    { "path", HttpUtility.UrlEncode(path) },
                    { "shareType", ((int)shareType).ToString() },
                    { "shareWith", HttpUtility.UrlEncode(userGroup) },
                    { "permissions", ((int)permissions).ToString() }
                };

            HttpContent content = new StringContent(
                string.Join("&", data.Select(kv => kv.Key + "=" + kv.Value)),
                Encoding.UTF8,
                "application/x-www-form-urlencoded");

            HttpResponseMessage response = await client.PostAsync("shares", content)
                .ConfigureAwait(false);

            response.EnsureSuccessStatusCode();

            return (await this.Deserialize<OcsResponse<OcsShare>>(response.Content))?.Data;
        }

        public async Task<OcsDirectoryInfo> GetDirectoryInfo(string name)
        {
            PropfindResponse response = await this.webDav.Propfind(name)
                .ConfigureAwait(false);

            if (!response.IsSuccessful)
                return null;

            WebDavResource resource = response.Resources
                .OrderBy(r => r.Uri.Length)
                .FirstOrDefault();

            if (resource == null)
                return new OcsDirectoryInfo { Name = name };

            WebDavProperty usedProperty = resource.Properties.FirstOrDefault(p => p.Name.LocalName == "quota-used-bytes");
            WebDavProperty availableProperty = resource.Properties.FirstOrDefault(p => p.Name.LocalName == "quota-available-bytes");

            long used, avail;
            if (usedProperty == null || !long.TryParse(usedProperty.Value, out used))
                used = long.MinValue;

            if (availableProperty == null || !long.TryParse(availableProperty.Value, out avail))
                avail = long.MinValue;

            long? quota = null;
            if (used != long.MinValue && avail != long.MinValue)
                quota = used + avail;

            return new OcsDirectoryInfo
            {
                Name = name,
                QuotaBytes = quota,
                SizeBytes = used != long.MinValue ? used : default(long?)
            };            
        }

        public async Task<bool> EnsureDirectory(string name)
        {
            WebDavResponse response = await this.webDav.Propfind(name)
                .ConfigureAwait(false);

            if (!response.IsSuccessful 
                && response.StatusCode == (int)HttpStatusCode.NotFound)
            {
                response = await this.webDav.Mkcol(name)
                    .ConfigureAwait(false);
            }

            return response.IsSuccessful;
        }

        private async Task<TResponse> Deserialize<TResponse>(HttpContent content) where TResponse : OcsBaseResponse
        {
            var serializer = new XmlSerializer(typeof(TResponse));

            Debug.WriteLine(await content.ReadAsStringAsync());

            Stream stream = await content.ReadAsStreamAsync().ConfigureAwait(false);
            var data = serializer.Deserialize(stream) as TResponse;

            if (data?.Meta?.StatusCode != 100)
                throw new HttpRequestException("Request returned an error: " + data?.Meta?.Status + " " + data?.Meta?.Message);

            return data;
        }
    }
}
