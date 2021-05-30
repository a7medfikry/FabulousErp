using FabulousDB.DB_Context;
using FabulousDB.DB_Tabels.Settings.Financial.GeneralSetting.Post;
using FabulousErp.MyRoleProvider;
using FabulousErp.Repository.Interface;
using FabulousModels.DTOModels.Settings.Financial.GeneralSettings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FabulousErp.Controllers.Settings.Financial.GeneralSetting.Post
{
    [BranchSecurityFilter]
    [FactorySecurityFilter]
    [AuthorizationFilter]
    public class CreatePostingSetupController : Controller
    {

        IRepetitionBusiness repetitionBusiness;

        public CreatePostingSetupController(IRepetitionBusiness repetitionBusiness)
        {
            this.repetitionBusiness = repetitionBusiness;
        }

        DBContext DB = new DBContext();


        // GET: TransactionPostingSetup
        [HttpGet]
       // [Authorize]
        [CustomAuthorize(Roles = "SPS")]
        public ActionResult PostingSetup()
        {
            string companyID = (string)FabulousErp.Business.GetCompanyId();
            //ViewBag.CompanyList = repetitionBusiness.RetrieveCompIDListCond(companyID);
            ViewBag.CompanyID = companyID;
            ViewBag.CompanyName = repetitionBusiness.GetCompanyName(companyID);
            ViewBag.Module = new SelectList(Business.GetAreasNames().Select(x=>new { Id=x,Name=x}),"Id","Name");
            return View();
        }


        //public JsonResult GetCompanyName(string companyID)
        //{
        //    return Json(repetitionBusiness.GetCompanyName(companyID), JsonRequestBehavior.AllowGet);
        //}



        public JsonResult SavePostingSetup(string companyID, string module, string postingType, string journalEntryPer, string batch, string postingDataFrom, string existingBatch, string editPostingDate)
        {
            var checkDuplicate = DB.PostingSetup_Tables.Where(x => x.CompanyID == companyID && x.Module == module).FirstOrDefault();

            if (checkDuplicate == null)
            {
                PostingSetup_Table postingSetup_Table = new PostingSetup_Table()
                {
                    CompanyID = companyID,

                    Module = module,

                    PostingType = postingType,

                    CreateJEPer = journalEntryPer,

                    Batch = batch,

                    PostingDataFrom = postingDataFrom,

                    ExistingBatch = existingBatch,

                    EditPostingDate = editPostingDate
                };

                DB.PostingSetup_Tables.Add(postingSetup_Table);
                DB.SaveChanges();
            }
            return null;
        }


        public JsonResult GetPostingSetup(string companyID, string module)
        {
            var getData =  DB.PostingSetup_Tables.Where(x => x.CompanyID == companyID && x.Module == module).FirstOrDefault();

            if (getData != null)
            {
                User_Post_DTO user_Post_DTO = new User_Post_DTO()
                {
                    PostingType = getData.PostingType,

                    CreateJEPer = getData.CreateJEPer,

                    Batch = getData.Batch,

                    PostingDataFrom = getData.PostingDataFrom,

                    ExistingBatch = getData.ExistingBatch,

                    EditPostingDate = getData.EditPostingDate
                };

                return Json(user_Post_DTO, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("False", JsonRequestBehavior.AllowGet);
            }
        }


        public JsonResult UpdatePostingSetup(string companyID, string module, string postingType, string journalEntryPer, string batch, string postingDataFrom, string existingBatch, string editPostingDate)
        {
            var checkTransactionData = DB.C_GeneralJournalEntry_Tables.Where(x=>x.Post == false).Any();

            if (checkTransactionData)
            {
                return Json("ExistData", JsonRequestBehavior.AllowGet);
            }
            else
            {
                var getData = DB.PostingSetup_Tables.Where(x => x.CompanyID == companyID && x.Module == module).FirstOrDefault();

                if (getData != null)
                {
                    int id = getData.PS_ID;

                    var update = DB.PostingSetup_Tables.Where(x => x.PS_ID == id).FirstOrDefault();

                    update.PostingType = postingType;
                    update.CreateJEPer = journalEntryPer;
                    update.Batch = batch;
                    update.PostingDataFrom = postingDataFrom;
                    update.ExistingBatch = existingBatch;
                    update.EditPostingDate = editPostingDate;

                    DB.SaveChanges();
                }

                return null;
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
        //            item.SPS = Value;
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
        //        if (item.SPS.ToString().Equals("True"))
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