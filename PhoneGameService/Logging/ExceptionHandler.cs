using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;

namespace PhoneGameService.Logging
{
    public static class ExceptionHandler
    {
        public static void LogAll(ILog log, Exception ex)
        {
            Exception exRoot = ex;
            do
            {
                log.Error("Exception caught: ", ex);
                ex = ex.InnerException;
            } while (null != ex);
        }
    }
}
