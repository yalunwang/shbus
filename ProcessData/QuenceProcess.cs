using ShBus.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YaLunWang.Common.Json;
using YaLunWang.RedisFramework;

namespace ProcessData
{
    public class QuenceProcess
    {
        public void Start()
        {
            CarDataAccess carDataAccess = new CarDataAccess();
            string jsonCar = RedisServiceConfig.Default.PopItemFromList(RedisKeys.CARS);
            Car car = JsonHelper.Deserialize<Car>(jsonCar);
            carDataAccess.ImportCarData();
        }
        public void Init()
        {
            CarDataAccess carDataAccess = new CarDataAccess();
            carDataAccess.ImportLineData();
        }
        
    }
}
