function CopyAccessFile() {

    var UserID = $("#UserID").val();
    $("#TargetUserName").text("");

    $.ajax({
        type: "GET",
        url: "/UGFormsAccess/FilterUserID?UserID=" + UserID,
        success: function (data) {
            $("#TargetUserID").empty();

            if (data.length == 0) {

                $("#TargetUserID").append($('<option/>', {
                    value: -1,
                    text: "Empty.. No User Created..!"

                })
                );
            }
            else {

                $("#TargetUserID").append($('<option/>', {
                    value: -1,
                    text: "---Choose---"

                })
                );

                $.each(data, function (index, row) {

                    $("#TargetUserID").append("<option value='" + row.UserID + "'>" + row.UserID + "</option>");

                });
            }
        },
        error: function (req, status, error) {

        }
    });

    $("#MyModal").modal("show");

}

function ConfirmCopy() {

    var TargetUserID = $("#TargetUserID").val();
    $('.CB').prop('checked', false);

    if (TargetUserID != "-1") {

        $.ajax({

            type: "GET",
            url: "/UGFormsAccess/CopyAccessOfUser?TargetUserID=" + TargetUserID,
            success: function (data) {

                $(data).each(function (i, res) {

                    $(".CB[value=" + res.FormName + "]").prop("checked", true);

                });

                if ($('.S').not(':checked').length === 0) {
                    $('#CheckAllSetting').prop('checked', true);
                } else {
                    $('#CheckAllSetting').prop('checked', false);
                }

                if ($('.I').not(':checked').length === 0) {
                    $('#CheckAllInquiry').prop('checked', true);
                } else {
                    $('#CheckAllInquiry').prop('checked', false);
                }

                if ($('.T').not(':checked').length === 0) {
                    $('#CheckAllTransaction').prop('checked', true);
                } else {
                    $('#CheckAllTransaction').prop('checked', false);
                }

                $("#MyModal").modal("hide");
            }
        });
    } 
}

$("#TargetUserID").change(function () {

    var TargetUserID = $(this).val();

    if (TargetUserID == "-1") {

        $("#TargetUserName").text("");

    } else {

        $.ajax({
            type: "GET",
            url: "/UGFormsAccess/GetUserName?UserID=" + TargetUserID,
            success: function (data) {
                $("#TargetUserName").text(data);
            }
        });

    }
});