using DataMigration.Models;
using System;
using System.Collections.Generic;

namespace DataMigration.Sql
{
    public interface IRecordAccess
    {
        List<RecordModel> GetAll();
        void DeleteAt(DateTime date);
    }
}