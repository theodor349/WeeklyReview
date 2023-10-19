using DataMigration.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace DataMigration.Sql
{
    public class RecordTimeAccess : IRecordTimeAccess
    {
        private readonly ISqlDataAccess _sql;
        private readonly string _dbName = "TRData";

        public RecordTimeAccess(ISqlDataAccess sql)
        {
            _sql = sql;
        }

        public void Create(RecordTimeModel model)
        {
            _sql.SaveData(StoreProcedure.spRecordTime_Create, model);
        }

        public List<RecordTimeModel> GetAll()
        {
            var records = _sql.LoadData<RecordTimeModel, dynamic>(StoreProcedure.spRecordTime_Get, new { });
            return records;
        }

        public List<RecordTimeModel> GetBetween(DateTime start, DateTime end)
        {
            var records = _sql.LoadData<RecordTimeModel, dynamic>(StoreProcedure.spRecordTime_GetBetween, new { Start = start, End = end });
            return records;
        }

        public RecordTimeModel GetBefore(DateTime date)
        {
            var records = _sql.LoadData<RecordTimeModel, dynamic>(StoreProcedure.spRecordTime_GetBefore, new { Date = date });
            return records.FirstOrDefault();
        }

        public RecordTimeModel GetAfter(DateTime date)
        {
            var records = _sql.LoadData<RecordTimeModel, dynamic>(StoreProcedure.spRecordTime_GetAfter, new { Date = date });
            return records.FirstOrDefault();
        }

        public void Update(DateTime date, int minutes)
        {
            _sql.SaveData(StoreProcedure.spRecordTime_Update, new { Date = date, Minutes = minutes });
        }

        public void Delete(DateTime date)
        {
            _sql.DeleteData(StoreProcedure.spRecordTime_Delete, new { Date = date });
        }
    }
}
