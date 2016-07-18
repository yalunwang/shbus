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
            Console.WriteLine("{0}调度系统初始化",DateTime.Now.ToString());
            JobManager.Initialize(new MyRegistry());
           
            Console.Read();
        }
       
    }
  
  

}
