using SqlSugar;
using System;

namespace Aurora.Jobs.Core.Business.Manager
{
    public class BaseManager
    {
        public SqlSugarClient db
        {
            get
            {
                return new DbManager().db;
            }
        }
    }
}
