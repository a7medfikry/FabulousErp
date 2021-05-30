const companyID = $('#IADI-CompanyID').text();
var requiredDecimalNum = 0,
    regRemoveCurrFormate = /[^\d.]/g,
    yearStartDate,
    yearEndDate;

$(document).ready(function () {
    DoPageCheck();
    $.ajax({
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        method: "get",
        async: false,
        url: "/api/TransactionApi/GetCurrencyFormate?companyID=" + companyID,
        success: function (result) {
            requiredDecimalNum = result.DecimalNumber;
            if ($("#IADI-GurrencyFormate").length <= 0) {
                $("body").append("<input type='hidden' id='IADI-GurrencyFormate' />")
            }
            $("#IADI-GurrencyFormate").maskMoney({ allowNegative: true, prefix: '' + result.Prefix + '', suffix: '' + result.Suffix + '', thousands: '' + result.Thousands + '', decimal: '' + result.Decimal + '', precision: parseInt(result.DecimalNumber) });
        }
    });

    $('#IADI-AccountID').change(function () {

        var accountID = $(this).val();

        $('#IADI-AccountName').val('');
        $('#IADI-tblBody').empty();
        $('#IASI-tblBody').empty();
        $('input[name="date"]').prop('checked', false);
        $('#IADI-searchError').text('');
        $('#IASI-totalDebit').text('');
        $('#IASI-totalCredit').text('');
        $('#IASI-totalNetChange').text('');
        $('#IASI-totalBallance').text('');

        if (accountID.length === 0) {
            $(this).css('border-color', 'red');
        } else {
            $(this).css('border-color', '');

            $.ajax({
                type: "GET",
                url: "/Inquiry_CompanyJETransactionsDetails/GetAccountName?AID=" + accountID,
                success: function (result) {
                    $('#IADI-AccountName').val(result);
                }
            });
        }
    });

    $('#IADI-Year').change(function () {
        var year = $(this).val();
        $('#IADI-tblBody').empty();
        $('#IASI-tblBody').empty();
        $('input[name="date"]').prop('checked', false);
        $('#IADI-searchError').text('');
        $('#IASI-totalDebit').text('');
        $('#IASI-totalCredit').text('');
        $('#IASI-totalNetChange').text('');
        $('#IASI-totalBallance').text('');
        if (year.length === 0) {
            $(this).css('border-color', 'red');
        } else {
            $(this).css('border-color', '');
        }
    });

    $('#IADI-allDate').click(function () {
        $('#IADI-startDate').prop('disabled', true);
        $('#IADI-startDate').val('');
        $('#IADI-endDate').prop('disabled', true);
        $('#IADI-endDate').val('');
        $('#IADI-search').prop('disabled', true);
        $('#IADI-searchError').text('');
        var accountID = $('#IADI-AccountID option:selected').val(),
            year = $('#IADI-Year').val();
        var check = IADI_validateSearchFields(accountID, year);
        if (check === true) {
            IADI_GetSearchData(accountID, year);
        } else {
            $(this).prop('checked', false);
        }
    });

    $('#IADI-fromDate').click(function () {
        FromToDateClicked();
    });
 

    //Search of account summary
    $('#IASI-search').click(function () {

        var accountID = $('#IADI-AccountID').val(),
            year = $('#IADI-Year').val();

        $('#IASI-tblBody').empty();
        $('#IASI-totalDebit').text('');
        $('#IASI-totalCredit').text('');
        $('#IASI-totalNetChange').text('');
        $('#IASI-totalBallance').text('');

        var check = IADI_validateSearchFields(accountID, year);
        if (check === true) {

            $.ajax({
                type: "GET",
                url: "/Inquiry_CompanyJETransactionsDetails/GetAccountSummary?AID=" + accountID + "&year=" + year,
                success: function (result) {
                    if (result.length > 0) {
                        $.each(result, function (index, row) {
                            var data = "<tr>"
                                + '<td >' + row.Periods + '</td>' +
                                '<td class="IASI-debit ">' + IADI_setSystemCurrFormate(+parseFloat(row.Debit)) + '</td>' +
                                '<td class="IASI-credit ">' + IADI_setSystemCurrFormate(+parseFloat(row.Credit)) + '</td>' +
                                '<td class="IASI-netChange ">' + IADI_setSystemCurrFormate(+parseFloat(row.NetChange)) + '</td>' +
                                '<td>' + IADI_setSystemCurrFormate(+parseFloat(row.EndingBalance)) + '</td>' +
                                '<td>' + '<button class="btn btn-secondary btn-sm Trans" id="IASI-viewDetails">View Details</button>' + '</td>' +
                                '</tr>';
                            $('#IASI-tblBody').append(data);
                            $('#IASI-totalBallance').text(IADI_setSystemCurrFormate(+parseFloat(row.EndingBalance)));
                        });
                        var sumdDebit = 0;
                        $(".IASI-debit").each(function () {
                            var value = $(this).text().replace(regRemoveCurrFormate, '');
                            // add only if the value is number
                            if (!isNaN(value) && value.length != 0) {
                                sumdDebit += parseFloat(value);
                            }
                        });
                        $('#IASI-totalDebit').text(IADI_setSystemCurrFormate(+parseFloat(sumdDebit)));

                        var sumCredit = 0;
                        $(".IASI-credit").each(function () {
                            var value = $(this).text().replace(regRemoveCurrFormate, '');
                            // add only if the value is number
                            if (!isNaN(value) && value.length != 0) {
                                sumCredit += parseFloat(value);
                            }
                        });
                        $('#IASI-totalCredit').text(IADI_setSystemCurrFormate(+parseFloat(sumCredit)));

                        $("#IASI-totalNetChange").text(IADI_setSystemCurrFormate(+parseFloat(sumdDebit - sumCredit)));
                        try {
                            ReTranslate()
                        } catch (err) {

                        }

                    }
                }
            });
        }
    });


   
    $("#TCGE-TTbl").on('click', '.MoreDetailsT', function () {
        var row = $(this).closest("tr");
        var tds = row.find("td");

        var accountIDTbl = tds.eq(4).text();
        var describtion = tds.eq(5).text();
        $("#TCGE-PUMDAccID").text(accountIDTbl);
        $("#TCGE-PUMDDes").text(describtion);
        $("#TCGE-PUMoreDetailsRecord").modal("show");
    });


    //Search of Batch Security
    var tblRowForCurr = $('#ICBSI-tbl').find('tbody').find('tr');
    tblRowForCurr.each(function (i, el) {
        var val = $(el).find('td').eq(7).text();
        $(el).find('td').eq(7).text(IADI_setSystemCurrFormate(+parseFloat(val)));
    });

    $('#ICBSI-searchByBatch').click(function () {
        $('#ICBSI-SHBID').show();
        $('#ICBSI-SHUID').hide();
        $('#ICBSI-BatchID').val('');
        $('#ICBSI-UserID').val('');

        $('#ICBSI-tbl').find('th').show();
        $('#ICBSI-tbl').find('tbody').find('td').show();
        $('#ICBSI-tbl').find('tbody').find('tr').show();
    });

    $('#ICBSI-searchByUCreate').click(function () {
        $('#ICBSI-SHBID').hide();
        $('#ICBSI-SHUID').show();
        $('#ICBSI-BatchID').val('');
        $('#ICBSI-UserID').val('');

        $('#ICBSI-tbl').find('th').show();
        $('#ICBSI-tbl').find('tbody').find('td').show();
        $('#ICBSI-tbl').find('tbody').find('tr').show();
    });

    $('#ICBSI-searchByUApproved').click(function () {
        $('#ICBSI-SHBID').hide();
        $('#ICBSI-SHUID').show();
        $('#ICBSI-BatchID').val('');
        $('#ICBSI-UserID').val('');

        $('#ICBSI-tbl').find('th').show();
        $('#ICBSI-tbl').find('tbody').find('td').show();
        $('#ICBSI-tbl').find('tbody').find('tr').show();
    });

    $('#ICBSI-BatchID').change(function () {

        $('#ICBSI-tbl').find('td:nth-child(1),th:nth-child(1)').hide();

        var filterValue = $('#ICBSI-BatchID  option:selected').text();

        var rows = $('#ICBSI-tbl').find('tbody').find('tr');
        rows.hide();
        rows.each(function (i, el) {

            if ($(el).find('td').eq(0).text() === filterValue) {
                $(el).show();
            }

        });


    });

    $('#ICBSI-UserID').change(function () {

        var filterValue = $('#ICBSI-UserID  option:selected').text();

        if ($('#ICBSI-searchByUCreate').is(':checked')) {

            $('#ICBSI-tbl').find('td:nth-child(4),th:nth-child(4)').hide();
            $('#ICBSI-tbl').find('td:nth-child(5),th:nth-child(5)').hide();
            $('#ICBSI-tbl').find('td:nth-child(6),th:nth-child(6)').hide();

            var rows = $('#ICBSI-tbl').find('tbody').find('tr');
            rows.hide();
            rows.each(function (i, el) {

                if ($(el).find('td').eq(3).text() === filterValue) {
                    $(el).show();
                }

            });

        } else if ($('#ICBSI-searchByUApproved').is(':checked')) {

            $('#ICBSI-tbl').find('td:nth-child(3),th:nth-child(3)').hide();
            $('#ICBSI-tbl').find('td:nth-child(4),th:nth-child(4)').hide();
            $('#ICBSI-tbl').find('td:nth-child(5),th:nth-child(5)').hide();

            var rows = $('#ICBSI-tbl').find('tbody').find('tr');
            rows.hide();
            rows.each(function (i, el) {

                if ($(el).find('td').eq(4).text() === filterValue) {
                    $(el).show();
                }

            });

        }

    });

    // ----------------------------------------------------------------------------------------------

    $(document).on('click', '#IASI-viewDetails', function () {
        if (window.location.pathname == '/Inquiry_CompanyJETransactionsDetails/AccountSummary') {
            var accountID = $('#IADI-AccountID option:selected').val(),
                yearID = $('#IADI-Year').val(),
                rowID = $(this).closest('tr'),
                periodCell = rowID.find('td:eq(0)').text(),
                periodNo = periodCell.replace(/^\D+/g, '');
            $.ajax({
                type: 'GET',
                url: '/Inquiry_CompanyJETransactionsDetails/PeriodsDate?yearID=' + yearID + '&periodNo=' + periodNo,
                success: function (result) {
                    window.location.href = '/Inquiry_CompanyJETransactionsDetails/AccountDetails?accountID='
                        + accountID + '&yearID=' + yearID + '&startDate=' + result.StartDate + '&endDate=' + result.EndDate;
                }
            });
        }
        else if (window.location.pathname == '/Inquiry_CompanyJETransactionsDetails/AccountDetails') {
            var accountID = getParameterByName("accountID"),
                yearID = getParameterByName("yearID"),
                startDate = getParameterByName("startDate"),
                endDate = getParameterByName("endDate");
            if (accountID && yearID && startDate && endDate) {
                $('#IADI-AccountID').val(accountID);
                $('#IADI-AccountID').trigger('change');
                $('#IADI-Year').val(yearID);
                $('#IADI-Year').trigger('change');
                $('#IADI-fromDate').trigger('click');
                $('#IADI-startDate').val(startDate);
                $('#IADI-endDate').val(endDate);
                $('#IADI-search').trigger('click');
            }
            var rowID = $(this).closest('tr'),
                journalEntryNumber = rowID.find('td:eq(11)').text();
            window.location.href = '/Inquiry_CompanyJETransactionsDetails/TransactionDetails?journalEntryNumber=' + journalEntryNumber;

        }
        else if (window.location.pathname == '/Inquiry_CompanyJETransactionsDetails/TransactionDetails') {
            var journalEntryNumber = getParameterByName("journalEntryNumber");
            if (journalEntryNumber) {
                $('#ITDI-JENum').val(journalEntryNumber);
                $('#ITDI-JENum').trigger('change');
            }
            var journalEntryNumber = $('#ITDI-JENum option:selected').val(),
                str = $('#ITDI-JENum option:selected').text(),
                n = str.lastIndexOf('-'),
                res = str.substring(n + 1);
            if (res == 'TCCR' || res == 'TCCW') {
                window.location.href = '/Inquiry_CheckbookTransactions/CashInquiry?journalEntryNumber=' + journalEntryNumber;
            } else if (res == 'TCBC' || res == 'TCBC') {
                window.location.href = '/Inquiry_CheckbookTransactions/BankInquiry?journalEntryNumber=' + journalEntryNumber;
            } else if (res == 'TCBT') {
                window.location.href = '/Inquiry_CheckbookTransactions/Transfer?journalEntryNumber=' + journalEntryNumber;
            } else if (res == 'TCGE') {
                return;
            };
        }
        else if (window.location.pathname == '/Inquiry_CompanyJETransactionsDetails/HistoricalAccountSummary') {
            var accountID = $('#IADI-AccountID option:selected').val(),
                yearID = $('#IADI-Year').val(),
                rowID = $(this).closest('tr'),
                periodCell = rowID.find('td:eq(0)').text(),
                periodNo = periodCell.replace(/^\D+/g, '');
            $.ajax({
                type: 'GET',
                url: '/Inquiry_CompanyJETransactionsDetails/PeriodsDate?yearID=' + yearID + '&periodNo=' + periodNo,
                success: function (result) {
                    window.location.href = '/Inquiry_CompanyJETransactionsDetails/HistoricalAccountDetails?accountID='
                        + accountID + '&yearID=' + yearID + '&startDate=' + result.StartDate + '&endDate=' + result.EndDate;
                }
            });
        } else if (window.location.pathname == '/Inquiry_CompanyJETransactionsDetails/HistoricalAccountDetails') {
            var accountID = getParameterByName("accountID"),
                yearID = getParameterByName("yearID"),
                startDate = getParameterByName("startDate"),
                endDate = getParameterByName("endDate");
            if (accountID && yearID && startDate && endDate) {
                $('#IADI-AccountID').val(accountID);
                $('#IADI-AccountID').trigger('change');
                $('#IADI-Year').val(yearID);
                $('#IADI-Year').trigger('change');
                $('#IADI-fromDate').trigger('click');
                $('#IADI-startDate').val(startDate);
                $('#IADI-endDate').val(endDate);
                $('#IADI-search').trigger('click');
            }
            var rowID = $(this).closest('tr'),
                journalEntryNumber = rowID.find('td:eq(11)').text();
            window.location.href = '/Inquiry_CompanyJETransactionsDetails/TransactionDetails?journalEntryNumber=' + journalEntryNumber;

        }

    });
    
   
    function IADI_setSystemCurrFormate(val) {
        if (val === 0) {
            return 0;
        } else {
            $(document).find('#IADI-GurrencyFormate').removeAttr("disabled");
            var fixedVal = parseFloat(val).toFixed(parseInt(requiredDecimalNum));
            $(document).find('#IADI-GurrencyFormate').maskMoney('mask', parseFloat(fixedVal));

            return $(document).find('#IADI-GurrencyFormate').val();
        }
    }
    function IADI_setHardCurrFormate(val) {
        if (val === 0) {
            return 0;
        } else {

            var fixedVal = parseFloat(val).toFixed(parseInt(requiredDecimalNum));

            $('#IADI-HardGurrencyFormate').maskMoney('mask', parseFloat(fixedVal));

            return $('#IADI-HardGurrencyFormate').val();
        }
    }


    // Functions of Accounnt Details
    function IADI_GetSearchData(accountID, year) {
        var tabl = $('#IADI-tblBody');
        tabl.empty();
        $.ajax({
            type: "GET",
            url: "/Inquiry_CompanyJETransactionsDetails/GetAllSearchData?AID=" + accountID + "&year=" + year,
            success: function (result) {
                if (result.length > 0) {
                    for (var i = 0; i < result.length; i++) {
                        IADI_AddToTbl(result[i].VoidJENum, result[i].Date, result[i].JournalEntryNumber, result[i].Currency, result[i].OriginalAmount, result[i].TransactionRate, result[i].Describtion, result[i].Debit, result[i].Credit, result[i].CurrencyID, result[i].PostingKey, result[i].PostingNumber);
                    }
                } else {
                    tabl.append('<tr><td colspan="10" class="text-danger font-weight-bold">' + NoExistData +'</td></tr>')
                }
                ReTranslate();

            }
        });
    }
    

})
//Search of transaction details
$('#ITDI-JENum').change(function () {

    var postingNumber = $(this).val();

    IADI_ClearData();
    $('#ITDI-NoAC').text("");

    if (postingNumber.length > 0) {

        $.ajax({
            url: "/api/TransactionApi/GetTransactionData?postingNumber=" + postingNumber,
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

                GetFiles(postingNumber, $("#ITDI-JENum").find("option:selected").text().split("-")[1]);
            }
        });
    }
});
function IADI_GetSearchDataByDate(accountID, year, startDate, endDate) {

    var tabl = $('#IADI-tblBody');
    tabl.empty();

    $.ajax({
        type: "get",
        url: "/Inquiry_CompanyJETransactionsDetails/GetSearchDataByDates?AID=" + accountID + "&year=" + year + "&dateFrom=" + startDate + "&dateTo=" + endDate,
        success: function (result) {
            if (result === "inValid") {
                $('#IADI-endDate').css('border-color', 'red');
                $('#IADI-startDate').css('border-color', 'red');
                $('#IADI-searchError').text('Invalid Dates with Choosen Year..!');
            } else {
                $('#IADI-endDate').css('border-color', '');
                $('#IADI-startDate').css('border-color', '');

                if (result.length > 0) {
                    for (var i = 0; i < result.length; i++) {

                        IADI_AddToTbl(result[i].VoidJENum, result[i].Date, result[i].JournalEntryNumber, result[i].Currency, result[i].OriginalAmount, result[i].TransactionRate, result[i].Describtion, result[i].Debit, result[i].Credit, result[i].CurrencyID, result[i].PostingKey, result[i].PostingNumber);
                    }
                } else {
                    tabl.append('<tr><td colspan="10" class="text-danger font-weight-bold">' + NoExistData +'</td></tr>')
                }

                ReTranslate();
            }
        }
    });
}
function IADI_AddToTbl(v, date, journalEntryNumber, currency, originalAmount, transactionRate, describtion, debit, credit, currencyID, postingKey, postingNumber) {

    if (currencyID !== companyID) {
        $("#IADI-HardGurrencyFormate").maskMoney({ suffix: ' ' + currency + '', thousands: ',', decimal: '.', precision: parseInt(requiredDecimalNum) });
        originalAmount = IADI_setHardCurrFormate(+parseFloat(originalAmount));
    } else {
        originalAmount = IADI_setSystemCurrFormate(+parseFloat(originalAmount));
    }

    if (v != null) {
        v = "*";
    } else {
        v = '';
    }

    transactionRate = IADI_setSystemCurrFormate(+parseFloat(transactionRate));
    debit = IADI_setSystemCurrFormate(+parseFloat(debit));
    credit = IADI_setSystemCurrFormate(+parseFloat(credit));

    var data = "<tr>"
        + '<td class="text-danger font-weight-bold">' + v + '</td>' +
        '<td>' + date + '</td>' +
        '<td>' + journalEntryNumber + '</td>' +
        '<td>' + postingKey + '</td>' +
        '<td>' + currency + '</td>' +
        '<td>' + originalAmount + '</td>' +
        '<td>' + transactionRate + '</td>' +
        '<td>' + describtion + '</td>' +
        '<td>' + debit + '</td>' +
        '<td>' + credit + '</td>' +
        '<td>' + '<button class="btn btn-secondary btn-sm Trans" id="IASI-viewDetails">View Details</button>' + '</td>' +
        '<td hidden>' + postingNumber + '</td>' +
        '</tr>';
    $('#IADI-tblBody').append(data);
}


//Functions Of transaction Details
function IADI_ClearData() {
    $("#TCGE-JEDate").val('');
    $("#TCGE-PostingDate").val('');
    $("#TCGE-CurrencyID").val('');
    $("#TCGE-SystemRate").val('');
    $("#TCGE-TransactionRate").val('');
    $("#TCGE-DiffrenceRate").val('');
    $("#TCGE-Reference").val('');
    $("#TCGE-TTbl").empty();
    $("#TCGE-TAccountAnalyticDB").empty();
    $("#TCGE-TCostCenterAccountDB").empty();
    $("#TCGE-TAccountAnalytic").empty();
    $("#TCGE-TCostCenter").empty();
    $("#TCGE-TMainCostCenter").empty();
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
function IADI_RetrieveToDBAnalyticTbl(analyticID, c_DistID, distributionID, distributionName, accountID, describtion, percentage, amount, debit, credit) {
    var analyticDBContent = "<tr class='DisDBrow_" + accountID + "'>" +
        "<td>" + analyticID + "</td>" +
        "<td>" + c_DistID + "</td>" +
        "<td>" + distributionID + "</td>" +
        "<td>" + distributionName + "</td>" +
        "<td>" + accountID + "</td>" +
        "<td>" + describtion + "</td>" +
        "<td>" + percentage + "</td>" +
        "<td>" + amount + "</td>" +
        "<td>" + debit + "</td>" +
        "<td>" + credit + "</td>" +
        "</tr>";

    $("#TCGE-TAccountAnalyticDB").append(analyticDBContent);
}
function IADI_RetrieveToDBCostCenterTbl(costCenterID, c_CAID, costAccountID, costAccountName, accountID, describtion, percentage, amount, debit, credit, costCenterType, MainCostCenterID, CostCenterIDPercentage, costCenterName) {

    var costCenterDBContent = "<tr class='CCAccDBrow_" + accountID + "'>" +
        "<td>" + costCenterID + "</td>" +
        "<td>" + c_CAID + "</td>" +
        "<td>" + costAccountID + "</td>" +
        "<td>" + costAccountName + "</td>" +
        "<td>" + accountID + "</td>" +
        "<td>" + describtion + "</td>" +
        "<td>" + percentage + "</td>" +
        "<td>" + amount + "</td>" +
        "<td>" + debit + "</td>" +
        "<td>" + credit + "</td>" +
        "<td>" + costCenterType + "</td>" +
        "<td>" + MainCostCenterID + "</td>" +
        "<td>" + CostCenterIDPercentage + "</td>" +
        "<td>" + costCenterName + "</td>" +
        "</tr>";

    $("#TCGE-TCostCenterAccountDB").append(costCenterDBContent);

}
function IADI_ShowAnalyticOfTrans(id) {

    $("#TCGE-ShowTAccountAnalytic").empty();

    $('#ITDI-NoAC').text("");

    if ($(".DisDBrow_" + id + "").length) {

        $("#TCGE-TAccountAnalyticDB").find(".DisDBrow_" + id + "").each(function () {

            var tds = $(this).find('td'),
                c_DistID = tds.eq(1).text(),
                distributionID = tds.eq(2).text(),
                distributionName = tds.eq(3).text(),
                describtion = tds.eq(5).text(),
                percentage = tds.eq(6).text().replace('%', ''),
                amount = tds.eq(7).text();

            //Function that add to analytic tbl taht exist in analytic popup
            IADI_RetrieveToAnalyticTbl(c_DistID, distributionID, distributionName, percentage, amount, describtion);

            $('#TCGE-PUShowAnalyticAccount').modal('show');
        });
    } else {
        $('#ITDI-NoAC').text('No Analytic To This Account');
    }
}
function IADI_RetrieveToAnalyticTbl(c_DistID, distributionID, distributionName, percentage, amount, describtion) {

    var analyticContent = "<tr class='Disrow_" + c_DistID + "'>" +
        "<td>" + distributionID + "</td>" +
        "<td class='hide-normal TCGE-TblDistID'>" + c_DistID + "</td>" +
        "<td>" + distributionName + "</td>" +
        "<td>" + percentage + "%" + "</td>" +
        "<td class='SumAmountTblAnalyticDist'>" + IADI_setSystemCurrFormate(+amount) + "</td>" +
        "<td>" + describtion + "</td>" +
        "</tr>";
    $("#TCGE-ShowTAccountAnalytic").append(analyticContent);
}
function IADI_ShowCostOfTrans(id) {

    $('#TCGE-TShowCostCenter').empty();

    $('#TCGE-TShowMainCostCenter').empty();

    $('#ITDI-NoAC').text("");

    if ($(".CCAccDBrow_" + id + "").length) {

        var getCCDBTbl = $(".CCAccDBrow_" + id + "").find('td');
        var costCenterID = getCCDBTbl.eq(0).text();
        var costCenterType = getCCDBTbl.eq(10).text();
        var mainCostCenterID = getCCDBTbl.eq(11).text();

        if (costCenterType === "CostCenter") {

            $("#TCGE-TblShowCostCenter").show();
            $("#TCGE-TblShowMainCostCenter").hide();

            //show Cost center ID in main data and hide Main cost center from Main Data
            $("#TCGE-PUSHCCHS").show();
            $("#TCGE-PUSHMCCHS").hide();

            $("#TCGE-PUSHCCCostID").text(costCenterID);

            $("#TCGE-TCostCenterAccountDB").find(".CCAccDBrow_" + id + "").each(function () {

                var tds = $(this).find('td'),
                    c_CAID = tds.eq(1).text(),
                    costAccountID = tds.eq(2).text(),
                    costAccountName = tds.eq(3).text(),
                    describtion = tds.eq(5).text(),
                    percentage = tds.eq(6).text().replace('%', ''),
                    amount = tds.eq(7).text(),
                    costCenterType = tds.eq(10).text();

                //Function that add data to cost center table that exist in CC popup
                IADI_RetrieveToCostCenterTbl(c_CAID, costAccountID, costAccountName, percentage, amount, describtion, costCenterType);

            });
        } else if (costCenterType === "MainCostCenter") {

            $("#TCGE-TblShowCostCenter").hide();
            $("#TCGE-TblShowMainCostCenter").show();

            //hide Cost center ID in main data and show Main cost center from Main Data
            $("#TCGE-PUSHCCHS").hide();
            $("#TCGE-PUSHMCCHS").show();

            $("#TCGE-PUSHCCMainCostID").text(mainCostCenterID);

            $("#TCGE-TCostCenterAccountDB").find(".CCAccDBrow_" + id + "").each(function () {

                var tds = $(this).find('td'),
                    costCenterID = tds.eq(0).text(),
                    c_CAID = tds.eq(1).text(),
                    costAccountID = tds.eq(2).text(),
                    costAccountName = tds.eq(3).text(),
                    describtion = tds.eq(5).text(),
                    percentage = tds.eq(6).text().replace('%', ''),
                    amount = tds.eq(7).text(),
                    costCenterType = tds.eq(10).text(),
                    cCIDPercentage = tds.eq(12).text().replace('%', ''),
                    costCenterName = tds.eq(13).text();

                //Function that add data to cost center table that exist in CC popup
                IADI_RetrieveToCostCenterTbl(c_CAID, costAccountID, costAccountName, percentage, amount, describtion, costCenterType, costCenterID, cCIDPercentage, costCenterName);

            });
        }

        $('#TCGE-PUShowCostCenter').modal('show');

    } else {
        $('#ITDI-NoAC').text('No Cost Center To This Account');
    }
}
function IADI_RetrieveToCostCenterTbl(c_CAID, CostAccountID, CostAccountName, percentage, amount, describtion, costCenterType, costCenterID, costCenterIDPercentage, costCenterName) {

    if (costCenterType === "CostCenter") {

        let CostContent = "<tr>" +
            "<td class='hide-normal TCGE-TblCCAccID'>" + c_CAID + "</td>" +
            "<td>" + CostAccountID + "</td>" +
            "<td>" + CostAccountName + "</td>" +
            "<td>" + percentage + "%" + "</td>" +
            "<td class='SumAmountTblCostAccount'>" + IADI_setSystemCurrFormate(+amount) + "</td>" +
            "<td>" + describtion + "</td>" +
            "<td class='hide-normal'>" + costCenterType + "</td>" +
            "</tr>";
        $("#TCGE-TShowCostCenter").append(CostContent);

    } else if (costCenterType === "MainCostCenter") {

        let CostContent = "<tr>" +
            "<td class='hide-normal TCGE-TblCCAccID'>" + c_CAID + "</td>" +
            "<td>" + costCenterID + "</td>" +
            "<td>" + costCenterIDPercentage + "%" + "</td>" +
            "<td>" + CostAccountID + "</td>" +
            "<td>" + CostAccountName + "</td>" +
            "<td>" + percentage + "%" + "</td>" +
            "<td class='SumAmountTblCostAccount'>" + IADI_setSystemCurrFormate(+amount) + "</td>" +
            "<td>" + describtion + "</td>" +
            "<td class='hide-normal'>" + costCenterType + "</td>" +
            "<td class='hide-normal'>" + costCenterName + "</td>" +
            "</tr>";
        $("#TCGE-TShowMainCostCenter").append(CostContent);

    }

}




//search of account details
$('#IADI-search').click(function () {

    var startDate = $('#IADI-startDate').val(),
        endDate = $('#IADI-endDate').val(),
        accountID = $('#IADI-AccountID').val(),
        year = $('#IADI-Year').val(),
        test = true;

    $('#IADI-tblBody').empty();
    $('#IADI-searchError').text('');

    if (startDate.length === 0) {
        $('#IADI-startDate').css('border-color', 'red');
        test = false;
    } else {
        $('#IADI-startDate').css('border-color', '');
    }

    if (endDate.length === 0) {
        $('#IADI-endDate').css('border-color', 'red');
        test = false;
    } else {
        $('#IADI-endDate').css('border-color', '');
    }

    if (endDate < startDate) {
        $('#IADI-endDate').css('border-color', 'red');
        test = false;
    } else {
        $('#IADI-endDate').css('border-color', '');
    }

    if (test === true) {

        IADI_GetSearchDataByDate(accountID, year, startDate, endDate);

    }
});
function DoPageCheck() {
    if (window.location.pathname == '/Inquiry_CompanyJETransactionsDetails/AccountDetails') {
        var accountID = getParameterByName("accountID"),
            yearID = getParameterByName("yearID"),
            startDate = getParameterByName("startDate"),
            endDate = getParameterByName("endDate");
        if (accountID && yearID && startDate && endDate) {
            $('#IADI-AccountID').val(accountID);
            $('#IADI-AccountID').trigger('change');
            $('#IADI-Year').val(yearID);
            $('#IADI-Year').trigger('change');
            $('#IADI-fromDate').trigger('click');
            FromToDateClicked();
            $('#IADI-startDate').val(startDate);
            $('#IADI-endDate').val(endDate);
            $('#IADI-search').trigger('click');
        }
    } else if (window.location.pathname == '/Inquiry_CompanyJETransactionsDetails/TransactionDetails') {
        $("#ITDI-JENum").val(getParameterByName("journalEntryNumber"));
        $("#ITDI-JENum").trigger("change");
    }
}
function IADI_validateSearchFields(accountID, year) {

    var test = true;

    if (accountID.length === 0) {
        $('#IADI-AccountID').css('border-color', 'red');
        test = false;
    } else {
        $('#IADI-AccountID').css('border-color', '');
    }

    if (year.length === 0) {
        $('#IADI-Year').css('border-color', 'red');
        test = false;
    } else {
        $('#IADI-Year').css('border-color', '');
    }

    return test;
}
function FromToDateClicked() {
    var accountID = $('#IADI-AccountID').val(),
        year = $('#IADI-Year').val();
    $('#IADI-tblBody').empty();
    $('#IADI-searchError').text('');
    var check = IADI_validateSearchFields(accountID, year);
    if (check === true) {
        $('#IADI-startDate').prop('disabled', false);
        $('#IADI-endDate').prop('disabled', false);
        $('#IADI-search').prop('disabled', false);
    } else {
        $(this).prop('checked', false);
    }
}