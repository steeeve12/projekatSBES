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
            string endpointConfigurationName = "";
            int serverNum;
            bool ok;

            do
            {
                Console.WriteLine("Enter endpoint name of server you want to target [1, 2 or 3]: ");
                ok = Int32.TryParse(Console.ReadLine(), out serverNum);

                switch (serverNum)
                {
                    case 1:
                        endpointConfigurationName = "Contracts.IService.ServerOne";
                        break;
                    case 2:
                        endpointConfigurationName = "Contracts.IService.ServerTwo";
                        break;
                    case 3:
                        endpointConfigurationName = "Contracts.IService.ServerThree";
                        break;
                    default:
                        ok = false;
                        break;
                }
            } while (!ok);

            using (ClientProxy proxy = new ClientProxy(endpointConfigurationName))
            {
                EProcessType procces = EProcessType.VisualStudio15;

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
