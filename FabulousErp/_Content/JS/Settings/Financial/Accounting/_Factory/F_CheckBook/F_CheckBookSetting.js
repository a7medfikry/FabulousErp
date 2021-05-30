$(document).ready(function () {
    var companyIDSession = $('#SFCB-CompanyID').text(),
        branchIDSession = $('#SFCB-BranchID').text(),
        factoryIDSession = $('#SFCB-FactoryID').text();

    //--------- ajax to get Factory-Name
    //--------- ajax to get Currency of Branch-ID related to Company-ID
    //--------- ajax to get Account-ID related to Branch-ID
    //--------- ajax to get Checkbook used for search
    if (companyIDSession.length > 0 || branchIDSession > 0) {
        $("#SFCB-factoryID").change(function () {
            Clear();
            var factoryID = $(this).val();
            FactoryData(factoryID);
        });
    } else if (factoryIDSession.length > 0) {
        FactoryData(factoryIDSession);
    }

    //--------- ajax to get Account-Name
    // Check The Currency Matching or Not
    $("#SFCB-accountID").change(function () {
        var AccountID = $(this).val();
        var currencyID = $('#SFCB-currency').val();
        var factoryID = $("#SFCB-factoryID").val();
        if (AccountID.length === 0) {
            $("#SFCB-accountName").val("");
        } else {
            if ($("#SFCB-currency").val().length === 0) {
                $("#SFCB-currency").focus();
                $("#SFCB-accountID").val("");
            } else {
                $.ajax({
                    type: "GET",
                    url: "/F_CheckBookSetting/GetAccountName?AccountID=" + AccountID + "&factoryID=" + factoryID,
                    success: function (result) {
                        for (var i = 0; i < result.length; i++) {
                            $("#SFCB-accountName").val(result[i].AccountName);
                        }
                    }
                });
                // Check The Currency Matching or Not
                $.ajax({
                    type: "GET",
                    url: "/F_CheckBookSetting/CheckAccountCurrency?AccountID=" + AccountID + "&currencyID=" + currencyID,
                    success: function (result) {
                        if (result === "False") {
                            $("#SFCB-GlobalErrors").text("This Account ID Currency Not Matching With The Chosen Currency!! ");
                            $("#SFCB-accountID").val("");
                            $("#SFCB-accountName").val("");
                        } else {
                            $("#SFCB-GlobalErrors").text("");
                        }
                    }
                });
            }
        }
    });

    //---------- Currency Change
    $("#SFCB-currency").change(function () {
        $("#SFCB-accountID").val("");
        $("#SFCB-accountName").val("");
        $("#SFCB-GlobalErrors").text("");
    });

    //-------- Change Between Cash and Bank
    $("#SFCB-checkbookType").change(function () {
        var Type = $(this).val();
        if (Type === "Cash") {
            $("#SFCB-bankName").prop("disabled", true);
            $("#SFCB-bankAccountNumber").prop("disabled", true);
            $("#SFCB-bankAccountName").prop("disabled", true);
            $("#SFCB-branchName2").prop("disabled", true);
            $("#SFCB-swiftCode").prop("disabled", true);
            $("#SFCB-IBAN").prop("disabled", true);
            $("#SFCB-bankName").val("");
            $("#SFCB-bankAccountNumber").val("");
            $("#SFCB-bankAccountName").val("");
            $("#SFCB-branchName2").val("");
            $("#SFCB-swiftCode").val("");
            $("#SFCB-IBAN").val("");
        }
        else if (Type === "Bank") {
            $("#SFCB-bankName").prop("disabled", false);
            $("#SFCB-bankAccountNumber").prop("disabled", false);
            $("#SFCB-bankAccountName").prop("disabled", false);
            $("#SFCB-branchName2").prop("disabled", false);
            $("#SFCB-swiftCode").prop("disabled", false);
            $("#SFCB-IBAN").prop("disabled", false);
        }
    });

    // --------------- Change Between User-ID and Password of Checkbook-Security
    $('#SFCB-CBuserID').change(function () {
        $('#SFCB-securityUserID').prop("disabled", false);
        $('#SFCB-securityPassword').prop("disabled", true);
        $('#SFCB-securityPassword').val("");
    });
    $('#SFCB-CBpassword').change(function () {
        $('#SFCB-securityUserID').prop("disabled", true);
        $('#SFCB-securityUserID').val("");
        $('#SFCB-securityPassword').prop("disabled", false);
    });

    //-------- change value of checkbox before Insert to database
    $("#SFCB-checkbookStatus").change(function () {
        if ($('#SFCB-checkbookStatus').is(":checked")) {
            $(this).attr('value', true);
        }
        else {
            $(this).attr('value', false);
        }
    });

    // --------------- Button to Save Factory-Checkbook-Setting
    $("#SFCB-CheckbookSave").click(function () {
        var FactoryID = $("#SFCB-factoryID").val();
        var CheckbookID = $("#SFCB-checkbookID").val();
        var CheckbookName = $("#SFCB-checkbookName").val();
        var Status = $("#SFCB-checkbookStatus").val();
        var CheckbookType = $("#SFCB-checkbookType").val();
        var Currency = $('#SFCB-currency').val();
        var AccountID = $('#SFCB-accountID').val();
        var MinAmount = $("#SFCB-maxAmount").val();
        var MaxAmount = $("#SFCB-minAmount").val();
        var NextWithdraw = $("#SFCB-Nextwithdraw").val();
        var NextDeposit = $("#SFCB-Nextdeposit").val();
        var CurrentBalance = $("#SFCB-currentBalance").val();
        var CurrentCash = $("#SFCB-currentCash").val();
        var LastReconsileBalance = $("#SFCB-lastReconcileBalance").val();
        var LastReconsileDate = $("#SFCB-lastReconcileDate").val();
        var BankName = $("#SFCB-bankName").val();
        var BankAccountNumber = $("#SFCB-bankAccountNumber").val();
        var BankAccountName = $("#SFCB-bankAccountName").val();
        var BranchName = $("#SFCB-branchName2").val();
        var SwiftCode = $("#SFCB-swiftCode").val();
        var IBAN = $("#SFCB-IBAN").val();
        var UserID = $("#SFCB-securityUserID").val();
        var Password = $("#SFCB-securityPassword").val();
        var check = true;

        //------------Check if there's Required empty field-------------
        if (FactoryID.length === 0) {
            $("#SFCB-factoryID").css("border", "1px solid red");
            check = false;
        }
        else {
            $("#SFCB-factoryID").css("border", "");
        }
        if (CheckbookID.length === 0) {
            $("#SFCB-checkbookID").css("border", "1px solid red");
            check = false;
        }
        else {
            $("#SFCB-checkbookID").css("border", "");
        }
        if (CheckbookName.length === 0) {
            $("#SFCB-checkbookName").css("border", "1px solid red");
            check = false;
        }
        else {
            $("#SFCB-checkbookName").css("border", "");
        }
        if (CheckbookType.length === 0) {
            $("#SFCB-checkbookType").css("border", "1px solid red");
            check = false;
        }
        else {
            $("#SFCB-checkbookType").css("border", "");
        }
        if (Currency.length === 0) {
            $("#SFCB-currency").css("border", "1px solid red");
            check = false;
        }
        else {
            $("#SFCB-currency").css("border", "");
        }
        if (AccountID.length === 0) {
            $("#SFCB-accountID").css("border", "1px solid red");
            check = false;
        }
        else {
            $("#SFCB-accountID").css("border", "");
        }
        if (NextWithdraw.length === 0) {
            $("#SFCB-Nextwithdraw").css("border", "1px solid red");
            check = false;
        }
        else {
            $("#SFCB-Nextwithdraw").css("border", "");
        }
        if (NextDeposit.length === 0) {
            $("#SFCB-NextDeposit").css("border", "1px solid red");
            check = false;
        }
        else {
            $("#SFCB-NextDeposit").css("border", "");
        }
        //-------------------------------------------------------------------------
        if (check === true) {
            $.ajax({
                type: "POST",
                url: "/F_CheckBookSetting/SaveCheckbookSetting?CheckbookID=" + CheckbookID + "&CheckbookName=" + CheckbookName + "&Status=" + Status + "&CheckbookType=" + CheckbookType + "&Currency=" + Currency + "&AccountID=" + AccountID +
                    "&MinAmount=" + MinAmount + "&MaxAmount=" + MaxAmount + "&NextWithdraw=" + NextWithdraw + "&NextDeposit=" + NextDeposit + "&CurrentBalance=" + CurrentBalance + "&CurrentCash=" + CurrentCash +
                    "&LastReconsileBalance=" + LastReconsileBalance + "&LastReconsileDate=" + LastReconsileDate + "&BankName=" + BankName + "&BankAccountNumber=" + BankAccountNumber + "&BankAccountName=" + BankAccountName +
                    "&BranchName=" + BranchName + "&SwiftCode=" + SwiftCode + "&IBAN=" + IBAN + "&UserID=" + UserID + "&Password=" + Password + "&FactoryID=" + FactoryID,
                success: function (result) {
                    if (result == "CheckbookFalse") {
                        $("#SFCB-GlobalErrors").text("This Checkbook ID Is Already Exist");
                        $("#SFCB-checkbookID").focus();
                    } else if (result == "AccountIDExist") {
                        $("#SFCB-GlobalErrors").text("This Account ID Is Already Linked To a CheckBook");
                        $("#SFCB-accountID").focus();
                    }
                    else {
                        location.reload();
                    }
                }
            });
        }


    });

    // --------------- CheckBook Search on Change
    $("#SFCB-checkbookSearch").change(function () {
        var CheckbookID = $(this).val();
        var factoryID = $("#SFCB-factoryID").val();
        $.ajax({
            type: "GET",
            url: "/F_CheckBookSetting/SearchCheckbook?CheckbookID=" + CheckbookID + "&factoryID=" + factoryID,
            success: function (result) {
                // Buttons
                $("#SFCB-CheckbookSave").prop("disabled", true);
                $("#SFCB-CheckbookUpdate").prop("disabled", false);
                $("#SFCB-CheckbookDelete").prop("disabled", false);

                $("#SFCB-checkbookID").val(result.CheckbookID);
                $("#SFCB-checkbookID").prop("disabled", true);
                $("#SFCB-checkbookName").val(result.CheckbookName);
                $("#SFCB-checkbookType").val(result.CheckbookType);
                $("#SFCB-checkbookType").prop("disabled", true);
                $("#SFCB-currency").val(result.CurrencyID);
                $("#SFCB-accountID").val(result.AccountID);
                $("#SFCB-accountName").val(result.AccountName);
                $("#SFCB-maxAmount").val(result.MaxAmount);
                $("#SFCB-minAmount").val(result.MinAmount);
                $("#SFCB-Nextwithdraw").val(result.NextWithDraw);
                $("#SFCB-Nextdeposit").val(result.NextDeposit);
                $("#SFCB-currentBalance").val(result.CurrentCheckbookBalance);
                $("#SFCB-currentCash").val(result.CurrentCashBalance);
                $("#SFCB-lastReconcileBalance").val(result.LastReconsileBalance);
                $("#SFCB-lastReconcileDate").val(result.LastReconsileDate);
                $("#SFCB-bankName").val(result.BankName);
                $("#SFCB-bankAccountNumber").val(result.BankAccountNumber);
                $("#SFCB-bankAccountName").val(result.BankAccountName);
                $("#SFCB-branchName2").val(result.BranchName);
                $("#SFCB-swiftCode").val(result.SwiftCode);
                $("#SFCB-IBAN").val(result.IBAN);
                $("#SFCB-securityUserID").val(result.UserID);
                $("#SFCB-securityPassword").val(result.Password);
                /////////////////////////
                if (result.Status === true) {
                    $("#SFCB-checkbookStatus").attr('value', true);
                    $("#SFCB-checkbookStatus").prop('checked', true);
                }
                else {
                    $("#SFCB-checkbookStatus").attr('value', false);
                    $("#SFCB-checkbookStatus").prop('checked', false);
                }
                /////////////////////////
                if (result.CheckbookType === "Bank") {
                    $("#SFCB-bankName").prop("disabled", false);
                    $("#SFCB-bankAccountNumber").prop("disabled", false);
                    $("#SFCB-bankAccountName").prop("disabled", false);
                    $("#SFCB-branchName2").prop("disabled", false);
                    $("#SFCB-swiftCode").prop("disabled", false);
                    $("#SFCB-IBAN").prop("disabled", false);
                } else {
                    $("#SFCB-bankName").val("");
                    $("#SFCB-bankAccountNumber").val("");
                    $("#SFCB-bankAccountName").val("");
                    $("#SFCB-branchName").val("");
                    $("#SFCB-swiftCode").val("");
                    $("#SFCB-IBAN").val("");
                    $("#SFCB-bankName").prop("disabled", true);
                    $("#SFCB-bankAccountNumber").prop("disabled", true);
                    $("#SFCB-bankAccountName").prop("disabled", true);
                    $("#SFCB-branchName2").prop("disabled", true);
                    $("#SFCB-swiftCode").prop("disabled", true);
                    $("#SFCB-IBAN").prop("disabled", true);
                }
            }
        });
        // Close Pop-up Modal
        $('#SFCB-searchModal').modal("hide");
    });

    // --------------- Button to Update Factory-Checkbook-Setting
    $("#SFCB-CheckbookUpdate").click(function () {
        var FactoryID = $("#SFCB-factoryID").val();
        var CheckbookID = $("#SFCB-checkbookID").val();
        var CheckbookName = $("#SFCB-checkbookName").val();
        var Status = $("#SFCB-checkbookStatus").val();
        var CheckbookType = $("#SFCB-checkbookType").val();
        var Currency = $('#SFCB-currency').val();
        var AccountID = $('#SFCB-accountID').val();
        var MinAmount = $("#SFCB-maxAmount").val();
        var MaxAmount = $("#SFCB-minAmount").val();
        var NextWithdraw = $("#SFCB-Nextwithdraw").val();
        var NextDeposit = $("#SFCB-Nextdeposit").val();
        var CurrentBalance = $("#SFCB-currentBalance").val();
        var CurrentCash = $("#SFCB-currentCash").val();
        var LastReconsileBalance = $("#SFCB-lastReconcileBalance").val();
        var LastReconsileDate = $("#SFCB-lastReconcileDate").val();
        var BankName = $("#SFCB-bankName").val();
        var BankAccountNumber = $("#SFCB-bankAccountNumber").val();
        var BankAccountName = $("#SFCB-bankAccountName").val();
        var BranchName = $("#SFCB-branchName2").val();
        var SwiftCode = $("#SFCB-swiftCode").val();
        var IBAN = $("#SFCB-IBAN").val();
        var UserID = $("#SFCB-securityUserID").val();
        var Password = $("#SFCB-securityPassword").val();
        var check = true;

        //------------Check if there's Required empty field-------------
        if (CheckbookName.length === 0) {
            $("#SFCB-checkbookName").css("border", "1px solid red");
            check = false;
        }
        else {
            $("#SFCB-checkbookName").css("border", "");
        }
        if (Currency.length === 0) {
            $("#SFCB-currency").css("border", "1px solid red");
            check = false;
        }
        else {
            $("#SFCB-currency").css("border", "");
        }
        if (AccountID.length === 0) {
            $("#SFCB-accountID").css("border", "1px solid red");
            check = false;
        }
        else {
            $("#SFCB-accountID").css("border", "");
        }
        if (NextWithdraw.length === 0) {
            $("#SFCB-Nextwithdraw").css("border", "1px solid red");
            check = false;
        }
        else {
            $("#SFCB-Nextwithdraw").css("border", "");
        }
        if (NextDeposit.length === 0) {
            $("#SFCB-NextDeposit").css("border", "1px solid red");
            check = false;
        }
        else {
            $("#SFCB-NextDeposit").css("border", "");
        }
        //-----------------------------------------------------------
        if (check === true) {
            $.ajax({
                type: "POST",
                url: "/F_CheckBookSetting/UpdateCheckbookSetting?CheckbookID=" + CheckbookID + "&CheckbookName=" + CheckbookName + "&Status=" + Status + "&CheckbookType=" + CheckbookType + "&Currency=" + Currency + "&AccountID=" + AccountID +
                    "&MinAmount=" + MinAmount + "&MaxAmount=" + MaxAmount + "&NextWithdraw=" + NextWithdraw + "&NextDeposit=" + NextDeposit + "&CurrentBalance=" + CurrentBalance + "&CurrentCash=" + CurrentCash +
                    "&LastReconsileBalance=" + LastReconsileBalance + "&LastReconsileDate=" + LastReconsileDate + "&BankName=" + BankName + "&BankAccountNumber=" + BankAccountNumber + "&BankAccountName=" + BankAccountName +
                    "&BranchName=" + BranchName + "&SwiftCode=" + SwiftCode + "&IBAN=" + IBAN + "&UserID=" + UserID + "&Password=" + Password + "&FactoryID=" + FactoryID,
                success: function (result) {
                    if (result == "True") {
                        $("#SFCB-GlobalSuccess").text("Checkbook Updated Successfuly");
                    }
                }
            });
        }
    });

    // --------------- Button to Delete Company-Checkbook-Setting
    $("#SFCB-CheckbookDelete").click(function () {
        var CheckbookID = $("#SFCB-checkbookID").val();
        $.ajax({
            type: "POST",
            url: "/F_CheckBookSetting/DeleteCheckbook?CheckbookID=" + CheckbookID,
            success: function (result) {
                if (result == "True") {
                    location.reload();
                }
            }
        });
    });





    // --------------- Reset Button
    $("#SFCB-CheckbookClear").click(function () {
        location.reload();
    });


});

// --------------- Clear Function
function Clear() {
    $("#SFCB-checkbookID").val("");
    $("#SFCB-checkbookName").val("");
    $("#SFCB-checkbookStatus").val("");
    $("#SFCB-checkbookType").val("");
    $('#SFCB-currency').val("");
    $('#SFCB-accountID').val("");
    $("#SFCB-maxAmount").val("");
    $("#SFCB-minAmount").val("");
    $("#SFCB-Nextwithdraw").val("");
    $("#SFCB-Nextdeposit").val("");
    $("#SFCB-currentBalance").val("");
    $("#SFCB-currentCash").val("");
    $("#SFCB-lastReconcileBalance").val("");
    $("#SFCB-lastReconcileDate").val("");
    $("#SFCB-bankName").val("");
    $("#SFCB-bankAccountNumber").val("");
    $("#SFCB-bankAccountName").val("");
    $("#SFCB-branchName2").val("");
    $("#SFCB-swiftCode").val("");
    $("#SFCB-IBAN").val("");
    $("#SFCB-securityUserID").val("");
    $("#SFCB-securityPassword").val("");
}

function FactoryData(factoryID) {
    $.ajax({
        type: "GET",
        url: "/F_CheckBookSetting/GetFactoryName?factoryID=" + factoryID,
        success: function (result) {
            $("#SFCB-factoryName").val(result);
        }
    });
    $.ajax({
        type: "GET",
        url: "/F_CheckBookSetting/GetCurrency?factoryID=" + factoryID,
        success: function (result) {
            if (result.length > 0) {
                $("#SFCB-currency").empty();
                $("#SFCB-currency").append($('<option/>', {
                    value: "",
                    text: "-Choose-"
                })
                );
                $.each(result, function (index, row) {
                    $("#SFCB-currency").append("<option value='" + row.CurrencyID + "'>" + row.CurrencyName + "</option>");
                });
            }
        }
    });
    $.ajax({
        type: "GET",
        url: "/F_CheckBookSetting/GetAccountsID?factoryID=" + factoryID,
        success: function (result) {
            if (result.length > 0) {
                $("#SFCB-accountID").empty();
                $("#SFCB-accountID").append($('<option/>', {
                    value: "",
                    text: "-Choose-"
                })
                );
                $.each(result, function (index, row) {
                    $("#SFCB-accountID").append("<option value='" + row.AccountID + "'>" + row.Factory_AccountsID + "(" + row.AccountName + ")" + "</option>");
                });
            } else {
                $("#SFCB-accountID").empty();
            }
        }
    });
    $.ajax({
        type: "GET",
        url: "/F_CheckBookSetting/GetFactoryCheckbook?factoryID=" + factoryID,
        success: function (result) {
            if (result.length > 0) {
                $("#SFCB-checkbookSearch").empty();
                $("#SFCB-checkbookSearch").append($('<option/>', {
                    value: "",
                    text: "-Choose-"
                })
                );
                $.each(result, function (index, row) {
                    $("#SFCB-checkbookSearch").append("<option value='" + row.CheckbookID + "'>" + row.CheckbookID + "</option>");
                });
            } else {
                $("#SFCB-checkbookSearch").empty();
            }
        }
    });
}