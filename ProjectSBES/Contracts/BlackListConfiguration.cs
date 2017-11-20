using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public enum Roles
    {
        RegularUser = 0,
        Admin = 1,
        Moderator = 2,
    }

    public class BlackListConfiguration
    {
        public static string GetBlackListValue(string processName)
        {
            string values = null;

            switch (processName)
            {
                case "GoogleChrome":
                    values = BlackListFile.GoogleChrome;
                    break;
                case "Mozilla":
                    values = BlackListFile.Mozilla;
                    break;
                case "VisualStudio15":
                    values = BlackListFile.VisualStudio15;
                    break;
                case "Notepad":
                    values = BlackListFile.Notepad;
                    break;
                default:
                    break;
            }

            return values;
        }

        public static string[] GetUsers(string blackListValue)
        {
            string[] values = blackListValue.Split('!');
            string[] users = null;

            if (values.Length != 1)
                users = values[1].Split(',');
            else
                users = values[0].Split(',');

            return users;
        }

        public static string[] GetRoles(string blackListValue)
        {
            string[] values = blackListValue.Split('!');

            string[] roles = values[0].Split(',');

            return roles;
        }
    }
}
