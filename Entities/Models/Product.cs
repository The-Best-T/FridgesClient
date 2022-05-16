using System;

namespace Entities.Models
{
    public class Product
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int DefaultQuantity { get; set; }
    }
}
