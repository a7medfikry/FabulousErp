$(document).ready(function () {

    var companyID = $("#CompanyID").val();
    GetCompanyAnalytic(companyID);

    $("#AnalyticID").keyup(function (event) {

        event.preventDefault();

        if (event.keyCode === 13) {
            $("#AddAnalyticToCompany").click();
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

        }
        //else if (!AnalyticID.match("^[A-Za-z].*")) {
        //    $(this).css("border-color", "red");
        //    $("#GlobalError").text("Analytic Accounts ID Must Start With Character..!");
        //}
        else {
            $(this).css("border-color", "");
        }

    });

    $("#AnalyticName").keyup(function (event) {

        event.preventDefault();

        if (event.keyCode === 13) {
            $("#AddAnalyticToCompany").click();
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


    $("#AddAnalyticToCompany").click(function () {

        var CompanyID = $("#CompanyID").val();

        var AnalyticID = $("#AnalyticID").val();

        var AnalyticName = $("#AnalyticName").val();

        var Test = true;

        if (CompanyID.length === 0) {
            $("#CompanyID").css("border-color", "red");
            Test = false;
        } else {
            $("#CompanyID").css("border-color", "");
        }

        if (AnalyticID.length === 0) {
            $("#AnalyticID").css("border-color", "red");
            Test = false;

        }
        //else if (!AnalyticID.match("^[A-Za-z].*")) {
        //    $("#AnalyticID").css("border-color", "red");
        //    $("#GlobalError").text("Analytic Accounts ID Must Start With Character..!");
        //    Test = false;

        //}
        else {
            $("#AnalyticID").css("border-color", "");
        }

        if (AnalyticName.length === 0) {
            $("#AnalyticName").css("border-color", "red");
            Test = false;
        } else {
            $("#AnalyticName").css("border-color", "");
        }

        if ($("#AddAnalyticToCompany").attr("data-update")) {
            $("." + $("#AddAnalyticToCompany").attr("data-update")).parents("tr").find(".DeleteItem").trigger("click");
        }

        if (Test === true) {
            RunAfterAjax(function () {
                $.ajax({
                    type: "POST",
                    url: "/C_CreateAnalyticAccounts/AddAnalyticAccounts?CompanyID=" + CompanyID + "&AnalyticID=" + AnalyticID + "&AnalyticName=" + AnalyticName,
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
                            GetCompanyAnalytic(CompanyID);
                        }
                        $("#AddAnalyticToCompany").removeAttr("data-update")
                    }
                });
            })

        }
    });

});

function GetCompanyAnalytic(companyID) {

    var tble = $("#CompanyAnalyticData");

    tble.html("");

    $.ajax({
        type: "GET",
        url: "/C_CreateAnalyticAccounts/GetAnalyticAccounts?companyID=" + companyID,
        success: function (result) {
            if (result.length === 0) {
                tble.append("<tr><td colspan='2' class='text-danger'>No Analytic Created To This Company..!</td></tr>")
            } else {

                for (var i = 0; i < result.length; i++) {

                    var data = "<tr>"
                        + "<td class='" + result[i].AnalyticAccountID.replace(" ","_")+"'>" + result[i].AnalyticAccountID + "</td>"
                        + "<td>" + result[i].AnalyticAccountName + "</td>"
                        + '<td><button class="btn btn-secondary btn-sm mr-1 EditItem"><span class="fa fa-edit"></span></button><button class="btn btn-danger btn-sm DeleteItem" data-value="' + result[i].AnalyticAccountID + '"><span class="fa fa-trash-o"></span></button></td>'
                        + "</tr>"

                    tble.append(data);
                }
            }
        }
    });
}
$(document).on("click", ".DeleteItem", function () {
    var Tr = $(this).parents("tr");
    $(this).attr("disabled", "disabled")
    $.ajax({
        url: "/C_CreateAnalyticAccounts/DelAnalyticAccounts?AnalyticAccountID=" + $(this).attr("data-value"),
        method: "POST",
        success: function (data) {
            $(Tr).remove();
        }
    })
})
$(document).on("click", ".EditItem", function () {
    var Tr = $(this).parents('tr')
    $("#AnalyticID").val($(Tr).find("td:eq(0)").text())
    $("#AnalyticName").val($(Tr).find("td:eq(1)").text())
    $("#AddAnalyticToCompany").attr("data-update", $(Tr).find("td:eq(0)").text().replace(" ","_"));
})