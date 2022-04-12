using System.Collections.Generic;
namespace Entities.Responses.Products
{
    public class CreateProductResponse
    {
        public Dictionary<string, IEnumerable<string>> Errors { get; set; }
    }
}
