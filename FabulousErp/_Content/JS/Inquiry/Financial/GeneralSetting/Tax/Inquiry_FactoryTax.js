$(document).ready(function () {
    $("#IFT-factoryID").change(function () {
        // ajax to get Factory-Name
        var factoryID = $(this).val();
        $.ajax({
            type: "GET",
            url: "/Inquiry_FactoryTax/GetFactoryName?factoryID=" + factoryID,
            success: function (result) {
                $("#IFT-factoryname").val(result);
            }
        });

        // ajax to get Data
        var setData = $("#IFT-tbody-append");
        setData.html("");
        $.ajax({
            type: "GET",
            url: "/Inquiry_FactoryTax/GetData?factoryID=" + factoryID,
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