using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace Utils
{
    public delegate T ActionDelegate<T>();
    public class StackRedisHelper : IDisposable
    {
        private static ConnectionMultiplexer conn;
        private static string connStr;
        private static int db;
        private static string name;
        private static object locker = new Object();
        private static object lockerInstance = new Object();
        private static StackRedisHelper instance;
        private StackRedisHelper() { }
        public static void InitRedis(string connStr, int db, string name)
        {
            StackRedisHelper.connStr = connStr;
            StackRedisHelper.db = db;
            StackRedisHelper.name = name;
            instance = new StackRedisHelper();
            conn = ConnectionMultiplexer.Connect(StackRedisHelper.connStr);
        }
        public static StackRedisHelper Instance
        {
            //TODO: 做成单例
            get { return instance; }
        }
        private static ConnectionMultiplexer Conn
        {
            get { return conn; }
        }
        private static string MergeKey (string key)
        {
            return $"{name}_{key}";
        }
        public IDatabase GetDatabase()
        {
            return Conn.GetDatabase(db);
        }
        public string Get(string key)
        {
            string nkey = MergeKey(key);
            string result = GetDatabase().StringGet(nkey);
            return result;
        }
        public async Task<string> GetAsync(string key)
        {
            key = MergeKey(key);
            string result = await GetDatabase().StringGetAsync(key);
            return result;
        }
        public async Task<T> GetAsync<T>(string key)
        {
            string resultStr = await GetAsync(key);
            T result = JsonConvert.DeserializeObject<T>(resultStr);
            return result;
        }
        public async Task<bool> SetAsync(string key, string value, int expireSeconds = 0)
        {
            key = MergeKey(key);
            return expireSeconds > 0 ? await GetDatabase().StringSetAsync(key, value, TimeSpan.FromSeconds(expireSeconds)) : 
                                    await GetDatabase().StringSetAsync(key, value);
        }
        public async Task<bool> SetAsync<T>(string key, T value, int expireSeconds = 0)
        {
            string valStr = JsonConvert.SerializeObject(value);
            return await SetAsync(key, valStr, expireSeconds);
        }
        public void Dispose()
        {
            if (conn != null)
            {
                conn.Close();
                instance = null;
            }
        }

    }
}