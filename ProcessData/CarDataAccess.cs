using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using ProcessData.Common;
using System.Data.Common;
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
        /// 未完成
        /// </summary>
        public void ImportCarData()
        {
            DbInfo db = DbInfo.CreateInstance();
            string sql = string.Format("insert into LineInfo values(751512,22,{0},1)", "'益江路张东路'");
            DbCommand Procdbcomm = db.GetSqlStringCommand(sql);

            db.ExecuteNonQuery(Procdbcomm);

        }
    }
}
