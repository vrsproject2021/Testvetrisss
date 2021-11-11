using Vetris.Report.Core.Dependency;
using System;
using System.Data;
using System.Data.SqlClient;
using SqlKata.Execution;

namespace Vetris.Report.DataAccess
{
    public interface IDatabaseContext: ITransientDependency
    {
        string error { get; }
        string errorCode { get; }
        Exception Exception { get; }
        void BeginTransaction();
        bool OpenConnection();
        bool CloseConnection();
        void Commit();
        void Rollback();
        void Dispose();
        IDbTransaction Transaction { get; }
        DatabaseContext WithConnectionString(string connectionstring);
        DatabaseContext SQL(string sql);
        DatabaseContext SQL(string sql, CommandType type);
        DatabaseContext StoredProcedure(string name);
        DatabaseContext AddParameter(string name, Object value);
        DatabaseContext AddParameter(string name, Object value, SqlDbType type);
        DatabaseContext AddParameter(string name, Object value, SqlDbType type, int size);
        DatabaseContext AddParameter(string name, Object value, SqlDbType type, ParameterDirection direction);
        DatabaseContext AddParameter(string name, Object value, SqlDbType type, int size, ParameterDirection direction);
        int ExecuteNonQuery(Action<SqlCommand> action = null);
        DataSet ExecuteDataSet(Action<SqlCommand> action = null);
        SqlDataReader ExecuteDataReader(Action<SqlCommand> action = null);
        QueryFactory QueryFactory();
    }
}