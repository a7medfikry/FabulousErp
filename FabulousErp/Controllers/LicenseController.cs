using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using FabulousDB.Models;
using FabulousDB;
using FabulousDB.DB_Context;
using System.Management;

namespace FabulousErp.Controllers
{
    public class LicenseController : Controller
    {
        // GET: License
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult RenewLicense()
        {
            HttpCookie Error = Request.Cookies["Error"];
            try
            {
                ViewBag.error = Error["Error"];
            }
            catch
            {
                ViewBag.error = "";
            }
            try
            {
                Error.Expires = DateTime.Now.AddMinutes(-1);
                Response.Cookies.Add(Error);
            }
            catch
            {

            }



            return View();
        }
        [HttpPost]
        public ActionResult Submit(HttpPostedFileBase file)
        {
            try
            {
                using (DBContext db = new DBContext())
                {
                    string Key = "";
                    string Value = "";
                    List<string> Text = new List<string>();
                    using (System.IO.StreamReader reader = new System.IO.StreamReader(file.InputStream))
                    {
                        while (!reader.EndOfStream)
                        {
                            Text.Add(reader.ReadLine());
                        }
                    }
                    string Txt = string.Join("", Text);
                    string RealString = Business.DecryptString(Txt, GetMachineKey());
                    string ThisMachineKey = GetMachineKey();

                    if (RealString == "")
                    {
                        TempData["Error"] = BusController.Translate("This License File Is InValid");
                        return RedirectToAction("RenewLicense");
                    }
                    Client_license ThisLicense = Enumerable.Empty<Client_license>().FirstOrDefault();
                    try
                    {
                        ThisLicense = Newtonsoft.Json.JsonConvert.DeserializeObject<Client_license>(RealString);
                    }
                    catch
                    {
                        TempData["Error"] = BusController.Translate("This License File Is InValid");
                        return RedirectToAction("RenewLicense");
                    }
                    if (ThisLicense.Machine_key != ThisMachineKey)
                    {
                        TempData["Error"] = BusController.Translate("This License Is Not For This Device");
                        return RedirectToAction("RenewLicense");
                    }
                    string ExpirationDate = Business.EncryptString("ExpirationDate", GetMachineKey());
                    db.Licenses.Add(new License
                    {
                        Key = ExpirationDate,
                        Value = Txt
                    });
                    string RealDay = Business.EncryptString("LRealDay", GetMachineKey());
                    db.Day_log.Add(new Day_log
                    {
                        Key = RealDay,
                        Value = Business.EncryptString(ThisLicense.Start_date, GetMachineKey())
                    });
                    db.SaveChanges();

                    return Redirect("/");
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = BusController.Translate("This License Is Not Valid ") + ex;

                return RedirectToAction("RenewLicense");
            }
        }


        public static DateTime ParseDate(string Date)
        {
            try
            {
                return DateTime.ParseExact(Date, "dd/MM/yyyy",
                                System.Globalization.CultureInfo.InvariantCulture);
            }
            catch
            {
                try
                {
                    Date = Date.Replace("ص", "");
                    Date = Date.Replace("م", "");
                    Date = Date.Replace("AM", "");
                    Date = Date.Replace("PM", "");
                    Date = Date.Trim();
                    return DateTime.ParseExact(Date, "dd/MM/yyyy hh:mm:ss",
                                    System.Globalization.CultureInfo.InvariantCulture);
                }
                catch
                {
                    return Convert.ToDateTime(Date);
                }
            }
        }
        public static string GetMachineKey()
        {
            string cpuInfo = string.Empty;
            ManagementClass mc = new ManagementClass("win32_processor");
            ManagementObjectCollection moc = mc.GetInstances();

            foreach (ManagementObject mo in moc)
            {
                if (cpuInfo == "")
                {
                    //Get only the first CPU's ID
                    cpuInfo = mo.Properties["processorID"].Value.ToString();
                    foreach (ManagementObject queryObj in baseboardSearcher.Get())
                    {
                        if (!queryObj["SerialNumber"].ToString().Contains("O.E.M."))
                            cpuInfo += queryObj["SerialNumber"].ToString();
                    }
                    break;
                }
            }
            return cpuInfo.Replace("To Be Filled By O.E.M.", "");
        }
        private static ManagementObjectSearcher motherboardSearcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_MotherboardDevice");
        private static ManagementObjectSearcher baseboardSearcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_BaseBoard");

    }
    public class Client_license
    {
        public string Start_date { get; set; }
        public string End_date { get; set; }
        public string Machine_key { get; set; }
    }
}