$(document).ready(function () {

    ViewData();

    GetCurrenciesForDropDown();

});


$("#CurrencyID").keyup(function (event) {

    event.preventDefault();
    if (event.keyCode === 13) {
        $("#addToList").click();
    }

    var CurrencyID = $(this).val();
    if (CurrencyID.length == 0) {
        $("#CurrencyID").css("border-color", "red");
    } else {
        $("#CurrencyID").css("border-color", "");
    }

}).focusout(function () {
    var CurrencyID = $(this).val();

    if (CurrencyID.length == 0) {
        $("#CurrencyID").css("border-color", "red");
    } else {
        $("#CurrencyID").css("border-color", "");
    }

});


$("#CurrencyName").keyup(function (event) {

    event.preventDefault();
    if (event.keyCode === 13) {
        $("#addToList").click();
    }

    var CurrencyName = $(this).val();
    if (CurrencyName.length == 0) {
        $("#CurrencyName").css("border-color", "red");
    } else {
        $("#CurrencyName").css("border-color", "");
    }

}).focusout(function () {
    var CurrencyName = $(this).val();

    if (CurrencyName.length == 0) {
        $("#CurrencyName").css("border-color", "red");
    } else {
        $("#CurrencyName").css("border-color", "");
    }

});


$("#ISOCode").keyup(function (event) {

    event.preventDefault();
    if (event.keyCode === 13) {
        $("#addToList").click();
    }

    var ISOCode = $(this).val();
    if (ISOCode.length == 0) {
        $("#ISOCode").css("border-color", "red");
    } else {
        $("#ISOCode").css("border-color", "");
    }

}).focusout(function () {
    var ISOCode = $(this).val();

    if (ISOCode.length == 0) {
        $("#ISOCode").css("border-color", "red");
    } else {
        $("#ISOCode").css("border-color", "");
    }

});

$("#addToList").click(function (e) {

    e.preventDefault();

    var Test = false;

    var CurrencyID = $("#CurrencyID").val();
    var CurrencyName = $("#CurrencyName").val();
    var ISOCode = $("#ISOCode").val();
    //var accountID = $("#AccountID").val();
    //var accountID2 = $("#AccountID2").val();

    if (CurrencyID.length == 0) {
        $("#CurrencyID").css("border-color", "red");
        $("#CurrencyID").focus();
        Test = true;
    } else {
        $("#CurrencyID").css("border-color", "");
    }

    if (CurrencyName.length == 0) {
        $("#CurrencyName").css("border-color", "red");
        $("#CurrencyName").focus();
        Test = true;
    } else {
        $("#CurrencyName").css("border-color", "");
    }

    if (ISOCode.length == 0) {
        $("#ISOCode").css("border-color", "red");
        $("#ISOCode").focus();
        Test = true;
    } else {
        $("#ISOCode").css("border-color", "");
    }

    //if (accountID.length === 0) {
    //    $("#AccountID").css("border-color", "red");
    //    $("#AccountID").focus();
    //    Test = true;
    //} else {
    //    $("#AccountID").css("border-color", "");
    //}

    //if (accountID2.length === 0) {
    //    $("#AccountID2").css("border-color", "red");
    //    $("#AccountID2").focus();
    //    Test = true;
    //} else {
    //    $("#AccountID2").css("border-color", "");
    //}

    if (Test === false) {

        $.ajax({
            type: "POST",
            url: "/CurrenciesDefinition/SaveCurrencyRecord?CurrencyID=" + CurrencyID + "&CurrencyName=" + CurrencyName + "&ISOCode=" + ISOCode,
            success: function (result) {
                if (result === "DCIDG") {
                    $("#GlobalError").text("Currency ID not Available Change it Please..");
                    $("#CurrencyID").css("border-color", "red");
                    $("#CurrencyID").focus();
                    $("#CurrencyName").css("border-color", "");
                    $("#ISOCode").css("border-color", "");
                } else if (result === "DCID") {
                    $("#GlobalError").text("Currency ID already Exist..");
                    $("#CurrencyID").css("border-color", "red");
                    $("#CurrencyID").focus();
                    $("#CurrencyName").css("border-color", "");
                    $("#ISOCode").css("border-color", "");
                } else if (result === "DCN") {
                    $("#GlobalError").text("Currency Name already Exist..");
                    $("#CurrencyID").css("border-color", "");
                    $("#CurrencyName").css("border-color", "red");
                    $("#CurrencyName").focus();
                    $("#ISOCode").css("border-color", "");
                } else if (result === "DISOC") {
                    $("#GlobalError").text("ISO Code already Exist..");
                    $("#CurrencyID").css("border-color", "");
                    $("#CurrencyName").css("border-color", "");
                    $("#ISOCode").css("border-color", "red");
                    $("#ISOCode").focus();
                } else if (result === "True") {
                    $("#GlobalError").text("");
                    $("#CurrencyID").css("border-color", "");
                    $("#CurrencyName").css("border-color", "");
                    $("#ISOCode").css("border-color", "");
                    clearItem();
                    GetCurrenciesForDropDown();
                }
            }
        });
    }
});

function clearItem() {
    ViewData();
    $("#CurrencyID").focus();
    $("#CurrencyID").val("");
    $("#CurrencyName").val("");
    $("#ISOCode").val("");
    $("#AccountID").val("");
    $("#AccountName").val("");
}


function ViewData() {

    var CurrencyData = $("#SetCurrencyInfo");

    CurrencyData.html("");

    $.ajax({
        type: "GET",
        url: "/CurrenciesDefinition/GetCurrenciesDefinition",
        contentType: "html",
        success: function (result) {
            if (result.length == 0) {
                CurrencyData.append('<tr style="color:red"><td colspan = "6" > This Company not has Currencies Definition </td></tr>');
            }
            else {
                for (var i = 0; i < result.length; i++) {
                    var AccountID = "";
                    if (result[i].AccountID !== null) {
                        AccountID = result[i].AccountID;
                    }
                    var Data = "<tr class='row_" + result[i].CurrencyID + "'>" +
                        "<td>" + result[i].CurrencyID + "</td>" +
                        "<td>" + result[i].CurrencyName + "</td>" +
                        "<td>" + result[i].ISOCode + "</td>" +
                        //"<td>" + AccountID + "</td>" +
                        "<td>" + '<a href="#" onclick="DeleteCurrency(\'' + result[i].CurrencyID + '\')"><span class="fa fa-trash-o"></span></a>' + "</td>" +
                        "<td>" + '<input type="checkbox" id="' + result[i].CurrencyID + '" onchange="DisactiveCurrency(\'' + result[i].CurrencyID + '\')" ' + result[i].DisActive + ' >' + "</td>" +
                        "</tr>";
                    CurrencyData.append(Data);
                }
            }
        }
    });

}

function DeleteCurrency(CurrencyID) {
    $("#CurrencyIDH").text(CurrencyID);

    $("#Deletebtn").show();
    $("#DisActivebtn").hide();
    $("#Activebtn").hide();

    $("#CancelDelete").show();
    $("#CancelDisActive").hide();
    $("#CancelActive").hide();

    $("#popUpHearder").text("Delete Currency");
    $("#popUpContent").text("Are You Sure? You Want To Delete This Currency..!");
    $("#DeleteConfirmation").modal("show");
}

function ConfirmDelete() {
    var CurrencyID = $("#CurrencyIDH").text();

    $.ajax({
        type: "POST",
        url: "/CurrenciesDefinition/DeleteCurrenciesDefinition?CurrencyID=" + CurrencyID,
        success: function (result) {
            $("#DeleteConfirmation").modal("hide");
            $(".row_" + CurrencyID).remove();
        }
    });
}

function DisactiveCurrency(CurrencyID) {

    $("#CurrencyIDH").text(CurrencyID);

    if ($('#' + CurrencyID + '').is(':checked')) {

        $("#Activebtn").hide();
        $("#Deletebtn").hide();
        $("#DisActivebtn").show();

        $("#CancelDelete").hide();
        $("#CancelActive").hide();
        $("#CancelDisActive").show();

        $("#popUpHearder").text("Dis Active Currency");
        $("#popUpContent").text("Are You Sure? You Want To Dis Active This Currency..!");
        $("#DeleteConfirmation").modal("show");

    } else {

        $("#Activebtn").show();
        $("#Deletebtn").hide();
        $("#DisActivebtn").hide();

        $("#CancelDelete").hide();
        $("#CancelActive").show();
        $("#CancelDisActive").hide();

        $("#popUpHearder").text("Active Currency");
        $("#popUpContent").text("Are You Sure? You Want To Active This Currency..!");
        $("#DeleteConfirmation").modal("show");

    }
}

function ConfirmDisActive() {
    var CurrencyID = $("#CurrencyIDH").text();

    $.ajax({
        type: "POST",
        url: "/CurrenciesDefinition/DisActiveCurrenciesDefinition?CurrencyID=" + CurrencyID,
        success: function (result) {
            $("#DeleteConfirmation").modal("hide");
        }
    });
}

function CancelD() {
    var CurrencyID = $("#CurrencyIDH").text();

    $('#' + CurrencyID + '').prop("checked", false);
}

function ConfirmActive() {
    var CurrencyID = $("#CurrencyIDH").text();

    $.ajax({
        type: "POST",
        url: "/CurrenciesDefinition/ActiveCurrenciesDefinition?CurrencyID=" + CurrencyID,
        success: function (result) {
            $("#DeleteConfirmation").modal("hide");
        }
    });
}

function CancelA() {
    var CurrencyID = $("#CurrencyIDH").text();

    $('#' + CurrencyID + '').prop("checked", true);
}


$("#AccountID").change(function () {

    $("#AccountName").val("");
    var accountID = $(this).val();
    if (accountID.length === 0) {
        $(this).css("border-color", "red");
    } else {
        $(this).css("border-color", "");

        $.ajax({
            type: "GET",
            url: "/CurrenciesDefinition/GetAccountName?C_AID=" + accountID,
            success: function (result) {
                $("#AccountName").val(result);
            }
        });

    }

});
$("#AccountID2").change(function () {

    $("#AccountName2").val("");
    var accountID = $(this).val();
    if (accountID.length === 0) {
        $(this).css("border-color", "red");
    } else {
        $(this).css("border-color", "");

        $.ajax({
            type: "GET",
            url: "/CurrenciesDefinition/GetAccountName?C_AID=" + accountID,
            success: function (result) {
                $("#AccountName2").val(result);
            }
        });

    }

});

$("#AccountCurrencyID").change(function () {
    var currencyID = $(this).val();
    $("#AccountCurrencyName").val("");
    $("#AccountID").css("border-color", "");
    $("#AccountID2").css("border-color", "");

    if (currencyID.length === 0) {
        $(this).css("border-color", "red");
    } else {
        $(this).css("border-color", "");

        $.ajax({
            type: "GET",
            url: "/CurrenciesDefinition/GetCurrencyName?currencyID=" + currencyID,
            success: function (result) {
                $("#AccountCurrencyName").val(result);

                $.ajax({
                    type: "GET",
                    url: "/CurrenciesDefinition/GetAccountsOfCurrency?currencyID=" + currencyID,
                    success: function (result) {

                        if (result.length > 0) {
                            $("#UpdateAccToCurr").show();
                            $("#AddAccToCurr").hide();

                            $.each(result, function (index, row) {
                                if (row.Type == "Profit") {
                                    $("#AccountID").val(row.C_AID);
                                    $("#AccountName").val(row.AccountName);
                                }
                                if (row.Type == "Loss") {
                                    $("#AccountID2").val(row.C_AID);
                                    $("#AccountName2").val(row.AccountName);
                                }

                            });
                        } else {
                            $("#UpdateAccToCurr").hide();
                            $("#AddAccToCurr").show();
                            $("#AccountID").val("");
                            $("#AccountName").val("");
                            $("#AccountID2").val("");
                            $("#AccountName2").val("");
                        }
                    }
                });

            }
        });

    }
});

function GetCurrenciesForDropDown() {
    $.ajax({
        type: "GET",
        url: "/CurrenciesDefinition/GetCurrenciesDefinition",
        success: function (result) {
            $("#AccountCurrencyID").empty();

            if (result.length == 0) {
                $("#AccountCurrencyID").append($('<option/>', {
                    value: "",
                    text: "No Currencies Created"
                })
                );
            } else {

                $("#AccountCurrencyID").append($('<option/>', {
                    value: "",
                    text: "-Choose-"
                })
                );
                $.each(result, function (index, row) {

                    $("#AccountCurrencyID").append("<option value='" + row.CurrencyID + "'>" + row.CurrencyID + "</option>");

                });
            }
        }
    });
}

$("#AddAccToCurr").click(function () {

    var currencyID = $("#AccountCurrencyID").val();

    var accountID1 = $("#AccountID").val();

    var accountID2 = $("#AccountID2").val();

    var test = true;

    if (currencyID.length === 0) {
        $("#AccountCurrencyID").css("border-color", "red");
        test = false;
    } else {
        $("#AccountCurrencyID").css("border-color", "");
    }

    if (accountID1.length === 0) {
        $("#AccountID").css("border-color", "red");
        test = false;
    } else {
        $("#AccountID").css("border-color", "");
    }

    if (accountID2.length === 0) {
        $("#AccountID2").css("border-color", "red");
        test = false;
    } else {
        $("#AccountID2").css("border-color", "");
    }

    if (test === true) {
        $.ajax({
            type: "POST",
            url: "/CurrenciesDefinition/SaveAccountToCurrency?currencyID=" + currencyID + "&c_AID=" + accountID1 + "&c_AID2=" + accountID2,
            success: function (result) {
                if (result !== "False") {
                    location.reload();
                }
            }
        });
    }

});

$("#UpdateAccToCurr").click(function () {

    var currencyID = $("#AccountCurrencyID").val();

    var accountID1 = $("#AccountID").val();

    var accountID2 = $("#AccountID2").val();

    var test = true;

    if (currencyID.length === 0) {
        $("#AccountCurrencyID").css("border-color", "red");
        test = false;
    } else {
        $("#AccountCurrencyID").css("border-color", "");
    }

    if (accountID1.length === 0) {
        $("#AccountID").css("border-color", "red");
        test = false;
    } else {
        $("#AccountID").css("border-color", "");
    }

    if (accountID2.length === 0) {
        $("#AccountID2").css("border-color", "red");
        test = false;
    } else {
        $("#AccountID2").css("border-color", "");
    }

    if (test === true) {
        $.ajax({
            type: "POST",
            url: "/CurrenciesDefinition/UpdateAccountToCurrency?currencyID=" + currencyID + "&c_AID=" + accountID1 + "&c_AID2=" + accountID2,
            success: function (result) {
                location.reload();
            }
        });
    }

});
