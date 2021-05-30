$(document).ready(function () {
    var companyID = $('#ICADA-companyID').val();

    // ajax to get 'Analytic-Accounts-ID' linked to 'Company-ID' 
    $.ajax({
        type: "GET",
        url: "/Inquiry_CompanyAnalyticDistribution/GetAnalyticAccountID?companyID=" + companyID,
        success: function (result) {
            if (result == "False") {
                $("#ICADA-CompAnalyticID").prop("disabled", true);
                $("#ICADA-errors").text("You do not have access to this company !!");
                ClearInputs();
            } else {
                $("#ICADA-CompAnalyticID").empty();
                $("#ICADA-CompAnalyticID").append($('<option/>', {
                    value: "",
                    text: ChooseTxt
                })
                );
                $.each(result, function (index, row) {
                    $("#ICADA-CompAnalyticID").append("<option value='" + row.Analytic_AccountID + "'>" + row.Analytic_AccountID + "</option>");
                });

                // Enable Analytic-Accounts-ID drop-down
                $("#ICADA-CompAnalyticID").prop("disabled", false);
                $("#ICADA-errors").text("");
            }
        }
    });

    $("#ICADA-CompAnalyticID").change(function () {
        var SetData = $("#ICADA-append-data");
        var AnalyticID = $(this).val();

        // ajax to get Analytic-Name
        $.ajax({
            type: "GET",
            url: "/Inquiry_CompanyAnalyticDistribution/GetAnalyticAccountName?AnalyticID=" + AnalyticID,
            success: function (result) {
                $("#ICADA-analyticname").val(result);
            }
        });

        // ajax to fill tbody table
        SetData.html("");
        $.ajax({
            type: "GET",
            url: "/Inquiry_CompanyAnalyticDistribution/GetData?AnalyticID=" + AnalyticID,
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
    $("#ICADA-CompanyAnalyticID").val("");
    $("#ICADA-analyticname").val("");
    $("#ICADA-append-data").html("");
}
