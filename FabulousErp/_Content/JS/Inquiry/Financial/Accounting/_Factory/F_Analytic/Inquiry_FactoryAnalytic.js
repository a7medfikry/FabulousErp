$("#InquiryFactoryID").change(function () {
    var SetData = $("#factoryanalytic-append-data");
    var factoryID = $(this).val();
    SetData.html("");
    $.ajax({
        type: "GET",
        url: "/Inquiry_FactoryAnalytic/GetData?factoryID=" + factoryID,
        contentType: "html",
        success: function (result) {
            if (result == "False") {
                SetData.append('<tr style="color:red"><td colspan="2"> No Result Found !!! </td></tr>')
            }
            else {
                for (var i = 0; i < result.length; i++) {
                    var Data = "<tr>" +
                        "<td width='20%'>" + result[i].F_AnalyticAccountID + "</td>" +
                        "<td width='80%'>" + result[i].F_AnalyticAccountName + "</td>" +
                        "</tr>";
                    SetData.append(Data);
                }
            }
        },
    });

    $.ajax({
        type: "GET",
        url: "/Inquiry_FactoryAnalytic/GetFactoryName?factoryID=" + factoryID,
        success: function (result) {
            $("#factoryname").val(result);
        }
    });


});

