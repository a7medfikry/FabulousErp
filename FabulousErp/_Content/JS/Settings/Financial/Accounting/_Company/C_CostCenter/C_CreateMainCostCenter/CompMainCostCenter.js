$(document).ready(function () {

    var companyID = $("#CompanyID").val();

    FilterCostCenterByComp(companyID);

    GetMainCostCenterID(companyID)

});


function FilterCostCenterByComp(CompanyID) {
    $.ajax({
        type: "GET",
        url: "/C_CreateMainCostCenter/FilterCostCenterIDForComp?CompanyID=" + CompanyID,
        success: function (data) {

            $("#CompCostCenterID").empty();

            if (data.length == 0) {

                $("#CompCostCenterName").val("");

                $("#CompCostCenterID").append($('<option/>', {
                    value: -1,
                    text: "No Cost Center Created in this Company!"

                })
                );

            } else {

                $("#CompCostCenterName").val("");

                $("#CompCostCenterID").append($('<option/>', {
                    value: -1,
                    text: "-Choose-"

                })
                );

                $.each(data, function (index, row) {

                    $("#CompCostCenterID").append("<option value='" + row.CostCenterID + "'>" + row.CostCenterID + "</option>");

                });
            }
        }
    });
}

function GetMainCostCenterID(companyID) {
    $.ajax({
        type: "GET",
        url: "/C_CreateMainCostCenter/GetSavedMainCC?companyID=" + companyID,
        success: function (data) {
            $("#rCosrCenterGroupID").empty();

            if (data.length == 0) {

                $("#rCosrCenterGroupID").append($('<option/>', {
                    value: "",
                    text: "No Cost Center Group Created in this Company!"
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


$("#CompCostCenterID").change(function () {

    var CompCostCenterID = $(this).val();

    if (CompCostCenterID == "-1") {

        $(this).css("border-color", "red");
        $("#CompCostCenterName").val("");

    } else {

        $(this).css("border-color", "");

        $.ajax({
            type: "GET",
            url: "/C_CreateMainCostCenter/GetCompCostCenter?CostCenterID=" + CompCostCenterID,
            success: function (result) {
                $("#CompCostCenterName").val(result);
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
            url: "/C_CreateMainCostCenter/CheckDuplicateCostCenterGroupIDComp?CostCenterGroupID=" + CostCenterGroupID,
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



$("#CompAddCostCenterID").click(function () {

    var CostCenterGroupID = $("#CostCenterGroupID").val();

    var CostCenterGroupName = $("#CostCenterGroupName").val();

    var CompCostCenterID = $("#CompCostCenterID").val();

    var CompCostCenterName = $("#CompCostCenterName").val();

    var Percentage = $("#Percentage").val();

    var TakedPercentage = $("#TakedPercentage").text();

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

    if (!Percentage) {
        Percentage = 0;
    }
    //if (Percentage.length === 0) {
    //    $("#Percentage").css("border-color", "red");
    //    Test = false;
    //} else if (Percentage == "0") {
    //    $("#Percentage").css("border-color", "red");
    //    Test = false;
    //} else {
    //    $("#Percentage").css("border-color", "");
    //}

    if (CompCostCenterID == "-1") {
        $("#CompCostCenterID").css("border-color", "red");
        Test = false;
    } else {
        $("#CompCostCenterID").css("border-color", "");
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
                $("#CompCostCenterID").css("border-color", "");

                if (tdContent == CompCostCenterID) {
                    testDuplicate = false;
                    $(this).css("border-color", "red");
                    $("#CompCostCenterID").css("border-color", "red");
                }

            });

            if (testDuplicate === true) {
                $("#TestTable td").css("border-color", "");

                var productItem = "<tr class='row_" + CompCostCenterID.replace(" ","_") + "'>" +
                    "<td id='CostCenterIDTbl' width='20%'>" + CompCostCenterID + "</td>" +
                    "<td width='40%'>" + CompCostCenterName + "</td>" +
                    "<td class='SumPercentage' width='20%'>" + Percentage + "</td>" +
                    "<td width='20%'>" + '<a href="#" onclick="DeleteCostCenter(\'' + CompCostCenterID + '\')"><span class="fa fa-trash-o"></span></a>' + "</td>" +
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
        $("#CompSaveBtn").show();
    }
});

function DeleteCostCenter(CompCostCenterID) {
    $(".row_" + CompCostCenterID.replace(" ", "_")).remove();

    if ($("#TestTable #CostCenterIDTbl").text().length === 0) {

        $("#CostCenterGroupID").prop("disabled", false);
        $("#CostCenterGroupName").prop("disabled", false);

        $("#ComparePercentage").hide();
        $("#TakedPercentage").text("");
        $("#CompSaveBtn").hide();
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



$("#CompSaveCostCenterGroup").click(function () {

    var CostCenterGroupID = $("#CostCenterGroupID").val();

    var CostCenterGroupName = $("#CostCenterGroupName").val();

    var CompanyID = $("#CompanyID").val();

    var SumPercentage = 0;

    var Test = true;


    $(".SumPercentage").each(function () {
        var value = $(this).text();
        // add only if the value is number
        if (!isNaN(value) && value.length != 0) {
            SumPercentage += parseFloat(value);
        }
    });

    //if (SumPercentage != "100") {
    //    $("#SaveError").text("Percentage Less Than 100 %");
    //    Test = false;
    //} else {
    //    $("#SaveError").text("");
    //}


    if (Test === true) {

        var orderArr = [];
        orderArr.length = 0;

        $.each($("#TestTable tbody tr"), function () {
            orderArr.push({
                C_CostCenterID: $(this).find('td:eq(0)').html(),
                C_Percentage: $(this).find('td:eq(2)').html()
            });
        });

        var data = JSON.stringify({
            order: orderArr
        });

        $.ajax({
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            type: "POST",
            url: "/C_CreateMainCostCenter/SaveCompCostGroup?CostCenterGroupID=" + CostCenterGroupID + "&CostCenterGroupName=" + CostCenterGroupName + "&CompanyID=" + CompanyID,
            data: data,
            success: function (result) {
                if (result == "True") {
                    $("#SaveSuccess").show();
                    $("#SaveSuccess").text("Cost Center Group Added Successfully..");
                    $("#SaveSuccess").fadeOut(6000);
                    $("#CompSaveBtn").hide();
                    $("#CompCostCenterID").val("-1");
                    $("#CompCostCenterName").val("");
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