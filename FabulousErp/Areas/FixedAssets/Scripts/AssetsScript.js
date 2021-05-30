$("#MyUseLife").on("keyup", function () {
    ChangeDays();
})
$("#Use_life").on("keyup", function () {
    var MyDays = date_diff_indays($("#Start_use").val(), $("#Use_life").val()) / 365
    $("#MyUseLife").val(Math.round(MyDays * 100) / 100);
    if ($("#MyUseLife").val() < 0) {
        $("#Use_life").val($("#Start_use").val());
        $("#Use_life").val($("#Start_use").val());
    }
    ChangeDays();
})
$("#Save").click(function () {
    $("#FirstForm").trigger("click")
})
$("#Start_use").change(function () {
    $("#Start_derecation_date").val($(this).val())
})
$(".Serial").click(function () {
    if ($(this).val() == 1) {
        $("#Serial_number").val(null);
        $(".SerialNumberDiv").hide()
    } else {
        $(".SerialNumberDiv").show()
    }
})
$("#CreateAssetsForm").click(function () {
    var BatchId = "";
    if ($("#ShowHideAssetsSec").attr("data-toggle") == "Hide") {
        $("#AssetsMainSection").find("form").validate();
        $("#AssetsTransactionForm").validate();
        CheckSerialNumber(function (data) {
            //$("#Serial_number").attr("placeholder", $("#Serial_number").val() + "  Already Exist");
            //$("#Serial_number").val("");
            Talert(data.msg)

        }, function () {
            $.ajax({
                url: "/FixedAssets/Assets/ValidateCreate",
                data: $("#AssetsForm").serialize(),
                method: "POST",
                success: function (data) {
                    if (data) {
                        if (isNaN(data)) {
                            Talert(JSON.stringify(data))
                        } else {
                            if ($("#AssetsMainSection").find("form").valid() && $("#AssetsTransactionForm").valid()) {
                                InsertTransactionData($("#TCGE-CompanyID").text(), $("#BostingToORThrow").val(), $("#TCGE-PostingDate").val(), $("#TCGE-JEDate").val(), $("#TCGE-Reference").val(), $("#TCGE-CurrencyID").val(), $("#TCGE-SystemRate").val(), $("#TCGE-TransactionRate").val(), "FIXDASSC", "FIXDASSC", BatchId, "",
                                    function (JournalEntryNumber) {
                                        $("#Gl_transaction_id").val(JournalEntryNumber)
                                        $("#FirstForm").trigger("click")
                                    }, null,null,null,true)
                            }
                            $("#Serial_number").removeAttr("placeholder");
                        }
                    } else {
                        Talert("Some Thing Went Wrong Please Try Again Later")
                    }

                }
            })
        })
    } else {
        if ($("#AssetsTransactionForm").valid()) {
            var Valid = true;
            if ($("#AssetsMainSection").find("form").length>0) {
                if (!$("#AssetsMainSection").find("form").valid()) {
                    Valid = false;

                }
            }
                if (Valid) {
                    InsertTransactionData($("#TCGE-CompanyID").text(), $("#BostingToORThrow").val(), $("#TCGE-PostingDate").val(), $("#TCGE-JEDate").val(), $("#TCGE-Reference").val(), $("#TCGE-CurrencyID").val(), $("#TCGE-SystemRate").val(), $("#TCGE-TransactionRate").val(), "FIXDASSC", "FIXDASSC", BatchId, "",
                        function (JournalEntryNumber) {
                            $("#Gl_transaction_id").val(JournalEntryNumber)
                            $("#FirstForm").trigger("click")
                        },null,null,null,true)
                }
            
        }
    }
})
$("#CreateOnlyAssets").click(function () {
    CheckSerialNumber(function (data) {
        //$("#Serial_number").attr("placeholder", $("#Serial_number").val() + "  Already Exist");
        //$("#Serial_number").val("");
        Talert(data.msg)

    }, function () {
        $("#SubmitAssetsForm").trigger("click");
    });
})
$("#Deprecation_method").change(function () {
    if ($(this).val() == 3) {
        $(".Number_of_unitsDiv").show();
    } else {
        $(".Number_of_unitsDiv").hide();
    }
    CalcFixedAssets();
})
$("#AcqCost_transaction,#MyUseLife,#Include_scerap_value,#Scrap_value").focusout(function () {
    CalcFixedAssets();
});
$("#Include_scerap_value").click(function () {
    CalcFixedAssets();
})
$("#Date_of_orgin").change(function () {
    $("#Start_use").val($(this).val())
})
$("#DOO_trainsaction").change(function () {
    $("#Date_of_orgin").val($(this).val())
})
$("#ShowHideAssetsSec").click(function () {
    $("#AssetsMainSection").toggle("ease");
    if ($(this).attr("data-toggle") == "Show") {
        $(this).attr("data-toggle","Hide") ;
    } else {
        $(this).attr("data-toggle", "Show");
    }
    $("#Deprecation_method").trigger("change");
    $("#Assets_main_id").trigger("change");
})
$("#TCGE-JEDate").focusout(function () {
    if (!$("#TCGE-PostingDate").val()) {
        $("#TCGE-PostingDate").val($("#TCGE-JEDate").val())
    }
    ValidatePeriod();

})
$("#Currency_id").change(function () {
    $("#TCGE-CurrencyID").val($("#NewAssetsTrnsSec").find("#Currency_id").val())
    $("#TCGE-CurrencyID").trigger("change")
})
$("#Assets_class_id_above,#Currency_id").change(function () {
    //if (!$("#DOO_trainsaction").val()) {
    //    Talert("You Should Choose Put Date Of Orgin First");
    //    $("#DOO_trainsaction").val("");
    //    $("#NewAssetsTrnsSec").find("#Currency_id").val("")
    //}
    ValidatePeriod(function () { $("#NewAssetsTrnsSec").find("#Currency_id").val("") });
})
$("#Reference").change(function () {
    $("#TCGE-Reference").val($(this).val())
})
$(document).on("change", "#Currency_id", function () {
    $("#TCGE-CurrencyID").val($(this).val())
})
//$("#Serial_number").focusout(function () {
//    CheckSerialNumber(function (data) {
//        $("#Serial_number").attr("placeholder", $("#Serial_number").val() + "  Already Exist");
//        $("#Serial_number").val("");
//        Talert(data.msg)

//    }, function () {
//        $("#Serial_number").removeAttr("placeholder");

//    });
//});
$("#Assets_number").focusout(function () {
    $.ajax({
        url: "/FixedAssets/Assets/CheckAssets_number?Assets_number=" + $("#Assets_number").val(),
        method: "POST",
        success: function (data) {
            if (data) {
                $("#Assets_number").attr("placeholder", $("#Assets_number").val() + "  Already Exist");
                $("#Assets_number").val("");
            }
        }
    })

})
$("#AcqCost_transaction").focusout(function () {
    $("#AssetsAcquisation_cost").val($(this).val())
})
String.prototype.replaceAll = function (search, replacement) {
    var target = this;
    return target.replace(new RegExp(search, 'g'), replacement);
};
$(document).on("change", "#Assets_class_id_below,#Assets_class_id_above", function () {
    $("#Assets_main_id").trigger("change")
})
$("#Assets_main_id").change(function () {
    var id = $(this).find("option:selected").val();
    if (id === undefined) {
        id = $(this).find("option").first().val();
    }
    if (id !== undefined) {
        $.ajax({
            url: "/Assets_main/GetNumberOfUnit",
            data: { id: $(this).find("option:selected").val() },
            method: "post",
            success: function (data) {
                $("#PartsSerialDiv").empty();
                if (data > 0) {
                    for (i = 0; i < data; i++) {
                        AddSerial(i, (i + 1).toString())
                    }
                } else {
                    $("#PartsSerialDiv").empty();

                    AddSerial(0, 1)
                }
            }
        })
    } else {
        $("#PartsSerialDiv").empty();

        AddSerial(0, 1)
    }
})
function AddSerial(Count,PartNumber) {
    var PartHtml = $("#AssetsPartSerial").text().toString();

    PartHtml = PartHtml.replaceAll("{{Count}}", Count)
        .replaceAll("{{Part_number}}", PartNumber)
    $("#PartsSerialDiv").append(PartHtml);
}
$(function () {
    $("#TCGE-BtnPUNewBatch").parents("label").first().remove();
    $("#TCGE-BatchID").parent("div").remove();
    //$(".DebitCreditSection").find("label").hide();
    //$(".DebitCreditSection").find("select").hide();
    //$(".DebitCreditSection").find("input").hide();
    //$(".DebitCreditSection").find("button").hide();
    $("#TCGE-SUD").hide();
    $("#TCGE-SUD").parent("div").prev("label").hide();
    $("#TCGE-CurrencyID").parents(".form-row").hide();
    $(".DebitCreditSection").show()
    $(".Serial[value='1']").trigger("click");
    $("#Deprecation_method").trigger("change");
    $("#TCGE-BatchAction").text("")

})

function CheckSerialNumber(SuccessFailCallBack, SuccessCallBack) {
    var Serial = [];

    $(document).find(".AssetsPartSerial").each(function () {
        Serial.push({
            Id: $(this).prevUntil(".AssetsPartId").val(),
            Serial: $(this).val()
        });
    })
    $.ajax({
        url: "/FixedAssets/Assets/CheckSerialNumber",
        data: { PartsSerial: Serial},
        method: "POST",
        async: false,
        success: function (data) {
            if (data.status==true) {
                if (SuccessFailCallBack != null) {
                    try {
                        SuccessFailCallBack(data);
                    } catch(err){
                        Talert(data)
                    }
                }
                return false
            } else {
                if (SuccessCallBack != null) {
                    SuccessCallBack(data)
                }
                return true

            }
        }
    })
}
function CountFeildNo() {
    var Count = 1;
    $("#AddtionalInfoDiv").find(".Field_no").each(function () {
        $(this).text(Count);

        Count++;
    })
    HideLoader();

}
function SubmitAssetsFormAdditnalInfo(Id) {
    if (!isNaN(Id)) {
        $("#AddtionalInfoDiv").find(".Assets_id").val(Id);
        $("#AddtionalInfoDiv").find("form").submit();
        window.location.reload();
        // RedirectInt("/Assets");
    }
}
function MyLoader() {
    Loader(function () {

    });
}
function HideIfNoneDepreication() {
    $("#Start_use").parents(".form-group").hide();
    $("#MyUseLife").parents(".form-group").hide();
    $("#Use_life").parents(".form-group").hide();
    $("#Start_derecation_date").parents(".form-group").hide();
    $(".MyTotalDays").parents(".form-group").hide();
    $("#Include_scerap_value").parents(".form-group").hide();
    $("#Deactive_depraction").parents(".form-group").hide();

}
function ShowIfNoneDepreication() {
    $("#Start_use").parents(".form-group").show();
    $("#MyUseLife").parents(".form-group").show();
    $("#Use_life").parents(".form-group").show();
    $("#Start_derecation_date").parents(".form-group").show();
    $(".MyTotalDays").parents(".form-group").show();
    $("#Include_scerap_value").parents(".form-group").show();
    $("#Deactive_depraction").parents(".form-group").show();
}
function CalcFixedAssets() {
    $(".DeprecationRateInputDiv").find("input").removeAttr("max");

    if ($("#Deprecation_method option:selected").val() == 1) {
        //  $("#Deprecation_rate").attr("disabled", "disabled")
        if (!$("#AcqCost_transactionTranslateValue").val() || !isNaN($("#AcqCost_transactionTranslateValue").val())) {
            if (!$("#AssetsAcquisation_cost").val()) {
                $("#AcqCost_transactionTranslateValue").val(0)
            } else {
                $("#AcqCost_transactionTranslateValue").val($("#AssetsAcquisation_cost").val())

            }
        }
        var MyAquisationCost = 0;
        if (!$("#Include_scerap_value").is(":checked")) {
            var ScrapValu = $("#Scrap_value").val();
            if (!ScrapValu) {
                ScrapValu = 0;
            }
            MyAquisationCost = parseFloat($("#AcqCost_transactionTranslateValue").val()) - parseFloat(ScrapValu);

        } else {
            MyAquisationCost = parseFloat($("#AcqCost_transactionTranslateValue").val());
        }
        MyAquisationCost = parseFloat(MyAquisationCost);
        var FinalRate = parseFloat(MyAquisationCost / parseFloat($("#MyUseLife").val()));

        // FinalRate = parseFloat(FinalRate / MyAquisationCost);
        if (isNaN(FinalRate)) {
            FinalRate = 0;
        }
        if (!isFinite(FinalRate)) {
            FinalRate = 0;
        }
        $("#Deprecation_rate").val(FinalRate)
        $(".DeprecationRateText").text(Math.round(parseFloat(FinalRate), 2));
        $(".DeprecationRateInputDiv").hide();
        $(".DeprecationRateDiv").show();
        ShowIfNoneDepreication();

    } else if ($("#Deprecation_method option:selected").val() == 3) {
        $(".DeprecationRateInputDiv").find("input").val(0);
        $(".DeprecationRateInputDiv").hide();
        $(".DeprecationRateDiv").hide();
        ShowIfNoneDepreication();

    } else if ($("#Deprecation_method option:selected").val() == 4) {
        $(".DeprecationRateInputDiv").find("input").val(0)
        $(".DeprecationRateInputDiv").hide();
        $(".DeprecationRateDiv").hide();
        $("#Deactive_depraction").attr("readonly", "readonly")
        HideIfNoneDepreication();
    } else {
        $(".DeprecationRateInputDiv").find("input").val(0);
        $(".DeprecationRateInputDiv").find("input").attr("max", 100);

        $("#Deprecation_rate").removeAttr("disabled")
        $(".DeprecationRateDiv").hide();
        $(".DeprecationRateInputDiv").show();
        ShowIfNoneDepreication();

    }
}
function ChangeDays() {
    var MyDate = new Date($("#Start_use").val())
    var Days = MyDate.getDate();
    var Monthes = MyDate.getMonth();
    var Year = MyDate.getFullYear()
    var LifeInt = $("#MyUseLife").val();
    var MyThisMonths = 0;
    var MyThisdays = 0;
    var EndDate = moment(MyDate).add(LifeInt, 'years')
    EndDate = EndDate.toDate();
    if (!isNaN(EndDate.getFullYear())) {
        $("#Use_life").val(EndDate.getFullYear() + "-" + GetTowDigitInt(parseInt(EndDate.getMonth()) + 1) + "-" + GetTowDigitInt(EndDate.getDate()));
        var TotalDays = 0;
        for (i = 0; i < parseInt(LifeInt); i++) {
            TotalDays += days_of_a_year(EndDate.getFullYear() + 1)
        }
        $(".MyTotalDays").text(TotalDays)
    }
    if ($("#MyUseLife").val() < 0) {
        $("#Use_life").val($("#Start_use").val())
        $("#MyUseLife").val(0)
    }
}
function GetTowDigitInt(Digit) {
    var Digits = ("0" + (Digit)).slice(-2)
    return Digits;
}
function CheckAssetsAddResult(data) {
    Talert("Assets Number :- " + data);
    if (!isNaN(data)) {
        SubmitAssetsFormAdditnalInfo(data)
    }
    window.open("/FixedAssets/Assets/Details/" + data+"?IsNumber=true")
}
