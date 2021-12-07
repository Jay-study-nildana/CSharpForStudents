﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Moq4Demo1.Services
{
    public interface ICartService
    {
        double Total();
        IEnumerable<CartItem> Items();
    }
}