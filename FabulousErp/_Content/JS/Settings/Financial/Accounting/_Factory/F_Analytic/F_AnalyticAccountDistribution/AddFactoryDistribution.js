$(document).ready(function () {

    var factoryIDFF = $("#FactoryIDFF").val();
    if ($("#FactoryIDFF").length > 0) {
        FilterDistribByFactory(factoryIDFF);
    }

});

function ClearInCBFChange() {
    $("#DistributionInputs").hide();
    $("#AddDistributionFactoryBtn").hide();
    $("#AccountDistributionTbl").html("");
    $("#DistributionID").val("");
    $("#DistributionName").val("");
    $("#DistributionID").css("border-color", "");
    $("#DistributionName").css("border-color", "");
    $("#AccessError").text("");
}

$("#FactoryID").change(function () {

    var FactoryID = $(this).val();

    ClearInCBFChange();

    if (FactoryID.length > 0) {

        $(this).css("border-color", "");

        $.ajax({
            type: "GET",
            url: "/F_AnalyticAccountDistribution/GetFatoryName?FactoryID=" + FactoryID,
            success: function (result) {

                if (result == "False") {
                    $("#AccessError").text("You Not Have Access To This Factory..!");
                    $("#FactoryAnalytic").hide();
                    $("#FactoryName").val("");
                    $(this).css("border-color", "red");
                } else {
                    $("#AccessError").text("");
                    $("#FactoryName").val(result);
                    $("#FactoryAnalytic").show();
                    FilterDistribByFactory(FactoryID);
                }
            }
        });
    } else {
        $("#AccessError").text("");
        $("#FactoryAnalytic").hide();
        $("#FactoryName").val("");
        $(this).css("border-color", "red");
    }
});

function FilterDistribByFactory(FactoryID) {
    $.ajax({
        type: "GET",
        url: "/F_AnalyticAccountDistribution/FilterAnalyticIDForFactory?FactoryID=" + FactoryID,
        success: function (data) {

            $("select#FactoryAnalyticID").empty();

            if (data.length == 0) {

                $("#FactoryAnalyticName").val("");

                $("#FactoryAnalyticID").append($('<option/>', {
                    value: -1,
                    text: "No Analytic Created in this Factory!"

                })
                );

            } else {

                $("#FactoryAnalyticName").val("");

                $("#FactoryAnalyticID").append($('<option/>', {
                    value: -1,
                    text: "-Choose-"

                })
                );

                $.each(data, function (index, row) {

                    $("#FactoryAnalyticID").append("<option value='" + row.AnalyticID + "'>" + row.AnalyticID + "</option>");

                });
            }
        }
    });
}

$("#FactoryAnalyticID").change(function () {

    var AnalyticID = $(this).val();

    if (AnalyticID == "-1") {

        ClearInCBFChange();

        $(this).css("border-color", "red");
        $("#FactoryAnalyticName").val("");
    } else {
        $(this).css("border-color", "");

        $("#DistributionInputs").show();
        $("#AddDistributionFactoryBtn").show();

        $.ajax({
            type: "GET",
            url: "/F_AnalyticAccountDistribution/GetFactoryAnalyticName?AnalyticID=" + AnalyticID,
            success: function (result) {
                $("#FactoryAnalyticName").val(result.Name);
                GetFactoryAnalyticData();
            }
        });
    }
});


$("#DistributionID").keyup(function (event) {

    event.preventDefault();

    if (event.keyCode === 13) {
        $("#AddDistributionFactoryBtn").click();
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
        $("#AddDistributionFactoryBtn").click();
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


$("#AddDistributionFactoryBtn").click(function () {

    var FactoryAnalyticID = $("#FactoryAnalyticID").val();

    var DistributionID = $("#DistributionID").val();

    var DistributionName = $("#DistributionName").val();

    var Test = true;


    // validation in button Save---------------------------

    if (FactoryAnalyticID == "-1") {
        $("#FactoryAnalyticID").css("border-color", "red");
        Test = false;
    } else {
        $("#FactoryAnalyticID").css("border-color", "");
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

        $.ajax({
            type: "POST",
            url: "/F_AnalyticAccountDistribution/SaveDistributionRecordFactory?FactoryAnalyticID=" + FactoryAnalyticID + "&DistributionID=" + DistributionID + "&DistributionName=" + DistributionName,
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

                    GetFactoryAnalyticData();
                }
            }
        });
    }
});


function GetFactoryAnalyticData() {

    var FactoryAnalyticID = $("#FactoryAnalyticID").val();

    var tableData = $("#AccountDistributionTbl");
    tableData.html("");

    $.ajax({
        type: "GET",
        url: "/F_AnalyticAccountDistribution/GetFactoryAnalyticDistributionData?FactoryAnalyticID=" + FactoryAnalyticID,
        success: function (result) {

            for (var i = 0; i < result.length; i++) {

                var Data = "<tr class='row_" + result[i].AccountDistributionID + "'>" +
                    "<td>" + result[i].AccountDistributionID + "</td>" +
                    "<td>" + result[i].AccountDistributionName + "</td>" +
                    "<td>" + '<a href="#" onclick="DeleteFactoryDistribution(\'' + result[i].AccountDistributionID + '\')"><span class="fa fa-trash-o"></span></a>' + "</td>" +
                    "</tr>";
                tableData.append(Data);
            }
        }
    });
}


function DeleteFactoryDistribution(AccountDistributionID) {

    $("#AccountDistributionID").text(AccountDistributionID);

    $("#DeleteConfirmation").modal("show");

}


function FactoryConfirmDelete() {

    var AccountDistributionID = $("#AccountDistributionID").text();

    $.ajax({
        type: "POST",
        url: "/F_AnalyticAccountDistribution/DeleteAccountDistributionFactory?AccountDistributionID=" + AccountDistributionID,
        success: function (result) {
            $("#DeleteConfirmation").modal("hide");
            $(".row_" + AccountDistributionID).remove();
        }
    });
}