$(document).ready(function () {

    var branchIDFB = $("#BranchIDFB").val();
    if ($("#BranchIDFB").length > 0) {
        //Get Branch Created Accounts
        GetBranchAccounts(branchIDFB);
    }

    $("#BranchID").change(function () {

        var BranchID = $(this).val();

        $("#BranchName").val("");
        $("#GlobalError").text("");
        $("#AccountID").empty();
        $("#AccountName").val("");
        $("#DistAccountID").empty();
        $("#DistAccountName").val("");
        $("#AnalyticAccountID").val("");
        $("#DistAccountTblBody").html("");

        if (BranchID.length === 0) {
            $(this).css("border-color", "red");
        } else {
            $(this).css("border-color", "");

            $.ajax({
                type: "GET",
                url: "/B_AddDistToAccount/GetBranchName?BranchID=" + BranchID,
                success: function (result) {

                    if (result === "False") {

                        $("#BranchID").val("");
                        $("#GlobalError").text("You Not Have Access To This Branch..!");

                    } else {
                        $("#BranchName").val(result);

                        //Get Branch Created Accounts
                        GetBranchAccounts(BranchID);

                    }
                }
            });
        }
    });



    //Get Branch Created Accounts
    function GetBranchAccounts(BranchID) {
        $.ajax({
            type: "GET",
            url: "/B_AddDistToAccount/GetAccountDist?BranchID=" + BranchID,
            success: function (data) {
                $("#AccountID").empty();

                $("#AccountName").val("");

                if (data.length == 0) {

                    $("#AccountID").append($('<option/>', {
                        value: "",
                        text: "No Account Created To This Branch"
                    })
                    );
                } else {

                    $("#AccountID").append($('<option/>', {
                        value: "",
                        text: "-Choose-"
                    })
                    );
                    $.each(data, function (index, row) {

                        $("#AccountID").append("<option value='" + row.B_AID + "'>" + row.B_AccountID + " ( " + row.AccountName + " )" + "</option>");

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

        if (AccountID.length === 0) {
            $(this).css("border-color", "red");
            $("#AccountName").val("");
            $("#GlobalError").text("");
            $("#AnalyticAccountID").val("");
        } else {

            $(this).css("border-color", "");

            $.ajax({
                type: "GET",
                url: "/B_AddDistToAccount/GetAccountName?AccountID=" + AccountID,
                success: function (result) {
                    $("#AccountName").val(result.AccountName);
                    $("#AnalyticAccountID").val(result.R_AnalyticAccountID);
                    $("#AccountChartID").text(result.AccountChartID);

                    if (result.R_AnalyticAccountID == "") {
                        $("#GlobalError").text("No Analytic Account ID Choosen To This Account go To Update This Account..").append("<p><a href='/B_LinkAnalyticToAccount/BranchLinkAnalyticToAccount'>Link Analytic To Account ID</a></p>");
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
            url: "/B_AddDistToAccount/GetDistAccountID?AnalyticAccountID=" + AnalyticAccountID,
            success: function (data) {
                $("#DistAccountID").empty();

                $("#DistAccountName").val("");

                if (data.length == 0) {

                    $("#DistAccountID").append($('<option/>', {
                        value: "",
                        text: "No Dist. Account Created To This Analytic Account"
                    })
                    );
                } else {

                    $("#DistAccountID").append($('<option/>', {
                        value: "",
                        text: "-Choose-"
                    })
                    );
                    $.each(data, function (index, row) {

                        $("#DistAccountID").append("<option value='" + row.B_DistID + "'>" + row.DistAccountID + "</option>");

                    });
                }
            }
        });

    }

    // Change of Distribution Accounts
    $("#DistAccountID").change(function () {

        var DistAccountID = $(this).val();

        if (DistAccountID.length === 0) {

            $(this).css("border-color", "red");
            $("#DistAccountName").val("");
        } else {

            $(this).css("border-color", "");

            $.ajax({
                type: "GET",
                url: "/B_AddDistToAccount/GetDistAccountName?DistAccountID=" + DistAccountID,
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

        if (DistAccountID.length === 0) {
            $("#DistAccountID").css("border-color", "red");
            Test = false;
        } else {
            $("#DistAccountID").css("border-color", "");
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

        $(".BranchAccDistPerTbl").each(function () {
            var value = $(this).text();
            // add only if the value is number
            if (!isNaN(value) && value.length != 0) {
                sumPercentage += parseFloat(value);
            }
        });

        if (parseFloat(sumPercentage) + parseFloat(Percentage) > 100) {
            $("#Percentage").css("border", "1px solid red");
            Test = false;
            $("#AddError").text("Wrong Percentage..!");
        }


        if (Test === true) {

            $.ajax({
                type: "POST",
                url: "/B_AddDistToAccount/AddDistributionAccount?AccountID=" + AccountID + "&AnalyticAccountID=" + AnalyticAccountID + "&DistAccountID=" + DistAccountID + "&Percentage=" + Percentage + "&AccountChartID=" + AccountChartID,
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
            url: "/B_AddDistToAccount/GetDistributionAccounts?AccountID=" + AccountID + "&AnalyticAccountID=" + AnalyticAccountID + "&AccountChartID=" + AccountChartID,
            success: function (result) {

                for (var i = 0; i < result.length; i++) {

                    var Data = "<tr class='row_" + result[i].ID + "'>" +
                        "<td>" + result[i].C_DistAccountID + "</td>" +
                        "<td>" + result[i].C_DistAccountName + "</td>" +
                        "<td class='BranchAccDistPerTbl'>" + result[i].Percentage + "</td>" +
                        "<td>" + '<a href="#" onclick="DeleteCompDistribution(\'' + result[i].ID + '\')"><span class="fa fa-trash-o"></span></a>' + "</td>" +
                        "</tr>";
                    tableData.append(Data);
                }

            }
        });

    }

});

//Delete Distribution from tha analytic that related to this Account ID
function DeleteCompDistribution(DistID) {

    $("#AccountDistributionID").text(DistID);

    $("#DeleteConfirmation").modal("show");

}

function CompConfirmDelete() {

    var AccountDistributionID = $("#AccountDistributionID").text();

    $.ajax({
        type: "POST",
        url: "/B_AddDistToAccount/DeleteAccountDistributionComp?AccountDistributionID=" + AccountDistributionID,
        success: function (result) {
            $("#DeleteConfirmation").modal("hide");
            $(".row_" + AccountDistributionID).remove();
        }
    });
}