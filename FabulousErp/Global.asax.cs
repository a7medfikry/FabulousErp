using FabulousDB.DB_Context;
using FabulousDB.DB_Tabels.Important;
using FabulousDB.Models;
using FabulousErp.Controllers;
using FabulousErp.Repository.Business;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace FabulousErp
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            UnityConfig.RegisterComponents();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-GB");
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-GB");
        }
        protected void Application_Error(object sender, EventArgs e)
        {
            DBContext DB = new DBContext();

            Exception exception = Server.GetLastError();

            if (exception != null )
            {
                string error = exception.ToString();
                if (exception.InnerException != null)
                {
                    error += "InnerException " + exception.InnerException.ToString();
                }
                Exceptions_Table exceptions_Table = new Exceptions_Table()
                {
                    URL = Request.Url.AbsolutePath,
                    Exception = error,
                    Date = DateTime.Now
                };
                DB.Exceptions_Tables.Add(exceptions_Table);
                DB.SaveChanges();

                // clear error on server
                Server.ClearError();
            }

            //Request.Url.AbsolutePath;
            //to database



        }
        protected void Application_BeginRequest()
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-GB");
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-GB");

            CultureInfo newCulture = (CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
            newCulture.DateTimeFormat.ShortDatePattern = "dd-MM-yyyy";
            newCulture.DateTimeFormat.DateSeparator = "-";
            Thread.CurrentThread.CurrentCulture = newCulture;
            Thread.CurrentThread.CurrentUICulture = newCulture;


            DBContext db = new DBContext();
            HttpCookie Login = Request.Cookies["Login"];

            #region License
            //License
            try
            {
                bool Fake = false;

                string MachineKey = LicenseController.GetMachineKey();

                string RealDay = FabulousErp.Business.EncryptString("RealDay", MachineKey);

                string LRealDay = FabulousErp.Business.EncryptString("LRealDay", MachineKey);

                Day_log FirstLisence = db.Day_log.Where(x => x.Key == LRealDay).ToList().LastOrDefault();
                string ExpirationDate = Business.EncryptString("ExpirationDate", MachineKey);


                License ThisConfig = db.Licenses.Where(x => x.Key == ExpirationDate).ToList().OrderByDescending(x => x.Id).FirstOrDefault();
                DateTime CurrentClaimedDay = LicenseController.ParseDate(DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"));
                Day_log LastDayConfig = db.Day_log.Where(x => x.Key == RealDay).ToList().DefaultIfEmpty(new Day_log
                {
                    Value = Business.EncryptString(CurrentClaimedDay.ToString(), MachineKey)
                }).LastOrDefault();
                DateTime LastDayConfigDate = LicenseController.ParseDate(Business.DecryptString(LastDayConfig.Value, MachineKey));


                DateTime CurrentDate = LicenseController.ParseDate(DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"));


                Client_license ThisLicense = null;

                try
                {
                    ThisLicense = Newtonsoft.Json.JsonConvert.DeserializeObject<Controllers.Client_license>(Business.DecryptString(ThisConfig.Value, MachineKey));
                }
                catch
                {
                    
                    if (!Request.Url.AbsolutePath.StartsWith("/License"))
                    {
                        if (!Request.Path.Contains("."))
                        Response.Redirect("/License/RenewLicense", false);
                    }
                }
                if (!db.Day_log.Any())
                {
                    try
                    {
                        db.Day_log.Add(new Day_log
                        {
                            Key = RealDay,
                            Value = Business.EncryptString(ThisLicense.Start_date, MachineKey)
                        });
                        db.SaveChanges();
                    }
                    catch
                    {
                    }

                }

                string RealDayEnc = Business.EncryptString(CurrentClaimedDay.ToString(), MachineKey);
                if (LastDayConfig.Value != RealDayEnc)
                {

                    if (db.Day_log.OrderByDescending(x => x.Id).FirstOrDefault().Value != RealDayEnc)
                    {
                        db.Day_log.Add(new Day_log
                        {
                            Key = RealDay,
                            Value = RealDayEnc
                        });
                        db.SaveChanges();
                    }

                }
                else
                {

                    if (db.Day_log.Where(x => x.Key == RealDay).ToList().Where(x => x.Value == Business.EncryptString(CurrentClaimedDay.ToString(), MachineKey)).Count() > 1)
                    {
                        Fake = true;
                    }
                }

                if (Fake)
                {
                    if (!Request.Url.AbsolutePath.StartsWith("/License")
                        )
                    {
                        if (!Request.Path.Contains("."))
                            Response.Redirect("/License/RenewLicense", false);
                    }
                }

                if (ThisLicense.Machine_key != MachineKey)
                {

                    if (!Request.Url.AbsolutePath.StartsWith("/License")
                        )
                    {
                        if (!Request.Path.Contains("."))
                            Response.Redirect("/License/RenewLicense", false);
                    }
                }
                DateTime ExpierdDateTime = LicenseController.ParseDate(ThisLicense.End_date);

                if (FirstLisence == null)
                {

                    if (!Request.Url.AbsolutePath.StartsWith("/License")
                         )
                    {
                        if (!Request.Path.Contains("."))
                            Response.Redirect("/License/RenewLicense", false);
                    }
                }
                else
                {

                    DateTime LDateTime = LicenseController.ParseDate(FabulousErp.Business.DecryptString(FirstLisence.Value, MachineKey));
                    if (CurrentDate < LDateTime)
                    {

                        if (!Request.Url.AbsolutePath.StartsWith("/License")
                             )
                        {
                            if (!Request.Path.Contains("."))
                                Response.Redirect("/License/RenewLicense", false);
                        }
                    }
                }

                if (CurrentDate >= ExpierdDateTime)
                {

                    if (!Request.Url.AbsolutePath.StartsWith("/License")
                         )
                    {
                        if (!Request.Path.Contains("."))
                            Response.Redirect("/License/RenewLicense", false);
                    }
                }

            }
            catch (Exception ex)
            {
                string Exp = "";
                HttpCookie Error = new HttpCookie("Error");
                Error["Error"] = Convert.ToString(ex);
                Exp = ex.ToString();
                try
                {
                    Error["Error"] += " " + ex.InnerException;
                    Exp += " " + ex.InnerException;
                }
                catch
                {

                }
                try
                {
                    db.Exceptions_Tables.Add(new Exceptions_Table
                    {
                        Exception = Exp,
                        Date = DateTime.Now,
                    });
                    db.SaveChanges();
                }
                catch
                {

                }
                Error.Expires = DateTime.Now.AddMinutes(1);
                HttpContext.Current.Response.Cookies.Add(Error);
                if (!Request.Url.AbsolutePath.StartsWith("/License")
                    )
                {
                    if (!Request.Path.Contains("."))
                    Response.Redirect("/License/RenewLicense", false);
                }
            }
            #endregion

            try
            {
                if (Request.Path != "/" && !Request.Path.StartsWith("/UserLogin")
                    && !Request.Url.AbsolutePath.StartsWith("/License")
                    && !(Request.Path.Contains(".")
                        || Request.Path == "/Js"
                        || Request.Path == "/Css"
                        //|| Request.Path.Contains(".css")
                        //|| Request.Path.Contains(".png")
                        //|| Request.Path.Contains(".jpg")
                        //|| Request.Path.Contains(".ico")
                        //|| Request.Path=="/Js"
                        //|| Request.Path=="/Css"
                        ))
                {
                    if (Business.GetCompanyId() == null)
                    {
                        Response.Redirect("/", true);
                    }
                }
            }
            catch
            {
                Response.Redirect("/", true);
            }

            if (Login != null)
            {
                string UserName = Login["UserID"];
                string Link = Request.Url.AbsolutePath;
                Link = Business.GetCleanLink(Link);

                if (db.Pages.Any(x => x.Link == Link))
                {
                    try
                    {
                        int PageId = db.Pages.FirstOrDefault(x => x.Link == Link).Id;
                        bool Access = db.UsersPageAccess.FirstOrDefault(x => x.UserID == UserName && x.Page_id == PageId).View;
                        if (!Access)
                        {
                            Response.Redirect("/Error/Error");
                        }
                    }
                    catch (Exception ex)
                    {
                        Response.Redirect("/Error/Error");
                    }
                }
            }

        }
        
    }

}
