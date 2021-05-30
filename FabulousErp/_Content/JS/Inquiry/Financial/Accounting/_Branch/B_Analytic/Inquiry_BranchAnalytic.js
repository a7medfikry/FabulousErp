/*----------------
 * Some Variables
 * ------------- */
var SetData = $("#branchanalytic-append-data"),
    companyIDSession = $('#IBAA-CompanyID').text(),
    branchIDSession = $('#IBAA-BranchID').text();
SetData.html("");

$(document).ready(function () {
    // When Session Is Company-Login
    if (companyIDSession != null) {
        $("#InquiryBranchID").change(function () {
            var branchID = $(this).val();
            // Function to Get-Data 
            GetData(branchID);

            // ajax to Get-Branch-Name
            $.ajax({
                type: "GET",
                url: "/Inquiry_BranchAnalytic/GetBranchName?branchID=" + branchID,
                success: function (result) {
                    $("#branchname").val(result);
                }
            });
        });
    }

    // When Session Is Branch-Login
    if (branchIDSession != null) {
        var branchID = $('#InquiryBranchID').val();
        GetData(branchID);
    }
});

function GetData(branchID) {
    $.ajax({
        type: "GET",
        url: "/Inquiry_BranchAnalytic/GetData?branchID=" + branchID,
        contentType: "html",
        success: function (result) {
            SetData.empty();
            if (result == "False") {
                SetData.append('<tr style="color:red"><td colspan="2"> No Result Found !!! </td></tr>')
            }
            else {
                for (var i = 0; i < result.length; i++) {
                    var Data = "<tr>" +
                        "<td width='20%'>" + result[i].B_AnalyticAccountID + "</td>" +
                        "<td width='80%'>" + result[i].B_AnalyticAccountName + "</td>" +
                        "</tr>";
                    SetData.append(Data);
                }
            }
        },
    });

}