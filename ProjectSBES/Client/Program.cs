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
            Console.WriteLine("Enter endpoint name of server you want to target [Contracts.IService.ServerOne, Contracts.IService.ServerTwo, Contracts.IService.ServeThree]: ");
            string endpointConfigurationName = Console.ReadLine();

            using (ClientProxy proxy = new ClientProxy(endpointConfigurationName))
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
