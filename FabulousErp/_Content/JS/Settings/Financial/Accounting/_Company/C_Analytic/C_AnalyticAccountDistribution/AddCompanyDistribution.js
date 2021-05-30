$(document).ready(function () {

    var companyID = $("#CompanyID").val();

    FilterDistribByComp(companyID);

});

function ClearInCBFChange() {
    $("#DistributionInputs").hide();
    $("#AddDistributionCompBtn").hide();
    $("#AccountDistributionTbl").html("");
    $("#DistributionID").val("");
    $("#DistributionName").val("");
    $("#DistributionID").css("border-color", "");
    $("#DistributionName").css("border-color", "");
    $("#AccessError").text("");
}

//$("#CompanyID").change(function () {

//    var CompanyID = $(this).val();

//    ClearInCBFChange();

//    if (CompanyID.length > 0) {

//        $(this).css("border-color", "");

//        $.ajax({
//            type: "GET",
//            url: "/C_AnalyticAccountDistribution/GetCompanyName?CompanyID=" + CompanyID,
//            success: function (result) {
//                if (result == "False") {
//                    $("#AccessError").text("You Not Have Access To This Company..!");
//                    $("#CompAnalytic").hide();
//                    $("#CompanyName").val("");
//                    $(this).css("border-color", "red");
//                } else {
//                    $("#AccessError").text("");
//                    $("#CompanyName").val(result);
//                    $("#CompAnalytic").show();
//                    FilterDistribByComp(CompanyID);
//                }
//            }
//        });

//    } else {
//        $("#AccessError").text("")
//        $("#CompAnalytic").hide();
//        $("#CompanyName").val("");
//        $(this).css("border-color", "red");
//    }
//});


function FilterDistribByComp(CompanyID) {
    $.ajax({
        type: "GET",
        url: "/C_AnalyticAccountDistribution/FilterAnalyticIDForComp?CompanyID=" + CompanyID,
        success: function (data) {

            $("select#CompAnalyticID").empty();

            if (data.length == 0) {

                $("#CompAnalyticName").val("");

                $("#CompAnalyticID").append($('<option/>', {
                    value: -1,
                    text: "No Analytic Created in this Company!"

                })
                );

            } else {

                $("#CompAnalyticName").val("");

                $("#CompAnalyticID").append($('<option/>', {
                    value: -1,
                    text: "-Choose-"

                })
                );

                $.each(data, function (index, row) {

                    $("#CompAnalyticID").append("<option value='" + row.AnalyticID + "'>" + row.AnalyticID + "</option>");

                });
            }
        }
    });

}


$("#CompAnalyticID").change(function () {

    var AnalyticID = $(this).val();

    if (AnalyticID == "-1") {
        $(this).css("border-color", "red");

        ClearInCBFChange();

        $("#CompAnalyticName").val("");

    } else {
        $(this).css("border-color", "");

        $("#DistributionInputs").show();
        $("#AddDistributionCompBtn").show();

        $.ajax({
            type: "GET",
            url: "/C_AnalyticAccountDistribution/GetCompAnalyticName?AnalyticID=" + AnalyticID,
            success: function (result) {
                $("#CompAnalyticName").val(result.Name);
                GetCompAnalyticData();
            }
        });
    }
});


$("#DistributionID").keyup(function (event) {

    event.preventDefault();

    if (event.keyCode === 13) {
        $("#AddDistributionCompBtn").click();
    }

    var DistributionID = $(this).val();

    if (DistributionID.length === 0) {
        $(this).css("border-color", "red");
    }
    else {
        $(this).css("border-color", "");
    }
}).focusout(function () {

    var DistributionID = $(this).val();

    if (DistributionID.length === 0) {
        $(this).css("border-color", "red");
    }
    else {
        $(this).css("border-color", "");
    }

});

$("#DistributionName").keyup(function (event) {

    event.preventDefault();

    if (event.keyCode === 13) {
        $("#AddDistributionCompBtn").click();
    }

    var DistributionName = $(this).val();

    if (DistributionName.length === 0) {
        $(this).css("border-color", "red");
    }
    else {
        $(this).css("border-color", "");
    }
}).focusout(function () {

    var DistributionName = $(this).val();

    if (DistributionName.length === 0) {
        $(this).css("border-color", "red");
    }
    else {
        $(this).css("border-color", "");
    }

    });

$("#AddDistributionCompBtn").click(function () {

    var CompAnalyticID = $("#CompAnalyticID").val();

    var DistributionID = $("#DistributionID").val();

    var DistributionName = $("#DistributionName").val();

    var Test = true;


    // validation in button Save---------------------------
    if (CompAnalyticID == "-1") {
        $("#CompAnalyticID").css("border-color", "red");
        Test = false;
    } else {
        $("#CompAnalyticID").css("border-color", "");
    }

    if (DistributionID.length === 0) {
        $("#DistributionID").css("border-color", "red");
        Test = false;
    } else {
        $("#DistributionID").css("border-color", "");
    }

    if (DistributionName.length === 0) {
        $("#DistributionName").css("border-color", "red");
        Test = false;
    } else {
        $("#DistributionName").css("border-color", "");
    }
    //-------------------------------------------------------------

    if (Test === true) {

        if ($(this).attr("data-value")) {
            $("#AccountDistributionID").text($(this).attr("data-value"));
            CompConfirmDelete()
            $(document).find(".DeleteItem[data-value='" + $(this).attr("data-value") + "']").trigger("click");
            $(this).attr("data-value","")
        }
        RunAfterAjax(function () {
            $.ajax({
                type: "POST",
                url: "/C_AnalyticAccountDistribution/SaveDistributionRecordComp?CompAnalyticID=" + CompAnalyticID + "&DistributionID=" + DistributionID + "&DistributionName=" + DistributionName,
                success: function (result) {
                    if (result === "False") {
                        $("#DistributionID").css("border-color", "red");
                        $("#GlobalError").text("Account Distribution ID not Valid..!");
                    } else {
                        $("#DistributionID").css("border-color", "");
                        $("#GlobalError").text("");
                        $("#DistributionID").val("");
                        $("#DistributionName").val("");
                        $("#DistributionID").focus();

                        GetCompAnalyticData();
                    }
                }
            });
        })
      
    }
});


function GetCompAnalyticData() {

    var CompAnalyticID = $("#CompAnalyticID").val();

    var tableData = $("#AccountDistributionTbl");
    tableData.html("");

    $.ajax({
        type: "GET",
        url: "/C_AnalyticAccountDistribution/GetCompAnalyticDistributionData?CompAnalyticID=" + CompAnalyticID,
        success: function (result) {

            for (var i = 0; i < result.length; i++) {

                var Data = "<tr class='row_" + result[i].AccountDistributionID + "'>" +
                    "<td class='Id' data-value='"+result[i].AccountDistributionID+"'>" + result[i].AccountDistributionID + "</td>" +
                    "<td class='Name'>" + result[i].AccountDistributionName + "</td>" +
                    "<td>" + '<button onclick="DeleteCompDistribution(\'' + result[i].AccountDistributionID + '\')" class="btn btn-danger DeleteItem"><span class="fa fa-trash-o"></span></button>' +
                    "<button class='btn btn-secondary btn-sm mr-1 EditItem'><span class='fa fa-edit'></span></button>"
                    +"</td>" 
                    +"</tr>";
                tableData.append(Data);
            }
        }
    });
}

$(document).on("click", ".EditItem", function () {
    var Tr = $(this).parents("tr");
    $("#DistributionID").val($(Tr).find(".Id").text())
    $("#DistributionName").val($(Tr).find(".Name").text())
    $("#AddDistributionCompBtn").attr("data-value", $(Tr).find(".Id").text());
})

function DeleteCompDistribution(AccountDistributionID) {

    $("#AccountDistributionID").text(AccountDistributionID);

    $("#DeleteConfirmation").modal("show");
}


function CompConfirmDelete() {

    var AccountDistributionID = $("#AccountDistributionID").text();

    $.ajax({
        type: "POST",
        url: "/C_AnalyticAccountDistribution/DeleteAccountDistributionComp?AccountDistributionID=" + AccountDistributionID,
        success: function (result) {
            $("#DeleteConfirmation").modal("hide");
            $(".row_" + AccountDistributionID).remove();
        }
    });
}