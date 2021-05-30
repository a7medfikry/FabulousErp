using FabulousDB.DB_Context;
using FabulousDB.DB_Tabels.Settings.Financial.Accounting._Factory.F_Analytic;
using FabulousErp.MyRoleProvider;
using FabulousErp.Repository.Interface;
using FabulousModels.DTOModels.Important;
using FabulousModels.DTOModels.Settings.Financial.Accounting.Analytic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FabulousErp.Controllers.Settings.Financial.Accounting._Factory.F_Analytic
{
    [AuthorizationFilter]
    public class F_AnalyticAccountDistributionController : Controller
    {

        IRepetitionBusiness repetitionBusiness;

        public F_AnalyticAccountDistributionController(IRepetitionBusiness repetitionBusiness)
        {
            this.repetitionBusiness = repetitionBusiness;
        }

        DBContext DB = new DBContext();

        // GET: F_AnalyticAccountDistribution
        [HttpGet]
       // [Authorize]
        [CustomAuthorize(Roles = "SFAAD")]
        public ActionResult FactoryAnalyticDistribution()
        {
            string companyID = (string)FabulousErp.Business.GetCompanyId();

            string branchID =null;

            string factoryID = null;

            if (companyID != null)
            {
                ViewBag.FactoryList = repetitionBusiness.RetrieveFactoryIDListCond(companyID);
            }
            else if (branchID != null)
            {
                ViewBag.FactoryList = repetitionBusiness.RetrieveFactoryIDListCondByB(branchID);
            }
            else if (factoryID != null)
            {
                ViewBag.FactoryList = factoryID;

                ViewBag.FactoryName = repetitionBusiness.GetFactoryName(factoryID);
            }
            
            return View();
        }

        public JsonResult GetFatoryName(string FactoryID)
        {
            string userID = FabulousErp.Business.GetUserId();

            var checkAccess = DB.UAFactoryPremission_Tables.Where(x => x.FactoryID == FactoryID && x.UserID == userID).FirstOrDefault();

            if (checkAccess != null)
            {
                return Json(repetitionBusiness.GetFactoryName(FactoryID), JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("False", JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult FilterAnalyticIDForFactory(string FactoryID)
        {
            DB.Configuration.ProxyCreationEnabled = false;

            List<Get_Small_Data_DTO> get_Small_Data_DTO = DB.F_AnalyticAccount_Tables.Where(x => x.FactoryID == FactoryID).Select(x => new Get_Small_Data_DTO
            {

                AnalyticID = x.F_AnalyticAccountID

            }).ToList();

            return Json(get_Small_Data_DTO, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetFactoryAnalyticName(string AnalyticID)
        {
            DB.Configuration.ProxyCreationEnabled = false;

            var getAnalyticName = DB.F_AnalyticAccount_Tables.Where(x => x.F_AnalyticAccountID == AnalyticID).FirstOrDefault();

            if (getAnalyticName != null)
            {
                Get_Small_Data_DTO get_Small_Data_DTO = new Get_Small_Data_DTO()
                {
                    Name = getAnalyticName.F_AnalyticAccountName
                };

                return Json(get_Small_Data_DTO, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return null;
            }
        }


        public JsonResult SaveDistributionRecordFactory(string FactoryAnalyticID, string DistributionID, string DistributionName)
        {

            string userID = FabulousErp.Business.GetUserId();

            var checkDuplicateFactory = DB.F_AnalyticDistribution_Tables.Where(x => x.F_AccountDistributionID == DistributionID && x.F_AnalyticAccountID == FactoryAnalyticID).FirstOrDefault();

            if (checkDuplicateFactory != null)
            {
                return Json("False", JsonRequestBehavior.AllowGet);
            }
            else
            {
                F_AnalyticDistribution_Table f_AnalyticDistribution_Table = new F_AnalyticDistribution_Table()
                {
                    F_AnalyticAccountID = FactoryAnalyticID,

                    F_AccountDistributionID = DistributionID,

                    F_AccountDistributionName = DistributionName,

                    MoveUserID = userID
                };
                DB.F_AnalyticDistribution_Tables.Add(f_AnalyticDistribution_Table);
                DB.SaveChanges();

                return null;
            }

        }


        public JsonResult GetFactoryAnalyticDistributionData(string FactoryAnalyticID)
        {
            List<Analytic_Account_Distribution_DTO> analytic_Account_Distribution_DTO = DB.F_AnalyticDistribution_Tables.Where(x => x.F_AnalyticAccountID == FactoryAnalyticID).Select(x => new Analytic_Account_Distribution_DTO
            {

                AccountDistributionID = x.F_AccountDistributionID,

                AccountDistributionName = x.F_AccountDistributionName

            }).ToList();

            return Json(analytic_Account_Distribution_DTO, JsonRequestBehavior.AllowGet);
        }



        public JsonResult DeleteAccountDistributionFactory(string AccountDistributionID)
        {
            var deleteDistribution = DB.F_AnalyticDistribution_Tables.Where(x => x.F_AccountDistributionID == AccountDistributionID).FirstOrDefault();

            DB.F_AnalyticDistribution_Tables.Remove(deleteDistribution);
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
        //            item.SFAAD = Value;
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
        //        if (item.SFAAD.ToString().Equals("True"))
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