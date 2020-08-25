using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Basket.API.Entities
{
    public class BasketCart
    {
        public string UserName { get; set; }
        public List<BasketCartItem> Items { get; set; } = new List<BasketCartItem>();

        public BasketCart()
        {

        }
        public BasketCart(string username)
        {
            this.UserName = username;
        }
        public decimal TotalPrice 
        {
            get
            {
                decimal totalPrice = 0;
                foreach(var i in Items)
                {
                    totalPrice += i.Price * i.Quantity;
                }
                return totalPrice;
            }
        }
    }
}
