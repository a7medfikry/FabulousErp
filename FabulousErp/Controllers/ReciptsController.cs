using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FabulousDB.DB_Tabels.Settings.Financial.GeneralSetting.C_B_F_Setup.CompanyInformation;
using FabulousModels.ViewModels;
using System.Data.Entity;
using FabulousDB.DB_Context;
using FabulousDB.DB_Tabels.Transaction.Financial.Company.Accounting;
using FabulousDB.DB_Tabels.Settings.Financial.Accounting.CurrenciesDefinition;
using FabulousErp.Payable.Models;
using FabulousErp.Receivable.Models;

namespace FabulousErp.Controllers
{
    public class ReciptsController : Controller
    {
        // GET: Recipts
        public ActionResult Recipt(string Action1, string Action2, List<ReciptsValues> Model1 = null
            , List<ReciptsValues> Model2 = null, bool? OnRecipt = false)
        {
            ViewBag.Action1 = Action1;
            ViewBag.Action2 = Action2;
            ViewBag.Model1 = Model1;
            ViewBag.Model2 = Model2;
            ViewBag.OneRecipt = OnRecipt;
            return View("~/Views/Recipts/Recipt.cshtml");
        }
        public PartialViewResult CheckBook2(List<ReciptsValues> RModel = null)
        {
            if (RModel == null)
            {
                RModel = FillModel();
            }
            return PartialView(RModel);
        }
        public PartialViewResult PayRecRecipts(int Id, bool IsPay = true, bool IsInv = false, int PO = 0)
        {
            List<PayRecRecipt> RModel = new List<PayRecRecipt>();
            FabulousDB.DB_Context.DBContext Mdb = new FabulousDB.DB_Context.DBContext();
            string CompanyId = Business.GetCompanyId();
            CompanyMainInfo_Table CM = Mdb.CompanyMainInfo_Tables
                .Include(x => x.C_TaxSetting_Tables).ToList()
                .FirstOrDefault(x => x.CompanyID == CompanyId);
            if (CM.CompanyCommInfo_Table == null)
            {
                CM.CompanyCommInfo_Table = new CompanyCommInfo_Table
                {

                };
            }
            if (CM.CompanyLegalInfo_Table == null)
            {
                CM.CompanyLegalInfo_Table = new CompanyLegalInfo_Table
                {

                };
            }
            List<FabulousDB.DB_Tabels.Tax.Tax> Tax =
                Enumerable.Empty<List<FabulousDB.DB_Tabels.Tax.Tax>>().FirstOrDefault();
            decimal? VatAmount = 0;
            decimal? Total_vat_amount = 0;
            decimal? Dacutta_amount = 0;
            ViewBag.IsPay = IsPay;
            ViewBag.IsInv = IsInv;
            ViewBag.InvItems = new List<FabulousModels.Inventory.PoItemsPrint>();
            if (IsPay)
            {
                using (DBContext db = new DBContext())
                {
                    Payable.Models.Payable_transaction P = db.Payable_transactions
                        .Include(x => x.Currency).Include(x => x.Vendor)
                        .Include(x => x.Trans_doc_type)
                        .FirstOrDefault(x => x.Id == Id);

                    if (P == null)
                    {
                        CurrenciesDefinition_Table C = db.CurrenciesDefinition_Tables
                            .FirstOrDefault();
                        if (PO != 0)
                        {
                            C_CheckbookTransactions_table CB = db.C_CheckbookTransactions_Tables.Where(x => x.C_PostingNumber == PO)
                                .FirstOrDefault();

                            P = new Payable.Models.Payable_transaction
                            {
                                Purchase = (CB.C_Balance > 0) ? CB.C_Balance : -CB.C_Balance,
                                Currency = C,
                                Doc_type = FabulousDB.Models.Doc_type.Return,
                                Vendor = new Payable.Models.Payable_creditor_setting
                                {
                                    Payable_address_info = new List<FabulousErp.Payable.Models.Payable_address_info>
                                    {
                                        new FabulousErp.Payable.Models.Payable_address_info
                                        {

                                        }
                                    },
                                    Payable_legal_info = new List<Payable_legal_info>
                                    {
                                        new Payable_legal_info
                                        {

                                        }
                                    }
                                },
                                Trans_doc_type = new Payable.Models.Payable_transactions_types
                                {

                                }
                            };

                        }
                        else
                        {
                            P = new Payable.Models.Payable_transaction
                            {
                                Currency = C,
                                Doc_type = FabulousDB.Models.Doc_type.Return,
                                Vendor = new Payable.Models.Payable_creditor_setting
                                {
                                    Payable_address_info = new List<FabulousErp.Payable.Models.Payable_address_info>
                                    {
                                        new FabulousErp.Payable.Models.Payable_address_info
                                        {

                                        }

                                    },
                                    Payable_legal_info = new List<Payable_legal_info>
                                    {
                                        new Payable_legal_info
                                        {

                                        }
                                    },
                                    
                                },
                                Trans_doc_type = new Payable.Models.Payable_transactions_types
                                {

                                },
                            };
                        }
                    }
                    Tax = Mdb.Taxs.Where(x => x.Journal_number == P.Journal_number)
                        .ToList().DefaultIfEmpty(new FabulousDB.DB_Tabels.Tax.Tax { })
                        .ToList();
                    VatAmount = Tax.Sum(x => x.Vat_amount);
                    Total_vat_amount = Tax.Sum(x => x.Total_vat_amount);
                    Dacutta_amount = Tax.Sum(x => x.Dacutta_amount);
                    if (!VatAmount.HasValue)
                    {
                        VatAmount = 0;
                    }
                    if (!Total_vat_amount.HasValue)
                    {
                        Total_vat_amount = 0;
                    }
                    if (!Dacutta_amount.HasValue)
                    {
                        Dacutta_amount = 0;
                    }
                    if (P.Purchase < 0)
                    {
                        P.Purchase = -P.Purchase;
                    }
                    decimal Total = (P.Purchase - P.Taken_discount + Total_vat_amount.Value - Dacutta_amount.Value);
                    string Moneytext = Bussiness.ConvertNumberToText.ConvertNumberToAlpha(Total.ToString(), P.Currency.Currency_unit_name, P.Currency.Currency_small_unit_name);

                    RModel.AddRange(FillPayRec(CM.CompanyName, Business.GetCompLogo(), P.Doc_type.ToString()
                        , P.Vendor.Vendor_name, P.Vendor.Payable_address_info.FirstOrDefault().Address,
                        P.Vendor.Payable_legal_info.FirstOrDefault().Tax_Id,
                        P.Vendor.Payable_address_info.FirstOrDefault().Phone_number,
                        P.Vendor.Payable_address_info.FirstOrDefault().Fax,
                        P.Vendor.Payable_legal_info.FirstOrDefault().Tax_file_no,
                        P.Trans_doc_type.Counter.ToString(),
                        P.Doc_date, P.Due_date, P.Trans_doc_type.Counter.ToString(),
                        P.Desc, P.Purchase, P.Taken_discount, Total_vat_amount, Dacutta_amount,
                        Total, Moneytext, "<span class='Right'> " + CM.AddressInformation_Table.City + "</span><span class='Right'>" + CM.AddressInformation_Table.Area + "</span> <span class='Right'>" + CM.AddressInformation_Table.BuldingNo + "</span>",
                        CM.C_TaxSetting_Tables.ToList().DefaultIfEmpty(new FabulousDB.DB_Tabels.Settings.Financial.GeneralSetting.Tax.C_TaxSetting_table { C_Taxcode = "" }).FirstOrDefault().C_Taxcode, CM.CompanyCommInfo_Table.TelephoneEX1 + "-" + CM.CompanyCommInfo_Table.Telephone1, CM.CompanyCommInfo_Table.FaxEX1 + CM.CompanyCommInfo_Table.Fax1,
                        CM.CompanyLegalInfo_Table.TaxFileNo, CM.CompanyLegalInfo_Table.CommericalRegister, CM.CompanyLegalInfo_Table.VatID
                        ));

                    if (IsInv)
                    {
                        if (P.Id == 0 && PO != 0)
                        {
                            List<FabulousModels.Inventory.PoItemsPrint> InvI = ViewBag.InvItems = db.Inv_sales_invoice_items.Include(x => x.Sales_invoice)
                                                     .Where(x => x.Sales_invoice.Posting_number == PO)
                                                     .Include(x => x.Item).Include(x => x.UOM)
                                                     .ToList().Select(x => new FabulousModels.Inventory.PoItemsPrint
                                                     {
                                                         Item_id = x.Item.Item_id,
                                                         Item_name = (x.Custom_name != null) ? x.Custom_name : x.Item.Short_description/*.Short_description*/,
                                                         Qty = x.Quantity,
                                                         Unit_price = Inventory.Controllers.InvBus.CalcItemEqUnitPrice(x.Item_id, x.UOM_id.Value, x.Quantity, x.Unit_price, true),
                                                         UOM = x.UOM.Unit_id,
                                                         Date = x.Sales_invoice.Transaction_date
                                                     }).ToList();

                            P.Due_date = P.Doc_date = InvI.FirstOrDefault().Date.Value;
                        }
                        else
                        {
                            ViewBag.InvItems = db.Inv_sales_invoice_items.Include(x => x.Sales_invoice)
                                                   .Where(x => x.Sales_invoice.Payable_id == Id)
                                                   .Include(x => x.Item).Include(x => x.UOM)
                                                   .ToList().Select(x => new FabulousModels.Inventory.PoItemsPrint
                                                   {
                                                       Item_id = x.Item.Item_id,
                                                       Item_name = (x.Custom_name != null) ? x.Custom_name : x.Item.Short_description/*.Short_description*/,
                                                       Qty = x.Quantity,
                                                       Unit_price = Inventory.Controllers.InvBus.CalcItemEqUnitPrice(x.Item_id, x.UOM_id.Value, x.Quantity, x.Unit_price, true),
                                                       UOM = x.UOM.Unit_id
                                                   }).ToList();
                        }


                        //if (P.Doc_type == FabulousDB.Models.Doc_type.Invoice)
                        //{
                        //    ViewBag.InvItems = db.Inv_sales_invoice_items.Include(x => x.Sales_invoice)
                        //                              .Where(x => x.Sales_invoice.Payable_id == Id)
                        //                              .Include(x => x.Item).Include(x => x.UOM)
                        //                              .Select(x => new FabulousModels.Inventory.PoItemsPrint
                        //                              {
                        //                                  Item_id = x.Item.Item_id,
                        //                                  Item_name = (x.Custom_name != null) ? x.Custom_name : x.Item.Short_description/*.Short_description*/,
                        //                                  Qty = x.Quantity,
                        //                                  Unit_price = x.Unit_price,
                        //                                  UOM = x.UOM.Unit_id
                        //                              }).ToList();
                        //}
                        //else
                        //{
                        //    var asd = db.Inv_sales_invoice_items.Include(x => x.Sales_invoice)
                        //   .ToList();
                        //    ViewBag.InvItems = db.Inv_sales_invoice.Include(x => x.Sales_invoice)
                        //   .Where(x => x.Sales_invoice.Payable_id == Id)
                        //   .Include(x => x.Item).Include(x => x.UOM)
                        //   .Select(x => new FabulousModels.Inventory.PoItemsPrint
                        //   {
                        //       Item_id = x.Item.Item_id,
                        //       Item_name = x.Item.Short_description,
                        //       Qty = x.Quantity,
                        //       Unit_price = x.Unit_price,
                        //       UOM = x.UOM.Unit_id
                        //   }).ToList();
                        //}
                    }

                    return PartialView(RModel);
                }

            }
            else
            {
                using (DBContext db = new DBContext())
                {
                    Receivable.Models.Receivable_transaction R = db.Receivable_transactions
                        .Include(x => x.Currency).Include(x => x.Vendor).Include(x => x.Trans_doc_type)
                        .ToList().DefaultIfEmpty(new Receivable.Models.Receivable_transaction { }).FirstOrDefault(x => x.Id == Id);
                    if (R == null)
                    {
                        int Jn = Business.GetJournalEntry(PO);
                        R = db.Receivable_transactions
                        .Include(x => x.Currency).Include(x => x.Vendor).Include(x => x.Trans_doc_type)
                        .ToList().DefaultIfEmpty(new Receivable.Models.Receivable_transaction { }).FirstOrDefault(x => x.Journal_number == Jn);
                    }
                    if (R == null)
                    {
                        CurrenciesDefinition_Table C = db.CurrenciesDefinition_Tables
                            .FirstOrDefault();
                        if (PO != 0)
                        {
                            C_CheckbookTransactions_table CB = db.C_CheckbookTransactions_Tables.Where(x => x.C_PostingNumber == PO)
                                .FirstOrDefault();

                            R = new Receivable.Models.Receivable_transaction
                            {
                                Purchase =(CB.C_Balance > 0) ? CB.C_Balance : -CB.C_Balance,
                                Currency = C,
                                Doc_type = FabulousDB.Models.Doc_type.Invoice,
                                Vendor = new Receivable.Models.Receivable_vendore_setting
                                {
                                    Receivable_address_info = new List<FabulousErp.Receivable.Models.Receivable_address_info>
                                    {
                                        new Receivable.Models.Receivable_address_info
                                        {

                                        }
                                    },
                                    Receivable_legal_info = new List<Receivable_legal_info>
                                    {
                                        new Receivable_legal_info
                                        {

                                        }
                                    }
                                },
                                Trans_doc_type = db.Inv_receivable_num
                                .Include(x=>x.Rec_inv).Where(x=>x.Checkbook_id==CB.C_CBT).ToList().DefaultIfEmpty(new FabulousDB.Models.Inventory.Inv_receivable_num { })
                                .FirstOrDefault().Rec_inv,
                                
                            };

                        }
                        else
                        {
                            R = new Receivable.Models.Receivable_transaction
                            {
                                Currency = C,
                                Doc_type = FabulousDB.Models.Doc_type.Invoice,
                                Vendor = new Receivable.Models.Receivable_vendore_setting
                                {
                                    Receivable_address_info = new List<FabulousErp.Receivable.Models.Receivable_address_info>
                                    {
                                        new Receivable.Models.Receivable_address_info
                                        {

                                        }
                                    },
                                    Receivable_legal_info = new List<Receivable_legal_info>
                                    {
                                        new Receivable_legal_info
                                        {

                                        }
                                    }
                                },
                                Trans_doc_type = new Receivable.Models.Receivable_transactions_types
                                {

                                }
                            };
                        }
                    }
                    Tax = Mdb.Taxs.Where(x => x.Journal_number == R.Journal_number).ToList().DefaultIfEmpty(new FabulousDB.DB_Tabels.Tax.Tax { })
                        .ToList();

                    VatAmount = Tax.Sum(x => x.Vat_amount);
                    Total_vat_amount = Tax.Sum(x => x.Total_vat_amount);
                    Dacutta_amount = Tax.Sum(x => x.Dacutta_amount);

                    if (!VatAmount.HasValue)
                    {
                        VatAmount = 0;
                    }
                    if (!Total_vat_amount.HasValue)
                    {
                        Total_vat_amount = 0;
                    }
                    if (!Dacutta_amount.HasValue)
                    {
                        Dacutta_amount = 0;
                    }
                    decimal Total = (R.Purchase - R.Taken_discount + Total_vat_amount.Value - Dacutta_amount.Value);
                    decimal PurchOSales = 0;
                    string Moneytext = Bussiness.ConvertNumberToText.ConvertNumberToAlpha(Total.ToString(), R.Currency.Currency_unit_name, R.Currency.Currency_small_unit_name);
                    PurchOSales = R.Purchase;
                    if (IsInv)
                    {
                        if (R.Doc_type == FabulousDB.Models.Doc_type.Invoice)
                        {
                            if (PO != 0 && R.Id == 0)
                            {
                                List<FabulousModels.Inventory.PoItemsPrint> InvItems =
      ViewBag.InvItems =
                             db.Inv_sales_invoice_items.Include(x => x.Sales_invoice)
                             .Where(x => x.Sales_invoice.Posting_number == PO)
                             .Include(x => x.Item).Include(x => x.UOM)
                             .ToList().Select(x => new FabulousModels.Inventory.PoItemsPrint
                             {
                                 Item_id = x.Item.Item_id,
                                 Item_name = (x.Custom_name != null) ? x.Custom_name : x.Item.Short_description/*.Short_description*/,
                                 Qty = x.Quantity,
                                 Unit_price = Inventory.Controllers.InvBus.CalcItemEqUnitPrice(x.Item_id, x.UOM_id.Value, x.Quantity, x.Unit_price, true),
                                 UOM = x.UOM.Unit_id,
                                 Discount = x.Discount,
                                 Fright = x.Fright,
                                 Tax = x.Vat_amount.HasValue ? x.Vat_amount : 0 - (x.Table_vat_amount.HasValue ? x.Table_vat_amount.Value : 0) + (x.Deduct_amount.HasValue ? x.Deduct_amount.Value : 0),
                                 Date = x.Sales_invoice.Transaction_date
                             }).ToList();
                                PurchOSales = InvItems.Sum(x => x.Unit_price * (decimal)x.Qty);
                                //Total = (InvItems.Sum(x => x.Unit_price) - InvItems.Sum(x => x.Discount) + InvItems.Sum(x => x.Tax.Value))
                                //    * (decimal)InvItems.Sum(x => x.Qty);
                                //(R.Purchase - R.Taken_discount + Total_vat_amount.Value - Dacutta_amount.Value);
                                R.Due_date = R.Doc_date = InvItems.FirstOrDefault().Date.Value;

                            }
                            else
                            {
                                if (Id == 0)
                                {
                                    Id = R.Id;
                                }
                                List<FabulousModels.Inventory.PoItemsPrint> InvItems =
      ViewBag.InvItems =
                             db.Inv_sales_invoice_items.Include(x => x.Sales_invoice)
                             .Where(x => x.Sales_invoice.Receivable_id == Id)
                             .Include(x => x.Item).Include(x => x.UOM)
                             .ToList().Select(x => new FabulousModels.Inventory.PoItemsPrint
                             {
                                 Item_id = x.Item.Item_id,
                                 Item_name = (x.Custom_name != null) ? x.Custom_name : x.Item.Short_description/*.Short_description*/,
                                 Qty = x.Quantity,
                                 Unit_price = Inventory.Controllers.InvBus.CalcItemEqUnitPrice(x.Item_id, x.UOM_id.Value, x.Quantity, x.Unit_price, true),
                                 UOM = x.UOM.Unit_id,
                                 Discount = x.Discount,
                                 Fright = x.Fright,
                                 Tax = x.Vat_amount.HasValue ? x.Vat_amount : 0 - (x.Table_vat_amount.HasValue ? x.Table_vat_amount.Value : 0) + (x.Deduct_amount.HasValue ? x.Deduct_amount.Value : 0)
                             }).ToList();
                                PurchOSales = InvItems.Sum(x => x.Unit_price * (decimal)x.Qty);
                                //Total = (InvItems.Sum(x => x.Unit_price) - InvItems.Sum(x => x.Discount) + InvItems.Sum(x => x.Tax.Value))
                                //    * (decimal)InvItems.Sum(x => x.Qty);
                                //(R.Purchase - R.Taken_discount + Total_vat_amount.Value - Dacutta_amount.Value);
                            }


                        }
                        else
                        {
                            List<FabulousModels.Inventory.PoItemsPrint> InvItems = ViewBag.InvItems = db.Inv_receive_po_items.Include(x => x.Receive_po)
                                                      .Where(x => x.Receive_po.Receivable_id == Id)
                                                      .Include(x => x.Item).Include(x => x.UOM)
                                                      .ToList().Select(x => new FabulousModels.Inventory.PoItemsPrint
                                                      {
                                                          Item_id = x.Item.Item_id,
                                                          Item_name = !string.IsNullOrEmpty(x.Item.Short_description) ? x.Item.Short_description : (!string.IsNullOrEmpty(x.Item.Description) ? x.Item.Description : x.Item.Item_id),
                                                          Qty = x.Quantity,
                                                          Unit_price = Inventory.Controllers.InvBus.CalcItemEqUnitPrice(x.Item_id, x.UOM_id.Value, x.Quantity, x.Unit_price, true),
                                                          UOM = x.UOM.Unit_id,
                                                          Discount = x.Discount,
                                                          Fright = x.Fright,
                                                          Tax = x.Vat_amount.HasValue ? x.Vat_amount : 0 - (x.Table_vat_amount.HasValue ? x.Table_vat_amount.Value : 0) + (x.Deduct_amount.HasValue ? x.Deduct_amount.Value : 0)
                                                      }).ToList();
                            PurchOSales = InvItems.Sum(x => x.Unit_price * (decimal)x.Qty);

                            Total = InvItems.Sum(x => x.Unit_price * (decimal)x.Qty) - InvItems.Sum(x => x.Discount) + InvItems.Sum(x => x.Tax.Value);

                        }

                    }



                    RModel.AddRange(FillPayRec(CM.CompanyName, Business.GetCompLogo(), R.Doc_type.ToString()
                        , R.Vendor.Vendor_name, R.Vendor.Receivable_address_info.FirstOrDefault().Address,
                        R.Vendor.Receivable_legal_info.FirstOrDefault().Tax_Id,
                        R.Vendor.Receivable_address_info.FirstOrDefault().Phone_number,
                        R.Vendor.Receivable_address_info.FirstOrDefault().Fax,
                        R.Vendor.Receivable_legal_info.FirstOrDefault().Tax_file_no,
                        R.Trans_doc_type.Counter.ToString(),
                        R.Doc_date, R.Due_date, R.Trans_doc_type.Counter.ToString(),
                        R.Desc, PurchOSales, R.Taken_discount, Total_vat_amount.Value, Dacutta_amount.Value,
                        Total, Moneytext, "<span class='Right'> " + CM.AddressInformation_Table.City + "</span><span class='Right'>" + CM.AddressInformation_Table.Area + "</span> <span class='Right'>" + CM.AddressInformation_Table.BuldingNo + "</span>",
                        CM.C_TaxSetting_Tables.ToList().DefaultIfEmpty(new FabulousDB.DB_Tabels.Settings.Financial.GeneralSetting.Tax.C_TaxSetting_table { C_Taxcode = "" }).FirstOrDefault().C_Taxcode, CM.CompanyCommInfo_Table.TelephoneEX1 + "-" + CM.CompanyCommInfo_Table.Telephone1, CM.CompanyCommInfo_Table.FaxEX1 + CM.CompanyCommInfo_Table.Fax1,
                        CM.CompanyLegalInfo_Table.TaxFileNo, CM.CompanyLegalInfo_Table.CommericalRegister, CM.CompanyLegalInfo_Table.VatID
                        ));

                    return PartialView(RModel);
                }
            }
        }
        public List<ReciptsValues> FillModel(string Comp_nam = null, string Logo = null, string Title = null,
        string Num = null, string Amount = null, DateTime? Date = null, string Recive_from = null,
        string Amount_text = null, string Ref = null, bool IsCash = true, string Cheque_num = null, DateTime? Cheque_date = null, string Cheque_bank = null, bool IsFrom = false, bool ShowBank = false,
        string CheckBook_Name = "", string CheckBook_id = "")
        {
            List<ReciptsValues> ReciptView = new List<ReciptsValues>();

            ReciptView.AddRange(new List<ReciptsValues>
            {
                    new ReciptsValues
                    {
                        Prop=ReciptProp.Comp_nam,
                        Value= Comp_nam
                    },
                    new ReciptsValues
                    {
                        Prop=ReciptProp.Title,
                        Value=Title
                    },
                    new ReciptsValues
                    {
                        Prop= ReciptProp.Logo,
                        Value= Logo
                    },
                    new ReciptsValues
                    {
                        Prop= ReciptProp.Amount,
                        Value=Amount
                    },
                    new ReciptsValues
                    {
                        Prop= ReciptProp.Num,
                        Value=Num
                    },
                    new ReciptsValues
                    {
                        Prop=ReciptProp.Date,
                        Value=(Date.HasValue)?Date.Value.ToString("yyyy/MM/dd",System.Globalization.CultureInfo.InvariantCulture)
                                 :""
                    },
                    new ReciptsValues
                    {
                        Prop=ReciptProp.Recive_from,
                        Value=Recive_from
                    },
                    new ReciptsValues
                    {
                        Prop=ReciptProp.Amount_text,
                        Value=Amount_text
                    },
                    new ReciptsValues
                    {
                        Prop=ReciptProp.Ref,
                        Value=Ref
                    },
                     new ReciptsValues
                    {
                        Prop=ReciptProp.Cheque_num,
                        Value=Cheque_num
                    },
                     new ReciptsValues
                    {
                        Prop=ReciptProp.Cheque_date,
                        Value=(Cheque_date.HasValue)?Cheque_date.Value.ToString("yyyy/MM/dd",System.Globalization.CultureInfo.InvariantCulture)
                                :""
                    },
                    new ReciptsValues
                    {
                        Prop=ReciptProp.Cheque_bank,
                        Value=Cheque_bank
                    },
                    new ReciptsValues
                    {
                        Prop=ReciptProp.IsCash,
                        Value=IsCash.ToString()
                    },
                    new ReciptsValues
                    {
                        Prop=ReciptProp.IsFrom,
                        Value=IsFrom.ToString()
                    },
                    new ReciptsValues
                    {
                        Prop=ReciptProp.ShowBank,
                        Value=ShowBank.ToString()
                    },
                    new ReciptsValues
                    {
                        Prop=ReciptProp.CheckBook_id,
                        Value=CheckBook_id
                    },
                    new ReciptsValues
                    {
                        Prop=ReciptProp.CheckBook_Name,
                        Value=CheckBook_Name
                    }
                });
            return ReciptView;
        }
        public List<PayRecRecipt> FillPayRec(string Comp_nam = null, string Logo = null, string Title = null, string Client_name = null, string Client_address = null,
       string Tax_file_no = null, string Phone = null, string Fax = null, string Reg_file = null, string Inv_num = null, DateTime? Inv_date = null,
      DateTime? Due_date = null, string Inv_count = null, string Ref = null, decimal? Purch = null, decimal? Discount = null, decimal? Vat_amount = null, decimal? Deduct = null, decimal? Total = null, string Money_text = null, string Comp_Address = null, string Comp_tax_file = null, string Comp_phon = null, string Comp_fax = null, string Comp_tax_reg_num = null, string ST = null, string VatId = null)
        {
            string ThisDueDate = "";
            string ThisInvDate = "";
            if (Due_date.HasValue)
            {
                ThisDueDate = Due_date.Value.ToString("yyyy/MM/dd", System.Globalization.CultureInfo.InvariantCulture);
            }
            if (Inv_date.HasValue)
            {
                ThisInvDate = Inv_date.Value.ToString("yyyy/MM/dd", System.Globalization.CultureInfo.InvariantCulture);
            }

            return new List<PayRecRecipt>
            {
                new PayRecRecipt
                {
                    Prop=PayRecProp.Comp_nam,
                    Value=Comp_nam
                } ,
                new PayRecRecipt
                {
                    Prop=PayRecProp.Logo,
                    Value=Logo
                },
                new PayRecRecipt
                {
                      Prop=PayRecProp.Title,
                    Value=Title
                },
                new PayRecRecipt
                {
                      Prop=PayRecProp.Client_address,
                    Value=Client_address
                },
                new PayRecRecipt
                {
                      Prop=PayRecProp.Client_name,
                    Value=Client_name
                },
                new PayRecRecipt
                {
                      Prop=PayRecProp.Comp_Address,
                    Value=Comp_Address
                },
                new PayRecRecipt
                {
                      Prop=PayRecProp.Comp_fax,
                    Value=Comp_fax
                },
                new PayRecRecipt
                {
                      Prop=PayRecProp.Comp_nam,
                    Value=Comp_nam
                },
                new PayRecRecipt
                {
                      Prop=PayRecProp.Comp_phon,
                    Value=Comp_phon
                },
                new PayRecRecipt
                {
                      Prop=PayRecProp.Comp_tax_file,
                    Value=Comp_tax_file
                },
                new PayRecRecipt
                {
                      Prop=PayRecProp.Comp_tax_reg_num,
                    Value=Comp_tax_reg_num
                },
                new PayRecRecipt
                {
                      Prop=PayRecProp.Deduct,
                    Value=Convert.ToString((Deduct<0)?-Deduct:Deduct)
                },
                new PayRecRecipt
                {
                      Prop=PayRecProp.Discount,
                    Value=Convert.ToString((Discount<0)?-Discount:Discount )
                },
                new PayRecRecipt
                {
                      Prop=PayRecProp.Due_date,
                    Value=ThisDueDate
                },
                new PayRecRecipt
                {
                      Prop=PayRecProp.Fax,
                    Value=Fax
                },
                new PayRecRecipt
                {
                      Prop=PayRecProp.Inv_count,
                    Value=Inv_count
                },
                new PayRecRecipt
                {
                      Prop=PayRecProp.Inv_date,
                    Value=ThisInvDate
                },
                new PayRecRecipt
                {
                      Prop=PayRecProp.Inv_num,
                    Value=Inv_num
                },
                new PayRecRecipt
                {
                      Prop=PayRecProp.Money_text,
                    Value=Money_text
                },
                new PayRecRecipt
                {
                      Prop=PayRecProp.Phone,
                    Value=Phone
                },
                new PayRecRecipt
                {
                      Prop=PayRecProp.Purch,
                    Value=Convert.ToString((Purch<0)?-Purch:Purch)
                },
                new PayRecRecipt
                {
                      Prop=PayRecProp.Ref,
                    Value=Ref
                },
                new PayRecRecipt
                {
                      Prop=PayRecProp.Reg_file,
                    Value=Reg_file
                },
                new PayRecRecipt
                {
                      Prop=PayRecProp.Tax_file_no,
                    Value=Tax_file_no
                },
                new PayRecRecipt
                {
                      Prop=PayRecProp.Total,
                    Value=Convert.ToString((Total<0)?-Total:Total)
                },
                new PayRecRecipt
                {
                      Prop=PayRecProp.Vat_amount,
                    Value=Convert.ToString((Vat_amount<0)?-Vat_amount:Vat_amount)
                },
                new PayRecRecipt
                {
                      Prop=PayRecProp.ST,
                    Value=ST
                },
                new PayRecRecipt
                {
                      Prop=PayRecProp.Vat_id,
                    Value=VatId
                }
            };
        }
    }

}