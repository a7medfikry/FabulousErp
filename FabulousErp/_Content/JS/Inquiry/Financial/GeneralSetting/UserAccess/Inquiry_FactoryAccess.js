$(document).ready(function () {
    var SetData = $("#factory-append-data");
    $.ajax({
        type: "GET",
        url: "/Inquiry_FactoryAccess/GetData",
        contentType: "html",
        success: function (result) {
            if (result == "False") {
                SetData.append('<tr style="color:red"><td colspan="3"> No Result Found !!! </td></tr>')
            }
            else {
                for (var i = 0; i < result.length; i++) {
                    var Data = "<tr>" +
                        "<td>" + result[i].FactoryName + "</td>" +
                        "<td>" + result[i].UserName + "</td>" +
                        "<td>" + result[i].UserID + "</td>" +
                        "</tr>";
                    SetData.append(Data);
                }
            }
        }
    });
});


$("#InquiryFactoryID").change(function () {
    var factoryID = $(this).val();
    var SetData = $("#factory-append-data");
    SetData.html("");

    $.ajax({
        type: "GET",
        url: "/Inquiry_FactoryAccess/GetBranchAccessData?branchID=" + factoryID,
        contentType: "html",
        success: function (result) {
            if (result == "False") {
                SetData.append('<tr style="color:red"><td colspan="3"> No Result Found !!! </td></tr>')
            }
            else {
                for (var i = 0; i < result.length; i++) {
                    var Data = "<tr>" +
                        "<td>" + result[i].FactoryName + "</td>" +
                        "<td>" + result[i].UserName + "</td>" +
                        "<td>" + result[i].UserID + "</td>" +
                        "</tr>";
                    SetData.append(Data);
                }
            }
        }
    });
});