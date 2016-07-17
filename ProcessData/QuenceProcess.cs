using ShBus.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using YaLunWang.Common.Json;
using YaLunWang.RedisFramework;

namespace ProcessData
{
    public class QuenceProcess
    {
        bool IsRun = false;
        Thread tread;
        public void Start()
        {
            if (tread == null)
            {
                IsRun = true;
                //启动消息监听线程
                tread = new Thread(new ThreadStart(BussProcess));
                tread.Priority = ThreadPriority.Lowest;
                tread.Start();
            }
            else
            {
                if (!IsRun)
                {
                    IsRun = true;
                    tread.Start();
                }
            }
        }
        public void Stop()
        {
            IsRun = false;
            try
            {
                tread.Abort();
            }
            catch (Exception ex){
                Console.WriteLine(DateTime.Now+"程序停止时出现异常",ex.ToString());
            }
        }
        /// <summary>
        /// 取出redis队列并插入数据库
        /// </summary>
        public void BussProcess()
        {

            CarDataAccess carDataAccess = new CarDataAccess();
            while (IsRun)
            {
                string jsonCar = RedisServiceConfig.Default.PopItemFromList(RedisKeys.CARS);

                if (!string.IsNullOrEmpty(jsonCar))
                {
                    try
                    {
                        Car car = JsonHelper.Deserialize<Car>(jsonCar);
                        carDataAccess.ImportCarData(car);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(DateTime.Now + "出现异常", ex.ToString());
                    }
                  
                   
                }
                string jsonStopCar = RedisServiceConfig.Default.PopItemFromList(RedisKeys.STOPCARS);

                if (!string.IsNullOrEmpty(jsonStopCar))
                {
                    try
                    {
                        StopCar stopCar = JsonHelper.Deserialize<StopCar>(jsonStopCar);
                        carDataAccess.ImportStopCar(stopCar);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(DateTime.Now + "出现异常", ex.ToString());
                    }


                }
                System.Threading.Thread.Sleep(100);//暂停1秒
            }

        }
        //public void Init()
        //{
        //    CarDataAccess carDataAccess = new CarDataAccess();
        //    carDataAccess.ImportLineData();
        //}

    }
}
