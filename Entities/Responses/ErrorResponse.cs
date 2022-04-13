using System.Collections.Generic;

namespace Entities.Responses
{
    public abstract class ErrorResponse
    {
        public Dictionary<string, IEnumerable<string>> Errors { get; set; }
    }
}
