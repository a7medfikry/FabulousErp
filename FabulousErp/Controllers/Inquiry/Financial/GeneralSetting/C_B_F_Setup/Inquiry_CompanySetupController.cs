using FabulousDB.DB_Context;
using FabulousDB.DB_Tabels.Settings.Financial.GeneralSetting.C_B_F_Setup.CompanyBranchInfo;
using FabulousDB.DB_Tabels.Settings.Financial.GeneralSetting.C_B_F_Setup.CompanyInformation;
using FabulousErp.MyRoleProvider;
using FabulousModels.DTOModels.Inquiry.Financial.GeneralSetting.C_B_F_Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FabulousErp.Controllers.Inquiry.Financial.GeneralSetting.C_B_F_Setup
{
    [BranchSecurityFilter]
    [FactorySecurityFilter]
    [AuthorizationFilter]
    public class Inquiry_CompanySetupController : Controller
    {
        DBContext DB = new DBContext();

        // GET: Inquiry_CompanySetup
        [HttpGet]
       // [Authorize]
        [CustomAuthorize(Roles = "ICI")]
        public ActionResult CompanyInformation()
        {
            var list = DB.CompanyMainInfo_Tables.ToList();
            SelectList CompanyID = new SelectList(list, "CompanyID", "CompanyID");
            SelectList CompanyName = new SelectList(list, "CompanyName", "CompanyName");
            ViewBag.CodeList = CompanyID;
            ViewBag.CodeList2 = CompanyName;
            return View();
        }

        public JsonResult CompanyDetails(string InquiryCompanyID , string InquiryCompanyName)
        {
            DB.Configuration.ProxyCreationEnabled = false;

            List<Inquiry_CompanySetup> inquiry_CompanySetups = new List<Inquiry_CompanySetup>();
            inquiry_CompanySetups = DB.CompanyMainInfo_Tables.Where(x => x.CompanyID == InquiryCompanyID || x.CompanyName == InquiryCompanyName).Select(x => new Inquiry_CompanySetup
            {
                CompanyID = x.CompanyID,
                CompanyName = x.CompanyName,
                CompanyAlies = x.CompanyAlies,
                CountryName = x.CountryName,
                Language = x.Language,
                Currency = x.Currency,
                CompanyMainActivity = x.CompanyMainActivity,

                CompanyType = x.CompanyLegalInfo_Table.CompanyType,
                EstablishDate = x.CompanyLegalInfo_Table.EstablishDate,
                CommericalRegister = x.CompanyLegalInfo_Table.CommericalRegister,
                CommericalOffice = x.CompanyLegalInfo_Table.CommericalOffice,
                TaxFileNo = x.CompanyLegalInfo_Table.TaxFileNo,
                TaxOffice = x.CompanyLegalInfo_Table.TaxOffice,
                VatID = x.CompanyLegalInfo_Table.VatID,
                TaxVaOffice = x.CompanyLegalInfo_Table.TaxVaOffice,
                ImporterID = x.CompanyLegalInfo_Table.ImporterID,
                ImportOffice = x.CompanyLegalInfo_Table.ImportOffice,
                ExportID = x.CompanyLegalInfo_Table.ExportID,
                ExportOffice = x.CompanyLegalInfo_Table.ExportOffice,
                SocialInsuranceID = x.CompanyLegalInfo_Table.SocialInsuranceID,
                SocialInsuranceOffice = x.CompanyLegalInfo_Table.SocialInsuranceOffice,

                StreetName = x.AddressInformation_Table.StreetName,
                BuldingNo = x.AddressInformation_Table.BuldingNo,
                FloorNo = x.AddressInformation_Table.FloorNo,
                Area = x.AddressInformation_Table.Area,
                City = x.AddressInformation_Table.City,
                Governorate = x.AddressInformation_Table.Governorate,

                International1 = x.CompanyCommInfo_Table.International1,
                Telephone1 = x.CompanyCommInfo_Table.Telephone1,
                TelephoneEX1 = x.CompanyCommInfo_Table.TelephoneEX1,
                Code1 = x.CompanyCommInfo_Table.Code1,
                Fax1 = x.CompanyCommInfo_Table.Fax1,
                FaxEX1 = x.CompanyCommInfo_Table.FaxEX1
            }).ToList();

            return Json(inquiry_CompanySetups, JsonRequestBehavior.AllowGet);
        }

        public JsonResult FactoryDetails(string Selected)
        {
            DB.Configuration.ProxyCreationEnabled = false;
            List<Inquiry_CompanySetup> inquiry_CompanySetups = new List<Inquiry_CompanySetup>();
            inquiry_CompanySetups = DB.CompanyFactoryInfo_Tables.Where(x => x.CompanyID == Selected).Select(x => new Inquiry_CompanySetup
            {
                FactoryID = x.FactoryID,
                FactoryName = x.FactoryName,
                FactoryAlies = x.FactoryAlies,

                StreetName = x.FactoryAddressInfo_Table.StreetName,
                BuldingNo = x.FactoryAddressInfo_Table.BuldingNo,
                FloorNo = x.FactoryAddressInfo_Table.FloorNo,
                Area = x.FactoryAddressInfo_Table.Area,
                City = x.FactoryAddressInfo_Table.City,
                Governorate = x.FactoryAddressInfo_Table.Governorate,

               SocialInsuranceID = x.FactoryLegalInfo_Table.InsuranceID,
               SocialInsuranceOffice = x.FactoryLegalInfo_Table.InsuranceOffice,

               International1 = x.FactoryCommInfo_Table.International1,
               Telephone1 = x.FactoryCommInfo_Table.Telephone1,
               TelephoneEX1 = x.FactoryCommInfo_Table.TelephoneEX1,
               International2 = x.FactoryCommInfo_Table.International2,
               Telephone2 = x.FactoryCommInfo_Table.Telephone2,
               TelephoneEX2 = x.FactoryCommInfo_Table.TelephoneEX2,
               Code1 = x.FactoryCommInfo_Table.Code1,
               Fax1 = x.FactoryCommInfo_Table.Fax1,
               FaxEX1 = x.FactoryCommInfo_Table.FaxEX1,
               Code2 = x.FactoryCommInfo_Table.Code2,
               Fax2 = x.FactoryCommInfo_Table.Fax2,
               FaxEX2 = x.FactoryCommInfo_Table.FaxEX2
            }).ToList();

            return Json(inquiry_CompanySetups, JsonRequestBehavior.AllowGet);
        }

        public JsonResult BranchDetails(string Selected)
        {
            DB.Configuration.ProxyCreationEnabled = false;
            List<Inquiry_CompanySetup> inquiry_CompanySetups = new List<Inquiry_CompanySetup>();
            inquiry_CompanySetups = DB.CompanyBranchInfo_Tables.Where(x => x.CompanyID == Selected).Select(x => new Inquiry_CompanySetup
            {
                BranchID = x.BranchID,
                BranchName = x.BranchName,
                BranchAlies = x.BranchAlies,

                StreetName = x.BranchAddressInfo_Table.StreetName,
                BuldingNo = x.BranchAddressInfo_Table.BuldingNo,
                FloorNo = x.BranchAddressInfo_Table.FloorNo,
                Area = x.BranchAddressInfo_Table.Area,
                City = x.BranchAddressInfo_Table.City,
                Governorate = x.BranchAddressInfo_Table.Governorate,

                SocialInsuranceID = x.BranchLegalInfo_Table.InsuranceID,
                SocialInsuranceOffice = x.BranchLegalInfo_Table.InsuranceOffice,

                International1 = x.BranchCommInfo_Table.International1,
                Telephone1 = x.BranchCommInfo_Table.Telephone1,
                TelephoneEX1 = x.BranchCommInfo_Table.TelephoneEX1,
                International2 = x.BranchCommInfo_Table.International2,
                Telephone2 = x.BranchCommInfo_Table.Telephone2,
                TelephoneEX2 = x.BranchCommInfo_Table.TelephoneEX2,
                Code1 = x.BranchCommInfo_Table.Code1,
                Fax1 = x.BranchCommInfo_Table.Fax1,
                FaxEX1 = x.BranchCommInfo_Table.FaxEX1,
                Code2 = x.BranchCommInfo_Table.Code2,
                Fax2 = x.BranchCommInfo_Table.Fax2,
                FaxEX2 = x.BranchCommInfo_Table.FaxEX2
            }).ToList();

            return Json(inquiry_CompanySetups, JsonRequestBehavior.AllowGet);
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
        //            item.ICI = Value;
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
        //        if (item.ICI.ToString().Equals("True"))
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