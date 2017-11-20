using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public enum Permissions
    {
        View = 0,
        Edit_Model = 1,
        Execute = 2,
        Edit_Config = 4
    }

    public enum Roles
    {
        Viewer = 0,
        Operator = 1,
        ModelManager = 2,
        AppAdmin = 3,
        SystemAdmin = 4
    }

    public class RolesConfiguration
    {
        static string[] AppAdminPermissions = new string[] { Permissions.View.ToString(), Permissions.Execute.ToString(), Permissions.Edit_Model.ToString() };
        static string[] OperatorPermissions = new string[] { Permissions.View.ToString(), Permissions.Execute.ToString() };
        static string[] ModelManagerPermissions = new string[] { Permissions.View.ToString(), Permissions.Edit_Model.ToString() };
        static string[] ViewerPermissions = new string[] { Permissions.View.ToString() };
        static string[] SystemAdminPermissions = new string[] { Permissions.Edit_Config.ToString() };
        static string[] Empty = new string[] { };

        public static string[] GetPermissions(string role)
        {

            switch (role)
            {
                case "Viewer": return ViewerPermissions;
                case "Operator": return OperatorPermissions;
                case "ModelManager": return ModelManagerPermissions;
                case "AppAdmin": return AppAdminPermissions;
                case "SystemAdmin": return SystemAdminPermissions;
                default: return Empty;
            }
        }
    }
}
