/*
 * 8.00 Apply Mask of Currency In Transaction Rate
 * 
 * 8.1 If Not Exist Year Alert And Convert him to Home Page Moved to the view
 * 
 * 8.2 If No Edit In Posting Dste Close textbox of Posting Date
 * 
 * 8.3 drop down Exist On Pop Up Several Rates To Confirm Choosen Rate And Fill his Data 
 * 
 * 8.4 FOUT Of TRate Check Validation and to generate defference between it and System rate and set result in Deff. Rate 
 *
 * 8.5 (F) Empty Old Data of Rates And Check if user Can Edit Of Transaction Rate or Not  & function Clear second Part
 * 
 * ------------------------------------------------------------------------------------
 * 
 * 8.5 Change of dropdown AccountID To validate it and get details of Choosen Account to validate others and check related with analytic or cost
 * 
 * 8.6 keyup of original amount to multiply OA * TR and put it in debit or credit by the type of account
 * 
 * 8.7 keyUp of debit and credit and describtion to handle enter button
 * 
 * 8.8 Button add row of table after check of all data and check account id change exist analytic or cost or not and check document mandatory
 * 
 * 8.9 button update data of record after click update icon from table and btn cancel update to refrech all fields
 * 
 * 8.10 button confirm delete exist in PU that appear in icon delete of table
 * 
 * 8.11 button to Switch Between Debit and credit
 * 
 * 8.12 (F) Expression function to generate identity id for table
 * 
 * 8.13 (F) function to sum debit and credit and get defference that call in add and update and delete
 *
 * 8.14 (F) function to add row to main table then return id of this row and clear data
 * 
 * 8.15 (F) function called in save and update to check typed data is true of false
 *
 * 8.16 (F) function to update row of main table by bath to it id and clear data
 * 
 * 8.17 (F) Functions that call from table button icons to pass id of row click
 *
 * -----------------------------------------------------------------------------------------------------------
 * 
 * 8.18 change of dropdown distribution in analytic popup to get name and validation and keydown to handle keyboard events
 * 
 * 8.19 keyup of percentage to get value of amount and set of it and to handle keyboard events
 * 
 * 8.20 Keyup of amount to get value of percentage and set of it and to handle keyboard events
 * 
 * 8.21 Keyup of description to validation it and handle keyboard events
 * 
 * 8.22 Btn add row of distribution in analytic popup
 * 
 * 8.23 Btn exist in distribution table in analytic popup to remove clicked row
 * 
 * 8.24 Btn Confirm Update Row exist in analytic popup to update distribution record
 * 
 * 8.25 Btn Cancel Update that refresh fields
 * 
 * 8.26 Btn Final Save that exist in analytic popup and check if exist cost center related with this account
 * 
 * 8.27 Btn Final Update that exist in analytic popup and check if exist cost center in Cost DB Tb, that mean this account related alo by CC
 * 
 * 8.28 (F) function that validate fileds before add and update distribution
 * 
 * 8.29 (F) function that add data to analytic distribution table that exist in analytic popup
 *
 * 8.30 (F) Function that SaveAndUpdate Analytic Distribution DB Tbl
 * 
 * 8.31 (F) Function That Open Analytic popUp without refresh and go to Database That call when popup opened at least one  and update only amount by D OR C
 * 
 * 8.32 (F) Function that calculate Assign and UnAssign that exist in Analytic PopUp
 * 
 * 8.33 (F) Functions that call from table button icons that exist in analytic popup and pass id of row click
 * 
 * -------------------------------------------------------------------------------------------------------------------------------------------
 * 
 * 8.34 Btn Exist In Cost Center PopUp to back to analytic popUp without refresh
 * 
 * 8.35 Change of cost account drop down that get name of cost account and in key down handle key events
 * 
 * 8.36 Keyup of percentage to get value of amount and handle key events Same as analytic
 * 
 * 8.37 KeyUp of amount to get value of percentage and handle key events Same as analytic
 * 
 * 8.38 KeyUp to validate it and handle key events Same as analytic
 * 
 * 8.39 Btn Add row that exist in cost center popup
 * 
 * 8.40 Btn Confirm Update exist in cost center popup
 * 
 * 8.41 Btn Cancel update record that exist in cost center popup
 * 
 * 8.42 Functions that call from table button icons that exist in Cost Center popup to delete
 * 
 * 8.44 Btn Final save that exist in Cost center popup that fill cost Center DB Table
 * 
 * 8.45 Btn Final Update that exist in cost center popup
 * 
 * 8.43 Functions that call from table button icons that exist in Cost Center popup to update
 * 
 * 8.46 Change of DropDown CostCenterID In MCC Mode to get Accounts Extends of data table
 * 
 * 8.47 Btn delete exist of mainCostCenter Table to delete row
 * 
 * 8.48 Btn Update exist of mainCostCenter Table to set data That need to update in main fields
 * 
 * 8.49 handle hide of modals to remove update mode if exist
 * 
 * 8.50 (F) Big Function that set data of costcenter popup in dropdown and tbl "Save Mode"
 *
 * 8.51 (F) Function That add data to cost center tbl that exist in Cost center popUp
 * 
 * 8.52 (F) Function That open Cost center popup without go to database when popup opened at least one and update only amount by D OR C
 * 
 * 8.53 (F) Function that validate fields of cost center popup before add and update row
 * 
 * 8.54 (F) Function that add data to cost center database table
 *
 * 8.55 (F) Big Function that set data of costcenter popup in dropdown and tbl "Update Mode"
 * 
 * 8.56 (F) Function That Calculate Assign and Unassign that exist in Cost center popup
 *
 * 8.57 (F) Function To clear Field and data In Main Cost Center Mode
 * 
 * 8.58 (F) Function That Calculate Assign and Unassign of Cost Center ID that exist in cost center popup
 */



var selector;
var CostCenterObj = {
    C_CostCenterID: "",
    C_CAID: "",
    C_AID: "",
    Describtion: "",
    C_Percentage: "",
    C_Amount: "",
    C_Debit: "",
    C_Credit: "",
    C_CostCenterGroupID: "",
    CostCenterPercentage: "",
    fromJson: function (json) {
        var data = JSON.parse(json);
        return new CostCenterObj(data);
    }
}

$(document).ready(function () {

    //console.log(document.referrer);

    var companyID = $("#TCGE-CompanyID").text();

    var checkYear = $(document).find("#TCGE-CheckYear").text(); //To check If Exist Year Open

    var editPD = $("#TCGE-EPD").text(); //To Check If User Can Edit In Posting Date Or Not

    var checkFormatSetting = $("#TCGE-FormatSetting").text();

    //8.00 Apply Mask of Currency In Transaction Rate
    //SetMaskCurr();

    if ($("#ValidPostringNumber").text() == "false") {
        Talert("No Postring Setup");
        window.location.href = "/Home/Financial_Home";

        setInterval(function () {
            window.location.href = "/Home/Financial_Home";
        }, 5000)

    } else {
        //8.1 If Not Exist Year Alert And Convert him to Home Page
        if (checkYear !== "Exist") {
            Talert("No Valid Year");
            window.location.href = "/Home/Financial_Home";
        }
        //8.2 If No Edit In Posting Dste Close textbox of Posting Date
        if (editPD === "F2") {
            $("#TCGE-PostingDate").prop("disabled", true);
            $("#CBT-postingDate").prop("disabled", true);
            $(".CBT-postingDate").prop("disabled", true);
            $('#TCT-PostingDate').prop('disabled', true);
        }

        if (checkFormatSetting !== "True") {
            Talert("This Company Not has Format Setting!");
            window.location.href = "/Home/Financial_Home";
        }


        //8.3 drop down Exist On Pop Up Several Rates To Confirm Choosen Rate And Fill his Data
        $("#TCGE-SeveralSRate").change(function () {

            var rate = $(this).val();

            if (rate.length === 0) {
                $(this).css("border-color", "red");
            } else {
                $(this).css("border-color", "");

                //Empty Old Data If Exist
                TCGE_ClearRates();

                $("#TCGE-SystemRate").val(setSystemCurrFormate(+rate));
                $("#TCGE-TransactionRate").val(setSystemCurrFormate(+rate));
                $("#TCGE-DiffrenceRate").val("0");

                // this content for checkbook transfer
                $(selector).find(".TCGE-SystemRate").val(setSystemCurrFormate(+rate));
                $(selector).find(".TCGE-TransactionRate").val(setSystemCurrFormate(+rate));
                $(selector).find(".TCGE-DiffrenceRate").val("0");

                // this content for Bank-Reconcile
                $(selector).find('td:eq(0)').find('input').val(setSystemCurrFormate(+rate));

                // this content for Checkbook-Revaluate
                if (location.pathname == "/CheckbookRevaluate/Revaluate") {
                    $(selector).find('td:eq(4)').find('.TCGE-SystemRate').val(setSystemCurrFormate(+rate));
                }

                if (location.pathname == "/CheckbookTransfer/Index") {
                    CalculateAmount();
                }

                // this content for check-transfer
                if (location.pathname == "/C_CheckTransfer/CompanyCheckTransfer") {
                    $("#TCT-transactionRate").val(setSystemCurrFormate(+rate));
                }

                $("#TCGE-PUSeveralRates").modal("hide");
            }
        });


        //8.4 FOUT Of TRate Check Validation and to generate defference between it and System rate and set result in Deff. Rate
        $("#TCGE-TransactionRate").keyup(function () {

            var transactionRate = $(this).val().replace(regRemoveCurrFormate, "");
            var systemRate = $("#TCGE-SystemRate").val().replace(regRemoveCurrFormate, "");
            if (transactionRate > 0) {
                $(this).css("border-color", "");
                var difference = parseFloat(transactionRate) - parseFloat(systemRate);
                $("#TCGE-DiffrenceRate").val(setSystemCurrFormate(+difference));
            } else {
                $(this).css("border-color", "red");
            }

        }).focusin(function () {

            ClearSecondPartOfMain();

        }).focusout(function () {

            var transactionRate = $(this).val().replace(regRemoveCurrFormate, "");

            var systemRate = $("#TCGE-SystemRate").val().replace(regRemoveCurrFormate, "");

            if (transactionRate > 0) {
                $(this).css("border-color", "");
            } else {
                $(this).css("border-color", "red");
                var difference = parseFloat(transactionRate) - parseFloat(systemRate);
                $("#TCGE-DiffrenceRate").val(setSystemCurrFormate(+difference));
            }

            // of tax transaction Page
            $('input[name=EffectiveInput]').val('');
            $('select[name=EffectiveInput]').val('');
            $("#TCT-taxID option").show();
            $("#TCT-vatID option").show();
            $('#TCT-unitPrice').val('');
        });


        //Function 8.5 Here


        //------End of The first Above data---------------------------start of middle data to add to main table-----------------------


        //8.5 Change of dropdown AccountID To validate it and get details of Choosen Account to validate others and check related with analytic or cost
        $("#TCGE-AccountID").change(function () {

            var accountID = $(this).val(),
                currencyID = $("#TCGE-CurrencyID").val(),
                reference = $("#TCGE-Reference").val(),
                transactionRate = $("#TCGE-TransactionRate").val();

            if (!reference) {
                reference = $("#TCGE-Describtion").val();
            }
            if (!currencyID) {
                currencyID = CompId;
            }
            //Refresh All other data depend on it
            $("#TCGE-AccountName").val("");
            $("#TCGE-GlobalError2").text("");
            $("#TCGE-CurrencyID").css("border-color", "");
            $("#TCGE-OriginalAmount").val("");
            $("#TCGE-Debit").val("");
            $("#TCGE-Credit").val("");
            $("#TCGE-OriginalAmount").prop("disabled", true);

            //Clear hidden spans that tell me this account related by analytic account and cost center
            $("#TCGE-SHPUAA").text("");
            $("#TCGE-SHPUCC").text("");

            //Clear span and hide btn that tell cost center popup that this account related with Analytic to show back btn
            $("#TCGE-PUCCBtnBTA").hide();
            $("#TCGE-PUCCCheckAnalytic").text("");

            $("#TCGE-TAccountAnalytic").html("");
            $("#TCGE-TCostCenter").html("");
            //EmptyCostCenter $("#TCGE-TMainCostCenter").html("");
            //----------------------------------------

            if (accountID.length === 0) {
                $(this).css("border-color", "red");
            } else {
                $(this).css("border-color", "");

                var test = true;

                //check Currency ID choosen or not
                if (currencyID.length === 0) {
                    $("#TCGE-CurrencyID").css("border-color", "red");
                    $(this).val("");
                    test = false;
                } else {
                    $("#TCGE-CurrencyID").css("border-color", "");
                }

                if (reference.length === 0) {
                    $("#TCGE-Reference").css("border-color", "red");
                    $("#TCGE-Describtion").css("border-color", "red");
                    $(this).val("");
                    test = false;
                } else {
                    $("#TCGE-Reference").css("border-color", "");
                }

                if (transactionRate.length === 0 && $('#TCGE-TransactionRate').is(':visible')) {
                    $("#TCGE-TransactionRate").css("border-color", "red");
                    $(this).val("");
                    test = false;
                } else {
                    $("#TCGE-TransactionRate").css("border-color", "");
                }

                if ($('#CBT-amount').length > 0) {
                    var amountFromRW = $('#CBT-amount').val();
                    if (amountFromRW.length === 0) {
                        $('#CBT-amount').css('border-color', 'red');
                        test = false;
                    } else {
                        $('#CBT-amount').css('border-color', '');
                    }
                }
                var IsAdjust = false;
                if (window.location.href.indexOf("AdjustmentGETransaction") != -1) {
                    IsAdjust = true;
                }
                if (test === true) {

                    // New Ajax by API To Get Account Details 
                    GetAccountDetails(accountID, currencyID, IsAdjust);
                    ////$.ajax({
                    ////    contentType: 'application/json; charset=utf-8',
                    ////    dataType: 'json',
                    ////    method: "get",
                    ////    url: "/api/TransactionApi/GetAccountDetails?c_AID=" + accountID + "&currencyID=" + currencyID + "&IsAdjust=" + IsAdjust,
                    ////    success: function (result) {
                    ////        //Choosen Account Must be created with the same currency choosen above
                    ////        $("#TCGE-AccountName").val(result.AccountName);
                    ////        $("#TCGE-AccountType").text(result.AccountType);
                    ////        $("#TCGE-OriginalAmount").prop("disabled", false);
                    ////        //check if user mandatory choose document or not
                    ////        if (result.SupportDocument === true) {
                    ////            $("#TCGE-CheckDocument").text("Mand");
                    ////        } else {
                    ////            $("#TCGE-CheckDocument").text("");
                    ////        }
                    ////        //check if account related with analytic or not
                    ////        $("#TCGE-CheckAnalytic").text(result.C_AnalyticAccountID);
                    ////        $("#TCGE-CostCenterType").text(result.CostCenterType);
                    ////        $("#TCGE-CheckCostCenter").text(result.C_CostCenterID);
                    ////        $("#TCGE-CheckMainCostCenter").text(result.C_CostCenterGroupID);
                    ////        $("#MaximumAmountPerTransaction").text(result.MaiximumAmount);
                    ////        $("#MinimumAmountPerTransaction").text(result.MinimumAmount);
                    ////    }, error: function (request, status, error) {
                    ////        if (request.status === 406) {
                    ////            $("#TCGE-GlobalError2").text(JSON.parse(request.responseText).Message);
                    ////            $("#TCGE-AccountID").val("");
                    ////        }
                    ////    }
                    ////});
                }
            }
        });

        // New Ajax by API To Get Account Details

        //8.6 keyup of original amount to multiply OA * TR and put it in debit or credit by the type of account
        $("#TCGE-OriginalAmount").keyup(function (event) {

            event.preventDefault();
            if (event.keyCode === 13) {
                if ($("0").is(":enabled")) {
                    $("#TCGE-BtnAddRow").click();
                } else if ($("#TCGE-ConfirmUpdateRecord").is(":enabled")) {
                    $("#TCGE-ConfirmUpdateRecord").click();
                }
            }

            var originalAmount = $(this).val().replace(regRemoveCurrFormate, "");

            var trasactionRate = $("#TCGE-TransactionRate").val().replace(regRemoveCurrFormate, "");
            var accountType = $("#TCGE-AccountType").text();

            if (originalAmount > 0) {
                $(this).css("border-color", "");

                var result = parseFloat(originalAmount) * parseFloat(trasactionRate);
                if (accountType === "Debit" && !isNaN(result) && result > 0) {
                    $("#TCGE-Credit").val("");
                    $("#TCGE-Debit").val(setSystemCurrFormate(+parseFloat(result)));
                } else if (accountType === "Credit" && !isNaN(result) && result > 0) {
                    $("#TCGE-Debit").val("");
                    $("#TCGE-Credit").val(setSystemCurrFormate(+parseFloat(result)));
                }
            }

        }).focusout(function () {

            var originalAmount = $(this).val().replace(regRemoveCurrFormate, "");

            if (originalAmount > 0) {
                $(this).css("border-color", "");
            } else {
                $(this).css("border-color", "red");
                $("#TCGE-Debit").val("");
                $("#TCGE-Credit").val("");
            }
        });


        //8.7 keyUp of debit and credit and describtion to handle enter button 
        $("#TCGE-Debit").keyup(function (event) {
            event.preventDefault();
            if (event.keyCode === 13) {
                if ($("#TCGE-BtnAddRow").is(":enabled")) {
                    $("#TCGE-BtnAddRow").click();
                } else if ($("#TCGE-ConfirmUpdateRecord").is(":enabled")) {
                    $("#TCGE-ConfirmUpdateRecord").click();
                }
            }

        });
        $("#TCGE-Credit").keyup(function (event) {
            event.preventDefault();
            if (event.keyCode === 13) {
                if ($("#TCGE-BtnAddRow").is(":enabled")) {
                    $("#TCGE-BtnAddRow").click();
                } else if ($("#TCGE-ConfirmUpdateRecord").is(":enabled")) {
                    $("#TCGE-ConfirmUpdateRecord").click();
                }
            }
        });
        $("#TCGE-Describtion").keyup(function (event) {
            event.preventDefault();
            if (event.keyCode === 13) {
                if ($("#TCGE-BtnAddRow").is(":enabled")) {
                    $("#TCGE-BtnAddRow").click();
                } else if ($("#TCGE-ConfirmUpdateRecord").is(":enabled")) {
                    $("#TCGE-ConfirmUpdateRecord").click();
                }
            }

            var describtion = $(this).val();
            if (describtion.length === 0) {
                $(this).css("border-color", "red");
            } else {
                $(this).css("border-color", "");
            }
        });


        //8.8 Button add row of table after check of all data and check account id change exist analytic or cost or not and check document mandatory
        $("#TCGE-BtnAddRow").click(function () {

            $("#TCGE-GlobalError2").text("");

            //Get Above data
            var currencyID = $("#TCGE-CurrencyID").val();
            var transactionRate = $("#TCGE-TransactionRate").val();

            //Get Recently data
            var accountID = $("#TCGE-AccountID").val();
            var accountIDTbl = $("#TCGE-AccountID option:selected").text();
            var accountName = $("#TCGE-AccountName").val();
            var describtion = $("#TCGE-Describtion").val();
            var originalAmount = $("#TCGE-OriginalAmount").val().replace(regRemoveCurrFormate, "");
            var debit = $("#TCGE-Debit").val().replace(regRemoveCurrFormate, "");
            var credit = $("#TCGE-Credit").val().replace(regRemoveCurrFormate, "");
            var minAmount = $("#MinimumAmountPerTransaction").text().replace(regRemoveCurrFormate, "");
            var maxAmount = $("#MaximumAmountPerTransaction").text().replace(regRemoveCurrFormate, "");
            var document = $("#TCGE-SUD").val();

            //.replace(/^[, ]+|[, ]+$|[, ]+/g, "").trim();

            var checkDocument = $("#TCGE-CheckDocument").text();
            var checkAnalytic = $("#TCGE-CheckAnalytic").text();
            var checkCostCenter = $("#TCGE-CheckCostCenter").text();
            var checkMainCostCenter = $("#TCGE-CheckMainCostCenter").text();
            var costCenterType = $("#TCGE-CostCenterType").text();


            //Call function that check data typed correctly or not
            var test = checkBeforeAU(currencyID, transactionRate, accountID, describtion, originalAmount, debit, credit, checkDocument, document, maxAmount, minAmount);

            if (test === true) {
                //If Account choosen related with analytic id
                CheckAnalyticAndCostCenter();
            }
        }).dblclick(function (e) {
            /*  Prevents default behaviour  */
            e.preventDefault();
        });

        //8.9 button update data of record after click update icon from table and btn cancel update to refrech all fields
        $("#TCGE-ConfirmUpdateRecord").click(function () {

            $("#TCGE-GlobalError2").text("");

            //Get row ID that pass from icon in table
            var id = $("#TCGE-UpdateID").text();

            //Get Above data
            var currencyID = $("#TCGE-CurrencyID").val();
            var transactionRate = $("#TCGE-TransactionRate").val();

            //Get Recently data
            var accountID = $("#TCGE-AccountID").val();
            var accountIDTbl = $("#TCGE-AccountID option:selected").text();
            var accountName = $("#TCGE-AccountName").val();
            var describtion = $("#TCGE-Describtion").val();
            var document = $("#TCGE-SUD").val();
            var originalAmount = $("#TCGE-OriginalAmount").val().replace(regRemoveCurrFormate, "");
            var debit = $("#TCGE-Debit").val().replace(regRemoveCurrFormate, "");
            var credit = $("#TCGE-Credit").val().replace(regRemoveCurrFormate, "");
            var minAmount = $("#MinimumAmountPerTransaction").text().replace(regRemoveCurrFormate, "");
            var maxAmount = $("#MaximumAmountPerTransaction").text().replace(regRemoveCurrFormate, "");

            //Get Result of check of Account ID in document
            var checkDocument = $("#TCGE-CheckDocument").text();
            var checkAnalytic = $("#TCGE-CheckAnalytic").text();

            //Call function that check data typed correctly or not
            var test = checkBeforeAU(currencyID, transactionRate, accountID, describtion, originalAmount, debit, credit, checkDocument, document, maxAmount, minAmount);


            if (test === true) {

                //K Update Check if Account Is Anayltic
                $.ajax({
                    url: "/api/TransactionApi/CheckIfAnalaltyic?C_aid=" + $("#TCGE-AccountID").val(),
                    success: function (data) {
                        if (data != "" && data != null) {
                            //Function that open analytic popup with old data
                            OpenPUAAWithOutRefrech(debit, credit);
                        }
                    }
                })
                //K Update
                //If this account has data in Distribution DB Tbl then it related with analytic Account
                if ($(".DisDBrow_" + id + "").length) {

                    //Get Analytic ID that related with this account
                    var getAnalyticDBTbl = $(".DisDBrow_" + id + "").find('td');
                    var analyticID = getAnalyticDBTbl.eq(0).text();

                    //if open pop up of analytic account at least one not get from database again to save his changed if exist
                    if ($("#TCGE-SHPUAA").text() === "true") {

                        //Function that open analytic popup with old data
                        OpenPUAAWithOutRefrech(debit, credit);

                    } else {
                        $("#TCGE-PUAAccDisID").val("");
                        $("#TCGE-PUAAccDisName").val("");
                        $("#TCGE-PUAAccDisPercentage").val("");
                        $("#TCGE-PUAAccDisAmount").val("");
                        $("#TCGE-PUAADisDescribtion").val("");
                        $("#TCGE-PUAGlobalError").text("");
                        $("#TCGE-TAccountAnalytic").html("");

                        //Set Main Data of analytic PopUp
                        $("#TCGE-PUAAAccID").text(accountIDTbl);
                        $("#TCGE-PUAAAccName").text(accountName);
                        $("#TCGE-PUAAAnaID").text(analyticID);

                        //Set Original amount and unassign of analytic popup
                        if (debit.length > 0) {
                            $("#TCGE-PUAAOriginalAmount").text(setSystemCurrFormate(+debit));
                            $("#TCGE-PUAAUnassigned").text(setSystemCurrFormate(+debit));
                        } else if (credit.length > 0) {
                            $("#TCGE-PUAAOriginalAmount").text(setSystemCurrFormate(+credit));
                            $("#TCGE-PUAAUnassigned").text(setSystemCurrFormate(+credit));
                        }

                        //In Update Get distribution again of this analytic cause if not use all in last time
                        $.ajax({
                            contentType: 'application/json; charset=utf-8',
                            dataType: 'json',
                            method: "get",
                            url: "/api/TransactionApi/GetDistributionOfAccountAnalytic?analyticID=" + $("#TCGE-PUAAAnaID").text(),
                            success: function (result) {
                                $("#TCGE-PUAAccDisID").empty();

                                //hide final save btn of analytic popup and show update btn of analytic popup
                                $("#TCGE-PUAAFinalSave").hide();
                                $("#TCGE-PUAAFinalUpdate").show();
                                $("#TCGE-PUAnalyticAccount").modal("show");

                                if (result.length == 0) {

                                    $("#TCGE-PUAAccDisID").append($('<option/>', {
                                        value: "",
                                        text: "No Distribution Account Created To This Analytic"
                                    })
                                    );
                                } else {

                                    $("#TCGE-PUAAccDisID").append($('<option/>', {
                                        value: "",
                                        text: ChooseTxt
                                    })
                                    );
                                    $.each(result, function (index, row) {

                                        $("#TCGE-PUAAccDisID").append("<option value='" + row.C_DistID2 + "'>" + row.C_AccountDistributionID2 + "</option>");
                                    });

                                    //Span Text true to know that analytic popup is opened not refresh it again
                                    $("#TCGE-SHPUAA").text("true");

                                    //Get Data of distribution from analyticDB Tbl by row ID
                                    $("#TCGE-TAccountAnalyticDB").find(".DisDBrow_" + id + "").each(function () {

                                        var tds = $(this).find('td'),
                                            c_DistID = tds.eq(1).text(),
                                            distributionID = tds.eq(2).text(),
                                            distributionName = tds.eq(3).text(),
                                            describtion = tds.eq(5).text(),
                                            percentage = tds.eq(6).text().replace('%', ''),
                                            amount = tds.eq(7).text();

                                        //Filter distribution dropdown from distributions id that exist in analytic DBTbl
                                        $("#TCGE-PUAAccDisID option[value=" + c_DistID + "]").remove();

                                        //Get New amount if user change of original amount to multiply it in percentage
                                        var newAmount = parseFloat(percentage / 100) * parseFloat($("#TCGE-PUAAOriginalAmount").text().replace(regRemoveCurrFormate, ""));

                                        //Function that add to analytic tbl taht exist in analytic popup
                                        AddToAnalyticTbl(c_DistID, distributionID, distributionName, percentage, newAmount, describtion);

                                    });

                                    //Calculate Assign and Unassign after get data from DB table
                                    CalcAnalyticAssUnAss();


                                }
                            }
                        });
                    }
                    //And if This account exist data in cost center DB table that mean that this account related with cost center
                } else if ($(".CCAccDBrow_" + id + "").length) {

                    //Check span of check analytic if exist then show button of back to analytic that exist in CC popup
                    if ($("#TCGE-PUCCCheckAnalytic").text() === "Exist") {
                        $("#TCGE-PUCCBtnBTA").show();
                    } else {
                        $("#TCGE-PUCCBtnBTA").hide();
                    }

                    //Big Function to set Cost accounts in dropdown and fill table there exist in cost center popup "Update mode"
                    SetDataInCCPUToUpdate(id, debit, credit, accountIDTbl, accountName);

                    //else if in update this account not have data in AnalyticDB and costDB tables will update main tbl directly
                } else {
                    $("#TCGE-AccountID").prop("disabled", false);
                    UpdateMainTbl(id, accountID, accountName, accountIDTbl, describtion, document, originalAmount, debit, credit);
                }
            }
        }).dblclick(function (e) {
            /*  Prevents default behaviour  */
            e.preventDefault();

        });
        $("#TCGE-CancelUpdateRecord").click(function () {
            $(this).prop("disabled", true);
            $("#TCGE-ConfirmUpdateRecord").prop("disabled", true);
            $("#TCGE-BtnAddRow").prop("disabled", false);
            $("#TCGE-AccountID").val("");
            $("#TCGE-AccountName").val("");
            $("#TCGE-Describtion").val("");
            $("#TCGE-SUD").val("");
            $("#TCGE-OriginalAmount").val("");
            $("#TCGE-OriginalAmount").prop("disabled", true);
            $("#TCGE-Debit").val("");
            $("#TCGE-Credit").val("");

            //Clear spans that tell if popups opened at least one or not
            $("#TCGE-SHPUAA").text("");
            $("#TCGE-SHPUCC").text("");

            //filter Accounts
            $(".TCGE-TblAccID").each(function () {
                var result = $(this).text();
                $("#TCGE-AccountID option[value=" + result + "]").hide();
            });

            $("#TCGE-AccountID").prop("disabled", false);
        });


        //8.10 button confirm delete exist in PU that appear in icon delete of table
        $("#TCGE-ConfirmDeleteRecord").click(function () {
            //Get id of row by click of icon in table
            var id = $("#TCGE-PUDeleteID").text();

            //check if user click in update icon and not cancel update and need to remove this row
            if ($("#TCGE-ConfirmUpdateRecord").is(":enabled")) {
                $("#TCGE-ConfirmUpdateRecord").prop("disabled", true);
                $("#TCGE-CancelUpdateRecord").prop("disabled", true);
                $("#TCGE-BtnAddRow").prop("disabled", false);

                $("#TCGE-AccountID").prop("disabled", false);

                $("#TCGE-AccountID").val("");
                $("#TCGE-AccountName").val("");
                $("#TCGE-Describtion").val("");
                $("#TCGE-SUD").val("");
                $("#TCGE-OriginalAmount").val("");
                $("#TCGE-OriginalAmount").prop("disabled", true);
                $("#TCGE-Debit").val("");
                //$("#TCGE-Debit").prop("disabled", true);
                $("#TCGE-Credit").val("");
                //$("#TCGE-Credit").prop("disabled", true);
            }

            //Get Account That want to deleted to add it again to dropdown
            var tds = $("#TCGE-TTbl").find(".row_" + id + "").find('td');
            var accountID = tds.eq(1).text();
            var accountIDTbl = tds.eq(4).text();

            //filter Accounts
            $(".TCGE-TblAccID").each(function () {
                var result = $(this).text();
                $("#TCGE-AccountID option[value=" + result + "]").hide();
            });

            try {
                //Add deleted account to drop down
                //$("#TCGE-AccountID").append("<option value='" + accountID + "'>" + accountIDTbl + "</option>");
                $("#TCGE-AccountID option[value=" + accountID + "]").show();



                //order drop down by account ID
                $("#TCGE-AccountID").html($("#TCGE-AccountID option").sort(function (a, b) {
                    return a.text == b.text ? 0 : a.text < b.text ? -1 : 1
                }));
                $("#TCGE-AccountID").val("");

                //$("#TCGE-PUDeleteRecord").modal("hide");
                $("#TCGE-CancelDeleteRecord").click();

                $(".row_" + id + "").remove();

                $(".DisDBrow_" + id + "").remove();

                $(".CCAccDBrow_" + id + "").remove();

                $("#TCGE-AccountID[value='" + id + "']").show();
                ForceRefreshPicker();

                //call function that calculate sum of new debit or credit and get refrence in footer
                SumDebitAndCredit();

                //get length of table data to check if no exist data of it will open all apove data with validate it
                var rowCount = $('#TCGE-GTbl >tbody >tr').length;

                if (window.location.pathname === "/C_GeneralEntryTransaction/CompanyGETransaction") {
                    if (rowCount === 0) {

                        ResetInLastDelete();

                        //if general entry page is currently page
                        var batchAction = $("#TCGE-BatchAction").text(); //get if batch is append or new 
                        var fJEPer = $("#TCGE-FJEPer").text(); //To check J E Per Transaction Or Batch
                        //if by treansaction disable batch dropdown and button in delete last row
                        if (fJEPer === "B1") {
                            $("#TCGE-BatchID").prop("disabled", true);
                            $("#TCGE-BtnPUNewBatch").prop("disabled", true);
                        } else {
                            //else if by batch check if batch by append or new to disable dropdown or not
                            if (batchAction === "E2") {
                                $("#TCGE-BatchID").prop("disabled", true);
                            } else {
                                $("#TCGE-BatchID").prop("disabled", false);
                            }
                        }
                    }
                }
                else if (window.location.pathname === "/C_CashReciept/CompanyCashReciept" || "/C_CashWithdraw/CompanyCashWithdraw") {
                    if (rowCount === 1) {
                        ResetInLastDelete();
                    }
                }
            } catch (err) {

            }
        });


        //8.11 button to Switch Between Debit and credit
        $("#TCGE-SwitchDC").click(function () {
            var debit = $("#TCGE-Debit").val();
            var credit = $("#TCGE-Credit").val();
            if (debit.length > 0) {
                $("#TCGE-Credit").val(debit);
                $("#TCGE-Debit").val("");
            }
            if (credit.length > 0) {
                $("#TCGE-Debit").val(credit);
                $("#TCGE-Credit").val("");
            }
        });


        $("#TCGE-TTbl").on('click', '.MoreDetailsT', function () {
            var row = $(this).closest("tr");
            var tds = row.find("td");

            var accountIDTbl = tds.eq(4).text();
            var describtion = tds.eq(5).text();
            $("#TCGE-PUMDAccID").text(accountIDTbl);
            $("#TCGE-PUMDDes").text(describtion);
            $("#TCGE-PUMoreDetailsRecord").modal("show");
        });


        //function 8.12 & 8.13 & 8.14 & 8.15 & 8.16 & 8.17 Here


        //-----------------------start of analytic distribution from middle data---------------------------


        //8.18 change of dropdown distribution in analytic popup to get name and validation and keydown to handle keyboard events
        $("#TCGE-PUAAccDisID").change(function () {
            var distributionID = $(this).val();
            $("#TCGE-PUAAccDisName").val("");
            if (distributionID.length === 0) {
                $(this).css("border-color", "red");
            } else {
                $(this).css("border-color", "");
                $.ajax({
                    contentType: 'application/json; charset=utf-8',
                    dataType: 'json',
                    method: "get",
                    url: "/api/TransactionApi/GetDistributionName?distributionID=" + distributionID,
                    success: function (result) {
                        $("#TCGE-PUAAccDisName").val(result);
                    }
                });
            }
        }).keydown(function (event) {
            //handle events
            event.preventDefault();
            //Enter Key
            if (event.keyCode === 13) {
                //check who disable and who enable of add btn and update btn to click in enable
                if ($("#TCGE-PUAAddRow").is(":enabled")) {
                    $("#TCGE-PUAAddRow").click();
                } else if ($("#TCGE-PUAConfirmUpdateRecord").is(":enabled")) {
                    $("#TCGE-PUAConfirmUpdateRecord").click();
                }
            }

            //Escape key to cancel update when update row btn is enable
            if (event.key === "Escape") {
                if ($("#TCGE-PUACancelUpdateRecord").is(":enabled")) {
                    $("#TCGE-PUACancelUpdateRecord").click();
                }
            }
        });


        //8.19 keyup of percentage to get value of amount and set of it and to handle keyboard events
        $("#TCGE-PUAAccDisPercentage").keyup(function (event) {

            //handle events--------------------------------------------------
            event.preventDefault();
            if (event.keyCode === 13) {
                if ($("#TCGE-PUAAddRow").is(":enabled")) {
                    $("#TCGE-PUAAddRow").click();
                } else if ($("#TCGE-PUAConfirmUpdateRecord").is(":enabled")) {
                    $("#TCGE-PUAConfirmUpdateRecord").click();
                }
            }

            if (event.key === "Escape") {
                if ($("#TCGE-PUACancelUpdateRecord").is(":enabled")) {
                    $("#TCGE-PUACancelUpdateRecord").click();
                }
            }
            //-------------------------------------------------------------------

            //set value of amount by percentage value
            var percentage = $(this).val().replace(regRemoveCurrFormate, "");
            if (parseFloat(percentage) <= 100) {
                var originalAmount = $("#TCGE-PUAAOriginalAmount").text().replace(regRemoveCurrFormate, "");
                var result = parseFloat(percentage / 100) * parseFloat(originalAmount);
                if (!isNaN(result)) {
                    $("#TCGE-PUAAccDisAmount").val(setSystemCurrFormate(+result));
                }
            } else {
                $(this).val("");
                $("#TCGE-PUAAccDisAmount").val("");

            }
        });


        //8.20 Keyup of amount to get value of percentage and set of it and to handle keyboard events
        $("#TCGE-PUAAccDisAmount").keyup(function (event) {

            //handle events-----------------------------------------------------
            event.preventDefault();
            if (event.keyCode === 13) {
                if ($("#TCGE-PUAAddRow").is(":enabled")) {
                    $("#TCGE-PUAAddRow").click();
                } else if ($("#TCGE-PUAConfirmUpdateRecord").is(":enabled")) {
                    $("#TCGE-PUAConfirmUpdateRecord").click();
                }
            }

            if (event.key === "Escape") {
                if ($("#TCGE-PUACancelUpdateRecord").is(":enabled")) {
                    $("#TCGE-PUACancelUpdateRecord").click();
                }
            }
            //---------------------------------------------------------------------

            //set value of percentage by amount value
            var amount = $(this).val().replace(regRemoveCurrFormate, "");
            var originalAmount = $("#TCGE-PUAAOriginalAmount").text().replace(regRemoveCurrFormate, "");
            if (parseFloat(amount) <= parseFloat(originalAmount)) {
                var result = parseFloat(amount / originalAmount) * 100;
                if (!isNaN(result)) {
                    $("#TCGE-PUAAccDisPercentage").val(result.toFixed(2));
                }
            } else {
                $(this).val("");
                $("#TCGE-PUAAccDisPercentage").val("");
            }

        });


        //8.21 Keyup of description to validation it and handle keyboard events
        $("#TCGE-PUAADisDescribtion").keyup(function (event) {

            event.preventDefault();
            if (event.keyCode === 13) {
                if ($("#TCGE-PUAAddRow").is(":enabled")) {
                    $("#TCGE-PUAAddRow").click();
                } else if ($("#TCGE-PUAConfirmUpdateRecord").is(":enabled")) {
                    $("#TCGE-PUAConfirmUpdateRecord").click();
                }
            }

            if (event.key === "Escape") {
                if ($("#TCGE-PUACancelUpdateRecord").is(":enabled")) {
                    $("#TCGE-PUACancelUpdateRecord").click();
                }
            }

            var describtion = $(this).val();
            if (describtion.length > 0) {
                $(this).css("border-color", "");
            }

        });


        //8.22 Btn add row of distribution in analytic popup
        $("#TCGE-PUAAddRow").click(function () {

            $("#TCGE-PUAGlobalError").text("");

            var c_DistID = $("#TCGE-PUAAccDisID").val();
            var distributionID = $("#TCGE-PUAAccDisID option:selected").text();
            var percentage = $("#TCGE-PUAAccDisPercentage").val();
            var amount = $("#TCGE-PUAAccDisAmount").val().replace(regRemoveCurrFormate, "");
            var distributionName = $("#TCGE-PUAAccDisName").val();
            var describtion = $("#TCGE-PUAADisDescribtion").val();

            //function check to validate all fields before add row btn
            var test = CheckBeforeAUDist(c_DistID, percentage, amount, describtion);

            if (test === true) {

                //function that add row if all fields is valid
                AddToAnalyticTbl(c_DistID, distributionID, distributionName, percentage, amount, describtion);

                //empety all fields
                $("#TCGE-PUAAccDisID").val("");
                $("#TCGE-PUAAccDisName").val("");
                $("#TCGE-PUAAccDisPercentage").val("");
                $("#TCGE-PUAAccDisAmount").val("");
                $("#TCGE-PUAADisDescribtion").val("");

                //Remove added distribution from dropdown list to not add it again
                $("#TCGE-PUAAccDisID option[value=" + c_DistID + "]").remove();

                //function that calculate assign and unassign value of analytic distribution
                CalcAnalyticAssUnAss();
                ForceRefreshThisPicker("#TCGE-PUAAccDisID");
            }
        }).dblclick(function (e) {
            /*  Prevents default behaviour  */
            e.preventDefault();

        });


        //8.23 Btn exist in distribution table in analytic popup to remove clicked row
        $('#TCGE-TblAccountAnalytic').on('click', '.PUADeleteD', function () {

            var row = $(this).closest("tr");
            var td = row.find("td");
            var distributionID = td.eq(0).text();
            var id = td.eq(1).text();

            //if deleted in update mode
            if ($("#TCGE-PUAConfirmUpdateRecord").is(":enabled")) {
                $("#TCGE-PUAConfirmUpdateRecord").prop("disabled", true);
                $("#TCGE-PUACancelUpdateRecord").prop("disabled", true);
                $("#TCGE-PUAAddRow").prop("disabled", false);

                $("#TCGE-PUAAccDisID").val("");
                $("#TCGE-PUAAccDisName").val("");
                $("#TCGE-PUAAccDisPercentage").val("");
                $("#TCGE-PUAAccDisAmount").val("");
                $("#TCGE-PUAADisDescribtion").val("");
            }

            //filter drop down list of distribution by remove all exist in distribution table
            $(".TCGE-TblDistID").each(function () {
                var result = $(this).text();
                $("#TCGE-PUAAccDisID option[value=" + result + "]").remove();
            });


            //add distribution id again to drop down list cause it deleted and can use it again
            $("#TCGE-PUAAccDisID").append("<option value='" + id + "'>" + distributionID + "</option>");

            //remove row from table
            row.remove();

            //calculate assign and unassign valus after delete record
            CalcAnalyticAssUnAss();
            ForceRefreshThisPicker("#TCGE-PUAAccDisID");

        });


        //8.24 Btn Confirm Update Row exist in analytic popup to update distribution record
        $("#TCGE-PUAConfirmUpdateRecord").click(function () {

            //get id of row that need to update
            var id = $("#TCGE-UpdateDistID").text();

            var c_DistID = $("#TCGE-PUAAccDisID").val();
            var distributionID = $("#TCGE-PUAAccDisID option:selected").text();
            var percentage = $("#TCGE-PUAAccDisPercentage").val();
            var amount = $("#TCGE-PUAAccDisAmount").val().replace(regRemoveCurrFormate, "");
            var distributionName = $("#TCGE-PUAAccDisName").val();
            var describtion = $("#TCGE-PUAADisDescribtion").val();

            //Check before update that all fields is valid
            var test = CheckBeforeAUDist(c_DistID, percentage, amount, describtion);

            //if all fields valid start of update
            if (test === true) {
                //remove updated distribution id from drop down cause it exactly exist in table
                $("#TCGE-PUAAccDisID option[value=" + c_DistID + "]").remove();

                var tds = $(".Disrow_" + id + "").find('td');
                tds.eq(0).text(distributionID);
                tds.eq(1).text(c_DistID);
                tds.eq(2).text(distributionName);
                tds.eq(3).text(percentage + "%");
                tds.eq(4).text(setSystemCurrFormate(+parseFloat(amount)));
                tds.eq(5).text(describtion);

                CalcAnalyticAssUnAss();

                $("#TCGE-PUAAccDisName").val("");
                $("#TCGE-PUAAccDisPercentage").val("");
                $("#TCGE-PUAAccDisAmount").val("");
                $("#TCGE-PUAADisDescribtion").val("");

                //close update mode buttons and open save mode btn
                $(this).prop("disabled", true);
                $("#TCGE-PUACancelUpdateRecord").prop("disabled", true);
                $("#TCGE-PUAAddRow").prop("disabled", false);

            }

        });
        //8.25 Btn Cancel Update that refresh fields
        $("#TCGE-PUACancelUpdateRecord").click(function () {
            $(this).prop("disabled", true);
            $("#TCGE-PUAConfirmUpdateRecord").prop("disabled", true);
            $("#TCGE-PUAAddRow").prop("disabled", false);

            $("#TCGE-PUAAccDisID").val("");
            $("#TCGE-PUAAccDisName").val("");
            $("#TCGE-PUAAccDisPercentage").val("");
            $("#TCGE-PUAAccDisAmount").val("");
            $("#TCGE-PUAADisDescribtion").val("");

            //filter dropdown of distribution id by that exist in table after cancel update
            $(".TCGE-TblDistID").each(function () {
                var result = $(this).text();
                $("#TCGE-PUAAccDisID option[value=" + result + "]").remove();
            });

            //calculate assign and unassign of distribution after cancel update
            CalcAnalyticAssUnAss();
        });




        //8.26 Btn Final Save that exist in analytic popup and check if exist cost center related with this account
        $("#TCGE-PUAAFinalSave").click(function () {

            var accountID = $("#TCGE-AccountID").val(); //-
            var accountIDTbl = $("#TCGE-AccountID option:selected").text();
            var accountName = $("#TCGE-AccountName").val();
            var describtion = $("#TCGE-Describtion").val();
            var originalAmount = $("#TCGE-OriginalAmount").val().replace(regRemoveCurrFormate, "");
            var debit = $("#TCGE-Debit").val().replace(regRemoveCurrFormate, ""); //-
            var credit = $("#TCGE-Credit").val().replace(regRemoveCurrFormate, ""); //-
            if (!debit) {
                debit = 0;
            }
            if (!credit) {
                credit = 0;
            }
            var document = $("#TCGE-SUD").val();

            var analyticID = $("#TCGE-PUAAAnaID").text();
            var unassigned = $("#TCGE-PUAAUnassigned").text();

            var checkCostCenter = $("#TCGE-CheckCostCenter").text();
            var checkMainCostCenter = $("#TCGE-CheckMainCostCenter").text();
            var costCenterType = $("#TCGE-CostCenterType").text();

            //Check if all money is assigned to all rows
            if (unassigned === "0") {
                $("#TCGE-PUAGlobalError").text("");
                //Clear span that tell popup is opened
                $("#TCGE-SHPUAA").text("");

                //Check if this account related also with this account
                if (checkCostCenter.length > 0 || checkMainCostCenter.length > 0) {

                    //Close analytic popup
                    $("#TCGE-PUAnalyticAccount").modal("hide");

                    //Big function that set Data of CAccounts dropdown and table there exist in cost PopUp "Save Mode"
                    SetDataInCCPopUp(debit, credit, accountIDTbl, accountName, costCenterType, checkCostCenter, checkMainCostCenter, accountID, describtion);

                    //empty check analytic cause if click in add row again open cost center
                    $("#TCGE-CheckAnalytic").text("");
                    //Exist of analytic to make cost center popup show the button of back to analytic
                    $("#TCGE-PUCCCheckAnalytic").text("Exist");
                    //Show btn of back to analytic cause it already opened from save's analytic popup
                    $("#TCGE-PUCCBtnBTA").show();

                    //else if this account related with analytic only
                } else {

                    //Check if main account id has data cause no duplicate data in fast click
                    if (accountID.length === 0) {
                        $("#TCGE-AccountID").css("border-color", "red");
                    } else {
                        $("#TCGE-AccountID").css("border-color", "");

                        //Function add to main table and get id of row that added to add same id to Analytic DB Tbl
                        var id;
                        if (!GetJvRow(accountID)) {
                            id = AddToMainTbl(accountID, accountName, document, accountIDTbl, describtion, originalAmount, debit, credit);
                        } else {
                            id = GetJvRowClassNumber(accountID);
                        }
                        //Function That add all data to analytic distribution Database tbl her send some of this data
                        SaveUpdateAnalyticDistTble(analyticID, accountID, debit, credit, id);

                        //Close popup analytic Account
                        $("#TCGE-PUAnalyticAccount").modal("hide");
                    }
                }

                //else if that user not assign all data
            } else {
                $("#TCGE-PUAGlobalError").text("Unassigned Must Be Equal 0");
            }
        }).dblclick(function (e) {
            /*  Prevents default behaviour  */
            e.preventDefault();

        });


        //8.27 Btn Final Update that exist in analytic popup and check if exist cost center in Cost DB Tb, that mean this account related alo by CC
        $("#TCGE-PUAAFinalUpdate").click(function () {

            //Get row ID that pass from icon in table
            var id = $("#TCGE-UpdateID").text();

            var accountID = $("#TCGE-AccountID").val(); //-
            var accountIDTbl = $("#TCGE-AccountID option:selected").text();
            var accountName = $("#TCGE-AccountName").val();
            var describtion = $("#TCGE-Describtion").val();
            var originalAmount = $("#TCGE-OriginalAmount").val().replace(regRemoveCurrFormate, "");
            var debit = $("#TCGE-Debit").val().replace(regRemoveCurrFormate, ""); //-
            var credit = $("#TCGE-Credit").val().replace(regRemoveCurrFormate, ""); //-
            var document = $("#TCGE-SUD").val();

            var analyticID = $("#TCGE-PUAAAnaID").text();
            var unassigned = $("#TCGE-PUAAUnassigned").text();

            //Check if user assign all values
            if (unassigned === "0") {
                $("#TCGE-PUAGlobalError").text("");
                //$("#TCGE-SHPUAA").text("");

                //Check if Cost Center DB Tbl exist data with the same id that mean this account also related with CC
                if ($(".CCAccDBrow_" + id + "").length) {

                    //Fill Span to know that exist analytic to show back btn in cost center popup
                    $("#TCGE-PUCCCheckAnalytic").text("Exist");
                    //show btn back to analytic case already CC popup opened from Update's analytic popup
                    $("#TCGE-PUCCBtnBTA").show();
                    //Close analytic popup
                    $("#TCGE-PUAnalyticAccount").modal("hide");

                    //Big Function That Set Data in cost center popup in table and dropdown "Update Mode"
                    SetDataInCCPUToUpdate(id, debit, credit, accountIDTbl, accountName);
                }

                //else if account related only with analytic account
                else {
                    //Check if main account id has data cause no duplicate data in fast click
                    if (accountID.length === 0) {
                        $("#TCGE-AccountID").css("border-color", "red");
                    } else {
                        $("#TCGE-AccountID").css("border-color", "");

                        //Remove old data from analytic DB table to add new data with new updates
                        $(".DisDBrow_" + id + "").remove();

                        //Function That Update Main Tbl
                        UpdateMainTbl(id, accountID, accountName, accountIDTbl, describtion, document, originalAmount, debit, credit);

                        //Function That add all data to analytic distribution Database tbl her send some of this data
                        SaveUpdateAnalyticDistTble(analyticID, accountID, debit, credit, id);

                        $("#TCGE-PUAnalyticAccount").modal("hide");
                    }
                }

                //else if that user not assign all data
            } else {
                $("#TCGE-PUAGlobalError").text("Unassigned Must Be Equal 0");
            }
        }).dblclick(function (e) {
            /*  Prevents default behaviour  */
            e.preventDefault();

        });


        //function 8.28 & 8.29 & 8.30 & 8.31 & 8.32 & 8.33


        //-------------------------------------------Start Of Cost Center Part----------------------------------------------


        //8.34 Btn Exist In Cost Center PopUp to back to analytic popUp without refresh
        $("#TCGE-PUCCBtnBTA").click(function () {

            var debit = $("#TCGE-Debit").val().replace(regRemoveCurrFormate, "");
            var credit = $("#TCGE-Credit").val().replace(regRemoveCurrFormate, "");

            //Close cost center popup
            $("#TCGE-PUCostCenter").modal("hide");

            //Function That Open analytic popup without refresh
            OpenPUAAWithOutRefrech(debit, credit);

        });


        //8.35 Change of cost account drop down that get name of cost account and in key down handle key events
        $("#TCGE-PUCCOstAccounts").change(function () {
            var costAccount = $(this).val();
            $("#TCGE-PUCCOstAccountName").val("");
            if (costAccount.length === 0) {
                $(this).css("border-color", "red");
            } else {
                $(this).css("border-color", "");
                $.ajax({
                    contentType: 'application/json; charset=utf-8',
                    dataType: 'json',
                    method: "get",
                    url: "/api/TransactionApi/GetCostAccountName?costAccountID=" + costAccount,
                    success: function (result) {
                        $("#TCGE-PUCCOstAccountName").val(result);
                    }
                });
            }
        }).keydown(function (event) {
            event.preventDefault();
            if (event.keyCode === 13) {
                if ($("#TCGE-PUCAddRow").is(":enabled")) {
                    $("#TCGE-PUCAddRow").click();
                } else if ($("#TCGE-PUCConfirmUpdateRecord").is(":enabled")) {
                    $("#TCGE-PUCConfirmUpdateRecord").click();
                }
            }

            if (event.key === "Escape") {
                if ($("#TCGE-PUCCancelUpdateRecord").is(":enabled")) {
                    $("#TCGE-PUCCancelUpdateRecord").click();
                }
            }
        });


        //8.36 Keyup of percentage to get value of amount and handle key events Same as analytic
        $(document).on("keyup", "#TCGE-PUCCostAccPercentage", function () {
            event.preventDefault();
            if (event.keyCode === 13) {
                if ($("#TCGE-PUCAddRow").is(":enabled")) {
                    $("#TCGE-PUCAddRow").click();
                } else if ($("#TCGE-PUCConfirmUpdateRecord").is(":enabled")) {
                    $("#TCGE-PUCConfirmUpdateRecord").click();
                }
            }

            if (event.key === "Escape") {
                if ($("#TCGE-PUCCancelUpdateRecord").is(":enabled")) {
                    $("#TCGE-PUCCancelUpdateRecord").click();
                }
            }

            var percentage = GetMaskNumber($(this).val());
            //var originalAmount = GetMaskNumber($(document).find("#TCGE-PUCCOriginalAmount").text());
            if (percentage <= 100) {

                if ($("#TCGE-PUCForMain").is(":visible")) {

                    let originalAmount = $("#TCGE-PUCCostCenterAmountForMain").val().replace(regRemoveCurrFormate, "");
                    console.log(originalAmount);
                    var result = parseFloat(percentage / 100) * parseFloat(originalAmount);
                    if (!isNaN(result)) {
                        $("#TCGE-PUCCostAccAmount").val(setSystemCurrFormate(+parseFloat(result)));
                    }

                } else {

                    let originalAmount = $("#TCGE-PUCCOriginalAmount").text().replace(regRemoveCurrFormate, "");
                    var result = parseFloat(percentage / 100) * parseFloat(originalAmount);
                    if (!isNaN(result)) {
                        $("#TCGE-PUCCostAccAmount").val(setSystemCurrFormate(+parseFloat(result)));
                    }
                }

            } else {
                $(this).val("");
                $("#TCGE-PUCCostAccAmount").val("");

            }
        });
        $(document).on("keyup", "#TCGE-PUCCostCenterPercentageForMain", function () {
            var percentage = GetMaskNumber($(this).val());
            //var originalAmount = GetMaskNumber($(document).find("#TCGE-PUCCOriginalAmount").text());
            if (percentage <= 100) {

                let originalAmount = GetMaskNumber($("#TCGE-PUCCOriginalAmount").text());
                var result = parseFloat(percentage / 100) * parseFloat(originalAmount);
                if (!isNaN(result)) {
                    $("#TCGE-PUCCostCenterAmountForMain").val(setSystemCurrFormate(+parseFloat(result)));
                }

            } else {
                $(this).val("");
                $("#TCGE-PUCCostCenterAmountForMain").val("");
            }
        })
        $("#TCGE-PUCCostCenterAmountForMain").keyup(function () {

            var amount = GetMaskNumber($(this).val());
            var originalAmount = GetMaskNumber($("#TCGE-PUCCOriginalAmount").text())
            if (parseFloat(amount) <= parseFloat(originalAmount)) {
                var result = parseFloat(amount / originalAmount) * 100;
                if (!isNaN(result)) {
                    $("#TCGE-PUCCostCenterPercentageForMain").val(result.toFixed(2));
                }
            } else {
                $(this).val("");
                $("#TCGE-PUCCostCenterPercentageForMain").val("");
            }

        });

        //8.37 KeyUp of amount to get value of percentage and handle key events Same as analytic
        $("#TCGE-PUCCostAccAmount").keyup(function () {
            event.preventDefault();
            if (event.keyCode === 13) {
                if ($("#TCGE-PUCAddRow").is(":enabled")) {
                    $("#TCGE-PUCAddRow").click();
                } else if ($("#TCGE-PUCConfirmUpdateRecord").is(":enabled")) {
                    $("#TCGE-PUCConfirmUpdateRecord").click();
                }
            }

            if (event.key === "Escape") {
                if ($("#TCGE-PUCCancelUpdateRecord").is(":enabled")) {
                    $("#TCGE-PUCCancelUpdateRecord").click();
                }
            }

            var amount = $(this).val().replace(regRemoveCurrFormate, "");
            var originalAmount = $("#TCGE-PUCCOriginalAmount").text().replace(regRemoveCurrFormate, "");
            if (parseFloat(amount) <= parseFloat(originalAmount)) {

                if ($("#TCGE-PUCForMain").is(":visible")) {

                    var result = parseFloat(amount / $("#TCGE-PUCCostCenterAmountForMain").val().replace(regRemoveCurrFormate, "")) * 100;
                    if (!isNaN(result)) {
                        $("#TCGE-PUCCostAccPercentage").val(result.toFixed(2));
                    }

                } else {

                    var result = parseFloat(amount / originalAmount) * 100;
                    if (!isNaN(result)) {
                        $("#TCGE-PUCCostAccPercentage").val(result.toFixed(2));
                    }
                }
            } else {
                $(this).val("");
                $("#TCGE-PUCCostAccPercentage").val("");
            }
        });


        //8.38 KeyUp to validate it and handle key events Same as analytic
        $("#TCGE-PUCCostAccDescribtion").keyup(function () {
            event.preventDefault();
            if (event.keyCode === 13) {
                if ($("#TCGE-PUCAddRow").is(":enabled")) {
                    $("#TCGE-PUCAddRow").click();
                } else if ($("#TCGE-PUCConfirmUpdateRecord").is(":enabled")) {
                    $("#TCGE-PUCConfirmUpdateRecord").click();
                }
            }

            if (event.key === "Escape") {
                if ($("#TCGE-PUCCancelUpdateRecord").is(":enabled")) {
                    $("#TCGE-PUCCancelUpdateRecord").click();
                }
            }

            var describtion = $(this).val();
            if (describtion.length > 0) {
                $(this).css("border-color", "");
            }
        });


        //8.39 Btn Add row that exist in cost center popup
        $("#TCGE-PUCAddRow").click(function () {

            $("#TCGE-PUCGlobalError").text("");

            var c_CAID = $("#TCGE-PUCCOstAccounts").val();
            var costAccountID = $("#TCGE-PUCCOstAccounts option:selected").text();
            var percentage = $("#TCGE-PUCCostAccPercentage").val();
            var amount = $("#TCGE-PUCCostAccAmount").val().replace(regRemoveCurrFormate, "");
            var costAccountName = $("#TCGE-PUCCOstAccountName").val();
            var describtion = $("#TCGE-PUCCostAccDescribtion").val();

            var costCenterType = $("#TCGE-CostCenterType").text();

            var costCenterIDFM = $("#TCGE-PUCDropCostCenterForMain").val();
            var costCenterPercentageFM = $("#TCGE-PUCCostCenterPercentageForMain").val();
            var costCenterName = $("#TCGE-PUCDropCostCenterNameForMain").val();

            //Check validation of fileds before add row
            var test = CheckBeforeAUCCAccount(c_CAID, percentage, amount, describtion);

            //If all fields is valid
            if (test === true) {

                if (costCenterType === "CostCenter") {

                    //Function that add data to Cost center tbl exist in CC popup
                    AddToCostCenterTbl(c_CAID, costAccountID, costAccountName, percentage, amount, describtion, costCenterType);

                } else if (costCenterType === "MainCostCenter") {

                    //Function that add data to Cost center tbl exist in CC popup
                    AddToCostCenterTbl(c_CAID, costAccountID, costAccountName, percentage, amount, describtion, costCenterType, costCenterIDFM, costCenterPercentageFM, costCenterName);
                    ClearPUCCFieldsInMCCMode();

                }


                $("#TCGE-PUCCOstAccounts").val("");
                $("#TCGE-PUCCOstAccountName").val("");
                $("#TCGE-PUCCostAccPercentage").val("");
                $("#TCGE-PUCCostAccAmount").val("");
                $("#TCGE-PUCCostAccDescribtion").val("");

                //Remove cost account from drop down cause it added to table
                $("#TCGE-PUCCOstAccounts option[value=" + c_CAID + "]").remove();
                //Calc assign and unassign after add new row with new amount
                CalcCostCenterAssUnAss();
            }

        }).dblclick(function (e) {
            /*  Prevents default behaviour  */
            e.preventDefault();

        });


        //8.40 Btn Confirm Update exist in cost center popup
        $("#TCGE-PUCConfirmUpdateRecord").click(function () {

            //Get id of row that need to update it
            var id = $("#TCGE-UpdateCostAccID").text();

            var c_CAID = $("#TCGE-PUCCOstAccounts").val();
            var costAccountID = $("#TCGE-PUCCOstAccounts option:selected").text();
            var percentage = $("#TCGE-PUCCostAccPercentage").val();
            var amount = $("#TCGE-PUCCostAccAmount").val().replace(regRemoveCurrFormate, "");
            var costAccountName = $("#TCGE-PUCCOstAccountName").val();
            var describtion = $("#TCGE-PUCCostAccDescribtion").val();

            var costCenterID = $("#TCGE-PUCDropCostCenterForMain").val();
            var costCenterName = $("#TCGE-PUCDropCostCenterNameForMain").val();
            var costCenterPercentage = $("#TCGE-PUCCostCenterPercentageForMain").val();

            //check validation of fields before update
            var test = CheckBeforeAUCCAccount(c_CAID, percentage, amount, describtion);

            //if all fields is valid
            if (test === true) {

                if ($("#TCGE-PUCForMain").is(":visible")) {

                    var tds = $("#TCGE-TMainCostCenter").find('tr').eq(parseInt(id)).find('td');
                    tds.eq(0).text(c_CAID);
                    tds.eq(1).text(costCenterID);
                    tds.eq(2).text(costCenterPercentage);
                    tds.eq(3).text(costAccountID);
                    tds.eq(4).text(costAccountName);
                    tds.eq(5).text(percentage + "%");
                    tds.eq(6).text(setSystemCurrFormate(+parseFloat(amount)));
                    tds.eq(7).text(describtion);
                    tds.eq(9).text(costCenterName);

                    ClearPUCCFieldsInMCCMode();
                } else {

                    //Remove account id from drop down that updated
                    $("#TCGE-PUCCOstAccounts option[value=" + c_CAID + "]").remove();

                    var tds = $("#TCGE-TCostCenter").find('tr').eq(parseInt(id)).find('td');
                    tds.eq(0).text(c_CAID);
                    tds.eq(1).text(costAccountID);
                    tds.eq(2).text(costAccountName);
                    tds.eq(3).text(percentage + "%");
                    tds.eq(4).text(setSystemCurrFormate(+parseFloat(amount)));
                    tds.eq(5).text(describtion);

                    $("#TCGE-PUCCOstAccountName").val("");
                    $("#TCGE-PUCCostAccPercentage").val("");
                    $("#TCGE-PUCCostAccAmount").val("");
                    $("#TCGE-PUCCostAccDescribtion").val("");

                }

                //Calculate assign and un assign after update new data of amount
                CalcCostCenterAssUnAss();

                //Close update mode and open save model of this popup
                $(this).prop("disabled", true);
                $("#TCGE-PUCCancelUpdateRecord").prop("disabled", true);
                $("#TCGE-PUCAddRow").prop("disabled", false);

            }

        });
        //8.41 Btn Cancel update record that exist in cost center popup
        $("#TCGE-PUCCancelUpdateRecord").click(function () {

            //close update mode and open save mode in this popup
            $(this).prop("disabled", true);
            $("#TCGE-PUCConfirmUpdateRecord").prop("disabled", true);
            $("#TCGE-PUCAddRow").prop("disabled", false);

            $("#TCGE-PUCCOstAccounts").val("");
            $("#TCGE-PUCCOstAccountName").val("");
            $("#TCGE-PUCCostAccPercentage").val("");
            $("#TCGE-PUCCostAccAmount").val("");
            $("#TCGE-PUCCostAccDescribtion").val("");

            //filter cost account in dropdown by cost account that exist in table
            $(".TCGE-TblCCAccID").each(function () {
                var result = $(this).text();
                $("#TCGE-PUCCOstAccounts option[value=" + result + "]").remove();
            });

            //Calc again assign and unassign values after cancel update
            CalcCostCenterAssUnAss();

            ClearPUCCFieldsInMCCMode();
        });


        //8.42 Functions that call from table button icons that exist in Cost Center popup to delete
        $('#TCGE-TblCostCenter').on('click', '.PUCDeleteA', function () {

            var row = $(this).closest("tr");
            var td = row.find("td");
            var c_CAID = td.eq(0).text();
            var costAccountID = td.eq(1).text();

            //If in update mode
            if ($("#TCGE-PUCConfirmUpdateRecord").is(":enabled")) {
                $("#TCGE-PUCConfirmUpdateRecord").prop("disabled", true);
                $("#TCGE-PUCCancelUpdateRecord").prop("disabled", true);
                $("#TCGE-PUCAddRow").prop("disabled", false);

                $("#TCGE-PUCCOstAccounts").val("");
                $("#TCGE-PUCCOstAccountName").val("");
                $("#TCGE-PUCCostAccPercentage").val("");
                $("#TCGE-PUCCostAccAmount").val("");
                $("#TCGE-PUCCostAccDescribtion").val("");
            }

            //Filter Cost accounts in dropdown by cost accounts that exist in tbl
            $(".TCGE-TblCCAccID").each(function () {
                var result = $(this).text();
                $("#TCGE-PUCCOstAccounts option[value=" + result + "]").remove();
            });

            //add cost account to drop down that immediatly deleted
            $("#TCGE-PUCCOstAccounts").append("<option value='" + c_CAID + "'>" + costAccountID + "</option>");

            row.remove();

            //calculate Assign and unassign after delete row
            CalcCostCenterAssUnAss();
        });


        //8.43 Functions that call from table button icons that exist in Cost Center popup to update
        $("#TCGE-TblCostCenter").on('click', '.PUCEditA', function () {

            //Calc assign and UnAssign
            CalcCostCenterAssUnAss();

            //filter Cost Center Accounts
            $(".TCGE-TblCCAccID").each(function () {
                var result = $(this).text();
                $("#TCGE-PUCCOstAccounts option[value=" + result + "]").remove();
            });

            $("#TCGE-PUCCOstAccounts").focus();

            var row = $(this).closest("tr");
            var tds = row.find("td");
            var c_CAID = tds.eq(0).text();
            var costAccountID = tds.eq(1).text();
            var costAccountName = tds.eq(2).text();
            var percentage = tds.eq(3).text().replace('%', '');
            var amount = tds.eq(4).text().replace(regRemoveCurrFormate, "");
            var describtion = tds.eq(5).text();
            $("#TCGE-PUCCOstAccounts").append("<option value='" + c_CAID + "'>" + costAccountID + "</option>");
            $("#TCGE-PUCCOstAccounts").val(c_CAID);
            $("#TCGE-PUCCOstAccountName").val(costAccountName);
            $("#TCGE-PUCCostAccPercentage").val(percentage);
            $("#TCGE-PUCCostAccAmount").val(setSystemCurrFormate(+parseFloat(amount)));
            $("#TCGE-PUCCostAccDescribtion").val(describtion);

            var assigned = $("#TCGE-PUCCAssigned").text().replace(regRemoveCurrFormate, "");
            var unassigned = $("#TCGE-PUCCUnassigned").text().replace(regRemoveCurrFormate, "");

            //retrive amount of row to unassign value and minus it from assign value to update of this row
            var assval = parseFloat(assigned) - parseFloat(amount);
            var unass = parseFloat(unassigned) + parseFloat(amount);

            $("#TCGE-PUCCAssigned").text(setSystemCurrFormate(+parseFloat(assval)));
            $("#TCGE-PUCCUnassigned").text(setSystemCurrFormate(+parseFloat(unass)));

            //open update mode and close save mode
            $("#TCGE-PUCConfirmUpdateRecord").prop("disabled", false);
            $("#TCGE-PUCCancelUpdateRecord").prop("disabled", false);
            $("#TCGE-PUCAddRow").prop("disabled", true);

            //set id of row need to update.. to use it confirm update btn
            $("#TCGE-UpdateCostAccID").text(row.index());

        });


        //8.44 Btn Final save that exist in Cost center popup that fill cost Center DB Table
        $("#TCGE-PUCCFinalSave").click(function () {

            var accountID = $("#TCGE-AccountID").val(); //-
            if (!accountID) {
                accountID = $("#TCGE-CAID").val();
            }
            var accountIDTbl = $("#TCGE-AccountID option:selected").text();
            var accountName = $("#TCGE-AccountName").val();
            var describtion = $("#TCGE-Describtion").val();
            var originalAmount = $("#TCGE-OriginalAmount").val().replace(regRemoveCurrFormate, "");
            var debit = $("#TCGE-Debit").val().replace(regRemoveCurrFormate, ""); //-
            var credit = $("#TCGE-Credit").val().replace(regRemoveCurrFormate, ""); //-
            var document = $("#TCGE-SUD").val();

            var analyticID = $("#TCGE-PUAAAnaID").text();

            var costCenterID = $("#TCGE-PUCCCostID").text();
            var mainCostCenter = $("#TCGE-PUCCMainCostID").text();

            var unassigned = $("#TCGE-PUCCUnassigned").text();

            //If User Assign All amount
            if (unassigned === "0") {
                $("#TCGE-PUCGlobalError").text("");

                //Clear span that know cost center popup opened at least one or not
                $("#TCGE-SHPUCC").text("");

                //check if main account id has data to not duplicate data in fast click
                if (accountID.length === 0) {
                    $("#TCGE-AccountID").css("border-color", "red");
                } else {
                    $("#TCGE-AccountID").css("border-color", "");

                    //add data to main table and get id of this row that added to use it in CC DB tbl and AA DB Tbl if exist
                    var id;
                    var TrJv;
                    if (!$("#TCGE-PUCCFinalSave").attr("data-add")) {
                        id = AddToMainTbl(accountID, accountName, document, accountIDTbl, describtion, originalAmount, debit, credit, false, false, false, false);
                        TrJv = $("#TCGE-TTbl").find("tr").last();
                    } else {
                        TrJv = GetJvRow($("#TCGE-CAID").val())
                        id = GetJvRowClassNumber($("#TCGE-CAID").val());
                    }


                    if ($("#TCGE-PUCCHS").is(":visible")) {

                        //Add data to Cost center Database Table but this a part of data
                        SaveUpdateCostAccTble(costCenterID, accountID, debit, credit, id, "CostCenter");
                        $(TrJv).attr("data-costcentertype", "CostCenter");
                        $(TrJv).attr("data-centerId", costCenterID);
                    } else if ($("#TCGE-PUMCCHS").is(":visible")) {

                        SaveUpdateCostAccTble(mainCostCenter, accountID, debit, credit, id, "MainCostCenter");
                        $(TrJv).attr("data-costcentertype", "MainCostCenter");
                        $(TrJv).attr("data-centerId", mainCostCenter);
                    }


                    //If Exist analytic data will add it to analytic DB table with save btn of cc popup
                    if ($("#TCGE-PUCCCheckAnalytic").text() === "Exist") {
                        SaveUpdateAnalyticDistTble(analyticID, accountID, debit, credit, id);
                    }

                    $("#TCGE-PUCostCenter").modal("hide");
                }
                //else if unassign is not zero "user not assign all amount"
            } else {
                $("#TCGE-PUCGlobalError").text("Unassigned Must Be Equal 0");
            }

        }).dblclick(function (e) {
            /*  Prevents default behaviour  */
            e.preventDefault();

        });


        //8.45 Btn Final Update that exist in cost center popup
        $("#TCGE-PUCCFinalUpdate").click(function () {

            //Get row ID that pass from icon in table
            var id = $("#TCGE-UpdateID").text();

            var accountID = $("#TCGE-AccountID").val(); //-
            var accountIDTbl = $("#TCGE-AccountID option:selected").text();
            var accountName = $("#TCGE-AccountName").val();
            var describtion = $("#TCGE-Describtion").val();
            var originalAmount = $("#TCGE-OriginalAmount").val().replace(regRemoveCurrFormate, "");
            var debit = $("#TCGE-Debit").val().replace(regRemoveCurrFormate, ""); //-
            var credit = $("#TCGE-Credit").val().replace(regRemoveCurrFormate, ""); //-
            var document = $("#TCGE-SUD").val();
            if (!debit) {
                debit = 0;
            }
            if (!credit) {
                credit = 0;
            }
            var analyticID = $("#TCGE-PUAAAnaID").text();

            var costCenterID = $("#TCGE-PUCCCostID").text();
            var mainCostCenter = $("#TCGE-PUCCMainCostID").text();

            var unassigned = $("#TCGE-PUCCUnassigned").text();

            //If User Assign all value
            if (unassigned === "0") {
                //$("#TCGE-SHPUCC").text("");
                $("#TCGE-PUCGlobalError").text("");

                //check if main account id has data to not duplicate data in fast click
                if (accountID.length === 0) {
                    $("#TCGE-AccountID").css("border-color", "red");
                } else {
                    $("#TCGE-AccountID").css("border-color", "");

                    //remove cost center accounts old data from DB table of it
                    $(".CCAccDBrow_" + id + "").remove();

                    //Function update main table
                    UpdateMainTbl(id, accountID, accountName, accountIDTbl, describtion, document, originalAmount, debit, credit);

                    if ($("#TCGE-PUCCHS").is(":visible")) {

                        //Function That Save new updated data to cost center database table
                        SaveUpdateCostAccTble(costCenterID, accountID, debit, credit, id, "CostCenter");

                    } else if ($("#TCGE-PUMCCHS").is(":visible")) {

                        //Function That Save new updated data to cost center database table
                        SaveUpdateCostAccTble(mainCostCenter, accountID, debit, credit, id, "MainCostCenter");

                    }

                    //If exist analytic updated data remove it from DB table and add new update data to it
                    if ($("#TCGE-PUCCCheckAnalytic").text() === "Exist") {
                        $(".DisDBrow_" + id + "").remove();
                        SaveUpdateAnalyticDistTble(analyticID, accountID, debit, credit, id);
                    }

                    //Close Cost Center Popup
                    $("#TCGE-PUCostCenter").modal("hide");
                }
                //else if user not assign all amount
            } else {
                $("#TCGE-PUCGlobalError").text("Unassigned Must Be Equal 0");
            }

        }).dblclick(function (e) {
            /*  Prevents default behaviour  */
            e.preventDefault();

        });


        //8.46 Change of DropDown CostCenterID In MCC Mode to get Accounts Extends of data table
        $("#TCGE-PUCDropCostCenterForMain").change(function () {

            var costCenterID = $(this).val();

            $("#TCGE-PUCGlobalError").text("");
            $("#TCGE-PUCCOstAccounts").empty();
            $("#TCGE-PUCCOstAccountName").val("");
            $("#TCGE-PUCCostAccPercentage").val("");
            $("#TCGE-PUCCostAccAmount").val("");
            $("#TCGE-PUCCostAccDescribtion").val("");

            if (costCenterID.length === 0) {
                ClearPUCCFieldsInMCCMode();
            } else {

                var mainCostCenter = $("#TCGE-PUCCMainCostID").text();

                var amount = $("#TCGE-PUCCOriginalAmount").text().replace(regRemoveCurrFormate, "");
                $("select#TCGE-PUCCOstAccounts").empty();
                ForceRefreshPicker();
                $.ajax({
                    contentType: 'application/json; charset=utf-8',
                    dataType: 'json',
                    method: "get",
                    url: "/api/TransactionApi/GetCostCenterIDDetails?costCenterID=" + costCenterID + "&mainCostCenter=" + mainCostCenter,
                    success: function (result) {
                        $("#TCGE-PUCDropCostCenterNameForMain").val(result.CostCenterName);
                        $("#TCGE-PUCCostCenterPercentageForMain").val(result.Percentage + "%");
                        var percentage = parseFloat(result.Percentage / 100);
                        if (percentage == 0) {
                            $("#TCGE-PUCCostCenterPercentageForMain").removeAttr("disabled");
                            $("#TCGE-PUCCostCenterAmountForMain").removeAttr("disabled");
                        } else {
                            $("#TCGE-PUCCostCenterPercentageForMain").attr("disabled", "disabled");
                            $("#TCGE-PUCCostCenterAmountForMain").attr("disabled", "disabled");
                        }
                        $("#TCGE-PUCCostCenterAmountForMain").val(setSystemCurrFormate(+percentage * parseFloat(amount)));
                        CalcMainCostCenterAssUnass();
                        $.ajax({
                            contentType: 'application/json; charset=utf-8',
                            dataType: 'json',
                            method: "get",
                            url: "/api/TransactionApi/GetAccountsOfCostCenter?costCenterID=" + costCenterID,
                            success: function (result) {

                                if (result.length == 0) {

                                    $("#TCGE-PUCCOstAccounts").append($('<option/>', {
                                        value: "",
                                        text: "No Accounts Created To This Cost Center"
                                    })
                                    );
                                } else {

                                    $("#TCGE-PUCCOstAccounts").append($('<option/>', {
                                        value: "",
                                        text: ChooseTxt
                                    })
                                    );
                                    $.each(result, function (index, row) {

                                        $("#TCGE-PUCCOstAccounts").append("<option value='" + row.C_CAID + "'>" + row.C_CostAccountID + "</option>");

                                    });

                                    $("#TCGE-TMainCostCenter").find("tr").each(function () {

                                        var tds = $(this).find('td');
                                        var costCenterIDFromMain = tds.eq(1).text();

                                        if (costCenterIDFromMain === costCenterID) {

                                            var c_CAID = tds.eq(0).text();
                                            //Remove cost account from drop down cause it added to table
                                            $("#TCGE-PUCCOstAccounts option[value=" + c_CAID + "]").remove();

                                        }

                                    });
                                }
                            }
                        });
                    }
                });
            }
        });


        //8.47 Btn delete exist of mainCostCenter Table to delete row
        $('#TCGE-TblMainCostCenter').on('click', '.PUCDeleteA', function () {

            var row = $(this).closest("tr");

            //If in update mode
            if ($("#TCGE-PUCConfirmUpdateRecord").is(":enabled")) {
                $("#TCGE-PUCConfirmUpdateRecord").prop("disabled", true);
                $("#TCGE-PUCCancelUpdateRecord").prop("disabled", true);
                $("#TCGE-PUCAddRow").prop("disabled", false);

                $("#TCGE-PUCCOstAccounts").val("");
                $("#TCGE-PUCCOstAccountName").val("");
                $("#TCGE-PUCCostAccPercentage").val("");
                $("#TCGE-PUCCostAccAmount").val("");
                $("#TCGE-PUCCostAccDescribtion").val("");
            }

            //Filter Cost accounts in dropdown by cost accounts that exist in tbl
            $(".TCGE-TblCCAccID").each(function () {
                var result = $(this).text();
                $("#TCGE-PUCCOstAccounts option[value=" + result + "]").remove();
            });

            row.remove();

            //calculate Assign and unassign after delete row
            CalcCostCenterAssUnAss();

            ClearPUCCFieldsInMCCMode();
        });


        //8.48 Btn Update exist of mainCostCenter Table to set data That need to update in main fields
        $("#TCGE-TblMainCostCenter").on('click', '.PUCEditA', function () {

            //Calc assign and UnAssign
            CalcCostCenterAssUnAss();

            //filter Cost Center Accounts
            $(".TCGE-TblCCAccID").each(function () {
                var result = $(this).text();
                $("#TCGE-PUCCOstAccounts option[value=" + result + "]").remove();
            });

            $("#TCGE-PUCCOstAccounts").focus();

            var row = $(this).closest("tr");
            var tds = row.find("td");
            var c_CAID = tds.eq(0).text();
            var costCenterID = tds.eq(1).text();
            var costCenterIDPercentage = tds.eq(2).text().replace('%', '');
            var costAccountID = tds.eq(3).text();
            var costAccountName = tds.eq(4).text();
            var percentage = tds.eq(5).text().replace('%', '');
            var amount = tds.eq(6).text().replace(regRemoveCurrFormate, "");
            var describtion = tds.eq(7).text();
            var costCenterName = tds.eq(9).text();

            $("#TCGE-PUCDropCostCenterForMain").val(costCenterID);
            $("#TCGE-PUCDropCostCenterNameForMain").val(costCenterName);
            $("#TCGE-PUCCostCenterPercentageForMain").val(costCenterIDPercentage + "%");
            var costCenterIDAmount = parseFloat(costCenterIDPercentage / 100) * parseFloat($("#TCGE-PUCCOriginalAmount").text().replace(regRemoveCurrFormate, ""));
            $("#TCGE-PUCCostCenterAmountForMain").val(setSystemCurrFormate(+parseFloat(costCenterIDAmount)));

            CalcMainCostCenterAssUnass();

            $("#TCGE-PUCCOstAccounts").append("<option value='" + c_CAID + "'>" + costAccountID + "</option>");
            $("#TCGE-PUCCOstAccounts").val(c_CAID);

            $("#TCGE-PUCCOstAccountName").val(costAccountName);
            $("#TCGE-PUCCostAccPercentage").val(percentage);
            $("#TCGE-PUCCostAccAmount").val(setSystemCurrFormate(+parseFloat(amount)));
            $("#TCGE-PUCCostAccDescribtion").val(describtion);

            //--------------------------------------------------------------------------------------------------

            var assigned = $("#TCGE-PUCCAssigned").text().replace(regRemoveCurrFormate, "");
            var unassigned = $("#TCGE-PUCCUnassigned").text().replace(regRemoveCurrFormate, "");

            //retrive amount of row to unassign value and minus it from assign value to update of this row
            var assval = parseFloat(assigned) - parseFloat(amount);
            var unass = parseFloat(unassigned) + parseFloat(amount);

            $("#TCGE-PUCCAssigned").text(setSystemCurrFormate(+parseFloat(assval)));
            $("#TCGE-PUCCUnassigned").text(setSystemCurrFormate(+parseFloat(unass)));

            //---------------------------------------------------------------------------------------------------

            var ccAssigned = $("#TCGE-PUCCCFMAssign").text().replace(regRemoveCurrFormate, "");
            var ccUnassigned = $("#TCGE-PUCCCFMUnassign").text().replace(regRemoveCurrFormate, "");

            var ccAssval = parseFloat(ccAssigned) - parseFloat(amount);
            var ccUnass = parseFloat(ccUnassigned) + parseFloat(amount);

            $("#TCGE-PUCCCFMAssign").text(setSystemCurrFormate(+parseFloat(ccAssval)));
            $("#TCGE-PUCCCFMUnassign").text(setSystemCurrFormate(+parseFloat(ccUnass)));

            //---------------------------------------------------------------------------------------------------

            //open update mode and close save mode
            $("#TCGE-PUCConfirmUpdateRecord").prop("disabled", false);
            $("#TCGE-PUCCancelUpdateRecord").prop("disabled", false);
            $("#TCGE-PUCAddRow").prop("disabled", true);

            //set id of row need to update.. to use it confirm update btn
            $("#TCGE-UpdateCostAccID").text(row.index());

        });


        //8.49 handle hide of modals to remove update mode if exist
        $("#TCGE-PUCostCenter").on('hidden.bs.modal', function (e) {
            $("#TCGE-PUCCancelUpdateRecord").prop("disabled", true);
            $("#TCGE-PUCConfirmUpdateRecord").prop("disabled", true);
            $("#TCGE-PUCAddRow").prop("disabled", false);
        });
        $("#TCGE-PUAnalyticAccount").on('hidden.bs.modal', function (e) {
            //filter dropdown of distribution id by that exist in table after cancel update
            $(".TCGE-TblDistID").each(function () {
                var result = $(this).text();
                $("#TCGE-PUAAccDisID option[value=" + result + "]").remove();
            });
            ForceRefreshThisPicker("#TCGE-PUAAccDisID");
            $("#TCGE-PUACancelUpdateRecord").prop("disabled", true);
            $("#TCGE-PUAConfirmUpdateRecord").prop("disabled", true);
            $("#TCGE-PUAAddRow").prop("disabled", false);
        });


        //Function 8.50 & 8.51 & 8.52 & 8.53 & 8.54 & 8.55 & 8.56 & 8.57 & 8.58 Here

        $('#TCGE-Reset').click(function () {
            location.reload();
        });

    }



});


//--------------------------------------Functions Of Header Page------------------------------------------

//8.5 (F) Empty Old Data of Rates And Check if user Can Edit Of Transaction Rate or Not & function Clear second Part
function TCGE_ClearRates(Selector) {

    var allowUserERate = $("#TCGE-AllowUserERate").text(); //To check If User Can Edit in Transaction Rate Or Not

    if (allowUserERate === "True") {
        $("#TCGE-TransactionRate").prop("disabled", false);
        $(".TCGE-TransactionRate").prop("disabled", false);
    } else {
        $("#TCGE-TransactionRate").prop("disabled", true);
        $(".TCGE-TransactionRate").prop("disabled", true);
    }
    $("#TCGE-SystemRate").val("");
    $("#TCGE-TransactionRate").val("");
    $("#TCGE-DiffrenceRate").val("");

    // this content for checkbook transfer
    $(Selector).find(".TCGE-SystemRate").val("");
    $(Selector).find(".TCGE-TransactionRate").val("");
    $(Selector).find(".TCGE-DiffrenceRate").val("");

    // disable transaction-rate of transfer-to Checkbook
    $('#transferTo').find('.TCGE-TransactionRate').prop('disabled', true);
}
function ClearSecondPartOfMain() {
    $("#TCGE-AccountID").val("");
    $("#TCGE-AccountName").val("");
    $("#TCGE-SUD").val("");
    $("#TCGE-Desctibtion").val("");
    $("#TCGE-OriginalAmount").val("");
    $("#TCGE-Debit").val("");
    $("#TCGE-Credit").val("");
    $("#TCGE-GlobalError2").text("");
}


//---------------------------------Functions Of Main Part Of Transaction------------------------------------------

//8.12 (F) Expression function to generate identity id for table
var generateID = (function () {
    //var counter = 0;
    return function () {
        return $("#TCGE-GTbl").find("tbody").find("tr").length + 1;
        // return counter += 1;
    }
})();


//8.13 (F) function to sum debit and credit and get defference that call in add and update and delete
function SumDebitAndCredit() {
    var sumDebit = 0;
    var sumCredit = 0;
    // iterate through each td based on class and add the values
    $(".sDebitTbl").each(function () {

        var value = parseFloat($(this).text().replace(regRemoveCurrFormate, ""));
        // add only if the value is number
        if (!isNaN(value) && value.length != 0) {
            sumDebit += value;
        }
    });

    $(".sCreditTbl").each(function () {

        var value = parseFloat($(this).text().replace(regRemoveCurrFormate, ""));
        // add only if the value is number
        if (!isNaN(value) && value.length != 0) {
            sumCredit += value;
        }
    });


    $("#DebitTblFoot").text(setSystemCurrFormate(+sumDebit));
    $("#CreditTblFoot").text(setSystemCurrFormate(+sumCredit));
    $("#DiffOfDAC").text(setSystemCurrFormate(parseFloat(sumDebit - sumCredit)));
}


//8.14 (F) function to add row to main table then return id of this row and clear data
function AddToMainTbl(accountID, accountName, document, accountIDTbl, describtion, originalAmount, debit, credit
    , disable = false, RemoveDelUpBtn = false, RemoveCostBtn = true, RemoveAnabtn = true, Merge = false, CallBack = null) {

    //disabled above data to not change of it
    $("#TCGE-BatchID").prop("disabled", true);
    if (disable) {
        $("#TCGE-PostingDate").prop("disabled", true);
        $("#CBT-postingDate").prop("disabled", true);
        $("#TCGE-JEDate").prop("disabled", true);
    }
    $("#TCGE-CurrencyID").prop("disabled", true);
    $("#TCGE-TransactionRate").prop("disabled", true);
    $("#TCGE-BtnPUNewBatch").prop("disabled", true);

    $("#CBT-checkbookID").prop("disabled", true);
    $("#CBT-transactionDate").prop("disabled", true);
    $("#TCGE-TransactionRate").prop("disabled", true);
    //---------------------------------------------

    if (debit.length > 0 && $("#TCGE-OriginalAmount").is(":hidden")) {
        originalAmount = setSystemCurrFormate(+debit);
    } else if (credit.length > 0 && $("#TCGE-OriginalAmount").is(":hidden")) {
        originalAmount = setSystemCurrFormate(+credit);
    } else {
        originalAmount = setHardCurrFormate(+originalAmount);
    }

    //call function in expression to generate a new id for new row
    var ID = generateID();
    var HideCostBtn = "";
    var HideAnaBtn = "";
    if (RemoveCostBtn) {
        HideCostBtn = "hide";
    }
    if (RemoveAnabtn) {
        HideAnaBtn = "hide";
    }
    var content = "<tr class='row_" + ID + "' data-costcentertype='' data-centerId=''>" +
        "<td>" + '<button type="button" class="btn btn-sm mr-1 MoreDetailsT"><span class="fa fa-eye"></span></button>'
        + '<button type="button" id="dmt" class="btn btn-danger btn-sm mr-1" onclick="DeleteT(\'' + ID + '\');"><span class="fa fa-trash-o"></span></button>' + '<button type="button" id="emt" class="btn btn-warning btn-sm" onclick="EditT(\'' + ID + '\');"><span class="fa fa-edit"></span></button>'
        + '<button type="button" class="btn btn-sm btn-secondary mr-1 GetAnalyticDetails hide"><span class="">A</span></button>'
        + '<button type="button" class="btn btn-sm btn-primary mr-1 GetCostCenterDetails hide"><span class="">C</span></button>'
        + "</td>" +
        "<td class='hide-normal TCGE-TblAccID'>" + accountID + "</td>" +
        "<td class='accountName'> " + accountName + "</td> " +
        "<td class='document'>" + document + "</td>" +
        "<td class='hide-normal accountIDTbl'>" + accountIDTbl + "</td>" +
        "<td class='hide-normal describtion'>" + describtion + "</td>" +
        "<td class='OrA'>" + originalAmount + "</td>" +
        "<td class='sDebitTbl'>" + setSystemCurrFormate(+debit) + "</td>" +
        "<td class='sCreditTbl'>" + setSystemCurrFormate(+credit) + "</td>" +
        "</tr>";

    if (!(debit == 0 && credit == 0)) {
        $("#TCGE-TTbl").append(content);
    }

    //call function that calculate sum of new debit or credit and get defference in footer
    SumDebitAndCredit();

    //refresh Recently data to add a new row
    $("#TCGE-AccountID").val("");
    $("#TCGE-AccountName").val("");
    $("#TCGE-Describtion").val("");
    $("#TCGE-SUD").val("");
    $("#TCGE-OriginalAmount").val("");
    $("#TCGE-OriginalAmount").prop("disabled", true);
    $("#TCGE-Debit").val("");
    $("#TCGE-Credit").val("");

    $("#TCGE-AccountID option[value=" + accountID + "]").hide();

    $("#collapseFirst").collapse("hide");
    if (RemoveDelUpBtn) {
        $("#TCGE-TTbl").find("tr").last().find("#dmt").remove();
        $("#TCGE-TTbl").find("tr").last().find("#emt").remove();
    }
    if (!CallShowHideAnayltic) {
        RunAfterAjax(function () {
            ShowHideAnaylticAndCost()
        },1000)
        CallShowHideAnayltic = true;
    }
    if (CallBack != null) {
        CallBack();
    }
    if (Merge) {
        MergJv();
        return GetJvRowClassNumber(accountID);
    } else {
        return ID;
    }
}
function CheckAccountAnalytic(AccId) {

}

//8.15 (F) function called in save and update to check typed data is true of false
function checkBeforeAU(currencyID, transactionRate, accountID, describtion, originalAmount, debit, credit, checkDocument, document, maximumAmount, MinimumAmount) {
    var test = true;

    if (currencyID.length === 0) {
        //$("#TCGE-CurrencyID").css("border-color", "red");
        NotValid($("#TCGE-CurrencyID"))
        $("select#TCGE-CurrencyID").removeAttr("disabled");
        $("button#TCGE-CurrencyID").removeAttr("disabled");

        test = false;
    } else {
        $("#TCGE-CurrencyID").css("border-color", "");
    }

    if ($("#TCGE-TransactionRate").is(":enabled")) {
        if (transactionRate.length === 0) {
            $("#TCGE-TransactionRate").css("border-color", "red");
            test = false;
        } else {
            $("#TCGE-TransactionRate").css("border-color", "");
        }
    }

    if (accountID.length === 0) {
        $("#TCGE-AccountID").css("border-color", "red");
        test = false;
    } else {
        $("#TCGE-AccountID").css("border-color", "");
    }

    if (describtion.trim().length === 0) {
        $("#TCGE-Describtion").css("border", "1px solid red");
        test = false;
    } else {
        $("#TCGE-Describtion").css("border", "");
    }

    if ($("#TCGE-OriginalAmount").is(":enabled") && $("#TCGE-OriginalAmount").is(":visible")) {
        if (originalAmount.length === 0) {
            $("#TCGE-OriginalAmount").css("border-color", "red");
            test = false;
        } else {
            $("#TCGE-OriginalAmount").css("border-color", "");
        }
    }

    if (GetMaskNumber(credit) > 0 && GetMaskNumber(debit) > 0) {
        $("#TCGE-GlobalError2").text("You Must Fill only one Debit OR Credit..!");
        test = false;
    } else {

        var amountDORC;
        if (credit.length > 0) {
            amountDORC = credit;
        } else if (debit.length > 0) {
            amountDORC = debit;
        }

        if (maximumAmount.length > 0 && MinimumAmount.length > 0) {

            if (parseFloat(amountDORC) >= parseFloat(MinimumAmount) && parseFloat(amountDORC) <= parseFloat(maximumAmount)) {
                $("#TCGE-GlobalError2").text("");
            } else {
                $("#TCGE-GlobalError2").text("Account Minimum Amount = " + MinimumAmount + " And Maximum Amount = " + maximumAmount + "")
                test = false;
            }

        } else if (maximumAmount.length > 0 && MinimumAmount.length === 0) {

            if (parseFloat(amountDORC) <= parseFloat(maximumAmount)) {
                $("#TCGE-GlobalError2").text("");
            } else {
                $("#TCGE-GlobalError2").text("Account Maximum Amount = " + maximumAmount + "")
                test = false;
            }

        } else if (MinimumAmount.length > 0 && maximumAmount.length === 0) {

            if (parseFloat(amountDORC) >= parseFloat(MinimumAmount)) {
                $("#TCGE-GlobalError2").text("");
            } else {
                $("#TCGE-GlobalError2").text("Account Minimum Amount = " + MinimumAmount + "")
                test = false;
            }
        }

    }

    if (checkDocument === "Mand") {
        if (document.length === 0) {
            $("#TCGE-SUD").css("border-color", "red");
            test = false
        } else {
            $("#TCGE-SUD").css("border-color", "");
        }
    } else {
        $("#TCGE-SUD").css("border-color", "");
    }

    if (test === true) {
        return true;
    } else {
        return false;
    }
}


//8.16 (F) function to update row of main table by bath to it id and clear data
function UpdateMainTbl(id, accountID, accountName, accountIDTbl, describtion, document, originalAmount, debit, credit) {

    if (debit.length > 0 && $("#TCGE-OriginalAmount").is(":hidden")) {
        originalAmount = setSystemCurrFormate(+debit);
    } else if (credit.length > 0 && $("#TCGE-OriginalAmount").is(":hidden")) {
        originalAmount = setSystemCurrFormate(+credit);
    } else {
        originalAmount = setHardCurrFormate(+originalAmount);
    }

    var tds = $(".row_" + id + "").find('td');
    tds.eq(1).text(accountID);
    tds.eq(2).text(accountName);
    tds.eq(3).text(document);
    tds.eq(4).text(accountIDTbl);
    tds.eq(5).text(describtion);
    tds.eq(6).text(originalAmount);
    tds.eq(7).text(setSystemCurrFormate(+debit));
    tds.eq(8).text(setSystemCurrFormate(+credit));

    $("#TCGE-AccountID option[value=" + accountID + "]").hide();

    //refresh Recently data to add a new row
    $("#TCGE-AccountID").val("");
    $("#TCGE-AccountName").val("");
    $("#TCGE-Describtion").val("");
    $("#TCGE-SUD").val("");
    $("#TCGE-OriginalAmount").val("");
    $("#TCGE-OriginalAmount").prop("disabled", true);
    $("#TCGE-Debit").val("");
    $("#TCGE-Credit").val("");

    $("#TCGE-ConfirmUpdateRecord").prop("disabled", true);
    $("#TCGE-CancelUpdateRecord").prop("disabled", true);
    $("#TCGE-BtnAddRow").prop("disabled", false);

    //call function that calculate sum of new debit or credit and get refrence in footer
    SumDebitAndCredit();
}


//8.17 (F) Functions that call from table button icons to pass id of row click
function DeleteT(id) {
    $("#TCGE-PUDeleteID").text(id);
    $("#TCGE-PUDeleteRecord").modal("show");
}
// 8.171 (F) Function That Delete From Jv Table With No Model
function DeletTNM(id) {
    $("#TCGE-PUDeleteID").text(id);
    $("#TCGE-ConfirmDeleteRecord").trigger("click");
}
function EditT(id) {
    $("#TCGE-AccountID").focus();

    $("#TCGE-SHPUAA").text("");
    $("#TCGE-SHPUCC").text("");
    $("#TCGE-PUCCCheckAnalytic").text("");
    $("#TCGE-AccountType").text('');

    //filter Accounts
    $(".TCGE-TblAccID").each(function () {
        var result = $(this).text();
        $("#TCGE-AccountID option[value=" + result + "]").hide();
    });

    $("#TCGE-AccountID").prop("disabled", true);

    var tds = $(".row_" + id + "").find('td');
    var accountID = tds.eq(1).text();
    var accountName = tds.eq(2).text();
    var document = tds.eq(3).text();
    var accountIDTbl = tds.eq(4).text();
    var describtion = tds.eq(5).text();
    var originalAmount = tds.eq(6).text();
    var debit = tds.eq(7).text();
    if (debit === "0") {
        debit = "";
    } else {
        $("#TCGE-AccountType").text('Debit');
    }
    var credit = tds.eq(8).text();
    if (credit === "0") {
        credit = "";
    } else {
        $("#TCGE-AccountType").text('Credit');
    }

    //$("#TCGE-AccountID").append("<option value='" + accountID + "'>" + accountIDTbl + "</option>");
    $("#TCGE-AccountID option[value=" + accountID + "]").show();
    $("#TCGE-AccountID").val(accountID);
    $("#TCGE-AccountName").val(accountName);
    $("#TCGE-Describtion").val(describtion);
    $("#TCGE-SUD").val(document);
    $("#TCGE-OriginalAmount").val(originalAmount);
    $("#TCGE-Debit").val(debit);
    $("#TCGE-Credit").val(credit);
    $("#TCGE-Debit").removeAttr("disabled");
    $("#TCGE-Credit").removeAttr("disabled");


    $("#TCGE-OriginalAmount").prop("disabled", false);
    //$("#TCGE-Debit").prop("disabled", false);
    //$("#TCGE-Credit").prop("disabled", false);

    $("#TCGE-ConfirmUpdateRecord").prop("disabled", false);
    $("#TCGE-CancelUpdateRecord").prop("disabled", false);
    $("#TCGE-BtnAddRow").prop("disabled", true);

    $("#TCGE-UpdateID").text(id);
}


function ResetInLastDelete() {
    var editPD = $("#TCGE-EPD").text(); //To Check If User Can Edit In Posting Date Or Not
    var companyID = $("#TCGE-CompanyID").text();

    $("#collapseFirst").collapse("show");
    $("#TCGE-JEDate").prop("disabled", false);
    $("#TCGE-CurrencyID").prop("disabled", false);
    $("#TCGE-BtnPUNewBatch").prop("disabled", false);

    $("#CBT-transactionDate").prop("disabled", false);
    $("#CBT-checkbookID").prop("disabled", false);

    if ($("#TCGE-CurrencyID").val() === companyID) {
        $("#TCGE-TransactionRate").prop("disabled", true);
    } else {
        var allowUserERate = $("#TCGE-AllowUserERate").text();
        if (allowUserERate === "True") {
            $("#TCGE-TransactionRate").prop("disabled", false);
        } else {
            $("#TCGE-TransactionRate").prop("disabled", true);
        }
    }

    if (editPD === "F2") {
        $("#TCGE-PostingDate").prop("disabled", true);
        $("#CBT-postingDate").prop("disabled", true);
        $(".CBT-postingDate").prop("disabled", true);
    } else {
        $("#TCGE-PostingDate").prop("disabled", false);
        $("#CBT-postingDate").prop("disabled", false);
        $(".CBT-postingDate").prop("disabled", false);
    }
}

//----------------------------Functions Of Analytic Part-----------------------------------------------------------

//8.28 (F) function that validate fileds before add and update distribution
function CheckBeforeAUDist(c_DistID, percentage, amount, describtion) {
    var test = true;

    if (c_DistID.length === 0) {
        $("#TCGE-PUAAccDisID").css("border-color", "red");
        test = false;
    } else {
        $("#TCGE-PUAAccDisID").css("border-color", "");
    }

    if (percentage.length === 0) {
        $("#TCGE-PUAAccDisPercentage").css("border-color", "red");
        test = false;
    } else if (percentage == 0) {
        $("#TCGE-PUAAccDisPercentage").css("border-color", "red");
        test = false;
    } else {
        $("#TCGE-PUAAccDisPercentage").css("border-color", "");
    }

    if (amount.length === 0) {
        $("#TCGE-PUAAccDisAmount").css("border-color", "red");
        test = false;
    } else if (amount == 0) {
        $("#TCGE-PUAAccDisAmount").css("border-color", "red");
        test = false;
    } else {
        $("#TCGE-PUAAccDisAmount").css("border-color", "");
    }

    if (describtion.length === 0) {
        $("#TCGE-PUAADisDescribtion").css("border-color", "red");
        test = false;
    } else {
        $("#TCGE-PUAADisDescribtion").css("border-color", "");
    }

    var unassigned = $("#TCGE-PUAAUnassigned").text().replace(regRemoveCurrFormate, "");

    var rOfAmountMUnass = parseFloat(unassigned - amount);
    if (rOfAmountMUnass < 0) {
        $("#TCGE-PUAGlobalError").text("Failed amount..!");
        test = false;
    } else {
        $("#TCGE-PUAGlobalError").text("");
    }

    if (test === true) {
        return true;
    } else {
        return false;
    }
}


//8.29 (F) function that add data to analytic distribution table that exist in analytic popup
function AddToAnalyticTbl(c_DistID, distributionID, distributionName, percentage, amount, describtion) {

    //onclick="PUADeleteD(\'' + c_DistID + '\');"
    var asd = $("#TCGE-TAccountAnalytic").find("tr").hasClass(distributionID.replace(" ", "_"));
    if (!$("#TCGE-TAccountAnalytic").find("tr").hasClass(distributionID.replace(" ", "_"))) {
        var analyticContent = "<tr class='Disrow_" + c_DistID + "  " + distributionID.replace(" ", "_") + "'>" +
            "<td>" + distributionID + "</td>" +
            "<td class='hide-normal TCGE-TblDistID'>" + c_DistID + "</td>" +
            "<td>" + distributionName + "</td>" +
            "<td>" + percentage + "%" + "</td>" +
            "<td class='SumAmountTblAnalyticDist'>" + setSystemCurrFormate(+amount) + "</td>" +
            "<td>" + describtion + "</td>" +
            "<td>" + '<button type="button" class="btn btn-danger btn-sm mr-1 PUADeleteD"><span class="fa fa-trash-o"></span></button>' + '<button type="button" class="btn btn-warning btn-sm" onclick="PUAEditD(\'' + c_DistID + '\');"><span class="fa fa-edit"></span></button>' + "</td>" +
            "</tr>";
        $("#TCGE-TAccountAnalytic").append(analyticContent);
    }
 

}


//8.30 (F) Function that SaveAndUpdate Analytic Distribution DB Tbl
function SaveUpdateAnalyticDistTble(analyticID, accountID, debit = 0, credit = 0, id) {
    $("#TCGE-AccountID").prop("disabled", false);
    $("#TCGE-TAccountAnalytic").find('tr').each(function () {

        var tds = $(this).find('td'),
            distributionID = tds.eq(0).text(),
            c_DistID = tds.eq(1).text(),
            distributionName = tds.eq(2).text(),
            percentage = tds.eq(3).text(),
            amount = tds.eq(4).text(),
            disdescribtion = tds.eq(5).text();

        var IsDebit = false;
        if (debit != 0) {
            debit = amount;
        } else {
            credit = amount;
        }
        if (!$("#TCGE-TAccountAnalytic").find("tr").hasClass("DisDBrow_" + id + "")) {
            var analyticDBContent = "<tr class='DisDBrow_" + id + "'>" +
                "<td class='" + analyticID + " analyticID'>" + analyticID + "</td>" +
                "<td>" + c_DistID + "</td>" +
                "<td>" + distributionID + "</td>" +
                "<td>" + distributionName + "</td>" +
                "<td>" + accountID + "</td>" +
                "<td>" + disdescribtion + "</td>" +
                "<td>" + percentage + "</td>" +
                "<td>" + amount + "</td>" +
                "<td>" + debit + "</td>" +
                "<td>" + credit + "</td>" +
                "</tr>";

            $("#TCGE-TAccountAnalyticDB").append(analyticDBContent);
        }
    });
}


//8.31 (F) Function That Open Analytic popUp without refresh and go to Database That call when popup opened at least one  and update only amount by D OR C
function OpenPUAAWithOutRefrech(debit, credit) {

    $("#TCGE-PUAAccDisID").val("");
    $("#TCGE-PUAAccDisName").val("");
    $("#TCGE-PUAAccDisPercentage").val("");
    $("#TCGE-PUAAccDisAmount").val("");
    $("#TCGE-PUAADisDescribtion").val("");
    $("#TCGE-PUAGlobalError").text("");

    //set original amount in pop up
    if (debit.length > 0) {
        $("#TCGE-PUAAOriginalAmount").text(setSystemCurrFormate(+debit));
        $("#TCGE-PUAAUnassigned").text(setSystemCurrFormate(+debit));
    } else if (credit.length > 0) {
        $("#TCGE-PUAAOriginalAmount").text(setSystemCurrFormate(+credit));
        $("#TCGE-PUAAUnassigned").text(setSystemCurrFormate(+credit));
    }
    //----------------------------------------------

    if ($("#TCGE-BtnAddRow").is(":enabled")) {
        $("#TCGE-PUAAFinalSave").show();
        $("#TCGE-PUAAFinalUpdate").hide();
    } else {
        $("#TCGE-PUAAFinalSave").hide();
        $("#TCGE-PUAAFinalUpdate").show();
    }


    $("#TCGE-PUAnalyticAccount").modal("show");

    //update amount and description
    $("#TCGE-TAccountAnalytic").find("tr").each(function () {

        var tds = $(this).find('td');
        var disRowPercentage = tds.eq(3).text().replace('%', '');

        var disRowamount = parseFloat(disRowPercentage / 100) * parseFloat($("#TCGE-PUAAOriginalAmount").text().replace(regRemoveCurrFormate, ""));
        tds.eq(4).text(setSystemCurrFormate(+disRowamount));

    });

    CalcAnalyticAssUnAss();
}


//8.32 (F) Function that calculate Assign and UnAssign that exist in Analytic PopUp
function CalcAnalyticAssUnAss() {

    var sumAmountTblAnalyticDist = 0;
    $(".SumAmountTblAnalyticDist").each(function () {
        var value = parseFloat($(this).text().replace(regRemoveCurrFormate, ""));
        // add only if the value is number
        if (!isNaN(value) && value.length != 0) {
            sumAmountTblAnalyticDist += value;
        }
    });

    $("#TCGE-PUAAAssigned").text(setSystemCurrFormate(+parseFloat(sumAmountTblAnalyticDist)));

    var originalAmount = $("#TCGE-PUAAOriginalAmount").text().replace(regRemoveCurrFormate, "");

    //console.log(originalAmount);

    var unAssignedVal = parseFloat(originalAmount) - sumAmountTblAnalyticDist;

    $("#TCGE-PUAAUnassigned").text(setSystemCurrFormate(+parseFloat(unAssignedVal)));
    ForceRefreshThisPicker("#TCGE-PUAAccDisID");
}


//8.33 (F) Functions that call from table button icons that exist in analytic popup and pass id of row click
function PUAEditD(id) {

    //Calc assign and UnAssign
    CalcAnalyticAssUnAss();

    //filter analytic distribution
    $(".TCGE-TblDistID").each(function () {
        var result = $(this).text();
        $("#TCGE-PUAAccDisID option[value=" + result + "]").remove();
    });

    $("#TCGE-PUAAccDisID").focus();

    var tds = $(".Disrow_" + id + "").find('td');
    var distributionID = tds.eq(0).text();
    var distributionName = tds.eq(2).text();
    var percentage = tds.eq(3).text().replace('%', '');
    var amount = tds.eq(4).text().replace(regRemoveCurrFormate, "");
    var describtion = tds.eq(5).text();
    $("#TCGE-PUAAccDisID").append("<option value='" + id + "'>" + distributionID + "</option>");
    $("#TCGE-PUAAccDisID").val(id);
    $("#TCGE-PUAAccDisName").val(distributionName);
    $("#TCGE-PUAAccDisPercentage").val(percentage);
    $("#TCGE-PUAAccDisAmount").val(setSystemCurrFormate(+parseFloat(amount)));
    $("#TCGE-PUAADisDescribtion").val(describtion);


    var assigned = $("#TCGE-PUAAAssigned").text().replace(regRemoveCurrFormate, "");
    var unassigned = $("#TCGE-PUAAUnassigned").text().replace(regRemoveCurrFormate, "");

    //retrive amount of row to unassign value and minus it from assign value to update of this row
    var assval = parseFloat(assigned) - parseFloat(amount);
    var unass = parseFloat(unassigned) + parseFloat(amount);

    $("#TCGE-PUAAAssigned").text(setSystemCurrFormate(+assval));
    $("#TCGE-PUAAUnassigned").text(setSystemCurrFormate(+unass));

    //open update mode and close save mode
    $("#TCGE-PUAConfirmUpdateRecord").prop("disabled", false);
    $("#TCGE-PUACancelUpdateRecord").prop("disabled", false);
    $("#TCGE-PUAAddRow").prop("disabled", true);

    //set id of row need to update.. to use it confirm update btn
    $("#TCGE-UpdateDistID").text(id);
}


//-----------------------------------Functions Of CostCenter Part--------------------------------------------

//8.50 (F) Big Function that set data of costcenter popup in dropdown and tbl "Save Mode"
function SetDataInCCPopUp(debit, credit, accountIDTbl, accountName, costCenterType, checkCostCenter, checkMainCostCenter, accountID, describtion) {

    //Set Original amount and unassigned that exist in CC popup
    if (debit) {
        $("#TCGE-PUCCOriginalAmount").text(setSystemCurrFormate(+debit));
        $("#TCGE-PUCCUnassigned").text(setSystemCurrFormate(+debit));
    } else if (credit) {
        $("#TCGE-PUCCOriginalAmount").text(setSystemCurrFormate(+credit));
        $("#TCGE-PUCCUnassigned").text(setSystemCurrFormate(+credit));
    }

    //Set Main Data of Cost center popup
    $("#TCGE-PUCCAccID").text(accountIDTbl);
    $("#TCGE-CAID").val(accountID);
    $("#TCGE-PUCCAccName").text(accountName);

    //Show Final Save btn and hide update cause we in Save mode
    $("#TCGE-PUCCFinalSave").show();
    $("#TCGE-PUCCFinalUpdate").hide();

    //If Type Cost Center
    if (costCenterType === "CostCenter" && checkCostCenter.length > 0) {

        $("#TCGE-TblCostCenter").show();
        $("#TCGE-TblMainCostCenter").hide();

        //EmptyCostCenter $("#TCGE-TMainCostCenter").html("");

        //show Cost center ID in main data and hide Main cost center from Main Data
        $("#TCGE-PUCCHS").show();
        $("#TCGE-PUMCCHS").hide();

        //Hide Part Of Fields That related with main cost center
        $("#TCGE-PUCForMain").hide();

        if ($("#TCGE-SHPUCC").text() === "true") {

            OpenPUCCWithOutRefrech(debit, credit, costCenterType);

        } else {

            //Set Cost Center ID in main part of CC popup
            $("#TCGE-PUCCCostID").text(checkCostCenter);

            //ajax get cost center acounts
            $.ajax({
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                method: "get",
                url: "/api/TransactionApi/GetAccountsOfCostCenter?costCenterID=" + checkCostCenter,
                success: function (result) {

                    $("#TCGE-PUCostCenter").modal("show");
                    $("#TCGE-PUCCOstAccounts").empty();

                    if (result.length == 0) {

                        $("#TCGE-PUCCOstAccounts").append($('<option/>', {
                            value: "",
                            text: "No Accounts Created To This Cost Center"
                        })
                        );
                    } else {

                        $("#TCGE-PUCCOstAccounts").append($('<option/>', {
                            value: "",
                            text: ChooseTxt
                        })
                        );
                        $.each(result, function (index, row) {

                            $("#TCGE-PUCCOstAccounts").append("<option value='" + row.C_CAID + "'>" + row.C_CostAccountID + "</option>");

                        });

                        //Set Span true to know that this popup opened at least one
                        $("#TCGE-SHPUCC").text("true");

                        //ajax to get data of account to table if exist
                        $.ajax({
                            contentType: 'application/json; charset=utf-8',
                            dataType: 'json',
                            method: "get",
                            url: "/api/TransactionApi/CheckExistCostAccounts?c_AID=" + accountID + "&costCenterType=" + costCenterType,
                            success: function (result) {
                                if (result.length === 0) {

                                } else {
                                    for (var i = 0; i < result.length; i++) {
                                        //Filter Cost Center Accounts drop down from Table accounts data
                                        $("#TCGE-PUCCOstAccounts option[value=" + result[i].C_CAID2 + "]").remove();

                                        //get New Amount that multiply in ready data that exist in database
                                        var amount = parseFloat(result[i].Percentage / 100) * parseFloat($("#TCGE-PUCCOriginalAmount").text().replace(regRemoveCurrFormate, ""));

                                        //Function that add in Cost center tbl that exist in cost center popup
                                        AddToCostCenterTbl(result[i].C_CAID2, result[i].C_CostAccountID2, result[i].C_CostAccountName, result[i].Percentage, amount, describtion, costCenterType);
                                    }
                                    //calculate assign and unassign values after get data from database
                                    CalcCostCenterAssUnAss();
                                }
                            }
                        });

                    }
                }
            });

        }

        //else of Type Main Cost Center
    } else if (costCenterType === "MainCostCenter" && checkMainCostCenter.length > 0) {

        $("#TCGE-TblCostCenter").hide();
        $("#TCGE-TblMainCostCenter").show();

        $("#TCGE-TCostCenter").html("");

        ClearPUCCFieldsInMCCMode();

        //hide Cost center ID in main data and show Main cost center from Main Data
        $("#TCGE-PUMCCHS").show();
        $("#TCGE-PUCCHS").hide();

        //show Part Of Fields That related with main cost center
        $("#TCGE-PUCForMain").show();

        if ($("#TCGE-SHPUCC").text() === "true") {

            OpenPUCCWithOutRefrech(debit, credit, costCenterType);

        } else {

            //Set Main Cost Center ID in main part of CC popup
            $("#TCGE-PUCCMainCostID").text(checkMainCostCenter);

            $.ajax({
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                method: "get",
                url: "/api/TransactionApi/GetCostCenterIDFromMain?mainCostCenter=" + checkMainCostCenter,
                success: function (result) {

                    $("#TCGE-PUCostCenter").modal("show");


                    $("#TCGE-PUCDropCostCenterForMain").empty();
                    if (result.length == 0) {

                        $("#TCGE-PUCDropCostCenterForMain").append($('<option/>', {
                            value: "",
                            text: "No Cost Center ID inserted to This Group"
                        })
                        );
                    } else {

                        $("#TCGE-PUCDropCostCenterForMain").append($('<option/>', {
                            value: "",
                            text: ChooseTxt
                        })
                        );
                        $.each(result, function (index, row) {

                            $("#TCGE-PUCDropCostCenterForMain").append("<option value='" + row.C_CostCenterID + "'>" + row.C_CostCenterID + "</option>");

                        });

                        //Set span by true to know that this popup opened at least one
                        $("#TCGE-SHPUCC").text("true");

                        $.ajax({
                            contentType: 'application/json; charset=utf-8',
                            dataType: 'json',
                            method: "get",
                            url: "/api/TransactionApi/CheckExistCostAccounts?c_AID=" + accountID + "&costCenterType=" + costCenterType,
                            success: function (result) {
                                if (result.length === 0) {

                                } else {
                                    for (var i = 0; i < result.length; i++) {
                                        var cCIDAmount = parseFloat(result[i].CostCenterIDPercentage / 100) * parseFloat($("#TCGE-PUCCOriginalAmount").text().replace(regRemoveCurrFormate, ""));

                                        var amount = parseFloat(result[i].Percentage / 100) * parseFloat(cCIDAmount);

                                        AddToCostCenterTbl(result[i].C_CAID2, result[i].C_CostAccountID2, result[i].C_CostAccountName, result[i].Percentage, amount, describtion, costCenterType, result[i].C_CostCenterID, result[i].CostCenterIDPercentage, result[i].C_CostCenterName);
                                    }
                                    CalcCostCenterAssUnAss();
                                }
                            }
                        });
                    }
                }
            });
        }
    }
    CalcCostCenterAssUnAss();

}


//8.51 (F) Function That add data to cost center tbl that exist in Cost center popUp
function AddToCostCenterTbl(c_CAID, CostAccountID, CostAccountName, percentage, amount, describtion, costCenterType, costCenterID, costCenterIDPercentage, costCenterName) {

    if (costCenterType === "CostCenter") {

        var CostContent = "<tr>" +
            "<td class='hide-normal TCGE-TblCCAccID'>" + c_CAID + "</td>" +
            "<td>" + CostAccountID + "</td>" +
            "<td>" + CostAccountName + "</td>" +
            "<td>" + percentage + "%" + "</td>" +
            "<td class='SumAmountTblCostAccount'>" + setSystemCurrFormate(+amount) + "</td>" +
            "<td>" + describtion + "</td>" +
            "<td class='hide-normal'>" + costCenterType + "</td>" +
            "<td>" + '<button type="button" class="btn btn-danger btn-sm mr-1 PUCDeleteA"><span class="fa fa-trash-o"></span></button>' + '<button type="button" class="btn btn-warning btn-sm PUCEditA"><span class="fa fa-edit"></span></button>' + "</td>" +
            "</tr>";
        $("#TCGE-TCostCenter").append(CostContent);

    } else if (costCenterType === "MainCostCenter") {

        var CostContent = "<tr>" +
            "<td class='hide-normal TCGE-TblCCAccID'>" + c_CAID + "</td>" +
            "<td>" + costCenterID + "</td>" +
            "<td class='hide-normal'>" + costCenterIDPercentage + "</td>" +
            "<td>" + CostAccountID + "</td>" +
            "<td>" + CostAccountName + "</td>" +
            "<td>" + percentage + "%" + "</td>" +
            "<td class='SumAmountTblCostAccount'>" + setSystemCurrFormate(+amount) + "</td>" +
            "<td>" + describtion + "</td>" +
            "<td class='hide-normal'>" + costCenterType + "</td>" +
            "<td class='hide-normal'>" + costCenterName + "</td>" +
            "<td>" + '<button type="button" class="btn btn-danger btn-sm mr-1 PUCDeleteA"><span class="fa fa-trash-o"></span></button>' + '<button type="button" class="btn btn-warning btn-sm PUCEditA"><span class="fa fa-edit"></span></button>' + "</td>" +
            "</tr>";
        $("#TCGE-TMainCostCenter").append(CostContent);

    }

}


//8.52 (F) Function That open Cost center popup without go to database when popup opened at least one and update only amount by D OR C
function OpenPUCCWithOutRefrech(debit, credit, costCenterType) {

    $("#TCGE-PUCCOstAccounts").val("");
    $("#TCGE-PUCCOstAccountName").val("");
    $("#TCGE-PUCCostAccPercentage").val("");
    $("#TCGE-PUCCostAccAmount").val("");
    $("#TCGE-PUCCostAccDescribtion").val("");
    $("#TCGE-PUCGlobalError").text("");

    //set original amount in pop up
    if (debit) {
        $("#TCGE-PUCCOriginalAmount").text(setSystemCurrFormate(+debit));
        $("#TCGE-PUCCUnassigned").text(setSystemCurrFormate(+debit));
    } else if (credit) {
        $("#TCGE-PUCCOriginalAmount").text(setSystemCurrFormate(+credit));
        $("#TCGE-PUCCUnassigned").text(setSystemCurrFormate(+credit));
    }
    //----------------------------------------------

    if ($("#TCGE-BtnAddRow").is(":enabled")) {
        $("#TCGE-PUCCFinalSave").show();
        $("#TCGE-PUCCFinalUpdate").hide();
    } else {
        $("#TCGE-PUCCFinalSave").hide();
        $("#TCGE-PUCCFinalUpdate").show();
    }

    $("#TCGE-PUCostCenter").modal("show");

    if (costCenterType === "CostCenter") {

        //update amount and description
        $("#TCGE-TCostCenter").find("tr").each(function () {

            var tds = $(this).find('td');
            var accRowPercentage = tds.eq(3).text().replace('%', '');

            var accRowamount = parseFloat(accRowPercentage / 100) * parseFloat($("#TCGE-PUCCOriginalAmount").text().replace(regRemoveCurrFormate, ""));
            tds.eq(4).text(setSystemCurrFormate(+accRowamount));

        });

    } else if (costCenterType === "MainCostCenter") {

        //update amount and description
        $("#TCGE-TMainCostCenter").find("tr").each(function () {

            var tds = $(this).find('td');
            var costCenterIDPercentage = tds.eq(2).text().replace('%', '');
            var accRowPercentage = tds.eq(5).text().replace('%', '');

            var cCIDAmount = parseFloat(costCenterIDPercentage / 100) * parseFloat($("#TCGE-PUCCOriginalAmount").text().replace(regRemoveCurrFormate, ""));


            var amount = parseFloat(accRowPercentage / 100) * parseFloat(cCIDAmount);

            tds.eq(6).text(setSystemCurrFormate(+amount));

        });

    }

    CalcCostCenterAssUnAss();
}


//8.53 (F) Function that validate fields of cost center popup before add and update row
function CheckBeforeAUCCAccount(c_CAID, percentage, amount, describtion) {

    var test = true;

    if (c_CAID.length === 0) {
        $("#TCGE-PUCCOstAccounts").css("border-color", "red");
        test = false;
    } else {
        $("#TCGE-PUCCOstAccounts").css("border-color", "");
    }

    if (percentage.length === 0) {
        $("#TCGE-PUCCostAccPercentage").css("border-color", "red");
        test = false;
    } else if (percentage == 0) {
        $("#TCGE-PUCCostAccPercentage").css("border-color", "red");
        test = false;
    } else if (parseFloat(percentage) > 100) {
        $("#TCGE-PUCCostAccPercentage").css("border-color", "red");
        test = false;
    } else {
        $("#TCGE-PUCCostAccPercentage").css("border-color", "");
    }

    if (amount.length === 0) {
        $("#TCGE-PUCCostAccAmount").css("border-color", "red");
        test = false;
    } else if (amount == 0) {
        $("#TCGE-PUCCostAccAmount").css("border-color", "red");
        test = false;
    } else {
        $("#TCGE-PUCCostAccAmount").css("border-color", "");
    }

    if (describtion.length === 0) {
        $("#TCGE-PUCCostAccDescribtion").css("border-color", "red");
        test = false;
    } else {
        $("#TCGE-PUCCostAccDescribtion").css("border-color", "");
    }

    if ($("#TCGE-PUCForMain").is(":visible")) {

        var recentlyAmount = $("#TCGE-PUCCostAccAmount").val().replace(regRemoveCurrFormate, "");

        var costCenterUnassigned = GetMaskNumber($("#TCGE-PUCCUnassigned").text()) /*$("#TCGE-PUCCCFMUnassign").text().replace(regRemoveCurrFormate, "")*/;

        var rOfAmountCCUnass = parseFloat(costCenterUnassigned - recentlyAmount);
        if (rOfAmountCCUnass < 0) {
            $("#TCGE-PUCGlobalError").text("Failed amount..!");
            test = false;
        } else {
            $("#TCGE-PUCGlobalError").text("");
        }

    } else {

        var unassigned = $("#TCGE-PUCCUnassigned").text().replace(regRemoveCurrFormate, "");

        var rOfAmountMUnass = parseFloat(unassigned - amount);
        if (rOfAmountMUnass < 0) {
            $("#TCGE-PUCGlobalError").text("Failed amount..!");
            test = false;
        } else {
            $("#TCGE-PUCGlobalError").text("");
        }
    }
    if (test === true) {
        return true;
    } else {
        return false;
    }

}


//8.54 (F) Function that add data to cost center database table
function SaveUpdateCostAccTble(costORMainCenterID, accountID, debit, credit, id, costCenterType) {
    $("#TCGE-AccountID").prop("disabled", false);
    // $(GetJvRow(accountID))
    var CC = [];
    if (costCenterType === "CostCenter") {

        $("#TCGE-TCostCenter").find('tr').each(function () {

            var tds = $(this).find('td'),
                c_CAID = tds.eq(0).text(),
                costAccountID = tds.eq(1).text(),
                costAccountName = tds.eq(2).text(),
                percentage = tds.eq(3).text(),
                amount = tds.eq(4).text(),
                cAccdescribtion = tds.eq(5).text(),
                costCenterType = tds.eq(6).text();

            var ThisCC = {
                Describtion: cAccdescribtion,
                C_AID: accountID,
                C_CostCenterID: $(document).find("#TCGE-PUCCCostID").text(),
                C_CAID: tds.eq(0).text(),
                C_Amount: GetMaskNumber(tds.eq(4).text()),
                C_Debit: GetMaskNumber(debit),
                C_Credit: GetMaskNumber(credit),
                C_Percentage: parseFloat(percentage),
                CostCenterPercentage: parseFloat(percentage)
            };

            CC.push(ThisCC);

            var costCenterDBContent = "<tr class='CCAccDBrow_" + id + "'>" +
                "<td>" + costORMainCenterID + "</td>" +
                "<td>" + c_CAID + "</td>" +
                "<td>" + costAccountID + "</td>" +
                "<td>" + costAccountName + "</td>" +
                "<td>" + accountID + "</td>" +
                "<td>" + cAccdescribtion + "</td>" +
                "<td>" + percentage + "</td>" +
                "<td>" + amount + "</td>" +
                "<td>" + debit + "</td>" +
                "<td>" + credit + "</td>" +
                "<td>" + costCenterType + "</td>" +
                "<td>" + "" + "</td>" +
                "<td>" + "" + "</td>" +
                "<td>" + "" + "</td>" +
                "</tr>";

            $("#TCGE-TCostCenterAccountDB").append(costCenterDBContent);
        });

    } else if (costCenterType === "MainCostCenter") {

        $("#TCGE-TMainCostCenter").find('tr').each(function () {

            var tds = $(this).find('td'),
                c_CAID = tds.eq(0).text(),
                costCenterIDFMain = tds.eq(1).text(),
                percentageFMain = tds.eq(2).text(),
                costAccountID = tds.eq(3).text(),
                costAccountName = tds.eq(4).text(),
                percentage = tds.eq(5).text(),
                amount = tds.eq(6).text(),
                cAccdescribtion = tds.eq(7).text(),
                costCenterType = tds.eq(8).text(),
                costCenterName = tds.eq(9).text();

            var ThisCC = {
                C_AID: accountID,
                Describtion: cAccdescribtion,
                C_CostCenterID: costCenterIDFMain,
                C_CAID: tds.eq(0).text(),
                C_Amount: GetMaskNumber(tds.eq(6).text()),
                C_Debit: debit,
                C_Credit: credit,
                C_Percentage: parseFloat(tds.eq(5).text()),
                C_CostCenterGroupID: costORMainCenterID,
                CostCenterPercentage: parseFloat(percentageFMain)
            };
            CC.push(ThisCC);

            var costCenterDBContent = "<tr class='CCAccDBrow_" + id + "'>" +
                "<td>" + costCenterIDFMain + "</td>" +
                "<td>" + c_CAID + "</td>" +
                "<td>" + costAccountID + "</td>" +
                "<td>" + costAccountName + "</td>" +
                "<td>" + accountID + "</td>" +
                "<td>" + cAccdescribtion + "</td>" +
                "<td>" + percentage + "</td>" +
                "<td>" + amount + "</td>" +
                "<td>" + debit + "</td>" +
                "<td>" + credit + "</td>" +
                "<td>" + costCenterType + "</td>" +
                "<td>" + costORMainCenterID + "</td>" +
                "<td>" + percentageFMain + "</td>" +
                "<td>" + costCenterName + "</td>" +
                "</tr>";

            $("#TCGE-TCostCenterAccountDB").append(costCenterDBContent);
        });

    }
    $(GetJvRow(accountID)).attr("data-ccvalue", JSON.stringify(CC));
}


//8.55 (F) Big Function that set data of costcenter popup in dropdown and tbl "Update Mode"
function SetDataInCCPUToUpdate(id, debit, credit, accountIDTbl, accountName) {

    //Get Cost center id and type of it from cost center DB table
    var getCCDBTbl = $(".CCAccDBrow_" + id + "").find('td');
    var costCenterID = getCCDBTbl.eq(0).text();
    var costCenterType = getCCDBTbl.eq(10).text();
    var mainCostCenterID = getCCDBTbl.eq(11).text();

    $("#TCGE-PUCCOstAccounts").val("");
    $("#TCGE-PUCCOstAccountName").val("");
    $("#TCGE-PUCCostAccPercentage").val("");
    $("#TCGE-PUCCostAccAmount").val("");
    $("#TCGE-PUCCostAccDescribtion").val("");
    $("#TCGE-PUCGlobalError").text("");

    //Set original amount and unassigned value
    if (debit.length > 0) {
        $("#TCGE-PUCCOriginalAmount").text(setSystemCurrFormate(+debit));
        $("#TCGE-PUCCUnassigned").text(setSystemCurrFormate(+debit));
    } else if (credit.length > 0) {
        $("#TCGE-PUCCOriginalAmount").text(setSystemCurrFormate(+credit));
        $("#TCGE-PUCCUnassigned").text(setSystemCurrFormate(+credit));
    }

    //set main data of Cost Center Popup
    $("#TCGE-PUCCAccID").text(accountIDTbl);
    $("#TCGE-PUCCAccName").text(accountName);


    if (costCenterType === "CostCenter") {

        $("#TCGE-TblCostCenter").show();
        $("#TCGE-TblMainCostCenter").hide();

        //EmptyCostCenter $("#TCGE-TMainCostCenter").html("");

        //show Cost center ID in main data and hide Main cost center from Main Data
        $("#TCGE-PUCCHS").show();
        $("#TCGE-PUMCCHS").hide();

        //Hide Part Of Fields That related with main cost center
        $("#TCGE-PUCForMain").hide();

        //If Cost center Pop Up opened at least one
        if ($("#TCGE-SHPUCC").text() === "true") {

            //Function that open cost center popup without refresh data only in debit and credit
            OpenPUCCWithOutRefrech(debit, credit, costCenterType);

        } else {

            $("#TCGE-TCostCenter").html("");

            $("#TCGE-PUCCCostID").text(costCenterID);

            //ajax get cost center accounts
            $.ajax({
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                method: "get",
                url: "/api/TransactionApi/GetAccountsOfCostCenter?costCenterID=" + costCenterID,
                success: function (result) {

                    $("#TCGE-PUCCFinalSave").hide();
                    $("#TCGE-PUCCFinalUpdate").show();
                    $("#TCGE-PUCostCenter").modal("show");

                    $("#TCGE-PUCCOstAccounts").empty();

                    if (result.length == 0) {

                        $("#TCGE-PUCCOstAccounts").append($('<option/>', {
                            value: "",
                            text: "No Accounts Created To This Cost Center"
                        })
                        );
                    } else {

                        $("#TCGE-PUCCOstAccounts").append($('<option/>', {
                            value: "",
                            text: "ChooseTxt"
                        })
                        );
                        $.each(result, function (index, row) {

                            $("#TCGE-PUCCOstAccounts").append("<option value='" + row.C_CAID + "'>" + row.C_CostAccountID + "</option>");

                        });

                        //Set span by true to know that this popup opened at least one
                        $("#TCGE-SHPUCC").text("true");

                        //Get table data from cost center database table
                        $("#TCGE-TCostCenterAccountDB").find(".CCAccDBrow_" + id + "").each(function () {

                            var tds = $(this).find('td'),
                                c_CAID = tds.eq(1).text(),
                                costAccountID = tds.eq(2).text(),
                                costAccountName = tds.eq(3).text(),
                                describtion = tds.eq(5).text(),
                                percentage = tds.eq(6).text().replace('%', ''),
                                amount = tds.eq(7).text(),
                                costCenterType = tds.eq(10).text();


                            //filter cost accounts from table accounts data
                            $("#TCGE-PUCCOstAccounts option[value=" + c_CAID + "]").remove();

                            //get new amount
                            var newAmount = parseFloat(percentage / 100) * parseFloat($("#TCGE-PUCCOriginalAmount").text().replace(regRemoveCurrFormate, ""));

                            //Function that add data to cost center table that exist in CC popup
                            AddToCostCenterTbl(c_CAID, costAccountID, costAccountName, percentage, newAmount, describtion, costCenterType);

                        });
                        //Calc assign and unassign values after get data if happen update in amount
                        CalcCostCenterAssUnAss();
                    }
                }
            });
        }

    } else if (costCenterType === "MainCostCenter") {

        $("#TCGE-TblCostCenter").hide();
        $("#TCGE-TblMainCostCenter").show();

        $("#TCGE-TCostCenter").html("");

        ClearPUCCFieldsInMCCMode();

        //hide Cost center ID in main data and show Main cost center from Main Data
        $("#TCGE-PUCCHS").hide();
        $("#TCGE-PUMCCHS").show();

        //show Part Of Fields That related with main cost center
        $("#TCGE-PUCForMain").show();

        //If Cost center Pop Up opened at least one
        if ($("#TCGE-SHPUCC").text() === "true") {

            //Function that open cost center popup without refresh data only in debit and credit
            OpenPUCCWithOutRefrech(debit, credit, costCenterType);

        } else {

            //EmptyCostCenter $("#TCGE-TMainCostCenter").html("");

            $("#TCGE-PUCCMainCostID").text(mainCostCenterID);

            $.ajax({
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                method: "get",
                url: "/api/TransactionApi/GetCostCenterIDFromMain?mainCostCenter=" + mainCostCenterID,
                success: function (result) {

                    $("#TCGE-PUCCFinalSave").hide();
                    $("#TCGE-PUCCFinalUpdate").show();
                    $("#TCGE-PUCostCenter").modal("show");

                    $("#TCGE-PUCDropCostCenterForMain").empty();

                    if (result.length == 0) {

                        $("#TCGE-PUCDropCostCenterForMain").append($('<option/>', {
                            value: "",
                            text: "No Cost Center ID inserted to This Group"
                        })
                        );
                    } else {

                        $("#TCGE-PUCDropCostCenterForMain").append($('<option/>', {
                            value: "",
                            text: "ChooseTxt"
                        })
                        );
                        $.each(result, function (index, row) {

                            $("#TCGE-PUCDropCostCenterForMain").append("<option value='" + row.C_CostCenterID + "'>" + row.C_CostCenterID + "</option>");

                        });

                        //Set span by true to know that this popup opened at least one
                        $("#TCGE-SHPUCC").text("true");

                        //Get table data from cost center database table
                        $("#TCGE-TCostCenterAccountDB").find(".CCAccDBrow_" + id + "").each(function () {

                            var tds = $(this).find('td'),
                                costCenterID = tds.eq(0).text(),
                                c_CAID = tds.eq(1).text(),
                                costAccountID = tds.eq(2).text(),
                                costAccountName = tds.eq(3).text(),
                                describtion = tds.eq(5).text(),
                                percentage = tds.eq(6).text().replace('%', ''),
                                amount = tds.eq(7).text(),
                                costCenterType = tds.eq(10).text(),
                                cCIDPercentage = tds.eq(12).text().replace('%', ''),
                                costCenterName = tds.eq(13).text();


                            var cCIDAmount = parseFloat(cCIDPercentage / 100) * parseFloat($("#TCGE-PUCCOriginalAmount").text().replace(regRemoveCurrFormate, ""));

                            var newAmount = parseFloat(percentage / 100) * parseFloat(cCIDAmount);

                            //Function that add data to cost center table that exist in CC popup
                            AddToCostCenterTbl(c_CAID, costAccountID, costAccountName, percentage, newAmount, describtion, costCenterType, costCenterID, cCIDPercentage, costCenterName);

                        });

                        //Calc assign and unassign values after get data if happen update in amount
                        CalcCostCenterAssUnAss();
                    }
                }
            });

        }

    }
}


//8.56 (F) Function That Calculate Assign and Unassign that exist in Cost center popup
function CalcCostCenterAssUnAss() {
    var sumAmountTblCostAccount = 0;
    $(".SumAmountTblCostAccount").each(function () {
        var value = parseFloat($(this).text().replace(regRemoveCurrFormate, ""));
        // add only if the value is number
        if (!isNaN(value) && value.length != 0) {
            sumAmountTblCostAccount += value;
        }
    });

    $("#TCGE-PUCCAssigned").text(setSystemCurrFormate(+parseFloat(sumAmountTblCostAccount)));

    var unassignVal = parseFloat($("#TCGE-PUCCOriginalAmount").text().replace(regRemoveCurrFormate, "")) - parseFloat(sumAmountTblCostAccount);

    $("#TCGE-PUCCUnassigned").text(setSystemCurrFormate(+parseFloat(unassignVal)));

}


//8.57 (F) Function To clear Field and data In Main Cost Center Mode
function ClearPUCCFieldsInMCCMode() {

    var SumOfPer = 0;

    $("#TCGE-TMainCostCenter tr").each(function () {
        if ($(this).find("td").eq(1).text() == $("#TCGE-PUCDropCostCenterForMain").val()) {
            SumOfPer += parseFloat($(this).find("td").eq(2).text())
        }
    })

    if ($("#TCGE-PUCCOstAccounts").is(":visible") && SumOfPer == 100) {
        $("#TCGE-PUCDropCostCenterForMain").val("");
        $("#TCGE-PUCDropCostCenterNameForMain").val("");
        $("#TCGE-PUCCostCenterPercentageForMain").val("");
        $("#TCGE-PUCCostCenterAmountForMain").val("");
        $("#TCGE-PUCCCFMUnassign").text("");
        $("#TCGE-PUCCCFMAssign").text("");
        ForceRefreshThisPicker("#TCGE-PUCDropCostCenterForMain");
        $("#TCGE-PUCCOstAccounts").empty();
        $("#TCGE-PUCCOstAccounts").append($('<option/>', {
            value: "",
            text: "ChooseTxt"
        })
        );
    } else if (!$("#TCGE-PUCCOstAccounts").is(":visible")) {
        $("#TCGE-PUCDropCostCenterForMain").val("");
        $("#TCGE-PUCDropCostCenterNameForMain").val("");
        $("#TCGE-PUCCostCenterPercentageForMain").val("");
        $("#TCGE-PUCCostCenterAmountForMain").val("");
        $("#TCGE-PUCCCFMUnassign").text("");
        $("#TCGE-PUCCCFMAssign").text("");
        ForceRefreshThisPicker("#TCGE-PUCDropCostCenterForMain");
        $("#TCGE-PUCCOstAccounts").empty();
        $("#TCGE-PUCCOstAccounts").append($('<option/>', {
            value: "",
            text: "ChooseTxt"
        })
        );
    }
    ForceRefreshThisPicker("#TCGE-PUCCOstAccounts");
    $("#TCGE-PUCCOstAccountName").val("");
    $("#TCGE-PUCCostAccPercentage").val("");
    $("#TCGE-PUCCostAccAmount").val("");
    $("#TCGE-PUCCostAccDescribtion").val("");


}


//8.58 (F) Function That Calculate Assign and Unassign of Cost Center ID that exist in cost center popup
function CalcMainCostCenterAssUnass() {

    var costCenterID = $("#TCGE-PUCDropCostCenterForMain").val();
    var originalAmount = $("#TCGE-PUCCostCenterAmountForMain").val().replace(regRemoveCurrFormate, "");

    var sumAmountOfSpecificCC = 0;
    $("#TCGE-TMainCostCenter").find("tr").each(function () {

        var tds = $(this).find('td');
        var costCenterIDFromMain = tds.eq(1).text();

        if (costCenterIDFromMain === costCenterID) {

            var value = parseFloat(tds.eq(6).text().replace(regRemoveCurrFormate, ""));
            // add only if the value is number
            if (!isNaN(value) && value.length != 0) {
                sumAmountOfSpecificCC += value;
            }
        }
    });

    $("#TCGE-PUCCCFMAssign").text(setSystemCurrFormate(+parseFloat(sumAmountOfSpecificCC)));
    $("#TCGE-PUCCCFMUnassign").text(setSystemCurrFormate(+parseFloat(originalAmount - sumAmountOfSpecificCC)));

}

//----------------------------------------------------------------------------------------------------------------


// Function To Get Currency Rates
function GetCurrencyRates(currencyID, postingDate, Selector) {

    var companyID = $("#TCGE-CompanyID").text();

    var iso = $("#TCGE-CurrencyID option:selected").text();
    var isoTransfer = $(Selector).find(".TCGE-CurrencyID option:selected").text();

    //var decimalNumber = $("#TCGE-DecimalNumber").text();

    selector = Selector;

    if (currencyID === companyID) {

        $("#TCGE-SystemRate").val("1");
        $("#TCGE-TransactionRate").val("1");
        $("#TCGE-TransactionRate").prop("disabled", true);
        $("#TCGE-DiffrenceRate").val("0");
        $("#TCGE-DiffrenceRate").prop("disabled", true);

        // this content for checkbook transfer
        $(Selector).find(".TCGE-SystemRate").val("1");
        $(Selector).find(".TCGE-TransactionRate").val("1");
        $(Selector).find(".TCGE-TransactionRate").prop("disabled", true);
        $(Selector).find(".TCGE-DiffrenceRate").val("0");
        $(Selector).find(".TCGE-DiffrenceRate").prop("disabled", true);

        // this content for Bank-Reconcile
        $(Selector).find('td:eq(0)').find('input').val("1");

        // this content for Checkbook-Revaluate
        if (location.pathname == "/CheckbookRevaluate/Revaluate") {
            $(Selector).find('td:eq(4)').find('.TCGE-SystemRate').val("1");
        }


        $(".TCGE-HSOAByC").hide();
        $("#TCGE-Debit").prop("disabled", false);
        $("#TCGE-Credit").prop("disabled", false);
        $('#TCCR-rateField').hide();

        // this content for checkbook transfer
        $(Selector).find('.TCCR-rateField').hide();
        $(Selector).find(".CBT-amount").maskMoney({ prefix: '' + prefix + '', suffix: '' + suffix + '', thousands: '' + thousands + '', decimal: '' + decimal + '', precision: parseInt(requiredDecimalNum) });

        $("#CBT-amount").maskMoney({ prefix: '' + prefix + '', suffix: '' + suffix + '', thousands: '' + thousands + '', decimal: '' + decimal + '', precision: parseInt(requiredDecimalNum) });

        // this content for tax transaction
        $("#TCT-unitPrice").maskMoney({ prefix: '' + prefix + '', suffix: '' + suffix + '', thousands: '' + thousands + '', decimal: '' + decimal + '', precision: parseInt(requiredDecimalNum) });

        // this content for check transfer
        if (location.pathname == "/C_CheckTransfer/CompanyCheckTransfer") {
            $("#TCT-transactionRate").val("1");
        }

    } else {
        $('#TCCR-rateField').show();

        // this content for checkbook transfer
        $(Selector).find('.TCCR-rateField').show();
        $(Selector).find(".CBT-amount").maskMoney({ suffix: ' ' + isoTransfer + '', thousands: ',', decimal: '.', precision: parseInt(requiredDecimalNum) });
        $("#TCGE-HardGurrencyFormateTransfer").maskMoney({ suffix: ' ' + isoTransfer + '', thousands: ',', decimal: '.', precision: parseInt(requiredDecimalNum) });


        $("#TCGE-OriginalAmount").maskMoney({ suffix: ' ' + iso + '', thousands: ',', decimal: '.', precision: parseInt(requiredDecimalNum) });
        $("#CBT-amount").maskMoney({ suffix: ' ' + iso + '', thousands: ',', decimal: '.', precision: parseInt(requiredDecimalNum) });
        $("#TCGE-HardGurrencyFormate").maskMoney({ suffix: ' ' + iso + '', thousands: ',', decimal: '.', precision: parseInt(requiredDecimalNum) });

        //this content for tax transaction
        $("#TCT-unitPrice").maskMoney({ suffix: ' ' + iso + '', thousands: ',', decimal: '.', precision: parseInt(requiredDecimalNum) });

        $(".TCGE-HSOAByC").show();
        $("#TCGE-Debit").prop("disabled", true);
        $("#TCGE-Credit").prop("disabled", true);


        $.ajax({
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            method: "get",
            //async: false,
            url: "/api/TransactionApi/GetExchangeCurrencyRate?currencyID=" + currencyID + "&postingDate=" + postingDate,
            success: function (result) {

                if (result.length === 0) {
                    $("#TCGE-SystemRate").val("");
                    $("#TCGE-TransactionRate").val("");
                    $("#TCGE-DiffrenceRate").val("");
                    $("#TCGE-TransactionRate").prop("disabled", true);
                    $("#TCGE-DiffrenceRate").prop("disabled", true);

                    // this content for checkbook transfer
                    $(Selector).find(".TCGE-SystemRate").val("");
                    $(Selector).find(".TCGE-TransactionRate").val("");
                    $(Selector).find(".TCGE-DiffrenceRate").val("");
                    $(Selector).find(".TCGE-TransactionRate").prop("disabled", true);
                    $(Selector).find(".TCGE-DiffrenceRate").prop("disabled", true);

                    // this content for Bank-Reconcile
                    $(Selector).find('td:eq(0)').find('input').val("");

                    // this content for Checkbook-Revaluate
                    if (location.pathname == "/CheckbookRevaluate/Revaluate") {
                        $(Selector).find('td:eq(4)').find('.TCGE-SystemRate').val("");
                    }

                    // this content for check-transfer
                    if (location.pathname == "/C_CheckTransfer/CompanyCheckTransfer") {
                        $("#TCT-transactionRate").val("");
                    }

                    $("#TCGE-GlobalError").text("No Rates In This Posting Date..!");
                    // $("#TCGE-CurrencyID").val("");
                } else {
                    var numberOfRates = 0;
                    for (var i = 0; i < result.length; i++) {
                        if (result[i].Rate) {
                            numberOfRates++;
                        }
                    }
                    //There Exist On Rate To This Currency
                    if (numberOfRates === 1) {
                        //Empty Old Data If Exist
                        TCGE_ClearRates(Selector);
                        for (var i = 0; i < result.length; i++) {
                            $("#TCGE-SystemRate").val(setSystemCurrFormate(+result[i].Rate));
                            $("#TCGE-TransactionRate").val(setSystemCurrFormate(+result[i].Rate));
                            $("#TCGE-DiffrenceRate").val("0");

                            // this content for checkbook transfer
                            $(Selector).find(".TCGE-SystemRate").val(setSystemCurrFormate(+result[i].Rate));
                            $(Selector).find(".TCGE-TransactionRate").val(setSystemCurrFormate(+result[i].Rate));
                            $(Selector).find(".TCGE-DiffrenceRate").val("0");

                            // this content for Bank-Reconcile
                            $(Selector).find('td:eq(0)').find('input').val(setSystemCurrFormate(+result[i].Rate));

                            // this content for Checkbook-Revaluate
                            if (location.pathname == "/CheckbookRevaluate/Revaluate") {
                                $(Selector).find('td:eq(4)').find('.TCGE-SystemRate').val(setSystemCurrFormate(+result[i].Rate));
                            }

                            // this content for check-transfer
                            if (location.pathname == "/C_CheckTransfer/CompanyCheckTransfer") {
                                $("#TCT-transactionRate").val(setSystemCurrFormate(+result[i].Rate));
                            }
                        }
                        //There Exist Several Rates To This Currency Will Appear it In Pop Up
                    } else {
                        $("#TCGE-SeveralSRate").empty();
                        $("#TCGE-SeveralSRate").append($('<option/>', {
                            value: "",
                            text: "ChooseTxt"
                        })
                        );
                        $.each(result, function (index, row) {
                            $("#TCGE-SeveralSRate").append("<option value='" + row.Rate + "'>" + row.Rate + "</option>");
                        });
                        $("#TCGE-PUSeveralRates").modal("show");
                    }
                }
            }
        });
    }
    return true;
}

// Function To check Dates in Open Period or Not
function CheckPostingDateInPeriods(postingDate, CallBack = null, ErrorCallBack = null) {
    const dateformat = /^\d{4}-\d{2}-\d{2}$/;
    var companyID = $("#TCGE-CompanyID").text();
    if (!postingDate.match(dateformat)) {
        $("#TCGE-PostingDate").css("border-color", "red");
        $("#TCGE-GlobalError").text("Wrong Date Formate..!");
        return false;
    } else {
        $("#TCGE-PostingDate").css("border-color", "");
        $("#TCGE-GlobalError").text("");
        var test = true;
        $.ajax({
            contentType: 'application/json; charset=utf-8',
            // dataType: 'json',
            method: "get",
            url: "/api/TransactionApi/CheckDateInPeriods?rDate=" + postingDate + "&companyID=" + companyID + "&AreaName=" + $("#AreaName").text(),
            success: function (result, statusText, xhr) {
                $("#TCGE-PostingDate").css("border-color", "");
                $("#TCGE-GlobalError").text("");
                test = true;
                if (CallBack != null) {
                    CallBack(test);
                }
            },
            error(xhr, status, text) {
                if (xhr.status == 404) {
                    $("#TCGE-PostingDate").css("border-color", "red");
                    $("#TCGE-GlobalError").text("Posting Date Not Valid..!");
                    test = false;
                } else if (xhr.status == 406) {
                    $("#TCGE-PostingDate").css("border-color", "red");
                    $("#TCGE-GlobalError").text("Posting Date In Close Period..!");
                    test = false;
                }
                if (ErrorCallBack != null) {
                    ErrorCallBack(xhr.status);
                }
            }
        });
        //return test;
    }
}

// Function To Save or Post Data of Transactions
function InsertTransactionData(companyID, inputType, postingDate, transactionDate, refrence, currencyID, systemRate, transactionRate, postingKey, transactionType
    , batchID, voidPostingNum, CallBack, ErrorCallBack, AlertError, NoDataCallBack = null, PrintJv = false, PrintJvAfterAjax = false) {

    var postingNumber = 0;
    var rowCount = $('#TCGE-GTbl >tbody >tr').length;
    if (rowCount === 0) {
        $("#TCGE-GlobalError").text("No Data To Save it..!");
        if (NoDataCallBack != null) {
            NoDataCallBack();
        }
    } else {
        var difference = parseFloat($("#DiffOfDAC").text().replace(regRemoveCurrFormate, ""));

        if (difference != 0) {
            $("#TCGE-GlobalError").text("Difference Between Debit And Credit Must Be equal 0");
            if (AlertError) {
                Talert("Difference Between Debit And Credit Must Be equal 0")
            }
        } else {
            var Test = true;
            var headerObj = {
                C_CBID: batchID,
                C_PostingDate: postingDate,
                C_TransactionDate: transactionDate,
                C_Refrence: refrence,
                CurrencyID: currencyID,
                C_SystemRate: systemRate.replace(regRemoveCurrFormate, ""),
                C_TransactionRate: transactionRate.replace(regRemoveCurrFormate, ""),
                C_PostingKey: postingKey,
                C_TransactionType: transactionType
            };

            var mainArr = [];
            $.each($("#TCGE-GTbl tbody tr"), function () {
                mainArr.push({
                    C_Describtion: $(this).find('td:eq(5)').html(),
                    C_Document: $(this).find('td:eq(3)').html(),
                    C_AID: $(this).find('td:eq(1)').html(),
                    C_OriginalAmount: $(this).find('td:eq(6)').html().replace(regRemoveCurrFormate, ""),
                    C_Debit: $(this).find('td:eq(7)').html().replace(regRemoveCurrFormate, ""),
                    C_Credit: $(this).find('td:eq(8)').html().replace(regRemoveCurrFormate, "")
                });
            });

            var analyticArr = [];
            $.each($("#TCGE-TDBAnalytic tbody tr"), function () {
                analyticArr.push({
                    C_AnalyticAccountID: $(this).find('td:eq(0)').html(),
                    C_DistID: $(this).find('td:eq(1)').html(),
                    C_AID: $(this).find('td:eq(4)').html(),
                    Describtion: $(this).find('td:eq(5)').html(),
                    C_Percentage: $(this).find('td:eq(6)').html().replace('%', ''),
                    C_Amount: $(this).find('td:eq(7)').html().replace(regRemoveCurrFormate, ""),
                    C_Debit: $(this).find('td:eq(8)').html().replace(regRemoveCurrFormate, ""),
                    C_Credit: $(this).find('td:eq(9)').html().replace(regRemoveCurrFormate, "")
                });
            });

            var costArr = [];
            $("#TCGE-TTbl tr").each(function () {
                if (Test) {
                    if ($(this).attr("data-costcentertype")) {
                        if ($(this).attr("data-ccvalue")) {
                            var ThisArr = JSON.parse($(this).attr("data-ccvalue"));
                            $.each(ThisArr, function (k, i) {
                                costArr.push(i);
                            })
                        } else {
                            Test = false;
                            $(this).find(".GetCostCenterDetails").trigger("click");
                        }
                    }

                }
            })
            //$.each($("#TCGE-TDBCost tbody tr"), function () {
            //    costArr.push({
            //        C_CostCenterID: $(this).find('td:eq(0)').html(),
            //        C_CAID: $(this).find('td:eq(1)').html(),
            //        C_AID: $(this).find('td:eq(4)').html(),
            //        Describtion: $(this).find('td:eq(5)').html(),
            //        C_Percentage: $(this).find('td:eq(6)').html().replace('%', ''),
            //        C_Amount: $(this).find('td:eq(7)').html().replace(regRemoveCurrFormate, ""),
            //        C_Debit: $(this).find('td:eq(8)').html().replace(regRemoveCurrFormate, ""),
            //        C_Credit: $(this).find('td:eq(9)').html().replace(regRemoveCurrFormate, ""),
            //        C_CostCenterGroupID: $(this).find('td:eq(11)').html(),
            //        CostCenterPercentage: $(this).find('td:eq(12)').html().replace('%', '')
            //    });
            //});

            var data = JSON.stringify({
                SaveTransaction: mainArr,
                SaveAnalytic: analyticArr,
                SaveCost: costArr,
                SaveHeader: headerObj
            });
            if (Test) {
                $.ajax({
                    contentType: 'application/json; charset=utf-8',
                    dataType: 'json',
                    method: "post",
                    url: "/api/TransactionApi/InsertTransactionData?inputType=" + inputType + "&companyID=" + companyID + "&voidPostingNum=" + voidPostingNum,
                    data: data,
                    async: false,
                    success: function (result) {

                        Talert("This Transaction Journal Entry Number is >> " + result.JournalEntryNumber);
                        postingNumber = result.PostingNumber;
                        $('#importantForReport').text(result.JournalEntryNumber);
                        $(document).find(".UploadJN").val(postingNumber);
                        if (PrintJv) {
                            if (inputType == 2) {
                                if (!PrintJvAfterAjax) {
                                    window.open(
                                        '/C_ReportsPrint/Done?searchNumber=' + postingNumber,
                                        '_blank'
                                    );
                                } else {
                                    RunAfterAjax(function () {
                                        window.open(
                                            '/C_ReportsPrint/Done?searchNumber=' + postingNumber,
                                            '_blank'
                                        );
                                    })
                                }

                            }
                        }
                        if (CallBack != null) {
                            CallBack(result.JournalEntryNumber, postingNumber);
                        }
                        $(document).find(".StartImageUpload").trigger("click");
                    },
                    error: function (a, b, c) {
                        Talert("Some Thing Went Wrong Please");
                        if (ErrorCallBack) {
                            ErrorCallBack();
                        }
                    }
                });
            }

        }
    }
    return postingNumber;
}

//Moved From Transaction Script
function TCGE_ClearRatesInChange() {
    $("#TCGE-SystemRate").val("");
    $("#TCGE-TransactionRate").val("");
    $("#TCGE-DiffrenceRate").val("");
    // $("#TCGE-CurrencyID").val("");
    $("#TCGE-TransactionRate").prop("disabled", true);
    $("#TCGE-DiffrenceRate").prop("disabled", true);
}
function getParameterByName(name, url) {
    if (!url) url = window.location.href;
    name = name.replace(/[\[\]]/g, '\\$&');
    var regex = new RegExp('[?&]' + name + '(=([^&#]*)|&|#|$)'),
        results = regex.exec(url);
    if (!results) return null;
    if (!results[2]) return '';
    return decodeURIComponent(results[2].replace(/\+/g, ' '));
}

function CheckJvAnaylticCost(CallBack = null) {
    var Accounts = [];
    $("#TCGE-TTbl").find("tr").each(function () {
        Accounts.push(parseInt($(this).find(".TCGE-TblAccID").text()));
    })
    $.ajax({
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        url: "/api/TransactionApi/CheckAccountHasCostOrAnayltic",
        method: "post",
        data: JSON.stringify(Accounts),
        success: function (data) {
            $.each(data, function (k, i) {
                var Tr = $("#TCGE-TTbl").find(".TCGE-TblAccID:textEquals(" + i.Aid + ")").parent("tr");

                var debit = GetMaskNumber($(Tr).find(".sDebitTbl").text())
                var credit = GetMaskNumber($(Tr).find(".sCreditTbl").text())

                if (i.HasAnayltic) {
                    GetAnaylticData(debit, credit);
                } else if (i.HasCostCenter) {

                    var accountID = $(Tr).find(".TCGE-TblAccID").text();
                    var accountIDTbl = $(Tr).find(".accountIDTbl").text();
                    var accountName = $(Tr).find(".accountName").text();
                    var describtion = $(Tr).find(".describtion").text();
                    var originalAmount = GetMaskNumber($(Tr).find(".OrA").text());
                    var document = $(Tr).find(".document").text();

                    SetDataInCCPopUp(debit, credit, accountIDTbl, accountName, i.CostCetnerType, i.CostCenterId, i.CostGroupId, accountID, describtion);
                }
            })
            if (CallBack != null) {
                CallBack();
            }
        }
    })
}
var CallShowHideAnayltic = false;
function ShowHideAnaylticAndCost(CallBack = null) {
    CallShowHideAnayltic = true;
    var Accounts = [];
    $("#TCGE-TTbl").find("tr").each(function () {
        Accounts.push(parseInt($(this).find(".TCGE-TblAccID").text()));
    })
    $.ajax({
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        url: "/api/TransactionApi/CheckAccountCostAndAnayltic",
        method: "post",
        data: JSON.stringify(Accounts),
        success: function (data) {
            $.each(data, function (k, i) {
                if (!i.HasAnayltic) {
                    $(document).find("#TCGE-TTbl").find(".TCGE-TblAccID:textEquals(" + i.Aid + ")")
                        .parents("tr").find(".GetAnalyticDetails").remove();
                } else {
                    $(document).find("#TCGE-TTbl").find(".TCGE-TblAccID:textEquals(" + i.Aid + ")")
                        .parents("tr").find(".GetAnalyticDetails").removeClass("hide");
                }

                if (!i.HasCostCenter) {
                    $(document).find("#TCGE-TTbl").find(".TCGE-TblAccID:textEquals(" + i.Aid + ")")
                        .parents("tr").find(".GetCostCenterDetails").remove();
                } else {
                    $(document).find("#TCGE-TTbl").find(".TCGE-TblAccID:textEquals(" + i.Aid + ")")
                        .parents("tr").find(".GetCostCenterDetails").removeClass("hide");

                    $(document).find("#TCGE-TTbl").find(".TCGE-TblAccID:textEquals(" + i.Aid + ")")
                        .parents("tr").attr("data-costcentertype", i.CostCetnerType);

                    $(document).find("#TCGE-TTbl").find(".TCGE-TblAccID:textEquals(" + i.Aid + ")")
                        .parents("tr").attr("data-centerId", i.CostCenterId);
                }
            })
            CallShowHideAnayltic = false;
            if (CallBack != null) {
                CallBack();
            }
        }
    })
}
function GetAccountDetails(accountID, currencyID, IsAdjust, CallBack = null) {
    $.ajax({
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        method: "get",
        url: "/api/TransactionApi/GetAccountDetails?c_AID=" + accountID + "&currencyID=" + currencyID + "&IsAdjust=" + IsAdjust,
        success: function (result) {

            //Choosen Account Must be created with the same currency choosen above
            $("#TCGE-AccountName").val(result.AccountName);
            $("#TCGE-AccountType").text(result.AccountType);
            $("#TCGE-OriginalAmount").prop("disabled", false);
            //check if user mandatory choose document or not
            if (result.SupportDocument === true) {
                $("#TCGE-CheckDocument").text("Mand");
            } else {
                $("#TCGE-CheckDocument").text("");
            }
            //check if account related with analytic or not
            $("#TCGE-CheckAnalytic").text(result.C_AnalyticAccountID);

            $("#TCGE-CostCenterType").text(result.CostCenterType);
            $("#TCGE-CheckCostCenter").text(result.C_CostCenterID);
            $("#TCGE-CheckMainCostCenter").text(result.C_CostCenterGroupID);
            $("#MaximumAmountPerTransaction").text(result.MaiximumAmount);
            $("#MinimumAmountPerTransaction").text(result.MinimumAmount);
            if (CallBack != null) {
                CallBack(AddToMainTbl, GetFromLastTr);
            }

        }, error: function (request, status, error) {
            if (request.status === 406) {
                $("#TCGE-GlobalError2").text(JSON.parse(request.responseText).Message);
                $("#TCGE-AccountID").val("");
            }
        }
    });
}

function CheckAnalyticAndCostCenter() {
    //Get Above data
    var currencyID = $("#TCGE-CurrencyID").val();
    var transactionRate = $("#TCGE-TransactionRate").val();

    //Get Recently data
    var accountID = $("#TCGE-AccountID").val();
    var accountIDTbl = $("#TCGE-AccountID option:selected").text();
    var accountName = $("#TCGE-AccountName").val();
    var describtion = $("#TCGE-Describtion").val();
    var originalAmount = $("#TCGE-OriginalAmount").val().replace(regRemoveCurrFormate, "");
    var debit = $("#TCGE-Debit").val().replace(regRemoveCurrFormate, "");
    var credit = $("#TCGE-Credit").val().replace(regRemoveCurrFormate, "");
    var minAmount = $("#MinimumAmountPerTransaction").text().replace(regRemoveCurrFormate, "");
    var maxAmount = $("#MaximumAmountPerTransaction").text().replace(regRemoveCurrFormate, "");
    var document = $("#TCGE-SUD").val();


    //Get Result of check of Account ID in document and analytic and Cost
    var checkDocument = $("#TCGE-CheckDocument").text();
    var checkAnalytic = $("#TCGE-CheckAnalytic").text();
    var checkCostCenter = $("#TCGE-CheckCostCenter").text();
    var checkMainCostCenter = $("#TCGE-CheckMainCostCenter").text();
    var costCenterType = $("#TCGE-CostCenterType").text();

    if (checkAnalytic.length > 0) {

        //if open pop up of analytic account at least one not get from database again to save his changed if exist
        if ($("#TCGE-SHPUAA").text() === "true") {
            //function to open analytic popup with old data
            OpenPUAAWithOutRefrech(debit, credit);
        } else {
            GetAnaylticData(debit, credit);
        }
        //If Account Choosen Related With Cost Center ID or Main Cost Center ID
    } else if (checkCostCenter.length > 0 || checkMainCostCenter.length > 0) {

        //Check IF Exist Analytic Popup to show or hide back Btn
        if ($("#TCGE-PUCCCheckAnalytic").text() === "Exist") {
            $("#TCGE-PUCCBtnBTA").show();
        } else {
            $("#TCGE-PUCCBtnBTA").hide();
        }

        //Big Function to get data of CAccount to dropdown and fill table of cost Account that ready of DB "Save Mode"
        SetDataInCCPopUp(debit, credit, accountIDTbl, accountName, costCenterType, checkCostCenter, checkMainCostCenter, accountID, describtion);
        //else if choosen account not related with analytic or cost will add to table directly
    } else {
        AddToMainTbl(accountID, accountName, document, accountIDTbl, describtion, originalAmount, debit, credit);
    }
}
function GetAnaylticData(debit, credit, data = null) {
    var currencyID = $("#TCGE-CurrencyID").val();
    var transactionRate = $("#TCGE-TransactionRate").val();

    //Get Recently data
    var accountID = $("#TCGE-AccountID").val();
    var accountIDTbl = $("#TCGE-AccountID option:selected").text();
    var accountName = $("#TCGE-AccountName").val();
    var describtion = $("#TCGE-Describtion").val();
    var originalAmount = $("#TCGE-OriginalAmount").val().replace(regRemoveCurrFormate, "");
    var debit = $("#TCGE-Debit").val().replace(regRemoveCurrFormate, "");
    var credit = $("#TCGE-Credit").val().replace(regRemoveCurrFormate, "");
    var minAmount = $("#MinimumAmountPerTransaction").text().replace(regRemoveCurrFormate, "");
    var maxAmount = $("#MaximumAmountPerTransaction").text().replace(regRemoveCurrFormate, "");
    var document = $("#TCGE-SUD").val();

    if (data != null) {
        accountID = data.accountID;
        accountIDTbl = data.accountIDTbl;
        accountName = data.accountName;
        describtion = data.describtion;
        originalAmount = data.originalAmount
        debit = data.debit
        credit = data.credit
        document = data.document

        $("#TCGE-AccountID").val(accountID);
        $("#TCGE-AccountID option:selected").text(accountIDTbl);
        $("#TCGE-AccountName").val(accountName);
        $("#TCGE-Describtion").val(describtion);
        $("#TCGE-OriginalAmount").val(originalAmount)
        $("#TCGE-Debit").val(debit)
        $("#TCGE-Credit").val(credit)
        $("#TCGE-SUD").val(document);

    }


    //Get Result of check of Account ID in document and analytic and Cost
    var checkDocument = $("#TCGE-CheckDocument").text();
    var checkAnalytic = $("#TCGE-CheckAnalytic").text();
    var checkCostCenter = $("#TCGE-CheckCostCenter").text();
    var checkMainCostCenter = $("#TCGE-CheckMainCostCenter").text();
    var costCenterType = $("#TCGE-CostCenterType").text();


    $("#TCGE-PUAAccDisID").val("");
    $("#TCGE-PUAAccDisName").val("");
    $("#TCGE-PUAAccDisPercentage").val("");
    $("#TCGE-PUAAccDisAmount").val("");
    $("#TCGE-PUAADisDescribtion").val("");
    $("#TCGE-PUAGlobalError").text("");

    //set original amount in pop up
    if (debit.length > 0) {
        $("#TCGE-PUAAOriginalAmount").text(setSystemCurrFormate(+debit));
        $("#TCGE-PUAAUnassigned").text(setSystemCurrFormate(+debit));
    } else if (credit.length > 0) {
        $("#TCGE-PUAAOriginalAmount").text(setSystemCurrFormate(+credit));
        $("#TCGE-PUAAUnassigned").text(setSystemCurrFormate(+credit));
    }
    //----------------------------------------------

    //set main data of popup analytic
    $("#TCGE-PUAAAccID").text(accountIDTbl);
    $("#TCGE-PUAAAccName").text(accountName);
    $("#TCGE-PUAAAnaID").text(checkAnalytic);



    //New ajax With API get analytic distribution drop down data
    $.ajax({
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        method: "get",
        url: "/api/TransactionApi/GetDistributionOfAccountAnalytic?analyticID=" + $("#TCGE-PUAAAnaID").text() + "&AccId=" + accountID,
        success: function (result) {

            //Show Final Save btn of Analytic PopUp and hide final update
            $("#TCGE-PUAAFinalSave").show();
            $("#TCGE-PUAAFinalUpdate").hide();
            $("#TCGE-PUAnalyticAccount").modal("show");

            $("#TCGE-PUAAccDisID").empty();

            if (result.length == 0) {
                $("#TCGE-PUAAccDisID").append($('<option/>', {
                    value: "",
                    text: "No Distribution Account Created To This Analytic"
                })
                );
            } else {

                $("#TCGE-PUAAccDisID").append($('<option/>', {
                    value: "",
                    text: ChooseTxt
                })
                );
                $.each(result, function (index, row) {

                    $("#TCGE-PUAAccDisID").append("<option value='" + row.C_DistID2 + "'>" + row.C_AccountDistributionID2 + "</option>");

                });

                //new ajax API get table data if exist and show popup
                $.ajax({
                    contentType: 'application/json; charset=utf-8',
                    dataType: 'json',
                    method: "get",
                    url: "/api/TransactionApi/CheckExistDistribution?c_AID=" + accountID,
                    success: function (result) {
                        //Span text to true to know that analytic popup is opened
                        $("#TCGE-SHPUAA").text("true");

                        if (result.length === 0) {

                        } else {
                            //$("#TCGE-TAccountAnalytic").html("");
                            for (var i = 0; i < result.length; i++) {

                                //Filter dropdown of distribution by delete items that will exist in distribution table
                                $("#TCGE-PUAAccDisID option[value=" + result[i].C_DistID + "]").remove();

                                //get value of amount by the data that exist in database
                                var amount = parseFloat(result[i].Percentage / 100) * parseFloat($("#TCGE-PUAAOriginalAmount").text().replace(regRemoveCurrFormate, ""));

                                //Function to set data of analytic table
                                AddToAnalyticTbl(result[i].C_DistID, result[i].C_AccountDistributionID, result[i].C_AccountDistributionName, result[i].Percentage, amount, describtion);
                            }

                            //Function to Calculate Assign and Unassign after fill table from DB
                            CalcAnalyticAssUnAss();
                        }
                    }
                });

            }
        }
    });
}