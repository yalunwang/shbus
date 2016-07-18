using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using FluentScheduler;
namespace shgj
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("系统于{0}开始采集",DateTime.Now.ToString());
            JobManager.Initialize(new MyRegistry());
            while (Console.ReadLine() != "exit")
            {
                ///空循环，保证只有输入Exit才退出
            }
            //process.Stop();
            Console.WriteLine("系统于{0}结束采集", DateTime.Now.ToString());
            Console.Read();
        }
       
    }
  
  

}
