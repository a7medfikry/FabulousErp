$(document).ready(function () {
    var SetData = $("#company-append-data");
    $.ajax({
        type: "GET",
        url: "/Inquiry_CompanyAccess/GetData",
        contentType: "html",
        success: function (result) {
            if (result == "False") {
                SetData.append('<tr style="color:red"><td colspan="3"> No Result Found !!! </td></tr>')
            }
            else {
                for (var i = 0; i < result.length; i++) {
                    var Data = "<tr>" +
                        "<td>" + result[i].CompanyName + "</td>" +
                        "<td>" + result[i].UserName + "</td>" +
                        "<td>" + result[i].UserID + "</td>" +
                        "</tr>";
                    SetData.append(Data);
                }
            }
        }
    });
});


$("#InquiryCompanyID").change(function () {
    var companyID = $(this).val();
    var SetData = $("#company-append-data");
    SetData.html("");

    $.ajax({
        type: "GET",
        url: "/Inquiry_CompanyAccess/GetCompanyAccessData?companyID=" + companyID,
        contentType: "html",
        success: function (result) {
            if (result == "False") {
                SetData.append('<tr style="color:red"><td colspan="3"> No Result Found !!! </td></tr>')
            }
            else {
                for (var i = 0; i < result.length; i++) {
                    var Data = "<tr>" +
                        "<td>" + result[i].CompanyName + "</td>" +
                        "<td>" + result[i].UserName + "</td>" +
                        "<td>" + result[i].UserID + "</td>" +                   
                        "</tr>";
                    SetData.append(Data);
                }
            }
        }
    });
});