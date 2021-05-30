$(document).ready(function () {
    // ajax to get Company-Name
    $("#ITS-companyID").change(function () {
        TaxSchdule();
    });
});
function TaxSchdule() {
    var companyID = $("#ITS-companyID").val();
    $.ajax({
        type: "GET",
        url: "/Inquiry_TaxSchedule/GetCompanyName?companyID=" + companyID,
        success: function (result) {
            $("#ITS-companyname").val(result);
        }
    });

    // ajax to get Data
    var setData = $("#ITS-tbody-append");
    setData.html("");
    $.ajax({
        type: "GET",
        url: "/Inquiry_TaxSchedule/GetData?companyID=" + companyID,
        contentType: "html",
        success: function (result) {
            if (result.length === 0) {
                setData.append('<tr style="color:red"><td colspan="4"> No Result Found !!! </td></tr>')
            }
            else {
                for (var i = 0; i < result.length; i++) {
                    var Data = "<tr>" +
                        "<td>" + result[i].TaxCode + "</td>" +
                        "<td>" + result[i].TaxType + "</td>" +
                        "<td>" + result[i].TaxDescribtion + "</td>" +
                        "<td>" + result[i].TaxPercentage + "</td>" +
                        "</tr>";
                    setData.append(Data);
                }
                ReTranslate();
            }
        },
    });
}