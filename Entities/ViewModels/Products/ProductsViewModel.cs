using Entities.RequestFeatures;
using System.Collections.Generic;

namespace Entities.ViewModels.Products
{
    public class ProductsViewModel
    {
        public IEnumerable<ProductViewModel> products { get; set; }
        public MetaData MetaData { get; set; }
    }
}
