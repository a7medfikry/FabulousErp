$(document).ready(function () {
    var companyIDSession = $('#SBCB-CompanyID').text(),
        branchIDSession = $('#SBCB-BranchID').text(),
        factoryIDSession = $('#SBCB-factoryID').text();

    //--------- ajax to get Branch-Name
    //--------- ajax to get Currency of Branch-ID related to Company-ID
    //--------- ajax to get Account-ID related to Branch-ID
    //--------- ajax to get Checkbook used for search
    if (companyIDSession.length > 0) {
        $("#SBCB-branchID").change(function () {
            Clear();
            var branchID = $(this).val();
            BranchData(branchID);
        });
    } else if (branchIDSession.length > 0) {
        BranchData(branchIDSession);
    } else if (factoryIDSession.length > 0) {
        Talert('fff');
    }


    //--------- ajax to get Account-Name
    // Check The Currency Matching or Not
    $("#SBCB-accountID").change(function () {
        var AccountID = $(this).val();
        var currencyID = $('#SBCB-currency').val();
        var branchID = $("#SBCB-branchID").val();
        if (AccountID.length === 0) {
            $("#SBCB-accountName").val("");
        } else {
            if ($("#SBCB-currency").val().length === 0) {
                $("#SBCB-currency").focus();
                $("#SBCB-accountID").val("");
            } else {
                $.ajax({
                    type: "GET",
                    url: "/B_CheckBookSetting/GetAccountName?AccountID=" + AccountID + "&branchID=" + branchID,
                    success: function (result) {
                        for (var i = 0; i < result.length; i++) {
                            $("#SBCB-accountName").val(result[i].AccountName);
                        }
                    }
                });
                // Check The Currency Matching or Not
                $.ajax({
                    type: "GET",
                    url: "/B_CheckBookSetting/CheckAccountCurrency?AccountID=" + AccountID + "&currencyID=" + currencyID,
                    success: function (result) {
                        if (result === "False") {
                            $("#SBCB-GlobalErrors").text("This Account ID Currency Not Matching With The Chosen Currency!! ");
                            $("#SBCB-accountID").val("");
                            $("#SBCB-accountName").val("");
                        } else {
                            $("#SBCB-GlobalErrors").text("");
                        }
                    }
                });
            }
        }
    });

    //---------------- change value of checkbox before Insert to database
    $("#SBCB-checkbookStatus").change(function () {
        if ($('#SBCB-checkbookStatus').is(":checked")) {
            $(this).attr('value', true);
        }
        else {
            $(this).attr('value', false);
        }
    });

    //---------------- Currency Change
    $("#SBCB-currency").change(function () {
        $("#SBCB-accountID").val("");
        $("#SBCB-accountName").val("");
        $("#SBCB-GlobalErrors").text("");
    });

    //---------------- Change Between Cash and Bank
    $("#SBCB-checkbookType").change(function () {
        var Type = $(this).val();
        if (Type === "Cash") {
            $("#SBCB-bankName").prop("disabled", true);
            $("#SBCB-bankAccountNumber").prop("disabled", true);
            $("#SBCB-bankAccountName").prop("disabled", true);
            $("#SBCB-branchName2").prop("disabled", true);
            $("#SBCB-swiftCode").prop("disabled", true);
            $("#SBCB-IBAN").prop("disabled", true);
            $("#SBCB-bankName").val("");
            $("#SBCB-bankAccountNumber").val("");
            $("#SBCB-bankAccountName").val("");
            $("#SBCB-branchName2").val("");
            $("#SBCB-swiftCode").val("");
            $("#SBCB-IBAN").val("");
        }
        else if (Type === "Bank") {
            $("#SBCB-bankName").prop("disabled", false);
            $("#SBCB-bankAccountNumber").prop("disabled", false);
            $("#SBCB-bankAccountName").prop("disabled", false);
            $("#SBCB-branchName2").prop("disabled", false);
            $("#SBCB-swiftCode").prop("disabled", false);
            $("#SBCB-IBAN").prop("disabled", false);
        }
    });

    // --------------- Change Between User-ID and Password of Checkbook-Security
    $('#SBCB-CBuserID').change(function () {
        $('#SBCB-securityUserID').prop("disabled", false);
        $('#SBCB-securityPassword').prop("disabled", true);
        $('#SBCB-securityPassword').val("");
    });
    $('#SBCB-CBpassword').change(function () {
        $('#SBCB-securityUserID').prop("disabled", true);
        $('#SBCB-securityUserID').val("");
        $('#SBCB-securityPassword').prop("disabled", false);
    });

    // --------------- Button to Save Branch-Checkbook-Setting
    $("#SBCB-CheckbookSave").click(function () {
        var BranchID = $("#SBCB-branchID").val();
        var CheckbookID = $("#SBCB-checkbookID").val();
        var CheckbookName = $("#SBCB-checkbookName").val();
        var Status = $("#SBCB-checkbookStatus").val();
        var CheckbookType = $("#SBCB-checkbookType").val();
        var Currency = $('#SBCB-currency').val();
        var AccountID = $('#SBCB-accountID').val();
        var MinAmount = $("#SBCB-maxAmount").val();
        var MaxAmount = $("#SBCB-minAmount").val();
        var NextWithdraw = $("#SBCB-Nextwithdraw").val();
        var NextDeposit = $("#SBCB-Nextdeposit").val();
        var CurrentBalance = $("#SBCB-currentBalance").val();
        var CurrentCash = $("#SBCB-currentCash").val();
        var LastReconsileBalance = $("#SBCB-lastReconcileBalance").val();
        var LastReconsileDate = $("#SBCB-lastReconcileDate").val();
        var BankName = $("#SBCB-bankName").val();
        var BankAccountNumber = $("#SBCB-bankAccountNumber").val();
        var BankAccountName = $("#SBCB-bankAccountName").val();
        var BranchName = $("#SBCB-branchName2").val();
        var SwiftCode = $("#SBCB-swiftCode").val();
        var IBAN = $("#SBCB-IBAN").val();
        var UserID = $("#SBCB-securityUserID").val();
        var Password = $("#SBCB-securityPassword").val();
        var check = true;

        //------------Check if there's Required empty field-------------
        if (companyIDSession.length > 0) {
            if (BranchID.length === 0) {
                $("#SBCB-branchID").css("border", "1px solid red");
                check = false;
            }
            else {
                $("#SBCB-branchID").css("border", "");
            }
        }
        if (CheckbookID.length === 0) {
            $("#SBCB-checkbookID").css("border", "1px solid red");
            check = false;
        }
        else {
            $("#SBCB-checkbookID").css("border", "");
        }
        if (CheckbookName.length === 0) {
            $("#SBCB-checkbookName").css("border", "1px solid red");
            check = false;
        }
        else {
            $("#SBCB-checkbookName").css("border", "");
        }
        if (CheckbookType.length === 0) {
            $("#SBCB-checkbookType").css("border", "1px solid red");
            check = false;
        }
        else {
            $("#SBCB-checkbookType").css("border", "");
        }
        if (Currency.length === 0) {
            $("#SBCB-currency").css("border", "1px solid red");
            check = false;
        }
        else {
            $("#SBCB-currency").css("border", "");
        }
        if (AccountID.length === 0) {
            $("#SBCB-accountID").css("border", "1px solid red");
            check = false;
        }
        else {
            $("#SBCB-accountID").css("border", "");
        }
        if (NextWithdraw.length === 0) {
            $("#SBCB-Nextwithdraw").css("border", "1px solid red");
            check = false;
        }
        else {
            $("#SBCB-Nextwithdraw").css("border", "");
        }
        if (NextDeposit.length === 0) {
            $("#SBCB-NextDeposit").css("border", "1px solid red");
            check = false;
        }
        else {
            $("#SBCB-NextDeposit").css("border", "");
        }
        //-------------------------------------------------------------------------
        if (check === true) {
            $.ajax({
                type: "POST",
                url: "/B_CheckBookSetting/SaveCheckbookSetting?CheckbookID=" + CheckbookID + "&CheckbookName=" + CheckbookName + "&Status=" + Status + "&CheckbookType=" + CheckbookType + "&Currency=" + Currency + "&AccountID=" + AccountID +
                    "&MinAmount=" + MinAmount + "&MaxAmount=" + MaxAmount + "&NextWithdraw=" + NextWithdraw + "&NextDeposit=" + NextDeposit + "&CurrentBalance=" + CurrentBalance + "&CurrentCash=" + CurrentCash +
                    "&LastReconsileBalance=" + LastReconsileBalance + "&LastReconsileDate=" + LastReconsileDate + "&BankName=" + BankName + "&BankAccountNumber=" + BankAccountNumber + "&BankAccountName=" + BankAccountName +
                    "&BranchName=" + BranchName + "&SwiftCode=" + SwiftCode + "&IBAN=" + IBAN + "&UserID=" + UserID + "&Password=" + Password + "&BranchID=" + BranchID,
                success: function (result) {
                    if (result == "CheckbookFalse") {
                        $("#SBCB-GlobalErrors").text("This Checkbook ID Is Already Exist");
                        $("#SBCB-checkbookID").focus();
                    } else if (result == "AccountIDExist") {
                        $("#SBCB-GlobalErrors").text("This Account ID Is Already Linked To a CheckBook");
                        $("#SBCB-accountID").focus();
                    }
                    else {
                        location.reload();
                    }
                }
            });
        }


    });

    // --------------- CheckBook Search on Change
    $("#SBCB-checkbookSearch").change(function () {
        var CheckbookID = $(this).val();
        var branchID = $("#SBCB-branchID").val();
        $.ajax({
            type: "GET",
            url: "/B_CheckBookSetting/SearchCheckbook?CheckbookID=" + CheckbookID + "&branchID=" + branchID,
            success: function (result) {
                // Buttons
                $("#SBCB-CheckbookSave").prop("disabled", true);
                $("#SBCB-CheckbookUpdate").prop("disabled", false);
                $("#SBCB-CheckbookDelete").prop("disabled", false);

                $("#SBCB-checkbookID").val(result.CheckbookID);
                $("#SBCB-checkbookID").prop("disabled", true);

                $("#SBCB-checkbookName").val(result.CheckbookName);
                $("#SBCB-checkbookType").val(result.CheckbookType);
                $("#SBCB-checkbookType").prop("disabled", true);
                $("#SBCB-currency").val(result.CurrencyID);
                $("#SBCB-accountID").val(result.AccountID);
                $("#SBCB-accountName").val(result.AccountName);
                $("#SBCB-maxAmount").val(result.MaxAmount);
                $("#SBCB-minAmount").val(result.MinAmount);
                $("#SBCB-Nextwithdraw").val(result.NextWithDraw);
                $("#SBCB-Nextdeposit").val(result.NextDeposit);
                $("#SBCB-currentBalance").val(result.CurrentCheckbookBalance);
                $("#SBCB-currentCash").val(result.CurrentCashBalance);
                $("#SBCB-lastReconcileBalance").val(result.LastReconsileBalance);
                $("#SBCB-lastReconcileDate").val(result.LastReconsileDate);
                $("#SBCB-bankName").val(result.BankName);
                $("#SBCB-bankAccountNumber").val(result.BankAccountNumber);
                $("#SBCB-bankAccountName").val(result.BankAccountName);
                $("#SBCB-branchName2").val(result.BranchName);
                $("#SBCB-swiftCode").val(result.SwiftCode);
                $("#SBCB-IBAN").val(result.IBAN);
                $("#SBCB-securityUserID").val(result.UserID);
                $("#SBCB-securityPassword").val(result.Password);
                //////////////////////////////////
                if (result.Status === true) {
                    $("#SBCB-checkbookStatus").attr('value', true);
                    $("#SBCB-checkbookStatus").prop('checked', true);
                }
                else {
                    $("#SBCB-checkbookStatus").attr('value', false);
                    $("#SBCB-checkbookStatus").prop('checked', false);
                }
                //////////////////////////////////
                if (result.CheckbookType === "Bank") {
                    $("#SBCB-bankName").prop("disabled", false);
                    $("#SBCB-bankAccountNumber").prop("disabled", false);
                    $("#SBCB-bankAccountName").prop("disabled", false);
                    $("#SBCB-branchName2").prop("disabled", false);
                    $("#SBCB-swiftCode").prop("disabled", false);
                    $("#SBCB-IBAN").prop("disabled", false);
                } else {
                    $("#SBCB-bankName").val("");
                    $("#SBCB-bankAccountNumber").val("");
                    $("#SBCB-bankAccountName").val("");
                    $("#SBCB-branchName").val("");
                    $("#SBCB-swiftCode").val("");
                    $("#SBCB-IBAN").val("");
                    $("#SBCB-bankName").prop("disabled", true);
                    $("#SBCB-bankAccountNumber").prop("disabled", true);
                    $("#SBCB-bankAccountName").prop("disabled", true);
                    $("#SBCB-branchName2").prop("disabled", true);
                    $("#SBCB-swiftCode").prop("disabled", true);
                    $("#SBCB-IBAN").prop("disabled", true);
                }
            }
        });
        // Close Pop-up Modal
        $('#SBCB-searchModal').modal("hide");
    });

    // --------------- Button to Update Branch-Checkbook-Setting
    $("#SBCB-CheckbookUpdate").click(function () {
        var BranchID = $("#SBCB-branchID").val();
        var CheckbookID = $("#SBCB-checkbookID").val();
        var CheckbookName = $("#SBCB-checkbookName").val();
        var Status = $("#SBCB-checkbookStatus").val();
        var CheckbookType = $("#SBCB-checkbookType").val();
        var Currency = $('#SBCB-currency').val();
        var AccountID = $('#SBCB-accountID').val();
        var MinAmount = $("#SBCB-maxAmount").val();
        var MaxAmount = $("#SBCB-minAmount").val();
        var NextWithdraw = $("#SBCB-Nextwithdraw").val();
        var NextDeposit = $("#SBCB-Nextdeposit").val();
        var CurrentBalance = $("#SBCB-currentBalance").val();
        var CurrentCash = $("#SBCB-currentCash").val();
        var LastReconsileBalance = $("#SBCB-lastReconcileBalance").val();
        var LastReconsileDate = $("#SBCB-lastReconcileDate").val();
        var BankName = $("#SBCB-bankName").val();
        var BankAccountNumber = $("#SBCB-bankAccountNumber").val();
        var BankAccountName = $("#SBCB-bankAccountName").val();
        var BranchName = $("#SBCB-branchName2").val();
        var SwiftCode = $("#SBCB-swiftCode").val();
        var IBAN = $("#SBCB-IBAN").val();
        var UserID = $("#SBCB-securityUserID").val();
        var Password = $("#SBCB-securityPassword").val();
        var check = true;

        //------------Check if there's Required empty field-------------
        if (CheckbookName.length === 0) {
            $("#SBCB-checkbookName").css("border", "1px solid red");
            check = false;
        }
        else {
            $("#SBCB-checkbookName").css("border", "");
        }
        if (Currency.length === 0) {
            $("#SBCB-currency").css("border", "1px solid red");
            check = false;
        }
        else {
            $("#SBCB-currency").css("border", "");
        }
        if (AccountID.length === 0) {
            $("#SBCB-accountID").css("border", "1px solid red");
            check = false;
        }
        else {
            $("#SBCB-accountID").css("border", "");
        }
        if (NextWithdraw.length === 0) {
            $("#SBCB-Nextwithdraw").css("border", "1px solid red");
            check = false;
        }
        else {
            $("#SBCB-Nextwithdraw").css("border", "");
        }
        if (NextDeposit.length === 0) {
            $("#SBCB-NextDeposit").css("border", "1px solid red");
            check = false;
        }
        else {
            $("#SBCB-NextDeposit").css("border", "");
        }
        //-----------------------------------------------------------
        if (check === true) {
            $.ajax({
                type: "POST",
                url: "/B_CheckBookSetting/UpdateCheckbookSetting?CheckbookID=" + CheckbookID + "&CheckbookName=" + CheckbookName + "&Status=" + Status + "&CheckbookType=" + CheckbookType + "&Currency=" + Currency + "&AccountID=" + AccountID +
                    "&MinAmount=" + MinAmount + "&MaxAmount=" + MaxAmount + "&NextWithdraw=" + NextWithdraw + "&NextDeposit=" + NextDeposit + "&CurrentBalance=" + CurrentBalance + "&CurrentCash=" + CurrentCash +
                    "&LastReconsileBalance=" + LastReconsileBalance + "&LastReconsileDate=" + LastReconsileDate + "&BankName=" + BankName + "&BankAccountNumber=" + BankAccountNumber + "&BankAccountName=" + BankAccountName +
                    "&BranchName=" + BranchName + "&SwiftCode=" + SwiftCode + "&IBAN=" + IBAN + "&UserID=" + UserID + "&Password=" + Password + "&BranchID=" + BranchID,
                success: function (result) {
                    if (result == "True") {
                        $("#SBCB-GlobalSuccess").text("Checkbook Updated Successfuly");
                    }
                }
            });
        }
    });

    // --------------- Button to Delete Branch-Checkbook-Setting
    $("#SBCB-CheckbookDelete").click(function () {
        var CheckbookID = $("#SBCB-checkbookID").val();
        $.ajax({
            type: "POST",
            url: "/B_CheckBookSetting/DeleteCheckbook?CheckbookID=" + CheckbookID,
            success: function (result) {
                if (result == "True") {
                    location.reload();
                }
            }
        });
    });



    // --------------- Reset Button
    $("#SBCB-CheckbookClear").click(function () {
        location.reload();
    });


});

function BranchData(branchID) {
    // Get Branch-Name
    $.ajax({
        type: "GET",
        url: "/B_CheckBookSetting/GetBranchName?branchID=" + branchID,
        success: function (result) {
            $("#SBCB-branchName").val(result);
        }
    });
    // Get Currency OF Branch-ID Releated to Company-ID
    $.ajax({
        type: "GET",
        url: "/B_CheckBookSetting/GetCurrency?branchID=" + branchID,
        success: function (result) {
            if (result.length > 0) {
                $("#SBCB-currency").empty();
                $("#SBCB-currency").append($('<option/>', {
                    value: "",
                    text: "-Choose-"
                })
                );
                $.each(result, function (index, row) {
                    $("#SBCB-currency").append("<option value='" + row.CurrencyID + "'>" + row.CurrencyName + "</option>");
                });
            }
        }
    });
    // Get Accounts-ID related to Branch-ID
    $.ajax({
        type: "GET",
        url: "/B_CheckBookSetting/GetAccountsID?branchID=" + branchID,
        success: function (result) {
            if (result.length > 0) {
                $("#SBCB-accountID").empty();
                $("#SBCB-accountID").append($('<option/>', {
                    value: "",
                    text: "-Choose-"
                })
                );
                $.each(result, function (index, row) {
                    $("#SBCB-accountID").append("<option value='" + row.AccountID + "'>" + row.Branch_AccountsID + "(" + row.AccountName + ")" + "</option>");
                });
            }
        }
    });
    // Get All Branch-Checkbook For Search
    $.ajax({
        type: "GET",
        url: "/B_CheckBookSetting/GetBranchCheckbook?branchID=" + branchID,
        success: function (result) {
            if (result.length > 0) {
                $("#SBCB-checkbookSearch").empty();
                $("#SBCB-checkbookSearch").append($('<option/>', {
                    value: "",
                    text: "-Choose-"
                })
                );
                $.each(result, function (index, row) {
                    $("#SBCB-checkbookSearch").append("<option value='" + row.CheckbookID + "'>" + row.CheckbookID + "</option>");
                });
            } else {
                $("#SBCB-checkbookSearch").empty();
            }
        }
    });
}

// --------------- Clear Function
function Clear() {
    $("#SBCB-checkbookID").val("");
    $("#SBCB-checkbookName").val("");
    $("#SBCB-checkbookStatus").val("");
    $("#SBCB-checkbookType").val("");
    $('#SBCB-currency').val("");
    $('#SBCB-accountID').val("");
    $("#SBCB-maxAmount").val("");
    $("#SBCB-minAmount").val("");
    $("#SBCB-Nextwithdraw").val("");
    $("#SBCB-Nextdeposit").val("");
    $("#SBCB-currentBalance").val("");
    $("#SBCB-currentCash").val("");
    $("#SBCB-lastReconcileBalance").val("");
    $("#SBCB-lastReconcileDate").val("");
    $("#SBCB-bankName").val("");
    $("#SBCB-bankAccountNumber").val("");
    $("#SBCB-bankAccountName").val("");
    $("#SBCB-branchName2").val("");
    $("#SBCB-swiftCode").val("");
    $("#SBCB-IBAN").val("");
    $("#SBCB-securityUserID").val("");
    $("#SBCB-securityPassword").val("");
}
