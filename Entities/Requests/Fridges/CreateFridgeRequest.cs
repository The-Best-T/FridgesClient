using System;

namespace Entities.Requests.Fridges
{
    public class CreateFridgeRequest : ManipuldateFridgeRequest
    {
        public Guid ModelId { get; set; }
    }
}
