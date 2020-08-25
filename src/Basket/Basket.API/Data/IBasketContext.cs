using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Basket.API.Data
{
    public interface IBasketContext
    {
        IDatabase Redis { get; }
    }
}
