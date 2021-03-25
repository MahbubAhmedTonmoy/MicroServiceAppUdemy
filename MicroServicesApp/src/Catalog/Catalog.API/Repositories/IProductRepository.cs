//using Catalog.API.Entities;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace Catalog.API.Repositories
//{
//    public interface IProductRepository
//    {
//        Task<IEnumerable<Product>> GetProducts();
//        Task<Product> GetProduct(string id);
//        Task<IEnumerable<Product>> GetProductByName(string name);
//        Task<IEnumerable<Product>> GetProductByCategory(string categoryName);
//        Task Create(Product product);
//        Task<bool> Update(Product product);
//        Task<bool> Delete(string id);
//    }
//}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Catalog.API.Repositories
{
    public interface IProductRepository
    {
        string DatabaseName { get; set; }

        void Delete<T>(Expression<Func<T, bool>> dataFilters);
        void Delete<T>(Expression<Func<T, bool>> dataFilters, string collectionName);
        string ExecuteCommand(string query);
        T GetItem<T>(Expression<Func<T, bool>> dataFilters);
        IQueryable<T> GetItems<T>(Expression<Func<T, bool>> dataFilters);
        IQueryable<T> GetItems<T>();
        void Initialize();
        void Save<T>(T data, string collectionName = "");
        void Save<T>(List<T> datas);
        void Update<T>(Expression<Func<T, bool>> dataFilters, T data);
        void UpdateMany<T>(Expression<Func<T, bool>> dataFilters, object data, string collectionName = "");
    }
}
