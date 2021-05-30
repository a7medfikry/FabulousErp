$("#CompanyID").change(function () {

    var CompanyID = $(this).val()

    if (CompanyID.length == 0) {
        $("#BranchID").prop("disabled", true);
        $("#FactoryID").prop("disabled", true);
        $("#FactoryName").prop("disabled", true);
        $("#EstablishDate").prop("disabled", true);
        $("#FactoryAlies").prop("disabled", true);
        $("#CompName").text("");
        $("#BranchName").text("");
        $("#BranchID").empty();
    }
    else {
        $("#BranchID").prop("disabled", false);
        $("#FactoryID").prop("disabled", false);
        $("#FactoryName").prop("disabled", false);
        $("#EstablishDate").prop("disabled", false);
        $("#FactoryAlies").prop("disabled", false);
        

            $.ajax({
                type: "GET",
                url: "/CompanyFactory/GetCompanyName?CompanyID=" + CompanyID,
                success: function (data) {

                    $("#CompName").show("fast");
                    $("#CompName").text(data);

                }
            });


        $.ajax({
            type: "GET",
            url: "/CompanyFactory/FilterBranchID?CompanyID=" + CompanyID,
            success: function (data) {

                $("#BranchID").empty();

                if (data.length == 0) {

                    $("#BranchName").text("");

                    $("#BranchID").append($('<option/>', {
                        value: -1,
                        text: "No Branch Created In this Company!"

                    })
                    );

                }
                else {

                    $("#BranchID").append($('<option/>', {
                        value: -1,
                        text: "---Choose---"

                    })
                    );

                    $.each(data, function (index, row) {

                        $("#BranchID").append("<option value='" + row.BranchID + "'>" + row.BranchID + "</option>");
                        /*
                        $.ajax({
                            type: "GET",
                            url: "/CompanyFactory/GetBranchName?BranchID=" + $("#BranchID").val(),
                            success: function (data) {

                                $("#BranchName").show("fast");
                                $("#BranchName").text(data);

                            }
                        });
                        */

                    });
                }
            },
            error: function (req, status, error) {

            }
        });


    }

    $("#BranchID").change(function () {
        $.ajax({
            type: "GET",
            url: "/CompanyFactory/GetBranchName?BranchID=" + $("#BranchID").val(),
            success: function (data) {

                $("#BranchName").show("fast");
                $("#BranchName").text(data);

            }
        });

    });

});