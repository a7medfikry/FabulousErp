$(document).ready(function () {

    var factoryIDFB = $("#CDTFA-FactoryIDFF").val();
    if ($("#CDTFA-FactoryIDFF").length > 0) {
        $("#CDTFA-AccountCurrencyID").prop("disabled", false);
        GetFactoryAccounts(factoryIDFB);
    }

    $("#CDTFA-FactoryID").change(function () {

        var factoryID = $(this).val();
        $("#CDTFA-FactoryName").val("");
        $("#CDTFA-AccountID").empty();
        $("#CDTFA-AccountID2").empty();
        $("#CDTFA-AddAccToCurr").show();
        $("#CDTFA-UpdateAccToCurr").hide();
        $("#CDTFA-AccountCurrencyID").prop("disabled", true);
        $("#CDTFA-AccountCurrencyID").val("");
        $("#CDTFA-AccountCurrencyName").val("");

        $("#CDTFA-AccountName").val("");
        $("#CDTFA-AccountName2").val("");
        $("#CDTFA-AccessError").text("");

        if (factoryID.length > 0) {
            $.ajax({
                type: "GET",
                url: "/F_LinkCDtoFA/GetFactoryName?factoryID=" + factoryID,
                success: function (result) {
                    if (result === "NotAccess") {
                        $("#CDTFA-FactoryID").val("");
                        $("#CDTFA-AccessError").text("You Not Have Access To This Factory..!");
                    } else {
                        $("#CDTFA-AccountCurrencyID").prop("disabled", false);
                        $("#CDTFA-FactoryName").val(result);
                        GetFactoryAccounts(factoryID);
                    }
                }
            });
        }

    });

    $("#CDTFA-AccountID").change(function () {

        $("#CDTFA-AccountName").val("");
        var accountID = $(this).val();
        if (accountID.length === 0) {
            $(this).css("border-color", "red");
        } else {
            $(this).css("border-color", "");

            $.ajax({
                type: "GET",
                url: "/F_LinkCDtoFA/GetAccountName?f_AID=" + accountID,
                success: function (result) {
                    $("#CDTFA-AccountName").val(result);
                }
            });

        }

    });
    $("#CDTFA-AccountID2").change(function () {

        $("#CDTFA-AccountName2").val("");
        var accountID = $(this).val();
        if (accountID.length === 0) {
            $(this).css("border-color", "red");
        } else {
            $(this).css("border-color", "");

            $.ajax({
                type: "GET",
                url: "/F_LinkCDtoFA/GetAccountName?f_AID=" + accountID,
                success: function (result) {
                    $("#CDTFA-AccountName2").val(result);
                }
            });

        }

    });

    $("#CDTFA-AccountCurrencyID").change(function () {
        var currencyID = $(this).val();
        $("#CDTFA-AccountCurrencyName").val("");
        $("#CDTFA-AccountID").css("border-color", "");
        $("#CDTFA-AccountID2").css("border-color", "");
        $("#CDTFA-AccountID").val("");
        $("#CDTFA-AccountID2").val("");
        $("#CDTFA-AccountName").val("");
        $("#CDTFA-AccountName2").val("");
        $("#CDTFA-AddAccToCurr").show();
        $("#CDTFA-UpdateAccToCurr").hide();

        if (currencyID.length === 0) {
            $(this).css("border-color", "red");
        } else {
            $(this).css("border-color", "");

            $.ajax({
                type: "GET",
                url: "/F_LinkCDtoFA/GetCurrencyName?currencyID=" + currencyID,
                success: function (result) {
                    $("#CDTFA-AccountCurrencyName").val(result);

                    $.ajax({
                        type: "GET",
                        url: "/F_LinkCDtoFA/GetAccountsOfCurrency?currencyID=" + currencyID,
                        success: function (result) {

                            if (result.length > 0) {
                                $("#CDTFA-UpdateAccToCurr").show();
                                $("#CDTFA-AddAccToCurr").hide();

                                $.each(result, function (index, row) {
                                    if (row.Type === "Profit") {
                                        $("#CDTFA-AccountID").val(row.F_AID);
                                        $("#CDTFA-AccountName").val(row.AccountName);
                                    }
                                    if (row.Type === "Loss") {
                                        $("#CDTFA-AccountID2").val(row.F_AID);
                                        $("#CDTFA-AccountName2").val(row.AccountName);
                                    }

                                });
                            } else {
                                $("#CDTFA-UpdateAccToCurr").hide();
                                $("#CDTFA-AddAccToCurr").show();
                                $("#CDTFA-AccountID").val("");
                                $("#CDTFA-AccountName").val("");
                                $("#CDTFA-AccountID2").val("");
                                $("#CDTFA-AccountName2").val("");
                            }
                        }
                    });
                }
            });
        }
    });


    $("#CDTFA-AddAccToCurr").click(function () {

        var currencyID = $("#CDTFA-AccountCurrencyID").val();

        var accountID1 = $("#CDTFA-AccountID").val();

        var accountID2 = $("#CDTFA-AccountID2").val();

        var test = true;

        if (currencyID.length === 0) {
            $("#CDTFA-AccountCurrencyID").css("border-color", "red");
            test = false;
        } else {
            $("#CDTFA-AccountCurrencyID").css("border-color", "");
        }

        if (accountID1.length === 0) {
            $("#CDTFA-AccountID").css("border-color", "red");
            test = false;
        } else {
            $("#CDTFA-AccountID").css("border-color", "");
        }

        if (accountID2.length === 0) {
            $("#CDTFA-AccountID2").css("border-color", "red");
            test = false;
        } else {
            $("#CDTFA-AccountID2").css("border-color", "");
        }

        if (test === true) {
            $.ajax({
                type: "POST",
                url: "/F_LinkCDtoFA/SaveAccountToCurrency?currencyID=" + currencyID + "&f_AID=" + accountID1 + "&f_AID2=" + accountID2,
                success: function (result) {
                    if (result !== "False") {
                        location.reload();
                    }
                }
            });
        }

    });

    $("#CDTFA-UpdateAccToCurr").click(function () {

        var currencyID = $("#CDTFA-AccountCurrencyID").val();

        var accountID1 = $("#CDTFA-AccountID").val();

        var accountID2 = $("#CDTFA-AccountID2").val();

        var test = true;

        if (currencyID.length === 0) {
            $("#CDTFA-AccountCurrencyID").css("border-color", "red");
            test = false;
        } else {
            $("#CDTFA-AccountCurrencyID").css("border-color", "");
        }

        if (accountID1.length === 0) {
            $("#CDTFA-AccountID").css("border-color", "red");
            test = false;
        } else {
            $("#CDTFA-AccountID").css("border-color", "");
        }

        if (accountID2.length === 0) {
            $("#CDTFA-AccountID2").css("border-color", "red");
            test = false;
        } else {
            $("#CDTFA-AccountID2").css("border-color", "");
        }

        if (test === true) {
            $.ajax({
                type: "POST",
                url: "/F_LinkCDtoFA/UpdateAccountToCurrency?currencyID=" + currencyID + "&f_AID=" + accountID1 + "&f_AID2=" + accountID2,
                success: function (result) {
                    location.reload();
                }
            });
        }

    });

});

function GetFactoryAccounts(factoryID) {

    $.ajax({

        type: "GET",
        url: "/F_LinkCDtoFA/GetFactoryAccounts?factoryID=" + factoryID,
        success: function (result) {

            if (result.length == 0) {
                $(".CDTFA-rAccountID").append($('<option/>', {
                    value: "",
                    text: "No Accounts Created To This Branch"
                })
                );
            } else {

                $(".CDTFA-rAccountID").append($('<option/>', {
                    value: "",
                    text: "-Choose-"
                })
                );
                $.each(result, function (index, row) {

                    $(".CDTFA-rAccountID").append("<option value='" + row.F_AID + "'>" + row.AccountID + " ( " + row.AccountName + " )" + "</option>");

                });
            }

        }

    });

}