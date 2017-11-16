using Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class ClientProxy : ChannelFactory<IService>, IService
    {
        IService factory;

        public ClientProxy(string endpointConfigurationName) : base(endpointConfigurationName)
        {
            factory = this.CreateChannel();
        }

        public bool RunProcess(EProcessType process)
        {
            return factory.RunProcess(process);
        }
    }
}
