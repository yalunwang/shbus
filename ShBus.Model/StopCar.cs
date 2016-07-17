using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShBus.Model
{
    /// <summary>
    /// 到站车辆
    /// </summary>
    public class StopCar
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
}
