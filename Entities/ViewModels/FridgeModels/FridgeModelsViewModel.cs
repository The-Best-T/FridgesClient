using Entities.RequestFeatures;
using System.Collections.Generic;

namespace Entities.ViewModels.FridgeModels
{
    public class FridgeModelsViewModel
    {
        public IEnumerable<FridgeModelViewModel> FridgeModels { get; set; }
        public MetaData MetaData { get; set; }
    }
}
