using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabulousDB.Models
{
    public enum Doc_type
    {
        Invoice = 1,
        [Display(Name = "Credit Memo")]
        Credit_Memo = 2,
        [Display(Name = "Debit Memo")]
        Debit_Memo = 3,
        Payment = 4,
        Return = 5,
        Transfer=6,
        Adjustment = 7
    }
    public enum TrxPay
    {
        Pay = 1,
        Trx = 2,
        Assign = 3
    }
    public enum Cash_type
    {
        Cash = 1,
        Cheque = 2,
        Installment = 3
    }
    public enum Amount_type
    {
        Percentage = 1,
        Amount = 2
    }
    public enum Date_option
    {
        [Display(Name = "From Due Date")]
        From_due_date = 1,
        [Display(Name = "From Document Date")]
        From_document_date = 2
    }
    public enum Credit_limit_enum
    {
        [Display(Name = "No Credit")]
        No_credit = 1,
        Unlimited = 2,
        Amount = 3
    }

    public enum Minimum_payment
    {
        [Display(Name = "No Mini.")]
        No_mini = 1,
        Percentage = 2,
        Amount = 3
    }
    public enum Payment_per
    {
        Invoice = 1,
        Any = 2
    }
    public enum Other_option_enum
    {
        [Display(Name = "Override Vendor Document Number In Transaction Entry")]
        Ovride = 1,
        [Display(Name = "Print Tax Details in Print Document")]
        Print_tax = 2,
        [Display(Name = "Active Payment In Transaction")]
        Active_payment = 3
    }
    public enum Password_optionEnum
    {
        [Display(Name = "Remove Inactive Creditor")]
        Remove_inaactive_creditor = 1,
        [Display(Name = "Exceed Min.& Max. Transaction")]
        Exceed_Min_max = 2,
        [Display(Name = "Exceed Credit Limit")]
        Exceed_credit_limit = 3
    }
}
