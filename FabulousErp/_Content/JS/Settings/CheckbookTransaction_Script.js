$(document).ready(function () {

    // Add First Row
    var tbody = $('#TCGE-TTbl');
    var tbodyData = "<tr>" +
        "<td>" + '<button type="button" class="btn btn-sm mr-1 MoreDetailsT"><span class="fa fa-eye"></span></button>' + "</td>" +
        "<td class='hide-normal'></td>" +
        "<td></td>" +
        "<td></td>" +
        "<td class='hide-normal'></td>" +
        "<td class='hide-normal'></td>" +
        "<td></td>" +
        "<td class='sDebitTbl'></td>" +
        "<td class='sCreditTbl'></td>" +
        "</tr>";
    tbody.append(tbodyData);

    var companyID = $("#TCGE-CompanyID").text();
    var EPDcheck = $("#TCGE-EPD").text();
    var fJEPer = $("#TCGE-FJEPer").text();

    if (fJEPer === "NoPS") {
        Talert("This Company in Financial Module Not have Posting Setup..!");
        window.location.href = "/Home/Financial_Home";
    }

    // 1.0 Transaction-Date (Today By Default)
    // Posting-Date (Date related to Transaction-Date)
    // Get the System-Rates Related to currency and Transaction-Date
    $('#CBT-transactionDate').focus(function () {
        if ($('#CBT-checkbookID').val().length === 0) {
            $('#CBT-checkbookID').focus();
        } else {
            var now = new Date();
            var day = ("0" + now.getDate()).slice(-2);
            var month = ("0" + (now.getMonth() + 1)).slice(-2);
            var today = now.getFullYear() + "-" + (month) + "-" + (day);
            $('#CBT-transactionDate').val(today);
        }
        //----------- for validation
        if ($(this).val().length > 0) {
            $(this).css("border", "");
        }
    });
    $('#CBT-transactionDate').focusout(function () {
        if (EPDcheck === "F2") {
            $('#CBT-postingDate').val($(this).val());
            var currencyID = $('#TCGE-CurrencyID').val();
            var postingDate = $(this).val();

            CheckPostingDateInPeriods(postingDate, function (checkPostingDate) {
                if (checkPostingDate !== true) {
                    $("#TCGE-SystemRate").val("");
                    $("#TCGE-TransactionRate").val("");
                    $("#TCGE-DiffrenceRate").val("");
                    $("#TCGE-TransactionRate").prop("disabled", true);
                } else {
                    GetCurrencyRates(currencyID, postingDate);
                }
            });
           
        }
        $('#CBT-amount').val("");
    });

    // 2.0 get the System-Rates Related to currency and Posting-Date
    $('#CBT-postingDate').focusout(function () {
        $('#CBT-amount').val("");
        var postingDate = $(this).val();
        var currencyID = $('#TCGE-CurrencyID').val();

        CheckPostingDateInPeriods(postingDate, function (checkPostingDate) {
            if (checkPostingDate !== true) {
                $("#TCGE-SystemRate").val("");
                $("#TCGE-TransactionRate").val("");
                $("#TCGE-DiffrenceRate").val("");
                $("#TCGE-TransactionRate").prop("disabled", true);
            } else {
                GetCurrencyRates(currencyID, postingDate);
            }
        });
       
    }).focusin(function () {
        if ($('#CBT-checkbookID').val().length === 0) {
            $('#CBT-checkbookID').focus();
        } else {
            var transactionDate = $('#CBT-transactionDate').val();
            $(this).val(transactionDate);
        }
        //----------- for validation
        if ($(this).val().length > 0) {
            $(this).css("border", "");
        }
    });

    // 3.0 Check Checkbook Security Before Get Data
    $("#CBT-checkbookID").change(function () {
        Clear();
        $("#TCGE-AccountID option").show();
        var checkbookID = $(this).val();
        if (checkbookID.length == 0) {
            Clear();
        } else {
            $.ajax({
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                method: "get",
                url: "/api/CheckBookAPIs/CheckbookSecurity?checkbookID=" + checkbookID + "&companyID=" + companyID,
                success: function (result) {
                    if (result.CB_Password === "Exist") {
                        $("#CBT-CBid").text(checkbookID);
                        $("#CBT-checkbookID").val("");
                        $("#CBT-checkbookName").val("");
                        $("#CBT-checkbookModal").modal("show");
                    } else if (result.CB_UserID === "UserIDAccess") {
                        GetCheckbookData(checkbookID);
                    } else if (result.CB_UserID === "NoPermit") {
                        $("#CBT-checkbookID").val("");
                        $("#CBT-checkbookName").val("");
                        Talert("you not have access to this checkbook");
                    } else {
                        GetCheckbookData(checkbookID);
                    }
                }
            });
        }
        //----------- for validation
        if ($(this).val().length === 0) {
            $(this).css("border", "1px solid red");
        } else {
            $(this).css("border", "");
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
                GetCheckbookData(checkbookID);
                $("#CBT-checkbookID").val(checkbookID);
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
                    $("#CBT-wrongpass").text(JSON.parse(request.responseText).Message);
                }
            }
            */
        });
    });

    // 4.0 Check If The Amount Between Max-Amount and Min.Amount OR Not
    // Fill The First-Row of Debit If Location Is (Cash Recipt)
    // Fill The First-Row of Credit If Location Is (Cash Withdraw)
    $('#CBT-amount').focusout(function () {
        var Amount = $(this).val().replace(regRemoveCurrFormate, "");
        var TransactionRate = $('#TCGE-TransactionRate').val().replace(regRemoveCurrFormate, "");
        var MaxAmount = $('#CBT-maxAmount').text();
        var MinAmount = $('#CBT-minAmount').text();
        var describtion = $('#CBT-describtion').val();
        if (describtion === undefined) {
            describtion = $("#TCGE-Reference").val();
        }
        var document = $('#CBT-document').val();
        if (document === undefined) {
            document = $("#TCGE-SUD").val();
        }
        var ReceivedFrom_PayTo = $('#CBT_receivedFrom_payTo').val();
        var transactionDate = $('#CBT-transactionDate').val();
        var postingDate = $('#CBT-postingDate').val();

        var test = true;
        try {
            if (document.length === 0) {
                test = false;
                $('#CBT-document').css("border", "1px solid red");
                $(this).val("");
            } else {
                $('#CBT-document').css("border", "");
            }
        } catch (err) {

        }
        
        if (ReceivedFrom_PayTo.length === 0) {
            test = false;
            $('#CBT_receivedFrom_payTo').css("border", "1px solid red");
            $(this).val("");
        } else {
            $('#CBT_receivedFrom_payTo').css("border", "");
        }
        if (transactionDate.length === 0) {
            test = false;
            $('#CBT-transactionDate').css("border", "1px solid red");
            $(this).val("");
        } else {
            $('#CBT-transactionDate').css("border", "");
        }
        if (postingDate.length === 0) {
            test = false;
            $('#CBT-postingDate').css("border", "1px solid red");
            $(this).val("");
        } else {
            $('#CBT-postingDate').css("border", "");
        }

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
                if ($("#TCGE-OriginalAmount").is(":hidden")) {
                    tbody.find('tr').eq(0).find('td').eq(6).text(setSystemCurrFormate(+parseFloat(Amount)));
                } else {
                    tbody.find('tr').eq(0).find('td').eq(6).text(setHardCurrFormate(+parseFloat(Amount)));
                }
                var dORc = parseFloat(Amount) * parseFloat(TransactionRate);
                // Fill The First-Row of Debit If Location Is (Cash Recipt && Bank Check received)
                if (window.location.pathname === "/C_CashReciept/CompanyCashReciept" || window.location.pathname === "/C_BankCheckReceived/CompanyBankCheckReceived") {
                    tbody.find('tr').eq(0).find('td').eq(7).text(setSystemCurrFormate(+parseFloat(dORc)));
                    tbody.find('tr').eq(0).find('td').eq(8).text("0");
                    // Fill The First-Row of Credit If Location Is (Cash Withdraw && Bank Check out)
                } else if (window.location.pathname === "/C_CashWithdraw/CompanyCashWithdraw" || window.location.pathname === "/C_BankCheckOut/CompanyBankCheckOut") {
                    tbody.find('tr').eq(0).find('td').eq(8).text(setSystemCurrFormate(+parseFloat(dORc)));
                    tbody.find('tr').eq(0).find('td').eq(7).text("0");
                }
                SumDebitAndCredit();
            }
        }
        //----------- for validation
        if ($(this).val().length === 0) {
            $(this).css("border", "1px solid red");
        } else {
            $(this).css("border", "");
        }
    });

    // 5.0 Fill The First-Row -----------------------------------------
    $('#CBT-document,#TCGE-SUD').change(function () {
        var document = $(this).val();
        tbody.find('tr').eq(0).find('td').eq(3).text(document);
        //----------- for validation
        if ($(this).val().length === 0) {
            $(this).css("border", "1px solid red");
        } else {
            $(this).css("border", "");
        }
    });


    // Check If Document-Number Is Exist Or Not
    $('#CBT-checkNumber').focusout(function () {
        var documentNumber = $(this).val();
        $.ajax({
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            method: "get",
            url: "/api/CheckBookAPIs/DocumentNumberCheck?documentNumber=" + documentNumber + "&companyID=" + companyID,
            success: function (result) {
                $('#CBT-checkNumber').css({
                    "border": ""
                });
            },
            statusCode: {
                406: function (request) {
                    $('#CBT-checkNumber').val('');
                    $('#CBT-checkNumber').css({
                        "border": "1px solid red"
                    });
                }
            }
        });
    });


    //-------------------- validation on keydown
    $("#TCGE-Reference").keyup(function () {
        var reference = $(this).val();
        tbody.find('tr').eq(0).find('td').eq(5).text(reference);
        //----------- for validation
        if ($(this).val().length === 0) {
            $(this).css("border", "1px solid red");
        } else {
            $(this).css("border", "");
        }
    });
    $("#CBT_receivedFrom_payTo").keyup(function () {
        if ($(this).val().length === 0) {
            $(this).css("border", "1px solid red");
        } else {
            $(this).css("border", "");
        }
    });
    $('#TCGE-TransactionRate').change(function () {
        $('#CBT-amount').val("");
    });


    // --------------- Button to Post Company-Checkbook-Transactions
    $('#CBT-Post').click(function () {
        var PTcheck = $("#CBT-PT").text(),
            CompanyID = $("#TCGE-CompanyID").text(),
            CheckbookID = $("#CBT-checkbookID").val(),
            Currency = $("#TCGE-CurrencyID").val(),
            TransactionDate = $("#CBT-transactionDate").val(),
            PostingDate = $("#CBT-postingDate").val(),
            SystemRate = $("#TCGE-SystemRate").val().replace(regRemoveCurrFormate, ""),
            TransactionRate = $("#TCGE-TransactionRate").val().replace(regRemoveCurrFormate, ""),
            Difference = $("#TCGE-DiffrenceRate").val().replace(regRemoveCurrFormate, ""),
            Reference = $("#TCGE-Reference").val(),
            DocumentType = $("#CBT-document").val(),
            ReceivedFrom_PayTo = $("#CBT_receivedFrom_payTo").val(),
            NextDepositNumber = $('#CBT-nextDeposit').text(),
            NextWithdrawNumber = $('#CBT-nextWithdraw').text(),
            Receipt_PaymentAmount = $("#CBT-amount").val().replace(regRemoveCurrFormate, ""),
            Bank_CheckNumber = $('#CBT-checkNumber').val(),
            Bank_DueDate = $('#CBT-dueDate').val(),
            PostingKey,
            TransactionType,
            PathLink = window.location.pathname,
            check = true;

        
        var document = $('#CBT-document').val();
        if (document === undefined) {
            document = $("#TCGE-SUD").val();
        }

        switch (PathLink) {
            case "/C_CashReciept/CompanyCashReciept":
                PostingKey = "TCCR";
                TransactionType = "Company Cash Reciept";
                break;
            case "/C_CashWithdraw/CompanyCashWithdraw":
                PostingKey = "TCCW";
                TransactionType = "Company Cash WithDraw";
                break;
            case "/C_BankCheckReceived/CompanyBankCheckReceived":
                PostingKey = "TCBR";
                TransactionType = "Bank Check Received";
                break;
            case "/C_BankCheckOut/CompanyBankCheckOut":
                PostingKey = "TCBC";
                TransactionType = "Bank Check Out";
                break;
        }

        //------------Check if there's Required empty field-------------
        if (CheckbookID.length === 0) {
            $("#CBT-checkbookID").css("border", "1px solid red");
            check = false;
        }
        else {
            $("#CBT-checkbookID").css("border", "");
        }
        if (TransactionDate.length === 0) {
            $("#CBT-transactionDate").css("border", "1px solid red");
            check = false;
        }
        else {
            $("#CBT-transactionDate").css("border", "");
        }
        if (PostingDate.length === 0) {
            $("#CBT-postingDate").css("border", "1px solid red");
            check = false;
        }
        else {
            $("#CBT-postingDate").css("border", "");
        }
        if (Reference.length === 0) {
            $("#TCGE-Reference").css("border", "1px solid red");
            check = false;
        }
        else {
            $("#TCGE-Reference").css("border", "");
        }
        
        var DocumentType = $('#CBT-document').val();
        if (DocumentType === undefined) {
            DocumentType = $("#TCGE-SUD").val();
        }
        if (DocumentType == "") {
            DocumentType=  $("#TCGE-TTbl").find("tr").first().find("td:eq(3)").text();
        }
        if (DocumentType.length === 0) {
            $("#CBT-document").css("border", "1px solid red");
            check = false;
        }
        else {
            $("#CBT-document").css("border", "");
        }

        if (ReceivedFrom_PayTo.length === 0) {
            $("#CBT_receivedFrom_payTo").css("border", "1px solid red");
            check = false;
        }
        else {
            $("#CBT_receivedFrom_payTo").css("border", "");
        }
        if (Receipt_PaymentAmount.length === 0) {
            $("#CBT-amount").css("border", "1px solid red");
            check = false;
        }
        else {
            $("#CBT-amount").css("border", "");
        }
        //-------------------------------------------------------------------------
        if (check === true) {
            // Check if SPS Posting to General Ladger OR Posting throw General Ladger
            if ($("#BostingToORThrow").val() === "1") {
                InsertTransactionData(CompanyID, 1, PostingDate, TransactionDate, Reference, Currency, SystemRate, TransactionRate, PostingKey, TransactionType, null, null,
                    function (JN,PO) {
                        SendToCheckBook(JN, PO)
                    });
            } else {
                // A2
                 InsertTransactionData(CompanyID, 2, PostingDate, TransactionDate, Reference, Currency, SystemRate, TransactionRate, PostingKey, TransactionType, null, null
                    , function (JN, PO) {
                        SendToCheckBook(JN, PO)
                    });
            }
            // Post Company-Checkbook-Transactions
            //if (journalEntryNumber > 0) {
              
            //}

        }
    });
    function SendToCheckBook(JN, PO) {
        var PTcheck = $("#CBT-PT").text(),
            CompanyID = $("#TCGE-CompanyID").text(),
            CheckbookID = $("#CBT-checkbookID").val(),
            Currency = $("#TCGE-CurrencyID").val(),
            TransactionDate = $("#CBT-transactionDate").val(),
            PostingDate = $("#CBT-postingDate").val(),
            SystemRate = $("#TCGE-SystemRate").val().replace(regRemoveCurrFormate, ""),
            TransactionRate = $("#TCGE-TransactionRate").val().replace(regRemoveCurrFormate, ""),
            Difference = $("#TCGE-DiffrenceRate").val().replace(regRemoveCurrFormate, ""),
            Reference = $("#TCGE-Reference").val(),
            DocumentType = $("#CBT-document").val(),
            ReceivedFrom_PayTo = $("#CBT_receivedFrom_payTo").val(),
            NextDepositNumber = $('#CBT-nextDeposit').text(),
            NextWithdrawNumber = $('#CBT-nextWithdraw').text(),
            Receipt_PaymentAmount = $("#CBT-amount").val().replace(regRemoveCurrFormate, ""),
            Bank_CheckNumber = $('#CBT-checkNumber').val(),
            Bank_DueDate = $('#CBT-dueDate').val(),
            PostingKey,
            TransactionType,
            PathLink = window.location.pathname,
            check = true;

        var DocumentType = $('#CBT-document').val();
        if (DocumentType === undefined) {
            DocumentType = $("#TCGE-SUD").val();
        }
        if (DocumentType == "") {
            DocumentType = $("#TCGE-TTbl").find("tr").first().find("td:eq(3)").text();
        }
        if (DocumentType.length === 0) {
            $("#CBT-document").css("border", "1px solid red");
            check = false;
        }

        switch (PathLink) {
            case "/C_CashReciept/CompanyCashReciept":
                PostingKey = "TCCR";
                TransactionType = "Company Cash Reciept";
                break;
            case "/C_CashWithdraw/CompanyCashWithdraw":
                PostingKey = "TCCW";
                TransactionType = "Company Cash WithDraw";
                break;
            case "/C_BankCheckReceived/CompanyBankCheckReceived":
                PostingKey = "TCBR";
                TransactionType = "Bank Check Received";
                break;
            case "/C_BankCheckOut/CompanyBankCheckOut":
                PostingKey = "TCBC";
                TransactionType = "Bank Check Out";
                break;
        }
        if (!Bank_DueDate) {
            Bank_DueDate = null;
        }
        $.ajax({
            type: "POST",
            url: "/C_CashReciept/SaveTransactions?CheckbookID=" + CheckbookID + "&PostingKey=" + PostingKey + "&Currency=" + Currency + "&TransactionDate=" + TransactionDate + "&PostingDate=" + PostingDate +
                "&SystemRate=" + SystemRate + "&TransactionRate=" + TransactionRate + "&Difference=" + Difference +
                "&Reference=" + Reference + "&DocumentType=" + DocumentType +
                "&ReceivedFrom=" + ReceivedFrom_PayTo + "&Receipt_PaymentAmount=" + Receipt_PaymentAmount + "&journalEntryNumber=" + PO +
                "&Bank_CheckNumber=" + Bank_CheckNumber + "&Bank_DueDate=" + Bank_DueDate,
            success: function (result) {
                switch (PathLink) {
                    case "/C_CashReciept/CompanyCashReciept":
                        Talert('Transaction Sucessfully , Deposit Number = ' + NextDepositNumber);
                        break;
                    case "/C_CashWithdraw/CompanyCashWithdraw":
                        Talert('Transaction Sucessfully , Withdraw Number = ' + NextWithdrawNumber);
                        break;
                    case "/C_BankCheckReceived/CompanyBankCheckReceived":
                        Talert('Transaction Sucessfully , Deposit Number = ' + NextDepositNumber);
                        break;
                    case "/C_BankCheckOut/CompanyBankCheckOut":
                        Talert('Transaction Sucessfully , Withdraw Number = ' + NextWithdrawNumber);
                        break;
                }
                if (PTcheck === 'A2') {
                    window.open(
                        '/C_ReportsPrint/Done?searchNumber=' + PO,
                        '_blank'
                    );
                }
                window.open(
                    '/C_CashReciept/PrintRecipt?JN=' + PO,
                    '_blank'
                );
                RedirectInt(window.location.href);
            }
        });
}


    function Clear() {
        tbody.find('tr').eq(0).find('td').eq(2).text("");
        tbody.find('tr').eq(0).find('td').eq(3).text("");
        tbody.find('tr').eq(0).find('td').eq(4).text("");
        tbody.find('tr').eq(0).find('td').eq(5).text("");
        tbody.find('tr').eq(0).find('td').eq(6).text("");
        tbody.find('tr').eq(0).find('td').eq(7).text("");
        tbody.find('tr').eq(0).find('td').eq(8).text("");

        $("#CBT-checkbookName").val("");
        $("#CBT-transactionDate").val("");
        $("#CBT-postingDate").val("");
        $("#CBT_receivedFrom_payTo").val("");
        $("#CBT-postingDate").val("");
        $("#CBT-amount").val("");
        $("#TCGE-CurrencyID").empty();
        $("#TCGE-SystemRate").val("");
        $("#TCGE-TransactionRate").val("");
        $("#TCGE-DiffrenceRate").val("");
        $("#TCGE-Reference").val("");
        $("#CBT-document").val("");
        $("#CBT-describtion").val("");
    }
    $('#CBT-checkbookModal').on('hidden.bs.modal', function (e) {
        $(this)
            .find("input")
            .val('')
            .end()
            .find("label").text('').end();
    });
});

function GetCheckbookData(checkbookID) {
    var companyID = $("#TCGE-CompanyID").text();
    var tbody = $('#TCGE-TTbl');
    $("#TCGE-CurrencyID").empty();
    $("#TCGE-CurrencyID").append("<option value=''></option>");
    $("#TCGE-CurrencyID").selectpicker('refresh');
    $.ajax({
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        method: "get",
        url: "/api/CheckBookAPIs/GetCheckbookData?checkbookID=" + checkbookID + "&companyID=" + companyID,
        success: function (result) {
            $("#TCGE-CurrencyID").append("<option value='" + result.CurrencyID + "'>" + result.CurrencyName + "</option>");
            $("#TCGE-CurrencyID").val(result.CurrencyID)
            $("#TCGE-CurrencyID").selectpicker('refresh');
            //$("select#TCGE-CurrencyID").attr("disabled","disabled")
            //$("button#TCGE-CurrencyID").attr("disabled", "disabled")
            $("#CBT-checkbookName").val(result.CheckbookName);
            $('#CBT-minAmount').text(result.MinAmount);
            $('#CBT-maxAmount').text(result.MaxAmount);

            $('#CBT-nextDeposit').text(result.NextDepositNumber);
            $('#CBT-nextWithdraw').text(result.NextWithdrawNumber);

            // Fill The First-Row of Account-Name
            tbody.find('tr').eq(0).find('td').eq(2).text(result.Company_AccountsName);

            // Fill The First-Row of Account-ID
            tbody.find('tr').eq(0).find('td').eq(4).text(result.Company_AccountsID);
            tbody.find('tr').eq(0).find('td').eq(1).text(result.C_AID);

            $("#TCGE-AccountID option[value=" + result.C_AID + "]").hide();

            if (result.CurrencyID === companyID) {
                // so currency is main
                $('#TCCR-rateField').hide();
            } else {
                $('#TCCR-rateField').show();
            }
        }
    });
}
