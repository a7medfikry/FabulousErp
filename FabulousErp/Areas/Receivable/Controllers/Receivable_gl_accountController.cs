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
using FabulousErp.Bussiness;
using FabulousErp.Receivable.Models;
using FabulousDB.DB_Context;
using FabulousDB.Models;
using FabulousErp;

namespace Receivable.Controllers
{
    public class Receivable_gl_accountController : Controller
    {
        private DBContext db = new DBContext();
        private DBContext dbM = new DBContext();
        public JsonResult GetTransactionAccount(int VendorId
            , decimal Sales, decimal Discount, decimal Total
            , decimal Orginal_amount, decimal Receivable, int BookId
            , Doc_type Doc_type, decimal Transaction_rate,
            string ThisCurrIso, bool ShowSalesBtn = true, int? ItemId = null, bool IsInv = false)
        {
            try
            {
                if (Transaction_rate == 0)
                {
                    Transaction_rate = 1;
                }
                string companyID = (string)FabulousErp.Business.GetCompanyId();
                IQueryable<Receivable_gl_account> GlAccounts = db.Receivable_gl_accounts.Where(x => x.Creditor_setting_id == VendorId);
                Inv_item_gl_accounts ItemAcc = Enumerable.Empty<Inv_item_gl_accounts>().FirstOrDefault();
                if (IsInv)
                {
                    ItemAcc = db.Inv_item_gl_accounts.Include(x => x.Sales_return)
                        .Include(x => x.Sales)
                        .FirstOrDefault(x => x.Item_id == ItemId);
                }
                List<JvHeaderDet> Res = new List<JvHeaderDet>();
                if (Doc_type == Doc_type.Invoice || Doc_type == Doc_type.Debit_Memo)
                {
                    var asdasd = GlAccounts.Include(x => x.Sales)
                   .Include(x => x.Discount)
                   .Include(x => x.Account_Receivable)
                   .ToList();
                    Res = GlAccounts.Include(x => x.Sales)
                   .Include(x => x.Discount)
                   .Include(x => x.Account_Receivable)
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
                                AID=(x.Discount!=null)?x.Discount.C_AID:0,
                                Debit=Discount,
                                Credit= 0,
                                AccountID=(x.Discount!=null)? x.Discount.AccountID:"",
                                AccountName = (x.Discount!=null)?x.Discount.AccountName:"",
                                Orginal_debit=Discount/Transaction_rate,
                                Orginal_curr=ThisCurrIso,
                                ShowBtn=ShowSalesBtn
                           },
                           new JvManyAccountFormate
                           {

                                AID=(!IsInv)?((x.Sales!=null)?x.Sales.C_AID:0)
                                            :(ItemAcc.Sales_id.HasValue)?ItemAcc.Sales.C_AID:0,
                                Debit= 0,
                                Credit=Sales,
                                AccountID =(!IsInv)?((x.Sales!=null)?x.Sales.AccountID:""):
                                                    ((ItemAcc.Sales_id.HasValue)?ItemAcc.Sales.AccountID:""),
                                AccountName =(!IsInv)?((x.Sales!=null)?x.Sales.AccountName:""):
                                                      ((ItemAcc.Sales_id.HasValue)?ItemAcc.Sales.AccountName:"") ,
                                Orginal_credit=Sales/Transaction_rate,
                                Orginal_curr=ThisCurrIso,
                                ShowBtn=ShowSalesBtn,
                                Mark="Sales"
                           },
                           new JvManyAccountFormate
                           {
                               AID=x.Account_Receivable.C_AID,
                                Debit=Total,
                                Credit= 0,
                                AccountID= x.Account_Receivable.AccountID,
                                AccountName = x.Account_Receivable.AccountName,
                                ShowBtn=false,
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
                                ShowBtn=false,
                                Orginal_debit=0,
                                Orginal_curr=ThisCurrIso
                           }
                       }

                   }).ToList();
                }
                else if (Doc_type == Doc_type.Credit_Memo)
                {
                    Res = GlAccounts.Include(x => x.Sales)
                   .Include(x => x.Discount)
                   .Include(x => x.Account_Receivable)
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
                               AID=(!IsInv)?((x.Sales!=null)?x.Sales.C_AID:0):
                                            (ItemAcc.Sales_id.HasValue)?ItemAcc.Sales.C_AID:0,
                                Debit= Sales,
                                Credit= 0,
                                AccountID =(!IsInv)?((x.Sales!=null)?x.Sales.AccountID:""):
                                                     (ItemAcc.Sales_id.HasValue)?ItemAcc.Sales.AccountID:"",
                                AccountName =(!IsInv)?((x.Sales!=null)?x.Sales.AccountName:""):
                                                      (ItemAcc.Sales_id.HasValue)?ItemAcc.Sales.AccountID:"",
                                Orginal_debit=Sales/Transaction_rate,
                                Orginal_curr=ThisCurrIso,
                                Mark="Sales"

                           },
                           new JvManyAccountFormate
                           {

                                  AID=(x.Discount!=null)?x.Discount.C_AID:0,
                                Debit=0,
                                Credit= Discount,
                                AccountID=(x.Discount!=null)?x.Discount.AccountID:"",
                                AccountName =(x.Discount!=null)?x.Discount.AccountName:"",
                                Orginal_credit=Discount/Transaction_rate,
                                Orginal_curr=ThisCurrIso

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


                           },
                           new JvManyAccountFormate
                           {

                                AID=x.Account_Receivable.C_AID,
                                Debit=0,
                                Credit=Total,
                                AccountID= x.Account_Receivable.AccountID,
                                AccountName = x.Account_Receivable.AccountName,
                                Orginal_credit=Total/Transaction_rate,
                                Orginal_curr=ThisCurrIso,
                                Mark="Vendore"

                           }


                       }

                   }).ToList();
                }
                else if (Doc_type == Doc_type.Return)
                {
                    Res = GlAccounts.Include(x => x.Returne)
                   .Include(x => x.Discount)
                   .Include(x => x.Account_Receivable)
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
                                AID=(!IsInv)?
                                ((x.Returne!=null)?x.Returne.C_AID:0):
                                (ItemAcc.Sales_return_id.HasValue)?ItemAcc.Sales_return_id.Value:0,
                                Debit=Sales,
                                Credit= 0,
                                AccountID =(!IsInv)?((x.Returne!=null)? x.Returne.AccountID:""):
                                 (ItemAcc.Sales_return_id.HasValue)?ItemAcc.Sales_return.AccountID:"",
                                AccountName =(!IsInv)?((x.Returne!=null)? x.Returne.AccountName:""):
                                            (ItemAcc.Sales_return_id.HasValue)?ItemAcc.Sales_return.AccountName:"" ,
                                Orginal_debit=Sales/Transaction_rate,
                                Orginal_curr=ThisCurrIso,
                                Mark="Sales"
                           },
                           new JvManyAccountFormate
                           {
                                 AID=(x.Discount!=null)? x.Discount.C_AID:0,
                                Debit=0,
                                Credit= Discount,
                                AccountID=(x.Discount!=null)? x.Discount.AccountID:"",
                                AccountName =(x.Discount!=null)? x.Discount.AccountName:"",
                                Orginal_credit=Discount/Transaction_rate,
                                Orginal_curr=ThisCurrIso


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

                           },
                           new JvManyAccountFormate
                           {
                               AID=x.Account_Receivable.C_AID,
                                Debit= 0,
                                Credit=Total,
                                AccountID= x.Account_Receivable.AccountID,
                                AccountName = x.Account_Receivable.AccountName,
                                Orginal_credit=Total/Transaction_rate,
                                Orginal_curr=ThisCurrIso,
                                Mark="Vendore"
                           }
                       }
                   }).ToList();
                }
                if (db.Receivable_other_options.Where(x => x.Option == Other_option_enum.Active_payment)
                    .ToList().DefaultIfEmpty(new Receivable_other_option { Checked = false }).FirstOrDefault().Checked
                    || db.Receivable_vendore_settings.Find(VendorId).Credit_limit == Credit_limit_enum.No_credit
                    || db.Receivable_vendore_settings.Find(VendorId).Credit_limit == Credit_limit_enum.Amount)
                {
                    Res.AddRange(PaymentAccount(VendorId, Orginal_amount, Discount, Receivable, BookId, Transaction_rate, ThisCurrIso, true));
                }
                return Json(Res);

            }
            catch (Exception ex)
            {
                return Json(ex.ToString(), JsonRequestBehavior.AllowGet);
            }
            return Json(db.Receivable_gl_accounts.Where(x => x.Creditor_setting_id == VendorId).FirstOrDefault());
        }

        public JsonResult GetTotalJv(decimal Total, int VendorId, string ThisCurrIso, decimal Transaction_rate, Doc_type Doc_type)
        {
            IQueryable<Receivable_gl_account> GlAccounts = db.Receivable_gl_accounts.Where(x => x.Creditor_setting_id == VendorId);
            List<JvHeaderDet> Res = new List<JvHeaderDet>();
            string companyID = FabulousErp.Business.GetCompanyId();
            if (Transaction_rate == 0)
            {
                Transaction_rate = 1;
            }
            if (Doc_type == Doc_type.Invoice || Doc_type == Doc_type.Debit_Memo)
            {
                Res = GlAccounts
                  .Include(x => x.Sales)
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
                              AID = x.Sales.C_AID,
                              Debit = 0,
                              Credit = Total,
                              AccountID = x.Sales.AccountID,
                              AccountName = x.Sales.AccountName,
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
                  .Include(x => x.Sales)
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
                               AID = x.Sales.C_AID,
                               Debit = Total,
                               Credit = 0,
                               AccountID = x.Sales.AccountID,
                               AccountName = x.Sales.AccountName,
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
        public JsonResult GetDiscountJV(decimal Discount, int VendorId
          , Doc_type Doc_type, string ThisCurrIso, decimal Transaction_rate)
        {
            string companyID = FabulousErp.Business.GetCompanyId();
            if (Transaction_rate == 0)
            {
                Transaction_rate = 1;
            }
            if (Doc_type == Doc_type.Invoice)
            {
                JvHeaderDet Res = db.Receivable_gl_accounts.Include(x => x.Discount)
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
                                AID=(x.Discount!=null)?x.Discount.C_AID:0,
                                Debit=Discount,
                                Credit=0 ,
                                AccountID=(x.Discount!=null)? x.Discount.AccountID:"",
                                AccountName = (x.Discount!=null)?x.Discount.AccountName:"",
                                Orginal_debit=Discount/Transaction_rate,
                                Orginal_credit=0,
                                Orginal_curr=ThisCurrIso,
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

                JvHeaderDet Res = db.Receivable_gl_accounts.Include(x => x.Discount)
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
                                AID=(x.Discount!=null)?x.Discount.C_AID:0,
                                Debit=0,
                                Credit=Discount ,
                                AccountID=(x.Discount!=null)? x.Discount.AccountID:"",
                                AccountName = (x.Discount!=null)?x.Discount.AccountName:"",
                                Orginal_credit=Discount/Transaction_rate,
                                Orginal_curr=ThisCurrIso,
                           }
           }
       }).FirstOrDefault();
                return Json(Res);


            }


        }
        public JsonResult GetFrightJV(decimal FrightAmount, List<int> ItemsIds
          , Doc_type DocType, string ThisCurrIso, decimal Transaction_rate = 1)
        {
            string companyID = FabulousErp.Business.GetCompanyId();
            if (Transaction_rate == 0)
            {
                Transaction_rate = 1;
            }
            List<Inv_item_gl_accounts> Items = db.Inv_item_gl_accounts
              .Include(x => x.Fright).Include(x => x.item)
              .Where(x => ItemsIds.Any(z => z == x.item.Id))
              .ToList();
            try
            {
                if (DocType == Doc_type.Return)
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
                else
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
                                            Credit=0 ,
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

            }
            catch
            {

            }

            return Json(null);



        }
        public List<JvHeaderDet> PaymentAccount(int VendorId
            , decimal Orginal_amount, decimal Discount, decimal Receivable
            , int CheckBookId, decimal Transaction_rate, string ThisCurrIso, bool IsFromTransaction = false)
        {
            IQueryable<Receivable_gl_account> GlAccounts = db.Receivable_gl_accounts.Where(x => x.Creditor_setting_id == VendorId);
            List<JvHeaderDet> Res = new List<JvHeaderDet>();
            string companyID = (string)FabulousErp.Business.GetCompanyId();
            if (CheckBookId == 0)
            {
                return new List<JvHeaderDet> { };
            }
            if (Transaction_rate == 0)
            {
                Transaction_rate = 1;
            }
            C_CreateAccount_Table CheckBookAccount = dbM.C_CheckBookSetting_Tables.Find(CheckBookId).C_CreateAccount_Table;
            Res = GlAccounts
                  .Include(x => x.Discount)
                  .Include(x => x.Account_Receivable)
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
                                AID=(CheckBookAccount!=null)?CheckBookAccount.C_AID:0,
                                Debit=Orginal_amount,
                                Credit=  0,
                                AccountID = (CheckBookAccount!=null)?CheckBookAccount.AccountID:"",
                                AccountName =(CheckBookAccount!=null)? CheckBookAccount.AccountName:"",
                                ShowBtn=false,
                                Orginal_debit=Orginal_amount/Transaction_rate,
                                Orginal_curr=ThisCurrIso,
                                Mark="CheckBook"
                           },
                           new JvManyAccountFormate
                           {
                                AID =0,
                                Debit=0,
                                Credit= 0,
                                AccountID= "",
                                AccountName = "",
                                ShowBtn=false,
                                Orginal_debit=0,
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
                                Orginal_credit=0,
                                Orginal_curr=ThisCurrIso

                           },
                           new JvManyAccountFormate
                           {
                               AID=x.Account_Receivable.C_AID,
                                Debit=0 ,
                                Credit=Receivable ,
                                AccountID =x.Account_Receivable.AccountID,
                                AccountName = x.Account_Receivable.AccountName,
                                ShowBtn=false,
                                Orginal_credit=Receivable/Transaction_rate,
                                Orginal_curr=ThisCurrIso,
                                Mark="Receivable"
                           }


                      }

                  })
                  .ToList();
            if (!IsFromTransaction)
            {
                Res.AddRange(GlAccounts
                 .Include(x => x.Discount)
                 .Include(x => x.Account_Receivable)
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

                            AID = (x.Discount!=null)?x.Discount.C_AID:0,
                            Debit =Discount,
                            Credit = 0,
                            AccountID = (x.Discount!=null)?x.Discount.AccountID:"",
                            AccountName =(x.Discount!=null)? x.Discount.AccountName:"",
                            Orginal_debit=Discount/Transaction_rate,
                                Orginal_curr=ThisCurrIso
                        },
                        new JvManyAccountFormate
                        {
                                AID = 0,
                            Debit = 0,
                            Credit = 0,
                            AccountID = "",
                            AccountName = "",
                            Orginal_debit=0,
                                Orginal_curr=ThisCurrIso

                        }
                     }
                 }));
            }
            return Res;
        }
        public JsonResult GetPaymentAccount(int VendorId
            , decimal Orginal_amount, decimal Discount, decimal Receivable
            , int CheckBookId, decimal Transaction_rate, string CurrencyIso)
        {
            if (Transaction_rate == 0)
            {
                Transaction_rate = 1;
            }
            return Json(PaymentAccount(VendorId, Orginal_amount, Discount, Receivable, CheckBookId, Transaction_rate, CurrencyIso));
        }
        public JsonResult GetInvCheckBookAccount(Doc_type Doc_type,int CheckBookId, int ItemId, decimal Amount, decimal Transaction_rate = 1)
        {
            List<JvHeaderDet> Res = new List<JvHeaderDet>();
            string companyID = FabulousErp.Business.GetCompanyId();
            C_CreateAccount_Table CheckBookAccount = dbM.C_CheckBookSetting_Tables.Find(CheckBookId).C_CreateAccount_Table;
            string ThisCurrIso = db.CompanyMainInfo_Tables.FirstOrDefault(x => x.CompanyID == companyID)
                .Currency;

            Inv_item_gl_accounts ItemAcc = db.Inv_item_gl_accounts.Include(x => x.Sales_return)
                       .Include(x => x.Sales)
                       .FirstOrDefault(x => x.Item_id == ItemId);

            if (Doc_type == Doc_type.Invoice)
            {
                Res = new List<JvHeaderDet>{
                new JvHeaderDet
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
                                AID=(CheckBookAccount!=null)?CheckBookAccount.C_AID:0,
                                Debit=Amount,
                                Credit=  0,
                                AccountID = (CheckBookAccount!=null)?CheckBookAccount.AccountID:"",
                                AccountName =(CheckBookAccount!=null)? CheckBookAccount.AccountName:"",
                                ShowBtn=false,
                                Orginal_debit=Amount/Transaction_rate,
                                Orginal_curr=ThisCurrIso,
                                Mark="CheckBook"
                           },
                           new JvManyAccountFormate
                           {
                                AID =0,
                                Debit=0,
                                Credit= 0,
                                AccountID= "",
                                AccountName = "",
                                ShowBtn=false,
                                Orginal_debit=0,
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
                                Orginal_credit=0,
                                Orginal_curr=ThisCurrIso

                           },
                           new JvManyAccountFormate
                           {
                                AID=ItemAcc.Sales.C_AID,
                                Debit= 0,
                                Credit=Amount,
                                AccountID =ItemAcc.Sales.AccountID,
                                AccountName =ItemAcc.Sales.AccountName,
                                Orginal_credit=Amount/Transaction_rate,
                                Orginal_curr=ThisCurrIso,
                                ShowBtn=false,
                                Mark="Sales"
                           }
                     }

                 }
                };
            }
            else
            {
                Res = new List<JvHeaderDet>{
                new JvHeaderDet
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
                                 AID =0,
                                Debit=0,
                                Credit= 0,
                                AccountID= "",
                                AccountName = "",
                                ShowBtn=false,
                                Orginal_debit=0,
                                Orginal_curr=ThisCurrIso
                           },
                           new JvManyAccountFormate
                           {
                                AID=(CheckBookAccount!=null)?CheckBookAccount.C_AID:0,
                                Debit=0,
                                Credit=Amount,
                                AccountID = (CheckBookAccount!=null)?CheckBookAccount.AccountID:"",
                                AccountName =(CheckBookAccount!=null)? CheckBookAccount.AccountName:"",
                                ShowBtn=false,
                                Orginal_credit=Amount/Transaction_rate,
                                Orginal_curr=ThisCurrIso,
                                Mark="CheckBook"
                           },
                           new JvManyAccountFormate
                           {
                                  AID=ItemAcc.Sales.C_AID,
                                Debit=Amount,
                                Credit=0,
                                AccountID =ItemAcc.Sales.AccountID,
                                AccountName =ItemAcc.Sales.AccountName,
                                Orginal_debit=Amount/Transaction_rate,
                                Orginal_curr=ThisCurrIso,
                                ShowBtn=false,
                                Mark="Sales"
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
                                Orginal_curr=ThisCurrIso
                           }
                     }

                 }
                };
            }
            return Json(Res);
        }

        public JsonResult GetAssignEarnAndLose(int VendorId
            , List<decimal> TakenDiscount, List<decimal> EarnAndLoss, List<string> Currencies, List<decimal> Rates,
             string MainCurr)
        {
            try
            {

                string companyID = (string)FabulousErp.Business.GetCompanyId();
                IQueryable<Receivable_gl_account> GlAccounts = db.Receivable_gl_accounts.Where(x => x.Creditor_setting_id == VendorId).Include(x => x.Account_Receivable).Include(x => x.Discount);
                List<CurrenciesDefinition_Tables> CDT = dbM.AccountCurrencyDefinition_Tables.Include(x => x.C_CreateAccount_Table).ToList();

                List<JvManyAccountFormate> EarnDebit = new List<JvManyAccountFormate>();
                List<JvManyAccountFormate> SumEarnDebit = new List<JvManyAccountFormate>();
                List<JvManyAccountFormate> EarnCredit = new List<JvManyAccountFormate>();
                List<JvManyAccountFormate> SumEarnCredit = new List<JvManyAccountFormate>();
                List<JvManyAccountFormate> TakenDiscountSum = new List<JvManyAccountFormate>();

                List<JvManyAccountFormate> ReceivableDebit = new List<JvManyAccountFormate>();
                List<JvManyAccountFormate> ReceivableCredit = new List<JvManyAccountFormate>();

                List<JvManyAccountFormate> ReceivableDebitSum = new List<JvManyAccountFormate>();
                List<JvManyAccountFormate> ReceivableCreditSum = new List<JvManyAccountFormate>();

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
                            ReceivableDebit.Add(new JvManyAccountFormate
                            {
                                AID = GlAccounts.FirstOrDefault().Account_Receivable.C_AID,
                                Debit = (TakenDiscount[i] - EarnAndLoss[i]),
                                Credit = 0,
                                AccountID = GlAccounts.FirstOrDefault().Account_Receivable.AccountID,
                                AccountName = GlAccounts.FirstOrDefault().Account_Receivable.AccountName,
                                Orginal_debit = (TakenDiscount[i] - EarnAndLoss[i]),
                                Orginal_curr = OrginalCurr,
                                ShowBtn = false
                            });
                        }
                        else
                        {
                            ReceivableCredit.Add(new JvManyAccountFormate
                            {
                                AID = GlAccounts.FirstOrDefault().Account_Receivable.C_AID,
                                Debit = 0,
                                Credit = TakenDiscount[i] - EarnAndLoss[i],
                                AccountID = GlAccounts.FirstOrDefault().Account_Receivable.AccountID,
                                AccountName = GlAccounts.FirstOrDefault().Account_Receivable.AccountName,
                                Orginal_credit = (TakenDiscount[i] - EarnAndLoss[i]),
                                Orginal_curr = OrginalCurr,
                                ShowBtn = false

                            });
                        }

                        TakenDiscountCredit.Add(new JvManyAccountFormate
                        {
                            AID = GlAccounts.FirstOrDefault().Discount.C_AID,
                            Debit = 0,
                            Credit = TakenDiscount[i],
                            AccountID = GlAccounts.FirstOrDefault().Discount.AccountID,
                            AccountName = GlAccounts.FirstOrDefault().Discount.AccountName,
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
                        AID = GlAccounts.FirstOrDefault().Discount.C_AID,
                        Debit = 0,
                        Credit = i.Sum(x => x.Credit),
                        AccountID = GlAccounts.FirstOrDefault().Discount.AccountID,
                        AccountName = GlAccounts.FirstOrDefault().Discount.AccountName,
                        Orginal_credit = i.Sum(x => x.Orginal_credit),
                        Orginal_curr = MainIso// i.FirstOrDefault().Orginal_curr
                    });
                }
                foreach (List<JvManyAccountFormate> i in ReceivableDebit.GroupBy(x => x.Orginal_curr).Select(x => x.ToList()))
                {
                    ReceivableDebitSum.Add(new JvManyAccountFormate
                    {
                        AID = GlAccounts.FirstOrDefault().Account_Receivable.C_AID,
                        Debit = i.Sum(x => x.Debit),
                        Credit = 0,
                        AccountID = GlAccounts.FirstOrDefault().Account_Receivable.AccountID,
                        AccountName = GlAccounts.FirstOrDefault().Account_Receivable.AccountName,
                        Orginal_debit = i.Sum(x => x.Orginal_debit),
                        Orginal_curr = i.FirstOrDefault().Orginal_curr,
                        ShowBtn = false

                    });
                }
                foreach (List<JvManyAccountFormate> i in ReceivableCredit.GroupBy(x => x.Orginal_curr).Select(x => x.ToList()))
                {
                    try
                    {
                        ReceivableCreditSum.Add(new JvManyAccountFormate
                        {
                            AID = CDT.Where(x => x.Type == "Profit").ToList().DefaultIfEmpty(new CurrenciesDefinition_Tables { C_CreateAccount_Table = new C_CreateAccount_Table { C_AID = -1 } }).FirstOrDefault().C_CreateAccount_Table.C_AID,
                            Debit = 0,
                            Credit = i.Sum(x => x.Credit),
                            AccountID = CDT.Where(x => x.Type == "Profit").ToList().DefaultIfEmpty(new CurrenciesDefinition_Tables { C_CreateAccount_Table = new C_CreateAccount_Table { AccountID = "00-00" } }).FirstOrDefault().C_CreateAccount_Table.AccountID,
                            AccountName = CDT.Where(x => x.Type == "Profit").ToList().DefaultIfEmpty(new CurrenciesDefinition_Tables { C_CreateAccount_Table = new C_CreateAccount_Table { AccountName = "No Receivable Acc" } }).FirstOrDefault().C_CreateAccount_Table.AccountName,
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

                ShowTransactions.AddRange(ReceivableDebitSum.DefaultIfEmpty(new JvManyAccountFormate { }));
                ShowTransactions.AddRange(ReceivableCreditSum.DefaultIfEmpty(new JvManyAccountFormate { }));

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
            return Json(db.Receivable_gl_accounts.Where(x => x.Creditor_setting_id == VendorId).FirstOrDefault());
        }
        // GET: Receivable/Receivable_gl_account
        public ActionResult Index()
        {
            var Receivable_gl_accounts = db.Receivable_gl_accounts.Include(g => g.Account_Receivable).Include(g => g.Accrued_sales).Include(g => g.Creditor_settings).Include(g => g.Receivable_Group_setting).Include(g => g.Sales).Include(g => g.Sales_variance).Include(g => g.Returne).Include(g => g.Discount);
            return View(Receivable_gl_accounts.ToList());
        }

        // GET: Receivable/Receivable_gl_account/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Receivable_gl_account Receivable_gl_account = db.Receivable_gl_accounts.Find(id);
            if (Receivable_gl_account == null)
            {
                return HttpNotFound();
            }
            return View(Receivable_gl_account);
        }

        // GET: Receivable/Receivable_gl_account/Create
        public ActionResult Create()
        {
            SetAccounts();
            return View();
        }

        // POST: Receivable/Receivable_gl_account/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Receivable_gl_account Receivable_gl_account)
        {

            if (ModelState.IsValid)
            {
                db.Receivable_gl_accounts.Add(Receivable_gl_account);
                db.SaveChanges();
                try
                {
                    dbM.C_CreateAccount_Tables.FirstOrDefault(x => x.C_AID == Receivable_gl_account.Account_Receivable_id)
               .C_Prefix = PayRecPrefix.Rec.ToString();
                    db.SaveChanges();
                }
                catch
                {

                }
                return Json(Receivable_gl_account.Id);
            }
            return Json(-1);
            SetAccounts(Receivable_gl_account);
            return View(Receivable_gl_account);
        }

        private void SetAccounts(Receivable_gl_account Receivable_gl_account = null)
        {
            if (Receivable_gl_account == null)
            {
                Receivable_gl_account = new Receivable_gl_account
                {

                };
            }
            ViewBag.Account_Receivable_id = new SelectList(dbM.C_CreateAccount_Tables.Where(x =>
            (x.C_Prefix == PayRecPrefix.Rec.ToString() || x.C_Prefix == null)
            && x.ReconcileAccount == true
            && x.FinancialArea == true
            && x.PostingType == PostingType.BallanceSheet.ToString()).Select(x => new { x.C_AID, AccountID = x.AccountName + " " + x.AccountID }), "C_AID", "AccountID", Receivable_gl_account.Account_Receivable_id);

            ViewBag.Accrued_Sales_id = new SelectList(dbM.C_CreateAccount_Tables.Where(x =>
            x.PostingType == PostingType.BallanceSheet.ToString()
            && x.C_Prefix == null
            && x.FinancialArea == true).Select(x => new { x.C_AID, AccountID = x.AccountName + " " + x.AccountID }), "C_AID", "AccountID", Receivable_gl_account.Accrued_sales_id);

            ViewBag.Sales_id = new SelectList(dbM.C_CreateAccount_Tables.Where(x =>
            x.C_Prefix == null
            //&& x.PostingType == PostingType.PL.ToString()
            && x.FinancialArea == true).Select(x => new { x.C_AID, AccountID = x.AccountName + " " + x.AccountID }), "C_AID", "AccountID", Receivable_gl_account.Sales_id);

            ViewBag.Sales_variance_id = new SelectList(dbM.C_CreateAccount_Tables.Where(x =>
            x.C_Prefix == null
            //&& x.PostingType == PostingType.PL.ToString()
            && x.FinancialArea == true).Select(x => new { x.C_AID, AccountID = x.AccountName + " " + x.AccountID }), "C_AID", "AccountID", Receivable_gl_account.Sales_variance_id);

            ViewBag.Returne_id = new SelectList(dbM.C_CreateAccount_Tables.Where(x =>
            x.C_Prefix == null
            //&& x.PostingType == PostingType.PL.ToString()
            && x.FinancialArea == true).Select(x => new { x.C_AID, AccountID = x.AccountName + " " + x.AccountID }), "C_AID", "AccountID", Receivable_gl_account.Returne_id);

            ViewBag.Discount_id = new SelectList(dbM.C_CreateAccount_Tables.Where(x =>
            x.C_Prefix == null
            //&& x.PostingType == PostingType.PL.ToString()
            && x.FinancialArea == true).Select(x => new { x.C_AID, AccountID = x.AccountName + " " + x.AccountID }), "C_AID", "AccountID", Receivable_gl_account.Discount_id);
        }

        // GET: Receivable/Receivable_gl_account/Edit/5
        public ActionResult Edit(int? GroupSetting = null, int? CreditorId = null)
        {
            Receivable_gl_account Receivable_gl_account = Enumerable.Empty<Receivable_gl_account>().FirstOrDefault();
            if (GroupSetting != null)
            {
                Receivable_gl_account = db.Receivable_gl_accounts.FirstOrDefault(x => x.Receivable_Group_setting_id == GroupSetting);
            }
            else if (CreditorId != null)
            {
                Receivable_gl_account = db.Receivable_gl_accounts.FirstOrDefault(x => x.Creditor_setting_id == CreditorId);
            }
            if (Receivable_gl_account == null)
            {
                return HttpNotFound();
            }
            SetAccounts(Receivable_gl_account);
            return View(Receivable_gl_account);
        }

        // POST: Receivable/Receivable_gl_account/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Receivable_gl_account Receivable_gl_account)
        {
            if (ModelState.IsValid)
            {
                db.Entry(Receivable_gl_account).State = EntityState.Modified;
                db.Entry(Receivable_gl_account).Property(x => x.Creditor_setting_id).IsModified = false;
                db.Entry(Receivable_gl_account).Property(x => x.Receivable_Group_setting_id).IsModified = false;
                db.SaveChanges();
                return Json(Receivable_gl_account.Id);
            }
            SetAccounts(Receivable_gl_account);
            return View(Receivable_gl_account);
        }

        // GET: Receivable/Receivable_gl_account/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Receivable_gl_account Receivable_gl_account = db.Receivable_gl_accounts.Find(id);
            if (Receivable_gl_account == null)
            {
                return HttpNotFound();
            }
            return View(Receivable_gl_account);
        }

        // POST: Receivable/Receivable_gl_account/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Receivable_gl_account Receivable_gl_account = db.Receivable_gl_accounts.Find(id);
            db.Receivable_gl_accounts.Remove(Receivable_gl_account);
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
