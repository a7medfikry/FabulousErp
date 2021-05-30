$(document).ready(function () {

    var branchIDFB = $("#BranchIDFB").val();

    if ($("#BranchIDFB").length > 0) {
        FilterCostCenterByBranch(branchIDFB);
        FilterCostCenterGroupIDBranch(branchIDFB);
    }

});

$("#BranchID").change(function () {

    var BranchID = $(this).val();

    if (BranchID.length > 0) {

        $(this).css("border-color", "");

        $("#FilterCostCenterGlobal").hide();

        $("#CostCenterGroupTbl").html("");

        $.ajax({
            type: "GET",
            url: "/B_UpdateMainCostCenter/GetBranchName?BranchID=" + BranchID,
            success: function (result) {

                if (result == "False") {
                    $("#AccessError").text("You Not Have Access To This Branch..!");
                    $("#FilterCostCenterGroupIDBranch").hide();
                    $("#BranchName").val("");
                    $(this).css("border-color", "red");
                } else {
                    $("#AccessError").text("");
                    $("#BranchName").val(result);
                    FilterCostCenterByBranch(BranchID);
                    FilterCostCenterGroupIDBranch(BranchID);
                    $("#FilterCostCenterGroupIDBranch").show();
                }
            }
        });

    } else {
        $("#AccessError").text("");
        $("#FilterCostCenterGroupIDBranch").hide();
        $("#BranchName").val("");
        $(this).css("border-color", "red");
    }
});

function FilterCostCenterGroupIDBranch(BranchID) {
    $.ajax({
        type: "GET",
        url: "/B_UpdateMainCostCenter/FilterCostCenterGroupIDFrorBranch?BranchID=" + BranchID,
        success: function (data) {

            $("#BranchCostCenterGroupID").empty();

            if (data.length == 0) {

                $("#BranchCostCenterGroupName").val("");

                $("#BranchCostCenterGroupID").append($('<option/>', {
                    value: -1,
                    text: "No Group Cost Center Created in this Branch!"

                })
                );

            } else {

                $("#BranchCostCenterGroupName").val("");

                $("#BranchCostCenterGroupID").append($('<option/>', {
                    value: -1,
                    text: "-Choose-"

                })
                );

                $.each(data, function (index, row) {

                    $("#BranchCostCenterGroupID").append("<option value='" + row.CostCenterGroupID + "'>" + row.CostCenterGroupID + "</option>");

                });
            }
        }
    });
}

function FilterCostCenterByBranch(BranchID) {
    $.ajax({
        type: "GET",
        url: "/B_UpdateMainCostCenter/FilterCostCenterIDForBranch?BranchID=" + BranchID,
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


$("#BranchCostCenterGroupID").change(function () {

    var BranchCostCenterGroupID = $(this).val();

    $("#AccessError").text("");
    $("#BranchCostCenterGroupName").val("");
    $("#CostCenterGroupTbl").html("");

    if (BranchCostCenterGroupID == "-1") {

        $(this).css("border-color", "red");
        $("#BranchCostCenterGroupName").val("");
        $("#CostCenterGroupTbl").html("");
        $("#FilterCostCenterIDBranch").hide();
        $("#FilterCostCenterGlobal").hide();
        $("#BranchAddBtn").hide();
        $("#BranchUpdateBtn").hide();
        $("#ComparePercentage").hide();

    } else {

        $(this).css("border-color", "");

        $("#BranchCostCenterID").val("-1");
        $("#BranchCostCenterName").val("");
        $("#Percentage").val("");

        $.ajax({
            type: "GET",
            url: "/B_UpdateMainCostCenter/GetCCGroupIDForBranch?CCGroupID=" + BranchCostCenterGroupID,
            success: function (result) {

                $("#BranchCostCenterGroupName").val(result);

                GetGroupCCDataBranch(BranchCostCenterGroupID);

                $("#FilterCostCenterIDBranch").show();

                $("#FilterCostCenterGlobal").show();

                $("#BranchAddBtn").show();

                $("#BranchUpdateBtn").show();
            }
        });
    }
});

function GetGroupCCDataBranch(BranchCostCenterGroupID) {

    var tableData = $("#CostCenterGroupTbl");
    tableData.html("");

    $.ajax({
        type: "GET",
        url: "/B_UpdateMainCostCenter/GetGroupCCDataBranch?BranchCostCenterGroupID=" + BranchCostCenterGroupID,
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



$("#BranchCostCenterID").change(function () {

    var BranchCostCenterID = $(this).val();

    if (BranchCostCenterID == "-1") {

        $(this).css("border-color", "red");
        $("#BranchCostCenterName").val("");

    } else {

        $(this).css("border-color", "");

        $.ajax({
            type: "GET",
            url: "/B_UpdateMainCostCenter/GetBranchCostCenter?CostCenterID=" + BranchCostCenterID,
            success: function (result) {
                $("#BranchCostCenterName").val(result);
            }
        });
    }
});



$("#BranchAddCostCenterID").click(function () {

    $("#ComparePercentage").show();

    var CostCenterID = $("#BranchCostCenterID").val();

    var CostCenterName = $("#BranchCostCenterName").val();

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
        $("#BranchCostCenterID").css("border-color", "red");
        Test = false;
    } else {
        $("#BranchCostCenterID").css("border-color", "");
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



$("#BranchUpdateCostCenterGroup").click(function () {

    var BranchCostCenterGroupID = $("#BranchCostCenterGroupID").val();

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
                B_CostCenterID: $(this).find('td:eq(0)').html(),
                B_Percentage: $(this).find('td:eq(2) input').val()
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
            url: "/B_UpdateMainCostCenter/UpdateBranchCostGroup?BranchCostCenterGroupID=" + BranchCostCenterGroupID,
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