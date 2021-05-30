$("#FactoryID").focusout(function () {
    if ($(this).val().length !== 9) {
        $(this).focus();
        $("#FIDError").text("Factory ID Is Required 9 Characters");
        $(this).addClass('input-error');
    } else {
        $(this).removeClass('input-error');
        $("#FIDError").text("");
    }
});


$("#FactoryName").focusout(function () {
    if ($(this).val() === "") {
        $(this).focus();
        $("#FNError").text("Factory Name Is Required");
        $(this).addClass('input-error');
    } else {
        $(this).removeClass('input-error');
        $("#FNError").text("");
    }
});

/*
$("#BranchID").focusout(function () {
    if ($(this).val() === "") {
        $(this).focus();
        $("#BIDError").text("Branch ID Is Required");
        $(this).removeClass('input-sucess');
        $(this).addClass('input-error');

    } else {
        $(this).removeClass('input-error');
        $("#BIDError").text("");
    }
});
*/

$("#CompanyID").focusout(function () {
    if ($(this).val() === "") {
        $(this).focus();
        $("#CIDError").text("Company ID Is Required");
        $(this).removeClass('input-sucess');
        $(this).addClass('input-error');

    } else {
        $(this).removeClass('input-error');
        $("#CIDError").text("");
    }
});


$("#StreetName").focusout(function () {
    if ($(this).val() === "") {
        $(this).focus();
        $("#StreetError").text("Street Name Is Required");
        $(this).removeClass('input-sucess');
        $(this).addClass('input-error');

    } else {
        $(this).removeClass('input-error');
        $("#StreetError").text("");
    }
});


$("#BuldingNo").focusout(function () {
    if ($(this).val() === "") {
        $(this).focus();
        $("#BuildingError").text("Building No Is Required");
        $(this).removeClass('input-sucess');
        $(this).addClass('input-error');

    } else {
        $(this).removeClass('input-error');
        $("#BuildingError").text("");
    }
});


$("#Governorate").focusout(function () {
    if ($(this).val() === "") {
        $(this).focus();
        $("#GovError").text("Governorate/State Is Required");
        $(this).addClass('input-error');

    } else {
        $(this).removeClass('input-error');
        $("#GovError").text("");
    }
});


