﻿using System;
using System.Data;

namespace Shoy.Data
{
    public class OracalBuilder:ISqlBuilder
    {
        #region ISQLBuilder 成员

        public string ReplaceSql(string sql)
        {
            return sql.Replace("@", ":");
        }

        public void SetParameter(IDataParameter dp, string name, object value, ParameterDirection direction)
        {
            dp.ParameterName = string.Concat(":", name);
            dp.Value = value ?? DBNull.Value;
            dp.Direction = direction;
        }

        #endregion
        
        public void SetProcParameter(IDataParameter dp, string name, object value, ParameterDirection direction)
        {
            dp.ParameterName = name;
            dp.Value = value ?? DBNull.Value;
            dp.Direction = direction;
        }
    }
}
