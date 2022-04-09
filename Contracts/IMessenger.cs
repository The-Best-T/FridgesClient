using Entities.Responses;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IMessenger
    {
        Task<JsonRespone> PostRequestAsync(string url, string json);
        Task<JsonRespone> GetRequestAsync(string url);
        Task<JsonRespone> PutRequestAsync(string url, string json);
        Task<JsonRespone> DeleteRequestAsync(string url);
    }
}
