using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public class CustomPrincipal : IPrincipal, IDisposable
    {
        private WindowsIdentity identity = null;
        private List<string> roles = new List<string>();
        private static string userIdentity;

        public CustomPrincipal(WindowsIdentity winIdentity)
        {
            this.identity = winIdentity;

            /// define list of roles based on Windows groups (roles) 			 
            foreach (IdentityReference group in this.identity.Groups)
            {
                SecurityIdentifier sid = (SecurityIdentifier)group.Translate(typeof(SecurityIdentifier));
                var name = sid.Translate(typeof(NTAccount));
                string groupName = Formatter.ParseName(name.ToString());    /// return name of the Windows group				

                if (!roles.Contains(groupName))
                {
                    roles.Add(groupName);
                }
            }
        }

        public IIdentity Identity
        {
            get { return this.identity; }
        }

        public void Dispose()
        {
            if (identity != null)
            {
                identity.Dispose();
                identity = null;
            }
        }

        public bool IsInRole(string processName)
        {
            bool isUserOnBlackList = false;

            string blackListValue = BlackListConfiguration.GetBlackListValue(processName);

            UserIdentity = this.identity.Name.Substring(this.identity.Name.LastIndexOf("\\") + 1);

            if (blackListValue != null)
            {
                string[] roles = BlackListConfiguration.GetRoles(blackListValue);
                string[] users = BlackListConfiguration.GetUsers(blackListValue);

                if (!CheckRoles(roles))
                {
                    if (!CheckUsers(users))
                    {
                        isUserOnBlackList = false;
                    }
                    else
                    {
                        isUserOnBlackList = true;
                    }
                }
                else
                {
                    isUserOnBlackList = true;
                }
            }

            return isUserOnBlackList;
        }

        private bool CheckRoles(string[] roles)
        {
            if (!roles[0].Equals(""))
            {
                foreach (string role in roles)
                {
                    if (this.roles.Contains(role))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private bool CheckUsers(string[] users)
        {
            if (!users[0].Equals(""))
            {
                foreach (string user in users)
                {
                    if (user.Equals(UserIdentity))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public static string UserIdentity
        {
            get
            {
                return userIdentity;
            }
            set
            {
                userIdentity = value;
            }
        }
    }
}
