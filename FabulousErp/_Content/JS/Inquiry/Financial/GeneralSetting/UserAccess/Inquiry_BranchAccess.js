$(document).ready(function () {
    var SetData = $("#company-append-data");
    $.ajax({
        type: "GET",
        url: "/Inquiry_BranchAccess/GetData",
        contentType: "html",
        success: function (result) {
            if (result == "False") {
                SetData.append('<tr style="color:red"><td colspan="3"> No Result Found !!! </td></tr>')
            }
            else {
                for (var i = 0; i < result.length; i++) {
                    var Data = "<tr>" +
                        "<td>" + result[i].BranchName + "</td>" +
                        "<td>" + result[i].UserName + "</td>" +
                        "<td>" + result[i].UserID + "</td>" +
                        "</tr>";
                    SetData.append(Data);
                }
            }
        }
    });
});


$("#InquiryBranchID").change(function () {
    var branchID = $(this).val();
    var SetData = $("#branch-append-data");
    SetData.html("");

    $.ajax({
        type: "GET",
        url: "/Inquiry_BranchAccess/GetBranchAccessData?branchID=" + branchID,
        contentType: "html",
        success: function (result) {
            if (result == "False") {
                SetData.append('<tr style="color:red"><td colspan="3"> No Result Found !!! </td></tr>')
            }
            else {
                for (var i = 0; i < result.length; i++) {
                    var Data = "<tr>" +
                        "<td>" + result[i].BranchName + "</td>" +
                        "<td>" + result[i].UserName + "</td>" +
                        "<td>" + result[i].UserID + "</td>" +
                        "</tr>";
                    SetData.append(Data);
                }
            }
        }
    });
});