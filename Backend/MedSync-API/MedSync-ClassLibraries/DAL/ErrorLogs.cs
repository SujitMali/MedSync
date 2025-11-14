using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Data.Common;
using System.Data;

namespace MedSync_ClassLibraries.DAL
{
    public class ErrorLogs
    {
        private readonly Database db;

        public ErrorLogs()
        {
            db = DatabaseFactory.CreateDatabase();
        }

        #region LogErrorToDatabase
        public void LogErrorToDatabase(string className, string methodName, string exceptionMessage, string stackTrace, int? createdBy = null)
        {
            try
            {
                DbCommand cmd = db.GetStoredProcCommand("MedSync_InsertErrorLog");

                db.AddInParameter(cmd, "@ClassName", DbType.String, className);
                db.AddInParameter(cmd, "@MethodName", DbType.String, methodName);
                db.AddInParameter(cmd, "@ExceptionMessage", DbType.String, exceptionMessage);
                db.AddInParameter(cmd, "@StackTrace", DbType.String, stackTrace);
                db.AddInParameter(cmd, "@CreatedBy", DbType.Int32, createdBy.HasValue ? createdBy.Value : (object)DBNull.Value);

                db.ExecuteNonQuery(cmd);
            }
            catch
            {
            }
        }

        #endregion

    }
}
