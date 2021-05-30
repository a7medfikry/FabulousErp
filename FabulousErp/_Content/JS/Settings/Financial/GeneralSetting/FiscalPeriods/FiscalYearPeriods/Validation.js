var dateformat = /^(19|20)\d\d-(0\d|1[012])-(0\d|1\d|2\d|3[01])$/;
var CountOutOfRange = 0;


$(".form-control1").focusout(function () {
    var Date = $(this).val();

    if (Date.match(dateformat)) {

        var Error = $(this);
        var YearID = $("#FiscalYearID").val();
        $.ajax({
            type: "GET",
            url: "/FiscalPeriods/CheckDate?Date=" + Date + "&YearID=" + YearID,
            success: function (Result) {
                if (Result == "False") {
                    Error.addClass("input-error");
                    $("#SubmitError").prop("disabled", true);
                    $("#NotMatchError").show();
                    $("#NotMatchError").text("This Date Out Of Range,,");
                }
                else {
                    Error.removeClass("input-error");
                    $("#SubmitError").prop("disabled", false);
                    $("#NotMatchError").hide();
                    $("#NotMatchError").text("");
                }
            }
        });

    }
    else {

        $("#NotMatchError").show();
        $("#NotMatchError").text("Invalid Date Format");
        $("#SubmitError").prop("disabled", true);

    }
});

$(".form-control2").focusout(function () {
    var Date = $(this).val();

    if (Date.match(dateformat)) {

        var Error = $(this);
        var YearID = $("#FiscalYearID").val();
        $.ajax({
            type: "GET",
            url: "/FiscalPeriods/CheckDate?Date=" + Date + "&YearID=" + YearID,
            success: function (Result) {
                if (Result == "False") {
                    Error.addClass("input-error");
                    $("#NotMatchError").show();
                    $("#NotMatchError").text("This Date Out Of Range,,");
                    $("#SubmitError").prop("disabled", true);
                }
                else {
                    Error.removeClass("input-error");
                    $("#SubmitError").prop("disabled", false);
                    $("#NotMatchError").hide();
                    $("#NotMatchError").text("");
                }
            }
        });

    }
    else {

        $("#NotMatchError").show();
        $("#NotMatchError").text("Invalid Date Format");
        $("#SubmitError").prop("disabled", true);

    }

});


$("#SubmitError").click(function () {
    var counter = 0;

    var NotMatchError = $("#NotMatchError").text();

    $(".form-control2").each(function () {
        if ($(this).val() === "") {
            $(this).focus();
            counter++;
        }
    });

    $(".form-control1").each(function () {
        if ($(this).val() === "") {
            $(this).focus();
            counter++;
        }
    });

    if (counter > 0) {
        Talert("Fill The Dates..");
    }
    else {

        var Form = $("#Form1").serialize();
        $.ajax({
            type: "POST",
            url: "/FiscalPeriods/FiscalYearPeriod",
            data: Form,
            success: function (Result) {
                if (Result == "True") {
                    $("#PeriodsDone").text("Periods Created Successfull");
                    $("table td,th").addClass("input-success");
                    $(".form-control1").removeClass("input-error");
                    $(".form-control2").removeClass("input-error");
                    $(".form-control1").attr('disabled', 'disabled');
                    $(".form-control2").attr('disabled', 'disabled');
                    $("#SubmitError").prop("disabled", true);
                    $("#ErrorDateAdj").hide();
                    $("#ErrorDate").hide();
                    $("#ErrorDate").hide();
                } else if (Result.Start.length > 0) {
                    $("table td,th").removeClass("input-success");
                    $("#ErrorDate").show();
                    $("#ErrorMsg").text(Result.Message);
                    $("#StartMsg").text(Result.Start);
                    $("#EndMsg").text(Result.End);
                    $("#ErrorDateAdj").hide();

                    var StartMsg = $("#StartMsg").text();

                    var EndMsg = $("#EndMsg").text();

                    $(".form-control1").each(function () {

                        if ($(this).val() === StartMsg || $(this).val() === EndMsg) {
                            $(this).addClass("input-error");
                        } else {
                            $(this).removeClass("input-error");
                        }
                    });
                }

                else if (Result.StartAdj.length > 0) {
                    $("table td,th").removeClass("input-success");
                    $("#ErrorDateAdj").show();
                    $("#ErrorMsgAdj").text("Two Dates In Adjusment Period Identical : ");
                    $("#StartMsgAdj").text(Result.StartAdj);
                    $("#ErrorDate").hide();

                    var StartMsgAdj = $("#StartMsgAdj").text();

                    $(".form-control2").each(function () {

                        if ($(this).val() === StartMsgAdj || $(this).val() === EndMsgAdj) {
                            $(this).addClass("input-error");
                        } else {
                            $(this).removeClass("input-error");
                        }
                    });

                }
            }
        });
    }
});


