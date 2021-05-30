function Addbtn() {

    var UserID = $("#UserID").val();

    var UserName = $("#UserName").val();

    var CompanyID = $("#CompanyID").val();

    var CompanyName = $("#CompanyName").val();

    $.ajax({

        type: "POST",
        url: "/CompanyPremissionAccess/AddUserToComp?UserID=" + UserID + "&UserName=" + UserName + "&CompanyID=" + CompanyID + "&CompanyName=" + CompanyName,
        success: function (data) {
            if (data == "False") {
                $("#UserAlreadyExistError").text("This User Already Exist In This Company..");
            } else {
                $("#UserAlreadyExistError").text("");

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
                            $.each(result, function (index, value) {
                                var Data = "<tr class='row_" + value.ID + "' >" +
                                    "<td>" + value.UserID + "</td>" +
                                    "<td>" + value.UserName + "</td>" +
                                    "<td>" + value.CompanyID + "</td>" +
                                    "<td>" + value.CompanyName + "</td>"
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