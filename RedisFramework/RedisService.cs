using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace YaLunWang.RedisFramework
{
    public class RedisService : IRedisService
    {
        /// <summary>
        /// 配置文件中的Redis服务器配置信息
        /// </summary>
        private string configIp;
        private RedisPoolManager _RedisPool;
        /// <summary>
        /// 从配置文件中读取Redis的服务器地址
        /// </summary>
        /// <param name="configIp"></param>
        public RedisService(string configIp)
        {
            this.configIp = configIp;
            _RedisPool = new RedisPoolManager(configIp);
            _RedisPool.BuildRedisPoolClient();
        }
       
        public IRedisClient GetClient()
        {
            var redis = _RedisPool.GetClient();
            return redis;
        }
        public void Dispose(IRedisClient redis)
        {
            redis.Dispose();
        }

        public T Get<T>(string key)
        {
            var redis = _RedisPool.GetClient();
            var ret = redis.Get<T>(key);
            redis.Dispose();
            return ret;
        }
        public bool Set(string key, object obj)
        {
            using (var redis = _RedisPool.GetClient())
            {

                return redis.Set(key, obj);
            }
        }
        public bool Set(string key, object obj, TimeSpan expiresIn)
        {
            using (var redis = _RedisPool.GetClient())
            {
                return redis.Set(key, obj, expiresIn);
            }
        }
        public bool Set(string key, object obj, DateTime exporesAt)
        {
            using (var redis = _RedisPool.GetClient())
            {
                return redis.Set(key, obj, exporesAt);
            }
        }
        public void PrependItemToList(string key, string obj)
        {
            using (var redis = _RedisPool.GetClient())
            {
                redis.PrependItemToList(key, obj);
            }
        }

        public long GetListCount(string listId)
        {
            using (var redis = _RedisPool.GetClient())
            {
                return redis.GetListCount(listId);
            }
        }

        public string PopItemFromList(string listId)
        {
            using (var redis = _RedisPool.GetClient())
            {
                return redis.PopItemFromList(listId);
            }
        }



        public void RemoveAll(IEnumerable<string> keys)
        {
            using (var redis = _RedisPool.GetClient())
            {
                redis.RemoveAll(keys);
            }
        }

        public void Remove(string key)
        {
            using (var redis = _RedisPool.GetClient())
            {
                redis.Remove(key);
            }
        }

        public List<string> SearchKeys(string pattern)
        {
            using (var redis = _RedisPool.GetClient())
            {
                return redis.SearchKeys(pattern);
            }
        }
        #region
        public void SetHash(string key, string obj, string value)
        {
            using (var redis = _RedisPool.GetClient())
            {
                redis.SetEntryInHash(key, obj, value);
            }

        }

        public Dictionary<string, string> GetAllEntriesFromHash(string id)
        {
            using (var redis = _RedisPool.GetClient())
            {
                return redis.GetAllEntriesFromHash(id);
            }
        }
        public List<string> GetHashValues(string key)
        {
            using (var redis = _RedisPool.GetClient())
            {
                return redis.GetHashValues(key);
            }
        }


        #endregion
    }
}
