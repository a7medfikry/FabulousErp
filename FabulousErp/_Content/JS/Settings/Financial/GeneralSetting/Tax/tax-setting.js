$(document).ready(function () {

    var AccountIDCheck = $("#STA-AccountIDCheck").text();
    if (AccountIDCheck === "NotExist") {
        Talert("This Company Not Have Any Accounts ID..!");
        window.location.href = "/Home/Financial_Home";
    }
    /*--------------------------------------------
    1.0:: Company-tax-Setting 
    2.0:: Branch-tax-Setting
    3.0:: Factory-tax-Setting
    ---------------------------------------------*/

    /*----------------------------------
     *  1.0:: Company -tax-Setting
     ----------------------------------*/
    // ------- ajax to get Accounts-ID related to company-ID-Login
    // ------- ajax to get Tax-Codes related to company-ID-Login
    var companyID = $('#STA-companyID').val();

    //--------- ajax to get Company-Account-Name related to Account-ID
    $("#STA-companyaccountID").change(function () {
        var companyaccountID = $(this).val();
        $.ajax({
            type: "GET",
            url: "/Tax/GetCompanyAccountName?companyaccountID=" + companyaccountID,
            success: function (result) {
                $("#STA-companyaccountname").val(result);
            }
        });
    });

    //-------- change value of checkbox before Insert to database
    $("#STA-companyprint").change(function () {
        if ($('#STA-companyprint').is(":checked")) {
            $(this).attr('value', true);
        }
        else {
            $(this).attr('value', false);
        }
    });

    //-------------------- validation on keydown
    $("#STA-companycode").keyup(function () {
        if ($(this).val().length === 0) {
            $(this).css("border", "1px solid red");
        } else {
            $(this).css("border", "");
        }
    });
    $("#STA-companydesc").keyup(function () {
        if ($(this).val().length === 0) {
            $(this).css("border", "1px solid red");
        } else {
            $(this).css("border", "");
        }
    });
    $("#STA-companypercentage").keyup(function () {
        if ($(this).val().length === 0) {
            $(this).css("border", "1px solid red");
        } else {
            $(this).css("border", "");
        }
    });
    $("#STA-companytype").change(function () {
        if ($(this).val().length === 0) {
            $(this).css("border", "1px solid red");
        } else {
            $(this).css("border", "");
        }
    });
    $("#STA-companyaccountID").change(function () {
        if ($(this).val().length === 0) {
            $(this).css("border", "1px solid red");
        } else {
            $(this).css("border", "");
        }
    });

    //------------------ ajax to search by Taxe-code related to company-ID
    $("#STA-companyTaxSearch").change(function () {
        var taxcode = $(this).val();
        var companyID = $("#STA-companyID").val();
        $("#STA-CompanySave").prop("disabled", true);
        $("#STA-CompanyUpdate").prop("disabled", false);
        $("#STA-companycode").prop("disabled", true);
        $("#STA-companycode").val("");
        $("#STA-companyERROR").text("");
        $.ajax({
            type: "GET",
            url: "/Tax/TaxSearch?taxcode=" + taxcode + "&companyID=" + companyID,
            success: function (result) {
                //$("#STA-companytype").val(result.TaxType);
                $("#STA-GroupID").val(result.TaxGroupID);
                $("#STA-companyaccountID").val(result.AccountID);
                $("#STA-companyaccountname").val(result.AccountName);
                $("#STA-companydesc").val(result.TaxDescribtion);
                $("#STA-companymintax").val(result.MinTaxable);
                $("#STA-companymaxtax").val(result.MaxTaxable);
                $("#Transaction_type").val(result.Transaction_type);
                if (result.PrintDocument === true) {
                    $("#STA-companyprint").attr('value', true);
                    $("#STA-companyprint").prop('checked', true);
                }
                else {
                    $("#STA-companyprint").attr('value', false);
                    $("#STA-companyprint").prop('checked', false);
                }

                if (result.TaxPercentage !== null) {
                    $("#STA-companypercentage").show();
                    $("#STA-companyAmount").hide();
                    $("#byPercentage").prop('checked', true);
                    $("#byAmount").prop('disabled', true);
                    $("#STA-companypercentage").val(result.TaxPercentage);
                } else {
                    $("#STA-companyAmount").show();
                    $("#STA-companypercentage").hide();
                    $("#byAmount").prop('checked', true);
                    $("#byPercentage").prop('disabled', true);
                    $("#STA-companyAmount").val(result.TaxAmount);
                }
            }
        });
    });

    //---------------- change between Amount & Percentage
    $('#byAmount').click(function () {
        $('#STA-companypercentage').val("");
        $('#STA-companypercentage').hide();
        $('#STA-companyAmount').show();
    });
    $('#byPercentage').click(function () {
        $('#STA-companypercentage').show();
        $('#STA-companyAmount').val("");
        $('#STA-companyAmount').hide();
    });

    // --------------- Button to Save Company-Tax-Setting
    $("#STA-CompanySave").click(function () {
        var Taxcode = $("#STA-companycode").val(),
            //Taxtype = $("#STA-companytype").val(),
            TaxDescribtion = $("#STA-companydesc").val(),
            TaxPercentage = $("#STA-companypercentage").val(),
            TaxAmount = $("#STA-companyAmount").val(),
            AccountID = $('#STA-companyaccountID').val(),
            CompanyID = $('#STA-companyID').val(),
            AccountName = $("#STA-companyaccountname").val(),
            MinAmount = $("#STA-companymintax").val(),
            MaxAmount = $("#STA-companymaxtax").val(),
            PrintDocument = $("#STA-companyprint").val(),
            taxGroupID = $('#STA-GroupID').val(),
            check = true;
        //------------Check if there's empty field-------------
        if (Taxcode.length === 0) {
            $("#STA-companycode").css("border-color", "red");
            check = false;
        }
        else {
            $("#STA-companycode").css("border-color", "");
        }

        //if (Taxtype.length === 0) {
        //    $("#STA-companytype").css("border", "1px solid red");
        //    check = false;
        //}
        //else {
        //    $("#STA-companytype").css("border", "");
        //}
        if (taxGroupID.length === 0) {
            $('#STA-GroupID').css('border-color', 'red');
            check = false;
        } else {
            $('#STA-GroupID').css('border-color', '');
        }

        if (TaxDescribtion.length === 0) {
            $("#STA-companydesc").css("border", "1px solid red");
            check = false;
        }
        else {
            $("#STA-companydesc").css("border", "");
        }

        if (!TaxPercentage && !TaxAmount) {
            $("#STA-companypercentage").css("border", "1px solid red");
            $("#STA-companyAmount").css("border", "1px solid red");
            check = false;
        }
        else {
            $("#STA-companypercentage").css("border", "");
            $("#STA-companyAmount").css("border", "");
        }

        if ($("#STA-companyID").val().length === 0) {
            $("#STA-companyID").css("border", "1px solid red");
            check = false;
        }
        else {
            $("#STA-companyID").css("border", "");
        }

        if ($("#STA-companyaccountID").val().length === 0) {
            $("#STA-companyaccountID").css("border", "1px solid red");
            check = false;
        }
        else {
            $("#STA-companyaccountID").css("border", "");
        }
        //----------------------------------------------------------------
        if (check === true) {
            $.ajax({
                type: "POST",
                url: "/Tax/SaveCompanyTax?Taxcode=" + Taxcode + "&taxGroupID=" + taxGroupID + "&TaxDescribtion=" + TaxDescribtion + "&TaxPercentage=" + TaxPercentage + "&TaxAmount=" + TaxAmount + "&AccountID=" + AccountID + "&CompanyID=" + CompanyID + "&MinAmount=" + MinAmount + "&MaxAmount=" + MaxAmount + "&PrintDocument=" + PrintDocument
                    + "&TransactionType=" + $("#Transaction_type").find("option:selected").val(),
                success: function (result) {
                    if (result == "false") {
                        $("#STA-companyERROR").text("This (Account ID) Is Already Linked To This (Tax Code)");
                    }
                    else if (result == "falsetax") {
                        $("#STA-companyERROR").text("This Tax Code Is Not Valid !!");
                    }
                    else {
                        $("#STA-companyERROR").text("");
                        Talert('Tax Saved Successfuly');
                        RedirectInt(window.location.href,100);
                    }
                }
            });
        }


    });

    // --------------- Button to Update Company-Tax-Setting
    $("#STA-CompanyUpdate").click(function () {
        var Taxcode = $("#STA-companyTaxSearch").val();
        //var Taxtype = $("#STA-companytype").val();
        var TaxDescribtion = $("#STA-companydesc").val();
        var TaxPercentage = $("#STA-companypercentage").val();
        var TaxAmount = $("#STA-companyAmount").val();
        var AccountID = $('#STA-companyaccountID').val();
        var CompanyID = $('#STA-companyID').val();
        var AccountName = $("#STA-companyaccountname").val();
        var MinAmount = $("#STA-companymintax").val();
        var MaxAmount = $("#STA-companymaxtax").val();
        var PrintDocument = $("#STA-companyprint").val();
        var taxGroupID = $('#STA-GroupID').val();
        var Transaction_type = $('#Transaction_type').find("option:selected").val();
        // var SelectPrintDocument = $("#")
        $.ajax({
            type: "POST",
            url: "/Tax/CompanyTaxUpdate?Taxcode=" + Taxcode + "&taxGroupID=" + taxGroupID + "&TaxDescribtion=" + TaxDescribtion + "&TaxPercentage=" + TaxPercentage + "&TaxAmount=" + TaxAmount + "&AccountID=" + AccountID + "&AccountName=" + AccountName + "&CompanyID=" + CompanyID + "&MinAmount=" + MinAmount + "&MaxAmount=" + MaxAmount + "&PrintDocument=" + PrintDocument + "&Transaction_type=" + Transaction_type,
            success: function (result) {
                if (result == "True") {
                    $("#STA-companyERROR").text("");
                    $("#STA-companySuccess").text("Records Updated Successfuly");
                    $("#STA-companySuccess").show();
                    $("#STA-companySuccess").fadeOut(1500);
                }
            }
        });
    });

    $("#STA-Reset").click(function () {
        location.reload();
    });



    /*----------------------------------
     * 2.0:: Branch-tax-Setting
   ----------------------------------*/
    // ------- ajax to get branch-name
    // ------- ajax to get Accounts-ID related to branch-ID
    $("#STA-branchID").change(function () {
        var branchID = $(this).val();
        if (branchID.length === 0) {
            $(this).css("border", "1px solid red");
            ResetBranch();
        } else {
            $(this).css("border", "");
            $.ajax({
                type: "GET",
                url: "/Tax/GetBranchName?branchID=" + branchID,
                success: function (result) {
                    $("#STA-branchname").val(result);
                }
            });
            $.ajax({
                type: "GET",
                url: "/Tax/GetBranchAccountsID?branchID=" + branchID,
                success: function (result) {
                    if (result.length > 0) {
                        $("#STA-branchaccountID").empty();
                        $("#STA-branchaccountID").append($('<option/>', {
                            value: "",
                            text: "-Choose-"
                        })
                        );
                        $.each(result, function (index, row) {
                            $("#STA-branchaccountID").append("<option value='" + row.B_AID + "'>" + row.Branch_AccountsID + "(" + row.AccountName + ")" + "</option>");
                        });
                        $("#STA-branchaccountID").prop("disabled", false);
                    }
                    else {
                        $("#STA-branchaccountID").prop("disabled", true);
                    }
                }
            });
        }
    });

    //--------- ajax to get Branch-Account-Name related to Branch-ID
    $("#STA-branchaccountID").change(function () {
        var branchaccountID = $(this).val();
        $.ajax({
            type: "GET",
            url: "/Tax/GetBranchAccountName?branchaccountID=" + branchaccountID,
            success: function (result) {
                $("#STA-branchaccountname").val(result);
            }
        });
    });

    //--------- ajax to get Details related to Company-Tax-Code
    $("#STA-branchcode").change(function () {
        var taxcode = $(this).val();
        $.ajax({
            type: "GET",
            url: "/Tax/GetDetails?taxcode=" + taxcode,
            success: function (result) {
                //$("#STA-branchtype").val(result[i].TaxType);
                $("#STA-branchdesc").val(result.TaxDescribtion);
                $('#STA-BranchGroupID').val(result.TaxGroupID);
            }
        });
    });

    //--------- Button to Save Branch-Tax-Setting
    $("#STA-BranchSave").click(function () {
        var BranchID = $("#STA-branchID").val();
        var Taxcode = $("#STA-branchcode").val();
        var AccountID = $('#STA-branchaccountID').val();
        var check = true;

        //------------Check if there's empty field-------------
        if (BranchID.length === 0) {
            $("#STA-branchID").css("border", "1px solid red");
            check = false;
        }
        else {
            $("#STA-branchID").css("border", "");
        }

        if (Taxcode.length === 0) {
            $("#STA-branchcode").css("border", "1px solid red");
            check = false;
        }
        else {
            $("#STA-branchcode").css("border", "");
        }

        if (AccountID.length === 0) {
            $("#STA-branchaccountID").css("border", "1px solid red");
            check = false;
        }
        else {
            $("#STA-branchaccountID").css("border", "");
        }
        //--------------------------------------------------------
        if (check === true) {
            $.ajax({
                type: "POST",
                url: "/Tax/SaveBranchTax?Taxcode=" + Taxcode + "&AccountID=" + AccountID,
                success: function (result) {
                    if (result == "False") {
                        $("#STA-branchERROR").text("This (Account ID) Is Already Linked To This (Tax Code)");
                    }
                    else {
                        $("#STA-branchERROR").text("");
                        Talert('Tax Saved Successfuly');
                    }
                }
            });
        }

    });

    // --------------- Button to Update Branch-Tax-Setting
    $("#STA-BranchUpdate").click(function () {
        var Taxcode = $("#STA-branchcode").val();
        var AccountID = $('#STA-branchaccountID').val();
        $.ajax({
            type: "POST",
            url: "/Tax/BranchTaxUpdate?Taxcode=" + Taxcode + "&AccountID=" + AccountID,
            success: function (result) {
                if (result == "True") {
                    $("#STA-branchERROR").text("");
                    $("#STA-branchSuccess").text("Records Updated Successfuly");
                    $("#STA-branchSuccess").show();
                    $("#STA-branchSuccess").fadeOut(1500);
                } else if (result == "False") {
                    $("#STA-branchERROR").text("This Account Is Not Linked To Tax-Code To Update");
                }
            }
        });
    });

    // --------------- Button to Clear Branch
    $('#STA-BranchReset').click(function () {
        ResetBranch();
    });



    /*----------------------------------
    * 3.0:: Factory-tax-Setting
  ----------------------------------*/
    // ------- ajax to get factory-name
    // ------- ajax to get Accounts-ID related to factory-ID
    $("#STA-factoryID").change(function () {
        var factoryID = $(this).val();
        if (factoryID.length === 0) {
            $(this).css("border", "1px solid red");
            ResetFactory();
        } else {
            $(this).css("border", "");
            $.ajax({
                type: "GET",
                url: "/Tax/GetFactoryName?factoryID=" + factoryID,
                success: function (result) {
                    $("#STA-factoryname").val(result);
                }
            });
            $.ajax({
                type: "GET",
                url: "/Tax/GetFactoryAccountsID?factoryID=" + factoryID,
                success: function (result) {
                    if (result.length > 0) {
                        $("#STA-factoryaccountID").empty();
                        $("#STA-factoryaccountID").append($('<option/>', {
                            value: "",
                            text: "-Choose-"
                        })
                        );
                        $.each(result, function (index, row) {
                            $("#STA-factoryaccountID").append("<option value='" + row.F_AID + "'>" + row.Factory_AccountsID + "(" + row.AccountName + ")" + "</option>");
                        });
                        $("#STA-factoryaccountID").prop("disabled", false);
                    }
                    else {
                        $("#STA-factoryaccountID").prop("disabled", true);
                    }
                }
            });
        }
    });

    //--------- ajax to get Factory-Account-Name related to Factory-ID
    $("#STA-factoryaccountID").change(function () {
        var factoryaccountID = $(this).val();
        $.ajax({
            type: "GET",
            url: "/Tax/GetFactoryAccountName?factoryaccountID=" + factoryaccountID,
            success: function (result) {
                $("#STA-factoryaccountname").val(result);
            }
        });
    });

    //--------- ajax to get Details related to Company-Tax-Code
    $("#STA-factorycode").change(function () {
        var taxcode = $(this).val();
        $.ajax({
            type: "GET",
            url: "/Tax/GetDetails?taxcode=" + taxcode,
            success: function (result) {
                //$("#STA-factorytype").val(result[i].TaxType);
                $("#STA-factorydesc").val(result.TaxDescribtion);
                $('#STA-FactoryGroupID').val(result.TaxGroupID);
            }
        });
    });

    //--------- Button to Save Factory-Tax-Setting
    $("#STA-FactorySave").click(function () {
        var FactoryID = $("#STA-factoryID").val();
        var Taxcode = $("#STA-factorycode").val();
        var AccountID = $('#STA-factoryaccountID').val();
        var check = true;

        //------------Check if there's empty field-------------
        if (FactoryID.length === 0) {
            $("#STA-factoryID").css("border", "1px solid red");
            check = false;
        }
        else {
            $("#STA-factoryID").css("border", "");
        }

        if (Taxcode.length === 0) {
            $("#STA-factorycode").css("border", "1px solid red");
            check = false;
        }
        else {
            $("#STA-factorycode").css("border", "");
        }

        if (AccountID.length === 0) {
            $("#STA-factoryaccountID").css("border", "1px solid red");
            check = false;
        }
        else {
            $("#STA-factoryaccountID").css("border", "");
        }
        //--------------------------------------------------------
        if (check === true) {
            $.ajax({
                type: "POST",
                url: "/Tax/SaveFactoryTax?Taxcode=" + Taxcode + "&AccountID=" + AccountID,
                success: function (result) {
                    if (result == "False") {
                        $("#STA-factoryERROR").text("This (Account ID) Is Already Linked To This (Tax Code)");
                    }
                    else {
                        $("#STA-factoryERROR").text("");
                        Talert('Tax Saved Successfuly');
                    }
                }
            });
        }

    });

    // --------------- Button to Update Factory-Tax-Setting
    $("#STA-FactoryUpdate").click(function () {
        var Taxcode = $("#STA-factorycode").val();
        var AccountID = $('#STA-factoryaccountID').val();
        $.ajax({
            type: "POST",
            url: "/Tax/FactoryTaxUpdate?Taxcode=" + Taxcode + "&AccountID=" + AccountID,
            success: function (result) {
                if (result == "True") {
                    $("#STA-factoryERROR").text("");
                    $("#STA-factorySuccess").text("Records Updated Successfuly");
                    $("#STA-factorySuccess").show();
                    $("#STA-factorySuccess").fadeOut(1500);
                } else if (result == "False") {
                    $("#STA-factoryERROR").text("This Account Is Not Linked To Tax-Code To Update");
                }
            }
        });
    });

    // --------------- Button to Clear Factory
    $('#STA-FactoryReset').click(function () {
        ResetFactory();
    });







    // --------- Drop-down Validation Change
    $("#STA-branchcode").change(function () {
        if ($(this).val().length === 0) {
            $(this).css("border", "1px solid red");
        } else {
            $(this).css("border", "");
        }
    });
    $("#STA-branchaccountID").change(function () {
        if ($(this).val().length === 0) {
            $(this).css("border", "1px solid red");
        } else {
            $(this).css("border", "");
        }
    });
    $("#STA-factorycode").change(function () {
        if ($(this).val().length === 0) {
            $(this).css("border", "1px solid red");
        } else {
            $(this).css("border", "");
        }
    });
    $("#STA-factoryaccountID").change(function () {
        if ($(this).val().length === 0) {
            $(this).css("border", "1px solid red");
        } else {
            $(this).css("border", "");
        }
    });


    // ----------- Add Group Of Tax
    $('#STA-taxGroupSave').click(function () {

        var taxGroupID = $('#STA-taxGroupID').val(),
            taxGroupDescription = $('#STA-taxGroupDescription').val(),
            taxType = $('#STA-taxType').val(),
            test = true;

        if (taxGroupID.length === 0) {
            $('#STA-taxGroupID').css('border-color', 'red');
            test = false;
        } else {
            $('#STA-taxGroupID').css('border-color', '');
        }

        if (taxGroupDescription.length === 0) {
            $('#STA-taxGroupDescription').css('border-color', 'red');
            test = false;
        } else {
            $('#STA-taxGroupDescription').css('border-color', '');
        }

        if (taxType.length === 0) {
            $('#STA-taxType').css('border-color', 'red');
            test = false;
        } else {
            $('#STA-taxType').css('border-color', '');
        }

        if (test === true) {

            $.ajax({
                type: "GET",
                url: "/Tax/AddTaxGroup?taxGroupID=" + taxGroupID + "&description=" + taxGroupDescription + "&type=" + taxType,
                success: function (result) {
                    if (result === false) {
                        $('#STA-groupError').text('Tax Group ID Not Valid..!');
                    } else {
                        location.reload();
                    }
                }
            });

        }

    });

});


// --------------- Button to Reset
function ResetBranch() {
    $("#STA-branchname").val("");
    $("#STA-branchcode").val("");
    $("#STA-branchtype").val("");
    $("#STA-branchdesc").val("");
    $("#STA-branchaccountname").val("");
    $("#STA-branchaccountID").val("");
}
function ResetFactory() {
    $("#STA-factoryname").val("");
    $("#STA-factorycode").val("");
    $("#STA-factorytype").val("");
    $("#STA-factorydesc").val("");
    $("#STA-factoryaccountname").val("");
    $("#STA-factoryaccountID").val("");
}
function ResetCompany() {
    $("#STA-companycode").val("");
    $("#STA-companyTaxSearch").val("");
    $("#STA-companytype").val("");
    $("#STA-companyaccountID").val("");
    $("#STA-companyaccountname").val("");
    $("#STA-companydesc").val("");
    $("#STA-companypercentage").val("");
    $("#STA-companymintax").val("");
    $("#STA-companymaxtax").val("");
    $("#STA-companyprint").val("");
    $("#STA-companyselectprint").val("");
    $("#STA-CompanyUpdate").prop("disabled", true);
    $("#STA-CompanySave").prop("disabled", false);
    $("#STA-companycode").prop("disabled", false);
}



//$.ajax({
    //    type: "GET",
    //    url: "/Tax/GetCompanyAccountsID?companyID=" + companyID,
    //    success: function (result) {
    //        if (result.length > 0) {
    //            $("#STA-companyaccountID").empty();
    //            $("#STA-companyaccountID").append($('<option/>', {
    //                value: "",
    //                text: "-Choose-"
    //            })
    //            );
    //            $.each(result, function (index, row) {
    //                $("#STA-companyaccountID").append("<option value='" + row.C_AID + "'>" + row.Company_AccountsID + "(" + row.AccountName + ")" + "</option>");
    //            });
    //        }
    //        else {
    //            $("#STA-companyaccountID").prop("disabled", true);
    //        }
    //    }
    //});

    //$.ajax({
    //    type: "GET",
    //    url: "/Tax/GetCompanyTaxCode?companyID=" + companyID,
    //    success: function (result) {
    //        if (result.length > 0) {
    //            $("#STA-companyTaxSearch").empty();
    //            $("#STA-companyTaxSearch").append($('<option/>', {
    //                value: "",
    //                text: "-Choose-"
    //            })
    //            );
    //            $.each(result, function (index, row) {
    //                $("#STA-companyTaxSearch").append("<option value='" + row.CT_ID + "'>" + row.CompanyTaxs + "</option>");
    //            });
    //        } else {
    //            $("#STA-companyTaxSearch").empty();
    //        }
    //    }
    //});