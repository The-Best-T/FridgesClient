using System;

namespace Entities.ViewModels.FridgeProducts
{
    public class FridgeProductViewModel
    {
        public Guid ProductId { get; set; }
        public Guid FridgeId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
    }
}
