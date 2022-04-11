using Contracts;
using Entities.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
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
            var requestMessage = new HttpRequestMessage(HttpMethod.Delete, url);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            using HttpResponseMessage response = await client.SendAsync(requestMessage);

            var headers = new Dictionary<string, string>();
            foreach (var header in response.Headers)
                headers.Add(header.Key, header.Value.First());

            return new JsonRespone
            {
                StatusCode = (int)response.StatusCode,
                Message = await response.Content.ReadAsStringAsync().ConfigureAwait(false),
                Headres = headers
            };
        }

        public async Task<JsonRespone> GetRequestAsync(string url, string token, string query)
        {
            var urlBuilder = new UriBuilder(url);
            urlBuilder.Query = query;
            url = urlBuilder.ToString();
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, url);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            using HttpResponseMessage response = await client.SendAsync(requestMessage);

            var headers = new Dictionary<string, string>();
            foreach (var header in response.Headers)
                headers.Add(header.Key, header.Value.First());

            return new JsonRespone
            {
                StatusCode = (int)response.StatusCode,
                Message = await response.Content.ReadAsStringAsync().ConfigureAwait(false),
                Headres = headers
            };
        }

        public async Task<JsonRespone> PostRequestAsync(string url, string token, string json)
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, url);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            requestMessage.Content = new StringContent(json, Encoding.UTF8, "application/json");

            using HttpResponseMessage response = await client.SendAsync(requestMessage);

            var headers = new Dictionary<string, string>();
            foreach (var header in response.Headers)
                headers.Add(header.Key, header.Value.First());

            return new JsonRespone
            {
                StatusCode = (int)response.StatusCode,
                Message = await response.Content.ReadAsStringAsync().ConfigureAwait(false),
                Headres = headers
            };
        }

        public async Task<JsonRespone> PutRequestAsync(string url, string token, string json)
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Put, url);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            requestMessage.Content = new StringContent(json, Encoding.UTF8, "application/json");

            using HttpResponseMessage response = await client.SendAsync(requestMessage);

            var headers = new Dictionary<string, string>();
            foreach (var header in response.Headers)
                headers.Add(header.Key, header.Value.First());

            return new JsonRespone
            {
                StatusCode = (int)response.StatusCode,
                Message = await response.Content.ReadAsStringAsync().ConfigureAwait(false),
                Headres = headers
            };
        }
    }
}
