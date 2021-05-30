using FabulousDB.DB_Context;
using FabulousDB.DB_Tabels.Settings.Financial.Accounting.CreateChartOfAccount;
using FabulousErp.MyRoleProvider;
using FabulousErp.Repository.Interface;
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
    public class CreateAccountGroupController : Controller
    {

        IRepetitionBusiness repetitionBusiness;

        public CreateAccountGroupController(IRepetitionBusiness repetitionBusiness)
        {
            this.repetitionBusiness = repetitionBusiness;
        }


        DBContext DB = new DBContext();

        // GET: CreateAccountGroup
        [HttpGet]
       // [Authorize]
        [CustomAuthorize(Roles = "SCAG")]
        public ActionResult AccountGroup()
        {
            ViewBag.AccountChartID = repetitionBusiness.RetrieveAccountChartIDList();

            return View();
        }



        public JsonResult GetAccountChartName(string AccountChartID)
        {
            return Json(repetitionBusiness.GetAccountChartName(AccountChartID), JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetViewOfChart(string ChartOfAccountID)
        {

            var check = DB.SegmentAccountChart_Table.Where(x => x.AccountChartID == ChartOfAccountID).FirstOrDefault();

            if (check != null)
            {

                List<Chart_View_DTO> chart_View_DTO = DB.SegmentAccountChart_Table.Where(x => x.AccountChartID == ChartOfAccountID).Select(x => new Chart_View_DTO
                {

                    NumberOfSegment = x.AccountChart_Table.NumberOfSegment,

                    Seperator = x.AccountChart_Table.Separate,

                    Length = x.Length

                }).ToList();

                return Json(chart_View_DTO, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var getData = DB.AccountChart_Table.Where(x => x.AccountChartID == ChartOfAccountID).FirstOrDefault();

                Chart_View_DTO chart_View = new Chart_View_DTO()
                {
                    Length = getData.AccountLength,

                    Status = "False"
                };

                return Json(chart_View, JsonRequestBehavior.AllowGet);
            }
        }


        public JsonResult SaveAccountGroup(string AccountGroupID, string AccountGroupName, string ChartOfAccountID, string AccountName, double AccountFrom, double AccountTo, string AccountFromWithSep, string AccountToWithSep)
        {

            var checkDuplicate = DB.AccountGroupOfChart_Tables.Where(x => x.AccountGroupChartID == AccountGroupID).FirstOrDefault();

            if (checkDuplicate == null)
            {
                AccountGroupOfChart_Table accountGroupOfChart_Table = new AccountGroupOfChart_Table()
                {
                    AccountGroupChartID = AccountGroupID,

                    AccountGroupChartName = AccountGroupName,

                    AccountChartID = ChartOfAccountID,

                    EstablishDate = DateTime.Now.ToShortDateString()
                };

                DB.AccountGroupOfChart_Tables.Add(accountGroupOfChart_Table);
                DB.SaveChanges();


                AddToContent(AccountGroupID, AccountName, AccountFrom, AccountTo, AccountFromWithSep, AccountToWithSep);


                return Json("True", JsonRequestBehavior.AllowGet);
            }
            else
            {
                var checkOverlapAF = DB.ChartGroupContent_Tables.Where(x => x.AccountGroupChartID == AccountGroupID && AccountFrom >= x.AccountFrom && AccountFrom <= x.AccountTo).FirstOrDefault();
                var checkOverlapAT = DB.ChartGroupContent_Tables.Where(x => x.AccountGroupChartID == AccountGroupID && AccountTo >= x.AccountFrom && AccountTo <= x.AccountTo).FirstOrDefault();

                if (checkOverlapAF != null)
                {
                    return Json("FFalse", JsonRequestBehavior.AllowGet);
                }
                else if(checkOverlapAT != null)
                {
                    return Json("TFalse", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    AddToContent(AccountGroupID, AccountName, AccountFrom, AccountTo, AccountFromWithSep, AccountToWithSep);

                    return Json("True", JsonRequestBehavior.AllowGet);
                }
            }
        }

        public string AddToContent(string AccountGroupID, string AccountName, double AccountFrom, double AccountTo, string AccountFromWithSep, string AccountToWithSep)
        {
            ChartGroupContent_Table chartGroupContent_Table = new ChartGroupContent_Table()
            {
                AccountGroupChartID = AccountGroupID,

                AccountName = AccountName,

                AccountFrom = AccountFrom,

                AccountTo = AccountTo,

                AccountFromWithSep = AccountFromWithSep,

                AccountToWithSep = AccountToWithSep
            };
            DB.ChartGroupContent_Tables.Add(chartGroupContent_Table);
            DB.SaveChanges();

            return null;
        }



        public JsonResult CheckDuplicateAccountGroupID(string AccountGroupID)
        {
            var check = DB.AccountGroupOfChart_Tables.Where(x => x.AccountGroupChartID == AccountGroupID).FirstOrDefault();

            if (check != null)
            {
                return Json("False", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return null;
            }
        }


        public JsonResult CheckDuplicateAccountChartID(string ChartOfAccountID)
        {
            var check = DB.AccountGroupOfChart_Tables.Where(x => x.AccountChartID == ChartOfAccountID).FirstOrDefault();

            if (check != null)
            {
                return Json("False", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return null;
            }
        }


        public JsonResult GetAccountGroupData(string ChartOfAccountID)
        {
            DB.Configuration.ProxyCreationEnabled = false;

            var getData = DB.AccountGroupOfChart_Tables.Where(x => x.AccountChartID == ChartOfAccountID).FirstOrDefault();

            Chart_Group_Content_DTO chart_Group_Content_DTO = new Chart_Group_Content_DTO()
            {

                AccountGroupID = getData.AccountGroupChartID,

                AccountGroupName = getData.AccountGroupChartName

            };

            return Json(chart_Group_Content_DTO, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetGroupContent(string AccountGroupID)
        {

            List<Chart_Group_Content_DTO> chart_Group_Content_DTOs = DB.ChartGroupContent_Tables.Where(x => x.AccountGroupChartID == AccountGroupID).Select(x => new Chart_Group_Content_DTO()
            {

                AccountName = x.AccountName,

                AccountFromWithSep = x.AccountFromWithSep,

                AccountToWithSep = x.AccountToWithSep,

                AccountFrom = x.AccountFrom,

                AccountTo = x.AccountTo

            }).ToList();


            return Json(chart_Group_Content_DTOs, JsonRequestBehavior.AllowGet);
        }


        public JsonResult RemoveAccountsGroup(string AccountName, string AccountGroupID, string ChartOfAccountID)
        {

            var checkUsed = DB.C_CreateAccount_Tables.Where(x => x.AccountsGroup == AccountName && x.AccountChartID == ChartOfAccountID).FirstOrDefault();

            if (checkUsed != null)
            {
                return Json("False", JsonRequestBehavior.AllowGet);
            }
            else
            {
                var getRecord = DB.ChartGroupContent_Tables.Where(x => x.AccountName == AccountName && x.AccountGroupChartID == AccountGroupID).FirstOrDefault();
                DB.ChartGroupContent_Tables.Remove(getRecord);
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
        //            item.SCAG = Value;
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
        //        if (item.SCAG.ToString().Equals("True"))
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