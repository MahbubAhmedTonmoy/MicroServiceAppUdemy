using Microsoft.EntityFrameworkCore;
using Ordering.Core.Entities;
using Ordering.Core.Repository;
using Ordering.Infrastructure.Data;
using Ordering.Infrastructure.Repository.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Infrastructure.Repository
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        public OrderRepository(DataContext dataContext): base(dataContext)
        {

        }
        public async Task<IEnumerable<Order>> GetOrderByUserName(string userName)
        {
            var result = await _dataContext.Orders.Where(x => x.UserName == userName).ToListAsync();
            return result;
        }
    }
}
