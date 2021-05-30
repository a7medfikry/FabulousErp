var regRemoveCurrFormate = /[^\d.]/g,
    companyID = $('#SCY-companyID').text(),
    requiredDecimalNum = 1,
    headerObj = {},
    //headerObjOfEnd = {},
    mainArr = [],
    mainArrOfEnd = [],
    startYearID = '',
    testPLAccounts = true,
    testBallanceSheetAccounts = true;

$(document).ready(function () {

    $.ajax({
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        method: "get",
        url: "/api/TransactionApi/GetCurrencyFormate?companyID=" + companyID,
        success: function (result) {
            requiredDecimalNum = result.DecimalNumber;
            $("#SCY-GurrencyFormate").maskMoney({ allowNegative: true, prefix: '' + result.Prefix + '', suffix: '' + result.Suffix + '', thousands: '' + result.Thousands + '', decimal: '' + result.Decimal + '', precision: parseInt(result.DecimalNumber) });
        }
    });

    $("#SCY-YearID").change(function () {

        var YearID = $(this).val();

        SCY_ClearRecords();

        if (YearID.length === 0) {
            $(this).css("border-color", "red");
        } else {
            $(this).css("border-color", "");
        }

    });

    $('#SCY-preview').click(function () {
        headerObj = {};
        //headerObjOfEnd = {};
        mainArr = [];
        mainArrOfEnd = [];

        SCY_ClearRecords();

        var yearID = $('#SCY-YearID').val(),
            yearName = $('#SCY-YearID option:selected').text(),
            bsAccount = $('#SCY-bsAccountID').val(),
            test = true;

        if (yearID.length === 0) {
            $('#SCY-YearID').css('border-color', 'red');
            test = false;
        } else {
            $('#SCY-YearID').css('border-color', '');
        }

        if (bsAccount.length === 0) {
            $('#SCY-bsAccountID').css('border-color', 'red');
            test = false;
        } else {
            $('#SCY-bsAccountID').css('border-color', '');
        }

        if (test === true) {
            $.ajax({
                type: "get",
                url: "/CloseFiscalYear/CheckNotSavedBatches?yearID=" + yearID + "&AID=" + bsAccount,
                success: function (result) {
                    if (result === false) {
                        $('#SCY-PreviewError').text('Please Post Saved Batches');
                    } else if (result === "NotNewYear") {
                        $('#SCY-CloseError').text("Not Found a new Year Created..!");
                    } else if (result === "ClosedYear") {
                        $('#SCY-CloseError').text("You Can't Close This Year , You Should Close Last Year First");

                    } else {
                        $('#SCY-tbl').show();
                        $('#SCY-BStbl').show();
                        $('#SCY-CloseYearBtn').show();

                        headerObj = {
                            C_CBID: '',
                            C_PostingDate: result.HeaderData.LastDayInYear,
                            C_TransactionDate: result.HeaderData.LastDayInYear,
                            C_Refrence: "Close Year " + yearName + "",
                            CurrencyID: companyID,
                            C_SystemRate: 1,
                            C_TransactionRate: 1,
                            C_PostingKey: 'SCY',
                            C_TransactionType: 'Close Fiscal Year'
                        };

                        //headerObjOfEnd = {
                        //    C_CBID: '',
                        //    C_PostingDate: result.HeaderData.FirstDayInNextYear,
                        //    C_TransactionDate: result.HeaderData.FirstDayInNextYear,
                        //    C_Refrence: "Open Year " + result.HeaderData.NextYearName + "",
                        //    CurrencyID: companyID,
                        //    C_SystemRate: 1,
                        //    C_TransactionRate: 1,
                        //    C_PostingKey: 'SCY',
                        //    C_TransactionType: 'Close Fiscal Year'
                        //};

                        startYearID = result.HeaderData.NextYearID;
                        var sumPLDebit = 0;
                        var sumPLCredit = 0;
                        if (result.PLAccounts.length > 0) {
                            let sumDebit = 0,
                                sumCredit = 0;
                            testPLAccounts = true;
                            $(result.PLAccounts).each(function (i, el) {

                                if (el.Ballance !== 0) {

                                    var debit = 0,
                                        credit = 0;

                                    if (el.Ballance > 0) {
                                        debit = el.Ballance;
                                        sumDebit += el.Ballance;
                                    } else {
                                        credit = Math.abs(el.Ballance);
                                        sumCredit += Math.abs(el.Ballance);
                                    }

                                    var data = "<tr>"
                                        + '<td>' + el.AccountID + '</td>' +
                                        '<td>' + el.AccountName + '</td>' +
                                        '<td>' + SCY_setSystemCurrFormate(+parseFloat(debit)) + '</td>' +
                                        '<td>' + SCY_setSystemCurrFormate(+parseFloat(credit)) + '</td>' +
                                        '</tr>';
                                    $('#SCY-tblBody').append(data);
                                    sumPLDebit = +parseFloat(sumDebit);
                                    sumPLCredit = +parseFloat(sumCredit);
                                    $("#SCY-DebitTblFoot").text(SCY_setSystemCurrFormate(+parseFloat(sumDebit)));
                                    $("#SCY-CreditTblFoot").text(SCY_setSystemCurrFormate(+parseFloat(sumCredit)));
                                    $("#SCY-DiffOfDAC").text(SCY_setSystemCurrFormate(+parseFloat(sumDebit - sumCredit)));

                                    mainArr.push({
                                        C_Describtion: "Close Year " + yearName + "",
                                        C_Document: "UnS",
                                        C_AID: el.AID,
                                        C_OriginalAmount: parseFloat(debit) + parseFloat(credit),
                                        C_Debit: credit,
                                        C_Credit: debit
                                    });

                                    mainArr.push({
                                        C_Describtion: "Close Year " + yearName + "",
                                        C_Document: "UnS",
                                        C_AID: bsAccount,
                                        C_OriginalAmount: parseFloat(debit) + parseFloat(credit),
                                        C_Debit: debit,
                                        C_Credit: credit
                                    });
                                }
                            });
                        } else {
                            testPLAccounts = false;
                            $('#SCY-tblBody').append('<tr><td colspan="4" class="text-danger font-weight-bold">Not Exist Data Of P&L Accounts..!</td></tr>');
                        }

                        if (result.BallanceSheetAccounts.length > 0) {
                            var PLAccountsCount = $('#SCY-tbl > tbody > tr').length;
                            let sumDebit = 0,
                                sumCredit = 0,
                                ballanceSheetAccountBallance,
                                //ballanceSheetAccountBallance = parseFloat($('#SCY-DebitTblFoot').text().replace(regRemoveCurrFormate, '')) - parseFloat($('#SCY-CreditTblFoot').text().replace(regRemoveCurrFormate, '')),
                                checkBallanceSheetAccounts = false;

                            if (PLAccountsCount > 0) {
                                ballanceSheetAccountBallance = parseFloat($('#SCY-DebitTblFoot').text().replace(regRemoveCurrFormate, '')) - parseFloat($('#SCY-CreditTblFoot').text().replace(regRemoveCurrFormate, ''));
                            } else {
                                ballanceSheetAccountBallance = 0;
                            }

                            testBallanceSheetAccounts = true;

                            $(result.BallanceSheetAccounts).each(function (i, el) {

                                var ballance = el.Ballance;
                                
                                if (el.AID == bsAccount && ballance!=0) {
                                    checkBallanceSheetAccounts = true;
                                    ballance = parseFloat(ballance) + parseFloat(ballanceSheetAccountBallance);
                                }

                                if (ballance !== 0) {

                                    var debit = 0,
                                        credit = 0;

                                    if (ballance > 0) {
                                        debit = ballance;
                                        sumDebit += ballance;
                                    } else {
                                        credit = Math.abs(ballance);
                                        sumCredit += Math.abs(ballance);
                                    }
                                    if (el.AID == $("#SCY-bsAccountID option:selected").val()) {
                                        //K Update
                                        if (sumPLDebit - sumPLCredit > 0) {
                                            debit = parseFloat(debit) + (parseFloat(sumPLDebit) - parseFloat(sumPLCredit))
                                        } else {
                                            sumCredit -= credit;
                                            credit = parseFloat(credit) - (parseFloat(sumPLDebit) - parseFloat(sumPLCredit))
                                            sumCredit += credit;
                                        }
                                        //K Update
                                    }
                                    var data = "<tr>"
                                        + '<td>' + el.AccountID + '</td>' +
                                        '<td>' + el.AccountName + '</td>' +
                                        '<td>' + SCY_setSystemCurrFormate(+parseFloat(debit)) + '</td>' +
                                        '<td>' + SCY_setSystemCurrFormate(+parseFloat(credit)) + '</td>' +
                                        '<td class="hide-normal">' + el.AID + '</td>' +
                                        '</tr>';
                                    $('#SCY-BStblBody').append(data);
                                }
                            });

                            if (checkBallanceSheetAccounts === false)
                            {

                                let debit = 0,
                                    credit = 0;

                                if (ballanceSheetAccountBallance > 0) {
                                    debit = ballanceSheetAccountBallance;
                                    sumDebit += ballanceSheetAccountBallance;
                                } else {
                                    credit = Math.abs(ballanceSheetAccountBallance);
                                    sumCredit += Math.abs(ballanceSheetAccountBallance);
                                }

                                if (bsAccount == $("#SCY-bsAccountID option:selected").val()) {
                                    //K Update
                                    if (sumPLDebit - sumPLCredit > 0) {
                                        debit = parseFloat(sumPLDebit) - parseFloat(sumPLCredit)
                                    } else {
                                        credit = -(parseFloat(sumPLDebit) - parseFloat(sumPLCredit))
                                        sumCredit += credit;
                                    }
                                    //K Update
                                }
                                var data = "<tr>"
                                    + '<td>' + result.HeaderData.AccountID + '</td>' +
                                    '<td>' + result.HeaderData.AccountName + '</td>' +
                                    '<td>' + SCY_setSystemCurrFormate(+parseFloat(debit)) + '</td>' +
                                    '<td>' + SCY_setSystemCurrFormate(+parseFloat(credit)) + '</td>' +
                                    '<td class="hide-normal">' + bsAccount + '</td>' +
                                    '</tr>';
                                $('#SCY-BStblBody').append(data);
                            }


                            //K Update
                            //if (sumPLDebit > 0) {
                            //    sumDebit = parseFloat(sumDebit) //+ parseFloat(sumPLDebit)
                            //} else {
                            //    sumCredit = parseFloat(sumCredit) //+ parseFloat(sumPLDebit)
                            //}
                            sumDebit = 0;
                            sumCredit = 0;
                            if ($(document).find("#Masktest").length == 0) {
                                $("body").append("<input type='hidden' id='Masktest'>");
                            }
                            $("#SCY-BStblBody").find("tr").each(function () {
                                $(document).find("#Masktest").val($(this).find("td:eq(2)").text());
                                var ThisDebit = $(document).find("#Masktest").maskMoney('unmasked')[0];

                                $(document).find("#Masktest").val($(this).find("td:eq(3)").text());
                                var ThisCredit = $(document).find("#Masktest").maskMoney('unmasked')[0];
                               
                                sumDebit += parseFloat(ThisDebit)
                                sumCredit += parseFloat(ThisCredit)
                            });
                            //K Update
                            $("#SCY-BSDebitTblFoot").text(SCY_setSystemCurrFormate(+parseFloat(sumDebit)));
                            $("#SCY-BSCreditTblFoot").text(SCY_setSystemCurrFormate(+parseFloat(sumCredit)));
                            $("#SCY-BSDiffOfDAC").text(SCY_setSystemCurrFormate(+parseFloat(sumDebit - sumCredit)));

                        } else {
                            $('#SCY-BStblBody').append('<tr><td colspan="4" class="text-danger font-weight-bold">Not Exist Data Of Ballance Sheet Accounts..!</td></tr>');
                            testBallanceSheetAccounts = false;
                        }
                    }
                }
            });
        }
    });

    $("#SCY-CloseYearBtn").click(function () {

        var j = 2,
            YearID = $("#SCY-YearID").val();

        $('#SCY-CloseError').text("");

        var Test = true;

        if (YearID.length === 0) {
            $("#SCY-YearID").css("border-color", "red");
            Test = false;
        } else {
            $("#SCY-YearID").css("border-color", "");
        }

        if (Test === true) {
            $.ajax({
                type: "POST",
                url: "/CloseFiscalYear/CheckCloseYear?YearID=" + YearID,
                success: function (result) {
                    if (result === true) {
                        mainArrOfEnd = [];
                        //testPLAccounts !== false &&
                        if (testBallanceSheetAccounts !== false) {

                            PureTransactionSave(companyID, headerObj, mainArr, function (PostingNum) {
                                $.ajax({
                                    url: "/api/TransactionApi/GetTransactionData?postingNumber=" + PostingNum,
                                    method: "GET",
                                    success: function (data) {
                                        data.ShowTransactions = data.ShowGeneralLedger
                                        ManyJvAction(data);
                                        var Head = data.ShowHeader;
                                        $("#JVNumb").attr("value", PostingNum)

                                        $("#BatchId").val("")
                                        $("#CurrencyId").attr("value", Head.CurrencyID);
                                        $("#PostingDate").attr("value", Head.PostingDate)
                                        $("#Ref").attr("value", Head.Reference)
                                        $("#SysRate").attr("value", Head.SystemRate)
                                        $("#JVDate").attr("value", Head.TransactionDate)
                                        $("#TransRate").attr("value", Head.TransactionDate)
                                        PrintThis($("#OtherHeader").html(), true, false, true, function () {
                                            window.location.reload();
                                        });
                                    }
                                });
                            });

                            $('#SCY-BStbl tbody tr').each(function () {
                                mainArrOfEnd.push({
                                    C_AID: $(this).find('td:eq(4)').html(),
                                    Credit: $(this).find('td:eq(3)').html().replace(regRemoveCurrFormate, ""),
                                    Debit: $(this).find('td:eq(2)').html().replace(regRemoveCurrFormate, ""),
                                    Ballance: parseFloat($(this).find('td:eq(2)').html().replace(regRemoveCurrFormate, "")) - parseFloat($(this).find('td:eq(3)').html().replace(regRemoveCurrFormate, "")),
                                    YearID: YearID,
                                    Type: 1,
                                });
                            });


                            $('#SCY-BStbl tbody tr').each(function () {
                                mainArrOfEnd.push({
                                    C_AID: $(this).find('td:eq(4)').html(),
                                    Credit: $(this).find('td:eq(3)').html().replace(regRemoveCurrFormate, ""),
                                    Debit: $(this).find('td:eq(2)').html().replace(regRemoveCurrFormate, ""),
                                    Ballance: parseFloat($(this).find('td:eq(2)').html().replace(regRemoveCurrFormate, "")) - parseFloat($(this).find('td:eq(3)').html().replace(regRemoveCurrFormate, "")),
                                    YearID: startYearID,
                                    Type: 2,
                                });
                            });

                            var data = JSON.stringify({
                                SaveEndingBegining: mainArrOfEnd
                            });

                            $.ajax({
                                contentType: 'application/json; charset=utf-8',
                                dataType: 'json',
                                type: "post",
                                url: "/CloseFiscalYear/CloseYear",
                                data: data,
                                success: function (result) {
                                    console.log(result);
                                    //location.reload();
                                }
                            });
                        } else {
                            location.reload();
                        }

                    } else {
                        //console.log(result);
                        $('#SCY-CloseError').text(result);
                    }
                }
            });
        }
    });
});


function SCY_setSystemCurrFormate(val) {
    if (val === 0) {
        return 0;
    } else {

        var fixedVal = parseFloat(val).toFixed(parseInt(requiredDecimalNum));

        $('#SCY-GurrencyFormate').maskMoney('mask', parseFloat(fixedVal));

        return $('#SCY-GurrencyFormate').val();
    }
}

function SCY_ClearRecords() {
    $('#SCY-CloseError').text("");
    $('#SCY-PreviewError').text('');
    $('#SCY-tblBody').empty();
    $('#SCY-BStblBody').empty();
    $('#SCY-tbl').hide();
    $('#SCY-BStbl').hide();
    $('#SCY-CloseYearBtn').hide();
    $('#SCY-DebitTblFoot').text('');
    $('#SCY-CreditTblFoot').text('');
    $('#SCY-DiffOfDAC').text('');
    $('#SCY-BSDebitTblFoot').text('');
    $('#SCY-BSCreditTblFoot').text('');
    $('#SCY-BSDiffOfDAC').text('');
}