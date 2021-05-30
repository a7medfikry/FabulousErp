$(document).ready(function () {

    var branchIDFB = $("#BranchIDFB").val();
    if ($("#BranchIDFB").length > 0) {
        FilterCostCenterByBranch(branchIDFB);

        GetMainCostCenterID(branchIDFB);
    }

});

$("#BranchID").change(function () {

    var BranchID = $(this).val();

    if (BranchID.length > 0) {

        $(this).css("border-color", "");

        $.ajax({
            type: "GET",
            url: "/B_CreateMainCostCenter/GetBranchName?BranchID=" + BranchID,
            success: function (result) {

                if (result == "False") {
                    $("#AccessError").text("You Not Have Access To This Branch..!");
                    $("#MainCostCenterDetails").hide();
                    $("#BranchName").val("");
                    $(this).css("border-color", "red");
                } else {
                    $("#MainCostCenterDetails").show();
                    $("#BranchName").val(result);
                    FilterCostCenterByBranch(BranchID);
                    GetMainCostCenterID(BranchID);
                    $("#AccessError").text("");
                }
            }
        });

    } else {
        $("#AccessError").text("");
        $("#MainCostCenterDetails").hide();
        $("#BranchName").val("");
        $(this).css("border-color", "red");
    }
});



function FilterCostCenterByBranch(BranchID) {
    $.ajax({
        type: "GET",
        url: "/B_CreateMainCostCenter/FilterCostCenterIDForBranch?BranchID=" + BranchID,
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

function GetMainCostCenterID(branchID) {
    $.ajax({
        type: "GET",
        url: "/B_CreateMainCostCenter/GetSavedMainCC?branchID=" + branchID,
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



$("#BranchCostCenterID").change(function () {

    var BranchCostCenterID = $(this).val();

    if (BranchCostCenterID == "-1") {

        $(this).css("border-color", "red");
        $("#BranchCostCenterName").val("");

    } else {

        $(this).css("border-color", "");

        $.ajax({
            type: "GET",
            url: "/B_CreateMainCostCenter/GetBranchCostCenter?CostCenterID=" + BranchCostCenterID,
            success: function (result) {
                $("#BranchCostCenterName").val(result);
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
            url: "/B_CreateMainCostCenter/CheckDuplicateCostCenterGroupIDBranch?CostCenterGroupID=" + CostCenterGroupID,
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



$("#BranchAddCostCenterID").click(function () {

    var CostCenterGroupID = $("#CostCenterGroupID").val();

    var CostCenterGroupName = $("#CostCenterGroupName").val();

    var BranchCostCenterID = $("#BranchCostCenterID").val();

    var BranchCostCenterName = $("#BranchCostCenterName").val();

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

    if (BranchCostCenterID == "-1") {
        $("#BranchCostCenterID").css("border-color", "red");
        Test = false;
    } else {
        $("#BranchCostCenterID").css("border-color", "");
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
                $("#BranchCostCenterID").css("border-color", "");

                if (tdContent == BranchCostCenterID) {
                    testDuplicate = false;
                    $(this).css("border-color", "red");
                    $("#BranchCostCenterID").css("border-color", "red");
                }

            });

            if (testDuplicate === true) {
                $("#TestTable td").css("border-color", "");

                var productItem = "<tr class='row_" + BranchCostCenterID + "'>" +
                    "<td id='CostCenterIDTbl'>" + BranchCostCenterID + "</td>" +
                    "<td>" + BranchCostCenterName + "</td>" +
                    "<td class='SumPercentage'>" + Percentage + "</td>" +
                    "<td>" + '<a href="#" onclick="DeleteCostCenter(\'' + BranchCostCenterID + '\')"><span class="fa fa-trash-o"></span></a>' + "</td>" +
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
        $("#BranchSaveBtn").show();
    }
});

function DeleteCostCenter(CompCostCenterID) {
    $(".row_" + CompCostCenterID).remove();

    if ($("#TestTable #CostCenterIDTbl").text().length === 0) {

        $("#CostCenterGroupID").prop("disabled", false);
        $("#CostCenterGroupName").prop("disabled", false);

        $("#ComparePercentage").hide();
        $("#TakedPercentage").text("");
        $("#BranchSaveBtn").hide();
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




$("#BranchSaveCostCenterGroup").click(function () {

    var CostCenterGroupID = $("#CostCenterGroupID").val();

    var CostCenterGroupName = $("#CostCenterGroupName").val();

    var BranchID = $("#BranchID").val();

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
                B_CostCenterID: $(this).find('td:eq(0)').html(),
                B_Percentage: $(this).find('td:eq(2)').html()
            });
        });

        var data = JSON.stringify({
            order: orderArr
        });

        $.ajax({
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            type: "POST",
            url: "/B_CreateMainCostCenter/SaveBranchCostGroup?CostCenterGroupID=" + CostCenterGroupID + "&CostCenterGroupName=" + CostCenterGroupName + "&BranchID=" + BranchID,
            data: data,
            success: function (result) {
                if (result == "True") {
                    $("#SaveSuccess").show();
                    $("#SaveSuccess").text("Cost Center Group Added Successfully..");
                    $("#SaveSuccess").fadeOut(6000);
                    $("#BranchSaveBtn").hide();
                    $("#BranchCostCenterID").val("-1");
                    $("#BranchCostCenterName").val("");
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