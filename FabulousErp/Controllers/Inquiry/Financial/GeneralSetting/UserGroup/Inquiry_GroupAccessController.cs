using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FabulousDB.DB_Context;
using FabulousErp.MyRoleProvider;
using FabulousErp.Repository.Interface;
using FabulousModels.DTOModels.Inquiry.Financial.GeneralSetting.UserGroup;

namespace FabulousErp.Controllers.Inquiry.Financial.GeneralSetting.UserGroup
{
    [BranchSecurityFilter]
    [FactorySecurityFilter]
    [AuthorizationFilter]
    public class Inquiry_GroupAccessController : Controller
    {
        IRepetitionBusiness repetitionBusiness;

        public Inquiry_GroupAccessController(IRepetitionBusiness repetitionBusiness)
        {
            this.repetitionBusiness = repetitionBusiness;
        }

        DBContext DB = new DBContext();

        // GET: Inquiry_GroupAccess
        [HttpGet]
       // [Authorize]
        [CustomAuthorize(Roles = "IGA")]
        public ActionResult GroupAccess()
        {
            ViewBag.CodeList = repetitionBusiness.RetrieveGroupIDList();
            return View();
        }

        public JsonResult GetGroupName(string groupID)
        {
            return Json(repetitionBusiness.GetGroupName(groupID), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetData(string groupID)
        {
            //7
            List<Inquiry_GroupAccess> inquiry_GroupAccesses = DB.UserGroup_Tables.Where(x => x.GroupID == groupID).Select(x => new Inquiry_GroupAccess
            {
                GroupID = x.GroupID,
                UserID = x.UserID,
                UserName = x.CreateAccount_Table.UserName
            }).ToList();
            return Json(inquiry_GroupAccesses, JsonRequestBehavior.AllowGet);
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
        //            item.IGA = Value;
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
        //        if (item.IGA.ToString().Equals("True"))
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