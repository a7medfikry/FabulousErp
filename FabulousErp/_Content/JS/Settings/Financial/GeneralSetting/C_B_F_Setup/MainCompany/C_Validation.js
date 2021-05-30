function CompanyID(e) {
    if ($(this).val().length !== 5) {
        $("#CIDError").text("Company ID Is Required 5 Characters");
        $(this).addClass('input-error');

    } else {
        $(this).removeClass('input-error');
        $("#CIDError").text("");
    }
}

function CompanyName(e) {
    if ($(this).val() == "") {
        $("#CNError").text("Company Name Is Required");
        $(this).addClass('input-error');

    } else {
        $(this).removeClass('input-error');
        $("#CNError").text("");
    }
}

function CountryName(e) {
    if ($(this).val() == "") {
        $("#CONError").text("Country Name Is Required");
        $(this).addClass('input-error');

    } else {
        $(this).removeClass('input-error');
        $("#CONError").text("");
    }
}

function Language(e) {
    if ($(this).val() == "") {
        $("#LError").text("Language Is Required");
        $(this).addClass('input-error');

    } else {
        $(this).removeClass('input-error');
        $("#LError").text("");
    }
}

function Currency(e) {
    if ($(this).val() == "") {
        $("#CuError").text("Currency Is Required");
        $(this).addClass('input-error');

    } else {
        $(this).removeClass('input-error');
        $("#CuError").text("");
    }
}

function Activity(e) {
    if ($(this).val() == "") {
        $("#ActivityError").text("Main Activity Is Required");
        $(this).addClass('input-error');

    } else {
        $(this).removeClass('input-error');
        $("#ActivityError").text("");
    }
}

function StreetName(e) {
    if ($(this).val() == "") {
        $("#StreetNameError").text("Street Name Is Required");
        $(this).addClass('input-error');

    } else {
        $(this).removeClass('input-error');
        $("#StreetNameError").text("");
    }
}

function BuildingNo(e) {
    if ($(this).val() == "") {
        $("#BuildingNoError").text("BuildingNo Is Required");
        $(this).addClass('input-error');

    } else {
        $(this).removeClass('input-error');
        $("#BuildingNoError").text("");
    }
}

function Governorate(e) {
    if ($(this).val() == "") {
        $("#GovError").text("Governorate/State Is Required");
        $(this).addClass('input-error');

    } else {
        $(this).removeClass('input-error');
        $("#GovError").text("");
    }
}

function CompanyType(e) {
    if ($(this).val() == "") {
        $("#CTError").text("Company Type Is Required");
        $(this).addClass('input-error');

    } else {
        $(this).removeClass('input-error');
        $("#CTError").text("");
    }
}

function CommericalRegister(e) {
    if ($(this).val() == "") {
        $("#CRError").text("Commerical Register Is Required");
        $(this).addClass('input-error');

    } else {
        $(this).removeClass('input-error');
        $("#CRError").text("");
    }
}

function TaxFile(e) {
    if ($(this).val() == "") {
        $("#TFError").text("Tax File No Is Required");
        $(this).addClass('input-error');

    } else {
        $(this).removeClass('input-error');
        $("#TFError").text("");
    }
}

document.getElementById("CompanyID").addEventListener("keyup", CompanyID);
document.getElementById("CompanyName").addEventListener("keyup", CompanyName);
document.getElementById("CountryName").addEventListener("change", CountryName);
document.getElementById("Language").addEventListener("change", Language);
document.getElementById("Currency").addEventListener("change", Currency);
document.getElementById("CompanyMainActivity").addEventListener("keyup", Activity);
document.getElementById("StreetName").addEventListener("keyup", StreetName);
document.getElementById("BuldingNo").addEventListener("keyup", BuildingNo);
document.getElementById("Governorate").addEventListener("keyup", Governorate);
document.getElementById("CompanyType").addEventListener("change", CompanyType);
document.getElementById("CommericalRegister").addEventListener("keyup", CommericalRegister);
document.getElementById("TaxFileNo").addEventListener("keyup", TaxFile);

/************************************************************************************************/

$('#Website').prop('title', 'Website must looks like www.fabulousERP.com');
