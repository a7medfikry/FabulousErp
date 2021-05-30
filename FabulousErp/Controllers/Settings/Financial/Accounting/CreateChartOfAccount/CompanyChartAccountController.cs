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
    public class CompanyChartAccountController : Controller
    {

        IRepetitionBusiness repetitionBusiness;

        public CompanyChartAccountController(IRepetitionBusiness repetitionBusiness)
        {
            this.repetitionBusiness = repetitionBusiness;
        }

        DBContext DB = new DBContext();

        // GET: CompanyChartAccount

        [HttpGet]
       // [Authorize]
        [CustomAuthorize(Roles = "SACTC")]
        public ActionResult AddChartToCompany()
        {
            string CompanyID = (string)FabulousErp.Business.GetCompanyId();

            ViewBag.CompanyList = repetitionBusiness.RetrieveCompIDList();

            ViewBag.AccountChartID = repetitionBusiness.RetrieveAccountChartIDList();

            return View();
        }

        public JsonResult GetCompanyName(string CompanyID)
        {
            return Json(repetitionBusiness.GetCompanyName(CompanyID), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetChartName(string ChartID)
        {
            return Json(repetitionBusiness.GetAccountChartName(ChartID), JsonRequestBehavior.AllowGet);
        }

        public JsonResult AddChartToComp(string CompanyID, string AccountChartID)
        {
            var checkFY = DB.CompanyFiscalYear_Tables.Where(x => x.CompanyID == CompanyID).FirstOrDefault();

            if (checkFY != null)
            {

                var check = DB.CompanyChartAccount_Table.Where(x => x.CompanyID == CompanyID).FirstOrDefault();

                string userID = FabulousErp.Business.GetUserId();

                var checkCompAccess = DB.UACompPremission_Tables.Where(x => x.CompanyID == CompanyID && x.UserID == userID).FirstOrDefault();

                if (check != null)
                {
                    return Json("False", JsonRequestBehavior.AllowGet);
                }
                else if (checkCompAccess == null)
                {
                    return Json("NoAccess", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    CompanyChartAccount_Table companyChartAccount_Table = new CompanyChartAccount_Table()
                    {
                        CompanyID = CompanyID,

                        AccountChartID = AccountChartID,

                        MoveUserID = userID
                    };

                    DB.CompanyChartAccount_Table.Add(companyChartAccount_Table);
                    DB.SaveChanges();

                    return Json("True", JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json("NoFY", JsonRequestBehavior.AllowGet);
            }
        }


        public JsonResult GetCompChartData(string CompanyID)
        {

            string userID = FabulousErp.Business.GetUserId();

            var checkCompAccess = DB.UACompPremission_Tables.Where(x => x.CompanyID == CompanyID && x.UserID == userID).FirstOrDefault();

            if (checkCompAccess != null)
            {

                List<Account_Chart_Company_DTO> account_Chart_Company_DTO = DB.CompanyChartAccount_Table.Where(x => x.CompanyID == CompanyID).Select(x => new Account_Chart_Company_DTO
                {

                    CompanyID = x.CompanyID,

                    CompanyName = x.CompanyMainInfo_Table.CompanyName,

                    AccountChartID = x.AccountChartID,

                    AccountChartName = x.AccountChart_Table.AccountChartName

                }).ToList();

                return Json(account_Chart_Company_DTO, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return null;
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
        //            item.SACTC = Value;
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
        //        if (item.SACTC.ToString().Equals("True"))
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