function Savebtn() {

    var count = 0;

    var GroupID = $("#GroupID").val();

    var GroupName = $("#GroupName").val();

    var GroupDescription = $("#GroupDescription").val();

    if (GroupID.length == 0) {
        $("#ErrorGroupID").text("This Field Required");
        $("#GroupID").addClass("input-error");
        $("#GroupID").removeClass("input-success");
        $("#GroupID").focus();
        count++;
    } else {

        $("#ErrorGroupID").text("");
        $("#GroupID").removeClass("input-error");
        $("#GroupID").addClass("input-success");
    }




    if (GroupName.length == 0) {
        $("#ErrorGroupName").text("This Field Required");
        $("#GroupName").addClass("input-error");
        $("#GroupName").removeClass("input-success");
        $("#GroupName").focus();
        count++;
    } else if (GroupName.length > 20) {
        $("#ErrorGroupName").text("Maximum 20 characters");
        $("#GroupName").addClass("input-error");
        $("#GroupName").removeClass("input-success");
        $("#GroupName").focus();
        count++;
    } else {
        $("#ErrorGroupName").text("");
        $("#GroupName").removeClass("input-error");
        $("#GroupName").addClass("input-success");
    }

    if (GroupDescription.length == 0) {
        $("#ErrorGroupDescription").text("This Field Required");
        $("#GroupDescription").addClass("input-error");
        $("#GroupDescription").removeClass("input-success");
        $("#GroupDescription").focus();
        count++;
    } else {
        $("#ErrorGroupDescription").text("");
        $("#GroupDescription").removeClass("input-error");
        $("#GroupDescription").addClass("input-success");
    }

    var FromCBF = ""
    if ($('input[name=BuidingType]:checked').length <= 0) {
        count++;
        $(".BuidingTypelbl").css("border", "1px solid red");
    } else {
        $(".BuidingTypelbl").css("border", "");
        FromCBF = $("input[name=BuidingType]:checked").val();
    }

    if (count == 0) {

        $.ajax({
            type: "POST",
            url: "/CreateNewGroup/AddGroup?GroupID=" + GroupID + "&GroupName=" + GroupName + "&GroupDescription=" + GroupDescription + "&FromCBF=" + FromCBF,
            success: function (data) {

                if (data == "True") {
                    $("#ErrorGroupID").text("The Group ID Not Available");
                    $("#GroupID").addClass("input-error");
                    $("#GroupID").removeClass("input-success");
                    $("#GroupID").focus();
                }
                else {
                    location.reload();
                }
            }
        });

    }

}

$("input[name=BuidingType]").change(function () {
    $(".BuidingTypelbl").css("border", "");
});