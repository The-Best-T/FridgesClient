using Contracts;
using Entities.Responses;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace MessengerService
{
    public class MessengerManager : IMessenger
    {
        private readonly HttpClient client = new HttpClient();
        public async Task<JsonRespone> DeleteRequestAsync(string url, string token)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",token);

            using HttpResponseMessage response = await client.DeleteAsync(url).ConfigureAwait(false);
            var statusCode = (int)response.StatusCode;
            var message = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            return new JsonRespone
            {
                StatusCode = statusCode,
                Message = message
            };
        }

        public async Task<JsonRespone> GetRequestAsync(string url, string token)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            using HttpResponseMessage response = await client.GetAsync(url).ConfigureAwait(false);

            var statusCode = (int)response.StatusCode;
            var message = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            return new JsonRespone
            {
                StatusCode = statusCode,
                Message = message
            };
        }

        public async Task<JsonRespone> PostRequestAsync(string url, string token, string json)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            using HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
            using HttpResponseMessage response = await client.PostAsync(url, content).ConfigureAwait(false);

            var statusCode = (int)response.StatusCode;
            var message = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            return new JsonRespone
            {
                StatusCode = statusCode,
                Message = message
            };
        }

        public async Task<JsonRespone> PutRequestAsync(string url, string token, string json)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            using HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
            using HttpResponseMessage response = await client.PutAsync(url, content).ConfigureAwait(false);

            var statusCode = (int)response.StatusCode;
            var message = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            return new JsonRespone
            {
                StatusCode = statusCode,
                Message = message
            };
        }
    }
}
