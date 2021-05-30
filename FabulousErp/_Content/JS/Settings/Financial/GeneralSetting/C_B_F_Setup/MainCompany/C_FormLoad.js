$(document).ready(function () {
    
    $("#InputLogo").change(function () {

        var File = this.files;

        if (File && File[0])
        {
            ReadImage(File[0]);
        }
    });
    /*
    $(".Docs").change(function () {

        var File = this.files;

        if (File && File[0]) {
            ReadDoc(File[0]);
        }
    });
    */
    /*
    var Check = $("#SuccessSubmit").text();

    if (Check.length > 0) {
        $("#PrintSpan").show("slow");
        $("#CompanyID").prop("disabled", true);
        $("#Currency").prop("disabled", true);
        $(".glyphicon-lock").show("slow");
        $("#SaveBtn").prop("disabled", true);
    }
    */

    //to not hide the dives of communication that have data after submit cause user maybe edit of it
    var International2 = $("#International2").val();
    var Telephone2 = $("#Telephone2").val();
    if (International2.length > 0 || Telephone2.length > 0) {
        $("#T2").show("fast");
    } else { $("#T2").hide("fast"); }


    var International3 = $("#International3").val();
    var Telephone3 = $("#Telephone3").val();
    if (International3.length > 0 || Telephone3.length > 0) {
        $("#T3").show("fast");
    } else { $("#T3").hide("fast"); }


    var International4 = $("#International4").val();
    var Telephone4 = $("#Telephone4").val();
    if (International4.length > 0 || Telephone4.length > 0) {
        $("#T4").show("fast");
    } else { $("#T4").hide("fast"); }


    var International5 = $("#International5").val();
    var Telephone5 = $("#Telephone5").val();
    if (International5.length > 0 || Telephone5.length > 0) {
        $("#T5").show("fast");
    } else { $("#T5").hide("fast"); }


    var Fax2 = $("#Fax2").val();
    if (Fax2.length > 0) {
        $("#F2").show("fast");
    } else { $("#F2").hide("fast"); }

    var Fax3 = $("#Fax3").val();
    if (Fax3.length > 0) {
        $("#F3").show("fast");
    } else { $("#F3").hide("fast"); }

    var Fax4 = $("#Fax4").val();
    if (Fax4.length > 0) {
        $("#F4").show("fast");
    } else { $("#F4").hide("fast"); }

    var Fax5 = $("#Fax5").val();
    if (Fax5.length > 0) {
        $("#F5").show("fast");
    } else { $("#F5").hide("fast"); }

});

function ReadImage(file)
{
    var reader = new FileReader;
    var image = new Image;

    reader.readAsDataURL(file);
    reader.onload = function (_file)
    {
        image.src = _file.target.result;
        image.onload = function () {

            $("#ImageLogo").attr('src', _file.target.result);
        }
    }
}

/*
function ReadDoc(file) {
    var reader = new FileReader;
    var image = new Image;

    reader.readAsDataURL(file);
    reader.onload = function (_file) {
        image.src = _file.target.result;
        image.onload = function () {

            $("#Documents").attr('src', _file.target.result);
        }
    }
}
*/