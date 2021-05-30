$(document).ready(function () {

    var companyID = $("#CompanyID").val();

    FilterCostCenterGroupIDComp(companyID);
    FilterCostCenterByComp(companyID);
});

function FilterCostCenterGroupIDComp(CompanyID) {
    $.ajax({
        type: "GET",
        url: "/C_UpdateMainCostCenter/FilterCostCenterGroupIDFrorComp?CompanyID=" + CompanyID,
        success: function (data) {

            $("#CompCostCenterGroupID").empty();

            if (data.length == 0) {

                $("#CompCostCenterGroupName").val("");

                $("#CompCostCenterGroupID").append($('<option/>', {
                    value: -1,
                    text: "No Group Cost Center Created in this Company!"

                })
                );

            } else {

                $("#CompCostCenterGroupName").val("");

                $("#CompCostCenterGroupID").append($('<option/>', {
                    value: -1,
                    text: "-Choose-"

                })
                );

                $.each(data, function (index, row) {

                    $("#CompCostCenterGroupID").append("<option value='" + row.CostCenterGroupID + "'>" + row.CostCenterGroupID + "</option>");

                });
            }
        }
    });
}

function FilterCostCenterByComp(CompanyID) {
    $.ajax({
        type: "GET",
        url: "/C_UpdateMainCostCenter/FilterCostCenterIDForComp?CompanyID=" + CompanyID,
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



$("#CompCostCenterGroupID").change(function () {

    var CompCostCenterGroupID = $(this).val();

    $("#AccessError").text("");
    $("#CompCostCenterGroupName").val("");
    $("#CostCenterGroupTbl").html("");

    if (CompCostCenterGroupID == "-1") {

        $("#FilterCostCenterIDComp").hide();
        $("#FilterCostCenterGlobal").hide();
        $("#CompAddBtn").hide();
        $("#CompUpdateBtn").hide();
        $(this).css("border-color", "red");
        $("#CompCostCenterGroupName").val("");
        $("#CostCenterGroupTbl").html("");
        $("#ComparePercentage").hide();

    } else {


        $(this).css("border-color", "");


        $("#CompCostCenterID").val("-1");
        $("#CompCostCenterName").val("");
        $("#Percentage").val("");

        $.ajax({
            type: "GET",
            url: "/C_UpdateMainCostCenter/GetCCGroupIDForComp?CCGroupID=" + CompCostCenterGroupID,
            success: function (result) {

                GetGroupCCDataComp(CompCostCenterGroupID);

                $("#CompCostCenterGroupName").val(result);
                $("#FilterCostCenterIDComp").show();

                $("#FilterCostCenterGlobal").show();

                $("#CompAddBtn").show();

                $("#CompUpdateBtn").show();
            }
        });
    }
});

function GetGroupCCDataComp(CompCostCenterGroupID) {

    var tableData = $("#CostCenterGroupTbl");
    tableData.html("");

    $.ajax({
        type: "GET",
        url: "/C_UpdateMainCostCenter/GetGroupCCDataComp?CompCostCenterGroupID=" + CompCostCenterGroupID,
        success: function (result) {

            for (var i = 0; i < result.length; i++) {
                var Data = "<tr class='row_" + result[i].CostCenterID + "' >" +
                    "<td class='CostCenterIDTbl' width='20%'>" + result[i].CostCenterID + "</td>" +
                    "<td width='40%'>" + result[i].CostCenterName + "</td>" +
                    "<td width='20%'> <input type='number' class='form-control form-control-sm SumPercentage' onkeydown = 'javascript: return event.keyCode == 69 ? false : true'  onkeypress = 'return (event.charCode == 8 || event.charCode == 0) ? null : event.charCode >= 48 && event.charCode <= 57' value=" + result[i].Percentage + "> </td>" +
                    "<td width='20%'>" + '<a href="#" onclick="DeleteCostCenter(\'' + result[i].CostCenterID + '\')"><span class="fa fa-trash-o"></span></a>' + "</td>" +
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

        $("#TakedPercentage").text("");
        $("#ComparePercentage").hide();

    }

    SumPercentagee();
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
            url: "/C_UpdateMainCostCenter/GetCompCostCenter?CostCenterID=" + CompCostCenterID,
            success: function (result) {
                $("#CompCostCenterName").val(result);
            }
        });
    }
});



$("#CompAddCostCenterID").click(function () {

    $("#ComparePercentage").show();

    var CostCenterID = $("#CompCostCenterID").val();

    var CostCenterName = $("#CompCostCenterName").val();

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
        $("#CompCostCenterID").css("border-color", "red");
        Test = false;
    } else {
        $("#CompCostCenterID").css("border-color", "");
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
                    "<td width='20%'>" + '<a href="#" class="btn btn-danger btn-sm" onclick="DeleteCostCenter(\'' + CostCenterID + '\')"><span class="fa fa-trash-o"></span></a>' + "</td>" +
                    "</tr>";

                tableData.append(Data);

                SumPercentagee();

            }
        }
    }
});


$("#CompUpdateCostCenterGroup").click(function () {

    var CompCostCenterGroupID = $("#CompCostCenterGroupID").val();

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

    //if (SumPercentage != "100") {
    //    $("#SaveError").text("Percentage Not Equal 100 %");
    //    Test = false;
    //} else {

    //}
    $("#SaveError").text("");
    $(document).find(".CostCenterIDTbl").each(function () {
        var value = $(this).text();
        //console.log(value);
        //var index = removedArr.remove(value);
        removedArr = $.grep(removedArr, function (e) {
            return e.CostCenterID != value;
        });
    });


    //var data1 = JSON.stringify({
    //    removedData: removedArr
    //});

    //console.log(removedArr);

    if (Test === true) {

        var orderArr = [];
        orderArr.length = 0;

        $.each($("#TestTable tbody tr"), function () {
            orderArr.push({
                C_CostCenterID: $(this).find('td:eq(0)').html(),
                C_Percentage: $(this).find('td:eq(2) input').val()
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
            url: "/C_UpdateMainCostCenter/UpdateCompCostGroup?CompCostCenterGroupID=" + CompCostCenterGroupID,
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