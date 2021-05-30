$(document).ready(function () {

    var factoryIDFF = $("#FactoryID").val();
    if (factoryIDFF.length > 0) {
        GetFactoryCost(factoryIDFF);
    }

    $("#FactoryID").change(function () {

        var FactoryID = $(this).val();

        $("#GlobalError").text("");
        $("#FactoryCostData").html("");

        if (FactoryID.length > 0) {

            $(this).css("border-color", "");

            $.ajax({
                type: "GET",
                url: "/F_CreateCostCenter/GetFactoryName?FactoryID=" + FactoryID,
                success: function (result) {

                    if (result == "False") {
                        $("#FactoryName").val("");
                        $("#AddCostCenterToFactory").prop("disabled", true);
                        $("#GlobalError").text("You Not Have Access To This Factory..!");
                    } else {
                        $("#FactoryName").val(result);
                        $("#AddCostCenterToFactory").prop("disabled", false);
                        GetFactoryCost(FactoryID);
                    }
                }
            });

        } else {
            $("#AddCostCenterToFactory").prop("disabled", false);
            $(this).css("border-color", "red");
            $("#FactoryName").val("");
        }
    });

    $("#CostCenterID").keyup(function (event) {

        event.preventDefault();

        if (event.keyCode === 13) {
            $("#AddCostCenterToFactory").click();
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
            $("#AddCostCenterToFactory").click();
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


    $("#AddCostCenterToFactory").click(function () {

        var FactoryID = $("#FactoryID").val();

        var CostCenterID = $("#CostCenterID").val();

        var CostCenterName = $("#CostCenterName").val();

        var Test = true;

        if (FactoryID.length === 0) {
            $("#FactoryID").css("border-color", "red");
            Test = false;
        } else {
            $("#FactoryID").css("border-color", "");
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
                url: "/F_CreateCostCenter/SaveFactoryCostCenter?FactoryID=" + FactoryID + "&CostCenterID=" + CostCenterID + "&CostCenterName=" + CostCenterName,
                success: function (result) {

                    if (result == "False") {
                        $("#GlobalError").text("Not Valid Cost Center ID..!");
                        $("#CostCenterID").css("border-color", "red");
                        $("#SaveSuccess").text("");
                    }
                    else {
                        $("#GlobalError").text("");
                        $("#CostCenterID").css("border-color", "");
                        $("#CostCenterID").val("");
                        $("#CostCenterName").val("");
                        $("#CostCenterID").focus();
                        $("#SaveSuccess").text("Saved..");
                        GetFactoryCost(FactoryID);
                    }
                }
            });
        }
    });

});

function GetFactoryCost(factoryID) {

    var tble = $("#FactoryCostData");

    tble.html("");

    $.ajax({
        type: "GET",
        url: "/F_CreateCostCenter/GetCostCenter?factoryID=" + factoryID,
        success: function (result) {
            if (result.length === 0) {
                tble.append("<tr><td colspan='2' class='text-danger'>No Cost Center Created To This Factory..!</td></tr>")
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


