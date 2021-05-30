$(document).ready(function () {
    var SetData = $("#append-data");
    $.ajax({
        type: "GET",
        url: "/Inquiry_UserData/GetData",
        contentType: "html",
        success: function (result) {
            GetuserDetails(result);
        },
        error: function (req, status, error) {
        }
    });
});

$("#InquiryuserID").change(function () {
    var InquiryuserID = $(this).val();
    var SetData = $("#append-data");
    SetData.html("");
    $.ajax({
        type: "GET",
        url: "/Inquiry_UserData/UserIDSearch?InquiryuserID=" + InquiryuserID,
        contentType: "html",
        success: function (result) {
            GetuserDetails(result);
        },
        error: function (req, status, error) {
        }
    });
});

$("#InquiryuserName").change(function () {
    var InquiryuserName = $(this).val();
    var SetData = $("#append-data");
    SetData.html("");
    $.ajax({
        type: "GET",
        url: "/Inquiry_UserData/UserNameSearch?InquiryuserName=" + InquiryuserName,
        contentType: "html",
        success: function (result) {
            GetuserDetails(result);
        },
        error: function (req, status, error) {
        }
    });
});



function GetuserDetails(result) {
    var SetData = $("#append-data");

    if (result.length == 0) {
        SetData.append('<tr style="color:red"><td> </td></tr>')
    }
    else {
        for (var i = 0; i < result.length; i++) {
            var Data = "<tr>" +
                "<td>" + result[i].UserID + "</td>" +
                "<td>" + result[i].UserName + "</td>" +
                "<td>" + result[i].Date + "</td>" +         
                "</tr>";
            SetData.append(Data);
        }
    }
}