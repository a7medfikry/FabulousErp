var regRemoveCurrFormate = /[^\d.]/g;
var requiredDecimalNum = 1,
    regRemoveCurrFormate = /[^\d.]/g,
    prefix = '',
    suffix = '',
    thousands = '',
    decimal = '';
var MaskCurr = {
    requiredDecimalNum: 1,
    regRemoveCurrFormate: /[^\d.]/g,
    prefix: '',
    suffix: '',
    thousands: '',
    decimal: '',
    decimalNumber: 2,
    Set: false
};
$(function () {
    $(document).find("#Create:visible").each(function () {
        DisableAjax($(this));
    })
    $(document).find("#Submit:visible").each(function () {
        DisableAjax($(this));
    })
})


function RunAfterAjax(Func, TimeOut = 100) {
    var AjaxInter = setInterval(function () {
        if ($.active <= 0) {
            clearInterval(AjaxInter);
            Func();
        }
    }, TimeOut)
}
function RunAfterEveryAjax(Func) {
    $(document).ajaxComplete(function () {
        Func();
    })
}
function SetMaskCurr(result) {
        requiredDecimalNum = result.DecimalNumber;
        prefix = result.Prefix;
        suffix = result.Suffix;
        thousands = result.Thousands;
        decimal = result.Decimal
        $("#TCGE-DecimalNumber").text(result.DecimalNumber);
        $("#TCGE-GurrencyFormate").maskMoney({ allowNegative: true, prefix: '' + result.Prefix + '', suffix: '' + result.Suffix + '', thousands: '' + result.Thousands + '', decimal: '' + result.Decimal + '', precision: parseInt(result.DecimalNumber) });

        $("#TCGE-TransactionRate").maskMoney({ prefix: '' + result.Prefix + '', suffix: '' + result.Suffix + '', thousands: '' + result.Thousands + '', decimal: '' + result.Decimal + '', precision: parseInt(result.DecimalNumber) });
        $(".TCGE-TransactionRate").maskMoney({ prefix: '' + result.Prefix + '', suffix: '' + result.Suffix + '', thousands: '' + result.Thousands + '', decimal: '' + result.Decimal + '', precision: parseInt(result.DecimalNumber) });
        $("#TCGE-OriginalAmount").maskMoney({ prefix: '' + result.Prefix + '', suffix: '' + result.Suffix + '', thousands: '' + result.Thousands + '', decimal: '' + result.Decimal + '', precision: parseInt(result.DecimalNumber) });
        $("#CBT-amount").maskMoney({ prefix: '' + result.Prefix + '', suffix: '' + result.Suffix + '', thousands: '' + result.Thousands + '', decimal: '' + result.Decimal + '', precision: parseInt(result.DecimalNumber) });
        $(".CBT-amount").maskMoney({ prefix: '' + result.Prefix + '', suffix: '' + result.Suffix + '', thousands: '' + result.Thousands + '', decimal: '' + result.Decimal + '', precision: parseInt(result.DecimalNumber) });
        $("#TCGE-Debit").maskMoney({ prefix: '' + result.Prefix + '', suffix: '' + result.Suffix + '', thousands: '' + result.Thousands + '', decimal: '' + result.Decimal + '', precision: parseInt(result.DecimalNumber) });
        $("#TCGE-Credit").maskMoney({ prefix: '' + result.Prefix + '', suffix: '' + result.Suffix + '', thousands: '' + result.Thousands + '', decimal: '' + result.Decimal + '', precision: parseInt(result.DecimalNumber) });
        $("#TCGE-PUAAccDisAmount").maskMoney({ prefix: '' + result.Prefix + '', suffix: '' + result.Suffix + '', thousands: '' + result.Thousands + '', decimal: '' + result.Decimal + '', precision: parseInt(result.DecimalNumber) });
        $("#TCGE-PUCCostCenterAmountForMain").maskMoney({ prefix: '' + result.Prefix + '', suffix: '' + result.Suffix + '', thousands: '' + result.Thousands + '', decimal: '' + result.Decimal + '', precision: parseInt(result.DecimalNumber) });
        $("#TCGE-PUCCostAccAmount").maskMoney({ prefix: '' + result.Prefix + '', suffix: '' + result.Suffix + '', thousands: '' + result.Thousands + '', decimal: '' + result.Decimal + '', precision: parseInt(result.DecimalNumber) });
        $("#TCGE-PUAAccDisPercentage").maskMoney({ thousands: '' + result.Thousands + '', decimal: '' + result.Decimal + '', precision: parseInt(2) });
        $("#TCGE-PUCCostAccPercentage").maskMoney({ thousands: '' + result.Thousands + '', decimal: '' + result.Decimal + '', precision: parseInt(2) });
        $("#TCGE-PUCCostCenterPercentageForMain").maskMoney({ thousands: '' + result.Thousands + '', decimal: '' + result.Decimal + '', precision: parseInt(2) });

        //for tax transaction
        $("#TCT-quantity").maskMoney({ thousands: '' + result.Thousands + '', decimal: '' + result.Decimal + '', precision: parseInt(result.DecimalNumber) });
        $("#TCT-discount").maskMoney({ prefix: '' + result.Prefix + '', suffix: '' + result.Suffix + '', thousands: '' + result.Thousands + '', decimal: '' + result.Decimal + '', precision: parseInt(result.DecimalNumber) });
        $("#TCT-vatAmount").maskMoney({ prefix: '' + result.Prefix + '', suffix: '' + result.Suffix + '', thousands: '' + result.Thousands + '', decimal: '' + result.Decimal + '', precision: parseInt(result.DecimalNumber) });
        $("#TCT-taxAmount").maskMoney({ prefix: '' + result.Prefix + '', suffix: '' + result.Suffix + '', thousands: '' + result.Thousands + '', decimal: '' + result.Decimal + '', precision: parseInt(result.DecimalNumber) });

        MaskCurr.prefix = prefix;
        MaskCurr.suffix = suffix;
        MaskCurr.thousands = thousands;
        MaskCurr.decimal = decimal;
        MaskCurr.decimalNumber = result.DecimalNumber;
        MaskCurr.Set = true;

    //$.ajax({
    //    contentType: 'application/json; charset=utf-8',
    //    dataType: 'json',
    //    method: "get",
    //    url: "/api/TransactionApi/GetCurrencyFormate?companyID=" + CompId,
    //    success: function (result) {
    //        requiredDecimalNum = result.DecimalNumber;
    //        prefix = result.Prefix;
    //        suffix = result.Suffix;
    //        thousands = result.Thousands;
    //        decimal = result.Decimal
    //        $("#TCGE-DecimalNumber").text(result.DecimalNumber);
    //        $("#TCGE-GurrencyFormate").maskMoney({ allowNegative: true, prefix: '' + result.Prefix + '', suffix: '' + result.Suffix + '', thousands: '' + result.Thousands + '', decimal: '' + result.Decimal + '', precision: parseInt(result.DecimalNumber) });

    //        $("#TCGE-TransactionRate").maskMoney({ prefix: '' + result.Prefix + '', suffix: '' + result.Suffix + '', thousands: '' + result.Thousands + '', decimal: '' + result.Decimal + '', precision: parseInt(result.DecimalNumber) });
    //        $(".TCGE-TransactionRate").maskMoney({ prefix: '' + result.Prefix + '', suffix: '' + result.Suffix + '', thousands: '' + result.Thousands + '', decimal: '' + result.Decimal + '', precision: parseInt(result.DecimalNumber) });
    //        $("#TCGE-OriginalAmount").maskMoney({ prefix: '' + result.Prefix + '', suffix: '' + result.Suffix + '', thousands: '' + result.Thousands + '', decimal: '' + result.Decimal + '', precision: parseInt(result.DecimalNumber) });
    //        $("#CBT-amount").maskMoney({ prefix: '' + result.Prefix + '', suffix: '' + result.Suffix + '', thousands: '' + result.Thousands + '', decimal: '' + result.Decimal + '', precision: parseInt(result.DecimalNumber) });
    //        $(".CBT-amount").maskMoney({ prefix: '' + result.Prefix + '', suffix: '' + result.Suffix + '', thousands: '' + result.Thousands + '', decimal: '' + result.Decimal + '', precision: parseInt(result.DecimalNumber) });
    //        $("#TCGE-Debit").maskMoney({ prefix: '' + result.Prefix + '', suffix: '' + result.Suffix + '', thousands: '' + result.Thousands + '', decimal: '' + result.Decimal + '', precision: parseInt(result.DecimalNumber) });
    //        $("#TCGE-Credit").maskMoney({ prefix: '' + result.Prefix + '', suffix: '' + result.Suffix + '', thousands: '' + result.Thousands + '', decimal: '' + result.Decimal + '', precision: parseInt(result.DecimalNumber) });
    //        $("#TCGE-PUAAccDisAmount").maskMoney({ prefix: '' + result.Prefix + '', suffix: '' + result.Suffix + '', thousands: '' + result.Thousands + '', decimal: '' + result.Decimal + '', precision: parseInt(result.DecimalNumber) });
    //        $("#TCGE-PUCCostCenterAmountForMain").maskMoney({ prefix: '' + result.Prefix + '', suffix: '' + result.Suffix + '', thousands: '' + result.Thousands + '', decimal: '' + result.Decimal + '', precision: parseInt(result.DecimalNumber) });
    //        $("#TCGE-PUCCostAccAmount").maskMoney({ prefix: '' + result.Prefix + '', suffix: '' + result.Suffix + '', thousands: '' + result.Thousands + '', decimal: '' + result.Decimal + '', precision: parseInt(result.DecimalNumber) });
    //        $("#TCGE-PUAAccDisPercentage").maskMoney({ thousands: '' + result.Thousands + '', decimal: '' + result.Decimal + '', precision: parseInt(2) });
    //        $("#TCGE-PUCCostAccPercentage").maskMoney({ thousands: '' + result.Thousands + '', decimal: '' + result.Decimal + '', precision: parseInt(2) });
    //        $("#TCGE-PUCCostCenterPercentageForMain").maskMoney({ thousands: '' + result.Thousands + '', decimal: '' + result.Decimal + '', precision: parseInt(2) });

    //        //for tax transaction
    //        $("#TCT-quantity").maskMoney({ thousands: '' + result.Thousands + '', decimal: '' + result.Decimal + '', precision: parseInt(result.DecimalNumber) });
    //        $("#TCT-discount").maskMoney({ prefix: '' + result.Prefix + '', suffix: '' + result.Suffix + '', thousands: '' + result.Thousands + '', decimal: '' + result.Decimal + '', precision: parseInt(result.DecimalNumber) });
    //        $("#TCT-vatAmount").maskMoney({ prefix: '' + result.Prefix + '', suffix: '' + result.Suffix + '', thousands: '' + result.Thousands + '', decimal: '' + result.Decimal + '', precision: parseInt(result.DecimalNumber) });
    //        $("#TCT-taxAmount").maskMoney({ prefix: '' + result.Prefix + '', suffix: '' + result.Suffix + '', thousands: '' + result.Thousands + '', decimal: '' + result.Decimal + '', precision: parseInt(result.DecimalNumber) });

    //        MaskCurr.prefix = prefix;
    //        MaskCurr.suffix = suffix;
    //        MaskCurr.thousands = thousands;
    //        MaskCurr.decimal = decimal;
    //        MaskCurr.decimalNumber = result.DecimalNumber;
    //        MaskCurr.Set = true;
    //    }
    //});

}
var SpecialChar = /[^a-zA-Z0-9-. / & _ ' ( ) %]/g;
$.expr[':'].textEquals = function (a, i, m, f) {
    var textToFind = m[3].replace(SpecialChar, "").trim()//.replace(/[-[\]{}(')*+?.[,\\^$|#\s]/g, '\\$&'); // escape special character for regex
    if ($(a).prop("tagName").toLowerCase() == "input") {
        return $(a).val().trim().match("^" + textToFind + "$");

    } else {
        return $(a).text().replace(":", "").replace("*", "").trim().match("^" + textToFind + "$");
    }
};
function Loader(CallBack, width, height) {

    $("#LoaderModel").modal("show");
    $("#LoaderModel").find('.modal-dialog').css("max-width", "100%");
    $("#LoaderModel").find('.modal-content').width(width);
    $("#LoaderModel").find('.modal-content').height(height);
    $("#LoaderModel").find('h2').css("line-height", height);
    $("#LoaderModel").find('.modal-content').css("margin", "auto");

    try {
        CallBack();
    }
    catch (Error) {
    }
}
function HideLoader() {
    $("#LoaderModel").modal("hide");
}
function date_diff_indays(date1, date2) {
    dt1 = new Date(date1);
    dt2 = new Date(date2);
    return Math.floor((Date.UTC(dt2.getFullYear(), dt2.getMonth(), dt2.getDate()) - Date.UTC(dt1.getFullYear(), dt1.getMonth(), dt1.getDate())) / (1000 * 60 * 60 * 24));
}
function days_of_a_year(year) {
    return isLeapYear(year) ? 366 : 365;
}
function date_diff_inyears(date1, date2) {
    dt1 = new Date(date1);
    dt2 = new Date(date2);
    return Math.floor((Date.UTC(dt2.getFullYear(), dt2.getMonth(), dt2.getDate()) - Date.UTC(dt1.getFullYear(), dt1.getMonth(), dt1.getDate())));
}
function isLeapYear(year) {
    return year % 400 === 0 || (year % 100 !== 0 && year % 4 === 0);
}
function FixedAssetsModel(html, HideSave) {
    FixedAssetsHide();
    setTimeout(function () {
        $("#FixedAssetsModal").modal("show")
        if (HideSave) {
            $("#FixedAssetsModal").find(".FixedAssetSave").hide();
        } else {
            $("#FixedAssetsModal").find(".FixedAssetSave").show();
        }
        $("#FixedAssetsModal").find(".modal-body").html(html);
    }, 1000)

}
function FixedAssetsHide() {
    $("#FixedAssetsModal").modal("hide")
}
var ModelCallBack = null;
var ModalShown = false;
function ModelMsg(html, title, HideSubmit = true, CallBack = null, CloseCallBack = null, SubmitTxt = "Submit", AfterShow = null, Width = null, HideClose = false) {
    if (!ModelVisiable()) {
        PrivateModelMsg(html, title, HideSubmit, CallBack, CloseCallBack, SubmitTxt, AfterShow, Width, HideClose)
    } else {
        setTimeout(function () {
            PrivateModelMsg(html, title, HideSubmit, CallBack, CloseCallBack, SubmitTxt, AfterShow, Width, HideClose)
        }, 500)
    }
}

function PrivateModelMsg(html, title, HideSubmit = false, CallBack = null, CloseCallBack = null, SubmitTxt = "Submit", AfterShow = null, Width = null, HideClose = false) {
    // Show the backdrop
    ModalShown = true;
    $("#Modal").modal("show")
    $("#Modal").find(".modal-title").html(title);
    $("#Modal").find(".modal-body").html(html);
    $("#Modal").find("#ModelSubmit").text(SubmitTxt);
    $("#Modal").find('.modal-dialog').removeAttr("style")
    $("#Modal").find('.modal-content').removeAttr("style")
    if (AfterShow != null) {
        AfterShow();
    }
    if (Width != null) {
        $("#Modal").find('.modal-dialog').css("max-width", "100%");
        $("#Modal").find('.modal-content').width(Width);
        $("#Modal").find('.modal-content').css("margin", "auto");
    }
    if (HideSubmit) {
        $("#Modal").find("#ModelSubmit").hide();
    } else {
        $("#Modal").find("#ModelSubmit").show();
    }
    if (HideClose) {
        $("#Modal").find("#ModelClose").hide();
    } else {
        $("#Modal").find("#ModelClose").show();
    }
    $(document).on("click", '#ModelSubmit', function () {
        if (CallBack != null) {
            CallBack(a = null, b = null, c = null);
        }
        if (ModelCallBack != null) {
            ModelCallBack(a = null, b = null, c = null);
        }
    });
    $('#Modal').on('hidden.bs.modal', function () {
        if (CloseCallBack != null) {
            CloseCallBack();
        }
        CallBack = null;
        $(document).find(".modal-backdrop").remove();
        ModalShown = false;
    })
}
function ModelVisiable() {
    if ($("#Modal").is(":visible") || ModalShown) {
        return true;
    } else {
        return false;
    }
}
$.fn.serializeObject = function () {
    var o = {};
    var a = this.serializeArray();
    $.each(a, function () {
        if (o[this.name]) {
            if (!o[this.name].push) {
                o[this.name] = [o[this.name]];
            }
            o[this.name].push(this.value || '');
        } else {
            o[this.name] = this.value || '';
        }
    });
    return o;
};
function EmptyGlTransaction() {
    $("#TCGE-GTbl").find("#TCGE-TTbl").find("tr").remove();
    SumDebitAndCredit();
}
function SetGlTbl(Cost, data, RmUpdDelBtnFrstRow, Append = false, Merge = false) {
    if (!Append) {
        $("#TCGE-GTbl").find("#TCGE-TTbl").find("tr").remove();
    }
    data = data[0]

    Cost = Math.round(Cost * 100) / 100;

    if (data.DebitAmount == null) {
        data.DebitAmount = Cost;
    }
    if (data.CreditAmount == null) {
        data.CreditAmount = Cost;
    }
    if (data.DebitAccId == null) {
        data.DebitAccId = data.Debit;
    }
    if (data.CreditAccId == null) {
        data.CreditAccId = data.Credit;
    }

    if (!data.Orginal_debit) {
        data.Orginal_debit = data.DebitAmount;
    } else if (!data.Orginal_crdit) {
        data.Orginal_crdit = data.CreditAmount;
    }

    $(".CreditAccount").val(data.DebitAccId)
    $(".DebitAccount").val(data.CreditAccId)
    $(".DebitCreditSection").find("#TCGE-AccountID").val(data.CreditAccId);
    $(".DebitCreditSection").find("#TCGE-Describtion").val(" ")
    var TBody = $("#TCGE-GTbl").find("#TCGE-TTbl");
    var Length = TBody.find("tr").length + 1

    var Row = GetNextDCRowClass(); //'row_' + Length
    var CruencyId = $("#Currency_id option:selected").val();
    var CruencyName = $("#Currency_id option:selected").text();

    var DocType = $(document).find("#TCGE-SUD").val();
    if (data.DocType) {
        DocType = data.DocType;
    }

    var Desc = "";
    if ($("#Desc").length > 0) {
        Desc = $("#Desc").val();
    }
    else if ($("#Reference").length > 0) {
        Desc = $("#Reference").val();
    }

    var RemoveBtn = RmUpdDelBtnFrstRow;
    if (data.ShowBtn == false) {
        RemoveBtn = true;
    }

    if (data.DebitAccId && data.DebitAmount!=0) {
        //GetAccountDetails(data.DebitAccId, CruencyId, false, CheckAnalyticAndCostCenter, true);
        AddToMainTbl(data.DebitAccId, data.DebitAccName, DocType, data.DebitAccNum, Desc, data.DebitAmount, data.DebitAmount, 0, false, RemoveBtn,false,false)
    }
    if (data.CreditAccId && data.CreditAmount != 0) {
        //GetAccountDetails(data.CreditAccId, CruencyId, false, CheckAnalyticAndCostCenter, true);
        AddToMainTbl(data.CreditAccId, data.CreditAccName, DocType, data.CreditAccNum, Desc, data.CreditAmount, 0, data.CreditAmount, false, RemoveBtn, false, false)
    }

    //if (data.ISO) {
    //    CruencyName = data.ISO;
    //}


    //OrginalCur = $("#Currency_id option:selected").text();

    //if (data.Orginal_curr) {
    //    OrginalCur = data.Orginal_curr;
    //}
    //var Btns1 = "";
    //var Btns2 = "";

    //if (data.ShowBtn == false) {
    //    RmUpdDelBtnFrstRow = true;
    //}

    //if (RmUpdDelBtnFrstRow || data.ShowBtn == false) {
    //        Btns1 = '<button type="button" class="btn btn-sm mr-1 MoreDetailsT"><span class="fa fa-eye"></span></button>' + ' <button type="button" class="btn btn-sm btn-warning mr-1 GetAnalyticDetails"><span class="">A</span></button>'
    //        Btns2 = '<button type="button" class="btn btn-sm mr-1 MoreDetailsT"><span class="fa fa-eye"></span></button>' + ' <button type="button" class="btn btn-sm btn-warning mr-1 GetAnalyticDetails"><span class="">A</span></button>'
    //} else {
    //    Btns1 = '<button type="button" class="btn btn-sm mr-1 MoreDetailsT"><span class="fa fa-eye"></span></button><button type="button" id="dmt" class="btn btn-danger btn-sm mr-1" onclick="DeleteT(' + Length + ');"><span class="fa fa-trash-o"></span></button><button type="button" id="emt" class="btn btn-warning btn-sm" onclick="EditT(' + Length + ');"><span class="fa fa-edit"></span></button>' + '<button type="button" class="btn btn-sm btn-warning mr-1 GetAnalyticDetails"><span class="">A</span></button>'
    //    Length = Length + 1
    //    Btns2 = '<button type="button" class="btn btn-sm mr-1 MoreDetailsT"><span class="fa fa-eye"></span></button><button type="button" id="dmt" class="btn btn-danger btn-sm mr-1" onclick="DeleteT(' + Length + ');"><span class="fa fa-trash-o"></span></button><button type="button" id="emt" class="btn btn-warning btn-sm" onclick="EditT(' + Length + ');"><span class="fa fa-edit"></span></button>' + '<button type="button" class="btn btn-sm btn-warning mr-1 GetAnalyticDetails"><span class="">A</span></button>'

    //}
    //var DocType = $(document).find("#TCGE-SUD").val();
    //if (data.DocType) {
    //    DocType = data.DocType;
    //}
    //var Desc = "";
    //if ($("#Desc").length > 0) {
    //    Desc = $("#Desc").val();
    //}
    //else if ($("#Reference").length > 0) {
    //    Desc = $("#Reference").val();
    //}
    //if (data.Debit != 0) {
    //    if ((data.DebitAccId) && data.DebitAccName) {
    //        $("#TCGE-GTbl").find("#TCGE-TTbl").append('<tr class="' + Row + '"><td>' + Btns1 + '</td><td class="hide-normal TCGE-TblAccID">' + data.DebitAccId + '</td><td>' + data.DebitAccName + '</td><td>' + DocType + '</td><td class="hide-normal">' + data.DebitAccNum + '</td><td class="hide-normal">' + Desc+ '</td><td>' + setCurrecnyCurrFormate(data.Orginal_debit) + '</td><td class="sDebitTbl">' + setCurrecnyCurrFormate(data.DebitAmount) + '</td><td class="sCreditTbl">0</td></tr>')
    //        var Length = TBody.find("tr").length + 1
    //        var Row = 'row_' + Length
    //    }
    //}
    //if (data.Credit != 0) {
    //    if ((data.CreditAccId) && data.CreditAccName) {
    //        $("#TCGE-GTbl").find("#TCGE-TTbl").append('<tr class="' + Row + '"><td>' + Btns2 + '</td><td class="hide-normal TCGE-TblAccID">' + data.CreditAccId + '</td><td>' + data.CreditAccName + '</td><td>' + DocType + '</td><td class="hide-normal">' + data.CreditAccNum + '</td><td class="hide-normal">' + Desc  + '</td><td>' + setCurrecnyCurrFormate(data.Orginal_credit) + '</td><td class="sDebitTbl">0</td><td class="sCreditTbl">' + setCurrecnyCurrFormate(data.CreditAmount) + '</td></tr>')
    //    }
    //}
    SumDebitAndCredit();
    if (Merge) {
        MergJv();
    }
}
function SetExchangeRate(Debit, Credit) {
    $("#TCGE-GTbl").find('tr').eq(1).find('td').eq(7).text(Debit + " EGP");
    $("#TCGE-GTbl").find('tr').eq(1).find('td').eq(8).text("0");

    $("#TCGE-GTbl").find('tr').eq(2).find('td').eq(7).text("0");
    $("#TCGE-GTbl").find('tr').eq(2).find('td').eq(8).text(Credit + " EGP");
    $("#DebitTblFoot").text(Debit)
    $("#CreditTblFoot").text(Credit)

}
function GetAccountsAndExchangeRate(Url, Amount, Date, MakeEmpty, RmUpadteDeleteRow, BeforeSendCallBack
    , SuccessFailCallBack, SuccessCallBack) {

    $.ajax({
        method: "POST",
        url: Url,
        beforeSend: function (xhr, opts) {
            if (BeforeSendCallBack != null) {
                BeforeSendCallBack(xhr, opts)
            }
        },
        success: function (Accountsdata) {
            if (Accountsdata == "") {
                Talert("No Accounts Are Found For That Assets Class")
                $("#" + MakeEmpty).val("")
            } else {
                if ($("#Reference").val()) {
                    $.each(Accountsdata, function (k, i) {
                        i.DebitAccNum = $("#Reference").val();
                        i.CreditAccNum = $("#Reference").val();

                        i.Orginal_credit = Amount;
                        i.Orginal_debit = Amount;
                    })
                }
                SetGlTbl(Amount, Accountsdata, RmUpadteDeleteRow)
                $.ajax({
                    method: "POST",
                    url: "/C_GeneralEntryTransaction/CalculateTransactionRate?CurrencyId=" + $("#TCGE-CurrencyID option:selected").val() + "&Amount=" + Amount + "&CompareDate=" + Date,
                    success: function (data) {
                        if (data.Msg) {
                            Talert(data.Msg)
                            if (SuccessFailCallBack != null) {
                                SuccessFailCallBack()
                            }
                        } else {
                            SetExchangeRate(data.Amount, data.Amount);
                            if (SuccessCallBack != null) {
                                SuccessCallBack(data.Amount, Accountsdata);
                            }
                        }
                    }
                })
            }
        }
    })
}
function ValidatePeriod(CallBack) {
    if ($("#TCGE-GlobalError").text() != "") {
        var Error = false;
        if ($("#TCGE-GlobalError").text() != "Wrong Date Formate..!") {
            Talert($("#TCGE-GlobalError").text());
            Error = true;
        }
        $("#TCGE-GlobalError").text("")
        if (CallBack != null) {
            CallBack(Error);
        }
    }
}
$(document).on("click", ".ToggleTransAction", function () {
    if ($("#Language").val() == "Arabic") {
        if ($(this).text().trim().toLowerCase() == "عرض القيد".toLowerCase()) {
            $(this).text("إخفاء قيد");
            $("#JVTransactionSec").show("ease");
        } else {
            $(this).text("عرض القيد");
            $("#JVTransactionSec").hide("ease");
        }
    } else {
        if ($(this).text().toLowerCase() == "Show Jv".toLowerCase()) {
            $(this).text("Hide Jv");
            $("#JVTransactionSec").show("ease");
        } else {
            $(this).text("Show Jv");
            $("#JVTransactionSec").hide("ease");
        }
    }

})

//function GetTransaction(PostingNum) {
//    $.ajax({
//        url: "/api/TransactionApi/GetTransactionData?postingNumber=" + PostingNum,
//        method: "GET",
//        success: function (data) {
//            var Cost = data.ShowTransactions[0].OriginalAmount
//            var TblCost = [{
//                Credit: data.ShowTransactions[1].Credit,
//                Debit: data.ShowTransactions[0].Debit,
//                CreditAccNum: data.ShowTransactions[1].AccountID,
//                DebitAccNum: data.ShowTransactions[0].AccountID,
//                DebitAccName: data.ShowTransactions[0].AccountName,
//                CreditAccName: data.ShowTransactions[1].AccountName,
//                Describtion: "",
//                ISO: data.ShowHeader.ISO
//            }];
//            SetGlTbl(Cost, TblCost, true, true);
//        }
//    });
//}
function PrintThis(AlsoPrint, removeFirstTh, LandScape, AlsoPrintPrePend, CallBack, Title, RemoveLogo = false
    , IgnorMainTbl, beforePrint, AddedStyle, AddHeader = true, EndAddedStyle = "", CenterTitle) {
    $.ajax({
        url: "/C_ReportsPrint/ReportsPrintHeader",
        dataType: "html",
        success: function (data) {
            if (!IgnorMainTbl) {
                $(".table").wrap("<section id='PrintSec' style='background:#fff;'></section>")
                if (AlsoPrint) {
                    if (AlsoPrintPrePend) {
                        $("#PrintSec").prepend("<section class='remove' style='width:100%;display:block;'>" + AlsoPrint + "</section>");
                    } else {
                        $("#PrintSec").append("<section class='remove' style='width:100%;display:block;'>" + AlsoPrint + "</section>");

                    }
                }
            } else {
                $('#' + AlsoPrint).wrap("<section id='PrintSec' style='background:#fff;'></section>")
            }

            if (removeFirstTh) {
                $(document).find(".remove").find("table").find("tr").each(function () {
                    $(this).find("th").first().remove();
                    $(this).find("td").first().remove();
                });
            }
            $("#PrintSec").find("button").remove();
            $("#PrintSec").find(".removeBeforePrint").remove();
            if (!AddedStyle) {
                AddedStyle = "";
            }
            if (LandScape) {
                AddedStyle += "@media print{@page {size: landscape}}";
            }
            var Header = "";
            if (AddHeader) {
                Header = "<div id='HeaderId'>" + data + "</div>";

            }
            if (Title) {
                var Center = "";
                if (CenterTitle) {
                    Center = "center"
                }
                Header += "<div class='remove " + Center + "'><h2>" + Title + "</h2></div>";
            }

            $(document).find("#PrintSec").prepend(Header);
            if (RemoveLogo) {
                $(document).find("#PrintSec").find(".header-logo").remove();
            }
            $(document).find(".NoPrint").remove();
            if (beforePrint) {
                beforePrint();
            }
            $("#PrintSec").print({
                noPrintSelector: "a",
               // append: "<style>" + AddedStyle + ".col-md-7{width:30%;}.col-md-2{width:15%;}.col-md-10{width:80%;}.table { margin-top: 20px; }.header-details{width:100%;}label{display:inline-block !important;width:auto !important;width:15% !important}input{ display:inline-block !important;}@page {margin:20px;margin-bottom: 100px;color: #000;background:#fff;}body{background:#fff;}.col-md{width:50%;}" + EndAddedStyle + "</style>",
                deferred: $.Deferred().done(function () {
                    $(document).find(".remove").remove();
                    $(document).find("#HeaderId").remove();
                    if (!IgnorMainTbl) {
                        $(".table").unwrap()
                    } else {
                        $('#' + AlsoPrint).unwrap();
                    }
                    if (CallBack) {
                        CallBack();
                    }
                })
            });
        }
    })
}
function JustPrintThis(Id, Title, CallBack, Style) {
    var Header = "";
    if (Title) {
        Header += "<div class='remove' id='HeaderId'><h2>" + Title + "</h2></div>";
    }
    //.col-md-2{width:15%;
    $(".col-md-2").addClass("RmColMd2");
    $(".col-md-2").removeClass("col-md-2");
    $(document).find("#" + Id).prepend(Header);
    $("#" + Id).print({
        noPrintSelector: "a,.NoPrint,.hide,.hide-normal",
        append: "<style>.col-md-6{width:48%;}.col-md-3{width:45%;}.col-md-7{width:30%;}}.col-md-10{width:80%;}.table { margin-top: 20px; }.header-details{width:100%;}label{display:inline-block !important;width:auto !important;width:15% !important}input{ display:inline-block !important;}@page {margin:20px;margin-bottom: 100px;color: #000;background:#fff;}body{background:#fff;}.col-md{width:50%;}*{direction:rtl;}div{display: inline-block !important;}" + Style + "</style>",
        deferred: $.Deferred().done(function () {
            if (CallBack) {
                CallBack();
            }
            $(document).find("#HeaderId").remove();
            $(".RmColMd2").addClass("col-md-2");
            $(".col-md-2").removeClass("RmColMd2");
        })
    });
}
function PrintAsImage(Id, AddHeader = true, CallBack = null) {
    var ImPr = "<section id='ImPr' style='padding:20px;'><style>.form-control,input,[disabled],button,.filter-option-inner-inner{ background: unset !important;border: unset!important;box-shadow: none !important;}.dropdown-toggle::after{display:none;}</style> </section>";
    if (AddHeader) {
        $.ajax({
            url: "/C_ReportsPrint/ReportsPrintHeader",
            dataType: "html",
            success: function (data) {
                if ($('body').find("#ImPr").length <= 0) {
                    $('body').append(ImPr)
                }
                $('body').find("#ImPr").append(data);
                AddToImage(Id, CallBack)

                // $('body').find("#ImPr").append(data);
                //$("#tempheader").append(data);
                //html2canvas(document.querySelector("#tempheader"), { scrollY: -window.scrollY }).then(canvas => {
                //    //var myImage = canvas.toDataURL("image/png");
                //    var myImage1 = canvas.toDataURL("image/png");
                //    $('body').find("#ImPr").append("<img src='" + myImage1 + "' style='float: right;'>");
                //    document.body.appendChild(canvas)
                //    AddToImage(Id)
                //    //$('body').find("#tempheader").remove();
                //})
            }
        })
    } else {
        if ($('body').find("#ImPr").length <= 0) {
            $('body').append(ImPr)
        }
        AddToImage(Id, CallBack)
    }
   
}
function AddToImage(Id,CallBack=null) {
    html2canvas(document.querySelector("#" + Id), { scrollY: -window.scrollY, imageTimeout:0 }).then(canvas => {
        //document.body.appendChild(canvas);
        var myImage = canvas.toDataURL("image/png");
        $('body').find("#ImPr").append("<img src='" + myImage +"' style='width:100%;float: right;' />");
        $('body').find("#ImPr").print({
            deferred: $.Deferred().done(function () {
                $('body').find("#ImPr").remove();
                if (CallBack != null) {
                    CallBack();
                }
            })
        })
        //$("<section>" + data + "' + <img src='" + myImage + "' style='width:100%;height:100%;padding:20px;padding-right:0px;'>+'</section>")
        //    .print()
    });
}
function JustPrintThisWithStyle(Div, Title, CallBack, Style) {
    var Header = "";
    if (Title) {
        Header += "<div class='remove' id='HeaderId'><h2>" + Title + "</h2></div>";
    }
    $(document).find("#" + Div).prepend(Header);
    $("#" + Div).print({
        noPrintSelector: "a,.NoPrint",
        append: "<style></style>",
        deferred: $.Deferred().done(function () {
            if (CallBack) {
                CallBack();
            }
            $(document).find("#HeaderId").remove();
        })
    });
}
function ExportToExcel(FileName, exclude, ManyTable = false, BeforeExport = null, CallBack = null, ExportTblIds = null) {
    if (BeforeExport != null) {
        BeforeExport();
    }
    if (ExportTblIds == null) {
        if (ManyTable) {
            var TblHtml = "";
            $(document).find("table").each(function () {
                TblHtml += $(this).html();
            });
            $(TblHtml).table2excel({
                exclude: ".noExl,.hide,a," + exclude + "",
                name: FileName,
                filename: FileName
            });
        } else {
            $('table').table2excel({
                exclude: ".noExl,.hide,a," + exclude + "",
                name: FileName,
                filename: FileName
            });
        }
    } else {
        $("#" + ExportTblIds).table2excel({
            exclude: ".noExl,.hide,a," + exclude + "",
            name: FileName,
            filename: FileName
        });
    }
    if (CallBack != null) {
        CallBack();
    }

}

function tablesToExcel(table, name, append, filename) {
    var uri = 'data:application/vnd.ms-excel;base64,'
        , template = '<html xmlns:o="urn:schemas-microsoft-com:office:office" xmlns:x="urn:schemas-microsoft-com:office:excel" xmlns="http://www.w3.org/TR/REC-html40"><head><!--[if gte mso 9]><xml><x:ExcelWorkbook><x:ExcelWorksheets>'
        , templateend = '</x:ExcelWorksheets></x:ExcelWorkbook></xml><![endif]--></head>'
        , body = '<body>'
        , tablevar = '<table>{table'
        , tablevarend = '}</table>'
        , bodyend = '</body></html>'
        , worksheet = '<x:ExcelWorksheet><x:Name>'
        , worksheetend = '</x:Name><x:WorksheetOptions><x:DisplayGridlines/></x:WorksheetOptions></x:ExcelWorksheet>'
        , worksheetvar = '{worksheet'
        , worksheetvarend = '}'
        , base64 = function (s) { return window.btoa(unescape(encodeURIComponent(s))) }
        , format = function (s, c) { return s.replace(/{(\w+)}/g, function (m, p) { return c[p]; }) }
        , wstemplate = ''
        , tabletemplate = '';

    var tables = table;

    for (var i = 0; i < tables.length; ++i) {
        wstemplate += worksheet + worksheetvar + i + worksheetvarend + worksheetend;
        tabletemplate += tablevar + i + tablevarend;
    }

    var allTemplate = template + wstemplate + templateend;
    var allWorksheet = body + tabletemplate + bodyend;
    var allOfIt = allTemplate + allWorksheet;

    var ctx = {};
    for (var j = 0; j < tables.length; ++j) {
        ctx['worksheet' + j] = name[j];
    }

    for (var k = 0; k < tables.length; ++k) {
        var exceltable;
        if (!tables[k].nodeType) exceltable = document.getElementById(tables[k]);
        ctx['table' + k] = append[k] + exceltable.innerHTML;
    }

    //document.getElementById("dlink").href = uri + base64(format(template, ctx));
    //document.getElementById("dlink").download = filename;
    //document.getElementById("dlink").click();

    window.location.href = uri + base64.encode(format(allOfIt, ctx));

}

function tablesToExcel(MainDivId, FileName) {
    var sheetName = FileName;
    var fileName = sheetName + ".xls";
    var fileRaw = document.getElementById(MainDivId).innerHTML;
    fileRaw = fileRaw.split("↑").join("");
    var file = fileRaw.split("↓").join("");
    //$log.info("here ", file);

    var uri = 'data:application/vnd.ms-excel;base64,'
        , template = '<html xmlns:o="urn:schemas-microsoft-com:office:office" xmlns:x="urn:schemas-microsoft-com:office:excel" xmlns="http://www.w3.org/TR/REC-html40"><head><!--[if gte mso 9]><?xml version="1.0" encoding="UTF-8" standalone="yes"?><x:ExcelWorkbook><x:ExcelWorksheets><x:ExcelWorksheet><x:WorksheetOptions><x:DisplayGridlines/></x:WorksheetOptions></x:ExcelWorksheet></x:ExcelWorksheets></x:ExcelWorkbook></xml><![endif]--></head><body>{table}</body></html>'
        , base64 = function (s) { return window.btoa(unescape(encodeURIComponent(s))) }
        , format = function (s, c) { return s.replace(/{(\w+)}/g, function (m, p) { return c[p]; }) }

    var toExcel = file;
    var ctx = {
        worksheet: sheetName || '',
        table: toExcel
    };

    var link = document.createElement('a');
    link.download = fileName;
    link.href = uri + base64(format(template, ctx));
    link.click();
}


var GlTransactionTbl = {
    ShowTransactions: [{
        Credit: 0,
        Debit: 0,
        CreditAccNum: "",
        DebitAccNum: "",
        DebitAccName: "",
        CreditAccName: "",
        Describtion: "",
        ISO: "",
        ShowBtn: true
    }]
}
function GetManyJvDataObject(Credit, Debit, CreditAccNum, DebitAccNum
    , DebitAccName, CreditAccName, Describtion, ISO, ShowBtn = true, Orginal = 0, Transaction_rate = 0
    , Orginal_debit = 0, Orginal_credit = 0, DocType = "", Orginal_curr = "", AID = 0) {
    return {
        Credit: Credit,
        Debit: Debit,
        CreditAccNum: CreditAccNum,
        DebitAccNum: DebitAccNum,
        DebitAccName: DebitAccName,
        CreditAccName: CreditAccName,
        Describtion: Describtion,
        ISO: ISO,
        ShowBtn: true,
        Transaction_rate: 0,
        Orginal_debit: Orginal_debit,
        Orginal_credit: Orginal_credit,
        DocType: DocType,
        Orginal_curr: Orginal_curr,
        AID: AID
    };
}
function ManyJvAction(data, RmUpate = true, Merge = false) {
    if (data.ShowTransactions.length % 2 == 1) {
        data.ShowTransactions.push(data.ShowTransactions[data.ShowTransactions.length - 1])
    }
    $.each(data.ShowTransactions, function (k, i) {
        var Cost = i.OriginalAmount


        if (k % 2 == 0) {
            TblCost = [{
                Credit: 0,
                Debit: 0,
                CreditAccNum: "",
                DebitAccNum: "",
                DebitAccName: "",
                CreditAccName: "",
                Describtion: "",
                ISO: "",
                ShowBtn: true,
                Orginal_debit: 0,
                Orginal_credit: 0,
                DocType: "",
                Orginal_curr: "",
                Orginal_curr: ""

            }];
            TblCost[0].Debit = i.Debit;
            TblCost[0].DebitAmount = i.Debit;
            TblCost[0].DebitAccNum = i.AccountID;
            TblCost[0].DebitAccName = i.AccountName;
            TblCost[0].DebitAccId = i.AID
            TblCost[0].Describtion = "";
            TblCost[0].ISO = data.ShowHeader.ISO;
            TblCost[0].Orginal_debit = i.Orginal_debit
            TblCost[0].DocType = data.ShowHeader.DocType;
            TblCost[0].Orginal_curr = i.Orginal_curr;

            TblCost[0].ShowBtn = i.ShowBtn;

        } else {
            TblCost[0].Credit = i.Credit;
            TblCost[0].CreditAmount = i.Credit;

            TblCost[0].CreditAccNum = i.AccountID;
            TblCost[0].CreditAccName = i.AccountName;
            TblCost[0].Describtion = "";
            TblCost[0].ISO = data.ShowHeader.ISO;
            TblCost[0].CreditAccId = i.AID

            TblCost[0].Orginal_credit = i.Orginal_credit
            TblCost[0].DocType = data.ShowHeader.DocType;
            if (!TblCost[0].Orginal_curr) {
                TblCost[0].Orginal_curr = i.Orginal_curr;
            }

            TblCost[0].ShowBtn = i.ShowBtn;

            SetGlTbl(Cost, TblCost, !TblCost[0].ShowBtn, true);
        }
    })
    if (Merge) {
        MergJv();
    }
}
var DecimalPoint;
var MaskSetup;
function MaskMoneyTxt(CompanyID, CallBack = null, CurrencyString) {
    if (!CompanyID) {
        CompanyID = CompId;
    }
    setTimeout(function () {
        if (MaskCurr.Set) {
            requiredDecimalNum = MaskCurr.decimalNumber;
            prefix = MaskCurr.prefix;
            if (!CurrencyString) {
                suffix = MaskCurr.suffix;
            } else {
                suffix = CurrencyString;
                MaskCurr.suffix = CurrencyString;
            }
            thousands = MaskCurr.thousands;
            decimal = MaskCurr.decimal
            $("#TCGE-DecimalNumber").text(MaskCurr.decimalNumber);
            DecimalPoint = requiredDecimalNum;
            MaskSetup = { prefix: '' + prefix + '', suffix: ' ' + suffix + '', thousands: '' + thousands + '', decimal: '' + decimal + '', precision: parseInt(MaskCurr.decimalNumber) };
            $(document).find(".maskmoney").each(function () {
                var Input = $(this).clone();
                $(this).removeClass("maskmoney");
                $(Input).addClass("maskmoney")

                Input = $(Input).attr("name", $(this).attr("name") + "Mask");
                Input = $(Input).attr("Id", $(this).attr("Id") + "Mask");

                var MainId = $(this).attr("Id");
                if (MainId.endsWith("Mask")) {
                    $(this).maskMoney('destroy')
                    $(this).maskMoney({ prefix: '' + prefix + '', suffix: ' ' + suffix + '', thousands: '' + thousands + '', decimal: '' + decimal + '', precision: parseInt(MaskCurr.decimalNumber) });
                    $(this).addClass("maskmoney");
                } else {
                    $(this).hide();
                    $(this).addClass("NoPrint");
                    $(Input).on("focusout change keyup", function () {
                        if (!$(this).hasClass("NotValid")) {
                            if (!isNaN($(this).val())) {
                                $(document).find("#" + MainId).val($(this).val());
                                $(document).find("#" + MainId).trigger("change");
                            } else {
                                var num = $(this).maskMoney('unmasked')[0];
                                $(this).addClass("maskmoney")
                                $(document).find("#" + MainId).val(num);
                                $(document).find("#" + MainId).trigger("change");
                            }
                        }
                    });
                    $(this).after(Input)
                    $(Input).maskMoney({ prefix: '' + prefix + '', suffix: ' ' + suffix + '', thousands: '' + thousands + '', decimal: '' + decimal + '', precision: parseInt(MaskCurr.decimalNumber) });
                }
            })
            if (CallBack != null) {
                CallBack();
            }
        } else {
            MaskMoneyTxt(CompanyID, CallBack, CurrencyString);
        }
    }, 100)

    //$.ajax({
    //    contentType: 'application/json; charset=utf-8',
    //    dataType: 'json',
    //    method: "get",
    //    cache: true,
    //    url: "/api/TransactionApi/GetCurrencyFormate?companyID=" + CompanyID,
    //    success: function (result) {
    //    }
    //});
}
function MaskMoneyLbl(CompanyID, CallBack = null, CurrencyString = "EGP") {
    RunAfterAjax(function () {
        $(document).find(".maskmoney").each(function () {
            var T = parseFloat($(this).text());
            $(this).text(setCurrecnyCurrFormate(T, CurrencyString))
        })
    })
}
function MaskThisMoney(ThisLabel, CurrencyString = "MainCurr") {
    if (CurrencyString == "MainCurr") {
        CurrencyString = CompId;
    }
    if ($("#TCGE-CurrencyID").val()) {
        CurrencyString = $("#TCGE-CurrencyID").val();
    }
    if (!ThisLabel) {
        ThisLabel = "";
    }
    var ThisTxt = setCurrecnyCurrFormate(ThisLabel, CurrencyString);
    var IndexOf = ThisTxt.toString().indexOf("-");
    if (ThisTxt.toString().indexOf("-") < 0
        && ThisLabel.toString().indexOf("-") > -1) {
        return "-" + ThisTxt;
    } else {
        return ThisTxt;
    }
}
function DestroyMaskInput(Id) {
    $("#" + Id+"Mask").maskMoney('destroy')
    $("#" + Id).show();
    $("#" + Id).addClass("maskmoney");
    $("#" + Id + "Mask").remove();
}
function MaskInputNoCurr(Input, suffix="") {
    requiredDecimalNum = MaskCurr.decimalNumber;
    prefix = MaskCurr.prefix;
    thousands = MaskCurr.thousands;
    decimal = MaskCurr.decimal

    var MyInput = $(Input).clone();

    MyInput = $(MyInput).attr("name", $(this).attr("name") + "Mask");
    MyInput = $(MyInput).attr("Id", $(this).attr("Id") + "Mask");

    var MainId = $(Input).attr("Id");
    if (MainId.endsWith("Mask")) {
        $(Input).maskMoney('destroy')
        $(Input).maskMoney({ prefix: '', suffix: suffix, thousands: '' + thousands + '', decimal: '' + decimal + '', precision: parseInt(MaskCurr.decimalNumber) });
        $(Input).addClass("maskmoney");
    } else {
        $(Input).hide();
        $(Input).addClass("NoPrint");
        $(MyInput).on("focusout change keyup", function () {
            if (!$(this).hasClass("NotValid")) {
                if (!isNaN($(this).val())) {
                    $(document).find("#" + MainId).val($(this).val());
                    $(document).find("#" + MainId).trigger("change");
                } else {
                    var num = $(this).maskMoney('unmasked')[0];
                    $(this).addClass("maskmoney")
                    $(document).find("#" + MainId).val(num);
                    $(document).find("#" + MainId).trigger("change");
                }
            }
        });
    }
    $(Input).after(MyInput)
    $(MyInput).maskMoney({ prefix: '', suffix: suffix, thousands: '' + thousands + '', decimal: '' + decimal + '', precision: parseInt(MaskCurr.decimalNumber) });
}
function getParameterByName(name, url) {
    if (!url) url = window.location.href;
    name = name.replace(/[\[\]]/g, '\\$&');
    var regex = new RegExp('[?&]' + name + '(=([^&#]*)|&|#|$)'),
        results = regex.exec(url);
    if (!results) return null;
    if (!results[2]) return '';
    return decodeURIComponent(results[2].replace(/\+/g, ' '));
}
function objectifyForm(formArray) {//serialize data function

    var returnArray = {};
    for (var i = 0; i < formArray.length; i++) {
        if (returnArray[formArray[i]['name']] != "") {
            returnArray[formArray[i]['name']] = formArray[i]['value'];
        }
    }
    return returnArray;
}
function QueryStringSerialize(Array) {
    var ObJs = "";
    $.each(Array, function (k, i) {
        var key = Object.keys(i);
        var value = Object.values(i);
        ObJs += key + "=" + value;
        ObJs += "&";


    })
    return ObJs;
}
function toObject(arr) {
    var rv = {};
    for (var i = 0; i < arr.length; ++i)
        rv[i] = arr[i];
    return rv;
}
Date.isLeapYear = function (year) {
    return (((year % 4 === 0) && (year % 100 !== 0)) || (year % 400 === 0));
};

Date.getDaysInMonth = function (year, month) {
    return [31, (Date.isLeapYear(year) ? 29 : 28), 31, 30, 31, 30, 31, 31, 30, 31, 30, 31][month];
};

Date.prototype.isLeapYear = function () {
    return Date.isLeapYear(this.getFullYear());
};

Date.prototype.getDaysInMonth = function () {
    return Date.getDaysInMonth(this.getFullYear(), this.getMonth());
};

Date.prototype.addMonths = function (value) {
    var n = this.getDate();
    this.setDate(1);
    this.setMonth(this.getMonth() + value);
    this.setDate(Math.min(n, this.getDaysInMonth()));
    return this;
};
function GetTwoDigit(value) {
    return ("0" + value).slice(-2)
}
function GetDigit(value, Number) {
    return ("0" + value).slice(-Number)
}
function RoundNumber(Number, FloatPoint) {
    try {
        if (!FloatPoint) {
            FloatPoint = GetSystemDPoints();
        }
        Number = parseFloat(Number);
        if (isNaN(Number)) {
            Number = 0;
        }
        return parseFloat(Number.toFixed(FloatPoint));
    } catch (err) {
        return parseFloat(Number);
    }
}
function CalcRate(Number, Multi, Divid) {
    return parseFloat(Number) * parseFloat(Multi) / parseFloat(Divid);
}
var DecimalNumber;

function GetSystemDPoints() {
    return DecimalPoint;
}
function NoNegitiveSubtract(First, Second) {
    if (First < Second)
        return RoundNumber(Second - First);
    else
        return RoundNumber(First - Second);
}
function GetJvTransactionEdit(PostingNumber, companyID, CallBack = null, Merge = false, Empty = false, RemoveBtns = false) {
    GetFiles(PostingNumber)
    $.ajax({
        url: "/api/TransactionApi/GetTransactionData?postingNumber=" + PostingNumber,
        method: "GET",
        success: function (data) {

            $("#TCGE-JEDate").val(data.ShowHeader.TransactionDate);
            $("#TCGE-PostingDate").val(data.ShowHeader.PostingDate);
            $("#TCGE-CurrencyID").val(data.ShowHeader.CurrencyID);

            if (data.ShowHeader.VoidJENum != null) {
                $('#ITDI-voidedDate').text(data.ShowHeader.VoidDate);
                $('#ITDI-voidedJENum').text(data.ShowHeader.VoidJENum + " ( " + data.ShowHeader.VoidPostingKey + " )");
                $('#ITDI-voidedTransactionInfo').show();
            } else {
                $('#ITDI-voidedTransactionInfo').hide();
            }

            if (data.ShowHeader.CurrencyID != companyID) {
                $('#TCCR-rateField').show();
                $(".TCGE-HSOAByC").show();

                var iso = data.ShowHeader.ISO;
                $("#IADI-HardGurrencyFormate").maskMoney({ suffix: ' ' + iso + '', thousands: ',', decimal: '.', precision: parseInt(requiredDecimalNum) });

                var hardCurrTest = true;
            } else {
                $('#TCCR-rateField').hide();
                $(".TCGE-HSOAByC").hide();
            }

            $("#TCGE-SystemRate").val(IADI_setSystemCurrFormate(+parseFloat(data.ShowHeader.SystemRate)));
            $("#TCGE-TransactionRate").val(IADI_setSystemCurrFormate(+parseFloat(data.ShowHeader.TransactionRate)));
            $("#TCGE-DiffrenceRate").val(parseFloat(IADI_setSystemCurrFormate(+parseFloat(data.ShowHeader.SystemRate - data.ShowHeader.TransactionRate))));

            $("#TCGE-Reference").val(data.ShowHeader.Reference);
            if (data.ShowTransactions.length <= 0) {
                data.ShowTransactions = data.ShowGeneralLedger;
            }
            if (Empty) {
                EmptyGlTransaction();
            }
            $.each(data.ShowTransactions, function (k, i) {
                AddToMainTbl(i.AccountID, i.AccountName, i.Document, i.AID,
                    i.Describtion, i.OriginalAmount, i.Debit, i.Credit, true);
            })
            // ManyJvAction(data, false, Merge);
            //**COmment If Error

            $("#collapseFirst").addClass("hide");
            $("#TCGE-JEDate").removeAttr("disabled");

            $("#TCGE-PostingDate").removeAttr("disabled");
            $("#TCGE-Reference").removeAttr("disabled");
            if (RemoveBtns) {
                $("#TCGE-GTbl").find("tr").find("button").not(".MoreDetailsT").remove();
            }
            if (CallBack != null) {
                CallBack();
            }
        }
    });
}
function GetJvTransaction(PostingNumber, companyID, CallBack = null) {
    GetFiles(PostingNumber)
    $.ajax({
        url: "/api/TransactionApi/GetTransactionData?postingNumber=" + PostingNumber,
        method: "GET",
        success: function (data) {

            $("#TCGE-JEDate").val(data.ShowHeader.TransactionDate);
            $("#TCGE-PostingDate").val(data.ShowHeader.PostingDate);
            $("#TCGE-CurrencyID").val(data.ShowHeader.CurrencyID);

            if (data.ShowHeader.VoidJENum != null) {
                $('#ITDI-voidedDate').text(data.ShowHeader.VoidDate);
                $('#ITDI-voidedJENum').text(data.ShowHeader.VoidJENum + " ( " + data.ShowHeader.VoidPostingKey + " )");
                $('#ITDI-voidedTransactionInfo').show();
            } else {
                $('#ITDI-voidedTransactionInfo').hide();
            }

            if (data.ShowHeader.CurrencyID != companyID) {
                $('#TCCR-rateField').show();
                $(".TCGE-HSOAByC").show();

                var iso = data.ShowHeader.ISO;
                $("#IADI-HardGurrencyFormate").maskMoney({ suffix: ' ' + iso + '', thousands: ',', decimal: '.', precision: parseInt(requiredDecimalNum) });

                var hardCurrTest = true;
            } else {
                $('#TCCR-rateField').hide();
                $(".TCGE-HSOAByC").hide();
            }

            $("#TCGE-SystemRate").val(IADI_setSystemCurrFormate(+parseFloat(data.ShowHeader.SystemRate)));
            $("#TCGE-TransactionRate").val(IADI_setSystemCurrFormate(+parseFloat(data.ShowHeader.TransactionRate)));
            $("#TCGE-DiffrenceRate").val(parseFloat(IADI_setSystemCurrFormate(+parseFloat(data.ShowHeader.SystemRate - data.ShowHeader.TransactionRate))));

            $("#TCGE-Reference").val(data.ShowHeader.Reference);

            for (let i = 0; i < data.ShowTransactions.length; i++) {

                var debit = data.ShowTransactions[i].Debit;
                if (data.ShowTransactions[i].Debit === 0) {
                    debit = "";
                }

                var credit = data.ShowTransactions[i].Credit;
                if (data.ShowTransactions[i].Credit === 0) {
                    credit = "";
                }
                RetrieveToMainTbl(data.ShowTransactions[i].AID, data.ShowTransactions[i].AccountName, data.ShowTransactions[i].Document, data.ShowTransactions[i].AccountID, data.ShowTransactions[i].Describtion, data.ShowTransactions[i].OriginalAmount, debit, credit, hardCurrTest, true);
            }
            //**COmment If Error
            for (let i = 0; i < data.ShowGeneralLedger.length; i++) {

                let debit = data.ShowGeneralLedger[i].Debit;
                if (data.ShowGeneralLedger[i].Debit === 0) {
                    debit = "";
                }

                let credit = data.ShowGeneralLedger[i].Credit;
                if (data.ShowGeneralLedger[i].Credit === 0) {
                    credit = "";
                }
                IADI_RetrieveToMainTbl(data.ShowGeneralLedger[i].AID, data.ShowGeneralLedger[i].AccountName, data.ShowGeneralLedger[i].Document, data.ShowGeneralLedger[i].AccountID, data.ShowGeneralLedger[i].Describtion, data.ShowGeneralLedger[i].OriginalAmount, debit, credit, hardCurrTest);
            }

            for (let i = 0; i < data.ShowAnalytics.length; i++) {
                let debit = data.ShowAnalytics[i].Debit;
                if (data.ShowAnalytics[i].Debit === null) {
                    debit = "";
                }

                let credit = data.ShowAnalytics[i].Credit;
                if (data.ShowAnalytics[i].Credit === null) {
                    credit = "";
                }

                IADI_RetrieveToDBAnalyticTbl(data.ShowAnalytics[i].AnalyticID, data.ShowAnalytics[i].DistID, data.ShowAnalytics[i].DistributionID, data.ShowAnalytics[i].DistributionName, data.ShowAnalytics[i].AID, data.ShowAnalytics[i].Describtion, data.ShowAnalytics[i].Percentage, data.ShowAnalytics[i].Amount, debit, credit);
            }

            for (let i = 0; i < data.ShowCostCenters.length; i++) {
                let debit = data.ShowCostCenters[i].Debit;
                if (data.ShowCostCenters[i].Debit === null) {
                    debit = "";
                }

                let credit = data.ShowCostCenters[i].Credit;
                if (data.ShowCostCenters[i].Credit === null) {
                    credit = "";
                }

                var mainCostCenterID = data.ShowCostCenters[i].MainCostCenterID;
                var costCenterType = 'MainCostCenter';
                if (data.ShowCostCenters[i].MainCostCenterID === null) {
                    mainCostCenterID = "";
                    costCenterType = 'CostCenter';
                }

                var costCenterIDPercentage = data.ShowCostCenters[i].CostCenterIDPercentage;
                if (data.ShowCostCenters[i].MainCostCenterID === null) {
                    costCenterIDPercentage = "";
                }

                IADI_RetrieveToDBCostCenterTbl(data.ShowCostCenters[i].CostCenterID, data.ShowCostCenters[i].CAID, data.ShowCostCenters[i].CostAccountID, data.ShowCostCenters[i].CostAccountName, data.ShowCostCenters[i].AID, data.ShowCostCenters[i].Describtion, data.ShowCostCenters[i].Percentage, data.ShowCostCenters[i].Amount, debit, credit, costCenterType, mainCostCenterID, costCenterIDPercentage, data.ShowCostCenters[i].CostCenterName);
            }
            //**COmment If Error

            $("#collapseFirst").addClass("hide");
            $("#TCGE-JEDate").removeAttr("disabled");

            $("#TCGE-PostingDate").removeAttr("disabled");
            $("#TCGE-Reference").removeAttr("disabled");
            if (CallBack != null) {
                CallBack();
            }
        }
    });
}
function GetJvTransactionVoid(PostingNumber, companyID) {
    GetFiles(PostingNumber)
    $.ajax({
        url: "/api/TransactionApi/GetTransactionData?postingNumber=" + PostingNumber,
        method: "GET",
        success: function (data) {

            $("#TCGE-JEDate").val(data.ShowHeader.TransactionDate);
            $("#TCGE-PostingDate").val(data.ShowHeader.PostingDate);
            $("#TCGE-CurrencyID").val(data.ShowHeader.CurrencyID);

            if (data.ShowHeader.VoidJENum != null) {
                $('#ITDI-voidedDate').text(data.ShowHeader.VoidDate);
                $('#ITDI-voidedJENum').text(data.ShowHeader.VoidJENum + " ( " + data.ShowHeader.VoidPostingKey + " )");
                $('#ITDI-voidedTransactionInfo').show();
            } else {
                $('#ITDI-voidedTransactionInfo').hide();
            }
            if (!companyID) {
                companyID = $('#TCGE-CompanyID').text();
            }
            if (data.ShowHeader.CurrencyID != companyID) {
                $('#TCCR-rateField').show();
                $(".TCGE-HSOAByC").show();

                var iso = data.ShowHeader.ISO;
                $("#IADI-HardGurrencyFormate").maskMoney({ suffix: ' ' + iso + '', thousands: ',', decimal: '.', precision: parseInt(requiredDecimalNum) });

                var hardCurrTest = true;
            } else {
                $('#TCCR-rateField').hide();
                $(".TCGE-HSOAByC").hide();
            }

            $("#TCGE-SystemRate").val(IADI_setSystemCurrFormate(+parseFloat(data.ShowHeader.SystemRate)));
            $("#TCGE-TransactionRate").val(IADI_setSystemCurrFormate(+parseFloat(data.ShowHeader.TransactionRate)));
            $("#TCGE-DiffrenceRate").val(parseFloat(IADI_setSystemCurrFormate(+parseFloat(data.ShowHeader.SystemRate - data.ShowHeader.TransactionRate))));

            $("#TCGE-Reference").val(data.ShowHeader.Reference);

            for (let i = 0; i < data.ShowTransactions.length; i++) {

                var debit = data.ShowTransactions[i].Debit;
                if (data.ShowTransactions[i].Debit === 0) {
                    debit = "";
                }

                var credit = data.ShowTransactions[i].Credit;
                if (data.ShowTransactions[i].Credit === 0) {
                    credit = "";
                }
                RetrieveToMainTbl(data.ShowTransactions[i].AID, data.ShowTransactions[i].AccountName, data.ShowTransactions[i].Document, data.ShowTransactions[i].AccountID, data.ShowTransactions[i].Describtion, data.ShowTransactions[i].OriginalAmount, credit, debit, hardCurrTest, true);
            }

            for (let i = 0; i < data.ShowGeneralLedger.length; i++) {

                let debit = data.ShowGeneralLedger[i].Debit;
                if (data.ShowGeneralLedger[i].Debit === 0) {
                    debit = "";
                }

                let credit = data.ShowGeneralLedger[i].Credit;
                if (data.ShowGeneralLedger[i].Credit === 0) {
                    credit = "";
                }
                IADI_RetrieveToMainTbl(data.ShowGeneralLedger[i].AID, data.ShowGeneralLedger[i].AccountName, data.ShowGeneralLedger[i].Document, data.ShowGeneralLedger[i].AccountID, data.ShowGeneralLedger[i].Describtion, data.ShowGeneralLedger[i].OriginalAmount, credit, debit, hardCurrTest);
            }

            for (let i = 0; i < data.ShowAnalytics.length; i++) {
                let debit = data.ShowAnalytics[i].Debit;
                if (data.ShowAnalytics[i].Debit === null) {
                    debit = "";
                }

                let credit = data.ShowAnalytics[i].Credit;
                if (data.ShowAnalytics[i].Credit === null) {
                    credit = "";
                }

                IADI_RetrieveToDBAnalyticTbl(data.ShowAnalytics[i].AnalyticID, data.ShowAnalytics[i].DistID, data.ShowAnalytics[i].DistributionID, data.ShowAnalytics[i].DistributionName, data.ShowAnalytics[i].AID, data.ShowAnalytics[i].Describtion, data.ShowAnalytics[i].Percentage, data.ShowAnalytics[i].Amount, credit, debit);
            }

            for (let i = 0; i < data.ShowCostCenters.length; i++) {
                let debit = data.ShowCostCenters[i].Debit;
                if (data.ShowCostCenters[i].Debit === null) {
                    debit = "";
                }

                let credit = data.ShowCostCenters[i].Credit;
                if (data.ShowCostCenters[i].Credit === null) {
                    credit = "";
                }

                var mainCostCenterID = data.ShowCostCenters[i].MainCostCenterID;
                var costCenterType = 'MainCostCenter';
                if (data.ShowCostCenters[i].MainCostCenterID === null) {
                    mainCostCenterID = "";
                    costCenterType = 'CostCenter';
                }

                var costCenterIDPercentage = data.ShowCostCenters[i].CostCenterIDPercentage;
                if (data.ShowCostCenters[i].MainCostCenterID === null) {
                    costCenterIDPercentage = "";
                }

                IADI_RetrieveToDBCostCenterTbl(data.ShowCostCenters[i].CostCenterID, data.ShowCostCenters[i].CAID, data.ShowCostCenters[i].CostAccountID, data.ShowCostCenters[i].CostAccountName, data.ShowCostCenters[i].AID, data.ShowCostCenters[i].Describtion, data.ShowCostCenters[i].Percentage, data.ShowCostCenters[i].Amount, credit, debit, costCenterType, mainCostCenterID, costCenterIDPercentage, data.ShowCostCenters[i].CostCenterName);
            }
            $("#collapseFirst").addClass("hide");
            $("#TCGE-JEDate").removeAttr("disabled");

            $("#TCGE-PostingDate").removeAttr("disabled");
            $("#TCGE-Reference").removeAttr("disabled");
        }
    });
}
function TransactionRateStatus(Link) {
    $.ajax({
        url: "/Bus/TransactionStatus?P=" + Link,
        method: "POST",
        success: function (data) {
            if (data) {
                $("#TCGE-TransactionRate").removeAttr("disabled")
            } else {
                $("#TCGE-TransactionRate").attr("disabled", "disabled")
            }
        }
    })
}
function IADI_setSystemCurrFormate(val) {
    if (val === 0) {
        return 0;
    } else {

        var fixedVal = parseFloat(val).toFixed(parseInt(requiredDecimalNum));
        if ($(document).find('#IADI-GurrencyFormate').length == 0) {
            $("body").append("<input id='IADI-GurrencyFormate' class='hide'>");
        }
        $(document).find('#IADI-GurrencyFormate').maskMoney('mask', parseFloat(fixedVal));

        return $(document).find('#IADI-GurrencyFormate').val();
    }
}
function IADI_RetrieveToMainTbl(accountID, accountName, document, accountIDTbl, describtion, originalAmount, debit, credit, hardCurrTest) {

    if (hardCurrTest) {
        originalAmount = IADI_setHardCurrFormate(+originalAmount);
    } else {
        originalAmount = IADI_setSystemCurrFormate(+originalAmount);
    }

    var content = "<tr class='row_" + accountID + "'>" +
        "<td>" + '<a href="#" class="mr-1 MoreDetailsT"><span class="fa fa-eye"></span></a>' + '<a href="#" class="mr-1" onclick="IADI_ShowAnalyticOfTrans(\'' + accountID + '\');"><span class="fa fa-list-ul">Analytic</span></a>' + '<a href="#" onclick="IADI_ShowCostOfTrans(\'' + accountID + '\');"><span class="fa fa-list-ul">Cost</span></a>' + "</td>" +
        "<td class='hide-normal TCGE-TblAccID'>" + accountID + "</td>" +
        "<td>" + accountName + "</td>" +
        "<td>" + document + "</td>" +
        "<td class='hide-normal'>" + accountIDTbl + "</td>" +
        "<td class='hide-normal'>" + describtion + "</td>" +
        "<td>" + originalAmount + "</td>" +
        "<td class='sDebitTbl'>" + IADI_setSystemCurrFormate(+debit) + "</td>" +
        "<td class='sCreditTbl'>" + IADI_setSystemCurrFormate(+credit) + "</td>" +
        "</tr>";

    $("#TCGE-TTbl").append(content);


    var sumDebit = 0;
    var sumCredit = 0;
    // iterate through each td based on class and add the values
    $(".sDebitTbl").each(function () {

        var value = parseFloat($(this).text().replace(regRemoveCurrFormate, ""));
        // add only if the value is number
        if (!isNaN(value) && value.length != 0) {
            sumDebit += value;
        }
    });

    $(".sCreditTbl").each(function () {

        var value = parseFloat($(this).text().replace(regRemoveCurrFormate, ""));
        // add only if the value is number
        if (!isNaN(value) && value.length != 0) {
            sumCredit += value;
        }
    });


    $("#DebitTblFoot").text(IADI_setSystemCurrFormate(+sumDebit));
    $("#CreditTblFoot").text(IADI_setSystemCurrFormate(+sumCredit));
    $("#DiffOfDAC").text(IADI_setSystemCurrFormate(parseFloat(sumDebit - sumCredit)));
}
function groupBy(list, keyGetter) {
    const map = new Map();
    list.forEach((item) => {
        const key = keyGetter(item);
        const collection = map.get(key);
        if (!collection) {
            map.set(key, [item]);
        } else {
            collection.push(item);
        }
    });
    return map;
}
function updateQueryStringParameter(uri, key, value) {
    var re = new RegExp("([?&])" + key + "=.*?(&|$)", "i");
    var separator = uri.indexOf('?') !== -1 ? "&" : "?";
    if (uri.match(re)) {
        return uri.replace(re, '$1' + key + "=" + value + '$2');
    }
    else {
        return uri + separator + key + "=" + value;
    }
}
function NotValid(input, Msg, Foucs = true,Empty=false) {
    $(document).find(input).addClass("NotValid");//.css("border-color", "red")
    var asd = $(document).find(input).prop("tagName");
    if ($(document).find(input).prop("tagName").toString().toLowerCase() == "select".toString().toLowerCase()) {
        $(document).find(input).nextAll("button").addClass("NotValid");
    }
    if ($(document).find(input).next("span").length == 0) {
        $(document).find(input).after("<span class='text-danger " + $(input).attr("id")+"VM'></span>");
    }
    if ($(document).find(input).attr("type") == "radio") {
        $(document).find(input).next("span").text("Requierd");
    }
    $(document).find(input).next("span").text(Msg);
    if (Foucs &&
        $(document).find(input).prop("tagName").toString().toLowerCase() != "select".toString().toLowerCase()
        ) {
        $(document).find(input).focus();
    }
    if (Empty) {
        $(input).val("");
    }
    $(document).find(input).change(function () {
        //  $(this).css("border-color", "#fff")
        $(this).next("span").text("");
        $(this).removeClass("NotValid")
        if ($(this).prop("tagName").toString().toLowerCase() == "select".toString().toLowerCase()) {
            $(document).find(input).nextAll("button").removeClass("NotValid");
        }
    })
}
function Valid(input) {
    $(document).find(input).removeClass("NotValid");
    $(document).find(input).nextAll("span").remove();
}
function openWindowWithPost(url, data) {
    var form = document.createElement("form");
    form.target = "_blank";
    form.method = "POST";
    form.action = url;
    form.style.display = "none";

    for (var key in data) {
        var input = document.createElement("input");
        input.type = "hidden";
        input.name = key;
        input.value = data[key];
        form.appendChild(input);
    }

    document.body.appendChild(form);
    form.submit();
    document.body.removeChild(form);
}
function openWindowWithPostArray(url, AppenHtml) {
    var form = document.createElement("form");
    form.target = "_blank";
    form.method = "POST";
    form.action = url;
    form.style.display = "none";


    $(form).append(AppenHtml);

    document.body.appendChild(form);
    form.submit();
    document.body.removeChild(form);
}
function PrintInt(Link) {
    setTimeout(function () {
        if ($.active <= 0) {
            window.open(Link);
        } else {
            PrintInt(Link)
        }
    }, 500)
}

function GetMaskNumber(Text,DefaultValue=null) {
    if (!Text) {
        if (DefaultValue != null) {
            return DefaultValue;
        } else {
             Text = 0;
        }
    }
    try {
        var Value = RoundNumber(Text.replace(/[^\d.-]/g, ""));
        if (Value == 0) {
            if (DefaultValue != null) {
                return DefaultValue;
            } else {
                return Value;
            }
        } else {
            return Value;
        }
    } catch (err) {
        return parseFloat(Text);
    }
}
function RemoveFromArray(arr, value) {
    return arr.filter(function (ele) { return ele != value; });
}
function RemoveFromObjArray(arr, prop, value) {
    return arr.filter(function (ele) { return ele[prop] != value; });
}
function LastIndexOfArray(array, n) {
    if (array == null)
        return void 0;
    if (n == null)
        return array[array.length - 1];
    return array.slice(Math.max(array.length - n, 0));
};
function MergJv(Inv = false) {
    var RemovedArr = [];
    $("#TCGE-TTbl").find("tr").each(function () {
        var ThisTr = $(this);
        if ($(document).find(ThisTr).length > 0) {

            var Text = $("#TCGE-TTbl").find("tr").find(".TCGE-TblAccID:contains(" + $(ThisTr).find(".TCGE-TblAccID").text() + ")").text();

            $("#TCGE-TTbl").find("tr").find(".TCGE-TblAccID:contains(" + $(ThisTr).find(".TCGE-TblAccID").text() + ")")
                .parent("tr").not(ThisTr).each(function () {
                    if ($(this).find("td:eq(3)").text() == $(ThisTr).find("td:eq(3)").text()
                        && GetMaskNumber($(ThisTr).find(".sDebitTbl").text()) == 0 &&
                        GetMaskNumber($(this).find(".sDebitTbl").text()) == 0
                        && $(this).find(".TCGE-TblAccID").text() == $(ThisTr).find(".TCGE-TblAccID").text()) {

                        var ThisOrginal = GetMaskNumber($(this).find("td:eq(6)").text());
                        var FirstOrginal = GetMaskNumber($(ThisTr).find("td:eq(6)").text());
                        $(ThisTr).find("td:eq(6)").text(setCurrecnyCurrFormate(ThisOrginal + FirstOrginal))
                        $(ThisTr).find(".sCreditTbl").text(setCurrecnyCurrFormate(ThisOrginal + FirstOrginal))
                        RemovedArr.push($(this).attr("class"));
                        $(this).remove();

                    } else if ($(this).find("td:eq(3)").text() == $(ThisTr).find("td:eq(3)").text()
                        && GetMaskNumber($(ThisTr).find(".sCreditTbl").text()) == 0
                        && GetMaskNumber($(this).find(".sCreditTbl").text()) == 0
                        && $(this).find(".TCGE-TblAccID").text() == $(ThisTr).find(".TCGE-TblAccID").text()) {
                        var ThisOrginal = GetMaskNumber($(this).find("td:eq(6)").text());
                        var FirstOrginal = GetMaskNumber($(ThisTr).find("td:eq(6)").text());
                        $(ThisTr).find("td:eq(6)").text(setCurrecnyCurrFormate(ThisOrginal + FirstOrginal))
                        $(ThisTr).find(".sDebitTbl").text(setCurrecnyCurrFormate(ThisOrginal + FirstOrginal))
                        RemovedArr.push($(this).attr("class"));
                        $(this).remove();
                    }
                })
        }
    });
    SumDebitAndCredit();
    var Counter = 1;



    if (Inv) {
        $("#TCGE-TTbl").find("tr").each(function () {
            var ThisClass = $(this).attr("class");
            var NewClass = "row_" + Counter;
            $("#itemsBody").find("td[data-jvindex='" + ThisClass + "']").each(function () {
                $(this).attr("data-jvindex", NewClass);
            });
            $(this).attr("class", "");
            $(this).addClass(NewClass);
            $(this).find("#dmt").attr("onclick", "DeleteT(" + Counter + ");")
            $(this).find("#emt").attr("onclick", "EditT(" + Counter + ");")
            Counter++;
            RemovedArr = removebyvalue(RemovedArr, NewClass);
        })
        //Counter = 1;
        //$.each(RemovedArr, function (k, i) {
        //    $("#itemsBody").find("td[data-jvindex='" + i + "']").each(function () {
        //        var Test = GetGlRowClassByAccId($(this).attr("data-tax-accid"));
        //        $(this).attr("data-jvindex", GetGlRowClassByAccId($(this).attr("data-tax-accid")) );
        //    });
        //})
    } else {
        $("#TCGE-TTbl").find("tr").each(function () {
            var ThisClass = $(this).attr("class");
            var NewClass = "row_" + Counter;
            $(this).attr("class", "");
            $(this).addClass(NewClass);
            $(this).find("#dmt").attr("onclick", "DeleteT(" + Counter + ");")
            $(this).find("#emt").attr("onclick", "EditT(" + Counter + ");")
            Counter++;
        })
    }

}
function GetGlAccIndex(AccId) {
    return $("#TCGE-TTbl").find(".TCGE-TblAccID:textEquals(" + AccId + ")").parent("tr").index();
}
function GetGlAccIndexRClass(Row) {
    try {
        if (Row != "") {
            return $("#TCGE-TTbl").find("." + Row).index();
        } else {
            return "";
        }
    } catch (err) {
        return "";
    }

}
function GetGlRowClassByAccId(AccId, IsDebit = -1) {
    if (IsDebit == true) {
        var ThisTr;
        $("#TCGE-TTbl").find(".TCGE-TblAccID:contains(" + AccId + ")").each(function () {
            if (GetMaskNumber($(this).parent("tr").find(".sDebitTbl").text()) > 0) {
                ThisTr = $(this).parent("tr").attr("class");
            }
        })
        return ThisTr;
    } else if (IsDebit == false) {
        var ThisTr;
        $("#TCGE-TTbl").find(".TCGE-TblAccID:contains(" + AccId + ")").each(function () {
            if (GetMaskNumber($(this).parent("tr").find(".sCreditTbl").text()) > 0) {
                ThisTr = $(this).parent("tr").attr("class");
            }
        })
        return ThisTr;
    } else {
        return $("#TCGE-TTbl").find(".TCGE-TblAccID:contains(" + AccId + ")").parent("tr").attr("class");
    }
}
function Interval(Func, TimeOut = 10000) {
    setInterval(function () {
        Func();
    }, TimeOut)
}
function Redirect(Link) {
    window.location.href = Link;
}
function RedirectInt(Link,TimeOut=1000) {
    RunAfterAjax(function () {
        if (window.location.toString().startsWith("FixedAssets")) {
            Link = "/FixedAssets" + Link;
        } else {
            window.location.href = Link
        }
    }, TimeOut);
}
function IntRedirctToHome() {
    Redirect("/Home/Financial_Home")
    Interval(function () {
        Redirect("/Home/Financial_Home")
    }, 5000);
}
function setSystemCurrFormate(val) {
    if (val === 0) {
        return 0;
    } else {
        if ($('#TCGE-GurrencyFormate').length == 0) {
            $("body").append("<input id='TCGE-GurrencyFormate' class='hide' />")
        }
        return CaclSetSystCF(val);
    }
}
function CaclSetSystCF(val) {
    var fixedVal = parseFloat(val).toFixed(parseInt(requiredDecimalNum));

    $(document).find('#TCGE-GurrencyFormate').maskMoney('mask', parseFloat(fixedVal));

    $(document).find('#TCGE-GurrencyFormate')
        .maskMoney({
            prefix: '' + MaskCurr.prefix + '',
            suffix: ' ' + MainIso + '',
            thousands: '' + MaskCurr.thousands + '',
            decimal: '' + MaskCurr.decimal + '',
            precision: parseInt(MaskCurr.decimalNumber)
        });
    return $(document).find('#TCGE-GurrencyFormate').val();
}
function setHardCurrFormate(val) {
    if (val === 0) {
        return 0;
    } else {
        var Currency = $("#TCGE-CurrencyID").find("option:selected").text();
        if (!Currency) {
            Currency = $("#Currency_id").find("option:selected").text()
        }
        if (!Currency) {
            Currency = $("#Currency_idTrans").find("option:selected").text()
        }
        if ($('#TCGE-HardGurrencyFormate').length == 0) {
            $("body").append("<input id='TCGE-HardGurrencyFormate' class='hide' />")
        }
        var fixedVal = parseFloat(val).toFixed(parseInt(requiredDecimalNum));
        console.log("Currency " + Currency)
        console.log("fixedVal " + fixedVal)
        $(document).find('#TCGE-HardGurrencyFormate').val(fixedVal)
        $(document).find('#TCGE-HardGurrencyFormate').maskMoney({
            prefix: '' + MaskCurr.prefix + '',
            suffix: ' ' + Currency + '',
            thousands: '' + MaskCurr.thousands + '',
            decimal: '' + MaskCurr.decimal + '',
            precision: parseInt(MaskCurr.decimalNumber)
        });

        return $(document).find('#TCGE-HardGurrencyFormate').val();
    }
}
function setCurrecnyCurrFormate(val, CurrencyId = null) {
    if (CurrencyId == null) {
        CurrencyId = $('#TCGE-CurrencyID').val();
    }
    return CurrencyFormate(val, CurrencyId);
}
function CurrencyFormate(val, CurrencyId) {
    var companyID = CompId;
    if (companyID == CurrencyId || CurrencyId == "") {
        return setSystemCurrFormate(val);
    } else {
        return setHardCurrFormate(val);
    }
}

function removebyvalue(arr, value) { return arr.filter(function (ele) { return ele != value; }); }
function SetDC(Tr, Amount) {
    if (Tr != -1) {
        Amount = RoundNumber(Amount);
        $("#TCGE-TTbl").find("tr:eq(" + Tr + ")").find("td:eq(6)").text(setCurrecnyCurrFormate(Amount))
        if (GetMaskNumber($("#TCGE-TTbl").find("tr:eq(" + Tr + ")").find(".sDebitTbl").text()) > 0) {
            $("#TCGE-TTbl").find("tr:eq(" + Tr + ")").find(".sDebitTbl").text(setCurrecnyCurrFormate(Amount));
            $("#TCGE-TTbl").find("tr:eq(" + Tr + ")").find(".sCreditTbl").text(0);
        } else {
            $("#TCGE-TTbl").find("tr:eq(" + Tr + ")").find(".sCreditTbl").text(setCurrecnyCurrFormate(Amount));
            $("#TCGE-TTbl").find("tr:eq(" + Tr + ")").find(".sDebitTbl").text(0);
        }
        SumDebitAndCredit();
    }
}
function SetDCBool(Tr, Amount, IsDebit) {
    Amount = RoundNumber(Amount);
    $("#TCGE-TTbl").find("tr:eq(" + Tr + ")").find("td:eq(6)").text(setCurrecnyCurrFormate(Amount))

    if (IsDebit) {
        $("#TCGE-TTbl").find("tr:eq(" + Tr + ")").find(".sDebitTbl").text(setCurrecnyCurrFormate(Amount));
        $("#TCGE-TTbl").find("tr:eq(" + Tr + ")").find(".sCreditTbl").text(0);
    } else {
        $("#TCGE-TTbl").find("tr:eq(" + Tr + ")").find(".sCreditTbl").text(setCurrecnyCurrFormate(Amount));
        $("#TCGE-TTbl").find("tr:eq(" + Tr + ")").find(".sDebitTbl").text(0);
    }
    SumDebitAndCredit();
}
//Get Amount Of Jv Row
function GetDc(Tr) {
    if (GetMaskNumber($("#TCGE-TTbl").find("tr:eq(" + Tr + ")").find(".sDebitTbl").text()) > 0) {
        return GetMaskNumber($("#TCGE-TTbl").find("tr:eq(" + Tr + ")").find(".sDebitTbl").text());
    } else {
        return GetMaskNumber($("#TCGE-TTbl").find("tr:eq(" + Tr + ")").find(".sCreditTbl").text());
    }
}
function GetDcByRowClass(RowClass) {
    if (GetMaskNumber($("#TCGE-TTbl").find("tr." + RowClass + "").find(".sDebitTbl").text()) > 0) {
        return GetMaskNumber($("#TCGE-TTbl").find("tr." + RowClass + "").find(".sDebitTbl").text());
    } else {
        return GetMaskNumber($("#TCGE-TTbl").find("tr." + RowClass + "").find(".sCreditTbl").text());
    }
}

function GetLastDcRowClass() {
    return $("#TCGE-TTbl").find("tr").last().attr("class");
}
function GetLastDcACid() {
    return $("#TCGE-TTbl").find("tr").last().find(".TCGE-TblAccID").text();
}
function GetNextDCRowClass() {
    try {
        var ThisRowC = $("#TCGE-TTbl").find("tr").last().attr("class").split("_");
        var Num = parseInt(ThisRowC[1]);
        Num += 1;
        return "row_" + Num;
    } catch (err) {
        return "row_1";

    }
}
function GetDCLastIndexRowClass() {
    try {
        return $("#TCGE-TTbl").find("tr").last().attr("class")
    } catch (err) {
        return "row_1";

    }
}
function RmDc(AID, RmIfDebit = false, RmIfCredit = false) {
    try {
        if (AID != "") {
            var ThisRow = $("#TCGE-TTbl").find(".TCGE-TblAccID:contains(" + AID + ")")
                .parent("tr");
            if (RmIfDebit) {
                if (GetMaskNumber($(ThisRow).find(".sDebitTbl").text()) != 0) {
                    $(ThisRow).filter(function () {
                        return (GetMaskNumber($(this).find(".sDebitTbl").text()) != 0 && $(this).find(".TCGE-TblAccID").text() == AID)
                    }).remove();
                }
            } else if (RmIfCredit) {
                if (GetMaskNumber($(ThisRow).find(".sCreditTbl").text()) != 0) {
                    $(ThisRow).filter(function () {
                        return (GetMaskNumber($(this).find(".sCreditTbl").text()) != 0 && $(this).find(".TCGE-TblAccID").text() == AID)
                    }).remove();
                }
            } else {
                $(ThisRow).remove();
            }
            SumDebitAndCredit();
        }

    } catch (err) {

    }

}
function RmDcByRowClass(Row) {
    if (Row != "") {
        $("#TCGE-TTbl").find("." + Row).remove();
    }
}
function RmDcByRowClassAmount(Row, IsAcId, Amount = -1, IsDebit = -1, ForceRemove = false) {
    try {
        if (Row != "") {
            var Row;
            if (IsAcId) {
                Row = GetGlRowClassByAccId(Row, IsDebit);
            }
            if (Amount == -1) {
                Amount = GetDc(GetGlAccIndexRClass(Row))
            }
            SetDC(GetGlAccIndexRClass(Row), NoNegitiveSubtract(GetDc(GetGlAccIndexRClass(Row)), Amount));
            if (GetDc(GetGlAccIndexRClass(Row)) <= 0) {
                var ThisRowClass = parseInt(Row.split("_")[1]);
                if (ThisRowClass > 3) {
                    $("#TCGE-TTbl").find("." + Row).remove();
                }
            }
            if (ForceRemove) {
                $("#TCGE-TTbl").find("." + Row).remove();
            }
        }
    } catch (err) {

    }

}
function sleep(ms) {
    return new Promise(resolve => setTimeout(resolve, ms));
}

function DisableThis(Elm) {
    $(Elm).attr("disabled", "disabled");
}
function EnabledThis(Elm) {
    $(Elm).removeAttr("disabled");

}
function ValidateAll(CallBackIfValid = null, NotValidCallBack = null) {
    var CheckValid = true;
    $(document).find("input").each(function () {
        if ($(this).attr("required") == "required") {
            if ($(this).attr("type") == "radio") {
                if (!$(this).is(":checked")) {
                    var ThisName = $(this).attr("name");
                    var OneValid = false;
                    $(document).find("input[name='" + ThisName + "']").each(function () {
                        if ($(this).is(":checked")) {
                            OneValid = true;
                        }
                    })
                    if (!OneValid) {
                        NotValid($(this));
                        CheckValid = false;
                    }
                }
            } else {
                if (!$(this).val() || $(this).val() == "0") {
                    NotValid($(this));
                    CheckValid = false;
                }
            }

        }
    })
    $(document).find("select").each(function () {
        if ($(this).attr("required") == "required") {
            if (!$(this).val() || $(this).val() == "0") {
                NotValid($(this));
                CheckValid = false;
            }
        }
    })
    if (CallBackIfValid != null) {
        if (CheckValid) {
            CallBackIfValid();
        } else {
            if (NotValidCallBack != null) {
                NotValidCallBack();
            }
        }
    }
}
function GetMarkAccount(data, Mark) {
    try {
        var ThisAId = "";
        $.each(data, function (k, i) {
            try {
                ThisAId = i.ShowTransactions.find(x => x.Mark == Mark).AID
                if (ThisAId) {
                    return ThisAId;
                }
            } catch (err) {

            }
        })
        return ThisAId;
    } catch (err) {
        return "";
    }
}
function DisableAjax(Input) {
    $(document).ajaxStart(function () {
        $(document).find(Input).attr("disabled", "disabled");
        RunAfterAjax(function () {
            $(document).find(Input).removeAttr("disabled");
        })
    })

    //$(document).ajaxComplete(function () {
    //})
}
var SelectPickerCalled = false;
function RefreshSelcetPicker(Input) {
    $(document).find(Input).selectpicker('refresh');
    $.each(RemoveClass, function (k, i) {
        $(document).find("div." + i).removeClass(i);
    });
    $(document).ajaxComplete(function () {
            RunAfterAjax(function () {
                $(document).find(Input).selectpicker('refresh');
                $.each(RemoveClass, function (k, i) {
                    $(document).find("div." + i).removeClass(i);
                });
                try {
                    if (SelectPickerCallBack) {
                        SelectPickerCallBack();
                    }
                } catch (err) {

                }
                SelectPickerCalled = false;
            }, 1000)
    })


}
function ForceRefreshPicker() {
    $(document).find("select:visible").each(function () {
        if (!$(this).hasClass("NoSelect")) {
            $(document).find(this).selectpicker('refresh');
            $(this).on("loaded.bs.select", function () {
                $.each(RemoveClass, function (k, i) {
                    $(document).find("div." + i).removeClass(i);
                });
            })
          
        }
    })
    try {
        if (SelectPickerCallBack) {
            SelectPickerCallBack();
        }
    } catch (err) {

    }
}
function ForceRefreshThisPicker(Id) {
    if (Id.indexOf("#") < 0) {
        Id = "#" + Id;
    }
    $(document).find(Id).each(function () {
        if (!$(this).hasClass("NoSelect")) {
            $(document).find(this).selectpicker('refresh');
            $(this).on("loaded.bs.select", function () {
                $.each(RemoveClass, function (k, i) {
                    $(document).find("div." + i).removeClass(i);
                });
            })
        }
    })
    try {
        if (SelectPickerCallBack) {
            SelectPickerCallBack();
        }
    } catch (err) {

    }
}
function RemovePickerDiv() {
    $.each(RemoveClass, function (k, i) {
        $(document).find("div." + i).removeClass(i);
    });
}
function NextPrev(Elm, BtnClick) {
    $(".Next").click(function () {
        if ($(Elm).val()) {
            $(Elm).val($(Elm).find('option:selected').next().val()).trigger("change");
            $(BtnClick).trigger("click");
        } else {
            $(Elm).val($(Elm).find("option:eq(1)").val()).trigger("change")
            $(BtnClick).trigger("click");
        }
    })
    $(".Prev").click(function () {
        if ($(Elm).val()) {
            $(Elm).val($(Elm).find('option:selected').prev().val()).trigger("change");
            $(BtnClick).trigger("click");
        } else {
            $(Elm).val($(Elm).find("option:eq(1)").val()).trigger("change")
            $(BtnClick).trigger("click");
        }
    })
}
$(document).on("click", ".GetAnalyticDetails", function () {
    debit = GetMaskNumber($(this).parents("tr").find(".sDebitTbl").text())
    credit = GetMaskNumber($(this).parents("tr").find(".sCreditTbl").text())
    var CAID = $(this).parents("tr").find(".TCGE-TblAccID").text();
    var Tr = GetJvRow(CAID);
    //Function That Open analytic popup without refresh
    ////OpenPUAAWithOutRefrech(debit, credit);
    var CAID = $(this).parents("tr").find(".TCGE-TblAccID").text();
    var data = GetRowData(CAID);
    $("#TCGE-PUAAOriginalAmount").text(data.originalAmount);
    GetAnaylticData(debit, credit, data);
    RunAfterAjax(function () {
        CalcAnalyticAssUnAss();
    })
})
$(document).on("click", ".GetCostCenterDetails", function () {
    var Tr = $(this).parents("tr");

    var accountID = $(Tr).find(".TCGE-TblAccID").text(); //-
    var accountIDTbl = $(Tr).find(".accountIDTbl").text();
    var accountName = $(Tr).find(".accountName").text();
    var describtion = $(Tr).find(".describtion").text();
    var originalAmount = GetMaskNumber($(Tr).find(".OrA").text());
    var document = $(Tr).find(".document").text();
    //var costCenterID = $("#TCGE-PUCCCostID").text();
    //var mainCostCenter = $("#TCGE-PUCCMainCostID").text();
    var CostCenterType = $(Tr).attr("data-costcentertype");
    $("#TCGE-CostCenterType").text(CostCenterType)

    debit = GetMaskNumber($(Tr).find(".sDebitTbl").text())
    credit = GetMaskNumber($(Tr).find(".sCreditTbl").text())
    //Function That Open analytic popup without refresh
    $("#TCGE-PUCCFinalSave").attr("data-add", "add")

    SetDataInCCPopUp(debit, credit, accountIDTbl, accountName,
        CostCenterType, $(Tr).attr("data-centerid"), $(Tr).attr("data-centerid"),
        accountID, describtion);


    $("#TCGE-AccountID").val(accountID); //-
    $("#TCGE-AccountName").val(accountName);
    $("#TCGE-Describtion").val(describtion);
    $("#TCGE-OriginalAmount").val(MaskThisMoney(originalAmount));
    $("#TCGE-Debit").val(debit)
    $("#TCGE-Credit").val(credit)
    $("#TCGE-SUD").val(document);

    // analyticID = $("#TCGE-PUAAAnaID").text();

    $("#TCGE-PUCCCostID").text($(Tr).attr("data-centerid"));
    $("#TCGE-PUCCMainCostID").text($(Tr).attr("data-centerid"));
    CostCenterTr = Tr;
})


$(document).on('hidden.bs.modal', "#TCGE-PUCostCenter", function () {
    SumDebitAndCredit();
    $("#TCGE-PUCCFinalSave").removeAttr("data-add")

})
function GetJvRow(AccId) {
    if ($("#TCGE-TTbl").find(".TCGE-TblAccID:textEquals(" + AccId + ")").parents("tr").length > 0) {
        return $("#TCGE-TTbl").find(".TCGE-TblAccID:textEquals(" + AccId + ")").parents("tr");
    }
    else {
        return null;
    }
}
function GetJvRowClassNumber(AccId) {
    try {
        return GetJvRowClassClass(AccId).split("_")[1];
    } catch (Error){
        return 1;
    }
}
function GetJvRowClassClass(AccId) {
    return $("#TCGE-TTbl").find(".TCGE-TblAccID:textEquals(" + AccId + ")").parents("tr").attr("class");
}
function GetRowData(AccId) {
    var Tr = GetJvRow(AccId);
  
    return {
        accountID: AccId,
        accountIDTbl :$(Tr).find(".accountIDTbl").text(),
        accountName : $(Tr).find(".accountName").text(),
        describtion : $(Tr).find(".describtion").text(),
        originalAmount :GetMaskNumber($(Tr).find(".OrA").text()),
        debit : GetMaskNumber($(Tr).find(".sDebitTbl").text()),
        credit : GetMaskNumber($(Tr).find(".sCreditTbl").text()),
        document : $(Tr).find(".describtion").text()
    }
}
//$(document).on("click", "a", function () {
//    if ($(this).attr("href") && $(this).attr("data-ajax")!="true") {
//        if ($(this).attr("href").indexOf("#") < 0) {
//            window.location.href = $(this).attr("href");
//        }
//    }
//})
function GetCheckBookTypeName(CType) {
    var type;
    if (CType == 'TCCR') {
        type = 'Cash Reciept';
    } else if (CType == 'TCCW') {
        type = 'Cash Withdraw';
    } else if (CType == 'TCBR') {
        type = 'Bank Check-Recieved'
    } else if (CType == 'TCBC') {
        type = 'Bank Check-out';
    } else if (CType == 'TCBT') {
        type = 'Checkbook Transfer';
    } else if (CType == 'TBR') {
        type = 'Bank Reconcile';
    } else if (CType == 'TCT') {
        type = 'Transfer Check To Bank';
    } else if (CType == 'TCVT' || CType == 'TCVC' || CType == 'TBVC') {
        type = 'Void';
    }
    return type;
}