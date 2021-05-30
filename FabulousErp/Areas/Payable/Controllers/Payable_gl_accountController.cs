using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FabulousDB.DB_Context;
using FabulousDB.DB_Tabels.Settings.Financial.Accounting._Company.C_Account;
using FabulousDB.DB_Tabels.Settings.Financial.Accounting.CurrenciesDefinition;
using FabulousDB.Models;
using FabulousErp.Bussiness;
using FabulousErp.Payable.Models; using FabulousDB.DB_Context;
using FabulousErp;

namespace Payable.Controllers
{
    public class Payable_gl_accountController : Controller
    {
        private DBContext db = new DBContext();
        private DBContext dbM = new DBContext();
        public JsonResult GetTransactionAccount(int VendorId
            , decimal Purchase, decimal Taken_discount, decimal Total
            , decimal Orginal_amount, decimal Payable, int BookId
            , Doc_type Doc_type, decimal Transaction_rate,
            string ThisCurrIso, int? ItemId = null, bool IsInv = false)
        {
            try
            {

                string companyID = FabulousErp.Business.GetCompanyId();
                IQueryable<Payable_gl_account> GlAccounts = db.Payable_gl_accounts.Where(x => x.Creditor_setting_id == VendorId);
                List<JvHeaderDet> Res = new List<JvHeaderDet>();
                Inv_item_gl_accounts ItemAcc = Enumerable.Empty<Inv_item_gl_accounts>().FirstOrDefault();
                if (IsInv)
                {
                    ItemAcc = db.Inv_item_gl_accounts.Include(x => x.Sales_return)
                        .Include(x => x.Sales)
                        .FirstOrDefault(x => x.Item_id == ItemId);
                }
                if (Doc_type == Doc_type.Invoice || Doc_type == Doc_type.Debit_Memo)
                {
                    Res = GlAccounts.Include(x => x.Purchase)
                   .Include(x => x.Taken_discount)
                   .Include(x => x.Account_payable)
                   .ToList().Select(x => new JvHeaderDet
                   {
                       ShowHeader = new JvHead
                       {
                           ISO = dbM.CurrenciesDefinition_Tables.Where(z => z.CurrencyID == companyID).ToList().DefaultIfEmpty(new FabulousDB.DB_Tabels.Settings.Financial.Accounting.CurrenciesDefinition.CurrenciesDefinition_Table { ISOCode = "Egp" }).FirstOrDefault().ISOCode,
                           DocType = "SED"

                       },
                       ShowTransactions = new List<JvManyAccountFormate>
                      {
                           new JvManyAccountFormate
                           {
                                AID=(x.Purchase!=null)?x.Purchase.C_AID:0,
                                Debit= Purchase,
                                Credit= 0,
                                AccountID =(x.Purchase!=null)?x.Purchase.AccountID:"",
                                AccountName = (x.Purchase!=null)?x.Purchase.AccountName:"",
                                Orginal_debit=Purchase/Transaction_rate,
                                Orginal_curr=ThisCurrIso,
                                Mark="Purchase"
                           },
                           new JvManyAccountFormate
                           {
                                AID=(x.Taken_discount!=null)?x.Taken_discount.C_AID:0,
                                Debit=0,
                                Credit= Taken_discount,
                                AccountID=(x.Taken_discount!=null)? x.Taken_discount.AccountID:"",
                                AccountName =(x.Taken_discount!=null)? x.Taken_discount.AccountName:"",
                                Orginal_credit=Taken_discount/Transaction_rate,
                                Orginal_curr=ThisCurrIso
                                

                           },
                           new JvManyAccountFormate
                           {
                                AID=0,
                                Debit= 0,
                                Credit= 0,
                                AccountID ="",
                                AccountName = "",
                                ShowBtn=false,
                                Orginal_debit=0,
                                Orginal_curr=ThisCurrIso
                           },

                           new JvManyAccountFormate
                           {
                                AID=x.Account_payable.C_AID,
                                Debit=0,
                                Credit= Total,
                                AccountID= x.Account_payable.AccountID,
                                AccountName = x.Account_payable.AccountName,
                                ShowBtn=false,
                                Orginal_credit=Total/Transaction_rate,
                                Orginal_curr=ThisCurrIso,
                                Mark="Vendore"
                           }
                       }

                   }).ToList();
                }
                else if (Doc_type == Doc_type.Credit_Memo)
                {
                    Res = GlAccounts.Include(x => x.Purchase)
                   .Include(x => x.Taken_discount)
                   .Include(x => x.Account_payable)
                   .ToList().Select(x => new JvHeaderDet
                   {
                       ShowHeader = new JvHead
                       {
                           ISO = dbM.CurrenciesDefinition_Tables.Where(z => z.CurrencyID == companyID).ToList().DefaultIfEmpty(new FabulousDB.DB_Tabels.Settings.Financial.Accounting.CurrenciesDefinition.CurrenciesDefinition_Table { ISOCode = "Egp" }).FirstOrDefault().ISOCode,
                           DocType = "SED"

                       },
                       ShowTransactions = new List<JvManyAccountFormate>
                      {
                            new JvManyAccountFormate
                           {
                                AID=(x.Taken_discount!=null)?x.Taken_discount.C_AID:0,
                                Debit=Taken_discount,
                                Credit= 0,
                                AccountID= (x.Taken_discount!=null)?x.Taken_discount.AccountID:"",
                                AccountName =(x.Taken_discount!=null)? x.Taken_discount.AccountName:"",
                                Orginal_debit=Taken_discount/Transaction_rate,
                                Orginal_curr=ThisCurrIso

                           },
                           new JvManyAccountFormate
                           {
                                AID=(x.Purchase!=null)?x.Purchase.C_AID:0,
                                Debit= 0,
                                Credit= Purchase,
                                AccountID = (x.Purchase!=null)?x.Purchase.AccountID:"",
                                AccountName =(x.Purchase!=null)?x.Purchase.AccountName:"",
                                Orginal_credit=Purchase/Transaction_rate,
                                Orginal_curr=ThisCurrIso


                           },
                           new JvManyAccountFormate
                           {
                                AID=x.Account_payable.C_AID,
                                Debit=Total,
                                Credit= 0,
                                AccountID= x.Account_payable.AccountID,
                                AccountName = x.Account_payable.AccountName,
                                Orginal_debit=Total/Transaction_rate,
                                Orginal_curr=ThisCurrIso,
                                Mark="Vendore"

                           },
                           new JvManyAccountFormate
                           {
                                AID=0,
                                Debit= 0,
                                Credit= 0,
                                AccountID ="",
                                AccountName = "",
                                Orginal_credit=0,
                                Orginal_curr=ThisCurrIso

                           }


                       }

                   }).ToList();
                }
                else if (Doc_type == Doc_type.Return)
                {
                    Res = GlAccounts.Include(x => x.Returne)
                   .Include(x => x.Taken_discount)
                   .Include(x => x.Account_payable)
                   .ToList().Select(x => new JvHeaderDet
                   {
                       ShowHeader = new JvHead
                       {
                           ISO = dbM.CurrenciesDefinition_Tables.Where(z => z.CurrencyID == companyID).ToList().DefaultIfEmpty(new FabulousDB.DB_Tabels.Settings.Financial.Accounting.CurrenciesDefinition.CurrenciesDefinition_Table { ISOCode = "Egp" }).FirstOrDefault().ISOCode,
                           DocType = "SED"

                       },
                       ShowTransactions = new List<JvManyAccountFormate>
                      {
                            new JvManyAccountFormate
                           {
                                AID=(x.Taken_discount!=null)?x.Taken_discount.C_AID:0,
                                Debit=Taken_discount,
                                Credit= 0,
                                AccountID= (x.Taken_discount!=null)?x.Taken_discount.AccountID:"",
                                AccountName =(x.Taken_discount!=null)? x.Taken_discount.AccountName:"",
                                Orginal_debit=Taken_discount/Transaction_rate,
                                Orginal_curr=ThisCurrIso


                           },
                           new JvManyAccountFormate
                           {
                                AID=(x.Returne!=null)?x.Returne.C_AID:0,
                                Debit= 0,
                                Credit= Purchase,
                                AccountID = (x.Returne!=null)?x.Returne.AccountID:"",
                                AccountName = (x.Returne!=null)?x.Returne.AccountName:"",
                                Orginal_credit=Purchase/Transaction_rate,
                                Orginal_curr=ThisCurrIso


                           },
                           new JvManyAccountFormate
                           {
                                AID=x.Account_payable.C_AID,
                                Debit=Total,
                                Credit= 0,
                                AccountID= x.Account_payable.AccountID,
                                AccountName = x.Account_payable.AccountName,
                                Orginal_debit=Total/Transaction_rate,
                                Orginal_curr=ThisCurrIso,
                                Mark="Vendore"
                           },
                           new JvManyAccountFormate
                           {
                                AID=0,
                                Debit= 0,
                                Credit= 0,
                                AccountID ="",
                                AccountName = "",
                                Orginal_credit=0,
                                Orginal_curr=ThisCurrIso
                           }
                       }

                   }).ToList();
                }

                if (db.Other_options.Where(x => x.Option == Other_option_enum.Active_payment)
                    .ToList().DefaultIfEmpty(new Payable_other_option { Checked = false }).FirstOrDefault().Checked
                    || db.Payable_creditor_setting.Find(VendorId).Credit_limit != Credit_limit_enum.No_credit)
                {
                    Res.AddRange(PaymentAccount(VendorId, Orginal_amount, Taken_discount, Payable, BookId, Transaction_rate, ThisCurrIso, true));
                }
                return Json(Res);

            }
            catch (Exception ex)
            {
                return Json(ex.ToString(), JsonRequestBehavior.AllowGet);
            }
            return Json(db.Payable_gl_accounts.Where(x => x.Creditor_setting_id == VendorId).FirstOrDefault());
        }

        public JsonResult GetTotalJv(decimal Total, int VendorId, string ThisCurrIso, decimal Transaction_rate, Doc_type Doc_type)
        {
            IQueryable<Payable_gl_account> GlAccounts = db.Payable_gl_accounts.Where(x => x.Creditor_setting_id == VendorId);
            List<JvHeaderDet> Res = new List<JvHeaderDet>();
            string companyID = FabulousErp.Business.GetCompanyId();

            if (Doc_type == Doc_type.Invoice || Doc_type == Doc_type.Debit_Memo)
            {
                Res = GlAccounts
                  .Include(x => x.Account_payable)
                  .ToList().Select(x => new JvHeaderDet
                  {
                      ShowHeader = new JvHead
                      {
                          ISO = dbM.CurrenciesDefinition_Tables.Where(z => z.CurrencyID == companyID).ToList().DefaultIfEmpty(new FabulousDB.DB_Tabels.Settings.Financial.Accounting.CurrenciesDefinition.CurrenciesDefinition_Table { ISOCode = "Egp" }).FirstOrDefault().ISOCode,
                          DocType = "SED"

                      },
                      ShowTransactions = new List<JvManyAccountFormate>
                      {
                            new JvManyAccountFormate
                          {
                              AID = 0,
                              Debit = 0,
                              Credit = 0,
                              AccountID = "",
                              AccountName = "",
                              ShowBtn = false,
                              Orginal_debit = 0,
                              Orginal_curr = ThisCurrIso
                          },

                          new JvManyAccountFormate
                          {
                              AID = x.Account_payable.C_AID,
                              Debit = 0,
                              Credit = Total,
                              AccountID = x.Account_payable.AccountID,
                              AccountName = x.Account_payable.AccountName,
                              ShowBtn = false,
                              Orginal_credit = Total / Transaction_rate,
                              Orginal_curr = ThisCurrIso
                          }
                      }
                  }).ToList();
            }
            else if (Doc_type == Doc_type.Return)
            {
                Res = GlAccounts
                  .Include(x => x.Account_payable)
                  .ToList().Select(x => new JvHeaderDet
                  {
                      ShowHeader = new JvHead
                      {
                          ISO = dbM.CurrenciesDefinition_Tables.Where(z => z.CurrencyID == companyID).ToList().DefaultIfEmpty(new FabulousDB.DB_Tabels.Settings.Financial.Accounting.CurrenciesDefinition.CurrenciesDefinition_Table { ISOCode = "Egp" }).FirstOrDefault().ISOCode,
                          DocType = "SED"

                      },
                      ShowTransactions = new List<JvManyAccountFormate>
                      {
                           new JvManyAccountFormate
                           {
                               AID = x.Account_payable.C_AID,
                               Debit = Total,
                               Credit = 0,
                               AccountID = x.Account_payable.AccountID,
                               AccountName = x.Account_payable.AccountName,
                               Orginal_debit = Total / Transaction_rate,
                               Orginal_curr = ThisCurrIso


                           },
                           new JvManyAccountFormate
                           {
                               AID = 0,
                               Debit = 0,
                               Credit = 0,
                               AccountID = "",
                               AccountName = "",
                               Orginal_credit = 0,
                               Orginal_curr = ThisCurrIso


                           }
                      }
                  }).ToList();
            }

            return Json(Res);
        }
        public JsonResult GetDiscountJV(decimal Taken_discount, int VendorId, string ThisCurrIso, decimal Transaction_rate,Nullable<Doc_type> Doc_type = null)
        {
            string companyID = FabulousErp.Business.GetCompanyId();
            if (Doc_type == FabulousDB.Models.Doc_type.Return)
            {
                JvHeaderDet Res = db.Payable_gl_accounts.Include(x => x.Taken_discount)
               .Where(x => x.Creditor_setting_id == VendorId).ToList().Select(x =>
                new JvHeaderDet
                {
                    ShowHeader = new JvHead
                    {
                        ISO = dbM.CurrenciesDefinition_Tables.Where(z => z.CurrencyID == companyID).ToList().DefaultIfEmpty(new FabulousDB.DB_Tabels.Settings.Financial.Accounting.CurrenciesDefinition.CurrenciesDefinition_Table { ISOCode = "Egp" }).FirstOrDefault().ISOCode,
                        DocType = "SED"

                    },
                    ShowTransactions = new List<JvManyAccountFormate>
                     {
                           new JvManyAccountFormate
                           {
                               AID=(x.Taken_discount!=null)?x.Taken_discount.C_AID:0,
                                Debit=Taken_discount,
                                Credit=0 ,
                                AccountID= (x.Taken_discount!=null)?x.Taken_discount.AccountID:"",
                                AccountName =(x.Taken_discount!=null)? x.Taken_discount.AccountName:"",
                                Orginal_debit=Taken_discount/Transaction_rate,
                                ShowBtn=false,
                                Orginal_curr=ThisCurrIso
                           },
                          new JvManyAccountFormate
                           {
                                AID=0,
                                Debit= 0,
                                Credit= 0,
                                AccountID ="",
                                AccountName = "",
                                ShowBtn=false,
                                Orginal_debit=0,
                                Orginal_curr=ThisCurrIso
                           }
                    }
                }).FirstOrDefault();
                return Json(Res);
            }
            else
            {
                JvHeaderDet Res = db.Payable_gl_accounts.Include(x => x.Taken_discount)
               .Where(x => x.Creditor_setting_id == VendorId).ToList().Select(x =>
                new JvHeaderDet
                {
                    ShowHeader = new JvHead
                    {
                        ISO = dbM.CurrenciesDefinition_Tables.Where(z => z.CurrencyID == companyID).ToList().DefaultIfEmpty(new FabulousDB.DB_Tabels.Settings.Financial.Accounting.CurrenciesDefinition.CurrenciesDefinition_Table { ISOCode = "Egp" }).FirstOrDefault().ISOCode,
                        DocType = "SED"

                    },
                    ShowTransactions = new List<JvManyAccountFormate>
                     {
                           new JvManyAccountFormate
                           {
                                AID=0,
                                Debit= 0,
                                Credit= 0,
                                AccountID ="",
                                AccountName = "",
                                ShowBtn=false,
                                Orginal_debit=0,
                                Orginal_curr=ThisCurrIso
                           },
                          new JvManyAccountFormate
                           {
                                AID=(x.Taken_discount!=null)?x.Taken_discount.C_AID:0,
                                Debit=0,
                                Credit= Taken_discount,
                                AccountID= (x.Taken_discount!=null)?x.Taken_discount.AccountID:"",
                                AccountName =(x.Taken_discount!=null)? x.Taken_discount.AccountName:"",
                                Orginal_credit=Taken_discount/Transaction_rate,
                                ShowBtn=false,
                                Orginal_curr=ThisCurrIso
                           }
                    }
                }).FirstOrDefault();
                return Json(Res);
            }
           


        }
        public JsonResult GetFrightJV(decimal FrightAmount, List<int> ItemsIds, string ThisCurrIso
            , Doc_type DocType, decimal Transaction_rate=1)
        {
            string companyID = FabulousErp.Business.GetCompanyId();
            DBContext Mdb = new DBContext();

            List<Inv_item_gl_accounts> Items = Mdb.Inv_item_gl_accounts
                .Include(x => x.Fright).Include(x => x.item)
                .Where(x => ItemsIds.Any(z => z == x.item.Id))
                .ToList();
           if (Doc_type.Return == DocType)
            {
                try
                {
                    JvHeaderDet Res = Items.ToList().Select(x =>
                   new JvHeaderDet
                   {
                       ShowHeader = new JvHead
                       {
                           ISO = dbM.CurrenciesDefinition_Tables.Where(z => z.CurrencyID == companyID).ToList().DefaultIfEmpty(new FabulousDB.DB_Tabels.Settings.Financial.Accounting.CurrenciesDefinition.CurrenciesDefinition_Table { ISOCode = "Egp" }).FirstOrDefault().ISOCode,
                           DocType = "SED"

                       },
                       ShowTransactions = new List<JvManyAccountFormate>
                        {
                           new JvManyAccountFormate
                           {
                                AID=0,
                                Debit= 0,
                                Credit= 0,
                                AccountID ="",
                                AccountName = "",
                                ShowBtn=false,
                                Orginal_debit=0,
                                Orginal_curr=ThisCurrIso
                           },
                           new JvManyAccountFormate
                           {
                                AID=(x!=null)?x.Fright.C_AID:0,
                                Debit=0,
                                Credit= FrightAmount,
                                AccountID= (x.Fright!=null)?x.Fright.AccountID:"",
                                AccountName =(x.Fright!=null)? x.Fright.AccountName:"",
                                Orginal_credit=FrightAmount/Transaction_rate,
                                ShowBtn=false,
                                Orginal_curr=ThisCurrIso
                           }
                       }
                   }).FirstOrDefault();
                    return Json(Res);
                }
                catch
                {
                    return Json(null);

                }
            }
            else
            {
                try
                {
                    JvHeaderDet Res = Items.ToList().Select(x =>
                   new JvHeaderDet
                   {
                       ShowHeader = new JvHead
                       {
                           ISO = dbM.CurrenciesDefinition_Tables.Where(z => z.CurrencyID == companyID).ToList().DefaultIfEmpty(new FabulousDB.DB_Tabels.Settings.Financial.Accounting.CurrenciesDefinition.CurrenciesDefinition_Table { ISOCode = "Egp" }).FirstOrDefault().ISOCode,
                           DocType = "SED"

                       },
                       ShowTransactions = new List<JvManyAccountFormate>
                        {
                           new JvManyAccountFormate
                           {
                                AID=(x!=null)?x.Fright.C_AID:0,
                                Debit=FrightAmount,
                                Credit= 0,
                                AccountID= (x.Fright!=null)?x.Fright.AccountID:"",
                                AccountName =(x.Fright!=null)? x.Fright.AccountName:"",
                                Orginal_debit=FrightAmount/Transaction_rate,
                                ShowBtn=false,
                                Orginal_curr=ThisCurrIso
                           },
                           new JvManyAccountFormate
                           {
                                AID=0,
                                Debit= 0,
                                Credit= 0,
                                AccountID ="",
                                AccountName = "",
                                ShowBtn=false,
                                Orginal_debit=0,
                                Orginal_curr=ThisCurrIso
                           }
                       }
                   }).FirstOrDefault();
                    return Json(Res);
                }
                catch
                {
                    return Json(null);

                }
            }




        }
        public JsonResult GetAccrualFrightJV(decimal FrightAmount, List<int> ItemsIds, string ThisCurrIso
            , Doc_type DocType, decimal Transaction_rate=1)
        {
            string companyID = FabulousErp.Business.GetCompanyId();
            DBContext Mdb = new DBContext();

            List<Inv_item_gl_accounts> Items = Mdb.Inv_item_gl_accounts
                .Include(x => x.Accrual_fright).Include(x => x.item)
                .Where(x => ItemsIds.Any(z => z == x.item.Id))
                .ToList();
           if (Doc_type.Invoice == DocType)
            {
                try
                {
                    JvHeaderDet Res = Items.ToList().Select(x =>
                   new JvHeaderDet
                   {
                       ShowHeader = new JvHead
                       {
                           ISO = dbM.CurrenciesDefinition_Tables.Where(z => z.CurrencyID == companyID).ToList().DefaultIfEmpty(new FabulousDB.DB_Tabels.Settings.Financial.Accounting.CurrenciesDefinition.CurrenciesDefinition_Table { ISOCode = "Egp" }).FirstOrDefault().ISOCode,
                           DocType = "SED"

                       },
                       ShowTransactions = new List<JvManyAccountFormate>
                        {
                           new JvManyAccountFormate
                           {
                                AID=0,
                                Debit= 0,
                                Credit= 0,
                                AccountID ="",
                                AccountName = "",
                                ShowBtn=false,
                                Orginal_debit=0,
                                Orginal_curr=ThisCurrIso
                           },
                           new JvManyAccountFormate
                           {
                                AID=(x!=null)?x.Accrual_fright.C_AID:0,
                                Debit=0,
                                Credit= FrightAmount,
                                AccountID= (x.Accrual_fright!=null)?x.Accrual_fright.AccountID:"",
                                AccountName =(x.Accrual_fright!=null)? x.Accrual_fright.AccountName:"",
                                Orginal_credit=FrightAmount/Transaction_rate,
                                ShowBtn=false,
                                Orginal_curr=ThisCurrIso
                           }
                       }
                   }).FirstOrDefault();
                    return Json(Res);
                }
                catch
                {
                    return Json(null);

                }
            }
            else
            {
                try
                {
                    JvHeaderDet Res = Items.ToList().Select(x =>
                   new JvHeaderDet
                   {
                       ShowHeader = new JvHead
                       {
                           ISO = dbM.CurrenciesDefinition_Tables.Where(z => z.CurrencyID == companyID).ToList().DefaultIfEmpty(new FabulousDB.DB_Tabels.Settings.Financial.Accounting.CurrenciesDefinition.CurrenciesDefinition_Table { ISOCode = "Egp" }).FirstOrDefault().ISOCode,
                           DocType = "SED"

                       },
                       ShowTransactions = new List<JvManyAccountFormate>
                        {
                           new JvManyAccountFormate
                           {
                                AID=(x!=null)?x.Accrual_fright.C_AID:0,
                                Debit=FrightAmount,
                                Credit= 0,
                                AccountID= (x.Accrual_fright!=null)?x.Accrual_fright.AccountID:"",
                                AccountName =(x.Accrual_fright!=null)? x.Accrual_fright.AccountName:"",
                                Orginal_debit=FrightAmount/Transaction_rate,
                                ShowBtn=false,
                                Orginal_curr=ThisCurrIso
                           },
                           new JvManyAccountFormate
                           {
                                AID=0,
                                Debit= 0,
                                Credit= 0,
                                AccountID ="",
                                AccountName = "",
                                ShowBtn=false,
                                Orginal_debit=0,
                                Orginal_curr=ThisCurrIso
                           }
                       }
                   }).FirstOrDefault();
                    return Json(Res);
                }
                catch
                {
                    return Json(null);

                }
            }




        }
        public List<JvHeaderDet> PaymentAccount(int VendorId
            , decimal Orginal_amount, decimal Taken_discount, decimal Payable
            , int CheckBookId, decimal Transaction_rate, string CurrencyIso, bool IsFromTransaction = false)
        {
            IQueryable<Payable_gl_account> GlAccounts = db.Payable_gl_accounts.Where(x => x.Creditor_setting_id == VendorId);
            List<JvHeaderDet> Res = new List<JvHeaderDet>();
            string companyID = FabulousErp.Business.GetCompanyId();

            if (CheckBookId == 0)
            {
                return new List<JvHeaderDet> { };
            }
            C_CreateAccount_Table CheckBookAccount = dbM.C_CheckBookSetting_Tables.Find(CheckBookId).C_CreateAccount_Table;
            Res = GlAccounts
                  .Include(x => x.Taken_discount)
                  .Include(x => x.Account_payable)
                  .ToList().Select(x => new JvHeaderDet
                  {
                      ShowHeader = new JvHead
                      {
                          ISO = dbM.CurrenciesDefinition_Tables.Where(z => z.CurrencyID == companyID).ToList().DefaultIfEmpty(new FabulousDB.DB_Tabels.Settings.Financial.Accounting.CurrenciesDefinition.CurrenciesDefinition_Table { ISOCode = "Egp" }).FirstOrDefault().ISOCode,
                          DocType = "SID"
                      },
                      ShowTransactions = new List<JvManyAccountFormate>
                     {
                            new JvManyAccountFormate
                           {
                                AID=0,
                                Debit=0,
                                Credit= 0,
                                AccountID= "",
                                AccountName = "",
                                ShowBtn=false,
                                Orginal_debit=0,
                                Orginal_curr=CurrencyIso

                           },
                           new JvManyAccountFormate
                           {
                                AID=(CheckBookAccount!=null)?CheckBookAccount.C_AID:0,
                                Debit= 0,
                                Credit= Orginal_amount,
                                AccountID = (CheckBookAccount!=null)?CheckBookAccount.AccountID:"",
                                AccountName =(CheckBookAccount!=null)?CheckBookAccount.AccountName:"",
                                ShowBtn=false,
                                Orginal_credit=Orginal_amount/Transaction_rate,
                                Orginal_curr=CurrencyIso


                           },
                           new JvManyAccountFormate
                           {
                                AID=x.Account_payable.C_AID,
                                Debit= Payable,
                                Credit= 0,
                                AccountID =x.Account_payable.AccountID,
                                AccountName = x.Account_payable.AccountName,
                                ShowBtn=false,
                                Orginal_debit=Payable/Transaction_rate,
                                Orginal_curr=CurrencyIso


                           },
                           new JvManyAccountFormate
                           {
                                AID=0,
                                Debit= 0,
                                Credit= 0,
                                AccountID ="",
                                AccountName = "",
                                ShowBtn=false,
                                Orginal_credit=0,
                                Orginal_curr=CurrencyIso
                           }


                      }

                  })
                  .ToList();
            if (!IsFromTransaction)
            {
                Res.AddRange(GlAccounts
                 .Include(x => x.Taken_discount)
                 .Include(x => x.Account_payable)
                 .ToList().Select(x => new JvHeaderDet
                 {
                     ShowHeader = new JvHead
                     {
                         ISO = dbM.CurrenciesDefinition_Tables.Where(z => z.CurrencyID == companyID).ToList().DefaultIfEmpty(new FabulousDB.DB_Tabels.Settings.Financial.Accounting.CurrenciesDefinition.CurrenciesDefinition_Table { ISOCode = "Egp" }).FirstOrDefault().ISOCode,
                         DocType = "SID"
                     },
                     ShowTransactions = new List<JvManyAccountFormate>
                     {
                        new JvManyAccountFormate
                        {
                            AID = 0,
                            Debit = 0,
                            Credit = 0,
                            AccountID = "",
                            AccountName = "",
                            Orginal_debit=0,
                                Orginal_curr=CurrencyIso,
                                ShowBtn=true,

                        },
                        new JvManyAccountFormate
                        {
                            AID =(x.Taken_discount!=null)?x.Taken_discount.C_AID:0,
                            Debit = 0,
                            Credit = Taken_discount,
                            AccountID =(x.Taken_discount!=null)?x.Taken_discount.AccountID:"",
                            AccountName =(x.Taken_discount!=null)?x.Taken_discount.AccountName:"",
                            Orginal_credit=Taken_discount/Transaction_rate,
                                Orginal_curr=CurrencyIso,
                                ShowBtn=true,

                        }
                     }
                 }));
            }
            return Res;
        }
        public JsonResult GetPaymentAccount(int VendorId
            , decimal Orginal_amount, decimal Taken_discount, decimal Payable
            , int CheckBookId, decimal Transaction_rate, string CurrencyIso)
        {
            return Json(PaymentAccount(VendorId, Orginal_amount, Taken_discount, Payable, CheckBookId, Transaction_rate, CurrencyIso));
        }


        public JsonResult GetAssignEarnAndLose(int VendorId
            , List<decimal> TakenDiscount, List<decimal> EarnAndLoss, List<string> Currencies, List<decimal> Rates,
            string MainCurr)
        {
            try
            {

                string companyID = (string)FabulousErp.Business.GetCompanyId();
                IQueryable<Payable_gl_account> GlAccounts = db.Payable_gl_accounts.Where(x => x.Creditor_setting_id == VendorId).Include(x => x.Account_payable).Include(x => x.Taken_discount);
                List<CurrenciesDefinition_Tables> CDT = dbM.AccountCurrencyDefinition_Tables.Where(x => Currencies.Contains(x.CurrencyID)).ToList();

                List<JvManyAccountFormate> EarnDebit = new List<JvManyAccountFormate>();
                List<JvManyAccountFormate> SumEarnDebit = new List<JvManyAccountFormate>();
                List<JvManyAccountFormate> EarnCredit = new List<JvManyAccountFormate>();
                List<JvManyAccountFormate> SumEarnCredit = new List<JvManyAccountFormate>();
                List<JvManyAccountFormate> TakenDiscountSum = new List<JvManyAccountFormate>();

                List<JvManyAccountFormate> PayableDebit = new List<JvManyAccountFormate>();
                List<JvManyAccountFormate> PayableCredit = new List<JvManyAccountFormate>();

                List<JvManyAccountFormate> PayableDebitSum = new List<JvManyAccountFormate>();
                List<JvManyAccountFormate> PayableCreditSum = new List<JvManyAccountFormate>();

                List<JvManyAccountFormate> TakenDiscountCredit = new List<JvManyAccountFormate>();
                List<JvManyAccountFormate> TakenDiscountCreditSum = new List<JvManyAccountFormate>();
                string OrginalCurr = dbM.CurrenciesDefinition_Tables.Where(z => z.CurrencyID == companyID).ToList().DefaultIfEmpty(new FabulousDB.DB_Tabels.Settings.Financial.Accounting.CurrenciesDefinition.CurrenciesDefinition_Table { ISOCode = "Egp" }).FirstOrDefault().ISOCode;
                string OrginalCurrId = dbM.CurrenciesDefinition_Tables.Where(z => z.CurrencyID == companyID).FirstOrDefault().CurrencyID;
                string MainIso = dbM.CurrenciesDefinition_Tables.Where(x => x.CurrencyID == OrginalCurrId).FirstOrDefault().ISOCode;

                for (int i = 0; i < TakenDiscount.Count(); i++)
                {
                    if ((OrginalCurrId != Currencies[i] && OrginalCurrId != MainCurr)
                        || TakenDiscount.Sum() > 0)
                    {
                        if (EarnAndLoss.Sum() > 0)
                        {
                            EarnCredit.Add(new JvManyAccountFormate
                            {
                                AID = CDT.Where(x => x.Type == "Profit").FirstOrDefault().C_CreateAccount_Table.C_AID,
                                Debit = 0,
                                Credit = EarnAndLoss[i],
                                AccountID = CDT.Where(x => x.Type == "Profit").FirstOrDefault().C_CreateAccount_Table.AccountID,
                                AccountName = CDT.Where(x => x.Type == "Profit").FirstOrDefault().C_CreateAccount_Table.AccountName,
                                Orginal_credit = EarnAndLoss[i],
                                Orginal_curr = OrginalCurr
                            });
                        }
                        else
                        {
                            try
                            {
                                EarnDebit.Add(new JvManyAccountFormate
                                {
                                    AID = CDT.Where(x => x.Type == "Loss").FirstOrDefault().C_CreateAccount_Table.C_AID,
                                    Debit = EarnAndLoss[i],
                                    Credit = 0,
                                    AccountID = CDT.Where(x => x.Type == "Loss").FirstOrDefault().C_CreateAccount_Table.AccountID,
                                    AccountName = CDT.Where(x => x.Type == "Loss").FirstOrDefault().C_CreateAccount_Table.AccountName,
                                    Orginal_debit = TakenDiscount[i] - EarnAndLoss[i],
                                    Orginal_curr = OrginalCurr
                                });
                            }
                            catch
                            {

                            }

                        }

                        if (TakenDiscount.Sum() - EarnAndLoss.Sum() < 0
                            || EarnAndLoss.Sum() == 0)
                        {
                            PayableDebit.Add(new JvManyAccountFormate
                            {
                                AID = GlAccounts.FirstOrDefault().Account_payable.C_AID,
                                Debit = (TakenDiscount[i] - EarnAndLoss[i]),
                                Credit = 0,
                                AccountID = GlAccounts.FirstOrDefault().Account_payable.AccountID,
                                AccountName = GlAccounts.FirstOrDefault().Account_payable.AccountName,
                                Orginal_debit = (TakenDiscount[i] - EarnAndLoss[i]),
                                Orginal_curr = OrginalCurr,
                                ShowBtn = false
                            });
                        }
                        else
                        {
                            PayableCredit.Add(new JvManyAccountFormate
                            {
                                AID = GlAccounts.FirstOrDefault().Account_payable.C_AID,
                                Debit = 0,
                                Credit = TakenDiscount[i] - EarnAndLoss[i],
                                AccountID = GlAccounts.FirstOrDefault().Account_payable.AccountID,
                                AccountName = GlAccounts.FirstOrDefault().Account_payable.AccountName,
                                Orginal_credit = (TakenDiscount[i] - EarnAndLoss[i]),
                                Orginal_curr = OrginalCurr,
                                ShowBtn = false

                            });
                        }

                        TakenDiscountCredit.Add(new JvManyAccountFormate
                        {
                            AID = GlAccounts.FirstOrDefault().Taken_discount.C_AID,
                            Debit = 0,
                            Credit = TakenDiscount[i],
                            AccountID = GlAccounts.FirstOrDefault().Taken_discount.AccountID,
                            AccountName = GlAccounts.FirstOrDefault().Taken_discount.AccountName,
                            Orginal_credit = TakenDiscount[i],
                            Orginal_curr = MainIso//OrginalCurr
                        });

                    }
                }

                foreach (List<JvManyAccountFormate> i in EarnCredit.GroupBy(x => x.Orginal_curr).Select(x => x.ToList()))
                {
                    SumEarnCredit.Add(new JvManyAccountFormate
                    {
                        AID = CDT.Where(x => x.Type == "Profit").FirstOrDefault().C_CreateAccount_Table.C_AID,
                        Debit = 0,
                        Credit = i.Sum(x => x.Credit),
                        AccountID = CDT.Where(x => x.Type == "Profit").FirstOrDefault().C_CreateAccount_Table.AccountID,
                        AccountName = CDT.Where(x => x.Type == "Profit").FirstOrDefault().C_CreateAccount_Table.AccountName,
                        Orginal_credit = i.Sum(x => x.Orginal_credit),
                        Orginal_curr = i.FirstOrDefault().Orginal_curr
                    });
                }
                foreach (List<JvManyAccountFormate> i in EarnDebit.GroupBy(x => x.Orginal_curr).Select(x => x.ToList()))
                {
                    SumEarnDebit.Add(new JvManyAccountFormate
                    {
                        AID = CDT.Where(x => x.Type == "Loss").FirstOrDefault().C_CreateAccount_Table.C_AID,
                        Debit = i.Sum(x => x.Debit),
                        Credit = 0,
                        AccountID = CDT.Where(x => x.Type == "Loss").FirstOrDefault().C_CreateAccount_Table.AccountID,
                        AccountName = CDT.Where(x => x.Type == "Loss").FirstOrDefault().C_CreateAccount_Table.AccountName,
                        Orginal_debit = i.Sum(x => x.Orginal_debit),
                        Orginal_curr = i.FirstOrDefault().Orginal_curr
                    });
                }
                foreach (List<JvManyAccountFormate> i in TakenDiscountCredit.GroupBy(x => x.Orginal_curr).Select(x => x.ToList()))
                {
                    TakenDiscountCreditSum.Add(new JvManyAccountFormate
                    {
                        AID = GlAccounts.FirstOrDefault().Taken_discount.C_AID,
                        Debit = 0,
                        Credit = i.Sum(x => x.Credit),
                        AccountID = GlAccounts.FirstOrDefault().Taken_discount.AccountID,
                        AccountName = GlAccounts.FirstOrDefault().Taken_discount.AccountName,
                        Orginal_credit = i.Sum(x => x.Orginal_credit),
                        Orginal_curr = MainIso//i.FirstOrDefault().Orginal_curr
                    });
                }
                foreach (List<JvManyAccountFormate> i in PayableDebit.GroupBy(x => x.Orginal_curr).Select(x => x.ToList()))
                {
                    PayableDebitSum.Add(new JvManyAccountFormate
                    {
                        AID = GlAccounts.FirstOrDefault().Account_payable.C_AID,
                        Debit = i.Sum(x => x.Debit),
                        Credit = 0,
                        AccountID = GlAccounts.FirstOrDefault().Account_payable.AccountID,
                        AccountName = GlAccounts.FirstOrDefault().Account_payable.AccountName,
                        Orginal_debit = i.Sum(x => x.Orginal_debit),
                        Orginal_curr = i.FirstOrDefault().Orginal_curr,
                        ShowBtn = false

                    });
                }
                foreach (List<JvManyAccountFormate> i in PayableCredit.GroupBy(x => x.Orginal_curr).Select(x => x.ToList()))
                {
                    try
                    {
                        PayableCreditSum.Add(new JvManyAccountFormate
                        {
                            AID = CDT.Where(x => x.Type == "Profit").ToList().DefaultIfEmpty(new CurrenciesDefinition_Tables { C_CreateAccount_Table = new C_CreateAccount_Table { C_AID = -1 } }).FirstOrDefault().C_CreateAccount_Table.C_AID,
                            Debit = 0,
                            Credit = i.Sum(x => x.Credit),
                            AccountID = CDT.Where(x => x.Type == "Profit").ToList().DefaultIfEmpty(new CurrenciesDefinition_Tables { C_CreateAccount_Table = new C_CreateAccount_Table { AccountID = "00-00" } }).FirstOrDefault().C_CreateAccount_Table.AccountID,
                            AccountName = CDT.Where(x => x.Type == "Profit").ToList().DefaultIfEmpty(new CurrenciesDefinition_Tables { C_CreateAccount_Table = new C_CreateAccount_Table { AccountName = "No Payable Acc" } }).FirstOrDefault().C_CreateAccount_Table.AccountName,
                            Orginal_credit = i.Sum(x => x.Orginal_credit),
                            Orginal_curr = i.FirstOrDefault().Orginal_curr,
                            ShowBtn = false

                        });
                    }
                    catch
                    {

                    }

                }

                List<JvManyAccountFormate> ShowTransactions = new List<JvManyAccountFormate>();

                ShowTransactions.AddRange(SumEarnDebit.DefaultIfEmpty(new JvManyAccountFormate { }));
                ShowTransactions.AddRange(SumEarnCredit.DefaultIfEmpty(new JvManyAccountFormate { }));

                ShowTransactions.AddRange(new List<JvManyAccountFormate> { new JvManyAccountFormate { } });
                ShowTransactions.AddRange(TakenDiscountCreditSum.DefaultIfEmpty(new JvManyAccountFormate { }));

                ShowTransactions.AddRange(PayableDebitSum.DefaultIfEmpty(new JvManyAccountFormate { }));
                ShowTransactions.AddRange(PayableCreditSum.DefaultIfEmpty(new JvManyAccountFormate { }));

                List<JvHeaderDet> Res = new List<JvHeaderDet>
                {
                    new JvHeaderDet
                    {
                        ShowHeader = new JvHead
                        {
                            ISO = dbM.CurrenciesDefinition_Tables.Where(z => z.CurrencyID == companyID).ToList().DefaultIfEmpty(new FabulousDB.DB_Tabels.Settings.Financial.Accounting.CurrenciesDefinition.CurrenciesDefinition_Table { ISOCode = "Egp" }).FirstOrDefault().ISOCode,
                            DocType = "SID"
                        },
                        ShowTransactions = ShowTransactions
                    }
                };


                return Json(Res);

            }
            catch (Exception ex)
            {
                return Json(ex.ToString(), JsonRequestBehavior.AllowGet);
            }
            return Json(db.Payable_gl_accounts.Where(x => x.Creditor_setting_id == VendorId).FirstOrDefault());
        }
        // GET: Payable/Payable_gl_account
        public ActionResult Index()
        {
            var Payable_gl_accounts = db.Payable_gl_accounts.Include(g => g.Account_payable).Include(g => g.Accrued_purchase).Include(g => g.Creditor_settings).Include(g => g.Payable_Group_setting).Include(g => g.Purchase).Include(g => g.Purchase_variance).Include(g => g.Returne).Include(g => g.Taken_discount);
            return View(Payable_gl_accounts.ToList());
        }

        // GET: Payable/Payable_gl_account/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Payable_gl_account Payable_gl_account = db.Payable_gl_accounts.Find(id);
            if (Payable_gl_account == null)
            {
                return HttpNotFound();
            }
            return View(Payable_gl_account);
        }

        // GET: Payable/Payable_gl_account/Create
        public ActionResult Create()
        {
            SetAccounts();
            return View();
        }

        // POST: Payable/Payable_gl_account/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Payable_gl_account Payable_gl_account)
        {

            if (ModelState.IsValid)
            {
                db.Payable_gl_accounts.Add(Payable_gl_account);
                db.SaveChanges();
                try
                {
                    dbM.C_CreateAccount_Tables.FirstOrDefault(x => x.C_AID == Payable_gl_account.Account_payable_id)
               .C_Prefix = Prefix.Pay.ToString();
                    db.SaveChanges();
                }
                catch
                {

                }
                return Json(Payable_gl_account.Id);
            }
            return Json(-1);
            SetAccounts(Payable_gl_account);
            return View(Payable_gl_account);
        }

        private void SetAccounts(Payable_gl_account Payable_gl_account = null)
        {
            if (Payable_gl_account == null)
            {
                Payable_gl_account = new Payable_gl_account
                {

                };
            }
            ViewBag.Account_payable_id = new SelectList(dbM.C_CreateAccount_Tables.Where(x =>
            (x.C_Prefix == Prefix.Pay.ToString() || x.C_Prefix == null)
            && x.ReconcileAccount == true
            && x.FinancialArea == true
            && x.PostingType == PostingType.BallanceSheet.ToString()).Select(x => new { x.C_AID, AccountID = x.AccountName + " " + x.AccountID }), "C_AID", "AccountID", Payable_gl_account.Account_payable_id);

            ViewBag.Accrued_purchase_id = new SelectList(dbM.C_CreateAccount_Tables.Where(x =>
            x.PostingType == PostingType.BallanceSheet.ToString()
            && x.C_Prefix == null
            && x.FinancialArea == true).Select(x => new { x.C_AID, AccountID = x.AccountName + " " + x.AccountID }), "C_AID", "AccountID", Payable_gl_account.Accrued_purchase_id);

            ViewBag.Purchase_id = new SelectList(dbM.C_CreateAccount_Tables.Where(x =>
            x.C_Prefix == null
            //&& x.PostingType == PostingType.PL.ToString()
            && x.FinancialArea == true).Select(x => new { x.C_AID, AccountID = x.AccountName + " " + x.AccountID }), "C_AID", "AccountID", Payable_gl_account.Purchase_id);

            ViewBag.Purchase_variance_id = new SelectList(dbM.C_CreateAccount_Tables.Where(x =>
            x.C_Prefix == null
            //&& x.PostingType == PostingType.PL.ToString()
            && x.FinancialArea == true).Select(x => new { x.C_AID, AccountID = x.AccountName + " " + x.AccountID }), "C_AID", "AccountID", Payable_gl_account.Purchase_variance_id);

            ViewBag.Returne_id = new SelectList(dbM.C_CreateAccount_Tables.Where(x =>
            x.C_Prefix == null
            //&& x.PostingType == PostingType.PL.ToString()
            && x.FinancialArea == true).Select(x => new { x.C_AID, AccountID = x.AccountName + " " + x.AccountID }), "C_AID", "AccountID", Payable_gl_account.Returne_id);

            ViewBag.Taken_discount_id = new SelectList(dbM.C_CreateAccount_Tables.Where(x =>
            x.C_Prefix == null
            //&& x.PostingType == PostingType.PL.ToString()
            && x.FinancialArea == true).Select(x => new { x.C_AID, AccountID = x.AccountName + " " + x.AccountID }), "C_AID", "AccountID", Payable_gl_account.Taken_discount_id);
        }

        // GET: Payable/Payable_gl_account/Edit/5
        public ActionResult Edit(int? GroupSetting = null, int? CreditorId = null)
        {
            Payable_gl_account Payable_gl_account = Enumerable.Empty<Payable_gl_account>().FirstOrDefault();
            if (GroupSetting != null)
            {
                Payable_gl_account = db.Payable_gl_accounts.FirstOrDefault(x => x.Payable_Group_setting_id == GroupSetting);
            }
            else if (CreditorId != null)
            {
                Payable_gl_account = db.Payable_gl_accounts.FirstOrDefault(x => x.Creditor_setting_id == CreditorId);
            }
            if (Payable_gl_account == null)
            {
                return HttpNotFound();
            }
            SetAccounts(Payable_gl_account);
            return View(Payable_gl_account);
        }

        // POST: Payable/Payable_gl_account/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Payable_gl_account Payable_gl_account)
        {
            if (ModelState.IsValid)
            {
                db.Entry(Payable_gl_account).State = EntityState.Modified;
                db.Entry(Payable_gl_account).Property(x => x.Creditor_setting_id).IsModified = false;
                db.Entry(Payable_gl_account).Property(x => x.Payable_Group_setting_id).IsModified = false;
                db.SaveChanges();
                return Json(Payable_gl_account.Id);
            }
            SetAccounts(Payable_gl_account);
            return View(Payable_gl_account);
        }

        // GET: Payable/Payable_gl_account/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Payable_gl_account Payable_gl_account = db.Payable_gl_accounts.Find(id);
            if (Payable_gl_account == null)
            {
                return HttpNotFound();
            }
            return View(Payable_gl_account);
        }

        // POST: Payable/Payable_gl_account/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Payable_gl_account Payable_gl_account = db.Payable_gl_accounts.Find(id);
            db.Payable_gl_accounts.Remove(Payable_gl_account);
            db.SaveChanges();
            return RedirectToAction("Index");
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
