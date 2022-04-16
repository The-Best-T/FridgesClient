using System;

namespace Entities.Responses.FridgeProducts
{
    public class FridgeProductResponse
    {
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
    }
}
