function FSaveButton() {
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
    $("#FactoryID").each(function () {
        if ($("#FactoryID").val() === "") {
            $("#FactoryID").addClass('input-error');
            $("#FIDError").text("Factory ID Is Required");
        }
    });
    $("#FactoryName").each(function () {
        if ($("#FactoryName").val() === "") {
            $("#FactoryName").addClass('input-error');
            $("#FNError").text("Factory Name Is Required");
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
    /**********************************************************************************/
    var Form = $("#CompanyForm").serialize();
    $.ajax({

        type: "POST",
        url: "/CompanyFactory/FactoryInformations",
        data: Form,
        success: function (result)
        {
            if (result === "FactoryIDExist") {
                Talert("There Exist Factory Take The Same ID");
            }
            else if (result === "Done")
            {
                $("#SuccessSubmit").text("You Can Update Now If You Need...");
                $("#PrintSpan").show("slow");
                $("#FactoryID").prop("disabled", true);
                $("#BranchID").prop("disabled", true);
                $("#CompanyID").prop("disabled", true);
                $(".fa-lock").show("slow");
                $("#SaveBtn").prop("disabled", true);
            }
        }
    });
}