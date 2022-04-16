using System;

namespace Entities.Models
{
    public class FridgeProduct
    {
        public Guid ProductId { get; set; }
        public Guid FridgeId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
    }
}
