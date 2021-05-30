$(document).ready(function () {
    $("#YearName").change(function () {
        var Year = $(this).val();
        $("#FinishSavePeriodsBtn").prop("disabled", true);
        $("#FiscalYearPeriodsBody").html("");
        $("#FiscalYearAdjusmentPeriodsBody").html("");
        if (Year.length === 0) {
            $(this).css("border-color", "red");
        } else {
            $("#FinishSavePeriodsBtn").prop("disabled", false);
            $(this).css("border-color", "");
            $.ajax({
                type: "GET",
                url: "/OpenAndClosFiscalPeriods/GetOpenClosePeriods?Year=" + Year,
                success: function (result) {
                    for (var i = 0; i < result.length; i++) {
                        var cbFinancial = "";
                        if (result[i].Financial === true) {
                            cbFinancial = "Checked";
                        }
                        var cbPurchasing = "";
                        if (result[i].Purchasing === true) {
                            cbPurchasing = "Checked";
                        }
                        var cbSales = "";
                        if (result[i].Sales === true) {
                            cbSales = "Checked";
                        }
                        var cbInventory = "";
                        if (result[i].Inventory === true) {
                            cbInventory = "Checked";
                        }
                        var AreaNameData = "";

                        $.each(result[i].AreaNames, function (k, i) {
                            if (i.Allowed) {
                                AreaNameData += "<td><input type='checkbox' id='" + i.Area_name + "' checked='checked'></td>";
                            } else {
                                AreaNameData += "<td><input type='checkbox' id='" + i.Area_name + "'></td>";
                            }
                        })

                        var Data = "<tr class='row_" + result[i].ID + "'>" +
                            "<td class= 'hide-normal'>" + result[i].ID + "</td>" +
                            "<td width='10%'>" + result[i].Period_No + "</td>" +
                            "<td width='15%'>" + result[i].Period_Start_Date + "</td>" +
                            "<td width='15%'>" + result[i].Period_End_Date + "</td>" +
                            AreaNameData
                            + "</tr>";
                       
                            //"<td width='15%'>" + '<input type="checkbox" id="' + result[i].ID + "Financial" + '" ' + cbFinancial + ' >' + "</td>" +
                            //"<td width='15%'>" + '<input type="checkbox" id="' + result[i].ID + "Purchasing" + '" ' + cbPurchasing + ' >' + "</td>" +
                            //"<td width='15%'>" + '<input type="checkbox" id="' + result[i].ID + "Sales" + '" ' + cbSales + ' >' + "</td>" +
                            //"<td width='15%'>" + '<input type="checkbox" id="' + result[i].ID + "Inventory" + '" ' + cbInventory + ' >' + "</td>" +
                        $("#FiscalYearPeriodsBody").append(Data);
                    }
                }
            });
            $.ajax({
                type: "GET",
                url: "/OpenAndClosFiscalPeriods/GetOpenCloseAdjusmentPeriods?Year=" + Year,
                success: function (result) {
                    for (var i = 0; i < result.length; i++) {

                        var cbFinancial = "";
                        if (result[i].Financial === true) {
                            cbFinancial = "Checked";
                        }
                        var cbPurchasing = "";
                        if (result[i].Purchasing === true) {
                            cbPurchasing = "Checked";
                        }
                        var cbSales = "";
                        if (result[i].Sales === true) {
                            cbSales = "Checked";
                        }
                        var cbInventory = "";
                        if (result[i].Inventory === true) {
                            cbInventory = "Checked";
                        }

                        var AreaNameData = "";

                        $.each(result[i].AreaNames, function (k, i) {
                            if (i.Allow_adjust) {
                                AreaNameData += "<td><input type='checkbox' id='" + i.Area_name + "' checked='checked'></td>";
                            } else {
                                AreaNameData += "<td><input type='checkbox' id='" + i.Area_name + "'></td>";
                            }
                        })

                        var Data = "<tr class='row_" + result[i].ID + "'>" +
                            "<td class= 'hide-normal'>" + result[i].ID + "</td>" +
                            "<td width='15%'>" + result[i].Period_No + "</td>" +
                            "<td width='25%'>" + result[i].Period_Start_Date + "</td>" +
                            AreaNameData
                            +"</tr>";
                        $("#FiscalYearAdjusmentPeriodsBody").append(Data);
                    }
                }
            });
        }
    });

    $("#adjYearName").change(function () {
        var Year = $(this).val();
        $("#FinishSaveAdjustmentBtn").prop("disabled", true);
        $("#FiscalYearPeriodsBody").html("");
        $("#FiscalYearAdjusmentPeriodsBody").html("");
        if (Year.length === 0) {
            $(this).css("border-color", "red");
        } else {
            $("#FinishSaveAdjustmentBtn").prop("disabled", false);
            $(this).css("border-color", "");
            $.ajax({
                type: "GET",
                url: "/OpenAndClosFiscalPeriods/GetOpenCloseAdjusmentPeriods?Year=" + Year,
                success: function (result) {
                    for (var i = 0; i < result.length; i++) {

                        var cbFinancial = "";
                        if (result[i].Financial === true) {
                            cbFinancial = "Checked";
                        }
                        var cbPurchasing = "";
                        if (result[i].Purchasing === true) {
                            cbPurchasing = "Checked";
                        }
                        var cbSales = "";
                        if (result[i].Sales === true) {
                            cbSales = "Checked";
                        }
                        var cbInventory = "";
                        if (result[i].Inventory === true) {
                            cbInventory = "Checked";
                        }

                        var AreaNameData = "";

                        $.each(result[i].AreaNames, function (k, i) {
                            if (i.Allow_adjust) {
                                AreaNameData += "<td><input type='checkbox' id='" + i.Area_name + "' checked='checked'></td>";
                            } else {
                                AreaNameData += "<td><input type='checkbox' id='" + i.Area_name + "'></td>";
                            }
                        })
                        var Data = "<tr class='row_" + result[i].ID + "'>" +
                            "<td class= 'hide-normal'>" + result[i].ID + "</td>" +
                            "<td width='15%'>" + result[i].Period_No + "</td>" +
                            "<td width='25%'>" + result[i].Period_Start_Date + "</td>" +
                            AreaNameData
                            +"</tr>";
                        //"<td width='15%'>" + '<input type="checkbox" id="' + result[i].ID + "AdjFinancial" + '" ' + cbFinancial + ' >' + "</td>" +
                        //    "<td width='15%'>" + '<input type="checkbox" id="' + result[i].ID + "AdjPurchasing" + '" ' + cbPurchasing + ' >' + "</td>" +
                        //    "<td width='15%'>" + '<input type="checkbox" id="' + result[i].ID + "AdjSales" + '" ' + cbSales + ' >' + "</td>" +
                        //    "<td width='15%'>" + '<input type="checkbox" id="' + result[i].ID + "AdjInventory" + '" ' + cbInventory + ' >' + "</td>" +

                        $("#FiscalYearAdjusmentPeriodsBody").append(Data);
                    }
                }
            });
        }
    });

    $("#FinishSavePeriodsBtn").click(function () {
        var PeriodsArr = [];
        PeriodsArr.length = 0;
        $.each($("#FiscalYearPeriodsTbl tbody tr"), function () {
            var ThisFiscal_year_area = [];
            $(this).find("input").each(function () {
                ThisFiscal_year_area.push({
                    Area_name: $(this).attr("id"),
                    Allowed: $(this).is(":checked")
                })
            });

            PeriodsArr.push({
                ID: $(this).find('td:eq(0)').html(),
                Period_No: $(this).find('td:eq(1)').html(),
                Period_Start_Date: $(this).find('td:eq(2)').html(),
                Period_End_Date: $(this).find('td:eq(3)').html(),
                Fiscal_year_area: ThisFiscal_year_area
                //Financial: $(this).find('td:eq(4) input').is(':checked'),
                //Purchasing: $(this).find('td:eq(5) input').is(':checked'),
                //Sales: $(this).find('td:eq(6) input').is(':checked'),
                //Inventory: $(this).find('td:eq(7) input').is(':checked')
            });
        });
        var data = JSON.stringify({
            FiscalYear: PeriodsArr
        });
        $.ajax({
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            type: "POST",
            url: "/OpenAndClosFiscalPeriods/SavePeriodsOAndC",
            data: data,
            success: function (data) {
                $("#SuccessSaved").text(data);
            }
        });
    });

    $("#FinishSaveAdjustmentBtn").click(function () {
        var AdjPeriodsArr = [];
        AdjPeriodsArr.length = 0;
        $.each($("#FiscalYearAdjusmentPeriodsTbl tbody tr"), function () {
            var ThisFiscal_year_area = [];
            $(this).find("input").each(function () {
                ThisFiscal_year_area.push({
                    Area_name: $(this).attr("id"),
                    Allow_adjust: $(this).is(":checked")
                })
            });
            AdjPeriodsArr.push({
                ID: $(this).find('td:eq(0)').html(),
                Period_No: $(this).find('td:eq(1)').html(),
                Period_Start_Date: $(this).find('td:eq(2)').html(),
                Fiscal_year_area: ThisFiscal_year_area,
                //Financial: $(this).find('td:eq(3) input').is(':checked'),
                //Purchasing: $(this).find('td:eq(4) input').is(':checked'),
                //Sales: $(this).find('td:eq(5) input').is(':checked'),
                //Inventory: $(this).find('td:eq(6) input').is(':checked')
            });
            
        });
        var data = JSON.stringify({
            AdjusmentFiscalYear: AdjPeriodsArr
        });
        $.ajax({
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            type: "POST",
            url: "/OpenAndClosFiscalPeriods/SaveAdjOAndC",
            data: data,
            success: function (data) {
                $("#SuccessSaved").text(data);
            }
        });
    });

});
