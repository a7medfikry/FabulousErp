using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FabulousDB.DB_Context;
using FabulousDB.DB_Tabels.Settings.Financial.Accounting.CurrenciesDefinition;
using FabulousDB.DB_Tabels.Settings.Financial.GeneralSetting.C_B_F_Setup;
using FabulousDB.DB_Tabels.Settings.Financial.GeneralSetting.C_B_F_Setup.CompanyInformation;
using FabulousErp.MyRoleProvider;
using FabulousModels.DTOModels.Settings.Financial.GeneralSettings.C_B_F_Setup.CompanyInformation;
using FabulousModels.ViewModels.Settings.Financial.GeneralSetting.C_B_F_Setup.CompanyInformation;

namespace FabulousErp.Controllers.Settings.Financial.GeneralSetting.CompanyInformation
{
    [BranchSecurityFilter]
    [FactorySecurityFilter]
    [AuthorizationFilter]
    public class MainCompanyController : Controller
    {

        DBContext DB = new DBContext();

        // GET: MainCompany
        [HttpGet]
       // [Authorize]
        [CustomAuthorize(Roles = "SCL")]
        public ActionResult CompanyInformation()
        {
            string CompId = Business.GetCompanyId();

            return View();
        }
       
        public JsonResult GetCurrencyUnit(string CurrId)
        {
            if (CurrId != null)
            {
                CurrenciesDefinition_Table ThisCurr = DB.CurrenciesDefinition_Tables.FirstOrDefault(x => x.CurrencyID == CurrId);
                return Json(new { ThisCurr.Currency_unit_name, ThisCurr.Currency_small_unit_name });
            }
            else
            {
                return Json("");
            }
        }
        [HttpPost]
        public ActionResult CompanyInformations(Company_Main_Info company_Main_Info, Company_Legal_Info company_Legal_Info, string CompanyID, string CompanyName, string CompanyAlies, string CountryName, string Language, string CompanyMainActivity,

            string StreetName, string BuldingNo, string FloorNo, string Governorate, string Area, string City, string CompanyType, string EstablishDate,

            string CommericalRegister, string CommericalOffice, string TaxFileNo, string TaxOffice, string VatID, string TaxVaOffice, string ImporterID, string ImportOffice,

            string ExportID, string ExportOffice, string SocialInsuranceID, string SocialInsuranceOffice, string International1, string Telephone1, string TelephoneEX1,

            string International2, string Telephone2, string TelephoneEX2, string International3, string Telephone3, string TelephoneEX3, string International4, string Telephone4, string TelephoneEX4,

            string International5, string Telephone5, string TelephoneEX5, string Fax1, string FaxEX1, string Fax2, string FaxEX2, string Fax3, string FaxEX3, string Fax4, string FaxEX4,

            string Fax5, string FaxEX5, string Website, string Code1, string Code2, string Code3, string Code4, string Code5, bool Status, string Currency)
        {
            string Logo_FileName = "";
            string Logo_Path = "";
            byte[] imagebyte = null;
            var imgsrc = "";

            HttpPostedFileBase InputLogo = company_Main_Info.InputLogo;
            if (InputLogo != null)
            {
                Logo_FileName = Path.GetFileName(InputLogo.FileName);
                Logo_Path = Path.Combine(Server.MapPath("~/UploadedFiles"), Logo_FileName);
                InputLogo.SaveAs(Logo_Path);
                BinaryReader reader = new BinaryReader(InputLogo.InputStream);
                imagebyte = reader.ReadBytes(InputLogo.ContentLength);

                var base64 = Convert.ToBase64String(imagebyte);
                imgsrc = string.Format("data:image/gif;base64,{0}", base64);
            }

            string Comm_FileName = "";
            string Comm_Path = "";

            HttpPostedFileBase InputCommerical = company_Legal_Info.InputCommerical;
            if (InputCommerical != null)
            {
                Comm_FileName = Path.GetFileName(InputCommerical.FileName);
                Comm_Path = Path.Combine(Server.MapPath("~/UploadedFiles"), Comm_FileName);
                InputCommerical.SaveAs(Comm_Path);                
            }



            string Tax_FileName = "";
            string Tax_Path = "";

            HttpPostedFileBase InputTax = company_Legal_Info.InputTax;
            if (InputTax != null)
            {
                Tax_FileName = Path.GetFileName(InputTax.FileName);
                Tax_Path = Path.Combine(Server.MapPath("~/UploadedFiles"), Tax_FileName);
                InputTax.SaveAs(Tax_Path);
            }



            string Vat_FileName = "";
            string VAt_Path = "";

            HttpPostedFileBase InputVat = company_Legal_Info.InputVat;
            if (InputVat != null)
            {
                Vat_FileName = Path.GetFileName(InputVat.FileName);
                VAt_Path = Path.Combine(Server.MapPath("~/UploadedFiles"), Vat_FileName);
                InputVat.SaveAs(VAt_Path);
            }


            string Import_FileName = "";
            string Import_Path = "";

            HttpPostedFileBase InputImport = company_Legal_Info.InputImport;
            if (InputImport != null)
            {
                Import_FileName = Path.GetFileName(InputImport.FileName);
                Import_Path = Path.Combine(Server.MapPath("~/UploadedFiles"), Import_FileName);
                InputImport.SaveAs(Import_Path);
            }


            string Export_FileName = "";
            string Export_Path = "";

            HttpPostedFileBase InputExport = company_Legal_Info.InputExport;
            if (InputExport != null)
            {
                Export_FileName = Path.GetFileName(InputExport.FileName);
                Export_Path = Path.Combine(Server.MapPath("~/UploadedFiles"), Export_FileName);
                InputExport.SaveAs(Export_Path);
            }


            string Insurance_FileName = "";
            string Insurance_Path = "";

            HttpPostedFileBase InputInsurance = company_Legal_Info.InputInsurance;
            if (InputInsurance != null)
            {
                Insurance_FileName = Path.GetFileName(InputInsurance.FileName);
                Insurance_Path = Path.Combine(Server.MapPath("~/UploadedFiles"), Insurance_FileName);
                InputInsurance.SaveAs(Insurance_Path);
            }


            if (ModelState.IsValid)
            {

                var CheckUniqueMain = DB.CompanyMainInfo_Tables.Where(x => x.CompanyID == CompanyID).FirstOrDefault();


                if (CheckUniqueMain != null)
                {
                    //Response.Write("<script> alert('There Exist Company Take The Same ID') </script>");
                    return Json("CompanyIDExist", JsonRequestBehavior.AllowGet);
                }
                else
                {


                    CompanyMainInfo_Table info_Table = new CompanyMainInfo_Table
                    {
                        CompanyID = CompanyID,

                        CompanyName = CompanyName,

                        CompanyAlies = CompanyAlies,

                        CompanyMainActivity = CompanyMainActivity,

                        CountryName = CountryName,

                        Currency = Currency,

                        Language = Language,

                        Status = Status,

                        LogoName = Logo_FileName,

                        LogoPath = Logo_Path,

                        LogoByte = imagebyte                     

                    };

                    DB.CompanyMainInfo_Tables.Add(info_Table);
                    DB.SaveChanges();

                    string lastCompanyID = info_Table.CompanyID;

                    CompanyAddressInfo_Table address_Table = new CompanyAddressInfo_Table
                    {
                        CompanyID = lastCompanyID,

                        StreetName = StreetName,

                        BuldingNo = BuldingNo,

                        FloorNo = FloorNo,

                        Area = Area,

                        City = City,

                        Governorate = Governorate

                    };

                    DB.CompanyAddressInfo_Tables.Add(address_Table);
                    DB.SaveChanges();

                    CompanyLegalInfo_Table legal_Table = new CompanyLegalInfo_Table
                    {
                        CompanyID = lastCompanyID,

                        CompanyType = CompanyType,

                        EstablishDate = EstablishDate,

                        CommericalRegister = CommericalRegister,

                        CommericalOffice = CommericalOffice,

                        TaxFileNo = TaxFileNo,

                        TaxOffice = TaxOffice,

                        VatID = VatID,

                        TaxVaOffice = TaxVaOffice,

                        ImporterID = ImporterID,

                        ImportOffice = ImportOffice,

                        ExportID = ExportID,

                        ExportOffice = ExportOffice,

                        SocialInsuranceID = SocialInsuranceID,

                        SocialInsuranceOffice = SocialInsuranceOffice,

                        CommericalRegisterName = Comm_FileName,

                        CommericalRegisterPath = Comm_Path,

                        TaxFileName = Tax_FileName,

                        TaxFilePath = Tax_Path,

                        VatIDName = Vat_FileName,

                        VatIDPath = VAt_Path,

                        ExportIDName = Export_FileName,

                        ExportIDPath = Export_Path,

                        ImportIDName = Import_FileName,

                        ImportIDPath = Import_Path,

                        InsuranceIDName = Insurance_FileName,

                        InsuranceIDPath = Insurance_Path
                    };

                    DB.CompanyLegalInfo_Tables.Add(legal_Table);
                    DB.SaveChanges();

                    CompanyCommInfo_Table communication_Table = new CompanyCommInfo_Table()
                    {
                        CompanyID = lastCompanyID,

                        International1 = International1,

                        Telephone1 = Telephone1,

                        TelephoneEX1 = TelephoneEX1,

                        International2 = International2,

                        Telephone2 = Telephone2,

                        TelephoneEX2 = TelephoneEX2,

                        International3 = International3,

                        Telephone3 = Telephone3,

                        TelephoneEX3 = TelephoneEX3,

                        International4 = International4,

                        Telephone4 = Telephone4,

                        TelephoneEX4 = TelephoneEX4,

                        International5 = International5,

                        Telephone5 = Telephone5,

                        TelephoneEX5 = TelephoneEX5,

                        Fax1 = Fax1,

                        FaxEX1 = FaxEX1,

                        Fax2 = Fax2,

                        FaxEX2 = FaxEX2,

                        Fax3 = Fax3,

                        FaxEX3 = FaxEX3,

                        Fax4 = Fax4,

                        FaxEX4 = FaxEX4,

                        Fax5 = Fax5,

                        FaxEX5 = FaxEX5,

                        Website = Website,

                        Code1 = Code1,

                        Code2 = Code2,

                        Code3 = Code3,

                        Code4 = Code4,

                        Code5 = Code5
                    };

                    
                    DB.CompanyCommInfo_Tables.Add(communication_Table);
                    DB.SaveChanges();

                    CurrenciesDefinition_Table currenciesDefinition_Table = new CurrenciesDefinition_Table()
                    {
                        CompanyID = lastCompanyID,

                        CurrencyID = lastCompanyID,

                        CurrencyName = "Main Currency",
                        
                        ISOCode = Currency
                    };

                    DB.CurrenciesDefinition_Tables.Add(currenciesDefinition_Table);
                    DB.SaveChanges();

                    return Json("Done", JsonRequestBehavior.AllowGet);
                }

            }
            else{
                return null;
            }
        }


        public JsonResult GetCompanyInfo(string CompanyID)
        {
            var RMainInfo = DB.CompanyMainInfo_Tables.Where(x => x.CompanyID == CompanyID).FirstOrDefault();
            var RAddressInfo = DB.CompanyAddressInfo_Tables.Where(x => x.CompanyID == CompanyID).FirstOrDefault();
            var RLegalInfo = DB.CompanyLegalInfo_Tables.Where(x => x.CompanyID == CompanyID).FirstOrDefault();
            var RCommunicationInfo = DB.CompanyCommInfo_Tables.Where(x => x.CompanyID == CompanyID).FirstOrDefault();

            if (RMainInfo != null)
            {

                var fullPath = "";
                if (RMainInfo.LogoByte != null)
                {
                    var base64 = Convert.ToBase64String(RMainInfo.LogoByte);
                    fullPath = string.Format("data:image/gif;base64,{0}", base64);
                }

                R_Company_Info_DTO info_DTO = new R_Company_Info_DTO()
                {
                    CompanyName = RMainInfo.CompanyName,

                    CompanyAlies = RMainInfo.CompanyAlies,

                    CountryName = RMainInfo.CountryName,

                    Language = RMainInfo.Language,

                    Currency = RMainInfo.Currency,

                    CompanyMainActivity = RMainInfo.CompanyMainActivity,

                    Logo = fullPath,

                    Status = RMainInfo.Status.ToString(),


                    StreetName = RAddressInfo.StreetName,

                    BuldingNo = RAddressInfo.BuldingNo,

                    FloorNo = RAddressInfo.FloorNo,

                    Area = RAddressInfo.Area,

                    City = RAddressInfo.City,

                    Governorate = RAddressInfo.Governorate,


                    CompanyType = RLegalInfo.CompanyType,

                    EstablishDate = RLegalInfo.EstablishDate,

                    CommericalRegister = RLegalInfo.CommericalRegister,

                    CommericalOffice = RLegalInfo.CommericalOffice,

                    TaxFileNo = RLegalInfo.TaxFileNo,

                    TaxOffice = RLegalInfo.TaxOffice,

                    VatID = RLegalInfo.VatID,

                    TaxVaOffice = RLegalInfo.TaxVaOffice,

                    ImporterID = RLegalInfo.ImporterID,

                    ImportOffice = RLegalInfo.ImportOffice,

                    ExportID = RLegalInfo.ExportID,

                    ExportOffice = RLegalInfo.ExportOffice,

                    SocialInsuranceID = RLegalInfo.SocialInsuranceID,

                    SocialInsuranceOffice = RLegalInfo.SocialInsuranceOffice,
                    
                    CommericalRegisterPath = RLegalInfo.CommericalRegisterPath,

                    ExportIDPath = RLegalInfo.ExportIDPath,

                    ImportIDPath = RLegalInfo.ImportIDPath,

                    TaxFilePath = RLegalInfo.TaxFilePath,

                    VatIDPath = RLegalInfo.VatIDPath,

                    InsuranceIDPath = RLegalInfo.InsuranceIDPath,
                    


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

                    Code5 = RCommunicationInfo.Code5,

                    Website = RCommunicationInfo.Website
                };
                /*
                var jsonResult = Json(info_DTO, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                */
                return Json(info_DTO, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("NoCompanyExist", JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult UpdateCompanyInfo(Company_Main_Info company_Main_Info, Company_Legal_Info company_Legal_Info , string CompanyID, string CompanyName , string CompanyAlies , string CountryName , string Language , string CompanyMainActivity,

            string StreetName , string BuldingNo , string FloorNo , string Governorate , string Area , string City , string CompanyType , string EstablishDate,
            
            string CommericalRegister, string CommericalOffice, string TaxFileNo, string TaxOffice, string VatID, string TaxVaOffice , string ImporterID, string ImportOffice,
            
            string ExportID , string ExportOffice , string SocialInsuranceID, string SocialInsuranceOffice , string International1 , string Telephone1 , string TelephoneEX1,

            string International2, string Telephone2, string TelephoneEX2, string International3, string Telephone3, string TelephoneEX3, string International4, string Telephone4, string TelephoneEX4,

            string International5, string Telephone5, string TelephoneEX5, string Fax1 , string FaxEX1 , string Fax2, string FaxEX2, string Fax3, string FaxEX3, string Fax4, string FaxEX4,

            string Fax5, string FaxEX5, string Website, string Code1, string Code2, string Code3, string Code4, string Code5, bool Status)

        {
            HttpPostedFileBase InputLogo = company_Main_Info.InputLogo;

           
            if (InputLogo != null && InputLogo.ContentLength > 0)

            {
                string Logo_FileName = Path.GetFileName(InputLogo.FileName);
                string Logo_Path = Path.Combine(Server.MapPath("~/UploadedFiles"), Logo_FileName);
                InputLogo.SaveAs(Logo_Path);
                BinaryReader reader = new BinaryReader(InputLogo.InputStream);
                byte[] imagebyte = reader.ReadBytes(InputLogo.ContentLength);

                var UpdateMainI = DB.CompanyMainInfo_Tables.Where(x => x.CompanyID == CompanyID).First();

                UpdateMainI.LogoName = Logo_FileName;
                UpdateMainI.LogoPath = Logo_Path;
                UpdateMainI.LogoByte = imagebyte;
            }


            
            HttpPostedFileBase InputCommerical = company_Legal_Info.InputCommerical;
            if (InputCommerical != null && InputCommerical.ContentLength > 0)
            {
                string Comm_FileName = Path.GetFileName(InputCommerical.FileName);
                string Comm_Path = Path.Combine(Server.MapPath("~/UploadedFiles"), Comm_FileName);
                InputCommerical.SaveAs(Comm_Path);
                
                var UpdateLegalI = DB.CompanyLegalInfo_Tables.Where(x => x.CompanyID == CompanyID).First();
                UpdateLegalI.CommericalRegisterName = Comm_FileName;
                UpdateLegalI.CommericalRegisterPath = Comm_Path;
            }




            HttpPostedFileBase InputTax = company_Legal_Info.InputTax;
            if (InputTax != null && InputTax.ContentLength > 0)
            {
                string Tax_FileName = Path.GetFileName(InputTax.FileName);
                string Tax_Path = Path.Combine(Server.MapPath("~/UploadedFiles"), Tax_FileName);
                InputTax.SaveAs(Tax_Path);
                
                var UpdateLegalI = DB.CompanyLegalInfo_Tables.Where(x => x.CompanyID == CompanyID).First();
                UpdateLegalI.TaxFileName = Tax_FileName;
                UpdateLegalI.TaxFilePath = Tax_Path;
            }



           
            HttpPostedFileBase InputVat = company_Legal_Info.InputVat;
            if (InputVat != null && InputVat.ContentLength > 0)
            {
                string Vat_FileName = Path.GetFileName(InputVat.FileName);
                string VAt_Path = Path.Combine(Server.MapPath("~/UploadedFiles"), Vat_FileName);
                InputVat.SaveAs(VAt_Path);
                
                var UpdateLegalI = DB.CompanyLegalInfo_Tables.Where(x => x.CompanyID == CompanyID).First();
                UpdateLegalI.VatIDName = Vat_FileName;
                UpdateLegalI.VatIDPath = VAt_Path;
            }


           
            HttpPostedFileBase InputImport = company_Legal_Info.InputImport;
            if (InputImport != null && InputImport.ContentLength > 0)
            {
                string Import_FileName = Path.GetFileName(InputImport.FileName);
                string Import_Path = Path.Combine(Server.MapPath("~/UploadedFiles"), Import_FileName);
                InputImport.SaveAs(Import_Path);
                
                var UpdateLegalI = DB.CompanyLegalInfo_Tables.Where(x => x.CompanyID == CompanyID).First();
                UpdateLegalI.ImportIDName = Import_FileName;
                UpdateLegalI.ImportIDPath = Import_Path;
            }


           
            HttpPostedFileBase InputExport = company_Legal_Info.InputExport;
            if (InputExport != null && InputExport.ContentLength > 0)
            {
                string Export_FileName = Path.GetFileName(InputExport.FileName);
                string Export_Path = Path.Combine(Server.MapPath("~/UploadedFiles"), Export_FileName);
                InputExport.SaveAs(Export_Path);
                
                var UpdateLegalI = DB.CompanyLegalInfo_Tables.Where(x => x.CompanyID == CompanyID).First();
                UpdateLegalI.ExportIDName = Export_FileName;
                UpdateLegalI.ExportIDPath = Export_Path;
            }


           
            HttpPostedFileBase InputInsurance = company_Legal_Info.InputInsurance;
            if (InputInsurance != null && InputInsurance.ContentLength > 0)
            {
                string Insurance_FileName = Path.GetFileName(InputInsurance.FileName);
                string Insurance_Path = Path.Combine(Server.MapPath("~/UploadedFiles"), Insurance_FileName);
                InputInsurance.SaveAs(Insurance_Path);
                
                var UpdateLegalI = DB.CompanyLegalInfo_Tables.Where(x => x.CompanyID == CompanyID).First();
                UpdateLegalI.InsuranceIDName = Insurance_FileName;
                UpdateLegalI.InsuranceIDPath = Insurance_Path;
            }


           
            var UpdateMain = DB.CompanyMainInfo_Tables.Where(x => x.CompanyID == CompanyID).First();
            UpdateMain.CompanyName = CompanyName;
            UpdateMain.CompanyAlies = CompanyAlies;
            UpdateMain.CountryName = CountryName;
            UpdateMain.Language = Language;
            UpdateMain.CompanyMainActivity = CompanyMainActivity;
            UpdateMain.Status = Status;

           
            var UpdateAddress = DB.CompanyAddressInfo_Tables.Where(x => x.CompanyID == CompanyID).First();

            UpdateAddress.StreetName = StreetName;
            UpdateAddress.BuldingNo = BuldingNo;
            UpdateAddress.FloorNo =FloorNo;
            UpdateAddress.Area = Area;
            UpdateAddress.City = City;
            UpdateAddress.Governorate = Governorate;

            
            var UpdateLegal = DB.CompanyLegalInfo_Tables.Where(x => x.CompanyID == CompanyID).First();
            UpdateLegal.CompanyType = CompanyType;
            UpdateLegal.EstablishDate = EstablishDate;
            UpdateLegal.CommericalRegister = CommericalRegister;
            UpdateLegal.CommericalOffice = CommericalOffice;
            UpdateLegal.TaxFileNo = TaxFileNo;
            UpdateLegal.TaxOffice = TaxOffice;
            UpdateLegal.VatID = VatID;
            UpdateLegal.TaxVaOffice = TaxVaOffice;
            UpdateLegal.ImporterID = ImporterID;
            UpdateLegal.ImportOffice = ImportOffice;
            UpdateLegal.ExportID = ExportID;
            UpdateLegal.ExportOffice = ExportOffice;
            UpdateLegal.SocialInsuranceID = SocialInsuranceID;
            UpdateLegal.SocialInsuranceOffice = SocialInsuranceOffice;
            

            
            var UpdateCommunication = DB.CompanyCommInfo_Tables.Where(x => x.CompanyID == CompanyID).FirstOrDefault();
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

            UpdateCommunication.Website = Website;


            var UpdateBranchActive = DB.CompanyBranchInfo_Tables.Where(x => x.CompanyID == CompanyID).ToList();

            if (UpdateBranchActive != null)
            {
                foreach(var Branch in UpdateBranchActive)
                {
                  
                    Branch.Status = Status;
                }
            }

            var UpdateFactoryActive = DB.CompanyFactoryInfo_Tables.Where(x => x.CompanyID == CompanyID).ToList();

            if (UpdateFactoryActive != null)
            {
                foreach(var Factory in UpdateFactoryActive)
                {
                   
                    Factory.Status = Status;
                }
            }

            DB.SaveChanges();

            return Json(JsonRequestBehavior.AllowGet);
        }
        


        public ActionResult DownloadComm(string CompanyID)
        {
            var RMainInfo = DB.CompanyLegalInfo_Tables.Where(x => x.CompanyID == CompanyID).FirstOrDefault();

            string _FileName = RMainInfo.CommericalRegisterName;
            string _Path = RMainInfo.CommericalRegisterPath;

            //string fullPath = Path.Combine(Server.MapPath("~/UploadedFiles"), _FileName);

            if(_FileName.Length > 0 && _Path.Length > 0)
            {
                if (System.IO.File.Exists(_Path))
                {
                    byte[] fileBytes = System.IO.File.ReadAllBytes(_Path);

                    return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, _FileName);
                }
                else
                {
                    return Json("False", JsonRequestBehavior.AllowGet);
                }

            }
            else
            {
                return Json(JsonRequestBehavior.AllowGet);
            }
           
        }


        public ActionResult DownloadTax(string CompanyID)
        {
            var RMainInfo = DB.CompanyLegalInfo_Tables.Where(x => x.CompanyID == CompanyID).FirstOrDefault();

            string _FileName = RMainInfo.TaxFileName;
            string _Path = RMainInfo.TaxFilePath;

            //string fullPath = Path.Combine(Server.MapPath("~/UploadedFiles"), _FileName);

            if (_FileName.Length > 0 && _Path.Length > 0)
            {
                if (System.IO.File.Exists(_Path))
                {
                    byte[] fileBytes = System.IO.File.ReadAllBytes(_Path);


                    return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, _FileName);
                }
                else
                {
                    return Json("False", JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(JsonRequestBehavior.AllowGet);
            }
        }


        public ActionResult DownloadVat(string CompanyID)
        {
            var RMainInfo = DB.CompanyLegalInfo_Tables.Where(x => x.CompanyID == CompanyID).FirstOrDefault();

            string _FileName = RMainInfo.VatIDName;
            string _Path = RMainInfo.VatIDPath;

            //string fullPath = Path.Combine(Server.MapPath("~/UploadedFiles"), _FileName);

            if (_FileName.Length > 0 && _Path.Length > 0)
            {
                if (System.IO.File.Exists(_Path))
                {
                    byte[] fileBytes = System.IO.File.ReadAllBytes(_Path);


                    return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, _FileName);
                }
                else
                {
                    return Json("False", JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(JsonRequestBehavior.AllowGet);
            }
        }


        public ActionResult DownloadImport(string CompanyID)
        {
            var RMainInfo = DB.CompanyLegalInfo_Tables.Where(x => x.CompanyID == CompanyID).FirstOrDefault();

            string _FileName = RMainInfo.ImportIDName;
            string _Path = RMainInfo.ImportIDPath;

            //string fullPath = Path.Combine(Server.MapPath("~/UploadedFiles"), _FileName);

            if (_FileName.Length > 0 && _Path.Length > 0)
            {
                if (System.IO.File.Exists(_Path))
                {
                    byte[] fileBytes = System.IO.File.ReadAllBytes(_Path);


                    return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, _FileName);
                }
                else
                {
                    return Json("False", JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(JsonRequestBehavior.AllowGet);
            }
        }


        public ActionResult DownloadExport(string CompanyID)
        {
            var RMainInfo = DB.CompanyLegalInfo_Tables.Where(x => x.CompanyID == CompanyID).FirstOrDefault();

            string _FileName = RMainInfo.ExportIDName;
            string _Path = RMainInfo.ExportIDPath;

            //string fullPath = Path.Combine(Server.MapPath("~/UploadedFiles"), _FileName);

            if (_FileName.Length > 0 && _Path.Length > 0)
            {
                if (System.IO.File.Exists(_Path))
                {
                    byte[] fileBytes = System.IO.File.ReadAllBytes(_Path);


                    return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, _FileName);
                }
                else
                {
                    return Json("False", JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(JsonRequestBehavior.AllowGet);
            }
        }


        public ActionResult DownloadInsurance(string CompanyID)
        {
            var RMainInfo = DB.CompanyLegalInfo_Tables.Where(x => x.CompanyID == CompanyID).FirstOrDefault();

            string _FileName = RMainInfo.InsuranceIDName;
            string _Path = RMainInfo.InsuranceIDPath;


            if (_FileName.Length > 0 && _Path.Length > 0)
            {
                if (System.IO.File.Exists(_Path))
                {
                    byte[] fileBytes = System.IO.File.ReadAllBytes(_Path);


                    return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, _FileName);
                }
                else
                {
                    return Json("False", JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult CheckChanges(string CompanyID)
        {
            var RMainInfo = DB.CompanyMainInfo_Tables.Where(x => x.CompanyID == CompanyID).FirstOrDefault();
            var RAddressInfo = DB.CompanyAddressInfo_Tables.Where(x => x.CompanyID == CompanyID).FirstOrDefault();
            var RLegalInfo = DB.CompanyLegalInfo_Tables.Where(x => x.CompanyID == CompanyID).FirstOrDefault();
            var RCommunicationInfo = DB.CompanyCommInfo_Tables.Where(x => x.CompanyID == CompanyID).FirstOrDefault();

            R_Company_Info_DTO info_DTO = new R_Company_Info_DTO()
            {
                CompanyName = RMainInfo.CompanyName,

                CompanyAlies = RMainInfo.CompanyAlies,

                CountryName = RMainInfo.CountryName,

                Language = RMainInfo.Language,

                Currency = RMainInfo.Currency,

                CompanyMainActivity = RMainInfo.CompanyMainActivity,

                Status = RMainInfo.Status.ToString(),


                StreetName = RAddressInfo.StreetName,

                BuldingNo = RAddressInfo.BuldingNo,

                FloorNo = RAddressInfo.FloorNo,

                Area = RAddressInfo.Area,

                City = RAddressInfo.City,

                Governorate = RAddressInfo.Governorate,


                CompanyType = RLegalInfo.CompanyType,

                EstablishDate = RLegalInfo.EstablishDate,

                CommericalRegister = RLegalInfo.CommericalRegister,

                CommericalOffice = RLegalInfo.CommericalOffice,

                TaxFileNo = RLegalInfo.TaxFileNo,

                TaxOffice = RLegalInfo.TaxOffice,

                VatID = RLegalInfo.VatID,

                TaxVaOffice = RLegalInfo.TaxVaOffice,

                ImporterID = RLegalInfo.ImporterID,

                ImportOffice = RLegalInfo.ImportOffice,

                ExportID = RLegalInfo.ExportID,

                ExportOffice = RLegalInfo.ExportOffice,

                SocialInsuranceID = RLegalInfo.SocialInsuranceID,

                SocialInsuranceOffice = RLegalInfo.SocialInsuranceOffice,

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

                Code5 = RCommunicationInfo.Code5,

                Website = RCommunicationInfo.Website
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
        //            item.SCL = Value;
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
        //        if (item.SCL.ToString().Equals("True"))
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