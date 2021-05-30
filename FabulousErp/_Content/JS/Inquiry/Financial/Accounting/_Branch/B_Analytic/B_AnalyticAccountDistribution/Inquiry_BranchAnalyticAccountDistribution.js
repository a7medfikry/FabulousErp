var companyIDSession = $('#IBADA-CompanyID').text(),
    branchIDSession = $('#IBADA-BranchID').text();

$(document).ready(function () {

    // When Session Is Company-Login
    if (companyIDSession.length > 0) {
        $("#InquiryBranchID").change(function () {
            "use strict"
            var branchID = $(this).val();

            if (branchID.length === 0) {
                $("#IBADA-branchname").val("");
                $("#IBADA-errors").text("");
                $("#IBADA-BranchAnalyticID").prop("disabled", true);
                ClearInputs();
            }
            else {
                // ajax to get Branch-Name
                $.ajax({
                    type: "GET",
                    url: "/Inquiry_BranchAnalyticDistribution/GetBranchName?branchID=" + branchID,
                    success: function (result) {
                        $("#IBADA-branchname").val(result);
                    }
                });

                // ajax to get 'Analytic-Accounts-ID' linked to 'Branch-ID' 
                GetAnalyticAccountsID(branchID);
            }
        });
        GetAnalyticData();
    }

    // When Session Is Branch-Login
    if (branchIDSession.length > 0) {
        GetAnalyticData();
        GetAnalyticAccountsID(branchIDSession);
    }

});


function ClearInputs() {
    $("#IBADA-BranchAnalyticID").val("");
    $("#IBADA-analyticname").val("");
    $("#IBADA-append-data").html("");
}
function GetAnalyticData() {
    $("#IBADA-BranchAnalyticID").change(function () {
        var SetData = $("#IBADA-append-data");
        var AnalyticID = $(this).val();

        // ajax to get Analytic-Name
        $.ajax({
            type: "GET",
            url: "/Inquiry_BranchAnalyticDistribution/GetAnalyticAccountName?AnalyticID=" + AnalyticID,
            success: function (result) {
                $("#IBADA-analyticname").val(result);
            }
        });

        // ajax to fill tbody table
        SetData.html("");
        $.ajax({
            type: "GET",
            url: "/Inquiry_BranchAnalyticDistribution/GetData?AnalyticID=" + AnalyticID,
            contentType: "html",
            success: function (result) {
                if (result.length === 0) {
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

}
function GetAnalyticAccountsID(branchID) {
    $.ajax({
        type: "GET",
        url: "/Inquiry_BranchAnalyticDistribution/GetAnalyticAccountID?branchID=" + branchID,
        success: function (result) {
            if (result === "False") {
                $("#IBADA-BranchAnalyticID").prop("disabled", true);
                $("#IBADA-errors").text("You do not have access to this branch !!");
                ClearInputs();
            } else {
                $("#IBADA-BranchAnalyticID").empty(); // clear old
                $("#IBADA-BranchAnalyticID").append($('<option/>', {
                    value: "",
                    text: "-Choose-"
                })
                );
                $.each(result, function (index, row) {
                    $("#IBADA-BranchAnalyticID").append("<option value='" + row.Analytic_AccountID + "'>" + row.Analytic_AccountID + "</option>");
                });

                // Enable Analytic-Accounts-ID drop-down
                $("#IBADA-BranchAnalyticID").prop("disabled", false);
                $("#IBADA-errors").text("");
            }
        }
    });
}

