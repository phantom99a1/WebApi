using Oracle.ManagedDataAccess.Client;
using SM.Training.SharedComponent.Constants;
using SM.Training.Utils;
using SoftMart.Core.Dao;
using SoftMart.Kernel.Entity;
using SoftMart.Kernel.Exceptions;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace SM.Training.Common
{
    public abstract class BaseDao
    {
        protected string BuildLikeFilter(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
                return null;
            return string.Format("%{0}%", keyword.Trim());
        }

        protected string BuildInCondition(List<int> lstValue)
        {
            if (lstValue == null || lstValue.Count == 0)
            {
                return "null";
            }
            else
            {
                return string.Join(", ", lstValue.ToArray());
            }
        }

        protected string BuildInCondition(List<decimal> lstValue)
        {
            if (lstValue.Count == 0)
            {
                return "null";
            }
            else
            {
                return string.Join(", ", lstValue.ToArray());
            }
        }

        protected string BuildInCondition(List<string> lstValue)
        {
            if (lstValue.Count == 0)
            {
                return "null";
            }
            else
            {
                string result = string.Empty;
                string separator = string.Empty;

                foreach (string item in lstValue)
                {
                    if (!string.IsNullOrWhiteSpace(item))
                    {
                        result += separator + "'" + item.Trim().Replace("'", "''") + "'";
                        separator = ",";
                    }
                }
                return result;
            }
        }

        /// <summary>
        /// Execute paging with RowNumber Mode
        /// </summary>
        protected List<T> ExecutePaging<T>(OracleCommand command, PagingInfo pagingInfo, bool countingRecordCount = false)
        {
            using (var dataContext = new DataContext())
            {
                return ExecutePaging<T>(dataContext, command, pagingInfo, countingRecordCount);
            }
        }

        protected List<T> ExecutePaging<T>(string query, PagingInfo pagingInfo, bool countingRecordCount = false)
        {
            OracleCommand sqlCmd = new OracleCommand(query);

            return ExecutePaging<T>(sqlCmd, pagingInfo, countingRecordCount);
        }

        protected List<T> ExecutePaging<T>(DataContext dataContext, OracleCommand command, PagingInfo pagingInfo, bool countingRecordCount = false)
        {
            int recordCord;
            List<T> lst;

            if (ConfigUtils.PagingMode == SoftMart.Core.Dao.SqlPagingMode.RowNumber)
            {
                lst = dataContext.ExecutePaging<T>(command, pagingInfo.PageIndex, pagingInfo.PageSize, out recordCord, countingRecordCount);
            }
            else
            {
                lst = dataContext.ExecuteOffset<T>(command, pagingInfo.PageIndex, pagingInfo.PageSize, out recordCord, countingRecordCount);
            }

            if (countingRecordCount)
            {
                pagingInfo.RecordCount = recordCord;
            }
            return lst;
        }

        protected List<T> ExecutePaging<T>(DataContext dataContext, string query, PagingInfo pagingInfo, bool countingRecordCount = false)
        {
            OracleCommand sqlCmd = new OracleCommand(query);

            return ExecutePaging<T>(dataContext, sqlCmd, pagingInfo, countingRecordCount);
        }

        public void InsertItem<T>(T item) where T : BaseEntity
        {
            using (DataContext context = new DataContext())
            {
                context.InsertItem<T>(item);
            }
        }

        public void InsertItems<T>(List<T> lstItem, int batchSize = 0) where T : BaseEntity
        {
            using (DataContext context = new DataContext())
            {
                foreach (var item in lstItem)
                {
                    context.InsertItem<T>(item);
                }
            }
        }

        public int UpdateItem<T>(T item) where T : BaseEntity
        {
            using (DataContext context = new DataContext())
            {
                int affectedRow = context.UpdateItem<T>(item);
                if (affectedRow == 0)
                {
                    throw new SMXException(Messages.ItemNotExitOrChanged);
                }

                return affectedRow;
            }
        }

        public void UpdateItems<T>(List<T> lstItem, string[] columns, bool throwExceptionIfUpdateFail = false) where T : BaseEntity
        {
            if (lstItem == null || lstItem.Count == 0)
                return;

            using (DataContext context = new DataContext())
            {
                int affectedRow = context.UpdateItems<T>(lstItem, columns);

                if (throwExceptionIfUpdateFail && affectedRow != lstItem.Count)
                {
                    throw new SMXException(Messages.ItemNotExitOrChanged);
                }
            }
        }

        public T GetItemByID<T>(object id) where T : class
        {
            using (DataContext context = new DataContext())
            {
                return context.SelectItemByID<T>(id);
            }
        }

        public T GetItemByID<T>(object id, string[] columns) where T : BaseEntity
        {
            if (id == null)
                return null;

            using (DataContext context = new DataContext())
            {
                return context.SelectFieldsByID<T>(columns, id);
            }
        }

        public List<T> GetItemsByColumn<T>(string columnName, int value) where T : BaseEntity
        {
            using (DataContext context = new DataContext())
            {
                return context.SelectItemByColumnName<T>(columnName, value);
            }
        }

        public void DeleteItemByColumn<T>(string columnName, int value) where T : BaseEntity
        {
            using (DataContext context = new DataContext())
            {
                context.DeleteItemByColumn<T>(columnName, value);
            }
        }

        public List<T> SelectItemByColumnName<T>(string columnName, object value) where T : SoftMart.Core.Dao.BaseEntity
        {
            using (DataContext dataContext = new DataContext())
            {
                return dataContext.SelectItemByColumnName<T>(columnName, value);
            }
        }

        public int RemoveById<T>(string name, object id) where T : BaseEntity
        {
            using (DataContext context = new DataContext())
            {
                return context.DeleteItemByColumn<T>(name, id);
            }
        }

        #region StoreProcedure

        protected int ExecuteStore(string storeName, int? timeoutInSecond, params object[] arrParam)
        {
            string query = string.Format("begin {0}", storeName);

            OracleCommand sqlCmd = new OracleCommand();
            if (arrParam != null)
            {
                if (arrParam.Length > 0)
                    query += "(";

                string delimieter = "";
                for (int index = 0; index < arrParam.Length; index++)
                {
                    string paramName = string.Format(":Param{0}", index);
                    query = query + delimieter + paramName;
                    sqlCmd.Parameters.Add(paramName, arrParam[index]);
                    delimieter = ", ";
                }

                if (arrParam.Length > 0)
                    query += ")";
            }

            query += "; end;";

            sqlCmd.CommandText = query;
            int timeout = (timeoutInSecond == null || timeoutInSecond.Value <= 0) ? 30 : timeoutInSecond.Value;

            using (DataContext context = new DataContext(timeout))
            {
                sqlCmd.CommandTimeout = timeout;
                int effectedItem = context.ExecuteNonQuery(sqlCmd);

                return effectedItem;
            }
        }

        protected DataTable ExecuteStoreDataTable(string storeName, int? timeoutInSecond, params object[] arrParam)
        {
            string query = string.Format("begin {0}", storeName);

            OracleCommand sqlCmd = new OracleCommand();
            query += "(";
            string delimieter = "";
            if (arrParam != null)
            {
                for (int index = 0; index < arrParam.Length; index++)
                {
                    string paramName = string.Format(":Param{0}", index);
                    query = query + delimieter + paramName;
                    sqlCmd.Parameters.Add(paramName, arrParam[index]);
                    delimieter = ", ";
                }
            }

            query = query + delimieter + ":C_table_result";
            sqlCmd.Parameters.Add(":C_table_result", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

            query += "); end;";

            sqlCmd.CommandText = query;
            int timeout = (timeoutInSecond == null || timeoutInSecond.Value <= 0) ? 30 : timeoutInSecond.Value;

            using (DataContext context = new DataContext(timeout))
            {
                sqlCmd.CommandTimeout = timeout;
                return context.ExecuteDataTable(sqlCmd);
            }
        }

        #endregion

        public System.Data.DataTable GetQueryData(string query, Dictionary<string, object> dicQueryParam)
        {
            if (string.IsNullOrWhiteSpace(query))
                return null;

            OracleCommand sqlCmd = new OracleCommand(query);
            if (dicQueryParam != null)
            {
                foreach (KeyValuePair<string, object> objParam in dicQueryParam)
                    sqlCmd.Parameters.AddWithValue(objParam.Key, objParam.Value);
                sqlCmd.Parameters.Add(":C_table_result", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
            }
            using (DataContext context = new DataContext())
            {
                return context.ExecuteDataTable(sqlCmd);
            }
        }

        protected void ExecuteStore(string storeName, Dictionary<string, object> arrParam = null)
        {
            OracleCommand cmd = new OracleCommand(storeName);
            cmd.CommandType = CommandType.StoredProcedure;

            if (arrParam != null)
            {
                foreach (KeyValuePair<string, object> entry in arrParam)
                    cmd.Parameters.AddWithValue(entry.Key, entry.Value);
            }

            using (DataContext context = new DataContext())
            {
                context.ExecuteNonQuery(cmd);
            }
        }

        /// <summary>
        /// Exec thủ tục trả về list
        /// </summary>
        /// <typeparam name="T">Kiểu object</typeparam>
        /// <param name="storeName">Tên thủ tục</param>
        /// <param name="arrParam">Tham số đầu vào</param>
        /// <param name="outputParam">Tham số đầu ra</param>
        /// <returns></returns>
        protected List<T> ExecuteStoreResult<T>(string storeName, string outputParam, Dictionary<string, object> arrParam = null) where T : BaseEntity
        {
            OracleCommand cmd = new OracleCommand(storeName);
            cmd.CommandType = CommandType.StoredProcedure;

            if (arrParam != null)
            {
                foreach (KeyValuePair<string, object> entry in arrParam)
                    cmd.Parameters.AddWithValue(entry.Key, entry.Value);
            }
            cmd.Parameters.Add(outputParam, OracleDbType.RefCursor).Direction = ParameterDirection.Output;

            using (DataContext context = new DataContext())
            {
                var rs = context.ExecuteSelect<T>(cmd);
                return rs ?? new List<T>();
            }
        }

        protected DataTable ExecuteStoreDataTable(string storeName, string outputParam, Dictionary<string, object> arrParam, int timeoutInSecond)
        {
            OracleCommand cmd = new OracleCommand(storeName);
            cmd.CommandType = CommandType.StoredProcedure;

            if (arrParam != null)
            {
                foreach (KeyValuePair<string, object> entry in arrParam)
                    cmd.Parameters.AddWithValue(entry.Key, entry.Value);
            }
            cmd.Parameters.Add(outputParam, OracleDbType.RefCursor).Direction = ParameterDirection.Output;

            using (DataContext context = new DataContext(timeoutInSecond))
            {
                return context.ExecuteDataTable(cmd);
            }
        }

        protected DataTable ExecuteStoreGetDataTable(string storeName, params object[] arrParam)
        {
            //Utils.LogManager.WebLogger.LogDebug(string.Format("Start {0}", storeName));

            string query = string.Format("exec {0} ", storeName);
            OracleCommand sqlCmd = new OracleCommand();
            if (arrParam != null)
            {
                string delimieter = "";
                for (int index = 0; index < arrParam.Length; index++)
                {
                    string paramName = string.Format(":param{0}", index);
                    query = query + delimieter + paramName;
                    sqlCmd.Parameters.AddWithValue(paramName, arrParam[index]);
                    delimieter = ", ";
                }
            }
            sqlCmd.CommandText = query;

            DataTable table = null;
            using (DataContext context = new DataContext())
            {
                table = context.ExecuteDataTable(sqlCmd);
            }

            int count = table == null ? 0 : table.Rows.Count;
            //Utils.LogManager.WebLogger.LogDebug(string.Format("End {0}. ItemCount: {1}", storeName, count));

            return table;
        }

        public DataTable ExecuteDataTable(OracleCommand command)
        {
            using (DataContext context = new DataContext())
            {
                return context.ExecuteDataTable(command);
            }
        }

        protected List<T> ExecuteStoreResultPaging<T>(PagingInfo pagingInfo, string storeName, string outputParam, Dictionary<string, object> arrParam = null)
        {
            OracleCommand cmd = new OracleCommand(storeName);
            cmd.CommandType = CommandType.StoredProcedure;

            if (arrParam != null)
            {
                foreach (KeyValuePair<string, object> entry in arrParam)
                    cmd.Parameters.AddWithValue(entry.Key, entry.Value);
            }
            cmd.Parameters.Add(outputParam, OracleDbType.RefCursor).Direction = ParameterDirection.Output;

            using (DataContext context = new DataContext())
            {
                List<T> lst = context.ExecuteSelect<T>(cmd);
                pagingInfo.RecordCount = lst.Count;
                lst = lst.Skip(pagingInfo.RowsSkip).Take(pagingInfo.PageSize).ToList();

                return lst;
            }
        }
    }
}
