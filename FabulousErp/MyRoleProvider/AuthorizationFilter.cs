using FabulousDB.DB_Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;

namespace FabulousErp.MyRoleProvider
{
    public class AuthorizationFilter : System.Web.Mvc.ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string UserID = Business.GetUserId();

            if (UserID == null)
            {
                Business.DelCookies();
                filterContext.Result = new RedirectResult("~/UserLogin/Login");
            }

            string CompanyID = FabulousErp.Business.GetCompanyId();
            string BranchID = "";
            string FactoryID ="";

           
            if ((UserID != "") && (CompanyID != null || BranchID != null || FactoryID != null))
            {
                if(UserID == "SA") { }
                else
                {
                    DBContext DB = new DBContext();
                    bool CheckActive = DB.CreateAccount_Tables.Where(x => x.UserID == UserID && x.DisActive != null).FirstOrDefault().DisActive.GetValueOrDefault();

                    bool CheckDelete = DB.CreateAccount_Tables.Where(x => x.UserID == UserID && x.Deleted != null).FirstOrDefault().Deleted.GetValueOrDefault();

                    bool CheckCompanyActive = false;
                    bool CheckBranchActive = false;
                    bool CheckFactoryActive = false;
                    int CompCont = DB.CompanyMainInfo_Tables.Count();
                    if (CompanyID != null)
                    {
                        if (CompCont > 1)
                        {
                            CheckCompanyActive = DB.CompanyMainInfo_Tables.Where(x => x.CompanyID == CompanyID && x.Status != null).FirstOrDefault().Status.GetValueOrDefault();
                        }
                    }
                    else if(BranchID != null)
                    {
                        if (CompCont > 1)
                            CheckBranchActive = DB.CompanyBranchInfo_Tables.Where(x => x.BranchID == BranchID && x.Status != null).FirstOrDefault().Status.GetValueOrDefault();
                    }else if(FactoryID != null)
                    {
                        if (CompCont > 1)
                            CheckFactoryActive = DB.CompanyFactoryInfo_Tables.Where(x => x.FactoryID == FactoryID && x.Status != null).FirstOrDefault().Status.GetValueOrDefault();
                    }

                    if (CheckActive == true
                        || CheckDelete == true
                        || CheckCompanyActive == true
                        || CheckBranchActive == true
                        || CheckFactoryActive == true)
                    {
                        Business.DelCookies();
                        filterContext.Result = new RedirectResult("~/UserLogin/Login");
                    }
                 
                }
            }
        }
    }
}