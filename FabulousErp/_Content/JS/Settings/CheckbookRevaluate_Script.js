/*--------- Global Variables ---------*/
var companyID = $("#TCGE-CompanyID").text(),
    EPDcheck = $("#TCGE-EPD").text(),
    fJEPer = $("#TCGE-FJEPer").text(),
    PTcheck = $("#CBT-PT").text(),
    profitID,
    lossID,
    headerObj = {},
    mainArr = [],
    rowSelector;

$(document).ready(function () {

    // 1.0
    $('#realRevaluate').click(function () {
        if ($('#revaluateDate').val().length > 0) {
            GetRevaluateData();
            $('#revaluateBtn').prop('disabled', false);
            $('#reverseDiv').show();
        } else {
            $('#revaluateDate').focus();
        }
    });
    $('#unRealRevaluate').click(function () {
        GetRevaluateData();
        $('#revaluateBtn').prop('disabled', true);
        $('#reverseDiv').hide();
        $('#revaluateReverseDate').hide();
    });
    $('#reverseRevaluate').change(function () {
        if (this.checked)
            $('#revaluateReverseDate').show();
        else
            $('#revaluateReverseDate').hide();
    });
    $('#revaluateDate').focusout(function () {
        var tableBody = $('#TCBR-appendData');
        tableBody.empty();
    });

    // 2.0
    $(document).on('focusin', 'input[name=revaluateSystemRate]', function () {
        var row = $(this).closest('tr'),
            currencyID = row.find('td:eq(0)').text(),
            postingDate = $('#revaluateDate').val();
        row.find($('input[name=revaluateNewBalance]')).val(0);
        row.find($('input[name=revaluateProfitLoss]')).val(0);

        CheckPostingDateInPeriods(postingDate, function (test) {
            if (test !== true) {
                $(".TCGE-SystemRate").val("");
            } else {
                GetCurrencyRates(currencyID, postingDate, row);
            }
        })
       
    });

    // 3.0
    $(document).on('focusin', 'input[name=revaluateNewBalance]', function () {

        var row = $(this).closest('tr'),
            balance = row.find('td:eq(2)').find('input').val(),
            balanceCurrentCashAccount = row.find('td:eq(3)').find('input').val(),
            systemRate = row.find('td:eq(4)').find('input').val().replace(regRemoveCurrFormate, ""),
            currencyID = row.find('td:eq(0)').text(),
            sum = 0;
        rowSelector = row;

        // First Get Account Profit OR Loss
        $.ajax({
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            method: "get",
            async: false,
            url: "/api/CheckBookAPIs/GetProfitLossAccount?currencyID=" + currencyID + "&companyID=" + companyID,
            success: function (result) {
                if (result.length > 0) {
                    $.each(result, function (key, item) {
                        if (item.AccountType === "Profit") {
                            // row.find($('input[name=revaluateProfitLossAccount]')).val(item.C_AID);
                            profitID = item.C_AID;
                        } else {
                            lossID = item.C_AID;
                            // row.find($('input[name=revaluateProfitLossAccount]')).val(item.C_AID);
                        }
                    });
                } else {
                    $('#CBT-AccountsList').modal('show');
                }
            }
        });

        // Calculate Profit/Loss Amount
        // Get Currency-Account-ID Of Profit OR Loss
        $(this).val(parseFloat(balance) * parseFloat(systemRate));
        var profit_loss = parseFloat(balance) * parseFloat(systemRate) - parseFloat(balanceCurrentCashAccount);
        row.find($('input[name=revaluateProfitLoss]')).val(profit_loss);
        if (profit_loss > 0) {
            row.find($('input[name=revaluateProfitLossAccount]')).val(profitID);
        } else {
            row.find($('input[name=revaluateProfitLossAccount]')).val(lossID);
        }

        // Loop on Profit/Loss To Calculate Total
        $('input[name=revaluateProfitLoss]').each(function () {
            var value = $(this).val();
            if (!isNaN(value) && value.length != 0) {
                sum += parseFloat(value);
                $('input[name=revaluateTotal]').val(sum);
            }
        });
    });

    // 4.0 Choose Profit or Loss Account If Currency Not Has
    $('#AccountsList').change(function () {
        var hiddenAccountID = $(this).val();
        $(rowSelector).find($('input[name=revaluateProfitLossAccount]')).val(hiddenAccountID);
        $('#CBT-AccountsList').modal('hide');
    });

    // 5.0 Revaluate Button
    $('#revaluateBtn').click(function (e) {
        var isValid = true;
        $('#TCBR-appendData :input').each(function () {
            if ($.trim($(this).val()) == 0) {
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
        if (isValid == false) {
            e.preventDefault();
        } else {
            if ($('#reverseRevaluate').is(':checked')) {
                Finish_Revaluate();
                Finish_Reverse_Revaluate();
            } else {
                Finish_Revaluate();
            }
            location.reload();
        }
    });


});

function ConverDate(date) {
    var d = new Date(date),
        month = '' + (d.getMonth() + 1),
        day = '' + d.getDate(),
        year = d.getFullYear();
    if (month.length < 2) month = '0' + month;
    if (day.length < 2) day = '0' + day;
    return [year, month, day].join('-');
}
function GetRevaluateData() {
    var date = $('#revaluateDate').val(),
        tableBody = $('#TCBR-appendData'),
        html = '';
    if (date.length > 0) {
        $.ajax({
            type: 'GET',
            url: "/CheckbookRevaluate/GetRevaluateData?date=" + date,
            success: function (result) {
                tableBody.empty();
                $.each(result, function (key, item) {
                    html += '<tr>';
                    html += '<td hidden>' + item.CurrencyID + '</td>';
                    html += '<td>' + "<input type='text' class='form-control form-control-sm' value=" + item.Checkbook_ID + '-' + item.CurrencyName + " readonly />" + '</td>';
                    html += '<td>' + "<input type='text' class='form-control form-control-sm' value=" + item.Balance + " readonly />" + '</td>';
                    html += '<td>' + "<input type='text' class='form-control form-control-sm' value=" + item.CashAccountBalance2 + " readonly />" + '</td>';
                    html += '<td>' + "<input type='text' name='revaluateSystemRate' class='form-control form-control-sm TCGE-SystemRate' value='0' />" + '</td>';
                    html += '<td>' + "<input type='number' name='revaluateNewBalance' class='form-control form-control-sm' value='0' readonly />" + '</td>';
                    html += '<td>' + "<input type='number' name='revaluateProfitLoss' class='form-control form-control-sm' value='0' readonly />" + '</td>';
                    html += '<td hidden>' + "<input type='text' name='revaluateProfitLossAccount' class='form-control form-control-sm' readonly />" + '</td>';
                    html += '<td hidden>' + item.C_AID + '</td>';
                    html += '</tr>';
                });
                tableBody.html(html);
            }
        });
    }
}

function Finish_Revaluate() {
    var revaluateDate = $('#revaluateDate').val();
    $('#TCBR-appendData input[name=revaluateProfitLoss]').each(function () {
        headerObj = {};
        mainArr = [];
        var rowProfitLoss = $(this).val(),
            thisRow = $(this).closest('tr'),
            rowAccountID = thisRow.find('td:eq(3)').find('select').val(),
            checkbookAccountID = thisRow.find('td:eq(8)').text(),
            currencyAccountID = thisRow.find('td:eq(7)').find('input').val();

        // Fill The Header Object
        headerObj = {
            C_CBID: '',
            C_PostingDate: revaluateDate,
            C_TransactionDate: revaluateDate,
            C_Refrence: 'Revaluate On ' + ConverDate(Date.now()),
            CurrencyID: companyID,
            C_SystemRate: 1,
            C_TransactionRate: 1,
            C_PostingKey: 'TCBR',
            C_TransactionType: 'Checkbook Revaluate'
        };

        // profit
        if (parseFloat(rowProfitLoss) > 0) {
            mainArr.push({
                C_Describtion: 'Revaluate On ' + ConverDate(Date.now()),
                C_Document: 'UnS',
                C_AID: checkbookAccountID,
                C_OriginalAmount: rowProfitLoss,
                C_Debit: rowProfitLoss,
                C_Credit: 0
            });
            mainArr.push({
                C_Describtion: 'Revaluate On ' + ConverDate(Date.now()),
                C_Document: 'UnS',
                C_AID: currencyAccountID,
                C_OriginalAmount: rowProfitLoss,
                C_Debit: 0,
                C_Credit: rowProfitLoss
            });
            var journalNumber = PureTransactionSave(companyID, headerObj, mainArr);
            Talert('This Transaction Journal Entry Number Is >> ' + journalNumber);
        }
        // Loss
        if (parseFloat(rowProfitLoss) < 0) {
            var number = Math.abs(rowProfitLoss);
            mainArr.push({
                C_Describtion: 'Revaluate On ' + ConverDate(Date.now()),
                C_Document: 'UnS',
                C_AID: currencyAccountID,
                C_OriginalAmount: number,
                C_Debit: number,
                C_Credit: 0
            });
            mainArr.push({
                C_Describtion: 'Revaluate On ' + ConverDate(Date.now()),
                C_Document: 'UnS',
                C_AID: checkbookAccountID,
                C_OriginalAmount: number,
                C_Debit: 0,
                C_Credit: number
            });
            var journalNumber = PureTransactionSave(companyID, headerObj, mainArr);
            Talert('This Transaction Journal Entry Number Is >> ' + journalNumber);
        }
    });
}
function Finish_Reverse_Revaluate() {
    var reverseDate = $('#revaluateReverseDate').val();
    $('#TCBR-appendData input[name=revaluateProfitLoss]').each(function () {
        headerObj = {};
        mainArr = [];
        var rowProfitLoss = $(this).val(),
            thisRow = $(this).closest('tr'),
            rowAccountID = thisRow.find('td:eq(3)').find('select').val(),
            checkbookAccountID = thisRow.find('td:eq(8)').text(),
            currencyAccountID = thisRow.find('td:eq(7)').find('input').val();

        // Fill The Header Object
        headerObj = {
            C_CBID: '',
            C_PostingDate: reverseDate,
            C_TransactionDate: reverseDate,
            C_Refrence: 'Revaluate On ' + ConverDate(Date.now()),
            CurrencyID: companyID,
            C_SystemRate: 1,
            C_TransactionRate: 1,
            C_PostingKey: 'TCBR',
            C_TransactionType: 'Checkbook Reverse Revaluate'
        };

        // profit
        if (parseFloat(rowProfitLoss) > 0) {
            mainArr.push({
                C_Describtion: 'Revaluate On ' + ConverDate(Date.now()),
                C_Document: 'UnS',
                C_AID: checkbookAccountID,
                C_OriginalAmount: rowProfitLoss,
                C_Debit: 0,
                C_Credit: rowProfitLoss
            });
            mainArr.push({
                C_Describtion: 'Revaluate On ' + ConverDate(Date.now()),
                C_Document: 'UnS',
                C_AID: currencyAccountID,
                C_OriginalAmount: rowProfitLoss,
                C_Debit: rowProfitLoss,
                C_Credit: 0
            });
            PureTransactionSave(companyID, headerObj, mainArr);
        }
        // Loss
        if (parseFloat(rowProfitLoss) < 0) {
            var number = Math.abs(rowProfitLoss);
            mainArr.push({
                C_Describtion: 'Revaluate On ' + ConverDate(Date.now()),
                C_Document: 'UnS',
                C_AID: currencyAccountID,
                C_OriginalAmount: number,
                C_Debit: 0,
                C_Credit: number
            });
            mainArr.push({
                C_Describtion: 'Revaluate On ' + ConverDate(Date.now()),
                C_Document: 'UnS',
                C_AID: checkbookAccountID,
                C_OriginalAmount: number,
                C_Debit: number,
                C_Credit: 0
            });
            PureTransactionSave(companyID, headerObj, mainArr);
        }
    });
}