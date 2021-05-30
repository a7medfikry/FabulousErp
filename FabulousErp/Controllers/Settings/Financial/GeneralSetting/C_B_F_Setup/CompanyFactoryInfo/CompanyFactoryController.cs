using FabulousDB.DB_Context;
using FabulousDB.DB_Tabels.Settings.Financial.GeneralSetting.C_B_F_Setup;
using FabulousDB.DB_Tabels.Settings.Financial.GeneralSetting.C_B_F_Setup.CompanyFactoryInfo;
using FabulousErp.MyRoleProvider;
using FabulousErp.Repository.Interface;
using FabulousModels.DTOModels.Settings.Financial.GeneralSettings.C_B_F_Setup.CompanyFactoryInfo;
using FabulousModels.ViewModels.Settings.Financial.GeneralSetting.C_B_F_Setup.CompanyFactoryInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FabulousErp.Controllers.Settings.Financial.GeneralSetting.CompanyFactoryInfo
{
    [BranchSecurityFilter]
    [FactorySecurityFilter]
    [AuthorizationFilter]
    public class CompanyFactoryController : Controller
    {

        IRepetitionBusiness repetitionBusiness;

        public CompanyFactoryController(IRepetitionBusiness repetitionBusiness)
        {
            this.repetitionBusiness = repetitionBusiness;
        }


        DBContext DB = new DBContext();

        // GET: CompanyFactory
        [HttpGet]
       // [Authorize]
        [CustomAuthorize(Roles = "SCPI")]
        public ActionResult FactoryInformation()
        {
            ViewBag.CompanyList = repetitionBusiness.RetrieveCompIDList();

            ViewBag.BranchList = repetitionBusiness.RetrieveBranchIDList();

            return View();
        }


        [HttpPost]
        public ActionResult FactoryInformations(Factory_Collection factory_Collection)
        {
            ViewBag.CompanyList = repetitionBusiness.RetrieveCompIDList();

            ViewBag.BranchList = repetitionBusiness.RetrieveBranchIDList();


            if (ModelState.IsValid)
            {

                var CheckUniqueMain = DB.CompanyFactoryInfo_Tables.Where(x => x.FactoryID == factory_Collection.Factory_Main_Info.FactoryID).FirstOrDefault();

                if (CheckUniqueMain != null)
                {
                    return Json("FactoryIDExist", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    string GetBranchID = "";

                    if (factory_Collection.Factory_Main_Info.BranchID == "-1")
                    {
                        GetBranchID = null;
                    }
                    else
                    {
                        GetBranchID = factory_Collection.Factory_Main_Info.BranchID;
                    }

                    CompanyFactoryInfo_Table FactoryInfo_Table = new CompanyFactoryInfo_Table()
                    {
                        FactoryID = factory_Collection.Factory_Main_Info.FactoryID,

                        FactoryName = factory_Collection.Factory_Main_Info.FactoryName,

                        FactoryAlies = factory_Collection.Factory_Main_Info.FactoryAlies,

                        BranchID = GetBranchID,

                        CompanyID = factory_Collection.Factory_Main_Info.CompanyID,

                        EstablishDate = factory_Collection.Factory_Main_Info.EstablishDate,

                        Status = factory_Collection.Factory_Main_Info.Status
                    };

                    DB.CompanyFactoryInfo_Tables.Add(FactoryInfo_Table);
                    DB.SaveChanges();

                    string lastFactoryID = FactoryInfo_Table.FactoryID;

                    FactoryAddressInfo_Table address_Table = new FactoryAddressInfo_Table()
                    {
                        FactoryID = lastFactoryID,

                        StreetName = factory_Collection.Address_Information.StreetName,

                        BuldingNo = factory_Collection.Address_Information.BuldingNo,

                        FloorNo = factory_Collection.Address_Information.FloorNo,

                        Area = factory_Collection.Address_Information.Area,

                        City = factory_Collection.Address_Information.City,

                        Governorate = factory_Collection.Address_Information.Governorate
                    };

                    DB.FactoryAddressInfo_Tables.Add(address_Table);
                    DB.SaveChanges();

                    FactoryLegalInfo_Table legal_Table = new FactoryLegalInfo_Table()
                    {
                        FactoryID = lastFactoryID,

                        InsuranceID = factory_Collection.Factory_Legal_Info.InsuranceID,

                        InsuranceOffice = factory_Collection.Factory_Legal_Info.InsuranceOffice
                    };

                    DB.FactoryLegalInfo_Tables.Add(legal_Table);
                    DB.SaveChanges();

                    FactoryCommInfo_Table communication_Table = new FactoryCommInfo_Table()
                    {
                        FactoryID = lastFactoryID,

                        International1 = factory_Collection.Communication_Info.International1,

                        Telephone1 = factory_Collection.Communication_Info.Telephone1,

                        TelephoneEX1 = factory_Collection.Communication_Info.TelephoneEX1,

                        International2 = factory_Collection.Communication_Info.International2,

                        Telephone2 = factory_Collection.Communication_Info.Telephone2,

                        TelephoneEX2 = factory_Collection.Communication_Info.TelephoneEX2,

                        International3 = factory_Collection.Communication_Info.International3,

                        Telephone3 = factory_Collection.Communication_Info.Telephone3,

                        TelephoneEX3 = factory_Collection.Communication_Info.TelephoneEX3,

                        International4 = factory_Collection.Communication_Info.International4,

                        Telephone4 = factory_Collection.Communication_Info.Telephone4,

                        TelephoneEX4 = factory_Collection.Communication_Info.TelephoneEX4,

                        International5 = factory_Collection.Communication_Info.International5,

                        Telephone5 = factory_Collection.Communication_Info.Telephone5,

                        TelephoneEX5 = factory_Collection.Communication_Info.TelephoneEX5,

                        Fax1 = factory_Collection.Communication_Info.Fax1,

                        FaxEX1 = factory_Collection.Communication_Info.FaxEX1,

                        Fax2 = factory_Collection.Communication_Info.Fax2,

                        FaxEX2 = factory_Collection.Communication_Info.FaxEX2,

                        Fax3 = factory_Collection.Communication_Info.Fax3,

                        FaxEX3 = factory_Collection.Communication_Info.FaxEX3,

                        Fax4 = factory_Collection.Communication_Info.Fax4,

                        FaxEX4 = factory_Collection.Communication_Info.FaxEX4,

                        Fax5 = factory_Collection.Communication_Info.Fax5,

                        FaxEX5 = factory_Collection.Communication_Info.FaxEX5,

                        Code1 = factory_Collection.Communication_Info.Code1,

                        Code2 = factory_Collection.Communication_Info.Code2,

                        Code3 = factory_Collection.Communication_Info.Code3,

                        Code4 = factory_Collection.Communication_Info.Code4,

                        Code5 = factory_Collection.Communication_Info.Code5
                    };

                    DB.FactoryCommInfo_Tables.Add(communication_Table);
                    DB.SaveChanges();

                    return Json("Done", JsonRequestBehavior.AllowGet);

                }
            }
            return Json(JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetFactoryInfo(string FactoryID, string CompanyID)
        {

            var RMainInfo = DB.CompanyFactoryInfo_Tables.Where(x => x.FactoryID == FactoryID && x.CompanyID == CompanyID).FirstOrDefault();
            var RAddressInfo = DB.FactoryAddressInfo_Tables.Where(x => x.FactoryID == FactoryID).FirstOrDefault();
            var RLegalInfo = DB.FactoryLegalInfo_Tables.Where(x => x.FactoryID == FactoryID).FirstOrDefault();
            var RCommunicationInfo = DB.FactoryCommInfo_Tables.Where(x => x.FactoryID == FactoryID).FirstOrDefault();

            if (RMainInfo != null)
            {

                R_Company_Info_DTO info_DTO = new R_Company_Info_DTO()
                {
                    FactoryName = RMainInfo.FactoryName,

                    FactoryAlies = RMainInfo.FactoryAlies,

                    BranchID = RMainInfo.BranchID,

                    CompanyID = RMainInfo.CompanyID,

                    EstablishDate = RMainInfo.EstablishDate,

                    Status = RMainInfo.Status.ToString(),


                    StreetName = RAddressInfo.StreetName,

                    BuldingNo = RAddressInfo.BuldingNo,

                    FloorNo = RAddressInfo.FloorNo,

                    Area = RAddressInfo.Area,

                    City = RAddressInfo.City,

                    Governorate = RAddressInfo.Governorate,


                    InsuranceID = RLegalInfo.InsuranceID,

                    InsuranceOffice = RLegalInfo.InsuranceOffice,


                    International1 = RCommunicationInfo.International1,

                    Telephone1 = RCommunicationInfo.Telephone1,

                    TelephoneEX1 = RCommunicationInfo.TelephoneEX1,

                    International2 = RCommunicationInfo.International2,

                    Telephone2 = RCommunicationInfo.Telephone2,

                    TelephoneEX2 = RCommunicationInfo.TelephoneEX2,

                    International3 = RCommunicationInfo.International3,

                    Telephone3 = RCommunicationInfo.Telephone3,

                    TelephoneEX3 = RCommunicationInfo.TelephoneEX3,

                    International4 = RCommunicationInfo.International4,

                    Telephone4 = RCommunicationInfo.Telephone4,

                    TelephoneEX4 = RCommunicationInfo.TelephoneEX4,

                    International5 = RCommunicationInfo.International5,

                    Telephone5 = RCommunicationInfo.Telephone5,

                    TelephoneEX5 = RCommunicationInfo.TelephoneEX5,

                    Fax1 = RCommunicationInfo.Fax1,

                    FaxEX1 = RCommunicationInfo.FaxEX1,

                    Fax2 = RCommunicationInfo.Fax2,

                    FaxEX2 = RCommunicationInfo.FaxEX2,

                    Fax3 = RCommunicationInfo.Fax3,

                    FaxEX3 = RCommunicationInfo.FaxEX3,

                    Fax4 = RCommunicationInfo.Fax4,

                    FaxEX4 = RCommunicationInfo.FaxEX4,

                    Fax5 = RCommunicationInfo.Fax5,

                    FaxEX5 = RCommunicationInfo.FaxEX5,

                    Code1 = RCommunicationInfo.Code1,

                    Code2 = RCommunicationInfo.Code2,

                    Code3 = RCommunicationInfo.Code3,

                    Code4 = RCommunicationInfo.Code4,

                    Code5 = RCommunicationInfo.Code5
                };

                return Json(info_DTO, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("False", JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult UpdateFactoryInfo(string FactoryID, string FactoryName, string FactoryAlies,

            string StreetName, string BuldingNo, string FloorNo, string Governorate, string Area, string City, string InsuranceID, string InsuranceOffice, string International1, string Telephone1, string TelephoneEX1,

            string International2, string Telephone2, string TelephoneEX2, string International3, string Telephone3, string TelephoneEX3, string International4, string Telephone4, string TelephoneEX4,

            string International5, string Telephone5, string TelephoneEX5, string Fax1, string FaxEX1, string Fax2, string FaxEX2, string Fax3, string FaxEX3, string Fax4, string FaxEX4,

            string Fax5, string FaxEX5, string EstablishDate, string Code1, string Code2, string Code3, string Code4, string Code5, bool Status)

        {
            /*
            var GetIDMain = DB.companyFactoryInfo_Tables.Where(x => x.FactoryID == FactoryID).FirstOrDefault();
            int IDMain = GetIDMain.ID;
            */
            var UpdateMain = DB.CompanyFactoryInfo_Tables.Where(x => x.FactoryID == FactoryID).First();
            UpdateMain.FactoryName = FactoryName;
            UpdateMain.FactoryAlies = FactoryAlies;
            UpdateMain.EstablishDate = EstablishDate;
            UpdateMain.Status = Status;

            /*
            var GetIDAdress = DB.AddressInformation_Tables.Where(x => x.SpecificId == FactoryID).FirstOrDefault();
            int IDAddress = GetIDAdress.ID;
            */
            var UpdateAddress = DB.FactoryAddressInfo_Tables.Where(x => x.FactoryID == FactoryID).First();
            UpdateAddress.StreetName = StreetName;
            UpdateAddress.BuldingNo = BuldingNo;
            UpdateAddress.FloorNo = FloorNo;
            UpdateAddress.Area = Area;
            UpdateAddress.City = City;
            UpdateAddress.Governorate = Governorate;

            /*
            var GetIDLegal = DB.factoryLegalInfo_Tables.Where(x => x.FactoryID == FactoryID).FirstOrDefault();
            int IDLegal = GetIDLegal.ID;
            */
            var UpdateLegal = DB.FactoryLegalInfo_Tables.Where(x => x.FactoryID == FactoryID).First();
            UpdateLegal.InsuranceID = InsuranceID;
            UpdateLegal.InsuranceOffice = InsuranceOffice;

            /*
            var GetIDCommunication = DB.FactoryCommInfo_Tables.Where(x => x.FactoryID == FactoryID).FirstOrDefault();
            int IDCommunication = GetIDCommunication.ID;
            */
            var UpdateCommunication = DB.FactoryCommInfo_Tables.Where(x => x.FactoryID == FactoryID).First();
            UpdateCommunication.International1 = International1;
            UpdateCommunication.Telephone1 = Telephone1;
            UpdateCommunication.TelephoneEX1 = TelephoneEX1;

            UpdateCommunication.International2 = International2;
            UpdateCommunication.Telephone2 = Telephone2;
            UpdateCommunication.TelephoneEX2 = TelephoneEX2;

            UpdateCommunication.International3 = International3;
            UpdateCommunication.Telephone3 = Telephone3;
            UpdateCommunication.TelephoneEX3 = TelephoneEX3;

            UpdateCommunication.International4 = International4;
            UpdateCommunication.Telephone4 = Telephone4;
            UpdateCommunication.TelephoneEX4 = TelephoneEX4;

            UpdateCommunication.International5 = International5;
            UpdateCommunication.Telephone5 = Telephone5;
            UpdateCommunication.TelephoneEX5 = TelephoneEX5;

            UpdateCommunication.Code1 = Code1;
            UpdateCommunication.Fax1 = Fax1;
            UpdateCommunication.FaxEX1 = FaxEX1;

            UpdateCommunication.Code2 = Code2;
            UpdateCommunication.Fax2 = Fax2;
            UpdateCommunication.FaxEX2 = FaxEX2;

            UpdateCommunication.Code3 = Code3;
            UpdateCommunication.Fax3 = Fax3;
            UpdateCommunication.FaxEX3 = FaxEX3;

            UpdateCommunication.Code4 = Code4;
            UpdateCommunication.Fax4 = Fax4;
            UpdateCommunication.FaxEX4 = FaxEX4;

            UpdateCommunication.Code5 = Code5;
            UpdateCommunication.Fax5 = Fax5;
            UpdateCommunication.FaxEX5 = FaxEX5;


            DB.SaveChanges();

            return Json(JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCompanyName(string CompanyID)
        {
            return Json(repetitionBusiness.GetCompanyName(CompanyID), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetBranchName(string BranchID)
        {
            return Json(repetitionBusiness.GetBranchName(BranchID), JsonRequestBehavior.AllowGet);
        }


        public JsonResult CheckChanges(string FactoryID)
        {

            var RMainInfo = DB.CompanyFactoryInfo_Tables.Where(x => x.FactoryID == FactoryID).FirstOrDefault();
            var RAddressInfo = DB.FactoryAddressInfo_Tables.Where(x => x.FactoryID == FactoryID).FirstOrDefault();
            var RLegalInfo = DB.FactoryLegalInfo_Tables.Where(x => x.FactoryID == FactoryID).FirstOrDefault();
            var RCommunicationInfo = DB.FactoryCommInfo_Tables.Where(x => x.FactoryID == FactoryID).FirstOrDefault();



            R_Company_Info_DTO info_DTO = new R_Company_Info_DTO()
            {
                FactoryName = RMainInfo.FactoryName,

                FactoryAlies = RMainInfo.FactoryAlies,

                BranchID = RMainInfo.BranchID,

                CompanyID = RMainInfo.CompanyID,

                EstablishDate = RMainInfo.EstablishDate,

                Status = RMainInfo.Status.ToString(),


                StreetName = RAddressInfo.StreetName,

                BuldingNo = RAddressInfo.BuldingNo,

                FloorNo = RAddressInfo.FloorNo,

                Area = RAddressInfo.Area,

                City = RAddressInfo.City,

                Governorate = RAddressInfo.Governorate,


                InsuranceID = RLegalInfo.InsuranceID,

                InsuranceOffice = RLegalInfo.InsuranceOffice,


                International1 = RCommunicationInfo.International1,

                Telephone1 = RCommunicationInfo.Telephone1,

                TelephoneEX1 = RCommunicationInfo.TelephoneEX1,

                International2 = RCommunicationInfo.International2,

                Telephone2 = RCommunicationInfo.Telephone2,

                TelephoneEX2 = RCommunicationInfo.TelephoneEX2,

                International3 = RCommunicationInfo.International3,

                Telephone3 = RCommunicationInfo.Telephone3,

                TelephoneEX3 = RCommunicationInfo.TelephoneEX3,

                International4 = RCommunicationInfo.International4,

                Telephone4 = RCommunicationInfo.Telephone4,

                TelephoneEX4 = RCommunicationInfo.TelephoneEX4,

                International5 = RCommunicationInfo.International5,

                Telephone5 = RCommunicationInfo.Telephone5,

                TelephoneEX5 = RCommunicationInfo.TelephoneEX5,

                Fax1 = RCommunicationInfo.Fax1,

                FaxEX1 = RCommunicationInfo.FaxEX1,

                Fax2 = RCommunicationInfo.Fax2,

                FaxEX2 = RCommunicationInfo.FaxEX2,

                Fax3 = RCommunicationInfo.Fax3,

                FaxEX3 = RCommunicationInfo.FaxEX3,

                Fax4 = RCommunicationInfo.Fax4,

                FaxEX4 = RCommunicationInfo.FaxEX4,

                Fax5 = RCommunicationInfo.Fax5,

                FaxEX5 = RCommunicationInfo.FaxEX5,

                Code1 = RCommunicationInfo.Code1,

                Code2 = RCommunicationInfo.Code2,

                Code3 = RCommunicationInfo.Code3,

                Code4 = RCommunicationInfo.Code4,

                Code5 = RCommunicationInfo.Code5
            };

            return Json(info_DTO, JsonRequestBehavior.AllowGet);

        }


        public JsonResult FilterBranchID(string CompanyID)
        {
            DB.Configuration.ProxyCreationEnabled = false;

            var BranchList = DB.CompanyBranchInfo_Tables.Where(x => x.CompanyID == CompanyID).ToList();

            return Json(BranchList, JsonRequestBehavior.AllowGet);
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
        //            item.SCPI = Value;
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
        //        if (item.SCPI.ToString().Equals("True"))
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