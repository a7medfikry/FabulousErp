function FactoryID(e) {
    if ($(this).val().length !== 9) {
        $("#FIDError").text("Factory ID Is Required 9 Characters");       
        $(this).addClass('input-error');
    } else {
        $(this).removeClass('input-error');
        $("#FIDError").text("");
    }
}

function FactoryName(e) {
    if ($(this).val() === "") {
        $("#FNError").text("Factory Name Is Required");       
        $(this).addClass('input-error');

    } else {
        $(this).removeClass('input-error');
        $("#FNError").text("");
    }
}

function CompanyID(e) {
    if ($(this).val() === "") {
        $("#CIDError").text("Company ID Is Required");
        $(this).removeClass('input-sucess');
        $(this).addClass('input-error');

    } else {
        $(this).removeClass('input-error');
        $("#CIDError").text("");
    }
}

function StreetName(e) {
    if ($(this).val() === "") {
        $("#StreetError").text("Street Name Is Required");
        $(this).removeClass('input-sucess');
        $(this).addClass('input-error');

    } else {
        $(this).removeClass('input-error');
        $("#StreetError").text("");
    }
}

function BuildingNo(e) {
    if ($(this).val() === "") {
        $("#BuildingError").text("Building No Is Required");
        $(this).removeClass('input-sucess');
        $(this).addClass('input-error');

    } else {
        $(this).removeClass('input-error');
        $("#BuildingError").text("");
    }
}

function Governorate(e) {
    if ($(this).val() === "") {
        $("#GovError").text("Governorate/State Is Required");
        $(this).addClass('input-error');

    } else {
        $(this).removeClass('input-error');
        $("#GovError").text("");
    }
}

document.getElementById("FactoryID").addEventListener("keyup", FactoryID);
document.getElementById("FactoryName").addEventListener("keyup", FactoryName);
document.getElementById("CompanyID").addEventListener("change", CompanyID);
document.getElementById("StreetName").addEventListener("keyup", StreetName);
document.getElementById("BuldingNo").addEventListener("keyup", BuildingNo);
document.getElementById("Governorate").addEventListener("keyup", Governorate);



$('form .btn-next').on('click', function () {
    var parent_fieldset = $(this).parents('fieldset');
    var next_step = true;
    var current_active_step = $(this).parents('form').find('.form-wizard.active');
    var progress_line = $(this).parents('form').find('.progress-line');

    parent_fieldset.find('#FactoryID').each(function () {
        if ($(this).val().length !== 9) {
            $(this).addClass('input-error');
            $("#FIDError").text("Factory ID Is Required");
            next_step = false;
        }
        else {
            $(this).removeClass('input-error');
        }
    });

    parent_fieldset.find('#FactoryName').each(function () {
        if ($(this).val() === "") {
            $(this).addClass('input-error');
            $("#FNError").text("Factory Name Is Required");
            next_step = false;
        }
        else {
            $(this).removeClass('input-error');
        }
    });
  
    parent_fieldset.find('#CompanyID').each(function () {
        if ($(this).val() == "") {
            $(this).addClass('input-error');
            $("#CIDError").text("Company ID Is Required");
            next_step = false;
        }
        else {
            $(this).removeClass('input-error');
            $(this).addClass('input-sucess');
        }
    });

    parent_fieldset.find('#StreetName').each(function () {
        if ($(this).val() == "") {
            $(this).addClass('input-error');
            $("#StreetError").text("Street Name Is Required");
            next_step = false;
        }
        else {
            $(this).removeClass('input-error');
        }
    });

    parent_fieldset.find('#BuldingNo').each(function () {
        if ($(this).val() == "") {
            $(this).addClass('input-error');
            $("#BuildingError").text("Building No Is Required");
            next_step = false;
        }
        else {
            $(this).removeClass('input-error');
        }
    });

    parent_fieldset.find('#Governorate').each(function () {
        if ($(this).val() == "") {
            $(this).addClass('input-error');
            $("#GovError").text("Governorate/State Is Required");
            next_step = false;
        }
        else {
            $(this).removeClass('input-error');
        }
    });

    if (next_step) {
        parent_fieldset.fadeOut(400, function () {
            current_active_step.removeClass('active').addClass('activated').next().addClass('active');
            bar_progress(progress_line, 'right');
            $(this).next().fadeIn();
            scroll_to_class($('form'), 20);
        });
    }

});

function FactoryMain() {
    if ($("#BarValid").text() === "True") {
        if ($("#StreetName").val() === "" || $("#BuldingNo").val() === "" || $("#Governorate").val() === "") {
        }
        else {
            $("#FactoryMain").show();
            $("#FactoryAddress").hide();
            $("#FactoryLegal").hide();
            $("#FactoryCommunication").hide();
        }
    }
}

function FactoryAddress() {
    if ($("#BarValid").text() === "True") {
        if ($("#BranchID").val() === "" || $("#Branchname").val() === "" || $("#CompanyID").val() === "" || $("#StreetName").val() === "" || $("#BuldingNo").val() === "" || $("#Governorate").val() === "") {
        }
        else {
            $("#FactoryMain").hide();
            $("#FactoryAddress").show();
            $("#FactoryLegal").hide();
            $("#FactoryCommunication").hide();
        }
    }
}

function FactoryLegal() {
    if ($("#BarValid").text() === "True") {
        if ($("#BranchID").val() === "" || $("#Branchname").val() === "" || $("#CompanyID").val() === "" || $("#StreetName").val() === "" || $("#BuldingNo").val() === "" || $("#Governorate").val() === "") {
        }
        else {
            $("#FactoryMain").hide();
            $("#FactoryAddress").hide();
            $("#FactoryLegal").show();
            $("#FactoryCommunication").hide();
        }
    }
}

function FactoryCommunication() {
    if ($("#BarValid").text() === "True") {
        if ($("#BranchID").val() === "" || $("#Branchname").val() === "" || $("#CompanyID").val() === "" || $("#StreetName").val() === "" || $("#BuldingNo").val() === "" || $("#Governorate").val() === "") {
        }
        else {
            $("#FactoryMain").hide();
            $("#FactoryAddress").hide();
            $("#FactoryLegal").hide();
            $("#FactoryCommunication").show();
        }
    }
}