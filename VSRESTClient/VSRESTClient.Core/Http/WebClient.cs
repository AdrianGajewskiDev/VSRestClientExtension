using System.Net.Http;
using System.Threading.Tasks;

namespace VSRESTClient.Core.Http
{
    public class WebClient
    {
        private HttpClient _httpClient;

        public async Task SendRequest(string url)
        {
            using (_httpClient = new HttpClient())
            {
                var result = await _httpClient.GetAsync(url);
                var content = await result.Content.ReadAsStringAsync();
            }

        }
    }
}
