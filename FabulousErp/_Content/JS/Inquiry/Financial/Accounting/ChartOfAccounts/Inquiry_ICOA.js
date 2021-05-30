$(document).ready(function () {
    // Get All Data On Page Load
    // Company View
    var companytbody = $("#ICCOA-companytbody");
    $.ajax({
        url: '/Inquiry_ICCOA/GetCompanyData',
        method: "GET",
        xhrFields: {
            withCredentials: true
        },
        success: function (result) {
            companytbody.empty();
            $.each(result, function (a, b) {
                companytbody.append("<tr><td>" + b.AccountID + "</td>" +
                    "<td>" + b.AccountName + "</td>" +
                    "<td>" + b.AccountType + "</td>" +
                    "<td>" + b.AccountGroup + "</td>" +
                    "<td>" + b.PostingType + "</td></tr>");
            });
            ReTranslate();
        }
    });
    $('#ICCOA-companySort').change(function () {
        var sortValue = $(this).val();
        $.ajax({
            type: "GET",
            url: "/Inquiry_ICCOA/GetCompanyData?sortValue=" + sortValue,
            success: function (result) {
                companytbody.empty();
                $.each(result, function (a, b) {
                    companytbody.append("<tr><td>" + b.AccountID + "</td>" +
                        "<td>" + b.AccountName + "</td>" +
                        "<td>" + b.AccountType + "</td>" +
                        "<td>" + b.AccountGroup + "</td>" +
                        "<td>" + b.PostingType + "</td></tr>");
                });
                ReTranslate();
            }
        });
    });


    // Branch View
    $('#IBCOA-branchID').change(function () {
        var branchtbody = $("#IBCOA-branchtbody");
        var branchID = $(this).val();
        // Get Branch-Name
        $.ajax({
            type: "GET",
            url: "/Inquiry_IBCOA/GetBranchName?branchID=" + branchID,
            success: function (result) {
                $('#IBCOA-branchName').val(result);
            }
        });

        // Get Chart-Of-Accounts Related to Branch-ID
        $.ajax({
            type: "GET",
            url: "/Inquiry_IBCOA/GetBranchData?branchID=" + branchID,
            success: function (result) {
                branchtbody.empty();
                $.each(result, function (a, b) {
                    branchtbody.append("<tr><td>" + b.AccountID + "</td>" +
                        "<td>" + b.AccountName + "</td>" +
                        "<td>" + b.AccountType + "</td>" +
                        "<td>" + b.AccountGroup + "</td>" +
                        "<td>" + b.PostingType + "</td></tr>");
                });
            }
        });
    });
    $('#IBCOA-branchSort').change(function () {
        var branchtbody = $("#IBCOA-branchtbody");
        var sortValue = $(this).val();
        var branchID = $('#IBCOA-branchID').val();
        $.ajax({
            type: "GET",
            url: "/Inquiry_IBCOA/GetBranchData?branchID=" + branchID + "&sortValue=" + sortValue,
            success: function (result) {
                branchtbody.empty();
                $.each(result, function (a, b) {
                    branchtbody.append("<tr><td>" + b.AccountID + "</td>" +
                        "<td>" + b.AccountName + "</td>" +
                        "<td>" + b.AccountType + "</td>" +
                        "<td>" + b.AccountGroup + "</td>" +
                        "<td>" + b.PostingType + "</td></tr>");
                });
            }
        });
    });



    // Factory View
    $('#IFCOA-factoryID').change(function () {
        var factorytbody = $("#IFCOA-factorytbody");
        var factoryID = $(this).val();
        // Get Factory-Name
        $.ajax({
            type: "GET",
            url: "/Inquiry_IFCOA/GetFactoryName?factoryID=" + factoryID,
            success: function (result) {
                $('#IFCOA-factoryName').val(result);
            }
        });

        // Get Chart-Of-Accounts Related to Factory-ID
        $.ajax({
            type: "GET",
            url: "/Inquiry_IFCOA/GetFactoryData?factoryID=" + factoryID,
            success: function (result) {
                factorytbody.empty();
                $.each(result, function (a, b) {
                    factorytbody.append("<tr><td>" + b.AccountID + "</td>" +
                        "<td>" + b.AccountName + "</td>" +
                        "<td>" + b.AccountType + "</td>" +
                        "<td>" + b.AccountGroup + "</td>" +
                        "<td>" + b.PostingType + "</td></tr>");
                });
            }
        });
    });
    $('#IFCOA-factorySort').change(function () {
        var factorytbody = $("#IFCOA-factorytbody");
        var sortValue = $(this).val();
        var factoryID = $('#IFCOA-factoryID').val();
        $.ajax({
            type: "GET",
            url: "/Inquiry_IFCOA/GetFactoryData?factoryID=" + factoryID + "&sortValue=" + sortValue,
            success: function (result) {
                factoryID.empty();
                $.each(result, function (a, b) {
                    factoryID.append("<tr><td>" + b.AccountID + "</td>" +
                        "<td>" + b.AccountName + "</td>" +
                        "<td>" + b.AccountType + "</td>" +
                        "<td>" + b.AccountGroup + "</td>" +
                        "<td>" + b.PostingType + "</td></tr>");
                });
            }
        });
    });

    // Export to Excel
    $('#btnExport').click(function () {
        $('#example').table2excel({
            exclude: ".noExl",
            name: "Chart Of Accounts",
            filename: "Chart Of Accounts"
        });
    });


});