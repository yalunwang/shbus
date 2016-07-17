

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YaLunWang.RedisFramework;
using YaLunWang.Common.Json;
using ShBus.Model;
namespace ProcessData
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("系统于{0}开始采集", DateTime.Now.ToString());
            QuenceProcess process = new QuenceProcess();
            process.Start();
            while (Console.ReadLine() != "exit")
            {
                ///空循环，保证只有输入Exit才退出
            }
            process.Stop();
            Console.WriteLine("系统于{0}结束采集", DateTime.Now.ToString());
            Console.Read();
        }
    }
}
