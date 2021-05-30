$(document).ready(function () {

    $("#TCGE-JEDate").prop('disabled', true);
    $("#TCGE-PostingDate").prop('disabled', true);
    $("#TCGE-CurrencyID").prop('disabled', true);
    $("#TCGE-SystemRate").prop('disabled', true);
    $("#TCGE-TransactionRate").prop('disabled', true);
    $("#TCGE-DiffrenceRate").prop('disabled', true);
    $("#TCGE-Reference").prop('disabled', true);
    $("#TCGE-AccountID").prop('disabled', true);
    $("#TCGE-BtnAddRow").prop('disabled', true);

    $("#TCGE-Debit").prop('disabled', false);
    $("#TCGE-Credit").prop('disabled', false);


    var companyID = $("#TCGE-CompanyID").text();

    $('#TCS-JENum').change(function () {
        GetJvTransactionData($(this).val())
    });
    function GetJvTransactionData(postingNumber) {
        
        ClearData();
        $("#TCGE-AccountID option").show();
        $('#TCGE-ConfirmUpdateRecord').prop('disabled', true);
        $('#TCGE-CancelUpdateRecord').prop('disabled', true);
        $('#TCS-NoAC').text("");

        if (postingNumber.length > 0) {
            $.ajax({
                url: "/api/TransactionApi/GetTransactionData?postingNumber=" + postingNumber,
                method: "GET",
                success: function (data) {

                    if (data.ShowHeader.PostingKey !== "TCGE") {
                        $('#TCS-Update').prop('disabled', true);
                        $("#TCGE-AccountID").prop('disabled', true);
                        $("#TCGE-BtnAddRow").prop('disabled', true);
                        var notGeneralEntry = true;
                    } else {
                        $("#TCGE-AccountID").prop('disabled', false);
                        $("#TCGE-BtnAddRow").prop('disabled', false);
                    }

                    $("#TCGE-JEDate").val(data.ShowHeader.TransactionDate);
                    $("#TCGE-PostingDate").val(data.ShowHeader.PostingDate);
                    $("#TCGE-CurrencyID").val(data.ShowHeader.CurrencyID);

                    if (data.ShowHeader.CurrencyID != companyID) {
                        $('#TCCR-rateField').show();
                        $(".TCGE-HSOAByC").show();
                        $("#TCGE-Debit").prop('disabled', true);
                        $("#TCGE-Credit").prop('disabled', true);

                        var iso = $("#TCGE-CurrencyID option:selected").text();
                        $("#TCGE-OriginalAmount").maskMoney({ suffix: ' ' + iso + '', thousands: ',', decimal: '.', precision: parseInt(requiredDecimalNum) });
                        $("#TCGE-HardGurrencyFormate").maskMoney({ suffix: ' ' + iso + '', thousands: ',', decimal: '.', precision: parseInt(requiredDecimalNum) });

                        var hardCurrTest = true;
                    } else {
                        $('#TCCR-rateField').hide();
                        $(".TCGE-HSOAByC").hide();
                        $("#TCGE-Debit").prop('disabled', false);
                        $("#TCGE-Credit").prop('disabled', false);
                    }

                    $("#TCGE-SystemRate").val(setSystemCurrFormate(+parseFloat(data.ShowHeader.SystemRate)));
                    $("#TCGE-TransactionRate").val(setSystemCurrFormate(+parseFloat(data.ShowHeader.TransactionRate)));
                    $("#TCGE-DiffrenceRate").val(parseFloat(setSystemCurrFormate(+parseFloat(data.ShowHeader.SystemRate - data.ShowHeader.TransactionRate))));

                    $("#TCGE-Reference").val(data.ShowHeader.Reference);

                    for (let i = 0; i < data.ShowTransactions.length; i++) {

                        var debit = data.ShowTransactions[i].Debit;
                        if (data.ShowTransactions[i].Debit === 0) {
                            debit = "";
                        }

                        var credit = data.ShowTransactions[i].Credit;
                        if (data.ShowTransactions[i].Credit === 0) {
                            credit = "";
                        }
                        RetrieveToMainTbl(data.ShowTransactions[i].AID, data.ShowTransactions[i].AccountName, data.ShowTransactions[i].Document, data.ShowTransactions[i].AccountID, data.ShowTransactions[i].Describtion, data.ShowTransactions[i].OriginalAmount, debit, credit, hardCurrTest, notGeneralEntry);
                    }

                    for (let i = 0; i < data.ShowAnalytics.length; i++) {
                        var debit = data.ShowAnalytics[i].Debit;
                        if (data.ShowAnalytics[i].Debit === null) {
                            debit = "";
                        }

                        var credit = data.ShowAnalytics[i].Credit;
                        if (data.ShowAnalytics[i].Credit === null) {
                            credit = "";
                        }
                        RetrieveToDBAnalyticTbl(data.ShowAnalytics[i].AnalyticID, data.ShowAnalytics[i].DistID, data.ShowAnalytics[i].DistributionID, data.ShowAnalytics[i].DistributionName, data.ShowAnalytics[i].AID, data.ShowAnalytics[i].Describtion, data.ShowAnalytics[i].Percentage, data.ShowAnalytics[i].Amount, debit, credit);
                    }

                    for (let i = 0; i < data.ShowCostCenters.length; i++) {
                        var debit = data.ShowCostCenters[i].Debit;
                        if (data.ShowCostCenters[i].Debit === null) {
                            debit = "";
                        }

                        var credit = data.ShowCostCenters[i].Credit;
                        if (data.ShowCostCenters[i].Credit === null) {
                            credit = "";
                        }

                        var mainCostCenterID = data.ShowCostCenters[i].MainCostCenterID;
                        var costCenterType = 'MainCostCenter';
                        if (data.ShowCostCenters[i].MainCostCenterID === null) {
                            mainCostCenterID = "";
                            costCenterType = 'CostCenter';
                        }

                        var costCenterIDPercentage = data.ShowCostCenters[i].CostCenterIDPercentage;
                        if (data.ShowCostCenters[i].MainCostCenterID === null) {
                            costCenterIDPercentage = "";
                        }
                        RetrieveToDBCostCenterTbl(data.ShowCostCenters[i].CostCenterID, data.ShowCostCenters[i].CAID, data.ShowCostCenters[i].CostAccountID, data.ShowCostCenters[i].CostAccountName, data.ShowCostCenters[i].AID, data.ShowCostCenters[i].Describtion, data.ShowCostCenters[i].Percentage, data.ShowCostCenters[i].Amount, debit, credit, costCenterType, mainCostCenterID, costCenterIDPercentage, data.ShowCostCenters[i].CostCenterName);
                    }
                }
            });
        }
    }
    $('#TCS-searchByBatch').click(function () {
        $('#TCS-BatchID').prop('disabled', false);
        $('#TCS-JENum').prop('disabled', true);
        $('#TCS-JENum').val('');
        ClearData();

        $.ajax({
            type: "GET",
            url: "/C_GeneralEntryTransaction/GetTransactionsBatches",
            success: function (data) {
                $("#TCS-BatchID").empty();
                if (data.length == 0) {

                    $("#TCS-BatchID").append($('<option/>', {
                        value: "",
                        text: "No Batches Created To This Company"
                    })
                    );
                } else {

                    $("#TCS-BatchID").append($('<option/>', {
                        value: "",
                        text: "-Choose-"
                    })
                    );
                    $.each(data, function (index, row) {

                        $("#TCS-BatchID").append("<option value='" + row.CBID + "'>" + row.BatchID + " ( " + row.BatchLocation + " )" + "</option>");

                    });
                }
            }
        });
    });

    $('#TCS-searchByJENum').click(function () {
        $('#TCS-BatchID').prop('disabled', true);
        $('#TCS-BatchID').val('');
        ClearData();

        $.ajax({
            type: "GET",
            url: "/C_GeneralEntryTransaction/GetAllJENum",
            success: function (data) {
                $('#TCS-JENum').prop('disabled', false);
                $("#TCS-JENum").empty();

                if (data.length == 0) {

                    $("#TCS-JENum").append($('<option/>', {
                        value: "",
                        text: "No Transactions Created To This Company"
                    })
                    );
                } else {

                    $("#TCS-JENum").append($('<option/>', {
                        value: "",
                        text: "-Choose-"
                    })
                    );
                    $.each(data, function (index, row) {

                        $("#TCS-JENum").append("<option value='" + row.PostingNumber + "'>" + row.JournalEntryNum + " ( " + row.PostingKey + " )" + "</option>");

                    });
                }
            }
        });
    });

    $('#TCS-BatchID').change(function () {
        var batchID = $(this).val();
        ClearData();
        if (batchID.length > 0) {
            $.ajax({
                type: 'GET',
                url: "/C_GeneralEntryTransaction/GetJENumByBatch?CBID=" + batchID,
                success: function (data) {
                    $('#TCS-JENum').empty();
                    $('#TCS-JENum').prop('disabled', false);
                    if (data.length == 0) {

                        $("#TCS-JENum").append($('<option/>', {
                            value: "",
                            text: "No Transactions Created To This Company"
                        })
                        );
                    } else {
                        $("#TCS-JENum").append($('<option/>', {
                            value: "",
                            text: "-Choose-"
                        })
                        );
                        $.each(data, function (index, row) {
                            $("#TCS-JENum").append("<option value='" + row.PostingNumber + "'>" + row.JournalEntryNum + " ( " + row.PostingKey + " )" + "</option>");
                        });
                    }
                }
            });
        } else {
            $('#TCS-JENum').prop('disabled', true);
            $('#TCS-JENum').val('');
        }
    });

    $('#TCS-Update').click(function () {

        var postingNumber = $('#TCS-JENum').val();

        var rowCount = $('#TCGE-GTbl >tbody >tr').length;
        if (rowCount === 0) {
            $("#TCGE-GlobalError").text("No Data To Update it..!");
        } else {

            var difference = $("#DiffOfDAC").text();

            if (difference !== "0") {
                $("#TCGE-GlobalError").text("Difference Between Debit And Credit Must Be equla 0");
            } else {

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

                $.each($("#TCGE-TDBCost tbody tr"), function () {
                    costArr.push({
                        C_CostCenterID: $(this).find('td:eq(0)').html(),
                        C_CAID: $(this).find('td:eq(1)').html(),
                        C_AID: $(this).find('td:eq(4)').html(),
                        Describtion: $(this).find('td:eq(5)').html(),
                        C_Percentage: $(this).find('td:eq(6)').html().replace('%', ''),
                        C_Amount: $(this).find('td:eq(7)').html().replace(regRemoveCurrFormate, ""),
                        C_Debit: $(this).find('td:eq(8)').html().replace(regRemoveCurrFormate, ""),
                        C_Credit: $(this).find('td:eq(9)').html().replace(regRemoveCurrFormate, ""),
                        C_CostCenterGroupID: $(this).find('td:eq(11)').html(),
                        CostCenterPercentage: $(this).find('td:eq(12)').html().replace('%', '')
                    });
                });


                var data = JSON.stringify({
                    SaveTransaction: mainArr,
                    SaveAnalytic: analyticArr,
                    SaveCost: costArr
                });

                $.ajax({
                    contentType: 'application/json; charset=utf-8',
                    dataType: 'json',
                    method: "post",
                    url: "/api/TransactionApi/SaveNewTransactionData?companyID=" + companyID + "&postingNumber=" + postingNumber,
                    data: data,
                    success: function (result) {
                        Talert(result);
                        location.reload();
                    }
                });

            }
        }

    });

});

function RetrieveToMainTbl(accountID, accountName, document, accountIDTbl, describtion, originalAmount, debit, credit, hardCurrTest, notGeneralEntry) {

    if (hardCurrTest) {
        originalAmount = setHardCurrFormate(+originalAmount);
    } else {
        originalAmount = setSystemCurrFormate(+originalAmount);
    }

    if (notGeneralEntry === true) {

        var content = "<tr class='row_" + accountID + "'>" +
            "<td>" + '<a href="#" class="mr-1 MoreDetailsT"><span class="fa fa-eye"></span></a>' + '<a href="#" class="mr-1" onclick="ShowAnalyticOfTrans(\'' + accountID + '\');"><span class="fa fa-list-ul">Analytic</span></a>' + '<a href="#" onclick="ShowCostOfTrans(\'' + accountID + '\');"><span class="fa fa-list-ul">Cost</span></a>' + "</td>" +
            "<td class='hide-normal TCGE-TblAccID'>" + accountID + "</td>" +
            "<td>" + accountName + "</td>" +
            "<td>" + document + "</td>" +
            "<td class='hide-normal'>" + accountIDTbl + "</td>" +
            "<td class='hide-normal'>" + describtion + "</td>" +
            "<td>" + originalAmount + "</td>" +
            "<td class='sDebitTbl'>" + setSystemCurrFormate(+debit) + "</td>" +
            "<td class='sCreditTbl'>" + setSystemCurrFormate(+credit) + "</td>" +
            "</tr>";

        $("#TCGE-TTbl").append(content);

    } else {

        var content = "<tr class='row_" + accountID + "'>" +
            "<td>" + '<button type="button" class="btn btn-sm mr-1 MoreDetailsT"><span class="fa fa-eye"></span></button>' + '<button type="button" id="dmt" class="btn btn-danger btn-sm mr-1" onclick="DeleteT(\'' + accountID + '\');"><span class="fa fa-trash-o"></span></button>' + '<button type="button" id="emt" class="btn btn-warning btn-sm" onclick="EditT(\'' + accountID + '\');"><span class="fa fa-edit"></span></button>' + "</td>" +
            "<td class='hide-normal TCGE-TblAccID'>" + accountID + "</td>" +
            "<td>" + accountName + "</td>" +
            "<td>" + document + "</td>" +
            "<td class='hide-normal'>" + accountIDTbl + "</td>" +
            "<td class='hide-normal'>" + describtion + "</td>" +
            "<td>" + originalAmount + "</td>" +
            "<td class='sDebitTbl'>" + setSystemCurrFormate(+debit) + "</td>" +
            "<td class='sCreditTbl'>" + setSystemCurrFormate(+credit) + "</td>" +
            "</tr>";

        $("#TCGE-TTbl").append(content);
    }

    //call function that calculate sum of new debit or credit and get defference in footer
    SumDebitAndCredit();

    $("#TCGE-AccountID option[value=" + accountID + "]").hide();
}

function RetrieveToDBAnalyticTbl(analyticID, c_DistID, distributionID, distributionName, accountID, describtion, percentage, amount, debit, credit) {
    var analyticDBContent = "<tr class='DisDBrow_" + accountID + "'>" +
        "<td>" + analyticID + "</td>" +
        "<td>" + c_DistID + "</td>" +
        "<td>" + distributionID + "</td>" +
        "<td>" + distributionName + "</td>" +
        "<td>" + accountID + "</td>" +
        "<td>" + describtion + "</td>" +
        "<td>" + percentage + "</td>" +
        "<td>" + amount + "</td>" +
        "<td>" + debit + "</td>" +
        "<td>" + credit + "</td>" +
        "</tr>";

    $("#TCGE-TAccountAnalyticDB").append(analyticDBContent);
}

function RetrieveToDBCostCenterTbl(costCenterID, c_CAID, costAccountID, costAccountName, accountID, describtion, percentage, amount, debit, credit, costCenterType, MainCostCenterID, CostCenterIDPercentage, costCenterName) {

    var costCenterDBContent = "<tr class='CCAccDBrow_" + accountID + "'>" +
        "<td>" + costCenterID + "</td>" +
        "<td>" + c_CAID + "</td>" +
        "<td>" + costAccountID + "</td>" +
        "<td>" + costAccountName + "</td>" +
        "<td>" + accountID + "</td>" +
        "<td>" + describtion + "</td>" +
        "<td>" + percentage + "</td>" +
        "<td>" + amount + "</td>" +
        "<td>" + debit + "</td>" +
        "<td>" + credit + "</td>" +
        "<td>" + costCenterType + "</td>" +
        "<td>" + MainCostCenterID + "</td>" +
        "<td>" + CostCenterIDPercentage + "</td>" +
        "<td>" + costCenterName + "</td>" +
        "</tr>";

    $("#TCGE-TCostCenterAccountDB").append(costCenterDBContent);

}

function ShowAnalyticOfTrans(id) {

    $("#TCGE-ShowTAccountAnalytic").empty();

    $('#TCS-NoAC').text("");

    if ($(".DisDBrow_" + id + "").length) {

        $("#TCGE-TAccountAnalyticDB").find(".DisDBrow_" + id + "").each(function () {

            var tds = $(this).find('td'),
                c_DistID = tds.eq(1).text(),
                distributionID = tds.eq(2).text(),
                distributionName = tds.eq(3).text(),
                describtion = tds.eq(5).text(),
                percentage = tds.eq(6).text().replace('%', ''),
                amount = tds.eq(7).text();

            //Function that add to analytic tbl taht exist in analytic popup
            RetrieveToAnalyticTbl(c_DistID, distributionID, distributionName, percentage, amount, describtion);

            $('#TCGE-PUShowAnalyticAccount').modal('show');
        });
    } else {
        $('#TCS-NoAC').text('No Analytic To This Account');
    }
}

function RetrieveToAnalyticTbl(c_DistID, distributionID, distributionName, percentage, amount, describtion) {

    var analyticContent = "<tr class='Disrow_" + c_DistID + "'>" +
        "<td>" + distributionID + "</td>" +
        "<td class='hide-normal TCGE-TblDistID'>" + c_DistID + "</td>" +
        "<td>" + distributionName + "</td>" +
        "<td>" + percentage + "%" + "</td>" +
        "<td class='SumAmountTblAnalyticDist'>" + setSystemCurrFormate(+amount) + "</td>" +
        "<td>" + describtion + "</td>" +
        "</tr>";
    $("#TCGE-ShowTAccountAnalytic").append(analyticContent);
}

function ShowCostOfTrans(id) {

    $('#TCGE-TShowCostCenter').empty();

    $('#TCGE-TShowMainCostCenter').empty();

    $('#TCS-NoAC').text("");

    if ($(".CCAccDBrow_" + id + "").length) {

        var getCCDBTbl = $(".CCAccDBrow_" + id + "").find('td');
        var costCenterID = getCCDBTbl.eq(0).text();
        var costCenterType = getCCDBTbl.eq(10).text();
        var mainCostCenterID = getCCDBTbl.eq(11).text();

        if (costCenterType === "CostCenter") {

            $("#TCGE-TblShowCostCenter").show();
            $("#TCGE-TblShowMainCostCenter").hide();

            //show Cost center ID in main data and hide Main cost center from Main Data
            $("#TCGE-PUSHCCHS").show();
            $("#TCGE-PUSHMCCHS").hide();

            $("#TCGE-PUSHCCCostID").text(costCenterID);

            $("#TCGE-TCostCenterAccountDB").find(".CCAccDBrow_" + id + "").each(function () {

                var tds = $(this).find('td'),
                    c_CAID = tds.eq(1).text(),
                    costAccountID = tds.eq(2).text(),
                    costAccountName = tds.eq(3).text(),
                    describtion = tds.eq(5).text(),
                    percentage = tds.eq(6).text().replace('%', ''),
                    amount = tds.eq(7).text(),
                    costCenterType = tds.eq(10).text();

                //Function that add data to cost center table that exist in CC popup
                RetrieveToCostCenterTbl(c_CAID, costAccountID, costAccountName, percentage, amount, describtion, costCenterType);

            });
        } else if (costCenterType === "MainCostCenter") {

            $("#TCGE-TblShowCostCenter").hide();
            $("#TCGE-TblShowMainCostCenter").show();

            //hide Cost center ID in main data and show Main cost center from Main Data
            $("#TCGE-PUSHCCHS").hide();
            $("#TCGE-PUSHMCCHS").show();

            $("#TCGE-PUSHCCMainCostID").text(mainCostCenterID);

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

                //Function that add data to cost center table that exist in CC popup
                RetrieveToCostCenterTbl(c_CAID, costAccountID, costAccountName, percentage, amount, describtion, costCenterType, costCenterID, cCIDPercentage, costCenterName);
            });
        }

        $('#TCGE-PUShowCostCenter').modal('show');

    } else {
        $('#TCS-NoAC').text('No Cost Center To This Account');
    }
}

function RetrieveToCostCenterTbl(c_CAID, CostAccountID, CostAccountName, percentage, amount, describtion, costCenterType, costCenterID, costCenterIDPercentage, costCenterName) {

    if (costCenterType === "CostCenter") {

        var CostContent = "<tr>" +
            "<td class='hide-normal TCGE-TblCCAccID'>" + c_CAID + "</td>" +
            "<td>" + CostAccountID + "</td>" +
            "<td>" + CostAccountName + "</td>" +
            "<td>" + percentage + "%" + "</td>" +
            "<td class='SumAmountTblCostAccount'>" + setSystemCurrFormate(+amount) + "</td>" +
            "<td>" + describtion + "</td>" +
            "<td class='hide-normal'>" + costCenterType + "</td>" +
            "</tr>";
        $("#TCGE-TShowCostCenter").append(CostContent);

    } else if (costCenterType === "MainCostCenter") {

        var CostContent = "<tr>" +
            "<td class='hide-normal TCGE-TblCCAccID'>" + c_CAID + "</td>" +
            "<td>" + costCenterID + "</td>" +
            "<td>" + costCenterIDPercentage + "%" + "</td>" +
            "<td>" + CostAccountID + "</td>" +
            "<td>" + CostAccountName + "</td>" +
            "<td>" + percentage + "%" + "</td>" +
            "<td class='SumAmountTblCostAccount'>" + setSystemCurrFormate(+amount) + "</td>" +
            "<td>" + describtion + "</td>" +
            "<td class='hide-normal'>" + costCenterType + "</td>" +
            "<td class='hide-normal'>" + costCenterName + "</td>" +
            "</tr>";
        $("#TCGE-TShowMainCostCenter").append(CostContent);

    }

}


function ClearData() {
    $("#TCGE-JEDate").val('');
    $("#TCGE-PostingDate").val('');
    $("#TCGE-CurrencyID").val('');
    $("#TCGE-SystemRate").val('');
    $("#TCGE-TransactionRate").val('');
    $("#TCGE-DiffrenceRate").val('');
    $("#TCGE-Reference").val('');
    $("#TCGE-TTbl").empty();
    $("#TCGE-TAccountAnalyticDB").empty();
    $("#TCGE-TCostCenterAccountDB").empty();
    $("#TCGE-TAccountAnalytic").empty();
    $("#TCGE-TCostCenter").empty();
    $("#TCGE-TMainCostCenter").empty();
    $('#DebitTblFoot').text('');
    $('#CreditTblFoot').text('');
    $('#DiffOfDAC').text('');

    $('#CBT-checkbookName').val('');
    $('#CBT-checkbookID').val('');
    $('#CBT_receivedFrom_payTo').val('');
    $('#CBT-amount').val('');
    $('#CBT-checkNumber').val('');
    $('#CBT-dueDate').val('');
    $('#C_TransactionDate').val('');
    $('#C_PostingDate').val('');
}