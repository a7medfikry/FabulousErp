using FabulousDB.DB_Context;
using FabulousDB.DB_Tabels.Settings.Financial.Accounting._Company.C_Analytic;
using FabulousErp.MyRoleProvider;
using FabulousErp.Repository.Interface;
using FabulousModels.DTOModels.Important;
using FabulousModels.DTOModels.Settings.Financial.Accounting.Analytic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FabulousErp.Controllers.Settings.Financial.Accounting._Company.C_Analytic
{
    [BranchSecurityFilter]
    [FactorySecurityFilter]
    [AuthorizationFilter]
    public class C_AnalyticAccountDistributionController : Controller
    {

        IRepetitionBusiness repetitionBusiness;

        public C_AnalyticAccountDistributionController(IRepetitionBusiness repetitionBusiness)
        {
            this.repetitionBusiness = repetitionBusiness;
        }

        DBContext DB = new DBContext();

        // GET: C_AnalyticAccountDistribution
        [HttpGet]
       // [Authorize]
        [CustomAuthorize(Roles = "SCAAD")]
        public ActionResult CompanyAnalyticDistribution()
        {
            string companyID = (string)FabulousErp.Business.GetCompanyId();

            //ViewBag.CompanyList = repetitionBusiness.RetrieveCompIDListCond(companyID);
            ViewBag.CompanyID = companyID;
            ViewBag.CompanyName = repetitionBusiness.GetCompanyName(companyID);

            return View();
        }

        //public JsonResult GetCompanyName(string CompanyID)
        //{
        //    string userID = FabulousErp.Business.GetUserId();

        //    var checkAccess = DB.UACompPremission_Tables.Where(x => x.CompanyID == CompanyID && x.UserID == userID).FirstOrDefault();

        //    if (checkAccess != null)
        //    {
        //        return Json(repetitionBusiness.GetCompanyName(CompanyID), JsonRequestBehavior.AllowGet);
        //    }
        //    else
        //    {
        //        return Json("False", JsonRequestBehavior.AllowGet);
        //    }
        //}

        public JsonResult FilterAnalyticIDForComp(string CompanyID)
        {
            DB.Configuration.ProxyCreationEnabled = false;

            List<Get_Small_Data_DTO> get_Small_Data_DTO = DB.C_AnalyticAccount_Tables.Where(x => x.CompanyID == CompanyID).Select(x => new Get_Small_Data_DTO
            {

                AnalyticID = x.C_AnalyticAccountID

            }).ToList();

            return Json(get_Small_Data_DTO, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCompAnalyticName(string AnalyticID)
        {
            DB.Configuration.ProxyCreationEnabled = false;

            var getAnalyticName = DB.C_AnalyticAccount_Tables.Where(x => x.C_AnalyticAccountID == AnalyticID).FirstOrDefault();

            if (getAnalyticName != null)
            {
                Get_Small_Data_DTO get_Small_Data_DTO = new Get_Small_Data_DTO()
                {
                    Name = getAnalyticName.C_AnalyticAccountName
                };

                return Json(get_Small_Data_DTO, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return null;
            }
        }


        public JsonResult SaveDistributionRecordComp(string CompAnalyticID, string DistributionID, string DistributionName)
        {

            string userID = FabulousErp.Business.GetUserId();

            var checkDuplicateComp = DB.C_AnalyticDistribution_Tables.Where(x => x.C_AccountDistributionID == DistributionID && x.C_AnalyticAccountID == CompAnalyticID).FirstOrDefault();


            if (checkDuplicateComp != null)
            {
                return Json("False", JsonRequestBehavior.AllowGet);
            }
            else
            {
                C_AnalyticDistribution_Table analyticAccountDistribution_Table = new C_AnalyticDistribution_Table()
                {
                    C_AnalyticAccountID = CompAnalyticID,

                    C_AccountDistributionID = DistributionID,

                    C_AccountDistributionName = DistributionName,

                    MoveUserID = userID
                };

                DB.C_AnalyticDistribution_Tables.Add(analyticAccountDistribution_Table);
                DB.SaveChanges();

                return null;
            }

        }


        public JsonResult GetCompAnalyticDistributionData(string CompAnalyticID)
        {
            List<Analytic_Account_Distribution_DTO> analytic_Account_Distribution_DTO = DB.C_AnalyticDistribution_Tables.Where(x => x.C_AnalyticAccountID == CompAnalyticID).Select(x => new Analytic_Account_Distribution_DTO
            {

                AccountDistributionID = x.C_AccountDistributionID,

                AccountDistributionName = x.C_AccountDistributionName

            }).ToList();

            return Json(analytic_Account_Distribution_DTO, JsonRequestBehavior.AllowGet);
        }


        public JsonResult DeleteAccountDistributionComp(string AccountDistributionID)
        {
            var deleteDistribution = DB.C_AnalyticDistribution_Tables.Where(x => x.C_AccountDistributionID == AccountDistributionID).FirstOrDefault();

            DB.C_AnalyticDistribution_Tables.Remove(deleteDistribution);
            DB.SaveChanges();

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
        //            item.SCAAD = Value;
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
        //        if (item.SCAAD.ToString().Equals("True"))
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