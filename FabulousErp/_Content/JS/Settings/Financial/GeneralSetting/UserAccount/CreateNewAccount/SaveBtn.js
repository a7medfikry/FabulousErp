function Save()
{
    var Count = 0;

    var UserID = $("#UserID").val();

    var UserName = $("#UserName").val();

    var Password = $("#Password").val();

    var ConfirmPassword = $("#ConfirmPassword").val();


    var ChangePassFirst = "False";
    if ($("#ChangePassFirst").is(":checked")) {
        ChangePassFirst = "True";
    }

    var UpdateProfFirst = "False";
    if ($("#UpdateProfFirst").is(":checked")) {
        UpdateProfFirst = "True";
    }

    var PasswordExpired = "False";
    if ($("#PasswordExpired").is(":checked")) {
        PasswordExpired = "True";
    }

    var DisActive = "False";
    if ($("#DisActive").is(":checked")) {
        DisActive = "True";
    }

    //validation User ID
    if (UserID.length == 0) {
        $("#ErrorUserID").text("User ID Is Required");
        $("#UserID").addClass("input-error");
        $("#UserID").removeClass("input-success");
        $("#UserID").focus();
        Count++;
        ReTranslate();
    } else {
        
        $.ajax({
            type: "GET",
            url: "/CreateNewAccount/CheckUserID?UserID=" + UserID,
            success: function (data) {
                if (data == "True") {
                    $("#ErrorUserID").text("The User ID Not Available");
                    $("#UserID").addClass("input-error");
                    $("#UserID").removeClass("input-success");
                    $("#UserID").focus();
                    Count++;
                } else {
                    $("#ErrorUserID").text("");
                    $("#UserID").removeClass("input-error");
                    $("#UserID").addClass("input-success");
                }
                ReTranslate();
            }
        });
        
    }
    
//***********************************************************************************

    //validation username
    if (UserName.length == 0) {
        $("#ErrorUserName").text("User Name Is Required");
        $("#UserName").addClass("input-error");
        $("#UserName").removeClass("input-success");
        $("#UserName").focus();
        Count++;
    } else if (!UserName.match("^[A-Za-z].*")) {
        $("#ErrorUserName").text("User Name Must Start With Character");
        $("#UserName").addClass("input-error");
        $("#UserName").removeClass("input-success");
        $("#UserName").focus();
        Count++;
    } else if (UserName.length < 6) {
        $("#ErrorUserName").text("User Name Required 6 Charachters");
        $("#UserName").addClass("input-error");
        $("#UserName").removeClass("input-success");
        $("#UserName").focus();
        Count++;
    } else if (UserName.length > 25) {
        $("#ErrorUserName").text("User Name Maximum 25 Characters");
        $("#UserName").addClass("input-error");
        $("#UserName").removeClass("input-success");
        $("#UserName").focus();
        Count++;
    } else {
        $("#ErrorUserName").text("");
        $("#UserName").removeClass("input-error");
        $("#UserName").addClass("input-success");
    }
//*********************************************************************************************

    //validation password

    if (Password.length < 8) {
        $('#length').removeClass('valid').addClass('invalid');
        $("#Password").addClass("input-error");
        $("#Password").removeClass("input-success");
        $('#pswd_info').show();
        Count++;
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
        Count++;
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
        Count++;
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
        Count++;
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

    /*
    if (Password.length > 50) {
        $('#Maxlength').removeClass('valid').addClass('invalid');
        $('#pswd_info').show();
        Count++;
    } else
    {
        $('#pswd_info').hide();
        $('#Maxlength').removeClass('invalid').addClass('valid');
    }
    if (Password.length == 0) {
        $("#ErrorPassword").text("Password Is Required");
        $("#Password").addClass("input-error");
        Count++;
    } else if (Password.length < 8) {
        $("#ErrorPassword").text("Password Required 8 Charachters");
        $("#Password").addClass("input-error");
        Count++;
    } else if (!Password.match(/[A-z]/)) {
        $("#ErrorPassword").text("Password Required at least one letter");
        $("#Password").addClass("input-error");
        Count++;
    } else if (!Password.match(/[A-Z]/)) {
        $("#ErrorPassword").text("Password Required at least one Capital letter");
        $("#Password").addClass("input-error");
        Count++;
    } else if (!Password.match(/\d/)) {
        $("#ErrorPassword").text("Password Required at least one Number");
        $("#Password").addClass("input-error");
        Count++;
    } else if (Password.length > 50) {
        $("#ErrorPassword").text("Password Maximum 50 Charachters");
        $("#Password").addClass("input-error");
        Count++;
    } else {
        $("#ErrorPassword").text("");
        $("#Password").removeClass("input-error");
    }
    */
    //********************************************************************************************
    if (ConfirmPassword.length == 0) {
        $("#ErrorConfirmPass").text("Confirm Password Is Required");
        $("#ConfirmPassword").addClass("input-error");
        $("#ConfirmPassword").removeClass("input-success");
        $("#ConfirmPassword").focus();
        Count++;
    } else if (ConfirmPassword != Password) {
        $("#ErrorConfirmPass").text("Passwords Not Matching");
        $("#ConfirmPassword").addClass("input-error");
        $("#ConfirmPassword").removeClass("input-success");
        $("#ConfirmPassword").focus();
        Count++;
    } else {
        $("#ErrorConfirmPass").text("");
        $("#ConfirmPassword").removeClass("input-error");
        $("#ConfirmPassword").addClass("input-success");
    }
    ReTranslateSpan();
    if (Count == 0) {

        $.ajax({
            type: "Post",
            url: "/CreateNewAccount/AddAccount?UserID=" + UserID + "&UserName=" + UserName + "&Password=" + Password
                + "&ChangePassFirst=" + ChangePassFirst + "&UpdateProfFirst=" + UpdateProfFirst + "&PasswordExpired=" + PasswordExpired
                + "&DisActive=" + DisActive,
            success: function (data) {

                if (data === "False") {
                    $("#ErrorUserID").text("The User ID Not Available");
                    $("#UserID").addClass("input-error");
                    $("#UserID").removeClass("input-success");
                    $("#UserID").focus();
                }
                else if (data === "DisActive") {
                    $("#GroupDisactiveError").show();
                    $("#GroupDisactiveError").text("This Group is Disactive you can't Choose it.. Please Contact System Admin");
                }
                else {
                    $("#GroupDisactiveError").hide();

                    $("#UserID").val("");
                    $("#UserName").val("");
                    $("#Password").val("");
                    $("#ConfirmPassword").val("");
                    $("#GroupName").text("");
                    $("#ErrorInSearch").text("");
                    $("#ErrorUserID").text("");

                    $("#UserID").removeClass("input-success");
                    $("#UserName").removeClass("input-success");
                    $("#Password").removeClass("input-success");
                    $("#ConfirmPassword").removeClass("input-success");

                    $('#length').removeClass('valid').addClass('invalid');
                    $('#letter').removeClass('valid').addClass('invalid');
                    $('#capital').removeClass('valid').addClass('invalid');
                    $('#number').removeClass('valid').addClass('invalid');

                    $("#ChangePassFirst").prop("checked", false);
                    $("#UpdateProfFirst").prop("checked", false);
                    $("#PasswordExpired").prop("checked", false);
                    $("#DisActive").prop("checked", false);

                    $("#SavedSuccess").show("fast");
                    $("#SavedSuccess").text("Account Added Successfully");
                    $("#SavedSuccess").fadeOut(4000);
                    ReTranslateSpan();
                }
            }
        });
    }
}