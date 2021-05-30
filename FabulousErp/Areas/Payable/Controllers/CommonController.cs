using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Payable.Controllers

{
    public class CommonController : Controller
    {
        // GET: Payable/Common
        public PartialViewResult ReportSearch(string section,string Cont)
        {
            ViewBag.section = section;
            ViewBag.Cont = Cont;
            return PartialView();
        }
      
    }
}