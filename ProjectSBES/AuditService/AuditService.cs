using Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Audit.AuditLibrary;

namespace Audit.AuditService
{
    public class AuditService : IAuditService
    {
        public bool WriteEvent(SecurityEvent sEvent)
        {
            AuditLibrary.Audit.WriteSecurityEvent("A", "b", sEvent);
            return true;
        }
    }
}
