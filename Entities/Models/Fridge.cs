using System;

namespace Entities.Models
{
    public class Fridge
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string OwnerName { get; set; }
        public FridgeModel Model { get; set; }
    }
}
