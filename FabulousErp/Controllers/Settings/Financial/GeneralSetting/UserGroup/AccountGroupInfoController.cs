using FabulousDB.DB_Context;
using FabulousDB.DB_Tabels.Settings.Financial.GeneralSetting.UserAccount;
using FabulousDB.DB_Tabels.Settings.Financial.GeneralSetting.UserGroup;
using FabulousErp.MyRoleProvider;
using FabulousErp.Repository.Interface;
using FabulousModels.DTOModels.Settings.Financial.GeneralSettings.UserGroup;
using FabulousModels.ViewModels.Settings.Financial.GeneralSetting.UserGroup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FabulousErp.Controllers.Settings.Financial.GeneralSetting.UserGroup
{
    [BranchSecurityFilter]
    [FactorySecurityFilter]
    [AuthorizationFilter]
    public class AccountGroupInfoController : Controller
    {

        IRepetitionBusiness repetitionBusiness;

        public AccountGroupInfoController(IRepetitionBusiness repetitionBusiness)
        {
            this.repetitionBusiness = repetitionBusiness;
        }

        DBContext DB = new DBContext();

        // GET: AccountGroupInfo
        [HttpGet]
       // [Authorize]
        [CustomAuthorize(Roles = "SAGI")]
        public ActionResult AddUserToGroup()
        {
            ViewBag.GroupList = repetitionBusiness.RetrieveGroupIDList();

            ViewBag.UserList = repetitionBusiness.RetrieveUserIDList();

            return View();
        }


        public JsonResult GetGroupInfo(string GroupID)
        {
            var checkGroup = DB.CreateGroup_Tables.Where(x => x.GroupID == GroupID).FirstOrDefault();
            if (checkGroup.DisActive == true)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            else
            {
                UserGroup_DTO userGroup_DTO = new UserGroup_DTO()
                {
                    R_Group_Info_DTO = DB.CreateGroup_Tables.Where(x => x.GroupID == GroupID).Select(x => new R_Group_Info_DTO
                    {
                        GroupName = x.GroupName,

                        CreationGroupDate = x.Date,

                        FromCBF = x.FromCBF
                    }).FirstOrDefault(),

                    UserInfos = DB.UserGroup_Tables.Where(x => x.GroupID == GroupID).Select(x => new UserInfo
                    {

                        UserID = x.UserID,

                        UserName = x.CreateAccount_Table.UserName,

                        Date = x.Date

                    }).ToList()
                };

                return Json(userGroup_DTO, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetUserName(string UserID, int FromCBF)
        {
            var checkUserAccessToComp = DB.UACompPremission_Tables.Where(x => x.UserID == UserID).FirstOrDefault();

            var checkUserAccessToBranch = DB.UABranchPremission_Tables.Where(x => x.UserID == UserID).FirstOrDefault();

            var checkUserAccessToFactory = DB.UAFactoryPremission_Tables.Where(x => x.UserID == UserID).FirstOrDefault();

            if (FromCBF == 1 && checkUserAccessToComp == null)
            {
                return Json("AFalse", JsonRequestBehavior.AllowGet);
            }
            else if (FromCBF == 2 && checkUserAccessToBranch == null)
            {
                return Json("BFalse", JsonRequestBehavior.AllowGet);
            }
            else if (FromCBF == 3 && checkUserAccessToFactory == null)
            {
                return Json("CFalse", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(repetitionBusiness.GetUserName(UserID), JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult AddUserToGroupWithCheck(string UserID, string GroupID, int FromCBF)
        {
            var checkUserInAnotherGroup = DB.UserGroup_Tables.Where(x => x.UserID == UserID).FirstOrDefault();

            if (checkUserInAnotherGroup != null)
            {
                R_Group_Info_DTO r_Group_Info_DTO = new R_Group_Info_DTO()
                {
                    GroupID = checkUserInAnotherGroup.GroupID,

                    GetGName = checkUserInAnotherGroup.CreateGroup_Table.GroupName
                };

                return Json(r_Group_Info_DTO, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(InsertUserToGroup(UserID, GroupID, FromCBF), JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult AddUserToGroupWithoutCheck(string UserID, string GroupID, int FromCBF)
        {
            DB.UserGroup_Tables.Remove(DB.UserGroup_Tables.Where(x => x.UserID == UserID).FirstOrDefault());

            return Json(InsertUserToGroup(UserID, GroupID, FromCBF), JsonRequestBehavior.AllowGet);
        }

        private bool InsertUserToGroup(string UserID, string GroupID, int FromCBF)
        {
            UserGroup_Table userGroup_Table = new UserGroup_Table()
            {
                UserID = UserID,

                GroupID = GroupID,

                Date = DateTime.Now.ToShortDateString()
            };
            DB.UserGroup_Tables.Add(userGroup_Table);

            var getGroupAccess = DB.GroupFormsAccess_Tables.Where(x => x.GroupID == GroupID).ToList();

            DB.UserFormsAccess_Tables.RemoveRange(DB.UserFormsAccess_Tables.Where(x => x.UserID == UserID).ToList());

            foreach (var groupForms in getGroupAccess)
            {
                UserFormsAccess_Table userFormsAccess_Table = new UserFormsAccess_Table()
                {
                    UserID = UserID,

                    FormCode = groupForms.FormCode,

                    Type = FromCBF
                };
                DB.UserFormsAccess_Tables.Add(userFormsAccess_Table);
            }

            DB.SaveChanges();

            return true;
        }

        public JsonResult GetGroupContent(string GroupID)
        {
            List<UserInfo> userInfo = DB.UserGroup_Tables.Where(x => x.GroupID == GroupID).Select(x => new UserInfo
            {

                UserID = x.UserID,

                UserName = x.CreateAccount_Table.UserName,

                Date = x.Date

            }).ToList();

            return Json(userInfo, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteFromGroup(string UserID, int FromCBF)
        {
            DB.UserGroup_Tables.Remove(DB.UserGroup_Tables.FirstOrDefault(x => x.UserID == UserID));

            DB.UserFormsAccess_Tables.RemoveRange(DB.UserFormsAccess_Tables.Where(x => x.UserID == UserID && x.Type == FromCBF).ToList());

            DB.SaveChanges();

            return Json(true, JsonRequestBehavior.AllowGet);
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
        //            item.SAGI = Value;
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
        //        if (item.SAGI.ToString().Equals("True"))
        //        {
        //            return Json("True", JsonRequestBehavior.AllowGet);
        //        }
        //        else
        //        {
        //            return Json("False", JsonRequestBehavior.AllowGet);
        //        }
        //    }
        //}






        //public JsonResult GetGroupInfo(string GroupID)
        //{

        //    var GetInfo = DB.CreateGroup_Tables.Where(x => x.GroupID == GroupID).FirstOrDefault();

        //    R_Group_Info_DTO r_Group_Info_DTO = new R_Group_Info_DTO()
        //    {
        //        GroupName = GetInfo.GroupName,

        //        CreationGroupDate = GetInfo.Date,

        //        FromCBF = GetInfo.FromCBF
        //    };

        //    return Json(r_Group_Info_DTO, JsonRequestBehavior.AllowGet);
        //}

        //public JsonResult GetGroupContent(string GroupID)
        //{

        //    List<CreateAccount_Table> createAccount_Table = new List<CreateAccount_Table>();

        //    createAccount_Table = DB.CreateAccount_Tables.Where(x => x.GroupID == GroupID && (x.Deleted == false || x.Deleted == null)).ToList();


        //    return Json(createAccount_Table, JsonRequestBehavior.AllowGet);
        //}

        //public JsonResult CheckIfUserInAnotherGroup(string UserID, string GroupID, string FromCBF)
        //{

        //    var Check = DB.CreateAccount_Tables.Where(x => x.UserID == UserID).FirstOrDefault();

        //    if (Check.GroupID.Length > 0 && Check.GroupID != GroupID && Check.GroupID != "EmptyGroup")
        //    {

        //        var GetGName = DB.CreateGroup_Tables.Where(x => x.GroupID == Check.GroupID).FirstOrDefault();

        //        R_Group_Info_DTO r_Group_Info_DTO = new R_Group_Info_DTO()
        //        {
        //            Message = "Exist",

        //            GroupID = Check.GroupID,

        //            GetGName = GetGName.GroupName
        //        };

        //        return Json(r_Group_Info_DTO, JsonRequestBehavior.AllowGet);
        //    }
        //    else
        //    {
        //        var checkUserAccessToComp = DB.UACompPremission_Tables.Where(x => x.UserID == UserID).FirstOrDefault();

        //        var checkUserAccessToBranch = DB.UABranchPremission_Tables.Where(x => x.UserID == UserID).FirstOrDefault();

        //        var checkUserAccessToFactory = DB.UAFactoryPremission_Tables.Where(x => x.UserID == UserID).FirstOrDefault();

        //        if (FromCBF == "A" && checkUserAccessToComp == null)
        //        {
        //            return Json("AFalse", JsonRequestBehavior.AllowGet);
        //        }
        //        else if (FromCBF == "B" && checkUserAccessToBranch == null)
        //        {
        //            return Json("BFalse", JsonRequestBehavior.AllowGet);
        //        }
        //        else if (FromCBF == "C" && checkUserAccessToFactory == null)
        //        {
        //            return Json("CFalse", JsonRequestBehavior.AllowGet);
        //        }
        //        else
        //        {
        //            //var ID = Check.ID;

        //            var UpdateUser = DB.CreateAccount_Tables.Where(x => x.UserID == UserID).FirstOrDefault();

        //            UpdateUser.GroupID = GroupID;

        //            UpdateUser.DateOfAssignGroup = DateTime.Now.ToShortDateString();

        //            //forms
        //            //25
        //            //var GetFormAccess = DB.CreateGroup_Tables.Where(x => x.GroupID == GroupID).FirstOrDefault();

        //            //string SCL = GetFormAccess.SCL;
        //            //string SCBI = GetFormAccess.SCBI;
        //            //string SCPI = GetFormAccess.SCPI;
        //            //string SCNA = GetFormAccess.SCNA;
        //            //string SLOU = GetFormAccess.SLOU;
        //            //string SCNG = GetFormAccess.SCNG;
        //            //string SLOG = GetFormAccess.SLOG;
        //            //string SAGI = GetFormAccess.SAGI;
        //            //string SUACP = GetFormAccess.SUACP;
        //            //string SUABP = GetFormAccess.SUABP;
        //            //string SUAFP = GetFormAccess.SUAFP;
        //            //string SUAP = GetFormAccess.SUAP;
        //            //string SFYD = GetFormAccess.SFYD;
        //            //string SCFY = GetFormAccess.SCFY;
        //            //string SCNY = GetFormAccess.SCNY;
        //            //string SCF = GetFormAccess.SCF;
        //            //string SCD = GetFormAccess.SCD;
        //            //string SCET = GetFormAccess.SCET;
        //            //string ILOU = GetFormAccess.ILOU;
        //            //string SCCOA = GetFormAccess.SCCOA;
        //            //string ICI = GetFormAccess.ICI;
        //            //string IUP = GetFormAccess.IUP;
        //            //string ISFP = GetFormAccess.ISFP;
        //            //string SACTC = GetFormAccess.SACTC;
        //            //string ILCOA = GetFormAccess.ILCOA;
        //            //string SCAA = GetFormAccess.SCAA;
        //            //string SCAAD = GetFormAccess.SCAAD;
        //            //string SBAA = GetFormAccess.SBAA;
        //            //string SFAA = GetFormAccess.SFAA;
        //            //string SCCC = GetFormAccess.SCCC;
        //            //string SBCC = GetFormAccess.SBCC;
        //            //string SFCC = GetFormAccess.SFCC;
        //            //string SCMCC = GetFormAccess.SCMCC;
        //            //string SUMCC = GetFormAccess.SUMCC;
        //            //string SCCCA = GetFormAccess.SCCCA;
        //            //string SCAG = GetFormAccess.SCAG;
        //            //string SCCA = GetFormAccess.SCCA;
        //            //string SCY = GetFormAccess.SCY;
        //            //string SBAAD = GetFormAccess.SBAAD;
        //            //string SFAAD = GetFormAccess.SFAAD;
        //            //string SBMCC = GetFormAccess.SBMCC;
        //            //string SFMCC = GetFormAccess.SFMCC;
        //            //string SUBMCC = GetFormAccess.SUBMCC;
        //            //string SUFMCC = GetFormAccess.SUFMCC;
        //            //string SBCCA = GetFormAccess.SBCCA;
        //            //string SFCCA = GetFormAccess.SFCCA;
        //            //string SBUAP = GetFormAccess.SBUAP;
        //            //string SFUAP = GetFormAccess.SFUAP;
        //            //string IUFA = GetFormAccess.IUFA;
        //            //string IBA = GetFormAccess.IBA;
        //            //string ICA = GetFormAccess.ICA;
        //            //string IFA = GetFormAccess.IFA;
        //            //string IGA = GetFormAccess.IGA;
        //            //string SDATCA = GetFormAccess.SDATCA;
        //            //string SCATCA = GetFormAccess.SCATCA;
        //            //string IBAA = GetFormAccess.IBAA;
        //            //string IBCC = GetFormAccess.IBCC;
        //            //string ICAA = GetFormAccess.ICAA;
        //            //string ICCC = GetFormAccess.ICCC;
        //            //string ILOC = GetFormAccess.ILOC;
        //            //string IFAA = GetFormAccess.IFAA;
        //            //string IFCC = GetFormAccess.IFCC;
        //            //string SBCA = GetFormAccess.SBCA;
        //            //string SDATBA = GetFormAccess.SDATBA;
        //            //string SCATBA = GetFormAccess.SCATBA;
        //            //string SFCA = GetFormAccess.SFCA;
        //            //string SDATFA = GetFormAccess.SDATFA;
        //            //string SCATFA = GetFormAccess.SCATFA;
        //            //string SOCP = GetFormAccess.SOCP;
        //            //string SUP = GetFormAccess.SUP;
        //            //string SPS = GetFormAccess.SPS;
        //            //string IBADA = GetFormAccess.IBADA;
        //            //string IBCCA = GetFormAccess.IBCCA;
        //            //string ICADA = GetFormAccess.ICADA;
        //            //string SPD = GetFormAccess.SPD;
        //            //string ICCCA = GetFormAccess.ICCCA;
        //            //string IFADA = GetFormAccess.IFADA;
        //            //string IFCCA = GetFormAccess.IFCCA;
        //            //string TCCB = GetFormAccess.TCCB;
        //            //string TBCB = GetFormAccess.TBCB;
        //            //string TFCB = GetFormAccess.TFCB;
        //            //string SUETR = GetFormAccess.SUETR;

        //            //-----------------------------------------------------------------------

        //            /*
        //            var GetIDForms = DB.AccountGroup_Tables.Where(x => x.UserID == UserID).FirstOrDefault();
        //            var IDForms = GetIDForms.ID;
        //            */
        //            //20
        //            //var UpdateUserInForms = DB.AccountGroup_Tables.Where(x => x.UserID == UserID).First();
        //            //UpdateUserInForms.GroupID = GroupID;

        //            //UpdateUserInForms.SCL = SCL;
        //            //UpdateUserInForms.SCBI = SCBI;
        //            //UpdateUserInForms.SCPI = SCPI;
        //            //UpdateUserInForms.SCNA = SCNA;
        //            //UpdateUserInForms.SLOU = SLOU;
        //            //UpdateUserInForms.SCNG = SCNG;
        //            //UpdateUserInForms.SLOG = SLOG;
        //            //UpdateUserInForms.SAGI = SAGI;
        //            //UpdateUserInForms.SUACP = SUACP;
        //            //UpdateUserInForms.SUABP = SUABP;
        //            //UpdateUserInForms.SUAFP = SUAFP;
        //            //UpdateUserInForms.SUAP = SUAP;
        //            //UpdateUserInForms.SFYD = SFYD;
        //            //UpdateUserInForms.SCFY = SCFY;
        //            //UpdateUserInForms.SCNY = SCNY;
        //            //UpdateUserInForms.SCF = SCF;
        //            //UpdateUserInForms.SCD = SCD;
        //            //UpdateUserInForms.SCET = SCET;
        //            //UpdateUserInForms.ILOU = ILOU;
        //            //UpdateUserInForms.SCCOA = SCCOA;
        //            //UpdateUserInForms.ICI = ICI;
        //            //UpdateUserInForms.IUP = IUP;
        //            //UpdateUserInForms.ISFP = ISFP;
        //            //UpdateUserInForms.SACTC = SACTC;
        //            //UpdateUserInForms.ILCOA = ILCOA;
        //            //UpdateUserInForms.SCAA = SCAA;
        //            //UpdateUserInForms.SCAAD = SCAAD;
        //            //UpdateUserInForms.SBAA = SBAA;
        //            //UpdateUserInForms.SFAA = SFAA;
        //            //UpdateUserInForms.SCCC = SCCC;
        //            //UpdateUserInForms.SBCC = SBCC;
        //            //UpdateUserInForms.SFCC = SFCC;
        //            //UpdateUserInForms.SCMCC = SCMCC;
        //            //UpdateUserInForms.SUMCC = SUMCC;
        //            //UpdateUserInForms.SCCCA = SCCCA;
        //            //UpdateUserInForms.SCAG = SCAG;
        //            //UpdateUserInForms.SCCA = SCCA;
        //            //UpdateUserInForms.SCY = SCY;
        //            //UpdateUserInForms.SBAAD = SBAAD;
        //            //UpdateUserInForms.SFAAD = SFAAD;
        //            //UpdateUserInForms.SBMCC = SBMCC;
        //            //UpdateUserInForms.SFMCC = SFMCC;
        //            //UpdateUserInForms.SUBMCC = SUBMCC;
        //            //UpdateUserInForms.SUFMCC = SUFMCC;
        //            //UpdateUserInForms.SBCCA = SBCCA;
        //            //UpdateUserInForms.SFCCA = SFCCA;
        //            //UpdateUserInForms.SBUAP = SBUAP;
        //            //UpdateUserInForms.SFUAP = SFUAP;
        //            //UpdateUserInForms.IUFA = IUFA;
        //            //UpdateUserInForms.IBA = IBA;
        //            //UpdateUserInForms.ICA = ICA;
        //            //UpdateUserInForms.IFA = IFA;
        //            //UpdateUserInForms.IGA = IGA;
        //            //UpdateUserInForms.SDATCA = SDATCA;
        //            //UpdateUserInForms.SCATCA = SCATCA;
        //            //UpdateUserInForms.IBAA = IBAA;
        //            //UpdateUserInForms.IBCC = IBCC;
        //            //UpdateUserInForms.ICAA = ICAA;
        //            //UpdateUserInForms.ICCC = ICCC;
        //            //UpdateUserInForms.ILOC = ILOC;
        //            //UpdateUserInForms.IFAA = IFAA;
        //            //UpdateUserInForms.IFCC = IFCC;
        //            //UpdateUserInForms.SBCA = SBCA;
        //            //UpdateUserInForms.SDATBA = SDATBA;
        //            //UpdateUserInForms.SCATBA = SCATBA;
        //            //UpdateUserInForms.SFCA = SFCA;
        //            //UpdateUserInForms.SDATFA = SDATFA;
        //            //UpdateUserInForms.SCATFA = SCATFA;
        //            //UpdateUserInForms.SOCP = SOCP;
        //            //UpdateUserInForms.SUP = SUP;
        //            //UpdateUserInForms.SPS = SPS;
        //            //UpdateUserInForms.IBADA = IBADA;
        //            //UpdateUserInForms.IBCCA = IBCCA;
        //            //UpdateUserInForms.ICADA = ICADA;
        //            //UpdateUserInForms.SPD = SPD;
        //            //UpdateUserInForms.ICCCA = ICCCA;
        //            //UpdateUserInForms.IFADA = IFADA;
        //            //UpdateUserInForms.IFCCA = IFCCA;
        //            //UpdateUserInForms.TCCB = TCCB;
        //            //UpdateUserInForms.TBCB = TBCB;
        //            //UpdateUserInForms.TFCB = TFCB;
        //            //UpdateUserInForms.SUETR = SUETR;

        //            //DB.SaveChanges();
        //            return null;
        //        }
        //    }
        //}

        //public JsonResult AssignUserWithoutCheck(string UserID, string GroupID)
        //{
        //    /*
        //    var Check = DB.createAccount_Tables.Where(x => x.UserID == UserID).FirstOrDefault();

        //    var ID = Check.ID;
        //    */
        //    var UpdateUser = DB.CreateAccount_Tables.Where(x => x.UserID == UserID).First();

        //    UpdateUser.GroupID = GroupID;

        //    UpdateUser.DateOfAssignGroup = DateTime.Now.ToShortDateString();

        //    //forms
        //    //26
        //    //var GetFormAccess = DB.CreateGroup_Tables.Where(x => x.GroupID == GroupID).FirstOrDefault();

        //    //string SCL = GetFormAccess.SCL;
        //    //string SCBI = GetFormAccess.SCBI;
        //    //string SCPI = GetFormAccess.SCPI;
        //    //string SCNA = GetFormAccess.SCNA;
        //    //string SLOU = GetFormAccess.SLOU;
        //    //string SCNG = GetFormAccess.SCNG;
        //    //string SLOG = GetFormAccess.SLOG;
        //    //string SAGI = GetFormAccess.SAGI;
        //    //string SUACP = GetFormAccess.SUACP;
        //    //string SUABP = GetFormAccess.SUABP;
        //    //string SUAFP = GetFormAccess.SUAFP;
        //    //string SUAP = GetFormAccess.SUAP;
        //    //string SFYD = GetFormAccess.SFYD;
        //    //string SCFY = GetFormAccess.SCFY;
        //    //string SCNY = GetFormAccess.SCNY;
        //    //string SCF = GetFormAccess.SCF;
        //    //string SCD = GetFormAccess.SCD;
        //    //string SCET = GetFormAccess.SCET;
        //    //string ILOU = GetFormAccess.ILOU;
        //    //string SCCOA = GetFormAccess.SCCOA;
        //    //string ICI = GetFormAccess.ICI;
        //    //string IUP = GetFormAccess.IUP;
        //    //string ISFP = GetFormAccess.ISFP;
        //    //string SACTC = GetFormAccess.SACTC;
        //    //string ILCOA = GetFormAccess.ILCOA;
        //    //string SCAA = GetFormAccess.SCAA;
        //    //string SCAAD = GetFormAccess.SCAAD;
        //    //string SBAA = GetFormAccess.SBAA;
        //    //string SFAA = GetFormAccess.SFAA;
        //    //string SCCC = GetFormAccess.SCCC;
        //    //string SBCC = GetFormAccess.SBCC;
        //    //string SFCC = GetFormAccess.SFCC;
        //    //string SCMCC = GetFormAccess.SCMCC;
        //    //string SUMCC = GetFormAccess.SUMCC;
        //    //string SCCCA = GetFormAccess.SCCCA;
        //    //string SCAG = GetFormAccess.SCAG;
        //    //string SCCA = GetFormAccess.SCCA;
        //    //string SCY = GetFormAccess.SCY;
        //    //string SBAAD = GetFormAccess.SBAAD;
        //    //string SFAAD = GetFormAccess.SFAAD;
        //    //string SBMCC = GetFormAccess.SBMCC;
        //    //string SFMCC = GetFormAccess.SFMCC;
        //    //string SUBMCC = GetFormAccess.SUBMCC;
        //    //string SUFMCC = GetFormAccess.SUFMCC;
        //    //string SBCCA = GetFormAccess.SBCCA;
        //    //string SFCCA = GetFormAccess.SFCCA;
        //    //string SBUAP = GetFormAccess.SBUAP;
        //    //string SFUAP = GetFormAccess.SFUAP;
        //    //string IUFA = GetFormAccess.IUFA;
        //    //string IBA = GetFormAccess.IBA;
        //    //string ICA = GetFormAccess.ICA;
        //    //string IFA = GetFormAccess.IFA;
        //    //string IGA = GetFormAccess.IGA;
        //    //string SDATCA = GetFormAccess.SDATCA;
        //    //string SCATCA = GetFormAccess.SCATCA;
        //    //string IBAA = GetFormAccess.IBAA;
        //    //string IBCC = GetFormAccess.IBCC;
        //    //string ICAA = GetFormAccess.ICAA;
        //    //string ICCC = GetFormAccess.ICCC;
        //    //string ILOC = GetFormAccess.ILOC;
        //    //string IFAA = GetFormAccess.IFAA;
        //    //string IFCC = GetFormAccess.IFCC;
        //    //string SBCA = GetFormAccess.SBCA;
        //    //string SDATBA = GetFormAccess.SDATBA;
        //    //string SCATBA = GetFormAccess.SCATBA;
        //    //string SFCA = GetFormAccess.SFCA;
        //    //string SDATFA = GetFormAccess.SDATFA;
        //    //string SCATFA = GetFormAccess.SCATFA;
        //    //string SOCP = GetFormAccess.SOCP;
        //    //string SUP = GetFormAccess.SUP;
        //    //string SPS = GetFormAccess.SPS;
        //    //string IBADA = GetFormAccess.IBADA;
        //    //string IBCCA = GetFormAccess.IBCCA;
        //    //string ICADA = GetFormAccess.ICADA;
        //    //string SPD = GetFormAccess.SPD;
        //    //string ICCCA = GetFormAccess.ICCCA;
        //    //string IFADA = GetFormAccess.IFADA;
        //    //string IFCCA = GetFormAccess.IFCCA;
        //    //string TCCB = GetFormAccess.TCCB;
        //    //string TBCB = GetFormAccess.TBCB;
        //    //string TFCB = GetFormAccess.TFCB;
        //    //string SUETR = GetFormAccess.SUETR;

        //    //-----------------------------------------------------------------------

        //    /*
        //    var GetIDForms = DB.AccountGroup_Tables.Where(x => x.UserID == UserID).FirstOrDefault();
        //    var IDForms = GetIDForms.ID;
        //    */

        //    //21
        //    //var UpdateUserInForms = DB.AccountGroup_Tables.Where(x => x.UserID == UserID).First();
        //    //UpdateUserInForms.GroupID = GroupID;

        //    //UpdateUserInForms.SCL = SCL;
        //    //UpdateUserInForms.SCBI = SCBI;
        //    //UpdateUserInForms.SCPI = SCPI;
        //    //UpdateUserInForms.SCNA = SCNA;
        //    //UpdateUserInForms.SLOU = SLOU;
        //    //UpdateUserInForms.SCNG = SCNG;
        //    //UpdateUserInForms.SLOG = SLOG;
        //    //UpdateUserInForms.SAGI = SAGI;
        //    //UpdateUserInForms.SUACP = SUACP;
        //    //UpdateUserInForms.SUABP = SUABP;
        //    //UpdateUserInForms.SUAFP = SUAFP;
        //    //UpdateUserInForms.SUAP = SUAP;
        //    //UpdateUserInForms.SFYD = SFYD;
        //    //UpdateUserInForms.SCFY = SCFY;
        //    //UpdateUserInForms.SCNY = SCNY;
        //    //UpdateUserInForms.SCF = SCF;
        //    //UpdateUserInForms.SCD = SCD;
        //    //UpdateUserInForms.SCET = SCET;
        //    //UpdateUserInForms.ILOU = ILOU;
        //    //UpdateUserInForms.SCCOA = SCCOA;
        //    //UpdateUserInForms.ICI = ICI;
        //    //UpdateUserInForms.IUP = IUP;
        //    //UpdateUserInForms.ISFP = ISFP;
        //    //UpdateUserInForms.SACTC = SACTC;
        //    //UpdateUserInForms.ILCOA = ILCOA;
        //    //UpdateUserInForms.SCAA = SCAA;
        //    //UpdateUserInForms.SCAAD = SCAAD;
        //    //UpdateUserInForms.SBAA = SBAA;
        //    //UpdateUserInForms.SFAA = SFAA;
        //    //UpdateUserInForms.SCCC = SCCC;
        //    //UpdateUserInForms.SBCC = SBCC;
        //    //UpdateUserInForms.SFCC = SFCC;
        //    //UpdateUserInForms.SCMCC = SCMCC;
        //    //UpdateUserInForms.SUMCC = SUMCC;
        //    //UpdateUserInForms.SCCCA = SCCCA;
        //    //UpdateUserInForms.SCAG = SCAG;
        //    //UpdateUserInForms.SCCA = SCCA;
        //    //UpdateUserInForms.SCY = SCY;
        //    //UpdateUserInForms.SBAAD = SBAAD;
        //    //UpdateUserInForms.SFAAD = SFAAD;
        //    //UpdateUserInForms.SBMCC = SBMCC;
        //    //UpdateUserInForms.SFMCC = SFMCC;
        //    //UpdateUserInForms.SUBMCC = SUBMCC;
        //    //UpdateUserInForms.SUFMCC = SUFMCC;
        //    //UpdateUserInForms.SBCCA = SBCCA;
        //    //UpdateUserInForms.SFCCA = SFCCA;
        //    //UpdateUserInForms.SBUAP = SBUAP;
        //    //UpdateUserInForms.SFUAP = SFUAP;
        //    //UpdateUserInForms.IUFA = IUFA;
        //    //UpdateUserInForms.IBA = IBA;
        //    //UpdateUserInForms.ICA = ICA;
        //    //UpdateUserInForms.IFA = IFA;
        //    //UpdateUserInForms.IGA = IGA;
        //    //UpdateUserInForms.SDATCA = SDATCA;
        //    //UpdateUserInForms.SCATCA = SCATCA;
        //    //UpdateUserInForms.IBAA = IBAA;
        //    //UpdateUserInForms.IBCC = IBCC;
        //    //UpdateUserInForms.ICAA = ICAA;
        //    //UpdateUserInForms.ICCC = ICCC;
        //    //UpdateUserInForms.ILOC = ILOC;
        //    //UpdateUserInForms.IFAA = IFAA;
        //    //UpdateUserInForms.IFCC = IFCC;
        //    //UpdateUserInForms.SBCA = SBCA;
        //    //UpdateUserInForms.SDATBA = SDATBA;
        //    //UpdateUserInForms.SCATBA = SCATBA;
        //    //UpdateUserInForms.SFCA = SFCA;
        //    //UpdateUserInForms.SDATFA = SDATFA;
        //    //UpdateUserInForms.SCATFA = SCATFA;
        //    //UpdateUserInForms.SOCP = SOCP;
        //    //UpdateUserInForms.SUP = SUP;
        //    //UpdateUserInForms.SPS = SPS;
        //    //UpdateUserInForms.IBADA = IBADA;
        //    //UpdateUserInForms.IBCCA = IBCCA;
        //    //UpdateUserInForms.ICADA = ICADA;
        //    //UpdateUserInForms.SPD = SPD;
        //    //UpdateUserInForms.ICCCA = ICCCA;
        //    //UpdateUserInForms.IFADA = IFADA;
        //    //UpdateUserInForms.IFCCA = IFCCA;
        //    //UpdateUserInForms.TCCB = TCCB;
        //    //UpdateUserInForms.TBCB = TBCB;
        //    //UpdateUserInForms.TFCB = TFCB;
        //    //UpdateUserInForms.SUETR = SUETR;

        //    DB.SaveChanges();

        //    return Json(JsonRequestBehavior.AllowGet);
        //}

        //public JsonResult DeleteFromGroup(string UserID)
        //{


        //    var UpdateCreateAccount = DB.CreateAccount_Tables.Where(x => x.UserID == UserID).FirstOrDefault();

        //    UpdateCreateAccount.GroupID = "EmptyGroup";
        //    UpdateCreateAccount.DateOfAssignGroup = "";

        //    //22
        //    //var UpdateAccountGroup = DB.AccountGroup_Tables.Where(x => x.UserID == UserID).FirstOrDefault();

        //    //UpdateAccountGroup.GroupID = "EmptyGroup";
        //    //UpdateAccountGroup.SCL = "";
        //    //UpdateAccountGroup.SCBI = "";
        //    //UpdateAccountGroup.SCPI = "";
        //    //UpdateAccountGroup.SCNA = "";
        //    //UpdateAccountGroup.SLOU = "";
        //    //UpdateAccountGroup.SCNG = "";
        //    //UpdateAccountGroup.SLOG = "";
        //    //UpdateAccountGroup.SAGI = "";
        //    //UpdateAccountGroup.SUACP = "";
        //    //UpdateAccountGroup.SUABP = "";
        //    //UpdateAccountGroup.SUAFP = "";
        //    //UpdateAccountGroup.SUAP = "";
        //    //UpdateAccountGroup.SFYD = "";
        //    //UpdateAccountGroup.SCFY = "";
        //    //UpdateAccountGroup.SCNY = "";
        //    //UpdateAccountGroup.SCF = "";
        //    //UpdateAccountGroup.SCD = "";
        //    //UpdateAccountGroup.SCET = "";
        //    //UpdateAccountGroup.ILOU = "";
        //    //UpdateAccountGroup.SCCOA = "";
        //    //UpdateAccountGroup.ICI = "";
        //    //UpdateAccountGroup.IUP = "";
        //    //UpdateAccountGroup.ISFP = "";
        //    //UpdateAccountGroup.SACTC = "";
        //    //UpdateAccountGroup.ILCOA = "";
        //    //UpdateAccountGroup.SCAA = "";
        //    //UpdateAccountGroup.SCAAD = "";
        //    //UpdateAccountGroup.SBAA = "";
        //    //UpdateAccountGroup.SFAA = "";
        //    //UpdateAccountGroup.SCCC = "";
        //    //UpdateAccountGroup.SBCC = "";
        //    //UpdateAccountGroup.SFCC = "";
        //    //UpdateAccountGroup.SCMCC = "";
        //    //UpdateAccountGroup.SUMCC = "";
        //    //UpdateAccountGroup.SCCCA = "";
        //    //UpdateAccountGroup.SCAG = "";
        //    //UpdateAccountGroup.SCCA = "";
        //    //UpdateAccountGroup.SCY = "";
        //    //UpdateAccountGroup.SBAAD = "";
        //    //UpdateAccountGroup.SFAAD = "";
        //    //UpdateAccountGroup.SBMCC = "";
        //    //UpdateAccountGroup.SFMCC = "";
        //    //UpdateAccountGroup.SUBMCC = "";
        //    //UpdateAccountGroup.SUFMCC = "";
        //    //UpdateAccountGroup.SBCCA = "";
        //    //UpdateAccountGroup.SFCCA = "";
        //    //UpdateAccountGroup.SBUAP = "";
        //    //UpdateAccountGroup.SFUAP = "";
        //    //UpdateAccountGroup.IUFA = "";
        //    //UpdateAccountGroup.IBA = "";
        //    //UpdateAccountGroup.ICA = "";
        //    //UpdateAccountGroup.IFA = "";
        //    //UpdateAccountGroup.IGA = "";
        //    //UpdateAccountGroup.SDATCA = "";
        //    //UpdateAccountGroup.SCATCA = "";
        //    //UpdateAccountGroup.IBAA = "";
        //    //UpdateAccountGroup.IBCC = "";
        //    //UpdateAccountGroup.ICAA = "";
        //    //UpdateAccountGroup.ICCC = "";
        //    //UpdateAccountGroup.ILOC = "";
        //    //UpdateAccountGroup.IFAA = "";
        //    //UpdateAccountGroup.IFCC = "";
        //    //UpdateAccountGroup.SBCA = "";
        //    //UpdateAccountGroup.SDATBA = "";
        //    //UpdateAccountGroup.SCATBA = "";
        //    //UpdateAccountGroup.SFCA = "";
        //    //UpdateAccountGroup.SDATFA = "";
        //    //UpdateAccountGroup.SCATFA = "";
        //    //UpdateAccountGroup.SOCP = "";
        //    //UpdateAccountGroup.SUP = "";
        //    //UpdateAccountGroup.SPS = "";
        //    //UpdateAccountGroup.IBADA = "";
        //    //UpdateAccountGroup.IBCCA = "";
        //    //UpdateAccountGroup.ICADA = "";
        //    //UpdateAccountGroup.SPD = "";
        //    //UpdateAccountGroup.ICCCA = "";
        //    //UpdateAccountGroup.IFADA = "";
        //    //UpdateAccountGroup.IFCCA = "";
        //    //UpdateAccountGroup.TCCB = "";
        //    //UpdateAccountGroup.TBCB = "";
        //    //UpdateAccountGroup.TFCB = "";
        //    //UpdateAccountGroup.SUETR = "";

        //    DB.SaveChanges();

        //    return Json("True", JsonRequestBehavior.AllowGet);
        //}

        //public JsonResult FilterUserID(string GroupID)
        //{
        //    var UserList = DB.CreateAccount_Tables.Where(x => x.GroupID != GroupID && (x.Deleted == false || x.Deleted == null)).ToList();

        //    return Json(UserList, JsonRequestBehavior.AllowGet);
        //}

        //public JsonResult GetUserName(string UserID)
        //{
        //    return Json(repetitionBusiness.GetUserName(UserID), JsonRequestBehavior.AllowGet);
        //}

        //public JsonResult CheckGroupActive(string GroupID)
        //{
        //    var check = DB.CreateGroup_Tables.Where(x => x.GroupID == GroupID).FirstOrDefault();

        //    if (check != null)
        //    {
        //        if (check.DisActive == true)
        //        {
        //            return Json("True", JsonRequestBehavior.AllowGet);
        //        }
        //        else
        //        {
        //            return Json("False", JsonRequestBehavior.AllowGet);
        //        }
        //    }
        //    else
        //    {
        //        return Json(JsonRequestBehavior.AllowGet);
        //    }
        //}


    }
}