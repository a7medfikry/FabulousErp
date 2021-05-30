$(document).ready(function () {
    var companyID = $('#InquiryCompanyID').val(),
        SetData = $("#companycost-append-data");
    SetData.html("");
    $.ajax({
        type: "GET",
        url: "/Inquiry_CompanyCostCenter/GetData?companyID=" + companyID,
        contentType: "html",
        success: function (result) {
            if (result == "False") {
                SetData.append('<tr style="color:red"><td colspan="2"> No Result Found !!! </td></tr>')
            }
            else {
                for (var i = 0; i < result.length; i++) {
                    var Data = "<tr>" +
                        "<td>" + result[i].C_CostCenterID + "</td>" +
                        "<td>" + result[i].C_CostCenterName + "</td>" +
                        "</tr>";
                    SetData.append(Data);
                }
            }
        },
    });
});