$("#NewPassword").keyup(function () {

    var Password = $(this).val();

    if (Password.length < 8) {
        $('#length').removeClass('valid').addClass('invalid');
        $("#NewPassword").addClass("input-error");
        $("#NewPassword").removeClass("input-success");
    } else {
        $('#length').removeClass('invalid').addClass('valid');
        $("#NewPassword").removeClass("input-error");
        $("#NewPassword").addClass("input-success");
    }

    if (!Password.match(/[A-z]/)) {
        $('#letter').removeClass('valid').addClass('invalid');
        $("#NewPassword").addClass("input-error");
        $("#NewPassword").removeClass("input-success");
    } else {
        $('#letter').removeClass('invalid').addClass('valid');
        $("#NewPassword").removeClass("input-error");
        $("#NewPassword").addClass("input-success");
    }

    //validate capital letter
    if (!Password.match(/[A-Z]/)) {
        $('#capital').removeClass('valid').addClass('invalid');
        $("#NewPassword").addClass("input-error");
        $("#NewPassword").removeClass("input-success");
    } else {
        $('#capital').removeClass('invalid').addClass('valid');
        $("#NewPassword").removeClass("input-error");
        $("#NewPassword").addClass("input-success");
    }

    //validate number
    if (!Password.match(/\d/)) {
        $('#number').removeClass('valid').addClass('invalid');
        $("#NewPassword").addClass("input-error");
        $("#NewPassword").removeClass("input-success");
    } else {
        $('#number').removeClass('invalid').addClass('valid');
        $("#NewPassword").removeClass("input-error");
        $("#NewPassword").addClass("input-success");
    }

    if (!Password.match(/\d/) || !Password.match(/[A-Z]/) || !Password.match(/[A-z]/) || Password.length < 8) {
        $('#pswd_info').show();
        $("#NewPassword").addClass("input-error");
        $("#NewPassword").removeClass("input-success");
    }

}).focus(function () {
    $('#pswd_info').show();
    $("#ErrorPassword").hide();
}).blur(function () {
    $('#pswd_info').hide();
    $("#ErrorPassword").show();
});

$("#OldPassword").keyup(function () {

    var OldPassword = $("#OldPassword").val();

    if (OldPassword.length == 0) {
        $("#ErrorOldPassword").text("Old Password Is Required");
        $("#OldPassword").addClass("input-error");
        $("#OldPassword").removeClass("input-success");      
    }
    else {
        $("#ErrorOldPassword").text("");
        $("#OldPassword").removeClass("input-error");
        $("#OldPassword").addClass("input-success");
    }
});

$("#ConfirmPassword").keyup(function () {

    var ConfirmPassword = $("#ConfirmPassword").val();
    var NewPassword = $("#NewPassword").val();

    if (ConfirmPassword.length == 0) {
        $("#ErrorConfirmPassword").text("Confirm Password Is Required");
        $("#ConfirmPassword").addClass("input-error");
        $("#ConfirmPassword").removeClass("input-success");
    }
    else if (ConfirmPassword != NewPassword) {
        $("#ErrorConfirmPassword").text("Confirm Password Is Wrong");
        $("#ConfirmPassword").addClass("input-error");
        $("#ConfirmPassword").removeClass("input-success");
    } else {
        $("#ErrorConfirmPassword").text("");
        $("#ConfirmPassword").removeClass("input-error");
        $("#ConfirmPassword").addClass("input-success");
    }

});