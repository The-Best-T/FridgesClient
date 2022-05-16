using Entities.RequestFeatures;
using System;
using System.Collections.Generic;

namespace Entities.ViewModels.FridgeProducts
{
    public class FridgeProductsViewModel
    {
        public IEnumerable<FridgeProductViewModel> fridgeProducts { get; set; }
        public MetaData MetaData { get; set; }
        public Guid FridgeId { get; set; }
    }
}
