$("#CompanyID").focusout(function () {
    if ($(this).val().length !== 5) {
        $(this).focus();
        $("#CIDError").text("Company ID Is Required 5 Characters");
        $(this).addClass('input-error');       
    } else {
        $(this).removeClass('input-error');
        $("#CIDError").text("");
    }
});

$("#CompanyName").focusout(function () {
    if ($(this).val() == "") {
        $(this).focus();
        $("#CNError").text("Company Name Is Required");
        $(this).addClass('input-error');

    } else {
        $(this).removeClass('input-error');
        $("#CNError").text("");
    }
});

/*
$("#CompanyAlies").focusout(function () {
    if ($(this).val() == "") {
        $(this).focus();
        $("#CAError").text("Company Alies Is Required");
        $(this).addClass('input-error');

    } else {
        $(this).removeClass('input-error');
        $("#CAError").text("");
    }
});
*/

$("#CountryName").focusout(function () {
    if ($(this).val() == "") {
        $(this).focus();
        $("#CONError").text("Country Name Is Required");
        $(this).addClass('input-error');

    } else {
        $(this).removeClass('input-error');
        $("#CONError").text("");
    }
});


$("#Language").focusout(function () {
    if ($(this).val() == "") {
        $(this).focus();
        $("#LError").text("Language Is Required");
        $(this).addClass('input-error');

    } else {
        $(this).removeClass('input-error');
        $("#LError").text("");
    }
});


$("#Currency").focusout(function () {
    if ($(this).val() == "") {
        $(this).focus();
        $("#CuError").text("Currency Is Required");
        $(this).addClass('input-error');

    } else {
        $(this).removeClass('input-error');
        $("#CuError").text("");
    }
});


$("#CompanyMainActivity").focusout(function () {
    if ($(this).val() == "") {
        $(this).focus();
        $("#ActivityError").text("Main Activity Is Required");
        $(this).addClass('input-error');

    } else {
        $(this).removeClass('input-error');
        $("#ActivityError").text("");
    }
});


$("#StreetName").focusout(function () {
    if ($(this).val() == "") {
        $(this).focus();
        $("#StreetNameError").text("Street Name Is Required");
        $(this).addClass('input-error');

    } else {
        $(this).removeClass('input-error');
        $("#StreetNameError").text("");
    }
});


$("#BuldingNo").focusout(function () {
    if ($(this).val() == "") {
        $(this).focus();
        $("#BuildingNoError").text("BuildingNo Is Required");
        $(this).addClass('input-error');

    } else {
        $(this).removeClass('input-error');
        $("#BuildingNoError").text("");
    }
});


$("#Governorate").focusout(function () {
    if ($(this).val() == "") {
        $(this).focus();
        $("#GovError").text("Governorate/State Is Required");
        $(this).addClass('input-error');

    } else {
        $(this).removeClass('input-error');
        $("#GovError").text("");
    }
});


$("#CompanyType").focusout(function () {
    if ($(this).val() == "") {
        $(this).focus();
        $("#CTError").text("Company Type Is Required");
        $(this).addClass('input-error');

    } else {
        $(this).removeClass('input-error');
        $("#CTError").text("");
    }
});


$("#CommericalRegister").focusout(function () {
    if ($(this).val() == "") {
        $(this).focus();
        $("#CRError").text("Commerical Register Is Required");
        $(this).addClass('input-error');

    } else {
        $(this).removeClass('input-error');
        $("#CRError").text("");
    }
});


$("#TaxFileNo").focusout(function () {
    if ($(this).val() == "") {
        $(this).focus();
        $("#TFError").text("Tax File No Is Required");
        $(this).addClass('input-error');

    } else {
        $(this).removeClass('input-error');
        $("#TFError").text("");
    }
});

