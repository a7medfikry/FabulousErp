/*----------[Master-JavaScript Index]-----------
 * 1.0 Inquiry-Company_Cost_Center (ICCCA)
 * 2.0 Inquiry-Branch_Cost_Center (IBCCA)
 * 3.0 Inquiry-Factory_Cost_Center (IFCCA)
----------------------------------------------*/
var companyIDSession = $('#IBCCA-CompanyID').text(),
    branchIDSession = $('#IBCCA-BranchID').text();

$(document).ready(function () {

    /*----------------------------------------------------------------------------------
       * 1.0 Inquiry-Company_Cost_Center (ICCCA) 
     ----------------------------------------------------------------------------------*/
    "use strict"
    var companyID = $('#ICCCA-companyID').val();
    // ajax to get 'Cost-Center-ID' linked to 'Company-ID' 
    $.ajax({
        type: "GET",
        url: "/Inquiry_CompanyCostCenterAccounts/GetCostCenterID?companyID=" + companyID,
        success: function (result) {
            if (result == "False") {
                $("#ICCCA-CompCostID").prop("disabled", true);
                $("#ICCCA-errors").text("You do not have access to this company !!");

                //clear If False 
                $("#ICCCA-CompCostID").val("");
                $("#ICCCA-append-data").html("");
                $("#ICCCA-costname").val("");
            } else {
                $("#ICCCA-CompCostID").empty();
                $("#ICCCA-CompCostID").append($('<option/>', {
                    value: "",
                    text: "-Choose-"
                })
                );
                $.each(result, function (index, row) {
                    $("#ICCCA-CompCostID").append("<option value='" + row.Cost_CenterID + "'>" + row.Cost_CenterID + "</option>");
                });

                // Enable Analytic-Accounts-ID drop-down
                $("#ICCCA-CompCostID").prop("disabled", false);
                $("#ICCCA-errors").text("");
            }
        }
    });
    $("#ICCCA-CompCostID").change(function () {
        var SetData = $("#ICCCA-append-data");
        var CostID = $(this).val();

        // ajax to get Cost-Center-Name
        $.ajax({
            type: "GET",
            url: "/Inquiry_CompanyCostCenterAccounts/GetCostCenterName?CostID=" + CostID,
            success: function (result) {
                $("#ICCCA-costname").val(result);
            }
        });

        // ajax to fill tbody table
        SetData.html("");
        $.ajax({
            type: "GET",
            url: "/Inquiry_CompanyCostCenterAccounts/GetData?CostID=" + CostID,
            contentType: "html",
            success: function (result) {
                if (result.length == 0) {
                    SetData.append('<tr style="color:red"><td colspan="2"> No Result Found !!! </td></tr>')
                }
                else {
                    for (var i = 0; i < result.length; i++) {
                        var Data = "<tr>" +
                            "<td width='30%'>" + result[i].Cost_AccountID + "</td>" +
                            "<td width='70%'>" + result[i].Cost_AccountName + "</td>" +
                            "</tr>";
                        SetData.append(Data);
                    }
                }
            },
        });
    });

    ////////////////////////////////////////////////////////////////////////////////////////////////////////



    /*----------------------------------------------------------------------------------
       * 2.0 Inquiry-Branch_Cost_Center (IBCCA) 
     ----------------------------------------------------------------------------------*/
    if (companyIDSession.length > 0) {
        $("#InquiryBranchID").change(function () {
            "use strict"
            var branchID = $(this).val();
            // clear
            if (branchID.length === 0) {
                $("#IBCCA-branchname").val("");
                $("#IBCCA-errors").text("");
                $("#IBCCA-BranchCostID").val("");
                $("#IBCCA-costname").val("");
                $("#IBCCA-append-data").html("");
                $("#IBCCA-BranchCostID").prop("disabled", true);
            } else {
                // ajax to get Branch-Name
                $.ajax({
                    type: "GET",
                    url: "/Inquiry_BranchCostCenterAccounts/GetBranchName?branchID=" + branchID,
                    success: function (result) {
                        $("#IBCCA-branchname").val(result);
                    }
                });

                // ajax to get 'Cost-Center-ID' linked to 'Branch-ID' 
                $.ajax({
                    type: "GET",
                    url: "/Inquiry_BranchCostCenterAccounts/GetBranchCostCenterID?branchID=" + branchID,
                    success: function (result) {
                        if (result == "False") {
                            $("#IBCCA-BranchCostID").prop("disabled", true);
                            $("#IBCCA-errors").text("You do not have access to this Branch !!");

                            //clear If False 
                            $("#IBCCA-BranchCostID").val("");
                            $("#IBCCA-append-data").html("");
                            $("#IBCCA-costname").val("");
                        } else {
                            $("#IBCCA-BranchCostID").empty();
                            $("#IBCCA-BranchCostID").append($('<option/>', {
                                value: "",
                                text: "-Choose-"
                            })
                            );
                            $.each(result, function (index, row) {
                                $("#IBCCA-BranchCostID").append("<option value='" + row.Cost_CenterID + "'>" + row.Cost_CenterID + "</option>");
                            });

                            // Enable Cost-Center-ID drop-down
                            $("#IBCCA-BranchCostID").prop("disabled", false);
                            $("#IBCCA-errors").text("");
                        }
                    }
                });
            }
        });
    }

    if (branchIDSession.length > 0) {
        var branchID = branchIDSession;
        // ajax to get 'Cost-Center-ID' linked to 'Branch-ID' 
        $.ajax({
            type: "GET",
            url: "/Inquiry_BranchCostCenterAccounts/GetBranchCostCenterID?branchID=" + branchID,
            success: function (result) {
                if (result == "False") {
                    $("#IBCCA-BranchCostID").prop("disabled", true);
                    $("#IBCCA-errors").text("You do not have access to this Branch !!");

                    //clear If False 
                    $("#IBCCA-BranchCostID").val("");
                    $("#IBCCA-append-data").html("");
                    $("#IBCCA-costname").val("");
                } else {
                    $("#IBCCA-BranchCostID").empty();
                    $("#IBCCA-BranchCostID").append($('<option/>', {
                        value: "",
                        text: "-Choose-"
                    })
                    );
                    $.each(result, function (index, row) {
                        $("#IBCCA-BranchCostID").append("<option value='" + row.Cost_CenterID + "'>" + row.Cost_CenterID + "</option>");
                    });

                    // Enable Cost-Center-ID drop-down
                    $("#IBCCA-BranchCostID").prop("disabled", false);
                    $("#IBCCA-errors").text("");
                }
            }
        });
    }
    $("#IBCCA-BranchCostID").change(function () {
        var SetData = $("#IBCCA-append-data");
        var CostID = $(this).val();

        // ajax to get Branch-Cost-Center-Name
        $.ajax({
            type: "GET",
            url: "/Inquiry_BranchCostCenterAccounts/GetBranchCostCenterName?CostID=" + CostID,
            success: function (result) {
                $("#IBCCA-costname").val(result);
            }
        });

        // ajax to fill tbody table
        SetData.html("");
        $.ajax({
            type: "GET",
            url: "/Inquiry_BranchCostCenterAccounts/GetData?CostID=" + CostID,
            contentType: "html",
            success: function (result) {
                if (result.length == 0) {
                    SetData.append('<tr style="color:red"><td colspan="2"> No Result Found !!! </td></tr>')
                }
                else {
                    for (var i = 0; i < result.length; i++) {
                        var Data = "<tr>" +
                            "<td width='30%'>" + result[i].Cost_AccountID + "</td>" +
                            "<td width='70%'>" + result[i].Cost_AccountName + "</td>" +
                            "</tr>";
                        SetData.append(Data);
                    }
                }
            },
        });
    });

    ////////////////////////////////////////////////////////////////////////////////////////////////////////



    /*----------------------------------------------------------------------------------
      * 3.0 Inquiry-Factory_Cost_Center (IFCCA) 
    ----------------------------------------------------------------------------------*/
    $("#IFCCA-FactoryID").change(function () {
        "use strict"
        var factoryID = $(this).val();

        // clear
        if (factoryID.length === 0) {
            $("#IFCCA-factoryname").val("");
            $("#IFCCA-errors").text("");
            $("#IFCCA-FactoryCostID").val("");
            $("#IFCCA-costname").val("");
            $("#IFCCA-append-data").html("");
            $("#IFCCA-FactoryCostID").prop("disabled", true);
        } else {
            // ajax to get Branch-Name
            $.ajax({
                type: "GET",
                url: "/Inquiry_FactoryCostCenterAccounts/GetFactoryName?factoryID=" + factoryID,
                success: function (result) {
                    $("#IFCCA-factoryname").val(result);
                }
            });

            // ajax to get 'Cost-Center-ID' linked to 'Branch-ID' 
            $.ajax({
                type: "GET",
                url: "/Inquiry_FactoryCostCenterAccounts/GetFactoryCostCenterID?factoryID=" + factoryID,
                success: function (result) {
                    if (result == "False") {
                        $("#IFCCA-FactoryCostID").prop("disabled", true);
                        $("#IFCCA-errors").text("You do not have access to this Factory !!");

                        //clear If False 
                        $("#IFCCA-FactoryCostID").val("");
                        $("#IFCCA-append-data").html("");
                        $("#IFCCA-costname").val("");
                    } else {
                        $("#IFCCA-FactoryCostID").empty();
                        $("#IFCCA-FactoryCostID").append($('<option/>', {
                            value: "",
                            text: "-Choose-"
                        })
                        );
                        $.each(result, function (index, row) {
                            $("#IFCCA-FactoryCostID").append("<option value='" + row.Cost_CenterID + "'>" + row.Cost_CenterID + "</option>");
                        });

                        // Enable Cost-Center-ID drop-down
                        $("#IFCCA-FactoryCostID").prop("disabled", false);
                        $("#IFCCA-errors").text("");
                    }
                }
            });
        }
    });
    $("#IFCCA-FactoryCostID").change(function () {
        var SetData = $("#IFCCA-append-data");
        var CostID = $(this).val();

        // ajax to get Factory-Cost-Center-Name
        $.ajax({
            type: "GET",
            url: "/Inquiry_FactoryCostCenterAccounts/GetFactoryCostCenterName?CostID=" + CostID,
            success: function (result) {
                $("#IFCCA-costname").val(result);
            }
        });

        // ajax to fill tbody table
        SetData.html("");
        $.ajax({
            type: "GET",
            url: "/Inquiry_FactoryCostCenterAccounts/GetData?CostID=" + CostID,
            contentType: "html",
            success: function (result) {
                if (result.length == 0) {
                    SetData.append('<tr style="color:red"><td colspan="2"> No Result Found !!! </td></tr>')
                }
                else {
                    for (var i = 0; i < result.length; i++) {
                        var Data = "<tr>" +
                            "<td width='30%'>" + result[i].Cost_AccountID + "</td>" +
                            "<td width='70%'>" + result[i].Cost_AccountName + "</td>" +
                            "</tr>";
                        SetData.append(Data);
                    }
                }
            },
        });
    });



















});