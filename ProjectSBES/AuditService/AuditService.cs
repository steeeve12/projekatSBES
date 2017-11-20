using Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Audit.AuditLibrary;
using System.Security.Principal;

namespace Audit.AuditService
{
    public class AuditService : IAuditService
    {
        public bool WriteEvent(SecurityEvent sEvent)
        {
            Console.WriteLine("Security event written.\n"); 
            AuditLibrary.Audit.WriteSecurityEvent("A", "b", sEvent);
            return true;
        }
    }
}
