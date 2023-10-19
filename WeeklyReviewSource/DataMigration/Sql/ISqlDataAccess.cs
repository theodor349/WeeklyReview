namespace DataMigration.Sql
{
    public interface ISqlDataAccess
    {
        void CommitTransaction();
        void DeleteData<T>(StoreProcedure storeProcedure, T parameters);
        void DeleteDataInTransaction<T>(StoreProcedure storeProcedure, T parameters);
        void Dispose();
        string GetConnectionString();
        List<T> LoadData<T, U>(StoreProcedure storeProcedure, U parameters);
        List<T> LoadDataInTransaction<T, U>(StoreProcedure storeProcedure, U parameters);
        void RollbackTransaction();
        void SaveData<T>(StoreProcedure storeProcedure, T parameters);
        void SaveDataInTransaction<T>(StoreProcedure storeProcedure, T parameters);
        void StartTransaction();
        void UpdateData<T>(StoreProcedure storeProcedure, T parameters);
    }
}