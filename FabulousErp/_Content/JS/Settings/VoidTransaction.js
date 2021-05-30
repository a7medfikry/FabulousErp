$(document).ready(function () {

    const dateformat = /^\d{4}-\d{2}-\d{2}$/;

    if ($('#TCGV-CheckPostingSetup').text() === 'NoPS') {
        Talert("This Company in Financial Module Not have Posting Setup..!");
        window.location.href = "/Home/Financial_Home";
    }

    $("#TCGE-JEDate").prop('disabled', false);
    $("#TCGE-PostingDate").prop('disabled', false);

    if ($('#TCGE-EPD').text() === 'F2') {
        $("#TCGE-PostingDate").prop('disabled', true);
    }

    var companyID = $("#TCGE-CompanyID").text();

    $('#TCGV-JENum').change(function () {

        var postingNumber = $(this).val();
        var name = $(this).find('option:selected').text();
        // Extract Account-Name From Account-ID
        var str = name,
            n = str.lastIndexOf('-'),
            result = str.substring(n + 2);

        ClearData();
        $('#TCS-NoAC').text("");

        if (postingNumber.length > 0) {

            $.ajax({
                url: "/api/TransactionApi/GetTransactionData?postingNumber=" + postingNumber,
                method: "GET",
                success: function (data) {

                    //$("#TCGE-JEDate").val(data.ShowHeader.TransactionDate);
                    //$("#TCGE-PostingDate").val(data.ShowHeader.PostingDate);
                    $("#TCGE-CurrencyID").val(data.ShowHeader.CurrencyID);

                    if (data.ShowHeader.CurrencyID != companyID) {
                        $('#TCCR-rateField').show();
                        var iso = $("#TCGE-CurrencyID option:selected").text();
                        $("#TCGE-HardGurrencyFormate").maskMoney({ suffix: ' ' + iso + '', thousands: ',', decimal: '.', precision: parseInt(requiredDecimalNum) });

                        var hardCurrTest = true;
                    } else {
                        $('#TCCR-rateField').hide();
                    }

                    $("#TCGE-SystemRate").val(setSystemCurrFormate(+parseFloat(data.ShowHeader.SystemRate)));
                    $("#TCGE-TransactionRate").val(setSystemCurrFormate(+parseFloat(data.ShowHeader.TransactionRate)));
                    $("#TCGE-DiffrenceRate").val(parseFloat(setSystemCurrFormate(+parseFloat(data.ShowHeader.SystemRate - data.ShowHeader.TransactionRate))));
                    $("#TCGE-Reference").val(data.ShowHeader.Reference);

                    if (window.location.pathname != "/C_GeneralEntryTransaction/CompanyVoidTransactions" && location.pathname != "/C_CheckbookVoid/Transfer" && location.pathname != '/C_CheckbookVoid/Check') {
                        $('#CBT-checkbookName').val(data.CheckbookData.CheckbookName);
                        $('#CBT-checkbookID').val(data.CheckbookData.C_CBSID);
                        $('#CBT_receivedFrom_payTo').val(data.CheckbookData.Payment_To_Recieved_From);
                        $('#C_CBT').text(data.CheckbookData.C_CBT);
                        $('#CBT-nextDeposit').text(data.CheckbookData.NextDepositNumber);
                        $('#CBT-nextWithdraw').text(data.CheckbookData.NextWithdrawNumber);
                        $('#C_DocumentType').text(data.CheckbookData.C_DocumentType);
                        var TDate = new moment(data.CheckbookData.C_TransactionDate).format('YYYY-MM-DD');
                        $('#C_TransactionDate').text(TDate);
                        $('#C_TransactionDate').val(TDate);
                        var PDate = new moment(data.CheckbookData.C_PostingDate).format('YYYY-MM-DD');
                        $('#C_PostingDate').text(PDate);
                        $('#C_PostingDate').val(PDate);
                        $('#CBT-checkNumber').val(data.CheckbookData.C_CheckNumber);
                        $('#CBT-dueDate').val(data.CheckbookData.C_DueDate);
                        var Balance = 0;
                        if (window.location.pathname == "/Inquiry_CheckbookTransactions/CashInquiry") {
                            Balance = parseFloat(data.CheckbookData.C_Balance)
                        } else {
                            Balance = parseFloat(data.CheckbookData.C_Balance) * -1;
                        }
                        $('#CBT-amount').val(Balance);
                        $('#documentNumberName').text(result);
                    }

                    if (window.location.pathname == "/C_CheckbookVoid/Transfer" || window.location.pathname == "/Inquiry_CheckbookTransactions/Transfer" || window.location.pathname == "/Inquiry_CheckbookTransactions/TransferVoidInquiry") {
                        $('#C_TransactionDate').text(data.CheckbookData.C_TransactionDate);
                        $('#TCGE-JEDate').val(data.CheckbookData.C_TransactionDate);
                        $('#C_VoidDate').val(data.CheckbookData.C_TransactionDate);
                        if (data.ShowHeader.CurrencyID != companyID) {
                            $('.TCCR-rateField').show();
                            var iso = $(".TCGE-CurrencyID option:selected").text();
                            $(".TCGE-HardGurrencyFormate").maskMoney({ suffix: ' ' + iso + '', thousands: ',', decimal: '.', precision: parseInt(requiredDecimalNum) });
                            var hardCurrTest = true;
                        } else {
                            $('.TCCR-rateField').hide();
                        }
                        $.each(data.TransferData, function (index, res) {
                            if (index == 0) {
                                $("#TCGE-HardGurrencyFormate").maskMoney({ suffix: ' ' + res.ISO + '', thousands: ',', decimal: '.', precision: parseInt(requiredDecimalNum), allowNegative: true });
                                $('#transferFrom').find('.C_CBT').text(res.C_CBT);
                                $('#transferFrom').find('.C_DocumentType').text(res.C_DocumentType);
                                $('#transferFrom').find('.CBT-checkbookID').val(res.C_CBSID);
                                $('#transferFrom').find('.CBT-checkbookName').val(res.CheckbookName);
                                $('#transferFrom').find('.CBT-CurrencyID').val(res.CurrencyID);
                                $("#transferFrom").find('.CBT-SystemRate').val(setSystemCurrFormate(+parseFloat(res.C_SystemRate)));
                                $("#transferFrom").find('.CBT-TransactionRate').val(setSystemCurrFormate(+parseFloat(res.C_TransactionRate)));
                                $("#transferFrom").find('.CBT-DiffrenceRate').val(setSystemCurrFormate(+parseFloat(res.C_Difference)));
                                $('#transferFrom').find('.CBT-amount').val(setHardCurrFormate(+parseFloat(res.C_Balance)));
                                $('#transferFrom').find('.CBT-Reference').val(res.C_Reference);

                                $('#transferFrom').find('.paymentCheck').text(res.PaymentCheck);
                                $('#transferFrom').find('.C_Balance').text(res.C_Balance);
                            } else if (index == 1) {
                                $("#TCGE-HardGurrencyFormate").maskMoney({ suffix: ' ' + res.ISO + '', thousands: ',', decimal: '.', precision: parseInt(requiredDecimalNum), allowNegative: true });
                                $('#transferTo').find('.C_CBT').text(res.C_CBT);
                                $('#transferTo').find('.C_DocumentType').text(res.C_DocumentType);
                                $('#transferTo').find('.CBT-checkbookID').val(res.C_CBSID);
                                $('#transferTo').find('.CBT-checkbookName').val(res.CheckbookName);
                                $('#transferTo').find('.CBT-CurrencyID').val(res.CurrencyID);
                                $("#transferTo").find('.CBT-SystemRate').val(setSystemCurrFormate(+parseFloat(res.C_SystemRate)));
                                $("#transferTo").find('.CBT-TransactionRate').val(setSystemCurrFormate(+parseFloat(res.C_TransactionRate)));
                                $("#transferTo").find('.CBT-DiffrenceRate').val(setSystemCurrFormate(+parseFloat(res.C_Difference)));
                                $('#transferTo').find('.CBT-amount').val(setHardCurrFormate(+parseFloat(res.C_Balance)));
                                $('#transferTo').find('.CBT-Reference').val(res.C_Reference);

                                $('#transferTo').find('.recieptCheck').text(res.RecieptCheck);
                                $('#transferTo').find('.C_Balance').text(res.C_Balance);
                            }
                        });
                    }

                    if (window.location.pathname == '/C_CheckbookVoid/Check' || window.location.pathname == '/Inquiry_CheckbookTransactions/CheckInquiry' || window.location.pathname == '/Inquiry_CheckbookTransactions/CheckVoidInquiry') {
                        $('#TCGE-JEDate').val(data.CheckbookData.C_TransactionDate);
                        $('#C_TransactionDate').val(data.CheckbookData.C_TransactionDate);
                        $('#TCGE-PostingDate').val(data.CheckbookData.C_PostingDate);
                        $('#C_PostingDate').val(data.CheckbookData.C_PostingDate);
                        $('#C_TransactionDate').text(data.CheckbookData.C_TransactionDate);
                        $('#C_PostingDate').text(data.CheckbookData.C_TransactionDate);
                        $.each(data.TransferData, function (index, res) {
                            if (index % 2 == 0) {
                                $('#transferFrom').find('.CBT-checkbookID').val(res.C_CBSID);
                                $('#transferFrom').find('.CBT-checkbookName').val(res.CheckbookName);
                                $('#transferFrom').find('.C_CBT').append('<span>' + res.C_CBT + '</span>');
                            } else {
                                $('#transferTo').find('.CBT-checkbookID').val(res.C_CBSID);
                                $('#transferTo').find('.CBT-checkbookName').val(res.CheckbookName);
                                $('#transferTo').find('.C_CBT').append('<span>' + res.C_CBT + '</span>');
                            }
                        });
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
                        if (window.location.pathname == "/Inquiry_CheckbookTransactions/CashInquiry") {
                            RetrieveToMainTbl(data.ShowGeneralLedger[i].AID, data.ShowGeneralLedger[i].AccountName, data.ShowGeneralLedger[i].Document, data.ShowGeneralLedger[i].AccountID, data.ShowGeneralLedger[i].Describtion, data.ShowGeneralLedger[i].OriginalAmount, debit, credit , hardCurrTest, true);
                        } else {
                            RetrieveToMainTbl(data.ShowGeneralLedger[i].AID, data.ShowGeneralLedger[i].AccountName, data.ShowGeneralLedger[i].Document, data.ShowGeneralLedger[i].AccountID, data.ShowGeneralLedger[i].Describtion, data.ShowGeneralLedger[i].OriginalAmount, credit, debit, hardCurrTest, true);
                        }
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

                        RetrieveToDBAnalyticTbl(data.ShowAnalytics[i].AnalyticID, data.ShowAnalytics[i].DistID, data.ShowAnalytics[i].DistributionID, data.ShowAnalytics[i].DistributionName, data.ShowAnalytics[i].AID, data.ShowAnalytics[i].Describtion, data.ShowAnalytics[i].Percentage, data.ShowAnalytics[i].Amount, credit, debit);
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
                        RetrieveToDBCostCenterTbl(data.ShowCostCenters[i].CostCenterID, data.ShowCostCenters[i].CAID, data.ShowCostCenters[i].CostAccountID, data.ShowCostCenters[i].CostAccountName, data.ShowCostCenters[i].AID, data.ShowCostCenters[i].Describtion, data.ShowCostCenters[i].Percentage, data.ShowCostCenters[i].Amount, credit, debit, costCenterType, mainCostCenterID, costCenterIDPercentage, data.ShowCostCenters[i].CostCenterName);
                    }
                    for (let i = 0; i < data.ShowTransactions.length; i++) {

                        var debit = data.ShowTransactions[i].Debit;
                        if (data.ShowTransactions[i].Debit === 0) {
                            debit = "";
                        }

                        var credit = data.ShowTransactions[i].Credit;
                        if (data.ShowTransactions[i].Credit === 0) {
                            credit = "";
                        }
                        RetrieveToMainTbl(data.ShowTransactions[i].AID, data.ShowTransactions[i].AccountName, data.ShowTransactions[i].Document, data.ShowTransactions[i].AccountID, data.ShowTransactions[i].Describtion, data.ShowTransactions[i].OriginalAmount, credit, debit , hardCurrTest, true);
                    }
                }
            });
        }
    });


    $("#TCGE-JEDate").focusin(function () {
        $("#TCGE-PostingDate").val('');
        var today = HandleDate(new Date());
        $(this).val(today);

    }).focusout(function () {

        var JEDate = $(this).val();

        if (!JEDate.match(dateformat)) {
            $("#TCGE-JEDate").css("border-color", "red");
            $("#TCGE-GlobalError").text("Wrong Date Formate..!");
        } else {
            CheckPostingDateInPeriods(JEDate);

            $("#TCGE-PostingDate").val(JEDate);
        }
    });

    $("#TCGE-PostingDate").focusout(function () {

        var postingDate = $(this).val();

        CheckPostingDateInPeriods(postingDate);

    });

    $('#TCGV-Void').click(function () {

        var jeNum = $('#TCGV-JENum').val(),
            transactionDate = $('#TCGE-JEDate').val(),
            postingDate = $('#TCGE-PostingDate').val(),
            ptCheck = $('#TCGV-PT').text(),
            postingKey = 'TCGV',
            transactionType = 'Company Void G Transactions',
            currency = $('#TCGE-CurrencyID').val(),
            systemRate = $('#TCGE-SystemRate').val(),
            transactionRate = $('#TCGE-TransactionRate').val(),
            reference = $('#TCGE-Reference').val(),
            test = true;

        if (jeNum.length === 0) {
            $('#TCGV-JENum').css('border-color', 'red');
            test = false;
        } else {
            $('#TCGV-JENum').css('border-color', '');
        }

        if (transactionDate.length === 0) {
            $('#TCGE-JEDate').css('border-color', 'red');
            test = false;
        } else {
            $('#TCGE-JEDate').css('border-color', '');
        }
        CheckPostingDateInPeriods(postingDate, function (CheckPosting) {
            if (postingDate.length === 0) {
                $('#TCGE-PostingDate').css('border-color', 'red');
                test = false;
            } else if (!CheckPosting) {
                $('#TCGE-PostingDate').css('border-color', 'red');
                test = false;
            } else {
                $('#TCGE-PostingDate').css('border-color', '');
            }

            if (test === true) {
                if ($("#BostingToORThrow").val() == "2") {
                    InsertTransactionData(companyID, 2, postingDate, transactionDate, reference, currency, systemRate, transactionRate, postingKey, transactionType, '', jeNum
                        , function (Jour) {
                            $.ajax({
                                url: "/Bus/GetPotingNumber?JornalEntry=" + Jour,
                                method: "POST",
                                success: function (data) {
                                    window.open(
                                        '/C_ReportsPrint/Done?searchNumber=' + data,
                                        '_blank'
                                    );
                                    RedirectInt(window.location.href)
                                }
                            })
                        }, null, null, null, true);
                } else {
                    InsertTransactionData(companyID, 1, postingDate, transactionDate, reference, currency, systemRate, transactionRate, postingKey, transactionType, '', jeNum, function () {
                        RedirectInt(window.location.href)
                    }, null, null, null, true);
                }
            }
        })
        

    });
});

function HandleDate(date) {
    var d = new Date(date),
        month = '' + (d.getMonth() + 1),
        day = '' + d.getDate(),
        year = d.getFullYear();

    if (month.length < 2) month = '0' + month;
    if (day.length < 2) day = '0' + day;

    return [year, month, day].join('-');
}