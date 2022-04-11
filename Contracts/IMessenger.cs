using Entities.Responses;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IMessenger
    {
        Task<JsonRespone> PostRequestAsync(string url, string token, string json);
        Task<JsonRespone> GetRequestAsync(string url, string token);
        Task<JsonRespone> PutRequestAsync(string url, string token, string json);
        Task<JsonRespone> DeleteRequestAsync(string url, string token);
    }
}
