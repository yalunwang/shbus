using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentScheduler;
namespace shgj
{
    public class MyJob : IJob
    {
        public void Execute()
        {
            Config.StartTime = DateTime.Now;
            Console.WriteLine("采集开始{0}",Config.StartTime);
            Process process = new Process();

            process.Start();
        }
    }
    
}
