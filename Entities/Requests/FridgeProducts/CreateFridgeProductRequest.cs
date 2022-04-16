using System;

namespace Entities.Requests.FridgeProducts
{
    public class CreateFridgeProductRequest : ManipulateFridgeProductRequest
    {
        public Guid ProductId { get; set; }
    }
}
