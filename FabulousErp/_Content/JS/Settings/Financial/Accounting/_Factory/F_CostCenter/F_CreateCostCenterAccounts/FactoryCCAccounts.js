$(document).ready(function () {

    var factoryIDFF = $("#FactoryIDFF").val();
    if ($("#FactoryIDFF").length > 0) {
        FilterCostCenterByFactory(factoryIDFF);
    }
});

function HideImpThings() {
    $("#CCAccountsInputs").hide();
    $("#AddCCAccountsFactoryBtn").hide();
    $("#CCAccountsTbl").html("");
    $("#CostAccountID").css("border-color", "");
    $("#CostAccountName").css("border-color", "");
    $("#CostAccountID").val("");
    $("#CostAccountName").val("");
    $("#AccessError").text("");
}

$("#FactoryID").change(function () {

    var FactoryID = $(this).val();

    HideImpThings();

    if (FactoryID.length > 0) {

        $(this).css("border-color", "");

        $.ajax({
            type: "GET",
            url: "/F_CreateCostCenterAccounts/GetFatoryName?FactoryID=" + FactoryID,
            success: function (result) {

                if (result == "False") {
                    $("#AccessError").text("You Not Have Access To This Factory..!");
                    $("#FilterCostCenterIDFactory").hide();
                    $("#FactoryName").val("");
                    $(this).css("border-color", "red");
                } else {
                    $("#FactoryName").val(result);
                    FilterCostCenterByFactory(FactoryID);
                    $("#FilterCostCenterIDFactory").show();
                    $("#AccessError").text("");

                }
            }
        });

    } else {
        $("#AccessError").text("");
        $("#FilterCostCenterIDFactory").hide();
        $("#FactoryName").val("");
        $(this).css("border-color", "red");
    }
});


function FilterCostCenterByFactory(FactoryID) {
    $.ajax({
        type: "GET",
        url: "/F_CreateCostCenterAccounts/FilterCostCenterIDForFactory?FactoryID=" + FactoryID,
        success: function (data) {

            $("#FactoryCostCenterID").empty();

            if (data.length == 0) {

                $("#FactoryCostCenterName").val("");

                $("#FactoryCostCenterID").append($('<option/>', {
                    value: -1,
                    text: "No Cost Center Created in this Factory!"

                })
                );

            } else {

                $("#FactoryCostCenterName").val("");

                $("#FactoryCostCenterID").append($('<option/>', {
                    value: -1,
                    text: "-Choose-"

                })
                );

                $.each(data, function (index, row) {

                    $("#FactoryCostCenterID").append("<option value='" + row.CostCenterID + "'>" + row.CostCenterID + "</option>");

                });
            }
        }
    });
}



$("#FactoryCostCenterID").change(function () {

    var FactoryCostCenterID = $(this).val();

    if (FactoryCostCenterID == "-1") {

        HideImpThings();

        $(this).css("border-color", "red");
        $("#FactoryCostCenterName").val("");

    } else {

        $("#CCAccountsInputs").show();
        $("#AddCCAccountsFactoryBtn").show();

        GetFactoryCCAccountsData();

        $(this).css("border-color", "");

        $.ajax({
            type: "GET",
            url: "/F_CreateCostCenterAccounts/GetFactoryCostCenter?CostCenterID=" + FactoryCostCenterID,
            success: function (result) {
                $("#FactoryCostCenterName").val(result);
            }
        });
    }
});

function GetFactoryCCAccountsData() {

    var FactoryCostCenterID = $("#FactoryCostCenterID").val();

    var tableData = $("#CCAccountsTbl");
    tableData.html("");

    $.ajax({
        type: "GET",
        url: "/F_CreateCostCenterAccounts/GetFactoryCCAccountsData?FactoryCostCenterID=" + FactoryCostCenterID,
        success: function (result) {

            for (var i = 0; i < result.length; i++) {

                var Data = "<tr class='row_" + result[i].CostAccountID + "'>" +
                    "<td>" + result[i].CostAccountID + "</td>" +
                    "<td>" + result[i].CostAccountName + "</td>" +
                    "<td>" + '<a href="#" onclick="DeleteFactoryCCAccount(\'' + result[i].CostAccountID + '\')"><span class="fa fa-trash-o"></span></a>' + "</td>" +
                    "</tr>";
                tableData.append(Data);
            }
        }
    });

}


$("#AddCCAccountsFactoryBtn").click(function () {

    var CostCenterID = $("#FactoryCostCenterID").val();

    var CostAccountID = $("#CostAccountID").val();

    var CostAccountName = $("#CostAccountName").val();

    var Test = true;

    if (CostCenterID == "-1") {
        $("#FactoryCostCenterID").css("border-color", "red");
        Test = false;
    } else {
        $("#FactoryCostCenterID").css("border-color", "");
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
            url: "/F_CreateCostCenterAccounts/SaveRecordFactoryCCAccounts?CostCenterID=" + CostCenterID + "&CostAccountID=" + CostAccountID + "&CostAccountName=" + CostAccountName,
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

                    GetFactoryCCAccountsData();
                }
            }
        });
    }
});



// Validation Cost center Accounts Content
$("#CostAccountID").keyup(function (event) {

    event.preventDefault();

    if (event.keyCode === 13) {
        $("#AddCCAccountsFactoryBtn").click();
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

        $("#AddCCAccountsFactoryBtn").click();
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



function DeleteFactoryCCAccount(CostAccountID) {
    $("#CCAccountID").text(CostAccountID);

    $("#DeleteConfirmation").modal("show");
}



function FactoryConfirmDelete() {

    var CCAccountID = $("#CCAccountID").text();

    $.ajax({
        type: "POST",
        url: "/F_CreateCostCenterAccounts/DeleteCostAccountFactory?CCAccountID=" + CCAccountID,
        success: function (result) {
            $("#DeleteConfirmation").modal("hide");
            $(".row_" + CCAccountID).remove();
        }
    });

}