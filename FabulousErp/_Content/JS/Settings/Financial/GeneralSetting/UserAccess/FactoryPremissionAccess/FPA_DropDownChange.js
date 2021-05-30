$("#UserID").change(function () {

    var UserID = $(this).val();

    if (UserID.length > 0) {

        $.ajax({
            type: "GET",
            url: "/FactoryPremissionAccess/GetUserName?UserID=" + UserID,
            success: function (data) {
                $("#UserName").val(data);
            }
        });

        $("#CompanyID").prop("disabled", false);
        
        var SetData = $("#SetUserList");

        SetData.html("");
        $.ajax({

            type: "GET",
            url: "/FactoryPremissionAccess/GetUserFactoryAccess?UserID=" + UserID,
            contentType: "html",
            success: function (result) {

                if (result.length == 0) {
                    SetData.append('<tr style="color:red"><td colspan = "4" > This User have not any access To any Factory  </td></tr>')
                }
                else {
                    $.each(result, function (index, value) {
                        var Data = "<tr class='row_" + value.ID + "' >" +
                            "<td width='15%'>" + value.UserID + "</td>" +
                            "<td width='35%'>" + value.UserName + "</td>" +
                            "<td width='15%'>" + value.FactoryID + "</td>" +
                            "<td width='35%'>" + value.FactoryName + "</td>"
                        "</tr>";
                        SetData.append(Data);
                    });
                }
            },
            error: function (req, status, error) {

            }
        });
        

    } else {
        $("#CompanyID").prop("disabled", true);
        var SetData = $("#SetUserList");
        SetData.html("");
        $("#UserName").val("");
        $("#UserAlreadyExistError").text("");
        $("#BranchID").empty();
        $("#FactoryID").empty();
        $("#FactoryName").val("");
        $("#CompanyID").val("");
        $("#CompanyName").val("");
        $("#BarnchName").val("");
        $("#SaveBtn").hide();
    }
});


$("#CompanyID").change(function () {

    var CompanyID = $(this).val();

    if (CompanyID.length > 0) {
        $("#UserAlreadyExistError").text("");
        $.ajax({
            type: "GET",
            url: "/FactoryPremissionAccess/GetCompanyName?CompanyID=" + CompanyID,
            success: function (data) {

                $("#CompanyName").val(data);

                $.ajax({
                    type: "GET",
                    url: "/FactoryPremissionAccess/FilterBranchID?CompanyID=" + CompanyID,
                    success: function (data) {

                        $("#BranchID").empty();

                        if (data.length == 0) {

                            $("#SaveBtn").hide();

                            $("#BranchName").text("");

                            $("#BranchID").append($('<option/>', {
                                value: -1,
                                text: "No Branch Created In this Company!"

                            })
                            );

                        }
                        else {

                            $("#BarnchName").val("");
                            $("#SaveBtn").hide();
                            $("#UserAlreadyExistError").text("");

                            $("#BranchID").append($('<option/>', {
                                value: -1,
                                text: "---Choose---"

                            })
                            );

                            $.each(data, function (index, row) {

                                $("#BranchID").append("<option value='" + row.BranchID + "'>" + row.BranchID + "</option>");

                            });
                        }
                    },
                    error: function (req, status, error) {

                    }
                });


                $.ajax({
                    type: "GET",
                    url: "/FactoryPremissionAccess/FilterFactoryID2?CompanyID=" + CompanyID,
                    success: function (data) {

                        $("#FactoryID").empty();

                        if (data.length == 0) {

                            // $("#SaveBtn").hide();

                            $("#FactoryName").val("");

                            $("#FactoryID").append($('<option/>', {
                                value: -1,
                                text: "No Factory Created Directly To this Company!"

                            })
                            );

                        }
                        else {

                            $("#FactoryName").val("");
                            //$("#SaveBtn").hide();
                            $("#UserAlreadyExistError").text("");

                            $("#FactoryID").append($('<option/>', {
                                value: -1,
                                text: "---Choose---"

                            })
                            );

                            $.each(data, function (index, row) {

                                $("#FactoryID").append("<option value='" + row.FactoryID + "'>" + row.FactoryID + "</option>");

                            });
                        }
                    },
                    error: function (req, status, error) {

                    }
                });

            }
        });
    } else {
        $("#UserAlreadyExistError").text("");
        $("#BranchID").empty();
        $("#FactoryID").empty();
        $("#CompanyName").val("");
        $("#FactoryName").val("");
        $("#BarnchName").val("");
        $("#SaveBtn").hide();
    }
});


$("#BranchID").change(function () {

    var BranchID = $(this).val();
    var CompanyID = $("#CompanyID").val();

    if (BranchID == "-1") {
        $("#BarnchName").val("");
        $("#SaveBtn").hide();
        $("#UserAlreadyExistError").text("");
        $.ajax({
            type: "GET",
            url: "/FactoryPremissionAccess/FilterFactoryID2?CompanyID=" + CompanyID,
            success: function (data) {

                $("#FactoryID").empty();

                if (data.length == 0) {

                    // $("#SaveBtn").hide();

                    $("#FactoryName").val("");

                    $("#FactoryID").append($('<option/>', {
                        value: -1,
                        text: "No Factory Created Directly To this Company!"

                    })
                    );

                }
                else {

                    $("#FactoryName").val("");
                    //$("#SaveBtn").hide();
                    $("#UserAlreadyExistError").text("");

                    $("#FactoryID").append($('<option/>', {
                        value: -1,
                        text: "---Choose---"

                    })
                    );

                    $.each(data, function (index, row) {

                        $("#FactoryID").append("<option value='" + row.FactoryID + "'>" + row.FactoryID + "</option>");

                    });
                }
            },
            error: function (req, status, error) {

            }
        });

    } else {
        $("#UserAlreadyExistError").text("");
        $.ajax({
            type: "GET",
            url: "/FactoryPremissionAccess/GetBranchName?BranchID=" + BranchID,
            success: function (data) {
                $("#BarnchName").val(data);
                $("#SaveBtn").hide();

                $.ajax({
                    type: "GET",
                    url: "/FactoryPremissionAccess/FilterFactoryID?BranchID=" + BranchID + "&CompanyID=" + CompanyID,
                    success: function (data) {

                        $("#FactoryID").empty();

                        if (data.length == 0) {

                            // $("#SaveBtn").hide();

                            $("#FactoryName").val("");

                            $("#FactoryID").append($('<option/>', {
                                value: -1,
                                text: "No Factory Created In this Branch that Follow This Company!"

                            })
                            );

                        }
                        else {

                            $("#FactoryName").val("");
                            //$("#SaveBtn").hide();
                            $("#UserAlreadyExistError").text("");

                            $("#FactoryID").append($('<option/>', {
                                value: -1,
                                text: "---Choose---"

                            })
                            );

                            $.each(data, function (index, row) {

                                $("#FactoryID").append("<option value='" + row.FactoryID + "'>" + row.FactoryID + "</option>");

                            });
                        }
                    },
                    error: function (req, status, error) {

                    }
                });

            }
        });

    }

});


$("#FactoryID").change(function () {

    var FactoryID = $(this).val();

    if (FactoryID == "-1") {
        $("#BarnchName").val("");
        $("#SaveBtn").hide();
        $("#UserAlreadyExistError").text("");
    } else
    {
        $("#UserAlreadyExistError").text("");
        $.ajax({
            type: "GET",
            url: "/FactoryPremissionAccess/GetFactoryName?FactoryID=" + FactoryID,
            success: function (data) {
                $("#FactoryName").val(data);
                $("#SaveBtn").show("slow");
            }
        });
    }

});

$(document).ready(function () {

    var UserID = $("#UserID").val();

    if (UserID.length == 0) {
        $("#CompanyID").prop("disabled", true);
    } else {
        $("#CompanyID").prop("disabled", false);
    }

});
