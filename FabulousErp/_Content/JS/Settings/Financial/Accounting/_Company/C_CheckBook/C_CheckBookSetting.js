$(document).ready(function () {
    //---------------- Get Company-Login Accounts-ID
    $.ajax({
        type: "GET",
        url: "/C_CheckBookSetting/GetCompanyAccountsID",
        success: function (result) {
            if (result.length > 0) {
                $("#SCCB-accountID").empty();
                $("#SCCB-accountID").append($('<option/>', {
                    value: "",
                    text: ChooseTxt
                })
                );
                $.each(result, function (index, row) {
                    $("#SCCB-accountID").append("<option value='" + row.AccountID + "'>" + row.Company_AccountsID + "(" + row.AccountName + ")" + "</option>");
                });
            }
            else {
                $("#SCCB-accountID").empty();
            }
        }
    });

    //---------------- Currency Change
    $("#SCCB-currency").change(function () {
        $("#SCCB-accountID").val("");
        $("#SCCB-accountName").val("");
        $("#SCCB-GlobalErrors").text("");
    });

    // ajax to get Account-Name Related to Account-ID
    // Check The Currency Matching or Not
    $("#SCCB-accountID").change(function () {
        var AccountID = $(this).val();
        var currencyID = $('#SCCB-currency').val();
        if (AccountID.length === 0) {
            $("#SCCB-accountName").val("");
        } else {
            if ($("#SCCB-currency").val().length === 0) {
                $("#SCCB-currency").focus();
                $("#SCCB-accountID").val("");
            } else {
                $.ajax({
                    type: "GET",
                    url: "/C_CheckBookSetting/GetAccountName?AccountID=" + AccountID,
                    success: function (result) {
                        for (var i = 0; i < result.length; i++) {
                            $("#SCCB-accountName").val(result[i].AccountName);
                        }
                    }
                });
                // Check The Currency Matching or Not
                $.ajax({
                    type: "GET",
                    url: "/C_CheckBookSetting/CheckAccountCurrency?AccountID=" + AccountID + "&currencyID=" + currencyID,
                    success: function (result) {
                        if (result === "False") {
                            $("#SCCB-GlobalErrors").text("This Account ID Currency Not Matching With The Chosen Currency!! ");
                            $("#SCCB-accountID").val("");
                            $("#SCCB-accountName").val("");
                        } else {
                            $("#SCCB-GlobalErrors").text("");
                        }
                    }
                });
            }
        }
    });

    //---------------- change value of checkbox before Insert to database
    $("#SCCB-checkbookStatus").change(function () {
        if ($('#SCCB-checkbookStatus').is(":checked")) {
            $(this).attr('value', true);
        }
        else {
            $(this).attr('value', false);
        }
    });

    // ---------------- Change Between Cash and Bank
    $("#SCCB-checkbookType").change(function () {
        var Type = $(this).val();
        if (Type === "Cash" || Type === "Check") {
            $("#SCCB-bankName").prop("disabled", true);
            $("#SCCB-bankAccountNumber").prop("disabled", true);
            $("#SCCB-bankAccountName").prop("disabled", true);
            $("#SCCB-branchName").prop("disabled", true);
            $("#SCCB-swiftCode").prop("disabled", true);
            $("#SCCB-IBAN").prop("disabled", true);
            $("#SCCB-bankName").val("");
            $("#SCCB-bankAccountNumber").val("");
            $("#SCCB-bankAccountName").val("");
            $("#SCCB-branchName").val("");
            $("#SCCB-swiftCode").val("");
            $("#SCCB-IBAN").val("");
        }
        else if (Type === "Bank") {
            $("#SCCB-bankName").prop("disabled", false);
            $("#SCCB-bankAccountNumber").prop("disabled", false);
            $("#SCCB-bankAccountName").prop("disabled", false);
            $("#SCCB-branchName").prop("disabled", false);
            $("#SCCB-swiftCode").prop("disabled", false);
            $("#SCCB-IBAN").prop("disabled", false);
        }
    });

    // --------------- Change Between User-ID and Password of Checkbook-Security
    $('#SCCB-CBuserID').change(function () {
        $('#SCCB-securityUserID').prop("disabled", false);
        $("button[data-id='SCCB-securityUserID']").removeAttr("disabled")
        $("button[data-id='SCCB-securityUserID']").removeClass("disabled")
        $('#SCCB-securityPassword').prop("disabled", true);
        $('#SCCB-securityPassword').val("");
    });
    $('#SCCB-CBpassword').change(function () {
        $('#SCCB-securityUserID').prop("disabled", true);
        $("button[data-id='SCCB-securityUserID']").attr("disabled", "disabled")
        $("button[data-id='SCCB-securityUserID']").addClass("disabled")
        $('#SCCB-securityUserID').val("");
        $('#SCCB-securityPassword').prop("disabled", false);
    });

    // --------------- Button to Save Company-Checkbook-Setting
    $("#SCCB-CheckbookSave").click(function () {
        var CheckbookID = $("#SCCB-checkbookID").val();
        var CheckbookName = $("#SCCB-checkbookName").val();
        var Status = $("#SCCB-checkbookStatus").val();
        var CheckbookType = $("#SCCB-checkbookType").val();
        var Currency = $('#SCCB-currency').val();
        var AccountID = $('#SCCB-accountID').val();
        var MinAmount = $("#SCCB-maxAmount").val();
        var MaxAmount = $("#SCCB-minAmount").val();
        var NextWithdraw = $("#SCCB-Nextwithdraw").val();
        var NextDeposit = $("#SCCB-Nextdeposit").val();
        var CurrentBalance = $("#SCCB-currentBalance").val();
        var CurrentCash = $("#SCCB-currentCash").val();
        var LastReconsileBalance = $("#SCCB-lastReconcileBalance").val();
        var LastReconsileDate = $("#SCCB-lastReconcileDate").val();
        var BankName = $("#SCCB-bankName").val();
        var BankAccountNumber = $("#SCCB-bankAccountNumber").val();
        var BankAccountName = $("#SCCB-bankAccountName").val();
        var BranchName = $("#SCCB-branchName").val();
        var SwiftCode = $("#SCCB-swiftCode").val();
        var IBAN = $("#SCCB-IBAN").val();
        var UserID = $("#SCCB-securityUserID").val();
        var Password = $("#SCCB-securityPassword").val();
        var check = true;

        //------------Check if there's Required empty field-------------
        if (CheckbookID.length === 0) {
            $("#SCCB-checkbookID").css("border", "1px solid red");
            check = false;
        }
        else {
            $("#SCCB-checkbookID").css("border", "");
        }
        if (CheckbookName.length === 0) {
            $("#SCCB-checkbookName").css("border", "1px solid red");
            check = false;
        }
        else {
            $("#SCCB-checkbookName").css("border", "");
        }
        if (CheckbookType.length === 0) {
            $("#SCCB-checkbookType").css("border", "1px solid red");
            check = false;
        }
        else {
            $("#SCCB-checkbookType").css("border", "");
        }
        if (Currency.length === 0) {
            $("#SCCB-currency").css("border", "1px solid red");
            check = false;
        }
        else {
            $("#SCCB-currency").css("border", "");
        }
        if (AccountID.length === 0) {
            $("#SCCB-accountID").css("border", "1px solid red");
            check = false;
        }
        else {
            $("#SCCB-accountID").css("border", "");
        }
        if (NextWithdraw.length === 0) {
            $("#SCCB-Nextwithdraw").css("border", "1px solid red");
            check = false;
        }
        else {
            $("#SCCB-Nextwithdraw").css("border", "");
        }
        if (NextDeposit.length === 0) {
            $("#SCCB-NextDeposit").css("border", "1px solid red");
            check = false;
        }
        else {
            $("#SCCB-NextDeposit").css("border", "");
        }
        //-------------------------------------------------------------------------
        if (check === true) {
            $.ajax({
                type: "POST",
                url: "/C_CheckBookSetting/SaveCheckbookSetting?CheckbookID=" + CheckbookID + "&CheckbookName=" + CheckbookName + "&Status=" + Status + "&CheckbookType=" + CheckbookType + "&Currency=" + Currency + "&AccountID=" + AccountID +
                    "&MinAmount=" + MinAmount + "&MaxAmount=" + MaxAmount + "&NextWithdraw=" + NextWithdraw + "&NextDeposit=" + NextDeposit + "&CurrentBalance=" + CurrentBalance + "&CurrentCash=" + CurrentCash +
                    "&LastReconsileBalance=" + LastReconsileBalance + "&LastReconsileDate=" + LastReconsileDate + "&BankName=" + BankName + "&BankAccountNumber=" + BankAccountNumber + "&BankAccountName=" + BankAccountName +
                    "&BranchName=" + BranchName + "&SwiftCode=" + SwiftCode + "&IBAN=" + IBAN + "&UserID=" + UserID + "&Password=" + Password,
                success: function (result) {
                    if (result == "CheckbookFalse") {
                        $("#SCCB-GlobalErrors").text("This Checkbook ID Is Already Exist");
                        $("#SCCB-checkbookID").focus();
                    } else if (result == "AccountIDExist") {
                        $("#SCCB-GlobalErrors").text("This Account ID Is Already Linked To a CheckBook");
                        $("#SCCB-accountID").focus();
                    }
                    else {
                        location.reload();
                    }
                }
            });
        }
    });

    // --------------- CheckBook Search on Change
    $("#SCCB-checkbookSearch").change(function () {
        var CheckbookID = $(this).val();
        $.ajax({
            type: "GET",
            url: "/C_CheckBookSetting/SearchCheckbook?CheckbookID=" + CheckbookID,
            success: function (result) {
                // Buttons
                $("#SCCB-CheckbookSave").prop("disabled", true);
                $("#SCCB-CheckbookUpdate").prop("disabled", false);
                $("#SCCB-CheckbookDelete").prop("disabled", false);

                $("#SCCB-checkbookID").val(result.CheckbookID);
                $("#SCCB-checkbookID").prop("disabled", true);

                $("#SCCB-checkbookName").val(result.CheckbookName);
                $("#SCCB-checkbookType").val(result.CheckbookType);
                $("#SCCB-checkbookType").prop("disabled", true);

                $("#SCCB-currency").val(result.CurrencyID);
                $("#SCCB-accountID").val(result.AccountID);
                $("#SCCB-accountName").val(result.AccountName);
                $("#SCCB-maxAmount").val(result.MaxAmount);
                $("#SCCB-minAmount").val(result.MinAmount);
                $("#SCCB-Nextwithdraw").val(result.NextWithDraw);
                $("#SCCB-Nextdeposit").val(result.NextDeposit);
                $("#SCCB-currentBalance").val(result.CurrentCheckbookBalance);
                $("#SCCB-currentCash").val(result.cashAccountBalance);
                $("#SCCB-lastReconcileBalance").val(result.LastReconsileBalance);
                $("#SCCB-lastReconcileDate").val(result.LastReconsileDate);
                $("#SCCB-bankName").val(result.BankName);
                $("#SCCB-bankAccountNumber").val(result.BankAccountNumber);
                $("#SCCB-bankAccountName").val(result.BankAccountName);
                $("#SCCB-branchName").val(result.BranchName);
                $("#SCCB-swiftCode").val(result.SwiftCode);
                $("#SCCB-IBAN").val(result.IBAN);
                $("#SCCB-securityUserID").val(result.UserID);
                $("#SCCB-securityPassword").val(result.Password);
                /////////////////////////////////////
                if (result.Status === true) {
                    $("#SCCB-checkbookStatus").attr('value', true);
                    $("#SCCB-checkbookStatus").prop('checked', true);
                }
                else {
                    $("#SCCB-checkbookStatus").attr('value', false);
                    $("#SCCB-checkbookStatus").prop('checked', false);
                }
                ///////////////////////////////////
                if (result.CheckbookType === "Bank") {
                    $("#SCCB-bankName").prop("disabled", false);
                    $("#SCCB-bankAccountNumber").prop("disabled", false);
                    $("#SCCB-bankAccountName").prop("disabled", false);
                    $("#SCCB-branchName").prop("disabled", false);
                    $("#SCCB-swiftCode").prop("disabled", false);
                    $("#SCCB-IBAN").prop("disabled", false);
                } else {
                    $("#SCCB-bankName").val("");
                    $("#SCCB-bankAccountNumber").val("");
                    $("#SCCB-bankAccountName").val("");
                    $("#SCCB-branchName").val("");
                    $("#SCCB-swiftCode").val("");
                    $("#SCCB-IBAN").val("");
                    $("#SCCB-bankName").prop("disabled", true);
                    $("#SCCB-bankAccountNumber").prop("disabled", true);
                    $("#SCCB-bankAccountName").prop("disabled", true);
                    $("#SCCB-branchName").prop("disabled", true);
                    $("#SCCB-swiftCode").prop("disabled", true);
                    $("#SCCB-IBAN").prop("disabled", true);
                }
            }
        });
    });

    // --------------- Button to Update Company-Checkbook-Setting
    $("#SCCB-CheckbookUpdate").click(function () {
        var FactoryID = $("#SCCB-factoryID").val();
        var CheckbookID = $("#SCCB-checkbookID").val();
        var CheckbookName = $("#SCCB-checkbookName").val();
        var Status = $("#SCCB-checkbookStatus").val();
        var CheckbookType = $("#SCCB-checkbookType").val();
        var Currency = $('#SCCB-currency').val();
        var AccountID = $('#SCCB-accountID').val();
        var MinAmount = $("#SCCB-minAmount").val();
        var MaxAmount = $("#SCCB-maxAmount").val();
        var NextWithdraw = $("#SCCB-Nextwithdraw").val();
        var NextDeposit = $("#SCCB-Nextdeposit").val();
        var CurrentBalance = $("#SCCB-currentBalance").val();
        var CurrentCash = $("#SCCB-currentCash").val();
        var LastReconsileBalance = $("#SCCB-lastReconcileBalance").val();
        var LastReconsileDate = $("#SCCB-lastReconcileDate").val();
        var BankName = $("#SCCB-bankName").val();
        var BankAccountNumber = $("#SCCB-bankAccountNumber").val();
        var BankAccountName = $("#SCCB-bankAccountName").val();
        var BranchName = $("#SCCB-branchName").val();
        var SwiftCode = $("#SCCB-swiftCode").val();
        var IBAN = $("#SCCB-IBAN").val();
        var UserID = $("#SCCB-securityUserID").val();
        var Password = $("#SCCB-securityPassword").val();
        var check = true;

        //------------Check if there's Required empty field-------------
        if (CheckbookName.length === 0) {
            $("#SCCB-checkbookName").css("border", "1px solid red");
            check = false;
        }
        else {
            $("#SCCB-checkbookName").css("border", "");
        }
        if (Currency.length === 0) {
            $("#SCCB-currency").css("border", "1px solid red");
            check = false;
        }
        else {
            $("#SCCB-currency").css("border", "");
        }
        if (AccountID.length === 0) {
            $("#SCCB-accountID").css("border", "1px solid red");
            check = false;
        }
        else {
            $("#SCCB-accountID").css("border", "");
        }
        if (NextWithdraw.length === 0) {
            $("#SCCB-Nextwithdraw").css("border", "1px solid red");
            check = false;
        }
        else {
            $("#SCCB-Nextwithdraw").css("border", "");
        }
        if (NextDeposit.length === 0) {
            $("#SCCB-NextDeposit").css("border", "1px solid red");
            check = false;
        }
        else {
            $("#SCCB-NextDeposit").css("border", "");
        }
        //-----------------------------------------------------------
        if (check === true) {
            $.ajax({
                type: "POST",
                url: "/C_CheckBookSetting/UpdateCheckbookSetting?CheckbookID=" + CheckbookID + "&CheckbookName=" + CheckbookName + "&Status=" + Status + "&CheckbookType=" + CheckbookType + "&Currency=" + Currency + "&AccountID=" + AccountID +
                    "&MinAmount=" + MinAmount + "&MaxAmount=" + MaxAmount + "&NextWithdraw=" + NextWithdraw + "&NextDeposit=" + NextDeposit + "&CurrentBalance=" + CurrentBalance + "&CurrentCash=" + CurrentCash +
                    "&LastReconsileBalance=" + LastReconsileBalance + "&LastReconsileDate=" + LastReconsileDate + "&BankName=" + BankName + "&BankAccountNumber=" + BankAccountNumber + "&BankAccountName=" + BankAccountName +
                    "&BranchName=" + BranchName + "&SwiftCode=" + SwiftCode + "&IBAN=" + IBAN + "&UserID=" + UserID + "&Password=" + Password,
                success: function (result) {
                    if (result == "True") {
                        $("#SCCB-GlobalSuccess").text("Checkbook Updated Successfuly");
                    }
                    else if (result == "False") {
                        $("#SCCB-GlobalErrors").text("This Account ID Is Already Linked To Another CheckBook");
                    }
                }
            });
        }
    });

    // --------------- Button to Delete Company-Checkbook-Setting
    $("#SCCB-CheckbookDelete").click(function () {
        var CheckbookID = $("#SCCB-checkbookID").val();
        $.ajax({
            type: "POST",
            url: "/C_CheckBookSetting/DeleteCheckbook?CheckbookID=" + CheckbookID,
            success: function (result) {
                if (result == "True") {
                    location.reload();
                }
            }
        });
    });





    // --------------- Reset Button
    $("#SCCB-CheckbookClear").click(function () {
        location.reload();
    });

    // --------------- Clear Function
    function Clear() {
        $("#SCCB-checkbookID").val("");
        $("#SCCB-checkbookName").val("");
        $("#SCCB-checkbookStatus").val("");
        $("#SCCB-checkbookType").val("");
        $('#SCCB-currency').val("");
        $('#SCCB-accountID').val("");
        $("#SCCB-maxAmount").val("");
        $("#SCCB-minAmount").val("");
        $("#SCCB-Nextwithdraw").val("");
        $("#SCCB-Nextdeposit").val("");
        $("#SCCB-currentBalance").val("");
        $("#SCCB-currentCash").val("");
        $("#SCCB-lastReconcileBalance").val("");
        $("#SCCB-lastReconcileDate").val("");
        $("#SCCB-bankName").val("");
        $("#SCCB-bankAccountNumber").val("");
        $("#SCCB-bankAccountName").val("");
        $("#SCCB-branchName").val("");
        $("#SCCB-swiftCode").val("");
        $("#SCCB-IBAN").val("");
        $("#SCCB-securityUserID").val("");
        $("#SCCB-securityPassword").val("");
    }







});