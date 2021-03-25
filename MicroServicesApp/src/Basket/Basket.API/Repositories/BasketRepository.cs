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
        private readonly ICacheStore _contest;
        public BasketRepository(ICacheStore contest)
        {
            _contest = contest;
        }
        public async Task<bool> DeleteBasket(string userName)
        {
            return await _contest.RemoveAsync(userName);
        }

        public async Task<BasketCart> GetBasketCart(string userName)
        {
            var basket = await _contest.GetValueAsync<BasketCart>(userName);
            if (basket == null)
            {
                return null;
            }
            return basket;
            //return JsonConvert.DeserializeObject<BasketCart>(basket);
        }

        public async Task<BasketCart> UpdateBasket(BasketCart basketCart)
        {
            var update = await _contest.AddAsync(basketCart.UserName, JsonConvert.SerializeObject(basketCart));
            if (!update)
            {
                return null;
            }
            return await GetBasketCart(basketCart.UserName);
        }
    }
}
