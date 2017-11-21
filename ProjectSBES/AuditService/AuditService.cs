using Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Audit.AuditLibrary;
using System.Security.Principal;
using System.Security.Cryptography.X509Certificates;
using AuditLibrary;
using System.Threading;
using System.ServiceModel;
using System.Diagnostics;

namespace Audit.AuditService
{
    public class AuditService : IAuditService
    {
        /// <summary>
        /// Write security event to Windows Event Log
        /// </summary>
        /// <param name="sEvent"> Security event to be written in Windows Event Log </param>
        /// <param name="sign"> Digital signature of a message </param>
        /// <returns> true if writing succeeded, otherwise false </returns>
        public bool WriteEvent(SecurityEvent sEvent, byte[] sign)
        {
            bool retVal = false;

            /// Get client's certificate from storage. It is expected that clients sign messages using the certificate with subjectName in the following format "<username>_sign" 		
            //string signCertCN = String.Format(Formatter.ParseName(((GenericPrincipal)((GenericPrincipal)(Thread.CurrentPrincipal.Identity).Name)) + "_sign"); // Check this!!!!!!!!!!!
            Debugger.Launch();
            var identity = ServiceSecurityContext.Current.PrimaryIdentity;
            string name = Formatter.ParseName(identity.Name);
            
            string signCertCN = String.Format(name + "_sign");

            X509Certificate2 clientCertificate = CertManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, signCertCN);

            /// Verify signature using SHA1 hash algorithm
			if (DigitalSignature.Verify(sEvent, "SHA1", sign, clientCertificate))
            {
                /// Digital signature is valid
                AuditLibrary.Audit.WriteSecurityEvent(sEvent);
                Console.WriteLine("Security event has been written.\n");
                retVal = true;
            }
            else
            {
                Console.WriteLine("Digital Signature is invalid.");
                retVal =  false;
            }

            return retVal;
        }
    }
}
