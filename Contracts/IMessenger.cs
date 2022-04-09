using Entities.Responses;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IMessenger
    {
        Task<JsoneRespone> PostRequestAsync(string url, string json);
        Task<JsoneRespone> GetRequestAsync(string url);
        Task<JsoneRespone> PutRequestAsync(string url, string json);
        Task<JsoneRespone> DeleteRequestAsync(string url);
    }
}
