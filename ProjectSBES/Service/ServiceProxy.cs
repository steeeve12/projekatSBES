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

namespace Service
{
    public class ServiceProxy : ChannelFactory<IAuditService>, IAuditService
    {

        private static ServiceProxy audit = null;
        private IAuditService factory;

        public ServiceProxy(NetTcpBinding binding, EndpointAddress address) : base(binding, address)
        {
            string cltCertCN = Formatter.ParseName(WindowsIdentity.GetCurrent().Name);

            this.Credentials.ServiceCertificate.Authentication.CertificateValidationMode = System.ServiceModel.Security.X509CertificateValidationMode.Custom;
            this.Credentials.ServiceCertificate.Authentication.CustomCertificateValidator = new ClientCertValidator();
            this.Credentials.ServiceCertificate.Authentication.RevocationMode = X509RevocationMode.NoCheck;

            /// Set appropriate client's certificate on the channel. Use CertManager class to obtain the certificate based on the "cltCertCN"
            this.Credentials.ClientCertificate.Certificate = CertManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, cltCertCN);

            factory = this.CreateChannel();
        }

        public static ServiceProxy Audit(NetTcpBinding binding, EndpointAddress address)
        {
            if(audit == null)
            {
                return new ServiceProxy(binding, address);
            }

            return audit;
        }
        public bool WriteEvent(SecurityEvent sEvent, byte[] sign)
        {
            return factory.WriteEvent(sEvent, sign);
        }
    }
}
