﻿using FastModel.DataModel;
using FastData.Context;
using FastUntility.Base;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System;
using System.Data.Common;
using Oracle.ManagedDataAccess.Client;
using MySql.Data.MySqlClient;
using System.Text;
using System.Data.SqlClient;
using FastData;

namespace FastService.Base
{
    public static class DataSchema
    {
        #region 获取连接字符串
        /// <summary>
        /// 获取连接字符串
        /// </summary>
        private static string GetConnStr(Base_DataSource link)
        {
            var connStr = "";

            if (link.Type == FastApp.DataDbType.Oracle)
            {
                connStr = string.Format("User Id={0};Password={1};Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST={2})(PORT={3})))(CONNECT_DATA=(SERVICE_NAME={4})));pooling=true;Min Pool Size=1;Max Pool Size=5;"
                                    , link.UserName, link.PassWord, link.Host, link.Port, link.ServerName);
            }
            else if (FastApp.DataDbType.SqlServer == link.Type)
            {
                connStr = string.Format("Data Source={0},{1};Initial Catalog={2};User Id={3};Password={4};pooling=true;Min Pool Size=1;Max Pool Size=5;"
                                    , link.Host, link.Port, link.ServerName, link.UserName, link.PassWord);
            }
            else if (FastApp.DataDbType.MySql == link.Type)
            {
                connStr = string.Format("server={0};port={1};Database={2};user id={3};password={4};pooling=true;Min Pool Size=1;Max Pool Size=5;CharSet=utf8;"
                                   , link.Host, link.Port, link.ServerName, link.UserName, link.PassWord);
            }

            return connStr;
        }
        #endregion

        #region 表是否存在
        /// <summary>
        /// 列不否存在
        /// </summary>
        /// <param name="db"></param>
        /// <param name="tableName"></param>
        public static bool IsExistsTable(DataContext db, string tableName)
        {
            var sql = "";

            if (db.config.DbType == FastApp.DataDbType.Oracle)
                sql = string.Format("select count(0) count from user_tables where table_name='{0}'", tableName.ToUpper());

            if (db.config.DbType == FastApp.DataDbType.SqlServer)
                sql = string.Format("select count(0) count from sysobjects where name='{0}'", tableName);

            if (db.config.DbType == FastApp.DataDbType.MySql)
                sql = string.Format("select table_name form information_schema.tables where table_name ='{0}'", tableName);

            return db.ExecuteSql(sql, null, false).DicList.First().GetValue("count").ToStr().ToInt(0) > 0;
        }
        #endregion

        #region 列是否存在
        /// <summary>
        /// 列是否存在
        /// </summary>
        /// <param name="db"></param>
        /// <param name="tableName"></param>
        /// <param name="columnName"></param>
        public static bool IsExistsColumn(DataContext db, string tableName, string columnName)
        {
            var sql = "";

            if (db.config.DbType == FastApp.DataDbType.Oracle)
                sql = string.Format("select count(0) count from user_tab_columns where table_name='{0}' and column_name='{1}'", tableName.ToUpper(), columnName.ToUpper());

            if (db.config.DbType == FastApp.DataDbType.SqlServer)
                sql = string.Format("select * from syscolumns where id = object_id('{0}') and name = '{1}'", tableName, columnName);

            if (db.config.DbType == FastApp.DataDbType.MySql)
                sql = string.Format("select count(0) count from  information_schema.columns where table_name='{0}' and column_name='{1}'", tableName, columnName);

            return db.ExecuteSql(sql, null, false).DicList.First().GetValue("count").ToStr().ToInt(0) > 0;
        }
        #endregion

        #region 获取serversql数据库版本
        /// <summary>
        /// 获取serversql数据库版本
        /// </summary>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public static int GetVersion(DataContext db)
        {
            var sql = "select SERVERPROPERTY('productversion') count";
            return db.ExecuteSql(sql, null, false).DicList.First().GetValue("count").ToStr().Split('.')[0].ToInt(0);
        }
        #endregion

        #region 取读取第一列数据
        /// <summary>
        /// 取读取第一列数据
        /// </summary>
        /// <returns></returns>
        public static List<Dictionary<string, object>> GetFirstColumnData(Dictionary<string, object> link, Base_Business_Details columnInfo, Base_Business tableInfo)
        {
            var list = new List<Dictionary<string, object>>();
            var type = link.GetValue("type").ToStr();
            var conn = link.GetValue("conn") as DbConnection;

            if (type == FastApp.DataDbType.Oracle.ToLower())
            {
                var cmd = conn.CreateCommand();
                if (string.IsNullOrEmpty(columnInfo.TableName))
                    cmd.CommandText = string.Format("select * from ({0}) where rownum <={1}", columnInfo.Sql, tableInfo.UpdateCount);
                else
                    cmd.CommandText = string.Format("select {0} a,{1} b from {2} where rownum <={3} order by {4} desc"
                                        , columnInfo.Key, columnInfo.ColumnName, columnInfo.TableName, tableInfo.UpdateCount, columnInfo.OrderBy);
                var dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    var dic = new Dictionary<string, object>();
                    dic.Add("key", dr[0]);
                    dic.Add("data", dr[1]);
                    list.Add(dic);
                }
                dr.Close();
            }

            if (type == FastApp.DataDbType.SqlServer.ToLower())
            {
                var cmd = conn.CreateCommand();
                if (string.IsNullOrEmpty(columnInfo.TableName))
                    cmd.CommandText = string.Format("select top {1} * from ({0})a", columnInfo.Sql, tableInfo.UpdateCount);
                else
                    cmd.CommandText = string.Format("select top {0} {1} a,{2} b from {3} order by {4} desc"
                            , tableInfo.UpdateCount, columnInfo.Key, columnInfo.ColumnName, columnInfo.TableName, columnInfo.OrderBy);
                var dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    var dic = new Dictionary<string, object>();
                    dic.Add("key", dr[0]);
                    dic.Add("data", dr[1]);
                    list.Add(dic);
                }
                dr.Close();
            }

            if (type == FastApp.DataDbType.MySql.ToLower())
            {
                var cmd = conn.CreateCommand();
                if (string.IsNullOrEmpty(columnInfo.TableName))
                    cmd.CommandText = string.Format("select * from ({0}) limit {1}", columnInfo.Sql, tableInfo.UpdateCount);
                else
                    cmd.CommandText = string.Format("select {0} a,{1} b from {2} where limit {3} order by {4} desc"
                                , columnInfo.Key, columnInfo.ColumnName, columnInfo.TableName, tableInfo.UpdateCount, columnInfo.OrderBy);
                var dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    var dic = new Dictionary<string, object>();
                    dic.Add("key", dr[0]);
                    dic.Add("data", dr[1]);
                    list.Add(dic);
                }

                dr.Close();
            }

            return list;
        }
        #endregion

        #region 取读取列数据第一条数据
        /// <summary>
        /// 取读取列数据第一条数据
        /// </summary>
        /// <returns></returns>
        public static object GetColumnData(Dictionary<string, object> link, Base_Business_Details columnInfo, object key)
        {
            object result = DBNull.Value;
            var type = link.GetValue("type").ToStr();
            var conn = link.GetValue("conn") as DbConnection;

            if (type == FastApp.DataDbType.Oracle.ToLower())
            {
                var cmd = conn.CreateCommand();
                cmd.CommandText = string.Format("select {0} from {1} where rownum =1 and {2}='{3}' order by {4} desc"
                                    , columnInfo.ColumnName, columnInfo.TableName, columnInfo.Key, key, columnInfo.OrderBy);
                var dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    result = dr[0];
                }
                dr.Close();
            }

            if (type == FastApp.DataDbType.SqlServer.ToLower())
            {
                var cmd = conn.CreateCommand();
                cmd.CommandText = string.Format("select top 1 {0} from {1} where {2}='{3}' order by {4} desc"
                                , columnInfo.ColumnName, columnInfo.TableName, columnInfo.Key, key, columnInfo.OrderBy);
                var dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    result = dr[0];
                }
                dr.Close();
            }

            if (type == FastApp.DataDbType.MySql.ToLower())
            {
                var cmd = conn.CreateCommand();
                cmd.CommandText = string.Format("select {0} from {1} where limit 1 and {2}={3} order by {4} desc"
                                , columnInfo.ColumnName, columnInfo.TableName, columnInfo.Key, key, columnInfo.OrderBy);
                var dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    result = dr[0];
                }

                dr.Close();
            }

            return result;
        }
        #endregion

        #region 迁移数据
        /// <summary>
        /// 迁移数据
        /// </summary>
        /// <param name="db"></param>
        /// <returns></returns>
        public static void AddList(DataContext db, DataTable dt, ref Base_Log log)
        {
            try
            {
                if (db.config.DbType == FastApp.DataDbType.SqlServer)
                {
                    if (DataSchema.GetVersion(db) >= 10)
                    {
                        #region tvps
                        using (var conn = new SqlConnection(db.config.ConnStr))
                        {
                            conn.Open();
                            var cmd = conn.CreateCommand();
                            cmd.CommandText = GetTvps(db, dt.TableName);
                            var catParam = cmd.Parameters.AddWithValue(string.Format("@{0}", dt.TableName), dt);
                            catParam.SqlDbType = SqlDbType.Structured;
                            catParam.TypeName = dt.TableName;

                            log.SuccessCount = cmd.ExecuteNonQuery();
                            if (log.SuccessCount > 0)
                                log.State = 1;
                            else
                                log.State = 0;
                            log.EndDateTime = DateTime.Now;
                            conn.Close();
                        }
                        #endregion
                    }
                    else
                    {
                        #region SqlBulkCopy
                        using (var conn = new SqlConnection(db.config.ConnStr))
                        {
                            conn.Open();
                            using (var bulk = new SqlBulkCopy(db.config.ConnStr, SqlBulkCopyOptions.UseInternalTransaction))
                            {
                                bulk.DestinationTableName = dt.TableName;
                                bulk.BatchSize = dt.Rows.Count;
                                bulk.WriteToServer(dt);
                                log.SuccessCount = dt.Rows.Count;
                                log.State = 1;
                                log.EndDateTime = DateTime.Now;
                            }
                            conn.Close();
                        }
                        #endregion
                    }
                }

                if (db.config.DbType == FastApp.DataDbType.MySql)
                {
                    #region 拼sql
                    using (var conn = new MySqlConnection(db.config.ConnStr))
                    {
                        conn.Open();
                        var cmd = conn.CreateCommand();
                        cmd.CommandText = GetMySql(dt,db);
                        log.SuccessCount = cmd.ExecuteNonQuery();
                        if (log.SuccessCount > 0)
                            log.State = 1;
                        else
                            log.State = 0;

                        log.EndDateTime = DateTime.Now;
                        conn.Close();
                    }
                    #endregion
                }

                if (db.config.DbType == FastApp.DataDbType.Oracle)
                {
                    #region odp.net特性
                    using (var conn = new OracleConnection(db.config.ConnStr))
                    {
                        conn.Open();
                        var cmd = conn.CreateCommand();

                        //关闭日志
                        cmd.CommandText = string.Format("alter table {0} nologging", dt.TableName);
                        cmd.ExecuteNonQuery();

                        cmd.ArrayBindCount = dt.Rows.Count;
                        cmd.BindByName = true;
                        GetOdpParam(dt, ref cmd, db, dt.TableName);

                        log.SuccessCount = cmd.ExecuteNonQuery();
                        if (log.SuccessCount > 0)
                            log.State = 1;
                        else
                            log.State = 0;

                        log.EndDateTime = DateTime.Now;

                        //开起日志
                        cmd.CommandText = string.Format("alter table {0} logging", dt.TableName);
                        cmd.ExecuteNonQuery();
                        conn.Close();
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                log.EndDateTime = DateTime.Now;
                log.SuccessCount = 0;
                log.State = 0;
                log.ErrorMsg = ex.StackTrace;
            }
        }
        #endregion

        #region 过期数据
        /// <summary>
        /// 过期数据
        /// </summary>
        public static void ExpireData(DataContext db, Base_Business item)
        {
            var month = item.SaveDataMonth.ToStr().ToInt(0) * -1;
            var sql = string.Format("delete from {0} where addtime<='{1}'", item.TableName, DateTime.Now.AddMonths(month));
            
            db.ExecuteSql(sql, null, true);
        }
        #endregion

        #region 重复数据
        /// <summary>
        /// 重复数据
        /// </summary>
        public static void RepeatData(DataContext db, Base_Business item, object key)
        {
            if (item.IsDel == "1")
            {
                var sql = string.Format("delete from {0} where key='{1}'", item.TableName, key);
                db.ExecuteSql(sql, null, true);
            }
        }
        #endregion

        #region 获取表datatable
        /// <summary>
        /// 获取表datatable
        /// </summary>
        public static DataTable GetTable(DataContext db, string table)
        {
            var dt = new DataTable();
            if (db.config.DbType == FastApp.DataDbType.Oracle)
            {
                using (var conn = DbProviderFactories.GetFactory(FastApp.Provider.Oracle).CreateConnection())
                {
                    conn.ConnectionString = db.config.ConnStr;
                    conn.Open();
                    var cmd = conn.CreateCommand();
                    cmd.CommandText = string.Format("select * from {0} where rownum <=1", table);
                    dt.Load(cmd.ExecuteReader());
                }
            }

            if (db.config.DbType == FastApp.DataDbType.MySql)
            {
                using (var conn = DbProviderFactories.GetFactory(FastApp.Provider.MySql).CreateConnection())
                {
                    conn.ConnectionString = db.config.ConnStr;
                    conn.Open();
                    var cmd = conn.CreateCommand();
                    cmd.CommandText = string.Format("select * from {0} where limit 1", table);
                    dt.Load(cmd.ExecuteReader());
                }
            }

            if (db.config.DbType == FastApp.DataDbType.SqlServer)
            {
                using (var conn = DbProviderFactories.GetFactory(FastApp.Provider.SqlServer).CreateConnection())
                {
                    conn.ConnectionString = db.config.ConnStr;
                    conn.Open();
                    var cmd = conn.CreateCommand();
                    cmd.CommandText = string.Format("select top 1 * from {0}", table);
                    dt.Load(cmd.ExecuteReader());
                }
            }

            dt.Clear();
            return dt;
        }
        #endregion

        #region 获取mysql批量插入sql
        /// <summary>
        /// 获取mysql批量插入sql
        /// </summary>
        /// <returns></returns>
        private static string GetMySql(DataTable dt, DataContext db)
        {
            var sql = new StringBuilder();
            sql.AppendFormat("insert into {0}(", dt.TableName);
            var list = DataSchema.ColumnList(db, dt.TableName);
            foreach (var item in list)
            {
                sql.AppendFormat("{0},",item.GetValue("name"));
            }
            sql.Append(")").Replace(",)", ")");
            sql.Append("values");

            foreach(DataRow dr in dt.Rows)
            {
                sql.Append("(");
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    sql.AppendFormat("'{0}',", dr[i]);
                }
                sql.Append("),").Replace(",)", ")");
            }

            return sql.ToStr().Substring(0, sql.ToStr().Length - 1);
        }
        #endregion

        #region odp.net 特性 获取sql
        /// <summary>
        ///  odp.net 特性 获取sql
        /// </summary>
        private static void GetOdpParam(DataTable dt, ref OracleCommand cmd, DataContext db, string tableName)
        {
            var i = 0;
            var sql = new StringBuilder();
            var list = DataSchema.ColumnList(db, tableName);

            sql.AppendFormat("insert into {0} values(", tableName);

            foreach (var item in list)
            {
                sql.AppendFormat(":{0},", item.GetValue("name"));
                var param = new OracleParameter(item.GetValue("name").ToStr(), GetOracleDbType(item.GetValue("type").ToStr()));
                param.Direction = ParameterDirection.Input;
                object[] pValue = new object[dt.Rows.Count];

                for (int j = 0; j < dt.Rows.Count; j++)
                {
                    var itemValue = dt.Rows[j][i];

                    if (itemValue == null)
                        itemValue = DBNull.Value;

                    pValue[j] = itemValue;
                }

                param.Value = pValue;
                cmd.Parameters.Add(param);
                i++;
            }
            sql.Append(")");

            cmd.CommandText = sql.ToString().Replace(",)", ")");
        }
        #endregion

        #region 获取tvps语句
        /// <summary>
        /// 获取tvps语句
        /// </summary>
        /// <returns></returns>
        private static string GetTvps(DataContext db, string tableName)
        {
            var sql1 = new StringBuilder();
            var sql2 = new StringBuilder();
            var list = DataSchema.ColumnList(db, tableName);

            sql1.AppendFormat("insert into {0} (", tableName);
            sql2.Append("select ");
            foreach (var item in list)
            {
                sql1.AppendFormat("{0},", item.GetValue("name").ToStr());
                sql2.AppendFormat("tb.{0},", item.GetValue("name").ToStr());
            }
            sql1.Append(")");
            sql2.AppendFormat("from @{0} as tb", tableName);

            return string.Format("{0}{1}", sql1.ToString().Replace(",)", ") "), sql2.ToString().Replace(",from", " from"));
        }
        #endregion

        #region 获取列类型oracle
        /// <summary>
        /// 获取列类型oracle
        /// </summary>
        /// <param name="list"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        private static OracleDbType GetOracleDbType(string colType)
        {
            switch (colType.ToUpper().Trim())
            {
                case "BFILE":
                    return OracleDbType.BFile;
                case "REAL":
                    return OracleDbType.Double;
                case "LONG":
                    return OracleDbType.Long;
                case "DATE":
                    return OracleDbType.Date;
                case "NUMBER":
                    return OracleDbType.Decimal;
                case "VARCHAR2":
                    return OracleDbType.Varchar2;
                case "NVARCHAR2":
                    return OracleDbType.NVarchar2;
                case "RAW":
                    return OracleDbType.Raw;
                case "DECIMAL":
                    return OracleDbType.Decimal;
                case "INTEGER":
                    return OracleDbType.Int32;
                case "CHAR":
                    return OracleDbType.Char;
                case "NCHAR":
                    return OracleDbType.NChar;
                case "FLOAT":
                    return OracleDbType.Double;
                case "BLOB":
                    return OracleDbType.Blob;
                case "CLOB":
                    return OracleDbType.Clob;
                case "NCLOB":
                    return OracleDbType.NClob;
                default:
                    return OracleDbType.NVarchar2;
            }
        }
        #endregion

        #region 获取列名和类型
        /// <summary>
        /// 获取列名和类型
        /// </summary>
        /// <returns></returns>
        private static List<Dictionary<string, object>> ColumnList(DataContext db, string tableName)
        {
            var sql = string.Format("select a.COLUMN_NAME name,a.DATA_TYPE type from user_tab_columns a where table_name='{0}'", tableName);
            return db.ExecuteSql(sql, null, false).DicList;
        }
        #endregion

        #region 初始化取数据长连接
        /// <summary>
        /// 初始化取数据长连接
        /// </summary>
        /// <returns></returns>
        public static List<Dictionary<string, object>> InitColLink(List<Base_Business_Details> list, DataContext db)
        {
            var result = new List<Dictionary<string, object>>();
            foreach (var item in list)
            {
                var link = FastRead.Query<Base_DataSource>(a => a.Id == item.DataSourceId).ToItem<Base_DataSource>(db);

                if (link.Type.ToLower() == FastApp.DataDbType.SqlServer.ToLower())
                {
                    var dic = new Dictionary<string, object>();
                    var conn = DbProviderFactories.GetFactory(FastApp.Provider.SqlServer).CreateConnection();
                    conn.ConnectionString = GetConnStr(link);
                    conn.Open();
                    dic.Add("conn", conn);
                    dic.Add("type", FastApp.DataDbType.SqlServer.ToLower());
                    result.Add(dic);
                }
                if (link.Type.ToLower() == FastApp.DataDbType.MySql.ToLower())
                {
                    var dic = new Dictionary<string, object>();
                    var conn = DbProviderFactories.GetFactory(FastApp.Provider.MySql).CreateConnection();
                    conn.ConnectionString = GetConnStr(link);
                    conn.Open();
                    dic.Add("conn", conn);
                    dic.Add("type", FastApp.DataDbType.MySql.ToLower());
                    result.Add(dic);
                }

                if (link.Type.ToLower() == FastApp.DataDbType.Oracle.ToLower())
                {
                    var dic = new Dictionary<string, object>();
                    var conn = DbProviderFactories.GetFactory(FastApp.Provider.Oracle).CreateConnection();
                    conn.ConnectionString = GetConnStr(link);
                    conn.Open();
                    dic.Add("conn", conn);
                    dic.Add("type", FastApp.DataDbType.Oracle.ToLower());
                    result.Add(dic);
                }
            }
            return result;
        }
        #endregion

        #region 关闭长连接
        /// <summary>
        /// 关闭长连接
        /// </summary>
        /// <returns></returns>
        public static void CloseLink(List<Dictionary<string, object>> link)
        {
            foreach (var item in link)
            {
                var conn = item.GetValue("conn") as DbConnection;
                conn.Close();
                conn.Dispose();
            }
        }
        #endregion
    }
}
