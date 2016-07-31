using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentScheduler;
using System.Threading;
namespace shgj
{

    public class MyRegistry : Registry
    {
        public MyRegistry()
        {
            // Schedule an IJob to run at an interval

            //Schedule<MyJob>().ToRunNow().AndEvery(2).Minutes();
            // Schedule<MyJob>().ToRunNow().ToRunEvery(1).Days().At(1, 30);

            //系统于每天的07：49开始采集
            Schedule<MyJob>().ToRunEvery(1).Days().At(07, 45);
            
            //// Schedule an IJob to run once, delayed by a specific time interval
            //Schedule<MyJob>().ToRunOnceIn(5).Seconds();

            //// Schedule a simple job to run at a specific time
            //Schedule(() => Console.WriteLine("It's 9:15 PM now."))
            //    .ToRunEvery(1).Days().At(21, 15);

            //// Schedule a more complex action to run immediately and on an monthly interval
            //Schedule(() =>
            //{
            //    Console.WriteLine("Complex job started at " + DateTime.Now);
            //    Thread.Sleep(10000);
            //    Console.WriteLine("Complex job ended at" + DateTime.Now);
            //}).ToRunNow().AndEvery(1).Months().OnTheFirst(DayOfWeek.Monday).At(3, 0);

            // Schedule multiple jobs to be run in a single schedule
            // Schedule<MyJob>().AndThen<MyOtherJob>().ToRunNow().AndEvery(5).Minutes();
        }
    }
}
