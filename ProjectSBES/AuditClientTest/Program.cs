using AuditLibrary;
using Contracts;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace AuditClientTest
{
    class Program
    {
        static void Main(string[] args)
        {
            /// Define the expected service certificate. It is required to establish cmmunication using certificates.
			string srvCertCN = String.Empty;//"wcfservice";

            /// Define the expected certificate for signing ("<username>_sign" is the expected subject name).
            /// .NET WindowsIdentity class provides information about Windows user running the given process
            string signCertCN = String.Empty;//"wcfclient";

            NetTcpBinding binding = new NetTcpBinding();
            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Certificate;

            /// Use CertManager class to obtain the certificate based on the "srvCertCN" representing the expected service identity.
			X509Certificate2 srvCert = null;//CertManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, srvCertCN);
            EndpointAddress address = new EndpointAddress(new Uri("net.tcp://localhost:50050/IAuditService"),
                                      new X509CertificateEndpointIdentity(srvCert));

            using (AuditClient proxy = new AuditClient(binding, address))
            {
                /// Get service's identity	
                WindowsIdentity winIdentity = WindowsIdentity.GetCurrent();
				SecurityEvent message = new SecurityEvent((winIdentity.User).ToString(), winIdentity.Name, "id", "compName", new DateTime(), 1, "event description");

                /// Create a signature based on the "signCertCN"
                X509Certificate2 signCert = null;//CertManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, signCertCN);

                /// Create a signature using SHA1 hash algorithm
                byte[] signature = DigitalSignature.Create(message, "SHA1", signCert);
                proxy.WriteEvent(message, signature);
          
                Console.ReadLine();
            }            
        }
    }
}
