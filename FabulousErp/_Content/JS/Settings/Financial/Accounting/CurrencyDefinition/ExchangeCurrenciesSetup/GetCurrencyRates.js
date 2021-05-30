var dateformat = /^(19|20)\d\d-(0\d|1[012])-(0\d|1\d|2\d|3[01])$/;

$("#CurrencyID").change(function () {

    var CurrencyID = $(this).val();

    if (CurrencyID.length > 0) {

        $.ajax({
            type: "GET",
            url: "/ExchangeCurrenciesSetup/GetCurrencyName?CurrencyID=" + CurrencyID,
            success: function (data) {
                $("#CurrencyName").val(data.Name);
                $("#CurrencyID").css("border-color", "");
                GetRates();
            }
        });
    }
    else {
        var RatesData = $("#SetExchangeCurrencyInfo");
        RatesData.html("");
        $("#CurrencyName").val("");
        $("#CurrencyID").css("border-color", "red");
    }
});





$("#StartDate").keyup(function (event) {

    event.preventDefault();
    if (event.keyCode === 13) {
        $("#addToList").click();
    }

    var StartDate = $(this).val();

    if (StartDate.length === 0) {
        $("#StartDate").css("border-color", "red");
    } else if (!StartDate.match(dateformat)) {
        $("#StartDate").css("border-color", "red");
    } else {
        $("#StartDate").css("border-color", "");
    }
}).focusout(function () {
    var StartDate = $(this).val();

    if (StartDate.length === 0) {
        $("#StartDate").css("border-color", "red");
    } else if (!StartDate.match(dateformat)) {
        $("#StartDate").css("border-color", "red");
    } else {

        $.ajax({
            type: "GET",
            url: "/ExchangeCurrenciesSetup/CheckStartDate?StartDate=" + StartDate,
            success: function (result) {
                if (result.Message == "NoYear") {
                    $("#StartDate").css("border-color", "red");
                    $("#StartDate").focus();
                    $("#GlobalError").text('No Fiscal Year Related With Company ID' + result.CompanyID + '');
                }
                else if (result == "NewYearNotExist") {
                    $("#StartDate").css("border-color", "red");
                    $("#StartDate").focus();
                    $("#GlobalError").text("This Year Not Created..!");
                }
                else if (result == "NewYearClosed") {
                    $("#StartDate").css("border-color", "red");
                    $("#StartDate").focus();
                    $("#GlobalError").text("This Year Closed..!");
                }
                else if (result == "False") {
                    $("#StartDate").css("border-color", "red");
                    $("#StartDate").focus();
                    $("#GlobalError").text("This Date Out of Range..!");
                }
                else if (result == "True") {
                    $("#StartDate").css("border-color", "");
                    $("#GlobalError").text("");
                    var date = new Date(StartDate);

                    date.setDate(date.getDate() + 31);

                    var day = date.getDate();
                    var month = date.getMonth() + 1;
                    var year = date.getFullYear();

                    var newDate = year + '-' + ('0' + month).slice(-2) + '-'
                        + ('0' + day).slice(-2);

                    $("#ExpireDate").val(newDate);
                }
            }
        });

    }
});

$("#ExpireDate").keyup(function (event) {

    event.preventDefault();
    if (event.keyCode === 13) {
        $("#addToList").click();
    }

    var ExpireDate = $(this).val();

    if (ExpireDate.length === 0) {
        $("#ExpireDate").css("border-color", "red");
    } else if (!ExpireDate.match(dateformat)) {
        $("#ExpireDate").css("border-color", "red");
    } else {
        $("#ExpireDate").css("border-color", "");
    }
}).focusout(function () {
    var ExpireDate = $(this).val();

    if (ExpireDate.length === 0) {
        $("#ExpireDate").css("border-color", "red");
    } else if (!ExpireDate.match(dateformat)) {
        $("#ExpireDate").css("border-color", "red");
    } else {
        $.ajax({
            type: "GET",
            url: "/ExchangeCurrenciesSetup/CheckEndDate?EndDate=" + ExpireDate,
            success: function (result) {
                if (result.Message == "NoYear") {
                    $("#ExpireDate").css("border-color", "red");
                    $("#ExpireDate").focus();
                    $("#GlobalError").text('No Fiscal Year Related With Company ID' + result.CompanyID + '');
                }
                else if (result == "NewYearNotExist") {
                    $("#ExpireDate").css("border-color", "red");
                    $("#ExpireDate").focus();
                    $("#GlobalError").text("This Year Not Created..!");
                }
                else if (result == "NewYearClosed") {
                    $("#ExpireDate").css("border-color", "red");
                    $("#ExpireDate").focus();
                    $("#GlobalError").text("This Year Closed..!");
                }
                else if (result == "False") {
                    $("#ExpireDate").css("border-color", "red");
                    $("#ExpireDate").focus();
                    $("#GlobalError").text("This Date Out of Range..!");
                }
                else if (result == "True") {
                    $("#ExpireDate").css("border-color", "");
                    $("#GlobalError").text("");
                }
            }
        });
    }
});


$("#Rate").keyup(function (event) {

    event.preventDefault();
    if (event.keyCode === 13) {
        $("#addToList").click();
    }

    var Rate = $(this).val();

    if (Rate.length === 0) {
        $("#Rate").css("border-color", "red");
    } else {
        $("#Rate").css("border-color", "");
    }
}).focusout(function () {
    var Rate = $(this).val();

    if (Rate.length === 0) {
        $("#Rate").css("border-color", "red");
    } else {
        $("#Rate").css("border-color", "");
    }
});

function GetRates() {

    var CurrencyID = $("#CurrencyID").val();

    var RatesData = $("#SetExchangeCurrencyInfo");

    RatesData.html("");

    if (CurrencyID.length > 0) {

        $.ajax({

            type: "GET",
            url: "/ExchangeCurrenciesSetup/GetRatesData?CurrencyID=" + CurrencyID,
            success: function (data) {

                if (data.length === 0) {
                    RatesData.append('<tr style="color:red"><td colspan = "5" > This Currency not has Rates </td></tr>');
                }
                else {
                    for (var i = 0; i < data.length; i++) {
                        var Data = "<tr class='row_" + data[i].ExchangeID + "'>" +
                            "<td>" + data[i].EstablishDate + "</td>" +
                            "<td>" + data[i].Rate + "</td>" +
                            "<td>" + data[i].StartDate + "</td>" +
                            "<td>" + data[i].ExpireDate + "</td>" +
                            "<td>" + '<a href="#" onclick="DeleteRate(\'' + data[i].ExchangeID + '\')"><span class="fa fa-trash-o"></span></a>' + "</td>" +
                            "</tr>";
                        RatesData.append(Data);
                    }
                }
            }
        });
    }
}

function DeleteRate(ExchangeID) {
    $("#ExchangeIDH").text(ExchangeID);
    $("#DeleteConfirmation").modal("show");
}

function ConfirmDelete() {

    var ExchangeID = $("#ExchangeIDH").text();

    $.ajax({
        type: "POST",
        url: "/ExchangeCurrenciesSetup/DeleteRate?ExchangeID=" + ExchangeID,
        success: function (data) {
            $("#DeleteConfirmation").modal("hide");
            $(".row_" + ExchangeID).remove();
        }
    });

}

/*else if (!/^[0-9,.]*$/.test(Rate)) {
        $("#Rate").css("border-color", "red");
        $("#Rate").focus();
        Test = false;
    }
    */

$(document).ready(function ($) {

    $.ajax({
        type: "GET",
        url: "/ExchangeCurrenciesSetup/GetCurrencyFormate",
        success: function (result) {

            $("#Rate").maskMoney({ formatOnBlur: true, reverse: true, selectAllOnFocus: true, allowEmpty: false, affixesStay: true, prefix: '' + result.Prefix + '', suffix: '' + result.Suffix + '', thousands: '' + result.Thousands + '', decimal: '' + result.Decimal + '', precision: parseInt(result.DecimalNumber) });

        }
    });

    $("#addToList").click(function (e) {

        e.preventDefault();

        var CurrencyID = $("#CurrencyID").val();

        var Rate = $("#Rate").val();

        var StartDate = $("#StartDate").val();

        var ExpireDate = $("#ExpireDate").val();

        var ClearlyRate = 0;

        var Test = true;

        if (CurrencyID.length === 0) {
            $("#CurrencyID").css("border-color", "red");
            $("#CurrencyID").focus();
            Test = false;
        } else {
            $("#CurrencyID").css("border-color", "");
        }

        if (Rate.length === 0) {
            $("#Rate").css("border", "1px solid red");
            $("#Rate").focus();
            Test = false;
        } else {
            $("#Rate").css("border", "");

            ClearlyRate = $("#Rate").maskMoney('unmasked')[0];
        }

        if (StartDate.length === 0) {
            $("#StartDate").css("border-color", "red");
            $("#StartDate").focus();
            Test = false;
        } else if (!StartDate.match(dateformat)) {
            $("#StartDate").css("border-color", "red");
            $("#StartDate").focus();
            Test = false;
        } else {
            $.ajax({
                type: "GET",
                url: "/ExchangeCurrenciesSetup/CheckStartDate?StartDate=" + StartDate,
                success: function (result) {
                    if (result.Message == "NoYear") {
                        $("#StartDate").css("border-color", "red");
                        $("#StartDate").focus();
                        $("#GlobalError").text('No Fiscal Year Related With Company ID' + result.CompanyID + '');
                        Test = false;
                    }
                    else if (result == "NewYearNotExist") {
                        $("#StartDate").css("border-color", "red");
                        $("#StartDate").focus();
                        $("#GlobalError").text("This Year Not Created..!");
                        Test = false;
                    }
                    else if (result == "NewYearClosed") {
                        $("#StartDate").css("border-color", "red");
                        $("#StartDate").focus();
                        $("#GlobalError").text("This Year Closed..!");
                        Test = false;
                    }
                    else if (result == "False") {
                        $("#StartDate").css("border-color", "red");
                        $("#StartDate").focus();
                        $("#GlobalError").text("This Date Out of Range..!");
                        Test = false;
                    }
                    else if (result == "True") {
                        $("#StartDate").css("border-color", "");
                        $("#GlobalError").text("");
                    }
                }
            });
        }

        if (ExpireDate.length === 0) {
            $("#ExpireDate").css("border-color", "red");
            $("#ExpireDate").focus();
            Test = false;
        } else if (!ExpireDate.match(dateformat)) {
            $("#ExpireDate").css("border-color", "red");
            $("#ExpireDate").focus();
            Test = false;
        } else {
            $.ajax({
                type: "GET",
                url: "/ExchangeCurrenciesSetup/CheckEndDate?EndDate=" + ExpireDate,
                success: function (result) {
                    if (result.Message == "NoYear") {
                        $("#ExpireDate").css("border-color", "red");
                        $("#ExpireDate").focus();
                        $("#GlobalError").text('No Fiscal Year Related With Company ID' + result.CompanyID + '');
                    }
                    else if (result == "NewYearNotExist") {
                        $("#ExpireDate").css("border-color", "red");
                        $("#ExpireDate").focus();
                        $("#GlobalError").text("This Year Not Created..!");
                    }
                    else if (result == "NewYearClosed") {
                        $("#ExpireDate").css("border-color", "red");
                        $("#ExpireDate").focus();
                        $("#GlobalError").text("This Year Closed..!");
                    }
                    else if (result == "False") {
                        $("#ExpireDate").css("border-color", "red");
                        $("#ExpireDate").focus();
                        $("#GlobalError").text("This Date Out of Range..!");
                    }
                    else if (result == "True") {
                        $("#ExpireDate").css("border-color", "");
                        $("#GlobalError").text("");
                    }
                }
            });
        }



        if (Test === true) {

            $.ajax({
                type: "POST",
                url: "/ExchangeCurrenciesSetup/SaveRates?CurrencyID=" + CurrencyID + "&Rate=" + ClearlyRate + "&StartDate=" + StartDate + "&ExpireDate=" + ExpireDate,
                success: function (data) {

                    if (data === "False") {
                        $("#GlobalError").text("Start Date must be Less Than Expire Date..!");
                    }
                    else if (data === "True") {
                        $("#GlobalError").text("");
                        $("#Rate").val("");
                        $("#StartDate").val("");
                        $("#ExpireDate").val("");
                        GetRates();
                        $("#Rate").focus();
                    }
                }
            });
        }
    });

});




