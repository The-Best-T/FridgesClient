﻿using System;

namespace Entities.Responses.Account
{
    public class ProductResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int DefaultQuantity { get; set; }
    }
}
