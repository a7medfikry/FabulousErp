$(function () {
    $("input[type='radio']:checked").trigger("click");
    $(".PayRadio").first().trigger("click");
})
$(document).on("click", "input[type='radio']", function () {
    if ($(this).attr("name") == "Minimum_payment") {
        if ($(this).attr("value") == "Amount" || $(this).attr("value") == "Percentage") {
            $("#MinPaymentAmount").removeClass("hide");
        } else {
            $("#MinPaymentAmount").addClass("hide");
        }
    } if ($(this).attr("name") == "Credit_limit") {
        if ($(this).attr("value") == "Amount") {
            $("#CreditLimitAmount").removeClass("hide");
        } else {
            $("#CreditLimitAmount").addClass("hide");

        }
    }
});
$(".Forms").find("h5").each(function () {
    $(this).append("<button class='btn HideDiv btn-link' type='button'>Show</button>")
})
$(document).on('click', ".HideDiv", function () {
    $(this).parents(".form-horizontal").find("div").toggle();
    if ($("#Lang").val() == "Arabic") {
        if ($(this).text() == "عرض") {
            $(this).text("إخفاء")
        } else {
            $(this).text("عرض")
        }
    } else {
        if ($(this).text() == "show") {
            $(this).text("hide")
        } else {
            $(this).text("show")
        }
    }
});
$(document).find(".HideDiv").trigger("click");
$(document).on("change", "#Group_setting_id", function () {
    var GroupSettingId = $(this).val();
    if (!GroupSettingId) {
        return;
    }
    $.ajax({
        url: "/Payable/Group_setting/HasPassword?Id=" + GroupSettingId,
        method: "POST",
        success: function (HasPassword) {
            if (HasPassword) {
                ModelMsg("<div class='col-sm-12'><label>Enter Password</label><input type='password' class='GroupSettingPas form-control'  /></div>", "Enter Password"
                    , false,
                    function () {
                        $.ajax({
                            url: "/Payable/Group_setting/CheckPass?Id=" + GroupSettingId + "&Password=" + $(document).find(".GroupSettingPas").val(),
                            method: "POST",
                            success: function (data) {
                                if (data) {
                                    GetGroupSetting(GroupSettingId)
                                } else {
                                    ModelMsg("<h4>Wrong Password</h4>", "Wrong Password", true)
                                }
                            }
                        })
                    })
            } else {
                GetGroupSetting(GroupSettingId)
            }
        }
    })
});
function GetGroupSetting(GroupSettingId) {
    $.ajax({
        url: "/Payable/Group_setting/GetGroupSeeting?Id=" + GroupSettingId,
        method: "POST",
        success: function (data) {
            $("#Currency_id").val(data.GS.Currency_id).trigger("change");
            $("#Payment_term_id").val(data.GS.Payment_term_id);
            $("#Shipping_method_id").val(data.GS.Shipping_method_id);
            $("#Tax_group_id").val(data.GS.Tax_group_id);
            $("#Inactive").attr("checked", data.GS.Inactive);
            $("#Customer").val(data.GS.Customer);
            $("#Revaluate").attr("checked", data.GS.Revaluate);
            $("#Payment_term_id").val(data.GS.Payment_terms);
            $("#Def_Checkbook").val(data.GS.Def_Checkbook);
            $(".Credit_limitRadio[data-value='" + data.GS.Credit_limit + "']").attr("checked", true);
            $(".PayRadio[data-value='" + data.GS.Minimum_payment + "']").attr("checked", true);
            $(".Payment_per[data-value='" + data.GS.Payment_per + "']").attr("checked", true);


            $("#Account_payable_id").val(data.PGA.Account_payable_id)
            $("#Purchase_id").val(data.PGA.Purchase_id)
            $("#Taken_discount_id").val(data.PGA.Taken_discount_id)
            $("#Purchase_variance_id").val(data.PGA.Purchase_variance_id)
            $("#Accrued_purchase_id").val(data.PGA.Accrued_purchase_id)
            $("#Returne_id").val(data.PGA.Returne_id)

            $("#Maximum_order").val(data.Max)
            $("#Minimum_order").val(data.Min)
        }
    })
}
$("#Minimum_order,#Maximum_order").on("focusout", function () {
    if ($("#Maximum_order").val() && $("#Minimum_order").val()) {
        if ($("#Maximum_order").val() < $("#Minimum_order").val()) {
            ModelMsg("Your Maximum Amount Can't Be More Than Minimum Amount", true)
            $("#Maximum_order").val("")
        }
    }
})