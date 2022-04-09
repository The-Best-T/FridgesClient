using Contracts;
using Entities.Responses;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MessengerService
{
    public class MessengerManager : IMessenger
    {
        private readonly HttpClient client = new HttpClient();
        public async Task<JsoneRespone> DeleteRequestAsync(string url)
        {
            using HttpResponseMessage response = await client.DeleteAsync(url).ConfigureAwait(false);

            var statusCode = (int)response.StatusCode;
            var message = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            return new JsoneRespone
            {
                StatusCode = statusCode,
                Message = message
            };
        }

        public async Task<JsoneRespone> GetRequestAsync(string url)
        {
            using HttpResponseMessage response = await client.GetAsync(url).ConfigureAwait(false);

            var statusCode = (int)response.StatusCode;
            var message = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            return new JsoneRespone
            {
                StatusCode = statusCode,
                Message = message
            };
        }

        public async Task<JsoneRespone> PostRequestAsync(string url, string json)
        {
            using HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
            using HttpResponseMessage response = await client.PostAsync(url, content).ConfigureAwait(false);

            var statusCode = (int)response.StatusCode;
            var message = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            return new JsoneRespone
            {
                StatusCode = statusCode,
                Message = message
            };
        }

        public async Task<JsoneRespone> PutRequestAsync(string url, string json)
        {
            using HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
            using HttpResponseMessage response = await client.PutAsync(url, content).ConfigureAwait(false);

            var statusCode = (int)response.StatusCode;
            var message = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            return new JsoneRespone
            {
                StatusCode = statusCode,
                Message = message
            };
        }
    }
}
