using FabulousErp.Repository.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace FabulousErp.MyRoleProvider
{
    public class CustomAuthorize : System.Web.Mvc.AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            // If they are authorized, handle accordingly
 
            /*this.AuthorizeCore(filterContext.HttpContext)*/
            //{
            //    this.
            //    base.OnAuthorization(filterContext);
            //}
            if (!SiteRole.UserHasRole(this.Roles))
            {
                filterContext.Result = new RedirectResult("~/Error/Error");
            }
        }
    }
}