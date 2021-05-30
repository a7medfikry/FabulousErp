$("#InquiryUserID").change(function () {
    var userID = $(this).val();
    var SetData = $("#formsaccess-append-data");
    SetData.html("");

    $.ajax({
        type: "GET",
        url: "/Inquiry_FormsAccess/GetFormsAccess?userID=" + userID,
        contentType: "html",
        success: function (result) {
            if (result == "False") {
                SetData.append('<tr style="color:red"><td colspan="2"> No Result Found !!! </td></tr>')
            }
            else {
                for (var i = 0; i < result.length; i++) {
                    var Data = "<tr>" +
                        "<td>" + result[i].UserID + "</td>" +
                        "<td>" + result[i].FormCode + "</td>" +
                        "</tr>";
                    SetData.append(Data);
                }
            }
        }
    });


    $.ajax({
        type: "GET",
        url: "/Inquiry_FormsAccess/GetUserName?userID=" + userID,
        success: function (result) {
            $("#UserName").text(result);
        }
    });



});