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

namespace Audit.AuditService
{
    public class AuditService : IAuditService
    {
        public bool WriteEvent(SecurityEvent sEvent, byte[] sign)
        {
            bool retVal = false;
            /// Get client's certificate from storage. It is expected that clients sign messages using the certificate with subjectName in the following format "<username>_sign" 			
			X509Certificate2 clientCertificate = CertManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, "wcfclient");

            /// Verify signature using SHA1 hash algorithm
			if (DigitalSignature.Verify(sEvent, "SHA1", sign, clientCertificate))
            {
                Console.WriteLine("Digital Signature is valid.");

                Console.WriteLine("Security event has been written.\n");
                AuditLibrary.Audit.WriteSecurityEvent("A", "b", sEvent);
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
