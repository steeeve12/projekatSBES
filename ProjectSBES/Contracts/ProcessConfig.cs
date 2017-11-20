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
