//validation of Chart Data------------------------------------------------------------------
$("#ChartAccountID").keyup(function () {

    var ChartAccountID = $(this).val();

    if (ChartAccountID.length === 0) {
        $("#ChartAccountID").css("border-color", "red");
    } else {

        $("#ChartAccountID").css("border-color", "");
    }

}).focusout(function () {

    var ChartAccountID = $(this).val();

    if (ChartAccountID.length === 0) {
        $("#ChartAccountID").css("border-color", "red");
    } else {

        $.ajax({
            type: "GET",
            url: "/CreateAccountChart/CheckChartID?ChartAccountID=" + ChartAccountID,
            success: function (result) {
                if (result === "False") {
                    $("#ChartAccountID").focus();
                    $("#ChartAccountID").css("border-color", "red");
                    $("#GlobalError").text("Chart Account ID not Available..!")
                } else {
                    $("#GlobalError").text("")
                    $("#ChartAccountID").css("border-color", "");
                }
            }
        });
    }

});

$("#ChartAccountName").keyup(function () {

    var ChartAccountName = $(this).val();

    if (ChartAccountName.length === 0) {
        $("#ChartAccountName").css("border-color", "red");
    } else {
        $("#ChartAccountName").css("border-color", "");
    }

}).focusout(function () {

    var ChartAccountName = $(this).val();

    if (ChartAccountName.length === 0) {
        $("#ChartAccountName").css("border-color", "red");
    } else {
        $("#ChartAccountName").css("border-color", "");
    }

});

$("#AccountLength").keyup(function () {

    var AccountLength = $(this).val();

    if (AccountLength.length === 0) {
        $("#AccountLength").css("border-color", "red");
    } else if (AccountLength > 50) {
        $("#AccountLength").css("border-color", "red");
    } else {
        $("#AccountLength").css("border-color", "");
    }

}).focusout(function () {

    var AccountLength = $(this).val();

    if (AccountLength.length === 0) {
        $("#AccountLength").css("border-color", "red");
    } else if (AccountLength > 50) {
        $("#AccountLength").css("border-color", "red");
    } else {
        $("#AccountLength").css("border-color", "");
    }

    });

$("#SegmentLength").keyup(function (event) {

    event.preventDefault();
    if (event.keyCode === 13) {
        $("#UpdateSegmentLength").click();
    }

    var SegmentLength = $(this).val();

    if (SegmentLength.length === 0) {
        $("#SegmentLength").css("border-color", "red");
    } else if (SegmentLength > 5) {
        $("#SegmentLength").css("border-color", "red");
    } else {
        $("#SegmentLength").css("border-color", "");
    }

}).focusout(function () {

    var SegmentLength = $(this).val();

    if (SegmentLength.length === 0) {
        $("#SegmentLength").css("border-color", "red");
    } else if (SegmentLength > 5) {
        $("#SegmentLength").css("border-color", "red");
    } else {
        $("#SegmentLength").css("border-color", "");
    }

    });

$("#MainSegment").change(function () {

    var MainSegment = $(this).val();

    if (MainSegment.length === 0) {
        $("#MainSegment").css("border-color", "red");
    } else {
        $("#MainSegment").css("border-color", "");
    }
});

$("#Separate").change(function () {

    var Separate = $(this).val();

    if (Separate.length === 0) {
        $("#Separate").css("border-color", "red");
    } else {
        $("#Separate").css("border-color", "");
    }
});

$("#NumberOfSegment").change(function () {

    var NumberOfSegment = $(this).val();

    if (NumberOfSegment.length === 0) {
        $("#SegmentTbl").hide();
        $("#SegmentDetails").hide();
        $("#SaveAllSegments").hide();
        $("#SaveAll").hide();
        $("#NumberOfSegment").css("border-color", "red");
    } else {
        CalculateSegments();
        $("#NumberOfSegment").css("border-color", "");
    }

});
//------------------------------------------------------------------------

function CalculateSegments() {

    var ChartAccountID = $("#ChartAccountID").val();

    var ChartAccountName = $("#ChartAccountName").val();

    var AccountLength = $("#AccountLength").val();

    var NumberOfSegment = $("#NumberOfSegment").val();

    //validation of Chart Data------------------------------------------------------------------
    var Test = true;

    if (ChartAccountID.length === 0) {
        $("#ChartAccountID").css("border-color", "red");
        Test = false;
    } else {
        $("#ChartAccountID").css("border-color", "");
    }

    if (ChartAccountName.length === 0) {
        $("#ChartAccountName").css("border-color", "red");
        Test = false;
    } else {
        $("#ChartAccountName").css("border-color", "");
    }

    if (AccountLength.length === 0) {
        $("#AccountLength").css("border-color", "red");
        Test = false;
    } else if (AccountLength > 50) {
        $("#AccountLength").css("border-color", "red");
        Test = false;
    } else {
        $("#AccountLength").css("border-color", "");
    }

    if (NumberOfSegment.length === 0) {
        $("#NumberOfSegment").css("border-color", "red");
        Test = false;
    } else {
        $("#NumberOfSegment").css("border-color", "");
    }
    //------------------------------------------------------------------------------------

    if (Test === true) {

        if (NumberOfSegment == "Non") {
            $("#SegmentTbl").hide();
            $("#SegmentDetails").hide();
            $("#SaveAllSegments").hide();
            $("#SaveAll").show();
        } else {

            $("#MainSegment").empty();

            $("#SegmentTbl").show();
            $("#SegmentDetails").show();
            $("#SaveAllSegments").show();
            $("#SaveAll").hide();

            var DataTbl = $("#SetSegmentResult");

            DataTbl.html("");

            for (var i = 0; i < parseInt(NumberOfSegment); i++) {

                var count = parseInt(i + 1);

                var Data = "<tr class='row_" + count + "'>" +
                    "<td width='15%'>" + count + "</td>" +
                    "<td width='25%'>" + "Segment " + count + "</td>" +
                    "<td width='20%'>" + "5" + "</td>" +
                    "<td width='20%' class = 'SumLength' id =" + count + ">" + 0 + "</td>" +
                    "<td width='20%'>" + '<button type="button" class="btn btn-warning btn-sm" onclick="EditLength(\'' + count + '\')"><span class="fa fa-edit"></span></a>' + "</button>" +
                    "</tr>";
                DataTbl.append(Data);

                $("#MainSegment").append("<option value='" + "Segment " + count + "'>" + "Segment " + count + "</option>");

            }
        }
    } else {
        $("#NumberOfSegment").val("");
    }
}




function EditLength(count) {

    var AccountLength = $("#AccountLength").val();

    var sumLength = 0;

    $(".SumLength").each(function () {
        var value = $(this).text();
        // add only if the value is number
        if (!isNaN(value) && value.length != 0) {
            sumLength += parseFloat(value);
        }
    });

    var remainder = parseInt(AccountLength - sumLength);

    $("#Remainder").text(remainder);

    $("#IncreaseSegment").text(count);
    $("#SegmentLength").val("");
    $("#SegmentLength").css("border-color", "");

    $("#SegmentName").text("Segment " + count);

    $("#EditSegmentModal").modal("show");
    $("#EditSegmentModal").on('shown.bs.modal', function () {
        $("#SegmentLength").focus();

    });
}

$("#UpdateSegmentLength").click(function () {


    var IncreaseSegment = $("#IncreaseSegment").text();

    var SegmentLength = $("#SegmentLength").val();

    var Test = true;

    if (SegmentLength.length === 0) {
        $("#SegmentLength").css("border-color", "red");
        Test = false;
    } else if (SegmentLength > 5) {
        $("#SegmentLength").css("border-color", "red");
        Test = false;
    } else {
        $("#SegmentLength").css("border-color", "");
    }

    if (Test === true) {
        $('#' + IncreaseSegment + '').text(SegmentLength);
        $("#EditSegmentModal").modal("hide");

    }

});





$("#SaveAllSegments").click(function () {

    var MainSegment = $("#MainSegment").val();

    var Separate = $("#Separate").val();

    var ChartAccountID = $("#ChartAccountID").val();

    var ChartAccountName = $("#ChartAccountName").val();

    var AccountLength = $("#AccountLength").val();

    var NumberOfSegment = $("#NumberOfSegment").val();

    var sumLength = 0;

    var emptyLength = true;


    $(".SumLength").each(function () {
        var value = $(this).text();
        // add only if the value is number
        if (!isNaN(value) && value.length != 0) {
            sumLength += parseFloat(value);
        }
        if (value == "null") {
            emptyLength = false;
        }
    });

    var test = true;

    if (emptyLength == false) {
        $("#SegmentError").text("Fill all Lengthes..!");
        test = false;
    }
    else if (sumLength != AccountLength) {
        $("#SegmentError").text("Sum of Length not equale Account length you Detected..!");
        $(".SumLength").css("border-color", "red");
        $("#AccountLength").css("border-color", "red");
        test = false;
    } else {
        $("#SegmentError").text("");
        $(".SumLength").css("border-color", "");
        $("#AccountLength").css("border-color", "");
    }

    if (MainSegment.length === 0) {
        $("#MainSegment").css("border-color", "red");
        test = false;
    } else {
        $("#MainSegment").css("border-color", "");
    }

    if (Separate.length === 0) {
        $("#Separate").css("border-color", "red");
        test = false;
    } else {
        $("#Separate").css("border-color", "");
    }

    if (AccountLength.length === 0) {
        $("#AccountLength").css("border-color", "red");
        test = false;
    } else if (AccountLength > 50) {
        $("#AccountLength").css("border-color", "red");
        test = false;
    } else {
        $("#AccountLength").css("border-color", "");
    }

    if (NumberOfSegment.length === 0) {
        $("#NumberOfSegment").css("border-color", "red");
        test = false;
    } else {
        $("#NumberOfSegment").css("border-color", "");
    }

    if (ChartAccountID.length === 0) {
        $("#ChartAccountID").css("border-color", "red");
        test = false;
    } else {
        $("#ChartAccountID").css("border-color", "");
    }

    if (ChartAccountName.length === 0) {
        $("#ChartAccountName").css("border-color", "red");
        test = false;
    } else {
        $("#ChartAccountName").css("border-color", "");
    }

    if (test === true) {

        var SegmentsArr = [];
        SegmentsArr.length = 0;

        $.each($("#SegmentsTable tbody tr"), function () {
            SegmentsArr.push({
                IncreaseSegment: $(this).find('td:eq(0)').html(),
                SegmentName: $(this).find('td:eq(1)').html(),
                Length: $(this).find('td:eq(3)').html()
            });
        });

        var data = JSON.stringify({
            order: SegmentsArr
        });


        $.ajax({
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            type: "POST",
            url: "/CreateAccountChart/FinalSaveSegments?ChartAccountID=" + ChartAccountID + "&ChartAccountName=" + ChartAccountName + "&AccountLength=" + AccountLength + "&NumberOfSegment=" + NumberOfSegment + "&MainSegment=" + MainSegment + "&Separate=" + Separate,
            data: data,
            success: function (result) {
                if (result == "True") {
                    location.reload();
                }
            }
        });
    }

});

$("#SaveAll").click(function () {

    var ChartAccountID = $("#ChartAccountID").val();

    var ChartAccountName = $("#ChartAccountName").val();

    var AccountLength = $("#AccountLength").val();

    var NumberOfSegment = $("#NumberOfSegment").val();

    var test = true;

    if (AccountLength.length === 0) {
        $("#AccountLength").css("border-color", "red");
        test = false;
    } else if (AccountLength > 50) {
        $("#AccountLength").css("border-color", "red");
        test = false;
    } else {
        $("#AccountLength").css("border-color", "");
    }

    if (NumberOfSegment.length === 0) {
        $("#NumberOfSegment").css("border-color", "red");
        test = false;
    } else {
        $("#NumberOfSegment").css("border-color", "");
    }

    if (ChartAccountID.length === 0) {
        $("#ChartAccountID").css("border-color", "red");
        test = false;
    } else {
        $("#ChartAccountID").css("border-color", "");
    }

    if (ChartAccountName.length === 0) {
        $("#ChartAccountName").css("border-color", "red");
        test = false;
    } else {
        $("#ChartAccountName").css("border-color", "");
    }

    if (test === true) {

        $.ajax({
            type: "POST",
            url: "/CreateAccountChart/FinalSave?ChartAccountID=" + ChartAccountID + "&ChartAccountName=" + ChartAccountName + "&AccountLength=" + AccountLength + "&NumberOfSegment=" + NumberOfSegment ,
            success: function (result) {
                if (result == "True") {
                    location.reload();
                }
            }
        });

    }

});


