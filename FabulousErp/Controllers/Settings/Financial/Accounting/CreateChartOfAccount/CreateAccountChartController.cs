using FabulousDB.DB_Context;
using FabulousDB.DB_Tabels.Settings.Financial.Accounting.CreateChartOfAccount;
using FabulousErp.MyRoleProvider;
using FabulousModels.DTOModels.Settings.Financial.Accounting.CreateChartOfAccount;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FabulousErp.Controllers.Settings.Financial.Accounting.CreateChartOfAccount
{
    [BranchSecurityFilter]
    [FactorySecurityFilter]
    [AuthorizationFilter]
    public class CreateAccountChartController : Controller
    {

        DBContext DB = new DBContext();

        // GET: CreateAccountChart
        [HttpGet]
       // [Authorize]
        [CustomAuthorize(Roles = "SCCOA")]
        public ActionResult AccountChart()
        {
            return View();
        }

        public JsonResult CheckChartID(string ChartAccountID)
        {
            var check = DB.AccountChart_Table.Where(x => x.AccountChartID == ChartAccountID).FirstOrDefault();

            if (check != null)
            {
                return Json("False", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("True", JsonRequestBehavior.AllowGet);
            }
        }


        public JsonResult FinalSaveSegments(string ChartAccountID, string ChartAccountName, string AccountLength, string NumberOfSegment, string MainSegment, string Separate, SegmentAccountChart_Table[] order)
        {
            var check = DB.AccountChart_Table.Where(x => x.AccountChartID == ChartAccountID).FirstOrDefault();

            if (check != null)
            {
                return null;
            }
            else
            {


                string userID = FabulousErp.Business.GetUserId();

                AccountChart_Table accountChart_Table = new AccountChart_Table()
                {
                    AccountChartID = ChartAccountID,

                    AccountChartName = ChartAccountName,

                    AccountLength = AccountLength,

                    NumberOfSegment = NumberOfSegment,

                    MainSegment = MainSegment,

                    Separate = Separate,

                    EstablishDate = DateTime.Now.ToShortDateString(),

                    MoveUserID = userID
                };

                DB.AccountChart_Table.Add(accountChart_Table);
                DB.SaveChanges();

                foreach (var item in order)
                {
                    SegmentAccountChart_Table segmentAccountChart_Table = new SegmentAccountChart_Table()
                    {

                        AccountChartID = ChartAccountID,

                        IncreaseSegment = item.IncreaseSegment,

                        SegmentName = item.SegmentName,

                        Length = item.Length

                    };
                    DB.SegmentAccountChart_Table.Add(segmentAccountChart_Table);
                    DB.SaveChanges();
                }

                return Json("True", JsonRequestBehavior.AllowGet);
            }

        }


        public JsonResult FinalSave(string ChartAccountID, string ChartAccountName, string AccountLength, string NumberOfSegment)
        {
            var check = DB.AccountChart_Table.Where(x => x.AccountChartID == ChartAccountID).FirstOrDefault();

            if (check != null)
            {
                return null;
            }
            else
            {


                string userID = FabulousErp.Business.GetUserId();

                AccountChart_Table accountChart_Table = new AccountChart_Table()
                {
                    AccountChartID = ChartAccountID,

                    AccountChartName = ChartAccountName,

                    AccountLength = AccountLength,

                    NumberOfSegment = NumberOfSegment,

                    EstablishDate = DateTime.Now.ToShortDateString(),

                    MoveUserID = userID
                };

                DB.AccountChart_Table.Add(accountChart_Table);
                DB.SaveChanges();

                return Json("True", JsonRequestBehavior.AllowGet);
            }

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
        //            item.SCCOA = Value;
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
        //        if (item.SCCOA.ToString().Equals("True"))
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