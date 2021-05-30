//validation User ID
$("#UserID").focusout(function () {

    var UserID = $(this).val();
    if (UserID.length == 0) {
        $("#ErrorUserID").text("User ID Is Required");
        $("#UserID").focus();
        $("#UserID").removeClass("input-success");
        $("#UserID").addClass("input-error");
    } else {

        $("#ErrorUserID").text("");
        $("#UserID").removeClass("input-error");
        $("#UserID").addClass("input-success");
        /*
        $.ajax({
            type: "GET",
            url: "/CreateNewAccount/CheckUserID?UserID=" + UserID,
            success: function (data) {
                if (data == "True") {
                    $("#ErrorUserID").text("The User ID Not Available");
                    $("#UserID").addClass("input-error");
                    $("#UserID").focus();
                }
                else {
                    $("#ErrorUserID").text("");
                    $("#UserID").removeClass("input-error");
                    $("#UserID").addClass("input-success");
                }
            }
        });
        */
        
    }
});
$("#UserID").keyup(function () {
    var UserID = $(this).val();

    if (UserID.length == 0) {
        $("#ErrorUserID").text("User ID Is Required");
        $("#UserID").removeClass("input-success");
        $("#UserID").addClass("input-error");
    } else if (UserID.length < 6) {
        $("#ErrorUserID").text("User Id Required 6 Charachters");
        $("#UserID").addClass("input-error");
        $("#UserID").removeClass("input-success");
    }  else {
        $("#ErrorUserID").text("");
        $("#UserID").removeClass("input-error");
        $("#UserID").addClass("input-success");
    }
}).keypress(function (event) {

    var ew = event.which;
    if (ew == 32)
        return true;
    if (48 <= ew && ew <= 57)
        return true;
    if (65 <= ew && ew <= 90)
        return true;
    if (97 <= ew && ew <= 122)
        return true;
    return false;

    });
//*********************************************************************************

//Validation UserName
$("#UserName").keyup(function () {

    var UserName = $(this).val();

    if (UserName.length == 0) {
        $("#ErrorUserName").text("User Name Is Required");
        $("#UserName").addClass("input-error");
        $("#UserName").removeClass("input-success");
    } else if (!UserName.match("^[A-Za-z].*")) {
        $("#ErrorUserName").text("User Name Must Start With Character");
        $("#UserName").addClass("input-error");
        $("#UserName").removeClass("input-success");
    } else if (UserName.length > 50) {
        $("#ErrorUserName").text("User Name Maximum 50 Characters");
        $("#UserName").addClass("input-error");
        $("#UserName").removeClass("input-success");
    } else {
        $("#ErrorUserName").text("");
        $("#UserName").removeClass("input-error");
        $("#UserName").addClass("input-success");
    }

});
//******************************************************************************************

//validation Password
$("#Password").keyup(function () {

    var Password = $(this).val();

    if (Password.length < 8) {
        $('#length').removeClass('valid').addClass('invalid');
        $("#Password").addClass("input-error");
        $("#Password").removeClass("input-success");
        $('#pswd_info').show();
    } else {
        $('#pswd_info').hide();
        $('#length').removeClass('invalid').addClass('valid');
        $("#Password").removeClass("input-error");
        $("#Password").addClass("input-success");
    }

    if (!Password.match(/[A-z]/)) {
        $('#letter').removeClass('valid').addClass('invalid');
        $("#Password").addClass("input-error");
        $("#Password").removeClass("input-success");
        $('#pswd_info').show();
    } else {
        $('#pswd_info').hide();
        $('#letter').removeClass('invalid').addClass('valid');
        $("#Password").removeClass("input-error");
        $("#Password").addClass("input-success");
    }

    //validate capital letter
    if (!Password.match(/[A-Z]/)) {
        $('#capital').removeClass('valid').addClass('invalid');
        $("#Password").addClass("input-error");
        $("#Password").removeClass("input-success");
        $('#pswd_info').show();
    } else {
        $('#pswd_info').hide();
        $('#capital').removeClass('invalid').addClass('valid');
        $("#Password").removeClass("input-error");
        $("#Password").addClass("input-success");
    }

    //validate number
    if (!Password.match(/\d/)) {
        $('#number').removeClass('valid').addClass('invalid');
        $("#Password").addClass("input-error");
        $("#Password").removeClass("input-success");
        $('#pswd_info').show();
    } else {
        $('#pswd_info').hide();
        $('#number').removeClass('invalid').addClass('valid');
        $("#Password").removeClass("input-error");
        $("#Password").addClass("input-success");
    }

    if (!Password.match(/\d/) || !Password.match(/[A-Z]/) || !Password.match(/[A-z]/) || Password.length < 8) {
        $('#pswd_info').show();
        $("#Password").addClass("input-error");
        $("#Password").removeClass("input-success");
    }

}).focus(function () {
    $('#pswd_info').show();
    $("#ErrorPassword").hide();
}).blur(function () {
    $('#pswd_info').hide();
    $("#ErrorPassword").show();
});
//************************************************************************************************

//validate Confirm Password
$("#ConfirmPassword").keyup(function () {

    var ConfirmPassword = $("#ConfirmPassword").val();
    var Password = $("#Password").val();

    if (ConfirmPassword.length == 0) {
        $("#ErrorConfirmPass").text("Confirm Password Is Required");
        $("#ConfirmPassword").addClass("input-error");
        $("#ConfirmPassword").removeClass("input-success");
    } else if (ConfirmPassword != Password) {
        $("#ErrorConfirmPass").text("Passwords Not Matching");
        $("#ConfirmPassword").addClass("input-error");
        $("#ConfirmPassword").removeClass("input-success");
    } else {
        $("#ErrorConfirmPass").text("");
        $("#ConfirmPassword").removeClass("input-error");
        $("#ConfirmPassword").addClass("input-success");
    }
});