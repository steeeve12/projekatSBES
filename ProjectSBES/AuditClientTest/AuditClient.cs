using AuditLibrary;
using Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace AuditClientTest
{
    public class AuditClient : ChannelFactory<IAuditService>, IAuditService
    {
        IAuditService factory;

        public AuditClient(NetTcpBinding binding, EndpointAddress address) : base(binding, address)
        {
            string cltCertCN = "wcfclient";

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
        /// <param name="sEvent"> Securi</param>
        /// <param name="sign"></param>
        /// <returns></returns>
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
