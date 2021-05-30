using FabulousDB.DB_Context;
using FabulousDB.DB_Tabels.Settings.Financial.Accounting.CurrenciesDefinition;
using FabulousErp.MyRoleProvider;
using FabulousErp.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static FabulousErp.Business;

namespace FabulousErp.Controllers.Settings.Financial.Accounting.CurrenciesDefinition
{
    [BranchSecurityFilter]
    [FactorySecurityFilter]
    [AuthorizationFilter]
    public class UserEditTRateController : Controller
    {

        IRepetitionBusiness repetitionBusiness;

        public UserEditTRateController(IRepetitionBusiness repetitionBusiness)
        {
            this.repetitionBusiness = repetitionBusiness;
        }

        DBContext DB = new DBContext();

        // GET: UserEditTRate
        [HttpGet]
       // [Authorize]
        [CustomAuthorize(Roles = "SUETR")]
        public ActionResult UEditRate()
        {

            string companyID = (string)FabulousErp.Business.GetCompanyId();

            ViewBag.CompanyList = repetitionBusiness.RetrieveCompIDListCond(companyID);

            ViewBag.BranchList = repetitionBusiness.RetrieveBranchIDListCond(companyID);

            ViewBag.FactoryList = repetitionBusiness.RetrieveFactoryIDListCond(companyID);

            List<string> FiForm = Enum.GetNames(typeof(FinancialForms)).ToList();

            List<C_EditTRate> CEditRate = DB.C_EditTRates.Where(x => x.CompanyID == companyID).ToList();

            List<string> ExistingRate = CEditRate.Where(x => x.Module == RateFormMoudles.Financial.ToString())
                .Select(x => x.TransactionFormName).ToList();

            ExistingRate.ForEach(x => x = x.Replace(" ", "_"));
            for (int i=0;i< ExistingRate.Count; i++)
            {
                ExistingRate[i] = ExistingRate[i].Replace(" ", "_");
            }
            List<string> AddedForms = FiForm.Except<string>(ExistingRate)
                .ToList();

            CEditRate.AddRange(AddedForms.Select(x => new C_EditTRate
            {
                TransactionFormName = x.Replace("_", " "),
                Module= RateFormMoudles.Financial.ToString(),
                CompanyID= companyID,
                AllowUserE=false,
                C_ETRID= CEditRate.DefaultIfEmpty(new C_EditTRate { C_ETRID=0 }).Max(z=>z.C_ETRID)+1
            }));

            return View(CEditRate);
        }

      
        public enum RateFormMoudles
        {
            Financial=1
        }
      
        public JsonResult GetCompanyName(string companyID)
        {
            return Json(repetitionBusiness.GetCompanyName(companyID), JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetBranchName(string branchID)
        {
            string userID = FabulousErp.Business.GetUserId();

            var checkAccess = DB.UABranchPremission_Tables.Where(x => x.UserID == userID && x.BranchID == branchID).FirstOrDefault();

            if (checkAccess != null)
            {
                return Json(repetitionBusiness.GetBranchName(branchID), JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("False", JsonRequestBehavior.AllowGet);
            }
        }


        public JsonResult GetFactoryName(string factoryID)
        {
            string userID = FabulousErp.Business.GetUserId();

            var checkAccess = DB.UAFactoryPremission_Tables.Where(x => x.UserID == userID && x.FactoryID == factoryID).FirstOrDefault();

            if(checkAccess != null)
            {
                return Json(repetitionBusiness.GetFactoryName(factoryID), JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("False", JsonRequestBehavior.AllowGet);
            }
        }



        public JsonResult SaveCompanyUE(string companyID, string module, string tFormName, bool allowUserE)
        {
            var checkDuplicate = DB.C_EditTRates.Where(x => x.CompanyID == companyID && x.Module == module && x.TransactionFormName == tFormName).FirstOrDefault();

            if (checkDuplicate != null)
            {
                return Json("False", JsonRequestBehavior.AllowGet);
            }
            else
            {
                C_EditTRate c_EditTRate = new C_EditTRate()
                {
                    CompanyID = companyID,

                    Module = module,

                    TransactionFormName = tFormName,

                    AllowUserE = allowUserE
                };

                DB.C_EditTRates.Add(c_EditTRate);
                DB.SaveChanges();

                return null;
            }
        }


        public JsonResult SaveBranchUE(string branchID, string module, string tFormName, bool allowUserE)
        {
            var checkDuplicate = DB.B_EditTRates.Where(x => x.BranchID == branchID && x.Module == module && x.TransactionFormName == tFormName).FirstOrDefault();

            if (checkDuplicate != null)
            {
                return Json("False", JsonRequestBehavior.AllowGet);
            }
            else
            {
                B_EditTRate c_EditTRate = new B_EditTRate()
                {
                    BranchID = branchID,

                    Module = module,

                    TransactionFormName = tFormName,

                    AllowUserE = allowUserE
                };

                DB.B_EditTRates.Add(c_EditTRate);
                DB.SaveChanges();

                return null;
            }
        }


        public JsonResult SaveFactoryUE(string factoryID, string module, string tFormName, bool allowUserE)
        {
            var checkDuplicate = DB.F_EditTRates.Where(x => x.FactoryID == factoryID && x.Module == module && x.TransactionFormName == tFormName).FirstOrDefault();

            if (checkDuplicate != null)
            {
                return Json("False", JsonRequestBehavior.AllowGet);
            }
            else
            {
                F_EditTRate c_EditTRate = new F_EditTRate()
                {
                    FactoryID = factoryID,

                    Module = module,

                    TransactionFormName = tFormName,

                    AllowUserE = allowUserE
                };

                DB.F_EditTRates.Add(c_EditTRate);
                DB.SaveChanges();

                return null;
            }
        }



        public JsonResult GetCompanyUE(string companyID, string module, string tFormName)
        {
            var getData = DB.C_EditTRates.Where(x => x.CompanyID == companyID && x.Module == module && x.TransactionFormName == tFormName).FirstOrDefault();

            if (getData != null)
            {
                return Json(getData.AllowUserE, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("False", JsonRequestBehavior.AllowGet);
            }
        }


        public JsonResult GetBranchUE(string branchID, string module, string tFormName)
        {
            var getData = DB.B_EditTRates.Where(x => x.BranchID == branchID && x.Module == module && x.TransactionFormName == tFormName).FirstOrDefault();

            if (getData != null)
            {
                return Json(getData.AllowUserE, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("False", JsonRequestBehavior.AllowGet);
            }
        }


        public JsonResult GetFactoryUE(string factoryID, string module, string tFormName)
        {
            var getData = DB.F_EditTRates.Where(x => x.FactoryID == factoryID && x.Module == module && x.TransactionFormName == tFormName).FirstOrDefault();

            if (getData != null)
            {
                return Json(getData.AllowUserE, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("False", JsonRequestBehavior.AllowGet);
            }
        }



        public JsonResult UpdateCompanyUE(List<C_EditTRate> Pages /* string module, string tFormName, bool allowUserE*/)
        {
            string companyID = (string)FabulousErp.Business.GetCompanyId();

            Pages.ForEach(x => x.CompanyID = companyID);

            foreach (C_EditTRate i in Pages)
            {
                if (DB.C_EditTRates.Find(i.C_ETRID) == null)
                {
                    DB.C_EditTRates.Add(i);
                }
                else
                {
                    DB.C_EditTRates.Find(i.C_ETRID).AllowUserE = i.AllowUserE;
                }
            }
            DB.SaveChanges();
            return Json(1);
            //var getData = DB.C_EditTRates.Where(x => x.CompanyID == companyID && x.Module == module && x.TransactionFormName == tFormName).FirstOrDefault();

            //if (getData != null)
            //{
            //    getData.AllowUserE = allowUserE;
            //    DB.SaveChanges();

            //    return null;
            //}
            //else
            //{
            //    return Json("False", JsonRequestBehavior.AllowGet);
            //}
        }


        public JsonResult UpdateBranchUE(string branchID, string module, string tFormName, bool allowUserE)
        {
            var getData = DB.B_EditTRates.Where(x => x.BranchID == branchID && x.Module == module && x.TransactionFormName == tFormName).FirstOrDefault();

            if (getData != null)
            {
                getData.AllowUserE = allowUserE;
                DB.SaveChanges();

                return null;
            }
            else
            {
                return Json("False", JsonRequestBehavior.AllowGet);
            }
        }


        public JsonResult UpdateFactoryUE(string factoryID, string module, string tFormName, bool allowUserE)
        {
            var getData = DB.F_EditTRates.Where(x => x.FactoryID == factoryID && x.Module == module && x.TransactionFormName == tFormName).FirstOrDefault();

            if (getData != null)
            {
                getData.AllowUserE = allowUserE;
                DB.SaveChanges();

                return null;
            }
            else
            {
                return Json("False", JsonRequestBehavior.AllowGet);
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
        //            item.SUETR = Value;
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
        //        if (item.SUETR.ToString().Equals("True"))
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