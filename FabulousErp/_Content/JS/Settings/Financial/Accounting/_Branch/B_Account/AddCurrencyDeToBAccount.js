$(document).ready(function () {

    var branchIDFB = $("#CDTBA-BranchIDFB").val();
    if ($("#CDTBA-BranchIDFB").length > 0) {
        $("#CDTBA-AccountCurrencyID").prop("disabled", false);
        GetBranchAccounts(branchIDFB);
    }

    $("#CDTBA-BranchID").change(function () {

        var branchID = $(this).val();
        $("#CDTBA-BranchName").val("");
        $("#CDTBA-AccountID").empty();
        $("#CDTBA-AccountID2").empty();
        $("#CDTBA-AddAccToCurr").show();
        $("#CDTBA-UpdateAccToCurr").hide();
        $("#CDTBA-AccountCurrencyID").prop("disabled", true);
        $("#CDTBA-AccountCurrencyID").val("");
        $("#CDTBA-AccountCurrencyName").val("");

        $("#CDTBA-AccountName").val("");
        $("#CDTBA-AccountName2").val("");
        $("#CDTBA-AccessError").text("");

        if (branchID.length > 0) {
            $.ajax({
                type: "GET",
                url: "/B_LinkCDtoBA/GetBranchName?branchID=" + branchID,
                success: function (result) {
                    if (result === "NotAccess") {
                        $("#CDTBA-BranchID").val("");
                        $("#CDTBA-AccessError").text("You Not Have Access To This Branch..!");
                    } else {
                        $("#CDTBA-AccountCurrencyID").prop("disabled", false);
                        $("#CDTBA-BranchName").val(result);
                        GetBranchAccounts(branchID);
                    }
                }
            });
        }

    });

    $("#CDTBA-AccountID").change(function () {

        $("#CDTBA-AccountName").val("");
        var accountID = $(this).val();
        if (accountID.length === 0) {
            $(this).css("border-color", "red");
        } else {
            $(this).css("border-color", "");

            $.ajax({
                type: "GET",
                url: "/B_LinkCDtoBA/GetAccountName?b_AID=" + accountID,
                success: function (result) {
                    $("#CDTBA-AccountName").val(result);
                }
            });

        }

    });
    $("#CDTBA-AccountID2").change(function () {

        $("#CDTBA-AccountName2").val("");
        var accountID = $(this).val();
        if (accountID.length === 0) {
            $(this).css("border-color", "red");
        } else {
            $(this).css("border-color", "");

            $.ajax({
                type: "GET",
                url: "/B_LinkCDtoBA/GetAccountName?b_AID=" + accountID,
                success: function (result) {
                    $("#CDTBA-AccountName2").val(result);
                }
            });

        }

    });

    $("#CDTBA-AccountCurrencyID").change(function () {
        var currencyID = $(this).val();
        $("#CDTBA-AccountCurrencyName").val("");
        $("#CDTBA-AccountID").css("border-color", "");
        $("#CDTBA-AccountID2").css("border-color", "");
        $("#CDTBA-AccountID").val("");
        $("#CDTBA-AccountID2").val("");
        $("#CDTBA-AccountName").val("");
        $("#CDTBA-AccountName2").val("");
        $("#CDTBA-AddAccToCurr").show();
        $("#CDTBA-UpdateAccToCurr").hide();

        if (currencyID.length === 0) {
            $(this).css("border-color", "red");
        } else {
            $(this).css("border-color", "");

            $.ajax({
                type: "GET",
                url: "/B_LinkCDtoBA/GetCurrencyName?currencyID=" + currencyID,
                success: function (result) {
                    $("#CDTBA-AccountCurrencyName").val(result);

                    $.ajax({
                        type: "GET",
                        url: "/B_LinkCDtoBA/GetAccountsOfCurrency?currencyID=" + currencyID,
                        success: function (result) {

                            if (result.length > 0) {
                                $("#CDTBA-UpdateAccToCurr").show();
                                $("#CDTBA-AddAccToCurr").hide();

                                $.each(result, function (index, row) {
                                    if (row.Type === "Profit") {
                                        $("#CDTBA-AccountID").val(row.B_AID);
                                        $("#CDTBA-AccountName").val(row.AccountName);
                                    }
                                    if (row.Type === "Loss") {
                                        $("#CDTBA-AccountID2").val(row.B_AID);
                                        $("#CDTBA-AccountName2").val(row.AccountName);
                                    }

                                });
                            } else {
                                $("#CDTBA-UpdateAccToCurr").hide();
                                $("#CDTBA-AddAccToCurr").show();
                                $("#CDTBA-AccountID").val("");
                                $("#CDTBA-AccountName").val("");
                                $("#CDTBA-AccountID2").val("");
                                $("#CDTBA-AccountName2").val("");
                            }
                        }
                    });
                }
            });
        }
    });


    $("#CDTBA-AddAccToCurr").click(function () {

        var currencyID = $("#CDTBA-AccountCurrencyID").val();

        var accountID1 = $("#CDTBA-AccountID").val();

        var accountID2 = $("#CDTBA-AccountID2").val();

        var test = true;

        if (currencyID.length === 0) {
            $("#CDTBA-AccountCurrencyID").css("border-color", "red");
            test = false;
        } else {
            $("#CDTBA-AccountCurrencyID").css("border-color", "");
        }

        if (accountID1.length === 0) {
            $("#CDTBA-AccountID").css("border-color", "red");
            test = false;
        } else {
            $("#CDTBA-AccountID").css("border-color", "");
        }

        if (accountID2.length === 0) {
            $("#CDTBA-AccountID2").css("border-color", "red");
            test = false;
        } else {
            $("#CDTBA-AccountID2").css("border-color", "");
        }

        if (test === true) {
            $.ajax({
                type: "POST",
                url: "/B_LinkCDtoBA/SaveAccountToCurrency?currencyID=" + currencyID + "&b_AID=" + accountID1 + "&b_AID2=" + accountID2,
                success: function (result) {
                    if (result !== "False") {
                        location.reload();
                    }
                }
            });
        }

    });

    $("#CDTBA-UpdateAccToCurr").click(function () {

        var currencyID = $("#CDTBA-AccountCurrencyID").val();

        var accountID1 = $("#CDTBA-AccountID").val();

        var accountID2 = $("#CDTBA-AccountID2").val();

        var test = true;

        if (currencyID.length === 0) {
            $("#CDTBA-AccountCurrencyID").css("border-color", "red");
            test = false;
        } else {
            $("#CDTBA-AccountCurrencyID").css("border-color", "");
        }

        if (accountID1.length === 0) {
            $("#CDTBA-AccountID").css("border-color", "red");
            test = false;
        } else {
            $("#CDTBA-AccountID").css("border-color", "");
        }

        if (accountID2.length === 0) {
            $("#CDTBA-AccountID2").css("border-color", "red");
            test = false;
        } else {
            $("#CDTBA-AccountID2").css("border-color", "");
        }

        if (test === true) {
            $.ajax({
                type: "POST",
                url: "/B_LinkCDtoBA/UpdateAccountToCurrency?currencyID=" + currencyID + "&b_AID=" + accountID1 + "&b_AID2=" + accountID2,
                success: function (result) {
                    location.reload();
                }
            });
        }

    });

});

function GetBranchAccounts(branchID) {

    $.ajax({

        type: "GET",
        url: "/B_LinkCDtoBA/GetBranchAccounts?branchID=" + branchID,
        success: function (result) {

            if (result.length == 0) {
                $(".CDTBA-rAccountID").append($('<option/>', {
                    value: "",
                    text: "No Accounts Created To This Branch"
                })
                );
            } else {

                $(".CDTBA-rAccountID").append($('<option/>', {
                    value: "",
                    text: "-Choose-"
                })
                );
                $.each(result, function (index, row) {

                    $(".CDTBA-rAccountID").append("<option value='" + row.B_AID + "'>" + row.AccountID + " ( " + row.AccountName + " )" + "</option>");

                });
            }

        }

    });

}