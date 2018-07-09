using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;
using Dapper.Contrib.Extensions;


namespace SV.Infrastructure
{
    public class SqlDbRepository<TEntity> : IDisposable, IDbRepository<TEntity> where TEntity : BaseEntity
    {
        private bool disposed;
        private readonly IDbConnection _connection;
        private IDbTransaction _transaction;

        public SqlDbRepository(string connectionString)
        {
            _connection = new SqlConnection(connectionString);
        }

        public IDbConnection Connection
        {
            get
            {
                if (disposed) throw new ObjectDisposedException(GetType().Name);

                if (_connection.State != ConnectionState.Open)
                    _connection.Open();

                return _connection;
            }
        }

        public void BeginTransaction()
        {
            if (_connection.State != ConnectionState.Open)
                _connection.Open();

            _transaction = _connection.BeginTransaction();
        }

        public void FinishTransaction()
        {
            if (_transaction != null)
            {
                _transaction.Commit();
            }
        }

        public void ExitTransaction()
        {
            if (_transaction != null)
            {
                _transaction.Rollback();
            }
        }

        public void Dispose()
        {
            if (disposed) return;

            if (_transaction != null)
            {
                _transaction.Dispose();
            }

            if (_connection != null)
            {
                _connection.Dispose();
            }

            disposed = true;
        }

        public async Task<long> AddAsync(TEntity item)
        {
            long id = 0;
            if (_transaction != null)
            {
                id = await Connection.InsertAsync(item, transaction: _transaction, commandTimeout: 120).ConfigureAwait(false);
            }
            else
            {
                id = await Connection.InsertAsync(item, commandTimeout: 120).ConfigureAwait(false);
            }

            if (id <= 0)
                throw new DataException("Entity not inserted.");

            return id;
        }

        public async Task<bool> UpdateAsync(TEntity item)
        {
            bool updated;
            if (_transaction != null)
            {
                updated = await Connection.UpdateAsync(item, transaction: _transaction, commandTimeout: 120).ConfigureAwait(false);
            }
            else
            {
                updated = await Connection.UpdateAsync(item, commandTimeout: 120).ConfigureAwait(false);
            }

            return updated;
        }

        public async Task<bool> DeleteAsync(TEntity item)
        {
            bool deleted;
            if (_transaction != null)
            {
                deleted = await Connection.DeleteAsync(item, transaction: _transaction, commandTimeout: 120).ConfigureAwait(false);
            }
            else
            {
                deleted = await Connection.DeleteAsync(item, commandTimeout: 120).ConfigureAwait(false);
            }

            return deleted;
        }

        public Task<T> ExecuteSingleResultSPAsync<T>(string query, object param)
        {
            if (_transaction != null)
            {
                return Connection.ExecuteScalarAsync<T>(query, param, commandType: CommandType.StoredProcedure, transaction: _transaction);
            }
            else
            {
                return Connection.ExecuteScalarAsync<T>(query, param, commandType: CommandType.StoredProcedure);
            }
        }

        public Task<int> ExecuteSPAsync(string query, object param)
        {
            return ExecuteAsync(query, param, CommandType.StoredProcedure);
        }

        public Task<int> ExecuteAsync(string query, object param)
        {
            return ExecuteAsync(query, param, CommandType.Text);
        }

        private Task<int> ExecuteAsync(string query, object param, CommandType commandType)
        {
            if (_transaction != null)
            {
                return Connection.ExecuteAsync(query, param, commandType: commandType, transaction: _transaction);
            }
            else
            {
                return Connection.ExecuteAsync(query, param, commandType: commandType);
            }

        }

        public Task<IEnumerable<T>> ExecuteQueryAsync<T>(string query, object param)
        {
            return Connection.QueryAsync<T>(query, param);
        }

        public Task<T> QuerySingleOrDefaultAsync<T>(string query, object param)
        {
            return Connection.QuerySingleOrDefaultAsync<T>(query, param);
        }

        public Task<IEnumerable<T>> ExecuteSPQueryAsync<T>(string query, object param, CommandType commandType)
        {
            return Connection.QueryAsync<T>(query, param, commandType: commandType);
        }
    }
}
