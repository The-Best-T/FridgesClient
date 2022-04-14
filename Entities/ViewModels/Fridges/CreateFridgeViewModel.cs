using Entities.ViewModels.FridgeModels;
using System;

namespace Entities.ViewModels.Fridges
{
    public class CreateFridgeViewModel : ManipulateFridgeViewModel
    {
        public Guid ModelId { get; set; }
        public FridgeModelsViewModel Models { get; set; }
    }
}
