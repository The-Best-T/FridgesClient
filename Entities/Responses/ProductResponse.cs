using System;

namespace Entities.Responses
{
    public class ProductResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int DefaultQuantity { get; set; }
    }
}
