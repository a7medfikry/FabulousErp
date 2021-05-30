using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FixedAssets.Controllers
{
    public class PPartialController : Controller
    {
        // GET: FixedAssets/PPartial
        public ActionResult VoidPartial(string Controller,string Currency,string PostingToOrThrow,string PostingNum,int Id
          , DateTime TransactionDate, string DeleteAction="Delete",string MyArea= "FixedAssets",string SubmitUrl="",
            string AdditinolParm="",string PostingKey= "FIXVRenw",string RedirectLink=null)
        {
            string companyID = (string)FabulousErp.Business.GetCompanyId();
            ViewBag.CompanyID = companyID;
            ViewBag.PostingToOrThrow = Business.Business.PostingToOrThrow();
            ViewBag.MyArea = MyArea;
           // ViewBag.PostingToOrThrow = PostingToOrThrow;
            ViewBag.Currency = Currency;
            ViewBag.SubmitController = Controller;
            ViewBag.SubmitAction = DeleteAction;
            ViewBag.PostingNum = PostingNum;
            ViewBag.Id = Id;
            ViewBag.SubmitUrl = SubmitUrl;
            ViewBag.TransactionDate = TransactionDate;
            ViewBag.AdditinolParm = AdditinolParm;
            ViewBag.PostingKey = PostingKey;
            ViewBag.RedirectLink = RedirectLink;
            return View();
        }
    }
}