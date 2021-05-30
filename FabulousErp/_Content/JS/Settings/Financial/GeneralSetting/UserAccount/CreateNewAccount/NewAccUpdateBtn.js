function UpdateAcc()
{
    var UserID = $("#UserID").val();

    var UpdateSuccess = $("#UpdateSuccess").text();

    if (UpdateSuccess.length > 0) {

        var PasswordExpired = "False";
        if ($("#PasswordExpired").is(":checked")) {
            PasswordExpired = "True";
        }

        var DisActive = "False";
        if ($("#DisActive").is(":checked")) {
            DisActive = "True";
        }

        $.ajax({
            type: "POST",
            url: "/CreateNewAccount/UpdateAccount?PasswordExpired=" + PasswordExpired + "&DisActive=" + DisActive + "&UserID=" + UserID,
            success: function (data) {

                $("#UpdateSuccess").show("fast");
                $("#UpdateSuccess").text("Account Updated Successfully");
                $("#UpdateSuccess").fadeOut(4000);
            }
        });

    }
    else {

        $("#UpdateSuccess").show("fast");
        $("#UpdateSuccess").text("You Can't Update Without Search..");

    }

   
}

function DeletAcc() {

    var UserID = $("#UserID").val();

    if (confirm("Do You want to Delete This User..") == true) {

        $.ajax({
            type: "POST",
            url: "/CreateNewAccount/DeleteAccount?DeleteAccount=" + "True" + "&UserID=" + UserID,
            success: function (data) {
                $("#DeleteAccount").hide();
                $("#DeletedError").show();
                $("#DeletedError").text("This Account Is Deleted..");

                $("#UserID").val("");
                $("#UserName").val("");
                $("#Password").val("");
                $("#ConfirmPassword").val("");

                $("#UserID").removeClass("input-success");
                $("#UserName").removeClass("input-success");
                $("#Password").removeClass("input-success");
                $("#ConfirmPassword").removeClass("input-success");
                $("#UserID").removeClass("input-error");
                $("#UserName").removeClass("input-error");
                $("#Password").removeClass("input-error");
                $("#ConfirmPassword").removeClass("input-error");

                $("#ChangePassFirst").prop("checked", false);
                $("#UpdateProfFirst").prop("checked", false);
                $("#PasswordExpired").prop("checked", false);
                $("#DisActive").prop("checked", false);

                $("#UserID").prop("disabled", false);
                $("#UserName").prop("disabled", false);
                $("#Password").prop("disabled", false);
                $("#ConfirmPassword").prop("disabled", false);
                $("#ChangePassFirst").prop("disabled", false);
                $("#UpdateProfFirst").prop("disabled", false);
                $("#SaveBtn").prop("disabled", false);


                $("#ErrorUserID").text("");
                $("#ErrorUserName").text("");
                $("#ErrorPassword").text("");
                $("#ErrorConfirmPass").text("");
                $("#ErrorConfirmPass").text("");

                $("#GroupName").text("");

                $("#DeleteAccount").hide();

                $("#DeletedError").fadeOut(4000);
            }
        });

    } else {
        //  x = "You pressed Cancel!";
    }
}