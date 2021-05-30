function UpdatePassBtn()
{
    var count = 0;

    var OldPassword = $("#OldPassword").val();
    var NewPassword = $("#NewPassword").val();
    var ConfirmPassword = $("#ConfirmPassword").val();

    //console.log(UserID);

    //console.log(OldPassword);

    if (OldPassword.length == 0) {
        $("#ErrorOldPassword").text("Old Password Is Required");
        $("#OldPassword").addClass("input-error");
        $("#OldPassword").removeClass("input-success");
        $("#OldPassword").focus();
        count++;
    }
    else {
        $("#ErrorOldPassword").text("");
        $("#OldPassword").removeClass("input-error");
        $("#OldPassword").addClass("input-success");
    }

    //validation password

    if (NewPassword.length < 8) {
        $('#length').removeClass('valid').addClass('invalid');
        $("#NewPassword").addClass("input-error");
        $("#NewPassword").removeClass("input-success");
        $('#pswd_info').show();      
        count++;
    } else {
        $('#pswd_info').hide();
        $('#length').removeClass('invalid').addClass('valid');
        $("#NewPassword").removeClass("input-error");
        $("#NewPassword").addClass("input-success");
    }

    if (!NewPassword.match(/[A-z]/)) {
        $('#letter').removeClass('valid').addClass('invalid');
        $('#pswd_info').show();
        $("#NewPassword").addClass("input-error");
        $("#NewPassword").removeClass("input-success");
        count++;
    } else {
        $('#pswd_info').hide();
        $('#letter').removeClass('invalid').addClass('valid');
        $("#NewPassword").removeClass("input-error");
        $("#NewPassword").addClass("input-success");
    }

    //validate capital letter
    if (!NewPassword.match(/[A-Z]/)) {
        $('#capital').removeClass('valid').addClass('invalid');
        $('#pswd_info').show();
        $("#NewPassword").addClass("input-error");
        $("#NewPassword").removeClass("input-success");
        count++;
    } else {
        $('#pswd_info').hide();
        $('#capital').removeClass('invalid').addClass('valid');
        $("#NewPassword").removeClass("input-error");
        $("#NewPassword").addClass("input-success");
    }

    //validate number
    if (!NewPassword.match(/\d/)) {
        $('#number').removeClass('valid').addClass('invalid');
        $('#pswd_info').show();
        $("#NewPassword").addClass("input-error");
        $("#NewPassword").removeClass("input-success");
        count++;
    } else {
        $('#pswd_info').hide();
        $('#number').removeClass('invalid').addClass('valid');
        $("#NewPassword").removeClass("input-error");
        $("#NewPassword").addClass("input-success");
    }

    if (!NewPassword.match(/\d/) || !NewPassword.match(/[A-Z]/) || !NewPassword.match(/[A-z]/) || NewPassword.length < 8)
    {
        $('#pswd_info').show();
        $("#NewPassword").addClass("input-error");
        $("#NewPassword").removeClass("input-success");
    }

    //**************************************************
    if (ConfirmPassword.length == 0) {
        $("#ErrorConfirmPassword").text("Confirm Password Is Required");
        $("#ConfirmPassword").addClass("input-error");
        $("#ConfirmPassword").removeClass("input-success");
        $("#ConfirmPassword").focus();
        count++;
    }
    else if (ConfirmPassword != NewPassword) {
        $("#ErrorConfirmPassword").text("Confirm Password Is Wrong");
        $("#ConfirmPassword").addClass("input-error");
        $("#ConfirmPassword").removeClass("input-success");
        $("#ConfirmPassword").focus();
        count++;
    } else {
        $("#ErrorConfirmPassword").text("");
        $("#ConfirmPassword").removeClass("input-error");
        $("#ConfirmPassword").addClass("input-success");
    }

    if (count == 0) {

        $.ajax({
            type: "GET",
            url: "/UserChangePassword/UChangePass?OldPassword=" + OldPassword + "&NewPassword=" + NewPassword,
            success: function (data) {
                
                if (data == "OldPassError") {
                    $("#ErrorOldPassword").text("Old Password is Wrong..");
                    $("#OldPassword").addClass("input-error");
                    $("#OldPassword").removeClass("input-success");
                    $("#OldPassword").focus();
                }else if (data == "ONIdentical") {

                    $("#ErrorIdentical").text("No Update In Password To Change It..");
                    $("#NewPassword").focus();
                    $("#NewPassword").addClass("input-error");
                    $("#NewPassword").removeClass("input-success");
                }else {

                    window.location = "/UserChangePassword/UChangePass2?OldPassword=" + OldPassword + "&NewPassword=" + NewPassword;
                    $("#ErrorOldPassword").text("");

                }
            }
        });

    }
}