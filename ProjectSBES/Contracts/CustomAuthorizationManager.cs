using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public class CustomAuthorizationManager : ServiceAuthorizationManager
    {
        protected override bool CheckAccessCore(OperationContext operationContext)
        {
            //bool authorized = false;
            IPrincipal principal = operationContext.ServiceSecurityContext.AuthorizationContext.Properties["Principal"] as IPrincipal;

            if (principal != null)
            {
                //    authorized = (principal as CustomPrincipal).IsInRole(Permissions.View.ToString());

                //    if (authorized == false)
                //    {
                //        /// audit authorization failed event	
                //        Audit.AuthorizationFailed(principal.Identity.Name, OperationContext.Current.IncomingMessageHeaders.Action, "Authorization failed. You don't have permission for this operation.");
                //    }
                //    else
                //    {
                //        /// audit successfull authorization event
                //        /// Audit.AuthorizationSuccess(principal.Identity.Name, OperationContext.Current.IncomingMessageHeaders.Action);
                //    }

                return true;
            }

            return false;
        }
    }
}
