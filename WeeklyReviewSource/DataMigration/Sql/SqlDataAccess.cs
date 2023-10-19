using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace DataMigration.Sql
{
    public enum StoreProcedure
    {
        // Record Time
        spRecordTime_Create,
        spRecordTime_Get,
        spRecordTime_GetBetween,
        spRecordTime_GetAfter,
        spRecordTime_GetBefore,
        spRecordTime_Update,
        spRecordTime_Delete,

        // Record
        spRecord_Create,
        spRecord_GetAll,
        spRecord_GetAt,
        spRecord_DeleteAt,

        // Activity
        spActivity_Create,
        spActivity_Get,
        spActivity_GetAll,
        spActivity_GetByNormName,
        spActivity_Update,

        // Category
        spCategory_Create,
        spCategory_Update,
        spCategory_Get,
        spCategory_GetAll,
        spCategory_GetByKeyword,
        spCategory_GetByNormName,
    }

    public class SqlDataAccess : IDisposable, ISqlDataAccess
    {
        private readonly IConfiguration _config;
        private readonly ILogger<SqlDataAccess> _logger;

        public SqlDataAccess(IConfiguration config)
        {
            _config = config;
        }

        public string GetConnectionString()
        {
            return _config.GetConnectionString("OldWeeklyReview");
        }

        public List<T> LoadData<T, U>(StoreProcedure storeProcedure, U parameters)
        {
            string connectionString = GetConnectionString();

            using (IDbConnection cnn = new SqlConnection(connectionString))
            {
                var rows = cnn.Query<T>(
                    "dbo." + storeProcedure.ToString(), parameters,
                    commandType: CommandType.StoredProcedure).ToList();
                return rows;
            }
        }

        public void SaveData<T>(StoreProcedure storeProcedure, T parameters)
        {
            string connectionString = GetConnectionString();

            using (IDbConnection cnn = new SqlConnection(connectionString))
            {
                cnn.Execute(
                    storeProcedure.ToString(), parameters,
                    commandType: CommandType.StoredProcedure);
            }
        }

        public void UpdateData<T>(StoreProcedure storeProcedure, T parameters)
        {
            SaveData(storeProcedure, parameters);
        }

        public void DeleteData<T>(StoreProcedure storeProcedure, T parameters)
        {
            string connectionString = GetConnectionString();

            using (IDbConnection cnn = new SqlConnection(connectionString))
            {
                cnn.Execute(
                    storeProcedure.ToString(), parameters,
                    commandType: CommandType.StoredProcedure);
            }
        }

        private IDbConnection _connection;
        private IDbTransaction _transaction;
        private bool _isClosed = false;

        public void StartTransaction()
        {
            string connectionString = GetConnectionString();
            _connection = new SqlConnection(connectionString);
            _connection.Open();
            _transaction = _connection.BeginTransaction();
            _isClosed = false;
        }

        public List<T> LoadDataInTransaction<T, U>(StoreProcedure storeProcedure, U parameters)
        {
            var rows = _connection.Query<T>(
                storeProcedure.ToString(), parameters,
                commandType: CommandType.StoredProcedure, transaction: _transaction).ToList();
            return rows;
        }

        public void SaveDataInTransaction<T>(StoreProcedure storeProcedure, T parameters)
        {
            _connection.Execute(
                storeProcedure.ToString(), parameters,
                commandType: CommandType.StoredProcedure, transaction: _transaction);
        }

        public void DeleteDataInTransaction<T>(StoreProcedure storeProcedure, T parameters)
        {
            _connection.Execute(
                storeProcedure.ToString(), parameters,
                commandType: CommandType.StoredProcedure, transaction: _transaction);
        }

        public void CommitTransaction()
        {
            _transaction?.Commit();
            _connection?.Close();
            _isClosed = true;
        }

        public void RollbackTransaction()
        {
            _transaction?.Rollback();
            _connection?.Close();
            _isClosed = true;
        }

        public void Dispose()
        {
            if (!_isClosed)
            {
                try
                {
                    CommitTransaction();
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "Commit transaction fail in the dispose method.");
                }
            }
            _connection = null;
            _transaction = null;
        }
    }
}
