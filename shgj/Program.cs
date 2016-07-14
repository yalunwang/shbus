using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace shgj
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("系统于{0}开始采集",DateTime.Now.ToString());
            Process process = new Process();
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
    class StopCar
    {
        /// <summary>
        /// 车牌号
        /// </summary>
        public string Terminal { get; set; }
        /// <summary>
        /// 到站时间
        /// </summary>
        public DateTime StopTime { get; set; }
    }
    public class Station
    {

    }
    class Car
    {
        public Car()
        {
            CurrentTime = DateTime.Now;
        }
        /// <summary>
        /// 车牌
        /// </summary>
        public string terminal { set; get; }
      
        /// <summary>
        /// 距离当前站还有几站
        /// </summary>
        public string stopdis { set; get; }
        /// <summary>
        /// 距离当前站的距离（10米）
        /// </summary>
        public string distance { set; get; }
        /// <summary>
        /// 距离当前站的距离时间(秒)
        /// </summary>
        public string time { set; get; }
        public DateTime CurrentTime { get; set; }

    }
}
