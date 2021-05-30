$("#UserID").change(function () {

    var UserID = $(this).val();
    $("#UserAlreadyExistError").text("");
    if (UserID.length > 0) {

        $.ajax({
            type: "GET",
            url: "/BranchPremissionAccess/GetUserName?UserID=" + UserID,
            success: function (data) {
                $("#UserName").val(data);
            }
        });

        $("#CompanyID").prop("disabled", false);

        var SetData = $("#SetUserList");

        SetData.html("");
        $.ajax({

            type: "GET",
            url: "/BranchPremissionAccess/GetUserBranchAccess?UserID=" + UserID,
            contentType: "html",
            success: function (result) {

                if (result.length == 0) {
                    SetData.append('<tr style="color:red"><td colspan = "4" > This User have not any access To any Branch  </td></tr>')
                }
                else {
                    $.each(result, function (index, value) {
                        var Data = "<tr class='row_" + value.ID + "' >" +
                            "<td width='15%'>" + value.UserID + "</td>" +
                            "<td width='35%'>" + value.UserName + "</td>" +
                            "<td width='15%'>" + value.BranchID + "</td>" +
                            "<td width='35%'>" + value.BranchName + "</td>"
                        "</tr>";
                        SetData.append(Data);
                    });
                }
            },
            error: function (req, status, error) {

            }
        });


    } else {
        $("#CompanyID").prop("disabled", true);
        var SetData = $("#SetUserList");
        SetData.html("");
        $("#UserName").val("");
        $("#UserAlreadyExistError").text("");
        $("#BranchID").empty();
        $("#CompanyID").val("");
        $("#CompanyName").val("");
        $("#BarnchName").val("");
        $("#SaveBtn").hide();
    }
});


$("#CompanyID").change(function () {

    var CompanyID = $(this).val();

    if (CompanyID.length > 0) {
        $.ajax({
            type: "GET",
            url: "/BranchPremissionAccess/GetCompanyName?CompanyID=" + CompanyID,
            success: function (data) {

                $("#CompanyName").val(data);

                $.ajax({
                    type: "GET",
                    url: "/BranchPremissionAccess/FilterBranchID?CompanyID=" + CompanyID,
                    success: function (data) {

                        $("#BranchID").empty();

                        if (data.length == 0) {

                            $("#SaveBtn").hide();

                            $("#BranchName").text("");

                            $("#BranchID").append($('<option/>', {
                                value: -1,
                                text: "No Branch Created In this Company!"

                            })
                            );

                        }
                        else {

                            $("#BarnchName").val("");
                            $("#SaveBtn").hide();
                            $("#UserAlreadyExistError").text("");

                            $("#BranchID").append($('<option/>', {
                                value: -1,
                                text: "---Choose---"

                            })
                            );

                            $.each(data, function (index, row) {

                                $("#BranchID").append("<option value='" + row.BranchID + "'>" + row.BranchID + "</option>");

                            });
                        }
                    },
                    error: function (req, status, error) {

                    }
                });

            }
        });
    } else {

        $("#BranchID").empty();
        $("#CompanyName").val("");
        $("#BarnchName").val("");
        $("#SaveBtn").hide();
    }
});

$("#BranchID").change(function () {

    var BranchID = $(this).val();

    if (BranchID == "-1") {
        $("#BarnchName").val("");
        $("#SaveBtn").hide();
    } else {

        $.ajax({
            type: "GET",
            url: "/BranchPremissionAccess/GetBranchName?BranchID=" + BranchID,
            success: function (data) {
                $("#BarnchName").val(data);
                $("#SaveBtn").show("slow");
            }
        });

    }

});

$(document).ready(function () {

    var UserID = $("#UserID").val();

    if (UserID.length == 0) {
        $("#CompanyID").prop("disabled", true);
    } else {
        $("#CompanyID").prop("disabled", false);
    }

});