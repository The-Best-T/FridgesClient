using System.Collections.Generic;

namespace Entities.Responses
{
    public class JsonRespone
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public Dictionary<string, string> Headres { get; set; }
    }
}
