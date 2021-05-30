function SearchBtn() {
    var UserID = $("#UserID").val();
    $("#ErrorUserID").text("");
    if (UserID == "") {
        $("#ErrorUserID").text("");
        $("#ErrorInSearch").text("Fill User ID to Search");
    }
    else {
        $("#ErrorInSearch").text("");
        $.ajax({
            type: "GET",
            url: "/CreateNewAccount/GetAccountInfo?UserID=" + UserID,
            success: function (data) {

                $('#pswd_info').hide();

                if (data.Deleted == true) {
                    $("#DeleteAccount").hide();
                    $("#DeletedError").show();
                    $("#DeletedError").text("This Account Is Deleted..");
                    $("#DeletedError").fadeOut(4000);
                }
                else if (data == "False") {

                    $("#ErrorInSearch").text("User ID Not Exist");

                } else {
                    $("#DeletedError").hide();
                    $("#DeleteAccount").show();

                    $("#UpdateSuccess").show("fast");
                    $("#UpdateSuccess").text("You Can Update Now");

                    $("#UserID").prop("disabled", true);
                    $("#UserName").prop("disabled", true);
                    $("#Password").prop("disabled", true);
                    $("#ConfirmPassword").prop("disabled", true);
                    $("#ChangePassFirst").prop("disabled", true);
                    $("#UpdateProfFirst").prop("disabled", true);
                    $("#SaveBtn").prop("disabled", true);

                    $("#ErrorUserID").text("");
                    $("#ErrorUserName").text("");
                    $("#ErrorPassword").text("");
                    $("#ErrorConfirmPass").text("");
                    $("#ErrorConfirmPass").text("");

                    $("#UserName").val(data.UserName);
                    $("#Password").val(data.Password);
                    $("#DateOfCreation").show();
                    $("#ConfirmPassword").val(data.Password);

                    if (data.ChangePassFirst == true) {
                        $("#ChangePassFirst").prop("checked", true);
                    }
                    else {
                        $("#ChangePassFirst").prop("checked", false);
                    }

                    if (data.UpdateProfFirst == true) {
                        $("#UpdateProfFirst").prop("checked", true);
                    } else {
                        $("#UpdateProfFirst").prop("checked", false);
                    }

                    if (data.PasswordExpired == true) {
                        $("#PasswordExpired").prop("checked", true);
                    } else {
                        $("#PasswordExpired").prop("checked", false);
                    }

                    if (data.DisActive == true) {
                        $("#DisActive").prop("checked", true);
                    } else {
                        $("#DisActive").prop("checked", false);
                    }
                }
            },
            error: function (req, status, error) {
                $("#ErrorInSearch").text("User ID Not Exist");
            }
        });
    } 
}