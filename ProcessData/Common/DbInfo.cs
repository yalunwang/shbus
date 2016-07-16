
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessData.Common
{
  
    public class DbInfo : BaseDbInfo
    {
        /// <summary>
        /// 创建常规的数据库操作对象
        /// </summary>
        /// <returns></returns>
        public static DbInfo CreateInstance()
        {
            DbInfo instance = new DbInfo();
            return instance;
        }
        /// <summary>
        /// 根据链接构建实例
        /// </summary>
        private DbInfo()
        {
            DatabaseProviderFactory factory = new DatabaseProviderFactory();
            db = factory.Create("DBConnStr");
            ExecuteResult = new DbResult();

            Procdbcomm = db.DbProviderFactory.CreateCommand();
            listDbParameter = new List<DbParameter>();
        }
    }
}
