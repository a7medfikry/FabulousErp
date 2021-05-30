$(document).ready(function () {

    var factoryIDFF = $("#FactoryIDFF").val();
    if ($("#FactoryIDFF").length > 0) {
        FilterCostCenterByFactory(factoryIDFF);
        GetMainCostCenterID(factoryIDFF);
    }

});

$("#FactoryID").change(function () {

    var FactoryID = $(this).val();

    if (FactoryID.length > 0) {

        $(this).css("border-color", "");

        $.ajax({
            type: "GET",
            url: "/F_CreateMainCostCenter/GetFatoryName?FactoryID=" + FactoryID,
            success: function (result) {

                if (result == "False") {
                    $("#AccessError").text("You Not Have Access To This Factory..!");
                    $("#MainCostCenterDetails").hide();
                    $("#FactoryName").val("");
                    $(this).css("border-color", "red");
                } else {
                    $("#AccessError").text("");
                    $("#FactoryName").val(result);
                    FilterCostCenterByFactory(FactoryID);
                    GetMainCostCenterID(FactoryID);
                    $("#MainCostCenterDetails").show();
                }
            }
        });

    } else {
        $("#AccessError").text("");
        $("#MainCostCenterDetails").hide();
        $("#FactoryName").val("");
        $(this).css("border-color", "red");
    }
});


function FilterCostCenterByFactory(FactoryID) {
    $.ajax({
        type: "GET",
        url: "/F_CreateMainCostCenter/FilterCostCenterIDForFactory?FactoryID=" + FactoryID,
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

function GetMainCostCenterID(factoryID) {
    $.ajax({
        type: "GET",
        url: "/F_CreateMainCostCenter/GetSavedMainCC?factoryID=" + factoryID,
        success: function (data) {
            $("#rCosrCenterGroupID").empty();

            if (data.length == 0) {

                $("#rCosrCenterGroupID").append($('<option/>', {
                    value: "",
                    text: "No Cost Center Group Created in this Branch!"
                })
                );

            } else {

                $("#rCosrCenterGroupID").append($('<option/>', {
                    value: "",
                    text: "Old Main"

                })
                );

                $.each(data, function (index, row) {

                    $("#rCosrCenterGroupID").append("<option value='" + row.MainCostCenterID + "' disabled>" + row.MainCostCenterID + " ( " + row.MainCostCenterName + " )" + "</option>");

                });
            }
        }
    });
}


$("#FactoryCostCenterID").change(function () {

    var FactoryCostCenterID = $(this).val();

    if (FactoryCostCenterID == "-1") {

        $(this).css("border-color", "red");
        $("#FactoryCostCenterName").val("");

    } else {

        $(this).css("border-color", "");

        $.ajax({
            type: "GET",
            url: "/F_CreateMainCostCenter/GetFactoryCostCenter?CostCenterID=" + FactoryCostCenterID,
            success: function (result) {
                $("#FactoryCostCenterName").val(result);
            }
        });
    }
});



$("#CostCenterGroupID").keyup(function () {

    var CostCenterGroupID = $(this).val();

    if (CostCenterGroupID.length === 0) {
        $(this).css("border-color", "red");
    } else {
        $(this).css("border-color", "");
    }
}).focusout(function () {

    var CostCenterGroupID = $(this).val();

    if (CostCenterGroupID.length === 0) {
        $(this).css("border-color", "red");
    } else {

        $.ajax({
            type: "GET",
            url: "/F_CreateMainCostCenter/CheckDuplicateCostCenterGroupIDFactory?CostCenterGroupID=" + CostCenterGroupID,
            success: function (result) {
                if (result == "False") {
                    $("#CostCenterGroupID").focus();
                    $("#CostCenterGroupID").css("border-color", "red");
                    $("#GlobalError").text("This Cost Center Group ID not Available..!");
                } else {
                    $("#CostCenterGroupID").css("border-color", "");
                    $("#GlobalError").text("");
                }
            }
        });
    }
});

$("#CostCenterGroupName").keyup(function () {

    var CostCenterGroupName = $(this).val();

    if (CostCenterGroupName.length === 0) {
        $(this).css("border-color", "red");
    } else {
        $(this).css("border-color", "");
    }
}).focusout(function () {

    var CostCenterGroupName = $(this).val();

    if (CostCenterGroupName.length === 0) {
        $(this).css("border-color", "red");
    } else {
        $(this).css("border-color", "");
    }
});

$("#Percentage").keyup(function () {

    var Percentage = $(this).val();

    if (Percentage.length === 0) {
        $(this).css("border-color", "red");
    } else {
        $(this).css("border-color", "");
    }
}).focusout(function () {

    var Percentage = $(this).val();

    if (Percentage.length === 0) {
        $(this).css("border-color", "red");
    } else {
        $(this).css("border-color", "");
    }
    });




$("#FactoryAddCostCenterID").click(function () {

    var CostCenterGroupID = $("#CostCenterGroupID").val();

    var CostCenterGroupName = $("#CostCenterGroupName").val();

    var FactoryCostCenterID = $("#FactoryCostCenterID").val();

    var FactoryCostCenterName = $("#FactoryCostCenterName").val();

    var Percentage = $("#Percentage").val();

    var SumPercentage = 0;

    var checkGreaterPer = 0;

    var Test = true;

    $(".SumPercentage").each(function () {
        var value = $(this).text();
        // add only if the value is number
        if (!isNaN(value) && value.length != 0) {
            checkGreaterPer += parseFloat(value);
        }
    });

    if (CostCenterGroupID.length === 0) {
        $("#CostCenterGroupID").css("border-color", "red");
        Test = false;
    } else {
        $("#CostCenterGroupID").css("border-color", "");
    }

    if (CostCenterGroupName.length === 0) {
        $("#CostCenterGroupName").css("border-color", "red");
        Test = false;
    } else {
        $("#CostCenterGroupName").css("border-color", "");
    }

    if (Percentage.length === 0) {
        $("#Percentage").css("border-color", "red");
        Test = false;
    } else if (Percentage == "0") {
        $("#Percentage").css("border-color", "red");
        Test = false;
    } else {
        $("#Percentage").css("border-color", "");
    }

    if (FactoryCostCenterID == "-1") {
        $("#FactoryCostCenterID").css("border-color", "red");
        Test = false;
    } else {
        $("#FactoryCostCenterID").css("border-color", "");
    }

    if (Test === true) {

        checkGreaterPer += parseFloat(Percentage);

        if (checkGreaterPer > 100) {
            $("#PercentageError").text("Percentage will be Greater than 100 %");
        } else {

            $("#PercentageError").text("");

            var testDuplicate = true;

            $("#TestTable #CostCenterIDTbl").each(function () {
                var tdContent = $(this).text();
                $(this).css("border-color", "");
                $("#FactoryCostCenterID").css("border-color", "");

                if (tdContent == FactoryCostCenterID) {
                    testDuplicate = false;
                    $(this).css("border-color", "red");
                    $("#BranchCostCenterID").css("border-color", "red");
                }

            });

            if (testDuplicate === true) {
                $("#TestTable td").css("border-color", "");

                var productItem = "<tr class='row_" + FactoryCostCenterID + "'>" +
                    "<td id='CostCenterIDTbl'>" + FactoryCostCenterID + "</td>" +
                    "<td>" + FactoryCostCenterName + "</td>" +
                    "<td class='SumPercentage'>" + Percentage + "</td>" +
                    "<td>" + '<a href="#" onclick="DeleteCostCenter(\'' + FactoryCostCenterID + '\')"><span class="fa fa-trash-o"></span></a>' + "</td>" +
                    "</tr>";

                $("#CostCenterGroupTbl").append(productItem);

                SumPercentagee(SumPercentage);
            }
        }
    }

    if ($("#TestTable #CostCenterIDTbl").text().length > 0) {
        $("#CostCenterGroupID").prop("disabled", true);
        $("#CostCenterGroupName").prop("disabled", true);

        $("#ComparePercentage").show();
        $("#FactorySaveBtn").show();
    }

});

function DeleteCostCenter(CompCostCenterID) {
    $(".row_" + CompCostCenterID).remove();

    if ($("#TestTable #CostCenterIDTbl").text().length === 0) {

        $("#CostCenterGroupID").prop("disabled", false);
        $("#CostCenterGroupName").prop("disabled", false);

        $("#ComparePercentage").hide();
        $("#TakedPercentage").text("");
        $("#FactorySaveBtn").hide();
    }

    $("#PercentageError").text("");

    var SumPercentage = 0;

    SumPercentagee(SumPercentage);
}

function SumPercentagee(SumPercentage) {

    $(".SumPercentage").each(function () {
        var value = $(this).text();
        // add only if the value is number
        if (!isNaN(value) && value.length != 0) {
            SumPercentage += parseFloat(value);
            $("#TakedPercentage").text(SumPercentage);
        }
    });
}




$("#FactorySaveCostCenterGroup").click(function () {

    var CostCenterGroupID = $("#CostCenterGroupID").val();

    var CostCenterGroupName = $("#CostCenterGroupName").val();

    var FactoryID = $("#FactoryID").val();

    var SumPercentage = 0;

    var Test = true;


    $(".SumPercentage").each(function () {
        var value = $(this).text();
        // add only if the value is number
        if (!isNaN(value) && value.length != 0) {
            SumPercentage += parseFloat(value);
        }
    });

    if (SumPercentage != "100") {
        $("#SaveError").text("Percentage Less Than 100 %");
        Test = false;
    } else {
        $("#SaveError").text("");
    }


    if (Test === true) {

        var orderArr = [];
        orderArr.length = 0;

        $.each($("#TestTable tbody tr"), function () {
            orderArr.push({
                F_CostCenterID: $(this).find('td:eq(0)').html(),
                F_Percentage: $(this).find('td:eq(2)').html()
            });
        });

        var data = JSON.stringify({
            order: orderArr
        });

        $.ajax({
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            type: "POST",
            url: "/F_CreateMainCostCenter/SaveFactoryCostGroup?CostCenterGroupID=" + CostCenterGroupID + "&CostCenterGroupName=" + CostCenterGroupName + "&FactoryID=" + FactoryID,
            data: data,
            success: function (result) {
                if (result == "True") {
                    $("#SaveSuccess").show();
                    $("#SaveSuccess").text("Cost Center Group Added Successfully..");
                    $("#SaveSuccess").fadeOut(6000);
                    $("#FactorySaveBtn").hide();
                    $("#FactoryCostCenterID").val("-1");
                    $("#FactoryCostCenterName").val("");
                    $("#Percentage").val("");
                    $("#CostCenterGroupTbl").html("");
                    $("#CostCenterGroupID").prop("disabled", false);
                    $("#CostCenterGroupName").prop("disabled", false);
                    $("#CostCenterGroupID").val("");
                    $("#CostCenterGroupName").val("");
                    $("#ComparePercentage").hide();
                    $("#PercentageError").text("");

                }
            }
        });
    }
});