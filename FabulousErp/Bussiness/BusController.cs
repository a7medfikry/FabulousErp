using FabulousDB.DB_Context;
using FabulousDB.Models;
using FabulousErp.Controllers;
using Spire.Xls;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace FabulousErp
{
    public class BusController : Controller
    {
        public JsonResult AddToCach(string name ,string value)
        {
            Business.AddToCache(name, value);
            return Json(1);
        }
        // GET: Bus
        public ActionResult Index()
        {
            return View(); 
        }
        public ActionResult Reports()
        {
            Workbook workbook = new Workbook();
            string Path = Server.MapPath("~/Reports/TTT.xlsx");
            workbook.LoadFromFile(Path);
            Worksheet sheet = workbook.Worksheets[0];
            string ImagePath = Server.MapPath("~/Reports/Render/sample.jpg");

            sheet.SaveToImage(ImagePath);
            ViewBag.Path = "/Reports/Render/sample.jpg";
            return View();
            //DBContext db = new DBContext();

            //if (RModel.CurrentPage == 0)
            //{
            //    // Create a report object
            //    Report report = new Report();
            //    // Load the report template into the Report object.
            //    report.Report.Load("D:\\Khaled\\FastReport-master\\Demos\\Reports\\Groups.frx");
            //    // Create dataset
            //    // Register the data source in the report object
            //    report.Report.RegisterData(new System.Data.DataSet { }); //registry data source in the web report

            //    // Making the report preparation for the show
            //    report.Prepare();
            //    // Create an export object in html format
            //    HTMLExport export = new HTMLExport();
            //    // Set Export Options
            //    export.Layers = true;
            //    export.EmbedPictures = true;
            //    // Perform report export to file
            //    export.Export(report, this.Server.MapPath("/App_Data/Simple List.html"));


            //}
            //// Open the file with the desired report page
            //using (FileStream fstream = System.IO.File.OpenRead(this.Server.MapPath("/App_Data/Simple List.files/Simple List" + (RModel.CurrentPage <= RModel.PagesCount ? RModel.CurrentPage.ToString() : RModel.PagesCount.ToString()) + ".html")))
            //{
            //    byte[] array = new byte[fstream.Length];
            //    // read data
            //    fstream.Read(array, 0, array.Length);
            //    // Save The Page Content In ViewData
            //    ViewData["Report"] = Encoding.UTF8.GetString(array);
            //}
            //return View();
            //WebReport webReport = new WebReport(); //create instance of WebReport object.
            //string report_path = "D:\\Khaled\\FabulousErp_LastFromAtef\\"; //reports directory
            //System.Data.DataSet dataSet = new System.Data.DataSet(); //create data set
            //DBContext db = new DBContext();
            ////dataSet.ReadXml(report_path + "nwind.xml"); //load xml data base
            //webReport.Report.RegisterData(db.Inv_item.ToList().ToDataSet()); //registry data source in the web report
            //webReport.Report.Load(report_path + ""); //load the report to WebReport
            //ViewBag.WebReport = webReport; //send the report to View
            //var asd = webReport.Render().Result;
            //return View();
        }
        public JsonResult GetDecimal()
        {
            try
            {
                string companyID = (string)FabulousErp.Business.GetCompanyId();
                using (FabulousDB.DB_Context.DBContext dbM = new FabulousDB.DB_Context.DBContext())
                {
                    string DecimalNumber = dbM.FormateSetting_Tables.Where(x => x.CompanyID == companyID).FirstOrDefault().DecimalNumber;
                    return Json(DecimalNumber);
                }
            }
            catch
            {
                return Json(2);
            }
        }
        public JsonResult TransactionStatus(Business.FinancialForms P)
        {
            return Json(Business.GetPageTransactionSetting(P));
        }
        public JsonResult GetPotingNumber(int? JornalEntry)
        {
            try
            {
                using (DBContext Mdb = new DBContext())
                {
                    if (JornalEntry.HasValue)
                    {
                        return Json(Mdb.C_GeneralJournalEntry_Tables.FirstOrDefault(x => x.C_JournalEntryNumber == JornalEntry.Value)
                        .C_PostingNumber);
                    }
                    else
                    {
                        return Json(0);
                    }
                }
            }
            catch
            {
                return Json(0);
            }

        }
        public static int GetJournal(int? PostingNumber)
        {
            try
            {
                using (DBContext Mdb = new DBContext())
                {
                    if (PostingNumber.HasValue)
                    {
                        return Mdb.C_GeneralJournalEntry_Tables.FirstOrDefault(x => x.C_PostingNumber == PostingNumber.Value)
                        .C_JournalEntryNumber;
                    }
                    else
                    {
                        return 0;
                    }
                }
            }
            catch
            {
                return 0;
            }

        }
        public static string Translate(string Word,bool RemoveDigit=false)
        {
            if (Business.GetLanguage()== Langs.Arabic.ToString())
            {
                using (DBContext db = new DBContext())
                {
                    return db.Translates.Where(x=>x.Key.Trim()==Word.Trim()).ToList().DefaultIfEmpty(new FabulousDB.Models.Translate {Arabic=Word }).FirstOrDefault().Arabic;
                }
            }
            return Word;
        }
        public ActionResult SetLanguage(FabulousDB.Models.Langs Lang)
        {
            HttpCookie LangCo = new HttpCookie("Lang");
            LangCo["Lang"] = Lang.ToString();
            //LangCo.Expires = new DateTime(2020, 12, 31);
            Response.Cookies.Add(LangCo);
            return Redirect("/Home/Financial_Home");
        }
        public FileResult TakeBackup()
        {
            using (DBContext db = new DBContext())
            {
                db.Database.ExecuteSqlCommand(TransactionalBehavior.DoNotEnsureTransaction,"exec [TakeBackup]");
                DirectoryInfo DirInfo = new DirectoryInfo(db.Client_DB.FirstOrDefault().Path);


                var MyFile = DirInfo.GetFiles()
             .OrderByDescending(f => f.CreationTime)
             .First();

              

                byte[] filedata = System.IO.File.ReadAllBytes(MyFile.FullName);

                return File(filedata,"bak", MyFile.Name);
            }

            return null;
        }
        public ActionResult RestoreData(string Msg="")
        {
            ViewBag.Msg = Msg;
            return View();
        }
        public ActionResult RestoreFileData()
        {
            try
            {
                FabulousErp.Controllers.UploadFileController Up = new Controllers.UploadFileController();

                string FileName = UploadImage();
                using (LogContext Ldb = new LogContext())
                {
                    Ldb.Database.ExecuteSqlCommand(TransactionalBehavior.DoNotEnsureTransaction, $"exec RestoreDb {FileName}");
                }
                DBContext db = new DBContext();
                var Test = db.CompanyMainInfo_Tables.ToList();
                ViewBag.Done = 1;
                return RedirectToAction("RestoreData",new { Msg = "Done" });

            }
            catch
            {
                ViewBag.Done = 0;
                return RedirectToAction("RestoreData",new { Msg="Error The Restore Has Been Faild" });
            }
        }
        public string UploadImage()
        {
            try
            {

                 HttpPostedFileBase file = Request.Files[0];
                string filename = (file.FileName);
                string fileExtension = System.IO.Path.GetExtension(file.FileName);
                DBContext db = new DBContext();
                string savepath = Server.MapPath(db.Client_DB.FirstOrDefault().Path);

                // filename = filename.Trim(fileExtension.ToCharArray()) + fileExtension;
                // If Image Name Is Dublicated
                filename = DateTime.Now.ToString("yyyy-MM-dd");
                if (System.IO.File.Exists(savepath + @"\" + filename))
                {
                    int counter = 1;
                    string FileName = System.IO.Path.GetFileNameWithoutExtension(filename) + counter + fileExtension;
                    while (System.IO.File.Exists(savepath + @"\" + FileName))
                    {
                        // if a file with this name already exists,
                        // prefix the filename with a number.
                        counter++;
                        FileName = System.IO.Path.GetFileNameWithoutExtension(filename) + counter + fileExtension;

                    }
                    filename = FileName;
                }
                file.SaveAs(savepath + @"\" + filename);
                return savepath + @"\" + filename;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
    public static class RModel
    {
        public static int PagesCount = 0;
        public static int CurrentPage = 0;
    }
    public class Sheet
    {
        public string SheetName { get; set; }
        public string Path { get; set; }
    }
}