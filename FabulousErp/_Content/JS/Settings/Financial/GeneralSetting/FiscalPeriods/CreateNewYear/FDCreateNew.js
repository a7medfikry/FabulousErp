var dateformat = /^(19|20)\d\d-(0\d|1[012])-(0\d|1\d|2\d|3[01])$/;
/*
function ShowPopCreate() {
    $("#CreateNewYearModal").modal("show");
}
*/
$("#CNStartDate").prop("disabled", true);
$("#CNEndDate").prop("disabled", true);

$("#CNFiscalYearID").change(function () {

    var ID = $(this).val();

    if (ID.length > 0) {

        clearInputs();

        $("#CNFiscalYearID").removeClass('input-error');
        $("#CNFiscalYearIDError").text("");

        $("#CNStartDate").prop("disabled", false);
        $("#CNEndDate").prop("disabled", false);

        $.ajax({

            type: "GET",
            url: "/CreateNewYear/GetFiscalYearName?FiscalYearID=" + ID,
            success: function (result) {
                $("#CNFiscalYearName").text(result);
            }
        });

    }
    else {
        $("#CNStartDate").prop("disabled", true);
        $("#CNEndDate").prop("disabled", true);
        $("#CNFiscalYearName").text("");
        $("#CNFiscalYearIDError").text("This Field Is Required");
        $("#CNFiscalYearID").removeClass('input-sucess');
        $("#CNFiscalYearID").addClass('input-error');
    }
});
function clearInputs() {

    $("#CNYear").val("");
    $("#CNStartDate").val("");
    $("#CNEndDate").val("");

    $("#CNYearError").text("");
    $("#CNStartDateError").text("");
    $("#CNEndDateError").text("");

}

$("#CNStartDate").keyup(function () {


    if (!$(this).val().match(dateformat)) {
        $("#CNStartDateError").text("Invalid Date Format");
        $(this).removeClass('input-sucess');
        $(this).addClass('input-error');

    } else {
        $(this).removeClass('input-error');
        $("#CNStartDateError").text("");
    }
}).focusout(function () {
    if (!$(this).val().match(dateformat)) {
        $("#CNStartDateError").text("Invalid Date Format");
        $(this).removeClass('input-sucess');
        $(this).addClass('input-error');

    } else if ($(this).val().match(dateformat)) {

        var CNFiscalYearID = $("#CNFiscalYearID").val();

        $.ajax({
            type: "GET",
            url: "/CreateNewYear/CheckStartDateOverlap?StartDate=" + $(this).val() + "&CNFiscalYearID=" + CNFiscalYearID,
            success: function (Result) {
                if (Result == "False") {
                    $("#CNStartDateError").text("Not Valid!,This Date Overlaps with anoher year");
                } else {
                    $("#CNStartDateError").text("");
                }
            }
        });

    } else {
        $(this).removeClass('input-error');
        $("#CNStartDateError").text("");
    }

});


$("#CNEndDate").keyup(function () {

    if (!$(this).val().match(dateformat)) {
        $("#CNEndDateError").text("Invalid Date Format");
        $(this).removeClass('input-sucess');
        $(this).addClass('input-error');

    } else {
        $(this).removeClass('input-error');
        $("#CNEndDateError").text("");
    }
}).focusout(function () {
    if (!$(this).val().match(dateformat)) {
        $("#CNEndDateError").text("Invalid Date Format");
        $(this).addClass('input-error');

    } else if ($(this).val().match(dateformat)) {

        var CNFiscalYearID = $("#CNFiscalYearID").val();

        $.ajax({
            type: "GET",
            url: "/CreateNewYear/CheckEndDateOverlap?EndDate=" + $(this).val() + "&CNFiscalYearID=" + CNFiscalYearID,
            success: function (Result) {
                if (Result == "False") {
                    $("#CNEndDateError").text("Not Valid!,This Date Overlaps with anoher year");
                } else {
                    $("#CNEndDateError").text("");
                }
            }
        });

    } else {
        $(this).removeClass('input-error');
        $("#CNEndDateError").text("");
    }

});


$("#CNYear").keyup(function () {
    if ($(this).val() === "") {
        $("#CNYearError").text("This Field Is Required");
        $(this).removeClass('input-sucess');
        $(this).addClass('input-error');

    } else {
        $(this).removeClass('input-error');
        $("#CNYearError").text("");
    }
}).focusout(function () {
    if ($(this).val() === "") {
        $("#CNYearError").text("This Field Is Required");
        $(this).removeClass('input-sucess');
        $(this).addClass('input-error');

    } else {
        $(this).removeClass('input-error');
        $("#CNYearError").text("");
    }
});


$("#CreateYear").click(function () {

    var Count = 0;

    var CNStartDateError = $("#CNStartDateError").text();
    var CNEndDateError = $("#CNEndDateError").text();

    var CNFiscalYearID = $("#CNFiscalYearID").val();

    var CNYear = $("#CNYear").val();

    var CNStartDate = $("#CNStartDate").val();

    var CNEndDate = $("#CNEndDate").val();

    if ($("#CNFiscalYearID").val().length === 0) {
        $("#CNFiscalYearIDError").text("This Field Is Required");
        $("#CNFiscalYearID").removeClass('input-sucess');
        $("#CNFiscalYearID").addClass('input-error');
        Count++;
    } else {
        $("#CNFiscalYearID").removeClass('input-error');
        $("#CNFiscalYearIDError").text("");
    }

    if ($("#CNYear").val() === "") {
        $("#CNYearError").text("This Field Is Required");
        $("#CNYear").removeClass('input-sucess');
        $("#CNYear").addClass('input-error');
        Count++;

    } else {
        $("#CNYear").removeClass('input-error');
        $("#CNYearError").text("");
    }

    if (!$("#CNStartDate").val().match(dateformat)) {
        $("#CNStartDateError").text("Invalid Date Format");
        $("#CNStartDate").removeClass('input-sucess');
        $("#CNStartDate").addClass('input-error');
        Count++;

    } else if ($("#CNStartDate").val().match(dateformat)) {

        $.ajax({
            type: "GET",
            url: "/CreateNewYear/CheckStartDateOverlap?StartDate=" + $("#CNStartDate").val(),
            success: function (Result) {
                if (Result == "False") {
                    $("#CNStartDateError").text("Not Valid!,This Date Overlaps with anoher year");
                }
            }
        });

    } else {
        $("#CNStartDate").removeClass('input-error');
        $("#CNStartDateError").text("");
    }


    if (!$("#CNEndDate").val().match(dateformat)) {
        $("#CNEndDateError").text("Invalid Date Format");
        $("#CNEndDate").removeClass('input-sucess');
        $("#CNEndDate").addClass('input-error');
        Count++;

    } else if ($("#CNEndDate").val().match(dateformat)) {

        $.ajax({
            type: "GET",
            url: "/CreateNewYear/CheckEndDateOverlap?EndDate=" + $("#CNEndDate").val(),
            success: function (Result) {
                if (Result == "False") {
                    $("#CNEndDateError").text("Not Valid!,This Date Overlaps with anoher year");
                }
            }
        });

    } else {
        $("#CNEndDate").removeClass('input-error');
        $("#CNEndDateError").text("");
    }


    if (CNStartDate.length > 0 && CNEndDate.length > 0) {
        if (CNStartDate == CNEndDate) {
            $("#CNEndDateError").text("Invalid..! End Date Must Not Equal to Start Date");
            Count++;
        }
        else if (CNEndDate < CNStartDate) {
            $("#CNEndDateError").text("End Date Must Be Greater Than Start Date");
            Count++;
        }
    }

    if (Count === 0 && CNStartDateError.length === 0 && CNEndDateError.length === 0) {
        $.ajax({

            type: "POST",
            url: "/CreateNewYear/CreateNewYear?CNFiscalYearID=" + CNFiscalYearID + "&CNYear=" + CNYear + "&CNStartDate=" + CNStartDate + "&CNEndDate=" + CNEndDate,
            success: function (result) {

                if (result === "Duplicate") {
                    $("#CNYearError").text("Year is Not Valid..!");
                    $("#CNYear").removeClass('input-sucess');
                    $("#CNYear").addClass('input-error');
                }
                else {

                    window.location = "/CreateNewYear/CreatePeriodsAndAdjusments?CNFiscalYearID=" + CNFiscalYearID + "&CNYear=" + result + "&CNStartDate=" + CNStartDate + "&CNEndDate=" + CNEndDate;

                }
            }
        });
    }
});

