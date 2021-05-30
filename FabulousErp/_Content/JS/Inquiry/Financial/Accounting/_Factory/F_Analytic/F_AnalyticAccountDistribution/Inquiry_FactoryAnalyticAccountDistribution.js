$(document).ready(function () {
    $("#IFADA-factoryID").change(function () {
        "use strict"
        var factoryID = $(this).val();

        if (factoryID.length === 0) {
            $("#IFADA-factoryname").val("");
            $("#IFADA-errors").text("");
            $("#IFADA-FactoryAnalyticID").prop("disabled", true);
            ClearInputs();
        }
        else {
            // ajax to get Factory-Name
            $.ajax({
                type: "GET",
                url: "/Inquiry_FactoryAnalyticDistribution/GetFactoryName?factoryID=" + factoryID,
                success: function (result) {
                    $("#IFADA-factoryname").val(result);
                }
            });

            // ajax to get 'Analytic-Accounts-ID' linked to 'Factory-ID' 
            $.ajax({
                type: "GET",
                url: "/Inquiry_FactoryAnalyticDistribution/GetAnalyticAccountID?factoryID=" + factoryID,
                success: function (result) {
                    if (result == "False") {
                        $("#IFADA-FactoryAnalyticID").prop("disabled", true);
                        $("#IFADA-errors").text("You do not have access to this factory !!");
                        ClearInputs();
                    } else {
                        $("#IFADA-FactoryAnalyticID").empty(); // clear old
                        $("#IFADA-FactoryAnalyticID").append($('<option/>', {
                            value: "",
                            text: "-Choose-"
                        })
                        );
                        $.each(result, function (index, row) {
                            $("#IFADA-FactoryAnalyticID").append("<option value='" + row.Analytic_AccountID + "'>" + row.Analytic_AccountID + "</option>");
                        });

                        // Enable Analytic-Accounts-ID drop-down
                        $("#IFADA-FactoryAnalyticID").prop("disabled", false);
                        $("#IFADA-errors").text("");
                    }
                }
            });
        }
    });
    ///////////////////////////////////////////////////////////////////////////////

    $("#IFADA-FactoryAnalyticID").change(function () {
        var SetData = $("#IFADA-append-data");
        var AnalyticID = $(this).val();

        // ajax to get Analytic-Name
        $.ajax({
            type: "GET",
            url: "/Inquiry_FactoryAnalyticDistribution/GetAnalyticAccountName?AnalyticID=" + AnalyticID,
            success: function (result) {
                $("#IFADA-analyticname").val(result);
            }
        });

        // ajax to fill tbody table
        SetData.html("");
        $.ajax({
            type: "GET",
            url: "/Inquiry_FactoryAnalyticDistribution/GetData?AnalyticID=" + AnalyticID,
            contentType: "html",
            success: function (result) {
                if (result.length == 0) {
                    SetData.append('<tr style="color:red"><td colspan="2"> No Result Found !!! </td></tr>')
                }
                else {
                    for (var i = 0; i < result.length; i++) {
                        var Data = "<tr>" +
                            "<td width='30%'>" + result[i].AnalyticDistribution_ID + "</td>" +
                            "<td width='70%'>" + result[i].AnalyticDistribution_Name + "</td>" +
                            "</tr>";
                        SetData.append(Data);
                    }
                }
            },
        });

    });


});


function ClearInputs() {
    $("#IFADA-FactoryAnalyticID").val("");
    $("#IFADA-analyticname").val("");
    $("#IFADA-append-data").html("");
}

