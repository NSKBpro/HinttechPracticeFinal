using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HinttechPractice.Core.Exceptions
{
    /// <summary>
    /// Exception handler for 404 error.
    /// </summary>
    public class Exception404PageNotFound : Exception
    {
        private ILog log;

        public Exception404PageNotFound() : base() { }

        public Exception404PageNotFound(string message, Exception ex)
            : base(message, ex)
        {

            log4net.GlobalContext.Properties["LogName"] = DateTime.Now.ToShortDateString().ToString();
            log = MyLogger.GetLogger(typeof(Exception404PageNotFound));

            log.Error(message, ex);
        }
    }
}
