
$("#CompanyID").change(function () {

    var CompanyID = $(this).val();

    $("#CompanyName").val("");

    if (CompanyID.length === 0) {
        $(this).css("border-color", "red");
        $("#Currency").val("");
        $("#DecimalNotation").val("");
        $("#DecimalNumbers").val("");
        $("#FormateSaveBtn").hide();
        $("#FormateUpdateBtn").hide();

    } else {
        $(this).css("border-color", "");

        $.ajax({
            type: "GET",
            url: "/CurrencyFormateSetting/GetFormatData?CompanyID=" + CompanyID,
            success: function (data) {

                if (data == "False") {
                    $("#FormateSaveBtn").show();
                    $("#FormateUpdateBtn").hide();
                } else {
                    $("#FormateSaveBtn").hide();
                    $("#FormateUpdateBtn").show();
                    $("#Currency").val(data.Currency);
                    $("#DecimalNotation").val(data.DecimalNation);
                    $("#DecimalNumbers").val(data.DecimalNumber);
                    GetCompORGCurrency(CompanyID);
                }
            }
        });

        $.ajax({
            type: "GET",
            url: "/CurrencyFormateSetting/GetCompanyName?companyID=" + CompanyID,
            success: function (result) {
                $("#CompanyName").val(result);
            }
        });
    }
});

function GetCompORGCurrency(CompanyID) {

    $.ajax({
        type: "GET",
        url: "/CurrencyFormateSetting/GetCompORGCurrency?CompanyID=" + CompanyID,
        success: function (result) {
            $("#CompORGCurrency").text(result);
        }
    });

}

$("#Currency").change(function () {

    var Currency = $(this).val();

    if (Currency.length == 0) {
        $("#CurrencyError").text("This Field is Required");
    }
    else {
        $("#CurrencyError").text("");
    }
});

$("#DecimalNotation").change(function () {

    var DecimalNotation = $(this).val();

    if (DecimalNotation.length == 0) {
        $("#DecimalNotationError").text("This Field is Required");
    }
    else {
        $("#DecimalNotationError").text("");
    }
});

$("#DecimalNumbers").change(function () {

    var DecimalNumbers = $(this).val();

    if (DecimalNumbers.length == 0) {
        $("#DecimalNumbersError").text("This Field is Required");
    }
    else {
        $("#DecimalNumbersError").text("");
    }
});


function SaveFormateBtn() {

    var Currency = $("#Currency").val();

    var DecimalNotation = $("#DecimalNotation").val();

    var DecimalNumbers = $("#DecimalNumbers").val();

    var CompanyID = $("#CompanyID").val();


    var Prefix = "";
    var Suffix = "";

    var Thousands = "";
    var Decimal = "";

    var count = 0;

    if (Currency.length == 0) {
        $("#CurrencyError").text("This Field is Required");
        count++;
    } else {
        $("#CurrencyError").text("");

        if (Currency == "1986.09 EGP") {
            Suffix = " " + $("#CompORGCurrency").text(); + "";
            Prefix = "";
        } else if (Currency == "EGP 1986.09") {
            Prefix = "" + $("#CompORGCurrency").text();+" ";
            Suffix = "";
        } else {
            Prefix = "";
            Suffix = "";
        }

    }

    if (DecimalNotation.length == 0) {
        $("#DecimalNotationError").text("This Field is Required");
        count++;
    } else {
        $("#DecimalNotationError").text("");

        if (DecimalNotation == "1,234,567.89") {
            Thousands = ",";
            Decimal = ".";

        } else if (DecimalNotation == "1 234 567.89") {
            Thousands = " ";
            Decimal = ".";

        }

    }


    if (DecimalNumbers.length == 0) {
        $("#DecimalNumbersError").text("This Field is Required");
        count++;
    } else {
        $("#DecimalNumbersError").text("");
    }


    if (count === 0) {

        $.ajax({
            type: "POST",
            url: "/CurrencyFormateSetting/SaveFormateSetting?Suffix=" + Suffix + "&Prefix=" + Prefix + "&Thousands=" + Thousands + "&Decimal=" + Decimal + "&DecimalNumbers=" + DecimalNumbers + "&Currency=" + Currency + "&DecimalNotation=" + DecimalNotation + "&CompanyID=" + CompanyID,
            success: function (data) {

                if (data === "True") {
                    location.reload();
                }
            }
        });
    }
}



function UpdateFormateBtn() {

    var Currency = $("#Currency").val();

    var DecimalNotation = $("#DecimalNotation").val();

    var DecimalNumbers = $("#DecimalNumbers").val();

    var CompanyID = $("#CompanyID").val();

    var Prefix = "";
    var Suffix = "";

    var Thousands = "";
    var Decimal = "";

    var count = 0;

    if (Currency.length == 0) {
        $("#CurrencyError").text("This Field is Required");
        count++;
    } else {
        $("#CurrencyError").text("");

        if (Currency == "1986.09 EGP") {
            Suffix = " " + $("#CompORGCurrency").text(); + "";
            Prefix = "";
        } else if (Currency == "EGP 1986.09") {
            Prefix = "" + $("#CompORGCurrency").text(); +" ";
            Suffix = "";
        } else {
            Prefix = "";
            Suffix = "";
        }

    }

    if (DecimalNotation.length == 0) {
        $("#DecimalNotationError").text("This Field is Required");
        count++;
    } else {
        $("#DecimalNotationError").text("");

        if (DecimalNotation == "1,234,567.89") {
            Thousands = ",";
            Decimal = ".";

        } else if (DecimalNotation == "1 234 567.89") {
            Thousands = " ";
            Decimal = ".";

        }

    }


    if (DecimalNumbers.length == 0) {
        $("#DecimalNumbersError").text("This Field is Required");
        count++;
    } else {
        $("#DecimalNumbersError").text("");
    }


    if (count === 0) {

        $.ajax({
            type: "POST",
            url: "/CurrencyFormateSetting/UpdateFormateSetting?Suffix=" + Suffix + "&Prefix=" + Prefix + "&Thousands=" + Thousands + "&Decimal=" + Decimal + "&DecimalNumbers=" + DecimalNumbers + "&Currency=" + Currency + "&DecimalNotation=" + DecimalNotation + "&CompanyID=" + CompanyID,
            success: function (data) {

                if (data === "NoChnages") {
                    $("#UpdateError").show();
                    $("#SuccessSaved").hide();
                    $("#UpdateError").text("No changes to Update..");
                }
                else if (data === "True") {
                    $("#CompanyIDError").text("");
                    $("#UpdateError").hide();
                    $("#SuccessSaved").show();
                    $("#SuccessSaved").text("Formate Updated Successfully");
                }
            }
        });
    }
}