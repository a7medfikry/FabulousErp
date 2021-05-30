$("#InquiryFactoryID").change(function () {
    var SetData = $("#factorycost-append-data");
    var factoryID = $(this).val();
    SetData.html("");
    $.ajax({
        type: "GET",
        url: "/Inquiry_FactoryCostCenter/GetData?factoryID=" + factoryID,
        contentType: "html",
        success: function (result) {
            if (result == "False") {
                SetData.append('<tr style="color:red"><td colspan="2"> No Result Found !!! </td></tr>')
            }
            else {
                for (var i = 0; i < result.length; i++) {
                    var Data = "<tr>" +
                        "<td width='30%'>" + result[i].F_CostCenterID + "</td>" +
                        "<td width='70%'>" + result[i].F_CostCenterName + "</td>" +
                        "</tr>";
                    SetData.append(Data);
                }
            }
        },
    });

    $.ajax({
        type: "GET",
        url: "/Inquiry_FactoryCostCenter/GetFactoryName?factoryID=" + factoryID,
        success: function (result) {
            $("#factoryname").val(result);
        }
    });


});

