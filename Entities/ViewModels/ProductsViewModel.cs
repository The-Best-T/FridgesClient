﻿using Entities.RequestFeatures;
using System.Collections.Generic;

namespace Entities.ViewModels
{
    public class ProductsViewModel
    {
        public IEnumerable<ProductViewModel> products { get; set; }
        public MetaData metaData { get; set; }
    }
}
