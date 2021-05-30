function LoginBtn() {
    var count = 0;

    var UserID = $("#UserID").val();
    var Password = $("#Password").val();

    var CompanyID = "False";
    var BranchID = "False";
    var FactoryID = "False";

    //if ($("#CompanyRB").is(':checked')) {
    //    CompanyID = $("#CompanyID").val();
    //    $("#LogInWithOutCBF").text("");
    //}
    //else if ($("#BranchRB").is(':checked')) {
    //    BranchID = $("#BranchID").val();
    //    $("#LogInWithOutCBF").text("");
    //}
    //else if ($("#FactoryRB").is(':checked')) {
    //    FactoryID = $("#FactoryID").val();
    //    $("#LogInWithOutCBF").text("");
    //} else if (UserID !== "SA") {
    //    count++;
    //    $("#LogInWithOutCBF").text("You Must Choose Company OR Branch OR Factory To Login");
    //    //alert('You Must Choose Company OR Branch OR Factory To Login');
    //}


    //if (UserID.length == 0) {
    //    $("#ErrorUserID").text("User ID is Required");
    //    $("#UserID").addClass("input-error");
    //    $("#UserID").removeClass("input-success");
    //    $("#UserID").focus();
    //    count++;
    //} else {
    //    $("#ErrorUserID").text("");
    //    $("#UserID").removeClass("input-error");
    //    $("#UserID").addClass("input-success");
    //}

    //if (Password.length == 0) {
    //    $("#ErrorPassword").text("Password is Required");
    //    $("#Password").focus();
    //    $("#Password").addClass("input-error");
    //    $("#Password").removeClass("input-success");
    //    count++;
    //} else {
    //    $("#ErrorPassword").text("");
    //    $("#Password").removeClass("input-error");
    //    $("#Password").addClass("input-success");
    //}

    if (count == 0) {

        var checkCompany = $("#CompanyID").val();
        var checkBranch = $("#BranchID").val();
        var checkFactory = $("#FactoryID").val();
        /*
        if (checkCompany.length === 0) {
            $("#CompanyID").addClass("input-error");
        } else {
            $("#CompanyID").removeClass("input-error");
        }

        if (checkBranch.length === 0) {
            $("#BranchID").addClass("input-error");
        } else {
            $("#BranchID").removeClass("input-error");
        }


        if (checkFactory.length === 0) {
            $("#FactoryID").addClass("input-error");
        } else {
            $("#FactoryID").removeClass("input-error");
        }
        */

        $.ajax({
            type: "GET",
            url: "/UserLogin/ULogin?UserID=" + UserID + "&Password=" + Password + "&CompanyID=" + CompanyID + "&BranchID=" + BranchID + "&FactoryID=" + FactoryID,
            success: function (data) {
                if (data == "Error") {
                    Talert('Wrong Username OR Password');
                }
                else if (data == "CompanyActiveError") {
                    $("#LogInWithOutCBF").text("This Company is Disactive.. Please Contact System Admin");
                }
                else if (data == "BranchActiveError") {
                    $("#LogInWithOutCBF").text("This Branch is Disactive.. Please Contact System Admin");
                }
                else if (data == "FactoryActiveError") {
                    $("#LogInWithOutCBF").text("This Factory is Disactive.. Please Contact System Admin");
                }
                else if (data == "CompanyAccessError") {
                    //alert('You Have No Access To this Company');
                    $("#LogInWithOutCBF").text("You Have No Access To this Company");
                }
                else if (data == "BranchAccessError") {
                    //alert('You Have No Access To this Branch');
                    $("#LogInWithOutCBF").text("You Have No Access To this Branch");
                }
                else if (data == "FactoryAccessError") {
                    //alert('You Have No Access To this Factory');
                    $("#LogInWithOutCBF").text("You Have No Access To this Factory");
                }
                else if (data == "DisactiveAcc") {
                    //alert('This User is Disactive Please Contact The System Admin');
                    $("#LogInWithOutCBF").text("This User is Disactive Please Contact The System Admin");
                }
                else if (data == "Deleted") {
                    //alert('This User is Deleted');
                    $("#LogInWithOutCBF").text("This User is Deleted");
                }
                else {
                    window.location = "/UserLogin/ULogin?UserID=" + UserID + "&Password=" + Password + "&CompanyID=" + CompanyID + "&BranchID=" + BranchID + "&FactoryID=" + FactoryID;
                }
            }
        });
    }
}

$("#UserID").focusout(function () {

    var userID = $(this).val();

    if (userID.length === 0) {

    } else {
        $.ajax({
            type: "GET",
            url: "/UserLogin/GetCompanyID?userID=" + userID,
            success: function (result) {
                $("#CompanyID").empty();

                if (result.length === 0) {
                    $("#CompanyRB").prop("disabled", true);
                } else {
                    $("#CompanyRB").prop("disabled", false);
                    $("#CompanyID").append($('<option/>', {
                        value: "",
                        text: "-Choose-"
                    })
                    );
                    $.each(result, function (index, row) {

                        $("#CompanyID").append("<option value='" + row.CompanyID + "'>" + row.CompanyID + "</option>");

                    });
                }
            }
        });

        $.ajax({
            type: "GET",
            url: "/UserLogin/GetBranchID?userID=" + userID,
            success: function (result) {
                $("#BranchID").empty();

                if (result.length === 0) {
                    $("#BranchRB").prop("disabled", true);
                } else {
                    $("#BranchRB").prop("disabled", false);
                    $("#BranchID").append($('<option/>', {
                        value: "",
                        text: "-Choose-"
                    })
                    );
                    $.each(result, function (index, row) {

                        $("#BranchID").append("<option value='" + row.BranchID + "'>" + row.BranchID + "</option>");

                    });
                }
            }
        });

        $.ajax({
            type: "GET",
            url: "/UserLogin/GetFactoryID?userID=" + userID,
            success: function (result) {
                $("#FactoryID").empty();

                if (result.length === 0) {
                    $("#FactoryRB").prop("disabled", true);
                } else {
                    $("#FactoryRB").prop("disabled", false);
                    $("#FactoryID").append($('<option/>', {
                        value: "",
                        text: "-Choose-"
                    })
                    );
                    $.each(result, function (index, row) {

                        $("#FactoryID").append("<option value='" + row.FactoryID + "'>" + row.FactoryID + "</option>");

                    });
                }
            }
        });
    }

});