$(document).ready(function () {

    var branchIDFB = $("#BranchIDFB").val();

    if ($("#BranchIDFB").length > 0) {
        FilterDistribByBranch(branchIDFB);
    }

});

function ClearInCBFChange() {
    $("#DistributionInputs").hide();
    $("#AddDistributionBranchBtn").hide();
    $("#AccountDistributionTbl").html("");
    $("#DistributionID").val("");
    $("#DistributionName").val("");
    $("#DistributionID").css("border-color", "");
    $("#DistributionName").css("border-color", "");
    $("#AccessError").text("");
}


$("#BranchID").change(function () {

    var BranchID = $(this).val();

    ClearInCBFChange();

    if (BranchID.length > 0) {

        $(this).css("border-color", "");

        $.ajax({
            type: "GET",
            url: "/B_AnalyticAccountDistribution/GetBranchName?BranchID=" + BranchID,
            success: function (result) {
                if (result == "False") {
                    $("#AccessError").text("You Not Have Access To This Branch..!");
                    $("#BranchAnalytic").hide();
                    $("#BranchName").val("");
                    $(this).css("border-color", "red");
                } else {
                    $("#AccessError").text("");
                    $("#BranchName").val(result);
                    $("#BranchAnalytic").show();
                    FilterDistribByBranch(BranchID);
                }
            }
        });

    } else {
        $("#AccessError").text("");
        $("#BranchAnalytic").hide();
        $("#BranchName").val("");
        $(this).css("border-color", "red");
    }
});


function FilterDistribByBranch(BranchID) {
    $.ajax({
        type: "GET",
        url: "/B_AnalyticAccountDistribution/FilterAnalyticIDForBranch?BranchID=" + BranchID,
        success: function (data) {

            $("select#BranchAnalyticID").empty();

            if (data.length == 0) {

                $("#BranchAnalyticName").val("");

                $("select#BranchAnalyticID").append($('<option/>', {
                    value: -1,
                    text: "No Analytic Created in this Branch!"

                })
                );

            } else {

                $("#BranchAnalyticName").val("");

                $("select#BranchAnalyticID").append($('<option/>', {
                    value: -1,
                    text: "-Choose-"

                })
                );

                $.each(data, function (index, row) {

                    $("#BranchAnalyticID").append("<option value='" + row.AnalyticID + "'>" + row.AnalyticID + "</option>");

                });
            }
        }
    });
}


$("#BranchAnalyticID").change(function () {

    var AnalyticID = $(this).val();

    if (AnalyticID == "-1") {

        ClearInCBFChange();

        $(this).css("border-color", "red");
        $("#BranchAnalyticName").val("");
    } else {
        $(this).css("border-color", "");

        $("#DistributionInputs").show();
        $("#AddDistributionBranchBtn").show();

        $.ajax({
            type: "GET",
            url: "/B_AnalyticAccountDistribution/GetBranchAnalyticName?AnalyticID=" + AnalyticID,
            success: function (result) {
                $("#BranchAnalyticName").val(result.Name);
                GetBranchAnalyticData();
            }
        });
    }
});


function GetBranchAnalyticData() {

    var BranchAnalyticID = $("#BranchAnalyticID").val();

    var tableData = $("#AccountDistributionTbl");
    tableData.html("");

    $.ajax({
        type: "GET",
        url: "/B_AnalyticAccountDistribution/GetBranchAnalyticDistributionData?BranchAnalyticID=" + BranchAnalyticID,
        success: function (result) {

            for (var i = 0; i < result.length; i++) {

                var Data = "<tr class='row_" + result[i].AccountDistributionID + "'>" +
                    "<td>" + result[i].AccountDistributionID + "</td>" +
                    "<td>" + result[i].AccountDistributionName + "</td>" +
                    "<td>" + '<a href="#" onclick="DeleteBranchDistribution(\'' + result[i].AccountDistributionID + '\')"><span class="fa fa-trash-o"></span></a>' + "</td>" +
                    "</tr>";
                tableData.append(Data);
            }
        }
    });
}


$("#DistributionID").keyup(function (event) {

    event.preventDefault();

    if (event.keyCode === 13) {
        $("#AddDistributionBranchBtn").click();
    }

    var DistributionID = $(this).val();

    if (DistributionID.length === 0) {
        $(this).css("border-color", "red");
    }
    else {
        $(this).css("border-color", "");
    }
}).focusout(function () {

    var DistributionID = $(this).val();

    if (DistributionID.length === 0) {
        $(this).css("border-color", "red");
    }
    else {
        $(this).css("border-color", "");
    }

});

$("#DistributionName").keyup(function (event) {

    event.preventDefault();

    if (event.keyCode === 13) {
        $("#AddDistributionBranchBtn").click();
    }

    var DistributionName = $(this).val();

    if (DistributionName.length === 0) {
        $(this).css("border-color", "red");
    }
    else {
        $(this).css("border-color", "");
    }
}).focusout(function () {

    var DistributionName = $(this).val();

    if (DistributionName.length === 0) {
        $(this).css("border-color", "red");
    }
    else {
        $(this).css("border-color", "");
    }

});



$("#AddDistributionBranchBtn").click(function () {

    var BranchAnalyticID = $("#BranchAnalyticID").val();

    var DistributionID = $("#DistributionID").val();

    var DistributionName = $("#DistributionName").val();

    var Test = true;


    // validation in button Save---------------------------

    if (BranchAnalyticID == "-1") {
        $("#BranchAnalyticID").css("border-color", "red");
        Test = false;
    } else {
        $("#BranchAnalyticID").css("border-color", "");
    }


    if (DistributionID.length === 0) {
        $("#DistributionID").css("border-color", "red");
        Test = false;
    } else {
        $("#DistributionID").css("border-color", "");
    }

    if (DistributionName.length === 0) {
        $("#DistributionName").css("border-color", "red");
        Test = false;
    } else {
        $("#DistributionName").css("border-color", "");
    }
    //-------------------------------------------------------------

    if (Test === true) {

        $.ajax({
            type: "POST",
            url: "/B_AnalyticAccountDistribution/SaveDistributionRecordBranch?BranchAnalyticID=" + BranchAnalyticID + "&DistributionID=" + DistributionID + "&DistributionName=" + DistributionName,
            success: function (result) {
                if (result === "False") {
                    $("#DistributionID").css("border-color", "red");
                    $("#GlobalError").text("Account Distribution ID not Valid..!");
                } else {
                    $("#DistributionID").css("border-color", "");
                    $("#GlobalError").text("");
                    $("#DistributionID").val("");
                    $("#DistributionName").val("");
                    $("#DistributionID").focus();

                    GetBranchAnalyticData();
                }
            }
        });
    }
});


function DeleteBranchDistribution(AccountDistributionID) {

    $("#AccountDistributionID").text(AccountDistributionID);

    $("#DeleteConfirmation").modal("show");

}

function BranchConfirmDelete() {

    var AccountDistributionID = $("#AccountDistributionID").text();

    $.ajax({
        type: "POST",
        url: "/B_AnalyticAccountDistribution/DeleteAccountDistributionBranch?AccountDistributionID=" + AccountDistributionID,
        success: function (result) {
            $("#DeleteConfirmation").modal("hide");
            $(".row_" + AccountDistributionID).remove();
        }
    });
}