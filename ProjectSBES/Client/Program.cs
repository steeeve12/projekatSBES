using Contracts;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Principal;
using System.ServiceModel;
using System.ServiceModel.Security;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            bool ok = false;
            bool end = false;
            int scenario;
            do
            {
                do
                {
                    Console.WriteLine("------------------------------------------------------------");
                    Console.Write("Enter scenario you want to see...\n(1)TestCase1 - Process is not on the BlackList\n(2)TestCase2 - Process is on the BlackList\n(3)TestManual\n>");
                    ok = Int32.TryParse(Console.ReadLine(), out scenario);

                    switch (scenario)
                    {
                        case 1:
                            if (TestCase1())
                            {
                                Console.BackgroundColor = ConsoleColor.DarkGreen;
                                Console.WriteLine("Test Case 1 passed.");
                                Console.BackgroundColor = ConsoleColor.Black;
                            }
                            else
                            {
                                Console.BackgroundColor = ConsoleColor.DarkRed;
                                Console.WriteLine("Test Case 1 failed.");
                                Console.BackgroundColor = ConsoleColor.Black;
                            }
                            break;
                        case 2:
                            if (TestCase2())
                            {
                                Console.BackgroundColor = ConsoleColor.DarkGreen;
                                Console.WriteLine("Test Case 2 passed.");
                                Console.BackgroundColor = ConsoleColor.Black;
                            }
                            else
                            {
                                Console.BackgroundColor = ConsoleColor.DarkRed;
                                Console.WriteLine("Test Case 2 failed.");
                                Console.BackgroundColor = ConsoleColor.Black;
                            }
                            break;
                        case 3:
                            TestManual();
                            break;
                        default:
                            Console.BackgroundColor = ConsoleColor.DarkRed;
                            Console.WriteLine("You entered a wrong number. Try Again!");
                            Console.BackgroundColor = ConsoleColor.Black;
                            ok = false;
                            break;
                    }

                } while (!ok);

                Console.WriteLine("------------------------------------------------------------");
                Console.WriteLine("Enter x if you want to exit or any other key to continue..");
                string read = Console.ReadLine();

                if (read.Equals("x"))
                {
                    end = true;
                }

            } while (!end);
        }

        /// <summary>
        /// Test case when process is not on a BlackList for logged in user
        /// </summary>
        /// <returns> true if testing succeeded, or false when every process is forbidden for logged in user </returns>
        static bool TestCase1()
        {
            return Test(false);
        }

        /// <summary>
        /// Test case when process is not on a BlackList for logged in user
        /// </summary>
        /// <returns>true if testing succeeded, or false when every process is not on a BlackList for logged in user </returns>
        static bool TestCase2()
        {
            return Test(true);
        }

        /// <summary>
        /// Changeable test case
        /// </summary>
        static void TestManual()
        {
            string endpointConfigurationName = "";
            int serverNum;
            int processNum;
            EProcessType processToExecute = EProcessType.GoogleChrome;
            bool ok;
            bool end = false;

            do
            {
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

                /// Run process
                Run(endpointConfigurationName, processToExecute);

                Console.WriteLine("Enter x if you want to exit or any other key to continue..");
                string read = Console.ReadLine();

                if (read.Equals("x"))
                {
                    end = true;
                }

            } while (!end);

        }

        /// <summary>
        /// Function used for finding a process which is or it is not on a BlackList and trying to run a process
        /// Client communicates with Service 1!
        /// </summary>
        /// <param name="onBlackList"> true if process is on the BlackList, otherwise false </param>
        /// <returns> true if trying to run a process succeeded, otherwise false </returns>
        static bool Test(bool onBlackList)
        {
            bool retVal = false;
            EProcessType processToExecute;
            int temp = 0;
            var numberOfProcesses = Enum.GetNames(typeof(EProcessType)).Length;

            /// Choose a service 
            string endpointConfigurationName = "Contracts.IService.ServerOne";

            /// Get client's name
            WindowsIdentity winIdentity = WindowsIdentity.GetCurrent();
            string user = Formatter.ParseName(winIdentity.Name);

            while (temp <= numberOfProcesses)
            {
                /// Particular process
                processToExecute = (EProcessType)(temp++);

                /// Get forbidden users for particular process
                string value = BlackListConfiguration.GetBlackListValue(processToExecute.ToString());
                string[] usersFromBlackListPerProcess;
                if (!string.IsNullOrEmpty(value))
                {
                    usersFromBlackListPerProcess = BlackListConfiguration.GetUsers(value);

                    if (onBlackList == usersFromBlackListPerProcess.Contains(user))
                    {
                        retVal =  Run(endpointConfigurationName, processToExecute);                        
                        break;
                    }
                }
            }

            return retVal;
        }

        /// <summary>
        /// Creates a channel to particular endpoint address
        /// </summary>
        /// <param name="endpointConfigurationName"></param>
        /// <param name="processToExecute"></param>
        static bool Run(string endpointConfigurationName, EProcessType processToExecute)
        {
            bool retVal = false;

            try
            {
                using (ClientProxy proxy = new ClientProxy(endpointConfigurationName))
                {
                    try
                    {

                        if (proxy.RunProcess(processToExecute))
                        {
                            Console.WriteLine(processToExecute.ToString() + " is started!");
                            retVal = true;
                        }
                        else
                        {
                            Console.WriteLine(processToExecute.ToString() + " is not started!");
                        }
                    }
                    catch (SecurityNegotiationException se)
                    {
                        Console.WriteLine("User is not authenticated");
                        Console.WriteLine(se.Message);
                    }

                }
            }
            catch (Exception e)
            {
            }

            return retVal;
        }
    }
}
