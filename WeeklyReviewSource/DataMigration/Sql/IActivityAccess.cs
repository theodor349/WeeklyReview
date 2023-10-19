using DataMigration.Models;
using System.Collections.Generic;

namespace DataMigration.Sql
{
    public interface IActivityAccess
    {
        ActivityModel Get(int id);
        ActivityModel Get(string normName);
        List<ActivityModel> GetAll();
    }
}