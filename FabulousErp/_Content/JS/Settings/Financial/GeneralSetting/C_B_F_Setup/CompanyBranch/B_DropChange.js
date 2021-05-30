$("#CompanyID").change(function () {
    if ($("#CompanyID").val().length == 0) {
        $("#BranchID").prop("disabled", true);
        $("#Branchname").prop("disabled", true);
        $("#EstablishDate").prop("disabled", true);
        $("#BranchAlies").prop("disabled", true);           
    }
    else {
        $("#BranchID").prop("disabled", false);
        $("#Branchname").prop("disabled", false);
        $("#EstablishDate").prop("disabled", false);
        $("#BranchAlies").prop("disabled", false);
        console.log($("#CompanyID").val());
        $.ajax({
            type: "GET",
            url: "/CompanyBranch/GetCompanyName?CompanyID=" + $("#CompanyID").val(),
            success: function (data) {
                
                $("#CompName").show("fast");
                $("#CompName").text(data);
                
            }
        });
    }  
});