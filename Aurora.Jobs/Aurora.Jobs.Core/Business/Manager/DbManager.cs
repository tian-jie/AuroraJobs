using SqlSugar;
using System;

namespace Aurora.Jobs.Core.Business.Manager
{
    public class DbManager
    {
        private string ConnectionString
        {
            get
            {
                return System.Configuration.ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
            }
        }

        private string DbType
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings.Get("DbType");
            }
        }

        public SqlSugarClient db
        {
            get
            {
                SqlSugarClient _db = null;

                if (DbType.ToLower() == "mysql")
                {
                    _db = new SqlSugarClient(new ConnectionConfig() { ConnectionString = ConnectionString, DbType = SqlSugar.DbType.MySql, IsAutoCloseConnection = true });
                }
                else if (DbType.ToLower() == "sqlserver")
                {
                    _db = new SqlSugarClient(new ConnectionConfig() { ConnectionString = ConnectionString, DbType = SqlSugar.DbType.SqlServer, IsAutoCloseConnection = true });
                }
                else
                {
                    throw new Exception("DbType:" + DbType + " 未知");
                }

                _db.Ado.IsEnableLogEvent = false;
                //_db.Ado.LogEventStarting = (sql, pars) =>
                //{
                //    Console.WriteLine(sql + "\r\n" + db.RewritableMethods.SerializeObject(pars));
                //    Console.WriteLine();
                //};
                if (_db != null)
                {
                    string ScheduledTaskMappingDbTable = GetDbTableNameSetting("ScheduledTaskMappingDbTable");
                    ScheduledTaskMappingDbTable = string.IsNullOrWhiteSpace(ScheduledTaskMappingDbTable) ? "ScheduledTask" : ScheduledTaskMappingDbTable;
                    _db.MappingTables.Add("ScheduledTaskInfo", ScheduledTaskMappingDbTable);

                    string ScheduledTaskHistoryMappingDbTable = GetDbTableNameSetting("ScheduledTaskHistoryMappingDbTable");
                    ScheduledTaskHistoryMappingDbTable = string.IsNullOrWhiteSpace(ScheduledTaskHistoryMappingDbTable) ? "ScheduledTaskHistory" : ScheduledTaskHistoryMappingDbTable;
                    _db.MappingTables.Add("ScheduledTaskHistoryInfo", ScheduledTaskHistoryMappingDbTable);

                }
                return _db;
            }
        }

        private string GetDbTableNameSetting(string dbName)
        {
            return System.Configuration.ConfigurationManager.AppSettings.Get(dbName);
        }
    }
}
