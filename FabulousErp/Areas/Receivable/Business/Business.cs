using FabulousErp.Receivable.Models; using FabulousDB.DB_Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Data;
using System.Data.Entity;
using FabulousDB.Models;
using System.Web.Mvc;

namespace Receivable.Controllers
{
    public class Business
    {
        public static int GetNextDocNumber(Doc_type DocType)
        {
            DBContext db = new DBContext();
            string NextNumber = db.Receivable_general_settings.FirstOrDefault(x => x.Doc_type == DocType).Next_number;
            int NextInt = 0;
            NextInt = GetDigits(NextNumber);
            string Newstring = "";
            foreach (char c in NextNumber)
            {
                if (!char.IsDigit(c))
                {
                    Newstring += c;
                }
            }
            db.Receivable_general_settings.FirstOrDefault(x => x.Doc_type == DocType).Next_number = Newstring + (NextInt + 1).ToString();
            db.SaveChanges();
            return NextInt;
        }

        public static int GetDigits(string NextNumber)
        {
            int NextInt;
            if (int.TryParse(NextNumber, out NextInt))
            {
                NextInt = Convert.ToInt32(NextNumber);
            }
            else
            {
                try
                {
                    NextInt = Convert.ToInt32(Regex.Match(NextNumber, @"\d+").Value);
                }
                catch
                {
                    NextInt = 1;
                }
            }
            return NextInt;
        }
        public static List<Receivable_transaction> GetUnpaidTransaction(int VendoreId=-1)
        {
            using (DBContext db = new DBContext())
            {
                List<Receivable_transaction> PT = new List<Receivable_transaction>();
                if (VendoreId != -1)
                {
                    PT = db.Receivable_transactions.Where(x => x.Vendor_id == VendoreId && x.Is_void == false).Include(x => x.Currency).Include(x => x.Trans_doc_type)
                .Include(x => x.Vendor).ToList();
                }
                else
                {

                    PT = db.Receivable_transactions.Where(x => x.Is_void == false).Include(x => x.Currency).Include(x => x.Trans_doc_type)
                    .Include(x => x.Vendor).ToList();
                }

                var Res = PT.Where(x => (x.Doc_type != Doc_type.Return || x.Doc_type != Doc_type.Credit_Memo)).ToList().Where(x =>
                 ((x.Purchase - x.Taken_discount + x.Tax) * x.Transaction_rate) -
                   db.Assign_Receivable_docs.Where(z => z.Trans_doc_type_id_to == x.Trans_doc_type_id && x.Is_void == false)
                 .ToList().DefaultIfEmpty(new Assign_Receivable_doc { Applay_assign = 0, Taken_discount = 0 })
                 .Sum(y => (y.Applay_assign - y.Taken_discount) * y.Transaction_rate)
                 -
                 db.Receivable_payments.Where(z => z.Transaction_id == x.Id && x.Is_void == false).ToList().DefaultIfEmpty(new Receivable_payment { Orginal_amount = 0, Taken_discount = 0 })
                 .Sum(z => (z.Orginal_amount - z.Taken_discount) * z.Transaction_rate)
                 > 0).ToList();

                Res.AddRange(PT.Where(x => (x.Doc_type == Doc_type.Return || x.Doc_type == Doc_type.Credit_Memo)).ToList().Where(x =>
                  ((x.Purchase - x.Taken_discount + x.Tax) * x.Transaction_rate) -
                    db.Assign_Receivable_docs.Where(z => z.Trans_doc_type_id_to == x.Trans_doc_type_id && x.Is_void == false)
                  .ToList().DefaultIfEmpty(new Assign_Receivable_doc { Applay_assign = 0, Taken_discount = 0 })
                  .Sum(y => (y.Applay_assign - y.Taken_discount) * y.Transaction_rate)
                  -
                  db.Receivable_payments.Where(z => z.Transaction_id == x.Id && x.Is_void == false).ToList().DefaultIfEmpty(new Receivable_payment { Orginal_amount = 0, Taken_discount = 0 })
                  .Sum(z => (z.Orginal_amount - z.Taken_discount) * z.Transaction_rate)
                  < 0).ToList());
                return Res;

            }

        }
        public static List<Receivable_payment> GetUnAssignPayment(int VendoreId=-1,Doc_type doc_Type=0,bool IncludeTrans=true)
        {
            using (DBContext db = new DBContext())
            {
                List<Receivable_payment> PP = new List<Receivable_payment>();
                List<Receivable_transaction> PT = new List<Receivable_transaction>();
                if (VendoreId != -1)
                {
                    PP = db.Receivable_payments.Include(x=>x.Vendor).Include(x=>x.Currency).Where(x => x.Vendor_id == VendoreId && x.Is_void == false).Include(x => x.Trans_doc_type)
                   .ToList();

                }
                else
                {
                    PP = db.Receivable_payments.Include(x => x.Vendor).Include(x => x.Currency).Where(x =>  x.Is_void == false).Include(x => x.Trans_doc_type)
                            .ToList();
                }
                
                if (doc_Type != 0)
                {
                    PP = PP.Where(x => x.Trans_doc_type.Doc_type == doc_Type).ToList();
                    if (VendoreId != -1)
                    {
                        PT = db.Receivable_transactions.Include(x => x.Currency).Where(x => x.Doc_type == doc_Type && x.Vendor_id == VendoreId && x.Is_void == false).Include(x => x.Trans_doc_type).ToList();
                    }
                    else
                    {
                        PT = db.Receivable_transactions.Include(x => x.Currency).Where(x => x.Doc_type == doc_Type && x.Is_void == false).Include(x => x.Trans_doc_type).ToList();

                    }
                    if (IncludeTrans)
                    {
                        PP.AddRange(PT
                       .ToList().Select(x => new Receivable_payment
                       {
                           Currency_id = x.Currency_id,
                           Due_date = x.Due_date,
                           Journal_number = x.Journal_number,
                           Orginal_amount = (x.Doc_type == Doc_type.Return
                                          || x.Doc_type == Doc_type.Credit_Memo) ? -(x.Purchase + x.Tax) : x.Purchase + x.Tax,
                           Taken_discount = (x.Doc_type == Doc_type.Return
                                          || x.Doc_type == Doc_type.Credit_Memo) ? -x.Taken_discount : x.Taken_discount,
                           Posting_date = x.Posting_date,
                           Transaction_date = x.Transaction_date,
                           Transaction_rate = x.Transaction_rate,
                           System_rate = x.System_rate,
                           Trans_doc_type = x.Trans_doc_type,
                           Trans_doc_type_id = x.Trans_doc_type_id
                       }));
                    }
                   
                }
                else
                {
                    if (VendoreId != -1)
                    {
                        PT = db.Receivable_transactions.Include(x => x.Currency).Where(x =>  x.Vendor_id == VendoreId && x.Is_void == false).Include(x => x.Trans_doc_type).ToList();
                    }
                    else
                    {
                        PT = db.Receivable_transactions.Include(x => x.Currency).Where(x => x.Is_void == false).Include(x => x.Trans_doc_type).ToList();

                    }
                    if (IncludeTrans)
                    {
                        PP.AddRange(PT.ToList().Select(x => new Receivable_payment
                        {
                            Currency_id = x.Currency_id,
                            Due_date = x.Due_date,
                            Journal_number = x.Journal_number,
                            Orginal_amount = (x.Doc_type == Doc_type.Return
                                          || x.Doc_type == Doc_type.Credit_Memo) ? -(x.Purchase + x.Tax) : x.Purchase + x.Tax,
                            Taken_discount = (x.Doc_type == Doc_type.Return
                                          || x.Doc_type == Doc_type.Credit_Memo) ? -x.Taken_discount : x.Taken_discount,
                            Posting_date = x.Posting_date,
                            Transaction_date = x.Transaction_date,
                            Transaction_rate = x.Transaction_rate,
                            System_rate = x.System_rate,
                            Trans_doc_type = x.Trans_doc_type,
                            Trans_doc_type_id = x.Trans_doc_type_id
                        }));
                    }
                }
                return  PP.Where(x => x.Trans_doc_type.Doc_type != Doc_type.Return ||
                x.Trans_doc_type.Doc_type != Doc_type.Credit_Memo)
                .Where(x =>
                 (x.Orginal_amount - x.Taken_discount -
                   db.Assign_Receivable_docs.Where(z => z.Trans_doc_type_id_to == x.Trans_doc_type_id&&x.Is_void==false)
                 .ToList().DefaultIfEmpty(new Assign_Receivable_doc { Applay_assign=0, Taken_discount = 0 })
                 .Sum(y =>y.Applay_assign- y.Taken_discount))
                 - db.Receivable_transactions.Where(z=>z.Id==x.Transaction_id&&x.Is_void==false).ToList().DefaultIfEmpty(new Receivable_transaction {Purchase=0,Taken_discount=0,Tax=0 })
                 .Sum(z=>z.Purchase-z.Taken_discount+z.Tax)
                 >0).ToList();

            }

        }
        public static decimal GetThisTrxAssignAmount(int TrxId)
        {
            try
            {
                using (DBContext db = new DBContext())
                {
                    return db.Assign_Receivable_docs.Include(x => x.Trans_doc_type_to).Where(x => x.Trans_doc_type_to.Receivable_transaction.Any(z => z.Id == TrxId)).Sum(x => x.Applay_assign - x.Taken_discount);
                }
            }
            catch
            {
                return 0;
            }
           
        }
        public static int TrxNum()
        {
            try
            {
                return db.Receivable_transactions_types.Max(x => x.Trx_num)+1;
            }
            catch
            {
                return 1;
            }
        }
        static DBContext db = new DBContext();
        public static SelectList GetCustomerReceivableSelect(int? VendoreId = null)
        {
            return new SelectList(db.Receivable_vendore_settings.Where(x => x.Inactive == false)
                      .Select(x => new { x.Id, Vendor_id = x.Vendor_id + " - " + x.Vendor_name }), "Id", "Vendor_id", VendoreId);
        }
    }
}