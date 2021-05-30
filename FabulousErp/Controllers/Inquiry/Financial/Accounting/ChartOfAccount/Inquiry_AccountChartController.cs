using FabulousDB.DB_Context;
using FabulousErp.MyRoleProvider;
using FabulousModels.DTOModels.Inquiry.Financial.Accounting.ChartOfAccount;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FabulousErp.Controllers.Inquiry.Financial.Accounting.ChartOfAccount
{
    [BranchSecurityFilter]
    [FactorySecurityFilter]
    [AuthorizationFilter]
    public class Inquiry_AccountChartController : Controller
    {
        DBContext DB = new DBContext();

        // GET: Inquiry_AccountChart
        [HttpGet]
       // [Authorize]
        [CustomAuthorize(Roles = "ILCOA")]
        public ActionResult ListAccountChart()
        {
            var chartID = DB.AccountChart_Table.ToList();
            SelectList ChartList = new SelectList(chartID, "AccountChartID", "AccountChartID");
            ViewBag.ChartList = ChartList;

            return View();
        }

        public JsonResult GetSearchList(string ChartID)
        {

            List<List_Account_Chart_DTO> list_Account_Chart_DTO = DB.AccountChart_Table.Where(x => x.AccountChartID == ChartID).Select(x => new List_Account_Chart_DTO
            {

                AccountChartID = x.AccountChartID,

                AccountChartName = x.AccountChartName,

                EstablishDate = x.EstablishDate

            }).ToList();  

            return Json(list_Account_Chart_DTO, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetList()
        {
            List<List_Account_Chart_DTO> list_Account_Chart_DTO = DB.AccountChart_Table.Select(x => new List_Account_Chart_DTO
            {

                AccountChartID = x.AccountChartID,

                AccountChartName = x.AccountChartName,

                EstablishDate = x.EstablishDate

            }).ToList();

            return Json(list_Account_Chart_DTO, JsonRequestBehavior.AllowGet);
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
        //            item.ILCOA = Value;
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
        //        if (item.ILCOA.ToString().Equals("True"))
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