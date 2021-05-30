var companyIDSession = $('#IBCC-CompanyID').text(),
    branchIDSession = $('#IBCC-BranchID').text(),
    SetData = $("#branchcost-append-data");
SetData.html("");

$(document).ready(function () {
    // When Session Is Company-Login
    if (companyIDSession != null) {
        $("#InquiryBranchID").change(function () {
            var branchID = $(this).val();
            GetData(branchID);
            $.ajax({
                type: "GET",
                url: "/Inquiry_BranchCostCenter/GetBranchName?branchID=" + branchID,
                success: function (result) {
                    $("#branchname").val(result);
                }
            });
        });
        // When Session Is Branch-Login
    } if (branchIDSession != null) {
        GetData(branchIDSession);
    }
});

function GetData(branchID) {
    $.ajax({
        type: "GET",
        url: "/Inquiry_BranchCostCenter/GetData?branchID=" + branchID,
        contentType: "html",
        success: function (result) {
            SetData.empty();
            if (result == "False") {
                SetData.append('<tr style="color:red"><td colspan="2"> No Result Found !!! </td></tr>')
            }
            else {
                for (var i = 0; i < result.length; i++) {
                    var Data = "<tr>" +
                        "<td width='20%'>" + result[i].B_CostCenterID + "</td>" +
                        "<td width='80%'>" + result[i].B_CostCenterName + "</td>" +
                        "</tr>";
                    SetData.append(Data);
                }
            }
        },
    });
}