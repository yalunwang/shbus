using YaLunWang.Common;
using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


namespace YaLunWang.RedisFramework
{
    /// <summary>
    /// Redis连接池管理
    /// </summary>
    public class RedisPoolManager
    {
        /// <summary>
        /// Redis服务器端的IP地址
        /// 如： 192.168.8.17:6379,192.168.8.18:6379,192.168.8.19:6379
        /// </summary>
        public List<string> RedisServiceAddress { get; set; }
        /// <summary>
        /// 连接池连接数
        /// </summary>
        public int RedisServicePoolSize { get; set; }
        private PooledRedisClientManager PooledRedisClient
        {
            get;
            set;
        }


        /// <summary>
        /// 建立连接池
        /// </summary>
        public void BuildRedisPoolClient()
        {
            RedisClientManagerConfig RedisConfig = new RedisClientManagerConfig();
            RedisConfig.AutoStart = true;
            RedisConfig.MaxReadPoolSize = RedisServicePoolSize;
            RedisConfig.MaxWritePoolSize = RedisServicePoolSize;
            PooledRedisClient = new PooledRedisClientManager(RedisServiceAddress, RedisServiceAddress, RedisConfig);
        }
        /// <summary>
        /// 构建链接驰信息
        /// </summary>
        /// <param name="configIp">配置服务器地址的Key</param>
        public RedisPoolManager(string configIp)
        {
            RedisServicePoolSize = ConfigHelper.AppSetting<int>("RedisServicePoolSize", 20);
            string ip = ConfigHelper.AppSetting<string>(configIp, "192.168.1.111:6379");
            if (string.IsNullOrEmpty(ip))
            {
                throw new KeyNotFoundException("没有添加配置参数：" + configIp);
            }
            RedisServiceAddress = Regex.Split(ip, ",").ToList<string>();

        }

        /// <summary>
        /// 获取一个链接
        /// </summary>
        /// <returns></returns>
        public IRedisClient GetClient()
        {
            return PooledRedisClient.GetClient();
        }

        /// <summary>
        /// 释放一个链接
        /// </summary>
        /// <param name="redisClient"></param>
        public void Dispose(IRedisClient redisClient)
        {
            redisClient.Dispose();
        }
    }
}
