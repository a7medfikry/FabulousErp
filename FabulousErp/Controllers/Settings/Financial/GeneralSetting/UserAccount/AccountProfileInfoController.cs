using FabulousDB.DB_Context;
using FabulousErp.MyRoleProvider;
using FabulousModels.DTOModels.Settings.Financial.GeneralSettings.UserAccount;
using FabulousModels.ViewModels.Settings.Financial.GeneralSetting.UserAccount;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace FabulousErp.Controllers.Settings.Financial.GeneralSetting.UserAccount
{
    [AuthorizationFilter]
    public class AccountProfileInfoController : Controller
    {

        DBContext DB = new DBContext();

        // GET: AccountProfileInfo
        [HttpGet]
        [AllowAnonymous]
        public ActionResult AccountInfo(string Message)
        {
            ViewBag.Message = Message;

            return View();
        }

        [HttpPost]
        public ActionResult AccountInfos(User_Profile_Info user_Profile_Info)
        {
            string UserID = FabulousErp.Business.GetUserId();


            string CompanyID = (string)FabulousErp.Business.GetCompanyId();

            var check = DB.CreateAccount_Tables.Where(x => x.UserID == UserID).FirstOrDefault();

           

            if (ModelState.IsValid)
            {
               
                var updateUserInfo = DB.CreateAccount_Tables.Where(x => x.UserID == UserID).First();

                updateUserInfo.TitlePER = user_Profile_Info.TitlePER;

                updateUserInfo.NationalORPassportIDPER = user_Profile_Info.NationalORPassportIDPER;

                updateUserInfo.FirstNamePER = user_Profile_Info.FirstNamePER;

                updateUserInfo.LastNamePER = user_Profile_Info.LastNamePER;

                updateUserInfo.FamilyNamePER = user_Profile_Info.FamilyNamePER;

                updateUserInfo.BuldingNoPER = user_Profile_Info.BuldingNoPER;

                updateUserInfo.StreetPER = user_Profile_Info.StreetPER;

                updateUserInfo.AvenuePER = user_Profile_Info.AvenuePER;

                updateUserInfo.StatePER = user_Profile_Info.StatePER;

                updateUserInfo.CountryPER = user_Profile_Info.CountryPER;

                updateUserInfo.HomePhonePER = user_Profile_Info.HomePhonePER;

                updateUserInfo.MobilePhonePER = user_Profile_Info.MobilePhonePER;

                updateUserInfo.OthMobilePhonePER = user_Profile_Info.OthMobilePhonePER;

                updateUserInfo.PositionFUN = user_Profile_Info.PositionFUN;

                updateUserInfo.DepartmentFUN = user_Profile_Info.DepartmentFUN;

                updateUserInfo.RoomNumFUN = user_Profile_Info.RoomNumFUN;

                updateUserInfo.FloorFUN = user_Profile_Info.FloorFUN;

                updateUserInfo.BuildingFUN = user_Profile_Info.BuildingFUN;

                updateUserInfo.TelephoneNumFUN = user_Profile_Info.TelephoneNumFUN;

                updateUserInfo.TEXtentionFUN = user_Profile_Info.TEXtentionFUN;

                updateUserInfo.FaxNumFUN = user_Profile_Info.FaxNumFUN;

                updateUserInfo.FExtentionFUN = user_Profile_Info.FExtentionFUN;

                updateUserInfo.MobilePhoneFUN = user_Profile_Info.MobilePhoneFUN;

                updateUserInfo.EmailFUN = user_Profile_Info.EmailFUN;

                updateUserInfo.CityPER = user_Profile_Info.CityPER;

                updateUserInfo.UpdateProfFirst = false;

                DB.SaveChanges();

                FormsAuthentication.SetAuthCookie(UserID, false);
                
                return (RedirectToAction("dashboard", "Dashboard"));
            }

            return View();

        }

        public JsonResult GetMainInfo()
        {
            string UserID = FabulousErp.Business.GetUserId();

            if (UserID == "SA")
            {
                return Json(JsonRequestBehavior.AllowGet);
            }
            else
            {

                var getInfo = DB.CreateAccount_Tables.Where(x => x.UserID == UserID).FirstOrDefault();

                if (getInfo.TitlePER != null && getInfo.NationalORPassportIDPER != null && getInfo.FirstNamePER != null && getInfo.LastNamePER != null && getInfo.FamilyNamePER != null
                    && getInfo.BuldingNoPER != null && getInfo.StreetPER != null && getInfo.AvenuePER != null && getInfo.StatePER != null && getInfo.CountryPER != null && getInfo.MobilePhonePER != null
                    && getInfo.PositionFUN != null && getInfo.DepartmentFUN != null && getInfo.RoomNumFUN != null && getInfo.FloorFUN != null && getInfo.BuildingFUN != null && getInfo.CityPER != null)
                {



                    var fullPath = "";
                    if (getInfo.ProfilePicByte != null)
                    {
                        var base64 = Convert.ToBase64String(getInfo.ProfilePicByte);
                        fullPath = string.Format("data:image/gif;base64,{0}", base64);
                    }

                    R_User_Account_DTO r_User_Account_DTO = new R_User_Account_DTO()
                    {
                        UserName = getInfo.UserName,

                        Date = getInfo.Date,

                        LastPasswordChangedDate = getInfo.LastPasswordChangedDate,

                        MembOfGroup = getInfo.UserGroup_Table.GroupID,

                        TitlePER = getInfo.TitlePER,

                        NationalORPassportIDPER = getInfo.NationalORPassportIDPER,

                        FirstNamePER = getInfo.FirstNamePER,

                        LastNamePER = getInfo.LastNamePER,

                        FamilyNamePER = getInfo.FamilyNamePER,

                        BuldingNoPER = getInfo.BuldingNoPER,

                        StreetPER = getInfo.StreetPER,

                        CountryPER = getInfo.CountryPER,

                        AvenuePER = getInfo.AvenuePER,

                        StatePER = getInfo.StatePER,

                        HomePhonePER = getInfo.HomePhonePER,

                        MobilePhonePER = getInfo.MobilePhonePER,

                        OthMobilePhonePER = getInfo.OthMobilePhonePER,

                        PositionFUN = getInfo.PositionFUN,

                        DepartmentFUN = getInfo.DepartmentFUN,

                        RoomNumFUN = getInfo.RoomNumFUN,

                        FloorFUN = getInfo.FloorFUN,

                        BuildingFUN = getInfo.BuildingFUN,

                        TelephoneNumFUN = getInfo.TelephoneNumFUN,

                        TEXtentionFUN = getInfo.TEXtentionFUN,

                        FaxNumFUN = getInfo.FaxNumFUN,

                        FExtentionFUN = getInfo.FExtentionFUN,

                        MobilePhoneFUN = getInfo.MobilePhoneFUN,

                        EmailFUN = getInfo.EmailFUN,

                        InputPersonalImg = fullPath,

                        CityPER = getInfo.CityPER
                    };

                    return Json(r_User_Account_DTO, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(JsonRequestBehavior.AllowGet);
                }
            }
        }
      

        public JsonResult UpdateProfilePicture(User_Profile_Info user_Profile_Info)
        {
            string UserID = FabulousErp.Business.GetUserId();


            byte[] imagebyte = null;

            HttpPostedFileBase InputPp = user_Profile_Info.InputPersonalImg;
            if (InputPp != null)
            {
                BinaryReader reader = new BinaryReader(InputPp.InputStream);
                imagebyte = reader.ReadBytes(InputPp.ContentLength);

            }

            var updateUserInfo = DB.CreateAccount_Tables.Where(x => x.UserID == UserID).First();

            updateUserInfo.ProfilePicByte = imagebyte;

            DB.SaveChanges();

            return null;
        }

    }
}