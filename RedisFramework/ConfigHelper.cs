using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedisFramework
{
    public class ConfigHelper
    {
        public static T AppSetting<T>(string configStr, T defaultValue = default(T))
        {
            object param = System.Configuration.ConfigurationManager.AppSettings[configStr];
            try
            {
                return (T)Convert.ChangeType(param, typeof(T));
            }
            catch
            {
                return defaultValue;
            }
        }
    }
}
