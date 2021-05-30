
$("#FiscalYearName").keyup(function () {
    if ($(this).val() === "") {
        $("#FiscalNameError").text("This Field Is Required");
        $(this).removeClass('input-sucess');
        $(this).addClass('input-error');

    } else {
        $(this).removeClass('input-error');
        $("#FiscalNameError").text("");
    }
});


$("#FiscalYearID").keyup(function () {
    if ($(this).val().length !== 5) {
        $("#FiscalIDError").text("Fiscal Year ID Is Required 5 Characters");
        $(this).removeClass('input-sucess');
        $(this).addClass('input-error');
    } else {
        $(this).removeClass('input-error');
        $("#FiscalIDError").text("");
    }
}).focusout(function () {
    if ($(this).val().length !== 5) {
        $("#FiscalIDError").text("Fiscal Year ID Is Required 5 Characters");
        $(this).removeClass('input-sucess');
        $(this).addClass('input-error');
        $(this).focus();
    } else {
        $(this).removeClass('input-error');
        $("#FiscalIDError").text("");
    }
});
/*
$("#FiscalStartDate").keyup(function () {
    if ($(this).val() === "") {
        $("#FiscalStartError").text("This Field Is Required");
        $(this).removeClass('input-sucess');
        $(this).addClass('input-error');

    } else {
        $(this).removeClass('input-error');
        $("#FiscalStartError").text("");
    }
}).focusout(function () {
    if ($(this).val() === "") {
        $("#FiscalStartError").text("This Field Is Required");    
        $(this).addClass('input-error');
        $(this).focus();

    } else {
        $(this).removeClass('input-error');
        $("#FiscalStartError").text("");
    }
});

$("#FiscalEndDate").keyup(function () {
    if ($(this).val() === "") {
        $("#FiscalEndError").text("This Field Is Required");
        $(this).removeClass('input-sucess');
        $(this).addClass('input-error');

    } else {
        $(this).removeClass('input-error');
        $("#FiscalEndError").text("");
    }
}).focusout(function () {
    if ($(this).val() === "") {
        $("#FiscalEndError").text("This Field Is Required");
        $(this).addClass('input-error');
        $(this).focus();

    } else {
        $(this).removeClass('input-error');
        $("#FiscalEndError").text("");
    }
});
*/
$("#Periods").keyup(function () {
    if ($(this).val() === "") {
        $("#PeriodsError").text("This Field Is Required");
        $(this).removeClass('input-sucess');
        $(this).addClass('input-error');

    } else {
        $(this).removeClass('input-error');
        $("#PeriodsError").text("");
    }
}).focusout(function () {
    if ($(this).val() === "") {
        $("#PeriodsError").text("This Field Is Required");
        $(this).addClass('input-error');
        $(this).focus();

    } else {
        $(this).removeClass('input-error');
        $("#PeriodsError").text("");
    }
});

$("#Adjustments").keyup(function () {
    if ($(this).val() === "") {
        $("#AdjError").text("This Field Is Required");
        $(this).removeClass('input-sucess');
        $(this).addClass('input-error');

    } else {
        $(this).removeClass('input-error');
        $("#AdjError").text("");
    }
}).focusout(function () {
    if ($(this).val() === "") {
        $("#AdjError").text("This Field Is Required");
        $(this).addClass('input-error');
        $(this).focus();

    } else {
        $(this).removeClass('input-error');
        $("#AdjError").text("");
    }
});


/***************************************************************************/
function formsubmit() {
    //var Counter = 0;
    /*
    var StartDate = $("#FiscalStartDate").val();
    var EndDate = $("#FiscalEndDate").val();
    
    $.ajax({
        type: "GET",
        url: "/FiscalDef/CheckStartDate?StartDate=" + StartDate + "&EndDate=" + EndDate,
        success: function (Result) {
            if (Result == "False") {
                Counter++;
            }
        }
    });
    */

    /*
    var d = new Date();    // This will get you current date
    var currentYear = d.getFullYear();
    var EnterDate = $("#FiscalYearName").val();
    if (parseInt(EnterDate) < parseInt(currentYear)) {
        $("#FiscalNameError").text("Please enter valid Fiscal year Name!");
        $("#FiscalYearName").addClass('input-error');
        Counter++;
    }
    */

    /*
    if (EndDate.length > 0 && StartDate.length > 0) {
        if (StartDate == EndDate) {
            $("#FiscalEndError").text("Invalid!,,Fiscal year End Must Not Equal to Fiscal year Start");
            Counter++;
        }
        else if (EndDate < StartDate) {
            $('#FiscalEndError').text("End Date Must Be greater Than Start Date");
            Counter++;
        }
    }
    if (Counter == 0 && $("#FiscalEndError").text().length == 0 && $("#FiscalStartError").text().length == 0) {
        $("#Form1").submit();
    }
    */
    var Count = 0;

    if ($("#FiscalYearID").val().length !== 5) {
        $("#FiscalIDError").text("Fiscal Year ID Is Required 5 Characters");
        $("#FiscalYearID").removeClass('input-sucess');
        $("#FiscalYearID").addClass('input-error');
        Count++;
    } else {
        $("#FiscalYearID").removeClass('input-error');
        $("#FiscalIDError").text("");
    }

    if ($("#FiscalYearName").val() === "") {
        $("#FiscalNameError").text("This Field Is Required");
        $("#FiscalYearName").removeClass('input-sucess');
        $("#FiscalYearName").addClass('input-error');
        Count++;

    } else {
        $("#FiscalYearName").removeClass('input-error');
        $("#FiscalNameError").text("");
    }

    if ($("#Periods").val() === "") {
        $("#PeriodsError").text("This Field Is Required");
        $("#Periods").removeClass('input-sucess');
        $("#Periods").addClass('input-error');
        Count++;
    } else {
        $("#Periods").removeClass('input-error');
        $("#PeriodsError").text("");
    }

    if ($("#Adjustments").val() === "") {
        $("#AdjError").text("This Field Is Required");
        $("#Adjustments").removeClass('input-sucess');
        $("#Adjustments").addClass('input-error');
        Count++;
    } else {
        $("#Adjustments").removeClass('input-error');
        $("#AdjError").text("");
    }

    if (Count == 0) {

        var Form = $("#Form1").serialize();

        $.ajax({
            type: "POST",
            url: "/FiscalDef/FiscalYearDefenition",
            data: Form,
            success: function (result) {

                if (result == "False") {
                    $("#FiscalIDError").text("Fiscal Year ID Is Not Valid!");
                    $("#FiscalYearID").addClass('input-error');
                    $("#FiscalYearID").focus();
                } else if (result == "True") {

                    $("#FiscalIDError").text("");
                    $("#FiscalYearID").removeClass('input-error');
                    $("#FiscalYearID").val("");
                    $("#FiscalYearName").val("");
                    $("#Periods").val("");
                    $("#Adjustments").val("");
                    $("#SuccessSaved").text("Definition Saved Successfully.. Click Create New Year To Create Periods.!");
                }
            }
        });
    }
}


$("#FiscalYearID").focusout(function () {
    var YearID = $("#FiscalYearID").val();
    $.ajax({
        type: "GET",
        url: "/FiscalDef/GetFiscalYearID?YearID=" + YearID,
        success: function (Result) {
            if (Result == "False") {
                $("#FiscalIDError").text("Fiscal Year ID Is Not Valid!");
                $("#FiscalYearID").addClass('input-error');
                $("#FiscalYearID").focus();
            } else {
                $("#FiscalIDError").text("");
                $("#FiscalYearID").removeClass('input-error');
            }
        }
    });
});

/*
$("#FiscalStartDate").focusout(function () {
    var StartDate = $("#FiscalStartDate").val();
    $.ajax({
        type: "GET",
        url: "/FiscalDef/CheckStartDate?StartDate=" + StartDate,
        success: function (Result) {
            if (Result == "False") {
                $("#FiscalStartError").text("Not Valid!,This Date Overlaps with anoher year");
            } else
            {
                $("#FiscalStartError").text("");
            }
        }
    });
});

$("#FiscalEndDate").focusout(function () {
        var EndDate = $("#FiscalEndDate").val();
        $.ajax({
            type: "GET",
            url: "/FiscalDef/CheckStartDate?EndDate=" + EndDate,
            success: function (Result) {
                if (Result == "False") {
                    $("#FiscalEndError").text("Not Valid!,This Date Overlaps with anoher year");
                } else
                {
                    $("#FiscalEndError").text("");
                }
            }
        });
    });
*/

/**************************************************************/