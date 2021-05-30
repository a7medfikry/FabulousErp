$(document).ready(function () {

    var branchIDFB = $("#BranchIDFB").val();
    if ($("#BranchIDFB").length > 0) {
        FilterCostCenterByBranch(branchIDFB);
    }

});

function HideImpThings() {
    $("#CCAccountsInputs").hide();
    $("#AddCCAccountsBranchBtn").hide();
    $("#CCAccountsTbl").html("");
    $("#CostAccountID").css("border-color", "");
    $("#CostAccountName").css("border-color", "");
    $("#CostAccountID").val("");
    $("#CostAccountName").val("");
    $("#AccessError").text("");
}

$("#BranchID").change(function () {

    var BranchID = $(this).val();

    HideImpThings();

    if (BranchID.length > 0) {

        $(this).css("border-color", "");

        $.ajax({
            type: "GET",
            url: "/B_CreateCostCenterAccounts/GetBranchName?BranchID=" + BranchID,
            success: function (result) {
                if (result == "False") {
                    $("#AccessError").text("You Not Have Access To This Branch..!");
                    $("#FilterCostCenterIDBranch").hide();
                    $("#BranchName").val("");
                    $(this).css("border-color", "red");
                } else {
                    $("#BranchName").val(result);
                    FilterCostCenterByBranch(BranchID);
                    $("#FilterCostCenterIDBranch").show();
                    $("#AccessError").text("");
                }
            }
        });

    } else {
        $("#AccessError").text("");
        $("#FilterCostCenterIDBranch").hide();
        $("#BranchName").val("");
        $(this).css("border-color", "red");
    }
});


function FilterCostCenterByBranch(BranchID) {
    $.ajax({
        type: "GET",
        url: "/B_CreateCostCenterAccounts/FilterCostCenterIDForBranch?BranchID=" + BranchID,
        success: function (data) {

            $("#BranchCostCenterID").empty();

            if (data.length == 0) {

                $("#BranchCostCenterName").val("");

                $("#BranchCostCenterID").append($('<option/>', {
                    value: -1,
                    text: "No Cost Center Created in this Branch!"

                })
                );

            } else {

                $("#BranchCostCenterName").val("");

                $("#BranchCostCenterID").append($('<option/>', {
                    value: -1,
                    text: "-Choose-"

                })
                );

                $.each(data, function (index, row) {

                    $("#BranchCostCenterID").append("<option value='" + row.CostCenterID + "'>" + row.CostCenterID + "</option>");

                });
            }
        }
    });
}



$("#BranchCostCenterID").change(function () {

    var BranchCostCenterID = $(this).val();

    if (BranchCostCenterID == "-1") {

        HideImpThings();

        $(this).css("border-color", "red");
        $("#BranchCostCenterName").val("");

    } else {

        $("#CCAccountsInputs").show();
        $("#AddCCAccountsBranchBtn").show();

        GetBranchCCAccountsData();

        $(this).css("border-color", "");

        $.ajax({
            type: "GET",
            url: "/B_CreateCostCenterAccounts/GetBranchCostCenter?CostCenterID=" + BranchCostCenterID,
            success: function (result) {
                $("#BranchCostCenterName").val(result);
            }
        });
    }
});

function GetBranchCCAccountsData() {

    var BranchCostCenterID = $("#BranchCostCenterID").val();

    var tableData = $("#CCAccountsTbl");
    tableData.html("");

    $.ajax({
        type: "GET",
        url: "/B_CreateCostCenterAccounts/GetBranchCCAccountsData?BranchCostCenterID=" + BranchCostCenterID,
        success: function (result) {

            for (var i = 0; i < result.length; i++) {

                var Data = "<tr class='row_" + result[i].CostAccountID + "'>" +
                    "<td>" + result[i].CostAccountID + "</td>" +
                    "<td>" + result[i].CostAccountName + "</td>" +
                    "<td>" + '<a href="#" onclick="DeleteBranchCCAccount(\'' + result[i].CostAccountID + '\')"><span class="fa fa-trash-o"></span></a>' + "</td>" +
                    "</tr>";
                tableData.append(Data);
            }
        }
    });

}


$("#AddCCAccountsBranchBtn").click(function () {

    var CostCenterID = $("#BranchCostCenterID").val();

    var CostAccountID = $("#CostAccountID").val();

    var CostAccountName = $("#CostAccountName").val();

    var Test = true;

    if (CostCenterID == "-1") {
        $("#BranchCostCenterID").css("border-color", "red");
        Test = false;
    } else {
        $("#BranchCostCenterID").css("border-color", "");
    }

    if (CostAccountID.length === 0) {
        $("#CostAccountID").css("border-color", "red");
        $("#CostAccountID").focus();
        Test = false;
    } else {
        $("#CostAccountID").css("border-color", "");
    }

    if (CostAccountName.length === 0) {
        $("#CostAccountName").css("border-color", "red");
        $("#CostAccountName").focus();
        Test = false;
    } else {
        $("#CostAccountName").css("border-color", "");
    }

    if (Test === true) {

        $.ajax({
            type: "POST",
            url: "/B_CreateCostCenterAccounts/SaveRecordBranchCCAccounts?CostCenterID=" + CostCenterID + "&CostAccountID=" + CostAccountID + "&CostAccountName=" + CostAccountName,
            success: function (result) {
                if (result == "False") {
                    $("#CostAccountID").css("border-color", "red");
                    $("#GlobalError").text("Cost Account ID not Valid..!");
                    $("#CostAccountID").focus();
                } else {
                    $("#CostAccountID").css("border-color", "");
                    $("#GlobalError").text("");
                    $("#CostAccountID").val("");
                    $("#CostAccountName").val("");
                    $("#CostAccountID").focus();

                    GetBranchCCAccountsData();
                }
            }
        });
    }
});


// Validation Cost center Accounts Content
$("#CostAccountID").keyup(function (event) {

    event.preventDefault();

    if (event.keyCode === 13) {
        $("#AddCCAccountsBranchBtn").click();
    }

    var CostAccountID = $(this).val();

    if (CostAccountID.length === 0) {
        $(this).css("border-color", "red");
    }
    else {
        $(this).css("border-color", "");
    }
}).focusout(function () {

    var CostAccountID = $(this).val();

    if (CostAccountID.length === 0) {
        $(this).css("border-color", "red");
    }
    else {
        $(this).css("border-color", "");
    }

});

$("#CostAccountName").keyup(function (event) {

    event.preventDefault();

    if (event.keyCode === 13) {
        $("#AddCCAccountsBranchBtn").click();
    }

    var CostAccountName = $(this).val();

    if (CostAccountName.length === 0) {
        $(this).css("border-color", "red");
    }
    else {
        $(this).css("border-color", "");
    }
}).focusout(function () {

    var CostAccountName = $(this).val();

    if (CostAccountName.length === 0) {
        $(this).css("border-color", "red");
    }
    else {
        $(this).css("border-color", "");
    }

});
//---------------------------------------------------------------------------------



function DeleteBranchCCAccount(CostAccountID) {
    $("#CCAccountID").text(CostAccountID);

    $("#DeleteConfirmation").modal("show");
}

function BranchConfirmDelete() {

    var CCAccountID = $("#CCAccountID").text();

    $.ajax({
        type: "POST",
        url: "/B_CreateCostCenterAccounts/DeleteCostAccountBranch?CCAccountID=" + CCAccountID,
        success: function (result) {
            $("#DeleteConfirmation").modal("hide");
            $(".row_" + CCAccountID).remove();
        }
    });

}