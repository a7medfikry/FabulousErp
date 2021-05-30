/*---- Global Variables ---- */
var firstRow_cAID, firstRow_AccountName, firstRow_AccountID,
    secondRow_cAID, secondRow_AccountName, secondRow_AccountID,
    profitHiddenID, profitAccountID, profitAccountName,
    lossHiddenID, lossAccountID, lossAccountName,
    companyID = $("#TCGE-CompanyID").text(),
    PTcheck = $("#CBT-PT").text(),
    transferNumber = $('#CBT-transferNumber').text();

$(document).ready(function () {

    var EPDcheck = $("#TCGE-EPD").text(),
        fJEPer = $("#TCGE-FJEPer").text(),
        FinalAmount = $('#transferTo').find('.CBT-amount'),
        tbody = $('#TCGE-TTbl'),
        ThisSlector;
    FinalAmount.prop('disabled', true);

    if (fJEPer === "NoPS") {
        Talert("This Company in Financial Module Not have Posting Setup..!");
        window.location.href = "/Home/Financial_Home";
    }
    $('#transferNumber').text(transferNumber);
    $('.CBT-transferDate').focus(function () {
        $('#transferTable').hide();
        var now = new Date();
        var day = ("0" + now.getDate()).slice(-2);
        var month = ("0" + (now.getMonth() + 1)).slice(-2);
        var today = now.getFullYear() + "-" + (month) + "-" + (day);
        $('.CBT-transferDate').val(today);
    });
    $('#reference').change(function () {
        $('#transferTable').hide();
    });
    $('.CBT-description').change(function () {
        $('#transferTable').hide();
    });


    // Check If Document-Number Is Exist Or Not
    $('#documentNumber').focusout(function () {
        var documentNumber = $(this).val();
        if (documentNumber) {
            $.ajax({
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                method: "get",
                url: "/api/CheckBookAPIs/DocumentNumberCheck?documentNumber=" + documentNumber + "&companyID=" + companyID,
                success: function (result) {
                    $('#documentNumber').css({
                        "border": ""
                    });
                },
                statusCode: {
                    406: function (request) {
                        $('#documentNumber').val('');
                        $('#documentNumber').css({
                            "border": "1px solid red"
                        });
                    }
                }
            });
        }
     
    }).focusin(function () {
        $('#transferTable').hide();
    });


    // Check Checkbook Security Before Get Data
    $(".CBT-checkbookID").change(function () {
        $('#transferTable').hide();
        if ($('.CBT-transferDate').val().length === 0) {
            $(this).val("");
            $('.CBT-transferDate').focus();
        } else {
            ThisSlector = $(this).parents(".MainSection");
            if ($(this).parents('#transferFrom').length > 0) {
                $('#transferTo').find('.CBT-checkbookID option').show();
                var checkbookIDFrom = $(this).val();
                $('#transferTo').find('.CBT-checkbookID option[value=' + checkbookIDFrom + ']').hide();
            } else if ($(this).parents('#transferTo').length > 0) {
                $('#transferFrom').find('.CBT-checkbookID option').show();
                var checkbookIDTo = $(this).val();
                $('#transferFrom').find('.CBT-checkbookID option[value=' + checkbookIDTo + ']').hide();
            }
            ThisSlector.find('.CBT-amount').val("");
            var checkbookID = $(this).val();
            if (checkbookID.length === 0) {
                Clear(ThisSlector);
            } else {
                $.ajax({
                    contentType: 'application/json; charset=utf-8',
                    dataType: 'json',
                    method: "get",
                    url: "/api/CheckBookAPIs/CheckbookSecurity?checkbookID=" + checkbookID + "&companyID=" + companyID,
                    success: function (result) {
                        if (result.CB_Password === "Exist") {
                            $("#CBT-CBid").text(checkbookID);
                            ThisSlector.find(".CBT-checkbookID").val("");
                            ThisSlector.find(".CBT-checkbookName").val("");
                            $("#CBT-checkbookModal").modal("show");
                        } else if (result.CB_UserID === "UserIDAccess") {
                            GetCheckbookData(checkbookID, ThisSlector);
                        } else if (result.CB_UserID === "NoPermit"
                            && window.location.pathname != "/CheckbookTransfer/Index") {
                            ThisSlector.find(".CBT-checkbookID").val("");
                            ThisSlector.find(".CBT-checkbookName").val("");
                            Talert("you not have access to this checkbook");
                        }
                        else {
                            GetCheckbookData(checkbookID, ThisSlector);
                        }
                    }
                });
            }
        }
    });
    $("#CBT-checkbookCheck").click(function () {
        var password = $("#CBT-checkbookPassword").val();
        var checkbookID = $("#CBT-CBid").text();

        $.ajax({
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            method: "get",
            url: "/api/CheckBookAPIs/CheckbookSecurityCheck?password=" + password + "&checkbookID=" + checkbookID + "&companyID=" + companyID,
            success: function (result) {
                GetCheckbookData(checkbookID, ThisSlector);
                $(ThisSlector).find(".CBT-checkbookID").val(checkbookID);
                $("#CBT-checkbookModal").modal("hide");
            },
            statusCode: {
                406: function (request) {
                    $("#CBT-wrongpass").text(JSON.parse(request.responseText).Message);
                }
            }
            /*
            error: function (request, statues, error) {
                if (request.status === 406) {
                    $(".CBT-wrongpass").text(JSON.parse(request.responseText).Message);
                }
            }
            */
        });
    });


    // Amount of Transfer-from checkbook
    $('.CBT-amount').focusout(function () {
        $('#transferTable').hide();
        ThisSlector = $(this).parents(".MainSection");
        if (ThisSlector.find('.CBT-checkbookID').val().length === 0) {
            $(this).val("");
            ThisSlector.find('.CBT-checkbookID').focus();
        } else {
            var Amount = $(this).val().replace(regRemoveCurrFormate, ""),
                TransactionRate = $('.TCGE-TransactionRate').val().replace(regRemoveCurrFormate, ""),
                MaxAmount = ThisSlector.find('#CBT-maxAmount').text(),
                MinAmount = ThisSlector.find('#CBT-minAmount').text(),
                currencyID = ThisSlector.find('.TCGE-CurrencyID').val(),
                difference = ThisSlector.find('.TCGE-DiffrenceRate').val(),
                test = true;

            if (test === true && $(this).val().length > 0) {

                var test1 = true;

                if (MaxAmount.length > 0 && MinAmount.length > 0) {
                    if (parseFloat(Amount) >= parseFloat(MinAmount) && parseFloat(Amount) <= parseFloat(MaxAmount)) {
                        $("#TCGE-GlobalError").text("");
                    } else {
                        test1 = false;
                        $(this).val("");
                        $(this).focus();
                        $("#TCGE-GlobalError").text("Checkbook Minimum Amount = " + MinAmount + " And Maximum Amount = " + MaxAmount + "")
                    }
                } else if (MaxAmount.length > 0 && MinAmount.length === 0) {

                    if (parseFloat(Amount) <= parseFloat(MaxAmount)) {
                        $("#TCGE-GlobalError").text("");
                    } else {
                        test1 = false;
                        $(this).val("");
                        $(this).focus();
                        $("#TCGE-GlobalError").text("Checkbook Maximum Amount = " + MaxAmount + "")
                    }
                } else if (MinAmount.length > 0 && MaxAmount.length === 0) {

                    if (parseFloat(Amount) >= parseFloat(MinAmount)) {
                        $("#TCGE-GlobalError").text("");
                    } else {
                        test1 = false;
                        $(this).val("");
                        $(this).focus();
                        $("#TCGE-GlobalError").text("Checkbook Minimum Amount = " + MinAmount + "")
                    }
                }
                if (test1 === true) {
                    // Get Account Profit OR Loss
                    $.ajax({
                        contentType: 'application/json; charset=utf-8',
                        dataType: 'json',
                        method: "get",
                        url: "/api/CheckBookAPIs/GetProfitLossAccount?currencyID=" + currencyID + "&companyID=" + companyID,
                        success: function (result) {
                            if (result.length > 0) {
                                $.each(result, function (key, item) {
                                    if (item.AccountType === "Profit") {
                                        profitHiddenID = item.C_AID;
                                        profitAccountID = item.Currency_AccountsID;
                                        profitAccountName = item.Company_AccountsName;
                                    } else {
                                        lossHiddenID = item.C_AID;
                                        lossAccountID = item.Currency_AccountsID;
                                        lossAccountName = item.Company_AccountsName;
                                    }
                                });
                            } else {
                                if (difference !== '0') {
                                    $('#CBT-AccountsList').modal('show');
                                }
                            }
                        }
                    });
                    CalculateAmount();
                }
            }
        }
    });


    // Calculate Difference If User Change Transaction-Rate
    $('.TCGE-TransactionRate').change(function () {
        $('#transferTable').hide();
        ThisSlector = $(this).parents(".MainSection");
        //clear
        ThisSlector.find('.CBT-amount').val('');

        var transactionRate = $(this).val().replace(regRemoveCurrFormate, ""),
            systemRate = ThisSlector.find(".TCGE-SystemRate").val().replace(regRemoveCurrFormate, ""),
            difference = ThisSlector.find('.TCGE-DiffrenceRate').val().replace(regRemoveCurrFormate, ""),
            currencyID = ThisSlector.find('.TCGE-CurrencyID').val();

        if (transactionRate >= 0) {
            $(this).css("border-color", "");
            difference = parseFloat(transactionRate) - parseFloat(systemRate);
            ThisSlector.find(".TCGE-DiffrenceRate").val(setSystemCurrFormate(+difference));
        } else {
            $(this).css("border-color", "red");
        }
    });


    // Choose Profit or Loss Account If Currency Not Has
    $('#AccountsList').change(function () {
        var hiddenAccountID = $(this).val(),
            AccountID = $(this).find('option:selected').text(),
            difference = $('#transferFrom').find('.TCGE-DiffrenceRate').val().replace(regRemoveCurrFormate, "");

        // Extract Account-Name From Account-ID
        var str = AccountID,
            n = str.lastIndexOf('-'),
            result = str.substring(n + 1);
        profitHiddenID = hiddenAccountID;
        profitAccountID = AccountID;
        profitAccountName = result;
        lossHiddenID = hiddenAccountID;
        lossAccountID = AccountID;
        lossAccountName = result;
        $('#CBT-AccountsList').modal('hide');
    });


    // Show J.V If user Want To See Table Before Transfer
    $('#transferShow').click(function (e) {
        tbody.empty();
        var isValid = true;
        // validation on transfer-From Checkbook
        $('#transferFrom input[type="text"]').not('input:disabled').each(function () {
            if ($(this).is(":visible") && $.trim($(this).val()) == '') {
                isValid = false;
                $(this).css({
                    "border": "1px solid red"
                });
            } else {
                $(this).css({
                    "border": ""
                });
            }
        });

        // validation on Transfer-To Checkbook
        if ($('#transferTo').find('.CBT-description').val() == '') {
            isValid = false;
            $('#transferTo').find('.CBT-description').css({
                "border": "1px solid red"
            });
        } else {
            $('#transferTo').find('.CBT-description').css({
                "border": ""
            });
        }

        // validation on Reference
        if ($('#reference').val() == '') {
            isValid = false;
            $('#reference').css({
                "border": "1px solid red"
            });
        } else {
            $('#reference').css({
                "border": ""
            });
        }

        if (isValid == false) {
            e.preventDefault();
        } else {
            GenerateFirstRow();
            GenerateSecondRow();
            SumDebitAndCredit();
            GenerateThirdRow();
            $('#transferTable').show();
        }
    });


    // Transfer Checkbook
    $('#transferCB').click(function (e) {
        tbody.empty();
        var isValid = true;
        // validation on transfer-From Checkbook
        $('#transferFrom input[type="text"]').not('input:disabled').each(function () {
            if ($(this).is(":visible") &&$.trim($(this).val()) == '') {
                isValid = false;
                $(this).css({
                    "border": "1px solid red"
                });
            } else {
                $(this).css({
                    "border": ""
                });
            }
        });

        // validation on Transfer-To Checkbook
        if ($('#transferTo').find('.CBT-description').val() == '') {
            isValid = false;
            $('#transferTo').find('.CBT-description').css({
                "border": "1px solid red"
            });
        } else {
            $('#transferTo').find('.CBT-description').css({
                "border": ""
            });
        }

        // validation on Reference
        if ($('#reference').val() == '') {
            isValid = false;
            $('#reference').css({
                "border": "1px solid red"
            });
        } else {
            $('#reference').css({
                "border": ""
            });
        }

        if (isValid == false) {
            e.preventDefault();
        } else {
            GenerateFirstRow();
            GenerateSecondRow();
            SumDebitAndCredit();
            GenerateThirdRow();
            SaveTransferCheckbook();
        }
    });



    $('#transferReset').click(function () {
        location.reload();
    });
















    $('#CBT-checkbookModal').on('hidden.bs.modal', function (e) {
        $(this)
            .find("input")
            .val('')
            .end()
            .find("label").text('').end();
    });
});















function GetCheckbookData(checkbookID, Selector) {
    var companyID = $("#TCGE-CompanyID").text(),
        currencyID,
        postingDate,
        tbody = $('#TCGE-TTbl');

    $.ajax({
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        method: "get",
        url: "/api/CheckBookAPIs/GetCheckbookData?checkbookID=" + checkbookID + "&companyID=" + companyID,
        success: function (result) {
            $(Selector).find("select.TCGE-CurrencyID").append("<option value='" + result.CurrencyID + "' selected>" + result.CurrencyName + "</option>");
            $(Selector).find(".CBT-checkbookName").val(result.CheckbookName);
            $(Selector).find('#CBT-minAmount').text(result.MinAmount);
            $(Selector).find('#CBT-maxAmount').text(result.MaxAmount);

            // Fill The First-Row of Transfer-From
            if ($(Selector).hasClass('transferFrom')) {
                firstRow_cAID = result.C_AID;
                firstRow_AccountName = result.Company_AccountsName;
                firstRow_AccountID = result.Company_AccountsID;
            }
            // Fill The Second-Row of Transfer-To
            if ($(Selector).hasClass('transferTo')) {
                secondRow_cAID = result.C_AID;
                secondRow_AccountName = result.Company_AccountsName;
                secondRow_AccountID = result.Company_AccountsID;
            }

            if (result.CurrencyID === companyID) {
                // so currency is main
                $(Selector).find('.TCCR-rateField').hide();
            } else {
                $(Selector).find('.TCCR-rateField').show();
            }

            // Get System and Transaction Rate
            currencyID = result.CurrencyID;
            postingDate = $('.CBT-transferDate').val();
            CheckPostingDateInPeriods(postingDate, function (checkPostingDate) {
                if (checkPostingDate !== true) {
                    $(Selector).find(".TCGE-SystemRate").val("");
                    $(Selector).find(".TCGE-TransactionRate").val("");
                    $(Selector).find(".TCGE-DiffrenceRate").val("");
                    $(Selector).find(".TCGE-TransactionRate").prop("disabled", true);
                } else {
                    if (GetCurrencyRates(currencyID, postingDate, Selector)) {
                        CalculateAmount();
                    }
                }
            });
          
        }
    });



}
function Clear(Selector) {
    $(Selector).find(".CBT-checkbookName").val("");
    $(Selector).find(".CBT-transferDate").val("");
    $(Selector).find(".CBT-postingDate").val("");
    $(Selector).find(".CBT-amount").val("");
    $(Selector).find(".TCGE-SystemRate").val("");
    $(Selector).find(".TCGE-TransactionRate").val("");
    $(Selector).find(".TCGE-DiffrenceRate").val("");
    $(Selector).find(".TCGE-reference").val("");
}
function CalculateAmount() {
    var companyID = $("#TCGE-CompanyID").text();
    var currencyCheck = $('#transferTo').find('.TCGE-CurrencyID').val();
    var Amount = $('#transferFrom').find('.CBT-amount').val().replace(regRemoveCurrFormate, ""),
        transactionRate1 = $('#transferFrom').find('.TCGE-TransactionRate').val().replace(regRemoveCurrFormate, ""),
        transactionRate2 = $('#transferTo').find('.TCGE-TransactionRate').val().replace(regRemoveCurrFormate, ""),
        finalAmount = parseFloat(Amount) * parseFloat(transactionRate1) / parseFloat(transactionRate2);
    if (currencyCheck == companyID) {
        $('#transferTo').find('.CBT-amount').val(setSystemCurrFormate(+finalAmount));
    } else {
        $('#transferTo').find('.CBT-amount').val(setHardCurrFormateTransfer(+finalAmount));
    }
}



function GenerateFirstRow() {
    var systemRate = $('#transferFrom').find('.TCGE-SystemRate').val().replace(regRemoveCurrFormate, ""),
        amount = $('#transferFrom').find('.CBT-amount').val().replace(regRemoveCurrFormate, ""),
        transactionRate = $('#transferFrom').find('.TCGE-TransactionRate').val().replace(regRemoveCurrFormate, ""),
        difference = $('#transferFrom').find('.TCGE-DiffrenceRate').val().replace(regRemoveCurrFormate, ""),
        credit,
        debit = 0,
        description = $('#transferFrom').find('.CBT-description').val(),
        tbodyData = "<tr>" +
            "<td>" + '<button type="button" class="btn btn-sm mr-1 MoreDetailsT"><span class="fa fa-eye"></span></button>' + "</td>" +
            "<td class='hide-normal'></td>" +
            "<td></td>" +
            "<td>SID</td>" +
            "<td class='hide-normal'></td>" +
            "<td class='hide-normal'></td>" +
            "<td></td>" +
            "<td class='sDebitTbl'></td>" +
            "<td class='sCreditTbl'></td>" +
            "</tr>",
        tbody = $('#TCGE-TTbl');

    var maskAmount = $('#transferFrom').find('.CBT-amount').val();

    // Calculate Credit
    credit = parseFloat(amount) * parseFloat(systemRate);

    tbody.append(tbodyData);
    // fill the first-Row with data
    tbody.find('tr').eq(0).find('td').eq(1).text(firstRow_cAID);
    tbody.find('tr').eq(0).find('td').eq(2).text(firstRow_AccountName);
    tbody.find('tr').eq(0).find('td').eq(4).text(firstRow_AccountID);
    tbody.find('tr').eq(0).find('td').eq(5).text(description);
    // tbody.find('tr').eq(0).find('td').eq(6).text(setHardCurrFormateTransfer(+parseFloat(amount)));
    tbody.find('tr').eq(0).find('td').eq(6).text(maskAmount);
    tbody.find('tr').eq(0).find('td').eq(8).text(setSystemCurrFormate(+parseFloat(credit)));
    tbody.find('tr').eq(0).find('td').eq(7).text(setSystemCurrFormate(+parseFloat(debit)));
}
function GenerateSecondRow() {
    var systemRate = $('#transferTo').find('.TCGE-SystemRate').val().replace(regRemoveCurrFormate, ""),
        amount = $('#transferTo').find('.CBT-amount').val().replace(regRemoveCurrFormate, ""),
        transactionRate = $('#transferTo').find('.TCGE-TransactionRate').val().replace(regRemoveCurrFormate, ""),
        difference = $('#transferTo').find('.TCGE-DiffrenceRate').val().replace(regRemoveCurrFormate, ""),
        debit,
        credit = 0,
        description = $('#transferTo').find('.CBT-description').val(),
        tbodyData = "<tr>" +
            "<td>" + '<button type="button" class="btn btn-sm mr-1 MoreDetailsT"><span class="fa fa-eye"></span></button>' + "</td>" +
            "<td class='hide-normal'></td>" +
            "<td></td>" +
            "<td>SID</td>" +
            "<td class='hide-normal'></td>" +
            "<td class='hide-normal'></td>" +
            "<td></td>" +
            "<td class='sDebitTbl'></td>" +
            "<td class='sCreditTbl'></td>" +
            "</tr>",
        tbody = $('#TCGE-TTbl');

    var maskAmount = $('#transferTo').find('.CBT-amount').val();

    // Calculate Credit
    debit = parseFloat(amount) * parseFloat(systemRate);

    tbody.append(tbodyData);
    // fill the Second-Row with data
    tbody.find('tr').eq(1).find('td').eq(1).text(secondRow_cAID);
    tbody.find('tr').eq(1).find('td').eq(2).text(secondRow_AccountName);
    tbody.find('tr').eq(1).find('td').eq(4).text(secondRow_AccountID);
    tbody.find('tr').eq(1).find('td').eq(5).text(description);
    //tbody.find('tr').eq(1).find('td').eq(6).text(setHardCurrFormateTransfer(+parseFloat(amount)));
    tbody.find('tr').eq(1).find('td').eq(6).text(maskAmount);
    tbody.find('tr').eq(1).find('td').eq(7).text(setSystemCurrFormate(+parseFloat(debit)));
    tbody.find('tr').eq(1).find('td').eq(8).text(setSystemCurrFormate(+parseFloat(credit)));
}
function GenerateThirdRow() {
    var debit = parseFloat($('#TCGE-TTbl').find('tr').eq(1).find('td').eq(7).text().replace(",", "")),
        credit = parseFloat($('#TCGE-TTbl').find('tr').eq(0).find('td').eq(8).text().replace(",", "")),
        difference = parseFloat(debit) - parseFloat(credit),
        description = $('#transferFrom').find('.CBT-description').val(),
        tbodyData = "<tr>" +
            "<td>" + '<button type="button" class="btn btn-sm mr-1 MoreDetailsT"><span class="fa fa-eye"></span></button>' + "</td>" +
            "<td class='hide-normal'></td>" +
            "<td></td>" +
            "<td>SID</td>" +
            "<td class='hide-normal'></td>" +
            "<td class='hide-normal'></td>" +
            "<td></td>" +
            "<td class='sDebitTbl'></td>" +
            "<td class='sCreditTbl'></td>" +
            "</tr>",
        tbody = $('#TCGE-TTbl');

    if (parseFloat(difference) === 0) {
        return;
    } else {
        tbody.append(tbodyData);

        // Fill The Third-Row
        tbody.find('tr').eq(2).find('td').eq(5).text(description);

        if (!isNaN(difference)) {
            if (parseFloat(difference) > 0) {
                // Profit Account
                tbody.find('tr').eq(2).find('td').eq(1).text(profitHiddenID);
                tbody.find('tr').eq(2).find('td').eq(2).text(profitAccountName);
                tbody.find('tr').eq(2).find('td').eq(4).text(profitAccountID);

                tbody.find('tr').eq(2).find('td').eq(7).text('');
                tbody.find('tr').eq(2).find('td').eq(8).text(setSystemCurrFormate(+parseFloat(difference)));
                tbody.find('tr').eq(2).find('td').eq(6).text(setSystemCurrFormate(+parseFloat(difference)));
            } else if (parseFloat(difference) < 0) {
                var Number = Math.abs(difference);

                //Loss Account
                tbody.find('tr').eq(2).find('td').eq(1).text(lossHiddenID);
                tbody.find('tr').eq(2).find('td').eq(2).text(lossAccountName);
                tbody.find('tr').eq(2).find('td').eq(4).text(lossAccountID);

                tbody.find('tr').eq(2).find('td').eq(8).text('');
                tbody.find('tr').eq(2).find('td').eq(7).text(setSystemCurrFormate(+parseFloat(Number)));
                tbody.find('tr').eq(2).find('td').eq(6).text(setSystemCurrFormate(+parseFloat(Number)));
            }
            SumDebitAndCredit();
        }
    }
}
function SaveTransferCheckbook() {
    // transfer-from checkbook
    var transferDate = $('.CBT-transferDate').val(),
        transferDocumentNumber = $('#documentNumber').val(),
        transferReference = $('#reference').val(),
        postingKey = "TCBT",
        first_CheckbookID = $('#transferFrom').find('.CBT-checkbookID').val(),
        first_CheckbookName = $('#transferFrom').find('.CBT-checkbookName').val(),
        first_Currency = $('#transferFrom').find('.TCGE-CurrencyID').val(),
        first_SystemRate = $('#transferFrom').find('.TCGE-SystemRate').val().replace(regRemoveCurrFormate, ""),
        first_TransactionRate = $('#transferFrom').find('.TCGE-TransactionRate').val().replace(regRemoveCurrFormate, ""),
        first_Difference = $('#transferFrom').find('.TCGE-DiffrenceRate').val().replace(regRemoveCurrFormate, ""),
        first_Description = $('#transferFrom').find('.CBT-description').val(),
        first_Amount = $('#transferFrom').find('.CBT-amount').val().replace(regRemoveCurrFormate, ""),
        // transfer-to checkbook
        second_CheckbookID = $('#transferTo').find('.CBT-checkbookID').val(),
        second_CheckbookName = $('#transferTo').find('.CBT-checkbookName').val(),
        second_Currency = $('#transferTo').find('.TCGE-CurrencyID').val(),
        second_SystemRate = $('#transferTo').find('.TCGE-SystemRate').val().replace(regRemoveCurrFormate, ""),
        second_TransactionRate = $('#transferTo').find('.TCGE-TransactionRate').val().replace(regRemoveCurrFormate, ""),
        second_Difference = $('#transferTo').find('.TCGE-DiffrenceRate').val().replace(regRemoveCurrFormate, ""),
        second_Description = $('#transferTo').find('.CBT-description').val(),
        final_Amount = $('#transferTo').find('.CBT-amount').val().replace(regRemoveCurrFormate, "");

    // Check if SPS Posting to General Ladger OR Posting throw General Ladger
    var PTOR;

    if (PTcheck === "A1") {
        PTOR = 1;
    } else {
        PTOR = 2;

    }
    InsertTransactionData(companyID, PTOR, transferDate, transferDate, transferReference, first_Currency, first_SystemRate, first_TransactionRate, postingKey, "Checkbook Transfer", "", "",
        function (journalEntryNumber,Po) {

        var transferArray = [
            {
                C_PostingNumber: Po,
                C_TransactionDate: transferDate,
                C_PostingDate: transferDate,
                C_CBSID: first_CheckbookID,
                CurrencyID: first_Currency,
                C_SystemRate: first_SystemRate,
                C_TransactionRate: first_TransactionRate,
                C_Difference: first_Difference,
                C_Reference: first_Description,
                C_DocumentType: "SID",
                C_Payment: first_Amount,
                C_Reciept: 0,
                C_Balance: 0 - first_Amount,
                C_CheckNumber: transferDocumentNumber,
                C_PostingKey: postingKey
            },
            {
                C_PostingNumber: Po,
                C_TransactionDate: transferDate,
                C_PostingDate: transferDate,
                C_CBSID: second_CheckbookID,
                CurrencyID: second_Currency,
                C_SystemRate: second_SystemRate,
                C_TransactionRate: second_TransactionRate,
                C_Difference: second_Difference,
                C_Reference: second_Description,
                C_DocumentType: "SID",
                C_Reciept: final_Amount,
                C_Payment: 0,
                C_Balance: final_Amount,
                C_CheckNumber: transferDocumentNumber,
                C_PostingKey: postingKey
            }
        ];
        transferArray = JSON.stringify({ 'transferArray': transferArray });
        RunAfterAjax(function () {
            $.ajax({
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                type: "POST",
                url: "/CheckbookTransfer/Transfer",
                data: transferArray,
                success: function (result) {
                    if (result === "True") {
                        Talert('Transfer Number = ' + transferNumber);
                        //if (PTcheck === 'A2') {
                        //    window.open(
                        //        '/C_ReportsPrint/Done?searchNumber=' + journalEntryNumber,
                        //        '_blank'
                        //    );
                        //}
                        window.open("/Inquiry_CheckbookTransactions/Transfer?PO=" + Po + "&TrsNum=" + transferNumber);
                        RedirectInt(window.location.href)
                        
                    }
                }
            });
        })

    }, null, null, null, true,false);

   
}
function setHardCurrFormateTransfer(val) {
    if (val === 0) {
        return 0;
    } else {

        var fixedVal = parseFloat(val).toFixed(parseInt(requiredDecimalNum));

        $('#TCGE-HardGurrencyFormateTransfer').maskMoney('mask', parseFloat(fixedVal));

        return $('#TCGE-HardGurrencyFormateTransfer').val();
    }
}

