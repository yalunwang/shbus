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
            DbInfo hh= DbInfo.CreateInstance();


            Database db = hh.DB;
            string procName = "insert into ";
            DbCommand Procdbcomm = db.GetStoredProcCommand(procName);
            db.AddInParameter(Procdbcomm, "@UID", DbType.Int64, "");
            db.ExecuteNonQuery(procName);
        }
    }
}
