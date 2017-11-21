﻿using AuditLibrary;
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
        public bool RunProcess(EProcessType process)
        {
            string processName = ProcessConfig.GetValue(process);

            Debugger.Launch();

            if (processName != null)
            {
                CustomPrincipal principal = Thread.CurrentPrincipal as CustomPrincipal;

                string userIdentity = "";

                if (!principal.IsInRole(process.ToString()))
                {
                    userIdentity = CustomPrincipal.UserIdentity;

                    if (!ServiceDataHelper.Helper().usersAttempts.ContainsKey(userIdentity))
                    {
                        ServiceDataHelper.Helper().usersAttempts.Add(userIdentity, 0);
                    }

                    try
                    {
                        ServiceDataHelper.Helper().forbidenUsers[userIdentity].Stop();
                        if (ServiceDataHelper.Helper().forbidenUsers[userIdentity].ElapsedMilliseconds > ServiceDataHelper.Helper().timeOfDenial)
                        {
                            Process.Start(processName);
                            ServiceDataHelper.Helper().usersAttempts[userIdentity] = 0;
                            ServiceDataHelper.Helper().forbidenUsers[userIdentity] = null;
                            return true;
                        }
                        else
                        {
                            ServiceDataHelper.Helper().forbidenUsers[userIdentity].Start();

                            WindowsIdentity winIdentity = WindowsIdentity.GetCurrent();
                            SecurityEvent message = new SecurityEvent((winIdentity.User).ToString(), winIdentity.Name, ((WindowsIdentity)Thread.CurrentPrincipal.Identity).User.ToString(), principal.Identity.Name, ServiceDataHelper.Helper().eventCnt++, "User tried to execute process from black list more than it is allowed");

                            NetTcpBinding binding = new NetTcpBinding();
                            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Certificate;

                            /// Define the expected service certificate. It is required to establish cmmunication using certificates.
                            string srvCertCN = "auditservice";

                            /// Use CertManager class to obtain the certificate based on the "srvCertCN" representing the expected service identity.
                            X509Certificate2 srvCert = CertManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, srvCertCN);
                            EndpointAddress address = new EndpointAddress(new Uri("net.tcp://localhost:50050/IAuditService"),
                                                      new X509CertificateEndpointIdentity(srvCert));

                            using (ServiceProxy proxy = new ServiceProxy(binding, address))
                            {
                                /// Define the expected certificate for signing client
                                string signCertCN = String.Format(Formatter.ParseName(WindowsIdentity.GetCurrent().Name) + "_sign");

                                /// Create a signature based on the "signCertCN"
                                X509Certificate2 signCert = CertManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, signCertCN);

                                /// Create a signature using SHA1 hash algorithm
                                byte[] signature = DigitalSignature.Create(message, "SHA1", signCert);
                                proxy.WriteEvent(message, signature);

                            }
                            return false;
                        }

                    }
                    catch(Exception e)
                    {

                        Process.Start(processName);
                        ServiceDataHelper.Helper().usersAttempts.Remove(userIdentity);
                        ServiceDataHelper.Helper().forbidenUsers.Remove(userIdentity);
                        return true;
                    }

                }
                else
                {
                    userIdentity = CustomPrincipal.UserIdentity;

                    if (!ServiceDataHelper.Helper().usersAttempts.ContainsKey(userIdentity))
                    {
                        ServiceDataHelper.Helper().usersAttempts.Add(userIdentity, 0);
                    }

                    ServiceDataHelper.Helper().usersAttempts[userIdentity]++;
                    if (ServiceDataHelper.Helper().usersAttempts[userIdentity] > ServiceDataHelper.Helper().maxAttempts)
                    {
                        if (!ServiceDataHelper.Helper().forbidenUsers.ContainsKey(userIdentity))
                        {
                            ServiceDataHelper.Helper().forbidenUsers.Add(userIdentity, new Stopwatch());
                        }

                        ServiceDataHelper.Helper().forbidenUsers[userIdentity].Start();
                    }
                }
            }

            return false;
        }
    }
}
