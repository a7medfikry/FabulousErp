using Inventory.Controllers;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Inventory.Controllers
{
    public class BussnissController : Controller
    {
        // GET: Inventory/Bussniss
        public JsonResult GetEqQty(int ItemId,int UOM,float Qty)
        {
            return Json(new { Qty = InvBus.CalcItemEq(ItemId, UOM, Qty),EqQty= InvBus.CalcItemEq(ItemId, UOM, 1) });
        }
    }
}