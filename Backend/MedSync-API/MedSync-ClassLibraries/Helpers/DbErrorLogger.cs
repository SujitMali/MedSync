using MedSync_ClassLibraries.DAL;
using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace MedSync_ClassLibraries.Helpers
{
    public class DbErrorLogger
    {
        public static void LogError(Exception ex, int? createdBy = null, [CallerMemberName] string methodName = "", [CallerFilePath] string filePath = "")
        {
            try
            {
                if (ex == null)
                    return;
                string className = Path.GetFileNameWithoutExtension(filePath);
                string message = ex.Message;

                if (ex.InnerException != null)
                    message += " | Inner: " + ex.InnerException.Message;

                var errorDal = new ErrorLogs();
                errorDal.LogErrorToDatabase(className, methodName, message, ex.StackTrace ?? "No stack trace available", createdBy);
            }
            catch
            {
            }
        }
    
    }
}
