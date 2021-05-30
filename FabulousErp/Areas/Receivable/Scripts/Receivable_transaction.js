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
var VZeroSum = false;
var CLType = "";
var CLAmount = 0;
var VeAmount = 0;
$(".ReceivablePayment").find("#Cash_type[value='Cash']").trigger("click");
$("#Vendor_id").change(function () {
    GetVendoreData()
})
function GetVendoreData() {
    VendoreValid = false;

    var Vendor_id = $("#Vendor_id").val()
    $.ajax({
        url: "/Receivable/Creditor_setting/HasPassword?id=" + Vendor_id,
        method: "POST",
        success: function (HasPassword) {
            if (HasPassword) {
                ModelMsg("<div class='col-sm-12'><label>Enter Password</label><input type='password' class='GroupSettingPas form-control'  /></div>", "Enter Password"
                    , false,
                    function () {
                        $.ajax({
                            url: "/Receivable/Creditor_setting/CheckPass?Id=" + Vendor_id + "&Password=" + $(document).find(".GroupSettingPas").val(),
                            method: "POST",
                            success: function (data) {
                                if (data) {
                                    GetCreditorName(Vendor_id)
                                    VendoreValid = true;

                                } else {
                                    ModelMsg("<h4>Wrong Password</h4>", "Wrong Password", true)
                                    $("#Vendor_id").val("");
                                    VendoreValid = false;
                                }
                            }
                        })
                    }, function () {
                        if (!VendoreValid) {
                            $("#Vendor_id").val("");
                        }
                    })
            } else {
                GetCreditorName(Vendor_id)
            }
        }
    });
    GetCustomerBalance(Vendor_id, function (VeAmount) {
        CalcCustomerBalance()
    });
}
function GetCustomerBalance(Customer_id, CallBack = null) {
    $.ajax({
        url: "/Receivable/Creditor_setting/CheckCreditorLimit?id=" + Customer_id,
        method: "POST",
        success: function (CL) {
            CLType = CL.status;
            CLAmount = CL.amount
            if (CL.status == Credit_limit_enum.Amount) {
                VeAmount = CL.veAmount;
            }
            if (HasNoPayment == "hide") {
                if (CL.status == Credit_limit_enum.No_credit || CL.status == Credit_limit_enum.Amount) {
                    $(".ReceivablePayment").removeClass("hide");
                    VZeroSum = true;
                    CLAmount = CL.amount
                } else {
                    $(".ReceivablePayment").addClass("hide");
                    VZeroSum = false;
                }
            } else {
                VZeroSum = true;
            }
            MinAmount = CL.MinAmount;
            MaxAmount = CL.MaxAmount;
            if (CallBack != null) {
                CallBack(CL.amount, MinAmount, MaxAmount)
            }
            try {
                if (getParameterByName("Piece") == "true") {
                    CalcNewBalance();
                }
            } catch (err) {

            }
        }
    })
}
function GetCreditorName(Id) {
    $.ajax({
        url: "/Receivable/Creditor_setting/GetNameAndCBookById?id=" + Id,
        success: function (data) {
            $("#VendorNamelbl").text(data.Name);
            $(".ReceivablePayment").find("#Check_book_id").val(data.CBook);
            $("#TransDiv").find("#Payment_terms_id").val(data.PTI);
            $("#TransDiv").find("#Shipping_method_id").val(data.SI);
        },
        method: "POST"
    })
}
var OverLimitValid = false;
var Changed = false;
$("#Create").click(function () {
    return ValidateCustomerLimit()
})
function ValidateCustomerLimit(InsertJv = true, IsInv = false) {
    $("#TCGE-JEDate").removeAttr("disabled")
    $("#TCGE-PostingDate").removeAttr("disabled")
    var OrginalAmount = GetMaskNumber($(document).find(".ReceivablePayment").find("#Orginal_amount").val());
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

    if (!OrginalAmount) {
        OrginalAmount = 0;
    }

    var TotalValue = 0;

    if (IsInv) {
        TotalValue = GetMaskNumber($("#TTotalMask").val());
    } else {
        TotalValue= parseFloat($("#TotalValue").val());
    }
    var TrasRate = 1;
    if ($("#TransDiv").find("#TCGE-TransactionRate").val()) {
        TrasRate = parseFloat($("#TransDiv").find("#TCGE-TransactionRate").val());
    }

    var ThisDocType;
    if (IsInv) {
        ThisDocType = $("#Doc_type").val();
    } else {
        ThisDocType = Doc_type[$("#Doc_type").val()];
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
            if ($("#Currency_id").val() != CompId) {
                AddedMsg = " Or " + MyThisMax + " " + $("#Currency_id").find("option:selected").text();
            }
            if (VeAmount + (OrginalAmount * TrasRate) < MyThisMax) {
                {
                    if (!OverLimitValid) {
                        $.ajax({
                            url: "/Receivable/Genral_setting/GetCreditLimitPasswordOption",
                            method: "POST",
                            success: function (data) {
                                if (data) {
                                    ModelMsg("<div class='col-sm-12'>You are overlimit your max limit is " + (VeAmount - parseFloat(CLAmount) - OrginalAmount) + CurrIso + AddedMsg + "<label>if you Want To Change Enter Password</label><input type='password' class='CreditLimitPass form-control'  /></div>", "Enter Password"
                                        , false, function () {
                                            $.ajax({
                                                url: "/Receivable/Genral_setting/CheckCreditLimitPasswordOption?Password=" + $(document).find(".CreditLimitPass").val(),
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
                        // Talert("You are overlimit your max limit is " + (parseFloat(CLAmount) - OrginalAmount).toString())
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
                    ModelMsg("This Customer Has No Credit Limit, Over Limit By " + (Left) + " ", "No Credit Limit", true)
                    return false;
                }
            }
        }
    }
    if (InsertJv) {
        $("#ReceivableForm").validate();
        if (!($("#ReceivableForm").valid())) {
            return false;
        } else {
            $.ajax({
                url: "/Receivable/Receivable_transaction/ValidateTransaction",
                data: $("#ReceivableForm").serialize(),
                method: "POST",
                success: function (data) {
                    if (data.status) {
                        var BatchId = "";

                        InsertTransactionData(CompId, $("#BostingToORThrow").val(), $("#TCGE-PostingDate").val(), $("#TCGE-JEDate").val(), $("#TCGE-Reference").val(), $("#TCGE-CurrencyID").val(), $("#TCGE-SystemRate").val(), $("#TCGE-TransactionRate").val(), "RecTrns", "RecTrns", BatchId, "",
                            function (JournalEntryNumber, PO) {
                                $(document).find(".Journal_number").val(JournalEntryNumber)
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
        if ($(document).find(".ReceivablePayment").find("#Orginal_amount").val() != 0) {
            $(document).find(".ReceivablePayment").find("#Transaction_date").val($("#TransDiv").find("#TCGE-JEDate").val());
            $(document).find(".ReceivablePayment").find("#Posting_date").val($("#TransDiv").find("#TCGE-PostingDate").val());
            $(document).find(".ReceivablePayment").find("#Vendor_id").val($("#TransDiv").find("#Vendor_id").find("option:selected").val());
            $(document).find(".ReceivablePayment").find("#Reference").val($("#TransDiv").find("#Desc").val());
            $(document).find(".ReceivablePayment").find("#System_rate").val($("#TransDiv").find("#System_rate").val());
            $(document).find(".ReceivablePayment").find("#Transaction_rate").val($("#TransDiv").find("#Transaction_rate").val());
            $(document).find(".ReceivablePayment").find("#Due_date").val($("#TransDiv").find("#Due_date").val());
            // $(document).find(".ReceivablePayment").find("#Orginal_amount").val($(document).find(".ReceivablePayment").find("#Paid_amount").val());
            $(document).find(".ReceivablePayment").find("#Taken_discount").val(0);
            $(document).find(".ReceivablePayment").find("#Transaction_id").val(data.PayId);
            $(document).find(".ReceivablePayment").find("#Trx_trans_doc_type_id").val(data.TrxId);
            $(document).find(".ReceivablePayment").find("#Paid_amount").val(0);
            $(document).find(".ReceivablePayment").find("#Paid_amountLbl").text(0);
            $(document).find(".ReceivablePayment").find("#Currency_id").val($("#TransDiv").find("#Currency_id").val());
            $(document).find(".ReceivablePayment").find("#SubmitPayment").trigger("click");
            Talert("Transaction Number ", data.Trx_num + " " + $("#Doc_type").find("option:selected").text(), "  Number ", data.Counter)
            RedirectInt(window.location);

        } else {
            RedirectInt(window.location);
        }
    }
    PrintInt("/Recipts/PayRecRecipts?Id=" + data.PayId + "&IsPay=false")
}




//////////
function InsertReceivableJv(Url, PostinNum, CompId, Empty = true, CallBack = null
    , GetIndex = -1, JvMerge = true, RmLastBtn = false, InvMarkTax = false) {
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
                var PorS = "S";
                if ($("#Doc_type").val() == Doc_type.Return) {
                    PorS = "P";
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
                if (RmLastBtn) {
                    $("#TCGE-TTbl").find("tr").last().find("#dmt").remove()
                    $("#TCGE-TTbl").find("tr").last().find("#emt").remove()
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
function GetPaymentJv(Url,EmptyJv=false, CallBack = null) {
    $.ajax({
        url: Url,
        method: "POST",
        success: function (data) {
            if (EmptyJv) {
                EmptyGlTransaction();
            }
            $.each(data, function (k, i) {
                ManyJvAction(i);
            })
            if (CallBack != null) {
                CallBack(data);
            }
        }
    })
}
function CalcCustomerBalance() {
    ThisTotal = GetMaskNumber(CLAmount - GetMaskNumber($(document).find(".ReceivablePayment").find("#Orginal_amountMask").val()));
    $("#CBTI").text(MaskThisMoney(ThisTotal));
    $("#OCBTI").text(MaskThisMoney(CLAmount));
    $("#CB").text(MaskThisMoney(CLAmount));
}
$(document).on("change focusout", "#Orginal_amountMask", function () {
    CalcCustomerBalance()
})