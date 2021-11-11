using Vetris.Report.Core;
using Vetris.Report.Core.Configurations;
using Vetris.Report.Core.Extensions;
using Vetris.Report.DataAccess.Uow;
using System;
using System.Data;
using System.Data.SqlClient;
using SqlKata.Compilers;
using SqlKata.Execution;

namespace Vetris.Report.DataAccess
{
    public class DatabaseContext : IDisposable, IDatabaseContext
    {
        public event EventHandler Commiteded;
        public event EventHandler<UnitOfWorkFailedEventArgs> Failed;

        private SqlConnection _connection = null;
        private SqlCommand _command = null;
        private SqlTransaction _transaction = null;
        private bool _internalOpen;
        private bool _transactionBegan;
        private int timeout = 90000;
        public string error { get; set; }
        public string errorCode { get; set; }
        private string connectionString { get; set; }

        private bool _isCommitCalledBefore;
        private bool _succeed;
        private Exception _exception;
        private readonly IConfigurationAccessor _configurationAccessor;

        public Exception Exception => _exception;

        public DatabaseContext(IConfigurationAccessor configurationAccessor)
        {
            _configurationAccessor = configurationAccessor;
        }
        public IDbTransaction Transaction { get { return _transaction; } }
        public void Dispose()
        {

            if (_transactionBegan && !_isCommitCalledBefore)
            {
                Commit();
            }

            if (_command != null)
            {
                if (_command.Parameters != null) _command.Parameters.Clear();
                _command.Dispose();
            }

            if (_connection != null)
            {
                _connection.Close();
                _connection.Dispose();
            }
        }

        /// <summary>
        /// Called to trigger <see cref="Commiteded"/> event.
        /// </summary>
        protected virtual void OnCommiteded()
        {
            Commiteded.InvokeSafely(this);
        }

        /// <summary>
        /// Called to trigger <see cref="Failed"/> event.
        /// </summary>
        /// <param name="exception">Exception that cause failure</param>
        protected virtual void OnFailed(Exception exception)
        {
            Failed.InvokeSafely(this, new UnitOfWorkFailedEventArgs(exception));
        }


        public void BeginTransaction()
        {
            PreventMultipleBegin();
            _succeed = false;
            _isCommitCalledBefore = false;
            if (_internalOpen)
            {
                if (_connection == null) OpenConnection();
                if (_connection.State != ConnectionState.Open) _connection.Open();
                _transaction = _connection.BeginTransaction();
               
            }
            else
            {
                OpenConnection();
                 _transaction = _connection.BeginTransaction();
                
            }
        }
        public void Commit()
        {

            if (!_transactionBegan)
                return;
            try
            {

                PreventMultipleCommit();
                _transaction.Commit();
                _succeed = true;
                OnCommiteded();

            }
            catch (SqlException DbEx)
            {
                _exception = DbEx;
                if (!_succeed)
                {
                    OnFailed(_exception);
                }
            }
            catch (Exception ex)
            {
                _exception = ex;
                if (!_succeed)
                {
                    OnFailed(_exception);
                }
            }
            finally
            {
                _transactionBegan = false;
                if (_transaction != null)
                    _transaction.Dispose();
                _transaction = null;
                _succeed = false;
            }
        }

        public void Rollback()
        {
            if (!_transactionBegan)
                return;
            try
            {
                _transaction.Rollback();

            }
            catch (SqlException DbEx)
            {
                _exception = DbEx;
                throw;
            }
            catch (Exception ex)
            {
                _exception = ex;
                throw;
            }
            finally
            {
                _transactionBegan = false;
                _isCommitCalledBefore = false;
                _succeed = false;
                if (_transaction != null)
                    _transaction.Dispose();
                _transaction = null;
            }
        }

        private void PreventMultipleBegin()
        {
            if (_transactionBegan)
            {
                throw new Exception("This transaction has started before. Can not call BeginTransaction method more than once.");
            }

            _transactionBegan = true;
        }
        private void PreventMultipleCommit()
        {
            if (_isCommitCalledBefore)
            {
                throw new Exception("Commit is called before!");
            }

            _isCommitCalledBefore = true;
        }
        public bool OpenConnection()
        {
            if (!_internalOpen)
            {
                if (_connection == null)
                {
                    if (connectionString.IsNullOrEmpty()) 
                        connectionString = _configurationAccessor.ConnectionString;
                    _connection = new SqlConnection(connectionString);
                }
                if (_connection.ConnectionString.IsNullOrEmpty())
                    throw new Exception("Connection string was not set!");

                if (_connection.State == ConnectionState.Closed)
                {
                    _connection.Open();

                    _internalOpen = true;
                }
            }
            else
            {
                if (_connection == null) _connection = new SqlConnection(_configurationAccessor.ConnectionString);
                if (_connection.State == ConnectionState.Closed)
                {
                    _connection.Open();
                }
            }
            return _internalOpen;
        }

        public bool CloseConnection()
        {
            if (_internalOpen)
            {
                if (_connection.State != ConnectionState.Closed)
                {
                    if (_transactionBegan)
                    {
                        Commit();
                    }
                    _connection.Close();

                    _internalOpen = false;
                }
            }
            else
            {
                if (_connection.State != ConnectionState.Closed)
                {
                    if (_transactionBegan)
                    {
                        Commit();
                    }
                    _connection.Close();
                }
            }
            return true;
        }


        public DatabaseContext WithConnectionString(string connectionstring)
        {
            connectionString = connectionString;
            return this;
        }
        public DatabaseContext SQL(string sql)
        {
            _command = new SqlCommand(sql);
            _command.CommandType = System.Data.CommandType.Text;
            return this;
        }
        public DatabaseContext SQL(string sql, CommandType type)
        {
            _command = new SqlCommand(sql);
            _command.CommandType = type;
            return this;
        }
        public DatabaseContext StoredProcedure(string name)
        {
            _command = new SqlCommand(name);
            _command.CommandType = System.Data.CommandType.StoredProcedure;
            return this;
        }

        public DatabaseContext AddParameter(string name, Object value, SqlDbType type, int size)
        {
            if (_command == null) throw new Exception("SQL/Stored procedure not set");
            var param = _command.Parameters.Add(name, type, size);
            if (value == null)
                param.Value = DBNull.Value;
            else
                param.Value = value;
            return this;
        }
        public DatabaseContext AddParameter(string name, Object value, SqlDbType type)
        {
            if (_command == null) throw new Exception("SQL/Stored procedure not set");
            var param = _command.Parameters.Add(name, type);
            if (value == null)
                param.Value = DBNull.Value;
            else
                param.Value = value;
            return this;
        }
        public DatabaseContext AddParameter(string name, Object value)
        {
            if (_command == null) throw new Exception("SQL/Stored procedure not set");
            if (value == null)
                _command.Parameters.AddWithValue(name, DBNull.Value);
            else
                _command.Parameters.AddWithValue(name, value);
            return this;
        }
        public DatabaseContext AddParameter(string name, Object value, SqlDbType type, ParameterDirection direction)
        {
            if (_command == null) throw new Exception("SQL/Stored procedure not set");
            var param = _command.Parameters.Add(name, type);
            if (value == null)
                param.Value = DBNull.Value;
            else
                param.Value = value;
            param.Direction = direction;
            return this;
        }
        public DatabaseContext AddParameter(string name, Object value, SqlDbType type, int size, ParameterDirection direction)
        {
            if (_command == null) throw new Exception("SQL/Stored procedure not set");
            var param = _command.Parameters.Add(name, type, size);
            if (value == null)
                param.Value = DBNull.Value;
            else
                param.Value = value;
            param.Direction = direction;
            return this;
        }


        public int ExecuteNonQuery(Action<SqlCommand> action = null)
        {
            var result = -1;
            try
            {
                if (_command == null) throw new Exception("SQL/Stored procedure not set");
                OpenConnection();
                _command.CommandTimeout = timeout;
                _command.Connection = _connection;
                result = _command.ExecuteNonQuery();
                // output parameter retrieval
                if (action != null)
                    action(_command);

                _command.Parameters.Clear();
            }
            catch (SqlException DbEx)
            {
                throw DbEx;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (_command != null)
                {
                    _command.Dispose();
                }
                _command = null;
            }
            return result;
        }

        public DataSet ExecuteDataSet(Action<SqlCommand> action = null)
        {
            DataSet result = null;
            try
            {
                if (_command == null) throw new Exception("SQL/Stored procedure not set");
                OpenConnection();
                _command.CommandTimeout = timeout;
                _command.Connection = _connection;
                SqlDataAdapter da = new SqlDataAdapter();
                result = new DataSet();
                da.SelectCommand = _command;
                var retval = da.Fill(result);
                // output parameter retrieval
                if (action != null)
                    action(_command);

                _command.Parameters.Clear();
            }
            catch (SqlException DbEx)
            {
                throw DbEx;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (_command != null)
                {
                    _command.Dispose();
                }
                _command = null;
            }
            return result;
        }

        public SqlDataReader ExecuteDataReader(Action<SqlCommand> action = null)
        {

            try
            {
                if (_command == null) throw new Exception("SQL/Stored procedure not set");
                OpenConnection();
                _command.CommandTimeout = timeout;
                _command.Connection = _connection;
                var dr = _command.ExecuteReader();
                // output parameter retrieval
                if (action != null)
                    action(_command);

                _command.Parameters.Clear();
                return dr;
            }
            catch (SqlException DbEx)
            {
                throw DbEx;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (_command != null)
                {
                    _command.Dispose();
                }
                _command = null;
            }
        }

        public QueryFactory QueryFactory()
        {
            var compiler = new SqlServerCompiler();
            var db = new QueryFactory(_connection, compiler);

            return db;

        }
    }
}