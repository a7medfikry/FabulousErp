$(document).ready(function () {
    $("#ICT-companyID").change(function () {

        // ajax to get Company-Name
        var companyID = $(this).val();
        $.ajax({
            type: "GET",
            url: "/Inquiry_CompanyTax/GetCompanyName?companyID=" + companyID,
            success: function (result) {
                $("#ICT-companyname").val(result);
            }
        });

        // ajax to get Data
        var setData = $("#ICT-tbody-append");
        setData.html("");
        $.ajax({
            type: "GET",
            url: "/Inquiry_CompanyTax/GetData?companyID=" + companyID,
            contentType: "html",
            success: function (result) {
                if (result.length === 0) {
                    setData.append('<tr style="color:red"><td colspan="4"> No Result Found !!! </td></tr>')
                }
                else {
                    for (var i = 0; i < result.length; i++) {
                        var Data = "<tr>" +
                            "<td>" + result[i].TaxCode + "</td>" +
                            "<td>" + result[i].AccountID + "</td>" +
                            "<td>" + result[i].TaxDescribtion + "</td>" +
                            "<td>" + result[i].TaxPercentage + "</td>" +
                            "</tr>";
                        setData.append(Data);
                    }
                }
            },
        });
    });
});