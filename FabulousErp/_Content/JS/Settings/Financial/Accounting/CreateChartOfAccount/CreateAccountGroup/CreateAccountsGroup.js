//validation of all texts
$("#AccountGroupID").keyup(function () {

    var AccountGroupID = $(this).val();

    if (AccountGroupID.length === 0) {
        $(this).css("border-color", "red");
    } else {

        $(this).css("border-color", "");
    }
}).focusout(function () {

    var AccountGroupID = $(this).val();

    if (AccountGroupID.length === 0) {
        $(this).css("border-color", "red");
    } else {

        $.ajax({
            type: "GET",
            url: "/CreateAccountGroup/CheckDuplicateAccountGroupID?AccountGroupID=" + AccountGroupID,
            success: function (result) {
                if (result == "False") {
                    $("#GlobalError").text("Account Group ID Not Valid..!");
                    $("#AccountGroupID").css("border-color", "red");
                    $("#AccountGroupID").focus();
                } else {
                    $("#AccountGroupID").css("border-color", "");
                    $("#GlobalError").text("");
                }
            }
        });
    }

});

$("#AccountGroupName").keyup(function () {

    var AccountGroupName = $(this).val();

    if (AccountGroupName.length === 0) {
        $(this).css("border-color", "red");
    } else {
        $(this).css("border-color", "");
    }
}).focusout(function () {

    var AccountGroupName = $(this).val();

    if (AccountGroupName.length === 0) {
        $(this).css("border-color", "red");
    } else {
        $(this).css("border-color", "");
    }
});

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
//----------------------------------------------------------------------------


// on change of chart id to get name and formate of it if not usef
$("#ChartOfAccountID").change(function () {

    var ChartOfAccountID = $(this).val();

    $("#AddError").text("");
    $("#AccountName").val("");
    $("#AccountsInfoTbl").html("");
    $("#AccountsInfoDBTbl").html("");
    $("#ChartFormat").text("");

    if (ChartOfAccountID.length === 0) {

        $(this).css("border-color", "red");
        $("#ChartOfAccountName").text("");
        $("#ChartFormat").text("");
        $("#AccountInformation").hide();

    } else {

        GetViewOfChart(ChartOfAccountID);
        $("#AccountGroupID").prop("disabled", false);
        $("#AccountGroupID").val("");
        $("#AccountGroupID").css("border-color", "");
        $("#AccountGroupName").prop("disabled", false);
        $("#AccountGroupName").val("");
        $("#AccountGroupName").css("border-color", "");

        $.ajax({

            type: "GET",
            url: "/CreateAccountGroup/GetAccountChartName?AccountChartID=" + ChartOfAccountID,
            success: function (result) {

                $("#ChartOfAccountName").text(result);

            }
        });

        $.ajax({
            type: "GET",
            url: "/CreateAccountGroup/CheckDuplicateAccountChartID?ChartOfAccountID=" + ChartOfAccountID,
            success: function (result) {

                if (result == "False") {

                    GetAccountGroupData(ChartOfAccountID);

                } else {
                    $("#ChartOfAccountID").css("border-color", "");
                    $("#GlobalError").text("");

                }
            }
        });
    }
});

function GetViewOfChart(ChartOfAccountID) {

    $("#SegmentsTexts").text("");
    $("#SegmentsTextsTo").text("");
    $("#ChartFormat").text("");
    $("#CheckLength").text("");

    $("#AccountInformation").show();

    $.ajax({
        type: "GET",
        url: "/CreateAccountGroup/GetViewOfChart?ChartOfAccountID=" + ChartOfAccountID,
        success: function (result) {

            if (result.Status == "False") {

                ChartWithoutSegments(result.Length);

            } else {

                //prepare account from inputs
                var text = '<input type="number" id="Segment#" value="" class="SFrom m-1" onkeydown = "javascript: return event.keyCode == 69 ? false : true" style="width:80px" oninput="javascript: if (this.value.length > this.maxLength) this.value = this.value.slice(0, this.maxLength);" min = "0" onkeypress = "return (event.charCode == 8 || event.charCode == 0) ? null : event.charCode >= 48 && event.charCode <= 57"/>';

                //prepare account to inputs
                var textTo = '<input type="number" id="SegmentTo#" value="" class="STo m-1" onkeydown = "javascript: return event.keyCode == 69 ? false : true" style="width:80px" oninput="javascript: if (this.value.length > this.maxLength) this.value = this.value.slice(0, this.maxLength);" min = "0" onkeypress = "return (event.charCode == 8 || event.charCode == 0) ? null : event.charCode >= 48 && event.charCode <= 57"/>';

                var formateInstructions = '<label id="InstFormate#"></label>' // instruction formate

                //prepare label of seperator between inputs
                var Separator = "<label> S# </label>";


                for (var i = 0; i < result.length; i++) {
                    //Append from texts with generate id
                    $("#SegmentsTexts").append(text.replace('#', i + 1));

                    //Append to texts with generate id
                    $("#SegmentsTextsTo").append(textTo.replace('#', i + 1));

                    // instruction formate
                    var str = " * ";

                    $("#ChartFormat").append(formateInstructions.replace('#', i + 1));

                    $("#InstFormate" + parseFloat(i + 1)).text(str.repeat(parseInt(result[i].Length)));
                    //------------------------------------------------------------------------------------


                    if (i < result[i].NumberOfSegment - 1) {
                        //put seperator between texts
                        $("#Segment" + parseFloat(i + 1)).after('<label id=GetSeparatorFrom' + parseFloat(i + 1) + '>' + result[i].Seperator + '</label>');
                        $("#SegmentTo" + parseFloat(i + 1)).after('<label id=GetSeparatorTo' + parseFloat(i + 1) + '>' + result[i].Seperator + '</label>');

                        $("#InstFormate" + parseFloat(i + 1)).after(Separator.replace('S#', result[i].Seperator)); // instruction formate
                    }
                    //secify max length of segments
                    $("#Segment" + parseFloat(i + 1)).attr('maxlength', parseInt(result[i].Length));
                    $("#SegmentTo" + parseFloat(i + 1)).attr('maxlength', parseInt(result[i].Length));

                    // check length for add btn
                    $("#CheckLength").append('<label id=SLength' + parseFloat(i + 1) + '>' + parseInt(result[i].Length) + '</label>');

                }
            }
        }
    });
}


function ChartWithoutSegments(Length) {

    var textNon = '<input type="number" id="SegmentNon" value="" class="m-1" onkeydown = "javascript: return event.keyCode == 69 ? false : true" oninput="javascript: if (this.value.length > this.maxLength) this.value = this.value.slice(0, this.maxLength);" min = "0" onkeypress = "return (event.charCode == 8 || event.charCode == 0) ? null : event.charCode >= 48 && event.charCode <= 57"/>';

    var textNonTo = '<input type="number" id="SegmentToNon" value="" class="m-1" onkeydown = "javascript: return event.keyCode == 69 ? false : true" oninput="javascript: if (this.value.length > this.maxLength) this.value = this.value.slice(0, this.maxLength);" min = "0" onkeypress = "return (event.charCode == 8 || event.charCode == 0) ? null : event.charCode >= 48 && event.charCode <= 57"/>';

    var formateInstructions = '<label id="InstFormateNon"></label>'

    $("#SegmentsTexts").append(textNon);

    $("#SegmentsTextsTo").append(textNonTo);

    $("#SegmentNon").attr('maxlength', parseInt(Length));
    $("#SegmentToNon").attr('maxlength', parseInt(Length));

    var str = " * ";

    $("#ChartFormat").append(formateInstructions);

    $("#InstFormateNon").text(str.repeat(parseInt(Length)));

    $("#CheckLength").text(Length);

}


function GetAccountGroupData(ChartOfAccountID) {

    $.ajax({
        type: "GET",
        url: "/CreateAccountGroup/GetAccountGroupData?ChartOfAccountID=" + ChartOfAccountID,
        success: function (result) {
            $("#AccountGroupID").prop("disabled", true);
            $("#AccountGroupID").val(result.AccountGroupID);
            $("#AccountGroupName").prop("disabled", true);
            $("#AccountGroupName").val(result.AccountGroupName);

            GetGroupContent(result.AccountGroupID);


        }
    });

}


function GetGroupContent(AccountGroupID) {

    var tableData = $("#AccountsInfoTbl");
    tableData.html("");

    $.ajax({
        type: "GET",
        url: "/CreateAccountGroup/GetGroupContent?AccountGroupID=" + AccountGroupID,
        success: function (result) {
            for (var i = 0; i < result.length; i++) {

                var accountItem = "<tr class='row_" + result[i].AccountName + "'>" +
                    "<td id='AccountNameTbl'>" + result[i].AccountName + "</td>" +
                    "<td>" + result[i].AccountFromWithSep + "</td>" +
                    "<td>" + result[i].AccountToWithSep + "</td>" +
                    "<td>" + '<button type="button" id="DeleteBeforeSave"  class="btn btn-danger" onclick="DeleteAccountSegment(\'' + result[i].AccountName + '\')"><span class="fa fa-trash-o"></span></button>' + "</td>" +
                    "</tr>";
                tableData.append(accountItem);
            }
        }
    });

}
//------------------------------------------------------------------


// Add btn to database
$("#AddAccountBtn").click(function () {

    var Test = true;
    // Custom validation of all
    var AccountGroupID = $("#AccountGroupID").val();

    if (AccountGroupID.length === 0) {
        $("#AccountGroupID").css("border-color", "red");
        Test = false;
    } else {
        $("#AccountGroupID").css("border-color", "");
    }

    var AccountGroupName = $("#AccountGroupName").val();

    if (AccountGroupName.length === 0) {
        $("#AccountGroupName").css("border-color", "red");
        Test = false;
    } else {
        $("#AccountGroupName").css("border-color", "");
    }

    var AccountName = $("#AccountName").val();

    if (AccountName.length === 0) {
        $("#AccountName").css("border-color", "red");
        Test = false;
    } else {
        $("#AccountName").css("border-color", "");
    }

    var ChartOfAccountID = $("#ChartOfAccountID").val();

    if (ChartOfAccountID.length === 0) {
        $("#ChartOfAccountID").css("border-color", "red");
        Test = false;
    } else {
        $("#ChartOfAccountID").css("border-color", "");
    }
    //-----------------------------------------------------------------


    //check what numbet of text segment exist and collect it one with separate to show and other to database
    var GetSeparatorFrom1 = $("#GetSeparatorFrom1").text();
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
            sWithSep1 = Segment1 + GetSeparatorFrom1;
        }
    }

    var GetSeparatorFrom2 = $("#GetSeparatorFrom2").text();
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
            sWithSep2 = Segment2 + GetSeparatorFrom2;
        }
    }

    var GetSeparatorFrom3 = $("#GetSeparatorFrom3").text();
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
            sWithSep3 = Segment3 + GetSeparatorFrom3;
        }
    }

    var GetSeparatorFrom4 = $("#GetSeparatorFrom4").text();
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
            sWithSep4 = Segment4 + GetSeparatorFrom4;
        }
    }

    var GetSeparatorFrom5 = $("#GetSeparatorFrom5").text();
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
            sWithSep5 = Segment5 + GetSeparatorFrom5;
        }
    }

    var GetSeparatorFrom6 = $("#GetSeparatorFrom6").text();
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
            sWithSep6 = Segment6 + GetSeparatorFrom6;
        }
    }

    var GetSeparatorFrom7 = $("#GetSeparatorFrom7").text();
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
            sWithSep7 = Segment7 + GetSeparatorFrom7;
        }
    }

    var GetSeparatorFrom8 = $("#GetSeparatorFrom8").text();
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
            sWithSep8 = Segment8 + GetSeparatorFrom8;
        }
    }

    var GetSeparatorFrom9 = $("#GetSeparatorFrom9").text();
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
            sWithSep9 = Segment9 + GetSeparatorFrom9;
        }
    }

    var GetSeparatorFrom10 = $("#GetSeparatorFrom10").text();
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
            sWithSep10 = Segment10 + GetSeparatorFrom10;
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




    var GetSeparatorTo1 = $("#GetSeparatorTo1").text();
    var sWithSepTo1 = "";
    var SegmentTo1 = "";
    if ($("#SegmentTo1").length > 0) {

        SegmentTo1 = $("#SegmentTo1").val();

        var SLength1 = $("#SLength1").text();

        if (SegmentTo1.length != SLength1) {

            $("#SegmentTo1").css("border", "1px solid red");
            Test = false;

        } else {
            $("#SegmentTo1").css("border", "");
            sWithSepTo1 = SegmentTo1 + GetSeparatorTo1;
        }
    }

    var GetSeparatorTo2 = $("#GetSeparatorTo2").text();
    var sWithSepTo2 = "";
    var SegmentTo2 = "";
    if ($("#SegmentTo2").length > 0) {

        SegmentTo2 = $("#SegmentTo2").val();

        var SLength2 = $("#SLength2").text();

        if (SegmentTo2.length != SLength2) {

            $("#SegmentTo2").css("border", "1px solid red");
            Test = false;

        } else {
            $("#SegmentTo2").css("border", "");
            sWithSepTo2 = SegmentTo2 + GetSeparatorTo2;
        }
    }

    var GetSeparatorTo3 = $("#GetSeparatorTo3").text();
    var sWithSepTo3 = "";
    var SegmentTo3 = "";
    if ($("#SegmentTo3").length > 0) {

        SegmentTo3 = $("#SegmentTo3").val();

        var SLength3 = $("#SLength3").text();

        if (SegmentTo3.length != SLength3) {

            $("#SegmentTo3").css("border", "1px solid red");
            Test = false;

        } else {
            $("#SegmentTo3").css("border", "");
            sWithSepTo3 = SegmentTo3 + GetSeparatorTo3;
        }
    }

    var GetSeparatorTo4 = $("#GetSeparatorTo4").text();
    var sWithSepTo4 = "";
    var SegmentTo4 = "";
    if ($("#SegmentTo4").length > 0) {

        SegmentTo4 = $("#SegmentTo4").val();

        var SLength4 = $("#SLength4").text();

        if (SegmentTo4.length != SLength4) {

            $("#SegmentTo4").css("border", "1px solid red");
            Test = false;

        } else {
            $("#SegmentTo4").css("border", "");
            sWithSepTo4 = SegmentTo4 + GetSeparatorTo4;

        }
    }

    var GetSeparatorTo5 = $("#GetSeparatorTo5").text();
    var sWithSepTo5 = "";
    var SegmentTo5 = "";
    if ($("#SegmentTo5").length > 0) {

        SegmentTo5 = $("#SegmentTo5").val();

        var SLength5 = $("#SLength5").text();

        if (SegmentTo5.length != SLength5) {

            $("#SegmentTo5").css("border", "1px solid red");
            Test = false;

        } else {
            $("#SegmentTo5").css("border", "");
            sWithSepTo5 = SegmentTo5 + GetSeparatorTo5;
        }
    }

    var GetSeparatorTo6 = $("#GetSeparatorTo6").text();
    var sWithSepTo6 = "";
    var SegmentTo6 = "";
    if ($("#SegmentTo6").length > 0) {

        SegmentTo6 = $("#SegmentTo6").val();

        var SLength6 = $("#SLength6").text();

        if (SegmentTo6.length != SLength6) {

            $("#SegmentTo6").css("border", "1px solid red");
            Test = false;

        } else {
            $("#SegmentTo6").css("border", "");
            sWithSepTo6 = SegmentTo6 + GetSeparatorTo6;
        }
    }

    var GetSeparatorTo7 = $("#GetSeparatorTo7").text();
    var sWithSepTo7 = "";
    var SegmentTo7 = "";
    if ($("#SegmentTo7").length > 0) {

        SegmentTo7 = $("#SegmentTo7").val();

        var SLength7 = $("#SLength7").text();

        if (SegmentTo7.length != SLength7) {

            $("#SegmentTo7").css("border", "1px solid red");
            Test = false;

        } else {
            $("#SegmentTo7").css("border", "");
            sWithSepTo7 = SegmentTo7 + GetSeparatorTo7;
        }
    }

    var GetSeparatorTo8 = $("#GetSeparatorTo8").text();
    var sWithSepTo8 = "";
    var SegmentTo8 = "";
    if ($("#SegmentTo8").length > 0) {

        SegmentTo8 = $("#SegmentTo8").val();

        var SLength8 = $("#SLength8").text();

        if (SegmentTo8.length != SLength8) {

            $("#SegmentTo8").css("border", "1px solid red");
            Test = false;

        } else {
            $("#SegmentTo8").css("border", "");
            sWithSepTo8 = SegmentTo8 + GetSeparatorTo8;
        }
    }

    var GetSeparatorTo9 = $("#GetSeparatorTo9").text();
    var sWithSepTo9 = "";
    var SegmentTo9 = "";
    if ($("#SegmentTo9").length > 0) {

        SegmentTo9 = $("#SegmentTo9").val();

        var SLength9 = $("#SLength9").text();

        if (SegmentTo9.length != SLength9) {

            $("#SegmentTo9").css("border", "1px solid red");
            Test = false;

        } else {
            $("#SegmentTo9").css("border", "");
            sWithSepTo9 = SegmentTo9 + GetSeparatorTo9;
        }
    }

    var GetSeparatorTo10 = $("#GetSeparatorTo10").text();
    var sWithSepTo10 = "";
    var SegmentTo10 = "";
    if ($("#SegmentTo10").length > 0) {

        SegmentTo10 = $("#SegmentTo10").val();

        var SLength10 = $("#SLength10").text();

        if (SegmentTo10.length != SLength10) {

            $("#SegmentTo10").css("border", "1px solid red");
            Test = false;

        } else {
            $("#SegmentTo10").css("border", "");
            sWithSepTo10 = SegmentTo10 + GetSeparatorTo10;
        }
    }

    var SegmentToNon = "";
    if ($("#SegmentToNon").length > 0) {

        SegmentToNon = $("#SegmentToNon").val();

        var CheckLength = $("#CheckLength").text();

        if (SegmentToNon.length != CheckLength) {
            $("#SegmentToNon").css("border", "1px solid red");
            Test = false;
        } else {
            $("#SegmentToNon").css("border", "");
        }
    }
    //-----------------------------------------------------------------------

    var totalSegmentFrom = Segment1 + Segment2 + Segment3 + Segment4 + Segment5 + Segment6 + Segment7 + Segment8 + Segment9 + Segment10 + SegmentNon;

    var totalSegmentTo = SegmentTo1 + SegmentTo2 + SegmentTo3 + SegmentTo4 + SegmentTo5 + SegmentTo6 + SegmentTo7 + SegmentTo8 + SegmentTo9 + SegmentTo10 + SegmentToNon;

    if (totalSegmentTo <= totalSegmentFrom) {

        $("#AddError").text(" 'Account To'  must be greater than  'Account From' ..!");
        Test = false;
    } else {
        $("#AddError").text("");
    }

    if (Test === true) {

        var testDuplicate = true;

        $("#AccountsSegmentsTbl #AccountNameTbl").each(function () {
            var tdContent = $(this).text();
            $(this).css("border", "");
            $("#AccountName").css("border", "");

            if (tdContent == AccountName) {
                testDuplicate = false;
                $(this).css("border", "1px solid red");
                $("#AccountName").css("border", "1px solid red");
            }

        });

        if (testDuplicate === true) {

            var totalSegmentFromWithSep = sWithSep1 + sWithSep2 + sWithSep3 + sWithSep4 + sWithSep5 + sWithSep6 + sWithSep7 + sWithSep8 + sWithSep9 + sWithSep10 + SegmentNon;

            var totalSegmentToWithSep = sWithSepTo1 + sWithSepTo2 + sWithSepTo3 + sWithSepTo4 + sWithSepTo5 + sWithSepTo6 + sWithSepTo7 + sWithSepTo8 + sWithSepTo9 + sWithSepTo10 + SegmentToNon;

            $.ajax({
                type: "POST",
                url: "/CreateAccountGroup/SaveAccountGroup?AccountGroupID=" + AccountGroupID + "&AccountGroupName=" + AccountGroupName + "&ChartOfAccountID=" + ChartOfAccountID + "&AccountName=" + AccountName + "&AccountFrom=" + totalSegmentFrom + "&AccountTo=" + totalSegmentTo + "&AccountFromWithSep=" + totalSegmentFromWithSep + "&AccountToWithSep=" + totalSegmentToWithSep,
                success: function (result) {
                    if (result == "True") {
                        GetAccountGroupData(ChartOfAccountID);
                        $("#AccountName").val("");
                        ClearSegments();
                    } else if (result === "FFalse") {
                        $("#AddError").text("Account From Overlapping with another Account..!");
                    } else if (result === "TFalse") {
                        $("#AddError").text("Account To Overlapping with another Account..!");
                    }
                }
            });
        }
    }
});

function ClearSegments() {
    $("#Segment1").val("");
    $("#Segment2").val("");
    $("#Segment3").val("");
    $("#Segment4").val("");
    $("#Segment5").val("");
    $("#Segment6").val("");
    $("#Segment7").val("");
    $("#Segment8").val("");
    $("#Segment9").val("");
    $("#Segment10").val("");
    $("#SegmentTo1").val("");
    $("#SegmentTo2").val("");
    $("#SegmentTo3").val("");
    $("#SegmentTo4").val("");
    $("#SegmentTo5").val("");
    $("#SegmentTo6").val("");
    $("#SegmentTo7").val("");
    $("#SegmentTo8").val("");
    $("#SegmentTo9").val("");
    $("#SegmentTo10").val("");

}



function DeleteAccountSegment(AccountName) {

    var AccountGroupID = $("#AccountGroupID").val();

    $("#AccountNameH").text(AccountName);
    $("#AccountGroupIDH").text(AccountGroupID);

    $("#DeleteError").text("");

    $("#DeleteConfirmation").modal("show");
}


$("#ConfirmDeletebtn").click(function () {

    var AccountName = $("#AccountNameH").text();
    var AccountGroupID = $("#AccountGroupIDH").text();
    var ChartOfAccountID = $("#ChartOfAccountID").val();

    $.ajax({
        type: "POST",
        url: "/CreateAccountGroup/RemoveAccountsGroup?AccountName=" + AccountName + "&AccountGroupID=" + AccountGroupID + "&ChartOfAccountID=" + ChartOfAccountID,
        success: function (result) {
            if (result == "True") {
                $(".row_" + AccountName).remove();
                $("#DeleteConfirmation").modal("hide");
            } else if (result == "False") {
                $("#DeleteError").text("This Account Group Used You Can not Delete It..!");
            }
        }
    });

});
//---------------------------------------------------------------------


$('#Reset').click(function () {
    location.reload();
});
//----------------------------------------------------------