using FabulousModels.DTOModels.Transaction.Financial;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabulousModels.ViewModels
{
    public class DocReplaceWords
    {
        public string Doc_word { get; set; }
        public string Replace_word { get; set; }
    }
    public class ReciptsValues
    {
        public ReciptProp Prop { get; set; }
        public string Value { get; set; }
    }
    public class PayRecRecipt
    {
        public PayRecProp Prop { get; set; }
        public string Value { get; set; }
    }
    public enum ReciptProp
    {
        Comp_nam,
        Logo,
        Title,
        Num,
        Amount,
        Date,
        Recive_from,
        Amount_text,
        Ref,
        IsCash,
        IsFrom,
        ShowBank,
        Cheque_num,
        Cheque_date,
        Cheque_bank,
        CheckBook_id,
        CheckBook_Name,
    }
    public enum PayRecProp
    {
        Comp_nam,
        Logo,
        Title,
        Client_name,
        Client_address,
        Tax_file_no,
        Phone,
        Fax,
        Reg_file,
        Inv_num,
        Inv_date,
        Due_date,
        Inv_count,
        Ref,
        Purch,
        Discount,
        Vat_amount,
        Deduct,
        Total,
        Money_text,
        Comp_Address,
        Comp_tax_file,
        Comp_phon,
        Comp_fax,
        Comp_tax_reg_num,
        ST,
        Vat_id
    }
    public enum Recipts
    {
        CheckBook2,
        PayRecRecipts
    }
    public class ReconcileHeader
    {
        public string Title { get; set; }
        public decimal? Total { get; set; }
        public bool IsDeposit { get; set; }
    }
    public class RecnocileData
    {
        public decimal Deposite { get; set; }
        public decimal Payment { get; set; }
        public string Type { get; set; }
        public string Cheque_num { get; set; }
    }
    public class ReconcileRpt
    {
        public ReconcileHeader Head { get; set; }
        public List<RecnocileData> Data { get; set; }
    }
}
