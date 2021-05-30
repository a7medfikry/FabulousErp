$("#InquiryGroupID").change(function () {
    var groupID = $(this).val();
    var SetData = $("#group-append-data");
    SetData.html("");

    $.ajax({
        type: "GET",
        url: "/Inquiry_GroupAccess/GetGroupName?groupID=" + groupID,
        success: function (result) {
            $("#GroupName").text(result);
        }
    });

    $.ajax({
        type: "GET",
        url: "/Inquiry_GroupAccess/GetData?groupID=" + groupID,
        contentType: "html",
        success: function (result) {
            if (result == "False") {
                SetData.append('<tr style="color:red"><td colspan="3"> No Result Found !!! </td></tr>')
            }
            else {
                for (var i = 0; i < result.length; i++) {
                    var Data = "<tr>" +
                        "<td>" + result[i].GroupID + "</td>" +
                        "<td>" + result[i].UserID + "</td>" +
                        "<td>" + result[i].UserName + "</td>" +
                        "</tr>";
                    SetData.append(Data);
                }
            }
        }
    });
});