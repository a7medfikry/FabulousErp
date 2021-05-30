$("#Group").change(function () {

    if ($("#Group").is(':checked')) {

        $("#User").prop("checked", false);

        $("#GroupID").prop("disabled", false);
        $("#UserID").prop("disabled", true);

        $("#GroupID").val("");
        $("#GroupName").val("");
        $("#SavedSuccess").text("");
        $("#FormsTable").hide();
        $("#SaveBtn").prop("disabled", true);
        $("#CopyAccsee").prop("disabled", true);

    } else {
        $("#GroupID").prop("disabled", true);
        $("#GroupID").val("");
        $("#SavedSuccess").text("");
        $("#GroupName").val("");
        $("#FormsTable").hide();
        $("#SaveBtn").prop("disabled", true);
        $("#CopyAccsee").prop("disabled", true);
    }

});

$("#User").change(function () {

    if ($("#User").is(':checked')) {

        $("#Group").prop("checked", false);
        $("#UserID").prop("disabled", false);
        $("#GroupID").prop("disabled", true);

        $("#UserID").val("");
        $("#UserName").val("");
        $("#SavedSuccess").text("");
        $("#FormsTable").hide();
        $("#SaveBtn").prop("disabled", true);
        $("#CopyAccsee").prop("disabled", true);

    } else {
        $("#UserID").prop("disabled", true);

        $("#UserID").val("");
        $("#SavedSuccess").text("");
        $("#UserName").val("");
        $("#FormsTable").hide();
        $("#SaveBtn").prop("disabled", true);
        $("#CopyAccsee").prop("disabled", true);
    }

});


$("#GroupID").change(function () {

    var GroupID = $(this).val();

    $("#GlobalError").text("");
    $("#FormsTable").hide();
    $("#SaveBtn").prop("disabled", true);
    $("#CopyAccsee").prop("disabled", true);
    $("#GroupName").val("");
    $('.CB').prop('checked', false);

    if (GroupID.length > 0) {

        $.ajax({
            type: "GET",
            url: "/UGFormsAccess/GetGroupAccessData?groupID=" + GroupID,
            success: function (data) {

                if (data === false) {
                    $('#GlobalError').text("This Group is Disactive you can't Edit In it.. Please Contact System Admin");
                } else {
                    $("#GroupName").val(data.Names.Name);

                    $(data.Forms).each(function (i, res) {

                        $(".CB[value=" + res.FormName + "]").prop("checked", "true");

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

                    $("#FormsTable").show();
                    $("#SaveBtn").prop("disabled", false);
                    $("#CopyAccsee").prop("disabled", false);
                }

            }
        });
    } else {
        $("#FormsTable").hide();
        $("#SaveBtn").prop("disabled", true);
        $("#GroupName").val("");
        $("#GroupDisactiveError").hide();
    }

});


$("#UserID").change(function () {

    var UserID = $(this).val();

    $("#GlobalError").text("");
    $("#FormsTable").hide();
    $("#SaveBtn").prop("disabled", true);
    $("#CopyAccsee").prop("disabled", true);
    $("#UserName").val("");
    $('.CB').prop('checked', false);

    if (UserID.length > 0) {

        $.ajax({
            type: "GET",
            url: "/UGFormsAccess/GetUserAccessData?UserID=" + UserID,
            success: function (data) {

                if (data === false) {

                    $("#GlobalError").text("This User Not Have Access to Any Company..!");

                } else {
                    $("#UserName").val(data.Names.Name);

                    $(data.Forms).each(function (i, res) {

                        $(".CB[value=" + res.FormName + "]").prop("checked", "true");

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

                    $("#FormsTable").show();
                    $("#SaveBtn").prop("disabled", false);
                    $("#CopyAccsee").prop("disabled", false);
                }
            }
        });
    }
});

$("#CheckAllSetting").change(function () {

    if ($("#CheckAllSetting").is(':checked')) {
        $(".S").prop("checked", true);
    }
    else {
        $(".S").prop("checked", false);
    }

});


$("#CheckAllInquiry").change(function () {

    if ($("#CheckAllInquiry").is(':checked')) {
        $(".I").prop("checked", true);
    }
    else {
        $(".I").prop("checked", false);
    }

});

$("#CheckAllTransaction").change(function () {

    if ($("#CheckAllTransaction").is(':checked')) {
        $(".T").prop("checked", true);
    }
    else {
        $(".T").prop("checked", false);
    }

});


$("#CheckAllTransaction").change(function () {

    if ($("#CheckAllTransaction").is(':checked')) {
        $(".T").prop("checked", true);
    }
    else {
        $(".T").prop("checked", false);
    }

});


        //$.ajax({
        //    type: "GET",
        //    url: "/UGFormsAccess/GetUserAccess?UserID=" + UserID,
        //    success: function (data) {

        //        if (data === "False") {
        //            $("#FormsTable").hide();
        //            $("#SaveBtn").prop("disabled", true);
        //            $("#CopyAccsee").prop("disabled", true);
        //            $("#GlobalError").text("This User Not Have Access to Any Company..!");
        //        }
        //        else {

        //            if (data.SCL == "SCL") {
        //                $("#SCL").prop("checked", true);
        //            } else { $("#SCL").prop("checked", false); }

        //            if (data.SCBI == "SCBI") {
        //                $("#SCBI").prop("checked", true);
        //            } else { $("#SCBI").prop("checked", false); }

        //            if (data.SCPI == "SCPI") {
        //                $("#SCPI").prop("checked", true);
        //            } else { $("#SCPI").prop("checked", false); }

        //            if (data.SCNA == "SCNA") {
        //                $("#SCNA").prop("checked", true);
        //            } else { $("#SCNA").prop("checked", false); }

        //            if (data.SLOU == "SLOU") {
        //                $("#SLOU").prop("checked", true);
        //            } else { $("#SLOU").prop("checked", false); }

        //            if (data.SCNG == "SCNG") {
        //                $("#SCNG").prop("checked", true);
        //            } else { $("#SCNG").prop("checked", false); }

        //            if (data.SLOG == "SLOG") {
        //                $("#SLOG").prop("checked", true);
        //            } else { $("#SLOG").prop("checked", false); }

        //            if (data.SAGI == "SAGI") {
        //                $("#SAGI").prop("checked", true);
        //            } else { $("#SAGI").prop("checked", false); }

        //            if (data.SUACP == "SUACP") {
        //                $("#SUACP").prop("checked", true);
        //            } else { $("#SUACP").prop("checked", false); }

        //            if (data.SUAP == "SUAP") {
        //                $("#SUAP").prop("checked", true);
        //            } else { $("#SUAP").prop("checked", false); }

        //            if (data.SFYD == "SFYD") {
        //                $("#SFYD").prop("checked", true);
        //            } else { $("#SFYD").prop("checked", false); }

        //            if (data.SCFY == "SCFY") {
        //                $("#SCFY").prop("checked", true);
        //            } else { $("#SCFY").prop("checked", false); }

        //            if (data.SCNY == "SCNY") {
        //                $("#SCNY").prop("checked", true);
        //            } else { $("#SCNY").prop("checked", false); }

        //            if (data.SCF == "SCF") {
        //                $("#SCF").prop("checked", true);
        //            } else { $("#SCF").prop("checked", false); }

        //            if (data.SCD == "SCD") {
        //                $("#SCD").prop("checked", true);
        //            } else { $("#SCD").prop("checked", false); }

        //            if (data.SCET == "SCET") {
        //                $("#SCET").prop("checked", true);
        //            } else { $("#SCET").prop("checked", false); }

        //            if (data.ILOU == "ILOU") {
        //                $("#ILOU").prop("checked", true);
        //            } else { $("#ILOU").prop("checked", false); }

        //            if (data.SCCOA == "SCCOA") {
        //                $("#SCCOA").prop("checked", true);
        //            } else { $("#SCCOA").prop("checked", false); }

        //            if (data.ICI == "ICI") {
        //                $("#ICI").prop("checked", true);
        //            } else { $("#ICI").prop("checked", false); }

        //            if (data.IUP == "IUP") {
        //                $("#IUP").prop("checked", true);
        //            } else { $("#IUP").prop("checked", false); }

        //            if (data.ISFP == "ISFP") {
        //                $("#ISFP").prop("checked", true);
        //            } else { $("#ISFP").prop("checked", false); }

        //            if (data.SACTC == "SACTC") {
        //                $("#SACTC").prop("checked", true);
        //            } else { $("#SACTC").prop("checked", false); }

        //            if (data.ILCOA == "ILCOA") {
        //                $("#ILCOA").prop("checked", true);
        //            } else { $("#ILCOA").prop("checked", false); }

        //            if (data.SCAA == "SCAA") {
        //                $("#SCAA").prop("checked", true);
        //            } else { $("#SCAA").prop("checked", false); }

        //            if (data.SCAAD == "SCAAD") {
        //                $("#SCAAD").prop("checked", true);
        //            } else { $("#SCAAD").prop("checked", false); }

        //            if (data.SCCC == "SCCC") {
        //                $("#SCCC").prop("checked", true);
        //            } else { $("#SCCC").prop("checked", false); }

        //            if (data.SCMCC == "SCMCC") {
        //                $("#SCMCC").prop("checked", true);
        //            } else { $("#SCMCC").prop("checked", false); }

        //            if (data.SUMCC == "SUMCC") {
        //                $("#SUMCC").prop("checked", true);
        //            } else { $("#SUMCC").prop("checked", false); }

        //            if (data.SCCCA == "SCCCA") {
        //                $("#SCCCA").prop("checked", true);
        //            } else { $("#SCCCA").prop("checked", false); }

        //            if (data.SCAG == "SCAG") {
        //                $("#SCAG").prop("checked", true);
        //            } else { $("#SCAG").prop("checked", false); }

        //            if (data.SCCA == "SCCA") {
        //                $("#SCCA").prop("checked", true);
        //            } else { $("#SCCA").prop("checked", false); }

        //            if (data.SCY == "SCY") {
        //                $("#SCY").prop("checked", true);
        //            } else { $("#SCY").prop("checked", false); }

        //            if (data.SBUAP == "SBUAP") {
        //                $("#SBUAP").prop("checked", true);
        //            } else { $("#SBUAP").prop("checked", false); }

        //            if (data.SFUAP == "SFUAP") {
        //                $("#SFUAP").prop("checked", true);
        //            } else { $("#SFUAP").prop("checked", false); }

        //            if (data.IUFA == "IUFA") {
        //                $("#IUFA").prop("checked", true);
        //            } else { $("#IUFA").prop("checked", false); }

        //            if (data.IBA == "IBA") {
        //                $("#IBA").prop("checked", true);
        //            } else { $("#IBA").prop("checked", false); }

        //            if (data.ICA == "ICA") {
        //                $("#ICA").prop("checked", true);
        //            } else { $("#ICA").prop("checked", false); }

        //            if (data.IFA == "IFA") {
        //                $("#IFA").prop("checked", true);
        //            } else { $("#IFA").prop("checked", false); }

        //            if (data.IGA == "IGA") {
        //                $("#IGA").prop("checked", true);
        //            } else { $("#IGA").prop("checked", false); }

        //            if (data.SDATCA == "SDATCA") {
        //                $("#SDATCA").prop("checked", true);
        //            } else { $("#SDATCA").prop("checked", false); }

        //            if (data.SCATCA == "SCATCA") {
        //                $("#SCATCA").prop("checked", true);
        //            } else { $("#SCATCA").prop("checked", false); }

        //            if (data.ICAA == "ICAA") {
        //                $("#ICAA").prop("checked", true);
        //            } else { $("#ICAA").prop("checked", false); }

        //            if (data.ICCC == "ICCC") {
        //                $("#ICCC").prop("checked", true);
        //            } else { $("#ICCC").prop("checked", false); }

        //            if (data.ILOC == "ILOC") {
        //                $("#ILOC").prop("checked", true);
        //            } else { $("#ILOC").prop("checked", false); }

        //            if (data.SOCP == "SOCP") {
        //                $("#SOCP").prop("checked", true);
        //            } else { $("#SOCP").prop("checked", false); }

        //            if (data.SUP == "SUP") {
        //                $("#SUP").prop("checked", true);
        //            } else { $("#SUP").prop("checked", false); }

        //            if (data.SPS == "SPS") {
        //                $("#SPS").prop("checked", true);
        //            } else { $("#SPS").prop("checked", false); }

        //            if (data.ICADA == "ICADA") {
        //                $("#ICADA").prop("checked", true);
        //            } else { $("#ICADA").prop("checked", false); }

        //            if (data.SPD == "SPD") {
        //                $("#SPD").prop("checked", true);
        //            } else { $("#SPD").prop("checked", false); }

        //            if (data.ICCCA == "ICCCA") {
        //                $("#ICCCA").prop("checked", true);
        //            } else { $("#ICCCA").prop("checked", false); }

        //            if (data.TCCB == "TCCB") {
        //                $("#TCCB").prop("checked", true);
        //            } else { $("#TCCB").prop("checked", false); }

        //            if (data.SUETR == "SUETR") {
        //                $("#SUETR").prop("checked", true);
        //            } else { $("#SUETR").prop("checked", false); }

        //            if (data.TCGE == "TCGE") {
        //                $("#TCGE").prop("checked", true);
        //            } else { $("#TCGE").prop("checked", false); }

        //            if (data.SUABP == "SUABP") {
        //                $("#SUABP").prop("checked", true);
        //            } else { $("#SUABP").prop("checked", false); }

        //            if (data.SUAFP == "SUAFP") {
        //                $("#SUAFP").prop("checked", true);
        //            } else { $("#SUAFP").prop("checked", false); }

        //            //Setting Check All
        //            if ($("#SCL:checked,#SCBI:checked,#SCPI:checked,#SCNA:checked,#SLOU:checked,#SCNG:checked,#SLOG:checked,#SAGI:checked,#SUACP:checked,#SUAP:checked,#SFYD:checked,#SCFY:checked,#SCNY:checked,#SCF:checked,#SCD:checked,#SCET:checked,#SCCOA:checked,#SACTC:checked,#SCAA:checked,#SCAAD:checked,#SCCC:checked,#SCMCC:checked,#SUMCC:checked,#SCCCA:checked,#SCAG:checked,#SCCA:checked,#SCY:checked,#SBUAP:checked,#SFUAP:checked,#SDATCA:checked,#SCATCA:checked,#SOCP:checked,#SUP:checked,#SPS:checked,#SPD:checked,#SUETR:checked,#SUABP:checked,#SUAFP:checked").length == 38) {

        //                $("#CheckAllSetting").prop("checked", true);

        //            } else {

        //                $("#CheckAllSetting").prop("checked", false);
        //            }
        //            //Inquiry Check All
        //            if ($("#ILOU:checked,#ICI:checked,#IUP:checked,#ISFP:checked,#ILCOA:checked,#IUFA:checked,#IBA:checked,#ICA:checked,#IFA:checked,#IGA:checked,#ICAA:checked,#ICCC:checked,#ILOC:checked,#ICADA:checked,#ICCCA:checked").length == 15) {

        //                $("#CheckAllInquiry").prop("checked", true);

        //            } else {

        //                $("#CheckAllInquiry").prop("checked", false);
        //            }
        //            //Transaction Check All
        //            if ($("#TCCB:checked,#TCGE:checked").length == 2) {

        //                $("#CheckAllTransaction").prop("checked", true);

        //            } else {

        //                $("#CheckAllTransaction").prop("checked", false);
        //            }

        //            $("#FormsTable").show();
        //            $("#SaveBtn").prop("disabled", false);
        //            $("#CopyAccsee").prop("disabled", false);
        //        }
        //    }

        //});








        //$.ajax({
        //    type: "GET",
        //    url: "/UGFormsAccess/CheckGroupActive?GroupID=" + GroupID,
        //    success: function (data) {

        //        if (data === "True") {

        //            $("#SCL").prop("disabled", true);
        //            $("#SCBI").prop("disabled", true);
        //            $("#SCPI").prop("disabled", true);
        //            $("#SCNA").prop("disabled", true);
        //            $("#SLOU").prop("disabled", true);
        //            $("#SCNG").prop("disabled", true);
        //            $("#SLOG").prop("disabled", true);
        //            $("#SAGI").prop("disabled", true);
        //            $("#SUACP").prop("disabled", true);
        //            $("#SUAP").prop("disabled", true);
        //            $("#SFYD").prop("disabled", true);
        //            $("#SCFY").prop("disabled", true);
        //            $("#SCNY").prop("disabled", true);
        //            $("#SCF").prop("disabled", true);
        //            $("#SCD").prop("disabled", true);
        //            $("#SCET").prop("disabled", true);
        //            $("#ILOU").prop("disabled", true);
        //            $("#SCCOA").prop("disabled", true);
        //            $("#ICI").prop("disabled", true);
        //            $("#IUP").prop("disabled", true);
        //            $("#ISFP").prop("disabled", true);
        //            $("#SACTC").prop("disabled", true);
        //            $("#ILCOA").prop("disabled", true);
        //            $("#SCAA").prop("disabled", true);
        //            $("#SCAAD").prop("disabled", true);
        //            $("#SCCC").prop("disabled", true);
        //            $("#SCMCC").prop("disabled", true);
        //            $("#SUMCC").prop("disabled", true);
        //            $("#SCCCA").prop("disabled", true);
        //            $("#SCAG").prop("disabled", true);
        //            $("#SCCA").prop("disabled", true);
        //            $("#SCY").prop("disabled", true);
        //            $("#SBUAP").prop("disabled", true);
        //            $("#SFUAP").prop("disabled", true);
        //            $("#IUFA").prop("disabled", true);
        //            $("#IBA").prop("disabled", true);
        //            $("#ICA").prop("disabled", true);
        //            $("#IFA").prop("disabled", true);
        //            $("#IGA").prop("disabled", true);
        //            $("#SDATCA").prop("disabled", true);
        //            $("#SCATCA").prop("disabled", true);
        //            $("#ICAA").prop("disabled", true);
        //            $("#ICCC").prop("disabled", true);
        //            $("#ILOC").prop("disabled", true);
        //            $("#SOCP").prop("disabled", true);
        //            $("#SUP").prop("disabled", true);
        //            $("#SPS").prop("disabled", true);
        //            $("#ICADA").prop("disabled", true);
        //            $("#SPD").prop("disabled", true);
        //            $("#ICCCA").prop("disabled", true);
        //            $("#TCCB").prop("disabled", true);
        //            $("#SUETR").prop("disabled", true);
        //            $("#TCGE").prop("disabled", true);
        //            $("#SUABP").prop("disabled", true);
        //            $("#SUAFP").prop("disabled", true);
        //            $("#CheckAllSetting").prop("disabled", true);
        //            $("#CheckAllInquiry").prop("disabled", true);

        //            $("#GroupDisactiveError").show();

        //            $("#GroupDisactiveError").text("This Group is Disactive you can't Edit In it.. Please Contact System Admin");

        //            $.ajax({
        //                type: "GET",
        //                url: "/UGFormsAccess/GetGroupAccess?GroupID=" + GroupID,
        //                success: function (data) {
        //                    if (data.SCL == "SCL") {
        //                        $("#SCL").prop("checked", true);
        //                    } else { $("#SCL").prop("checked", false); }

        //                    if (data.SCBI == "SCBI") {
        //                        $("#SCBI").prop("checked", true);
        //                    } else { $("#SCBI").prop("checked", false); }

        //                    if (data.SCPI == "SCPI") {
        //                        $("#SCPI").prop("checked", true);
        //                    } else { $("#SCPI").prop("checked", false); }

        //                    if (data.SCNA == "SCNA") {
        //                        $("#SCNA").prop("checked", true);
        //                    } else { $("#SCNA").prop("checked", false); }

        //                    if (data.SLOU == "SLOU") {
        //                        $("#SLOU").prop("checked", true);
        //                    } else { $("#SLOU").prop("checked", false); }

        //                    if (data.SCNG == "SCNG") {
        //                        $("#SCNG").prop("checked", true);
        //                    } else { $("#SCNG").prop("checked", false); }

        //                    if (data.SLOG == "SLOG") {
        //                        $("#SLOG").prop("checked", true);
        //                    } else { $("#SLOG").prop("checked", false); }

        //                    if (data.SAGI == "SAGI") {
        //                        $("#SAGI").prop("checked", true);
        //                    } else { $("#SAGI").prop("checked", false); }

        //                    if (data.SUACP == "SUACP") {
        //                        $("#SUACP").prop("checked", true);
        //                    } else { $("#SUACP").prop("checked", false); }

        //                    if (data.SUAP == "SUAP") {
        //                        $("#SUAP").prop("checked", true);
        //                    } else { $("#SUAP").prop("checked", false); }

        //                    if (data.SFYD == "SFYD") {
        //                        $("#SFYD").prop("checked", true);
        //                    } else { $("#SFYD").prop("checked", false); }

        //                    if (data.SCFY == "SCFY") {
        //                        $("#SCFY").prop("checked", true);
        //                    } else { $("#SCFY").prop("checked", false); }

        //                    if (data.SCNY == "SCNY") {
        //                        $("#SCNY").prop("checked", true);
        //                    } else { $("#SCNY").prop("checked", false); }

        //                    if (data.SCF == "SCF") {
        //                        $("#SCF").prop("checked", true);
        //                    } else { $("#SCF").prop("checked", false); }

        //                    if (data.SCD == "SCD") {
        //                        $("#SCD").prop("checked", true);
        //                    } else { $("#SCD").prop("checked", false); }

        //                    if (data.SCET == "SCET") {
        //                        $("#SCET").prop("checked", true);
        //                    } else { $("#SCET").prop("checked", false); }

        //                    if (data.ILOU == "ILOU") {
        //                        $("#ILOU").prop("checked", true);
        //                    } else { $("#ILOU").prop("checked", false); }

        //                    if (data.SCCOA == "SCCOA") {
        //                        $("#SCCOA").prop("checked", true);
        //                    } else { $("#SCCOA").prop("checked", false); }

        //                    if (data.ICI == "ICI") {
        //                        $("#ICI").prop("checked", true);
        //                    } else { $("#ICI").prop("checked", false); }

        //                    if (data.IUP == "IUP") {
        //                        $("#IUP").prop("checked", true);
        //                    } else { $("#IUP").prop("checked", false); }

        //                    if (data.ISFP == "ISFP") {
        //                        $("#ISFP").prop("checked", true);
        //                    } else { $("#ISFP").prop("checked", false); }

        //                    if (data.SACTC == "SACTC") {
        //                        $("#SACTC").prop("checked", true);
        //                    } else { $("#SACTC").prop("checked", false); }

        //                    if (data.ILCOA == "ILCOA") {
        //                        $("#ILCOA").prop("checked", true);
        //                    } else { $("#ILCOA").prop("checked", false); }

        //                    if (data.SCAA == "SCAA") {
        //                        $("#SCAA").prop("checked", true);
        //                    } else { $("#SCAA").prop("checked", false); }

        //                    if (data.SCAAD == "SCAAD") {
        //                        $("#SCAAD").prop("checked", true);
        //                    } else { $("#SCAAD").prop("checked", false); }

        //                    if (data.SCCC == "SCCC") {
        //                        $("#SCCC").prop("checked", true);
        //                    } else { $("#SCCC").prop("checked", false); }

        //                    if (data.SCMCC == "SCMCC") {
        //                        $("#SCMCC").prop("checked", true);
        //                    } else { $("#SCMCC").prop("checked", false); }

        //                    if (data.SUMCC == "SUMCC") {
        //                        $("#SUMCC").prop("checked", true);
        //                    } else { $("#SUMCC").prop("checked", false); }

        //                    if (data.SCCCA == "SCCCA") {
        //                        $("#SCCCA").prop("checked", true);
        //                    } else { $("#SCCCA").prop("checked", false); }

        //                    if (data.SCAG == "SCAG") {
        //                        $("#SCAG").prop("checked", true);
        //                    } else { $("#SCAG").prop("checked", false); }

        //                    if (data.SCCA == "SCCA") {
        //                        $("#SCCA").prop("checked", true);
        //                    } else { $("#SCCA").prop("checked", false); }

        //                    if (data.SCY == "SCY") {
        //                        $("#SCY").prop("checked", true);
        //                    } else { $("#SCY").prop("checked", false); }

        //                    if (data.SBUAP == "SBUAP") {
        //                        $("#SBUAP").prop("checked", true);
        //                    } else { $("#SBUAP").prop("checked", false); }

        //                    if (data.SFUAP == "SFUAP") {
        //                        $("#SFUAP").prop("checked", true);
        //                    } else { $("#SFUAP").prop("checked", false); }

        //                    if (data.IUFA == "IUFA") {
        //                        $("#IUFA").prop("checked", true);
        //                    } else { $("#IUFA").prop("checked", false); }

        //                    if (data.IBA == "IBA") {
        //                        $("#IBA").prop("checked", true);
        //                    } else { $("#IBA").prop("checked", false); }

        //                    if (data.ICA == "ICA") {
        //                        $("#ICA").prop("checked", true);
        //                    } else { $("#ICA").prop("checked", false); }

        //                    if (data.IFA == "IFA") {
        //                        $("#IFA").prop("checked", true);
        //                    } else { $("#IFA").prop("checked", false); }

        //                    if (data.IGA == "IGA") {
        //                        $("#IGA").prop("checked", true);
        //                    } else { $("#IGA").prop("checked", false); }

        //                    if (data.SDATCA == "SDATCA") {
        //                        $("#SDATCA").prop("checked", true);
        //                    } else { $("#SDATCA").prop("checked", false); }

        //                    if (data.SCATCA == "SCATCA") {
        //                        $("#SCATCA").prop("checked", true);
        //                    } else { $("#SCATCA").prop("checked", false); }

        //                    if (data.ICAA == "ICAA") {
        //                        $("#ICAA").prop("checked", true);
        //                    } else { $("#ICAA").prop("checked", false); }

        //                    if (data.ICCC == "ICCC") {
        //                        $("#ICCC").prop("checked", true);
        //                    } else { $("#ICCC").prop("checked", false); }

        //                    if (data.ILOC == "ILOC") {
        //                        $("#ILOC").prop("checked", true);
        //                    } else { $("#ILOC").prop("checked", false); }

        //                    if (data.SOCP == "SOCP") {
        //                        $("#SOCP").prop("checked", true);
        //                    } else { $("#SOCP").prop("checked", false); }

        //                    if (data.SUP == "SUP") {
        //                        $("#SUP").prop("checked", true);
        //                    } else { $("#SUP").prop("checked", false); }

        //                    if (data.SPS == "SPS") {
        //                        $("#SPS").prop("checked", true);
        //                    } else { $("#SPS").prop("checked", false); }

        //                    if (data.ICADA == "ICADA") {
        //                        $("#ICADA").prop("checked", true);
        //                    } else { $("#ICADA").prop("checked", false); }

        //                    if (data.SPD == "SPD") {
        //                        $("#SPD").prop("checked", true);
        //                    } else { $("#SPD").prop("checked", false); }

        //                    if (data.ICCCA == "ICCCA") {
        //                        $("#ICCCA").prop("checked", true);
        //                    } else { $("#ICCCA").prop("checked", false); }

        //                    if (data.TCCB == "TCCB") {
        //                        $("#TCCB").prop("checked", true);
        //                    } else { $("#TCCB").prop("checked", false); }

        //                    if (data.SUETR == "SUETR") {
        //                        $("#SUETR").prop("checked", true);
        //                    } else { $("#SUETR").prop("checked", false); }

        //                    if (data.TCGE == "TCGE") {
        //                        $("#TCGE").prop("checked", true);
        //                    } else { $("#TCGE").prop("checked", false); }

        //                    if (data.SUABP == "SUABP") {
        //                        $("#SUABP").prop("checked", true);
        //                    } else { $("#SUABP").prop("checked", false); }

        //                    if (data.SUAFP == "SUAFP") {
        //                        $("#SUAFP").prop("checked", true);
        //                    } else { $("#SUAFP").prop("checked", false); }


        //                    //Setting Check All
        //                    if ($("#SCL:checked,#SCBI:checked,#SCPI:checked,#SCNA:checked,#SLOU:checked,#SCNG:checked,#SLOG:checked,#SAGI:checked,#SUACP:checked,#SUAP:checked,#SFYD:checked,#SCFY:checked,#SCNY:checked,#SCF:checked,#SCD:checked,#SCET:checked,#SCCOA:checked,#SACTC:checked,#SCAA:checked,#SCAAD:checked,#SCCC:checked,#SCMCC:checked,#SUMCC:checked,#SCCCA:checked,#SCAG:checked,#SCCA:checked,#SCY:checked,#SBUAP:checked,#SFUAP:checked,#SDATCA:checked,#SCATCA:checked,#SOCP:checked,#SUP:checked,#SPS:checked,#SPD:checked,#SUETR:checked,#SUABP:checked,#SUAFP:checked").length == 38) {

        //                        $("#CheckAllSetting").prop("checked", true);

        //                    } else {

        //                        $("#CheckAllSetting").prop("checked", false);
        //                    }
        //                    //Inquiry Check All
        //                    if ($("#ILOU:checked,#ICI:checked,#IUP:checked,#ISFP:checked,#ILCOA:checked,#IUFA:checked,#IBA:checked,#ICA:checked,#IFA:checked,#IGA:checked,#ICAA:checked,#ICCC:checked,#ILOC:checked,#ICADA:checked,#ICCCA:checked").length == 15) {

        //                        $("#CheckAllInquiry").prop("checked", true);

        //                    } else {

        //                        $("#CheckAllInquiry").prop("checked", false);
        //                    }
        //                    //Transaction Check All
        //                    if ($("#TCCB:checked, #TCGE:checked").length == 2) {

        //                        $("#CheckAllTransaction").prop("checked", true);

        //                    } else {

        //                        $("#CheckAllTransaction").prop("checked", false);
        //                    }

        //                }
        //            });
        //            $("#FormsTable").show();
        //            $("#SaveBtn").prop("disabled", true);
        //        }
        //        else if (data === "False") {

        //            $("#GroupDisactiveError").hide();

        //            $("#SCL").prop("disabled", false);
        //            $("#SCBI").prop("disabled", false);
        //            $("#SCPI").prop("disabled", false);
        //            $("#SCNA").prop("disabled", false);
        //            $("#SLOU").prop("disabled", false);
        //            $("#SCNG").prop("disabled", false);
        //            $("#SLOG").prop("disabled", false);
        //            $("#SAGI").prop("disabled", false);
        //            $("#SUACP").prop("disabled", false);
        //            $("#SUAP").prop("disabled", false);
        //            $("#SFYD").prop("disabled", false);
        //            $("#SCFY").prop("disabled", false);
        //            $("#SCNY").prop("disabled", false);
        //            $("#SCF").prop("disabled", false);
        //            $("#SCD").prop("disabled", false);
        //            $("#SCET").prop("disabled", false);
        //            $("#ILOU").prop("disabled", false);
        //            $("#SCCOA").prop("disabled", false);
        //            $("#ICI").prop("disabled", false);
        //            $("#IUP").prop("disabled", false);
        //            $("#ISFP").prop("disabled", false);
        //            $("#SACTC").prop("disabled", false);
        //            $("#ILCOA").prop("disabled", false);
        //            $("#SCAA").prop("disabled", false);
        //            $("#SCAAD").prop("disabled", false);
        //            $("#SCCC").prop("disabled", false);
        //            $("#SCMCC").prop("disabled", false);
        //            $("#SUMCC").prop("disabled", false);
        //            $("#SCCCA").prop("disabled", false);
        //            $("#SCAG").prop("disabled", false);
        //            $("#SCCA").prop("disabled", false);
        //            $("#SCY").prop("disabled", false);
        //            $("#SBUAP").prop("disabled", false);
        //            $("#SFUAP").prop("disabled", false);
        //            $("#IUFA").prop("disabled", false);
        //            $("#IBA").prop("disabled", false);
        //            $("#ICA").prop("disabled", false);
        //            $("#IFA").prop("disabled", false);
        //            $("#IGA").prop("disabled", false);
        //            $("#SDATCA").prop("disabled", false);
        //            $("#SCATCA").prop("disabled", false);
        //            $("#ICAA").prop("disabled", false);
        //            $("#ICCC").prop("disabled", false);
        //            $("#ILOC").prop("disabled", false);
        //            $("#SOCP").prop("disabled", false);
        //            $("#SUP").prop("disabled", false);
        //            $("#SPS").prop("disabled", false);
        //            $("#ICADA").prop("disabled", false);
        //            $("#SPD").prop("disabled", false);
        //            $("#ICCCA").prop("disabled", false);
        //            $("#TCCB").prop("disabled", false);
        //            $("#SUETR").prop("disabled", false);
        //            $("#TCGE").prop("disabled", false);
        //            $("#SUABP").prop("disabled", false);
        //            $("#SUAFP").prop("disabled", false);
        //            $("#CheckAllSetting").prop("disabled", false);
        //            $("#CheckAllInquiry").prop("disabled", false);

        //            $.ajax({
        //                type: "GET",
        //                url: "/UGFormsAccess/GetGroupAccess?GroupID=" + GroupID,
        //                success: function (data) {
        //                    if (data.SCL == "SCL") {
        //                        $("#SCL").prop("checked", true);
        //                    } else { $("#SCL").prop("checked", false); }

        //                    if (data.SCBI == "SCBI") {
        //                        $("#SCBI").prop("checked", true);
        //                    } else { $("#SCBI").prop("checked", false); }

        //                    if (data.SCPI == "SCPI") {
        //                        $("#SCPI").prop("checked", true);
        //                    } else { $("#SCPI").prop("checked", false); }

        //                    if (data.SCNA == "SCNA") {
        //                        $("#SCNA").prop("checked", true);
        //                    } else { $("#SCNA").prop("checked", false); }

        //                    if (data.SLOU == "SLOU") {
        //                        $("#SLOU").prop("checked", true);
        //                    } else { $("#SLOU").prop("checked", false); }

        //                    if (data.SCNG == "SCNG") {
        //                        $("#SCNG").prop("checked", true);
        //                    } else { $("#SCNG").prop("checked", false); }

        //                    if (data.SLOG == "SLOG") {
        //                        $("#SLOG").prop("checked", true);
        //                    } else { $("#SLOG").prop("checked", false); }

        //                    if (data.SAGI == "SAGI") {
        //                        $("#SAGI").prop("checked", true);
        //                    } else { $("#SAGI").prop("checked", false); }

        //                    if (data.SUACP == "SUACP") {
        //                        $("#SUACP").prop("checked", true);
        //                    } else { $("#SUACP").prop("checked", false); }

        //                    if (data.SUAP == "SUAP") {
        //                        $("#SUAP").prop("checked", true);
        //                    } else { $("#SUAP").prop("checked", false); }

        //                    if (data.SFYD == "SFYD") {
        //                        $("#SFYD").prop("checked", true);
        //                    } else { $("#SFYD").prop("checked", false); }

        //                    if (data.SCFY == "SCFY") {
        //                        $("#SCFY").prop("checked", true);
        //                    } else { $("#SCFY").prop("checked", false); }

        //                    if (data.SCNY == "SCNY") {
        //                        $("#SCNY").prop("checked", true);
        //                    } else { $("#SCNY").prop("checked", false); }

        //                    if (data.SCF == "SCF") {
        //                        $("#SCF").prop("checked", true);
        //                    } else { $("#SCF").prop("checked", false); }

        //                    if (data.SCD == "SCD") {
        //                        $("#SCD").prop("checked", true);
        //                    } else { $("#SCD").prop("checked", false); }

        //                    if (data.SCET == "SCET") {
        //                        $("#SCET").prop("checked", true);
        //                    } else { $("#SCET").prop("checked", false); }

        //                    if (data.ILOU == "ILOU") {
        //                        $("#ILOU").prop("checked", true);
        //                    } else { $("#ILOU").prop("checked", false); }

        //                    if (data.SCCOA == "SCCOA") {
        //                        $("#SCCOA").prop("checked", true);
        //                    } else { $("#SCCOA").prop("checked", false); }

        //                    if (data.ICI == "ICI") {
        //                        $("#ICI").prop("checked", true);
        //                    } else { $("#ICI").prop("checked", false); }

        //                    if (data.IUP == "IUP") {
        //                        $("#IUP").prop("checked", true);
        //                    } else { $("#IUP").prop("checked", false); }

        //                    if (data.ISFP == "ISFP") {
        //                        $("#ISFP").prop("checked", true);
        //                    } else { $("#ISFP").prop("checked", false); }

        //                    if (data.SACTC == "SACTC") {
        //                        $("#SACTC").prop("checked", true);
        //                    } else { $("#SACTC").prop("checked", false); }

        //                    if (data.ILCOA == "ILCOA") {
        //                        $("#ILCOA").prop("checked", true);
        //                    } else { $("#ILCOA").prop("checked", false); }

        //                    if (data.SCAA == "SCAA") {
        //                        $("#SCAA").prop("checked", true);
        //                    } else { $("#SCAA").prop("checked", false); }

        //                    if (data.SCAAD == "SCAAD") {
        //                        $("#SCAAD").prop("checked", true);
        //                    } else { $("#SCAAD").prop("checked", false); }

        //                    if (data.SCCC == "SCCC") {
        //                        $("#SCCC").prop("checked", true);
        //                    } else { $("#SCCC").prop("checked", false); }

        //                    if (data.SCMCC == "SCMCC") {
        //                        $("#SCMCC").prop("checked", true);
        //                    } else { $("#SCMCC").prop("checked", false); }

        //                    if (data.SUMCC == "SUMCC") {
        //                        $("#SUMCC").prop("checked", true);
        //                    } else { $("#SUMCC").prop("checked", false); }

        //                    if (data.SCCCA == "SCCCA") {
        //                        $("#SCCCA").prop("checked", true);
        //                    } else { $("#SCCCA").prop("checked", false); }

        //                    if (data.SCAG == "SCAG") {
        //                        $("#SCAG").prop("checked", true);
        //                    } else { $("#SCAG").prop("checked", false); }

        //                    if (data.SCCA == "SCCA") {
        //                        $("#SCCA").prop("checked", true);
        //                    } else { $("#SCCA").prop("checked", false); }

        //                    if (data.SCY == "SCY") {
        //                        $("#SCY").prop("checked", true);
        //                    } else { $("#SCY").prop("checked", false); }

        //                    if (data.SBUAP == "SBUAP") {
        //                        $("#SBUAP").prop("checked", true);
        //                    } else { $("#SBUAP").prop("checked", false); }

        //                    if (data.SFUAP == "SFUAP") {
        //                        $("#SFUAP").prop("checked", true);
        //                    } else { $("#SFUAP").prop("checked", false); }

        //                    if (data.IUFA == "IUFA") {
        //                        $("#IUFA").prop("checked", true);
        //                    } else { $("#IUFA").prop("checked", false); }

        //                    if (data.IBA == "IBA") {
        //                        $("#IBA").prop("checked", true);
        //                    } else { $("#IBA").prop("checked", false); }

        //                    if (data.ICA == "ICA") {
        //                        $("#ICA").prop("checked", true);
        //                    } else { $("#ICA").prop("checked", false); }

        //                    if (data.IFA == "IFA") {
        //                        $("#IFA").prop("checked", true);
        //                    } else { $("#IFA").prop("checked", false); }

        //                    if (data.IGA == "IGA") {
        //                        $("#IGA").prop("checked", true);
        //                    } else { $("#IGA").prop("checked", false); }

        //                    if (data.SDATCA == "SDATCA") {
        //                        $("#SDATCA").prop("checked", true);
        //                    } else { $("#SDATCA").prop("checked", false); }

        //                    if (data.SCATCA == "SCATCA") {
        //                        $("#SCATCA").prop("checked", true);
        //                    } else { $("#SCATCA").prop("checked", false); }

        //                    if (data.ICAA == "ICAA") {
        //                        $("#ICAA").prop("checked", true);
        //                    } else { $("#ICAA").prop("checked", false); }

        //                    if (data.ICCC == "ICCC") {
        //                        $("#ICCC").prop("checked", true);
        //                    } else { $("#ICCC").prop("checked", false); }

        //                    if (data.ILOC == "ILOC") {
        //                        $("#ILOC").prop("checked", true);
        //                    } else { $("#ILOC").prop("checked", false); }

        //                    if (data.SOCP == "SOCP") {
        //                        $("#SOCP").prop("checked", true);
        //                    } else { $("#SOCP").prop("checked", false); }

        //                    if (data.SUP == "SUP") {
        //                        $("#SUP").prop("checked", true);
        //                    } else { $("#SUP").prop("checked", false); }

        //                    if (data.SPS == "SPS") {
        //                        $("#SPS").prop("checked", true);
        //                    } else { $("#SPS").prop("checked", false); }

        //                    if (data.ICADA == "ICADA") {
        //                        $("#ICADA").prop("checked", true);
        //                    } else { $("#ICADA").prop("checked", false); }

        //                    if (data.SPD == "SPD") {
        //                        $("#SPD").prop("checked", true);
        //                    } else { $("#SPD").prop("checked", false); }

        //                    if (data.ICCCA == "ICCCA") {
        //                        $("#ICCCA").prop("checked", true);
        //                    } else { $("#ICCCA").prop("checked", false); }

        //                    if (data.TCCB == "TCCB") {
        //                        $("#TCCB").prop("checked", true);
        //                    } else { $("#TCCB").prop("checked", false); }

        //                    if (data.SUETR == "SUETR") {
        //                        $("#SUETR").prop("checked", true);
        //                    } else { $("#SUETR").prop("checked", false); }

        //                    if (data.TCGE == "TCGE") {
        //                        $("#TCGE").prop("checked", true);
        //                    } else { $("#TCGE").prop("checked", false); }

        //                    if (data.SUABP == "SUABP") {
        //                        $("#SUABP").prop("checked", true);
        //                    } else { $("#SUABP").prop("checked", false); }

        //                    if (data.SUAFP == "SUAFP") {
        //                        $("#SUAFP").prop("checked", true);
        //                    } else { $("#SUAFP").prop("checked", false); }

        //                    //Setting Check All
        //                    if ($("#SCL:checked,#SCBI:checked,#SCPI:checked,#SCNA:checked,#SLOU:checked,#SCNG:checked,#SLOG:checked,#SAGI:checked,#SUACP:checked,#SUAP:checked,#SFYD:checked,#SCFY:checked,#SCNY:checked,#SCF:checked,#SCD:checked,#SCET:checked,#SCCOA:checked,#SACTC:checked,#SCAA:checked,#SCAAD:checked,#SCCC:checked,#SCMCC:checked,#SUMCC:checked,#SCCCA:checked,#SCAG:checked,#SCCA:checked,#SCY:checked,#SBUAP:checked,#SFUAP:checked,#SDATCA:checked,#SCATCA:checked,#SOCP:checked,#SUP:checked,#SPS:checked,#SPD:checked,#SUETR:checked,#SUABP:checked,#SUAFP:checked").length == 38) {

        //                        $("#CheckAllSetting").prop("checked", true);

        //                    } else {

        //                        $("#CheckAllSetting").prop("checked", false);
        //                    }
        //                    //Inquiry Check All
        //                    if ($("#ILOU:checked,#ICI:checked,#IUP:checked,#ISFP:checked,#ILCOA:checked,#IUFA:checked,#IBA:checked,#ICA:checked,#IFA:checked,#IGA:checked,#ICAA:checked,#ICCC:checked,#ILOC:checked,#ICADA:checked,#ICCCA:checked").length == 15) {

        //                        $("#CheckAllInquiry").prop("checked", true);

        //                    } else {

        //                        $("#CheckAllInquiry").prop("checked", false);
        //                    }
        //                    //Transaction Check All
        //                    if ($("#TCCB:checked, #TCGE:checked").length == 2) {

        //                        $("#CheckAllTransaction").prop("checked", true);

        //                    } else {

        //                        $("#CheckAllTransaction").prop("checked", false);
        //                    }
        //                }
        //            });


        //            $("#FormsTable").show();
        //            $("#SaveBtn").prop("disabled", false);

        //        }
        //    }
        //});