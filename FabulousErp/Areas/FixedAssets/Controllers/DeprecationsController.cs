using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FabulousDB.DB_Context;
using FabulousDB.DB_Tabels.Settings.Financial.GeneralSetting.FiscalPeriods;
using FabulousDB.DB_Tabels.Transaction.Financial.Company.Accounting;
using FabulousDB.Models;
using FabulousErp;
using FabulousErp.Controllers.API;
using static FixedAssets.Business.Business;

namespace FixedAssets.Controllers
{
    public class DeprecationsController : Controller
    {
        private DBContext db = new DBContext();
        private DBContext DB = new DBContext();

        // GET: Deprecations
        public ActionResult Index()
        {
            var deprecations = db.Deprecations.Include(d => d.Deprecation_periods);
            return View(deprecations.ToList());
        }

        // GET: Deprecations/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Deprecation deprecation = db.Deprecations.Find(id);
            if (deprecation == null)
            {
                return HttpNotFound();
            }
            return View(deprecation);
        }
        public JsonResult GetNewPeriodMonth(DateTime TransactionDate)
        {
            ViewBag.Type = db.Deprecation_Setting.FirstOrDefault().Deprecation_calcualtion;

            try
            {
                ViewBag.Type = db.Deprecation_Setting.FirstOrDefault().Deprecation_calcualtion;


                if (ViewBag.Type == (int)Deprecation_calcualtion.Periodic)
                {
                    List<int?> DonePeriod = db.Deprecations.Select(x => x.Period_id).ToList();
                    return Json(DB.FiscalYear_Tables.Where(x => !DonePeriod.Any(z => z.Value == x.ID))
                                 .ToList().Where(x => Convert.ToDateTime(x.Period_Start_Date).Year == TransactionDate.Year).Select(x=>new {value=x.ID,text=x.Period_No,Month=Convert.ToDateTime(x.Period_Start_Date).Month }));
                }
                else if (ViewBag.Type == (int)Deprecation_calcualtion.Monthly)
                {
                    List<int> Months = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
                    db.Deprecations.Where(x => x.Deprecation_date.Year == TransactionDate.Year && x.Month != null && x.Month != 0).Select(x => x.Month).ToList()
                        .ForEach(x => Months.Remove(x.Value)); ;

                    return Json(Months.Select(x=>new {value=x,text=x }));
                }
                else
                {
                    return Json(null);
                }
               
            }
            catch
            {
                return Json(null);
            }
        }
        // GET: Deprecations/Create
        public ActionResult Create()
        {
            if (!db.Deprecation_Setting.Any())
            {
                return Redirect("/FixedAssets/Deprecation_Setting");
            }
            List<int?> DonePeriod = db.Deprecations.Select(x => x.Period_id).ToList();
            ViewBag.Type = db.Deprecation_Setting.FirstOrDefault().Deprecation_calcualtion;
            if (ViewBag.Type == (int)Deprecation_calcualtion.Periodic)
            {
                //ReturnHERE 
                string AreaName = FabulousErp.Business.GetCurrentAreaName();
                ViewBag.Period_id = new SelectList(DB.FiscalYear_Tables.Include(x=>x.Fiscal_year_area).Where(x =>x.Fiscal_year_area.Any(z=>z.Area_name== AreaName&&z.Allowed==true) && !DonePeriod.Any(z => z.Value == x.ID))
          .ToList().Where(x => Convert.ToDateTime(x.Period_Start_Date).Year == DateTime.Now.Year), "ID", "Period_No");

            }
            else if (ViewBag.Type == (int)Deprecation_calcualtion.Monthly)
            {
                List<int> Months = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
                db.Deprecations.Where(x => x.Deprecation_date.Year == DateTime.Now.Year && x.Month != null && x.Month != 0).Select(x => x.Month).ToList()
                    .ForEach(x => Months.Remove(x.Value)); ;

                ViewBag.Month = new SelectList(Months);
            }
            string CompId = FabulousErp.Business.GetCompanyId();
            ViewBag.Year = new SelectList(DB.NewFiscalYear_Table.Where(x => x.Fiscal_Year_ID == CompId&&x.Closed!=true)
                .ToList(), "Year", "Year");
            return View();
        }
        // POST: Deprecations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create(Deprecation_temp deprecation, List<Number_of_deprecation_units> NumberOfUnits, int DepreicationType
            , DateTime Transaction_date, DateTime Deprecation_date
            , int Period_id, int MonthNumber, List<int> Assets_class = null, List<int> Assets = null
            , bool IsTemp=true,int Year=1)
        {
            if (!db.Deprecation_Setting.Any())
            {
                return Redirect("/FixedAssets/Deprecation_Setting");
            }

            string DeprectionNote = "";
            deprecation.Deprecation_no = db.Deprecations.Max(x => x.Deprecation_no) + 1;
            if (deprecation.Deprecation_no == null)
            {
                deprecation.Deprecation_no = 1;
            }

            List<Asset> MyAssets = new List<Asset>();
            List<Fixed_assets_renewal> MyRenwal = new List<Fixed_assets_renewal>();
            if (DepreicationType == 2)
            {
                if (Assets != null)
                {
                    MyAssets.AddRange(db.Assets.ToList().Where(x => Assets.Contains(x.Id)));
                }
                if (Assets_class != null)
                {
                    MyAssets.AddRange(db.Assets.Where(x => Assets_class.Contains(x.Assets_class_id)));
                }
                MyRenwal.AddRange(db.Fixed_assets_renewal.ToList().Where(x => MyAssets.All(z => z.Id == x.Assets_id)));

            }
            else
            {
                MyAssets = db.Assets.ToList();
                // MyRenwal = db.Fixed_assets_renewal.ToList();
            }
            int MainPeriod_days = 0;

            int Type = db.Deprecation_Setting.FirstOrDefault().Deprecation_calcualtion;

            //ReturnHERE 
            string AreaName = FabulousErp.Business.GetCurrentAreaName();

            FiscalYear_Table Depreication_period = DB.FiscalYear_Tables.Include(x=>x.Fiscal_year_area).ToList().Where(x => Convert.ToDateTime(x.Period_End_Date).Year == Transaction_date.Year&&x.Fiscal_year_area.Any(z=>z.Area_name==AreaName && z.Allowed==true)
            &&Convert.ToDateTime(x.Period_End_Date)<=Transaction_date).FirstOrDefault();

            if (Type == (int)Deprecation_calcualtion.Periodic)
            {
                MyAssets = MyAssets.Where(x => x.Start_derecation_date <= Convert.ToDateTime(Depreication_period.Period_End_Date)).ToList();

                //MyAssets = MyAssets.Where(x => x.Start_derecation_date.Year <= Convert.ToDateTime(Depreication_period.Period_End_Date).Year
                //                               && x.Start_derecation_date.Month <= Convert.ToDateTime(Depreication_period.Period_End_Date).Month
                //                               && x.Start_derecation_date.Day <= Convert.ToDateTime(Depreication_period.Period_End_Date).Day).ToList();

                MainPeriod_days = Convert.ToInt32(Convert.ToDateTime(Depreication_period.Period_End_Date).Subtract(Convert.ToDateTime(Depreication_period.Period_Start_Date)).TotalDays) + 1;
                deprecation.Period_id = Period_id;
                DeprectionNote = $" For Period {Period_id}";
                
            }
            else if (Type == (int)Deprecation_calcualtion.Monthly)
            {
                MyAssets = MyAssets.Where(x => x.Start_derecation_date <= Convert.ToDateTime(deprecation.Deprecation_date)).ToList();
                deprecation.Month = MonthNumber;
                deprecation.Year = Transaction_date.Year;
                DeprectionNote = $" For Month Number {MonthNumber}";

            }
            else
            {
                MyAssets = MyAssets.Where(x => x.Start_derecation_date.Year == Year).ToList();
                deprecation.Year = Transaction_date.Year;
                DeprectionNote = $" For Year {Year}";

            }
            MyAssets = MyAssets.Where(x => x.Disposal == true || !x.Fixed_assets_disposel.Any()).ToList();

            
                db.Deprecation_temp.Add(deprecation);

            List<Deprecation_temp_record> ThisDepreciation = new List<Deprecation_temp_record>();
            foreach (Asset asset in MyAssets.Where(x=>x.Deactive_depraction==false))
            {
                Deprecation_method Deprecation_method;
                decimal Scrap_value = 0;
                decimal Renwal_amount = 0;
                int DepreicatedUnits = 0;
                bool Include_scrap_value = true;
                List<Fixed_assets_renewal> fixed_assets_renewal = db.Fixed_assets_renewal.Where(x => x.Assets_id == asset.Id).ToList();

                if (Type == (int)Deprecation_calcualtion.Periodic)
                {
                    fixed_assets_renewal = fixed_assets_renewal.Where(x => x.Renwal_date <= Convert.ToDateTime(Depreication_period.Period_End_Date)).ToList();//Period_Start_Date
                }
                else if (Type == (int)Deprecation_calcualtion.Monthly)
                {
                    fixed_assets_renewal = fixed_assets_renewal.Where(x => x.Renwal_date.Value.Year == DateTime.Now.Year &&
                    (x.Renwal_date.Value.Month <= MonthNumber)).ToList();
                }
                else
                {
                    fixed_assets_renewal = fixed_assets_renewal.Where(x => x.Renwal_date.Value.Year == Year).ToList();
                }
                if (fixed_assets_renewal != null)
                {
                    if (fixed_assets_renewal.Any())
                    {
                        Renwal_amount = fixed_assets_renewal.Sum(x => x.Renewal_amount).Value;

                    }
                }
                if (asset.Include_scerap_value == false)
                {
                    if (asset.Scrap_value.HasValue)
                    {
                        Scrap_value = asset.Scrap_value.Value;
                    }

                }
                if (NumberOfUnits != null && NumberOfUnits.Any(x => x.Asset_id == asset.Id))
                {
                    DepreicatedUnits = NumberOfUnits.FirstOrDefault(x => x.Asset_id == asset.Id).Deprecation_unit;
                }
                Include_scrap_value = asset.Include_scerap_value;
                int Period_days = MainPeriod_days;
                try
                {
                    asset.Acquisation_cost = asset.Fixed_assets_revaluate.ToList().DefaultIfEmpty(new Fixed_assets_revaluate { Adjustment_cost = asset.Acquisation_cost }).ToList().LastOrDefault().Adjustment_cost.Value;
                    if (Type == (int)Deprecation_calcualtion.Periodic)
                    {
                        try
                        {
                            if (Convert.ToDateTime(Depreication_period.Period_End_Date).Month ==
                                asset.Start_derecation_date.Month)
                            {
                                Period_days = MainPeriod_days - asset.Start_derecation_date.Day + 1;
                            }
                            else
                            {

                            }

                        }
                        catch
                        {

                        }
                    }
                    ThisDepreciation.Add(CalcDeprecationByMethod(asset, Period_days, fixed_assets_renewal, deprecation.Transaction_date.Value
                  , 0, Period_id, MonthNumber, DepreicatedUnits, Deprecation_date,Year));
                }
                catch
                {

                }

            }
            ThisDepreciation.ForEach(x => x.Deprecation_id = deprecation.Id);
            db.Deprecation_temp_record.AddRange(ThisDepreciation);
            db.SaveChanges();
            
            db.SaveChanges();
            return Json(deprecation.Id);


            ViewBag.Period = new SelectList(db.Deprecation_periods, "Id", "text", deprecation.Period);
            return View(deprecation);
        }
        private Deprecation_temp_record CalcDeprecationByMethod(Asset Asset, int Period_days,
            List<Fixed_assets_renewal> Renwal
            , DateTime Depreication_date, decimal Disposal
            , int Period_id, int MonthNumber, int Deprecation_unit, DateTime Deprecation_date,int Year)
        {
            decimal UseLife = (Asset.Use_life.AddYears(-Asset.Start_use.Year)).Year;
            decimal UseLifeDays = Convert.ToDecimal((Asset.Use_life.Subtract(Asset.Start_use)).TotalDays);


            decimal Net_deprecation = 0;

            if (Asset.Number_of_units.HasValue)
            {
                if (Asset.Number_of_units.Value == 0)
                {
                    Asset.Number_of_units = 1;
                }
            }
            else
            {
                Asset.Number_of_units = 0;
            }
            if (!Asset.Scrap_value.HasValue)
            {
                Asset.Scrap_value = 0;
            }

            Deprecation_method Deprecation_method = (Deprecation_method)Asset.Deprecation_method;
            decimal Old_deprication = 0;
            int CalcualtionType = db.Deprecation_Setting.FirstOrDefault().Deprecation_calcualtion;
            decimal SumOfThisARenwal = 0;
            decimal Renewal_depreication = 0;
            decimal DaysBeforeThatRenwal = 0;
            decimal ThisYearDays = (decimal)new DateTime(Year, 12, 31).Subtract(new DateTime(Year, 1, 1)).TotalDays+1;

            if (Renwal != null)
            {
                foreach (Fixed_assets_renewal R in Renwal)
                {
                    if (!R.Renewal_amount.HasValue)
                    {
                        R.Renewal_amount = 0;
                    }
                    DaysBeforeThatRenwal = (decimal)(R.Renwal_date.Value.Subtract(Asset.Start_use).TotalDays);

                    if (Deprecation_method == Deprecation_method.Fixed)
                    {
                        if (CalcualtionType == (int)Deprecation_calcualtion.Yearly)
                        {
                            decimal AmountOfUsageBeforeThisRenwal = R.Renewal_amount.Value / (UseLifeDays - DaysBeforeThatRenwal);
                            SumOfThisARenwal += (decimal)AmountOfUsageBeforeThisRenwal * (decimal)new DateTime(Year, 12, 31).Subtract(R.Renwal_date.Value).TotalDays;
                        }
                        else
                        {
                            SumOfThisARenwal += R.Renewal_amount.Value / (UseLifeDays - DaysBeforeThatRenwal);

                        }
                    }
                    else if (Deprecation_method == Deprecation_method.Decreased)
                    {
                        SumOfThisARenwal += R.Renewal_amount.Value;
                    }
                    else if (Deprecation_method == Deprecation_method.Number_of_units)
                    {
                        SumOfThisARenwal += R.Renewal_amount.Value;
                    }


                }
            }
            if (db.Deprecation_record.Any(x => x.Asset_id == Asset.Id))
            {
                Old_deprication = CalculateOldDeprication(Asset.Id, Depreication_date);
            }

            if (CalcualtionType == (int)Deprecation_calcualtion.Periodic)
            {
                if (Deprecation_method == Deprecation_method.Fixed)
                {
                    Renewal_depreication = SumOfThisARenwal * Period_days;

                    Net_deprecation = (((Asset.Acquisation_cost - Asset.Scrap_value.Value) / UseLife) / 365) * Period_days;
                }
                else if (Deprecation_method == Deprecation_method.Decreased)
                {
                    Net_deprecation = (((Asset.Acquisation_cost - Asset.Scrap_value.Value - Old_deprication) * (Asset.Deprecation_rate / 100)) / 365) * Period_days;
                    Renewal_depreication = (((((Asset.Acquisation_cost + SumOfThisARenwal) - Asset.Scrap_value.Value - Old_deprication) * (Asset.Deprecation_rate / 100)) / 365) * Period_days) - Net_deprecation;

                }
                else if (Deprecation_method == Deprecation_method.Number_of_units)
                {
                    Net_deprecation = (((Asset.Acquisation_cost - Asset.Scrap_value.Value) / Asset.Number_of_units.Value) * (Deprecation_unit));
                    Renewal_depreication = ((((Asset.Acquisation_cost + SumOfThisARenwal) / Asset.Number_of_units.Value) * (Deprecation_unit))) - Net_deprecation;
                }
            }
            else if (CalcualtionType == (int)Deprecation_calcualtion.Monthly)
            {
                string companyID = FabulousErp.Business.GetCompanyId();
                decimal DaysInMonth = DateTime.DaysInMonth(Year, MonthNumber);

                if (MonthNumber== Asset.Start_derecation_date.Month)
                {
                    DaysInMonth = DaysInMonth - Asset.Start_derecation_date.Day+1;
                }


                if (Deprecation_method == Deprecation_method.Fixed)
                {
                    Renewal_depreication = SumOfThisARenwal * DaysInMonth;

                    Net_deprecation = (((Asset.Acquisation_cost - Asset.Scrap_value.Value) / UseLife) / 365) * DaysInMonth;
                }
                else if (Deprecation_method == Deprecation_method.Decreased)
                {
                    Net_deprecation = (((Asset.Acquisation_cost - Asset.Scrap_value.Value - Old_deprication) * (Asset.Deprecation_rate / 100)) / 365) * DaysInMonth;
                    Renewal_depreication = (((((Asset.Acquisation_cost + SumOfThisARenwal) - Asset.Scrap_value.Value - Old_deprication) * (Asset.Deprecation_rate / 100)) / 365) * DaysInMonth) - Net_deprecation;

                }
                else if (Deprecation_method == Deprecation_method.Number_of_units)
                {
                    Net_deprecation = (((Asset.Acquisation_cost - Asset.Scrap_value.Value) / Asset.Number_of_units.Value) * (Deprecation_unit));
                    Renewal_depreication = ((((Asset.Acquisation_cost + SumOfThisARenwal) / Asset.Number_of_units.Value) * (Deprecation_unit))) - Net_deprecation;

                }
            }
            else
            {
                
                if (Asset.Start_derecation_date.Year== Year)
                {
                    ThisYearDays = ThisYearDays - (decimal)Asset.Start_derecation_date.Subtract(new DateTime(Year, 1, 1)).TotalDays + 1;
                }

                if (Deprecation_method == Deprecation_method.Fixed)
                {
                    Net_deprecation = (((Asset.Acquisation_cost - Asset.Scrap_value.Value) / UseLife) / 365) * ThisYearDays;
                    //Renewal_depreication Calculated In The Renwal ForEach

                }
                else if (Deprecation_method == Deprecation_method.Decreased)
                {
                    Net_deprecation = (((Asset.Acquisation_cost - Asset.Scrap_value.Value - Old_deprication) * (Asset.Deprecation_rate / 100)) / 365) * ThisYearDays;
                    Renewal_depreication = (((((Asset.Acquisation_cost + SumOfThisARenwal) - Asset.Scrap_value.Value - Old_deprication) * (Asset.Deprecation_rate / 100)) / 365) * ThisYearDays) - Net_deprecation;

                }
                else if (Deprecation_method == Deprecation_method.Number_of_units)
                {
                    Net_deprecation = (((Asset.Acquisation_cost - Asset.Scrap_value.Value) / Asset.Number_of_units.Value) * (Deprecation_unit));
                    Renewal_depreication = ((((Asset.Acquisation_cost + SumOfThisARenwal) / Asset.Number_of_units.Value) * (Deprecation_unit))) - Net_deprecation;

                }
            }














            decimal Deprecation = Net_deprecation; //(Acquisation_cost - Scrap_value) * Deprecation_rate;

            decimal Total_cost = (Asset.Acquisation_cost + Renwal.Sum(x => x.Renewal_amount.Value)) - Disposal;
            decimal End = Old_deprication + Deprecation + Renewal_depreication;
            decimal Net = Total_cost - End;
            //if (deprecation_method == Deprecation_method.Decreased)
            //{
            //    Deprecation = (Total_cost - Old_deprication - Scrap_value) * Deprecation_rate;
            //}
            return new Deprecation_temp_record
            {
                Total = Total_cost,
                Asset_id = Asset.Id,
                Assets_acquisition_cost = Asset.Acquisation_cost,
                Renewal_amount = Renwal.Sum(x => x.Renewal_amount.Value),
                Net_assets_cost = decimal.Round(Net, 4),
                Date = Depreication_date,
                Beginning_deprecation_accumulated = Old_deprication,
                Depreication = Deprecation,
                Renewal_depreication = Renewal_depreication,
                Ending_deprecication_accumulated = Old_deprication + Deprecation + Renewal_depreication,
                Disposal_amount = Disposal
            };
        }

        public static decimal CalculateOldDeprication(int AssetsId,DateTime DeprDate)
        {
            decimal Old_deprication = 0;
            DBContext db = new DBContext();

            List<Deprecation_record> ThisOld = db.Deprecation_record.Where(x => x.Asset_id == AssetsId&&x.Date<DeprDate).ToList();

            foreach (Deprecation_record item in ThisOld)
            {
                try
                {
                    if (!item.Disposal_depreication.HasValue)
                    {
                        item.Disposal_depreication = 0;
                    }
                    if (!item.Renewal_depreication.HasValue)
                    {
                        item.Depreication = 0;
                    }
                    if (!item.Depreication.HasValue)
                    {
                        item.Depreication = 0;
                    }
                    Old_deprication += item.Depreication.Value + item.Renewal_depreication.Value + item.Disposal_depreication.Value;
                }
                catch
                {

                }
            }

            return Old_deprication;
        }
        public JsonResult GetTempJV(int DepTempId)
        {
            int Type = db.Deprecation_Setting.FirstOrDefault().Deprecation_calcualtion;
            string DeprectionNote = "";
            Deprecation_temp Dep = db.Deprecation_temp.Find(DepTempId);
            FiscalYear_Table Depreication_period = DB.FiscalYear_Tables.ToList().FirstOrDefault(x => Convert.ToDateTime(x.Period_End_Date).Year == Dep.Transaction_date.Value.Year);

            if (Type == (int)Deprecation_calcualtion.Periodic)
            {

                DeprectionNote = $" For Period {Dep.Period_id}";

            }
            else if (Type == (int)Deprecation_calcualtion.Monthly)
            {

                DeprectionNote = $" For Month Number {Dep.Month}";

            }
            else
            {
                DeprectionNote = $" For Year {DateTime.Now.Year}";
            }

            TransactionApiController T = new TransactionApiController();
            string companyID = FabulousErp.Business.GetCompanyId();
            var detectJEPer =FabulousErp.Business.GetPostingSetup();//  Business.GetPostingSetup();
            int PostingToOrThrow = Business.Business.PostingToOrThrow();
            string JornalEntry = "";
            C_GeneralJournalEntry_Table SaveHeader = new C_GeneralJournalEntry_Table
            {
                C_PostingDate = Dep.Transaction_date.Value.ToString("yyyy-MM-dd"),
                C_TransactionDate = Dep.Transaction_date.Value.ToString("yyyy-MM-dd"),
                C_Refrence = DeprectionNote.Insert(0, "Depreciation For "),
                CurrencyID = companyID,
                C_SystemRate = 1,
                C_TransactionRate = 1,
                C_PostingKey = "FIXDEP",
                C_TransactionType = "FIXDEP"
            };
            List<Deprecation_temp_record> ThisDepreciation = db.Deprecation_temp_record.Where(x => x.Deprecation_id == DepTempId).ToList();

            List<C_SaveTransaction_Table> SaveTransaction = new List<C_SaveTransaction_Table>();
            foreach (int ClassId in Dep.Deprecation_temp_record.Select(x => x.Asset).GroupBy(x => x.Assets_class_id).Select(x => x.Key).OrderBy(x=>x))
            {
                Assets_class Class = db.Assets_class.FirstOrDefault(x => x.Id == ClassId);
                double Value = (double)ThisDepreciation.AsQueryable().Include(x => x.Asset).Where(x => x.Asset.Assets_class_id == ClassId).Sum(x => x.Depreication.Value + x.Renewal_depreication.Value);

                string asd = DB.C_CreateAccount_Tables.Find(Class.Assets_accounts.FirstOrDefault().Deprcation).AccountName;
                SaveTransaction.Add(new C_SaveTransaction_Table
                {
                    C_Describtion = DeprectionNote.Insert(0, "Depreciation For "),
                    C_Document = "",
                    C_AID = Class.Assets_accounts.FirstOrDefault().Deprcation,
                    C_OriginalAmount = Value,
                    C_Debit = Value,
                    C_Credit = 0,
                    C_CreateAccount_Table=new FabulousDB.DB_Tabels.Settings.Financial.Accounting._Company.C_Account.C_CreateAccount_Table
                    {
                        AccountName= DB.C_CreateAccount_Tables.Find(Class.Assets_accounts.FirstOrDefault().Deprcation).AccountName,
                        AccountID = DB.C_CreateAccount_Tables.Find(Class.Assets_accounts.FirstOrDefault().Deprcation).AccountID
                    }
                });
                SaveTransaction.Add(new C_SaveTransaction_Table
                {
                    C_Describtion = DeprectionNote.Insert(0, "Depreciation For "),
                    C_Document = "",
                    C_AID = Class.Assets_accounts.FirstOrDefault().Deprecation_accumulated_account,
                    C_OriginalAmount = Value,
                    C_Debit = 0,
                    C_Credit = Value,
                    C_CreateAccount_Table = new FabulousDB.DB_Tabels.Settings.Financial.Accounting._Company.C_Account.C_CreateAccount_Table
                    {
                        AccountName = DB.C_CreateAccount_Tables.Find(Class.Assets_accounts.FirstOrDefault().Deprecation_accumulated_account).AccountName,
                        AccountID = DB.C_CreateAccount_Tables.Find(Class.Assets_accounts.FirstOrDefault().Deprecation_accumulated_account).AccountID
                    }

                });

            }
            FabulousModels.APIModels.TransactionApiData Data= new FabulousModels.APIModels.TransactionApiData
            {
                SaveAnalytic = null,
                SaveCost = null,
                SaveHeader = SaveHeader,
                SaveTransaction = SaveTransaction.ToArray()
            };
            Dep.Jornal_number = JornalEntry;
            return Json(Data);
        }
        public ActionResult PostDeprciation(int DepTempId)
        {
            int Type = db.Deprecation_Setting.FirstOrDefault().Deprecation_calcualtion;
            string DeprectionNote = "";
            Deprecation_temp Dep = db.Deprecation_temp.Find(DepTempId);
            FiscalYear_Table Depreication_period = DB.FiscalYear_Tables.ToList().FirstOrDefault(x => Convert.ToDateTime(x.Period_End_Date).Year == Dep.Transaction_date.Value.Year);

            if (Type == (int)Deprecation_calcualtion.Periodic)
            {

                DeprectionNote = $" For Period {Dep.Period_id}";

            }
            else if (Type == (int)Deprecation_calcualtion.Monthly)
            {
               
                DeprectionNote = $" For Month Number {Dep.Month}";

            }
            else
            {
                DeprectionNote = $" For Year {DateTime.Now.Year}";
            }

            TransactionApiController T = new TransactionApiController();
            string companyID = FabulousErp.Business.GetCompanyId();
            var detectJEPer =FabulousErp.Business.GetPostingSetup();// Business.GetPostingSetup();
            int PostingToOrThrow = Business.Business.PostingToOrThrow();
            string JornalEntry = "";
            C_GeneralJournalEntry_Table SaveHeader = new C_GeneralJournalEntry_Table
            {
                C_PostingDate = Dep.Transaction_date.Value.ToString("yyyy-MM-dd"),
                C_TransactionDate = Dep.Transaction_date.Value.ToString("yyyy-MM-dd"),
                C_Refrence =DeprectionNote.Insert(0, "Depreciation For "),
                CurrencyID = companyID,
                C_SystemRate = 1,
                C_TransactionRate = 1,
                C_PostingKey = "FIXDEP",
                C_TransactionType = "FIXDEP"
            };
            List<Deprecation_record> ThisDepreciation = new List<Deprecation_record>();

            List<C_SaveTransaction_Table> SaveTransaction = new List<C_SaveTransaction_Table>();
            foreach (int ClassId in Dep.Deprecation_temp_record.Select(x=>x.Asset).GroupBy(x => x.Assets_class_id).Select(x => x.Key))
            {
                Assets_class Class = db.Assets_class.FirstOrDefault(x => x.Id == ClassId);
                double Value = (double)ThisDepreciation.AsQueryable().Include(x => x.Asset).Where(x => x.Asset.Assets_class_id == ClassId).Sum(x => x.Depreication.Value + x.Renewal_depreication.Value);

                SaveTransaction.Add(new C_SaveTransaction_Table
                {
                    C_Describtion = DeprectionNote.Insert(0, "Depreciation For "),
                    C_Document = "",
                    C_AID = Class.Assets_accounts.FirstOrDefault().Deprcation,
                    C_OriginalAmount = Value,
                    C_Debit = Value,
                    C_Credit = 0
                });
                SaveTransaction.Add(new C_SaveTransaction_Table
                {
                    C_Describtion = DeprectionNote.Insert(0, "Depreciation For "),
                    C_Document = "",
                    C_AID = Class.Assets_accounts.FirstOrDefault().Deprecation_accumulated_account,
                    C_OriginalAmount = Value,
                    C_Debit = 0,
                    C_Credit = Value
                });

            }
            JornalEntry += T.InsertTransactionData(companyID, PostingToOrThrow, null, new FabulousModels.APIModels.TransactionApiData
            {
                SaveAnalytic = null,
                SaveCost = null,
                SaveHeader = SaveHeader,
                SaveTransaction = SaveTransaction.ToArray()
            }, false).ReasonPhrase;
            Dep.Jornal_number = JornalEntry;
            return Json(1);
        }
        // GET: Deprecations/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Deprecation deprecation = db.Deprecations.Find(id);
            if (deprecation == null)
            {
                return HttpNotFound();
            }
            ViewBag.Period = new SelectList(db.Deprecation_periods, "Id", "text", deprecation.Period);
            return View(deprecation);
        }

        // POST: Deprecations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Deprecation_no,Transaction_date,Deprecation_date,Period,Is_assets_class,Acquisition_cost,Depreciation_accumulated,Adjustment_cost,Deprecation_rate,Depreication_cost,Special_depreication")] Deprecation deprecation)
        {
            if (ModelState.IsValid)
            {
                db.Entry(deprecation).State = EntityState.Modified;
                db.SaveChanges();
                return Json(deprecation.Id);
            }
            ViewBag.Period = new SelectList(db.Deprecation_periods, "Id", "text", deprecation.Period);
            return View(deprecation);
        }
        private DBContext MDB = new DBContext();

        // GET: Deprecations/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Deprecation deprecation = db.Deprecations.Find(id);

            ViewBag.TransactionDate = MDB.C_GeneralJournalEntry_Tables.Where(x => x.C_PostingNumber.ToString() == deprecation.Jornal_number.ToString()).ToList().DefaultIfEmpty(new C_GeneralJournalEntry_Table { C_TransactionDate = DateTime.Now.ToShortDateString().ToString() })
                .FirstOrDefault().C_TransactionDate;
            if (deprecation == null)
            {
                return HttpNotFound();
            }
            try
            {
                int Jornal_number = Convert.ToInt32(deprecation.Jornal_number);
                int PostingNumber = DB.C_GeneralJournalEntry_Tables.FirstOrDefault(x => x.C_PostingNumber == Jornal_number).C_JournalEntryNumber;
                ViewBag.PostingNum = Jornal_number;
                string companyID = (string)FabulousErp.Business.GetCompanyId();
                ViewBag.Currency = DB.C_GeneralJournalEntry_Tables.FirstOrDefault(x => x.C_PostingNumber == Jornal_number).CurrencyID;
                ViewBag.PostingToOrThrow = Business.Business.PostingToOrThrow();

            }
            catch
            {

            }
            return View(deprecation);
        }

        // POST: Deprecations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int Id)
        {

            Deprecation deprecation = db.Deprecations.Where(x => x.Id == Id).FirstOrDefault();
            List<Deprecation_record> DprRec = db.Deprecation_record.Where(x => x.Deprecation_id == Id).ToList();
            int Jornal_number = Convert.ToInt32(deprecation.Jornal_number);

            db.Deprecation_record.RemoveRange(DprRec);
            db.Deprecations.Remove(deprecation);
            string companyID = (string)FabulousErp.Business.GetCompanyId();
            db.SaveChanges();
            return Json(Jornal_number);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
