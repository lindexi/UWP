using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace BaqulukaNercerewhelbeba.Util
{
    public class MatterMost : INotifyTool
    {
        public void SendText(string url, string text)
        {
            var httpClient = new HttpClient();
            var json = JsonSerializer.Serialize(new
            {
                text = text
            });
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
            System.Console.WriteLine($"Post {url} {json}");
            httpClient.PostAsync(url, content);
        }
    }
}
