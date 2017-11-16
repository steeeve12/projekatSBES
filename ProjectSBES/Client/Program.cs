using Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            using (ClientProxy proxy = new ClientProxy("Contracts.IService"))
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
