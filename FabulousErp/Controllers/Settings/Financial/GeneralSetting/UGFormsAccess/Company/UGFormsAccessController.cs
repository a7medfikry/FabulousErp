using FabulousDB.DB_Context;
using FabulousDB.DB_Tabels.Settings.Financial.GeneralSetting.UserGroup;
using FabulousErp.MyRoleProvider;
using FabulousErp.Repository.Interface;
using FabulousModels.DTOModels.Settings.Financial.GeneralSettings.UGFormAccess;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FabulousErp.Controllers.Settings.Financial.GeneralSetting.UGFormsAccess.Company
{
    [BranchSecurityFilter]
    [FactorySecurityFilter]
    [AuthorizationFilter]
    public class UGFormsAccessController : Controller
    {
        IRepetitionBusiness repetitionBusiness;

        public UGFormsAccessController(IRepetitionBusiness repetitionBusiness)
        {
            this.repetitionBusiness = repetitionBusiness;
        }

        DBContext DB = new DBContext();
        [HttpPost]
        public JsonResult AddPage (string Link,string Name,string Sec)
        {
            if (Link.Contains("Edit"))
            {
                Name += " " + "Edit";
            }
            else if (Link.Contains("Details"))
            {
                Name += " " + "Details";
            }
            else if (Link.Contains("Delete"))
            {
                Name += " " + "Delete";
            }
            Link = Business.GetCleanLink(Link);
            if (!DB.Pages.Any(x => x.Link == Link))
            {
                Pages ThisP = new Pages
                {
                    Link = Link,
                    Name = Name,
                    Page_section = Sec
                };
                DB.Pages.Add(ThisP);
                DB.SaveChanges();
                foreach (FabulousDB.DB_Tabels.Settings.Financial.GeneralSetting.UserAccount.CreateAccount_Table
                    U in DB.CreateAccount_Tables.ToList())
                {
                    DB.UsersPageAccess.Add(new UsersPageAccess
                    {
                        Page_id=ThisP.Id,
                        View=true,
                        Update=true,
                        Delete=true,
                        Edit=true,
                        UserID=U.UserID
                    });
                }
                DB.SaveChanges();
            }
            return Json(1);
        }
        // GET: UGFormsAccess
        [HttpGet]
       // [Authorize]
        [CustomAuthorize(Roles = "SUAP")]
        public ActionResult FormsAccess()
        {
            var getUsersInCompany = DB.CreateAccount_Tables.Where(x => x.UACompPremission_Table.Any()).ToList();
            SelectList UserList = new SelectList(getUsersInCompany, "UserID", "UserID");
            ViewBag.UserList = UserList;


            var GroupID = DB.CreateGroup_Tables.Where(a => a.FromCBF == 1 && (a.Deleted == false || a.Deleted == null)).ToList();
            SelectList GroupList = new SelectList(GroupID, "GroupID", "GroupID");

            ViewBag.GroupList = GroupList;

            ViewBag.Type = 1;

            ViewBag.Pages = DB.Pages.ToList();
            return View();
        }

        //public JsonResult GetGroupName(string GroupID)
        //{
        //    return Json(repetitionBusiness.GetGroupName(GroupID), JsonRequestBehavior.AllowGet);
        //}

        public JsonResult GetUserName(string UserID)
        {
            return Json(repetitionBusiness.GetUserName(UserID), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetUserAccessData(string UserID)
        {
            var checkUserCompAccess = DB.UACompPremission_Tables.Where(x => x.UserID == UserID).FirstOrDefault();

            if (checkUserCompAccess != null)
            {
                UserGroupAccess_DTO userGroupAccess_DTO = new UserGroupAccess_DTO()
                {
                    Names = DB.CreateAccount_Tables.Where(x => x.UserID == UserID).Select(x => new Names
                    {
                        Name = x.UserName

                    }).FirstOrDefault(),

                    Forms = DB.UserFormsAccess_Tables.Where(x => x.UserID == UserID).Select(x => new Forms
                    {
                        FormName = x.FormCode

                    }).ToList()
                };
                return Json(userGroupAccess_DTO, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }
        public PartialViewResult GetPagesWithUserAcess (string UserId,string Sec)
        {
            ViewBag.Sec = Sec;
            List<Pages> Pages = DB.Pages.Where(x=>x.Page_section==Sec).ToList();
            ViewBag.UserPermission = DB.UsersPageAccess.ToList().Where(x => x.UserID == UserId&& Pages.Select(z=>z.Id).Contains(x.Page_id)).ToList();
            return PartialView(Pages);
        }
        public JsonResult PostPageAccess(List<UsersPageAccess> Pages)
        {
            bool Change = false;
            foreach (UsersPageAccess P in Pages)
            {
                UsersPageAccess ThisP = DB.UsersPageAccess.FirstOrDefault(x => x.Page_id == P.Page_id && x.UserID == P.UserID);
                if (ThisP == null)
                {
                    DB.UsersPageAccess.Add(P);
                    Change = true;
                }
                else
                {
                    if (ThisP.View != P.View)
                    {
                        ThisP.View = P.View;
                        Change = true;
                    }
                }
            }
            DB.SaveChanges();
            return Json(Change);
        }
        public JsonResult GetGroupAccessData(string groupID)
        {
            var checkGroup = DB.CreateGroup_Tables.Where(x => x.GroupID == groupID).FirstOrDefault();

            if (checkGroup.DisActive == true)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            else
            {
                UserGroupAccess_DTO userGroupAccess_DTO = new UserGroupAccess_DTO()
                {
                    Names = DB.CreateGroup_Tables.Where(x => x.GroupID == groupID).Select(x => new Names
                    {

                        Name = x.GroupName

                    }).FirstOrDefault(),

                    Forms = DB.GroupFormsAccess_Tables.Where(x => x.GroupID == groupID).Select(x => new Forms
                    {

                        FormName = x.FormCode

                    }).ToList()
                };
                return Json(userGroupAccess_DTO, JsonRequestBehavior.AllowGet);
            }
        }


        public JsonResult SaveAccessToGroup(string groupID, int type, List<GroupFormsAccess_Table> groupFormsAccess)
        {
            var dbData = DB.GroupFormsAccess_Tables.Where(x => x.GroupID == groupID && x.Type == type).ToList();

            var getUsersOfGroup = DB.UserGroup_Tables.Where(x => x.GroupID == groupID).ToList();

            if (groupFormsAccess == null)
            {
                DB.GroupFormsAccess_Tables.RemoveRange(dbData);

                if (getUsersOfGroup != null)
                {
                    foreach (var userItem in getUsersOfGroup)
                    {
                        var user = DB.UserFormsAccess_Tables.Where(x => x.UserID == userItem.UserID).ToList();

                        DB.UserFormsAccess_Tables.RemoveRange(user);
                    }
                }
            }
            else
            {
                var checkChange = groupFormsAccess.Select(x => x.FormCode).Except(dbData.Select(x => x.FormCode)).ToList();
                var checkChange2 = dbData.Select(x => x.FormCode).Except(groupFormsAccess.Select(x => x.FormCode)).ToList();

                if (!checkChange.Any() && !checkChange2.Any())
                {
                    return Json(false, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    DB.GroupFormsAccess_Tables.RemoveRange(dbData);

                    DB.GroupFormsAccess_Tables.AddRange(groupFormsAccess);

                    if (getUsersOfGroup != null)
                    {
                        foreach (var userItem in getUsersOfGroup)
                        {
                            var user = DB.UserFormsAccess_Tables.Where(x => x.UserID == userItem.UserID).ToList();

                            DB.UserFormsAccess_Tables.RemoveRange(user);

                            foreach (var groupItem in groupFormsAccess)
                            {
                                UserFormsAccess_Table userFormsAccess_Table = new UserFormsAccess_Table()
                                {
                                    UserID = userItem.UserID,

                                    FormCode = groupItem.FormCode,

                                    FormName = groupItem.FormName,

                                    Type = type
                                };
                                DB.UserFormsAccess_Tables.Add(userFormsAccess_Table);
                            }
                        }
                    }
                }
            }
            DB.SaveChanges();
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        //public JsonResult SaveAccessToGroup(string GroupID, string SCL, string SCBI, string SCPI, string SCNA, string SLOU, string SCNG, string SLOG, string SAGI, string SUACP, string SUAP, string SFYD, string SCFY, string SCNY, string SCF, string SCD, string SCET, string ILOU, string SCCOA, string ICI, string IUP,
        //                                    string ISFP, string SACTC, string ILCOA, string SCAA, string SCAAD, string SCCC, string SCMCC, string SUMCC, string SCCCA, string SCAG, string SCCA, string SCY, string SBUAP, string SFUAP, string IUFA, string IBA, string ICA, string IFA, string IGA, string SDATCA, string SCATCA,
        //                                    string ICAA, string ICCC, string ILOC, string SOCP, string SUP, string SPS, string ICADA, string SPD, string ICCCA, string TCCB, string SUETR, string TCGE, string SUABP, string SUAFP)
        //{
        //    //23
        //    //var GetGroupID = DB.CreateGroup_Tables.Where(x => x.GroupID == GroupID).FirstOrDefault();

        //    //if (GetGroupID.SCL == SCL && GetGroupID.SCBI == SCBI && GetGroupID.SCPI == SCPI && GetGroupID.SCNA == SCNA && GetGroupID.SLOU == SLOU && GetGroupID.SCNG == SCNG && GetGroupID.SLOG == SLOG && GetGroupID.SAGI == SAGI && GetGroupID.SUACP == SUACP && GetGroupID.SUAP == SUAP && GetGroupID.SFYD == SFYD
        //    //    && GetGroupID.SCFY == SCFY && GetGroupID.SCNY == SCNY && GetGroupID.SCF == SCF && GetGroupID.SCD == SCD && GetGroupID.SCET == SCET && GetGroupID.ILOU == ILOU && GetGroupID.SCCOA == SCCOA && GetGroupID.ICI == ICI && GetGroupID.IUP == IUP && GetGroupID.ISFP == ISFP && GetGroupID.SACTC == SACTC && GetGroupID.ILCOA == ILCOA
        //    //    && GetGroupID.SCAA == SCAA && GetGroupID.SCAAD == SCAAD && GetGroupID.SCCC == SCCC && GetGroupID.SCMCC == SCMCC && GetGroupID.SUMCC == SUMCC && GetGroupID.SCCCA == SCCCA && GetGroupID.SCAG == SCAG && GetGroupID.SCCA == SCCA && GetGroupID.SCY == SCY && GetGroupID.SBUAP == SBUAP && GetGroupID.SFUAP == SFUAP
        //    //    && GetGroupID.IUFA == IUFA && GetGroupID.IBA == IBA && GetGroupID.ICA == ICA && GetGroupID.IFA == IFA && GetGroupID.IGA == IGA && GetGroupID.SDATCA == SDATCA && GetGroupID.SCATCA == SCATCA && GetGroupID.ICAA == ICAA && GetGroupID.ICCC == ICCC && GetGroupID.ILOC == ILOC && GetGroupID.SOCP == SOCP && GetGroupID.SUP == SUP
        //    //    && GetGroupID.SPS == SPS && GetGroupID.ICADA == ICADA && GetGroupID.SPD == SPD && GetGroupID.ICCCA == ICCCA && GetGroupID.TCCB == TCCB && GetGroupID.SUETR == SUETR && GetGroupID.TCGE == TCGE && GetGroupID.SUABP == SUABP && GetGroupID.SUAFP == SUAFP)
        //    //{
        //    //    return Json("False", JsonRequestBehavior.AllowGet);
        //    //}
        //    //else
        //    //{
        //    //    //var GID = GetGroupID.ID;
        //    //    var UpdateGroup = DB.CreateGroup_Tables.Where(x => x.GroupID == GroupID).FirstOrDefault();

        //    //    UpdateGroup.SCL = SCL;
        //    //    UpdateGroup.SCBI = SCBI;
        //    //    UpdateGroup.SCPI = SCPI;
        //    //    UpdateGroup.SCNA = SCNA;
        //    //    UpdateGroup.SLOU = SLOU;
        //    //    UpdateGroup.SCNG = SCNG;
        //    //    UpdateGroup.SLOG = SLOG;
        //    //    UpdateGroup.SAGI = SAGI;
        //    //    UpdateGroup.SUACP = SUACP;
        //    //    UpdateGroup.SUAP = SUAP;
        //    //    UpdateGroup.SFYD = SFYD;
        //    //    UpdateGroup.SCFY = SCFY;
        //    //    UpdateGroup.SCNY = SCNY;
        //    //    UpdateGroup.SCF = SCF;
        //    //    UpdateGroup.SCD = SCD;
        //    //    UpdateGroup.SCET = SCET;
        //    //    UpdateGroup.ILOU = ILOU;
        //    //    UpdateGroup.SCCOA = SCCOA;
        //    //    UpdateGroup.ICI = ICI;
        //    //    UpdateGroup.IUP = IUP;
        //    //    UpdateGroup.ISFP = ISFP;
        //    //    UpdateGroup.SACTC = SACTC;
        //    //    UpdateGroup.ILCOA = ILCOA;
        //    //    UpdateGroup.SCAA = SCAA;
        //    //    UpdateGroup.SCAAD = SCAAD;
        //    //    UpdateGroup.SCCC = SCCC;
        //    //    UpdateGroup.SCMCC = SCMCC;
        //    //    UpdateGroup.SUMCC = SUMCC;
        //    //    UpdateGroup.SCCCA = SCCCA;
        //    //    UpdateGroup.SCAG = SCAG;
        //    //    UpdateGroup.SCCA = SCCA;
        //    //    UpdateGroup.SCY = SCY;
        //    //    UpdateGroup.SBUAP = SBUAP;
        //    //    UpdateGroup.SFUAP = SFUAP;
        //    //    UpdateGroup.IUFA = IUFA;
        //    //    UpdateGroup.IBA = IBA;
        //    //    UpdateGroup.ICA = ICA;
        //    //    UpdateGroup.IFA = IFA;
        //    //    UpdateGroup.IGA = IGA;
        //    //    UpdateGroup.SDATCA = SDATCA;
        //    //    UpdateGroup.SCATCA = SCATCA;
        //    //    UpdateGroup.ICAA = ICAA;
        //    //    UpdateGroup.ICCC = ICCC;
        //    //    UpdateGroup.ILOC = ILOC;
        //    //    UpdateGroup.SOCP = SOCP;
        //    //    UpdateGroup.SUP = SUP;
        //    //    UpdateGroup.SPS = SPS;
        //    //    UpdateGroup.ICADA = ICADA;
        //    //    UpdateGroup.SPD = SPD;
        //    //    UpdateGroup.ICCCA = ICCCA;
        //    //    UpdateGroup.TCCB = TCCB;
        //    //    UpdateGroup.SUETR = SUETR;
        //    //    UpdateGroup.TCGE = TCGE;
        //    //    UpdateGroup.SUABP = SUABP;
        //    //    UpdateGroup.SUAFP = SUAFP;


        //    //    DB.SaveChanges();

        //        //12
        //        //var GetUserID = DB.AccountGroup_Tables.Where(x => x.GroupID == GroupID).ToList();

        //        //foreach (var User in GetUserID)
        //        //{
        //        //    //var UID = User.ID;
        //        //    //var UpdateUser = DB.AccountGroup_Tables.Where(x => x.GroupID == GroupID).First();

        //        //    User.SCL = SCL;
        //        //    User.SCBI = SCBI;
        //        //    User.SCPI = SCPI;
        //        //    User.SCNA = SCNA;
        //        //    User.SLOU = SLOU;
        //        //    User.SCNG = SCNG;
        //        //    User.SLOG = SLOG;
        //        //    User.SAGI = SAGI;
        //        //    User.SUACP = SUACP;
        //        //    User.SUAP = SUAP;
        //        //    User.SFYD = SFYD;
        //        //    User.SCFY = SCFY;
        //        //    User.SCNY = SCNY;
        //        //    User.SCF = SCF;
        //        //    User.SCD = SCD;
        //        //    User.SCET = SCET;
        //        //    User.ILOU = ILOU;
        //        //    User.SCCOA = SCCOA;
        //        //    User.ICI = ICI;
        //        //    User.IUP = IUP;
        //        //    User.ISFP = ISFP;
        //        //    User.SACTC = SACTC;
        //        //    User.ILCOA = ILCOA;
        //        //    User.SCAA = SCAA;
        //        //    User.SCAAD = SCAAD;
        //        //    User.SCCC = SCCC;
        //        //    User.SCMCC = SCMCC;
        //        //    User.SUMCC = SUMCC;
        //        //    User.SCCCA = SCCCA;
        //        //    User.SCAG = SCAG;
        //        //    User.SCCA = SCCA;
        //        //    User.SCY = SCY;
        //        //    User.SBUAP = SBUAP;
        //        //    User.SFUAP = SFUAP;
        //        //    User.IUFA = IUFA;
        //        //    User.IBA = IBA;
        //        //    User.ICA = ICA;
        //        //    User.IFA = IFA;
        //        //    User.IGA = IGA;
        //        //    User.SDATCA = SDATCA;
        //        //    User.SCATCA = SCATCA;
        //        //    User.ICAA = ICAA;
        //        //    User.ICCC = ICCC;
        //        //    User.ILOC = ILOC;
        //        //    User.SOCP = SOCP;
        //        //    User.SUP = SUP;
        //        //    User.SPS = SPS;
        //        //    User.ICADA = ICADA;
        //        //    User.SPD = SPD;
        //        //    User.ICCCA = ICCCA;
        //        //    User.TCCB = TCCB;
        //        //    User.SUETR = SUETR;
        //        //    User.TCGE = TCGE;
        //        //    User.SUABP = SUABP;
        //        //    User.SUAFP = SUAFP;

        //        //    DB.SaveChanges();
        //        //}

        //        return Json("True", JsonRequestBehavior.AllowGet);
        //    //}
        //}



        //public JsonResult GetGroupAccess(string GroupID)
        //{
        //    //24
        //    //var GetAccess = DB.CreateGroup_Tables.Where(x => x.GroupID == GroupID).FirstOrDefault();

        //    //R_UG_Access_DTO r_Group_Access_DTO = new R_UG_Access_DTO()
        //    //{

        //    //    SCL = GetAccess.SCL,

        //    //    SCBI = GetAccess.SCBI,

        //    //    SCPI = GetAccess.SCPI,

        //    //    SCNA = GetAccess.SCNA,

        //    //    SLOU = GetAccess.SLOU,

        //    //    SCNG = GetAccess.SCNG,

        //    //    SLOG = GetAccess.SLOG,

        //    //    SAGI = GetAccess.SAGI,

        //    //    SUACP = GetAccess.SUACP,

        //    //    SUAP = GetAccess.SUAP,

        //    //    SFYD = GetAccess.SFYD,

        //    //    SCFY = GetAccess.SCFY,

        //    //    SCNY = GetAccess.SCNY,

        //    //    SCF = GetAccess.SCF,

        //    //    SCD = GetAccess.SCD,

        //    //    SCET = GetAccess.SCET,

        //    //    ILOU = GetAccess.ILOU,

        //    //    SCCOA = GetAccess.SCCOA,

        //    //    ICI = GetAccess.ICI,

        //    //    IUP = GetAccess.IUP,

        //    //    ISFP = GetAccess.ISFP,

        //    //    SACTC = GetAccess.SACTC,

        //    //    ILCOA = GetAccess.ILCOA,

        //    //    SCAA = GetAccess.SCAA,

        //    //    SCAAD = GetAccess.SCAAD,

        //    //    SCCC = GetAccess.SCCC,

        //    //    SCMCC = GetAccess.SCMCC,

        //    //    SUMCC = GetAccess.SUMCC,

        //    //    SCCCA = GetAccess.SCCCA,

        //    //    SCAG = GetAccess.SCAG,

        //    //    SCCA = GetAccess.SCCA,

        //    //    SCY = GetAccess.SCY,

        //    //    SBUAP = GetAccess.SBUAP,

        //    //    SFUAP = GetAccess.SFUAP,

        //    //    IUFA = GetAccess.IUFA,

        //    //    IBA = GetAccess.IBA,

        //    //    ICA = GetAccess.ICA,

        //    //    IFA = GetAccess.IFA,

        //    //    IGA = GetAccess.IGA,

        //    //    SDATCA = GetAccess.SDATCA,

        //    //    SCATCA = GetAccess.SCATCA,

        //    //    ICAA = GetAccess.ICAA,

        //    //    ICCC = GetAccess.ICCC,

        //    //    ILOC = GetAccess.ILOC,

        //    //    SOCP = GetAccess.SOCP,

        //    //    SUP = GetAccess.SUP,

        //    //    SPS = GetAccess.SPS,

        //    //    ICADA = GetAccess.ICADA,

        //    //    SPD = GetAccess.SPD,

        //    //    ICCCA = GetAccess.ICCCA,

        //    //    TCCB = GetAccess.TCCB,

        //    //    SUETR = GetAccess.SUETR

        //    //};

        //    //return Json(r_Group_Access_DTO, JsonRequestBehavior.AllowGet);
        //    return null;
        //}


        public JsonResult SaveAccessToUser(string userID, int type, List<UserFormsAccess_Table> userForms)
        {
            var dbData = DB.UserFormsAccess_Tables.Where(x => x.UserID == userID && x.Type == type).ToList();

            if (userForms == null)
            {
                DB.UserFormsAccess_Tables.RemoveRange(dbData);
            }
            else
            {
                var checkChange = userForms.Select(x => x.FormCode).Except(dbData.Select(x => x.FormCode)).ToList();
                var checkChange2 = dbData.Select(x => x.FormCode).Except(userForms.Select(x => x.FormCode)).ToList();

                if (!checkChange.Any() && !checkChange2.Any())
                {
                    return Json(false, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    DB.UserFormsAccess_Tables.RemoveRange(dbData);

                    DB.UserFormsAccess_Tables.AddRange(userForms);

                    var removeUserGroup = DB.UserGroup_Tables.Where(x => x.UserID == userID).FirstOrDefault();
                    if (removeUserGroup != null)
                    {
                        DB.UserGroup_Tables.Remove(removeUserGroup);
                    }
                }
            }
            DB.SaveChanges();
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        //public JsonResult SaveAccessToUser(string UserID, string SCL, string SCBI, string SCPI, string SCNA, string SLOU, string SCNG, string SLOG, string SAGI, string SUACP, string SUAP, string SFYD, string SCFY, string SCNY, string SCF, string SCD, string SCET, string ILOU, string SCCOA, string ICI, string IUP,
        //                                    string ISFP, string SACTC, string ILCOA, string SCAA, string SCAAD, string SCCC, string SCMCC, string SUMCC, string SCCCA, string SCAG, string SCCA, string SCY, string SBUAP, string SFUAP, string IUFA, string IBA, string ICA, string IFA, string IGA, string SDATCA, string SCATCA,
        //                                    string ICAA, string ICCC, string ILOC, string SOCP, string SUP, string SPS, string ICADA, string SPD, string ICCCA, string TCCB, string SUETR, string TCGE, string SUABP, string SUAFP)
        //{
        //    //13
        //    //var GetUserID = DB.AccountGroup_Tables.Where(x => x.UserID == UserID).FirstOrDefault();

        //    //if (GetUserID.SCL == SCL && GetUserID.SCBI == SCBI && GetUserID.SCPI == SCPI && GetUserID.SCNA == SCNA && GetUserID.SLOU == SLOU && GetUserID.SCNG == SCNG && GetUserID.SLOG == SLOG && GetUserID.SAGI == SAGI && GetUserID.SUACP == SUACP && GetUserID.SUAP == SUAP && GetUserID.SFYD == SFYD
        //    //    && GetUserID.SCFY == SCFY && GetUserID.SCNY == SCNY && GetUserID.SCF == SCF && GetUserID.SCD == SCD && GetUserID.SCET == SCET && GetUserID.ILOU == ILOU && GetUserID.SCCOA == SCCOA && GetUserID.ICI == ICI && GetUserID.IUP == IUP && GetUserID.ISFP == ISFP && GetUserID.SACTC == SACTC && GetUserID.ILCOA == ILCOA
        //    //    && GetUserID.SCAA == SCAA && GetUserID.SCAAD == SCAAD && GetUserID.SCCC == SCCC && GetUserID.SCMCC == SCMCC && GetUserID.SUMCC == SUMCC && GetUserID.SCCCA == SCCCA && GetUserID.SCAG == SCAG && GetUserID.SCCA == SCCA && GetUserID.SCY == SCY && GetUserID.SBUAP == SBUAP && GetUserID.SFUAP == SFUAP
        //    //    && GetUserID.IUFA == IUFA && GetUserID.IBA == IBA && GetUserID.ICA == ICA && GetUserID.IFA == IFA && GetUserID.IGA == IGA && GetUserID.SDATCA == SDATCA && GetUserID.SCATCA == SCATCA && GetUserID.ICAA == ICAA && GetUserID.ICCC == ICCC && GetUserID.ILOC == ILOC && GetUserID.SOCP == SOCP && GetUserID.SUP == SUP
        //    //    && GetUserID.SPS == SPS && GetUserID.ICADA == ICADA && GetUserID.SPD == SPD && GetUserID.ICCCA == ICCCA && GetUserID.TCCB == TCCB && GetUserID.SUETR == SUETR && GetUserID.TCGE == TCGE && GetUserID.SUABP == SUABP && GetUserID.SUAFP == SUAFP)
        //    //{
        //    //    return Json("False", JsonRequestBehavior.AllowGet);
        //    //}
        //    //else
        //    //{

        //    //    //var UID = GetUserID.ID;
        //    //    var UpdateUser = DB.AccountGroup_Tables.Where(x => x.UserID == UserID).FirstOrDefault();

        //    //    UpdateUser.SCL = SCL;
        //    //    UpdateUser.SCBI = SCBI;
        //    //    UpdateUser.SCPI = SCPI;
        //    //    UpdateUser.SCNA = SCNA;
        //    //    UpdateUser.SLOU = SLOU;
        //    //    UpdateUser.SCNG = SCNG;
        //    //    UpdateUser.SLOG = SLOG;
        //    //    UpdateUser.SAGI = SAGI;
        //    //    UpdateUser.SUACP = SUACP;
        //    //    UpdateUser.SUAP = SUAP;
        //    //    UpdateUser.SFYD = SFYD;
        //    //    UpdateUser.SCFY = SCFY;
        //    //    UpdateUser.SCNY = SCNY;
        //    //    UpdateUser.SCF = SCF;
        //    //    UpdateUser.SCD = SCD;
        //    //    UpdateUser.SCET = SCET;
        //    //    UpdateUser.ILOU = ILOU;
        //    //    UpdateUser.SCCOA = SCCOA;
        //    //    UpdateUser.ICI = ICI;
        //    //    UpdateUser.IUP = IUP;
        //    //    UpdateUser.ISFP = ISFP;
        //    //    UpdateUser.SACTC = SACTC;
        //    //    UpdateUser.ILCOA = ILCOA;
        //    //    UpdateUser.SCAA = SCAA;
        //    //    UpdateUser.SCAAD = SCAAD;
        //    //    UpdateUser.SCCC = SCCC;
        //    //    UpdateUser.SCMCC = SCMCC;
        //    //    UpdateUser.SUMCC = SUMCC;
        //    //    UpdateUser.SCCCA = SCCCA;
        //    //    UpdateUser.SCAG = SCAG;
        //    //    UpdateUser.SCCA = SCCA;
        //    //    UpdateUser.SCY = SCY;
        //    //    UpdateUser.SBUAP = SBUAP;
        //    //    UpdateUser.SFUAP = SFUAP;
        //    //    UpdateUser.IUFA = IUFA;
        //    //    UpdateUser.IBA = IBA;
        //    //    UpdateUser.ICA = ICA;
        //    //    UpdateUser.IFA = IFA;
        //    //    UpdateUser.IGA = IGA;
        //    //    UpdateUser.SDATCA = SDATCA;
        //    //    UpdateUser.SCATCA = SCATCA;
        //    //    UpdateUser.ICAA = ICAA;
        //    //    UpdateUser.ICCC = ICCC;
        //    //    UpdateUser.ILOC = ILOC;
        //    //    UpdateUser.SOCP = SOCP;
        //    //    UpdateUser.SUP = SUP;
        //    //    UpdateUser.SPS = SPS;
        //    //    UpdateUser.ICADA = ICADA;
        //    //    UpdateUser.SPD = SPD;
        //    //    UpdateUser.ICCCA = ICCCA;
        //    //    UpdateUser.TCCB = TCCB;
        //    //    UpdateUser.SUETR = SUETR;
        //    //    UpdateUser.TCGE = TCGE;
        //    //    UpdateUser.SUABP = SUABP;
        //    //    UpdateUser.SUAFP = SUAFP;
        //    //    UpdateUser.GroupID = "EmptyGroup";

        //    //    /*
        //    //    var GetCreateAccountID = DB.createAccount_Tables.Where(x => x.UserID == UserID).FirstOrDefault();
        //    //    var AccountID = GetCreateAccountID.ID;
        //    //    */
        //    //    var UpdateCreateAccount = DB.CreateAccount_Tables.Where(x => x.UserID == UserID).FirstOrDefault();

        //    //    UpdateCreateAccount.GroupID = "EmptyGroup";
        //    //    UpdateCreateAccount.DateOfAssignGroup = "";

        //    //    DB.SaveChanges();


        //    //    return Json("True", JsonRequestBehavior.AllowGet);
        //    //}
        //    return null;
        //}




        //public JsonResult GetUserAccess(string UserID)
        //{
        //    // 14
        //    //var checkUserCompAccess = DB.UACompPremission_Tables.Where(x => x.UserID == UserID).FirstOrDefault();

        //    //if (checkUserCompAccess != null)
        //    //{

        //    //    var GetAccess = DB.AccountGroup_Tables.Where(x => x.UserID == UserID).FirstOrDefault();

        //    //    R_UG_Access_DTO r_Group_Access_DTO = new R_UG_Access_DTO()
        //    //    {

        //    //        SCL = GetAccess.SCL,

        //    //        SCBI = GetAccess.SCBI,

        //    //        SCPI = GetAccess.SCPI,

        //    //        SCNA = GetAccess.SCNA,

        //    //        SLOU = GetAccess.SLOU,

        //    //        SCNG = GetAccess.SCNG,

        //    //        SLOG = GetAccess.SLOG,

        //    //        SAGI = GetAccess.SAGI,

        //    //        SUACP = GetAccess.SUACP,

        //    //        SUAP = GetAccess.SUAP,

        //    //        SFYD = GetAccess.SFYD,

        //    //        SCFY = GetAccess.SCFY,

        //    //        SCNY = GetAccess.SCNY,

        //    //        SCF = GetAccess.SCF,

        //    //        SCD = GetAccess.SCD,

        //    //        SCET = GetAccess.SCET,

        //    //        ILOU = GetAccess.ILOU,

        //    //        SCCOA = GetAccess.SCCOA,

        //    //        ICI = GetAccess.ICI,

        //    //        IUP = GetAccess.IUP,

        //    //        ISFP = GetAccess.ISFP,

        //    //        SACTC = GetAccess.SACTC,

        //    //        ILCOA = GetAccess.ILCOA,

        //    //        SCAA = GetAccess.SCAA,

        //    //        SCAAD = GetAccess.SCAAD,

        //    //        SCCC = GetAccess.SCCC,

        //    //        SCMCC = GetAccess.SCMCC,

        //    //        SUMCC = GetAccess.SUMCC,

        //    //        SCCCA = GetAccess.SCCCA,

        //    //        SCAG = GetAccess.SCAG,

        //    //        SCCA = GetAccess.SCCA,

        //    //        SCY = GetAccess.SCY,

        //    //        SBUAP = GetAccess.SBUAP,

        //    //        SFUAP = GetAccess.SFUAP,

        //    //        IUFA = GetAccess.IUFA,

        //    //        IBA = GetAccess.IBA,

        //    //        ICA = GetAccess.ICA,

        //    //        IFA = GetAccess.IFA,

        //    //        IGA = GetAccess.IGA,

        //    //        SDATCA = GetAccess.SDATCA,

        //    //        SCATCA = GetAccess.SCATCA,

        //    //        ICAA = GetAccess.ICAA,

        //    //        ICCC = GetAccess.ICCC,

        //    //        ILOC = GetAccess.ILOC,

        //    //        SOCP = GetAccess.SOCP,

        //    //        SUP = GetAccess.SUP,

        //    //        SPS = GetAccess.SPS,

        //    //        ICADA = GetAccess.ICADA,

        //    //        SPD = GetAccess.SPD,

        //    //        ICCCA = GetAccess.ICCCA,

        //    //        TCCB = GetAccess.TCCB,

        //    //        SUETR = GetAccess.SUETR,

        //    //        TCGE = GetAccess.TCGE,

        //    //        SUABP = GetAccess.SUABP,

        //    //        SUAFP = GetAccess.SUAFP
        //    //    };

        //    //    return Json(r_Group_Access_DTO, JsonRequestBehavior.AllowGet);
        //    //}
        //    //else
        //    //{
        //    //    return Json("False", JsonRequestBehavior.AllowGet);
        //    //}
        //    return null;
        //}


        public JsonResult CopyAccessOfUser(string TargetUserID)
        {
            List<Forms> forms = DB.UserFormsAccess_Tables.Where(x => x.UserID == TargetUserID).Select(x => new Forms
            {
                FormName = x.FormCode
            }).ToList();
            return Json(forms, JsonRequestBehavior.AllowGet);
        }

        //public JsonResult CopyAccessOfUser(string TargetUserID)
        //{
        //    //15
        //    //var GetAccess = DB.AccountGroup_Tables.Where(x => x.UserID == TargetUserID).FirstOrDefault();

        //    //R_UG_Access_DTO r_UG_Access_DTO = new R_UG_Access_DTO()
        //    //{
        //    //    SCL = GetAccess.SCL,

        //    //    SCBI = GetAccess.SCBI,

        //    //    SCPI = GetAccess.SCPI,

        //    //    SCNA = GetAccess.SCNA,

        //    //    SLOU = GetAccess.SLOU,

        //    //    SCNG = GetAccess.SCNG,

        //    //    SLOG = GetAccess.SLOG,

        //    //    SAGI = GetAccess.SAGI,

        //    //    SUACP = GetAccess.SUACP,

        //    //    SUAP = GetAccess.SUAP,

        //    //    SFYD = GetAccess.SFYD,

        //    //    SCFY = GetAccess.SCFY,

        //    //    SCNY = GetAccess.SCNY,

        //    //    SCF = GetAccess.SCF,

        //    //    SCD = GetAccess.SCD,

        //    //    SCET = GetAccess.SCET,

        //    //    ILOU = GetAccess.ILOU,

        //    //    SCCOA = GetAccess.SCCOA,

        //    //    ICI = GetAccess.ICI,

        //    //    IUP = GetAccess.IUP,

        //    //    ISFP = GetAccess.ISFP,

        //    //    SACTC = GetAccess.SACTC,

        //    //    ILCOA = GetAccess.ILCOA,

        //    //    SCAA = GetAccess.SCAA,

        //    //    SCAAD = GetAccess.SCAAD,

        //    //    SCCC = GetAccess.SCCC,

        //    //    SCMCC = GetAccess.SCMCC,

        //    //    SUMCC = GetAccess.SUMCC,

        //    //    SCCCA = GetAccess.SCCCA,

        //    //    SCAG = GetAccess.SCAG,

        //    //    SCCA = GetAccess.SCCA,

        //    //    SCY = GetAccess.SCY,

        //    //    SBUAP = GetAccess.SBUAP,

        //    //    SFUAP = GetAccess.SFUAP,

        //    //    IUFA = GetAccess.IUFA,

        //    //    IBA = GetAccess.IBA,

        //    //    ICA = GetAccess.ICA,

        //    //    IFA = GetAccess.IFA,

        //    //    IGA = GetAccess.IGA,

        //    //    SDATCA = GetAccess.SDATCA,

        //    //    SCATCA = GetAccess.SCATCA,

        //    //    ICAA = GetAccess.ICAA,

        //    //    ICCC = GetAccess.ICCC,

        //    //    ILOC = GetAccess.ILOC,

        //    //    SOCP = GetAccess.SOCP,

        //    //    SUP = GetAccess.SUP,

        //    //    SPS = GetAccess.SPS,

        //    //    ICADA = GetAccess.ICADA,

        //    //    SPD = GetAccess.SPD,

        //    //    ICCCA = GetAccess.ICCCA,

        //    //    TCCB = GetAccess.TCCB,

        //    //    SUETR = GetAccess.SUETR,

        //    //    TCGE = GetAccess.TCGE,

        //    //    SUABP = GetAccess.SUABP,

        //    //    SUAFP = GetAccess.SUAFP
        //    //};

        //    //return Json(r_UG_Access_DTO, JsonRequestBehavior.AllowGet);
        //    return null;
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


        public JsonResult FilterUserID(string UserID)
        {
            DB.Configuration.ProxyCreationEnabled = false;
            var UserList = DB.CreateAccount_Tables.Where(x => x.UserID != UserID && (x.Deleted == false || x.Deleted == null)).ToList();

            return Json(UserList, JsonRequestBehavior.AllowGet);
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
        //            item.SUAP = Value;
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
        //        if (item.SUAP.ToString().Equals("True"))
        //        {
        //            return Json("True", JsonRequestBehavior.AllowGet);
        //        }
        //        else
        //        {
        //            return Json("False", JsonRequestBehavior.AllowGet);
        //        }
        //    }
        //}




        // GET: UGBranchFormAccess
        [HttpGet]
       // [Authorize]
        [CustomAuthorize(Roles = "SBUAP")]
        public ActionResult BranchFormsAccess()
        {
            var getUsersInCompany = DB.CreateAccount_Tables.Where(x => x.UABranchPremission_Table.Any()).ToList();
            SelectList UserList = new SelectList(getUsersInCompany, "UserID", "UserID");
            ViewBag.UserList = UserList;

            var GroupID = DB.CreateGroup_Tables.Where(a => a.FromCBF == 2 && (a.Deleted == false || a.Deleted == null)).ToList();
            SelectList GroupList = new SelectList(GroupID, "GroupID", "GroupID");

            ViewBag.GroupList = GroupList;

            ViewBag.Type = 2;

            return View();
        }

        // GET: UGFactoryFormAccess
        [HttpGet]
       // [Authorize]
        [CustomAuthorize(Roles = "SFUAP")]
        public ActionResult FactoryFormsAccess()
        {
            var getUsersInCompany = DB.CreateAccount_Tables.Where(x => x.UAFactoryPremission_Table.Any()).ToList();
            SelectList UserList = new SelectList(getUsersInCompany, "UserID", "UserID");
            ViewBag.UserList = UserList;

            var GroupID = DB.CreateGroup_Tables.Where(a => a.FromCBF == 3 && (a.Deleted == false || a.Deleted == null)).ToList();
            SelectList GroupList = new SelectList(GroupID, "GroupID", "GroupID");

            ViewBag.GroupList = GroupList;

            ViewBag.Type = 3;

            return View();
        }
    }
}