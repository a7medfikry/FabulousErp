$(document).ready(function () {

    var branchIDFB = $("#BranchID").val();
    if (branchIDFB.length > 0) {
        GetBranchCost(branchIDFB);
    }

    $("#BranchID").change(function () {

        var BranchID = $(this).val();

        $("#GlobalError").text("");
        $("#BranchCostData").html("");

        if (BranchID.length > 0) {

            $(this).css("border-color", "");

            $.ajax({
                type: "GET",
                url: "/B_CreateCostCenter/GetBranchName?BranchID=" + BranchID,
                success: function (result) {

                    if (result == "False") {
                        $("#BranchName").val("");
                        $("#AddCostCenterToBranch").prop("disabled", true);
                        $("#GlobalError").text("You not Have Access To This Company..!");
                    } else {
                        $("#BranchName").val(result);
                        $("#AddCostCenterToBranch").prop("disabled", false);
                        GetBranchCost(BranchID);
                    }
                }
            });

        } else {
            $(this).css("border-color", "red");
            $("#BranchName").val("");
            $("#AddCostCenterToBranch").prop("disabled", false);
        }
    });

    $("#CostCenterID").keyup(function (event) {

        event.preventDefault();

        if (event.keyCode === 13) {
            $("#AddCostCenterToBranch").click();
        }

        var CostCenterID = $(this).val();

        if (CostCenterID.length === 0) {
            $(this).css("border-color", "red");

        } else if (!CostCenterID.match("^[A-Za-z].*")) {
            $(this).css("border-color", "red");
            $("#GlobalError").text("Cost Center ID Must Start With Character..!");

        } else {
            $(this).css("border-color", "");
        }
    }).focusout(function () {

        var CostCenterID = $(this).val();

        if (CostCenterID.length === 0) {
            $(this).css("border-color", "red");
        } else {
            $(this).css("border-color", "");
        }

    });

    $("#CostCenterName").keyup(function (event) {

        event.preventDefault();

        if (event.keyCode === 13) {
            $("#AddCostCenterToBranch").click();
        }

        var CostCenterName = $(this).val();

        if (CostCenterName.length === 0) {
            $(this).css("border-color", "red");
        } else {
            $(this).css("border-color", "");
        }
    }).focusout(function () {

        var CostCenterName = $(this).val();

        if (CostCenterName.length === 0) {
            $(this).css("border-color", "red");
        } else {
            $(this).css("border-color", "");
        }

    });


    $("#AddCostCenterToBranch").click(function () {

        var BranchID = $("#BranchID").val();

        var CostCenterID = $("#CostCenterID").val();

        var CostCenterName = $("#CostCenterName").val();

        var Test = true;

        if (BranchID.length === 0) {
            $("#BranchID").css("border-color", "red");
            Test = false;
        } else {
            $("#BranchID").css("border-color", "");
        }

        if (CostCenterID.length === 0) {
            $("#CostCenterID").css("border-color", "red");
            Test = false;

        } else if (!CostCenterID.match("^[A-Za-z].*")) {
            $("#CostCenterID").css("border-color", "red");
            $("#GlobalError").text("Cost Cenetr ID Must Start With Character..!");
            Test = false;

        } else {
            $("#CostCenterID").css("border-color", "");
        }

        if (CostCenterName.length === 0) {
            $("#CostCenterName").css("border-color", "red");
            Test = false;
        } else {
            $("#CostCenterName").css("border-color", "");
        }

        if (Test === true) {

            $.ajax({

                type: "POST",
                url: "/B_CreateCostCenter/SaveBranchCostCenter?BranchID=" + BranchID + "&CostCenterID=" + CostCenterID + "&CostCenterName=" + CostCenterName,
                success: function (result) {
                    if (result == "False") {
                        $("#GlobalError").text("Not Valid Cost Center ID..!");
                        $("#CostCenterID").css("border-color", "red");
                        $("#SaveSuccess").text("");
                    } else {
                        $("#GlobalError").text("");
                        $("#CostCenterID").css("border-color", "");
                        $("#CostCenterID").val("");
                        $("#CostCenterName").val("");
                        $("#SaveSuccess").text("Saved..");
                        $("#CostCenterID").focus();
                        GetBranchCost(BranchID);
                    }
                }
            });
        }
    });

});


function GetBranchCost(branchID) {

    var tble = $("#BranchCostData");

    tble.html("");

    $.ajax({
        type: "GET",
        url: "/B_CreateCostCenter/GetCostCenter?branchID=" + branchID,
        success: function (result) {
            if (result.length === 0) {
                tble.append("<tr><td colspan='2' class='text-danger'>No Cost Center Created To This Branch..!</td></tr>")
            } else {

                for (var i = 0; i < result.length; i++) {

                    var data = "<tr>"
                        + "<td>" + result[i].CostCenterID + "</td>"
                        + "<td>" + result[i].CostCenterName + "</td>"
                        + "</tr>"

                    tble.append(data);
                }
            }
        }
    });
}
