
/*----------------------------------------------------------------------------
 :: 8.0 C_General Entry Transaction (TCGE->Company General Entry Transaction)
------------------------------------------------------------------------------*/

/*
 * 8.1 Validation OF JEPer What and Batch Action Inside It
 * 
 * 8.2 Change Of BatchID and Check if Posting Date Create From Batch Will Set Creation Date of Batch of it
 * 
 * 8.3 FIN J E Date Set The Date of today And FOUT check Date In Periods and Check if Posting Date Create From Transaction will Copy it
 * 
 * 8.4 FOUT Posting Date Check Date In Periods And FIN Reload Currency
 * 
 * 8.5 Change of Currency ID And make all validations of all above data to ensure that data fill correctly
 * 
 * 8.6 Btn To show PopUp of create new Batch and empty All fields of it
 * 
 * 8.7 Btn Exist Inside popup of create new batch to confirm add it if all valid
 * 
 * 8.8 KeyUp of Refrence to remove border error
 * 
 * 8.9 Function to handle date formate to set it in text
 * 
 * 8.10 Reload Currency if make any change of JE Date or Posting Date
 */


//jQuery.noConflict();
//jQuery(document).ready(function ($)

$(document).ready(function () {

    const dateformat = /^\d{4}-\d{2}-\d{2}$/;

    var companyID = $("#TCGE-CompanyID").text();

    var fJEPer = $("#TCGE-FJEPer").text(); //To check J E Per Transaction Or Batch
    var batchAction = $("#TCGE-BatchAction").text(); //To Check Batch Will Append Or Create new
    var postDateType = $("#TCGE-PostDateType").text(); //To Get Posting Date From Transaction Date Or From Batch Creation Date

    if ($("#ValidPostringNumber").text() == "true") {

        if (fJEPer === "B1") {
            $("#TCGE-BatchID").prop("disabled", true);
            $("#TCGE-BtnPUNewBatch").prop("disabled", true);

            //$("#TCGE-Post").prop("disabled", false);
            //$("#TCGE-Save").prop("disabled", true);

            //If Journal Entry Per Batch Open DropDown BatchID and Create New Btn
        } else if (fJEPer === "B2") {

            //If Action Create New Close Dropdown of batch ID
            if (batchAction === "E2") {
                $("#TCGE-BatchID").prop("disabled", true);
            }
            $("#TCGE-BtnPUNewBatch").prop("disabled", false);

            //$("#TCGE-Post").prop("disabled", true);
            //$("#TCGE-Save").prop("disabled", false);

            //If No Posting Setup Create Appear Alert And Convert To Home Page
        } else if (fJEPer === "NoPS") {
            Talert("This Company in Financial Module Not have Posting Setup..!");
            window.location.href = "/Home/Financial_Home";
        }


        if ($("#BostingToORThrow").val() == "1") {

            $("#TCGE-Post").prop("disabled", true);
            $("#TCGE-Save").prop("disabled", false);

        } else if ($("#BostingToORThrow").val() == "2") {

            $("#TCGE-Post").prop("disabled", false);
            $("#TCGE-Save").prop("disabled", true);

        }
        //8.2 Change Of BatchID and Check if Posting Date Create From Batch Will Set Creation Date of Batch of it
        $("#TCGE-BatchID").change(function () {
            var batchID = $(this).val();
            if (batchID.length === 0) {
                $(this).css("border-color", "red");
            } else {
                $(this).css("border-color", "");
                if (postDateType === "D2") {
                    $("#TCGE-PostingDate").val("");
                    $("#TCGE-PostingDate").css("border-color", "");
                    $.ajax({
                        type: "GET",
                        url: "/C_GeneralEntryTransaction/GetBatchCreationDate?batchID=" + batchID,
                        success: function (result) {
                            var today = HandleDate(result);
                            CheckPostingDateInPeriods(today, function (checkPostingDate) {
                                if (checkPostingDate === true) {
                                    $("#TCGE-PostingDate").val(today);
                                }
                            });

                        }
                    });
                }
            }
        });


        //8.3 FIN J E Date Set The Date of today And FOUT check Date In Periods and Check if Posting Date Create From Transaction will Copy it
        $("#TCGE-JEDate").focusin(function () {
            //Reload Currency And All Details To Choose it Again with new Date
            TCGE_ClearRatesInChange();
            var today = HandleDate(new Date());
            $(this).val(today);
        }).focusout(function () {
            var JEDate = $(this).val();
            if (!JEDate.match(dateformat)) {
                $("#TCGE-JEDate").css("border-color", "red");
                $("#TCGE-GlobalError").text("Wrong Date Formate..!");
            } else {
                $("#TCGE-JEDate").css("border-color", "");
                if (fJEPer === "B1") {
                    $("#TCGE-PostingDate").val(JEDate);
                    $("#TCGE-AdjPostingDate").val(JEDate);
                } else if (postDateType === "D1") {
                    $("#TCGE-PostingDate").val(JEDate);
                    $("#TCGE-AdjPostingDate").val(JEDate);
                }
            }
        });


        //8.4 FOUT Posting Date Check Date In Periods And FIN Reload Currency
        $("#TCGE-PostingDate").focusout(function () {
            var postingDate = $(this).val();
            CheckPostingDateInPeriods(postingDate);
        }).focusin(function () {
            //Reload Currency And All Details To Choose it Again with new Date
            TCGE_ClearRatesInChange();
        });


        /*8.5 Change of Currency ID And make all validations of all above data to ensure that data fill correctly
        -Check of Batch ID, Check of JE Date ANd Posting Date with Periods,
        -If Main Currency Choosen No Action need else Get System rate if one or several and make TRate The same and
        Defference Rate By Zero
        */
        $("#TCGE-CurrencyID").change(function () {

            $("#TCGE-GlobalError").text("");
            $("#TCGE-TransactionRate").css("border-color", "");
            ClearSecondPartOfMain();
            $("#TCGE-SystemRate").val("");
            $("#TCGE-TransactionRate").val("");
            $("#TCGE-DiffrenceRate").val("");

            $("#TCGE-TransactionRate").prop("disabled", true);

            var jeDate = $("#TCGE-JEDate").val();
            var postingDate = $("#TCGE-PostingDate").val();
            var batchID = $("#TCGE-BatchID").val();

            var Test = true;

            var currencyID = $(this).val();

            if (currencyID.length === 0) {
                $(this).css("border-color", "red");
            } else {
                $(this).css("border-color", "");

                CheckPostingDateInPeriods(postingDate, function (checkPostingDate) {
                    if (!jeDate.match(dateformat)) {
                        $(this).val("");
                        $("#TCGE-JEDate").css("border-color", "red");
                        Test = false;
                    } else {
                        $("#TCGE-JEDate").css("border-color", "");
                    }

                    if (!postingDate.match(dateformat)) {
                        $(this).val("");
                        $("#TCGE-PostingDate").css("border-color", "red");
                        Test = false;

                    } else if (checkPostingDate !== true) {
                        $(this).val("");
                        Test = false;
                    } else {
                        $("#TCGE-PostingDate").css("border-color", "");
                    }

                    //Make sure that transaction by batch and batch is enabled
                    if (fJEPer === "B2") {
                        if (batchID.length === 0) {
                            $(this).val("");
                            if (batchAction === "E2") {
                                $("#TCGE-BtnPUNewBatch").css("border-color", "red");
                                Test = false;
                            } else if (batchAction === "E1") {
                                $("#TCGE-BatchID").css("border-color", "red");
                                Test = false;
                            }
                        } else {
                            $("#TCGE-BtnPUNewBatch").css("border-color", "");
                            $("#TCGE-BatchID").css("border-color", "");
                        }
                    }
                    if (Test === true) {
                        GetCurrencyRates(currencyID, postingDate);
                    }
                });


            }
        });


        //8.6 Btn To show PopUp of create new Batch and empty All fields of it
        $("#TCGE-BtnPUNewBatch").click(function () {
            $("#TCGE-ABatchID").val("");
            $("#TCGE-ABatchDescription").val("");
            $("#TCGE-ANBError").text("");
            $("#TCGE-ABatchID").css("border-color", "");
            $("#TCGE-ABatchDescription").css("border-color", "");
            $("#TCGE-PUAddNewBatch").modal("show");
        });


        //8.7 Btn Exist Inside popup of create new batch to confirm add it if all valid
        $("#TCGE-ConfirmAddNewBatch").click(function () {

            var batchID = $("#TCGE-ABatchID").val();
            var batchDescription = $("#TCGE-ABatchDescription").val();

            var Test = true;

            if (batchID.length === 0) {
                $("#TCGE-ABatchID").css("border-color", "red");
                Test = false;
            } else {
                $("#TCGE-ABatchID").css("border-color", "");
            }

            if (batchDescription.length === 0) {
                $("#TCGE-ABatchDescription").css("border-color", "red");
                Test = false;
            } else {
                $("#TCGE-ABatchDescription").css("border-color", "");
            }

            if (Test === true) {
                $.ajax({
                    type: "POST",
                    url: "/C_GeneralEntryTransaction/AddNewBatch?batchID=" + batchID + "&batchDescription=" + batchDescription,
                    success: function (result) {
                        if (result === "False") {
                            $("#TCGE-ANBError").text("Batch ID Not Valid..!");
                            $("#TCGE-ABatchID").css("border-color", "red");
                        } else {
                            $("#TCGE-PUAddNewBatch").modal("hide");
                            $("#TCGE-ANBError").text("");
                            $("#TCGE-ABatchID").css("border-color", "");
                            //If Batch Action Choosen Create New
                            if (batchAction === "E2") {
                                $("#TCGE-BatchID").empty();
                                $("#TCGE-BatchID").append("<option value='" + result.C_CBID + "'>" + batchID + "</option>");
                                //If Posting Date by Batch Set it From Creation Date (Today)
                                if (postDateType === "D2") {

                                    $("#TCGE-PostingDate").val("");
                                    var today = HandleDate(result.C_BatchCreationDate);

                                    CheckPostingDateInPeriods(today, function (checkPostingDate) {
                                        if (checkPostingDate === true) {
                                            $("#TCGE-PostingDate").val(today);
                                        }
                                    });


                                }
                                //If Batch Action Choosen Append
                            } else if (batchAction === "E1") {
                                $("#TCGE-BatchID").append("<option value='" + result.C_CBID + "'>" + batchID + "</option>");
                            }
                        }
                    }
                });
            }
        });


        //8.8 KeyUp of Refrence to remove border error
        $("#TCGE-Reference").keyup(function () {
            var reference = $(this).val();
            if (reference.length === 0) {
                $(this).css("border-color", "red");
            } else {
                $(this).css("border-color", "");
            }
        });


        //--------------------------------------------------Start Of Insert Data in DB--------------------------------------------------------------


        //8.11 Finally Save Function to save header data and transaction data, analytic and cost if exist(If Posting setup ber Batch)
        $("#TCGE-Save").click(function () {

            $("#TCGE-GlobalError").text("");

            var companyID = $("#TCGE-CompanyID").text();

            var inputType = 1; // Save
            var batchID = $("#TCGE-BatchID").val();
            var transactionDate = $("#TCGE-JEDate").val();
            var postingDate = $("#TCGE-PostingDate").val();
            var refrence = $("#TCGE-Reference").val();
            var currencyID = $("#TCGE-CurrencyID").val();
            var systemRate = $("#TCGE-SystemRate").val();
            var transactionRate = $("#TCGE-TransactionRate").val();
            var postingKey = "TCGE";
            if (window.location.href.indexOf("/C_GeneralEntryTransaction/AdjustmentGETransaction") != -1) {
                postingKey = "TCGA";
            }
            var transactionType = "General Entry";

            //Function To Insert Data to database
            InsertTransactionData(companyID, inputType, postingDate, transactionDate, refrence, currencyID, systemRate, transactionRate, postingKey, transactionType, batchID);
            location.reload();
        });

        //8.12 Finally Post Function to save header data and transaction data, analytic and cost if exist(If Posting setup ber Transaction)
        $("#TCGE-Post").click(function () {

            $("#TCGE-GlobalError").text("");
            var companyID = $("#TCGE-CompanyID").text();
            var inputType = 2; //Post
            var transactionDate = $("#TCGE-JEDate").val();
            var postingDate = $("#TCGE-PostingDate").val();
            var refrence = $("#TCGE-Reference").val();
            var currencyID = $("#TCGE-CurrencyID").val();
            var systemRate = $("#TCGE-SystemRate").val();
            var transactionRate = $("#TCGE-TransactionRate").val();
            var postingKey = "TCGE";
            if (window.location.href.indexOf("/C_GeneralEntryTransaction/AdjustmentGETransaction") != -1) {
                postingKey = "TCGA";
            }
            var transactionType = "General Entry";

            //Function To Insert Data to database
            InsertTransactionData(companyID, inputType, postingDate, transactionDate, refrence, currencyID, systemRate, transactionRate, postingKey, transactionType);
            // if (location.pathname === '/C_GeneralEntryTransaction/CompanyGETransaction' || location.pathname === '/C_GeneralEntryTransaction/CompanyVoidTransactions')
            {
                // some code here
                window.open(
                    '/C_ReportsPrint/Done?searchNumber=' + $('#importantForReport').text(),
                    '_blank'
                );
                location.reload();
            }
        });
    }
});

//8.9 Function to handle date formate to set it in text
function HandleDate(date) {
    var d = new Date(date),
        month = '' + (d.getMonth() + 1),
        day = '' + d.getDate(),
        year = d.getFullYear();

    if (month.length < 2) month = '0' + month;
    if (day.length < 2) day = '0' + day;

    return [year, month, day].join('-');
}


//8.10


