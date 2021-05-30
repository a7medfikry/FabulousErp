
function BranchID(e)
{
    if ($(this).val().length !== 7) {
        $("#BIDError").text("Branch ID Is Required 7 Characters");    
        $(this).addClass('input-error');     
    } else {
        $(this).removeClass('input-error');
        $("#BIDError").text("");
    }
}


function BranchName(e) {
    if ($(this).val() === "") {
        $("#BNError").text("Branch Name Is Required");
        $(this).removeClass('input-sucess');
        $(this).addClass('input-error');

    } else {
        $(this).removeClass('input-error');
        $("#BNError").text("");
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
document.getElementById("BranchID").addEventListener("keyup", BranchID);
document.getElementById("Branchname").addEventListener("keyup", BranchName);
document.getElementById("CompanyID").addEventListener("change", CompanyID);
document.getElementById("StreetName").addEventListener("keyup", StreetName);
document.getElementById("BuldingNo").addEventListener("keyup", BuildingNo);
document.getElementById("Governorate").addEventListener("keyup", Governorate);



$('form .btn-next').on('click', function () {
    var parent_fieldset = $(this).parents('fieldset');
    var next_step = true;
    var current_active_step = $(this).parents('form').find('.form-wizard.active');
    var progress_line = $(this).parents('form').find('.progress-line');

    parent_fieldset.find('#BranchID').each(function () {
        if ($(this).val().length !== 7) {
            $(this).addClass('input-error');
            $("#BIDError").text("Branch ID Is Required");
            next_step = false;
        }
        else {
            $(this).removeClass('input-error');
        }
    });

    parent_fieldset.find('#Branchname').each(function () {
        if ($(this).val() === "") {
            $(this).addClass('input-error');
            $("#BNError").text("Branch Name Is Required");
            next_step = false;
        }
        else {           
            $(this).removeClass('input-error');           
        }
    });

    parent_fieldset.find('#CompanyID').each(function () {
        if ($(this).val() === "") {
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
        if ($(this).val() === "") {
            $(this).addClass('input-error');
            $("#StreetError").text("Street Name Is Required");
            next_step = false;
        }
        else {
            $(this).removeClass('input-error');
        }
    });

    parent_fieldset.find('#BuldingNo').each(function () {
        if ($(this).val() === "") {
            $(this).addClass('input-error');
            $("#BuildingError").text("Building No Is Required");
            next_step = false;
        }
        else {
            $(this).removeClass('input-error');
        }
    });

    parent_fieldset.find('#Governorate').each(function () {
        if ($(this).val() === "") {
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


function BranchMain() {
    if ($("#BarValid").text() === "True") {
        if ($("#StreetName").val() === "" || $("#BuldingNo").val() === "" || $("#Governorate").val() === "") {
        }
        else {
            $("#BranchMain").show();        
            $("#BranchAddress").hide();
            $("#BranchLegal").hide();
            $("#BranchCommunication").hide();
        }
    }
}

function BranchAddress() {
    if ($("#BarValid").text() === "True") {
        if ($("#BranchID").val() === "" || $("#Branchname").val() ==="" || $("#CompanyID").val() === "" || $("#StreetName").val() === "" || $("#BuldingNo").val() === "" || $("#Governorate").val() === "") {
        }
        else {
            $("#BranchMain").hide();
            $("#BranchAddress").show();
            $("#BranchLegal").hide();
            $("#BranchCommunication").hide();
        }
    }
}

function BranchLegal() {
    if ($("#BarValid").text() === "True") {
        if ($("#BranchID").val() ==="" || $("#Branchname").val() === "" || $("#CompanyID").val() === "" || $("#StreetName").val() === "" || $("#BuldingNo").val() === "" || $("#Governorate").val() === "") {
        }
        else {
            $("#BranchMain").hide();
            $("#BranchAddress").hide();
            $("#BranchLegal").show();
            $("#BranchCommunication").hide();
        }
    }
}

function BranchCommunication() {
    if ($("#BarValid").text() === "True") {
        if ($("#BranchID").val() === "" || $("#Branchname").val() === "" || $("#CompanyID").val() === "" || $("#StreetName").val() === "" || $("#BuldingNo").val() === "" || $("#Governorate").val() === "") {
        }
        else {
            $("#BranchMain").hide();
            $("#BranchAddress").hide();
            $("#BranchLegal").hide();
            $("#BranchCommunication").show();
        }
    }
}

