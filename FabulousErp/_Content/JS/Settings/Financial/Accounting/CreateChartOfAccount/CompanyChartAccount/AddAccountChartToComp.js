$("#CompanyID").change(function () {
    var CompanyID = $(this).val();
    $("#GlobalError").text("");
    if (CompanyID.length > 0) {
        GetCompanyChartTbl(CompanyID);
        $("#CompanyID").css("border-color", "");
        $.ajax({
            type: "GET",
            url: "/CompanyChartAccount/GetCompanyName?CompanyID=" + CompanyID,
            success: function (result) {
                $("#CompanyName").val(result);
            }
        });
    } else {
        $("#CompanyID").css("border-color", "red");
    }
});
$("#AccountChartID").change(function () {
    var AccountChartID = $(this).val();
    if (AccountChartID.length > 0) {
        $("#AccountChartID").css("border-color", "");
        $.ajax({
            type: "GET",
            url: "/CompanyChartAccount/GetChartName?ChartID=" + AccountChartID,
            success: function (result) {
                $("#ChartName").val(result);
            }
        })
    } else {
        $("#AccountChartID").css("border-color", "red");
    }
});

$("#AddChartToCompBtn").click(function () {

    var CompanyID = $("#CompanyID").val();

    var AccountChartID = $("#AccountChartID").val();

    var Test = true;

    if (CompanyID.length === 0) {
        $("#CompanyID").css("border-color", "red");
        Test = false;
    } else {
        $("#CompanyID").css("border-color", "");
    }

    if (AccountChartID.length === 0) {
        $("#AccountChartID").css("border-color", "red");
        Test = false;
    } else {
        $("#AccountChartID").css("border-color", "");
    }

    if (Test === true) {
        $.ajax({
            type: "POST",
            url: "/CompanyChartAccount/AddChartToComp?CompanyID=" + CompanyID + "&AccountChartID=" + AccountChartID,
            success: function (result) {
                if (result === "NoFY") {
                    $("#GlobalError").text("This Company ID Not Have Fiscal Year..!");
                    $("#CompanyID").css("border-color", "red");
                }
                else if (result === "False") {
                    $("#GlobalError").text("This Company ID already has Account Chart..!");
                    $("#CompanyID").css("border-color", "red");
                }
                else if (result === "NoAccess") {
                    $("#GlobalError").text("You not have any Access to This Company..!");
                    $("#CompanyID").css("border-color", "red");
                }
                else if (result === "True") {

                    GetCompanyChartTbl(CompanyID);

                    $("#GlobalError").text("");
                    $("#CompanyID").css("border-color", "");
                    $("#CompanyID").val("");
                    $("#AccountChartID").val("");
                    $("#CompanyName").val("");
                    $("#ChartName").val("");
                }
            }
        });
    }
});

function GetCompanyChartTbl(CompanyID) {

    var tblData = $("#SetChartAccountComp");

    tblData.html("");

    $.ajax({
        type: "GET",
        url: "/CompanyChartAccount/GetCompChartData?CompanyID=" + CompanyID,
        success: function (result) {
            for (var i = 0; i < result.length; i++) {

                var Data = "<tr class='row_" + result[i].CompanyID + "'>" +
                    "<td width='20%'>" + result[i].CompanyID + "</td>" +
                    "<td width='30%'>" + result[i].CompanyName + "</td>" +
                    "<td width='20%'>" + result[i].AccountChartID + "</td>" +
                    "<td width='30%'>" + result[i].AccountChartName + "</td>" +
                    "</tr>";
                tblData.append(Data);

            }
        }
    });
}