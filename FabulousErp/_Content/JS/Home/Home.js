$(".transactions-field").mouseover(function () {
    $(".transactions-field").css("border", "2px solid #b366ff");
});
$(".inquiry-field").mouseover(function () {
    $(".inquiry-field").css("border", "2px solid #ff9980");
});
$(".report-field").mouseover(function () {
    $(".report-field").css("border", "2px solid #b3d1ff");
});
$(".Setting-Field").mouseover(function () {
    $(".Setting-Field").css("border", "2px solid #40bf80");
});
$(".transactions-field").mouseout(function () {
    $(".transactions-field").css("border", "0 none");
});
$(".inquiry-field").mouseout(function () {
    $(".inquiry-field").css("border", "0 none");
});
$(".report-field").mouseout(function () {
    $(".report-field").css("border", "0 none");
});
$(".Setting-Field").mouseout(function () {
    $(".Setting-Field").css("border", "0 none");
});
/***********************************************************************************/

/*********************************************************************************/
function ShowFinancial() {
    var x = $("#Financialpanel");
    var y = $("#GeneralSettingpanel");
    var z = $("#GeneralSettingContent");
    if (x.css('display') == 'none') {
        x.show("slow");
    }
    else {
        x.hide("fast");
        y.hide("fast");
        z.hide("fast");
    }
}

function ShowGeneralSetting() {
    var x = $("#GeneralSettingpanel");
    var y = $("#GeneralSettingContent");
    if (x.css('display') == 'none') {
        x.show("slow");
    }
    else {
        x.hide("fast");
        y.hide("fast");
    }
}

function ShowGeneralSettingContent() {
    var x = $("#GeneralSettingContent");
    if (x.css('display') == 'none') {
        x.show("slow");
    }
    else {
        x.hide("fast");
    }
}
/**********************************************************************************************/
function ShowInquiryFinancial() {
    var x = $("#inquiry-financialpanel");
    var y = $("#inquiry-generalsettingpanel");
    var z = $("#inquiry-generalsettingcontent");
    if (x.css('display') == 'none') {
        x.show("slow");
    }
    else {
        x.hide("fast");
        y.hide("fast");
        z.hide("fast");
    }
}

function ShowInquiryGeneralSetting() {
    var x = $("#inquiry-generalsettingpanel");
    var y = $("#inquiry-generalsettingcontent");
    if (x.css('display') == 'none') {
        x.show("slow");
    }
    else {
        x.hide("fast");
        y.hide("fast");
    }
}

function ShowInquiryGeneralSettingContent() {
    var x = $("#inquiry-generalsettingcontent");
    if (x.css('display') == 'none') {
        x.show("slow");
    }
    else {
        x.hide("fast");
    }
}
/**********************************************************************************************/
function ShowGeneralSettingFav() {
    var x = $("#GeneralSettingFav");
    if (x.css('display') == 'none') {
        x.show("slow");
    }
    else {
        x.hide("fast");
    }
}
