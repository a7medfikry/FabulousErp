$(document).ready(function () {

    var factoryIDFF = $("#FactoryIDFF").val();
    if ($("#FactoryIDFF").length > 0) {
        FilterCostCenterByFactory(factoryIDFF);
        FilterCostCenterGroupIDFactory(factoryIDFF);
    }

});

$("#FactoryID").change(function () {

    var FactoryID = $(this).val();

    if (FactoryID.length > 0) {

        $(this).css("border-color", "");

        $("#FilterCostCenterGlobal").hide();

        $("#CostCenterGroupTbl").html("");

        $.ajax({
            type: "GET",
            url: "/F_UpdateMainCostCenter/GetFatoryName?FactoryID=" + FactoryID,
            success: function (result) {

                if (result == "False") {
                    $("#AccessError").text("You Not Have Access To This Factory..!");
                    $("#FilterCostCenterGroupIDFactory").hide();
                    $("#FactoryName").val("");
                    $(this).css("border-color", "red");
                } else {
                    $("#AccessError").text("");
                    $("#FactoryName").val(result);
                    FilterCostCenterByFactory(FactoryID);
                    FilterCostCenterGroupIDFactory(FactoryID);
                    $("#FilterCostCenterGroupIDFactory").show();

                }
            }
        });

    } else {
        $("#AccessError").text("");
        $("#FilterCostCenterGroupIDFactory").hide();
        $("#FactoryName").val("");
        $(this).css("border-color", "red");
    }
});

function FilterCostCenterGroupIDFactory(FactoryID) {
    $.ajax({
        type: "GET",
        url: "/F_UpdateMainCostCenter/FilterCostCenterGroupIDFrorFactory?FactoryID=" + FactoryID,
        success: function (data) {

            $("#FactoryCostCenterGroupID").empty();

            if (data.length == 0) {

                $("#FactoryCostCenterGroupName").val("");

                $("#FactoryCostCenterGroupID").append($('<option/>', {
                    value: -1,
                    text: "No Group Cost Center Created in this Factory!"

                })
                );

            } else {

                $("#FactoryCostCenterGroupName").val("");

                $("#FactoryCostCenterGroupID").append($('<option/>', {
                    value: -1,
                    text: "-Choose-"

                })
                );

                $.each(data, function (index, row) {

                    $("#FactoryCostCenterGroupID").append("<option value='" + row.CostCenterGroupID + "'>" + row.CostCenterGroupID + "</option>");

                });
            }
        }
    });
}

function FilterCostCenterByFactory(FactoryID) {
    $.ajax({
        type: "GET",
        url: "/F_UpdateMainCostCenter/FilterCostCenterIDForFactory?FactoryID=" + FactoryID,
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



$("#FactoryCostCenterGroupID").change(function () {

    var FactoryCostCenterGroupID = $(this).val();

    $("#AccessError").text("");
    $("#FactoryCostCenterGroupName").val("");
    $("#CostCenterGroupTbl").html("");

    if (FactoryCostCenterGroupID == "-1") {

        $("#FilterCostCenterIDFactory").hide();
        $("#FilterCostCenterGlobal").hide();
        $("#FactoryAddBtn").hide();
        $("#FactoryUpdateBtn").hide();
        $(this).css("border-color", "red");
        $("#FactoryCostCenterGroupName").val("");
        $("#CostCenterGroupTbl").html("");
        $("#ComparePercentage").hide();

    } else {


        $(this).css("border-color", "");


        $("#FactoryCostCenterName").val("");
        $("#FactoryCostCenterID").val("-1");
        $("#Percentage").val("");

        $.ajax({
            type: "GET",
            url: "/F_UpdateMainCostCenter/GetCCGroupIDForFactory?CCGroupID=" + FactoryCostCenterGroupID,
            success: function (result) {

                $("#FactoryCostCenterGroupName").val(result);

                GetGroupCCDataFactory(FactoryCostCenterGroupID);

                $("#FilterCostCenterIDFactory").show();

                $("#FilterCostCenterGlobal").show();

                $("#FactoryAddBtn").show();

                $("#FactoryUpdateBtn").show();
            }
        });
    }
});

function GetGroupCCDataFactory(FactoryCostCenterGroupID) {

    var tableData = $("#CostCenterGroupTbl");
    tableData.html("");

    $.ajax({
        type: "GET",
        url: "/F_UpdateMainCostCenter/GetGroupCCDataFactory?FactoryCostCenterGroupID=" + FactoryCostCenterGroupID,
        success: function (result) {

            for (var i = 0; i < result.length; i++) {
                var Data = "<tr class='row_" + result[i].CostCenterID + "' >" +
                    "<td class='CostCenterIDTbl'>" + result[i].CostCenterID + "</td>" +
                    "<td>" + result[i].CostCenterName + "</td>" +
                    "<td width='20%'> <input type='number' class='form-control form-control-sm SumPercentage' onkeydown = 'javascript: return event.keyCode == 69 ? false : true'  onkeypress = 'return (event.charCode == 8 || event.charCode == 0) ? null : event.charCode >= 48 && event.charCode <= 57' value=" + result[i].Percentage + "> </td>" +
                    "<td>" + '<a href="#" onclick="DeleteCostCenter(\'' + result[i].CostCenterID + '\')"><span class="fa fa-trash-o"></span></a>' + "</td>" +
                    "</tr>";

                tableData.append(Data);

                $("#ComparePercentage").show();

                SumPercentagee();
            }

        }
    });
}


function SumPercentagee() {

    var SumPercentage = 0;

    $(".SumPercentage").each(function () {
        var value = $(this).val();
        // add only if the value is number
        if (!isNaN(value) && value.length != 0) {
            SumPercentage += parseFloat(value);
            $("#TakedPercentage").text(SumPercentage);
        }
    });
}

$("#TestTable").on("focusout", ".SumPercentage", function () {

    SumPercentagee();

}).on("keyup", ".SumPercentage", function () {

    var tblPercentage = $(this).val();

    if (tblPercentage > 100) {
        $(this).val("");
    } else if (tblPercentage === "0") {
        $(this).val("");
    }
    SumPercentagee();
});

var removedArr = [];

function DeleteCostCenter(CostCenterID) {
    $(".row_" + CostCenterID).remove();

    $("#PercentageError").text("");

    removedArr.push({
        CostCenterID: CostCenterID,
    });


    if ($("#TestTable .CostCenterIDTbl").text().length === 0) {

        console.log("aaaa");
        $("#TakedPercentage").text("");
        $("#ComparePercentage").hide();
    }

    SumPercentagee();
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
            url: "/F_UpdateMainCostCenter/GetFactoryCostCenter?CostCenterID=" + FactoryCostCenterID,
            success: function (result) {
                $("#FactoryCostCenterName").val(result);
            }
        });
    }
});



$("#FactoryAddCostCenterID").click(function () {

    $("#ComparePercentage").show();

    var CostCenterID = $("#FactoryCostCenterID").val();

    var CostCenterName = $("#FactoryCostCenterName").val();

    var Percentage = $("#Percentage").val();

    var checkGreaterPer = 0;

    var Test = true;

    $(".SumPercentage").each(function () {
        var value = $(this).val();
        // add only if the value is number
        if (!isNaN(value) && value.length != 0) {
            checkGreaterPer += parseFloat(value);
        }
    });

    if (CostCenterID == "-1") {
        $("#FactoryCostCenterID").css("border-color", "red");
        Test = false;
    } else {
        $("#FactoryCostCenterID").css("border-color", "");
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

    if (Test === true) {

        checkGreaterPer += parseFloat(Percentage);


        if (checkGreaterPer > 100) {
            $("#PercentageError").text("Percentage will be Greater than 100 %");
        } else {

            $("#PercentageError").text("");

            var testDuplicate = true;

            $("#TestTable .CostCenterIDTbl").each(function () {
                var tdContent = $(this).text();
                $(this).css("border-color", "");
                $("#CompCostCenterID").css("border-color", "");

                if (tdContent == CostCenterID) {
                    testDuplicate = false;
                    $(this).css("border-color", "red");
                    $("#CompCostCenterID").css("border-color", "red");
                }

            });

            if (testDuplicate === true) {

                $("#TestTable td").css("border-color", "");
                var tableData = $("#CostCenterGroupTbl");

                var Data = "<tr class='row_" + CostCenterID + "' >" +
                    "<td class='CostCenterIDTbl'>" + CostCenterID + "</td>" +
                    "<td>" + CostCenterName + "</td>" +
                    "<td width='20%'> <input type='number' class='form-control form-control-sm SumPercentage' onkeydown = 'javascript: return event.keyCode == 69 ? false : true'  onkeypress = 'return (event.charCode == 8 || event.charCode == 0) ? null : event.charCode >= 48 && event.charCode <= 57' value=" + Percentage + "> </td>" +
                    "<td>" + '<a href="#" class="btn btn-danger" onclick="DeleteCostCenter(\'' + CostCenterID + '\')"><span class="fa fa-trash-o"></span></a>' + "</td>" +
                    "</tr>";

                tableData.append(Data);

                SumPercentagee();

            }
        }
    }
});




$("#FactoryUpdateCostCenterGroup").click(function () {

    var FactoryCostCenterGroupID = $("#FactoryCostCenterGroupID").val();

    var SumPercentage = 0;

    var Test = true;


    $(".SumPercentage").each(function () {
        var value = $(this).val();
        // add only if the value is number
        if (!isNaN(value) && value.length != 0) {
            SumPercentage += parseFloat(value);
            $(this).css("border-color", "");
        } else {
            Test = false;
            $(this).css("border-color", "red");
        }
    });

    if (SumPercentage != "100") {
        $("#SaveError").text("Percentage Less Than 100 %");
        Test = false;
    } else {
        $("#SaveError").text("");
    }

    $(document).find(".CostCenterIDTbl").each(function () {
        var value = $(this).text();
        removedArr = $.grep(removedArr, function (e) {
            return e.CostCenterID != value;
        });
    });

    if (Test === true) {

        var orderArr = [];
        orderArr.length = 0;

        $.each($("#TestTable tbody tr"), function () {
            orderArr.push({
                F_CostCenterID: $(this).find('td:eq(0)').html(),
                F_Percentage: $(this).find('td:eq(2) input').val()
            });
        });

        var data = JSON.stringify({
            order: orderArr,
            deleted: removedArr
        });


        $.ajax({
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            type: "POST",
            url: "/F_UpdateMainCostCenter/UpdateFactoryCostGroup?FactoryCostCenterGroupID=" + FactoryCostCenterGroupID,
            data: data,
            success: function (result) {
                if (result == "True") {
                    $("#SaveSuccess").show();
                    $("#SaveSuccess").text("Cost Center Group Updated Successfully..");
                    $("#SaveSuccess").fadeOut(6000);
                    $("#PercentageError").text("");
                }
            }
        });


    }
});