using Contracts;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Audit.AuditLibrary
{
    public class Audit
    {
        private static EventLog customLog = null;
        const string SourceName = "AuditLibrary.Audit";
        const string LogName = "ServiceSecurityEvents";
        private static Object lockObject = new Object();

        static Audit()
        {
            try
            {
                if (!EventLog.SourceExists(SourceName))
                {
                    EventLog.CreateEventSource(SourceName, LogName);
                }

                /// Create customLog handle
                customLog = new EventLog(LogName, Environment.MachineName, SourceName);
            }
            catch (Exception e)
            {
                customLog = null;
                Console.WriteLine("Error while trying to create log handle. Error = {0}", e.Message);
            }
        }

        /// <summary>
        /// Writing security event to Windows Event Log
        /// </summary>
        /// <param name="sEvent"> security event to be written to event log </param>
        public static void WriteSecurityEvent(SecurityEvent sEvent)
        {
            if (customLog != null)
            {
                string s = String.Format("[{0}] Client [id - {1}, computer name - {2}] failed with running process [{3}] on service [id - {4}, computer name - {5}]", sEvent.Timestamp, sEvent.ClientId, sEvent.ClientComputerName, sEvent.EventId, sEvent.ServiceId, sEvent.ClientComputerName);
                
                lock (lockObject)
                {
                    customLog.WriteEntry(s);
                }
            }
            else
            {
                throw new ArgumentException(string.Format("Error while trying to write security event to event log."));
            }
        }

    }
}
