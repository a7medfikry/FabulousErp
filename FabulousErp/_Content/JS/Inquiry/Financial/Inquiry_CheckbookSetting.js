$(document).ready(function () {
    var companyID = $('#ICCBS-CompanyID').text(),
        branchID = $('#IBCBS-BranchID').text(),
        factoryID = $('#IFCBS-FactoryID').text();


    /* [---------------------------------------------
     * Company-View
     * ---------------------------------------------] */
    // Company-View when Session Is Company-Login
    if (companyID.length > 0) {
        GetCompanyCheckbook()
    }



    /* [---------------------------------------------
     * Branch-View
     * ---------------------------------------------] */
    // Branch-View when Session Is Company-Login
    $('#InquiryBranchID').change(function () {
        var branchID = $(this).val();
        var table = $("#IBCBS-table tbody");
        GetBranchCheckbook(branchID);
    });
    // Branch-View when Session Is Branch-Login
    if (branchID.length > 0) {
        GetBranchCheckbook(branchID);
    }


    /* [---------------------------------------------
     * Factory-View
     * ---------------------------------------------] */
    // Factory-View when Session Is Company-Login OR Branch-Login
    $('#IFCBS-factoryID').change(function () {
        var factoryID = $(this).val();
        var table = $("#IFCBS-table tbody");
        GetFactoryCheckbook(factoryID);
    });
    // Factory-View when Session Is Factory-Login
    if (factoryID.length > 0) {
        GetFactoryCheckbook(factoryID);
    }




});




function GetCompanyCheckbook() {
    var table = $("#ICCBS-table tbody");
    $.ajax({
        url: '/Inquiry_CheckbookSetting/GetCompanyCheckbook',
        method: "GET",
        xhrFields: {
            withCredentials: true
        },
        success: function (result) {
            $.each(result, function (a, b) {
                table.append("<tr><td>" + b.CheckbookID + "</td>" +
                    "<td>" + b.CheckbookName + "</td>" +
                    "<td>" + b.CheckbookType + "</td>" +
                    "<td>" + b.CheckbookStatus + "</td>" +
                    "<td>" + b.CheckbookCurrency + "</td>" +
                    "<td>" + b.CheckbookAccountID + "</td></tr>");
            });
        }
    });
}
function GetBranchCheckbook(branchID) {
    var table = $("#IBCBS-table tbody");
    $.ajax({
        type: "GET",
        url: '/Inquiry_CheckbookSetting/GetBranchCheckbook?branchID=' + branchID,
        success: function (result) {
            table.empty();
            if (result.length > 0) {
                $.each(result, function (a, b) {
                    table.append("<tr><td>" + b.CheckbookID + "</td>" +
                        "<td>" + b.CheckbookName + "</td>" +
                        "<td>" + b.CheckbookType + "</td>" +
                        "<td>" + b.CheckbookStatus + "</td>" +
                        "<td>" + b.CheckbookCurrency + "</td>" +
                        "<td>" + b.CheckbookAccountID + "</td></tr>");
                    $('#IBCBS-branchName').val(b.Branch_Factory_Name);
                });
            } else {
                $('#IBCBS-branchName').val("");
                table.append("<tr><td colspan='6'>" + "There's No Any Checkbook Created In This Branch" + "</td></tr>");
            }
        }
    });
}
function GetFactoryCheckbook(factoryID) {
    var table = $("#IFCBS-table tbody");
    $.ajax({
        type: "GET",
        url: '/Inquiry_CheckbookSetting/GetFactoryCheckbook?factoryID=' + factoryID,
        success: function (result) {
            table.empty();
            if (result.length > 0) {
                $.each(result, function (a, b) {
                    table.append("<tr><td>" + b.CheckbookID + "</td>" +
                        "<td>" + b.CheckbookName + "</td>" +
                        "<td>" + b.CheckbookType + "</td>" +
                        "<td>" + b.CheckbookStatus + "</td>" +
                        "<td>" + b.CheckbookCurrency + "</td>" +
                        "<td>" + b.CheckbookAccountID + "</td></tr>");
                    $('#IFCBS-factoryName').val(b.Branch_Factory_Name);
                });
            } else {
                $('#IFCBS-factoryName').val("");
                table.append("<tr><td colspan='6'>" + "There's No Any Checkbook Created In This Factory" + "</td></tr>");
            }
        }
    });
}

