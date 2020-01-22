using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace BaqulukaNercerewhelbeba.Util
{
    public class MatterMost
    {
        /// <inheritdoc />
        public MatterMost(string url)
        {
            Url = url;
        }

        public string Url { get; }

        public void SendText(string text)
        {
            var httpClient = new HttpClient();
            var json = JsonSerializer.Serialize(new
            {
                text = text
            });
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
            httpClient.PostAsync(Url, content);
        }
    }
}