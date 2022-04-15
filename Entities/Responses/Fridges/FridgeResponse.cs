using Entities.Responses.FridgeModels;
using System;

namespace Entities.Responses.Fridges
{
    public class FridgeResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string OwnerName { get; set; }
        public FridgeModelResponse Model { get; set; }
    }
}
