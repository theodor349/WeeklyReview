using DataMigration.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataMigration.Sql
{
    public class RecordAccess : IRecordAccess
    {
        private readonly ISqlDataAccess _sql;
        private readonly string _dbName = "TRData";

        private List<RecordModel> _records;

        public RecordAccess(ISqlDataAccess sql)
        {
            _sql = sql;
        }

        public List<RecordModel> GetAll()
        {
            LoadCache();
            var res = _sql.LoadData<RecordModel, dynamic>(StoreProcedure.spRecord_GetAll, new { });
            return res;
        }

        public void DeleteAt(DateTime date)
        {
        }

        private void LoadCache()
        {
            if (_records is null)
                _records = _sql.LoadData<RecordModel, dynamic>(StoreProcedure.spRecord_GetAll, new { });
        }

        private List<RecordModel> DBGetAt(DateTime date)
        {
            var res = _sql.LoadData<RecordModel, dynamic>(StoreProcedure.spRecord_GetAt, new { Date = date });
            return res;
        }

        private void DBDeleteAt(DateTime date)
        {
            _sql.DeleteData(StoreProcedure.spRecord_DeleteAt, new { Date = date });
        }

    }
}
