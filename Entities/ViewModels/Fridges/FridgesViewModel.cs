using Entities.RequestFeatures;
using System.Collections.Generic;

namespace Entities.ViewModels.Fridges
{
    public class FridgesViewModel
    {
        public IEnumerable<FridgeViewModel> fridges { get; set; }
        public MetaData MetaData { get; set; }
    }
}
