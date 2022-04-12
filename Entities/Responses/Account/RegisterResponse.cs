using System.Collections.Generic;

namespace Entities.Responses.Account
{
    public class RegisterResponse
    {
        public Dictionary<string, IEnumerable<string>> Errors { get; set; }
    }
}
