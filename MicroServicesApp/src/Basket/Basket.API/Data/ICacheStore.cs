using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Basket.API.Data
{
    public interface ICacheStore
    {
		bool Add(string key, object value);

		Task<bool> AddAsync(string key, object value);

		bool AddOrUpdate(string key, object value);

		Task<bool> AddOrUpdateAsync(string key, object value);

		bool AddWithExprity(string key, object value, long keyLifeSpan);

		Task<bool> AddWithExprityAsync(string key, object value, long keyLifeSpanInSeconds);

		bool Remove(string key);

		Task<bool> RemoveAsync(string key);

		T GetValue<T>(string key);

		Task<T> GetValueAsync<T>(string key);

		bool Exists(string key);

		Task<bool> ExistsAsync(string key);
	}
}
