using FabulousDB.DB_Context;
using FabulousErp.MyRoleProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace FabulousErp.Controllers.Settings.Financial.GeneralSetting.UserAccount
{
    [AuthorizationFilter]
    public class UserChangePasswordController : Controller
    {

        DBContext DB = new DBContext();

        // GET: ChangePassword
        [HttpGet]
        public ActionResult ChangePassword(string Expyred)
        {
            ViewBag.Expyred = Expyred;
            return View();
        }

        public ActionResult UChangePass(string OldPassword, string NewPassword)
        {
            string UserID = FabulousErp.Business.GetUserId();

            string CompanyID = (string)FabulousErp.Business.GetCompanyId();

            var checkOldPass = DB.CreateAccount_Tables.Where(x => x.UserID == UserID && x.oldPassword == OldPassword).FirstOrDefault();
            
            
            if (checkOldPass != null)
            {
                if (OldPassword == NewPassword)
                {
                    return Json("ONIdentical", JsonRequestBehavior.AllowGet);
                }
                else
                {

                    if (checkOldPass.UpdateProfFirst == true)
                    {

                        var UpdatePass = DB.CreateAccount_Tables.Where(x => x.UserID == UserID).FirstOrDefault();

                        UpdatePass.Password = NewPassword;

                        UpdatePass.LastPasswordChangedDate = DateTime.Now.ToShortDateString();

                        UpdatePass.ChangePassFirst = false;

                        DB.SaveChanges();

                        return (RedirectToAction("AccountInfo", "AccountProfileInfo", new { Message = "This Your First Login, Update Your Profile To login" }));
                    }
                    else
                    {

                        var UpdatePass = DB.CreateAccount_Tables.Where(x => x.UserID == UserID).FirstOrDefault();

                        UpdatePass.Password = NewPassword;

                        UpdatePass.LastPasswordChangedDate = DateTime.Now.ToShortDateString();

                        UpdatePass.ChangePassFirst = false;

                        DB.SaveChanges();

                        FormsAuthentication.SetAuthCookie(UserID, false);
                        return (RedirectToAction("dashboard", "Dashboard"));
                    }
                }
            }
            else
            {
                return Json("OldPassError", JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult UChangePass2(string OldPassword, string NewPassword)
        {
            string UserID = FabulousErp.Business.GetUserId();


            string CompanyID = (string)FabulousErp.Business.GetCompanyId();

            var checkOldPass = DB.CreateAccount_Tables.Where(x => x.UserID == UserID && x.oldPassword == OldPassword).FirstOrDefault();

            if (checkOldPass.UpdateProfFirst == true)
            {

                var UpdatePass = DB.CreateAccount_Tables.Where(x => x.UserID == UserID).FirstOrDefault();

                UpdatePass.Password = NewPassword;

                UpdatePass.oldPassword = NewPassword;

                UpdatePass.LastPasswordChangedDate = DateTime.Now.ToShortDateString();

                UpdatePass.ChangePassFirst = false;

                DB.SaveChanges();

                return (RedirectToAction("AccountInfo", "AccountProfileInfo", new { Message = "This Your First Login, Update Your Profile To login" }));
            }
            else
            {
                var UpdatePass = DB.CreateAccount_Tables.Where(x => x.UserID == UserID).FirstOrDefault();

                UpdatePass.Password = NewPassword;

                UpdatePass.oldPassword = NewPassword;

                UpdatePass.LastPasswordChangedDate = DateTime.Now.ToShortDateString();

                UpdatePass.ChangePassFirst = false;

                DB.SaveChanges();


                FormsAuthentication.SetAuthCookie(UserID, false);
                Session.Remove("UserID");
                return (RedirectToAction("dashboard", "Dashboard"));
            }
        }

       
    }
}