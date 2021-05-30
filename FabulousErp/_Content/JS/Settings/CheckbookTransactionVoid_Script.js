var checkCurrencyID,
    PTcheck = $("#TCGV-PT").text(),
    companyID = $("#TCGE-CompanyID").text();

$(document).ready(function () {
    $('#TCGE-JEDate').focusout(function () {
        var transactionDate = ConverDate($(this).val()),
            oldDate = ConverDate($('#C_TransactionDate').text());
        if (transactionDate < oldDate) {
            Talert('Error!,Choosen Date Is Before Original Transaction-Date');
            $(this).val('');
        }
    });

    $('#TCGE-PostingDate').focusout(function () {
        var postingDate = ConverDate($(this).val()),
            oldDate = ConverDate($('#C_PostingDate').text());
        if (postingDate < oldDate) {
            Talert('Error!,Choosen Date Is Before Original Posting-Date');
            $(this).val('');
        }
    });

    $('#CBT-Void').click(function () {
        var companyID = $("#TCGE-CompanyID").text(),
            CheckbookID = $('#CBT-checkbookID').val(),
            jeNum = $('#TCGV-JENum').val(),
            transactionDate = $('#TCGE-JEDate').val(),
            postingDate = $('#TCGE-PostingDate').val(),
            ptCheck = $('#TCGV-PT').text(),
            postingKey,
            postingKeyVoid = $('#documentNumberName').text(),
            transactionType,
            currency = $('#TCGE-CurrencyID').val(),
            SystemRate = $('#TCGE-SystemRate').val().replace(regRemoveCurrFormate, ""),
            TransactionRate = $('#TCGE-TransactionRate').val().replace(regRemoveCurrFormate, ""),
            Difference = $("#TCGE-DiffrenceRate").val().replace(regRemoveCurrFormate, ""),
            NextDepositNumber = $('#CBT-nextDeposit').text(),
            NextWithdrawNumber = $('#CBT-nextWithdraw').text(),
            DocumentType = $('#C_DocumentType').text(),
            ReceivedFrom_PayTo = $('#CBT_receivedFrom_payTo').val(),
            Receipt_PaymentAmount = $('#CBT-amount').val(),
            reference = $('#TCGE-Reference').val(),
            Bank_CheckNumber = $('#CBT-checkNumber').val(),
            Bank_DueDate = $('#CBT-dueDate').val(),
            identityVoid = $('#C_CBT').text(),
            PathLink = window.location.pathname,
            test = true;

        switch (PathLink) {
            case "/C_CheckbookVoid/Cash":
                postingKey = "TCVC";
                transactionType = "Company Void Cash";
                break;
            case "/C_CheckbookVoid/Bank":
                postingKey = "TBVC";
                transactionType = "Company Void Bank";
                break;
        }

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

        if (postingDate.length === 0) {
            $('#TCGE-PostingDate').css('border-color', 'red');
            test = false;
        }  else {
            $('#TCGE-PostingDate').css('border-color', '');
        }

        if (test==true) {
            CheckPostingDateInPeriods(postingDate, function (test) {
                if (test == true) {
                    var PTOR;
                    if (ptCheck === "A1") {
                        PTOR = 1;
                    } else {
                        PTOR = 2;
                    }

                       InsertTransactionData(companyID, PTOR, postingDate, transactionDate, reference, currency, SystemRate, TransactionRate, postingKey, transactionType, '',
                        jeNum, function (JN, PO) {
                            // Post Company-Checkbook-Transactions
                            $.ajax({
                                type: "POST",
                                url: "/C_CashReciept/SaveTransactions?CheckbookID=" + CheckbookID + "&PostingKey=" + postingKey + "&Currency=" + currency + "&TransactionDate=" + transactionDate + "&PostingDate=" + postingDate +
                                    "&SystemRate=" + SystemRate + "&TransactionRate=" + TransactionRate + "&Difference=" + Difference +
                                    "&Reference=" + reference + "&DocumentType=" + DocumentType +
                                    "&ReceivedFrom=" + ReceivedFrom_PayTo + "&Receipt_PaymentAmount=" + Receipt_PaymentAmount + "&journalEntryNumber=" + JN +
                                    "&Bank_CheckNumber=" + Bank_CheckNumber + "&Bank_DueDate=" + Bank_DueDate + "&identityVoid=" + identityVoid + "&postingKeyVoid=" + postingKeyVoid,
                                success: function (result) {
                                    if (ptCheck === 'A2') {
                                        window.open(
                                            '/C_ReportsPrint/Done?searchNumber=' + JN,
                                            '_blank'
                                        );
                                        RedirectInt(window.location.href)
                                    }
                                    //switch (PathLink) {
                                    //    case "/C_CashReciept/CompanyCashReciept":
                                    //        Talert('Transaction Sucessfully , Deposit Number = ' + NextDepositNumber);
                                    //        break;
                                    //    case "/C_CashWithdraw/CompanyCashWithdraw":
                                    //        Talert('Transaction Sucessfully , Withdraw Number = ' + NextWithdrawNumber);
                                    //        break;
                                    //    case "/C_BankCheckReceived/CompanyBankCheckReceived":
                                    //        Talert('Transaction Sucessfully , Withdraw Number = ' + NextDepositNumber);
                                    //        break;
                                    //    case "/C_BankCheckOut/CompanyBankCheckOut":
                                    //        Talert('Transaction Sucessfully , Withdraw Number = ' + NextWithdrawNumber);
                                    //        break;
                                    //}
                                }
                            });
                        });
                   
                } else {
                    $('#TCGE-PostingDate').css('border-color', 'red');
                }
                
            })
        }
    });

    $('#CBT-transferVoid').click(function () {
        SaveTransferCheckbook();
    });

    $('#TCGV-JENum').change(function () {
        var jvNumber = $(this).val(),
            tableBody = $('#TCVVT-table > tbody');
        if (tableBody.length == 0) {
            tableBody = $('#TCT-table > tbody');
        }
        $.ajax({
            type: "GET",
            url: "/C_CheckbookVoid/GetCheckData?jvNumber=" + jvNumber,
            success: function (result) {
                tableBody.empty();
                $.each(result, function (index, res) {
                    checkCurrencyID = res.CurrencyID;
                    var Data = "<tr>" +
                        "<td>" + res.CheckNumber + "</td>" +
                        "<td>" + res.Date + "</td>" +
                        "<td>" + res.Balance + "</td>" +
                        "<td>" + res.RecievedFrom + "</td>" +
                        "</tr>";
                    tableBody.append(Data);
                });
                DataTable();
            }
        });
        try {
            GetFiles(jvNumber);
        } catch (err) {

        }
    });
});



function SaveTransferCheckbook() {
    // transfer-from checkbook
    var companyID = $("#TCGE-CompanyID").text(),
        voidDate = $('#TCGE-JEDate').val(),
        jeNum = $('#TCGV-JENum').val(),
        ptCheck = $('#TCGV-PT').text(),
        transferReference = $('#TCVT-Reference').val(),
        transactionType = "Company Void Transfer",
        postingKey = "TCVT",
        test = true,

        paymentAmount = $('#transferFrom').find('.paymentCheck').text(),
        first_Balance = $('#transferFrom').find('.C_Balance').text(),
        first_IdentityVoid = $('#transferFrom').find('.C_CBT').text(),
        first_CheckbookID = $('#transferFrom').find('.CBT-checkbookID').val(),
        first_CheckbookName = $('#transferFrom').find('.CBT-checkbookName').val(),
        first_Currency = $('#transferFrom').find('.CBT-CurrencyID').val(),
        first_SystemRate = $('#transferFrom').find('.CBT-SystemRate').val().replace(regRemoveCurrFormate, ""),
        first_TransactionRate = $('#transferFrom').find('.CBT-TransactionRate').val().replace(regRemoveCurrFormate, ""),
        first_Difference = $('#transferFrom').find('.CBT-DiffrenceRate').val().replace(regRemoveCurrFormate, ""),
        first_Description = $('#transferFrom').find('.CBT-Reference').val(),
        first_Amount = $('#transferFrom').find('.CBT-amount').val().replace(regRemoveCurrFormate, ""),
        // transfer-to checkbook
        recieptAmount = $('#transferTo').find('.recieptCheck').text(),
        second_Balance = $('#transferTo').find('.C_Balance').text(),
        second_IdentityVoid = $('#transferTo').find('.C_CBT').text(),
        second_CheckbookID = $('#transferTo').find('.CBT-checkbookID').val(),
        second_CheckbookName = $('#transferTo').find('.CBT-checkbookName').val(),
        second_Currency = $('#transferTo').find('.CBT-CurrencyID').val(),
        second_SystemRate = $('#transferTo').find('.CBT-SystemRate').val().replace(regRemoveCurrFormate, ""),
        second_TransactionRate = $('#transferTo').find('.CBT-TransactionRate').val().replace(regRemoveCurrFormate, ""),
        second_Difference = $('#transferTo').find('.CBT-DiffrenceRate').val().replace(regRemoveCurrFormate, ""),
        second_Description = $('#transferTo').find('.CBT-Reference').val(),
        final_Amount = $('#transferTo').find('.CBT-amount').val().replace(regRemoveCurrFormate, "");


    if (jeNum.length === 0) {
        $('#TCGV-JENum').css('border-color', 'red');
        test = false;
    } else {
        $('#TCGV-JENum').css('border-color', '');
    }

    if (voidDate.length === 0) {
        $('#TCGE-JEDate').css('border-color', 'red');
        test = false;
    } else {
        $('#TCGE-JEDate').css('border-color', '');
    }


    if (test === true) {
        // Check if SPS Posting to General Ladger OR Posting throw General Ladger
        var PTOR;
        if (ptCheck === "A1") {
            PTOR = 1;
        } else {
            PTOR = 2;
        }

        InsertTransactionData(companyID, PTOR, voidDate, voidDate, transferReference, first_Currency, first_SystemRate, first_TransactionRate, postingKey, transactionType, '', jeNum
            , function (journalEntryNumber,Po) {
                var transferArray = [
                    {
                        C_CBTVoid: first_IdentityVoid,
                        C_PostingNumber: Po,
                        C_TransactionDate: voidDate,
                        C_PostingDate: voidDate,
                        C_CBSID: first_CheckbookID,
                        CurrencyID: first_Currency,
                        C_SystemRate: first_SystemRate,
                        C_TransactionRate: first_TransactionRate,
                        C_Difference: first_Difference,
                        C_Reference: first_Description,
                        C_DocumentType: "SID",
                        C_Payment: parseFloat(paymentAmount) * -1,
                        C_Reciept: 0,
                        C_Balance: parseFloat(first_Balance) * -1,
                        C_CheckNumber: '',
                        C_PostingKey: postingKey
                    },
                    {
                        C_CBTVoid: second_IdentityVoid,
                        C_PostingNumber: Po,
                        C_TransactionDate: voidDate,
                        C_PostingDate: voidDate,
                        C_CBSID: second_CheckbookID,
                        CurrencyID: second_Currency,
                        C_SystemRate: second_SystemRate,
                        C_TransactionRate: second_TransactionRate,
                        C_Difference: second_Difference,
                        C_Reference: second_Description,
                        C_DocumentType: "SID",
                        C_Reciept: parseFloat(recieptAmount) * -1,
                        C_Payment: 0,
                        C_Balance: parseFloat(second_Balance) * -1,
                        C_CheckNumber: '',
                        C_PostingKey: postingKey
                    }
                ];
                transferArray = JSON.stringify({ 'transferArray': transferArray });
                $.ajax({
                    contentType: 'application/json; charset=utf-8',
                    dataType: 'json',
                    type: "POST",
                    url: "/C_CheckbookVoid/TransferVoid",
                    data: transferArray,
                    success: function (result) {
                        if (result === "True") {
                            if (ptCheck === 'A2') {
                                window.open(
                                    '/C_ReportsPrint/Done?searchNumber=' + journalEntryNumber,
                                    '_blank'
                                );
                                location.reload();
                            }
                        }
                    }
                });
        });

    
    }
}
function Save_Check_Transfer_Void() {
    var companyID = $("#TCGE-CompanyID").text(),
        voidDate = $('#TCGE-JEDate').val(),
        jeNum = $('#TCGV-JENum').val(),
        ptCheck = $('#TCGV-PT').text(),
        transferReference = 'Check Void Transfer',
        transactionType = "Company Void Check Transfer",
        postingKey = "TCV",
        test = true,

        // transfer-from checkbook
        paymentAmount = $('#transferFrom').find('.paymentCheck').text(),
        first_Balance = $('#transferFrom').find('.C_Balance').text(),
        first_IdentityVoid = $('#transferFrom').find('.C_CBT').text(),
        first_CheckbookID = $('#transferFrom').find('.CBT-checkbookID').val(),
        first_CheckbookName = $('#transferFrom').find('.CBT-checkbookName').val(),
        first_Currency = $('#transferFrom').find('.CBT-CurrencyID').val(),
        first_SystemRate = $('#transferFrom').find('.CBT-SystemRate').val().replace(regRemoveCurrFormate, ""),
        first_TransactionRate = $('#transferFrom').find('.CBT-TransactionRate').val().replace(regRemoveCurrFormate, ""),
        first_Difference = $('#transferFrom').find('.CBT-DiffrenceRate').val().replace(regRemoveCurrFormate, ""),
        first_Description = $('#transferFrom').find('.CBT-Reference').val(),
        first_Amount = $('#transferFrom').find('.CBT-amount').val().replace(regRemoveCurrFormate, ""),
        // transfer-to checkbook
        recieptAmount = $('#transferTo').find('.recieptCheck').text(),
        second_Balance = $('#transferTo').find('.C_Balance').text(),
        second_IdentityVoid = $('#transferTo').find('.C_CBT').text(),
        second_CheckbookID = $('#transferTo').find('.CBT-checkbookID').val(),
        second_CheckbookName = $('#transferTo').find('.CBT-checkbookName').val(),
        second_Currency = $('#transferTo').find('.CBT-CurrencyID').val(),
        second_SystemRate = $('#transferTo').find('.CBT-SystemRate').val().replace(regRemoveCurrFormate, ""),
        second_TransactionRate = $('#transferTo').find('.CBT-TransactionRate').val().replace(regRemoveCurrFormate, ""),
        second_Difference = $('#transferTo').find('.CBT-DiffrenceRate').val().replace(regRemoveCurrFormate, ""),
        second_Description = $('#transferTo').find('.CBT-Reference').val(),
        final_Amount = $('#transferTo').find('.CBT-amount').val().replace(regRemoveCurrFormate, "");
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

//--------------------------------------- Check Transfer Void -------------------------------------------
$('#CBT-checkTransferVoid').click(function () {
    Check_Transfer_Void_JV();
});
//function Update_Void_Reconcile() {
//    var reconcileArray = [];
//    $('#TCVVT-table > tbody > tr').each(function () {
//        reconcileArray.push(
//            {
//                C_CBSID: $('#transferFrom').find('.CBT-checkbookID').val(),
//                C_CheckNumber: $(this).find('td:eq(0)').text()
//            });
//    });
//    reconcileArray = JSON.stringify({ 'reconcileArray': reconcileArray });
//    $.ajax({
//        contentType: 'application/json; charset=utf-8',
//        dataType: 'json',
//        type: "POST",
//        url: "/C_CheckbookVoid/UpdateVoidReconcile",
//        data: reconcileArray,
//        success: function (result) {
//            if (result === "True") {
//                Check_Transfer_Void_JV();
//            }
//        }
//    });
//}
function Check_Transfer_Void_JV() {
    var transferArray = [],
        transactionDate = $('#TCGE-JEDate').val(),
        postingDate = $('#TCGE-PostingDate').val(),
        transactionRate = $('#TCGE-TransactionRate').val().replace(regRemoveCurrFormate, ""),
        reference = 'Transfer Void Check To Bank',
        documentType = 'SID',
        postingKey = 'TCVVT';

    var PTOR;
    if (PTcheck === "A1") {
        PTOR = 1;
    } else {
        PTOR = 2;
    }

    InsertTransactionData(companyID, PTOR, postingDate, transactionDate, reference, checkCurrencyID, transactionRate, transactionRate, postingKey, 'Check Void Transfer', '', '',
        function (journalEntryNumber,PO) {
            var counter = 0;
            $('#TCVVT-table > tbody > tr').each(function () {
                var checkNumber = $(this).find('td:eq(0)').text(),
                    dueDate = $(this).find('td:eq(1)').text(),
                    originalAmount = $(this).find('td:eq(2)').text(),
                    recievedFrom = $(this).find('td:eq(3)').text();

                first_IdentityVoid = $('#transferFrom').find('.C_CBT').find('span:eq(' + counter + ')').text();
                second_IdentityVoid = $('#transferTo').find('.C_CBT').find('span:eq(' + counter + ')').text();
                counter++;

                transferArray.push(
                    {
                        C_CBTVoid: first_IdentityVoid,
                        C_PostingNumber: PO,
                        C_TransactionDate: transactionDate,
                        C_PostingDate: postingDate,
                        C_CBSID: $('#transferFrom').find('.CBT-checkbookID').val(),
                        CurrencyID: checkCurrencyID,
                        C_SystemRate: transactionRate,
                        C_TransactionRate: transactionRate,
                        C_Difference: 0,
                        C_Reference: reference,
                        C_DocumentType: documentType,
                        C_Payment: 0,
                        C_Reciept: originalAmount,
                        C_Balance: originalAmount,
                        C_CheckNumber: checkNumber,
                        C_PostingKey: postingKey,
                        C_Payment_To_Recieved_From: recievedFrom
                    },
                    {
                        C_CBTVoid: second_IdentityVoid,
                        C_PostingNumber: PO,
                        C_TransactionDate: transactionDate,
                        C_PostingDate: postingDate,
                        C_CBSID: $('#transferTo').find('.CBT-checkbookID').val(),
                        CurrencyID: checkCurrencyID,
                        C_SystemRate: transactionRate,
                        C_TransactionRate: transactionRate,
                        C_Difference: 0,
                        C_Reference: reference,
                        C_DocumentType: documentType,
                        C_Payment: originalAmount,
                        C_Reciept: 0,
                        C_Balance: originalAmount,
                        C_CheckNumber: checkNumber,
                        C_PostingKey: postingKey,
                        C_Payment_To_Recieved_From: recievedFrom
                    });
            });
            transferArray = JSON.stringify({ 'transferArray': transferArray });
            ptCheck = $('#TCGV-PT').text(),

                $.ajax({
                    contentType: 'application/json; charset=utf-8',
                    dataType: 'json',
                    type: "POST",
                    url: "/C_CheckbookVoid/CheckVoidTransfer",
                    data: transferArray,
                    success: function (result) {

                        if (result === "True") {
                            if (ptCheck === 'A2') {
                                window.open(
                                    '/C_ReportsPrint/Done?searchNumber=' + journalEntryNumber,
                                    '_blank'
                                );
                            }
                            location.reload();
                        }
                    }
                });
        });

  
}


