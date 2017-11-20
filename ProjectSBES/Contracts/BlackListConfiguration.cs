using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Resources;
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
        private static ResourceManager resourceManager = null;
        private static ResourceSet resourceSet = null;
        private static object resourceLock = new object();

        private static ResourceManager ResourceManager
        {
            get
            {
                lock (resourceLock)
                {
                    if (resourceManager == null)
                    {
                        resourceManager = new ResourceManager(typeof(BlackListFile).FullName, Assembly.GetExecutingAssembly());
                    }

                    return resourceManager;
                }
            }
        }

        public static ResourceSet ResourceSet
        {
            get
            {
                lock (resourceLock)
                {
                    if (resourceSet == null)
                    {
                        resourceSet = ResourceManager.GetResourceSet(CultureInfo.InvariantCulture, true, true);
                    }

                    return resourceSet;
                }
            }
        }

        public static string GetBlackListValue(string processName)
        {
            string values = null;
            string path = "../../../Contracts/BlackListFile.resx";

            ResXResourceReader reader = new ResXResourceReader(path);
            foreach (DictionaryEntry node in reader)
            {
                if (processName == node.Key.ToString())
                {
                    values = node.Value.ToString();
                    break;
                }
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
