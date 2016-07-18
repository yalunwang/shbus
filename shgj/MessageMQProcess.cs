using ShBus.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using YaLunWang.Common.Json;
using YaLunWang.RedisFramework;
namespace shgj
{
    public class Process
    {
        bool IsRun = false;
        Thread tread;
        //存储车辆到站数据，目的：防止到站车辆重复记录（只存储近二个小时内的到站车辆，因为一辆车在同一天或者第二天还会再经过）
        List<StopCar> stopCar = new List<StopCar>();
        //疑似到站车辆（将距离2站的车辆存起来防止漏算，因为有可能一直是2辆。)
        //判断最近的车辆是不是疑似车辆，如果不是就是漏算了他，那就将他认为是到站车辆
        List<Car> TempCar = new List<Car>();
      
        List<string> test = new List<string>();
        public Process()
        {
        }

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
                //trdReceive.Abort();
            }
            catch { }
        }
        public void BussProcess()
        {
            HttpClient _httpClient = new HttpClient();
            _httpClient.Timeout = new TimeSpan(0, 4, 0);//设置超时时间为4分钟

            string url = string.Format("http://bst.shdzyb.com:36001/Project/Ver2/carMonitor.ashx?lineid=751512&stopid=22&direction=true&my=E37B51992140936B0AA50FBE9B561711&t={0}", DateTime.Now.ToString("yyyy-MM-ddHH:mm"));

            while (IsRun)
            {
                #region test
                //test.Add(DateTime.Now.ToString());
                //Console.WriteLine("/*********************************/");
                //foreach (var item in test)
                //{

                //    Console.WriteLine(item);
                //}
                #endregion
                if (Config.StartTime.AddMinutes(2) < DateTime.Now)
                {
                    Console.WriteLine("截止时间到了");
                    Stop();
                    break;
                }
                string html = GetRequest(url, _httpClient);
                if (html == "Failed")
                {
                    System.Threading.Thread.Sleep(30000);//暂停20秒
                    continue;
                }
                XmlDocument doc = new XmlDocument();

                doc.InnerXml = html;
                List<Dictionary<string, string>> list = new List<Dictionary<string, string>>();
                foreach (XmlNode node in doc.SelectSingleNode("//result").ChildNodes)
                {
                    foreach (XmlNode nodec in node.ChildNodes)
                    {
                        Dictionary<string, string> dic = new Dictionary<string, string>();
                        foreach (XmlNode noded in nodec.ChildNodes)
                        {
                            dic.Add(noded.Name, noded.InnerText);
                        }
                        list.Add(dic);

                    }
                }

                string json = JsonHelper.Serialize(list);
                List<Car> car = JsonHelper.Deserialize<List<Car>>(json);
                Console.WriteLine("/&&&&&当前时间：{0}实时统计开始&&&&&/", DateTime.Now);
                string msg = string.Format("/&&&&&当前时间：{0}实时统计开始&&&&&/", DateTime.Now);
                Utils.WriteLog(msg, DateTime.Now.ToString("yyyy-MM-dd") + "-shisi.txt");

                foreach (var item in car)
                {
                    if (item.terminal != "null")
                    {
                        msg = "车牌:  " + item.terminal;
                        Console.WriteLine("车牌:  " + item.terminal);
                        Utils.WriteLog(msg, DateTime.Now.ToString("yyyy-MM-dd") + "-shisi.txt");


                        msg = "距离当前还有:  " + item.stopdis + "站";
                        Console.WriteLine(msg);
                        Utils.WriteLog(msg, DateTime.Now.ToString("yyyy-MM-dd") + "-shisi.txt");


                        Console.WriteLine("距离:  " + Convert.ToInt32(item.distance) * 0.01 + "Km");
                        Utils.WriteLog("距离:  " + Convert.ToInt32(item.distance) * 0.01 + "Km", DateTime.Now.ToString("yyyy-MM-dd") + "-shisi.txt");


                        msg = string.Format("还有: {1}分钟{0}秒 ", Convert.ToInt32(item.time) % 60, Convert.ToInt32(item.time) / 60);
                        Console.WriteLine("还有: {1}分钟{0}秒 ", Convert.ToInt32(item.time) % 60, Convert.ToInt32(item.time) / 60);
                        Utils.WriteLog(msg, DateTime.Now.ToString("yyyy-MM-dd") + "-shisi.txt");
                        string jsonCar = JsonHelper.Serialize(item);
                        try
                        {
                            RedisServiceConfig.Default.PrependItemToList(RedisKeys.CARS, jsonCar);
                        }
                        catch (Exception e)
                        {

                            Console.WriteLine("\r\n");
                            Console.WriteLine(e.ToString());
                            Utils.WriteLog(e.ToString(), DateTime.Now.ToString("yyyy-MM-dd") + "-error.txt");
                            Console.WriteLine("\r\n");
                        }


                    }


                }
                Console.WriteLine("/&&&&&&&&实时统计结束&&&&&&&&&&/", DateTime.Now);
                Console.WriteLine("\n");
                Utils.WriteLog("/&&&&&&&&实时统计结束&&&&&&&&&&/", DateTime.Now.ToString("yyyy-MM-dd") + "-shisi.txt");

                if (car.Count != 0 && car[0].time != "null")
                {
                  
                    #region 到站车辆
                    //时间小于40秒，或者距离有一站而且时间小于80秒  认做车是到站了
                    if (Convert.ToInt32(car[0].time) <= 40 || (Convert.ToInt32(car[0].stopdis) == 1 && Convert.ToInt32(car[0].time) <= 80))
                    {
                        if (stopCar.Count != 0)
                        {
                            //如果车牌号不在
                            if (!stopCar.Select(o => o.Terminal).ToList().Contains(car[0].terminal))
                            {
                                StopCar stopCarItem = new StopCar();
                                stopCarItem.Terminal = car[0].terminal;
                                stopCarItem.StopTime = DateTime.Now;
                                stopCar.Add(stopCarItem);
                                string msgStop = string.Empty;
                                Console.WriteLine("/现在是{0}***统计到站车辆/", DateTime.Now);
                                msgStop = string.Format("/现在是{0}***统计到站车辆/", DateTime.Now);
                                Utils.WriteLog(msgStop, DateTime.Now.ToString("yyyy-MM-dd") + "-tj.txt");


                                msgStop = string.Format("车牌号：{0}，{1}到益江路张东路", stopCarItem.Terminal, stopCarItem.StopTime);
                                Console.WriteLine("车牌号：{0}，{1}到益江路张东路", stopCarItem.Terminal, stopCarItem.StopTime);
                                Utils.WriteLog(msgStop, DateTime.Now.ToString("yyyy-MM-dd") + "-tj.txt");

                                try
                                {
                                    string jsonCar = JsonHelper.Serialize(stopCarItem);
                                    RedisServiceConfig.Default.PrependItemToList(RedisKeys.STOPCARS, jsonCar);
                                }
                                catch (Exception e)
                                {

                                    Console.WriteLine("\r\n");
                                    Console.WriteLine(e.ToString());
                                    Utils.WriteLog(e.ToString(), DateTime.Now.ToString("yyyy-MM-dd") + "-error.txt");
                                    Console.WriteLine("\r\n");
                                }

                                if (stopCar.Where(o => o.StopTime < DateTime.Now.AddHours(-2)).Count() != 0)
                                {
                                    foreach (var item in stopCar.Where(o => o.StopTime < DateTime.Now.AddHours(-2)))
                                    {
                                        stopCar.Remove(item);//删除2个小时以前的到站数据
                                    }
                                }
                            }

                        }
                        else
                        {
                            StopCar stopCarItem = new StopCar();
                            stopCarItem.Terminal = car[0].terminal;
                            stopCarItem.StopTime = DateTime.Now;
                            stopCar.Add(stopCarItem);
                            Console.WriteLine("/现在是{0}***统计到站车辆/", DateTime.Now);
                            string msgStop = string.Empty;
                            msgStop = string.Format("/现在是{0}***统计到站车辆/", DateTime.Now);
                            Utils.WriteLog(msgStop, DateTime.Now.ToString("yyyy-MM-dd") + "-tj.txt");

                            msgStop = string.Format("车牌号：{0}，{1}到益江路张东路", stopCarItem.Terminal, stopCarItem.StopTime);
                            Console.WriteLine("车牌号：{0}，{1}到益江路张东路", stopCarItem.Terminal, stopCarItem.StopTime);
                            Utils.WriteLog(msgStop, DateTime.Now.ToString("yyyy-MM-dd") + "-tj.txt");
                            try
                            {
                                string jsonCar = JsonHelper.Serialize(stopCarItem);
                                RedisServiceConfig.Default.PrependItemToList(RedisKeys.STOPCARS, jsonCar);
                            }
                            catch (Exception e)
                            {

                                Console.WriteLine("\r\n");
                                Console.WriteLine(e.ToString());
                                Utils.WriteLog(e.ToString(), DateTime.Now.ToString("yyyy-MM-dd") + "-error.txt");
                                Console.WriteLine("\r\n");
                            }
                        }



                    }
                    #endregion
                  
                }


                System.Threading.Thread.Sleep(30000);//暂停20秒
            }


        }
        public static string GetRequest(string url, HttpClient httpClient)
        {
            try
            {
                HttpResponseMessage response = httpClient.GetAsync(new Uri(url)).Result;
                return response.Content.ReadAsStringAsync().Result;

            }
            catch (Exception e)
            {
                Utils.WriteLog(e.ToString(), DateTime.Now.ToString("yyyy-MM-dd") + "-error.txt");
                return "Failed";
            }

        }
    }
}
