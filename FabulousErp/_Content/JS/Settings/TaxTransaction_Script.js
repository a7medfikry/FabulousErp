// two variables to check updates of quantity and unit Price
var changeQuantity,
    changeUnitPrice,
    vatC_ID,
    vatAccountID,
    vatAccountName,
    tableVatC_ID,
    tableVatAccountID,
    tableVatAccountName,
    decuttaTaxC_ID,
    decuttaTaxAccountID,
    tableVatAmount,
    decuttaTaxAccountName;
var JvArr = [];
$(document).ready(function () {

    const dateformat = /^\d{4}-\d{2}-\d{2}$/; //to check date in correct formate
    var EPDcheck = $("#TCGE-EPD").text(); //to check user can edit posting date or not
    var fJEPer = $("#TCGE-FJEPer").text(); //to check if company has posting setup or not
    var companyID = $('#TCGE-CompanyID').text();


    if ($("#ValidPostringNumber").text() == "true") {

        //if company not have posting setup in financial module
        if (fJEPer === "NoPS") {
            Talert("This Company in Financial Module Not have Posting Setup..!");
            window.location.href = "/Home/Financial_Home";
        }

        //focus in of transaction date to set day of today and focus out to set posting date if disabled and check it in periods
        $('#TCT-JEDate').focusin(function () {

            TCGE_ClearRatesInChange(); //refrech rates in focusin of date
            var now = new Date();
            var day = ("0" + now.getDate()).slice(-2);
            var month = ("0" + (now.getMonth() + 1)).slice(-2);
            var today = now.getFullYear() + "-" + (month) + "-" + (day);
            $(this).val(today);

        }).focusout(function () {

            var jeDate = $(this).val();

            if (!jeDate.match(dateformat)) {
                $(this).css('border-color', 'red');
                $('#TCGE-GlobalError').text('Wrong Date Formate..!');
            } else {
                $(this).css('border-color', '');
                $('#TCGE-GlobalError').text('');

                if (EPDcheck === "F2") {
                    $('#TCT-PostingDate').val(jeDate);
                    var postingDate = $(this).val();
                    //check posting date in periods or not because it now disabled
                    CheckPostingDateInPeriods(postingDate);
                }
            }
        });

        //focus in to take transaction date and focus out to check date in periods
        $('#TCT-PostingDate,#TCGE-PostingDate').on("focusout", function () {

            var postingDate = $(this).val();

            CheckPostingDateInPeriods(postingDate);

        }).focusin(function () {
            //Reload Currency And All Details To Choose it Again with new Date
            TCGE_ClearRatesInChange();

            var transactionDate = $('#TCT-JEDate').val();
            $(this).val(transactionDate);

        });

        //change of currency id to validate all dates and reload buttom data of tax to ready it by new currency and get rates if currency has several rates
        $('#TCGE-CurrencyID').change(function () {

            $("#TCGE-GlobalError").text("");
            $("#TCGE-TransactionRate").css("border-color", "");
            $(this).css('border-color', '');

            //refresh buttom data of tax details
            $('input[name=EffectiveInput]').val('');
            $('select[name=EffectiveInput]').val('');
            $("#TCT-tableVatID option").show();
            $("#TCT-vatID option").show();
            $('#TCT-unitPrice').val('');

            $("#TCGE-SystemRate").val("");
            $("#TCGE-TransactionRate").val("");
            $("#TCGE-DiffrenceRate").val("");

            //make transaction rate disable to make sure open it in get rates or take it disabled
            $("#TCGE-TransactionRate").prop("disabled", true);

            var jeDate = $("#TCT-JEDate").val();
            if (!jeDate) {
                var jeDate = $("#TCGE-JEDate").val();


            }
            var postingDate = $("#TCT-PostingDate").val();
            var Test = true;
            var currencyID = $(this).val();

            if (currencyID.length === 0) {
                $(this).css("border-color", "red");
            } else {
                //check of dates in correct formate and in periods
                if (!postingDate) {
                    postingDate = $("#TCGE-PostingDate").val();
                }
                CheckPostingDateInPeriods(postingDate, function (checkPostingDate) {
                    if (!jeDate.match(dateformat)) {
                        $(this).val("");
                        $("#TCT-JEDate").css("border-color", "red");
                        Test = false;
                    } else {
                        $("#TCT-JEDate").css("border-color", "");
                    }

                    if (!postingDate.match(dateformat)) {
                        $(this).val("");
                        $("#TCT-PostingDate").css("border-color", "red");
                        Test = false;
                    } else if (checkPostingDate !== true) {
                        $(this).val("");
                        Test = false;
                    } else {
                        $("#TCT-PostingDate").css("border-color", "");
                    }

                    //if all validation of dates is true then get rates of currency
                    if (Test === true) {
                        GetCurrencyRates(currencyID, postingDate);
                    }
                });



            }
        });

        //Change of tax Group to get tax code
        $('#TCT-taxGroupID').change(function () {

            var taxGroupID = $(this).val(),
                taxTableType = $('#TCT-taxTableType').val();

            $('#TCT-vatAmount').val('');
            $('#TCT-vatID').empty();
            $('#TCT-tableVatID').empty();
            $('#TCT-decuttaTaxID').empty();
            $('#TCT-tableVatAmount').val('');
            $('#TCT-dacuttaAmount').val('');

            if (taxTableType === '0') {

                $('#TCT-tableVatAmount').val('0');
            }

            if (taxGroupID.length > 0) {
                $.ajax({
                    type: 'GET',
                    url: "/C_TaxTransaction/GetTaxCodeByGroup?taxGroupID=" + 1002,
                    success: function (result) {
                        if (result.length == 0) {
                            $("#TCT-vatID").append($('<option/>', {
                                value: "",
                                text: "No Tax Code Created To this Group"
                            }));

                            $("#TCT-tableVatID").append($('<option/>', {
                                value: "",
                                text: "No Tax Code Created To this Group"
                            }));

                            //$("#TCT-decuttaTaxID").append($('<option/>', {
                            //    value: "",
                            //    text: "No Tax Code Created To this Group"
                            //}));

                        } else {

                            $("#TCT-vatID").append($('<option/>', {
                                value: "",
                                text: "Choose"
                            })
                            );
                            $("#TCT-tableVatID").append($('<option/>', {
                                value: "",
                                text: "Choose"
                            }));

                            //$("#TCT-decuttaTaxID").append($('<option/>', {
                            //    value: "",
                            //    text:Choose
                            //}));

                            $.each(result, function (index, row) {
                                $("#TCT-vatID").append("<option value='" + row.CT_ID + "'>" + row.C_Taxcode + "</option>");
                                $("#TCT-tableVatID").append("<option value='" + row.CT_ID + "'>" + row.C_Taxcode + "</option>");
                                //$("#TCT-decuttaTaxID").append("<option value='" + row.CT_ID + "'>" + row.C_Taxcode + "</option>");
                            });
                        }
                    }
                });
                $.ajax({
                    type: 'GET',
                    url: "/C_TaxTransaction/GetTaxCodeByGroup?taxGroupID=" + $("#TCT-taxGroupID").val(),
                    success: function (result) {
                        $("#TCT-decuttaTaxID").append("<option value=''> Choose </option>");

                        $.each(result, function (index, row) {
                            $("#TCT-decuttaTaxID").append("<option value='" + row.CT_ID + "'>" + row.C_Taxcode + "</option>");
                        });
                    }
                })
            }
        });

        //Change of taxType to show and hide the data that depend on its value
        $('#TCT-taxType').change(function () {

            $('.taxTypeEffect').show();
            $('.taxTableTypeEffect').show();
            $('#TCT-tableVatID').val('');
            $('#TCT-tableVatAmount').val('');
            $("#TCT-tableVatID option").show();
            $('#TCT-vatID').val('');
            $('#TCT-vatAmount').val('');
            $("#TCT-vatID option").show();

            var taxType = $(this).val();
            if (taxType.length > 0) {

                if (taxType === "1") {
                    $('.taxTypeEffect').hide();
                }
            }
        });

        //Change of tax Table type to show and hide the data that depend on its value
        $('#TCT-taxTableType').change(function () {

            var taxTableType = $(this).val(),
                taxType = $('#TCT-taxType').val(),
                netAmount = $('#TCT-netAmount').val();


            $('#TCT-tableVatID').prop('disabled', false);
            $('#TCT-tableVatAmount').prop('disabled', false);
            $('.taxTableTypeEffect').show();

            if (taxTableType.length > 0) {

                if (taxType.length === 0) {
                    $(this).val('');
                    $('#TCT-taxType').css('border-color', 'red');
                } else {
                    $('#TCT-taxType').css('border-color', '');

                    if (!netAmount) {
                        netAmount = 0;
                    }
                    if (netAmount.length > 0) {
                        $('#TCT-totalAfterVatTable').val(netAmount);
                    }

                    $('#TCT-tableVatID').val('');
                    $('#TCT-tableVatAmount').val('');
                    $("#TCT-tableVatID option").show();
                    $('#TCT-vatID').val('');
                    $('#TCT-vatAmount').val('');
                    $("#TCT-vatID option").show();

                    if (taxTableType === "0") {

                        $('#TCT-tableVatID').prop('disabled', true);
                        $('#TCT-tableVatAmount').prop('disabled', true);
                        $('#TCT-tableVatAmount').val('0');
                        $('#TCT-tableVatID').css('border-color', '');

                    } else if (taxTableType === "1") {

                        $('.taxTableTypeEffect').hide();
                    }
                }
            }
        });

        // change of vat id to get amount or percentage of this tax
        $('#TCT-vatID').change(function () {

            var vatID = $(this).val(),
                totalAfterVatTable = GetMaskNumber($('#TCT-totalAfterVatTable').val()),
                tableVatID = $('#TCT-tableVatID'),
                netAmount = GetMaskNumber($('#TCT-netAmount').val());
            //show all options of tax id drop down because will hide the choosen vat from her
            $("#TCT-tableVatID option").show();

            $('#TCT-vatAmount').val('');

            if (vatID.length > 0) {

                //validation of above data of tax that depend on it
                var testValidation = ValidateBeforeChooseTax();

                if (testValidation === true) {
                    if (tableVatID.is(':enabled') && tableVatID.is(':visible') && tableVatID.val().length === 0) {
                        $(this).val('');
                        tableVatID.css('border-color', 'red');
                    } else {
                        tableVatID.css('border-color', '');

                        //remove choosen vat from tax drop down
                        $("#TCT-tableVatID option[value=" + vatID + "]").hide();

                        $.ajax({
                            type: 'GET',
                            url: "/C_TaxTransaction/GetTaxCodeValue?taxCodeID=" + vatID,
                            success: function (result) {

                                vatC_ID = result.C_AID;
                                vatAccountID = result.AccountID;
                                vatAccountName = result.AccountName;
                                var ThisNetAmount = 0;
                                if ($("#TCT-taxTableType").find("option:selected").val() == 1) {
                                    ThisNetAmount = parseFloat(netAmount);
                                } else if ($("#TCT-taxTableType").find("option:selected").val() == 2) {
                                    ThisNetAmount = parseFloat(totalAfterVatTable);
                                    if (isNaN(ThisNetAmount)) {
                                        ThisNetAmount = parseFloat(netAmount);
                                    }
                                } else {
                                    ThisNetAmount = GetMaskNumber(netAmount);
                                }
                                if (isNaN(ThisNetAmount)) {
                                    ThisNetAmount = 0;
                                }
                                if (result.MinAmount == null || result.MinAmount <= ThisNetAmount) {

                                    if (result.MaxAmount != null && result.MaxAmount < ThisNetAmount) {
                                        ThisNetAmount = result.MaxAmount;
                                    }
                                    if (result.Amount !== null || 0) {
                                        //Amount
                                        $('#TCT-vatAmount').val(setSystemCurrFormate(+parseFloat(result.Amount)));
                                    } else if (result.Percentage !== null || 0) {

                                        if ($('#TCT-totalAfterVatTable').is(':hidden')) {
                                            //Percentage
                                            $('#TCT-vatAmount').val(setSystemCurrFormate(+parseFloat(ThisNetAmount) * parseFloat(result.Percentage / 100)));
                                        }
                                        else {
                                            //Percentage
                                            $('#TCT-vatAmount').val(setSystemCurrFormate(+parseFloat(ThisNetAmount) * parseFloat(result.Percentage / 100)));
                                        }
                                    }
                                } else {
                                    $('#TCT-vatAmount').val(setSystemCurrFormate(0));
                                }
                                CalcTotalVal();
                            }
                        });
                    }
                } else {
                    $(this).val('');
                }
            }
        });

        // The same of Vat ID
        $('#TCT-tableVatID').change(function () {

            var taxID = $(this).val(),
                netAmount = $('#TCT-netAmount').val().replace(regRemoveCurrFormate, '');

            $("#TCT-vatID option").show();

            $('#TCT-tableVatAmount').val('');

            if (taxID.length > 0) {

                var testValidation = ValidateBeforeChooseTax();

                if (testValidation === true) {

                    $("#TCT-vatID option[value=" + taxID + "]").hide();

                    $.ajax({
                        type: 'GET',
                        url: "/C_TaxTransaction/GetTaxCodeValue?taxCodeID=" + taxID,
                        success: function (result) {

                            tableVatC_ID = result.C_AID;
                            tableVatAccountID = result.AccountID;
                            tableVatAccountName = result.AccountName;
                            var ThisNetAmount = parseFloat(netAmount);
                            if (result.MinAmount == null || result.MinAmount <= ThisNetAmount) {

                                if (result.MaxAmount != null && result.MaxAmount < ThisNetAmount) {
                                    ThisNetAmount = result.MaxAmount;
                                }
                                if (result.Amount !== null || 0) {

                                    $('#TCT-tableVatAmount').val(setSystemCurrFormate(+parseFloat(result.Amount)));

                                } else if (result.Percentage !== null || 0) {

                                    $('#TCT-tableVatAmount').val(setSystemCurrFormate(+parseFloat(ThisNetAmount) * parseFloat(result.Percentage / 100)));

                                }

                                tableVatAmount = $('#TCT-tableVatAmount').val().replace(regRemoveCurrFormate, '');

                                var totalAfterVatTable = parseFloat(tableVatAmount) + parseFloat(ThisNetAmount);
                                $('#TCT-totalAfterVatTable').val(setSystemCurrFormate(+parseFloat(totalAfterVatTable)));
                            } else {
                                $('#TCT-totalAfterVatTable').val(setSystemCurrFormate(0));

                            }
                            CalcTotalVal();
                        }
                    });

                } else {
                    $(this).val('');
                }
            } else {
                $('#TCT-totalAfterVatTable').val(setSystemCurrFormate(+parseFloat(netAmount)));
            }
        });

        // focusin of quantity to validate data that quantity depend on it
        $('#TCT-quantity').focusin(function () {

            ValidateBeforeQuantityAndPrice();

            //set the value in check in to test update of it in check out
            changeQuantity = $(this).val();

        }).keyup(function () { //key up to remove error validation if exist

            var quantity = $(this).val();
            if (quantity.length > 0) {
                $(this).css('border-color', '');
            }
        }).focusout(function () { //focus out to set error validation if exist

            var quantity = $(this).val();
            if (quantity.length === 0) {
                $(this).css('border-color', 'red');
            }

            //if exist change clear append data
            if (quantity !== changeQuantity) {
                $('input[name=EffectiveInput]').val('');
                $('select[name=EffectiveInput]').val('');
                $("#TCT-tableVatID option").show();
                $("#TCT-vatID option").show();
                // $('#TCT-unitPrice').val('');
            }
            if (window.location.href.indexOf("/C_TaxTransaction/CompanyTaxTransaction") == -1) {
                $('#TCT-unitPrice').trigger("focusout");
            }
        });

        // focusin of unit Price to validate the data that append of it
        $('#TCT-unitPrice').focusin(function () {

            var validateCurrency = ValidateBeforeQuantityAndPrice(),
                quantity = $('#TCT-quantity').val().replace(regRemoveCurrFormate, '');

            //set the value in check in to test update of it in check out
            changeUnitPrice = $(this).val();

            //validate quantity null or not
            if (validateCurrency === true && quantity.length === 0) {

                $('#TCT-quantity').css('border-color', 'red');
                $('#TCT-quantity').focus();
            }

        }).focusout(function () { // calculate data that need number of unit price

            var quantity = $('#TCT-quantity').val().replace(regRemoveCurrFormate, ''),
                unitPrice = GetMaskNumber($(this).val()),
                currencyID = $('#TCGE-CurrencyID').val(),
                taxTableType = $('#TCT-taxTableType').val();

            if ($('#TCGE-TransactionRate').length != -1) {
                transactionRate = $('#TCGE-TransactionRate').val().replace(regRemoveCurrFormate, '')
            } else {
                transactionRate = $('#TCGE-TransactionRate').val().replace(regRemoveCurrFormate, '')
            }


            //if exist change clear append data
            if (unitPrice !== changeUnitPrice) {
                $('input[name=EffectiveInput]').val('');
                $('select[name=EffectiveInput]').val('');
                $("#TCT-tableVatID option").show();
                $("#TCT-vatID option").show();
            }

            if (taxTableType === '0') {
                $('#TCT-tableVatAmount').val('0');
            }

            //if (unitPrice.length > 0)
            {

                var totalAmount = parseFloat(quantity) * parseFloat(unitPrice);

                if (!transactionRate) {
                    transactionRate = $("#Transaction_rateTrans").val();
                }
                if (!transactionRate) {
                    transactionRate = 1;
                }
                var totalAmountBySystemCurrency = totalAmount * transactionRate;


                if (companyID == currencyID) {
                    //system Currency
                    $('#TCT-totalAmount').val(setSystemCurrFormate(+parseFloat(totalAmount)));
                } else {
                    //hard Currency
                    $('#TCT-totalAmount').val(setHardCurrFormate(+parseFloat(totalAmount)));
                }

                //if (CheckIsMainPage()) {
                //    //check if system currency is choosen or hard currency to set formate in totalAmount

                //} else {
                //    if (companyID == currencyID) {
                //        //system Currency
                //        $(this).val(setSystemCurrFormate(+parseFloat(totalAmount / quantity)))
                //    } else {
                //        //hard Currency
                //        $(this).val(setHardCurrFormate(+parseFloat(totalAmount / quantity)))
                //    }
                //}



                $('#TCT-totalAmountbySC').val(setSystemCurrFormate(+parseFloat(totalAmountBySystemCurrency)));
                $('#TCT-netAmount').val(setSystemCurrFormate(+parseFloat(totalAmountBySystemCurrency)));

                tableVatAmount = $('#TCT-tableVatAmount').val();
                if (tableVatAmount.length > 0) {
                    var totalAfterVatTable = totalAmountBySystemCurrency + parseFloat(tableVatAmount);
                    $('#TCT-totalAfterVatTable').val(setSystemCurrFormate(+parseFloat(totalAfterVatTable)));
                }

            }
            //else {
            //    $(this).css('border-color', 'red');
            //}

        }).keyup(function () { // keyup to remove error validation if exist

            var unitPrice = $(this).val();
            if (unitPrice.length > 0) {
                $(this).css('border-color', '');
            }

        });;

        // focus out of discount to calculate new net amount and validate this discount with total amount by currency
        $('#TCT-discount').focusout(function () {
            var taxTableType = $('#TCT-taxTableType').val();

            $('#TCT-discountError').text('');
            $('select[name=EffectiveInput]').val('');
            $('#TCT-vatAmount').val('');
            $('#TCT-tableVatAmount').val('');
            $("#TCT-tableVatID option").show();
            $("#TCT-vatID option").show();

            if (taxTableType === '0') {
                $('#TCT-tableVatAmount').val('0');
            }

            var discount = $(this).val().replace(regRemoveCurrFormate, ''),
                totalAmountByCurrency = $('#TCT-totalAmountbySC').val().replace(regRemoveCurrFormate, '');

            if (totalAmountByCurrency == "0") {
                totalAmountByCurrency = $("#Transaction_rateTrans").val();
            }

            if (discount.length > 0) {
                //validate discount if equal or greater than total amount by currency
                if (parseFloat(discount) > parseFloat(totalAmountByCurrency) || parseFloat(discount) === parseFloat(totalAmountByCurrency)) {
                    $('#TCT-discountError').text('Wrong Discount..!');
                    $(this).val('');

                    $('#TCT-netAmount').val(setSystemCurrFormate(+parseFloat(totalAmountByCurrency)));
                    $('#TCT-totalAfterVatTable').val(setSystemCurrFormate(+ parseFloat(totalAmountByCurrency)));
                } else {

                    var resultAfterDiscount = parseFloat(totalAmountByCurrency) - parseFloat(discount);
                    $('#TCT-netAmount').val(setSystemCurrFormate(+parseFloat(resultAfterDiscount)));

                    tableVatAmount = $('#TCT-tableVatAmount').val();
                    if (tableVatAmount.length > 0) {
                        var totalAfterVatTable = resultAfterDiscount + parseFloat(tableVatAmount);
                        $('#TCT-totalAfterVatTable').val(setSystemCurrFormate(+ parseFloat(totalAfterVatTable)));
                    }
                }
            }

        });

        // keyup of all inputs to remove validation error if exist
        $('.validateTaxHeader input').keyup(function () {
            if (NotInput.indexOf($(this).attr("id")) == -1) {
                if ($(this).val().length > 0) {
                    $(this).css('border-color', '');
                } else {
                    $(this).css('border-color', 'red');
                }
            }
        });

        // change of all select to remove validation error if exist
        $('.validateTaxHeader select').change(function () {

            if ($(this).val().length > 0) {
                $(this).css('border-color', '');
            } else {
                $(this).css('border-color', 'red');
            }

        });

        // delete button that exist in table to delete row and refresh data if exist
        $('#TCT-taxTblBody').on('click', '.DeleteItem', function () {

            var row = $(this).closest("tr");
            var itemID = row.find('td').eq(1).text();
            row.remove();
            var rowCount = $('#TCT-taxTblBody >tr').length;
            if (rowCount === 0) {
                $('#TCT-taxGroupID').prop('disabled', false);
            }

            ////remove this data of item id from transaction table
            //$('#TCT-TTbl > tr').each(function () {

            //    var removedItem = $(this).find('td').eq(6);
            //    if (removedItem.text() === itemID) {
            //        removedItem.closest('tr').remove();
            //    }
            //});

            RefreshInUpdateAndCancel();
        });

        // edit button that exist in table that open update mode and retrieve data to inputs 
        $('#TCT-taxTblBody').on('click', '.EditItem', function () {

            $('#TCT-itemName').prop('disabled', true);
            $("#TCT-tableVatID option").show();
            $("#TCT-vatID option").show();
            $('.taxTypeEffect').show();
            $('.taxTableTypeEffect').show();
            $('#TCT-tableVatID').prop('disabled', false);
            $('#TCT-tableVatAmount').prop('disabled', false);

            var row = $(this).closest("tr"),
                tds = row.find('td'),
                itemName = tds.eq(1).text(),
                unitOfMeasure = tds.eq(2).text(),
                quantity = tds.eq(3).text(),
                unitPrice = tds.eq(4).text(),
                totalAmount = tds.eq(5).text(),
                totalAmountByCurrency = tds.eq(6).text(),
                discount = tds.eq(7).text(),
                netAmount = tds.eq(8).text(),
                tableVatAmount = tds.eq(10).text(),
                vatAmount = tds.eq(12).text(),
                tableVatID = tds.eq(13).text(),
                vatID = tds.eq(14).text(),
                taxType = tds.eq(15).text(),
                taxTableType = tds.eq(16).text(),
                itemType = tds.eq(17).text(),
                totalAfterVatTable = tds.eq(18).text();

            if (taxType === "1") {
                $('.taxTypeEffect').hide();
            } else if (taxTableType === "0") {
                $('#TCT-tableVatID').prop('disabled', true);
                $('#TCT-tableVatAmount').prop('disabled', true);
                $('#TCT-tableVatAmount').val('0');
            } else if (taxTableType === "1") {
                $('.taxTableTypeEffect').hide();
            }

            if (vatID.length > 0) {
                $("#TCT-tableVatID option[value=" + vatID + "]").hide();
            }

            if (tableVatID.length > 0) {
                $("#TCT-vatID option[value=" + tableVatID + "]").hide();
            }

            $('#TCT-btnAddTaxDetails').prop('disabled', true);
            $('#TCT-btnUpdateTaxDetails').prop('disabled', false);
            $('#TCT-btnCancelUpdateTaxDetails').prop('disabled', false);

            $('#TCT-taxType').val(taxType);
            $('#TCT-taxTableType').val(taxTableType);
            $('#TCT-itemName').val(itemName);
            $('#TCT-itemType').val(itemType);
            $('#TCT-unitOfMeasure').val(unitOfMeasure);
            $('#TCT-quantity').val(quantity);
            $('#TCT-unitPrice').val(unitPrice);
            $('#TCT-totalAmount').val(totalAmount);
            $('#TCT-totalAmountbySC').val(totalAmountByCurrency);
            $('#TCT-discount').val(discount);
            $('#TCT-netAmount').val(netAmount);
            $('#TCT-vatID').val(vatID);
            $('#TCT-vatAmount').val(vatAmount);
            $('#TCT-tableVatID').val(tableVatID);
            $('#TCT-tableVatAmount').val(tableVatAmount);
            $('#TCT-totalAfterVatTable').val(totalAfterVatTable);

            $('#TCT-taxDetailsUpdatedID').text(row.index());
        });

        //btn to add tax data to table and check if data filled correctly before add
        $('#TCT-btnAddTaxDetails').click(function () {

            var taxType = $('#TCT-taxType').val(),
                taxTableType = $('#TCT-taxTableType').val(),
                itemName = $('#TCT-itemName').val(),
                itemType = $('#TCT-itemType').val(),
                unitOfMeasure = $('#TCT-unitOfMeasure').find("option:selected").val(),
                unitOfMeasureTxt = $('#TCT-unitOfMeasure').find("option:selected").text(),
                quantity = $('#TCT-quantity').val(),
                unitPrice = $('#TCT-unitPrice').val(),
                totalAmount = $('#TCT-totalAmount').val(),
                totalAmountByCurrency = $('#TCT-totalAmountbySC').val(),
                discount = $('#TCT-discount').val(),
                netAmount = GetMaskNumber($('#TCT-netAmount').val()),
                tableVatIDDB = $('#TCT-tableVatID').val(),
                tableVatID = $('#TCT-tableVatID option:selected').text(),
                tableVatAmount = $('#TCT-tableVatAmount').val(),
                totalAfterVatTable = $('#TCT-totalAfterVatTable').val(),
                vatIDDB = $('#TCT-vatID').val(),
                vatID = $('#TCT-vatID option:selected').text(),
                vatAmount = $('#TCT-vatAmount').val(),
                decuttaIDDB = $('#TCT-decuttaTaxID').val(),
                decuttaID = $('#TCT-decuttaTaxID option:selected').text(),
                decuttaAmount = $('#TCT-dacuttaAmount').val();

            var validateMainData = ValidateBeforeAddAndUpdate();
            if (validateMainData === true) {
                var testIdentity = true;
                if ($("#AllowTaxDuple").val() != "true") {
                    testIdentity = ValidateIdentityOfItemIDBeforeAddAndUpdate(itemName);
                }

                if (testIdentity === true) {
                    if ($('#TCT-taxType').val() == 1) {
                        tableVatAmount = 0;
                    }
                    AddToTaxTbl(taxType, taxTableType, itemName, itemType, unitOfMeasure, unitOfMeasureTxt, quantity, unitPrice, totalAmount, totalAmountByCurrency, discount, netAmount, tableVatIDDB, tableVatID, tableVatAmount, totalAfterVatTable, vatIDDB, vatID, vatAmount, decuttaIDDB, decuttaID, decuttaAmount, vatAccountID, vatC_ID, tableVatAccountID, tableVatC_ID, decuttaTaxAccountID);

                    ////vatC_ID = '';
                    ////vatAccountID = '';
                    ////vatAccountName = '';
                    ////tableVatC_ID = '';
                    ////tableVatAccountID = '';
                    ////tableVatAccountName = '';
                    ////decuttaTaxC_ID = '';
                    ////decuttaTaxAccountID = '';
                    ////decuttaTaxAccountName = '';

                    //AddToTransactionTbl(itemID, vatAccountID, vatAccountName, vatAmount, vatAmount, 0, vatC_ID);
                    //AddToTransactionTbl(itemID, tableVatAccountID, tableVatAccountName, taxAmount, 0, taxAmount, tableVatC_ID);
                }
            }
        });

        // Update btn that finally update data that require to edit it and remove this row and add new row with new updates
        $('#TCT-btnUpdateTaxDetails').click(function () {

            var rowNeedToUpdate = $('#TCT-taxDetailsUpdatedID').text();

            var validateMainData = ValidateBeforeAddAndUpdate();

            if (validateMainData === true) {

                $('#TCT-taxTblBody').find('tr').eq(rowNeedToUpdate).remove();

                $('#TCT-btnAddTaxDetails').click();

                RefreshInUpdateAndCancel();
            }

        });

        // Cancel update that refresh fields and back to save mode
        $('#TCT-btnCancelUpdateTaxDetails').click(function () {

            RefreshInUpdateAndCancel();

        });

        $('#TCT-doneTaxDetails').click(function () {
            if (CheckIsMainPage()) {
                InsertTaxJv();
            } else {
                InsertTaxJv(false, "SED");
            }
        });
        window.InsertTaxJv = function (RmUpdate = false, DocType = null, GetIndex = -1, PorS = "P") {
            if ($('#TCT-taxTable > tbody > tr').length > 0) {
                var totalTableVatAmount = 0,
                    totalVatAmount = 0,
                    totalDacuttaAmount = 0,
                    totalAmountSys = 0,
                    totalNetAmount = 0,
                    totalDiscount = 0,
                    taxArray = [];

                var data = {
                    ShowHeader: {
                        ISO: $("#TCGE-CurrencyID").find("option:selected").text(),
                    },
                    ShowTransactions: []
                };
                if (window.location.href.indexOf("/C_TaxTransaction/CompanyTaxTransaction") > -1) {
                    $("#TCGE-GTbl").find("#TCGE-TTbl").find("tr").remove();
                }

                $('#TCT-taxTable > tbody > tr').each(function () {
                    if ($(this).index() == GetIndex || GetIndex == -1) {
                        let tableVatAmount = parseFloat($(this).find('td:eq(10)').text().replace(regRemoveCurrFormate, "")),
                            vatAmount = parseFloat($(this).find('td:eq(13)').text().replace(regRemoveCurrFormate, "")),
                            dacuttaAmount = parseFloat($(this).find(".TblDecuttaId").text().replace(regRemoveCurrFormate, "")),
                            amountSys = parseFloat($(this).find('td:eq(5)').text().replace(regRemoveCurrFormate, "")),
                            netAmount = parseFloat($(this).find('td:eq(8)').text().replace(regRemoveCurrFormate, "")),
                            discount = parseFloat($(this).find('td:eq(7)').text().replace(regRemoveCurrFormate, ""));

                        totalTableVatAmount = (isNaN(tableVatAmount)) ? 0 : parseFloat(tableVatAmount);
                        totalVatAmount = (isNaN(vatAmount)) ? 0 : parseFloat(vatAmount);
                        totalDacuttaAmount = (isNaN(dacuttaAmount)) ? 0 : parseFloat(dacuttaAmount);
                        totalAmountSys = (isNaN(amountSys)) ? 0 : parseFloat(amountSys);
                        totalNetAmount = (isNaN(netAmount)) ? 0 : parseFloat(netAmount);
                        totalDiscount = (isNaN(discount)) ? 0 : parseFloat(discount);

                        $('#TCT-taxDeb').val(totalTableVatAmount + totalVatAmount);
                        $('#TCT-taxCR').val(totalDacuttaAmount);
                        $('#TCT-pushAmount').val(totalAmountSys);
                        $('#TCT-payAmount').val(netAmount + totalVatAmount + totalTableVatAmount - totalDacuttaAmount);
                        $('#TCT-disAmount').val(totalDiscount);

                        var TblAccountId = $(this).find(".TblAccountId").text()
                        var TblAcountAId = $(this).find(".TblAccountId").attr("data-value");
                        var TblAccountName = vatAccountName;//$(this).find(".TblAccountId").text();

                        var TblVatAccountId = $(this).find(".TblVatAccountId").text();
                        var TblVatAccountAId = $(this).find(".TblVatAccountId").attr("data-value");
                        var TblVatAccountName = tableVatAccountName;//$(this).find(".TblVatAccountId").text();


                        if (vatAmount) {
                            data.ShowTransactions.push({
                                Orginal_debit: vatAmount,

                                OriginalAmount: vatAmount,
                                Debit: vatAmount,
                                AID: TblAcountAId,
                                CreditAccNum: TblAccountId,
                                CreditAccName: TblAccountName,
                                Describtion: "",
                                ISO: $("#TCGE-CurrencyID").find("option:selected").text(),
                                AccountID: TblAccountId,
                                AccountName: TblAccountName,
                                IsDebit: true,
                                ShowBtn: RmUpdate,
                            }, {});
                        }
                        if (tableVatAmount) {

                            data.ShowTransactions.push(
                                {
                                    OriginalAmount: tableVatAmount,
                                    Orginal_debit: tableVatAmount,
                                    Credit: 0,
                                    AID: TblVatAccountAId,
                                    Debit: tableVatAmount,
                                    CreditAccNum: "",
                                    AccountID: TblVatAccountId,
                                    AccountName: TblVatAccountName,
                                    CreditAccName: "",
                                    Describtion: "",
                                    ISO: $("#TCGE-CurrencyID").find("option:selected").text(),
                                    IsDebit: true,
                                    ShowBtn: RmUpdate,
                                }, {});
                        }
                        if (dacuttaAmount) {
                            data.ShowTransactions.push({},
                                {
                                    OriginalAmount: dacuttaAmount,
                                    Orginal_credit: dacuttaAmount,
                                    Credit: dacuttaAmount,
                                    Debit: 0,
                                    AID: decuttaTaxC_ID,
                                    DebitAccNum: "",
                                    DebitAccName: "",
                                    AccountID: decuttaTaxAccountID,
                                    AccountName: decuttaTaxAccountName,
                                    Describtion: "",
                                    ISO: $("#TCGE-CurrencyID").find("option:selected").text(),
                                    IsDebit: false,
                                    ShowBtn: RmUpdate,

                                });
                        }
                    }

                })

                var NewData = {
                    ShowHeader: {
                        ISO: $("#TCGE-CurrencyID").find("option:selected").text(),
                        DocType: DocType

                    },
                    ShowTransactions: []
                };

                var NewList = groupBy(data.ShowTransactions, x => x.AID);
                NewList.forEach(function (i, k) {
                    if (k) {
                        var Amount = 0;
                        $.each(i, function (kk, ii) {
                            Amount += ii.OriginalAmount;
                        })
                        var IsSales = false;

                        if (PorS == "S") {
                            IsSales = true;
                        } else if (PorS == "P") {
                            IsSales = false;
                        } else if (getParameterByName("Tax") == "Sales" || $("#TaxTable").val() == "Sales") {
                            var IsSales = false;
                        }

                        if (IsSales) {
                            if (i[0].IsDebit) {
                                NewData.ShowTransactions.push({}, {
                                    OriginalAmount: Amount,
                                    Orginal_credit: Amount,
                                    Orginal_debit: 0,//i[0].OriginalAmount,
                                    Credit: Amount,
                                    Debit: 0,
                                    AID: i[0].AID,
                                    DebitAccNum: "",
                                    DebitAccName: "",
                                    AccountID: i[0].AccountID,
                                    AccountName: i[0].AccountName,
                                    Describtion: "",
                                    ISO: $("#TCGE-CurrencyID").find("option:selected").text(),
                                    ShowBtn: RmUpdate,
                                })
                            } else {
                                NewData.ShowTransactions.push({
                                    OriginalAmount: Amount,
                                    Orginal_debit: Amount,
                                    Credit: 0,
                                    Debit: Amount,
                                    AID: i[0].AID,
                                    DebitAccNum: "",
                                    DebitAccName: "",
                                    AccountID: i[0].AccountID,
                                    AccountName: i[0].AccountName,
                                    Describtion: "",
                                    ISO: $("#TCGE-CurrencyID").find("option:selected").text(),
                                    ShowBtn: RmUpdate,

                                }, {})
                            }
                        }
                        else {
                            if (i[0].IsDebit) {
                                NewData.ShowTransactions.push({
                                    OriginalAmount: Amount,
                                    Orginal_debit: Amount,//i[0].OriginalAmount,
                                    Credit: 0,
                                    Debit: Amount,
                                    AID: i[0].AID,
                                    DebitAccNum: "",
                                    DebitAccName: "",
                                    AccountID: i[0].AccountID,
                                    AccountName: i[0].AccountName,
                                    Describtion: "",
                                    ISO: $("#TCGE-CurrencyID").find("option:selected").text(),
                                    ShowBtn: RmUpdate,

                                }, {})
                            } else {
                                NewData.ShowTransactions.push({}, {
                                    OriginalAmount: Amount,
                                    Orginal_credit: i[0].OriginalAmount,
                                    Credit: Amount,
                                    Debit: 0,
                                    AID: i[0].AID,
                                    DebitAccNum: "",
                                    DebitAccName: "",
                                    AccountID: i[0].AccountID,
                                    AccountName: i[0].AccountName,
                                    Describtion: "",
                                    ISO: $("#TCGE-CurrencyID").find("option:selected").text(),
                                    ShowBtn: RmUpdate,

                                })
                            }
                        }

                    }

                })
                $.each(NewData.ShowTransactions, function (k, i) {
                    if (i.AID) {
                        JvArr.push(i.AID)
                    }
                })
                ManyJvAction(NewData, RmUpdate);
                return NewData;
            }
        }
        //$('#TCT-partAfterDone').show();



        /********************************************************************************************************/
        // some important code here 
        //Deleted By Khaled
        //$('#TCT-taxTable > tbody > tr').each(function () {
        //    if ($(this).find('td:eq(9)').text() != '') {
        //        taxArray.push({
        //            TaxID: $(this).find('td:eq(9)').text(),
        //            Amount: $(this).find('td:eq(10)').text(),
        //            AccountID: $(this).find('td:eq(11)').text(),
        //            Code: 'debit'
        //        });
        //    }
        //    if ($(this).find('td:eq(12)').text() != '') {
        //        taxArray.push({
        //            TaxID: $(this).find('td:eq(12)').text(),
        //            Amount: $(this).find('td:eq(13)').text(),
        //            AccountID: $(this).find('td:eq(14)').text(),
        //            Code: 'debit'
        //        });
        //    }
        //    if ($(this).find('td:eq(15)').text() != '') {
        //        taxArray.push({
        //            TaxID: $(this).find('td:eq(15)').text(),
        //            Amount: $(this).find('td:eq(16)').text(),
        //            AccountID: $(this).find('td:eq(17)').text(),
        //            Code: 'credit'
        //        });
        //    }
        //});
        //console.log(taxArray);


        //var map = taxArray.reduce(function (map, e) {
        //    map[e.TaxID] = +e.Amount.replace(regRemoveCurrFormate, "") + (map[e.TaxID] || 0)
        //    return map
        //}, {})
        //var result = Object.keys(map).map(function (k) {
        //    return { TaxID: k, Amount: map[k] }
        //})
        //console.log(result);
        //}
    }
});

$('#TCT-decuttaTaxID').change(function () {

    var decuttaTaxId = $(this).val(),
        netAmount = GetMaskNumber($('#TCT-netAmount').val());

    if (decuttaTaxId) {

        $(this).css('border-color', '');

        //validation of above data of tax that depend on it
        var testValidation = ValidateBeforeChooseTax();

        if (testValidation === true) {
            $.ajax({
                type: 'GET',
                url: "/C_TaxTransaction/GetTaxCodeValue?taxCodeID=" + decuttaTaxId,
                success: function (result) {

                    decuttaTaxC_ID = result.C_AID;
                    decuttaTaxAccountID = result.AccountID;
                    decuttaTaxAccountName = result.AccountName;

                    var ThisNetAmount = parseFloat(netAmount);
                    if (result.MinAmount == null || result.MinAmount <= ThisNetAmount) {

                        if (result.MaxAmount != null && result.MaxAmount < ThisNetAmount) {
                            ThisNetAmount = result.MaxAmount;
                        }
                        if (result.Amount !== null || 0) {

                            $('#TCT-dacuttaAmount').val(setSystemCurrFormate(+parseFloat(result.Amount)));

                        } else if (result.Percentage !== null || 0) {

                            $('#TCT-dacuttaAmount').val(setSystemCurrFormate(+parseFloat(ThisNetAmount) * parseFloat(result.Percentage / 100)));

                        }
                    } else {
                        $('#TCT-dacuttaAmount').val(setSystemCurrFormate(0));
                    }

                }
            });
        } else {
            $(this).val('');
        }
    } else {
        $(this).css('border-color', 'red');
    }
});

$('#TCT-SOrPAccount').change(function () {

    var accountIDSOrP = $(this).val(),
        accountIDROrP = $('#TCT-ROrPAccount').val(),
        accountIDDiscount = $('#TCT-discountAccount').val(),
        test = true;


    if (accountIDSOrP.length > 0) {
        $(this).css('border-color', '');

        if (accountIDROrP.length > 0 && accountIDSOrP === accountIDROrP) {
            $(this).val('');
            test = false;
        }

        if (accountIDDiscount.length > 0 && accountIDSOrP === accountIDDiscount) {
            $(this).val('');
            test = false;
        }

        if (test === true) {

        }

    } else {
        $(this).css('border-color', 'red');
    }
});

$('#TCT-ROrPAccount').change(function () {

    var accountIDROrP = $(this).val(),
        accountIDSOrP = $('#TCT-SOrPAccount').val(),
        accountIDDisacount = $('#TCT-discountAccount').val(),
        test = true;

    if (accountIDROrP.length > 0) {
        $(this).css('border-color', '');

        if (accountIDSOrP.length > 0 && accountIDROrP === accountIDSOrP) {
            $(this).val('');
            test = false;
        }

        if (accountIDDisacount.length > 0 && accountIDROrP === accountIDDisacount) {
            $(this).val('');
            test = false;
        }

        if (test === true) {

        }

    } else {
        $(this).css('border-color', 'red');
    }
});

$('#TCT-discountAccount').change(function () {

    var accountIDDiscount = $(this).val(),
        accountIDSOrP = $('#TCT-SOrPAccount').val(),
        accountIDROrP = $('#TCT-ROrPAccount').val(),
        test = true;

    if (accountIDDiscount.length > 0) {
        $(this).css('border-color', '');

        if (accountIDSOrP.length > 0 && accountIDDiscount === accountIDSOrP) {
            $(this).val('');
            test = false;
        }

        if (accountIDROrP.length > 0 && accountIDDiscount === accountIDROrP) {
            $(this).val('');
            test = false;
        }

        if (test === true) {

        }

    } else {
        $(this).css('border-color', 'red');
    }
});

//Validate of quantity and unit price before choose tax
function ValidateBeforeChooseTax() {

    var quantity = $('#TCT-quantity').val().replace(regRemoveCurrFormate, ''),
        unitPrice = $('#TCT-unitPrice').val().replace(regRemoveCurrFormate, ''),
        taxTableType = $('#TCT-taxTableType').val();
    test = true;

    if (quantity.length === 0) {
        $('#TCT-quantity').css('border-color', 'red');
        test = false;
    } else {
        $('#TCT-quantity').css('border-color', '');
    }

    if (unitPrice.length === 0) {
        $('#TCT-unitPrice').css('border-color', 'red');
        test = false;
    } else {
        $('#TCT-unitPrice').css('border-color', '');
    }

    if ($('#TCT-taxTableType').is(':visible')) {
        if (taxTableType.length === 0) {
            $('#TCT-taxTableType').css('border-color', 'red');
            test = false;
        } else {
            $('#TCT-taxTableType').css('border-color', '');
        }
    }
    return test;
}

//validate of currency and rates before type quantity and unit price
function ValidateBeforeQuantityAndPrice() {

    var currencyID = $('#TCGE-CurrencyID').val(),
        transactionRate = $('#TCGE-TransactionRate').val().replace(regRemoveCurrFormate, ''),
        companyID = $('#TCGE-CompanyID').val(),
        taxType = $('#TCT-taxType').val(),
        taxTableType = $('#TCT-taxTableType').val(),
        taxGroupID = $('#TCT-taxGroupID').val(),
        test = true;



    if (currencyID.length === 0) {
        $('#TCGE-CurrencyID').css('border-color', 'red');
        $('#TCGE-CurrencyID').focus();
        test = false;

    } else if (currencyID !== companyID && transactionRate.length === 0) {
        $('#TCGE-TransactionRate').css('border-color', 'red');
        $('#TCGE-TransactionRate').focus();
        test = false;
    }

    if (taxType.length === 0) {
        $('#TCT-taxType').css('border-color', 'red');
        test = false;
    } else {
        $('#TCT-taxType').css('border-color', '');
    }

    if (taxTableType.length === 0) {
        $('#TCT-taxTableType').css('border-color', 'red');
        test = false;
    } else {
        $('#TCT-taxTableType').css('border-color', '');
    }

    if (taxGroupID.length === 0) {
        $('#TCT-taxGroupID').css('border-color', 'red');
        test = false;
    } else {
        $('#TCT-taxGroupID').css('border-color', '');
    }

    return test;
}
var NotInput = [];

NotInput.push("TCT-address");
NotInput.push("TCT-nationalID");
NotInput.push("TCT-mobileNumber");

NotInput.push("TCT-taxFileNo");
NotInput.push("TCT-taxRegisterNo");
//validate before add and update of tax in table
function ValidateBeforeAddAndUpdate() {
    var test = true;


    $('.validateTaxHeader input, .validateTaxHeader select').each(function () {

        if (NotInput.indexOf($(this).attr("id")) == -1) {
            if ($(this).val().length === 0 && $(this).is(':enabled') && $(this).is(':visible')) {
                $(this).css('border-color', 'red');
                $(this).focus();
                test = false;
            } else {
                $(this).css('border-color', '');
            }
        }

    });

    return test;
}

//validate to make sure that item id not repeat
function ValidateIdentityOfItemIDBeforeAddAndUpdate(itemID) {

    var testIdentityOfItemID = true;

    $('#TCT-taxTblBody > tr').each(function () {

        var itemIDTbl = $(this).find('td').eq(1);

        if (itemIDTbl.text() === itemID) {

            itemIDTbl.css('border-color', 'red');
            $('#TCT-itemName').css('border-color', 'red');
            testIdentityOfItemID = false;

        } else {
            itemIDTbl.css('border-color', '');
        }
    });

    return testIdentityOfItemID;
}

//function to add data to table
function AddToTaxTbl(taxType, taxTableType, itemName, itemType, unitOfMeasure, unitOfMeasureTxt, quantity, unitPrice, totalAmount, totalAmountByCurrency, discount, netAmount, tableVatIDDB, tableVatID, tableVatAmount, totalAfterVatTable, vatIDDB, vatID, vatAmount, decuttaIDDB, decuttaID, decuttaAmount, vatAccountID, vatC_ID, tableVatAccountID, tableVatC_ID, decuttaTaxAccountID) {

    //disable taxgroupid because table has data now
    $('#TCT-taxGroupID').prop('disabled', true);

    if (tableVatIDDB.length === 0) {
        tableVatID = '';
    }

    if (vatIDDB.length === 0) {
        vatID = '';
    }
    var DeductId = '';
    if ($("#TCT-decuttaTaxID").find("option:selected").val()) {
        DeductId = $("#TCT-decuttaTaxID").find("option:selected").val()
    }
    if (decuttaID == "-Choose-" || decuttaID == Choose) {
        decuttaID = "";
    }
    var DocType = "";
    if ($("#TCT-documentType").find("option:selected").val()) {
        DocType = $("#TCT-documentType").find("option:selected").val();
    } else {
        DocType = $("select[name='Doc_type']").find("option:selected").val();
        if (DocType == "Return") {
            DocType = "";
        }
    }
    var DocNum = "";
    var Date = null;
    var VendoreName = null;
    if (CheckIsMainPage()) {
        Date = $("#TCT-documentDate").val();
        VendoreName = $("#TCT-vendorName").val();
        DocNum = $("#TCT-documentNumber").val();
    } else {
        Date = $("#TCGE-PostingDate").val();
        VendoreName = $("select[name='Vendor_id']").val();
        DocNum = $("#VDocument_number").val();
    }
    if (!discount) {
        discount = 0;
    }
    var x = "<tr>"
        + "<td>" + '<button class="btn btn-secondary btn-sm mr-1 EditItem"><span class="fa fa-edit"></span></button>' + '<button class="btn btn-danger btn-sm DeleteItem"><span class="fa fa-trash-o"></span></button>' + "</td>" +
        "<td>" + itemName + "</td>" +
        "<td data-value='" + unitOfMeasure + "'>" + unitOfMeasureTxt + "</td>" +
        "<td>" + quantity + "</td>" +
        "<td class='Unit_price'>" + unitPrice + "</td>" +
        "<td class=''>" + totalAmount + "</td>" +
        "<td class=''>" + totalAmountByCurrency + "</td>" +
        "<td class='Discount'>" + discount + "</td>" +
        "<td class=''>" + netAmount + "</td>" +
        "<td class='hide'> " + tableVatID + "</td> " +
        "<td class='hide'>" + tableVatAmount + "</td>" +
        "<td  class='hide TblVatAccountId' data-value='" + tableVatC_ID + "'>" + tableVatAccountID + "</td>" +
        "<td class='hide'>" + vatID + "</td>" +
        "<td class='hide'>" + vatAmount + "</td>" +
        "<td  class='hide TblAccountId' data-value='" + vatC_ID + "'>" + vatAccountID + "</td>" +
        "<td class='TblTotalVatamount'>" + setSystemCurrFormate(CalcTotalVal()) + "</td>" +
        "<td>" + decuttaID + "</td>" +
        "<td data-value='" + decuttaTaxC_ID + "' class='TblDecuttaId'>" + decuttaAmount + "</td>" +
        "<td class='hide'>" + decuttaTaxAccountID + "</td>" +
        "<td class='hide-normal'>" + tableVatIDDB + "</td>" +
        "<td class='hide-normal'>" + vatIDDB + "</td>" +
        "<td class='hide-normal'>" + decuttaIDDB + "</td>" +
        "<td class='hide-normal'>" + taxType + "</td>" +
        "<td class='hide-normal'>" + taxTableType + "</td>" +
        "<td class='hide-normal'>" + itemType + "</td>" +
        "<td class='hide-normal sumTotalAfterVatTable'>" + totalAfterVatTable + "</td>" +

        "<td class='hide Doc_type'>" + DocType + "</td>" +
        "<td class='hide Doc_num'>" + DocNum + "</td>" +
        "<td class='hide Date'>" + Date + "</td>" +
        "<td class='hide Vendor_name'>" + VendoreName + "</td>" +
        "<td class='hide Tax_reg_num'>" + $("#TCT-taxRegisterNo").val() + "</td>" +
        "<td class='hide Tax_file_number'>" + $("#TCT-taxFileNo").val() + "</td>" +
        "<td class='hide Address'>" + $("#TCT-address").val() + "</td>" +
        "<td class='hide National_id'>" + $("#TCT-nationalID").val() + "</td>" +
        "<td class='hide Mobile_number'>" + $("#TCT-mobileNumber").val() + "</td>" +
        "<td class='hide Tax_type'>" + $("#TCT-taxType").find("option:selected").val() + "</td>" +
        "<td class='hide Other_tax_type'>" + $("#TCT-taxTableType").find("option:selected").val() + "</td>" +
        "<td class='hide Item_code'>" + $("#TCT-itemCode").val() + "</td>" +
        "<td class='hide Item_name'>" + $("#TCT-itemName").val() + "</td>" +
        "<td class='hide Item_type'>" + $("#TCT-itemType").find("option:selected").val() + "</td>" +
        "<td class='hide Unit_of_measure_id'>" + $("#TCT-unitOfMeasure").find("option:selected").val() + "</td>" +
        "<td class='hide Quantity'>" + $("#TCT-quantity").val() + "</td>" +
        "<td class='hide Unit_price'>" + $("#TCT-unitPrice").val().toString().replace(regRemoveCurrFormate, "") + "</td>" +
         "<td class='hide Total_amount'>" + $("#TCT-totalAmount").val().toString().replace(regRemoveCurrFormate, "") + "</td>" +
         "<td class='hide Total_amount_sys_curr'>" + $("#TCT-totalAmountbySC").val().toString().replace(regRemoveCurrFormate, "") + "</td>" +
         "<td class='hide Discount'>" + $("#TCT-discount").val().toString().replace(regRemoveCurrFormate, "") + "</td>" +
         "<td class='hide Net_amount'>" + $("#TCT-netAmount").val().toString().replace(regRemoveCurrFormate, "") + "</td>" +
        "<td class='hide Tbl_vat_id'>" + $("#TCT-tableVatID").find("option:selected").val() + "</td>" +
        "<td class='hide Table_vat_amount'>" + GetMaskNumber($("#TCT-tableVatAmount").val()) + "</td>" +
        "<td class='hide Table_after_vat_amount'>" + $("#TCT-totalAfterVatTable").val().toString().replace(regRemoveCurrFormate, "") + "</td>" +
        "<td class='hide Vat_id'>" + $("#TCT-vatID").find("option:selected").val().toString().replace(regRemoveCurrFormate, "") + "</td>" +
        "<td class='hide Vat_amount'>" + GetMaskNumber($("#TCT-vatAmount").val()) + "</td>" +
        "<td class='hide Dacutta_id'>" + DeductId + "</td>" +
        "<td class='hide Dacutta_amount'>" + $("#TCT-dacuttaAmount").val() + "</td>" +
        "<td class='hide Tax_group_id'>" + $("#TCT-taxGroupID").val() + "</td>" +
        "<td class='hide FinalCostPrice'></td>" +
        "<td class='hide PageKey'>"+$(".PageKey").val()+"</td>" +
        + "</tr>"

    $(document).find('#TCT-taxTblBody').append(x);
    // clear data after add and return tax in default
    $('.clearTax').val('');
    $('#TCT-tableVatID').prop('disabled', false);
    $('#TCT-tableVatAmount').prop('disabled', false);
    $("#TCT-tableVatID option").show();
    $("#TCT-vatID option").show();
    $('.taxTypeEffect').show();
    $('.taxTableTypeEffect').show();
    $("#TaxPop").modal("hide");

}

// Refrech data in update and cancel update and retrive it to save mode
function RefreshInUpdateAndCancel() {
    $('#TCT-btnAddTaxDetails').prop('disabled', false);
    $('#TCT-btnUpdateTaxDetails').prop('disabled', true);
    $('#TCT-btnCancelUpdateTaxDetails').prop('disabled', true);
    $('#TCT-itemName').prop('disabled', false);
    $('.clearTax').val('');
    $("#TCT-tableVatID option").show();
    $("#TCT-vatID option").show();
}
$('#TaxPop').on('hidden.bs.modal', function () {
    var vatAmount = 0;
    var ThistableVatAmount = 0;
    var dacuttaAmount = 0;
    $('#TCT-taxTable > tbody > tr').each(function () {
        if (!isNaN(parseFloat($(this).find('td:eq(13)').text().replace(regRemoveCurrFormate, "")))) {
            vatAmount = parseFloat($(this).find('td:eq(13)').text().replace(regRemoveCurrFormate, ""));
        }
        if (!isNaN(parseFloat($(this).find('td:eq(10)').text().replace(regRemoveCurrFormate, "")))) {
            ThistableVatAmount = parseFloat($(this).find('td:eq(10)').text().replace(regRemoveCurrFormate, ""));

        }
        if (!isNaN(parseFloat($(this).find(".TblDecuttaId").text().replace(regRemoveCurrFormate, "")))) {
            dacuttaAmount = parseFloat($(this).find(".TblDecuttaId").text().replace(regRemoveCurrFormate, ""));
        }
    })
    $("#TaxMask").val((parseFloat(ThistableVatAmount) + parseFloat(vatAmount) - parseFloat(dacuttaAmount))).trigger("mask.maskMoney").trigger("change").trigger("focusout")

});
$("#TCT-doneTaxDetails").click(function () {
    if (window.location.href.indexOf("/C_TaxTransaction/CompanyTaxTransaction") == -1) {
        $("#TCT-btnAddTaxDetails").trigger("click");
    }
})
function CalcTotalVal() {
    if (window.location.href == "/Inventory/Inv_receive_po/Create") {
        var ThisVatAmount = GetMaskNumber($('#TCT-vatAmount').val());
        var ThisTableVatAmount = GetMaskNumber($("#TCT-tableVatAmount").val())
        if (!ThisVatAmount) {
            ThisVatAmount = 0;
        }
        if (!ThisTableVatAmount) {
            ThisTableVatAmount = 0;
        }
        $("#TaxTotalvatAmount").val(setSystemCurrFormate(ThisVatAmount + ThisTableVatAmount));
        return (ThisVatAmount + ThisTableVatAmount);
    } else {
        var ThisVatAmount = GetMaskNumber($('#TCT-vatAmount').val());
        var ThisTableVatAmount = GetMaskNumber($("#TCT-tableVatAmount").val())
        if (!ThisVatAmount) {
            ThisVatAmount = 0;
        }
        if (!ThisTableVatAmount) {
            ThisTableVatAmount = 0;
        }
        $("#TaxTotalvatAmount").val(setSystemCurrFormate(ThisVatAmount + ThisTableVatAmount));
        return (ThisVatAmount + ThisTableVatAmount);
    }
}
function CheckIsMainPage() {
    return window.location.href.indexOf("/C_TaxTransaction/CompanyTaxTransaction") != -1
}
$(document).on("click", "#TaxAdd", function () {
    InsertTransactionData($("#TCGE-CompanyID").text(), $("#BostingToORThrow").val(), $("#TCT-PostingDate").val(), $("#TCT-JEDate").val(), $("#TCGE-Reference").val(), $("#TCGE-CurrencyID").val(), $("#TCGE-SystemRate").val(), $("#TCGE-TransactionRate").val(), "Tax", "Tax", "", "",
        InsertIntoTax, null, true)

})
function InsertIntoTax(JournalEntryNumber) {
    var Data = [];
    var StatmentType=null;
    try {
        StatmentType = $(document).find("#StatementType").val();
    } catch (err) {

    }
    $('#TCT-taxTable > tbody > tr').each(function () {
        Data.push({
            Doc_type: $(this).find(".Doc_type").text(),
            Doc_num: $(this).find(".Doc_num").text(),
            Date: $(this).find(".Date").text(),
            Vendor_name: $(this).find(".Vendor_name").text(),
            Tax_reg_num: $(this).find(".Tax_reg_num").text(),
            Tax_file_number: $(this).find(".Tax_file_number").text(),
            Address: $(this).find(".Address").text(),
            National_id: $(this).find(".National_id").text(),
            Mobile_number: $(this).find(".Mobile_number").text(),
            Tax_type: $(this).find(".Tax_type").text(),
            Other_tax_type: $(this).find(".Other_tax_type").text(),
            Item_code: $(this).find(".Item_code").text(),
            Item_name: $(this).find(".Item_name").text(),
            Item_type: $(this).find(".Item_type").text(),
            Unit_of_measure_id: $(this).find(".Unit_of_measure_id").text(),
            Quantity: $(this).find(".Quantity").text(),
            Unit_price: $(this).find(".Unit_price").text(),
            Total_amount: $(this).find(".Total_amount").text(),
            Total_amount_sys_curr: $(this).find(".Total_amount_sys_curr").text(),
            Discount: $(this).find(".Discount").text(),
            Net_amount: $(this).find(".Net_amount").text(),
            Tbl_vat_id: $(this).find(".Tbl_vat_id").text(),
            Table_vat_amount: $(this).find(".Table_vat_amount").text(),
            Table_after_vat_amount: $(this).find(".Table_after_vat_amount").text(),
            Vat_id: $(this).find(".Vat_id").text(),
            Vat_amount: $(this).find(".Vat_amount").text(),
            Dacutta_id: $(this).find(".Dacutta_id").text(),
            Dacutta_amount: $(this).find(".Dacutta_amount").text().replace(regRemoveCurrFormate, ''),
            Tax_group_id: $(this).find(".Tax_group_id").text(),
            Final_cost_price: GetMaskNumber($(this).find(".FinalCostPrice").text()),
            Journal_number: JournalEntryNumber,
            Total_vat_amount: parseFloat($(this).find(".TblTotalVatamount").text().replace(regRemoveCurrFormate, '')),
            Page_key: $(this).find(".PageKey").text(),
            Statment_type: StatmentType
        });
    });
    $.ajax({
        url: "/C_TaxTransaction/AddTax",
        contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
        method: "POST",
        data: {
            Tax: Data
        },
        success: function (data) {
            if (data == 1) {
                if (CheckIsMainPage()) {
                    if ($("#BostingToORThrow").val() == 2) {
                        $.ajax({
                            url: "/Bus/GetPotingNumber?JornalEntry=" + JournalEntryNumber,
                            method: "POST",
                            success: function (data) {
                                openWindowWithPost('/C_ReportsPrint/Done?searchNumber=' + data)
                                //window.open(
                                //    '/C_ReportsPrint/Done?searchNumber=' + data,
                                //    '_blank'
                                //);
                                RedirectInt(window.location.href)
                            }
                        })
                    } else {
                        RedirectInt(window.location.href)
                    }
                }
            } else {
                if (CheckIsMainPage()) {
                    if ($("#BostingToORThrow").val() == 2) {
                        $.ajax({
                            url: "/Bus/GetPotingNumber?JornalEntry=" + JournalEntryNumber,
                            method: "POST",
                            success: function (data) {
                                openWindowWithPost('/C_ReportsPrint/Done?searchNumber=' + data)

                            }
                        })
                    } else {
                        RedirectInt(window.location.href)

                    }
                }
            }
        }
    })
}
//function AddToTransactionTbl(ID, accountID, accountName, originalAmount, debit, credit, c_AID) {

//    var x = "<tr>"
//        + "<td>" + accountID + "</td>" +
//        "<td>" + accountName + "</td>" +
//        "<td>" + originalAmount + "</td>" +
//        "<td>" + debit + "</td>" +
//        "<td>" + credit + "</td>" +
//        "<td>" + c_AID + "</td>" +
//        "<td>" + ID + "</td>" +
//        + "</tr>"
//    $('#TCT-TTbl').append(x);
//}