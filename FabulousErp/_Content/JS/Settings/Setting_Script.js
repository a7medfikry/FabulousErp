/* -------------------------------------[Main Script Of Setting & Transaction]----------------------------------
 :: 1.0 Access Of User Post (SUP->User Post)
 :: 2.0 Create Posting Setup (SPS->Posting Setup)
 :: 3.0 Print Document Setting (SPD->Print Document)

 :: 4.0 C_Create Batch (TCCB->Company Create Batch)
 :: 5.0 B_Create Batch (TBCB->Branch Create Batch)
 :: 6.0 F_Create Batch (TFCB->Factory Create Batch)

 :: 7.0 User Edit T Rate (SUETR->U Edit Rate)

 :: 8.0 C_General Entry Transaction (TCGE->Company General Entry Transaction)
----------------------------------------------------------------------------------------------------------------*/



/*------------------------------------------
:: 1.0 Access Of User Post (SUP->User Post)
--------------------------------------------*/

/*
 * 1.1 Change of DropDown User ID To Get Name of User
 * 1.2 Change of DropDown Form Name
 * 1.3 Click Of Button That Add Access Of Post To User
 * 1.4 Function To Get Access Forms of User In => ( Change Of User ID & Btn User Post Access ) In Table
 * 1.5 Function From Table Delete Post Access To Show Pop Up And Pass UPID(ID identity) of User
 * 1.6 Click Of Button In popUp That Confirm Delete Post Access From User
*/

//1.1 Change of DropDown User ID To Get Name of User
$("#SUP-UserID").change(function () {

    var userID = $(this).val();
    $("#SUP-UserName").val("");
    $("#SUP-ErrorSaveBtn").text("");
    $("#SUP-FormCode").val("");

    if (userID.length === 0) {
        $(this).css("border-color", "red");
    } else {
        $(this).css("border-color", "");

        //Get Post Access Form of This User
        SUPGetAccessOfUser(userID);

        $.ajax({
            type: "GET",
            url: "/AccessOfUserPost/GetUserName?userID=" + userID,
            success: function (result) {
                $("#SUP-UserName").val(result);
            }
        });
    }
});

//1.2 Change of DropDown Form Name
$("#SUP-FormCode").change(function () {

    var formCode = $(this).val();
    $("#SUP-FormName").val("");

    if (formCode.length === 0) {
        $(this).css("border-color", "red");
    } else {
        $(this).css("border-color", "");
        console.log(formCode);
        if (formCode === "TCJE") {
            $("#SUP-FormName").val("Company Transaction Journal Entry");
        }
    }
});

//1.3 Click Of Button That Add Access Of Post To User
$("#SUP-UPostAccessBtn").click(function () {

    var userID = $("#SUP-UserID").val();

    var formCode = $("#SUP-FormCode").val();

    //------Validation-------
    var Test = true;

    if (userID.length === 0) {
        $("#SUP-UserID").css("border-color", "red");
        Test = false;
    } else {
        $("#SUP-UserID").css("border-color", "");
    }

    if (formCode.length === 0) {
        $("#SUP-FormCode").css("border-color", "red");
        Test = false;
    } else {
        $("#SUP-FormCode").css("border-color", "");
    }
    //------------------------

    if (Test === true) {

        //Send Values By Ajax
        $.ajax({
            type: "POST",
            url: "/AccessOfUserPost/SaveUserPostAccess?userID=" + userID + "&formCode=" + formCode,
            success: function (result) {
                $("#SUP-ErrorSaveBtn").text(result);

                //Show Saved Post Access Form of This User
                SUPGetAccessOfUser(userID);

            }
        });

    }

});

//1.4 Function To Get Access Forms of User In => ( Change Of User ID & Btn User Post Access ) In Table
function SUPGetAccessOfUser(userID) {

    var tblData = $("#SUP-UPostAccessTbl");
    tblData.html("");

    $.ajax({
        type: "GET",
        url: "/AccessOfUserPost/GetAccessOfUser?userID=" + userID,
        success: function (result) {

            if (result.length === 0) {
                tblData.append('<tr style="color:red"><td colspan = "3" > This User Not Have Access To Any Post Form </td></tr>');
            } else {
                for (var i = 0; i < result.length; i++) {

                    var formName = "";
                    if (result[i].FormCode === "TCJE") {
                        formName = "Company Transaction Journal Entry";
                    }

                    var data = "<tr class='SUprow_" + result[i].ID + "'>" +
                        "<td width='20%'>" + result[i].UserID + "</td>" +
                        "<td width='40%'>" + result[i].FormCode + "</td>" +
                        "<td width='40%'>" + formName + "</td>" +
                        "<td width='40%'>" + '<a href="#" class="btn btn-danger btn-sm" onclick="SUPDeletePostAccess(\'' + result[i].ID + '\')"><span class="fa fa-trash-o"></span></a>' + "</td>" +
                        "</tr>";
                    tblData.append(data);
                }
            }
        }
    });
}

//1.5 Function From Table Delete Post Access To Show Pop Up And Pass UPID(ID identity) of User
function SUPDeletePostAccess(ID) {
    $("#SUP-PUUserID").text(ID);
    $("#SUP-DeleteConfirmation").modal("show");
}

//1.6 Click Of Button In popUp That Confirm Delete Post Access From User
$("#SUP-ConfirmDeletePostAccessBtn").click(function () {

    var userID = $("#SUP-PUUserID").text();

    $.ajax({
        type: "POST",
        url: "/AccessOfUserPost/RemoveAccessOfUser?userID=" + userID,
        success: function (result) {
            $(".SUprow_" + userID).remove(); //Remove Item From Showed Table
            $("#SUP-DeleteConfirmation").modal("hide"); //Hide PopUp
        }
    });

});

//----------------------------------------END OF SUP-------------------------------------------------



/*-----------------------------------------------
:: 2.0 Create Posting Setup (SPS->Posting Setup)
-------------------------------------------------*/

/*
 * 2.1 Change of DropDown Comapny ID To Get Company Name
 * 2.2 Change of DropDown Module & Make Search by Company ID and module if exist Data
 * 2.3 Btn Save of Posting Setup
 * 2.4 Btn Update of Posting setup
 * 2.5 Btn Reset to Reload Page
 * 2.6 Event Change of Radio Button To Remove border error in Select and click of JEP Batch OR Trans. to Open all In Buttom OR Close
 */

//2.1 Change of DropDown Comapny ID To Get Company Name
//$("#SPS-CompanyID").change(function () {

//    var companyID = $(this).val();
//    $("#SPS-CompanyName").val("");

//    //-------Clear module dropdown------------
//    $("#SPS-Module").prop("disabled", true);
//    $("#SPS-Module").css("border-color", "");
//    $("#SPS-Module").val("");
//    //----------------------------------------

//    if (companyID.length === 0) {
//        $(this).css("border-color", "red");
//    } else {
//        $(this).css("border-color", "");

//        $.ajax({
//            type: "GET",
//            url: "/CreatePostingSetup/GetCompanyName?companyID=" + companyID,
//            success: function (result) {
//                $("#SPS-CompanyName").val(result);
//                $("#SPS-Module").prop("disabled", false);
//            }
//        });

//    }

//});

//2.2 Change of DropDown Module & Make Search by Company ID and module if exist Data
$("#SPS-Module").change(function () {

    var module = $(this).val();

    $("#SPS-ErrorSearch").text("");

    //Reset Buttons in change of module to disable one of it
    $("#SPS-SavePSBtn").prop("disabled", false);
    $("#SPS-UpdatePSBtn").prop("disabled", false);
    //------------------------------------------------------

    if (module.length === 0) {
        $(this).css("border-color", "red");
    } else {
        $(this).css("border-color", "");

        //-------------------Search by module and company to Get Data if Exist------------------------------
        var companyID = $("#SPS-CompanyID").val();
        $.ajax({
            type: "GET",
            url: "/CreatePostingSetup/GetPostingSetup?companyID=" + companyID + "&module=" + module,
            success: function (result) {

                if (result === "False") {
                    //If No matching Data close update button
                    $("#SPS-UpdatePSBtn").prop("disabled", true);
                } else {
                    //If Existing matching data Close saveBtn and main data
                    $("#SPS-CompanyID").prop("disabled", true);
                    $("#SPS-Module").prop("disabled", true);
                    $("#SPS-SavePSBtn").prop("disabled", true);
                    //------------------------------------------------------

                    //Remove validation error in search because Data get Correctly
                    $(".SPS-PostingTypelbl").css("border", "");
                    $(".SPS-JournalEntryPerlbl").css("border", "");
                    $(".SPS-Batchlbl").css("border", "");
                    $(".SPS-PostingDataFromlbl").css("border", "");
                    $(".SPS-IfExistingBatchlbl").css("border", "");
                    $(".SPS-EIPDlbl").css("border", "");
                    //-------------------------------------------------------------

                    //Enable This all three RB if was choosen JEPer Batch with value B2 to can update of it
                    if (result.CreateJEPer === "B2") {
                        $("input[name=SPS-Batch]").prop("disabled", false);
                        $("input[name=SPS-PostingDataFrom]").prop("disabled", false);
                        $("input[name=SPS-IfExistingBatch]").prop("disabled", false);

                        $("input[name=SPS-Batch][value=" + result.Batch + "]").prop('checked', true);
                        $("input[name=SPS-PostingDataFrom][value=" + result.PostingDataFrom + "]").prop('checked', true);
                        $("input[name=SPS-IfExistingBatch][value=" + result.ExistingBatch + "]").prop('checked', true);
                    }
                    //--------------------------------------------------------------------------------------

                    $("input[name=SPS-PostingType][value=" + result.PostingType + "]").prop('checked', true);
                    $("input[name=SPS-JournalEntryPer][value=" + result.CreateJEPer + "]").prop('checked', true);
                    $("input[name=SPS-EIPD][value=" + result.EditPostingDate + "]").prop('checked', true);

                }
            }
        });
        //---------------------------------End Of Search----------------------------------------------------------
    }
});

//2.3 Btn Save of Posting Setup
$("#SPS-SavePSBtn").click(function () {

    var companyID = $("#SPS-CompanyID").val();

    var module = $("#SPS-Module").val();

    var Test = true;

    //Validation in Button Click and Get value of radio if checked
    if (companyID.length === 0) {
        $("#SPS-CompanyID").css("border-color", "red");
        Test = false;
    } else {
        $("#SPS-CompanyID").css("border-color", "");
    }

    if (module.length === 0) {
        $("#SPS-Module").css("border-color", "red");
        Test = false;
    } else {
        $("#SPS-Module").css("border-color", "");
    }

    var postingType = "";
    if ($('input[name=SPS-PostingType]:checked').length <= 0) {
        Test = false;
        $(".SPS-PostingTypelbl").css("border", "1px solid red");
    } else {
        $(".SPS-PostingTypelbl").css("border", "");
        postingType = $("input[name=SPS-PostingType]:checked").val();
    }

    var journalEntryPer = "";
    if ($('input[name=SPS-JournalEntryPer]:checked').length <= 0) {
        Test = false;
        $(".SPS-JournalEntryPerlbl").css("border", "1px solid red");
    } else {
        $(".SPS-JournalEntryPerlbl").css("border", "");
        journalEntryPer = $("input[name=SPS-JournalEntryPer]:checked").val();
    }

    var editPostingDate = "";
    if ($('input[name=SPS-EIPD]:checked').length <= 0) {
        Test = false;
        $(".SPS-EIPDlbl").css("border", "1px solid red");
    } else {
        $(".SPS-EIPDlbl").css("border", "");
        editPostingDate = $("input[name=SPS-EIPD]:checked").val();
    }

    //This validations when JEPBatch checked because all mandatory when it checked
    var batch = "";
    if ($('input[name=SPS-Batch]:checked').length <= 0 && $("#SPS-JEPBatch").is(":checked")) {
        Test = false;
        $(".SPS-Batchlbl").css("border", "1px solid red");
    } else if ($('input[name=SPS-Batch]:checked').length > 0 && $("#SPS-JEPBatch").is(":checked")) {
        $(".SPS-Batchlbl").css("border", "");
        batch = $("input[name=SPS-Batch]:checked").val();
    }

    var postingDataFrom = "";
    if ($('input[name=SPS-PostingDataFrom]:checked').length <= 0 && $("#SPS-JEPBatch").is(":checked")) {
        Test = false;
        $(".SPS-PostingDataFromlbl").css("border", "1px solid red");
    } else if ($('input[name=SPS-PostingDataFrom]:checked').length > 0 && $("#SPS-JEPBatch").is(":checked")) {
        $(".SPS-PostingDataFromlbl").css("border", "");
        postingDataFrom = $("input[name=SPS-PostingDataFrom]:checked").val();
    }

    var existingBatch = "";
    if ($('input[name=SPS-IfExistingBatch]:checked').length <= 0 && $("#SPS-JEPBatch").is(":checked")) {
        Test = false;
        $(".SPS-IfExistingBatchlbl").css("border", "1px solid red");
    } else if ($('input[name=SPS-IfExistingBatch]:checked').length > 0 && $("#SPS-JEPBatch").is(":checked")) {
        $(".SPS-IfExistingBatchlbl").css("border", "");
        existingBatch = $("input[name=SPS-IfExistingBatch]:checked").val();
    }

    //----------------------End Validation-----------------------------------------

    if (Test === true) {
        $.ajax({
            type: "POST",
            url: "/CreatePostingSetup/SavePostingSetup?companyID=" + companyID + "&module=" + module + "&postingType=" + postingType + "&journalEntryPer=" + journalEntryPer + "&batch=" + batch + "&postingDataFrom=" + postingDataFrom + "&existingBatch=" + existingBatch + "&editPostingDate=" + editPostingDate,
            success: function () {
                location.reload();
            }
        });
    }
});

//2.4 Btn Update of Posting setup
$("#SPS-UpdatePSBtn").click(function () {

    var Test = true;

    $('#SPS-Error').text('');

    var companyID = $("#SPS-CompanyID").val();

    var module = $("#SPS-Module").val();

    var postingType = $("input[name=SPS-PostingType]:checked").val();

    var journalEntryPer = $("input[name=SPS-JournalEntryPer]:checked").val();

    var editPostingDate = $("input[name=SPS-EIPD]:checked").val();

    var batch = "";
    if ($('input[name=SPS-Batch]:checked').length <= 0 && $("#SPS-JEPBatch").is(":checked")) {
        Test = false;
        $(".SPS-Batchlbl").css("border", "1px solid red");
    } else if ($('input[name=SPS-Batch]:checked').length > 0 && $("#SPS-JEPBatch").is(":checked")) {
        $(".SPS-Batchlbl").css("border", "");
        batch = $("input[name=SPS-Batch]:checked").val();
    }

    var postingDataFrom = "";
    if ($('input[name=SPS-PostingDataFrom]:checked').length <= 0 && $("#SPS-JEPBatch").is(":checked")) {
        Test = false;
        $(".SPS-PostingDataFromlbl").css("border", "1px solid red");
    } else if ($('input[name=SPS-PostingDataFrom]:checked').length > 0 && $("#SPS-JEPBatch").is(":checked")) {
        $(".SPS-PostingDataFromlbl").css("border", "");
        postingDataFrom = $("input[name=SPS-PostingDataFrom]:checked").val();
    }

    var existingBatch = "";
    if ($('input[name=SPS-IfExistingBatch]:checked').length <= 0 && $("#SPS-JEPBatch").is(":checked")) {
        Test = false;
        $(".SPS-IfExistingBatchlbl").css("border", "1px solid red");
    } else if ($('input[name=SPS-IfExistingBatch]:checked').length > 0 && $("#SPS-JEPBatch").is(":checked")) {
        $(".SPS-IfExistingBatchlbl").css("border", "");
        existingBatch = $("input[name=SPS-IfExistingBatch]:checked").val();
    }

    if (Test === true) {
        $.ajax({
            type: "POST",
            url: "/CreatePostingSetup/UpdatePostingSetup?companyID=" + companyID + "&module=" + module + "&postingType=" + postingType + "&journalEntryPer=" + journalEntryPer + "&batch=" + batch + "&postingDataFrom=" + postingDataFrom + "&existingBatch=" + existingBatch + "&editPostingDate=" + editPostingDate,
            success: function (result) {
                if (result === "ExistData") {
                    $('#SPS-Error').text("Please Post Saved Batches..");
                } else {
                    location.reload();
                }
            }
        });
    }
});

//2.5 Btn Reset to Reload Page
$("#SPS-ResetBtn").click(function () {
    location.reload();
});

//2.6 Event Change of Radio Button To Remove border error in Select and click of JEP Batch OR Trans. to Open all In Buttom OR Close
$("input[name=SPS-PostingType]").change(function () {
    $(".SPS-PostingTypelbl").css("border", "");
});
$("input[name=SPS-JournalEntryPer]").change(function () {
    $(".SPS-JournalEntryPerlbl").css("border", "");
});
$("input[name=SPS-Batch]").change(function () {
    $(".SPS-Batchlbl").css("border", "");
});
$("input[name=SPS-PostingDataFrom]").change(function () {
    $(".SPS-PostingDataFromlbl").css("border", "");
});
$("input[name=SPS-IfExistingBatch]").change(function () {
    $(".SPS-IfExistingBatchlbl").css("border", "");
});
$("input[name=SPS-EIPD]").change(function () {
    $(".SPS-EIPDlbl").css("border", "");
});
$("#SPS-JEPBatch").click(function () {
    if ($("#SPS-JEPBatch").is(':checked')) {
        $("input[name=SPS-Batch]").prop("disabled", false);
        $("input[name=SPS-PostingDataFrom]").prop("disabled", false);
        $("input[name=SPS-IfExistingBatch]").prop("disabled", false);
    }
});
$("#SPS-JEPTransaction").click(function () {
    if ($("#SPS-JEPTransaction").is(':checked')) {
        $("input[name=SPS-Batch]").prop("disabled", true);
        $("input[name=SPS-PostingDataFrom]").prop("disabled", true);
        $("input[name=SPS-IfExistingBatch]").prop("disabled", true);
        //Remove Validation and checked of buttom of it because it no mandatory when check JEPTrans.
        $(".SPS-Batchlbl").css("border", "");
        $(".SPS-PostingDataFromlbl").css("border", "");
        $(".SPS-IfExistingBatchlbl").css("border", "");

        $("input[name=SPS-Batch]").prop("checked", false);
        $("input[name=SPS-PostingDataFrom]").prop("checked", false);
        $("input[name=SPS-IfExistingBatch]").prop("checked", false);
    }
});

//----------------------------------------END OF SPS-------------------------------------------------



/*--------------------------------------------------
:: 3.0 Print Document Setting (SPD->Print Document)
----------------------------------------------------*/

/*
 * 3.1 Change of DropDown Comapny ID To Get Company Name
 * 3.2 Change of DropDown Module and fill drop down of Transaction Form Name
 * 3.3 Change of DropDown Transaction forms and get data of matching form name and company id and module name
 * 3.4 Btn Save of Print Document
 * 3.5 Btn Update of Print Document
 * 3.6 Btn Reset To Refresh Page
 * 3.7 Change of checkboxes te set value if check by true and uncheck by false
 */

//3.1 Change of DropDown Comapny ID To Get Company Name
$("#SPD-CompanyID").change(function () {

    var companyID = $(this).val();
    $("#SPD-CompanyName").val("");

    //-------Clear module dropdown------------
    $("#SPD-Module").prop("disabled", true);
    $("#SPD-Module").css("border-color", "");
    $("#SPD-Module").val("");
    //----------------------------------------

    if (companyID.length === 0) {
        $(this).css("border-color", "red");
    } else {
        $(this).css("border-color", "");

        $.ajax({
            type: "GET",
            url: "/PrintDocumentSetting/GetCompanyName?companyID=" + companyID,
            success: function (result) {
                $("#SPD-CompanyName").val(result);
                $("#SPD-Module").prop("disabled", false);
            }
        });
    }
});

//3.2 Change of DropDown Module and fill drop down of Transaction Form Name
$("#SPD-Module").change(function () {

    var module = $(this).val();

    //-------Clear Transaction Forms dropdown------------
    $("#SPD-TFormName").prop("disabled", true);
    $("#SPD-TFormName").css("border-color", "");
    $("#SPD-TFormName").val("");
    //----------------------------------------

    //Forms of Financial module
    var financialForms = {
        form1: "Company Journal Entry Transaction"
    };

    if (module.length === 0) {
        $(this).css("border-color", "red");
    } else {
        $(this).css("border-color", "");

        //Open dropdown forms and check the value of module to pass Forms that related with it
        $("#SPD-TFormName").prop("disabled", false);
        $("#SPD-TFormName").empty();
        $("#SPD-TFormName").append($('<option/>', {
            value: "",
            text: "-Choose-"
        })
        );

        if (module === "Financial") {
            $.each(financialForms, function (val, text) {
                $("#SPD-TFormName").append("<option value='" + text + "'>" + text + "</option>")
            });
        }
    }
});

//3.3 Change of DropDown Transaction forms and get data of matching form name and company id and module name
$("#SPD-TFormName").change(function () {

    var tFormName = $(this).val();

    if (tFormName.length === 0) {
        $(this).css("border-color", "red");
    } else {
        $(this).css("border-color", "");
        //Get Data of form name with its company and module if exist
        var companyID = $("#SPD-CompanyID").val();

        var module = $("#SPD-Module").val();

        $.ajax({
            type: "GET",
            url: "/PrintDocumentSetting/GetPrintDocument?companyID=" + companyID + "&module=" + module + "&tFormName=" + tFormName,
            success: function (result) {
                //if not exist matching data close update button to Save id he need
                if (result === "False") {
                    $("#SPD-UpdatePSBtn").prop("disabled", true);
                } else {
                    //if exist data close all main data and retrive data to checkboxes and close save btn
                    $("#SPD-CompanyID").prop("disabled", true);
                    $("#SPD-Module").prop("disabled", true);
                    $("#SPD-TFormName").prop("disabled", true);
                    $("#SPD-SavePSBtn").prop("disabled", true);

                    //else in ask because it by default have check and all :)
                    if (result.Ask === true) {
                        $("#SPD-CBAsk").prop("checked", true);
                        $("#SPD-CBAsk").attr('value', true);
                    } else {
                        $("#SPD-CBAsk").prop("checked", false);
                        $("#SPD-CBAsk").attr('value', false);
                    }

                    if (result.PrintDirect === true) {
                        $("#SPD-CBPrintDirect").prop("checked", true);
                        $("#SPD-CBPrintDirect").attr('value', true);
                    } else {
                        $("#SPD-CBPrintDirect").prop("checked", false);
                        $("#SPD-CBPrintDirect").attr('value', false);
                    }

                    if (result.Analytic === true) {
                        $("#SPD-CBAnalytic").prop("checked", true);
                        $("#SPD-CBAnalytic").attr('value', true);
                    } else {
                        $("#SPD-CBAnalytic").prop("checked", false);
                        $("#SPD-CBAnalytic").attr('value', false);
                    }

                    if (result.CostCenter === true) {
                        $("#SPD-CBCostCenter").prop("checked", true);
                        $("#SPD-CBCostCenter").attr('value', true);
                    } else {
                        $("#SPD-CBCostCenter").prop("checked", false);
                        $("#SPD-CBCostCenter").attr('value', false);
                    }

                }
            }
        });
    }
});

//3.4 Btn Save of Print Document
$("#SPD-SavePSBtn").click(function () {

    var companyID = $("#SPD-CompanyID").val();

    var module = $("#SPD-Module").val();

    var tFormName = $("#SPD-TFormName").val();

    var ask = $("#SPD-CBAsk").val();

    var printDirect = $("#SPD-CBPrintDirect").val();

    var analytic = $("#SPD-CBAnalytic").val();

    var costCenter = $("#SPD-CBCostCenter").val();

    var Test = true;

    //---Validation of company and module and form name---
    if (companyID.length === 0) {
        $("#SPD-CompanyID").css("border-color", "red");
        Test = false;
    } else {
        $("#SPD-CompanyID").css("border-color", "");
    }

    if (module.length === 0) {
        $("#SPD-Module").css("border-color", "red");
        Test = false;
    } else {
        $("#SPD-Module").css("border-color", "");
    }

    if (tFormName.length === 0) {
        $("#SPD-TFormName").css("border-color", "red");
        Test = false;
    } else {
        $("#SPD-TFormName").css("border-color", "");
    }
    //------------------End Validation---------------------

    if (Test === true) {

        $.ajax({
            type: "POST",
            url: "/PrintDocumentSetting/SavePrintDocument?companyID=" + companyID + "&module=" + module + "&tFormName=" + tFormName + "&ask=" + ask + "&printDirect=" + printDirect + "&analytic=" + analytic + "&costCenter=" + costCenter,
            success: function () {
                location.reload();
            }
        });

    }

});

//3.5 Btn Update of Print Document
$("#SPD-UpdatePSBtn").click(function () {

    var companyID = $("#SPD-CompanyID").val();

    var module = $("#SPD-Module").val();

    var tFormName = $("#SPD-TFormName").val();

    var ask = $("#SPD-CBAsk").val();

    var printDirect = $("#SPD-CBPrintDirect").val();

    var analytic = $("#SPD-CBAnalytic").val();

    var costCenter = $("#SPD-CBCostCenter").val();

    $.ajax({
        type: "POST",
        url: "/PrintDocumentSetting/UpdatePrintDocument?companyID=" + companyID + "&module=" + module + "&tFormName=" + tFormName + "&ask=" + ask + "&printDirect=" + printDirect + "&analytic=" + analytic + "&costCenter=" + costCenter,
        success: function () {
            location.reload();
        }
    });
});

//3.6 Btn Reset To Refresh Page
$("#SPD-ResetBtn").click(function () {
    location.reload();
});

//3.7 Change of checkboxes te set value if check by true and uncheck by false
$("#SPD-CBAsk").on('change', function () {
    if ($(this).is(':checked')) {
        $(this).attr('value', true);
    } else {
        $(this).attr('value', false);
    }
});
$("#SPD-CBPrintDirect").on('change', function () {
    if ($(this).is(':checked')) {
        $(this).attr('value', true);
    } else {
        $(this).attr('value', false);
    }
});
$("#SPD-CBAnalytic").on('change', function () {
    if ($(this).is(':checked')) {
        $(this).attr('value', true);
    } else {
        $(this).attr('value', false);
    }
});
$("#SPD-CBCostCenter").on('change', function () {
    if ($(this).is(':checked')) {
        $(this).attr('value', true);
    } else {
        $(this).attr('value', false);
    }
});

//----------------------------------------END OF SPD-------------------------------------------------



/*-------------------------------------------------
:: 4.0 C_Create Batch (TCCB->Company Create Batch)
---------------------------------------------------*/

/*
 * 4.1 Change of Dropdown Company ID to Get Company Name
 * 4.2 Change of Dropdow module for only validation
 * 4.3 Keyup of batchID for only validation
 * 4.4 Keyup of batchID Description for only validation
 * 4.5 Btn Save Company Batch
 * 4.6 Btn Reset To reload Page
 */

//4.1 Change of Dropdown Company ID to Get Company Name
$("#TCCB-CompanyID").change(function () {

    var companyID = $(this).val();

    $("#TCCB-CompanyName").val("");
    $("#TCCB-GlobalError").text("");

    if (companyID.length === 0) {
        $(this).css("border-color", "red");
    } else {
        $(this).css("border-color", "");

        $.ajax({
            type: "GET",
            url: "/C_CreateBatch/GetCompanyName?companyID=" + companyID,
            success: function (result) {
                $("#TCCB-CompanyName").val(result);
            }
        });
    }
});

//4.2 Change of Dropdow module for only validation
$("#TCCB-Module").change(function () {

    var module = $(this).val();

    $("#TCCB-GlobalError").text("");

    if (module.length === 0) {
        $(this).css("border-color", "red");
    } else {
        $(this).css("border-color", "");
    }
});

//4.3 Keyup of batchID for only validation
$("#TCCB-BatchID").keyup(function () {

    var batchID = $(this).val();

    if (batchID.length === 0) {
        $(this).css("border-color", "red");
    } else {
        $(this).css("border-color", "");
    }
});

//4.4 Keyup of batchID Description for only validation
$("#TCCB-BatchDescription").keyup(function () {

    var batchID = $(this).val();

    if (batchID.length === 0) {
        $(this).css("border-color", "red");
    } else {
        $(this).css("border-color", "");
    }
});

//4.5 Btn Save Company Batch
$("#TCCB-SaveBatch").click(function () {

    var companyID = $("#TCCB-CompanyID").val();

    var module = $("#TCCB-Module").val();

    var batchID = $("#TCCB-BatchID").val();

    var batchDescription = $("#TCCB-BatchDescription").val();

    var Test = true;

    //------------Validation check empty Fields------------------
    if (companyID.length === 0) {
        $("#TCCB-CompanyID").css("border-color", "red");
        Test = false;
    } else {
        $("#TCCB-CompanyID").css("border-color", "");
    }

    if (module.length === 0) {
        $("#TCCB-Module").css("border-color", "red");
        Test = false;
    } else {
        $("#TCCB-Module").css("border-color", "");
    }

    if (batchID.length === 0) {
        $("#TCCB-BatchID").css("border-color", "red");
        Test = false;
    } else {
        $("#TCCB-BatchID").css("border-color", "");
    }

    if (batchDescription.length === 0) {
        $("#TCCB-BatchDescription").css("border-color", "red");
        Test = false;
    } else {
        $("#TCCB-BatchDescription").css("border-color", "");
    }
    //----------------------End Validation-------------------------

    if (Test === true) {

        $.ajax({
            type: "POST",
            url: "/C_CreateBatch/SaveCompanyBatch?companyID=" + companyID + "&module=" + module + "&batchID=" + batchID + "&batchDescription=" + batchDescription,
            success: function (result) {
                if (result === "False") {
                    $("#TCCB-GlobalError").text("Batch ID Not Valid In This Module..!");
                    $("#TCCB-BatchID").css("border-color", "red");
                } else {
                    $("#TCCB-GlobalError").text("");
                    $("#TCCB-BatchID").css("border-color", "");
                    location.reload();
                } 
            }
        });

    }

});

//4.6 Btn Reset To reload Page
$("#TCCB-ResetPage").click(function () {
    location.reload();
});

//----------------------------------------END OF TCCB------------------------------------------------



/*------------------------------------------------
:: 5.0 B_Create Batch (TBCB->Branch Create Batch)
--------------------------------------------------*/

/*
 * 5.1 Change of Dropdown Branch ID to Get Branch Name And Check Acess
 * 5.2 Change of Dropdow module for only validation
 * 5.3 Keyup of batchID for only validation
 * 5.4 Keyup of batchID Description for only validation
 * 5.5 Btn Save Branch Batch
 * 5.6 Btn Reset To reload Page
 */

//5.1 Change of Dropdown Branch ID to Get Branch Name And Check Acess
$("#TBCB-BranchID").change(function () {

    var branchID = $(this).val();

    $("#TBCB-BranchName").val("");
    $("#TBCB-GlobalError").text("");

    if (branchID.length === 0) {
        $(this).css("border-color", "red");
    } else {
        $(this).css("border-color", "");

        $.ajax({
            type: "GET",
            url: "/B_CreateBatch/GetBranchName?branchID=" + branchID,
            success: function (result) {
                if (result === "False") {
                    $("#TBCB-GlobalError").text("You Not Have Access To This Factory..!");
                    $("#TBCB-BranchID").val("");

                } else {
                    $("#TBCB-BranchName").val(result);
                }
            }
        });
    }
});

//5.2 Change of Dropdow module for only validation
$("#TBCB-Module").change(function () {

    var module = $(this).val();

    $("#TBCB-GlobalError").text("");

    if (module.length === 0) {
        $(this).css("border-color", "red");
    } else {
        $(this).css("border-color", "");
    }
});

//5.3 Keyup of batchID for only validation
$("#TBCB-BatchID").keyup(function () {

    var batchID = $(this).val();

    if (batchID.length === 0) {
        $(this).css("border-color", "red");
    } else {
        $(this).css("border-color", "");
    }
});

//5.4 Keyup of batchID Description for only validation
$("#TBCB-BatchDescription").keyup(function () {

    var batchID = $(this).val();

    if (batchID.length === 0) {
        $(this).css("border-color", "red");
    } else {
        $(this).css("border-color", "");
    }
});

//5.5 Btn Save Branch Batch
$("#TBCB-SaveBatch").click(function () {

    var branchID = $("#TBCB-BranchID").val();

    var module = $("#TBCB-Module").val();

    var batchID = $("#TBCB-BatchID").val();

    var batchDescription = $("#TBCB-BatchDescription").val();

    var Test = true;

    //------------Validation check empty Fields------------------
    if (branchID.length === 0) {
        $("#TBCB-BranchID").css("border-color", "red");
        Test = false;
    } else {
        $("#TBCB-BranchID").css("border-color", "");
    }

    if (module.length === 0) {
        $("#TBCB-Module").css("border-color", "red");
        Test = false;
    } else {
        $("#TBCB-Module").css("border-color", "");
    }

    if (batchID.length === 0) {
        $("#TBCB-BatchID").css("border-color", "red");
        Test = false;
    } else {
        $("#TBCB-BatchID").css("border-color", "");
    }

    if (batchDescription.length === 0) {
        $("#TBCB-BatchDescription").css("border-color", "red");
        Test = false;
    } else {
        $("#TBCB-BatchDescription").css("border-color", "");
    }
    //----------------------End Validation-------------------------

    if (Test === true) {
        $.ajax({
            type: "POST",
            url: "/B_CreateBatch/SaveBranchBatch?branchID=" + branchID + "&module=" + module + "&batchID=" + batchID + "&batchDescription=" + batchDescription,
            success: function (result) {
                if (result === "False") {
                    $("#TBCB-GlobalError").text("Batch ID Not Valid In This Module..!");
                    $("#TBCB-BatchID").css("border-color", "red");
                } else {
                    $("#TBCB-GlobalError").text("");
                    $("#TBCB-BatchID").css("border-color", "");
                    location.reload();
                }
            }
        });
    }
});

//5.6 Btn Reset To reload Page
$("#TBCB-ResetPage").click(function () {
    location.reload();
});

//----------------------------------------END OF TBCB------------------------------------------------



/*-------------------------------------------------
:: 6.0 F_Create Batch (TFCB->Factory Create Batch)
---------------------------------------------------*/

/*
 * 6.1 Change of Dropdown Factory ID to Get Factory Name and check Access
 * 6.2 Change of Dropdow module for only validation
 * 6.3 Keyup of batchID for only validation
 * 6.4 Keyup of batchID Description for only validation
 * 6.5 Btn Save Factory Batch
 * 6.6 Btn Reset To reload Page
 */

//6.1 Change of Dropdown Factory ID to Get Factory Name and check Access
$("#TFCB-FactoryID").change(function () {

    var factoryID = $(this).val();

    $("#TFCB-FactoryName").val("");
    $("#TFCB-GlobalError").text("");

    if (factoryID.length === 0) {
        $(this).css("border-color", "red");
    } else {
        $(this).css("border-color", "");

        $.ajax({
            type: "GET",
            url: "/F_CreateBatch/GetFactoryName?factoryID=" + factoryID,
            success: function (result) {
                if (result === "False") {
                    $("#TFCB-GlobalError").text("You Not Have Access To This Branch..!");
                    $("#TFCB-FactoryID").val("");
                } else {
                    $("#TFCB-FactoryName").val(result);
                }
            }
        })
    }
});

//6.2 Change of Dropdow module for only validation
$("#TFCB-Module").change(function () {

    var module = $(this).val();

    $("#TFCB-GlobalError").text("");

    if (module.length === 0) {
        $(this).css("border-color", "red");
    } else {
        $(this).css("border-color", "");
    }
});

//6.3 Keyup of batchID for only validation
$("#TFCB-BatchID").keyup(function () {

    var batchID = $(this).val();

    if (batchID.length === 0) {
        $(this).css("border-color", "red");
    } else {
        $(this).css("border-color", "");
    }
});

//6.4 Keyup of batchID Description for only validation
$("#TFCB-BatchDescription").keyup(function () {

    var batchID = $(this).val();

    if (batchID.length === 0) {
        $(this).css("border-color", "red");
    } else {
        $(this).css("border-color", "");
    }
});

//6.5 Btn Save Factory Batch
$("#TFCB-SaveBatch").click(function () {

    var factoryID = $("#TFCB-FactoryID").val();

    var module = $("#TFCB-Module").val();

    var batchID = $("#TFCB-BatchID").val();

    var batchDescription = $("#TFCB-BatchDescription").val();

    var Test = true;

    //------------Validation check empty Fields------------------
    if (factoryID.length === 0) {
        $("#TFCB-FactoryID").css("border-color", "red");
        Test = false;
    } else {
        $("#TFCB-FactoryID").css("border-color", "");
    }

    if (module.length === 0) {
        $("#TFCB-Module").css("border-color", "red");
        Test = false;
    } else {
        $("#TFCB-Module").css("border-color", "");
    }

    if (batchID.length === 0) {
        $("#TFCB-BatchID").css("border-color", "red");
        Test = false;
    } else {
        $("#TFCB-BatchID").css("border-color", "");
    }

    if (batchDescription.length === 0) {
        $("#TFCB-BatchDescription").css("border-color", "red");
        Test = false;
    } else {
        $("#TFCB-BatchDescription").css("border-color", "");
    }
    //----------------------End Validation-------------------------

    if (Test === true) {
        $.ajax({
            type: "POST",
            url: "/F_CreateBatch/SaveFactoryBatch?factoryID=" + factoryID + "&module=" + module + "&batchID=" + batchID + "&batchDescription=" + batchDescription,
            success: function (result) {
                if (result === "False") {
                    $("#TFCB-GlobalError").text("Batch ID Not Valid In This Module..!");
                    $("#TFCB-BatchID").css("border-color", "red");
                } else {
                    $("#TFCB-GlobalError").text("");
                    $("#TFCB-BatchID").css("border-color", "");
                    location.reload();
                }
            }
        });
    }
});

//6.6 Btn Reset To reload Page
$("#TFCB-ResetPage").click(function () {
    location.reload();
});

//----------------------------------------END OF TFCB------------------------------------------------



/*-------------------------------------------
:: 7.0 User Edit T Rate (SUETR->U Edit Rate)
---------------------------------------------*/

/*
 * 7.1 Click of RB To handle C B F DropDown and all Page
 * 7.2 Change Of Company Branch Factory Drop Down To Validate it and Get Name And check Access
 * 7.3 Change of DropDown Module and fill drop down of Transaction Form Name
 * 7.4 Function Clear Items In C B F Change And RB Change
 * 7.5 Change of DropDown Transaction forms and get data of matching form name and C B F and module name
 * 7.6 Btn Save of Let User Edit Or Not OF Choosen Form
 * 7.7 Change of checkbox te set value if check by true and uncheck by false and change of RB to remove error
 * 7.8 Btn Update of Let User Edit Or Not OF Choosen Form
 * 7.9 Btn Reset to Refresh Page
 */

//7.1 Click of RB To handle C B F DropDown and all Page
$("#SUETR-RBCompany").click(function () {
    CompanyClick();
});
function CompanyClick() {
    if ($("#SUETR-RBCompany").is(":checked")) {
        CompanyRadioChecked()
    } else {
        $("#SUETR-CompanyID").prop("disabled", true);
    }
}
function CompanyRadioChecked() {
    $("#SUETR-CompanyID").prop("disabled", false);
    $("#SUETR-BranchID").prop("disabled", true);
    $("#SUETR-FactoryID").prop("disabled", true);
    $("#SUETR-GlobalError").text("");

    $("#SUETR-BranchName").val("");
    $("#SUETR-BranchID").css("border-color", "");
    $("#SUETR-BranchID").val("");

    $("#SUETR-FactoryName").val("");
    $("#SUETR-FactoryID").css("border-color", "");
    $("#SUETR-FactoryID").val("");

    SUETR_ClearModuleItems();
    SUETR_ClearTFNameItems();
}
$("#SUETR-RBBranch").click(function () {
    if ($(this).is(":checked")) {
        $("#SUETR-BranchID").prop("disabled", false);
        $("#SUETR-CompanyID").prop("disabled", true);
        $("#SUETR-FactoryID").prop("disabled", true);
        $("#SUETR-GlobalError").text("");

        $("#SUETR-CompanyName").val("");
        $("#SUETR-CompanyID").css("border-color", "");
        $("#SUETR-CompanyID").val("");

        $("#SUETR-FactoryName").val("");
        $("#SUETR-FactoryID").css("border-color", "");
        $("#SUETR-FactoryID").val("");

        SUETR_ClearModuleItems();
        SUETR_ClearTFNameItems();
    } else {
        $("#SUETR-BranchID").prop("disabled", true);
    }
});
$("#SUETR-RBFactory").click(function () {
    if ($(this).is(":checked")) {
        $("#SUETR-FactoryID").prop("disabled", false);
        $("#SUETR-CompanyID").prop("disabled", true);
        $("#SUETR-BranchID").prop("disabled", true);
        $("#SUETR-GlobalError").text("");

        $("#SUETR-CompanyName").val("");
        $("#SUETR-CompanyID").css("border-color", "");
        $("#SUETR-CompanyID").val("");

        $("#SUETR-BranchName").val("");
        $("#SUETR-BranchID").css("border-color", "");
        $("#SUETR-BranchID").val("");

        SUETR_ClearModuleItems();
        SUETR_ClearTFNameItems();
    } else {
        $("#SUETR-FactoryID").prop("disabled", true);
    }
});

//7.2 Change Of Company Branch Factory Drop Down To Validate it and Get Name And check Access
$("#SUETR-CompanyID").change(function () {

    var companyID = $(this).val();
    $("#SUETR-CompanyName").val("");

    SUETR_ClearModuleItems();
    SUETR_ClearTFNameItems();

    if (companyID.length === 0) {
        $(this).css("border-color", "red");
    } else {
        $(this).css("border-color", "");

        $.ajax({
            type: "GET",
            url: "/UserEditTRate/GetCompanyName?companyID=" + companyID,
            success: function (result) {
                $("#SUETR-CompanyName").val(result);
                $("#SUETR-Module").prop("disabled", false);
            }
        });
    }
});
$("#SUETR-BranchID").change(function () {

    var branchID = $(this).val();
    $("#SUETR-BranchName").val("");
    $("#SUETR-GlobalError").text("");

    SUETR_ClearModuleItems();
    SUETR_ClearTFNameItems();

    if (branchID.length === 0) {
        $(this).css("border-color", "red");
    } else {
        $(this).css("border-color", "");

        $.ajax({
            type: "GET",
            url: "/UserEditTRate/GetBranchName?branchID=" + branchID,
            success: function (result) {
                if (result === "False") {
                    $("#SUETR-GlobalError").text("You Not Have Access To This Branch..!");
                    $("#SUETR-BranchID").val("");
                } else {
                    $("#SUETR-BranchName").val(result);
                    $("#SUETR-Module").prop("disabled", false);
                }
            }
        });
    }
});
$("#SUETR-FactoryID").change(function () {

    var factoryID = $(this).val();
    $("#SUETR-FactoryName").val("");
    $("#SUETR-GlobalError").text("");

    SUETR_ClearModuleItems();
    SUETR_ClearTFNameItems();

    if (factoryID.length === 0) {
        $(this).css("border-color", "red");
    } else {
        $(this).css("border-color", "");

        $.ajax({
            type: "GET",
            url: "/UserEditTRate/GetFactoryName?factoryID=" + factoryID,
            success: function (result) {
                if (result === "False") {
                    $("#SUETR-GlobalError").text("You Not Have Access To This Factory..!");
                    $("#SUETR-FactoryID").val("");
                } else {
                    $("#SUETR-FactoryName").val(result);
                    $("#SUETR-Module").prop("disabled", false);
                }
            }
        });
    }
});

//7.3 Change of DropDown Module and fill drop down of Transaction Form Name
$("#SUETR-Module").change(function () {

    var module = $(this).val();

    SUETR_ClearTFNameItems();

    //Forms of Financial module
    var financialForms = {
        form1: "Company Journal Entry Transaction",
        form2: "Company Cash Reciept",
        form3: "Company Cash Withdraw",
        form4: "Company Bank Checkout",
        form5: "Company Bank CheckReceived",
        form6: "Checkbook Transfer"
    };

    if (module.length === 0) {
        $(this).css("border-color", "red");
    } else {
        $(this).css("border-color", "");

        //Open dropdown forms and check the value of module to pass Forms that related with it
        $("#SUETR-TFormName").prop("disabled", false);
        $("#SUETR-TFormName").empty();
        $("#SUETR-TFormName").append($('<option/>', {
            value: "",
            text: "-Choose-"
        })
        );

        if (module === "Financial") {
            $.each(financialForms, function (val, text) {
                $("#SUETR-TFormName").append("<option value='" + text + "'>" + text + "</option>")
            });
        }
    }
});

//7.4 Function Clear Items In C B F Change And RB Change
function SUETR_ClearModuleItems() {
    //-------Clear module dropdown------------
    $("#SUETR-Module").prop("disabled", true);
    $("#SUETR-Module").css("border-color", "");
    $("#SUETR-Module").val("");
    //----------------------------------------

    //------Reset Buttons and CB--------------------
    $("#SUETR-SavePSBtn").prop("disabled", false);
    $("#SUETR-UpdatePSBtn").prop("disabled", false);
    $("#SUETR-CBAUTETR").prop("checked", false);
    //-----------------------------------------------
}
function SUETR_ClearTFNameItems() {
    //-------Clear Transaction Forms dropdown------------
    $("#SUETR-TFormName").prop("disabled", true);
    $("#SUETR-TFormName").css("border-color", "");
    $("#SUETR-TFormName").val("");
    //----------------------------------------

    //------Reset Buttons and CB--------------------
    $("#SUETR-SavePSBtn").prop("disabled", false);
    $("#SUETR-UpdatePSBtn").prop("disabled", false);
    $("#SUETR-CBAUTETR").prop("checked", false);
    //-----------------------------------------------
}

//7.5 Change of DropDown Transaction forms and get data of matching form name and C B F and module name
$("#SUETR-TFormName").change(function () {

    var tFormName = $(this).val();

    var module = $("#SUETR-Module").val();

    if (tFormName.length === 0) {
        $(this).css("border-color", "red");
    } else {
        $(this).css("border-color", "");

        //-----Check who user choosen C or B or F to detect retrieve data from specific table-------
        if ($("#SUETR-RBCompany").is(":checked")) {

            var companyID = $("#SUETR-CompanyID").val();

            //ajax of retrieve value of checkbox if exist and set in it if user choose company
            $.ajax({
                type: "GET",
                url: "/UserEditTRate/GetCompanyUE?companyID=" + companyID + "&module=" + module + "&tFormName=" + tFormName,
                success: function (result) {
                    //if not exist matching data close update button to Save id he need
                    if (result === "False") {
                        $("#SUETR-UpdatePSBtn").prop("disabled", true);
                    } else {

                        //if exist data close all main data and retrive data to checkboxes and close save btn
                        $("#SUETR-CompanyID").prop("disabled", true);
                        $("#SUETR-Module").prop("disabled", true);
                        $("#SUETR-TFormName").prop("disabled", true);
                        $("#SUETR-SavePSBtn").prop("disabled", true);

                        if (result === true) {
                            $("#SUETR-CBAUTETR").prop("checked", true);
                            $("#SUETR-CBAUTETR").attr('value', true);
                        } else {
                            $("#SUETR-CBAUTETR").prop("checked", false);
                            $("#SUETR-CBAUTETR").attr('value', false);
                        }
                    }
                }
            });
        }
        else if ($("#SUETR-RBBranch").is(":checked")) {

            var branchID = $("#SUETR-BranchID").val();

            //ajax of retrieve value of checkbox if exist and set in it if user choose branch
            $.ajax({
                type: "GET",
                url: "/UserEditTRate/GetBranchUE?branchID=" + branchID + "&module=" + module + "&tFormName=" + tFormName,
                success: function (result) {
                    //if not exist matching data close update button to Save id he need
                    if (result === "False") {
                        $("#SUETR-UpdatePSBtn").prop("disabled", true);
                    } else {

                        //if exist data close all main data and retrive data to checkboxes and close save btn
                        $("#SUETR-BranchID").prop("disabled", true);
                        $("#SUETR-Module").prop("disabled", true);
                        $("#SUETR-TFormName").prop("disabled", true);
                        $("#SUETR-SavePSBtn").prop("disabled", true);

                        if (result === true) {
                            $("#SUETR-CBAUTETR").prop("checked", true);
                            $("#SUETR-CBAUTETR").attr('value', true);
                        } else {
                            $("#SUETR-CBAUTETR").prop("checked", false);
                            $("#SUETR-CBAUTETR").attr('value', false);
                        }
                    }
                }
            });
        }
        else if ($("#SUETR-RBFactory").is(":checked")) {

            var factoryID = $("#SUETR-FactoryID").val();

            //ajax of retrieve value of checkbox if exist and set in it if user choose factory
            $.ajax({
                type: "GET",
                url: "/UserEditTRate/GetFactoryUE?factoryID=" + factoryID + "&module=" + module + "&tFormName=" + tFormName,
                success: function (result) {
                    //if not exist matching data close update button to Save id he need
                    if (result === "False") {
                        $("#SUETR-UpdatePSBtn").prop("disabled", true);
                    } else {

                        //if exist data close all main data and retrive data to checkboxes and close save btn
                        $("#SUETR-FactoryID").prop("disabled", true);
                        $("#SUETR-Module").prop("disabled", true);
                        $("#SUETR-TFormName").prop("disabled", true);
                        $("#SUETR-SavePSBtn").prop("disabled", true);

                        if (result === true) {
                            $("#SUETR-CBAUTETR").prop("checked", true);
                            $("#SUETR-CBAUTETR").attr('value', true);
                        } else {
                            $("#SUETR-CBAUTETR").prop("checked", false);
                            $("#SUETR-CBAUTETR").attr('value', false);
                        }
                    }
                }
            });
        }
        //---------------------------------------------------------------------------------------------
    }
});

//7.6 Btn Save of Let User Edit Or Not OF Choosen Form
$("#SUETR-SavePSBtn").click(function () {

    var module = $("#SUETR-Module").val();

    var tFormName = $("#SUETR-TFormName").val();

    var allowUserE = $("#SUETR-CBAUTETR").val();

    var Test = true;

    //-------Validation of module and Transaction FN if enabled---------
    if ($("#SUETR-Module").is(":enabled")) {

        if (module.length === 0) {
            $("#SUETR-Module").css("border-color", "red");
            Test = false;
        } else {
            $("#SUETR-Module").css("border-color", "");
        }
    }

    if ($("#SUETR-TFormName").is(":enabled")) {

        if (tFormName.length === 0) {
            $("#SUETR-TFormName").css("border-color", "red");
            Test = false;
        } else {
            $("#SUETR-TFormName").css("border-color", "");
        }
    }
    //--------------------------------------------------------------------


    //---Check what user chosen C or B or F To detect will insert to specific tables---
    //Choosen Company
    if ($("#SUETR-RBCompany").is(":checked")) {

        var companyID = $("#SUETR-CompanyID").val();

        if (companyID.length === 0) {
            $("#SUETR-CompanyID").css("border-color", "red");
            Test = false;
        } else {
            $("#SUETR-CompanyID").css("border-color", "");
        }
        //Check success validation
        if (Test === true) {
            //ajax to final save company
            $.ajax({
                type: "POST",
                url: "/UserEditTRate/SaveCompanyUE?companyID=" + companyID + "&module=" + module + "&tFormName=" + tFormName + "&allowUserE=" + allowUserE,
                success: function (result) {
                    if (result === "False") { }
                    else {
                        location.reload();
                    }
                }
            });
        }
    }
    //Choosen Branch
    else if ($("#SUETR-RBBranch").is(":checked")) {

        var branchID = $("#SUETR-BranchID").val();

        if (branchID.length === 0) {
            $("#SUETR-BranchID").css("border-color", "red");
            Test = false;
        } else {
            $("#SUETR-BranchID").css("border-color", "");
        }
        //Check success validation
        if (Test === true) {
            //ajax to final save barnch
            $.ajax({
                type: "POST",
                url: "/UserEditTRate/SaveBranchUE?branchID=" + branchID + "&module=" + module + "&tFormName=" + tFormName + "&allowUserE=" + allowUserE,
                success: function (result) {
                    if (result === "False") { }
                    else {
                        location.reload();
                    }
                }
            });
        }
    }
    //Choosen factory
    else if ($("#SUETR-RBFactory").is(":checked")) {

        var factoryID = $("#SUETR-FactoryID").val();

        if (factoryID.length === 0) {
            $("#SUETR-FactoryID").css("border-color", "red");
            Test = false;
        } else {
            $("#SUETR-FactoryID").css("border-color", "");
        }
        //Check success validation
        if (Test === true) {
            //ajax to final save factory
            $.ajax({
                type: "POST",
                url: "/UserEditTRate/SaveFactoryUE?factoryID=" + factoryID + "&module=" + module + "&tFormName=" + tFormName + "&allowUserE=" + allowUserE,
                success: function (result) {
                    if (result === "False") { }
                    else {
                        location.reload();
                    }
                }
            });
        }
    }
    else {
        $(".SUETR-BuidingTypelbl").css("border", "1px solid red");
    }
});

//7.7 Change of checkbox te set value if check by true and uncheck by false and change of RB to remove error
$("#SUETR-CBAUTETR").on('change', function () {
    if ($(this).is(':checked')) {
        $(this).attr('value', true);
    } else {
        $(this).attr('value', false);
    }
});
$("input[name=SUETR-BuidingType]").change(function () {
    $(".SUETR-BuidingTypelbl").css("border", "");
});

//7.8 Btn Update of Let User Edit Or Not OF Choosen Form
$("#SUETR-UpdatePSBtn").click(function () {

    var module = $("#SUETR-Module").val();

    var tFormName = $("#SUETR-TFormName").val();

    var allowUserE = $("#SUETR-CBAUTETR").val();

    if ($("#SUETR-RBCompany").is(":checked")) {

        var companyID = $("#SUETR-CompanyID").val();

        $.ajax({
            type: "POST",
            url: "/UserEditTRate/UpdateCompanyUE?companyID=" + companyID + "&module=" + module + "&tFormName=" + tFormName + "&allowUserE=" + allowUserE,
            success: function (result) {
                if (result === "False") { }
                else {
                    location.reload();
                }
            }
        });
    }
    else if ($("#SUETR-RBBranch").is(":checked")) {

        var branchID = $("#SUETR-BranchID").val();

        $.ajax({
            type: "POST",
            url: "/UserEditTRate/UpdateBranchUE?branchID=" + branchID + "&module=" + module + "&tFormName=" + tFormName + "&allowUserE=" + allowUserE,
            success: function (result) {
                if (result === "False") { }
                else {
                    location.reload();
                }
            }
        });
    }
    else if ($("#SUETR-RBFactory").is(":checked")) {

        var factoryID = $("#SUETR-FactoryID").val();

        $.ajax({
            type: "POST",
            url: "/UserEditTRate/UpdateFactoryUE?factoryID=" + factoryID + "&module=" + module + "&tFormName=" + tFormName + "&allowUserE=" + allowUserE,
            success: function (result) {
                if (result === "False") { }
                else {
                    location.reload();
                }
            }
        });
    }
    //---------------------------------------------------------------------------------------------

});

//7.9 Btn Reset to Refresh Page
$("#SUETR-ResetBtn").click(function () {
    location.reload();
});

//----------------------------------------END OF SUETR------------------------------------------------

//Link Analytic To Account ID
//Link Currency Definition To Account ID 
$(document).ready(function () {
    //Company Link Analytic To Account ID
    $("#SCLAA-analyticID").change(function () {

        $("#SCLAA-analyticName").val("");
        $("#SCLAA-dvTableOfLinked").html("");
        var analyticID = $(this).val();
        if (analyticID.length > 0) {
            $.ajax({
                type: "GET",
                url: "/C_LinkAnalyticToAccount/GetAnalyticName?analyticID=" + analyticID,
                success: function (result) {
                    $("#SCLAA-analyticName").val(result);
                    //CreateCards(analyticID);
                    SCLAA_CreateTable(analyticID);
                }
            });
        }
    });

    $("#SCLAA-dvTableOfLinked").on("change", "input[name='SCLAA-chklistitem']", function () {

        var analyticID = $("#SCLAA-analyticID").val();
        var accountID = $(this).val();

        if (analyticID.length > 0) {

            if (this.checked) {

                $.ajax({
                    type: "POST",
                    url: "/C_LinkAnalyticToAccount/AddAnalyticToAcc?c_AID=" + accountID + "&analyticID=" + analyticID,
                    success: function () { }
                });

            } else {

                $.ajax({
                    type: "POST",
                    url: "/C_LinkAnalyticToAccount/RemoveAnalyticFromAcc?c_AID=" + accountID,
                    success: function () { }
                });

            }
        }
    });


    //Branch Link Analytic To Account ID
    $("#SBLAA-BranchID").change(function () {
        $('#SBLAA-dvTableOfLinked').html("");
        $("#SBLAA-analyticID").empty();
        $("#SBLAA-BranchName").val("");
        $("#SBLAA-analyticName").val("");
        var branchID = $(this).val();
        if (branchID.length > 0) {
            $.ajax({
                type: "GET",
                url: "/B_LinkAnalyticToAccount/GetBranchName?branchID=" + branchID,
                success: function (result) {
                    $("#SBLAA-BranchName").val(result);

                    $.ajax({
                        type: "GET",
                        url: "/B_LinkAnalyticToAccount/GetAnalyticAccounts?branchID=" + branchID,
                        success: function (result) {

                            if (result.length == 0) {

                                $("#SBLAA-analyticID").append($('<option/>', {
                                    value: "",
                                    text: "No Analytic Created in this Branch!"

                                })
                                );

                            } else {

                                $("#SBLAA-analyticID").append($('<option/>', {
                                    value: "",
                                    text: "-Choose-"

                                })
                                );

                                $.each(result, function (index, row) {

                                    $("#SBLAA-analyticID").append("<option value='" + row.AnalyticID + "'>" + row.AnalyticID + "</option>");

                                });
                            }
                        }
                    });

                }
            });
        }

    });

    $("#SBLAA-analyticID").change(function () {
        $('#SBLAA-dvTableOfLinked').html("");
        $("#SBLAA-analyticName").val("");
        var analyticID = $(this).val();
        if (analyticID.length > 0) {
            $.ajax({
                type: "GET",
                url: "/B_LinkAnalyticToAccount/GetAnalyticName?analyticID=" + analyticID,
                success: function (result) {
                    $("#SBLAA-analyticName").val(result);
                    SBLAA_CreateTable(analyticID);
                }
            });

        }
    });

    $("#SBLAA-dvTableOfLinked").on("change", "input[name='SBLAA-chklistitem']", function () {

        var analyticID = $("#SBLAA-analyticID").val();
        var accountID = $(this).val();

        if (analyticID.length > 0) {

            if (this.checked) {

                $.ajax({
                    type: "POST",
                    url: "/B_LinkAnalyticToAccount/AddAnalyticToAcc?b_AID=" + accountID + "&analyticID=" + analyticID,
                    success: function () { }
                });

            } else {

                $.ajax({
                    type: "POST",
                    url: "/B_LinkAnalyticToAccount/RemoveAnalyticFromAcc?b_AID=" + accountID,
                    success: function () { }
                });

            }
        }
    });


    //Factory Link Analytic To Account ID
    $("#SFLAA-FactoryID").change(function () {
        $("#SFLAA-dvTableOfLinked").html("");
        $("#SFLAA-analyticID").empty();
        $("#SFLAA-FactoryName").val("");
        $("#SFLAA-analyticName").val("");
        var factoryID = $(this).val();
        if (factoryID.length > 0) {
            $.ajax({
                type: "GET",
                url: "/F_LinkAnalyticToAccount/GetFactoryName?factoryID=" + factoryID,
                success: function (result) {
                    $("#SFLAA-FactoryName").val(result);

                    $.ajax({
                        type: "GET",
                        url: "/F_LinkAnalyticToAccount/GetAnalyticAccounts?factoryID=" + factoryID,
                        success: function (result) {

                            if (result.length == 0) {

                                $("#SFLAA-analyticID").append($('<option/>', {
                                    value: "",
                                    text: "No Analytic Created in this Branch!"

                                })
                                );

                            } else {

                                $("#SFLAA-analyticID").append($('<option/>', {
                                    value: "",
                                    text: "-Choose-"

                                })
                                );

                                $.each(result, function (index, row) {

                                    $("#SFLAA-analyticID").append("<option value='" + row.AnalyticID + "'>" + row.AnalyticID + "</option>");

                                });
                            }
                        }
                    });

                }
            });
        }

    });

    $("#SFLAA-analyticID").change(function () {
        $("#SFLAA-dvTableOfLinked").html("");
        $("#SFLAA-analyticName").val("");
        var analyticID = $(this).val();
        if (analyticID.length > 0) {
            $.ajax({
                type: "GET",
                url: "/F_LinkAnalyticToAccount/GetAnalyticName?analyticID=" + analyticID,
                success: function (result) {
                    $("#SFLAA-analyticName").val(result);
                    SFLAA_CreateTable(analyticID);
                }
            });
        }

    });

    $("#SFLAA-dvTableOfLinked").on("change", "input[name='SFLAA-chklistitem']", function () {

        var analyticID = $("#SFLAA-analyticID").val();
        var accountID = $(this).val();

        if (analyticID.length > 0) {

            if (this.checked) {

                $.ajax({
                    type: "POST",
                    url: "/F_LinkAnalyticToAccount/AddAnalyticToAcc?f_AID=" + accountID + "&analyticID=" + analyticID,
                    success: function () { }
                });

            } else {

                $.ajax({
                    type: "POST",
                    url: "/F_LinkAnalyticToAccount/RemoveAnalyticFromAcc?f_AID=" + accountID,
                    success: function () { }
                });

            }
        }
    });

    //---------------------------------------------------------------------------------------------------------

});



//Company Link Analytic To Account ID
function SCLAA_CreateTable(analyticID) {
    $.ajax({
        type: "GET",
        url: "/C_LinkAnalyticToAccount/GetAccounts?analyticID=" + analyticID,
        success: function (result) {
            if (result.length === 0) {
                $('#SCLAA-dvTableOfLinked').append('<div class="text-center text-danger">No Accounts Available To This Analytic ID</div>');
            } else {
                var table = $('<table class="table table-bordered table-striped display nowrap"></table>');
                var thead = "<tr>" +
                    "<th> Account ID </th>" +
                    "<th> Account Name </th>" +
                    "<th> Linked </th>" +
                    "</tr>";
                table.append($('<thead></thead>').append(thead));

                var counter = 0;
                $(result).each(function () {

                    var tbody = "<tr>" +
                        "<td>" + this.AccountID + "</td>" +
                        "<td>" + this.AccountName + "</td>" +
                        "<td>" + "<input type='checkbox' name='SCLAA-chklistitem', value=" + this.AID + " />" + "</td>" +
                        "</tr>"

                    table.append($('<tbody></tbody>').append(tbody));

                    $('#SCLAA-dvTableOfLinked').append(table);

                    if (this.AnalyticID === analyticID) {
                        $("input[name=SCLAA-chklistitem][value=" + this.AID + "]").prop('checked', true);
                    }
                });
            }
        }
    });
}


//Branch Link Analytic To Account ID
function SBLAA_CreateTable(analyticID) {
    $.ajax({
        type: "GET",
        url: "/B_LinkAnalyticToAccount/GetAccounts?analyticID=" + analyticID,
        success: function (result) {
            if (result.length === 0) {
                $('#SBLAA-dvTableOfLinked').append('<div class="text-center text-danger">No Accounts Available To This Analytic ID</div>');
            } else {

                var table = $('<table class="table table-bordered table-striped display nowrap"></table>');

                var thead = "<tr>" +
                    "<th> Account ID </th>" +
                    "<th> Account Name </th>" +
                    "<th> Linked </th>" +
                    "</tr>";

                table.append($('<thead></thead>').append(thead));

                var counter = 0;
                $(result).each(function () {

                    var tbody = "<tr>" +
                        "<td>" + this.AccountID + "</td>" +
                        "<td>" + this.AccountName + "</td>" +
                        "<td>" + "<input type='checkbox' name='SBLAA-chklistitem', value=" + this.AID + " />" + "</td>" +
                        "</tr>"

                    table.append($('<tbody></tbody>').append(tbody));

                    $('#SBLAA-dvTableOfLinked').append(table);

                    if (this.AnalyticID === analyticID) {
                        $("input[name=SBLAA-chklistitem][value=" + this.AID + "]").prop('checked', true);
                    }
                });
            }
        }
    });
}


//Factory Link Analytic To Account ID
function SFLAA_CreateTable(analyticID) {
    $.ajax({
        type: "GET",
        url: "/F_LinkAnalyticToAccount/GetAccounts?analyticID=" + analyticID,
        success: function (result) {
            if (result.length === 0) {
                $('#SFLAA-dvTableOfLinked').append('<div class="text-center text-danger">No Accounts Available To This Analytic ID</div>');
            } else {

                var table = $('<table class="table table-bordered table-striped display nowrap"></table>');

                var thead = "<tr>" +
                    "<th> Account ID </th>" +
                    "<th> Account Name </th>" +
                    "<th> Linked </th>" +
                    "</tr>";

                table.append($('<thead></thead>').append(thead));

                var counter = 0;
                $(result).each(function () {

                    var tbody = "<tr>" +
                        "<td>" + this.AccountID + "</td>" +
                        "<td>" + this.AccountName + "</td>" +
                        "<td>" + "<input type='checkbox' name='SFLAA-chklistitem', value=" + this.AID + " />" + "</td>" +
                        "</tr>"

                    table.append($('<tbody></tbody>').append(tbody));

                    $('#SFLAA-dvTableOfLinked').append(table);

                    if (this.AnalyticID === analyticID) {
                        $("input[name=SFLAA-chklistitem][value=" + this.AID + "]").prop('checked', true);
                    }
                });
            }
        }
    });
}

//------------------------------------------------




//function CreateCards(analyticID) {
//    $.ajax({
//        type: "GET",
//        url: "/C_LinkAnalyticToAccount/GetAccounts?analyticID=" + analyticID,
//        success: function (result) {
//            if (result.length === 0) {
//                $('#dvCheckBoxListControl').append('<div class="text-center text-danger">No Accounts Available To This Analytic ID</div>');
//            } else {

//                var div = $('<div class="row"></div>');
//                var counter = 0;
//                $(result).each(function () {

//                    var card = $('<div class="card bg-secondary border-primary mb-1 mr-1"></div>');

//                    card.append($('<div class="card-header p-1 text-center"></div>').append($('<div class="custom-control custom-checkbox"></div>').append($('<input>').attr({
//                        type: 'checkbox', name: 'chklistitem', value: this.C_AID, id: 'chklistitem' + counter, 'class': 'custom-control-input'
//                    })).append(
//                        $('<label class="p-1">').attr({
//                            for: 'chklistitem' + counter++,
//                            'class': 'custom-control-label text-white'
//                        }).text("Linked"))));

//                    card.append($('<div class="card-body p-1 text-white"></div>').append($('<div>Account ID : ' + this.AccountID + '</div>' + '<div>Account Name : ' + this.AccountName + '</div>')));

//                    div.append(card);

//                    $('#dvCheckBoxListControl').append(div);

//                    if (this.AnalyticID === analyticID) {
//                        $("input[name=chklistitem][value=" + this.C_AID + "]").prop('checked', true);
//                    }
//                });
//            }
//        }
//    });
//}