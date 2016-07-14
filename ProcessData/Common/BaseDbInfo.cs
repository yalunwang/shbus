using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopJet.P2PService.DataAccess
{
    public class BaseDbInfo
    {
        /// <summary>
        /// 处理结果
        /// </summary>
        public DbResult ExecuteResult { get; set; }

        protected Database db;
        public Database DB { get { return db; } }

        protected List<DbParameter> listDbParameter;
        public List<DbParameter> ListDbParameter { get { return listDbParameter; } }

        public DbCommand Procdbcomm { get; set; }

        public DbCommand GetStoredProcCommand(string storedProcedureName)
        {
            Procdbcomm = db.GetStoredProcCommand(storedProcedureName);
            Procdbcomm.CommandTimeout = 1200;
            return Procdbcomm;
        }
        public DbCommand GetSqlStringCommand(string query)
        {
            Procdbcomm = db.GetSqlStringCommand(query);
            Procdbcomm.CommandTimeout = 1200;
            return Procdbcomm;
        }
        /// <summary>
        /// 添加输入型参数
        /// </summary>
        /// <param name="command">这个参数不用，可以传空</param>
        /// <param name="name"></param>
        /// <param name="dbType"></param>
        /// <param name="value"></param>
        public void AddInParameter(DbCommand command, string name, DbType dbType, object value)
        {
            db.AddInParameter(Procdbcomm, name, dbType, value);
        }
        /// <summary>
        /// 添加非string型的输出参数
        /// </summary>
        /// <param name="command">这个参数不用，可以传空</param>
        /// <param name="name"></param>
        /// <param name="dbType"></param>
        /// <param name="size"></param>
        public void AddOutParameter(DbCommand command, string name, DbType dbType, int size)
        {
            db.AddOutParameter(Procdbcomm, name, dbType, size);
        }
        /// <summary>
        /// 添加string型的输出参数，因为Linux的特殊性必须有默认代入值
        /// </summary>
        /// <param name="command">这个参数不用，可以传空</param>
        /// <param name="name"></param>
        /// <param name="size"></param>
        public void AddInOutParameter(DbCommand command, string name, int size)
        {
            db.AddParameter(Procdbcomm, name, DbType.String, size, ParameterDirection.InputOutput, false, 0, 0, "", DataRowVersion.Default, "");
        }

        /// <summary>
        /// 获取参数的值
        /// </summary>
        /// <param name="command">这个参数不用，可以传空</param>
        /// <param name="name"></param>
        /// <returns></returns>
        public object GetParameterValue(DbCommand command, string name)
        {
            if (ExecuteResult == null)
                throw new Exception("ExecuteResult没有传值.");
            return ExecuteResult.OutValues[name];
        }


        /// <summary>
        /// 执行结果集查询
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public DataSet ExecuteDataSet(DbCommand command)
        {
            try
            {
                ExecuteResult.Ds = db.ExecuteDataSet(Procdbcomm);
                return ExecuteResult.Ds;
            }
            finally
            {
                Clear();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public object ExecuteScalar(DbCommand command)
        {
            try
            {
                return db.ExecuteScalar(Procdbcomm);
            }
            finally
            {
                Clear();
            }
        }

        /// <summary>
        /// 执行查询操作
        /// </summary>
        /// <param name="command">这个参数没什么用</param>
        /// <returns></returns>
        public int ExecuteNonQuery(DbCommand command)
        {
            int ret = 0;
            ExecuteResult = new DbResult();
            try
            {
                if (Procdbcomm.CommandType == CommandType.StoredProcedure)
                    db.AddParameter(Procdbcomm, "ReturnValue", DbType.Int32, ParameterDirection.ReturnValue, "", DataRowVersion.Default, 0);

                //执行查询
                ret = db.ExecuteNonQuery(Procdbcomm);
                for (int i = 0; i < Procdbcomm.Parameters.Count; i++)
                {
                    var item = Procdbcomm.Parameters[i];
                    ///输出类型的参数
                    if (item != null)
                    {
                        if (item.Direction == ParameterDirection.InputOutput
                        || item.Direction == ParameterDirection.Output)
                        {
                            object paramValue = item.Value;// db.GetParameterValue(Procdbcomm, item.ParameterName);
                            if (item.ParameterName.ToLower() == "@ret")
                            {
                                if (paramValue is long)
                                    ExecuteResult.LongRet = (long)paramValue;
                                if (paramValue is int)
                                    ExecuteResult.IntRet = (int)paramValue;
                            }
                            if (item.ParameterName.ToLower() == "@stroutput")
                            {
                                ExecuteResult.StrOutput = paramValue.ToString();
                            }
                            ExecuteResult.OutValues.Add(item.ParameterName, paramValue);
                        }
                        if (item.Direction == ParameterDirection.ReturnValue)
                        {
                            ExecuteResult.ReturnValue = (int)item.Value;
                        }
                    }
                }
            }
            finally
            {
                Clear();
            }
            return ret;
        }

        /// <summary>
        /// 释放使用到的所有资源
        /// </summary>
        public void Clear()
        {
            if (Procdbcomm != null)
            {
                if (Procdbcomm.Connection != null)
                    Procdbcomm.Connection.Dispose();
                Procdbcomm.Dispose();
            }
            if (listDbParameter != null)
            {
                listDbParameter.Clear();
                listDbParameter = null;
            }
            Procdbcomm = null;
            db = null;
        }
    }
}
