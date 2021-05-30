$(document).ready(function () {

    var companyID = $("#CompanyID").val();
    GetCompanyCost(companyID);

    $("#CostCenterID").keyup(function (event) {

        event.preventDefault();

        if (event.keyCode === 13) {
            $("#AddCostCenterToCompany").click();
        }

        var CostCenterID = $(this).val();

        if (CostCenterID.length === 0) {
            $(this).css("border-color", "red");

        } else if (!isNaN(CostCenterID[0])/*!CostCenterID.match("^[A-Za-z].*")*/) {
            $(this).css("border-color", "red");
            $("#GlobalError").text("Cost Center ID Must Start With Character..!");

        } else {
            $(this).css("border-color", "");
            $("#GlobalError").text("");

        }
    }).focusout(function () {

        var CostCenterID = $(this).val();

        if (CostCenterID.length === 0) {
            $(this).css("border-color", "red");
        } else {
            $(this).css("border-color", "");
        }

    });

    $("#CostCenterName").keyup(function (event) {

        event.preventDefault();

        if (event.keyCode === 13) {
            $("#AddCostCenterToCompany").click();
        }

        var CostCenterName = $(this).val();

        if (CostCenterName.length === 0) {
            $(this).css("border-color", "red");
        } else {
            $(this).css("border-color", "");
        }
    }).focusout(function () {

        var CostCenterName = $(this).val();

        if (CostCenterName.length === 0) {
            $(this).css("border-color", "red");
        } else {
            $(this).css("border-color", "");
        }

    });


    $("#AddCostCenterToCompany").click(function () {

        var CompanyID = $("#CompanyID").val();

        var CostCenterID = $("#CostCenterID").val();

        var CostCenterName = $("#CostCenterName").val();

        var Test = true;

        if (CompanyID.length === 0) {
            $("#CompanyID").css("border-color", "red");
            Test = false;
        } else {
            $("#CompanyID").css("border-color", "");
        }

        if (CostCenterID.length === 0) {
            $("#CostCenterID").css("border-color", "red");
            Test = false;
        } else if (!isNaN(CostCenterID[0])/*!CostCenterID.match("^[A-Za-z].*")*/) {
            $("#CostCenterID").css("border-color", "red");
            $("#GlobalError").text("Cost Cenetr ID Must Start With Character..!");
            Test = false;

        } else {
            $("#CostCenterID").css("border-color", "");
        }

        if (CostCenterName.length === 0) {
            $("#CostCenterName").css("border-color", "red");
            Test = false;
        } else {
            $("#CostCenterName").css("border-color", "");
        }

        if (Test === true) {
            if ($(this).attr("data-value")) {
                $(document).find(".DeleteItem[data-value='" + $(this).attr("data-value") + "']").trigger("click");
                $(this).attr("data-value","")
            }
            RunAfterAjax(function () {
                $.ajax({
                    type: "POST",
                    url: "/C_CreateCostCenter/AddCompCostCenter?CompanyID=" + CompanyID + "&CostCenterID=" + CostCenterID + "&CostCenterName=" + CostCenterName,
                    success: function (result) {
                        if (result == "False") {
                            $("#GlobalError").text("Not Valid Cost Center ID..!");
                            $("#CostCenterID").css("border-color", "red");
                            $("#SaveSuccess").text("");
                        } else {
                            $("#GlobalError").text("");
                            $("#CostCenterID").css("border-color", "");
                            $("#CostCenterID").val("");
                            $("#CostCenterID").focus();
                            $("#CostCenterName").val("");
                            $("#SaveSuccess").text("Saved..");
                            GetCompanyCost(CompanyID);
                        }
                    }
                });
            })
           
        }
    });

});

function GetCompanyCost(companyID) {

    var tble = $("#CompanyCostData");

    tble.html("");

    $.ajax({
        type: "GET",
        url: "/C_CreateCostCenter/GetCostCenter?companyID=" + companyID,
        success: function (result) {
            if (result.length === 0) {
                tble.append("<tr><td colspan='2' class='text-danger'>No Cost Center Created To This Company..!</td></tr>")
            } else {

                for (var i = 0; i < result.length; i++) {

                    var data = "<tr>"
                        + "<td class='Id'>" + result[i].CostCenterID + "</td>"
                        + "<td class='Name'>" + result[i].CostCenterName + "</td>"
                        + "<td><button class='btn btn-secondary btn-sm mr-1 EditItem'><span class='fa fa-edit'>"
                        + "</span></button>"
                        + "<button class='btn btn-danger btn-sm DeleteItem' data-value='" + result[i].CostCenterID+"'><span class='fa fa-trash-o'></span></button></td>"
                        + "</tr>"

                    tble.append(data);

                }
                ReTranslateTh();
            }
        }
    });
}

$(document).on("click", ".DeleteItem", function () {
    var Id = $(this).parents("tr").find(".Id").text();
    var Tr = $(this).parents("tr");
    $(Tr).find(".DeleteItem").attr("disabled", "disabled")
    $.ajax({
        url: "/C_CreateCostCenter/DelCostCenter?Id=" + Id,
        method: "POST",
        success: function (data) {
            if (data) {
                $(Tr).remove();
            } else {
                $(Tr).find(".DeleteItem").removeAttr("disabled")
            }
        }
    })
})
$(document).on("click", ".EditItem", function () {
    var Tr = $(this).parents("tr");
    $("#CostCenterID").val($(Tr).find(".Id").text())
    $("#CostCenterName").val($(Tr).find(".Name").text())
    $("#AddCostCenterToCompany").attr("data-value", $(Tr).find(".Id").text());
})