using FabulousDB.DB_Context;
using FabulousDB.DB_Tabels.Settings.Financial.Accounting._Company.C_CheckBook;
using FabulousDB.DB_Tabels.Transaction.Financial.Company.Accounting;
using FabulousErp.Controllers.Transaction.Financial.Company.Checkbook;
using FabulousModels.DTOModels.Transaction.Financial;
using FabulousModels.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FabulousErp.Controllers
{
    public class SysReportsController : Controller
    {
        DBContext db = new DBContext();
        // GET: Reports
        public ActionResult Reconcile(int BankRecnocileNumber, List<RecnocileData> CBData)
        {
            string companyID = Business.GetCompanyId();
            List<ReconcileRpt> Rpt = new List<ReconcileRpt>();
            C_BankReconcile_table ReconcileList = db.C_BankReconcile_Tables
              .FirstOrDefault(x => x.CompanyID == companyID && x.BankReconcile_Number == BankRecnocileNumber);

            C_CheckBookSetting_table CB = ReconcileList.C_CheckBookSetting_Table;

            Rpt.Add(new ReconcileRpt
            {
                Head = new ReconcileHeader
                {
                    Title = $"رصيد البنك من كشف الحساب في {ReconcileList.Bank_Statment_Ending_Date}",
                    Total= ReconcileList.Bank_Statment_Ending_Balance
                }
            });

            //using (BankReconcileController R = new BankReconcileController())
            //{
            //    CBData.AddRange(R.RecnocileTransaction(ReconcileList.C_CBSID.Value,
            //        ReconcileList.Book_Statment_Ending_Date,3));
            //}
            Rpt.Add(new ReconcileRpt
            {
                Head = new ReconcileHeader
                {
                    Title = $"يضاف مبالغ لم تظهر بكشف الحساب",
                    Total = CBData.Where(x => x.Deposite == 0).Sum(x=>x.Payment),
                    IsDeposit=false
                },
                Data = CBData.Where(x => x.Deposite == 0).ToList()
            });
           
            Rpt.Add(new ReconcileRpt
            {
                Head = new ReconcileHeader
                {
                    Title = $"يخصم مبالغ لم تظهر بالدفتر",
                    Total = CBData.Where(x => x.Payment == 0).Sum(x => x.Deposite),
                    IsDeposit = true

                },
                Data = CBData.Where(x => x.Payment == 0).ToList()
            });

            Rpt.Add(new ReconcileRpt
            {
                Head = new ReconcileHeader
                {
                    Title = $"الرصيد الدفتري في {ReconcileList.Bank_Statment_Ending_Date}",
                    Total = ReconcileList.Bank_Statment_Ending_Balance - CBData.Where(x => x.Payment == 0).Sum(x => x.Deposite) + CBData.Where(x => x.Deposite == 0).Sum(x => x.Payment)
                }
            });
            ViewBag.BankCode = CB.C_CheckbookID;
            ViewBag.BankName = CB.C_CheckbookName;
            return View(Rpt);
        }


    }
  
}