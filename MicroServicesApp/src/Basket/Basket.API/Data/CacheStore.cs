using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Basket.API.Data
{
    public class CacheStore : ICacheStore
    {
        private readonly ConnectionMultiplexer _redisConnection;
		public CacheStore(ConnectionMultiplexer redisConnection)
        {
            _redisConnection = redisConnection;
        }
        
		public bool Add(string key, object value)
		{
			IDatabase database = _redisConnection.GetDatabase();
			return _redisConnection.GetDatabase().StringSet(key, GetFormatedData(value));
		}

		public Task<bool> AddAsync(string key, object value)
		{
			return _redisConnection.GetDatabase().StringSetAsync(key, GetFormatedData(value));
		}

		public bool AddOrUpdate(string key, object value)
		{
			return Add(key, value);
		}

		public Task<bool> AddOrUpdateAsync(string key, object value)
		{
			return AddAsync(key, value);
		}

		public bool AddWithExprity(string key, object value, long keyLifeSpan)
		{
			_redisConnection.GetDatabase().StringSet(key, GetFormatedData(value));
			DateTime value2 = DateTime.UtcNow.AddSeconds(keyLifeSpan);
			return _redisConnection.GetDatabase().KeyExpire(key, value2);
		}

		public Task<bool> AddWithExprityAsync(string key, object value, long keyLifeSpanInSeconds)
		{
			TimeSpan value2 = TimeSpan.FromSeconds(keyLifeSpanInSeconds);
			return _redisConnection.GetDatabase().StringSetAsync(key, GetFormatedData(value), value2);
		}

		public bool Remove(string key)
		{
			return _redisConnection.GetDatabase().KeyDelete(key);
		}

		public Task<bool> RemoveAsync(string key)
		{
			return _redisConnection.GetDatabase().KeyDeleteAsync(key);
		}

		public T GetValue<T>(string key)
		{
			RedisValue value = _redisConnection.GetDatabase().StringGet(key);
			return GetObjectFromString<T>(value);
		}

		public async Task<T> GetValueAsync<T>(string key)
		{
			return GetObjectFromString<T>(await _redisConnection.GetDatabase().StringGetAsync(key));
		}

		public bool Exists(string key)
		{
			return _redisConnection.GetDatabase().KeyExists(key);
		}

		public Task<bool> ExistsAsync(string key)
		{
			return _redisConnection.GetDatabase().KeyExistsAsync(key);
		}

		private string GetFormatedData(object data)
		{
			return JsonConvert.SerializeObject(data);
		}

		private T GetObjectFromString<T>(string stringData)
		{
            try
            {
				var type = typeof(T).Name;
                var result = JsonConvert.DeserializeObject<T>(stringData, new JsonSerializerSettings
				{
					NullValueHandling = NullValueHandling.Ignore
				});
                return result;
            }
            catch (Exception ex)
            {

                throw;
            }
		}
	}
}
