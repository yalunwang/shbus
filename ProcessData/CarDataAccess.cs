using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using ProcessData.Common;
using System.Data.Common;
using ShBus.Model;
namespace ProcessData
{
    public class CarDataAccess
    {
        public void ImportLineData()
        {
            DbInfo db = DbInfo.CreateInstance();
            string sql = string.Format("insert into LineInfo values(751512,22,{0},1)", "'益江路张东路'");
            DbCommand Procdbcomm = db.GetSqlStringCommand(sql);

            db.ExecuteNonQuery(Procdbcomm);

        }
        /// <summary>
        /// 导入车辆数据
        /// </summary>
        public void ImportCarData(Car car)
        {
            DbInfo db = DbInfo.CreateInstance();
            string sql = string.Format(@"insert into CarRecord (terminal,stopdis,distance,time,currentTime,lineInfoId) 
            values('{0}',{1},{2},{3},'{4}',{5})", car.terminal, car.stopdis, car.distance, car.time, car.CurrentTime, 2);
            DbCommand Procdbcomm = db.GetSqlStringCommand(sql);

            db.ExecuteNonQuery(Procdbcomm);

        }
        /// <summary>
        /// 插入到站车辆
        /// </summary>
        /// <param name="stopCar"></param>
        public void ImportStopCar(StopCar stopCar)
        {
            DbInfo db = DbInfo.CreateInstance();
            string sql = string.Format("insert into StopCar (terminal,stopTime,lineInfoId) values('0}','{1}',{2})", stopCar.Terminal, stopCar.StopTime, 2);
            DbCommand Procdbcomm = db.GetSqlStringCommand(sql);

            db.ExecuteNonQuery(Procdbcomm);

        }
    }
}
