$("#CompanyID").change(function () {
    var CompanyID = $("#CompanyID").val();
    $("#CompanyName").val("");
    $("#FiscalYearID").val("");
    $("#FiscalYearName").val("");
    $("#CIDError").text("");
    $("#FiscalYearID").prop("disabled", false);

    if (CompanyID.length > 0) {

        $.ajax({
            type: "GET",
            url: "/CompanyFiscalYear/GetCompanyID?CompanyID=" + CompanyID,
            success: function (result) {
                if (result.length > 0) {
                    $("#CompanyName").val(result);
                } else {
                    $("#FiscalYearID").prop("disabled", true);
                    $("#CompanyName").val(result.CompanyName);
                    $("#FiscalYearID").val(result.FiscalYearID);
                    $("#FiscalYearName").val(result.FiscalYearName);
                }
            }
        });
    } else {
        $("#CompanyID").css("border-color", "red");
        $("#CIDError").text("This Field is Required..!");
    }
});

$("#FiscalYearID").change(function () {
    var FiscalyearID = $("#FiscalYearID").val();

    if (FiscalyearID.length > 0) {

        $("#FiscalYearName").css("border-color", "");
        $("#FIDError").text("");

        $.ajax({
            type: "GET",
            url: "/CompanyFiscalYear/GetFiscalYearID?FiscalyearID=" + FiscalyearID,
            success: function (Result) {
                $("#FiscalYearName").val(Result);
            }
        });

    } else {
        $("#FiscalYearName").css("border-color", "red");
        $("#FiscalYearName").val("");
        $("#FIDError").text("This Field is Required..!");
    }
});


$("#AssignFYToCompBtn").click(function () {

    var CompanyID = $("#CompanyID").val();

    var FiscalYearID = $("#FiscalYearID").val();

    var Test = true;

    if (CompanyID.length === 0) {
        $("#CompanyID").css("border-color", "red");
        $("#CIDError").text("#This Fiels is Required..!");
        Test = false;
    } else {
        $("#CompanyID").css("border-color", "");
        $("#CIDError").text("");
    }

    if (FiscalYearID.length === 0) {
        $("#FiscalYearID").css("border-color", "red");
        $("#FIDError").text("#This Fiels is Required..!");
        Test = false;
    } else {
        $("#FiscalYearID").css("border-color", "");
        $("#FIDError").text("");
    }

    if (Test === true) {

        $.ajax({
            type: "POST",
            url: "/CompanyFiscalYear/AssignCompanyFiscalYear?CompanyID=" + CompanyID + "&FiscalYearID=" + FiscalYearID,
            success: function (result) {

                if (result == "False") {
                    $("#CIDError").text("Company ID Is Already Assigned..!");
                    $("#SuccessAssign").text("");
                    $("#CompanyID").css("border-color", "red");
                } else {
                    $("#CIDError").text("");
                    $("#CompanyID").css("border-color", "");
                    $("#SuccessAssign").text("Successful Assign..");
                }

            }
        });

    }

});

$("#Reset").click(function () {
    location.reload();
});




