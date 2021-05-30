using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FabulousDB.DB_Tabels;
using FabulousDB.DB_Context;
using FabulousDB.Models;
using FabulousDB.DB_Tabels.Settings.Financial.Accounting._Company.C_Account;


namespace FixedAssets.Controllers
{
    public class ReportsController : Controller
    {
        private DBContext db = new DBContext();

        // GET: FixedAssets/Reports
        public ActionResult Year()
        {
            return View();
        }
        public ActionResult YearRpt(int Year)
        {

            List<YearRptC> Rpt = CalculateYearRpt(Year);
            List<YearRptC> LastRpt = CalculateYearRpt(Year - 1);
            Rpt.ForEach(x => { x.Equ.Net_assets_prev_year = LastRpt.Where(z => z.Classes_id == x.Classes_id).ToList().DefaultIfEmpty(new Controllers.YearRptC { Equ = new YearRptEquations { Net_assets_this_year = 0 } }).FirstOrDefault().Equ.Net_assets_this_year; });
            return PartialView(Rpt.ToList());
        }

        private List<YearRptC> CalculateYearRpt(int Year)
        {
            List<YearRptC> Rpt = new List<YearRptC>();

            foreach (Assets_class Prv in db.Assets_class.ToList())
            {
                decimal AllACqPrvYear = db.Assets.Where(x=>x.Start_use.Year<Year&&x.Assets_class_id==Prv.Id).ToList().DefaultIfEmpty(new Asset {Acquisation_cost=0 }).Sum(x => x.Acquisation_cost);
                decimal AllRenwalPrvYear = db.Fixed_assets_renewal.Where(x => x.Asset.Assets_class_id == Prv.Id&& x.Renwal_date.Value.Year < Year ).ToList().DefaultIfEmpty(new Fixed_assets_renewal { Renewal_amount = 0 }).Sum(x => x.Renewal_amount).Value;
                decimal AllDisposalPrvYear = db.Fixed_assets_disposel.Where(x => x.Asset.Assets_class_id == Prv.Id && x.Disposal_date.Value.Year <Year ).ToList().DefaultIfEmpty(new Fixed_assets_disposel { Disposal_amount = 0 }).Sum(x => x.Disposal_amount).Value;
                decimal PrvBeging = AllACqPrvYear + AllRenwalPrvYear - AllDisposalPrvYear;
                decimal AllPrvDeprciation = db.Deprecation_record.Where(x=>x.Asset.Assets_class_id==Prv.Id&&x.Deprecation.Deprecation_date.Year<Year).ToList().DefaultIfEmpty(new Deprecation_record {Depreication=0 }).Sum(x => x.Depreication).Value;

                Rpt.Add(new YearRptC
                {
                    Classes = Prv.Description,
                    Classes_id = Prv.Id,
                    Equ = new YearRptEquations
                    {
                        Beging_assets_cost = PrvBeging,
                        Renwal = 0,
                        Disposal = 0,
                        Ending_assets_cost = 0,
                        Beging_accumlated_cost = AllPrvDeprciation + AllRenwalPrvYear,
                        Depreciation = 0,
                        Ending_accumlate_cost = 0,
                        Net_assets_this_year = 0,
                        Net_assets_prev_year = 0
                    }
                });
            }
            foreach (Assets_class This in db.Assets_class.ToList())
            {
                decimal AllRenwalThisYear = db.Assets.Where(x => x.Fixed_assets_renewal
                .Any(z => z.Renwal_date.Value.Year == Year && z.Assets_id == x.Id)).ToList().DefaultIfEmpty(new Asset { Fixed_assets_renewal = new List<Fixed_assets_renewal> { new Fixed_assets_renewal { Renewal_amount = 0 } } }).SelectMany(x => x.Fixed_assets_renewal).ToList().DefaultIfEmpty(new Fixed_assets_renewal { Renewal_amount=0}).Sum(x=>x.Renewal_amount).Value
                    + db.Assets.Where(x=>x.Start_use.Year==Year&&x.Assets_class_id==This.Id).ToList().DefaultIfEmpty(new Asset { Acquisation_cost=0}).Sum(x => x.Acquisation_cost);

                decimal AllDisposalThisYear = db.Fixed_assets_disposel.Where(x => x.Disposal_date.Value.Year == Year&&x.Asset.Assets_class_id==This.Id).ToList().DefaultIfEmpty(new Fixed_assets_disposel { Disposal_amount = 0 }).Sum(x => x.Disposal_amount).Value;

                decimal Depreciation = db.Deprecation_record.Where(x => x.Asset.Assets_class_id == This.Id&&x.Asset.Start_use.Year==Year).ToList().DefaultIfEmpty(new Deprecation_record { Depreication=0}).Sum(x => x.Depreication).Value;

                decimal AllACqThisYear = db.Assets.Where(x => x.Start_use.Year == Year && x.Assets_class_id == This.Id).ToList().DefaultIfEmpty(new Asset {Acquisation_cost=0 }).Sum(x => x.Acquisation_cost);

                YearRptEquations ThisRpt = Rpt.Where(x => x.Classes_id == This.Id).Select(x => x.Equ).FirstOrDefault();

                if (ThisRpt != null)
                {
                    ThisRpt.Renwal = AllRenwalThisYear;
                    ThisRpt.Disposal = AllDisposalThisYear;
                    ThisRpt.Ending_assets_cost = ThisRpt.Beging_assets_cost + AllRenwalThisYear - AllDisposalThisYear;
                    ThisRpt.Depreciation = Depreciation;
                    ThisRpt.Ending_accumlate_cost = ThisRpt.Beging_assets_cost + Depreciation;
                    ThisRpt.Net_assets_this_year = ThisRpt.Ending_assets_cost - ThisRpt.Ending_accumlate_cost;
                    ThisRpt.Added_asset = AllACqThisYear + AllRenwalThisYear;
                }
                else
                {
                    Rpt.Add(new YearRptC
                    {
                        Classes = This.Description,

                        Classes_id = This.Id,
                        Equ = new YearRptEquations()
                        {
                            Net_assets_this_year = 0,
                            Added_asset= AllACqThisYear + AllRenwalThisYear,
                            Renwal = AllRenwalThisYear,
                            Disposal= AllDisposalThisYear,
                            Ending_assets_cost=0,
                            Depreciation= Depreciation,
                        }
                    });
                }

            }
            foreach (YearRptEquations R in Rpt.Select(x => x.Equ).ToList())
            {
                if (R.Disposal > 0)
                {
                    R.Disposal = -R.Disposal;
                }
                if (R.Depreciation > 0)
                {
                    R.Depreciation = -R.Depreciation;
                }
                if (R.Beging_accumlated_cost > 0)
                {
                    R.Beging_accumlated_cost = -R.Beging_accumlated_cost;
                }
                if (R.Ending_accumlate_cost > 0)
                {
                    R.Ending_accumlate_cost = -R.Ending_accumlate_cost;
                }
            }
            return Rpt;
        }
    }
    public class YearRptEquations
    {
        public decimal Beging_assets_cost { get; set; }
        public decimal Renwal { get; set; }
        public decimal Added_asset { get; set; }
        public decimal Disposal { get; set; }
        public decimal Ending_assets_cost { get; set; }
        public decimal Beging_accumlated_cost { get; set; }
        public decimal Depreciation { get; set; }
        public decimal Ending_accumlate_cost { get; set; }
        public decimal Net_assets_this_year { get; set; }
        public decimal Net_assets_prev_year { get; set; }

    }
    public class YearRptC
    {
        public string Classes { get; set; }
        public int Classes_id { get; set; }
        public YearRptEquations Equ { get; set; }
    }
}