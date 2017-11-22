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
            int processNum;
            EProcessType processToExecute = EProcessType.GoogleChrome;
            bool ok;
            bool end = false;

            do {

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

                do
                {
                    Console.WriteLine("Enter process you wish to execute [1-GoogleChrome, 2-Mozilla, 3-Notepad, 4-VisualStudio15]: ");
                    ok = Int32.TryParse(Console.ReadLine(), out processNum);

                    switch (processNum)
                    {
                        case 1:
                            processToExecute = EProcessType.GoogleChrome;
                            break;
                        case 2:
                            processToExecute = EProcessType.Mozilla;
                            break;
                        case 3:
                            processToExecute = EProcessType.Notepad;
                            break;
                        case 4:
                            processToExecute = EProcessType.VisualStudio15;
                            break;
                        default:
                            ok = false;
                            break;
                    }
                } while (!ok);

                using (ClientProxy proxy = new ClientProxy(endpointConfigurationName))
                {
                    if (proxy.RunProcess(processToExecute))
                    {
                        Console.WriteLine(processToExecute.ToString() + " is started!");
                    }
                    else
                    {
                        Console.WriteLine(processToExecute.ToString() + " is not started!");
                    }

                }

                Console.WriteLine("Enter x if you want to exit or any other key to continue..");
                string read = Console.ReadLine();

                if (read.Equals("x"))
                    end = true;

            } while (!end);
        }
    }
}
