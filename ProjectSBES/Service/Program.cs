using Contracts;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Policy;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    class Program
    {
        static void Main(string[] args)
        {
            ServiceHost _serviceHost = new ServiceHost(typeof(ServiceApp));
            _serviceHost.Authorization.ServiceAuthorizationManager = new CustomAuthorizationManager();

            List<IAuthorizationPolicy> policies = new List<IAuthorizationPolicy>();
            policies.Add(new CustomAuthorizationPolicy());
            _serviceHost.Authorization.ExternalAuthorizationPolicies = policies.AsReadOnly();

            _serviceHost.Authorization.PrincipalPermissionMode = PrincipalPermissionMode.Custom;

            _serviceHost.Description.Behaviors.Remove(typeof(ServiceDebugBehavior));
            _serviceHost.Description.Behaviors.Add(new ServiceDebugBehavior() { IncludeExceptionDetailInFaults = true });

            _serviceHost.Open();

            Console.WriteLine("SERVER: Ready and waiting for requests. Press any key to exit.");

            Console.ReadLine();

            _serviceHost.Close();
            Console.WriteLine("SERVER: Stopped.");
        }
    }
}
