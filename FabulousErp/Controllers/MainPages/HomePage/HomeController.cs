using FabulousDB.DB_Context;
using FabulousModels.ViewModels.Important;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FabulousModels.DTOModels.Important;
using FabulousErp.MyRoleProvider;
using System.Threading;
using System.Globalization;

namespace FabulousErp.Controllers.MainPages.HomePage
{
    [AuthorizationFilter]
    public class HomeController : Controller
    {
        DBContext DB = new DBContext();

       // [Authorize]
        public ActionResult Financial_Home()
        {
            return View();
        }

        public ActionResult Purchasing_Home()
        {
            return View();
        }
        public ActionResult Sales_Home()
        {
            return View();
        }
        public ActionResult Inventory_Home()
        {
            return View();
        }

        public JsonResult GetSearchValue(string SearchValue)
        {
            //Note : you can bind same list from database  
            List<All_Project_Forms> ObjList = new List<All_Project_Forms>()
            {
                new All_Project_Forms {FormCode="SCL",FormName="Company Information" },
                new All_Project_Forms {FormCode="SCBI",FormName="Company Branch Information" },
                new All_Project_Forms {FormCode="SCPI",FormName="Company Factory Information" },
                new All_Project_Forms {FormCode="SCNA",FormName="Creat Account" },
                new All_Project_Forms {FormCode="SLOU",FormName="S List of Users" },
                new All_Project_Forms {FormCode="SCNG",FormName="Create Group" },
                new All_Project_Forms {FormCode="SLOG",FormName="S List of Groups" },
                new All_Project_Forms {FormCode="SAGI",FormName="Account Group Information" },
                new All_Project_Forms {FormCode="SUACP",FormName="User Access Company Premission" },
                new All_Project_Forms {FormCode="SUABP",FormName="User Access Branch Premission" },
                new All_Project_Forms {FormCode="SUAFP",FormName="User Access Factory Premission" },
                new All_Project_Forms {FormCode="SUAP",FormName="User & Group Access Premission" },
                new All_Project_Forms {FormCode="SFYD",FormName="Fiscal Year Definition" },
                new All_Project_Forms {FormCode="SCFY",FormName="Assign Fiscal Year To Company" },
                new All_Project_Forms {FormCode="SCNY",FormName="Create New Fiscal Year" },
                new All_Project_Forms {FormCode="SCY",FormName="Close Fiscal Year" },
                new All_Project_Forms {FormCode="SCF",FormName="Formate Setting" },
                new All_Project_Forms {FormCode="SCD",FormName="Currencies Definition" },
                new All_Project_Forms {FormCode="SCET",FormName="Exchange Currencies" },
                new All_Project_Forms {FormCode="SACTC",FormName="Add Chart Of Account To Company" },
                new All_Project_Forms {FormCode="SCCOA",FormName="Create Chart of Account" },
                new All_Project_Forms {FormCode="ICI",FormName="Company Information" },
                new All_Project_Forms {FormCode="IUP",FormName="User Profile" },
                new All_Project_Forms {FormCode="ISFP",FormName="Show Fiscal Periods" },
                new All_Project_Forms {FormCode="ILOU",FormName="List Of User" },
                new All_Project_Forms {FormCode="ILCOA",FormName="List Chart of Account" },
                new All_Project_Forms {FormCode="SCAA",FormName="Company Analytic Accounts" },
                new All_Project_Forms {FormCode="SBAA",FormName="Branch Analytic Accounts" },
                new All_Project_Forms {FormCode="SFAA",FormName="Factory Analytic Accounts" },
                new All_Project_Forms {FormCode="SCCC",FormName="Company Cost Center" },
                new All_Project_Forms {FormCode="SBCC",FormName="Branch Cost Center" },
                new All_Project_Forms {FormCode="SFCC",FormName="Factory Cost Center" },
                new All_Project_Forms {FormCode="SCMCC",FormName="Create Main Cost Center" },
                new All_Project_Forms {FormCode="SUMCC",FormName="Company Update Main Cost Center" },
                new All_Project_Forms {FormCode="SCCCA",FormName="Create Cost Center Accounts" },
                new All_Project_Forms {FormCode="SCAG",FormName="Create Account Group" },
                new All_Project_Forms {FormCode="SCCA",FormName="Company Create Account" },
                new All_Project_Forms {FormCode="SCAAD",FormName="Company Analytic Account Distribution" },
                new All_Project_Forms {FormCode="SBAAD",FormName="Branch Analytic Account Distribution" },
                new All_Project_Forms {FormCode="SFAAD",FormName="Factory Analytic Account Distribution" },
                new All_Project_Forms {FormCode="SCLAA",FormName="Company Link Analytic To Account" },
                new All_Project_Forms {FormCode="SBLAA",FormName="Branch Link Analytic To Account" },
                new All_Project_Forms {FormCode="SFLAA",FormName="Factory Link Analytic To Account" },
                new All_Project_Forms {FormCode="SBMCC",FormName="Branch Main Cost Center" },
                new All_Project_Forms {FormCode="SFMCC",FormName="Factory Main Cost Center" },
                new All_Project_Forms {FormCode="SUBMCC",FormName="Branch Update Main Cost Center" },
                new All_Project_Forms {FormCode="SUFMCC",FormName="Factory Update Main Cost Center" },
                new All_Project_Forms {FormCode="SBCCA",FormName="Branch Create Cost Center Accounts" },
                new All_Project_Forms {FormCode="SFCCA",FormName="Factory Create Cost Center Accounts" },
                new All_Project_Forms {FormCode="SBUAP",FormName="Branch User Forms Access" },
                new All_Project_Forms {FormCode="SFUAP",FormName="Factory User Forms Access" },
                new All_Project_Forms {FormCode="IUFA",FormName="List Of User Forms Access" },
                new All_Project_Forms {FormCode="ICA",FormName="List Of Company Access" },
                new All_Project_Forms {FormCode="IBA",FormName="List Of Branch Access" },
                new All_Project_Forms {FormCode="IFA",FormName="List Of Factory Access" },
                new All_Project_Forms {FormCode="IGA",FormName="List Of Group Access" },
                new All_Project_Forms {FormCode="SDATCA",FormName="Add Dist. Accounts To Company Account" },
                new All_Project_Forms {FormCode="SCATCA",FormName="Add Cost Accounts To Company Account" },
                new All_Project_Forms {FormCode="IBAA",FormName="List Of Branch Analytic Account" },
                new All_Project_Forms {FormCode="IBCC",FormName="List Of Branch Cost Center" },
                new All_Project_Forms {FormCode="ICAA",FormName="List Of Company Analytic Account" },
                new All_Project_Forms {FormCode="ICCC",FormName="List Of Company Cost Center" },
                new All_Project_Forms {FormCode="SBCA",FormName="Branch Create Account" },
                new All_Project_Forms {FormCode="SDATBA",FormName="Add Dist. Accounts To Branch Account" },
                new All_Project_Forms {FormCode="SCATBA",FormName="Add Cost Accounts To Branch Account" },
                new All_Project_Forms {FormCode="SFCA",FormName="Factory Create Account" },
                new All_Project_Forms {FormCode="SDATFA",FormName="Add Dist. Accounts To Factory Account" },
                new All_Project_Forms {FormCode="SCATFA",FormName="Add Cost Accounts To Factory Account" },
                new All_Project_Forms {FormCode="SOCP",FormName="Open / Close Periods" },
                new All_Project_Forms {FormCode="SUP",FormName="User Post" },
                new All_Project_Forms {FormCode="SPS",FormName="Posting Setup" },
                new All_Project_Forms {FormCode="IBADA",FormName="List of Branch Analytic Distribution" },
                new All_Project_Forms {FormCode="IBCCA",FormName="List of Branch Center Accounts" },
                new All_Project_Forms {FormCode="ICADA",FormName="List of Company Analytic Distribution" },
                new All_Project_Forms {FormCode="SPD",FormName="Print Document" },
                new All_Project_Forms {FormCode="ICCCA",FormName="List of Company Cost Center Accounts" },
                new All_Project_Forms {FormCode="IFADA",FormName="List of Factory Analytic Distribution" },
                new All_Project_Forms {FormCode="IFCCA",FormName="List of Factory Cost Center Accounts" },
                new All_Project_Forms {FormCode="TCCB",FormName="Company Create Batch" },
                new All_Project_Forms {FormCode="TBCB",FormName="Branch Create Batch" },
                new All_Project_Forms {FormCode="TFCB",FormName="Factory Create Batch" },
                new All_Project_Forms {FormCode="SUETR",FormName="User Edit Transaction Rate" },
                new All_Project_Forms {FormCode="TCGE",FormName="Company General Entry Transaction" },
                new All_Project_Forms {FormCode="CDTBA",FormName="Link Currency Definition To Branch Account" },
                new All_Project_Forms {FormCode="CDTFA",FormName="Link Currency Definition To Factory Account" },
                new All_Project_Forms {FormCode="TCBA",FormName="Company Batch Approval" },
                new All_Project_Forms {FormCode="TCS",FormName="Show Company Transactions" },
                new All_Project_Forms {FormCode="IADI",FormName="List of Account Transactions Details" },
                new All_Project_Forms {FormCode="IASI",FormName="List of Account Transactions Summary" },
                new All_Project_Forms {FormCode="ITDI",FormName="List of Transactions Details" },
                new All_Project_Forms {FormCode="ICBSI",FormName="List of Batches Security" },
                new All_Project_Forms {FormCode="TCGV",FormName="Company Void General Transactions" },
                new All_Project_Forms {FormCode="IHADI",FormName="Historical Account Details Inquiry" },
                new All_Project_Forms {FormCode="IHASI",FormName="Historical Account Summary Inquiry" }

        };
            //Searching records from list using LINQ query  
            var CityList = (from N in ObjList
                            where N.FormCode.Contains(SearchValue)
                            select new { N.FormCode, N.FormName });

            return Json(CityList, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Accounting()
        {
            return View();
        }
        public ActionResult FixedAssets()
        {
            return View();
        }
        public ActionResult CheckBook()
        {
            return View();
        } 
        public ActionResult Payable()
        {
            return View();
        } 
        public ActionResult Receivable()
        {
            return View();
        }
        public ActionResult Installment()
        {
            return View();
        }
        public ActionResult Inventory()
        {
            return View();
        }
        //public JsonResult CheckFavourites()
        //{
        //    string userID = FabulousErp.Business.GetUserId();

        //    if (userID == "SA")
        //    {
        //        return Json(JsonRequestBehavior.AllowGet);
        //    }
        //    else
        //    {
        //        var item = DB.FavouritesForms_Tables.Where(x => x.UserID == userID).FirstOrDefault();

        //        if(item != null)
        //        {
        //            Favourites favourites = new Favourites()
        //            {
        //                SCBI = item.SCBI.ToString(),
        //                SCL = item.SCL.ToString(),
        //                SCPI = item.SCPI.ToString(),
        //                SFYD = item.SFYD.ToString(),
        //                SAGI = item.SAGI.ToString(),
        //                SCFY = item.SCFY.ToString(),
        //                SCNA = item.SCNA.ToString(),
        //                SCNG = item.SCNG.ToString(),
        //                SLOG = item.SLOG.ToString(),
        //                SLOU = item.SLOU.ToString(),
        //                SUABP = item.SUABP.ToString(),
        //                SUACP = item.SUACP.ToString(),
        //                SUAFP = item.SUAFP.ToString(),
        //                SUAP = item.SUAP.ToString(),
        //                SCNY = item.SCNY.ToString(),
        //                SCF = item.SCF.ToString(),
        //                SCD = item.SCD.ToString(),
        //                SCET = item.SCET.ToString(),
        //                SCCOA = item.SCCOA.ToString(),
        //                SCAA = item.SCAA.ToString(),
        //                SBAA = item.SBAA.ToString(),
        //                SFAA = item.SFAA.ToString(),
        //                SCAAD = item.SCAAD.ToString(),
        //                SACTC = item.SACTC.ToString(),
        //                SCCC = item.SCCC.ToString(),
        //                SBCC = item.SBCC.ToString(),
        //                SFCC = item.SFCC.ToString(),
        //                SCMCC = item.SCMCC.ToString(),
        //                ICI = item.ICI.ToString(),
        //                ILOU = item.ILOU.ToString(),
        //                ISFP = item.ISFP.ToString(),
        //                IUP = item.IUP.ToString(),
        //                ILCOA = item.ILCOA.ToString(),
        //                SCCCA = item.SCCCA.ToString(),
        //                SUMCC = item.SUMCC.ToString(),
        //                SCAG = item.SCAG.ToString(),
        //                SCY = item.SCY.ToString(),
        //                SFUAP = item.SFUAP.ToString(),
        //                SBUAP = item.SBUAP.ToString()
        //            };
        //            return Json(favourites, JsonRequestBehavior.AllowGet);
        //        }
        //        else
        //        {
        //            return Json(JsonRequestBehavior.AllowGet);
        //        }
        //    }

        //}


    }
}