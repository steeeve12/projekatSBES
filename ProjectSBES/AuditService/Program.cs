using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Audit.AuditService
{
    class Program
    {
        static void Main(string[] args)
        {
            ServiceHost _serviceHost = new ServiceHost(typeof(AuditService));

            // Auditing
            ServiceSecurityAuditBehavior newAudit = new ServiceSecurityAuditBehavior();
            newAudit.AuditLogLocation = AuditLogLocation.Application;
            newAudit.ServiceAuthorizationAuditLevel = AuditLevel.SuccessOrFailure;
            newAudit.SuppressAuditFailure = true;

            _serviceHost.Description.Behaviors.Remove<ServiceSecurityAuditBehavior>();
            _serviceHost.Description.Behaviors.Add(newAudit);
            //

            _serviceHost.Open();

            Console.WriteLine("AUDIT_SERVER: Ready and waiting for requests. Press any key to exit.");

            Console.ReadLine();

            _serviceHost.Close();
            Console.WriteLine("AUDIT_SERVER: Stopped.");

            Thread.Sleep(3 * 1000);
        }
    }
}
