using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopJet.FrameWork.Helper.Redis
{
    /// <summary>
    /// Redis的配置信息
    /// </summary>
    public class RedisServiceConfig
    {
        private static object _syncObject = new object();
        private static IRedisService _Default;
        /// <summary>
        /// 默认的Redis配置
        /// </summary>
        public static IRedisService Default
        {
            get
            {
                if (_Default == null)
                {
                    lock (_syncObject)
                    {
                        if (_Default == null)
                        {
                            _Default = new RedisService("RedisServiceDefault");
                        }
                    }
                }
                return _Default;
            }
        }

        private static IRedisService _SysLog;
        /// <summary>
        /// 系统操作日志配置
        /// </summary>
        public static IRedisService SysLog
        {
            get
            {
                if (_SysLog == null)
                {
                    lock (_syncObject)
                    {
                        if (_SysLog == null)
                        {
                            _SysLog = new RedisService("RedisServiceForSystemLog");
                        }
                    }
                }
                return _SysLog;
            }
        }


        private static Dictionary<string, IRedisService> _RedisDic;
        /// <summary>
        /// 通过配置字符串强行获取Redis服务
        /// </summary>
        /// <param name="configIp"></param>
        /// <returns></returns>
        public static IRedisService GetService(string configIp)
        {
            if (_RedisDic == null)
            {
                _RedisDic = new Dictionary<string, IRedisService>();
            }
            IRedisService redis;
            if (!_RedisDic.TryGetValue(configIp, out redis))
            {
                redis = new RedisService(configIp);
                _RedisDic.Add(configIp, redis);
            }
            return redis;
        }
    }
}
