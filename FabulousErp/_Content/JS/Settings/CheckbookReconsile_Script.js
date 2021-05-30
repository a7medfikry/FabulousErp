/*import { parse } from "url";*/

/*--------- Global Variables ---------*/
var companyID = $("#TCGE-CompanyID").text(),
    EPDcheck = $("#TCGE-EPD").text(),
    fJEPer = $("#TCGE-FJEPer").text(),
    checkbookBalance = 0,
    transactionDate = null,
    systemRate,
    PTcheck = $("#CBT-PT").text();

$(document).ready(function () {

    /*-------------- For Checks ------------- */
    if (fJEPer === "NoPS") {
        Talert("This Company in Financial Module Not have Posting Setup..!");
        window.location.href = "/Home/Financial_Home";
    }

    // 1.0 Check Checkbook Security Before Get Data
    $("#TBR-checkbookID").change(function () {
        var checkbookID = $(this).val();
        if (checkbookID.length == 0) {
        } else {
            $.ajax({
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                method: "get",
                url: "/api/CheckBookAPIs/CheckbookSecurity?checkbookID=" + checkbookID + "&companyID=" + companyID,
                success: function (result) {
                    if (result.CB_Password === "Exist") {
                        $("#CBT-CBid").text(checkbookID);
                        $("#TBR-checkbookID").val("");
                        $("#TBR-checkbookName").val("");
                        $("#CBT-checkbookModal").modal("show");
                    } else if (result.CB_UserID === "UserIDAccess") {
                        GetCheckbookData(checkbookID);
                    } else if (result.CB_UserID === "NoPermit") {
                        $("#TBR-checkbookID").val("");
                        $("#TBR-checkbookName").val("");
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
                $("#TBR-checkbookID").val(checkbookID);
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


    // 2.0 Save Bank Reconcile
    $('#saveBankReconcile').click(function (e) {
        var bankReconcileNumber = $('#TBR-bankRenconcileNumber').text(),
            checkbookID = $('#TBR-checkbookID').val(),
            bankEndingBalance = $('#TBR-bankEndingBalance').val(),
            bankEndingDate = $('#TBR-bankEndingDate').val(),
            bookEndingDate = $('#TBR-bookEndingDate').val(),
            sortType = 1,
            isValid = true;

        $('#firstPart input[type="date"],#firstPart input[type="number"],#firstPart select').not('input:disabled,select:disabled').each(function () {
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

        if (isValid == false) {
            e.preventDefault();
        } else {
            $.ajax({
                type: 'POST',
                url: "/BankReconcile/SaveBankReconcile?bankReconcileNumber=" + bankReconcileNumber
                    + "&checkbookID=" + checkbookID + "&bankEndingBalance=" + bankEndingBalance
                    + "&bankEndingDate=" + bankEndingDate + "&bookEndingDate=" + bookEndingDate,
                success: function (result) {
                    $('#firstPart').collapse("hide");
                    $('#secondPart').collapse("show");

                    $('#saveBankReconcile').prop('disabled', true);
                    $('#TBR-checkbookID').prop('disabled', true);
                    $('#TBR-bookEndingDate').prop('disabled', true);
                    $('#TBR-bankEndingDate').prop('disabled', true);
                    $('#TBR-bankEndingBalance').prop('disabled', true);

                    Talert('Bank Reconcile Number = ' + bankReconcileNumber);
                    window.location.reload();
                }
            }).done(function () {
                // Show Second-Part
                GetTransactionData(sortType);
            });
        }
    });


    // 3.0 Update Reconcile In C_CheckbookTransaction_table when Checkbox Checked
    // 3.0 Calculate No. & Amount Of Deposit OR Payment On Every Reconcile-Check
    $(document).on('click', '.TBR-reconcileUpdate', function () {

        //----- change value of checkbox before Insert to database
        if ($(this).is(":checked")) {
            $(this).attr('value', true);
        }
        else {
            $(this).attr('value', false);
        }

        var rowID = $(this).closest('tr').attr('id'),
            checkbookID = $('#TBR-checkbookID').val(),
            reconcileStatus = $(this).val(),
            reconcileNumber = $('#TBR-bankRenconcileNumber').text(),
            bankStatementEndingBalance = $('#TBR-bankEndingBalance').val();
        $('#depositAmount').val(0);
        $('#paymentAmount').val(0);
        $('#bankBalanceAmount').val(0);
        $('#bookBalanceAmount').val(0);
        $('#bankReconcileDifference').val('0');

        CalculateNo();

        // Save True OR False In Database According To Reconcile-Check
        $.ajax({
            type: 'POST',
            url: "/BankReconcile/UpdateReconcile?checkbookID=" + checkbookID + "&rowID=" + rowID
                + "&reconcileStatus=" + reconcileStatus + "&reconcileNumber=" + reconcileNumber,
            success: function (result) { }
        });
    });


    // 4.0 Get Checkbook Balance From Start-Date To Book Statement Ending Date
    $('#TBR-bookEndingDate').focusout(function () {
        var checkbookID = $('#TBR-checkbookID').val(),
            bookStatementEndingDate = $(this).val();
        $.ajax({
            type: "get",
            url: "/BankReconcile/GetBookEndingDateBalance?checkbookID=" + checkbookID + "&bookStatementEndingDate=" + bookStatementEndingDate,
            success: function (result) {
                checkbookBalance = result;
            }
        });
    });


    // 5.0 Sort Table By Doc-Sort OR Date
    $('#docSort').click(function () {
        var sortType = 2;
        GetTransactionData(sortType);
    });
    $('#dateSort').click(function () {
        var sortType = 1;
        GetTransactionData(sortType);
    });


    // 6.0 Bank-Reconcile Search
    $('#TBR-reconcileSearch').change(function () {
        var bankReconcileNumber = $(this).val();
        $.ajax({
            type: 'GET',
            url: "/BankReconcile/SearchReconcile?bankReconcileNumber=" + bankReconcileNumber,
            success: function (result) {
                DisabledPartOne();
                $('#TBR-bankRenconcileNumber').text(result.BankReconcile_Number);
                $('#TBR-checkbookID').val(result.C_CBSID);
                $("#TBR-currencyID").append("<option value='" + result.CurrencyID + "'>" + result.CurrencyName + "</option>");
                $('#TBR-checkbookName').val(result.CheckbookName);
                $('#TBR-bankEndingBalance').val(result.Bank_Statment_Ending_Balance);
                $('#TBR-bankEndingDate').val(result.Bank_Statment_Ending_Date);
                $('#TBR-bookEndingDate').val(result.Book_Statment_Ending_Date);
                transactionDate = result.Date;


                $('#TBR-accountID').empty();
                $('#TBR-adjustmentCheckbookCurrency').empty();
                $("#TBR-accountID").append("<option value='" + result.C_AID + "'>" + result.Company_AccountsID + "</option>");
                $('#TBR-accountName').val(result.Company_AccountsName);
                $("#TBR-adjustmentCheckbookName").val(result.CheckbookName);
                $("#TBR-adjustmentCheckbookCurrency").append("<option value='" + result.CurrencyID + "'>" + result.CurrencyName + "</option>");
            }
        }).done(function () {
            $('#firstPart').collapse("hide");
            $('#secondPart').collapse("show");
            $('#saveBankReconcile').prop('disabled', true);
            $('#TBR-checkbookID').prop('disabled', true);
            $('#TBR-bookEndingDate').prop('disabled', true);
            $('#TBR-bankEndingDate').prop('disabled', true);
            $('#TBR-bankEndingBalance').prop('disabled', true);

            $('#dateSort').prop('disabled', true);
            $('#docSort').prop('disabled', true);
            GetReconcileSearch(bankReconcileNumber);
        });
    });


    // 7.0 Reconciled Button
    $('#finishBankReconcile').click(function () {
        var bankReconcileNumber = $('#TBR-bankRenconcileNumber').text();
        InsertAdjustReconcile();
        $.ajax({
            type: 'GET',
            url: "/BankReconcile/ReconcileChecked?bankReconcileNumber=" + bankReconcileNumber,
            success: function (result) {
                RedirectInt(window.location.href)
            }
        });
    });


    // 8.0 Button to Delete Bank-Reconcile
    $("#TBR-reconcileDelete").click(function () {
        var bankReconcileNumber = $('#TBR-bankRenconcileNumber').text();
        if ($('#TBR-checkbookID').val().length > 0) {
            $.ajax({
                type: "POST",
                url: "/BankReconcile/DeleteBankReconcile?bankReconcileNumber=" + bankReconcileNumber,
                success: function (result) {
                    if (result == "NotFound") {
                        location.reload();
                    }
                }
            });
        }
    });


    //////////////////////////////////////////////////////////////////////////////////////////////////

    $('#addMoreRow').on('click', function () {
        var data = $("#TBR-adjustmentTable tr:eq(1)").clone(true).appendTo("#TBR-adjustmentData");
        data.find("input").val('');
    });
    $(document).on('click', '.removeRow', function () {
        var Balance = GetInputBalance();
        var trIndex = $(this).closest("tr").index();
        if (trIndex > 0) {
            $(this).closest("tr").remove();
        } else {
            $("#TBR-adjustmentTable tr:eq(1)").find("input").val(0);
            $("#TBR-adjustmentTable tr:eq(1)").find("select").val("");
            // Talert("Sorry!! Can't remove first row!");
        }

        Calculate_NetAdjustment();

        if (Balance > 0) {
            var MinusPlus = GetMinusPlus();

            plus = MinusPlus[1];
            minus = MinusPlus[0];
            netAdjustment = plus - minus;
            var FullAdjust = parseFloat($('#TBR-adjustmentDifference').attr("data-orginal")) - parseFloat(netAdjustment);
            $("#bankBalanceAmount").val(RoundNumber(parseFloat($("#bankBalanceAmount").val()) + FullAdjust,2))
            $("#bankReconcileDifference").val(RoundNumber(FullAdjust,2))
            CheckReconcileState();
        }
    });

    $('input[name=adjustmentAmount]').focusout(function () {
        Calculate_NetAdjustment();
    });

    $('input[name=adjustmentDate]').focusout(function () {
        var currencyID = $('#TBR-currencyID').val(),
            postingDate = $(this).val(),
            
            Selector = $(this).closest('tr');
        CheckPostingDateInPeriods(postingDate, function (test) {
            if (test !== true) {
                $("#TCGE-SystemRate").val("");
            } else {
                GetCurrencyRates(currencyID, postingDate, Selector);
            }
        });
       
    });

    $('select[name=adjustmentType]').on('change', function () {
        Calculate_NetAdjustment();
    });

    var reconcileArr = [];
    $('#TBR-finishAdjustment').click(function (e) {

        var checkbookID = $('#TBR-checkbookID').val(),
            currencyID = $('#TBR-currencyID').val(),
            checkbookAccountID = $('#TBR-accountID').val(),
            postingKey = 'TBR',
            documentType = 'SED',
            documentNumber = $('#TBR-bankRenconcileNumber').text(),
            payment_To_received_From = $('#TBR-checkbookName').val();

        if (transactionDate == null) {
            transactionDate = ConverDate(Date.now());
        }

        var isValid = true;
        $('#TBR-adjustmentData :input,#TBR-adjustmentData select').each(function () {
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
        if (isValid == false) {
            e.preventDefault();
        } else {
            var headerObj = {},
                mainArr = [],
                reconcileArr = [];
            $('#TBR-adjustmentTable select[name=adjustmentType]').each(function () {
                headerObj = {};
                mainArr = [];
                reconcileArr = [];

                var rowType = $(this).val(),
                    row = $(this).closest('tr'),
                    rowSystemRate = row.find('td:eq(0)').find('input').val().replace(regRemoveCurrFormate, ""),
                    rowDate = row.find('td:eq(2)').find('input').val(),
                    rowAccountID = row.find('td:eq(3)').find('select').val(),
                    rowDescription = row.find('td:eq(4)').find('input').val(),
                    rowAmount = row.find('td:eq(5)').find('input').val(),
                    originalAmount = parseFloat(rowAmount) * parseFloat(rowSystemRate);

                // Fill The Header Object
                headerObj = {
                    C_CBID: '',
                    C_PostingDate: rowDate,
                    C_TransactionDate: rowDate,
                    C_Refrence: rowDescription,
                    CurrencyID: currencyID,
                    C_SystemRate: rowSystemRate,
                    C_TransactionRate: rowSystemRate,
                    C_PostingKey: 'TBR',
                    C_TransactionType: 'Bank Reconcile'
                };
                if (rowType == 'OI' || rowType == 'II') {
                    mainArr.push({
                        C_Describtion: rowDescription,
                        C_Document: documentType,
                        C_AID: rowAccountID,
                        C_OriginalAmount: originalAmount,
                        C_Debit: 0,
                        C_Credit: originalAmount
                    });
                    mainArr.push({
                        C_Describtion: rowDescription,
                        C_Document: documentType,
                        C_AID: checkbookAccountID,
                        C_OriginalAmount: originalAmount,
                        C_Debit: originalAmount,
                        C_Credit: 0
                    });
                    var journalEntryNumber = 0;// //InsertJv PureTransactionSave(companyID, headerObj, mainArr);
                    reconcileArr.push({
                        //C_PostingNumber: journalEntryNumber,
                        C_DocumentNumber: documentNumber,
                        C_TransactionDate: transactionDate,
                        C_PostingDate: rowDate,
                        C_CBSID: checkbookID,
                        CurrencyID: currencyID,
                        C_SystemRate: rowSystemRate,
                        C_TransactionRate: rowSystemRate,
                        C_Difference: 0,
                        C_Reference: rowDescription,
                        C_Payment: 0,
                        C_Reciept: rowAmount,
                        C_Balance: rowAmount,
                        C_CheckNumber: '',
                        C_PostingKey: postingKey,
                        C_DocumentType: documentType,
                        C_Reconcile: true,
                        C_ReconcileNumber: documentNumber,
                        C_Payment_To_Recieved_From: payment_To_received_From
                    });
                }
                if (rowType == 'OE' || rowType == 'IE') {
                    mainArr.push({
                        C_Describtion: rowDescription,
                        C_Document: documentType,
                        C_AID: rowAccountID,
                        C_OriginalAmount: originalAmount,
                        C_Debit: originalAmount,
                        C_Credit: 0
                    });
                    mainArr.push({
                        C_Describtion: rowDescription,
                        C_Document: documentType,
                        C_AID: checkbookAccountID,
                        C_OriginalAmount: originalAmount,
                        C_Debit: 0,
                        C_Credit: originalAmount
                    });
                    var journalEntryNumber = 0; //InsertJv PureTransactionSave(companyID, headerObj, mainArr);
                    reconcileArr.push({
                        //C_PostingNumber: journalEntryNumber,
                        C_DocumentNumber: documentNumber,
                        C_TransactionDate: transactionDate,
                        C_PostingDate: rowDate,
                        C_CBSID: checkbookID,
                        CurrencyID: currencyID,
                        C_SystemRate: rowSystemRate,
                        C_TransactionRate: rowSystemRate,
                        C_Difference: 0,
                        C_Reference: rowDescription,
                        C_Payment: rowAmount,
                        C_Reciept: 0,
                        C_Balance: 0 - rowAmount,
                        C_CheckNumber: '',
                        C_PostingKey: postingKey,
                        C_DocumentType: documentType,
                        C_Reconcile: true,
                        C_ReconcileNumber: documentNumber,
                        C_Payment_To_Recieved_From: payment_To_received_From
                    });
                }
                //K Update

                var balance = 0;
                $.each(reconcileArr, function (k, i) {
                    balance += i.C_Balance;
                })
                if (balance < 0) {
                    balance = -balance;
                }
                //K Update

                reconcileArr = JSON.stringify({ 'reconcileArr': reconcileArr });
                //K Update
                $('#adjustmentModal').modal('hide');
                bankBalanceAmount = parseFloat($("#bankBalanceAmount").val());
                if (!$("#bankBalanceAmount").attr("OrginalBal")) {
                    $("#bankBalanceAmount").attr("OrginalBal", bankBalanceAmount);
                }
                var BankOrginal = parseFloat($("#bankBalanceAmount").attr("OrginalBal"));
                $("#bankBalanceAmount").val(parseFloat(BankOrginal) + parseFloat(balance));

                var ThisbankFinalResult = parseFloat($("#bankBalanceAmount").val());
                var ThisbookFinalResult = parseFloat($("#bookBalanceAmount").val());
                $('#bankReconcileDifference').val(RoundNumber(parseFloat(ThisbankFinalResult) - parseFloat(ThisbookFinalResult), 2));
                $('#TBR-adjustmentDifference').val(RoundNumber(parseFloat(ThisbankFinalResult) - parseFloat(ThisbookFinalResult), 2));
                if ($('#bankReconcileDifference').val() === '0') {
                    $('#finishBankReconcile').prop('disabled', false);
                    $('#adjustmentModal').prop('disabled', true);
                } else {
                    $('#finishBankReconcile').prop('disabled', true);
                    $('#adjustmentModal').prop('disabled', false);
                }


                var MinusPlus = GetMinusPlus();
                plus = MinusPlus[1];
                minus = MinusPlus[0];
                netAdjustment = plus - minus;
                netAdjustment = parseFloat(netAdjustment);
                $('#TBR-netAdjustment').val(netAdjustment);
                var FullAdjust = parseFloat($('#TBR-adjustmentDifference').attr("data-orginal")) - parseFloat(netAdjustment);
                //$('#TBR-adjustmentReconcileDifference').val(RoundNumber(parseFloat(reconcileDifference) - netAdjustment, 2));
                //$('#TBR-adjustmentDifference').val(parseFloat($('#TBR-adjustmentDifference').attr("data-orginal")) - netAdjustment)
                $("#bankReconcileDifference").val(RoundNumber(FullAdjust,2))

                CheckReconcileState();

                //$.ajax({
                //    contentType: 'application/json; charset=utf-8',
                //    dataType: 'json',
                //    type: "POST",
                //    url: "/BankReconcile/CheckbookAdjustment",
                //    data: reconcileArr,
                //    success: function (result) {
                //        if (result === "True") {
                //            $('#TBR-reconcileSearch').append("<option value='" + documentNumber + "'>" + documentNumber + "</option>")
                //            $('#TBR-reconcileSearch').val(documentNumber);
                //            $('#TBR-reconcileSearch').change();
                //        }
                //    }
                //}).done(function () {
                //    $('#adjustmentModal').modal('hide');
                //});
                //K Update

            });
        }
    });

    $('#CBT-checkbookModal').on('hidden.bs.modal', function (e) {
        $(this)
            .find("input")
            .val('')
            .end()
            .find("label").text('').end();
    });
});

function InsertAdjustReconcile() {
    var checkbookID = $('#TBR-checkbookID').val(),
        currencyID = $('#TBR-currencyID').val(),
        checkbookAccountID = $('#TBR-accountID').val(),
        postingKey = 'TBR',
        documentType = 'SED',
        documentNumber = $('#TBR-bankRenconcileNumber').text(),
        payment_To_received_From = $('#TBR-checkbookName').val();

    $('#TBR-adjustmentTable select[name=adjustmentType]').each(function () {
        headerObj = {};
        mainArr = [];
        reconcileArr = [];

        var rowType = $(this).val(),
            row = $(this).closest('tr'),
            rowSystemRate = row.find('td:eq(0)').find('input').val().replace(regRemoveCurrFormate, ""),
            rowDate = row.find('td:eq(2)').find('input').val(),
            rowAccountID = row.find('td:eq(3)').find('select').val(),
            rowDescription = row.find('td:eq(4)').find('input').val(),
            rowAmount = row.find('td:eq(5)').find('input').val(),
            originalAmount = parseFloat(rowAmount) * parseFloat(rowSystemRate);

        // Fill The Header Object
        headerObj = {
            C_CBID: '',
            C_PostingDate: rowDate,
            C_TransactionDate: rowDate,
            C_Refrence: rowDescription,
            CurrencyID: currencyID,
            C_SystemRate: rowSystemRate,
            C_TransactionRate: rowSystemRate,
            C_PostingKey: 'TBR',
            C_TransactionType: 'Bank Reconcile'
        };
        if (rowType == 'OI' || rowType == 'II') {
            mainArr.push({
                C_Describtion: rowDescription,
                C_Document: documentType,
                C_AID: rowAccountID,
                C_OriginalAmount: originalAmount,
                C_Debit: 0,
                C_Credit: originalAmount
            });
            mainArr.push({
                C_Describtion: rowDescription,
                C_Document: documentType,
                C_AID: checkbookAccountID,
                C_OriginalAmount: originalAmount,
                C_Debit: originalAmount,
                C_Credit: 0
            });
            var journalEntryNumber = PureTransactionSave(companyID, headerObj, mainArr);
            reconcileArr.push({
                C_PostingNumber: journalEntryNumber,
                C_DocumentNumber: documentNumber,
                C_TransactionDate: transactionDate,
                C_PostingDate: rowDate,
                C_CBSID: checkbookID,
                CurrencyID: currencyID,
                C_SystemRate: rowSystemRate,
                C_TransactionRate: rowSystemRate,
                C_Difference: 0,
                C_Reference: rowDescription,
                C_Payment: 0,
                C_Reciept: rowAmount,
                C_Balance: rowAmount,
                C_CheckNumber: '',
                C_PostingKey: postingKey,
                C_DocumentType: documentType,
                C_Reconcile: true,
                C_ReconcileNumber: documentNumber,
                C_Payment_To_Recieved_From: payment_To_received_From
            });
        }
        if (rowType == 'OE' || rowType == 'IE') {
            mainArr.push({
                C_Describtion: rowDescription,
                C_Document: documentType,
                C_AID: rowAccountID,
                C_OriginalAmount: originalAmount,
                C_Debit: originalAmount,
                C_Credit: 0
            });
            mainArr.push({
                C_Describtion: rowDescription,
                C_Document: documentType,
                C_AID: checkbookAccountID,
                C_OriginalAmount: originalAmount,
                C_Debit: 0,
                C_Credit: originalAmount
            });
            var journalEntryNumber = PureTransactionSave(companyID, headerObj, mainArr);
            reconcileArr.push({
                C_PostingNumber: journalEntryNumber,
                C_DocumentNumber: documentNumber,
                C_TransactionDate: transactionDate,
                C_PostingDate: rowDate,
                C_CBSID: checkbookID,
                CurrencyID: currencyID,
                C_SystemRate: rowSystemRate,
                C_TransactionRate: rowSystemRate,
                C_Difference: 0,
                C_Reference: rowDescription,
                C_Payment: rowAmount,
                C_Reciept: 0,
                C_Balance: 0 - rowAmount,
                C_CheckNumber: '',
                C_PostingKey: postingKey,
                C_DocumentType: documentType,
                C_Reconcile: true,
                C_ReconcileNumber: documentNumber,
                C_Payment_To_Recieved_From: payment_To_received_From
            });
        }
        //K Update

        var balance = 0;
        $.each(reconcileArr, function (k, i) {
            balance += i.C_Balance;
        })
        if (balance < 0) {
            balance = -balance;
        }
        //K Update
        reconcileArr = JSON.stringify({ 'reconcileArr': reconcileArr });
        //K Update
        $('#adjustmentModal').modal('hide');
        bankBalanceAmount = parseFloat($("#bankBalanceAmount").val());
        if (!$("#bankBalanceAmount").attr("OrginalBal")) {
            $("#bankBalanceAmount").attr("OrginalBal", RoundNumber(bankBalanceAmount,2));
        }
        var BankOrginal = parseFloat($("#bankBalanceAmount").attr("OrginalBal"));
        $("#bankBalanceAmount").val(RoundNumber(BankOrginal + parseFloat(balance),2));

        var ThisbankFinalResult = parseFloat($("#bankBalanceAmount").val());
        var ThisbookFinalResult = parseFloat($("#bookBalanceAmount").val());

        $('#bankReconcileDifference').val(RoundNumber(parseFloat(ThisbankFinalResult) - parseFloat(ThisbookFinalResult), 2));
        $('#TBR-adjustmentDifference').val(RoundNumber(parseFloat(ThisbankFinalResult) - parseFloat(ThisbookFinalResult), 2));
        if ($('#bankReconcileDifference').val() === '0') {
            $('#finishBankReconcile').prop('disabled', false);
            $('#adjustmentModal').prop('disabled', true);
        } else {
            $('#finishBankReconcile').prop('disabled', true);
            $('#adjustmentModal').prop('disabled', false);
        }
        var MinusPlus = GetMinusPlus();
        plus = MinusPlus[1];
        minus = MinusPlus[0];
        netAdjustment = plus - minus;
        netAdjustment = parseFloat(netAdjustment);
        $('#TBR-netAdjustment').val(netAdjustment);
        var FullAdjust = parseFloat($('#TBR-adjustmentDifference').attr("data-orginal")) - parseFloat(netAdjustment);
        //$('#TBR-adjustmentReconcileDifference').val(RoundNumber(parseFloat(reconcileDifference) - netAdjustment, 2));
        //$('#TBR-adjustmentDifference').val(parseFloat($('#TBR-adjustmentDifference').attr("data-orginal")) - netAdjustment)
        $("#bankReconcileDifference").val(RoundNumber(FullAdjust,2))


        //$.ajax({
        //    contentType: 'application/json; charset=utf-8',
        //    dataType: 'json',
        //    type: "POST",
        //    url: "/BankReconcile/CheckbookAdjustment",
        //    data: reconcileArr,
        //    success: function (result) {
        //        if (result === "True") {
        //            $('#TBR-reconcileSearch').append("<option value='" + documentNumber + "'>" + documentNumber + "</option>")
        //            $('#TBR-reconcileSearch').val(documentNumber);
        //            $('#TBR-reconcileSearch').change();
        //        }
        //    }
        //}).done(function () {
        //    $('#adjustmentModal').modal('hide');
        //});
        //K Update
    });
    $.ajax({
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        type: "POST",
        url: "/BankReconcile/CheckbookAdjustment",
        data: reconcileArr,
        success: function (result) {
            //if (result === "True") {
            //    $('#TBR-reconcileSearch').append("<option value='" + documentNumber + "'>" + documentNumber + "</option>")
            //    $('#TBR-reconcileSearch').val(documentNumber);
            //    $('#TBR-reconcileSearch').change();
            //}
        }
    })

    var CBData = [];
    $("#TBR-appendData").find("tr").each(function () {
        var This = $(this);
        if (!$(This).find(".TBR-reconcileUpdate").is(":checked")) {
            CBData.push({
                Deposite: $(This).find("td").eq(6).text(),
                Payment: $(This).find("td").eq(5).text(),
                Type: "row",
                Cheque_num: $(This).find("td").eq(2).text()
            })
        }
    });
    $("#TBR-adjustmentData").find("tr").each(function () {
        var This = $(this);

        var Payment = 0;
        var Depo = 0;
        var rowType = $(This).find('.RowType').val();
        if (rowType == 'OI' || rowType == 'II') {
             Depo= $(This).find(".adjustmentAmount").val()
        }
        if (rowType == 'OE' || rowType == 'IE') {
            Payment = $(This).find(".adjustmentAmount").val()
        }
        CBData.push({
            Deposite: Depo,
            Payment: Payment,
            Type: "Rec",
            Cheque_num: $(This).find('.RowType option:selected').text()
        })
    });

    var QStr = "";
    $.each(CBData, function (k, i) {
        QStr += "<input name='CBData[" + k + "][Deposite]' value='" + i.Deposite + "' />"
        QStr += "<input name='CBData[" + k + "][Payment]' value='" + i.Payment + "' />"
        QStr += "<input name='CBData[" + k + "][Type]' value = '" + i.Type + "' /> "
        QStr += "<input name='CBData[" + k + "][Cheque_num]' value = '" + i.Cheque_num + "' />"
    })
    openWindowWithPostArray("/SysReports/Reconcile?BankRecnocileNumber=" + $("#TBR-reconcileSearch").val(),
        QStr)

}
$(".Print").click(function () {
    var CBData = [];
    $("#TBR-appendData").find("tr").each(function () {
        var This = $(this);
        if (!$(This).find(".TBR-reconcileUpdate").is(":checked")) {
            CBData.push({
                Deposite: $(This).find("td").eq(6).text(),
                Payment: $(This).find("td").eq(5).text(),
                Type: "row",
                Cheque_num: $(This).find("td").eq(2).text()
            })
        }
    });
    $("#TBR-adjustmentData").find("tr").each(function () {
        var This = $(this);

        var Payment = 0;
        var Depo = 0;
        var rowType = $(This).find('.RowType').val();
        if (rowType == 'OI' || rowType == 'II') {
             Depo = $(This).find(".adjustmentAmount").val()
        }
        if (rowType == 'OE' || rowType == 'IE') {
            Payment  = $(This).find(".adjustmentAmount").val()
        }
        CBData.push({
            Deposite: Depo,
            Payment: Payment,
            Type: "Rec",
            Cheque_num: $(This).find('.RowType option:selected').text()
        })
    });
    var QStr = "";
    $.each(CBData, function (k, i) {
        QStr += "<input name='CBData[" + k + "][Deposite]' value='"+i.Deposite+"' />"
        QStr += "<input name='CBData[" + k + "][Payment]' value='" + i.Payment +"' />"
        QStr += "<input name='CBData[" + k + "][Type]' value = '" + i.Type +"' /> "
        QStr += "<input name='CBData[" + k + "][Cheque_num]' value = '" + i.Cheque_num + "' />"


    })
    openWindowWithPostArray("/SysReports/Reconcile?BankRecnocileNumber=" + $("#TBR-reconcileSearch").val(),
        QStr)
})
function GetCheckbookData(checkbookID) {
    $.ajax({
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        method: "get",
        url: "/api/CheckBookAPIs/GetCheckbookData?checkbookID=" + checkbookID + "&companyID=" + companyID,
        success: function (result) {
            $('#TBR-currencyID').empty();
            $("#TBR-currencyID").append("<option value='" + result.CurrencyID + "'>" + result.CurrencyName + "</option>");
            $("#TBR-checkbookName").val(result.CheckbookName);

            $('#TBR-accountID').empty();
            $("#TBR-accountID").append("<option value='" + result.C_AID + "'>" + result.Company_AccountsID + "</option>");
            $("#TBR-accountName").val(result.Company_AccountsName);
            $("#TBR-adjustmentCheckbookName").val(result.CheckbookName);
            $("#TBR-adjustmentCheckbookCurrency").append("<option value='" + result.CurrencyID + "'>" + result.CurrencyName + "</option>");
        }
    });
}
function GetTransactionData(sortType) {
    var checkbookID = $('#TBR-checkbookID').val(),
        bookEndingDate = $('#TBR-bookEndingDate').val(),
        tableBody = $('#TBR-appendData'),
        html = '',
        type = null;
    $.ajax({
        type: 'POST',
        url: "/BankReconcile/GetTransactionData?checkbookID=" + checkbookID
            + "&bookEndingDate=" + bookEndingDate + "&sortType=" + sortType,
        success: function (result) {
            tableBody.empty();
            ClearCalculatedInputs();
            if (result.length > 0) {
                $.each(result, function (key, item) {
                    if (item.CheckNumber == null || item.CheckNumber == "") {
                        var checkNumber = "-"
                    } else {
                        checkNumber = item.CheckNumber;
                    }
                    if (item.DocumentType == 'TCCR') {
                        type = 'Cash Reciept';
                    } else if (item.DocumentType == 'TCCW') {
                        type = 'Cash Withdraw';
                    } else if (item.DocumentType == 'TCBR') {
                        type = 'Bank Check-Recieved'
                    } else if (item.DocumentType == 'TCBC') {
                        type = 'Bank Check-out';
                    } else if (item.DocumentType == 'TCBT') {
                        type = 'Checkbook Transfer';
                    } else if (item.DocumentType == 'TBR') {
                        type = 'Bank Reconcile';
                    }
                    html += '<tr id="' + item.C_CBT + '">';
                    html += '<td>' + type + '</td>';
                    html += '<td>' + item.DocumentNumber + '</td>';
                    html += '<td>' + checkNumber + '</td>';
                    html += '<td>' + item.Date + '</td>';
                    html += '<td>' + "<input type='checkbox' class='TBR-reconcileUpdate' />" + '</td>';
                    html += '<td>' + item.Payment + '</td>';
                    html += '<td>' + item.Deposit + '</td>';
                    html += '</tr>';
                });
                tableBody.html(html);
            } else {
                tableBody.append("<tr><td colspan='7'>" + "There's No Any Transactions In This Checkbook" + "</td></tr>");
            }
            CalculateNo();
        }
    });
    return true;
}
function GetReconcileSearch(bankReconcileNumber) {
    var bookEndingDate = $('#TBR-bookEndingDate').val(),
        checkbookID = $('#TBR-checkbookID').val(),
        tableBody = $('#TBR-appendData'),
        html = '',
        type = null;
    // Get book-Statement-Date Balance
    $.ajax({
        type: "get",
        async: false,
        url: "/BankReconcile/GetBookEndingDateBalance?checkbookID=" + checkbookID + "&bookStatementEndingDate=" + bookEndingDate,
        success: function (result) {
            checkbookBalance = result;
        }
    }).done(function () {
        $.ajax({
            type: 'POST',
            url: "/BankReconcile/GetReconcileTable?checkbookID=" + checkbookID
                + "&bookEndingDate=" + bookEndingDate + "&bankReconcileNumber=" + bankReconcileNumber,
            success: function (result) {
                tableBody.empty();
                ClearCalculatedInputs();
                if (result.length > 0) {
                    $.each(result, function (key, item) {
                        if (item.CheckNumber == null || item.CheckNumber == "") {
                            var checkNumber = "-"
                        } else {
                            checkNumber = item.CheckNumber;
                        }
                        var status = 'checked';
                        if (item.ReconcileStatus === false) {
                            status = 'notChecked';
                        }
                        if (item.DocumentType == 'TCCR') {
                            type = 'Cash Reciept';
                        } else if (item.DocumentType == 'TCCW') {
                            type = 'Cash Withdraw';
                        } else if (item.DocumentType == 'TCBR') {
                            type = 'Bank Check-Recieved'
                        } else if (item.DocumentType == 'TCBC') {
                            type = 'Bank Check-out';
                        } else if (item.DocumentType == 'TCBT') {
                            type = 'Checkbook Transfer';
                        } else if (item.DocumentType == 'TBR') {
                            type = 'Bank Reconcile';
                        }
                        html += '<tr id="' + item.C_CBT + '">';
                        html += '<td>' + type + '</td>';
                        html += '<td>' + item.DocumentNumber + '</td>';
                        html += '<td>' + checkNumber + '</td>';
                        html += '<td>' + item.Date + '</td>';
                        html += '<td>' + "<input type='checkbox' class='TBR-reconcileUpdate' " + status + " />" + '</td>';
                        html += '<td>' + item.Payment + '</td>';
                        html += '<td>' + item.Deposit + '</td>';
                        html += '</tr>';
                        tableBody.html(html);
                    });
                } else {
                    tableBody.append("<tr><td colspan='7'>" + "There's No Any Transactions In This Checkbook" + "</td></tr>");
                }
                CalculateNo();
            }
        });

    });
}
function DisabledPartOne() {
    $('#TBR-currencyID').empty();
    $('#saveBankReconcile').prop('disabled', true);
    $('#TBR-checkbookID').prop('disabled', true);
    $('#TBR-bankEndingBalance').prop('disabled', true);
    $('#TBR-bankEndingDate').prop('disabled', true);
    $('#TBR-bookEndingDate').prop('disabled', true);

    $('#TBR-adjustmentDifference').val(0);
}
function ClearCalculatedInputs() {
    $('#depositAmount').val(0);
    $('#paymentAmount').val(0);
    $('#depositCounter').val(0);
    $('#paymentCounter').val(0);
    $('#bankBalanceAmount').val(0);
    $('#bookBalanceAmount').val(0);
    $('#bankReconcileDifference').val(0);
}
function CalculateNo() {
    var depositCalculate = 0,
        bankDepositCalculate = 0,
        bankPaymentCalculate = 0,
        bookPaymentCalculate = 0,
        bookDepositCalculate = 0,
        paymentCalculate = 0,
        totalDeposit = 0,
        totalPayment = 0,
        bankFinalResult = 0,
        bookFinalResult = 0,
        bankStatementEndingBalance = $('#TBR-bankEndingBalance').val(),
        bankFinalResult = bankStatementEndingBalance,
        bookFinalResult = checkbookBalance;

    // Calculate Amount Of Deposit and payment IF Checked
    // Calculate No. Of Deposit and payment If Checked
    $('.TBR-reconcileUpdate:checked').each(function () {
        var payment = $(this).closest('tr').find('td').eq(5).text(),
            deposit = $(this).closest('tr').find('td').eq(6).text();

        depositCalculate += parseFloat($(this).closest('tr').find('td').eq(6).text());
        paymentCalculate += parseFloat($(this).closest('tr').find('td').eq(5).text());
        $('#depositAmount').val(RoundNumber(depositCalculate,2));
        $('#paymentAmount').val(RoundNumber(paymentCalculate,2));

        if (parseFloat(deposit) > 0) {
            totalDeposit++;
        } else if (parseFloat(payment) > 0) {
            totalPayment++;
        }
        $('#depositCounter').val(totalDeposit);
        $('#paymentCounter').val(totalPayment);
    });


    // Calculate Adjustment Bank Balance & Adjustment Book Balance & Difference Between Them
    if ($('#TBR-appendData').children().length == 1) {
        bankFinalResult = parseFloat(bankStatementEndingBalance) + bookDepositCalculate - bookPaymentCalculate;
        bookFinalResult = parseFloat(checkbookBalance) - bankDepositCalculate + bankPaymentCalculate;
    }

    $('.TBR-reconcileUpdate:not(:checked)').each(function () {
        bankPaymentCalculate += parseFloat($(this).closest('tr').find('td').eq(5).text());
        bankDepositCalculate += parseFloat($(this).closest('tr').find('td').eq(6).text());

        bookPaymentCalculate += parseFloat($(this).closest('tr').find('td').eq(5).text());
        bookDepositCalculate += parseFloat($(this).closest('tr').find('td').eq(6).text());
        bookFinalResult = parseFloat(checkbookBalance) - bankDepositCalculate + bankPaymentCalculate;
        bankFinalResult = parseFloat(bankStatementEndingBalance) + bookDepositCalculate - bookPaymentCalculate;

    });
    if ($('.TBR-reconcileUpdate:not(:checked)').length == 0) {
        bankFinalResult = parseFloat(bankStatementEndingBalance) + bookDepositCalculate - bookPaymentCalculate;
        bookFinalResult = parseFloat(checkbookBalance) - bankDepositCalculate + bankPaymentCalculate;
    }
    $('#bankBalanceAmount').val(RoundNumber(parseFloat(bankFinalResult),2));
    $('#bookBalanceAmount').val(RoundNumber(parseFloat(bookFinalResult),2));
    $('#bankReconcileDifference').val(RoundNumber(parseFloat(bankFinalResult) - parseFloat(bookFinalResult), 2));
    $('#bankReconcileDifference').attr("data-orginal", RoundNumber(parseFloat(bankFinalResult) - parseFloat(bookFinalResult), 2));
    $('#TBR-adjustmentDifference').val(RoundNumber(parseFloat(bankFinalResult) - parseFloat(bookFinalResult), 2));
    $('#TBR-adjustmentDifference').attr("data-orginal", RoundNumber(parseFloat(bankFinalResult) - parseFloat(bookFinalResult), 2));
    if ($('#bankReconcileDifference').val() === '0') {
        $('#finishBankReconcile').prop('disabled', false);
        $('#adjustmentModal').prop('disabled', true);
    } else {
        $('#finishBankReconcile').prop('disabled', true);
        $('#adjustmentModal').prop('disabled', false);
    }
}

function Calculate_NetAdjustment() {
    var netAdjustment = 0,
        plus = 0,
        minus = 0,
        reconcileDifference = parseFloat($('#TBR-adjustmentDifference').attr("data-orginal"));
    $('#TBR-adjustmentDifference').val(RoundNumber($('#TBR-adjustmentDifference').attr("data-orginal"),2))
    netAdjustment = 0;
    var MinusPlus = GetMinusPlus();
    plus = MinusPlus[1];
    minus = MinusPlus[0];
    netAdjustment = plus - minus;
    netAdjustment = parseFloat(netAdjustment);
    $('#TBR-netAdjustment').val(netAdjustment);
    //var FullAdjust = parseFloat($('#TBR-adjustmentDifference').attr("data-orginal")) - parseFloat(netAdjustment);
    $('#TBR-adjustmentReconcileDifference').val(RoundNumber(parseFloat(reconcileDifference) - netAdjustment, 2));
    $('#TBR-adjustmentDifference').val(RoundNumber(parseFloat($('#TBR-adjustmentDifference').attr("data-orginal")) - netAdjustment,2))
    //$("#bankReconcileDifference").val(FullAdjust)

    if (parseFloat($('#TBR-adjustmentReconcileDifference').val()) == 0) {
        $('#TBR-finishAdjustment').prop('disabled', false);
    } else {
        $('#TBR-finishAdjustment').prop('disabled', true);
    }
    CheckReconcileState();

}
function GetMinusPlus() {
    var plus = 0,
        minus = 0;
    $('#TBR-adjustmentTable input[name=adjustmentAmount]').each(function () {
        var amount = $(this).val(),
            row = $(this).closest('tr'),
            rowType = row.find('td:eq(1)').find('select').val();
        if (rowType == 'OI' || rowType == 'II') {
            var tempPlus = Number(amount);
            plus = plus + tempPlus;
        }
        if (rowType == 'OE' || rowType == 'IE') {
            var tempMinus = Number(amount);
            minus = minus + tempMinus;
        }
    });
    return [minus, plus]
}
function GetInputBalance() {
    var Balance = 0;
    $('#TBR-adjustmentTable input[name=adjustmentAmount]').each(function () {
        Balance += parseFloat($(this).val());
    });
    return parseFloat(Balance);
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
$('#adjustmentModal').on('shown.bs.modal', function () {
    $('#adjustmentModal').find('select[name="adjustmentType"]').trigger('change')
})
function CheckReconcileState() {
    if ($('#bankReconcileDifference').val() === '0') {
        $('#finishBankReconcile').prop('disabled', false);
        $('#adjustmentModal').prop('disabled', true);
    } else {
        $('#finishBankReconcile').prop('disabled', true);
        $('#adjustmentModal').prop('disabled', false);
    }
}