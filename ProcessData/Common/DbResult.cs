using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
   
    public class DbResult
    {
        public DbResult()
        {
            ReturnValue = -999;
            IntRet = -999;
            LongRet = -999;
            StrOutput = "";
            OutValues = new Dictionary<string, object>();
            Ds = new DataSet();
        }

        /// <summary>
        /// 存储过程执行的返回值 ReturnValue
        /// </summary>
        public int ReturnValue { get; set; }

        public int IntRet { get; set; }
        public long LongRet { get; set; }

        public string StrOutput { get; set; }

        public Dictionary<string, object> OutValues { get; set; }

        public DataSet Ds { get; set; }
    }
}
