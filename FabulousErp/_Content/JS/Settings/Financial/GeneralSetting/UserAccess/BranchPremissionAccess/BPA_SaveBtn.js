function Addbtn() {

    var UserID = $("#UserID").val();

    var UserName = $("#UserName").val();

    var CompanyID = $("#CompanyID").val();

    var CompanyName = $("#CompanyName").val();

    var BranchID = $("#BranchID").val();

    var BarnchName = $("#BarnchName").val();

    $.ajax({

        type: "POST",
        url: "/BranchPremissionAccess/AddUserToBranch?UserID=" + UserID + "&UserName=" + UserName + "&CompanyID=" + CompanyID + "&CompanyName=" + CompanyName
            + "&BranchID=" + BranchID + "&BarnchName=" + BarnchName,
        success: function (data) {
            if (data == "False") {
                $("#UserAlreadyExistError").text("This User Already Exist In This Branch..");
            } else {
                $("#UserAlreadyExistError").text("");

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
                                    "<td>" + value.UserID + "</td>" +
                                    "<td>" + value.UserName + "</td>" +
                                    "<td>" + value.BranchID + "</td>"+
                                    "<td>" + value.BranchName + "</td>"
                                "</tr>";
                                SetData.append(Data);
                            });
                        }
                    },
                    error: function (req, status, error) {

                    }
                });

            }
        }

    });
}