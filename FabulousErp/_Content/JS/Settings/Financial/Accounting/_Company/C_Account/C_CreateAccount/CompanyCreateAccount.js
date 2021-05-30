// Get Company Data by Session and save update button in ready to inheret mask money
$(document).ready(function ($) {

    GetCompanyName();

    $.ajax({
        type: "GET",
        url: "/C_CreateAccount/GetCurrencyFormate",
        success: function (result) {

            $("#MaxAmountPerTransaction").maskMoney({ formatOnBlur: true, reverse: true, selectAllOnFocus: true, allowEmpty: true, affixesStay: false, prefix: '' + result.Prefix + '', suffix: '' + result.Suffix + '', thousands: '' + result.Thousands + '', decimal: '' + result.Decimal + '', precision: parseInt(result.DecimalNumber) });
            $("#MinAmountPerTransaction").maskMoney({ formatOnBlur: true, reverse: true, selectAllOnFocus: true, allowEmpty: true, affixesStay: false, prefix: '' + result.Prefix + '', suffix: '' + result.Suffix + '', thousands: '' + result.Thousands + '', decimal: '' + result.Decimal + '', precision: parseInt(result.DecimalNumber) });

        }
    });


    $("#ComAccountSaveBtn").click(function () {
        CreateAccount();
    });

   

    $("#ComAccountUpdateBtn").click(function () {

        UpdateAccount();

    });


    GetCompanyAccounts();

    $("#dvCheckBoxListControl").on("change", "input[name='chklistitem']", function () {

        var accountID = $("#CurrAccountID").val();
        var currencyID = $(this).val();

        if (accountID.length > 0) {

            if (this.checked) {

                $.ajax({
                    type: "POST",
                    url: "/C_CreateAccount/SaveCurrencyToAccount?c_AID=" + accountID + "&currencyID=" + currencyID,
                    success: function () {}
                });

            } else {

                $.ajax({
                    type: "POST",
                    url: "/C_CreateAccount/RemoveCurrencyFromAccount?c_AID=" + accountID + "&currencyID=" + currencyID,
                    success: function () {}
                });

            }
        }
    });



    $("#CurrAccountID").change(function () {
        var accountID = $(this).val();
        $("#CurrAccountName").val("");
        $("input[name=chklistitem]").prop('checked', false);
        if (accountID.length === 0) {
            $(this).css("border-color", "red");
        } else {
            $(this).css("border-color", "");
            $.ajax({
                type: "GET",
                url: "/C_CreateAccount/GetAccountName?c_AID=" + accountID,
                success: function (result) {
                    $("#CurrAccountName").val(result);

                    $.ajax({
                        type: "GET",
                        url: "/C_CreateAccount/GetAccountCurrencies?c_AID=" + accountID,
                        success: function (result) {
                            if (result.length > 0) {
                                for (var i = 0; i < result.length; i++) {
                                    $("input[name=chklistitem][value=" + result[i].CurrencyID + "]").prop('checked', true);
                                }
                            }

                        }
                    });

                }
            });
        }
    });

    $("#next").click(function () {
        var nextElement = $('#CurrAccountID > option:selected').next('option');
        if (nextElement.length > 0) {
            $('#CurrAccountID > option:selected').removeAttr('selected').next('option').attr('selected', 'selected');
            $("#CurrAccountID").change();
        }
    });

    $("#prev").click(function () {
        var nextElement = $('#CurrAccountID > option:selected').prev('option');
        if (nextElement.length > 0) {
            $('#CurrAccountID > option:selected').removeAttr('selected').prev('option').attr('selected', 'selected');
            $("#CurrAccountID").change();
        }
    });


    $("#nextSearch").click(function () {
        var nextElement = $('#SearchAccountID > option:selected').next('option');
        if (nextElement.length > 0) {
            $('#SearchAccountID > option:selected').removeAttr('selected').next('option').attr('selected', 'selected');
            $("#SearchAccountID").change();
        }
    });

    $("#prevSearch").click(function () {
        var nextElement = $('#SearchAccountID > option:selected').prev('option');
        var check = nextElement.val();
        if (check.length === 0) { } else {
            if (nextElement.length > 0) {
                $('#SearchAccountID > option:selected').removeAttr('selected').prev('option').attr('selected', 'selected');
                $("#SearchAccountID").change();
            }
        }
    });
});
function CreateAccount(Reload = true) {

    var Test = true;

    var GetSeparator1 = $("#GetSeparator1").text();
    var sWithSep1 = "";
    var Segment1 = "";
    if ($("#Segment1").length > 0) {

        Segment1 = $("#Segment1").val();

        var SLength1 = $("#SLength1").text();

        if (Segment1.length != SLength1) {

            $("#Segment1").css("border", "1px solid red");
            Test = false;

        } else {
            $("#Segment1").css("border", "");
            sWithSep1 = Segment1 + GetSeparator1;
        }
    }

    var GetSeparator2 = $("#GetSeparator2").text();
    var sWithSep2 = "";
    var Segment2 = "";
    if ($("#Segment2").length > 0) {

        Segment2 = $("#Segment2").val();

        var SLength2 = $("#SLength2").text();

        if (Segment2.length != SLength2) {
            $("#Segment2").css("border", "1px solid red");
            Test = false;
        } else {
            $("#Segment2").css("border", "");
            sWithSep2 = Segment2 + GetSeparator2;
        }
    }

    var GetSeparator3 = $("#GetSeparator3").text();
    var sWithSep3 = "";
    var Segment3 = "";
    if ($("#Segment3").length > 0) {

        Segment3 = $("#Segment3").val();

        var SLength3 = $("#SLength3").text();

        if (Segment3.length != SLength3) {
            $("#Segment3").css("border", "1px solid red");
            Test = false;
        } else {
            $("#Segment3").css("border", "");
            sWithSep3 = Segment3 + GetSeparator3;
        }
    }

    var GetSeparator4 = $("#GetSeparator4").text();
    var sWithSep4 = "";
    var Segment4 = "";
    if ($("#Segment4").length > 0) {

        Segment4 = $("#Segment4").val();

        var SLength4 = $("#SLength4").text();

        if (Segment4.length != SLength4) {
            $("#Segment4").css("border", "1px solid red");
            Test = false;
        } else {
            $("#Segment4").css("border", "");
            sWithSep4 = Segment4 + GetSeparator4;
        }
    }

    var GetSeparator5 = $("#GetSeparator5").text();
    var sWithSep5 = "";
    var Segment5 = "";
    if ($("#Segment5").length > 0) {

        Segment5 = $("#Segment5").val();

        var SLength5 = $("#SLength5").text();

        if (Segment5.length != SLength5) {
            $("#Segment5").css("border", "1px solid red");
            Test = false;
        } else {
            $("#Segment5").css("border", "");
            sWithSep5 = Segment5 + GetSeparator5;
        }
    }

    var GetSeparator6 = $("#GetSeparator6").text();
    var sWithSep6 = "";
    var Segment6 = "";
    if ($("#Segment6").length > 0) {

        Segment6 = $("#Segment6").val();

        var SLength6 = $("#SLength6").text();

        if (Segment6.length != SLength6) {
            $("#Segment6").css("border", "1px solid red");
            Test = false;
        } else {
            $("#Segment6").css("border", "");
            sWithSep6 = Segment6 + GetSeparator6;
        }
    }

    var GetSeparator7 = $("#GetSeparator7").text();
    var sWithSep7 = "";
    var Segment7 = "";
    if ($("#Segment7").length > 0) {

        Segment7 = $("#Segment7").val();

        var SLength7 = $("#SLength7").text();

        if (Segment7.length != SLength7) {
            $("#Segment7").css("border", "1px solid red");
            Test = false;
        } else {
            $("#Segment7").css("border", "");
            sWithSep7 = Segment7 + GetSeparator7;
        }
    }

    var GetSeparator8 = $("#GetSeparator8").text();
    var sWithSep8 = "";
    var Segment8 = "";
    if ($("#Segment8").length > 0) {

        Segment8 = $("#Segment8").val();

        var SLength8 = $("#SLength8").text();

        if (Segment8.length != SLength8) {
            $("#Segment8").css("border", "1px solid red");
            Test = false;
        } else {
            $("#Segment8").css("border", "");
            sWithSep8 = Segment8 + GetSeparator8;
        }
    }

    var GetSeparator9 = $("#GetSeparator9").text();
    var sWithSep9 = "";
    var Segment9 = "";
    if ($("#Segment9").length > 0) {

        Segment9 = $("#Segment9").val();

        var SLength9 = $("#SLength9").text();

        if (Segment9.length != SLength9) {
            $("#Segment9").css("border", "1px solid red");
            Test = false;
        } else {
            $("#Segment9").css("border", "");
            sWithSep9 = Segment9 + GetSeparator9;
        }
    }

    var GetSeparator10 = $("#GetSeparator10").text();
    var sWithSep10 = "";
    var Segment10 = "";
    if ($("#Segment10").length > 0) {

        Segment10 = $("#Segment10").val();

        var SLength10 = $("#SLength10").text();

        if (Segment10.length != SLength10) {
            $("#Segment10").css("border", "1px solid red");
            Test = false;
        } else {
            $("#Segment10").css("border", "");
            sWithSep10 = Segment10 + GetSeparator10;
        }
    }

    var SegmentNon = "";
    if ($("#SegmentNon").length > 0) {

        SegmentNon = $("#SegmentNon").val();

        var CheckLength = $("#CheckLength").text();

        if (SegmentNon.length != CheckLength) {
            $("#SegmentNon").css("border", "1px solid red");
            Test = false;
        } else {
            $("#SegmentNon").css("border", "");
        }
    }


    var accountID = sWithSep1 + sWithSep2 + sWithSep3 + sWithSep4 + sWithSep5 + sWithSep6 + sWithSep7 + sWithSep8 + sWithSep9 + sWithSep10 + SegmentNon;

    var accountIDWithoutSep = Segment1 + Segment2 + Segment3 + Segment4 + Segment5 + Segment6 + Segment7 + Segment8 + Segment9 + Segment10;

    var AccountName = $("#AccountName").val();

    var AccountsGroup = $("#AccountsGroup").val();

    var CompanyID = $("#CompanyID").text();

    var ChartAccountID = $("#ChartAccountID").text();

    //var Currency = $("#Currency").val();


    var DisActive = false;
    if ($("#DisActive").is(":checked")) {
        DisActive = true;
    }

    var AccountType = "";
    if ($("#Debit").is(":checked")) {
        AccountType = "Debit";
    } else if ($("#Credit").is(":checked")) {
        AccountType = "Credit";
    }

    var PostingType = "";
    if ($("#BallanceSheet").is(":checked")) {
        PostingType = "BallanceSheet";
    } else if ($("#PL").is(":checked")) {
        PostingType = "PL";
    }

    var ReconcileAccount = false;
    if ($("#ReconcileAccount").is(":checked")) {
        ReconcileAccount = true;
    }
    var AllowAdjusment = false;
    if ($("#AllowAdjusment").is(":checked")) {
        AllowAdjusment = true;
    }
    var Reevaluate = false;
    if ($("#Reevaluate").is(":checked")) {
        Reevaluate = true;
    }
    var ConslidationAccount = false;
    if ($("#ConslidationAccount").is(":checked")) {
        ConslidationAccount = true;
    }
    var SupportDocument = false;
    if ($("#SupportDocument").is(":checked")) {
        SupportDocument = true;
    }

    var MaxAmountPerTransaction = $("#MaxAmountPerTransaction").val().replace(/^[, ]+|[, ]+$|[, ]+/g, "").trim();
    var MinAmountPerTransaction = $("#MinAmountPerTransaction").val().replace(/^[, ]+|[, ]+$|[, ]+/g, "").trim();

    var FinancialArea = $("#FinancialArea").val();
    var SalesArea = $("#SalesArea").val();
    var PurshacingArea = $("#PurshacingArea").val();
    var InventoryArea = $("#InventoryArea").val();


    if (AccountsGroup == "-1") {
        $("#AccountsGroup").css("border-color", "red");
        Test = false;
    } else {
        $("#AccountsGroup").css("border-color", "");
    }

    //if (Currency.length == 0) {
    //    $("#Currency").css("border-color", "red");
    //    Test = false;
    //} else {
    //    $("#Currency").css("border-color", "");
    //}

    if (AccountName.length == 0) {
        $("#AccountName").css("border-color", "red");
        Test = false;
    } else {
        $("#AccountName").css("border-color", "");
    }

    if ($('input[name=AccountType]:checked').length <= 0) {
        Test = false;
        $(".RBSAT").css("border", "1px solid red");
    } else {
        $(".RBSAT").css("border", "");
    }

    if ($('input[name=PostingType]:checked').length <= 0) {
        Test = false;
        $(".RBPT").css("border", "1px solid red");
    } else {
        $(".RBPT").css("border", "");
    }


    var AAID = $("#AnalyticAccountID").val();
    var AnalyticAccountID = "";
    if (AAID != "-1") {
        AnalyticAccountID = AAID;
    }


    var CostCenterType = "";
    if ($("#CostCenter").is(":checked")) {
        CostCenterType = "CostCenter";
    } else if ($("#MainCostCenter").is(":checked")) {
        CostCenterType = "MainCostCenter";
    }

    var CCID = $("#CostCenterID").val();
    var CostCenterID = "";
    if (CCID != "-1") {
        CostCenterID = CCID;
    }

    var MCCID = $("#MainCostCenterID").val();
    var MainCostCenterID = "";
    if (MCCID != "-1") {
        MainCostCenterID = MCCID;
    }

    if (Test === true) {

        $.ajax({
            type: "POST",
            url: "/C_CreateAccount/SaveCompAccountInfo?AccountID=" + accountID + "&AccountName=" + AccountName + "&AccountsGroup=" + AccountsGroup + "&CompanyID=" + CompanyID + "&ChartAccountID=" + ChartAccountID + "&DisActive=" + DisActive + "&AccountType=" + AccountType
                + "&PostingType=" + PostingType + "&ReconcileAccount=" + ReconcileAccount + "&AllowAdjusment=" + AllowAdjusment + "&Reevaluate=" + Reevaluate + "&ConslidationAccount=" + ConslidationAccount + "&MaxAmountPerTransaction=" + MaxAmountPerTransaction + "&MinAmountPerTransaction=" + MinAmountPerTransaction
                + "&AnalyticAccountID=" + AnalyticAccountID + "&CostCenterType=" + CostCenterType + "&CostCenterID=" + CostCenterID + "&MainCostCenterID=" + MainCostCenterID + "&AccountIDWithoutSep=" + accountIDWithoutSep + "&FinancialArea=" + FinancialArea + "&SalesArea=" + SalesArea + "&PurshacingArea=" + PurshacingArea + "&InventoryArea=" + InventoryArea
                + "&SupportDocument=" + SupportDocument,
            success: function (result) {
                if (result == "Out") {
                    $("#AccountIDError").text("Out Of Range..!");
                } else if (result == "False") {
                    $("#AccountIDError").text("Account ID Not Valid..!");
                } else {
                    if (Reload) {
                        location.reload();

                    }
                }
            }
        });
    }
}

function UpdateAccount(Reload = true) {
    //var Currency = $("#Currency").val();

    var MaxAmountPerTransaction = $("#MaxAmountPerTransaction").val().replace(/^[, ]+|[, ]+$|[, ]+/g, "").trim();

    var MinAmountPerTransaction = $("#MinAmountPerTransaction").val().replace(/^[, ]+|[, ]+$|[, ]+/g, "").trim();

    var ChartAccountID = $("#ChartAccountID").text();

    var FinancialArea = $("#FinancialArea").is(":checked");
    var SalesArea = $("#SalesArea").is(":checked");
    var PurshacingArea = $("#PurshacingArea").is(":checked");
    var InventoryArea = $("#InventoryArea").is(":checked");

    var Test = true;

    var accountID = $("#SearchAccountID").val();

    if (accountID.length === 0) {
        $("#SearchAccountID").css("border-color", "red");
        Test = false;
    } else {
        $("#SearchAccountID").css("border-color", "");
    }

    //if (Currency.length == 0) {
    //    $("#Currency").css("border-color", "red");
    //    Test = false;
    //} else {
    //    $("#Currency").css("border-color", "");
    //}

    var DisActive = false;
    if ($("#DisActive").is(":checked")) {
        DisActive = true;
    }

    var AccountType = "";
    if ($("#Debit").is(":checked")) {
        AccountType = "Debit";
    } else if ($("#Credit").is(":checked")) {
        AccountType = "Credit";
    }

    var PostingType = "";
    if ($("#BallanceSheet").is(":checked")) {
        PostingType = "BallanceSheet";
    } else if ($("#PL").is(":checked")) {
        PostingType = "PL";
    }

    var ReconcileAccount = false;
    if ($("#ReconcileAccount").is(":checked")) {
        ReconcileAccount = true;
    }

    var AllowAdjusment = false;
    if ($("#AllowAdjusment").is(":checked")) {
        AllowAdjusment = true;
    }

    var Reevaluate = false;
    if ($("#Reevaluate").is(":checked")) {
        Reevaluate = true;
    }

    var ConslidationAccount = false;
    if ($("#ConslidationAccount").is(":checked")) {
        ConslidationAccount = true;
    }

    var SupportDocument = false;
    if ($("#SupportDocument").is(":checked")) {
        SupportDocument = true;
    }

    var AnalyticAccountID = "";
    var AAID = $("#AnalyticAccountID").val();
    if (AAID != "-1") {
        AnalyticAccountID = AAID;
    }

    var CostCenterType = "";
    if ($("#CostCenter").is(":checked")) {
        CostCenterType = "CostCenter";
    } else if ($("#MainCostCenter").is(":checked")) {
        CostCenterType = "MainCostCenter";
    }

    var CostCenterID = "";
    var CCID = $("#CostCenterID").val();
    if (CCID != "-1") {
        CostCenterID = CCID;
    }

    var MainCostCenterID = "";
    var MCCID = $("#MainCostCenterID").val();
    if (MCCID != "-1") {
        MainCostCenterID = MCCID;
    }

    if (Test === true) {

        $.ajax({
            type: "POST",
            url: "/C_CreateAccount/UpdateCompAccountInfo?AccountID=" + accountID + "&DisActive=" + DisActive + "&AccountType=" + AccountType
                + "&PostingType=" + PostingType + "&ReconcileAccount=" + ReconcileAccount + "&AllowAdjusment=" + AllowAdjusment + "&Reevaluate=" + Reevaluate + "&ConslidationAccount=" + ConslidationAccount + "&MaxAmountPerTransaction=" + MaxAmountPerTransaction + "&MinAmountPerTransaction=" + MinAmountPerTransaction
                + "&AnalyticAccountID=" + AnalyticAccountID + "&CostCenterType=" + CostCenterType + "&CostCenterID=" + CostCenterID + "&MainCostCenterID=" + MainCostCenterID + "&ChartAccountID=" + ChartAccountID + "&FinancialArea=" + FinancialArea + "&SalesArea=" + SalesArea + "&PurshacingArea=" + PurshacingArea + "&InventoryArea=" + InventoryArea
                + "&SupportDocument=" + SupportDocument,
            success: function (result) {
                if (result == "True") {
                    if (Reload) {
                        location.reload();
                    }
                }
            }
        });

    }
}
//Get Company data of Chart and view of it
function GetCompanyName() {

    $.ajax({
        type: "GET",
        url: "/C_CreateAccount/GetCompanyName",
        success: function (result) {

            if (result == "NoChart") {
                $("#GlobalError").text("No Chart Of Account Created To This Company..!");
            } else {
                $("#GlobalError").text("");
                $("#CompanyName").text(result.CBFName);
                $("#ChartAccountID").text(result.ChartOfAccountID);
                $("#ChartAccountName").text(result.ChartOfAccountName);

                //Get View of chart with Segment and non segment
                GetChartView();

                //Get Content of Group That related with this chart
                GetAccountsGroup();

                //Get Analytic Account that related with this Company
                GetAnalyticAccount();

                //Get Cost Center that related with this Company before choosen cc
                GetCostCenter();

                //Get Main Cost Center that related with this Company before choosen Mcc
                GetMainCostCenter();

                $.ajax({
                    type: "GET",
                    url: "/C_CreateAccount/GetCompanyAccounts?sortType=" + 1,
                    success: function (result) {
                        $("#SearchAccountID").empty();

                        if (result.length == 0) {

                            $("#SearchAccountID").append($('<option/>', {
                                value: "",
                                text: "No Accounts Created To This Company"
                            })
                            );
                        } else {

                            $("#SearchAccountID").append($('<option/>', {
                                value: "",
                                text: "-Choose-"
                            })
                            );
                            $.each(result, function (index, row) {

                                $("#SearchAccountID").append("<option value='" + row.C_AID + "'>" + row.C_AccountID + " ( " + row.AccountName + " )" + "</option>");

                            });
                        }
                    }
                });

            }

        }
    });

}


//Get View of chart with Segment
function GetChartView() {

    var ChartAccountID = $("#ChartAccountID").text();

    $("#SegmentsTexts").text("");
    $("#ChartFormat").text("");
    $("#CheckLength").text("");

    $.ajax({
        type: "GET",
        url: "/C_CreateAccount/GetViewOfChart?ChartOfAccountID=" + ChartAccountID,
        success: function (result) {

            if (result.Status == "False") {

                ChartWithoutSegments(result.Length);

            } else {

                //prepare account inputs
                var text = '<input type="number" id="Segment#" value="" class="Segments m-1" onkeydown = "javascript: return event.keyCode == 69 ? false : true" style="width:44px; font-size:12px; padding-left: 2px;" oninput="javascript: if (this.value.length > this.maxLength) this.value = this.value.slice(0, this.maxLength);" min = "0" onkeypress = "return (event.charCode == 8 || event.charCode == 0) ? null : event.charCode >= 48 && event.charCode <= 57"/>';

                var formateInstructions = '<label id="InstFormate#"></label>' // instruction formate

                //prepare label of seperator between inputs
                var Separator = "<label> S# </label>";


                for (var i = 0; i < result.length; i++) {
                    //Append from texts with generate id
                    $("#SegmentsTexts").append(text.replace('#', i + 1));

                    // instruction formate
                    var str = " * ";

                    $("#ChartFormat").append(formateInstructions.replace('#', i + 1));

                    $("#InstFormate" + parseFloat(i + 1)).text(str.repeat(parseInt(result[i].Length)));
                    //------------------------------------------------------------------------------------


                    if (i < result[i].NumberOfSegment - 1) {
                        //put seperator between texts
                        $("#Segment" + parseFloat(i + 1)).after('<label id=GetSeparator' + parseFloat(i + 1) + '>' + result[i].Seperator + '</label>');

                        $("#InstFormate" + parseFloat(i + 1)).after(Separator.replace('S#', result[i].Seperator)); // instruction formate
                    }
                    //secify max length of segments
                    $("#Segment" + parseFloat(i + 1)).attr('maxlength', parseInt(result[i].Length));

                    // check length for add btn
                    $("#CheckLength").append('<label id=SLength' + parseFloat(i + 1) + '>' + parseInt(result[i].Length) + '</label>');

                }
            }
        }
    });

}
// and non segment
function ChartWithoutSegments(Length) {

    var textNon = '<input type="number" id="SegmentNon" value="" class="Segments m-1" onkeydown = "javascript: return event.keyCode == 69 ? false : true" oninput="javascript: if (this.value.length > this.maxLength) this.value = this.value.slice(0, this.maxLength);" min = "0" onkeypress = "return (event.charCode == 8 || event.charCode == 0) ? null : event.charCode >= 48 && event.charCode <= 57"/>';

    var formateInstructions = '<label id="InstFormateNon"></label>'

    $("#SegmentsTexts").append(textNon);

    $("#SegmentNon").attr('maxlength', parseInt(Length));

    var str = " * ";

    $("#ChartFormat").append(formateInstructions);

    $("#InstFormateNon").text(str.repeat(parseInt(Length)));

    $("#CheckLength").text(Length);

}


//Get Content of Group That related with this chart
function GetAccountsGroup() {

    var ChartAccountID = $("#ChartAccountID").text();

    $.ajax({
        type: "GET",
        url: "/C_CreateAccount/GetAccountsGroup?ChartOfAccountID=" + ChartAccountID,
        success: function (data) {
            $("#AccountsGroup").empty();

            if (data.length == 0) {

                $("#AccountsGroup").append($('<option/>', {
                    value: -1,
                    text: "No Account Group Created To This Chart"
                })
                );
            } else {

                $("#AccountsGroup").append($('<option/>', {
                    value: -1,
                    text: "-Choose-"
                })
                );
                $.each(data, function (index, row) {

                    $("#AccountsGroup").append("<option value='" + row.AccountGroupName + "'>" + row.AccountGroupName + "</option>");

                });
            }
        }
    });
}
//Change of Content
$("#AccountsGroup").click(function () {

    var AccountsGroup = $(this).val();

    var Test = true;

    if (AccountsGroup == "-1") {
        $(this).css("border-color", "red");
        $("#GroupFrom").text("");
        $("#GroupTo").text("");
    } else {

        $.ajax({
            type: "GET",
            url: "/C_CreateAccount/GetFromTo?AccountsGroup=" + AccountsGroup,
            success: function (result) {
                $("#GroupFrom").text(result.FromWithSep);
                $("#GroupTo").text(result.ToWithSep);
            }
        });

        $(this).css("border-color", "");

        var Segment1 = "";
        if ($("#Segment1").length > 0) {

            Segment1 = $("#Segment1").val();

            var SLength1 = $("#SLength1").text();

            if (Segment1.length != SLength1) {

                $("#Segment1").css("border", "1px solid red");
                Test = false;

            } else {
                $("#Segment1").css("border", "");
            }
        }

        var Segment2 = "";
        if ($("#Segment2").length > 0) {

            Segment2 = $("#Segment2").val();

            var SLength2 = $("#SLength2").text();

            if (Segment2.length != SLength2) {
                $("#Segment2").css("border", "1px solid red");
                Test = false;
            } else {
                $("#Segment2").css("border", "");
            }
        }

        var Segment3 = "";
        if ($("#Segment3").length > 0) {

            Segment3 = $("#Segment3").val();

            var SLength3 = $("#SLength3").text();

            if (Segment3.length != SLength3) {
                $("#Segment3").css("border", "1px solid red");
                Test = false;
            } else {
                $("#Segment3").css("border", "");
            }
        }

        var Segment4 = "";
        if ($("#Segment4").length > 0) {

            Segment4 = $("#Segment4").val();

            var SLength4 = $("#SLength4").text();

            if (Segment4.length != SLength4) {
                $("#Segment4").css("border", "1px solid red");
                Test = false;
            } else {
                $("#Segment4").css("border", "");
            }
        }

        var Segment5 = "";
        if ($("#Segment5").length > 0) {

            Segment5 = $("#Segment5").val();

            var SLength5 = $("#SLength5").text();

            if (Segment5.length != SLength5) {
                $("#Segment5").css("border", "1px solid red");
                Test = false;
            } else {
                $("#Segment5").css("border", "");
            }
        }

        var Segment6 = "";
        if ($("#Segment6").length > 0) {

            Segment6 = $("#Segment6").val();

            var SLength6 = $("#SLength6").text();

            if (Segment6.length != SLength6) {
                $("#Segment6").css("border", "1px solid red");
                Test = false;
            } else {
                $("#Segment6").css("border", "");
            }
        }

        var Segment7 = "";
        if ($("#Segment7").length > 0) {

            Segment7 = $("#Segment7").val();

            var SLength7 = $("#SLength7").text();

            if (Segment7.length != SLength7) {
                $("#Segment7").css("border", "1px solid red");
                Test = false;
            } else {
                $("#Segment7").css("border", "");
            }
        }

        var Segment8 = "";
        if ($("#Segment8").length > 0) {

            Segment8 = $("#Segment8").val();

            var SLength8 = $("#SLength8").text();

            if (Segment8.length != SLength8) {
                $("#Segment8").css("border", "1px solid red");
                Test = false;
            } else {
                $("#Segment8").css("border", "");
            }
        }

        var Segment9 = "";
        if ($("#Segment9").length > 0) {

            Segment9 = $("#Segment9").val();

            var SLength9 = $("#SLength9").text();

            if (Segment9.length != SLength9) {
                $("#Segment9").css("border", "1px solid red");
                Test = false;
            } else {
                $("#Segment9").css("border", "");
            }
        }

        var Segment10 = "";
        if ($("#Segment10").length > 0) {

            Segment10 = $("#Segment10").val();

            var SLength10 = $("#SLength10").text();

            if (Segment10.length != SLength10) {
                $("#Segment10").css("border", "1px solid red");
                Test = false;
            } else {
                $("#Segment10").css("border", "");
            }
        }

        var SegmentNon = "";
        if ($("#SegmentNon").length > 0) {

            SegmentNon = $("#SegmentNon").val();

            var CheckLength = $("#CheckLength").text();

            if (SegmentNon.length != CheckLength) {
                $("#SegmentNon").css("border", "1px solid red");
                Test = false;
            } else {
                $("#SegmentNon").css("border", "");
            }
        }

        var totalSegment = Segment1 + Segment2 + Segment3 + Segment4 + Segment5 + Segment6 + Segment7 + Segment8 + Segment9 + Segment10 + SegmentNon;

        if (Test === true) {

            $.ajax({
                type: "GET",
                url: "/C_CreateAccount/GetFromTo?AccountsGroup=" + AccountsGroup,
                success: function (result) {
                    var AccountFrom = result.From;
                    var AccountTo = result.To;

                    if (totalSegment >= AccountFrom && totalSegment <= AccountTo) {
                        $("#AccountIDError").text("")
                    } else {
                        $("#AccountIDError").text("Out Of Range..!")
                    }
                }
            });

        }

    }

});



//Get Analytic Account that related with this Company
function GetAnalyticAccount() {

    var CompanyID = $("#CompanyID").text();

    $.ajax({
        type: "GET",
        url: "/C_CreateAccount/GetAnalyticAccount?CompanyID=" + CompanyID,
        success: function (data) {
            $("#AnalyticAccountID").empty();

            $("#AnalyticAccountName").text("");

            if (data.length == 0) {

                $("#AnalyticAccountID").append($('<option/>', {
                    value: -1,
                    text: "No Analytic Account Created To This Company"
                })
                );
            } else {

                $("#AnalyticAccountID").append($('<option/>', {
                    value: -1,
                    text: "-Choose-"
                })
                );
                $.each(data, function (index, row) {

                    $("#AnalyticAccountID").append("<option value='" + row.AAccountID + "'>" + row.AAccountID + "</option>");

                });
            }
        }
    });
}
//Change of Analytic Accounts
$("#AnalyticAccountID").change(function () {

    var AnalyticAccountID = $(this).val();

    if (AnalyticAccountID == "-1") {
        $(this).css("border-color", "red");
        $("#AnalyticAccountName").text("");
    } else {
        $(this).css("border-color", "");
        $.ajax({
            type: "GET",
            url: "/C_CreateAccount/GetAnalyticAccountName?AnalyticAccountID=" + AnalyticAccountID,
            success: function (Data) {
                $("#AnalyticAccountName").text(Data);
            }
        });
    }

});



//Get Cost Center that related with this Company before choosen cc
function GetCostCenter() {
    var CompanyID = $("#CompanyID").text();

    $.ajax({
        type: "GET",
        url: "/C_CreateAccount/GetCostCenter?CompanyID=" + CompanyID,
        success: function (data) {
            $("#CostCenterID").empty();

            $("#CostCenterName").text("");

            if (data.length == 0) {

                $("#CostCenterID").append($('<option/>', {
                    value: -1,
                    text: "No Cost Center Account Created To This Company"
                })
                );
            } else {

                $("#CostCenterID").append($('<option/>', {
                    value: -1,
                    text: "-Choose-"
                })
                );
                $.each(data, function (index, row) {

                    $("#CostCenterID").append("<option value='" + row.CostCenterID + "'>" + row.CostCenterID + "</option>");

                });
            }
        }
    });
}
//Change of Cost Center
$("#CostCenterID").change(function () {

    var CostCenterID = $(this).val();

    if (CostCenterID == "-1") {
        $(this).css("border-color", "red");
        $("#CostCenterName").text("");
    } else {

        $(this).css("border-color", "");
        $.ajax({
            type: "GET",
            url: "/C_CreateAccount/GetCostCenterName?CostCenterID=" + CostCenterID,
            success: function (Data) {
                $("#CostCenterName").text(Data);
            }
        });
    }
});



//Get Main Cost Center that related with this Company before choosen Mcc
function GetMainCostCenter() {
    var CompanyID = $("#CompanyID").text();

    $.ajax({
        type: "GET",
        url: "/C_CreateAccount/GetMainCostCenter?CompanyID=" + CompanyID,
        success: function (data) {
            $("#MainCostCenterID").empty();

            $("#MainCostCenterName").text("");

            if (data.length == 0) {

                $("#MainCostCenterID").append($('<option/>', {
                    value: -1,
                    text: "No Main Cost Center Account Created To This Company"
                })
                );
            } else {

                $("#MainCostCenterID").append($('<option/>', {
                    value: -1,
                    text: "-Choose-"
                })
                );
                $.each(data, function (index, row) {

                    $("#MainCostCenterID").append("<option value='" + row.MainCostCenterID + "'>" + row.MainCostCenterID + "</option>");

                });
            }
        }
    });
}
//Change of Main Cost Center 
$("#MainCostCenterID").change(function () {

    var MainCostCenterID = $(this).val();

    if (MainCostCenterID == "-1") {
        $(this).css("border-color", "red");
        $("#MainCostCenterName").text("");
    } else {

        $(this).css("border-color", "");
        $.ajax({
            type: "GET",
            url: "/C_CreateAccount/GetMainCostCenterName?MainCostCenterID=" + MainCostCenterID,
            success: function (Data) {
                $("#MainCostCenterName").text(Data);
            }
        });
    }
});



// Custom Validation
$("#AccountName").keyup(function () {

    var AccountName = $(this).val();

    if (AccountName.length === 0) {
        $(this).css("border-color", "red");
    } else {
        $(this).css("border-color", "");
    }

}).focusout(function () {

    var AccountName = $(this).val();

    if (AccountName.length === 0) {
        $(this).css("border-color", "red");
    } else {
        $(this).css("border-color", "");
    }

});


//------------------------------------------------------------------



//Radio buttons of cost center or main cost center
$("#CostCenter").click(function () {
    if ($("#CostCenter").is(':checked')) {
        $("#SHCC").show();
        $("#SHMCC").hide();
        $("#MainCostCenterID").val("-1");
        $("#MainCostCenterName").text("");
    }
});

$("#MainCostCenter").click(function () {
    if ($("#MainCostCenter").is(':checked')) {
        $("#SHMCC").show();
        $("#SHCC").hide();
        $("#CostCenterID").val("-1");
        $("#CostCenterName").text("");
    }
});
//------------------------------------------------



// Search Button To Update
$("#SearchAccountID").change(function () {
    var accountID = $(this).val();
    $("#SearchAccountID").prop("disabled", false);

    var ChartAccountID = $("#ChartAccountID").text();
    if (accountID.length === 0) {
        $(this).css("border-color", "red");
    } else {
        $(this).css("border-color", "");
        
        $.ajax({
            type: "GET",
            url: "/C_CreateAccount/GetCompAccountData?AccountID=" + accountID + "&ChartAccountID=" + ChartAccountID,
            success: function (result) {

                if (result == "False") {
                    $("#AccountIDError").text("Wrong Account ID..!");
                } else {

                    $("#AccountIDError").text("");
                    $("#SearchAccountID").prop("disabled", true);
                    $(".Segments").prop("disabled", true);
                    $("#AccountName").prop("disabled", true);
                    $("#AccountsGroup").prop("disabled", true);
                    $("#FTHide").hide();
                    $("#ComAccountSaveBtn").hide();
                    $("#ComAccountUpdateBtn").show();

                    $("#AccountName").val(result.AccountName);
                    $("#AccountsGroup").val(result.AccountsGroup);
                    //$("#Currency").val(result.Currency);
                    $("#MaxAmountPerTransaction").val(result.MaximumAmountPerTransaction);
                    $("#MinAmountPerTransaction").val(result.MinimumAmountPerTransaction);

                    if (result.C_Prefix != null) {
                        $('#AnalyticAccountID').prop('disabled', true);
                        $('#CostCenter').prop('disabled', true);
                        $('#MainCostCenter').prop('disabled', true);
                        $('#CostCenterID').prop('disabled', true);
                        $('#MainCostCenterID').prop('disabled', true);
                    }

                    if (result.FinancialArea === true && result.SalesArea === true && result.PurshacingArea === true && result.InventoryArea === true) {
                        $("#AllArea").prop("checked", true);
                    } else {
                        $("#AllArea").prop("checked", false);
                    }

                    if (result.FinancialArea === true) {
                        $("#FinancialArea").prop("checked", true);
                        $("#FinancialArea").attr('value', true);
                    } else {
                        $("#FinancialArea").prop("checked", false);
                        $("#FinancialArea").attr('value', false);
                    }
                    if (result.SalesArea === true) {
                        $("#SalesArea").prop("checked", true);
                        $("#SalesArea").attr('value', true);
                    } else {
                        $("#SalesArea").prop("checked", false);
                        $("#SalesArea").attr('value', false);
                    }
                    if (result.PurshacingArea === true) {
                        $("#PurshacingArea").prop("checked", true);
                        $("#PurshacingArea").attr('value', true);
                    } else {
                        $("#PurshacingArea").prop("checked", false);
                        $("#PurshacingArea").attr('value', false);
                    }
                    if (result.InventoryArea === true) {
                        $("#InventoryArea").prop("checked", true);
                        $("#InventoryArea").attr('value', true);
                    } else {
                        $("#InventoryArea").prop("checked", false);
                        $("#InventoryArea").attr('value', false);
                    }


                    if (result.DisActive == true) {
                        $("#DisActive").prop("checked", true);
                    } else { $("#DisActive").prop("checked", false); }

                    if (result.ReconcileAccount == true) {
                        $("#ReconcileAccount").prop("checked", true);
                    } else { $("#ReconcileAccount").prop("checked", false); }

                    if (result.AllowAdjusment == true) {
                        $("#AllowAdjusment").prop("checked", true);
                    } else { $("#AllowAdjusment").prop("checked", false); }

                    if (result.Reevaluate == true) {
                        $("#Reevaluate").prop("checked", true);
                    } else { $("#Reevaluate").prop("checked", false); }

                    if (result.ConsolidationAccount == true) {
                        $("#ConslidationAccount").prop("checked", true);
                    } else { $("#ConslidationAccount").prop("checked", false); }

                    if (result.SupportDocument == true) {
                        $("#SupportDocument").prop("checked", true);
                    } else { $("#SupportDocument").prop("checked", false); }

                    if (result.AccountType == "Debit") {
                        $("#Debit").prop("checked", true);
                    } else {
                        $("#Credit").prop("checked", true);
                    }

                    if (result.PostingType == "BallanceSheet") {
                        $("#BallanceSheet").prop("checked", true);
                    } else {
                        $("#PL").prop("checked", true);
                    }

                    if (result.R_AnalyticAccountID == "") {
                        $("#AnalyticAccountID").val("-1");
                    } else {
                        $("#AnalyticAccountID").val(result.R_AnalyticAccountID);
                    }

                    if (result.CostCenterType == "CostCenter") {
                        $("#CostCenter").prop("checked", true);
                        $("#SHCC").show();
                        $("#SHMCC").hide();
                    } else if (result.CostCenterType == "MainCostCenter") {
                        $("#MainCostCenter").prop("checked", true);
                        $("#SHCC").hide();
                        $("#SHMCC").show();
                    }

                    if (result.R_CostCenterID == "") {
                        $("#CostCenterID").val("-1");
                    } else {
                        $("#CostCenterID").val(result.R_CostCenterID);
                    }

                    if (result.R_CostCenterGroupID == "") {
                        $("#MainCostCenterID").val("-1");
                    } else {
                        $("#MainCostCenterID").val(result.R_CostCenterGroupID);
                    }
                }
            }
        });
    }
});


// Reset Button To Reload Page
$("#ComAccountClearBtn").click(function () {
    location.reload();
});



$("#AllArea").on('change', function () {
    if ($(this).is(':checked')) {
        $("#FinancialArea").prop("checked", true);
        $("#FinancialArea").attr('value', true);
        $("#SalesArea").prop("checked", true);
        $("#SalesArea").attr('value', true);
        $("#PurshacingArea").prop("checked", true);
        $("#PurshacingArea").attr('value', true);
        $("#InventoryArea").prop("checked", true);
        $("#InventoryArea").attr('value', true);
    } else {
        $("#FinancialArea").prop("checked", false);
        $("#FinancialArea").attr('value', false);
        $("#SalesArea").prop("checked", false);
        $("#SalesArea").attr('value', false);
        $("#PurshacingArea").prop("checked", false);
        $("#PurshacingArea").attr('value', false);
        $("#InventoryArea").prop("checked", false);
        $("#InventoryArea").attr('value', false);
    }
});
$("#FinancialArea").on('change', function () {
    if ($(this).is(':checked')) {
        $(this).attr('value', true);
    } else {
        $(this).attr('value', false);
    }
});
$("#SalesArea").on('change', function () {
    if ($(this).is(':checked')) {
        $(this).attr('value', true);
    } else {
        $(this).attr('value', false);
    }
});
$("#PurshacingArea").on('change', function () {
    if ($(this).is(':checked')) {
        $(this).attr('value', true);
    } else {
        $(this).attr('value', false);
    }
});
$("#InventoryArea").on('change', function () {
    if ($(this).is(':checked')) {
        $(this).attr('value', true);
    } else {
        $(this).attr('value', false);
    }
});



// Get Account From External Excel
$("#ImportExcel").click(function () {

    $("#PUImportExcel").modal("show");

});

$("#ConfirmImport").click(function () {

    var AccountsExcel = $("#AccountsExcel").get(0).files;

    var dataa = new FormData;
    dataa.append("AccountsExcel", AccountsExcel[0]);

    var CompanyID = $("#CompanyID").text();

    var ChartAccountID = $("#ChartAccountID").text();

    $.ajax({
        type: "POST",
        url: "/C_CreateAccount/UploadExcel?CompanyID=" + CompanyID + "&ChartAccountID=" + ChartAccountID,
        data: dataa,
        cache: false,
        contentType: false,
        processData: false,
        success: function (result) {
            if (result == "success") {
                location.reload();
            } else {
                $("#UploadError").text(result);
            }
        }
    });

});


$("#AddCurrToAcc").click(function () {

    $('#dvCheckBoxListControl').html("");
    $("#CurrAccountID").val("");
    $("#CurrAccountName").val("");
    $.ajax({
        type: "GET",
        url: "/C_CreateAccount/GetAllCurrencies",
        success: function (result) {
            var table = $('<table></table>');
            var counter = 0;
            $(result).each(function () {
                table.append($('<tr></tr>').append($('<td></td>').append($('<input>').attr({
                    type: 'checkbox', name: 'chklistitem', value: this.CurrencyID, id: 'chklistitem' + counter, 'class': 'chkListItem'
                })).append(
                    $('<label class="p-1">').attr({
                        for: 'chklistitem' + counter++
                    }).text(this.ISOCode))));
            });
            $("#PUAddCurrencies").modal("show");
            $('#dvCheckBoxListControl').append(table);
        }
    });
});




$("#SortAccountID").change(function () {

    var sortType = $(this).val();


    GetCompanyAccounts(sortType);
});

function GetCompanyAccounts(sortType) {
    $.ajax({
        type: "GET",
        url: "/C_CreateAccount/GetCompanyAccounts?sortType=" + sortType,
        success: function (result) {
            $("#CurrAccountID").empty();

            if (result.length == 0) {

                $("#CurrAccountID").append($('<option/>', {
                    value: "",
                    text: "No Accounts Created To This Company"
                })
                );
            } else {

                $("#CurrAccountID").append($('<option/>', {
                    value: "",
                    text: "-Choose-"
                })
                );
                $.each(result, function (index, row) {

                    $("#CurrAccountID").append("<option value='" + row.C_AID + "'>" + row.C_AccountID + " ( " + row.AccountName + " )" + "</option>");

                });
            }
        }
    });
}






//$("#SaveCurrToAcc").click(function () {

//    var accountID = $("#CurrAccountID").val();

//    if (accountID.length === 0) {
//        $("#CurrAccountID").css("border-color", "red");
//    } else {
//        $("#CurrAccountID").css("border-color", "");

//        var allVals = [];
//        $('.chkListItem:checked').each(function () {
//            allVals.push({
//                CurrencyID: $(this).val()
//            });
//        });

//        var data = JSON.stringify({
//            Currencies: allVals
//        });

//        $.ajax({
//            contentType: 'application/json; charset=utf-8',
//            type: "POST",
//            url: "/C_CreateAccount/SaveAccountCurrencies?c_AID=" + accountID,
//            data: data,
//            success: function () {
//                $("#PUAddCurrencies").modal("hide");
//            }
//        });

//    }

//});





