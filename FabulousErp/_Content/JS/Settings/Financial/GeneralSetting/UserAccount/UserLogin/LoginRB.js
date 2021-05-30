$("#CompanyRB").click(function () {
    if ($("#CompanyRB").is(':checked')) {

        $("#CompanyLogin").show();
        $("#BranchLogin").hide();
        $("#FactoryLogin").hide();

        $("#LogInWithOutCBF").text("");

    }
});

$("#BranchRB").click(function () {
    if ($("#BranchRB").is(':checked')) {

        $("#BranchLogin").show();
        $("#CompanyLogin").hide();
        $("#FactoryLogin").hide();

        $("#LogInWithOutCBF").text("");

    }
});

$("#FactoryRB").click(function () {
    if ($("#FactoryRB").is(':checked')) {

        $("#FactoryLogin").show();
        $("#CompanyLogin").hide();
        $("#BranchLogin").hide();

        $("#LogInWithOutCBF").text("");

    }
});

$("#CompanyID").change(function () {

    var ID = $(this).val();

    $.ajax({
        type: "GET",
        url: "/UserLogin/GetCompanyName?CompanyID=" + ID,
        success: function (data) {
            $("#NameOfCompany").text(data);
        }
    });
   
});

$("#BranchID").change(function () {

    var ID = $(this).val();

    $.ajax({
        type: "GET",
        url: "/UserLogin/GetBranchName?BranchID=" + ID,
        success: function (data) {
            $("#NameOfBranch").text(data);
        }
    });

});

$("#FactoryID").change(function () {

    var ID = $(this).val();

    $.ajax({
        type: "GET",
        url: "/UserLogin/GetFactoryName?FactoryID=" + ID,
        success: function (data) {
            $("#NameOfFactory").text(data);
        }
    });
});
/*
$(document).ready(function () {

    $.ajax({
        type: "GET",
        url: "/UserLogin/GetCompanyID",
        success: function (data) {

            $("#CompanyID").empty();

            if (data.length == 0) {

                $("#CompanyID").append($('<option/>', {
                    value: -1,
                    text: "No Company Created!"

                })
                );

            }
            else {

                $.each(data, function (index, row) {

                    $("#CompanyID").append("<option value='" + row.CompanyID + "'>" + row.CompanyID + "</option>");

                    var CompanyID = $("#CompanyID").val();

                    $.ajax({
                        type: "GET",
                        url: "/UserLogin/GetCompanyName?CompanyID=" + CompanyID,
                        success: function (data) {
                            $("#NameOfCompany").text(data);
                        }
                    });

                });
            }
        },
        error: function (req, status, error) {

        }
    });



    $.ajax({
        type: "GET",
        url: "/UserLogin/GetBranchID",
        success: function (data) {

            $("#BranchID").empty();

            if (data.length == 0) {

                $("#BranchID").append($('<option/>', {
                    value: -1,
                    text: "No Branch Created!"

                })
                );

            }
            else {

                $.each(data, function (index, row) {

                    $("#BranchID").append("<option value='" + row.BranchID + "'>" + row.BranchID + "</option>");

                    var BranchID = $("#BranchID").val();

                    $.ajax({
                        type: "GET",
                        url: "/UserLogin/GetBranchName?BranchID=" + BranchID,
                        success: function (data) {
                            $("#NameOfBranch").text(data);
                        }
                    });

                });
            }
        },
        error: function (req, status, error) {

        }
    });



    $.ajax({
        type: "GET",
        url: "/UserLogin/GetFactoryID",
        success: function (data) {

            $("#FactoryID").empty();

            if (data.length == 0) {

                $("#FactoryID").append($('<option/>', {
                    value: -1,
                    text: "No Factory Created!"

                })
                );

            }
            else {

                $.each(data, function (index, row) {

                    $("#FactoryID").append("<option value='" + row.FactoryID + "'>" + row.FactoryID + "</option>");

                    var FactoryID = $("#FactoryID").val();

                    $.ajax({
                        type: "GET",
                        url: "/UserLogin/GetFactoryName?FactoryID=" + FactoryID,
                        success: function (data) {
                            $("#NameOfFactory").text(data);
                        }
                    });

                });
            }
        },
        error: function (req, status, error) {

        }
    });

});
*/