var Credit_limit_enum = {
    No_credit: 1,
    Unlimited: 2,
    Amount: 3
};
var Doc_type = {
    Invoice: 1,
    Credit_Memo: 2,
    Debit_Memo: 3,
    Payment: 4,
    Return: 5
};
var Payment_per ={
    Invoice : 1,
    Any : 2
}
$("#Currency_idTrans").change(function () {
    if (window.location.href.indexOf("Details") == -1) {
        TransactionRateStatus(TransRateStatus)
    }
})
$(function () {
    if (!HasNoPayment) {
        HasNoPayment = "";
    }
})
var HasNoPayment;
var TransRateStatus;
var MaxAmount;
var MinAmount;
var VendoreValid;
var CLType;
var CLAmount;
var VeAmount;
var VZeroSum;
$(".PayablePayment").find(".Cash_type[value='Cash']").trigger("click");
$("#Vendor_idTrans").change(function () {
    GetVendoreData($(this))
})
function GetVendoreData(Vendore, LimitCallBack=null) {
    VendoreValid = false;
    var Vendor_id = $(Vendore).val()
    $.ajax({
        url: "/Payable/Creditor_setting/HasPassword?id=" + Vendor_id,
        method: "POST",
        success: function (HasPassword) {
            if (HasPassword) {
                ModelMsg("<div class='col-sm-12'><label>Enter Password</label><input type='password' class='GroupSettingPas form-control'  /></div>", "Enter Password"
                    , false,
                    function () {
                        $.ajax({
                            url: "/Payable/Creditor_setting/CheckPass?Id=" + Vendor_id + "&Password=" + $(document).find(".GroupSettingPas").val(),
                            method: "POST",
                            success: function (data) {
                                if (data) {
                                    GetCreditorName(Vendor_id)
                                    VendoreValid = true;
                                } else {
                                    ModelMsg("<h4>Wrong Password</h4>", "Wrong Password", true)
                                    $("#Vendor_idTrans").val("");
                                    VendoreValid = false;
                                }
                            }
                        })
                    }, function () {
                        if (!VendoreValid) {
                            $("#Vendor_idTrans").val("");
                        }
                    })
            } else {
                GetCreditorName(Vendor_id)
            }
        }
    });
    $.ajax({
        url: "/Payable/Creditor_setting/CheckCreditorLimit?id=" + Vendor_id,
        method: "POST",
        success: function (CL) {
            CLType = CL.status;
            CLAmount = CL.amount
            if (CL.status == Credit_limit_enum.Amount) {
                VeAmount = CL.veAmount;
            }
            if (HasNoPayment == "hide") {
                if (CL.status == Credit_limit_enum.No_credit || CL.status == Credit_limit_enum.Amount) {
                    $(".PayablePayment").removeClass("hide");
                    VZeroSum = true;
                    CLAmount = CL.amount
                } else {
                    $(".PayablePayment").addClass("hide");
                    VZeroSum = false;
                }
            } else {
                VZeroSum = true;
            }
            MinAmount = CL.MinAmount;
            MaxAmount = CL.MaxAmount;
            if (LimitCallBack != null) {
                LimitCallBack(CLAmount, MinAmount, MaxAmount);
            }
        }
    })
    $("#VDocument_number").trigger("focusout");
}
function GetCreditorName(Id, CallBack = null) {
    $.ajax({
        url: "/Payable/Creditor_setting/GetNameAndCBookById?id=" + Id,
        success: function (data) {
            $("#VendorNamelbl").text(data.Name);
            $(".PayablePayment").find("#Check_book_id").val(data.CBook);
            $("#TransDiv").find("#Payment_terms_id").val(data.PTI);
            $("#TransDiv").find("#Shipping_method_id").val(data.SI);

            if (CallBack != null) {
                CallBack(data.CBook);
            }
        },
        method: "POST"
    })
}
var OverLimitValid = false;
var Changed = false;

$("#Create").click(function () {
    return ValidateVendoreLimit();
})
function ValidateVendoreLimit(InsertJv = true, IsInv = false) {

    var OrginalAmount = GetMaskNumber($(document).find(".PayablePayment").find("#Orginal_amount").val());
    if (IsInv) {
        HasNoPayment = "hide";
    }
    if (HasNoPayment == "") {
        $("#PaymentForm").validate();
        $("#PaymentForm").valid();
        if (!$("#PaymentForm").valid()) {
            if (OrginalAmount != 0) {
                return false;
            }
        }
    }

    var TotalValue = parseFloat($("#TotalValue").val());
    if (!TotalValue) {
        TotalValue = 0;
    }
    if (TotalValue == 0) {
        TotalValue = GetMaskNumber($("#TTotalMask").val());
    }
    if (!OrginalAmount) {
        OrginalAmount = 0;
    }
    var TrasRate = 1;
    if ($("#TransDiv").find("#TCGE-TransactionRate").val()) {
        TrasRate = parseFloat($("#TransDiv").find("#TCGE-TransactionRate").val());
    }
    var ThisDocType = "";

    if ($("#Doc_typeTrans").length > 0) {
        ThisDocType = $("#Doc_typeTrans").val();
        ThisDocType = Doc_type[ThisDocType];
    } else {
        ThisDocType = parseInt($("#Doc_type").val());


    }


    if (ThisDocType == Doc_type.Credit_Memo
        || ThisDocType == Doc_type.Return) {
        VZeroSum = false;
    } else {
        VZeroSum = true;
    }
    if (OrginalAmount * TrasRate == TotalValue * TrasRate) {
        VZeroSum = false;
    }
    if (VZeroSum) {
        if (CLType == Credit_limit_enum.Amount) {
            var MyThisMax = parseFloat(CLAmount) + ((TotalValue - OrginalAmount) * TrasRate);
            var AddedMsg = "";
            if ($("#Currency_idTrans").val() != CompId) {
                AddedMsg = " Or " + MyThisMax + " " + $("#Currency_idTrans").find("option:selected").text();
            }
            if (VeAmount + (OrginalAmount * TrasRate) < MyThisMax) {
                {
                    if (!OverLimitValid) {
                        $.ajax({
                            url: "/Payable/Genral_setting/GetCreditLimitPasswordOption",
                            method: "POST",
                            success: function (data) {
                                if (data) {
                                    ModelMsg("<div class='col-sm-12'>You are overlimit your max limit is " + (VeAmount - parseFloat(CLAmount) - OrginalAmount) + CurrIso + AddedMsg + "<label>if you Want To Change Enter Password</label><input type='password' class='CreditLimitPass form-control'  /></div>", "Enter Password"
                                        , false, function () {
                                            $.ajax({
                                                url: "/Payable/Genral_setting/CheckCreditLimitPasswordOption?Password=" + $(document).find(".CreditLimitPass").val(),
                                                method: "POST",
                                                success: function (data) {
                                                    if (data) {
                                                        OverLimitValid = true;
                                                        Changed = true;
                                                    } else {
                                                        ModelMsg("<h4>Wrong Password</h4>", "Wrong Password", true)
                                                    }
                                                }
                                            })
                                        }, null)

                                } else {
                                    if (!Changed) {
                                        ModelMsg("You are overlimit your max limit is " + (VeAmount - parseFloat(CLAmount) - OrginalAmount) + CurrIso + AddedMsg, "Over Limit", true, null, null)
                                    }

                                }
                            }
                        })
                        return false;
                        // alert("You are overlimit your max limit is " + (parseFloat(CLAmount) - OrginalAmount).toString())
                    } else {
                        if (!Changed) {
                            ModelMsg("You are overlimit your max limit is " + (parseFloat(CLAmount) - OrginalAmount) + " " + CurrIso + AddedMsg, "Over Limit", true)
                        }
                    }
                }
            }
        } else if (CLType == Credit_limit_enum.No_credit) {
            var Left = (parseFloat(CLAmount) + ((TotalValue - OrginalAmount) * TrasRate));
            if (Left > 0) {
                if (VeAmount + (OrginalAmount * TrasRate) < Left) {
                    //parseFloat($(document).find(".PayablePayment").find("#Orginal_amount").val()) < parseFloat($("#TotalValue").val()
                    ModelMsg("This Vendore Has No Credit Limit, Over Limit By " + (Left) + " ", "No Credit Limit", true)
                    return false;
                }
            }
        }
    }
    if (InsertJv) {
        $("#PayableForm").validate()
        if (!($("#PayableForm").valid())) {
            return false;
        } else {
            $.ajax({
                url: "/Payable/Payable_transaction/ValidateTransaction",
                data: $("#PayableForm").serialize(),
                method: "POST",
                success: function (data) {
                    if (data.status) {
                        var BatchId = "";

                        InsertTransactionData(CompId, $("#BostingToORThrow").val(), $("#TCGE-PostingDate").val(), $("#TCGE-JEDate").val(), $("#TCGE-Reference").val(), $("#TCGE-CurrencyID").val(), $("#TCGE-SystemRate").val(), $("#TCGE-TransactionRate").val(), "PayTrns", "PayTrns", BatchId, "",
                            function (JournalEntryNumber, PO) {
                                $(document).find(".Journal_number").val(JournalEntryNumber)
                                if (!$("#TransDiv").find("#Taken_discount").val()) {
                                    $("#TransDiv").find("#Taken_discount").val(0)
                                }
                                if (!$("#TransDiv").find("#Tax").val()) {
                                    $("#TransDiv").find("#Tax").val(0)
                                }
                                $("#FirstForm").trigger("click");
                                InsertIntoTax(JournalEntryNumber)

                            }, null, true, null, true)
                    } else {
                        ModelMsg(data.msg, "", true, null, null);
                    }
                }
            })
        }
    }
    if (IsInv) {
        return true;
    }
}
function SubmitPayment(data) {
    if (isNaN(data.PayId)) {
        ModelMsg(data.Error, "", true, null, null);
    } else {
        if ($(document).find(".PayablePayment").find("#Orginal_amount").val() != 0) {
            $(document).find(".PayablePayment").find("#Transaction_date").val($("#TransDiv").find("#TCGE-JEDate").val());
            $(document).find(".PayablePayment").find("#Posting_date").val($("#TransDiv").find("#TCGE-PostingDate").val());
            $(document).find(".PayablePayment").find("#Vendor_id").val($("#TransDiv").find("#Vendor_idTrans").find("option:selected").val());
            $(document).find(".PayablePayment").find("#Reference").val($("#TransDiv").find("#Desc").val());
            $(document).find(".PayablePayment").find("#System_rateTrans").val($("#TransDiv").find("#System_rateTrans").val());
            $(document).find(".PayablePayment").find("#Transaction_rate").val($("#TransDiv").find("#Transaction_rateTrans").val());
            $(document).find(".PayablePayment").find("#Due_date").val($("#TransDiv").find("#Due_dateTrans").val());
            // $(document).find(".PayablePayment").find("#Orginal_amount").val($(document).find(".PayablePayment").find("#Paid_amount").val());
            $(document).find(".PayablePayment").find("#Taken_discount").val(0);
            $(document).find(".PayablePayment").find("#Transaction_id").val(data.PayId);
            $(document).find(".PayablePayment").find("#Trx_trans_doc_type_id").val(data.TrxId);
            $(document).find(".PayablePayment").find("#Paid_amount").val(0);
            $(document).find(".PayablePayment").find("#Paid_amountLbl").text(0);
            $(document).find(".PayablePayment").find("#Currency_id").val($("#TransDiv").find("#Currency_idTrans").val());
            $(document).find(".PayablePayment").find("#SubmitPayment").trigger("click");
            Talert("Transaction Number ", data.Trx_num + " " + $("#Doc_typeTrans").find("option:selected").text(), "  Number ", data.Counter)
            RedirectInt(window.location);

        } else {
            RedirectInt(window.location);
        }
    }
    PrintInt("/Recipts/PayRecRecipts?Id=" + data.PayId + "&IsPay=true")
}

$("#VDocument_number").focusout(function () {
    $.ajax({
        url: "/Payable/Payable_transaction/CheckDocNumWithVendore?VendoreId=" + $("#Vendor_idTrans").find("option:selected").val() + "&DocNum=" + $(this).val(),
        method: "POST",
        success: function (data) {
            if (data) {
                ModelMsg("<p>" + $("#VDocument_number").val() + " Already Exist With That Vendore</p>",
                    "Duplicate Document number ", true);
                $("#VDocument_number").val("")
            }
        }

    })
});

//////////////
function InsertPayableJv(Url, PostinNum, CompId, Empty = true, CallBack = null
    , GetIndex = -1, JvMerge = true, InvMarkTax = false) {
    $.ajax({
        url: Url,
        method: "POST",
        success: function (data) {
            var TaxJv;
            try {
                if (IsDetails) {
                    GetJvTransaction(PostinNum, CompId);
                }
            } catch (err) {
                if (Empty) {
                    EmptyGlTransaction();
                }
                $.each(data, function (k, i) {
                    ManyJvAction(i, false);
                })
                var PorS = "P";
                if ($("#Doc_type").val() == Doc_type.Return) {
                    PorS = "S";
                }
                TaxJv = InsertTaxJv(false, "SED", GetIndex, PorS);
                if (InvMarkTax == true) {
                    InvMarkTaxJv(TaxJv);
                }
                if (JvMerge) {
                    MergJv(true);
                    try {
                        PoMergeJvTax();
                    } catch{

                    }
                }
            }
            try {
                if (IsDetails) {
                    $("#TCGE-TTbl").find("button").not(".MoreDetailsT").remove();
                    $(document).find(".GlAddedBatch").nextUntil(".mt-2").hide();
                    $("#JVSecCont").attr("style", "display:block;");
                }
            } catch (err) {

            }
            if (CallBack != null) {
                if (TaxJv) {
                    data[0].ShowTransactions = data[0].ShowTransactions.concat(TaxJv.ShowTransactions)
                }
                CallBack(data);

            }
        }
    })
}

