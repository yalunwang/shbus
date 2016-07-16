using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShBus.Model
{
    public class Car
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
