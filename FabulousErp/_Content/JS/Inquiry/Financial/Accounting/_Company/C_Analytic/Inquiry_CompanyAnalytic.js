$(document).ready(function () {
    var companyID = $('#InquiryCompanyID').val(),
        SetData = $("#companyanalytic-append-data");
    SetData.html("");
    $.ajax({
        type: "GET",
        url: "/Inquiry_CompanyAnalytic/GetData?companyID=" + companyID,
        contentType: "html",
        success: function (result) {
            if (result == "False") {
                SetData.append('<tr style="color:red"><td colspan="2"> No Result Found !!! </td></tr>')
            }
            else {
                for (var i = 0; i < result.length; i++) {
                    var Data = "<tr>" +
                        "<td>" + result[i].C_AnalyticAccountID + "</td>" +
                        "<td>" + result[i].C_AnalyticAccountName + "</td>" +
                        "</tr>";
                    SetData.append(Data);
                }
            }
        },
    });
});

