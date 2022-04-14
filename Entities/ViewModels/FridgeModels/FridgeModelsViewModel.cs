using Entities.RequestFeatures;
using System.Collections.Generic;

namespace Entities.ViewModels.FridgeModels
{
    public class FridgeModelsViewModel
    {
        public IEnumerable<FridgeModelViewModel> fridgeModels { get; set; }
        public MetaData metaData { get; set; }
    }
}
