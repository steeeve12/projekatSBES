using Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class ServiceProxy : ChannelFactory<IAuditService>, IAuditService
    {

        private static ServiceProxy audit = null;
        private IAuditService factory;

        public ServiceProxy(string endpointConfigurationName) : base(endpointConfigurationName)
        {
            factory = this.CreateChannel();
        }

        public static ServiceProxy Audit(string endpointConfigurationName)
        {
            if(audit == null)
            {
                return new ServiceProxy(endpointConfigurationName);
            }

            return audit;
        }
        public bool WriteEvent(SecurityEvent sEvent, byte[] sign)
        {
            throw new NotImplementedException();
        }
    }
}
