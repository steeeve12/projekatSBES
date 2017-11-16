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

        static Audit()
        {
            try
            {
                if (!EventLog.SourceExists(SourceName))
                {
                    EventLog.CreateEventSource(SourceName, LogName);
                }
                /// create customLog handle
                customLog = new EventLog(LogName, Environment.MachineName, SourceName);
            }
            catch (Exception e)
            {
                customLog = null;
                Console.WriteLine("Error while trying to create log handle. Error = {0}", e.Message);
            }
        }

        public static void WriteSecurityEvent(string serviceId, string computerName, SecurityEvent sEvent)
        {
            if (customLog != null)
            {
                string s = String.Format("[{0}] Client [{1}, {2}] failed with running process [{3}] on service [{4}, {5}]", sEvent.Timestamp, sEvent.ClientId, sEvent.ClientComputerName, sEvent.EventId, serviceId, computerName);
                customLog.WriteEntry(s);
            }
            else
            {
                throw new ArgumentException(string.Format("Error while trying to write event to event log."));
            }
        }

    }
}
