using Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            string address = "net.tcp://10.1.212.165:50030/IService";
            EndpointAddress ea = new EndpointAddress(new Uri(address), EndpointIdentity.CreateUpnIdentity("Administrator@P04-05"));
            using (ClientProxy proxy = new ClientProxy(new NetTcpBinding(), ea))
            {

                EProcessType procces = EProcessType.GoogleChrome;

                if(proxy.RunProcess(procces))
                {
                    Console.WriteLine(procces.ToString() + " is started!");
                }
                else
                {
                    Console.WriteLine(procces.ToString() + " is not started!");
                }         
            }
            Console.ReadLine();
        }
    }
}
