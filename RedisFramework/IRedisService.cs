using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YaLunWang.RedisFramework
{

    public interface IRedisService
    {
        IRedisClient GetClient();
        void Dispose(IRedisClient redis);
 
        T Get<T>(string key);
        bool Set(string key, object obj);
        bool Set(string key, object obj, TimeSpan expiresIn);
        bool Set(string key, object obj, DateTime exporesAt);
        void PrependItemToList(string key, string obj);

        long GetListCount(string listId);

        string PopItemFromList(string listId);



        void RemoveAll(IEnumerable<string> keys);

        void Remove(string key);

        List<string> SearchKeys(string pattern);
        void SetHash(string key, string jj, string kk);
        Dictionary<string, string> GetAllEntriesFromHash(string id);
        List<string> GetHashValues(string key);
    }
}
