using DataAccess;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessData
{
    class Program
    {
        static void Main(string[] args)
        {
            DbInfo db= DbInfo.CreateInstance();
            string sql =string.Format("insert into LineInfo values(751512,22,{0},1)","'益江路张东路'");
            DbCommand Procdbcomm = db.GetSqlStringCommand(sql);

            db.ExecuteNonQuery(Procdbcomm);
        }
    }
}
