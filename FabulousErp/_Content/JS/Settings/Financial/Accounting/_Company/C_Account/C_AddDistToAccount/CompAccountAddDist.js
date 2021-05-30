$(document).ready(function () {

    GetCompanyAccounts();

});

//Get Company Created Accounts
function GetCompanyAccounts() {

    $.ajax({
        type: "GET",
        url: "/C_AddDistToAccount/GetAccountDist",
        success: function (data) {
            $("#AccountID").empty();

            $("#AccountName").val("");

            if (data.length == 0) {

                $("#AccountID").append($('<option/>', {
                    value: -1,
                    text: "No Account Created To This Company"
                })
                );
            } else {

                $("#AccountID").append($('<option/>', {
                    value: -1,
                    text: "-Choose-"
                })
                );
                $.each(data, function (index, row) {

                    $("#AccountID").append("<option value='" + row.C_AID + "'>" + row.C_AccountID + " ( " + row.AccountName + " )" + "</option>");

                });
            }
        }
    });

}
//Change of Accounts ID
$("#AccountID").change(function () {

    var AccountID = $(this).val();

    $("#DistAccountID").empty();
    $("#DistAccountID").css("border-color", "");
    $("#DistAccountName").val("");
    $("#Percentage").val("");
    $("#Percentage").css("border", "");
    $("#AddError").text("");
    $("#DistAccountTblBody").html("");

    if (AccountID == "-1") {
        $(this).css("border-color", "red");
        $("#AccountName").val("");
        $("#GlobalError").text("");
        $("#AnalyticAccountID").val("");
    } else {

        $(this).css("border-color", "");

        $.ajax({
            type: "GET",
            url: "/C_AddDistToAccount/GetAccountName?AccountID=" + AccountID,
            success: function (result) {
                $("#AccountName").val(result.AccountName);
                $("#AnalyticAccountID").val(result.R_AnalyticAccountID);
                $("#AccountChartID").text(result.AccountChartID);

                if (result.R_AnalyticAccountID == "") {
                    $("#GlobalError").text("No Analytic Account ID Choosen To This Account go To Update This Account..").append("<p><a href='/C_LinkAnalyticToAccount/CompanyLinkAnalyticToAccount'>Link Analytic To Account ID</a></p>");
                } else {
                    $("#GlobalError").text("");
                    //Get Analytic Distribution by Analytic Account ID that related with Account 
                    GetAnalyticDistribution(result.R_AnalyticAccountID);
                    //Get Distribution that created in Analytic to show in table
                    GetDistData(AccountID, result.R_AnalyticAccountID);
                }

            }
        });
    }
});


//Get Analytic Distribution by Analytic Account ID that related with Account 
function GetAnalyticDistribution(AnalyticAccountID) {

    $.ajax({
        type: "GET",
        url: "/C_AddDistToAccount/GetDistAccountID?AnalyticAccountID=" + AnalyticAccountID,
        success: function (data) {
            $("#DistAccountID").empty();

            $("#DistAccountName").val("");

            if (data.length == 0) {

                $("#DistAccountID").append($('<option/>', {
                    value: -1,
                    text: "No Dist. Account Created To This Analytic Account"
                })
                );
            } else {

                $("#DistAccountID").append($('<option/>', {
                    value: -1,
                    text: "-Choose-"
                })
                );
                $.each(data, function (index, row) {

                    $("#DistAccountID").append("<option value='" + row.C_DistID + "'>" + row.DistAccountID + "</option>");

                });
            }
        }
    });

}
// Change of Distribution Accounts
$("#DistAccountID").change(function () {

    var DistAccountID = $(this).val();

    if (DistAccountID == "-1") {

        $(this).css("border-color", "red");
        $("#DistAccountName").val("");
    } else {

        $(this).css("border-color", "");

        $.ajax({
            type: "GET",
            url: "/C_AddDistToAccount/GetDistAccountName?DistAccountID=" + DistAccountID,
            success: function (result) {
                $("#DistAccountName").val(result);
            }
        });
    }
});


//Add button to Add percantage of the Dist to this Account
$("#AddDistAccountBtn").click(function () {

    $("#AddError").text("");

    var AccountID = $("#AccountID").val();

    var AnalyticAccountID = $("#AnalyticAccountID").val();

    var DistAccountID = $("#DistAccountID").val();

    var Percentage = $("#Percentage").val();

    var AccountChartID = $("#AccountChartID").text();

    var Test = true;

    if (DistAccountID == "-1" || DistAccountID.length == 0) {
        $("#DistAccountID").css("border-color", "red");
        Test = false;
    } else {
        $("#DistAccountID").css("border-color", "");
    }
    if (!Percentage) {
        Percentage = 0;
    }
    //if (Percentage.length === 0) {
    //    $("#Percentage").css("border", "1px solid red");
    //    Test = false;
    //} else if (Percentage > 100) {
    //    $("#Percentage").css("border", "1px solid red");
    //    Test = false;
    //} else {
    //    $("#Percentage").css("border", "");
    //}

    var sumPercentage = 0;

    $(".CopmAccDistPerTbl").each(function () {
        var value = $(this).text();
        // add only if the value is number
        if (!isNaN(value) && value.length != 0) {
            sumPercentage += parseFloat(value);
        }
    });


    //if (parseFloat(sumPercentage) + parseFloat(Percentage) > 100) {
    //    $("#Percentage").css("border", "1px solid red");
    //    Test = false;
    //    $("#AddError").text("Wrong Percentage..!");
    //}

    if (Test === true) {

        $.ajax({
            type: "POST",
            url: "/C_AddDistToAccount/AddDistributionAccount?AccountID=" + AccountID + "&AnalyticAccountID=" + AnalyticAccountID + "&DistAccountID=" + DistAccountID + "&Percentage=" + Percentage + "&AccountChartID=" + AccountChartID,
            success: function (result) {
                if (result == "False") {
                    $("#DistAccountID").css("border-color", "red");
                    $("#AddError").text("Dist. Account ID Already Taked Persantage..!");
                } else {
                    $("#DistAccountID").css("border-color", "");
                    $("#AddError").text("");

                    GetDistData(AccountID, AnalyticAccountID, AccountChartID);
                }
            }
        });

    }

});


//Get Distribution that created in Analytic to show in table
function GetDistData(AccountID, AnalyticAccountID, AccountChartID) {

    var tableData = $("#DistAccountTblBody");
    tableData.html("");

    $.ajax({
        type: "GET",
        url: "/C_AddDistToAccount/GetDistributionAccounts?AccountID=" + AccountID + "&AnalyticAccountID=" + AnalyticAccountID + "&AccountChartID=" + AccountChartID,
        success: function (result) {

            for (var i = 0; i < result.length; i++) {

                var Data = "<tr class='row_" + result[i].ID + "'>" +
                    "<td width='20%'>" + result[i].C_DistAccountID + "</td>" +
                    "<td width='40%'>" + result[i].C_DistAccountName + "</td>" +
                    "<td width='20%' class='CopmAccDistPerTbl'>" + result[i].Percentage + "</td>" +
                    "<td width='20%'>" + '<a href="#" onclick="DeleteCompDistribution(\'' + result[i].ID + '\')"><span class="fa fa-trash-o"></span></a>' + "</td>" +
                    "</tr>";
                tableData.append(Data);
            }

        }
    });

}


//Delete Distribution from tha analytic that related to this Account ID
function DeleteCompDistribution(DistID) {

    $("#AccountDistributionID").text(DistID);

    $("#DeleteConfirmation").modal("show");

}

function CompConfirmDelete() {

    var AccountDistributionID = $("#AccountDistributionID").text();

    $.ajax({
        type: "POST",
        url: "/C_AddDistToAccount/DeleteAccountDistributionComp?AccountDistributionID=" + AccountDistributionID,
        success: function (result) {
            $("#DeleteConfirmation").modal("hide");
            $(".row_" + AccountDistributionID).remove();
        }
    });
}