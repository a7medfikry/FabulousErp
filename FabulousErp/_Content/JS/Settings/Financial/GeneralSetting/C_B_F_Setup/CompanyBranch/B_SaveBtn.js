function BSaveButton() {
    $("#CompanyID").each(function () {
        if ($("#CompanyID").val() === "") {
            $("#CompanyID").addClass('input-error');
            $("#CIDError").text("Company ID Is Required");
        }
    });
    $("#BranchID").each(function () {
        if ($("#BranchID").val() === "") {
            $("#BranchID").addClass('input-error');
            $("#BIDError").text("Branch ID Is Required");
        }
    });
    $("#Branchname").each(function () {
        if ($("#Branchname").val() === "") {
            $("#Branchname").addClass('input-error');
            $("#BNError").text("Branch Name Is Required");
        }
    });
    $("#StreetName").each(function () {
        if ($("#StreetName").val() === "") {
            $("#StreetName").addClass('input-error');
            $("#StreetError").text("Street Name Is Required");
        }
    });
    $("#BuldingNo").each(function () {
        if ($("#BuldingNo").val() === "") {
            $("#BuldingNo").addClass('input-error');
            $("#BuildingError").text("Building No. Is Required");
        }
    });
    $("#Governorate").each(function () {
        if ($("#Governorate").val() === "") {
            $("#Governorate").addClass('input-error');
            $("#GovError").text("Governorate / State Is Required");
        }
    });

    /**************************************************************************/
    var Form = $("#Form1").serialize();
    $.ajax({
        type: "POST",
        url: "/CompanyBranch/BranchInformations",
        data: Form,
        success: function (result) {

            if (result === "BranchIDExist") {
                Talert("There Exist Branch Take The Same ID");
            }
            else if (result === "Done") {
                $("#SuccessSubmit").text("You Can Update Now If You Need...");
                $("#PrintSpan").show("slow");
                $("#BranchID").prop("disabled", true);
                $("#CompanyID").prop("disabled", true);
                //$("#EstablishDate").prop("disabled", true);
                $(".glyphicon-lock").show("slow");
                $("#SaveBtn").prop("disabled", true);
            }

        }
    });
}