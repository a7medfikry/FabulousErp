$("#UserID").keyup(function () {

    var UserID = $("#UserID").val();

    if (UserID.length == 0) {
        $("#ErrorUserID").text("User ID is Required");
        $("#UserID").addClass("input-error");
        $("#UserID").removeClass("input-success");
        $("#UserID").focus();
    } else {
        $("#ErrorUserID").text("");
        $("#UserID").removeClass("input-error");
        $("#UserID").addClass("input-success");
    }

});

$("#Password").keyup(function () {

    var Password = $("#Password").val();

    if (Password.length == 0) {
        $("#ErrorPassword").text("Password is Required");
        $("#Password").focus();
        $("#Password").addClass("input-error");
        $("#Password").removeClass("input-success");
    } else {
        $("#ErrorPassword").text("");
        $("#Password").removeClass("input-error");
        $("#Password").addClass("input-success");
    }

});