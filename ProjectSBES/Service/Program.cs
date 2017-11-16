using Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    class Program
    {
        static void Main(string[] args)
        {
            ServiceHost _serviceHost = new ServiceHost(typeof(ServiceApp));
            _serviceHost.Open();

            Console.WriteLine("SERVER: Ready and waiting for requests. Press any key to exit.");

            Console.ReadLine();

            _serviceHost.Close();
            Console.WriteLine("SERVER: Stopped.");
        }
    }
}
