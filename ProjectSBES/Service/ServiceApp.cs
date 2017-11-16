using Contracts;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class ServiceApp : IService
    {
        public bool RunProcess(EProcessType process)
        {
            string processName = ProcessConfig.GetValue(process);

            if (processName != null)
            {
                Process.Start(processName);
                return true;
            }

            return false;
        }
    }
}
