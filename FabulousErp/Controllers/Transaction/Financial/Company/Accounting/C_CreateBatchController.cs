using FabulousDB.DB_Context;
using FabulousDB.DB_Tabels.Transaction.Financial.Company.Accounting;
using FabulousErp.MyRoleProvider;
using FabulousErp.Repository.Interface;
using FabulousModels.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FabulousErp.Controllers.Transaction.Financial.Company.Accounting
{
    [BranchSecurityFilter]
    [FactorySecurityFilter]
    [AuthorizationFilter]
    public class C_CreateBatchController : Controller
    {
        IRepetitionBusiness repetitionBusiness;

        public C_CreateBatchController(IRepetitionBusiness repetitionBusiness)
        {
            this.repetitionBusiness = repetitionBusiness;
        }

        DBContext DB = new DBContext();


        // GET: C_CreateBatch
        [HttpGet]
       // [Authorize]
        [CustomAuthorize(Roles = "TCCB")]
        public ActionResult CompanyCreateBatch()
        {
            string companyID = (string)FabulousErp.Business.GetCompanyId();
            ViewBag.Module = ViewBag.Module = new SelectList(Business.GetAreasNames().Select(x => new { Id = x, Name = x }), "Id", "Name");
            ;
            ViewBag.CompanyList = repetitionBusiness.RetrieveCompIDListCond(companyID);

            var checkPS = FabulousErp.Business.GetPostingSetup();
            if (checkPS == null)
            {
                ViewBag.PSExist = false;
            }

            return View();
        }

        public JsonResult GetCompanyName(string companyID)
        {
            return Json(repetitionBusiness.GetCompanyName(companyID), JsonRequestBehavior.AllowGet);
        }

        public JsonResult SaveCompanyBatch(string companyID, string module, string batchID, string batchDescription)
        {
            var checkDuplicate = DB.C_CreateBatch_Tables.Where(x => x.CompanyID == companyID && x.C_Module == module && x.C_BatchLocation == "TCGE" && x.C_BatchID == batchID && x.Removed == false).FirstOrDefault();

            if (checkDuplicate != null)
            {
                return Json("False", JsonRequestBehavior.AllowGet);
            }
            else
            {
                var postingSetup = Business.GetPostingSetup();//  Business.GetPostingSetup();

                bool notApproval = false;
                bool approval = false;
                if (postingSetup.Batch == "C2")
                {
                    notApproval = true;
                    approval = false;
                }
                else
                {
                    approval = true;
                    notApproval = false;
                }

                C_CreateBatch_Table c_CreateBatch_Table = new C_CreateBatch_Table()
                {
                    CompanyID = companyID,

                    C_Module = module,

                    C_BatchID = batchID,

                    C_BatchDescription = batchDescription,

                    C_BatchCreationDate = DateTime.Now.ToShortDateString(),

                    C_BatchLocation = "TCGE",

                    NotApproval = notApproval,

                    Approval = approval,

                    Removed = false,

                    UserID = FabulousErp.Business.GetUserId()
                };
                DB.C_CreateBatch_Tables.Add(c_CreateBatch_Table);
                DB.SaveChanges();
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
        //            item.TCCB = Value;
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
        //        if (item.TCCB.ToString().Equals("True"))
        //        {
        //            return Json("True", JsonRequestBehavior.AllowGet);
        //        }
        //        else
        //        {
        //            return Json("False", JsonRequestBehavior.AllowGet);
        //        }
        //    }
        //}



        [HttpGet]
       // [Authorize]
        public ActionResult CompanyBatchApproval()
        {
            string companyID = (string)FabulousErp.Business.GetCompanyId();
            
            List<BatchApproval> batchApprovals = DB.C_CreateBatch_Tables.Where(x => x.CompanyID == companyID && x.Removed == false && x.Approval == false).OrderBy(x => x.C_BatchCreationDate).Select(x => new BatchApproval
            {
                CBID = x.C_CBID,

                BatchID = x.C_BatchID,

                Module = x.C_Module,

                Description = x.C_BatchDescription,

                CreationDate = x.C_BatchCreationDate,

                BatchLocation = x.C_BatchLocation,

                Approval = x.Approval ?? false,

                NotApproval = x.NotApproval ?? false,

                NumOfTransactions = x.C_GeneralJournalEntry_Tables.Count

            }).ToList();

            var checkPS = Business.GetPostingSetup();
            if (checkPS == null)
            {
                ViewBag.PSExist = false;
            }
            else
            {
                ViewBag.PSExist = checkPS.Batch;
            }

            return View(batchApprovals);
        }

        [HttpPost]
        public ActionResult CompanyBatchApproval(List<BatchApproval> batchApprovals)
        {
            if (ModelState.IsValid)
            {
                if (batchApprovals != null)
                {
                    foreach (var batch in batchApprovals)
                    {
                        var check = DB.C_CreateBatch_Tables.Where(x => x.C_CBID == batch.CBID).FirstOrDefault();
                        if (check != null)
                        {
                            check.Approval = batch.Approval;
                            check.NotApproval = batch.NotApproval;
                        }

                        C_UserBatchApproval_Table addUser = new C_UserBatchApproval_Table()
                        {
                            C_CBID = batch.CBID,

                            UserID = FabulousErp.Business.GetUserId(),

                            ApprovedDate = DateTime.Today.ToShortDateString()
                        };
                        if (!DB.C_UserBatchApproval_Tables.Any(x=>x.C_CBID== addUser.C_CBID))
                        {
                            DB.C_UserBatchApproval_Tables.Add(addUser);
                        }
                        else
                        {
                            C_UserBatchApproval_Table ThisaddUser = DB.C_UserBatchApproval_Tables.FirstOrDefault(x => x.C_CBID == addUser.C_CBID);
                            ThisaddUser.ApprovedDate = DateTime.Today.ToShortDateString();
                            ThisaddUser.UserID = FabulousErp.Business.GetUserId();

                        }
                    }
                }
                DB.SaveChanges();
            }
            return RedirectToAction("CompanyBatchApproval", "C_CreateBatch");
        }



        [HttpGet]
       // [Authorize]
        public ActionResult CompanyMultiBatchesPost()
        {
            string companyID = (string)FabulousErp.Business.GetCompanyId();
            List<BatchApproval> batchApprovals = DB.C_CreateBatch_Tables.Where(x => x.CompanyID == companyID && x.Removed == false).OrderBy(x => x.C_BatchCreationDate).Select(x => new BatchApproval
            {
                CBID = x.C_CBID,
                BatchID = x.C_BatchID,
                Module = x.C_Module,
                Description = x.C_BatchDescription,
                CreationDate = x.C_BatchCreationDate,
                BatchLocation = x.C_BatchLocation,
                Approval = x.Approval ?? false,
                NotApproval = x.NotApproval ?? false,
                NumOfTransactions = x.C_GeneralJournalEntry_Tables.Count
            }).ToList();
            return View(batchApprovals);
        }

        public JsonResult DeleteBtaches(List<BatchApproval> deletedRange)
        {
            if (deletedRange != null)
            {
                foreach (var item in deletedRange)
                {
                    //
                    var delete = DB.C_CreateBatch_Tables.Include("C_UserBatchApproval_Table").FirstOrDefault(x => x.C_CBID == item.CBID && x.Approval == true);
                    if (delete != null)
                    {
                        DB.C_CreateBatch_Tables.Remove(delete);
                        try
                        {
                            DB.SaveChanges();

                        }
                        catch (Exception ex)
                        {
                            try
                            {
                                C_UserBatchApproval_Table CUT = DB.C_UserBatchApproval_Tables.Find(delete.C_CBID);
                                if (CUT != null)
                                {
                                    DB.C_UserBatchApproval_Tables.Remove(CUT);
                                }
                                DB.C_CreateBatch_Tables.Remove(delete);
                                DB.SaveChanges();
                            }
                            catch
                            {

                            }
                          

                        }
                    }
                }
            }

            return Json(true, JsonRequestBehavior.AllowGet);
        }

        public JsonResult PostBatches(List<BatchApproval> postedRange)
        {
            string companyID = (string)FabulousErp.Business.GetCompanyId();

            int posting = 1;
            var postingCheck = DB.C_GeneralJournalEntry_Tables
                .Where(x => x.CompanyID == companyID)
                .OrderByDescending(x => x.C_Posting)
                .FirstOrDefault();
            if (postingCheck != null)
            {
                posting = postingCheck.C_Posting + 1;
            }

            if (postedRange != null)
            {
                foreach (var item in postedRange)
                {
                    var getJournal = DB.C_GeneralJournalEntry_Tables.Where(x => x.C_CBID == item.CBID).ToList();

                    var checkApproved = DB.C_CreateBatch_Tables.FirstOrDefault(x => x.C_CBID == item.CBID && x.Approval == true);

                    if (checkApproved != null)
                    {
                        foreach (var batches in getJournal)
                        {
                            batches.Post = true;
                            batches.C_Posting = posting;
                            batches.C_CreateBatch_Table.Removed = true;

                            foreach (var transactionItem in batches.C_SaveTransaction_Tables)
                            {
                                C_GeneralLedger_Table c_GeneralLedger_Table = new C_GeneralLedger_Table()
                                {
                                    C_AID = transactionItem.C_AID,

                                    Ballance = transactionItem.Ballance,

                                    C_Credit = transactionItem.C_Credit,

                                    C_Debit = transactionItem.C_Debit,

                                    C_Describtion = transactionItem.C_Describtion,

                                    C_Document = transactionItem.C_Document,

                                    C_OriginalAmount = transactionItem.C_OriginalAmount,

                                    C_PostingNumber = transactionItem.C_PostingNumber
                                };
                                DB.C_GeneralLedger_Tables.Add(c_GeneralLedger_Table);
                            }

                            batches.C_SaveAnalytic_Tables.All(x => x.Post = true);

                            batches.C_SaveCostCenter_Tables.All(x => x.Post = true);

                            DB.C_SaveTransaction_Tables.RemoveRange(batches.C_SaveTransaction_Tables);
                        }
                    }
                }
                DB.SaveChanges();
            }

            return Json(true, JsonRequestBehavior.AllowGet);
        }

    }
}