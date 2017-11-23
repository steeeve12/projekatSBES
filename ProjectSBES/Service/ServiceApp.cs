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
using System.Threading;
using System.Threading.Tasks;

namespace Service
{
    public class ServiceApp : IService
    {
        private static object syncLock = new object();
        public bool RunProcess(EProcessType process)
        {
            lock (syncLock)
            {
                string processName = ProcessConfig.GetValue(process);

                if (processName != null)
                {
                    CustomPrincipal principal = Thread.CurrentPrincipal as CustomPrincipal;

                    string userIdentity = principal.Identity.Name;
                  
                    try
                    {
                        ServiceDataHelper.Helper().forbidenUsers[userIdentity].Stop();
                        if (ServiceDataHelper.Helper().forbidenUsers[userIdentity].ElapsedMilliseconds > ServiceDataHelper.Helper().timeOfDenial)
                        {
                            ServiceDataHelper.Helper().usersAttempts.Remove(userIdentity);
                            ServiceDataHelper.Helper().forbidenUsers.Remove(userIdentity);

                            return CheckBlackList(principal, process, userIdentity, processName);
                        }
                        else
                        {
                            ServiceDataHelper.Helper().forbidenUsers[userIdentity].Start();
                        }
                    }
                    catch
                    {
                        return CheckBlackList(principal, process, userIdentity, processName);
                    }
                }

                return false;
            }
        }

        private static bool CheckBlackList(CustomPrincipal principal, EProcessType process, string userIdentity, string processName)
        {
            if (!ServiceDataHelper.Helper().usersAttempts.ContainsKey(userIdentity))
            {
                ServiceDataHelper.Helper().usersAttempts.Add(userIdentity, 0);
            }

            if (!principal.IsInRole(process.ToString()))
            {
                Process.Start(processName);
                ServiceDataHelper.Helper().usersAttempts.Remove(userIdentity);
                ServiceDataHelper.Helper().forbidenUsers.Remove(userIdentity);
                return true;
            }
            else
            {
                ServiceDataHelper.Helper().usersAttempts[userIdentity]++;
                if (ServiceDataHelper.Helper().usersAttempts[userIdentity] > ServiceDataHelper.Helper().maxAttempts)
                {
                    if (!ServiceDataHelper.Helper().forbidenUsers.ContainsKey(userIdentity))
                    {
                        ServiceDataHelper.Helper().forbidenUsers.Add(userIdentity, new Stopwatch());

                        SendEvent(principal, processName);
                    }

                    ServiceDataHelper.Helper().forbidenUsers[userIdentity].Start();
                }

                return false;
            }
        } 


        private static void SendEvent(CustomPrincipal principal, string processName)
        {
            WindowsIdentity winIdentity = WindowsIdentity.GetCurrent();
            SecurityEvent message = new SecurityEvent((winIdentity.User).ToString(), winIdentity.Name, ((WindowsIdentity)Thread.CurrentPrincipal.Identity).User.ToString(), principal.Identity.Name, ServiceDataHelper.Helper().eventCnt++, "User tried to execute process " + processName + " from black list many times than it is allowed");

            NetTcpBinding binding = new NetTcpBinding();
            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Certificate;

            /// Define the expected service certificate. It is required to establish cmmunication using certificates.
            string srvCertCN = "auditservice";

            /// Use CertManager class to obtain the certificate based on the "srvCertCN" representing the expected service identity.
            X509Certificate2 srvCert = CertManager.GetCertificateFromStorage(StoreName.TrustedPeople, StoreLocation.LocalMachine, srvCertCN);
            EndpointAddress address = new EndpointAddress(new Uri("net.tcp://10.1.212.185:50050/IAuditService"),
                                      new X509CertificateEndpointIdentity(srvCert));

            /// Define the expected certificate for signing client
            string signCertCN = String.Format(Formatter.ParseName(WindowsIdentity.GetCurrent().Name) + "_sign");

            /// Create a signature based on the "signCertCN"
            X509Certificate2 signCert = CertManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, signCertCN);

            /// Create a signature using SHA1 hash algorithm
            byte[] signature = DigitalSignature.Create(message, "SHA1", signCert);

            ServiceProxy.Audit(binding, address).WriteEvent(message, signature);
        }
    }
}
