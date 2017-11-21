using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public class Formatter
    {
        /// <summary>
        /// Returns username based on the Windows Logon Name. 
        /// </summary>
        /// <param name="winLogonName"> Windows logon name can be formatted either as a User Principal Name or a Service Principal Name </param>
        /// <returns> username </returns>
        public static string ParseName(string winLogonName)
        {
            string[] parts = new string[] { };

            if (winLogonName.Contains("@"))
            {
                ///UPN format
                parts = winLogonName.Split('@');
                return parts[0];
            }
            else if (winLogonName.Contains("\\"))
            {
                /// SPN format
                parts = winLogonName.Split('\\');
                return parts[1];
            }
            else if (winLogonName.Contains("="))
            {
                parts = winLogonName.Split('=');
                parts = parts[1].Split(';');
                return parts[0];
            }
            else
            {
                return winLogonName;
            }
        }
    }
}
