$(document).ready(function () {

    var branchIDFB = $("#BranchID").val();
    if (branchIDFB.length > 0) {
        GetBranchAnalytic(branchIDFB)
    }

    $("#BranchID").change(function () {

        var BranchID = $(this).val();

        $("#GlobalError").text("");
        $("#BranchAnalyticData").html("");
        $("#BranchName").val("");

        if (BranchID.length === 0) {

            $(this).css("border-color", "red");
            $("#AddAnalyticToBranch").prop("disabled", false);

        } else {

            $(this).css("border-color", "");

            $.ajax({
                type: "GET",
                url: "/B_CreateAnalyticAccounts/GetBranchName?BranchID=" + BranchID,
                success: function (result) {
                    if (result == "False") {
                        $("#AddAnalyticToBranch").prop("disabled", true);
                        $("#GlobalError").text("You Not Have Access To This Branch..!");
                        $("#BranchName").val("");
                    } else {
                        $("#AddAnalyticToBranch").prop("disabled", false);
                        $("#BranchName").val(result);

                        GetBranchAnalytic(BranchID);
                    }
                }
            });
        }
    });

    $("#AnalyticID").keyup(function (event) {

        event.preventDefault();

        if (event.keyCode === 13) {
            $("#AddAnalyticToBranch").click();
        }

        var AnalyticID = $(this).val();

        if (AnalyticID.length === 0) {
            $(this).css("border-color", "red");

        } else if (!AnalyticID.match("^[A-Za-z].*")) {
            $(this).css("border-color", "red");
            $("#GlobalError").text("Analytic Accounts ID Must Start With Character..!");
        } else {
            $(this).css("border-color", "");
        }
    }).focusout(function () {

        var AnalyticID = $(this).val();

        if (AnalyticID.length === 0) {
            $(this).css("border-color", "red");
        } else {
            $(this).css("border-color", "");
        }

    });

    $("#AnalyticName").keyup(function (event) {

        event.preventDefault();

        if (event.keyCode === 13) {
            $("#AddAnalyticToBranch").click();
        }

        var AnalyticName = $(this).val();

        if (AnalyticName.length === 0) {
            $(this).css("border-color", "red");
        } else {
            $(this).css("border-color", "");
        }
    }).focusout(function () {

        var AnalyticName = $(this).val();

        if (AnalyticName.length === 0) {
            $(this).css("border-color", "red");
        } else {
            $(this).css("border-color", "");
        }

    });


    $("#AddAnalyticToBranch").click(function () {

        var BranchID = $("#BranchID").val();

        var AnalyticID = $("#AnalyticID").val();

        var AnalyticName = $("#AnalyticName").val();

        var Test = true;

        if (BranchID.length === 0) {
            $("#BranchID").css("border-color", "red");
            Test = false;
        } else {
            $("#BranchID").css("border-color", "");
        }

        if (AnalyticID.length === 0) {
            $("#AnalyticID").css("border-color", "red");
            Test = false;

        } else if (!AnalyticID.match("^[A-Za-z].*")) {
            $("#AnalyticID").css("border-color", "red");
            $("#GlobalError").text("Analytic Accounts ID Must Start With Character..!");
            Test = false;

        } else {
            $("#AnalyticID").css("border-color", "");
        }

        if (AnalyticName.length === 0) {
            $("#AnalyticName").css("border-color", "red");
            Test = false;
        } else {
            $("#AnalyticName").css("border-color", "");
        }


        if (Test === true) {

            $.ajax({
                type: "POST",
                url: "/B_CreateAnalyticAccounts/AddAnalyticAccounts?BranchID=" + BranchID + "&AnalyticID=" + AnalyticID + "&AnalyticName=" + AnalyticName,
                success: function (result) {
                    if (result === "False") {
                        $("#AnalyticID").css("border-color", "red");
                        $("#GlobalError").text("Analytic Account ID not valid..!");
                        $("#SaveSuccess").text("");
                    } else {
                        $("#AnalyticID").css("border-color", "");
                        $("#GlobalError").text("");
                        $("#AnalyticID").val("");
                        $("#AnalyticName").val("");
                        $("#SaveSuccess").text("Saved..");
                        $("#AnalyticID").focus();
                        GetBranchAnalytic(BranchID);
                    }
                }
            });
        }
    });

});

function GetBranchAnalytic(branchID) {

    var tbl = $("#BranchAnalyticData");

    tbl.html("");

    $.ajax({
        type: "GET",
        url: "/B_CreateAnalyticAccounts/GetAnalyticAccounts?branchID=" + branchID,
        success: function (result) {
            if (result.length === 0) {
                tbl.append("<tr><td colspan='2' class='text-danger'>No Analytic Created To This Branch..!</td></tr>")
            } else {

                for (var i = 0; i < result.length; i++) {

                    var data = "<tr>"
                        + "<td>" + result[i].AnalyticAccountID + "</td>"
                        + "<td>" + result[i].AnalyticAccountName + "</td>"
                        + "</tr>"

                    tbl.append(data);
                }
            }
        }
    });
}
