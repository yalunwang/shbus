using System;
using System.Collections;
using System.Data;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;

namespace YaLunWang.Common.Json
{
    public static class JsonHelper
    {
        private static JsonSerializerSettings _jsonSettings;

        static JsonHelper()
        {
            IsoDateTimeConverter datetimeConverter = new IsoDateTimeConverterContent();
            datetimeConverter.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
             
            _jsonSettings = new JsonSerializerSettings();
            _jsonSettings.MissingMemberHandling = Newtonsoft.Json.MissingMemberHandling.Ignore;
            _jsonSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
            _jsonSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            _jsonSettings.Converters.Add(datetimeConverter);
            _jsonSettings.ContractResolver = new LowercaseContractResolver();
        }

        /// <summary>
        /// 将指定的对象序列化成 JSON 数据。
        /// </summary>
        /// <param name="obj">要序列化的对象。</param>
        /// <returns></returns>
        public static string Serialize(object obj)
        {
            try
            {
                if (null == obj)
                    return null;

                return JsonConvert.SerializeObject(obj, Formatting.None, _jsonSettings);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 将指定的 JSON 数据反序列化成指定对象。
        /// </summary>
        /// <typeparam name="T">对象类型。</typeparam>
        /// <param name="json">JSON 数据。</param>
        /// <returns></returns>
        public static T Deserialize<T>(string json)
        {
            if (string.IsNullOrEmpty(json))
                return default(T);
            try
            {
                return JsonConvert.DeserializeObject<T>(json, _jsonSettings);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 将转换后的Key全部设置为小写
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static SortedDictionary<string, object> DeserializeLower(string json)
        {
            var obj = Deserialize<SortedDictionary<string, object>>(json);
            SortedDictionary<string, object> nobj = new SortedDictionary<string, object>();

            foreach (var item in obj)
            {
                nobj[item.Key.ToLower()] = item.Value;
            }
            obj.Clear();
            obj = null;
            return nobj;
        }
    }


    public class LowercaseContractResolver : Newtonsoft.Json.Serialization.DefaultContractResolver
    {

        protected override string ResolvePropertyName(string propertyName)
        {
            return propertyName.ToLower();
        }
    }
}


