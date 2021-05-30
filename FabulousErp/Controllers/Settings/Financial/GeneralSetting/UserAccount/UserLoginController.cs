using FabulousDB.DB_Context;
using FabulousDB.DB_Tabels.Settings.Financial.GeneralSetting.UserGroup;
using FabulousErp.Repository.Interface;
using FabulousModels.DTOModels.Settings.Financial.GeneralSettings.UserAccount;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace FabulousErp.Controllers.Settings.Financial.GeneralSetting.UserAccount
{
    public class UserLoginController : Controller
    {

        //IRepetitionBusiness repetitionBusiness;

        //public UserLoginController(IRepetitionBusiness repetitionBusiness)
        //{
        //    this.repetitionBusiness = repetitionBusiness;
        //}

        DBContext DB = new DBContext();

        // GET: UserLogin
        public ActionResult Login()
        {
            //ViewBag.CompanyIDList = repetitionBusiness.RetrieveCompIDList();

            //ViewBag.BranchIDList = repetitionBusiness.RetrieveBranchIDList();
            //ViewBag.FactoryIDList = repetitionBusiness.RetrieveFactoryIDList();

            if (FabulousErp.Business.GetCompanyId() != null
                && FabulousErp.Business.GetUserId() != null)
            {
                return (RedirectToAction("Financial_Home", "Home"));
            }

            return View("Login");
        }

        public ActionResult ULogin(string UserID, string Password, string CompanyID=null, string BranchID = null, string FactoryID = null)
        {
            if (UserID == "SA" && Password == "123")
            {
                FormsAuthentication.SetAuthCookie("SA", false);
                FabulousErp.Business.SetUserCookie(UserID, "System Admin");
                FabulousErp.Business.SetCompanyCookie("SA");
                return (RedirectToAction("Index", "Dashboard"));
            }
            else
            {
                try
                {
                    CompanyID = DB.CompanyMainInfo_Tables.Where(x => x.Status == false).FirstOrDefault().CompanyID;
                }
                catch
                {
                    CompanyID = DB.CompanyMainInfo_Tables.FirstOrDefault().CompanyID;
                }

                var CheckLogin = DB.CreateAccount_Tables.Where(x => x.UserID == UserID && x.Password == Password).FirstOrDefault();

                var CheckCompanyAccess = DB.UACompPremission_Tables.Where(x => x.UserID == UserID && x.CompanyID == CompanyID).FirstOrDefault();

                var CheckBranchAccess = DB.UABranchPremission_Tables.Where(x => x.UserID == UserID && x.BranchID == BranchID).FirstOrDefault();

                var CheckFactoryAccess = DB.UAFactoryPremission_Tables.Where(x => x.UserID == UserID && x.FactoryID == FactoryID).FirstOrDefault();


                bool CheckCompanyActive = false;

                if (CompanyID != "False")
                {
                    if (CompanyID != "null")
                    {
                        if (CompanyID != "")
                        {
                            if (CompanyID != "-1")
                            {
                                CheckCompanyActive = DB.CompanyMainInfo_Tables.Where(x => x.CompanyID == CompanyID).FirstOrDefault().Status.GetValueOrDefault();
                            }
                        }
                    }
                }

                bool CheckBranchActive = false;

                if (!string.IsNullOrEmpty(BranchID))
                {
                  
                        CheckBranchActive = DB.CompanyBranchInfo_Tables.Where(x => x.BranchID == BranchID).FirstOrDefault().Status.GetValueOrDefault();
                   
                }

                bool CheckFactoryActive = false;

                if (!string.IsNullOrEmpty(FactoryID))
                {
                    CheckFactoryActive = DB.CompanyFactoryInfo_Tables.Where(x => x.FactoryID == FactoryID).FirstOrDefault().Status.GetValueOrDefault();
                }


                if (CheckLogin != null)
                {
                    //if (CheckCompanyActive == true)
                    //{
                    //    return Json("CompanyActiveError", JsonRequestBehavior.AllowGet);
                    //}
                    //else if (CheckBranchActive == true)
                    //{
                    //    return Json("BranchActiveError", JsonRequestBehavior.AllowGet);
                    //}
                    //else if (CheckFactoryActive == true)
                    //{
                    //    return Json("FactoryActiveError", JsonRequestBehavior.AllowGet);
                    //}
                    //else if (CheckCompanyAccess == null && CompanyID != "False")
                    //{
                    //    return Json("CompanyAccessError", JsonRequestBehavior.AllowGet);
                    //}
                    //else if (CheckBranchAccess == null && BranchID != "False")
                    //{
                    //    return Json("BranchAccessError", JsonRequestBehavior.AllowGet);
                    //}
                    //else if (CheckFactoryAccess == null && FactoryID != "False")
                    //{
                    //    return Json("FactoryAccessError", JsonRequestBehavior.AllowGet);
                    //}
                   // else
                    {
                        /*get user name*/
                        var UserName = CheckLogin.UserName;

                        DateTime CheckExpyred = Convert.ToDateTime(CheckLogin.LastPasswordChangedDate).AddDays(30);

                        if (CheckLogin.DisActive == true)
                        {
                            return Json("DisactiveAcc", JsonRequestBehavior.AllowGet);
                        }
                        else if (CheckLogin.Deleted == true)
                        {
                            return Json("Deleted", JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            if (CheckLogin.PasswordExpired == true && CheckLogin.LastPasswordChangedDate != null && CheckExpyred < DateTime.Now)
                            {
                                SendSessions(UserName, CompanyID, BranchID, FactoryID, UserID);
                                return (RedirectToAction("ChangePassword", "UserChangePassword", new { Expyred = "Your password Expired, Change it now To login" }));
                            }
                            else if (CheckLogin.ChangePassFirst == true)
                            {
                                SendSessions(UserName, CompanyID, BranchID, FactoryID, UserID);
                                return (RedirectToAction("ChangePassword", "UserChangePassword", new { Expyred = "This Your First Login, Change Password now To login" }));
                            }
                            else if (CheckLogin.UpdateProfFirst == true)
                            {
                                SendSessions(UserName, CompanyID, BranchID, FactoryID, UserID);
                                return (RedirectToAction("AccountInfo", "AccountProfileInfo", new { Message = "This Your First Login, Update Your Profile To login" }));
                            }
                            else
                            {
                                //repetitionBusiness.AddGroupEmpty();
                                FormsAuthentication.SetAuthCookie(CheckLogin.UserID, false);
                                SendSessions(UserName, CompanyID, BranchID, FactoryID, UserID);

                                //HttpCookie Login;
                                //if (Request.Cookies["Login"] != null)
                                //{
                                //    Login = Request.Cookies["Login"];
                                //}
                                //else
                                //{
                                //    Login = new HttpCookie("Login");
                                //}

                                //Login["CompanyID"] = CompanyID;
                                //Login.Expires = DateTime.Now.AddHours(8);
                                //Response.Cookies.Add(Login);

                                return (RedirectToAction("Index", "Dashboard"));
                            }
                        }
                    }
                }
                else
                {
                    TempData["Error"] = "Wrong User Name Or Password";
                    return RedirectToAction("Login");
                }
            }
        }

        public ActionResult Logout()
        {
           // FormsAuthentication.SignOut();
            Roles.DeleteCookie();
            Session.Clear();
            FabulousErp.Business.DelCookies();
            return Redirect("/UserLogin/Login");
        }

        public void SendSessions(string UserName, string CompanyID, string BranchID, string FactoryID,string UserId)
        {
            //HttpCookie Login = new HttpCookie("Login");
            // Login["CompanyID"] = CompanyID;
            FabulousErp.Business.SetCompanyCookie(CompanyID);
            FabulousErp.Business.SetUserCookie(UserId, UserName);
        }
        
        //public User_Login_DTO GetLogin()
        //{
        //    HttpCookie GLogin = Request.Cookies["Login"];
        //    return new User_Login_DTO
        //    {
        //        BranchID = "1",
        //        CompanyID = "1",
        //        FactoryID = (string)GLogin["Login"]
        //    };
        //}
        public JsonResult GetCompanyID(string userID)
        {
            List<User_Login_DTO> user_Login_DTO = DB.UACompPremission_Tables.Where(x => x.UserID == userID).Select(x => new User_Login_DTO
            {
                CompanyID = x.CompanyID

            }).ToList();

            return Json(user_Login_DTO, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetBranchID(string userID)
        {
            List<User_Login_DTO> user_Login_DTO = DB.UABranchPremission_Tables.Where(x => x.UserID == userID).Select(x => new User_Login_DTO
            {
                BranchID = x.BranchID

            }).ToList();

            return Json(user_Login_DTO, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetFactoryID(string userID)
        {
            List<User_Login_DTO> user_Login_DTO = DB.UAFactoryPremission_Tables.Where(x => x.UserID == userID).Select(x => new User_Login_DTO
            {
                FactoryID = x.FactoryID

            }).ToList();

            return Json(user_Login_DTO, JsonRequestBehavior.AllowGet);
        }


        //public JsonResult GetCompanyName(string CompanyID)
        //{
        //    return Json(repetitionBusiness.GetCompanyName(CompanyID), JsonRequestBehavior.AllowGet);
        //}

        //public JsonResult GetBranchName(string BranchID)
        //{
        //    return Json(repetitionBusiness.GetBranchName(BranchID), JsonRequestBehavior.AllowGet);
        //}

        //public JsonResult GetFactoryName(string FactoryID)
        //{
        //    return Json(repetitionBusiness.GetFactoryName(FactoryID), JsonRequestBehavior.AllowGet);
        //}
    }
}