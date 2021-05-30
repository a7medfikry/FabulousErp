$(document).ready(function () {

    $("#GroupID").change(function () {

        var groupID = $(this).val();
        var SetData = $("#SetUserList");
        SetData.html("");
        $("#GroupDisactiveError").text("");
        $("#UserID option").show();

        $.ajax({
            type: "get",
            url: "/AccountGroupInfo/GetGroupInfo?GroupID=" + groupID,
            success: function (result) {

                if (result === false) {
                    $("#GroupDisactiveError").text("This Group is Disactive you can't Add Another Users To it.. Please Contact System Admin");

                    $("#Popupbtn").hide();
                } else {
                    $("#Popupbtn").show();
                    $("#GroupName").val(result.R_Group_Info_DTO.GroupName);

                    $("#CreationGroupDate").val(result.R_Group_Info_DTO.CreationGroupDate);

                    $("#FromCBF").text(result.R_Group_Info_DTO.FromCBF);

                    $(result.UserInfos).each(function (i, res) {

                        var Data = "<tr class='row_" + res.UserID + "'>" +
                            "<td>" + res.UserID + "</td>" +
                            "<td>" + res.UserName + "</td>" +
                            "<td>" + res.Date + "</td>" +
                            "<td>" + '<a href="#" onclick="DeleteFromGroup(\'' + res.UserID + '\')"><span class="fa fa-trash-o"></span></a>' + "</td>" +
                            "</tr>";
                        SetData.append(Data);

                    });

                }
            }
        });

    });

    $("#UserID").change(function () {

        var UserID = $(this).val(),
            FromCBF = $("#FromCBF").text();

        $("#UserAccessError").text("");
        $("#UserName").text("");
        $('#SaveRecord').prop('disabled', false);

        if (UserID.length > 0) {
            $.ajax({
                type: "GET",
                url: "/AccountGroupInfo/GetUserName?UserID=" + UserID + "&FromCBF=" + FromCBF,
                success: function (data) {
                    if (data === "AFalse") {
                        $("#UserAccessError").text("This User Not Have Access To Any Company");
                        $('#SaveRecord').prop('disabled', true);
                    } else if (data === "BFalse") {
                        $("#UserAccessError").text("This User Not Have Access To Any Branch");
                        $('#SaveRecord').prop('disabled', true);
                    } else if (data === "CFalse") {
                        $("#UserAccessError").text("This User Not Have Access To Any Factory");
                        $('#SaveRecord').prop('disabled', true);
                    } else {
                        $("#UserName").text(data);
                    }
                }
            });
        }

    });

    $('#SaveRecord').click(function () {

        var UserID = $("#UserID").val(),
            GroupID = $("#GroupID").val(),
            FromCBF = $("#FromCBF").text();

        $.ajax({
            type: "Post",
            url: "/AccountGroupInfo/AddUserToGroupWithCheck?UserID=" + UserID + "&GroupID=" + GroupID + "&FromCBF=" + FromCBF,
            success: function (result) {
                if (result === true) {

                    ViewData();

                    $("#MyModal").modal("hide");

                } else {
                    if (confirm("This User Exist In Another Group ID : " + result.GroupID + " With Name : " + result.GetGName + " Do You Want To Transfer It")) {

                        $.ajax({
                            type: "POST",
                            url: "/AccountGroupInfo/AddUserToGroupWithoutCheck?UserID=" + UserID + "&GroupID=" + GroupID + "&FromCBF=" + FromCBF,
                            success: function (result) {
                                if (result === true) {
                                    ViewData();

                                    $("#MyModal").modal("hide");
                                }
                            }

                        });

                    }
                }
            }
        });

    });

});

function AddUserToGroup() {

    $("#UserName").text("");
    $("#UserAccessError").text("");

    $('#SetUserList tr').each(function () {

        var userIDTD = $(this).find('td').eq(0).text();
        $("#UserID option[value=" + userIDTD + "]").hide();

    });

    $("#form")[0].reset();
    $("#ModalTitle").html("Add user to Account Group");
    $('#MyModal').modal('show');

}

function ViewData() {

    var GroupID = $("#GroupID").val();

    var SetData = $("#SetUserList");
    SetData.html("");
    $.ajax({

        type: "GET",
        url: "/AccountGroupInfo/GetGroupContent?GroupID=" + GroupID,
        success: function (result) {

            for (var i = 0; i < result.length; i++) {
                var Data = "<tr class='row_" + result[i].UserID + "'>" +
                    "<td>" + result[i].UserID + "</td>" +
                    "<td>" + result[i].UserName + "</td>" +
                    "<td>" + result[i].Date + "</td>" +
                    "<td>" + '<a href="#" onclick="DeleteFromGroup(\'' + result[i].UserID + '\')"><span class="fa fa-trash-o"></span></a>' + "</td>" +
                    "</tr>";
                SetData.append(Data);
            }
        }
    });

}

function DeleteFromGroup(UserID) {
    $("#UserIDH").text(UserID);
    $("#DeleteConfirmation").modal("show");
}

function ConfirmDelete() {

    var UserID = $("#UserIDH").text(),
        FromCBF = $("#FromCBF").text();

    $.ajax({
        type: 'delete',
        url: '/AccountGroupInfo/DeleteFromGroup?UserID=' + UserID + "&FromCBF=" + FromCBF,
        success: function (result) {
            if (result === true) {
                ViewData();

                $("#DeleteConfirmation").modal("hide");
            }
        }
    });

}



















//$("#UserID").change(function () {

//    var UserID = $(this).val();
//    $("#UserAccessError").text("");

//    if (UserID == "-1") {
//        $("#UserName").text("");
//    } else {

//        $.ajax({

//            type: "GET",
//            url: "/AccountGroupInfo/GetUserName?UserID=" + UserID,
//            success: function (data) {

//                $("#UserName").text(data);

//            },
//            error: function (req, status, error) {

//            }
//        });

//    }

//});


//$("#GroupID").change(function () {

//    var GroupID = $(this).val();


//    console.log(GroupID);

//    $("#UserName").text("");
//    $("#UserAccessError").text("");

//    if (GroupID.length > 0) {

//        $.ajax({
//            type: "GET",
//            url: "/AccountGroupInfo/FilterUserID?GroupID=" + GroupID,
//            success: function (data) {
//                $("#UserID").empty();

//                if (data.length == 0) {

//                    $("#UserName").text("");

//                    $("#UserID").append($('<option/>', {
//                        value: -1,
//                        text: "Empty.. All User In this Group OR Not Exist Users Yet..!"

//                    })
//                    );

//                }
//                else {

//                    $("#UserID").append($('<option/>', {
//                        value: -1,
//                        text: "---Choose---"

//                    })
//                    );

//                    $.each(data, function (index, row) {

//                        $("#UserID").append("<option value='" + row.UserID + "'>" + row.UserID + "</option>");

//                    });
//                }
//            },
//            error: function (req, status, error) {

//            }
//        });


//        $.ajax({

//            type: "GET",
//            url: "/AccountGroupInfo/GetGroupInfo?GroupID=" + GroupID,
//            success: function (data) {

//                $("#GroupName").val(data.GroupName);

//                $("#CreationGroupDate").val(data.CreationGroupDate);

//                $("#FromCBF").text(data.FromCBF);

//            },
//            error: function (req, status, error) {

//            }
//        });

//        ViewData();


//        $.ajax({

//            type: "GET",
//            url: "/AccountGroupInfo/CheckGroupActive?GroupID=" + GroupID,
//            success: function (data) {
//                if (data === "True") {

//                    $("#GroupDisactiveError").show();

//                    $("#GroupDisactiveError").text("This Group is Disactive you can't Add Another Users To it.. Please Contact System Admin");

//                    $("#Popupbtn").hide();

//                }
//                else if (data === "False") {

//                    $("#GroupDisactiveError").hide();

//                    $("#Popupbtn").show();

//                }
//            }
//        });
//    }
//    else {
//        SetData.html("");
//        $("#Popupbtn").hide();
//        $("#GroupName").val("");
//        $("#GroupDisactiveError").hide();
//        $("#CreationGroupDate").val("");
//    }




//});

//function AddUserToGroup() {


//    var UserID = $("#UserID").val();

//    var GroupID = $("#GroupID").val();

//    $("#UserName").text("");
//    $("#UserAccessError").text("");

//    $.ajax({
//        type: "GET",
//        url: "/AccountGroupInfo/FilterUserID?GroupID=" + GroupID,
//        success: function (data) {
//            $("#UserID").empty();

//            if (data.length == 0) {

//                $("#UserName").text("");

//                $("#UserID").append($('<option/>', {
//                    value: -1,
//                    text: "Empty.. All User In this Group OR Not Exist Users Yet..!"

//                })
//                );

//            }
//            else {

//                $("#UserID").append($('<option/>', {
//                    value: -1,
//                    text: "---Choose---"

//                })
//                );

//                $.each(data, function (index, row) {

//                    $("#UserID").append("<option value='" + row.UserID + "'>" + row.UserID + "</option>");

//                });
//            }
//        },
//        error: function (req, status, error) {

//        }
//    });

//    $("#form")[0].reset();
//    $("#ModalTitle").html("Add user to Account Group");
//    $("#MyModal").modal();
//}

//$("#SaveRecord").click(function () {

//    var UserID = $("#UserID").val();
//    var GroupID = $("#GroupID").val();
//    var FromCBF = $("#FromCBF").text();

//    if (UserID == "-1") { }

//    else {
//        $.ajax({
//            type: "POST",
//            url: "/AccountGroupInfo/CheckIfUserInAnotherGroup?UserID=" + UserID + "&GroupID=" + GroupID + "&FromCBF=" + FromCBF,
//            success: function (data) {

//                if (data.Message == "Exist") {

//                    if (confirm("This User Exist In Another Group ID : " + data.GroupID + " With Name : " + data.GetGName + " Do You Want To Transfer It") == true) {


//                        $.ajax({
//                            type: "POST",
//                            url: "/AccountGroupInfo/AssignUserWithoutCheck?UserID=" + UserID + "&GroupID=" + GroupID,
//                            success: function (data) {
//                                ViewData();
//                            }
//                        });

//                    } else { }
//                }
//                else if (data === "AFalse") {
//                    $("#UserAccessError").text("This User Not Have Access To Any Company");
//                } else if (data === "BFalse") {
//                    $("#UserAccessError").text("This User Not Have Access To Any Branch");
//                } else if (data === "CFalse") {
//                    $("#UserAccessError").text("This User Not Have Access To Any Factory");
//                } else {
//                    $("#UserAccessError").text("");
//                    ViewData();

//                    $("#MyModal").modal("hide");
//                }


//            },
//            error: function (req, status, error) {

//            }
//        });
//    }
//});

//function DeleteFromGroup(UserID) {

//    $("#UserIDH").text(UserID);
//    $("#DeleteConfirmation").modal("show");
//    console.log(UserID);
//}

//function ConfirmDelete() {
//    var UserID = $("#UserIDH").text();
//    var GroupID = $("#GroupID").val();
//    console.log(UserID);
//    $.ajax({
//        type: "POST",
//        url: "/AccountGroupInfo/DeleteFromGroup?UserID=" + UserID,
//        success: function (data) {
//            $("#DeleteConfirmation").modal("hide");

//            ViewData()

//            $.ajax({
//                type: "GET",
//                url: "/AccountGroupInfo/FilterUserID?GroupID=" + GroupID,
//                success: function (data) {
//                    $("#UserID").empty();

//                    if (data.length == 0) {

//                        $("#UserID").append($('<option/>', {
//                            value: -1,
//                            text: "Empty.. All User In this Group OR Not Exist Users Yet..!"

//                        })
//                        );

//                    }
//                    else {

//                        $.each(data, function (index, row) {

//                            $("#UserID").append("<option value='" + row.UserID + "'>" + row.UserID + "</option>");

//                        });
//                    }
//                },
//                error: function (req, status, error) {

//                }
//            });

//        }
//    });
//}

//function ViewData() {

//    var GroupID = $("#GroupID").val();

//    var SetData = $("#SetUserList");
//    SetData.html("");
//    $.ajax({

//        type: "GET",
//        url: "/AccountGroupInfo/GetGroupContent?GroupID=" + GroupID,
//        success: function (result) {

//            if (result.length == 0) {
//                SetData.append('<tr style="color:red"><td colspan = "4" > No Users In this Group </td></tr>')
//            }
//            else {
//                for (var i = 0; i < result.length; i++) {
//                    var Data = "<tr class='row_" + result[i].UserID + "'>" +
//                        "<td>" + result[i].UserID + "</td>" +
//                        "<td>" + result[i].UserName + "</td>" +
//                        "<td>" + result[i].DateOfAssignGroup + "</td>" +
//                        "<td>" + '<a href="#" onclick="DeleteFromGroup(\'' + result[i].UserID + '\')"><span class="fa fa-trash-o"></span></a>' + "</td>" +
//                        "</tr>";
//                    SetData.append(Data);
//                }
//            }
//        },
//        error: function (req, status, error) {

//        }
//    });

//}
