using AuditLibrary;
using Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace AuditClientTest
{
    public class AuditClient : ChannelFactory<IAuditService>, IAuditService
    {
        IAuditService factory;

        /// <summary>
        /// Create channel used for communication between client and service
        /// </summary>
        /// <param name="binding"> NetTcpBinding </param>
        /// <param name="address"> service address </param>
        public AuditClient(NetTcpBinding binding, EndpointAddress address) : base(binding, address)
        {
            /// cltCertCN.SubjectName should be set to the client's username. 
            /// .NET WindowsIdentity class provides information about Windows user running the given process
            string cltCertCN = Formatter.ParseName(WindowsIdentity.GetCurrent().Name);

            this.Credentials.ServiceCertificate.Authentication.CertificateValidationMode = System.ServiceModel.Security.X509CertificateValidationMode.Custom;
            this.Credentials.ServiceCertificate.Authentication.CustomCertificateValidator = new ClientCertValidator();
            this.Credentials.ServiceCertificate.Authentication.RevocationMode = X509RevocationMode.NoCheck;

            /// Set appropriate client's certificate on the channel. Use CertManager class to obtain the certificate based on the "cltCertCN"
            this.Credentials.ClientCertificate.Certificate = CertManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, cltCertCN);

            factory = this.CreateChannel();
        }

        /// <summary>
        /// Writing a security event to Windows Event log
        /// </summary>
        /// <param name="sEvent"> Security event </param>
        /// <param name="
        /// 
        /// "> Digital signature of a message </param>
        /// <returns> true is writing succeeded, otherwise false </returns>
        public bool WriteEvent(SecurityEvent sEvent, byte[] sign)
        {
            bool retVal = false;
            try
            {
                retVal = factory.WriteEvent(sEvent, sign);
                Console.WriteLine("[WriteEvent] SUCCESS");
            }
            catch (Exception e)
            {
                Console.WriteLine("[WriteEvent] ERROR = {0}", e.Message);
            }
            return retVal;
        }
    }
}
