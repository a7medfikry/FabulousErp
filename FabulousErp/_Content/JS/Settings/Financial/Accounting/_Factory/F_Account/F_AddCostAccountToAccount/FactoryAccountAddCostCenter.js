$(document).ready(function () {

    var factoryIDFF = $("#FactoryIDFF").val();
    if ($("#FactoryIDFF").length > 0) {
        //Get Factory Accounts
        GetFactoryAccounts(factoryIDFF);
    }

    $("#FactoryID").change(function () {

        var FactoryID = $(this).val();

        $("#FactoryName").val("");
        $("#GlobalError").text("");
        $("#AccountID").empty();
        ClearData();

        if (FactoryID.length === 0) {
            $(this).css("border-color", "red");
        } else {
            $(this).css("border-color", "");

            $.ajax({
                type: "GET",
                url: "/F_AddCostAccountToAccount/GetFactoryName?FactoryID=" + FactoryID,
                success: function (result) {
                    if (result === "False") {
                        $("#FactoryID").val("");
                        $("#GlobalError").text("You Not Have Access To This Factory..!");
                    } else {
                        $("#FactoryName").val(result);

                        //Get Factory Accounts
                        GetFactoryAccounts(FactoryID);

                    }
                }
            });

        }

    });



    // Get Accounts that related to this Factory that related to company to the chart
    function GetFactoryAccounts(FactoryID) {

        $.ajax({
            type: "GET",
            url: "/F_AddCostAccountToAccount/GetAccountDist?FactoryID=" + FactoryID,
            success: function (data) {
                $("#AccountID").empty();

                $("#AccountName").val("");

                if (data.length == 0) {

                    $("#AccountID").append($('<option/>', {
                        value: "",
                        text: "No Account Created To This Factory"
                    })
                    );
                } else {

                    $("#AccountID").append($('<option/>', {
                        value: "",
                        text: "-Choose-"
                    })
                    );
                    $.each(data, function (index, row) {

                        $("#AccountID").append("<option value='" + row.F_AID + "'>" + row.F_AccountID + " ( " + row.AccountName + " )" + "</option>");

                    });
                }
            }
        });

    }
    // Change of Account ID
    $("#AccountID").change(function () {

        var AccountID = $(this).val();

        ClearData();

        if (AccountID.length === 0) {
            $(this).css("border-color", "red");
            
        } else {

            $(this).css("border-color", "");

            $("#TypeCostCenter").hide();
            $("#TypeMainCostCenter").hide();
            $("#CostAccountTbl").hide();

            $.ajax({
                type: "GET",
                url: "/F_AddCostAccountToAccount/GetAccountName?AccountID=" + AccountID,
                success: function (result) {
                    $("#AccountName").val(result.AccountName);
                    $("#Type").text(result.CostCenterType);
                    $("#AccountChartID").text(result.AccountChartID);

                    //if Type of Account -> Cost Center
                    if (result.CostCenterType == "CostCenter") {

                        $("#CC").show();
                        $("#MCC").hide();

                        if (result.R_CostCenterID == "") {
                            $("#GlobalError").text("No Cost Center ID Choosen To This Account go To Update This Account..").append("<p><a href='/F_CreateAccount/FactoryAccount'>Factory Create Account</a></p>");
                        } else {
                            $("#GlobalError").text("");
                            $("#CostCenterID").val(result.R_CostCenterID);
                            $("#TypeCostCenter").show();
                            $("#CostAccountTbl").show();
                            // get Cost Center Accounts from cost center id that related to this Account
                            GetCostCenterAccounts(result.R_CostCenterID);
                            // get Cost center Accouts with percentage that added to this Account to A table
                            GetCostAccountsData(AccountID, result.R_CostCenterID, result.CostCenterType);
                        }
                    }
                    // if Type of Account -> Main Cost Center
                    else if (result.CostCenterType == "MainCostCenter") {

                        $("#CC").hide();
                        $("#MCC").show();

                        if (result.R_CostCenterGroupID == "") {
                            $("#GlobalError").text("No Main Cost Center ID Choosen To This Account go To Update This Account..").append("<p><a href='/F_CreateAccount/FactoryAccount'>Factory Create Account</a></p>");
                        } else {
                            $("#GlobalError").text("");
                            $("#MainCostCenterID").val(result.R_CostCenterGroupID);
                            $("#TypeMainCostCenter").show();
                            $("#CostAccountTbl").show();
                            //Get Content of Main Cost Center That related to this Account
                            GetMainCostCenterID(result.R_CostCenterGroupID);
                        }
                    } else {
                        $("#GlobalError").text("No Main OR Cost Center ID Choosen To This Account go To Update This Account..").append("<p><a href='/F_CreateAccount/FactoryAccount'>Factory Create Account</a></p>");
                    }
                }
            });
        }
    });



    // get Cost Center Accounts from cost center id that related to this Account
    function GetCostCenterAccounts(CostCenterID) {

        $.ajax({
            type: "GET",
            url: "/F_AddCostAccountToAccount/GetCostCenterAccounts?CostCenterID=" + CostCenterID,
            success: function (data) {
                $("#CostAccountID").empty();

                $("#CostAccountName").val("");

                if (data.length == 0) {

                    $("#CostAccountID").append($('<option/>', {
                        value: "",
                        text: "No Cost Account Created To This Cost Center ID"
                    })
                    );
                } else {

                    $("#CostAccountID").append($('<option/>', {
                        value: "",
                        text: "-Choose-"
                    })
                    );
                    $.each(data, function (index, row) {

                        $("#CostAccountID").append("<option value='" + row.C_CAID + "'>" + row.CostAccountID + "</option>");

                    });
                }
            }
        });

    }
    // Change of Cost Accounts
    $("#CostAccountID").change(function () {

        var CostAccountID = $(this).val();

        if (CostAccountID.length == 0) {
            $(this).css("border-color", "red");
            $("#CostAccountName").val("");
        } else {
            $(this).css("border-color", "");

            $.ajax({
                type: "GET",
                url: "/F_AddCostAccountToAccount/GetCostAccountName?CostAccountID=" + CostAccountID,
                success: function (result) {
                    $("#CostAccountName").val(result);
                }
            });
        }
    });



    // Add button to Account And percentage in a specific cost center
    $("#AddCCAccountbtn").click(function () {

        $("#AddCCAccountbtnError").text("");

        var AccountID = $("#AccountID").val();

        var CostCenterID = $("#CostCenterID").val();

        var CostAccountID = $("#CostAccountID").val();

        var Percentage = $("#Percentage").val();

        var Type = $("#Type").text();

        var AccountChartID = $("#AccountChartID").text();

        var Test = true;

        if (CostAccountID.length == 0) {
            $("#CostAccountID").css("border-color", "red");
            Test = false;
        } else {
            $("#CostAccountID").css("border-color", "");
        }

        if (Percentage.length === 0) {
            $("#Percentage").css("border", "1px solid red");
            Test = false;
        } else if (Percentage > 100) {
            $("#Percentage").css("border", "1px solid red");
            Test = false;
        } else {
            $("#Percentage").css("border", "");
        }

        var sumPercentage = 0;

        $(".FactoryCostAccPerTbl").each(function () {
            var value = $(this).text();
            // add only if the value is number
            if (!isNaN(value) && value.length != 0) {
                sumPercentage += parseFloat(value);
            }
        });


        if (parseFloat(sumPercentage) + parseFloat(Percentage) > 100) {
            $("#Percentage").css("border", "1px solid red");
            Test = false;
            $("#AddCCAccountbtnError").text("Wrong Percentage..!");
        }

        if (Test === true) {

            $.ajax({
                type: "POST",
                url: "/F_AddCostAccountToAccount/AddCostAccountTypeCC?AccountID=" + AccountID + "&CostCenterID=" + CostCenterID + "&CostAccountID=" + CostAccountID + "&Percentage=" + Percentage + "&Type=" + Type + "&AccountChartID=" + AccountChartID,
                success: function (result) {

                    if (result == "False") {
                        $("#CostAccountID").css("border-color", "red");
                        $("#AddCCAccountbtnError").text("Cost Account ID Already Taked Persantage..!");
                    } else {
                        $("#CostAccountID").css("border-color", "");
                        $("#AddCCAccountbtnError").text("");

                        GetCostAccountsData(AccountID, CostCenterID, Type, AccountChartID);

                    }

                }
            });

        }

    });



    // get Cost center Accouts with percentage that added to this Account to A table
    function GetCostAccountsData(AccountID, CostCenterID, Type, AccountChartID) {

        var tableData = $("#CostAccountTblBody");
        tableData.html("");

        $.ajax({
            type: "GET",
            url: "/F_AddCostAccountToAccount/GetCostAccounts?AccountID=" + AccountID + "&CostCenterID=" + CostCenterID + "&Type=" + Type + "&AccountChartID=" + AccountChartID,
            success: function (result) {

                for (var i = 0; i < result.length; i++) {

                    var Data = "<tr class='row_" + result[i].ID + "'>" +
                        "<td>" + result[i].C_CostAccountID + "</td>" +
                        "<td>" + result[i].C_CostAccountName + "</td>" +
                        "<td class='FactoryCostAccPerTbl'>" + result[i].Percentage + "</td>" +
                        "<td>" + '<a href="#" onclick="DeleteCompCostAccount(\'' + result[i].ID + '\')"><span class="fa fa-trash-o"></span></a>' + "</td>" +
                        "</tr>";
                    tableData.append(Data);
                }

            }
        });
    }



    // Get Content of Main Cost Center That related to this Account
    function GetMainCostCenterID(CostCenterGroupID) {

        $.ajax({
            type: "GET",
            url: "/F_AddCostAccountToAccount/GetMainCostCenterID?CostCenterGroupID=" + CostCenterGroupID,
            success: function (data) {
                $("#CostCenterIDFromMain").empty();

                $("#CostCenterNameFromMain").val("");

                if (data.length == 0) {

                    $("#CostCenterIDFromMain").append($('<option/>', {
                        value: "",
                        text: "No Cost Account Created To This Cost Center ID"
                    })
                    );
                } else {

                    $("#CostCenterIDFromMain").append($('<option/>', {
                        value: "",
                        text: "-Choose-"
                    })
                    );
                    $.each(data, function (index, row) {

                        $("#CostCenterIDFromMain").append("<option value='" + row.GroupID + "'>" + row.CostCenterID + "</option>");

                    });
                }
            }
        });

    }

    // Change of Content of Main Cost Center That read cost center ID
    $("#CostCenterIDFromMain").change(function () {

        var CostCenterIDFromMain = $("#CostCenterIDFromMain option:selected").text();
        var MainCostCenterID = $("#MainCostCenterID").val();
        var AccountID = $("#AccountID").val();
        var Type = $("#Type").text();

        $("#CostCenterNameFromMain").val("");
        $("#PercentageCostCenterID").val("");
        $("#CostAccountIDFromMain").empty();
        $("#CostAccountNameFromMain").val("");

        if (CostCenterIDFromMain.length === 0) {
            $(this).css("border-color", "red");
        } else {
            $(this).css("border-color", "");

            $.ajax({
                type: "GET",
                url: "/F_AddCostAccountToAccount/GetCostCenterIDData?CostCenterIDFromMain=" + CostCenterIDFromMain + "&MainCostCenterID=" + MainCostCenterID,
                success: function (result) {
                    $("#CostCenterNameFromMain").val(result.CostCenterName);
                    $("#PercentageCostCenterID").val(result.CostCenterPercentage);

                    //get Cost Center Accounts from cost center id that related with main cost center that related with Account
                    GetCostCenterAccountsFromMain(CostCenterIDFromMain);

                    // get Cost center Accouts with percentage that added to this Account to A table
                    GetCostAccountsData(AccountID, CostCenterIDFromMain, Type);
                }
            });
        }
    });



    // get Cost Center Accounts from cost center id that related with main cost center that related with Account
    function GetCostCenterAccountsFromMain(CostCenterIDFromMain) {

        $.ajax({
            type: "GET",
            url: "/F_AddCostAccountToAccount/GetCostCenterAccounts?CostCenterID=" + CostCenterIDFromMain,
            success: function (data) {
                $("#CostAccountIDFromMain").empty();

                $("#CostAccountNameFromMain").val("");

                if (data.length == 0) {

                    $("#CostAccountIDFromMain").append($('<option/>', {
                        value: "",
                        text: "No Cost Account Created To This Cost Center ID"
                    })
                    );
                } else {

                    $("#CostAccountIDFromMain").append($('<option/>', {
                        value: "",
                        text: "-Choose-"
                    })
                    );
                    $.each(data, function (index, row) {

                        $("#CostAccountIDFromMain").append("<option value='" + row.C_CAID + "'>" + row.CostAccountID + "</option>");

                    });
                }
            }
        });

    }

    // change of Cost Accounts From Main
    $("#CostAccountIDFromMain").change(function () {

        var CostAccountID = $(this).val();

        if (CostAccountID.length == 0) {
            $(this).css("border-color", "red");
            $("#CostAccountNameFromMain").val("");
        } else {
            $(this).css("border-color", "");

            $.ajax({
                type: "GET",
                url: "/F_AddCostAccountToAccount/GetCostAccountName?CostAccountID=" + CostAccountID,
                success: function (result) {
                    $("#CostAccountNameFromMain").val(result);
                }
            });
        }

    });



    // Add Accounts with percentage but from Type Main cost center
    $("#AddMainCCAccountbtn").click(function () {

        $("#AddMainCCAccountbtnError").text("");

        var AccountID = $("#AccountID").val();

        var MainCostCenterID = $("#MainCostCenterID").val();

        var CostAccountIDFromMain = $("#CostAccountIDFromMain").val();

        var PercentageFromMain = $("#PercentageFromMain").val();

        var AccountChartID = $("#AccountChartID").text();

        var Type = $("#Type").text();

        // Details of cost center ID from Group That have Accounts
        //var CostCenterIDFromMain = $("#CostCenterIDFromMain").val();
        var CostCenterIDFromMainVal = $("#CostCenterIDFromMain").val();
        var CostCenterIDFromMainText = $("#CostCenterIDFromMain option:selected").text();
        var CostCenterNameFromMain = $("#CostCenterNameFromMain").val();
        var PercentageCostCenterID = $("#PercentageCostCenterID").val();

        var Test = true;

        if (CostCenterIDFromMainVal.length == 0) {
            $("#CostCenterIDFromMain").css("border-color", "red");
            Test = false;
        } else {
            $("#CostCenterIDFromMain").css("border-color", "");
        }

        if (CostAccountIDFromMain.length == 0) {
            $("#CostAccountIDFromMain").css("border-color", "red");
            Test = false;
        } else {
            $("#CostAccountIDFromMain").css("border-color", "");
        }

        if (PercentageFromMain.length == 0) {
            $("#PercentageFromMain").css("border-color", "red");
            Test = false;
        } else if (PercentageFromMain > 100) {
            $("#PercentageFromMain").css("border", "1px solid red");
            Test = false;
        } else {
            $("#PercentageFromMain").css("border-color", "");
        }

        var sumPercentage = 0;

        $(".FactoryCostAccPerTbl").each(function () {
            var value = $(this).text();
            // add only if the value is number
            if (!isNaN(value) && value.length != 0) {
                sumPercentage += parseFloat(value);
            }
        });


        if (parseFloat(sumPercentage) + parseFloat(PercentageFromMain) > 100) {
            $("#PercentageFromMain").css("border", "1px solid red");
            Test = false;
            $("#AddMainCCAccountbtnError").text("Wrong Percentage..!");
        }

        if (Test === true) {

            $.ajax({
                type: "POST",
                url: "/F_AddCostAccountToAccount/AddCostAccountTypeMCC?AccountID=" + AccountID + "&MainCostCenterID=" + MainCostCenterID + "&CostCenterIDFromMain=" + CostCenterIDFromMainText + "&CostCenterNameFromMain=" + CostCenterNameFromMain + "&PercentageCostCenterID=" + PercentageCostCenterID + "&CostAccountIDFromMain=" + CostAccountIDFromMain + "&PercentageFromMain=" + PercentageFromMain + "&Type=" + Type + "&AccountChartID=" + AccountChartID + "&GroupID=" + CostCenterIDFromMainVal,
                success: function (result) {

                    if (result == "False") {
                        $("#CostAccountIDFromMain").css("border-color", "red");
                        $("#AddMainCCAccountbtnError").text("Cost Account ID Already Taked Persantage..!");
                    } else {
                        $("#CostAccountIDFromMain").css("border-color", "");
                        $("#AddMainCCAccountbtnError").text("");

                        GetCostAccountsData(AccountID, CostCenterIDFromMainText, Type);

                    }

                }
            });

        }

    });

});


// Delete Cost Accounts with percentage
function DeleteCompCostAccount(CAccountID) {

    $("#CostCenterAccountID").text(CAccountID);

    $("#DeleteConfirmation").modal("show");

}
function CompConfirmDelete() {

    var CostCenterAccountID = $("#CostCenterAccountID").text();

    $.ajax({
        type: "POST",
        url: "/F_AddCostAccountToAccount/DeleteCostAccountComp?CostAccountID=" + CostCenterAccountID,
        success: function (result) {
            $("#DeleteConfirmation").modal("hide");
            $(".row_" + CostCenterAccountID).remove();
        }
    });
}

function ClearData() {

    $("#CostAccountID").empty();
    $("#CostAccountID").css("border-color", "");
    $("#CostAccountIDFromMain").empty();
    $("#CostAccountIDFromMain").css("border-color", "");
    $("#CostAccountName").val("");
    $("#CostAccountNameFromMain").val("");
    $("#Percentage").val("");
    $("#Percentage").css("border", "");
    $("#PercentageFromMain").val("");
    $("#PercentageFromMain").css("border", "");
    $("#AddCCAccountbtnError").text("");
    $("#AddMainCCAccountbtnError").text("");
    $("#CostAccountTblBody").html("");
    $("#AccountName").val("");
    $("#GlobalError").text("");
    $("#CostCenterID").val("");
    $("#MainCostCenterID").val("");
    $("#CostCenterIDFromMain").empty();
    $("#CostCenterNameFromMain").val("");
    $("#PercentageCostCenterID").val("");

}