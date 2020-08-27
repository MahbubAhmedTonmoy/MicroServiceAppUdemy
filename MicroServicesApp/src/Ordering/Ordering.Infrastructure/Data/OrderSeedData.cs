using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Ordering.Core.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Infrastructure.Data
{
    public class OrderSeedData
    {
        public static async Task SeedAsync(DataContext dataContext,int? retray = 0)
        {
            int retryForAvailability = retray.Value;

            try
            {
                dataContext.Database.Migrate();
                if (!dataContext.Orders.Any())
                {
                    dataContext.Orders.AddRange(PrepareSeed());
                    await dataContext.SaveChangesAsync();
                }
            }
            catch(Exception ex)
            {
                if (retryForAvailability < 5)
                {
                    retryForAvailability++;
                    await SeedAsync(dataContext, retryForAvailability);
                }
                throw;
            }
        }

        private static IEnumerable<Order> PrepareSeed()
        {
            return new List<Order>()
            {
                new Order() { UserName = "swn", FirstName = "Mehmet", LastName = "Ozkaya", EmailAddress = "meh@ozk.com", AddressLine = "Bahcelievler", TotalPrice = 5239 },
                new Order() { UserName = "swn", FirstName = "Selim", LastName = "Arslan", EmailAddress ="sel@ars.com", AddressLine = "Ferah", TotalPrice = 3486 }
            };
        }
    }
}
