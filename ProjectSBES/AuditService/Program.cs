using AuditLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Security;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Audit.AuditService
{
    class Program
    {
        static void Main(string[] args)
        {

            string srvCertCN = "wcfservice";

            ServiceHost _serviceHost = new ServiceHost(typeof(AuditService));

            /// Auditing
            ServiceSecurityAuditBehavior newAudit = new ServiceSecurityAuditBehavior();
            newAudit.AuditLogLocation = AuditLogLocation.Application;
            newAudit.ServiceAuthorizationAuditLevel = AuditLevel.SuccessOrFailure;
            newAudit.SuppressAuditFailure = true;

            _serviceHost.Description.Behaviors.Remove<ServiceSecurityAuditBehavior>();
            _serviceHost.Description.Behaviors.Add(newAudit);


            /// Certificates
            ///Custom validation mode enables creation of a custom validator - CustomCertificateValidator
            _serviceHost.Credentials.ClientCertificate.Authentication.CertificateValidationMode = X509CertificateValidationMode.Custom;
            _serviceHost.Credentials.ClientCertificate.Authentication.CustomCertificateValidator = new ServiceCertValidator();

            /// If CA doesn't have a CRL associated, WCF blocks every client because it cannot be validated
            _serviceHost.Credentials.ClientCertificate.Authentication.RevocationMode = X509RevocationMode.NoCheck;

            /// Set appropriate service's certificate on the host. Use CertManager class to obtain the certificate based on the "srvCertCN"
            _serviceHost.Credentials.ServiceCertificate.Certificate = CertManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, srvCertCN);        
            
            try
            {
                _serviceHost.Open();
                Console.WriteLine("AUDIT_SERVER: Ready and waiting for requests. Press any key to exit.");
                Console.ReadLine();
            }
            catch (Exception e)
            {
                Console.WriteLine("[ERROR] {0}", e.Message);
            }
            finally
            {
                _serviceHost.Close();
                Console.WriteLine("AUDIT_SERVER: Stopped.");

                Thread.Sleep(3 * 1000);
            }
        }
    }
}
