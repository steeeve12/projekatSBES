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
            using (ClientProxy proxy = new ClientProxy("Service.IService"))
            {
                if(proxy.RunProcess(EProcessType.GoogleChrome))
                {
                    Console.WriteLine("Google Chrome is started!");
                }
                else
                {
                    Console.WriteLine("Google Chrome is not started!");
                }         
            }
            Console.ReadLine();
        }
    }
}
