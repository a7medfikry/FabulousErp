using FabulousDB.DB_Context;
using FabulousDB.DB_Tabels.Settings.Financial.GeneralSetting.Post;
using FabulousErp.MyRoleProvider;
using FabulousErp.Repository.Interface;
using FabulousModels.DTOModels.Settings.Financial.GeneralSettings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FabulousErp.Controllers.Settings.Financial.GeneralSetting.Post
{
    [BranchSecurityFilter]
    [FactorySecurityFilter]
    [AuthorizationFilter]
    public class PrintDocumentSettingController : Controller
    {

        IRepetitionBusiness repetitionBusiness;

        public PrintDocumentSettingController(IRepetitionBusiness repetitionBusiness)
        {
            this.repetitionBusiness = repetitionBusiness;
        }

        DBContext DB = new DBContext();

        // GET: PrintDocumentSetting
        [HttpGet]
       // [Authorize]
        [CustomAuthorize(Roles = "SPD")]
        public ActionResult PrintDocument()
        {
            string companyID = (string)FabulousErp.Business.GetCompanyId();

            ViewBag.CompanyList = repetitionBusiness.RetrieveCompIDListCond(companyID);

            return View();
        }



        public JsonResult GetCompanyName(string companyID)
        {
            return Json(repetitionBusiness.GetCompanyName(companyID), JsonRequestBehavior.AllowGet);
        }


        public JsonResult SavePrintDocument(string companyID, string module, string tFormName, bool ask, bool printDirect, bool analytic, bool costCenter)
        {
            var checkDuplicate = DB.PrintDocument_Tables.Where(x => x.CompanyID == companyID && x.Module == module && x.TransactionFormName == tFormName).FirstOrDefault();

            if (checkDuplicate == null)
            {
                PrintDocument_Table printDocument_Table = new PrintDocument_Table()
                {
                    CompanyID = companyID,

                    Module = module,

                    TransactionFormName = tFormName,

                    Ask = ask,

                    PrintDirect = printDirect,

                    Analytic = analytic,

                    CostCenter = costCenter
                };

                DB.PrintDocument_Tables.Add(printDocument_Table);
                DB.SaveChanges();
            }

            return null;
        }


        public JsonResult GetPrintDocument(string companyID, string module, string tFormName)
        {
            var getData = DB.PrintDocument_Tables.Where(x => x.CompanyID == companyID && x.Module == module && x.TransactionFormName == tFormName).FirstOrDefault();

            if (getData != null)
            {
                User_Post_DTO user_Post_DTO = new User_Post_DTO()
                {
                    Ask = getData.Ask,

                    PrintDirect = getData.PrintDirect,

                    Analytic = getData.Analytic,

                    CostCenter = getData.CostCenter
                };
                return Json(user_Post_DTO, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("False", JsonRequestBehavior.AllowGet);
            }
        }


        public JsonResult UpdatePrintDocument(string companyID, string module, string tFormName, bool ask, bool printDirect, bool analytic, bool costCenter)
        {
            var getRow = DB.PrintDocument_Tables.Where(x => x.CompanyID == companyID && x.Module == module && x.TransactionFormName == tFormName).FirstOrDefault();

            if(getRow != null)
            {
                int id = getRow.PD_ID;

                var updateData = DB.PrintDocument_Tables.Where(x => x.PD_ID == id).FirstOrDefault();

                updateData.Ask = ask;
                updateData.PrintDirect = printDirect;
                updateData.Analytic = analytic;
                updateData.CostCenter = costCenter;

                DB.SaveChanges();
            }

            return null;
        }



        //public JsonResult AddFavourites(bool Value)
        //{
        //    string UserID = FabulousErp.Business.GetUserId();

        //    if (UserID == "SA")
        //    {
        //        return Json(JsonRequestBehavior.AllowGet);
        //    }
        //    else
        //    {

        //        var item = DB.FavouritesForms_Tables.Where(x => x.UserID == UserID).FirstOrDefault();
        //        if (item != null)
        //        {
        //            item.SPD = Value;
        //            DB.SaveChanges();
        //        }
        //        return Json(JsonRequestBehavior.AllowGet);
        //    }
        //}

        //public JsonResult CheckFavourites()
        //{
        //    string UserID = FabulousErp.Business.GetUserId();

        //    if (UserID == "SA")
        //    {
        //        return Json(JsonRequestBehavior.AllowGet);
        //    }
        //    else
        //    {

        //        var item = DB.FavouritesForms_Tables.Where(x => x.UserID == UserID).FirstOrDefault();
        //        if (item.SPD.ToString().Equals("True"))
        //        {
        //            return Json("True", JsonRequestBehavior.AllowGet);
        //        }
        //        else
        //        {
        //            return Json("False", JsonRequestBehavior.AllowGet);
        //        }
        //    }
        //}

    }
}