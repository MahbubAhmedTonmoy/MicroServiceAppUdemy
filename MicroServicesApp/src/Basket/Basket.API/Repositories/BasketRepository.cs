using Basket.API.Data;
using Basket.API.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Basket.API.Repositories
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IBasketContext _contest;
        public BasketRepository(IBasketContext contest)
        {
            _contest = contest;
        }
        public async Task<bool> DeleteBasket(string userName)
        {
            return await _contest.Redis.KeyDeleteAsync(userName);
        }

        public async Task<BasketCart> GetBasketCart(string userName)
        {
            var basket = await _contest.Redis.StringGetAsync(userName);
            if (basket.IsNullOrEmpty)
            {
                return null;
            }
            return JsonConvert.DeserializeObject<BasketCart>(basket);
        }

        public async Task<BasketCart> UpdateBasket(BasketCart basketCart)
        {
            var update = await _contest.Redis.StringSetAsync(basketCart.UserName, JsonConvert.SerializeObject(basketCart));
            if (!update)
            {
                return null;
            }
            return await GetBasketCart(basketCart.UserName);
        }
    }
}
