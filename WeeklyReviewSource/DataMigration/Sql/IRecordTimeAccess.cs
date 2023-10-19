using DataMigration.Models;
using System;
using System.Collections.Generic;

namespace DataMigration.Sql
{
    public interface IRecordTimeAccess
    {
        void Create(RecordTimeModel model);
        void Delete(DateTime date);
        List<RecordTimeModel> GetAll();
        RecordTimeModel GetAfter(DateTime date);
        RecordTimeModel GetBefore(DateTime date);
        List<RecordTimeModel> GetBetween(DateTime start, DateTime end);
        void Update(DateTime date, int minutes);
    }
}