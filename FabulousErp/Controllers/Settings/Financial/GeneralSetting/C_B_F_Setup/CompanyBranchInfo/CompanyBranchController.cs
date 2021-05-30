using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FabulousDB.DB_Context;
using FabulousDB.DB_Tabels.Settings.Financial.GeneralSetting.C_B_F_Setup;
using FabulousDB.DB_Tabels.Settings.Financial.GeneralSetting.C_B_F_Setup.CompanyBranchInfo;
using FabulousErp.MyRoleProvider;
using FabulousErp.Repository.Interface;
using FabulousModels.DTOModels.Settings.Financial.GeneralSettings.C_B_F_Setup.CompanyBranchInfo;
using FabulousModels.ViewModels.Settings.Financial.GeneralSetting.C_B_F_Setup.CompanyBranchInfo;

namespace FabulousErp.Controllers.Settings.Financial.GeneralSetting.CompanyBranchInfo
{
    [BranchSecurityFilter]
    [FactorySecurityFilter]
    [AuthorizationFilter]
    public class CompanyBranchController : Controller
    {

        IRepetitionBusiness repetitionBusiness;

        public CompanyBranchController(IRepetitionBusiness repetitionBusiness)
        {
            this.repetitionBusiness = repetitionBusiness;
        }

        DBContext DB = new DBContext();
        // GET: CompanyBranch
        [HttpGet]
       // [Authorize]
        [CustomAuthorize(Roles = "SCBI")]
        public ActionResult BranchInformation()
        {
            
            ViewBag.CompanyList = repetitionBusiness.RetrieveCompIDList();

            return View();
        }


        [HttpPost]
        public ActionResult BranchInformations(Branch_Collections branch_Collections)
        {
            ViewBag.CompanyList = repetitionBusiness.RetrieveCompIDList();

            if (ModelState.IsValid)
            {

                var CheckUniqueMain = DB.CompanyBranchInfo_Tables.Where(x => x.BranchID == branch_Collections.Branch_Main_Info.BranchID).FirstOrDefault();


                if (CheckUniqueMain != null)
                {
                    //Response.Write("<script> alert('There Exist Branch Take The Same ID') </script>");
                    return Json("BranchIDExist", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    CompanyBranchInfo_Table BranchInfo_Table = new CompanyBranchInfo_Table()
                    {
                        BranchID = branch_Collections.Branch_Main_Info.BranchID,

                        BranchName = branch_Collections.Branch_Main_Info.Branchname,

                        BranchAlies = branch_Collections.Branch_Main_Info.BranchAlies,

                        CompanyID = branch_Collections.Branch_Main_Info.CompanyID,

                        EstablishDate = branch_Collections.Branch_Main_Info.EstablishDate,

                        Status = branch_Collections.Branch_Main_Info.Status
                    };

                    DB.CompanyBranchInfo_Tables.Add(BranchInfo_Table);
                    DB.SaveChanges();

                    string lastBranchID = BranchInfo_Table.BranchID;

                    BranchAddressInfo_Table address_Table = new BranchAddressInfo_Table()
                    {
                        BranchID = lastBranchID,

                        StreetName = branch_Collections.Address_Information.StreetName,

                        BuldingNo = branch_Collections.Address_Information.BuldingNo,

                        FloorNo = branch_Collections.Address_Information.FloorNo,

                        Area = branch_Collections.Address_Information.Area,

                        City = branch_Collections.Address_Information.City,

                        Governorate = branch_Collections.Address_Information.Governorate
                    };

                    DB.BranchAddressInfo_Tables.Add(address_Table);
                    DB.SaveChanges();

                    BranchLegalInfo_Table legal_Table = new BranchLegalInfo_Table()
                    {
                        BranchID = lastBranchID,

                        InsuranceID = branch_Collections.Branch_Legal_Info.InsuranceID,

                        InsuranceOffice = branch_Collections.Branch_Legal_Info.InsuranceOffice
                    };

                    DB.BranchLegalInfo_Tables.Add(legal_Table);
                    DB.SaveChanges();

                    BranchCommInfo_Table communication_Table = new BranchCommInfo_Table()
                    {
                        BranchID = lastBranchID,

                        International1 = branch_Collections.Communication_Info.International1,

                        Telephone1 = branch_Collections.Communication_Info.Telephone1,

                        TelephoneEX1 = branch_Collections.Communication_Info.TelephoneEX1,

                        International2 = branch_Collections.Communication_Info.International2,

                        Telephone2 = branch_Collections.Communication_Info.Telephone2,

                        TelephoneEX2 = branch_Collections.Communication_Info.TelephoneEX2,

                        International3 = branch_Collections.Communication_Info.International3,

                        Telephone3 = branch_Collections.Communication_Info.Telephone3,

                        TelephoneEX3 = branch_Collections.Communication_Info.TelephoneEX3,

                        International4 = branch_Collections.Communication_Info.International4,

                        Telephone4 = branch_Collections.Communication_Info.Telephone4,

                        TelephoneEX4 = branch_Collections.Communication_Info.TelephoneEX4,

                        International5 = branch_Collections.Communication_Info.International5,

                        Telephone5 = branch_Collections.Communication_Info.Telephone5,

                        TelephoneEX5 = branch_Collections.Communication_Info.TelephoneEX5,

                        Fax1 = branch_Collections.Communication_Info.Fax1,

                        FaxEX1 = branch_Collections.Communication_Info.FaxEX1,

                        Fax2 = branch_Collections.Communication_Info.Fax2,

                        FaxEX2 = branch_Collections.Communication_Info.FaxEX2,

                        Fax3 = branch_Collections.Communication_Info.Fax3,

                        FaxEX3 = branch_Collections.Communication_Info.FaxEX3,

                        Fax4 = branch_Collections.Communication_Info.Fax4,

                        FaxEX4 = branch_Collections.Communication_Info.FaxEX4,

                        Fax5 = branch_Collections.Communication_Info.Fax5,

                        FaxEX5 = branch_Collections.Communication_Info.FaxEX5,

                        Code1 = branch_Collections.Communication_Info.Code1,

                        Code2 = branch_Collections.Communication_Info.Code2,

                        Code3 = branch_Collections.Communication_Info.Code3,

                        Code4 = branch_Collections.Communication_Info.Code4,

                        Code5 = branch_Collections.Communication_Info.Code5
                    };


                    DB.BranchCommInfo_Tables.Add(communication_Table);
                    DB.SaveChanges();

                    return Json("Done", JsonRequestBehavior.AllowGet);

                    //ViewBag.Message = ViewBag.Message + "You Can Update Now If You Need...";
                }
            }
            return Json(JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetBranchInfo(string BranchID, string CompanyID)
        {
            var RMainInfo = DB.CompanyBranchInfo_Tables.Where(x => x.BranchID == BranchID && x.CompanyID == CompanyID).FirstOrDefault();
            var RAddressInfo = DB.BranchAddressInfo_Tables.Where(x => x.BranchID == BranchID).FirstOrDefault();
            var RLegalInfo = DB.BranchLegalInfo_Tables.Where(x => x.BranchID == BranchID).FirstOrDefault();
            var RCommunicationInfo = DB.BranchCommInfo_Tables.Where(x => x.BranchID == BranchID).FirstOrDefault();           

            if (RMainInfo != null)
            {

                R_Branch_Info_DTO info_DTO = new R_Branch_Info_DTO()
                {
                    Branchname = RMainInfo.BranchName,

                    BranchAlies = RMainInfo.BranchAlies,

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
            else {
                return Json("False", JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult UpdateBranchInfo(string BranchID, string Branchname, string BranchAlies,

            string StreetName, string BuldingNo, string FloorNo, string Governorate, string Area, string City,string InsuranceID, string InsuranceOffice, string International1, string Telephone1, string TelephoneEX1,

            string International2, string Telephone2, string TelephoneEX2, string International3, string Telephone3, string TelephoneEX3, string International4, string Telephone4, string TelephoneEX4,

            string International5, string Telephone5, string TelephoneEX5, string Fax1, string FaxEX1, string Fax2, string FaxEX2, string Fax3, string FaxEX3, string Fax4, string FaxEX4,

            string Fax5, string FaxEX5, string EstablishDate, string Code1, string Code2, string Code3, string Code4, string Code5, bool Status)

        {
            /*
            var GetIDMain = DB.companyBranchInfo_Tables.Where(x => x.BranchID == BranchID).FirstOrDefault();
            int IDMain = GetIDMain.ID;
            */
            var UpdateMain = DB.CompanyBranchInfo_Tables.Where(x => x.BranchID == BranchID).First();
            UpdateMain.BranchName = Branchname;
            UpdateMain.BranchAlies = BranchAlies;
            UpdateMain.EstablishDate = EstablishDate;
            UpdateMain.Status = Status;

            /*
            var GetIDAdress = DB.AddressInformation_Tables.Where(x => x.SpecificId == BranchID).FirstOrDefault();
            int IDAddress = GetIDAdress.ID;
            */
            var UpdateAddress = DB.BranchAddressInfo_Tables.Where(x => x.BranchID == BranchID).First();
            UpdateAddress.StreetName = StreetName;
            UpdateAddress.BuldingNo = BuldingNo;
            UpdateAddress.FloorNo = FloorNo;
            UpdateAddress.Area = Area;
            UpdateAddress.City = City;
            UpdateAddress.Governorate = Governorate;

            /*
            var GetIDLegal = DB.branchLegalInfo_Tables.Where(x => x.BranchID == BranchID).FirstOrDefault();
            int IDLegal = GetIDLegal.ID;
            */
            var UpdateLegal = DB.BranchLegalInfo_Tables.Where(x => x.BranchID == BranchID).First();
            UpdateLegal.InsuranceID = InsuranceID;
            UpdateLegal.InsuranceOffice = InsuranceOffice;

            /*
            var GetIDCommunication = DB.BranchCommInfo_Tables.Where(x => x.BranchID == BranchID).FirstOrDefault();
            int IDCommunication = GetIDCommunication.ID;
            */
            var UpdateCommunication = DB.BranchCommInfo_Tables.Where(x => x.BranchID == BranchID).First();
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


            var UpdateFactoryActive = DB.CompanyFactoryInfo_Tables.Where(x => x.BranchID == BranchID).ToList();

            if (UpdateFactoryActive != null)
            {
                foreach (var Factory in UpdateFactoryActive)
                {
                    /*
                    var FactoryIDActive = Factory.ID;
                    var UpdateFactoryActive = DB.companyFactoryInfo_Tables.Where(x => x.ID == FactoryIDActive).FirstOrDefault();
                    */
                    Factory.Status = Status;
                }
            }


            DB.SaveChanges();

            return Json(JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCompanyName(string CompanyID)
        {
            return Json(repetitionBusiness.GetCompanyName(CompanyID), JsonRequestBehavior.AllowGet);
        }


        public JsonResult CheckChanges(string BranchID)
        {
            var RMainInfo = DB.CompanyBranchInfo_Tables.Where(x => x.BranchID == BranchID).FirstOrDefault();
            var RAddressInfo = DB.BranchAddressInfo_Tables.Where(x => x.BranchID == BranchID).FirstOrDefault();
            var RLegalInfo = DB.BranchLegalInfo_Tables.Where(x => x.BranchID == BranchID).FirstOrDefault();
            var RCommunicationInfo = DB.BranchCommInfo_Tables.Where(x => x.BranchID == BranchID).FirstOrDefault();


                R_Branch_Info_DTO info_DTO = new R_Branch_Info_DTO()
                {
                    Branchname = RMainInfo.BranchName,

                    BranchAlies = RMainInfo.BranchAlies,

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
        //            item.SCBI = Value;
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
        //        if (item.SCBI.ToString().Equals("True"))
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