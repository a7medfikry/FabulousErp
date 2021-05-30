$(document).ready(function () {
    var SetData = $("#currency-append-data"),
        companyID = $('#InquiryCompanyID').val();

    SetData.html("");
    $.ajax({
        type: "GET",
        url: "/Inquiry_CurrencyList/GetCompanyCurrency?companyID=" + companyID,
        contentType: "html",
        success: function (result) {
            if (result == "False") {
                SetData.append('<tr style="color:red"><td colspan="4"> No Result Found !!! </td></tr>')
            }
            else {
                for (var i = 0; i < result.length; i++) {
                    var statues = "Active";
                    if (result[i].DisActive == "Checked") {
                        statues = "Dis-Active";
                    }
                    var Data = "<tr>" +
                        "<td>" + result[i].CurrencyID + "</td>" +
                        "<td>" + result[i].CurrencyName + "</td>" +
                        "<td>" + result[i].ISOCode + "</td>" +
                        "<td>" + statues + "</td>" +
                        "</tr>";
                    SetData.append(Data);
                }
            }
        },
    });
});