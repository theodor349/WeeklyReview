using DataMigration.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace DataMigration.Sql
{
    public class ActivityAccess : IActivityAccess
    {
        private readonly ISqlDataAccess _sql;
        private readonly string _dbName = "TRData";

        public ActivityAccess(ISqlDataAccess sql)
        {
            _sql = sql;
        }

        public ActivityModel Get(int id)
        {
            var res = _sql.LoadData<ActivityModel, dynamic>(StoreProcedure.spActivity_Get, new { Id = id });
            return res.FirstOrDefault();
        }

        public ActivityModel Get(string normName)
        {
            var res = _sql.LoadData<ActivityModel, dynamic>(StoreProcedure.spActivity_GetByNormName, new { NormalizedName = normName });
            return res.FirstOrDefault();
        }

        public List<ActivityModel> GetAll()
        {
            var res = _sql.LoadData<ActivityModel, dynamic>(StoreProcedure.spActivity_GetAll, new { });
            return res.ConvertAll<ActivityModel>(a => a);
        }
    }
}
