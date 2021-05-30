$(document).ready(function () {

    var factoryIDFB = $("#FactoryID").val();
    if (factoryIDFB.length > 0) {
        GetFactoryAnalytic(factoryIDFB);
    }

    $("#FactoryID").change(function () {

        var FactoryID = $(this).val();

        $("#GlobalError").text("");
        $("#FactoryAnalyticData").html("");
        $("#FactoryName").val("");

        if (FactoryID.length === 0) {

            $(this).css("border-color", "red");
            $("#AddAnalyticToFactory").prop("disabled", false);

        } else {

            $(this).css("border-color", "");

            $.ajax({
                type: "GET",
                url: "/F_CreateAnalyticAccounts/GetFactoryName?FactoryID=" + FactoryID,
                success: function (result) {

                    if (result == "False") {

                        $("#FactoryName").val();
                        $("#AddAnalyticToFactory").prop("disabled", true);
                        $("#GlobalError").text("You not Have Access To This Factory..!");

                    } else {
                        $("#FactoryName").val(result);
                        $("#AddAnalyticToFactory").prop("disabled", false);
                        GetFactoryAnalytic(FactoryID);
                    }
                }
            });
        }
    });

    $("#AnalyticID").keyup(function (event) {

        event.preventDefault();

        if (event.keyCode === 13) {
            $("#AddAnalyticToFactory").click();
        }

        var AnalyticID = $(this).val();

        if (AnalyticID.length === 0) {
            $(this).css("border-color", "red");
        } else {
            $(this).css("border-color", "");
        }
    }).focusout(function () {

        var AnalyticID = $(this).val();

        if (AnalyticID.length === 0) {
            $(this).css("border-color", "red");

        } else if (!AnalyticID.match("^[A-Za-z].*")) {
            $(this).css("border-color", "red");
            $("#GlobalError").text("Analytic Accounts ID Must Start With Character..!");

        } else {
            $(this).css("border-color", "");
        }

    });

    $("#AnalyticName").keyup(function (event) {

        event.preventDefault();

        if (event.keyCode === 13) {
            $("#AddAnalyticToFactory").click();
        }

        var AnalyticName = $(this).val();

        if (AnalyticName.length === 0) {
            $(this).css("border-color", "red");
        } else {
            $(this).css("border-color", "");
        }
    }).focusout(function () {

        var AnalyticName = $(this).val();

        if (AnalyticName.length === 0) {
            $(this).css("border-color", "red");
        } else {
            $(this).css("border-color", "");
        }

    });


    $("#AddAnalyticToFactory").click(function () {

        var FactoryID = $("#FactoryID").val();

        var AnalyticID = $("#AnalyticID").val();

        var AnalyticName = $("#AnalyticName").val();

        var Test = true;

        if (FactoryID.length === 0) {
            $("#FactoryID").css("border-color", "red");
            Test = false;
        } else {
            $("#FactoryID").css("border-color", "");
        }

        if (AnalyticID.length === 0) {
            $("#AnalyticID").css("border-color", "red");
            Test = false;

        } else if (!AnalyticID.match("^[A-Za-z].*")) {
            $("#AnalyticID").css("border-color", "red");
            $("#GlobalError").text("Analytic Accounts ID Must Start With Character..!");
            Test = false;

        } else {
            $("#AnalyticID").css("border-color", "");
        }

        if (AnalyticName.length === 0) {
            $("#AnalyticName").css("border-color", "red");
            Test = false;
        } else {
            $("#AnalyticName").css("border-color", "");
        }


        if (Test === true) {

            $.ajax({
                type: "POST",
                url: "/F_CreateAnalyticAccounts/AddAnalyticAccounts?FactoryID=" + FactoryID + "&AnalyticID=" + AnalyticID + "&AnalyticName=" + AnalyticName,
                success: function (result) {
                    if (result === "False") {
                        $("#AnalyticID").css("border-color", "red");
                        $("#GlobalError").text("Analytic Account ID not valid..!");
                        $("#SaveSuccess").text("");
                    } else {
                        $("#AnalyticID").css("border-color", "");
                        $("#GlobalError").text("");
                        $("#AnalyticID").val("");
                        $("#AnalyticName").val("");
                        $("#SaveSuccess").text("Saved..");
                        $("#AnalyticID").focus();
                        GetFactoryAnalytic(FactoryID)
                    }
                }
            });
        }
    });

});


function GetFactoryAnalytic(factoryID) {

    var tbl = $("#FactoryAnalyticData");

    tbl.html("");

    $.ajax({
        type: "GET",
        url: "/F_CreateAnalyticAccounts/GetAnalyticAccounts?factoryID=" + factoryID,
        success: function (result) {
            if (result.length === 0) {
                tbl.append("<tr><td colspan='2' class='text-danger'>No Analytic Created To This Factory..!</td></tr>")
            } else {

                for (var i = 0; i < result.length; i++) {

                    var data = "<tr>"
                        + "<td>" + result[i].AnalyticAccountID + "</td>"
                        + "<td>" + result[i].AnalyticAccountName + "</td>"
                        + "</tr>"

                    tbl.append(data);
                }
            }
        }
    });
}