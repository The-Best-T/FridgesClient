using System;
using System.ComponentModel.DataAnnotations;

namespace Entities.ViewModels.Products
{
    public class CreateProductViewModel
    {
        [Required(ErrorMessage ="Name is required.")]
        public string Name { get; set; }

        [Range(1, 99, ErrorMessage = "Default Quantity is required and it must be in range [1,99].")]
        public int DefaultQuantity { get; set; }
    }
}
