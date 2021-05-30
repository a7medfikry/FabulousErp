$("#UserID").change(function () {

    var UserID = $(this).val();

    if (UserID.length > 0) {

        $.ajax({
            type: "GET",
            url: "/CompanyPremissionAccess/GetUserName?UserID=" + UserID,
            success: function (data) {
                $("#UserName").val(data);
            }
        });

        $("#CompanyID").prop("disabled", false);

        var SetData = $("#SetUserList");

        SetData.html("");
        $.ajax({

            type: "GET",
            url: "/CompanyPremissionAccess/GetUserCompAccess?UserID=" + UserID,
            contentType: "html",
            success: function (result) {

                if (result.length == 0) {
                    SetData.append('<tr style="color:red"><td colspan = "4" > This User have not any access To any Company  </td></tr>')
                }
                else {
                    for (var i = 0; i < result.length; i++) {
                        var Data = "<tr class='row_" + result[i].ID + "' >" +
                            "<td width='15%'>" + result[i].UserID + "</td>" +
                            "<td width='35%'>" + result[i].UserName + "</td>" +
                            "<td width='15%'>" + result[i].CompanyID + "</td>" +
                            "<td width='35%'>" + result[i].CompanyName + "</td>" 
                        "</tr>";
                        SetData.append(Data);
                    }
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
        $("#CompanyName").val("");
        $("#SaveBtn").hide();
        $("#CompanyID").val("");
    }
});


$("#CompanyID").change(function () {

    var CompanyID = $(this).val();

    if (CompanyID.length > 0) {

        $("#UserAlreadyExistError").text("");


        $.ajax({
            type: "GET",
            url: "/CompanyPremissionAccess/GetCompanyName?CompanyID=" + CompanyID,
            success: function (data) {
                $("#CompanyName").val(data);
                $("#SaveBtn").show("slow");
            }
        });
    } else {
        $("#CompanyName").val("");
        $("#SaveBtn").hide();
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