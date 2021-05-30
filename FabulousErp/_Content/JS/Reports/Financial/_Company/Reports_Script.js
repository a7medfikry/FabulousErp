var accountsArray = [],
    fiscalYearStart,
    fiscalYearEnd;

$(document).ready(function () {

    // 1.0 Get years If Open -- History -- Current
    $('#openYear').click(function () {
        var status = 'open';
        GetYears(status);
    });
    $('#historyYear').click(function () {
        var status = 'history';
        GetYears(status);
    });
    $('#currentYear').click(function () {
        var status = 'current';
        GetYears(status);
    });
    $('#allYears').change(function () {
        var yearID = $(this).val();
        $.ajax({
            type: 'GET',
            url: '/C_Reports/GetYearsDate?yearID=' + yearID,
            success: function (result) {
                fiscalYearStart = result.First_Year_Start;
                fiscalYearEnd = result.First_Year_End;
                $('#startDate').val(result.First_Year_Start);
                $('#endDate').val(result.First_Year_End);
            }
        });
    });
    $('#startDate').focusout(function () {
        var startDate = $(this).val();
        if (ConverDate(startDate) < ConverDate(fiscalYearStart)) {
            console.log("asd");
        }
        if (ConverDate(startDate) < ConverDate(fiscalYearStart) || ConverDate(startDate) > ConverDate(fiscalYearEnd)) {
            $('#startDate').val("");
        }
    });
    $('#endDate').focusout(function () {
        var endDate = $(this).val();
       
        if (moment(endDate).isAfter(fiscalYearEnd)) {
              $('#endDate').val("");
        } 
        if (!moment(endDate).isAfter(fiscalYearStart)) {
            $('#endDate').val("");
        }
        //if (ConverDate(endDate) < ConverDate(fiscalYearEnd) || ConverDate(endDate) < ConverDate(fiscalYearEnd) || ConverDate(endDate) > ConverDate($('#startDate').val())) {
        //    $('#endDate').val("");
        //}
    });
    $('#accountSort').change(function () {
        $('select.allAccounts').empty();
        RemovePickerDiv();
        var sortValue = $(this).val();
        if (sortValue.length > 0) {
            $.ajax({
                type: "GET",
                url: "/C_Reports/GetAccounts?sortValue=" + sortValue,
                success: function (result) {
                    $('.allAccounts').append($('<option>',
                        {
                            value: '',
                            text: '-Choose-'
                        }));
                    $.each(result, function (index, res) {
                        $('.allAccounts').append($('<option>',
                            {
                                value: res.C_AID,
                                text: res.AccountID + '-' + res.AccountName
                            }));
                    });
                }
            });
        } else {
            $('.allAccounts').append($('<option>',
                {
                    text: '-Choose-'
                }));
        }
    });
    $('#accountIDFrom').change(function () {
        $('#accountIDTO').val("");
        var selectedIndex = $(this).children('option:selected').index(),
            filterArr = [],
            i;
        for (i = 1; i < selectedIndex; i++) {
            filterArr.push($('#accountIDFrom option').eq(i).val());
        }
        $.each(filterArr, function (index, res) {
            $('#accountIDTO option').each(function () {
                if ($(this).val() == res) {
                    $(this).remove();
                }
            });
        });
    });
    $('#accountIDTO').change(function () {
        accountsArray = [];
        var selectedIndex = $(this).children('option:selected').index(),
            i;
        for (i = 1; i <= selectedIndex; i++) {
            accountsArray.push({
                C_AID: $('#accountIDTO option').eq(i).val()
            });
        }
    }).focusout(function () {
        $('#accountSort').prop('disabled', true);
        $('.allAccounts').prop('disabled', true);
        accountsArray = JSON.stringify({ 'accountsArray': accountsArray });
    });
    $('input[type=radio][name=currencyIn]').change(function () {
        if (this.id == 'currencyFunctional') {
            $('#currencyID').prop('disabled', true);
            $('#currencyRate').prop('disabled', true);
            $('#currencyOperation').prop('disabled', true);
        }
        else if (this.id == 'currencyReporting') {
            $('#currencyID').prop('disabled', false);
            $('#currencyRate').prop('disabled', false);
            $('#currencyOperation').prop('disabled', false);
        }
    });




    $('#RAD-allAccounts').change(function () {
        var accountID = $(this).val();
        $.ajax({
            type: "GET",
            url: "/C_Reports/GetAccountName?accountID=" + accountID,
            success: function (result) {
                $('#RAD-accountName').val(result);
            }
        }).done(function () {
            if ($('#allYears').val().length === 0) {
                $('#allYears').focus();
            } else {
                allDate_btn();
                $('#RAD-allDate').prop('checked', true);
                $('#RAD-startDate').prop('disabled', true);
                $('#RAD-endDate').prop('disabled', true);
                $('#RAD-search').prop('disabled', true);
            }
        });
    });
    $('#allYears').change(function () {
        if ($('#RAD-allAccounts').val().length === 0) {
            $('#RAD-allAccounts').focus();
        } else {
            allDate_btn();
            $('#RAD-allDate').prop('checked', true);
            $('#RAD-startDate').prop('disabled', true);
            $('#RAD-endDate').prop('disabled', true);
            $('#RAD-search').prop('disabled', true);
        }
    });
    $('#RAD-allDate').click(function () {
        $('#RAD-startDate').prop('disabled', true);
        $('#RAD-endDate').prop('disabled', true);
        $('#RAD-search').prop('disabled', true);
        allDate_btn();
    });
    $('#RAD-fromDate').click(function () {
        $('#RAD-startDate').prop('disabled', false);
        $('#RAD-endDate').prop('disabled', false);
        $('#RAD-search').prop('disabled', false);
    });
    $('#RAD-search').click(function () {
        ByDate_btn();
    });
    $('input[type="checkbox"][name="RAD-multiCurrency"]').click(function () {
        if ($(this).prop("checked") == true || $(this).prop("checked") == false) {
            if ($('#RAD-allDate').is(':checked')) {
                allDate_btn();
            } else {
                ByDate_btn();
            }
        }
    });



    $('#RAC-cutDate').focusout(function () {
        var date = $(this).val();
        $.ajax({
            type: 'GET',
            url: "/C_Reports/GetAvailableCashData?date=" + date,
            success: function (result) {
                $('.RAC-table').html(result);
            }
        });
        //$.ajax({
        //    type: 'GET',
        //    url: "/C_Reports/Get_Available_Cash_Data?date=" + date,
        //    success: function (result) {
        //        $.each(result, function (index, res) {
        //            if (res.Description == 'Cash') {
        //                console.log(index);
        //                var cashData = '<tr>' +
        //                    '<td>' + res.Checkbook_Name + '</td>' +
        //                    '<td>' + res.Balance + '</td>' +
        //                    '<td>' + res.IsoCode + '</td>' +
        //                    '<td>' + '<input type="text" class="form-control form-control-sm"' + '</td>' +
        //                    '<td>' + "" + '</td>' +
        //                    '</tr>';
        //                $('#RAC-tbl > tbody').append(cashData);
        //            } else if (res.Description == 'Bank') {
        //                var bankData = '<tr>' +
        //                    '<td>' + res.Checkbook_Name + '</td>' +
        //                    '<td>' + res.Balance + '</td>' +
        //                    '<td>' + res.IsoCode + '</td>' +
        //                    '<td>' + '<input type="text" class="form-control form-control-sm"' + '</td>' +
        //                    '<td>' + "" + '</td>' +
        //                    '</tr>';
        //                $('#RAC-tbl > tbody').append(bankData);
        //            } else if (res.Description == 'Check') {
        //                var checkData = '<tr>' +
        //                    '<td>' + res.Checkbook_Name + '</td>' +
        //                    '<td>' + res.Balance + '</td>' +
        //                    '<td>' + res.IsoCode + '</td>' +
        //                    '<td>' + '<input type="text" class="form-control form-control-sm"' + '</td>' +
        //                    '<td>' + "" + '</td>' +
        //                    '</tr>';
        //                $('#RAC-tbl > tbody').append(checkData);
        //            }
        //        });
        //        console.log(result);
        //    }
        //});
    });



    // Export to Excel
    $('#trailBalanceExport').click(function () {
        $('#trailBalance').table2excel({
            exclude: ".noExl",
            name: "Trial Balance Report",
            filename: "Trial Balance Report"
        });
    });
    $('#RAD-export').click(function () {
        $('#RAD-tbl').table2excel({
            exclude: ".noExl",
            name: "Account Details Report",
            filename: "Account Details Report"
        });
    });
    $('#RAC-export').click(function () {
        $('#RAC-tbl').table2excel({
            exclude: ".noExl",
            name: "Available Cash Report",
            filename: "Available Cash Report"
        });
    });

});




function GetYears(status) {
    var date = ConverDate(Date.now());
    $.ajax({
        type: "GET",
        url: "/C_Reports/GetYears?status=" + status,
        success: function (result) {
            $('select#allYears').empty();
            if (result.length > 0) {
                $('#allYears').append($('<option>',
                    {
                        text: "-Choose-"
                    }));
                $.each(result, function (index, res) {
                    if (status == 'open') {
                        if ((date < ConverDate(res.First_Year_Start) && date < ConverDate(res.First_Year_End)) || (date > ConverDate(res.First_Year_Start) && date > ConverDate(res.First_Year_End))) {
                            $('#allYears').append($('<option>',
                                {
                                    value: res.YearID,
                                    text: res.YearName
                                }));
                        }
                    } else if (status == 'current') {
                        if (date >= ConverDate(res.First_Year_Start) && date <= ConverDate(res.First_Year_End)) {
                            $('#allYears').append($('<option>',
                                {
                                    value: res.YearID,
                                    text: res.YearName
                                }));
                        }
                    } else if (status == 'history') {
                        $('#allYears').append($('<option>',
                            {
                                value: res.YearID,
                                text: res.YearName
                            }));
                    }
                });
            } else {
                $('#allYears').append($('<option>',
                    {
                        text: "-Choose-"
                    }));
            }
        }
    });
}
function ConverDate(date) {
    var d = new Date(date),
        month = '' + (d.getMonth() + 1),
        day = '' + d.getDate(),
        year = d.getFullYear();
    if (month.length < 2) month = '0' + month;
    if (day.length < 2) day = '0' + day;
    return [year, month, day].join('-');
}
function GetData() {
    var startDate = $('#startDate').val(),
        endDate = $('#endDate').val(),
        yearID = $('#allYears').val(),
        tableBody = $('#reportAppendData'),
        currencyId = $('#currencyID').val(),
        currencyRate = $('#currencyRate').val(),
        currencyOperation = $('#currencyOperation').val(),
        isValid = true;
    $('input[name="hasValidation"]').each(function () {
        if ($.trim($(this).val()) == '') {
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

    if ($('.allAccounts#accountIDFrom').val() == '') {
        $('.allAccounts#accountIDFrom').find("option:eq(1)").attr("selected", "selected")
        $('#accountIDTO').trigger("change");
        $('#accountIDTO').trigger("focusout");
        //isValid = false;
        //$('.allAccounts#accountIDFrom').css({
        //    "border": "1px solid red"
        //});
    } else {
        $('.allAccounts#accountIDFrom').css({
            "border": ""
        });
    } if ($('.allAccounts#accountIDTO').val() == '') {
        $('.allAccounts#accountIDTO').find("option").last().attr("selected", "selected");
        $('#accountIDTO').trigger("change");
        $('#accountIDTO').trigger("focusout");
        //isValid = false;
        //$('.allAccounts#accountIDTO').css({
        //    "border": "1px solid red"
        //});
    } else {
        $('.allAccounts#accountIDTO').css({
            "border": ""
        });
    }

    if (isValid == false) {
        console.log('Error');
    } else {
        $.ajax({
            type: 'POST',
            url: '/C_Reports/GetData?startDate=' + startDate + '&endDate=' + endDate + '&yearID=' + yearID,
            data: {
                accountsArray: accountsArray
            } ,
            success: function (result) {
                tableBody.empty();
                if (currencyRate != null && currencyOperation != null && currencyOperation == 1 && currencyId !== companyID) {
                    $("#IADI-HardGurrencyFormate").maskMoney({ suffix: ' ' + currencyId + '', thousands: ',', decimal: '.', precision: parseInt(requiredDecimalNum) });
                    $.each(result, function (index, res) {
                        tableBody.append("<tr><td>" + res.AccountID + "</td>" +
                            "<td>" + res.AccountName + "</td>" +
                            "<td>" + IADI_setHardCurrFormate(+parseFloat(res.Beginning_Balance / currencyRate)) + "</td>" +
                            "<td>" + IADI_setHardCurrFormate(+parseFloat(res.Sum_Debit / currencyRate)) + "</td>" +
                            "<td>" + IADI_setHardCurrFormate(+parseFloat(res.Sum_Credit / currencyRate)) + "</td>" +
                            "<td>" + IADI_setHardCurrFormate(+parseFloat(res.Net_Change / currencyRate)) + "</td>" +
                            "<td>" + IADI_setHardCurrFormate(+parseFloat(res.Ending_Balance / currencyRate)) + "</td></tr>");
                    });
                } else if (currencyRate != null && currencyOperation != null && currencyOperation == 2 && currencyId !== companyID) {
                    $("#IADI-HardGurrencyFormate").maskMoney({ suffix: ' ' + currencyId + '', thousands: ',', decimal: '.', precision: parseInt(requiredDecimalNum) });
                    $.each(result, function (index, res) {
                        tableBody.append("<tr><td>" + res.AccountID + "</td>" +
                            "<td>" + res.AccountName + "</td>" +
                            "<td>" + IADI_setHardCurrFormate(+parseFloat(res.Beginning_Balance * currencyRate)) + "</td>" +
                            "<td>" + IADI_setHardCurrFormate(+parseFloat(res.Sum_Debit * currencyRate)) + "</td>" +
                            "<td>" + IADI_setHardCurrFormate(+parseFloat(res.Sum_Credit * currencyRate)) + "</td>" +
                            "<td>" + IADI_setHardCurrFormate(+parseFloat(res.Net_Change * currencyRate)) + "</td>" +
                            "<td>" + IADI_setHardCurrFormate(+parseFloat(res.Ending_Balance * currencyRate)) + "</td></tr>");
                    });
                } else if (currencyRate != null && currencyOperation != null && currencyOperation == 2 && currencyId == companyID) {
                    $.each(result, function (index, res) {
                        tableBody.append("<tr><td>" + res.AccountID + "</td>" +
                            "<td>" + res.AccountName + "</td>" +
                            "<td>" + IADI_setSystemCurrFormate(+parseFloat(res.Beginning_Balance * currencyRate)) + "</td>" +
                            "<td>" + IADI_setSystemCurrFormate(+parseFloat(res.Sum_Debit * currencyRate)) + "</td>" +
                            "<td>" + IADI_setSystemCurrFormate(+parseFloat(res.Sum_Credit * currencyRate)) + "</td>" +
                            "<td>" + IADI_setSystemCurrFormate(+parseFloat(res.Net_Change * currencyRate)) + "</td>" +
                            "<td>" + IADI_setSystemCurrFormate(+parseFloat(res.Ending_Balance * currencyRate)) + "</td></tr>");
                    });
                } else if (currencyRate != null && currencyOperation != null && currencyOperation == 1 && currencyId == companyID) {
                    $.each(result, function (index, res) {
                        tableBody.append("<tr><td>" + res.AccountID + "</td>" +
                            "<td>" + res.AccountName + "</td>" +
                            "<td>" + IADI_setSystemCurrFormate(+parseFloat(res.Beginning_Balance / currencyRate)) + "</td>" +
                            "<td>" + IADI_setSystemCurrFormate(+parseFloat(res.Sum_Debit / currencyRate)) + "</td>" +
                            "<td>" + IADI_setSystemCurrFormate(+parseFloat(res.Sum_Credit / currencyRate)) + "</td>" +
                            "<td>" + IADI_setSystemCurrFormate(+parseFloat(res.Net_Change / currencyRate)) + "</td>" +
                            "<td>" + IADI_setSystemCurrFormate(+parseFloat(res.Ending_Balance / currencyRate)) + "</td></tr>");
                    });
                }
                else {
                    $.each(result, function (index, res) {
                        tableBody.append("<tr><td>" + res.AccountID + "</td>" +
                            "<td>" + res.AccountName + "</td>" +
                            "<td>" + IADI_setSystemCurrFormate(+parseFloat(res.Beginning_Balance)) + "</td>" +
                            "<td>" + IADI_setSystemCurrFormate(+parseFloat(res.Sum_Debit)) + "</td>" +
                            "<td>" + IADI_setSystemCurrFormate(+parseFloat(res.Sum_Credit)) + "</td>" +
                            "<td>" + IADI_setSystemCurrFormate(+parseFloat(res.Net_Change)) + "</td>" +
                            "<td>" + IADI_setSystemCurrFormate(+parseFloat(res.Ending_Balance)) + "</td></tr>");
                    });
                }
            },
            complete: function () {
                Trail_Balance_tableLoop();
            }
        });
    }
}
function Trail_Balance_tableLoop() {
    var totalDebit = 0,
        totalCredit = 0,
        totalNetChange = 0,
        totalEndingBalance = 0;
    if ($('#zeroBallance').is(':checked')) {
        return;
    } else {
        $('#trailBalance > tbody > tr').each(function () {
            var endingBalance = $(this).find('td:eq(6)').text();
            if (endingBalance == 0) {
                $(this).remove();
            }
        });
    }
    var regRemoveCurrFormate=/[^\d.-]/g
    $('#trailBalance > tbody > tr').each(function () {
        var debitSum = $(this).find('td:eq(3)').text().replace(regRemoveCurrFormate, ""),
            creditSum = $(this).find('td:eq(4)').text().replace(regRemoveCurrFormate, ""),
            netChangeSum = $(this).find('td:eq(5)').text().replace(regRemoveCurrFormate, "");
        ThisEndingSum = $(this).find('td:eq(6)').text().replace(regRemoveCurrFormate, "");
        totalDebit = parseFloat(totalDebit) + parseFloat(debitSum);
        totalCredit = parseFloat(totalCredit) + parseFloat(creditSum);
        totalNetChange = parseFloat(totalNetChange) + parseFloat(netChangeSum);
        totalEndingBalance = parseFloat(totalEndingBalance) + parseFloat(ThisEndingSum);

        $('#totalDebit').text(IADI_setSystemCurrFormate(totalDebit));
        $('#totalCredit').text(IADI_setSystemCurrFormate(totalCredit));
        $('#totalNetChange').text(IADI_setSystemCurrFormate(totalNetChange));
        $('#totalEndingBalance').text(IADI_setSystemCurrFormate(totalEndingBalance));
    });
}
function Account_Details_tableLoop() {
    var debitNo = 0,
        debitTotal = 0,
        creditNo = 0,
        creditTotal = 0,
        SED_No = 0,
        SID_No = 0,
        UNS_No = 0;
    $('#RAD-tbl > tbody > tr').each(function () {
        var debit = $(this).find('td:eq(10)').text().replace(regRemoveCurrFormate, ""),
            credit = $(this).find('td:eq(11)').text().replace(regRemoveCurrFormate, ""),
            document = $(this).find('td:eq(12)').text();
        if (debit > 0) {
            debitNo++;
            debitTotal = parseFloat(debitTotal) + parseFloat(debit);
        } else if (credit > 0) {
            creditNo++;
            creditTotal = parseFloat(creditTotal) + parseFloat(credit);
        } else {
            return;
        }
        if (document === 'SED') {
            SED_No++;
        } else if (document === 'SID') {
            SID_No++;
        } else if (document === 'UnS') {
            UNS_No++;
        } else {
            return;
        }
    });
    $('#RAD-debitNo').val(debitNo);
    $('#RAD-creditNo').val(creditNo);
    $('#RAD-debitAmount').val(IADI_setSystemCurrFormate(debitTotal));
    $('#RAD-creditAmount').val(IADI_setSystemCurrFormate(creditTotal));
    $('#RAD-SED').val(SED_No);
    $('#RAD-SID').val(SID_No);
    $('#RAD-UNS').val(UNS_No);
}
function allDate_btn() {
    var accountID = $('#RAD-allAccounts').val(),
        yearID = $('#allYears').val();
    RAD_GetSearchData(accountID, yearID);
}
function ByDate_btn() {
    var accountID = $('#RAD-allAccounts').val(),
        yearID = $('#allYears').val(),
        startDate = $('#RAD-startDate').val(),
        endDate = $('#RAD-endDate').val();
    RAD_GetSearchDataByDate(accountID, yearID, startDate, endDate);
}
function RAD_GetSearchData(accountID, yearID) {
    var tbody = $('#RAD-tbl > tbody');
    tbody.empty();
    $.ajax({
        type: "GET",
        url: "/C_Reports/Get_All_Account_Details_Data?accountID=" + accountID + "&yearID=" + yearID,
        success: function (result) {
            if (result.length > 0) {
                for (var i = 0; i < result.length; i++) {
                    RAD_AddToTbl(result[i].VoidJENum, result[i].Date, result[i].JournalNumber, result[i].PostingNumber, result[i].Postingkey, result[i].JournalType, result[i].Description, result[i].OriginalAmount, result[i].CurrencyID, result[i].TransactionRate, result[i].Sum_Debit, result[i].Sum_Credit, result[i].Document);
                }
            } else {
                tbody.append('<tr><td colspan="13" class="text-danger font-weight-bold">Not Exist Data To this Account..!</td></tr>')
            }
        },
        complete: function () {
            Account_Details_tableLoop();
        }
    });

}
function RAD_GetSearchDataByDate(accountID, yearID, startDate, endDate) {
    var tbody = $('#RAD-tbl > tbody');
    tbody.empty();
    $.ajax({
        type: "get",
        url: "/C_Reports/Get_All_Account_Details_ByData?accountID=" + accountID + "&yearID=" + yearID + "&startDate=" + startDate + "&endDate=" + endDate,
        success: function (result) {
            if (result.length > 0) {
                for (var i = 0; i < result.length; i++) {
                    RAD_AddToTbl(result[i].VoidJENum, result[i].Date, result[i].JournalNumber, result[i].PostingNumber, result[i].Postingkey, result[i].JournalType, result[i].Description, result[i].OriginalAmount, result[i].CurrencyID, result[i].TransactionRate, result[i].Sum_Debit, result[i].Sum_Credit, result[i].Document);
                }
            } else {
                tbody.append('<tr><td colspan="13" class="text-danger font-weight-bold">Not Exist Data To this Account..!</td></tr>')
            }
        },
        complete: function () {
            Account_Details_tableLoop();
        }
    });
}
function RAD_AddToTbl(v, date, journalNumber, postingNumber, postingkey, journalType, description, originalAmount, currencyID, transactionRate, debit, credit, document) {

    if (v != null) {
        v = "*";
    } else {
        v = '';
    }
    var data = "<tr>"
        + '<td class="text-danger font-weight-bold">' + v + '</td>' +
        '<td>' + date + '</td>' +
        '<td>' + journalNumber + '</td>' +
        '<td>' + postingNumber + '</td>' +
        '<td>' + postingkey + '</td>' +
        '<td>' + journalType + '</td>' +
        '<td>' + description + '</td>' +
        '<td class="forHidden">' + IADI_setSystemCurrFormate(+parseFloat(originalAmount)) + '</td>' +
        '<td class="forHidden">' + currencyID + '</td>' +
        '<td class="forHidden">' + transactionRate + '</td>' +
        '<td>' + IADI_setSystemCurrFormate(+parseFloat(debit)) + '</td>' +
        '<td>' + IADI_setSystemCurrFormate(+parseFloat(credit)) + '</td>' +
        '<td>' + document + '</td>' +
        '</tr>';
    $('#RAD-tbl > tbody').append(data);
    if ($('#RAD-multiCurrency').is(':checked')) {
        $('.forHidden').hide();
    } else {
        $('.forHidden').show();
    }
}
