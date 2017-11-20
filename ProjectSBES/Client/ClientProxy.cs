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

        public ClientProxy(NetTcpBinding binding, EndpointAddress a) : base(binding, a)
        {
            factory = this.CreateChannel();
        }

        public bool RunProcess(EProcessType process)
        {
            return factory.RunProcess(process);
        }
    }
}
