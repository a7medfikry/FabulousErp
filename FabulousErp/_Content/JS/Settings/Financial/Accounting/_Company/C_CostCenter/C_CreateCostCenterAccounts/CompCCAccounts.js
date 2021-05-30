$(document).ready(function () {

    var companyID = $("#CompanyID").val();

    FilterCostCenterByComp(companyID);

});

function HideImpThings() {
    $("#CCAccountsInputs").hide();
    $("#AddCCAccountsCompBtn").hide();
    $("#CCAccountsTbl").html("");
    $("#CostAccountID").css("border-color", "");
    $("#CostAccountName").css("border-color", "");
    $("#CostAccountID").val("");
    $("#CostAccountName").val("");
    $("#AccessError").text("");
}

//$("#CompanyID").change(function () {

//    var CompanyID = $(this).val();

//    HideImpThings();

//    if (CompanyID.length > 0) {

//        $(this).css("border-color", "");


//        $.ajax({
//            type: "GET",
//            url: "/C_CreateCostCenterAccounts/GetCompanyName?CompanyID=" + CompanyID,
//            success: function (result) {

//                if (result == "False") {
//                    $("#AccessError").text("You Not Have Access To This Company..!");
//                    $("#FilterCostCenterIDComp").hide();
//                    $("#CompanyName").val("");
//                    $(this).css("border-color", "red");
//                } else {
//                    $("#CompanyName").val(result);
//                    FilterCostCenterByComp(CompanyID);
//                    $("#FilterCostCenterIDComp").show();
//                    $("#AccessError").text("");
//                }
//            }
//        });

//    } else {
//        $("#AccessError").text("");
//        $("#FilterCostCenterIDComp").hide();
//        $("#CompanyName").val("");
//        $(this).css("border-color", "red");
//    }
//});


function FilterCostCenterByComp(CompanyID) {
    $.ajax({
        type: "GET",
        url: "/C_CreateCostCenterAccounts/FilterCostCenterIDForComp?CompanyID=" + CompanyID,
        success: function (data) {

            $("#CompCostCenterID").empty();

            if (data.length == 0) {

                $("#CompCostCenterName").val("");

                $("#CompCostCenterID").append($('<option/>', {
                    value: -1,
                    text: "No Cost Center Created in this Company!"

                })
                );

            } else {

                $("#CompCostCenterName").val("");

                $("#CompCostCenterID").append($('<option/>', {
                    value: -1,
                    text: "-Choose-"

                })
                );

                $.each(data, function (index, row) {

                    $("#CompCostCenterID").append("<option value='" + row.CostCenterID + "'>" + row.CostCenterID + "</option>");

                });
            }
        }
    });
}


$("#CompCostCenterID").change(function () {

    var CompCostCenterID = $(this).val();

    if (CompCostCenterID == "-1") {

        HideImpThings();

        $(this).css("border-color", "red");
        $("#CompCostCenterName").val("");

    } else {

        GetCompCCAccountsData();

        $("#CCAccountsInputs").show();
        $("#AddCCAccountsCompBtn").show();

        $(this).css("border-color", "");

        $.ajax({
            type: "GET",
            url: "/C_CreateCostCenterAccounts/GetCompCostCenter?CostCenterID=" + CompCostCenterID,
            success: function (result) {
                $("#CompCostCenterName").val(result);
            }
        });
    }
});

function GetCompCCAccountsData() {

    var CompCostCenterID = $("#CompCostCenterID").val();

    var tableData = $("#CCAccountsTbl");
    tableData.html("");

    $.ajax({
        type: "GET",
        url: "/C_CreateCostCenterAccounts/GetCompCCAccountsData?CompCostCenterID=" + CompCostCenterID,
        success: function (result) {

            for (var i = 0; i < result.length; i++) {

                var Data = "<tr class='row_" + result[i].CostAccountID + "'>" +
                    "<td width='20%' class='Id' data-value='" + result[i].CostAccountID+"'>" + result[i].CostAccountID + "</td>" +
                    "<td width='60%' class='Name'>" + result[i].CostAccountName + "</td>" +
                    "<td width='20%'>" +
                    '<button class="btn btn-danger DeleteItem" data-value="' + result[i].CostAccountID+'" onclick="DeleteCompCCAccount(\'' + result[i].CostAccountID + '\')"><span class="fa fa-trash-o"></span></a>' 
                    +"<button class='btn btn-secondary btn-sm mr-1 EditItem'><span class='fa fa-edit'></span></button>"
                    +"</td>"
                   +"</tr>";
                tableData.append(Data);
            }
        }
    });

}

$(document).on("click", ".EditItem", function () {
    var Tr = $(this).parents("tr");
    $("#CostAccountID").val($(Tr).find(".Id").text())
    $("#CostAccountName").val($(Tr).find(".Name").text())
    $("#AddCCAccountsCompBtn").attr("data-value", $(Tr).find(".Id").text());
})
$("#AddCCAccountsCompBtn").click(function () {

    var CostCenterID = $("#CompCostCenterID").val();

    var CostAccountID = $("#CostAccountID").val();

    var CostAccountName = $("#CostAccountName").val();

    var Test = true;

    if (CostCenterID == "-1") {
        $("#CompCostCenterID").css("border-color", "red");
        Test = false;
    } else {
        $("#CompCostCenterID").css("border-color", "");
    }

    if (CostAccountID.length === 0) {
        $("#CostAccountID").css("border-color", "red");
        $("#CostAccountID").focus();
        Test = false;
    } else {
        $("#CostAccountID").css("border-color", "");
    }

    if (CostAccountName.length === 0) {
        $("#CostAccountName").css("border-color", "red");
        $("#CostAccountName").focus();
        Test = false;
    } else {
        $("#CostAccountName").css("border-color", "");
    }

    if (Test === true) {
        if ($(this).attr("data-value")) {
            $("#CCAccountID").text($(this).attr("data-value"));
            CompConfirmDelete()
            $(this).attr("data-value", "");
            //$(document).find(".DeleteItem[data-value='" + $(this).attr("data-value") + "']").trigger("click");
        }
        RunAfterAjax(function () {
            $.ajax({
                type: "POST",
                url: "/C_CreateCostCenterAccounts/SaveRecordCompCCAccounts?CostCenterID=" + CostCenterID + "&CostAccountID=" + CostAccountID + "&CostAccountName=" + CostAccountName,
                success: function (result) {
                    if (result == "False") {
                        $("#CostAccountID").css("border-color", "red");
                        $("#GlobalError").text("Cost Account ID not Valid..!");
                        $("#CostAccountID").focus();
                    } else {
                        $("#CostAccountID").css("border-color", "");
                        $("#GlobalError").text("");
                        $("#CostAccountID").val("");
                        $("#CostAccountName").val("");
                        $("#CostAccountID").focus();

                        GetCompCCAccountsData();
                    }
                }
            });
        })
       
    }
});



// Validation Cost center Accounts Content
$("#CostAccountID").keyup(function (event) {

    event.preventDefault();

    if (event.keyCode === 13) {
        $("#AddCCAccountsCompBtn").click();
    }

    var CostAccountID = $(this).val();

    if (CostAccountID.length === 0) {
        $(this).css("border-color", "red");
    }
    else {
        $(this).css("border-color", "");
    }
}).focusout(function () {

    var CostAccountID = $(this).val();

    if (CostAccountID.length === 0) {
        $(this).css("border-color", "red");
    }
    else {
        $(this).css("border-color", "");
    }

});

$("#CostAccountName").keyup(function (event) {

    event.preventDefault();

    if (event.keyCode === 13) {
        $("#AddCCAccountsCompBtn").click();
    }

    var CostAccountName = $(this).val();

    if (CostAccountName.length === 0) {
        $(this).css("border-color", "red");
    }
    else {
        $(this).css("border-color", "");
    }
}).focusout(function () {

    var CostAccountName = $(this).val();

    if (CostAccountName.length === 0) {
        $(this).css("border-color", "red");
    }
    else {
        $(this).css("border-color", "");
    }

});
//---------------------------------------------------------------------------------


function DeleteCompCCAccount(CostAccountID) {
    $("#CCAccountID").text(CostAccountID);

    $("#DeleteConfirmation").modal("show");
}


function CompConfirmDelete() {

    var CCAccountID = $("#CCAccountID").text();

    $.ajax({
        type: "POST",
        url: "/C_CreateCostCenterAccounts/DeleteCostAccountComp?CCAccountID=" + CCAccountID,
        success: function (result) {
            $("#DeleteConfirmation").modal("hide");
            $(".row_" + CCAccountID).remove();
        }
    });

}