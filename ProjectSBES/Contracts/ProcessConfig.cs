using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public class ProcessConfig
    {
        //private static ResourceManager resourceManager = null;
        //private static ResourceSet resourceSet = null;
        //private static object resourceLock = new object();

        //private static ResourceManager ResourceManager
        //{
        //    get
        //    {
        //        lock (resourceLock)
        //        {
        //            if (resourceManager == null)
        //            {
        //                resourceManager = new ResourceManager(typeof(ProcessConfigFile).FullName, Assembly.GetExecutingAssembly());
        //            }

        //            return resourceManager;
        //        }
        //    }
        //}

        //private static ResourceSet ResourceSet
        //{
        //    get
        //    {
        //        lock (resourceLock)
        //        {
        //            if (resourceLock == null)
        //            {
        //                resourceLock = ResourceManager.GetResourceSet(CultureInfo.InvariantCulture, true, true);
        //            }

        //            return resourceSet;
        //        }
        //    }
        //}

        public static string GetValue(EProcessType process)
        {
            string values = null;

            switch (process)
            {
                case EProcessType.GoogleChrome:
                    values = ProcessConfigFile.GoogleChrome;
                    break;
                case EProcessType.Mozilla:
                    values = ProcessConfigFile.Mozilla;
                    break;
                case EProcessType.VisualStudio15:
                    values = ProcessConfigFile.VisualStudio15;
                    break;
                case EProcessType.Notepad:
                    values = ProcessConfigFile.Notepad;
                    break;
                default:
                    break;
            }

            return values;
        }
    }
}
