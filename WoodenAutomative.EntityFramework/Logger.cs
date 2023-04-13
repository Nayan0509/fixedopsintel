using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WoodenAutomative.EntityFramework
{
   public class Logger
    {
        private static readonly ILog log = LogManager.GetLogger("ErrorLog");
        
        /// <summary>
        /// To Log error messages
        /// </summary>
        /// <param name="ex">Exception object</param>
        public static void Error(Exception exception)
        {
            if (exception != null)
            {
                log.Error(exception.ToString());
            }
        }

        /// <summary>
        /// To Log error messages
        /// </summary>
        /// <param name="message">Comment/Message to log</param>
        public static void Error(string message)
        {
            if (!string.IsNullOrWhiteSpace(message))
            {
                log.Error(message);
            }
        }

        /// <summary>
        /// To Log error messages
        /// </summary>
        /// <param name="ex">Exception object</param>
        /// <param name="message">Comment/Message to log</param>
        public static void Error(Exception exception, string message)
        {
            if (exception!=null && !string.IsNullOrWhiteSpace(message))
            {
                log.Error(message + " " +exception);
            }
        }

        /// <summary>
        /// To Log Warning messages
        /// </summary>
        /// <param name="message">Comment/Message to log</param>
        public static void Warn(string message)
        {
            if (!string.IsNullOrWhiteSpace(message))
            {
                log.Warn(message);
            }
        }

        /// <summary>
        /// To Log informational messages
        /// </summary>
        /// <param name="message">Comment/Message to log</param>
        public static void Info(string message)
        {
            if (!string.IsNullOrWhiteSpace(message))
            {
                log.Info(message);
            }
        }

        /// <summary>
        /// To Log Debug Messages
        /// </summary>
        /// <param name="message">Comment/Message to log</param>
        public static void Debug(string message)
        {
            if (!string.IsNullOrWhiteSpace(message))
            {
                log.Debug(message);
            }
        }
    }
}
