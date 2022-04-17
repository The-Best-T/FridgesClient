using System;
using System.ComponentModel.DataAnnotations;

namespace Entities.ViewModels.FridgeProducts
{
    public abstract class ManipulateFridgeProductViewModel
    {
        [Range(0, 99, ErrorMessage = "Quantity is required and it must be in range[0,99]")]
        public int Quantity { get; set; }
        public Guid FridgeId { get; set; }
    }
}
