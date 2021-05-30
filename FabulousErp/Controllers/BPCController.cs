using FabulousDB.DB_Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FabulousDB.Models;
using FabulousDB.DB_Tabels.Transaction.Financial.Company.Accounting;
using FabulousDB.DB_Tabels.Settings.Financial.Accounting._Company.C_Account;
using System.Data;
using System.Data.Entity;
namespace FabulousErp.Controllers
{
    public class BPCController : Controller
    {
        DBContext db = new DBContext();

        // GET: BalanceSheet
        public ActionResult RowSettings(string RptType)
        {
            Report_type MyReport_type = (Report_type)Enum.Parse(typeof(Report_type), RptType);
            if (MyReport_type != Report_type.CashFlow)
            {
                ViewBag.Accounts_id = GetAccountsSelect(RptType);

            }
            else
            {
                ViewBag.Accounts_id = new SelectList(db.BPCRowsSettings.Where(x => x.Type == Account_row.Row &&
                 (x.Report_type == Report_type.BallanceSheet || x.Report_type == Report_type.PL)), "Id", "Row_name");
            }
            ViewBag.Rows_id = new SelectList(db.BPCRowsSettings.Where(x => x.Report_type == MyReport_type).ToList().OrderBy(x => x.Priority), "Id", "Row_name");
            return View();
        }

        private SelectList GetAccountsSelect(string RptType)
        {
            Report_type MyReport_type = (Report_type)Enum.Parse(typeof(Report_type), RptType);

            List<C_CreateAccount_Table> Accounts = GetAccounts(RptType);

            return new SelectList(Accounts
                .Select(x => new { C_AID = x.C_AID, AccountName = x.AccountName + " " + x.AccountID }).ToList(), "C_AID", "AccountName");
        }

        private List<C_CreateAccount_Table> GetAccounts(string RptType)
        {
            Report_type MyReport_type = (Report_type)Enum.Parse(typeof(Report_type), RptType);

            List<C_CreateAccount_Table> Accounts;
            List<int> UsedAccount = db.BPCRelation.Where(x => x.Account_id != null && x.Report_type == MyReport_type).Select(x => x.Account_id.Value).ToList();

            Accounts = db.C_CreateAccount_Tables.Where(x => !UsedAccount.Any(z => z == x.C_AID)).ToList();

            if (RptType == Report_type.BallanceSheet.ToString() ||
                RptType == Report_type.PL.ToString())
            {
                Accounts = Accounts.Where(x => x.PostingType == RptType && !UsedAccount.Any(z => z == x.C_AID)).ToList();
            }
            else if (RptType == Report_type.CashFlow.ToString())
            {
                Accounts = Accounts.Where(x => (x.PostingType == Report_type.BallanceSheet.ToString()
                || x.PostingType == Report_type.PL.ToString()) && !UsedAccount.Any(z => z == x.C_AID)).ToList();
            }

            return Accounts;
        }

        public ActionResult UpdateRows(string RptType)
        {
            Report_type MyReport_type = (Report_type)Enum.Parse(typeof(Report_type), RptType);
            ViewBag.RptType = RptType;

            List<int> UsedAccount = db.BPCRelation.Where(x => x.Account_id != null && x.Report_type == MyReport_type).Select(x => x.Account_id.Value).ToList();
            ViewBag.Rows_id2 = new SelectList(db.BPCRowsSettings.Where(x => x.Report_type == MyReport_type && x.Report_type == MyReport_type).OrderBy(x => x.Priority).ToList(), "Id", "Row_name");
            ViewBag.Accounts_id2 = GetAccountsSelect(RptType);

            //BPC_raw_settings ThisRow = db.BPCRowsSettings.Find(RowId);
            //List<BPC_Relation> ThisRelation = db.BPCRelation.Where(x => x.Balance_sheet_id == RowId).ToList();
            //EditBPC RowSettingRel = new EditBPC
            //{
            //    Relation = ThisRelation,
            //    Setting = ThisRow
            //};
            return View();
        }
        public ActionResult UpdateRow(int Rows_id2, List<int> Accounts_id2, string RptType, int? Priority)
        {
            BPC_raw_settings ThisRow = db.BPCRowsSettings.Find(Rows_id2);
            List<BPC_Relation> R = new List<BPC_Relation>();
            Report_type MyReport_type = (Report_type)Enum.Parse(typeof(Report_type), RptType);

            if (Accounts_id2 != null)
            {
                foreach (int Ac in Accounts_id2)
                {
                    R.Add(new BPC_Relation
                    {
                        Account_id = Ac,
                        Balance_sheet_id = Rows_id2,
                        Report_type = MyReport_type
                    });
                }

            }
            if (Priority.HasValue)
            {
                RedoPriority(Rows_id2, Priority, ThisRow, MyReport_type);
            }
            db.BPCRelation.AddRange(R);
            db.SaveChanges();
            return RowList(RptType);

        }

        private void RedoPriority(int Rows_id, int? Priority, BPC_raw_settings ThisRow, Report_type MyReport_type)
        {
            int Count = 1;

            //ThisRow.Priority = Priority.Value;
            List<BPC_raw_settings> ThisRep = db.BPCRowsSettings.Where(x => x.Report_type == MyReport_type).OrderBy(x => x.Priority).ToList();
            if (Priority> ThisRep.Count())
            {
                Priority = ThisRep.Count();
            }
            ThisRep.Remove(ThisRow);
            ThisRep.Insert(Priority.Value-1, ThisRow);
            for (int i = 0; i < ThisRep.Count(); i++)
            {
                ThisRep.ToArray()[i].Priority = i+1;
            }

            db.SaveChanges();
            //db.BPCRowsSettings.Where(x => x.Report_type == MyReport_type && x.Priority >= Priority.Value && x.Id != Rows_id).OrderBy(x => x.Priority).ToList().ForEach(x =>
            //{
            //    x.Priority = Priority.Value + Count;
            //    Count++;
            //});
            //int CurrentPrio = db.BPCRowsSettings.Find(Rows_id).Priority;
            //Count = 0;

            //db.BPCRowsSettings.Where(x => x.Report_type == MyReport_type && x.Priority > CurrentPrio && x.Id != Rows_id).OrderBy(x => x.Priority).ToList().ForEach(x =>
            //{
            //    x.Priority = CurrentPrio + Count;
            //    Count++;
            //});
            //ThisRow.Priority = Priority.Value;
        }

        public ActionResult CreateRow(BPC_raw_settings Row, List<int> Accounts_id, List<int> Rows_id, string RptType)
        {
            Row.Report_type = (Report_type)Enum.Parse(typeof(Report_type), RptType);
            if (Row.Report_type== Report_type.CashFlow)
            {
                if (Accounts_id!=null && Rows_id == null)
                {
                    Rows_id = Accounts_id;
                    Accounts_id = null;
                }
                
            }
            if (db.BPCRowsSettings.Where(x => x.Report_type == Row.Report_type).Any())
            {
                Row.Priority = db.BPCRowsSettings.Where(x => x.Report_type == Row.Report_type).Max(x => x.Priority) + 1;
            }
            else
            {
                Row.Priority = 1;
            }
            db.BPCRowsSettings.Add(Row);
            db.SaveChanges();
            List<BPC_Relation> R = new List<BPC_Relation>();
            if (Accounts_id != null)
            {
                foreach (int Ac in Accounts_id)
                {
                    R.Add(new BPC_Relation
                    {
                        Account_id = Ac,
                        Balance_sheet_id = Row.Id,
                        Report_type = Row.Report_type
                    });
                }

            }
            else if (Rows_id != null)
            {
                foreach (int RId in Rows_id)
                {
                    R.Add(new BPC_Relation
                    {
                        Row_id = RId,
                        Balance_sheet_id = Row.Id,
                        Report_type = Row.Report_type
                    });
                }
            }
            else
            {
                R.Add(new BPC_Relation
                {
                    Balance_sheet_id = Row.Id,
                    Report_type = Row.Report_type
                });
            }
            db.BPCRelation.AddRange(R);
            db.SaveChanges();
            return RowList(RptType);
        }
        public ActionResult RowList(string RptType)
        {
            Report_type Report_type = (Report_type)Enum.Parse(typeof(Report_type), RptType);
            ViewBag.RptType = RptType;
            //ViewBag.Relation = db.BPCRelation.ToList();
            return PartialView("RowList", db.BPCRelation
                .Include("BalanceSheet").Include("Account").Include("Row")
                .Where(x => x.Report_type == Report_type).OrderBy(x => x.BalanceSheet.Priority).ToList());
        }
        [HttpPost]
        public JsonResult RowId(string RptType)
        {
            Report_type MyReport_type = (Report_type)Enum.Parse(typeof(Report_type), RptType);
            //if (RptType == Report_type.CashFlow.ToString())
            //{
            //    MyReport_type = Report_type.BallanceSheet;
            //}
            return Json(db.BPCRowsSettings.Where(x => x.Report_type == MyReport_type).OrderBy(x => x.Priority).ToList()
                .Select(x => new { x.Id, x.Row_name }).ToList());
        }
        [HttpPost]
        public JsonResult AccountsId(string RptType)
        {
            return Json(GetAccounts(RptType).Select(x => new { Id = x.C_AID, Name = x.AccountName + " " + x.AccountID }));
        }
        public ActionResult BalanceSheetReport()
        {
            Report_type ThisType = (Report_type)Enum.Parse(typeof(Report_type), Request["RptType"]);
            if (ThisType == Report_type.BallanceSheet)
            {
                ViewBag.Title = "Balance Sheet Report";
            }
            else if (ThisType == Report_type.PL)
            {
                ViewBag.Title = "PL Report";
            }
            else if (ThisType == Report_type.CashFlow)
            {
                ViewBag.Title = "Cash Flow Report";
            }
            ViewBag.Year = new SelectList(db.NewFiscalYear_Table.ToList().Where(x=>x.Fiscal_Year_ID== Business.GetCompanyId())
                .Select(x =>new { x.Year }).OrderByDescending(x=>x.Year), "Year", "Year");
            return View();
        }
        public ActionResult GetBalanceSheetReport(int Year, int? CompareYear, string RptType)
        {
            Report_type MyReport_type = (Report_type)Enum.Parse(typeof(Report_type), RptType);
            ViewBag.RptType = MyReport_type;
            List<BPC_report> Rpt = new List<BPC_report>();
            if (MyReport_type != Report_type.CashFlow)
            {
                var asd = db.BPCRelation.Include("Account").Include("BalanceSheet").Where(x => x.Account_id != null && x.Report_type == MyReport_type).ToList();
                foreach (BPC_Relation R in db.BPCRelation.Include("Account").Include("BalanceSheet").Where(x => x.Account_id != null && x.Report_type == MyReport_type).ToList())
                {
                    Rpt.Add(new BPC_report
                    {
                        OrderId = R.BalanceSheet.Priority,
                        Balance_sheet_id = R.Balance_sheet_id,
                        Raw_name = R.BalanceSheet.Row_name,
                        Account_id = R.Account.AccountID,
                        Account_name = R.Account.AccountName,
                        Current_year = CalcBalance(Year, R),
                        Last_year = CalcBalance(Year - 1, R),
                        Last_2_year = CalcBalance(Year - 2, R),
                        Last_3_year = CalcBalance(Year - 3, R),
                        Last_4_year = CalcBalance(Year - 4, R),
                        CompareYear = CalcBalance(CompareYear, R),
                        Raw_id = R.Row_id
                    });
                }
                foreach (IGrouping<int, BPC_Relation> R in db.BPCRelation.Include("BalanceSheet").Where(x => (x.Row_id != null||(x.Row_id==null&&x.Account_id==null)) && x.Report_type == MyReport_type).ToList().GroupBy(x => x.Balance_sheet_id).ToList())
                {
                    Rpt.Add(new BPC_report
                    {
                        Raw_id = R.FirstOrDefault().Row_id,
                        Balance_sheet_id = R.Key,

                        OrderId = R.FirstOrDefault().BalanceSheet.Priority,
                        Raw_name = db.BPCRowsSettings.ToList().Where(x => x.Id == R.FirstOrDefault().Balance_sheet_id).ToList().DefaultIfEmpty(new BPC_raw_settings { }).FirstOrDefault().Row_name,
                        CompareYear = Rpt.Where(x => R.ToList().Any(z => z.Row_id == x.Balance_sheet_id)).Sum(x => x.CompareYear),
                        Current_year = Rpt.Where(x => R.ToList().Any(z => z.Row_id == x.Balance_sheet_id)).Sum(x => x.Current_year),
                        Last_year = Rpt.Where(x => R.ToList().Any(z => z.Row_id == x.Balance_sheet_id)).Sum(x => x.Last_year),
                        Last_2_year = Rpt.Where(x => R.ToList().Any(z => z.Row_id == x.Balance_sheet_id)).Sum(x => x.Last_2_year),
                        Last_3_year = Rpt.Where(x => R.ToList().Any(z => z.Row_id == x.Balance_sheet_id)).Sum(x => x.Last_3_year),
                        Last_4_year = Rpt.Where(x => R.ToList().Any(z => z.Row_id == x.Balance_sheet_id)).Sum(x => x.Last_4_year)
                    });
                }
            }
            else
            {
                foreach (BPC_Relation R in db.BPCRelation.Include(x=>x.Row).Include(x=>x.Account).Include("BalanceSheet").Where(x =>x.Row_id != null&& db.BPCRowsSettings.FirstOrDefault(z=>z.Id==x.Balance_sheet_id).Type==Account_row.Account &&  x.Report_type == MyReport_type).ToList())
                {
                    List<int?> AccountId =db.BPCRelation.Where(y=> db.BPCRelation.Where(x => x.Balance_sheet_id == R.Row_id).Select(x=>x.Row_id).ToList().Contains(y.Balance_sheet_id)).Select(x=>x.Account_id).ToList();

                    foreach (int AId in AccountId)
                    {
                        BPC_report BPCRep = new BPC_report
                        {
                            OrderId = R.BalanceSheet.Priority,
                            Balance_sheet_id = R.Balance_sheet_id,
                            Raw_name = R.BalanceSheet.Row_name,
                            Account_id = db.C_CreateAccount_Tables.FirstOrDefault(x => x.C_AID == AId).AccountID,
                            Account_name = R.Row.Row_name,
                            Raw_id = R.Row_id
                        };
                        R.Account_id = AId;
                        if (R.BalanceSheet.Minus == Minus.Current_Minus_Last)
                        {
                            if (R.Row.Report_type == Report_type.BallanceSheet)
                            {
                                R.Report_type = Report_type.BallanceSheet;
                                BPCRep.Current_year = CalcBalance(Year, R) - CalcBalance(Year - 1, R);
                            }
                            else if (R.Row.Report_type == Report_type.PL)
                            {
                                R.Report_type = Report_type.PL;

                                BPCRep.Current_year = CalcPL(Year, R) - CalcPL(Year - 1, R);
                            }
                        }
                        else if (R.BalanceSheet.Minus == Minus.Last_Minus_Current)
                        {
                            if (R.Row.Report_type == Report_type.BallanceSheet)
                            {
                                R.Report_type = Report_type.BallanceSheet;

                                BPCRep.Current_year = CalcBalance(Year - 1, R) - CalcBalance(Year, R);
                            }
                            else if (R.Row.Report_type == Report_type.PL)
                            {
                                R.Report_type = Report_type.PL;

                                BPCRep.Current_year = CalcPL(Year - 1, R) - CalcPL(Year, R);

                            }
                        }
                        Rpt.Add(BPCRep);
                    }
                 
                }

                foreach (IGrouping<int, BPC_Relation> R in db.BPCRelation.Include("BalanceSheet").Where(x => x.Row_id != null && db.BPCRowsSettings.FirstOrDefault(z => z.Id == x.Balance_sheet_id).Type == Account_row.Row && x.Report_type == MyReport_type).ToList().GroupBy(x => x.Balance_sheet_id).ToList())
                {
                    BPC_report BPCRep = new BPC_report
                    {
                        Raw_id = R.FirstOrDefault().Row_id,
                        Balance_sheet_id = R.Key,
                        Current_year = Rpt.Where(x => R.ToList().Any(z => z.Row_id == x.Balance_sheet_id)).Sum(x => x.Current_year),
                        OrderId = R.FirstOrDefault().BalanceSheet.Priority,
                        Raw_name = db.BPCRowsSettings.ToList().Where(x => x.Id == R.FirstOrDefault().Balance_sheet_id).ToList().DefaultIfEmpty(new BPC_raw_settings { }).FirstOrDefault().Row_name,
                    };
                    Rpt.Add(BPCRep);
                }

            }
            //foreach (BPC_Relation R in db.BPCRelation.Include(x=>x.Row).Include("Account").Include("BalanceSheet").Where(x =>  x.Row_id != null && db.BPCRowsSettings.FirstOrDefault(z => z.Id == x.Balance_sheet_id).Type == Account_row.Row && x.Report_type == MyReport_type).ToList())
            //{
            //    Rpt.Add(new BPC_report
            //    {
            //        Raw_name = R.BalanceSheet.Row_name,
            //        OrderId = R.BalanceSheet.Priority,
            //        Raw_id = R.Row_id
            //    });
            //}
            ViewBag.CompareYear = CompareYear;
            ViewBag.Year = Year;

            //Rpt.Where(x => x.Current_year < 0).ToList().ForEach(x => x.Current_year = -x.Current_year);
            //Rpt.Where(x => x.Last_year < 0).ToList().ForEach(x => x.Last_year = -x.Last_year);
            //Rpt.Where(x => x.Last_2_year < 0).ToList().ForEach(x => x.Last_2_year = -x.Last_2_year);
            //Rpt.Where(x => x.Last_3_year < 0).ToList().ForEach(x => x.Last_3_year = -x.Last_3_year);
            //Rpt.Where(x => x.Last_4_year < 0).ToList().ForEach(x => x.Last_4_year = -x.Last_4_year);
            return View(Rpt);
        }

        private double CalcBalance(int? Year, BPC_Relation R)
        {
            try
            {
                if (Year.HasValue)
                {
                    if (R.Report_type == Report_type.BallanceSheet)
                    {
                        return CalcBalanceSheet(Year, R);
                    }
                    else if (R.Report_type == Report_type.PL)
                    {
                        return CalcPL(Year, R);
                    }
                    else if (R.Report_type == Report_type.CashFlow)
                    {

                        if (R.Account.PostingType == Report_type.BallanceSheet.ToString())
                        {
                            return CalcBalanceSheet(Year, R);

                        }
                        else if (R.Account.PostingType == Report_type.PL.ToString())
                        {
                            return CalcPL(Year, R);
                        }
                        else
                        {
                            return 0;
                        }
                    }
                    return 0;
                }
                else
                {
                    return 0;
                }

            }
            catch (Exception ex)
            {
                return 0;
            }

        }

        private double CalcPL(int? Year, BPC_Relation R)
        {
            List<C_GeneralLedger_Table> CGT = db.C_GeneralJournalEntry_Tables.Where(x => x.C_PostingKey != "SCY" && x.C_GeneralLedger_Tables.Any(z => z.C_AID == R.Account_id)).ToList()
               .Where(x => Convert.ToDateTime(x.C_PostingDate).Year == Year).SelectMany(x => x.C_GeneralLedger_Tables).Where(x => x.C_AID == R.Account_id).ToList();
            return CGT.Sum(x => x.Ballance);
        }

        private double CalcBalanceSheet(int? Year, BPC_Relation R)
        {
            return db.C_EndingBeginingYears.Include(x=>x.NewFiscalYear_Table).ToList().Where(x => x.C_AID == R.Account_id &&
                        x.NewFiscalYear_Table.Year == Year.ToString() && x.Type == 1)
                       .Sum(x => x.Ballance + ((x.Adjustment.HasValue) ? x.Adjustment.Value : 0));
        }

        public ActionResult DeleteSettingRow(int Id, string RptType)
        {
            BPC_Relation ThisRel = db.BPCRelation.Find(Id);
            Report_type MyRptType = (Report_type)Enum.Parse(typeof(Report_type), RptType);
            db.BPCRelation.Remove(ThisRel);
            db.SaveChanges();
            int Priory = 0;
            if (!db.BPCRelation.Any(x => x.Balance_sheet_id == ThisRel.Balance_sheet_id))
            {
                BPC_raw_settings BPC = db.BPCRowsSettings.Find(ThisRel.Balance_sheet_id);
                Priory = BPC.Priority;

                RedoPriority(ThisRel.Balance_sheet_id, Priory, BPC, MyRptType);
                db.BPCRelation.RemoveRange(db.BPCRelation.Where(x => x.Balance_sheet_id == ThisRel.Balance_sheet_id).ToList());
                db.BPCRowsSettings.Remove(BPC);
                db.SaveChanges();
            }
            return RowList(RptType);

        }
    }
    public class BPC_report
    {
        public int OrderId { get; set; }
        public int Balance_sheet_id { get; set; }
        public int? Raw_id { get; set; }

        public string Raw_name { get; set; }
        public string Account_name { get; set; }
        public string Account_id { get; set; }

        public double Current_year { get; set; }
        public double Last_year { get; set; }
        public double Last_2_year { get; set; }
        public double Last_3_year { get; set; }
        public double Last_4_year { get; set; }
        public double CompareYear { get; set; }

    }
    public class EditBPC
    {
        public BPC_raw_settings Setting { get; set; }
        public List<BPC_Relation> Relation { get; set; }
    }
    public static class Array2
    {
        public static void SwapValues<T>(this T[] source, long index1, long index2)
        {
            T temp = source[index1];
            source[index1] = source[index2];
            source[index2] = temp;
        }
    }
    
}