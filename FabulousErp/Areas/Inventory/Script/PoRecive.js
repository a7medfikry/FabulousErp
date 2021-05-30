
//function EqQty(Qty) {
//    var EqQty = 1;
//    if (!$("#Quantity").attr("data-Eq")) {
//        EqQty = 1;
//    }
//    if ($("#Quantity").attr("data-action") == 1) {
//        EqQty = Qty * EqQty;
//    } else if ($("#Quantity").attr("data-action") == 2) {
//        EqQty = Qty / EqQty;
//    }
//    if (!EqQty) {
//        EqQty = 1;
//    }
//    return EqQty;
//}
$(document).on("focusout", "#Quantity", function () {
  //  var ThisQty = EqQty(RoundNumber($("#Quantity").val())) ;
    var ThisQty = RoundNumber($("#Quantity").val());
    if ((getParameterByName("Sales") == "true"
        && $("#Doc_type").val() == Doc_type.Invoice)
        || (getParameterByName("Sales") == "false"
            && $("#Doc_type").val() == Doc_type.Return && !$("#GNums").val())) {

        GetPriceList();
        GetItemDetials();
        RunAfterAjax(function () {
            $("#UpdateItem").removeAttr("disabled")
        })
    }
    else if (getParameterByName("Sales") == "false"
        && $("#Doc_type").val() == Doc_type.Return) {
        $("#UpdateItem").attr("disabled", "disabled")
        //if ($("#UpdateItem").is(":visible"))
        {
            GetPurchasItemDetials(function (Q, U, C, Discount, Fright, UOMId) {
                $("#Quantity").val(Q);
                $("#Unit_priceMask").val(U).trigger("mask.maskMoney").trigger("change");
                $("#Cost_priceMask").val(C).trigger('mask.maskMoney').trigger("change");
                RunAfterAjax(function () {
                    if ($("#UpdateItem").length <= 0) {
                        $("#UOM_id").val(UOMId)
                        $("#UOM_id").selectpicker('refresh');
                    }
                })
                $("#UpdateItem").removeAttr("disabled")

                $("#DiscountMask").val(Discount).trigger("mask.maskMoney").trigger("change");
                $("#FrightMask").val(Fright).trigger("mask.maskMoney").trigger("change");
                var MainDiscount = GetMaskNumber($("#TTradeDiscountMask").val());
                $("#TTradeDiscountMask").val(MainDiscount + Discount)
                var MainFright = GetMaskNumber($("#TFrightMask").val());
                $("#TFrightMask").val(MainFright + Fright)
            })
        }
    }
    else if (getParameterByName("Sales") == "true" && $("#Doc_type").val() == Doc_type.Return) {
        var asd = $("#GNums").val();
        if (!$("#GNums").val()) {
            $("#Cost_priceMask").removeAttr("disabled")
            GetPriceList();
            $.ajax({
                url: "/Inventory/Inv_sales_invoice/GetSalesCostPrice?ItemId=" + $("#Item_id").val() + "&StoreId=" + $("#Store_id").val(),
                method: "POST",
                success: function (data) {
                    $("#Cost_priceMask").val(RoundNumber(data)).trigger('mask.maskMoney').trigger("change").trigger("focusout")
                }
            })
        }
    }
  
})
function GetPriceList() {
    var ThisQty = RoundNumber($("#Quantity").val());
    if ($("#Price_lvl").val()!="0") {
       // NotValid($("#Price_lvl"));
        $.ajax({
            url: "/Inventory/Inv_pricelist/GetUnitPrice?ItemId=" + $("#Item_id").val() + "&Qty=" + ThisQty + "&Pricelvl=" + $("#Price_lvl").val() + "&UOM=" + $("#UOM_id").val(),
            success: function (data) {
                if (data > 0) {
                    $("#Unit_priceMask").val(data).trigger('mask.maskMoney').trigger("change").trigger("focusout");
                }
            },
            method: "POST"
        })
    }
   
}
function GetItemDetials(GetAvaliable=false,CallBack = null) {
    var ReservedTmp = [];
    $("#itemsBody").find("tr").each(function () {
        try {
            var Obj = $(this).find(".PoItem").text().substring(1)
            Obj = Obj.substring(0, Obj.length - 1)
            Obj = JSON.parse(Obj);
            Obj.item_id = $(this).find(".ItemId").text();
            ReservedTmp.push(Obj)
        } catch (err) {

        }

    })
    var JsonData = { SoldItems: ReservedTmp }
    $.ajax({
        url: "/Inventory/Inv_receive_po_items/GetItemDetails?ItemId=" + $("#Item_id").val() + "&GetAvaliable=" + GetAvaliable + "&StoreId=" + $("#Store_id").val()
            + "&Qty=" + RoundNumber($("#Quantity").val()) + "&UOM=" + $("#UOM_id").val(),
        data: JSON.stringify(JsonData),
        contentType: 'application/json',
        success: function (data) {
            if (data.Avaliable) {
                if (GetAvaliable) {
                    var AvQty = 0;
                    $.each(data.POs, function (k,i) {
                        AvQty += i.Qty
                    })
                    $("#Quantity").val(AvQty)
                    CalcUpdateTax();
                }
                $("#Cost_priceMask").val(data.CostPrice).trigger('mask.maskMoney').trigger("change");
                $("#FrmPoItem").val(JSON.stringify(data.POs));
            } else {
                if ($("#Quantity").val()!="") {
                    NotValid($("#Quantity"), NotEnoughInv, false)
                }
                $("#Quantity").val("");
                $("#Cost_priceMask").val("");
                $("#Unit_priceMask").val("");
            }
            if (CallBack != null) {
                CallBack();
            }
        },
        method: "POST"
    })
}
function GetPurchasItemDetials(CallBack = null) {
    var Qty = RoundNumber($("#Quantity").val());
    if (!Qty) {
        Qty = 0;
    }
    var ChangeQty = false;
    if ($("#UpdateItem").length>0) {
        ChangeQty = true;
    }
    $.ajax({
        url: "/Inventory/Inv_receive_po_items/GetCalcPurchasDetails?ItemId=" + $("#Item_id").val() + "&RPoId=" + $("#GNums").val() + "&StoreId=" + $("#Store_id").val() +
            "&Qty=" + Qty + "&ChangeQty=" + ChangeQty + "&UOM_id=" + $("#UOM_id").val(),
        method: "POST",
        success: function (data) {
            if (CallBack != null) {
                CallBack(data.Qty, RoundNumber(data.UnitPrice), RoundNumber(data.CostPrice), data.Discount, data.Fright, data.UOMId)
            }
            CalcUpdateTax();
        }
    })
}
function GetSalesItemDetials(CallBack = null) {
    $.ajax({
        url: "/Inventory/Inv_sales_invoice/GetSalesItemsDetils?SalesId=" + $("#GNums").val()+"&ItemId=" + $("#Item_id").val(),
        contentType: 'application/json',
        success: function (data) {
            if (CallBack != null) {
                CallBack(data.Qty, data.Unit_price, data.Total, data.CostPrice, data.Discount, data.Fright,data.UOM_id)
            }
            CalcUpdateTax();
        },
        method: "POST"
    })
}
var SalesUnitPrice = 0;
$(document).on("change", "#Item_id", function () {
    try {
        GetUnitOfMeasureId();
    } catch (err) {

    }
    $("#Quantity").trigger("focusout");
})

$(document).on("click", ".DeletePoItem", function () {

    $(this).find(".BodyFright").text(0);
    ReCalcFright()
    $("#TCT-taxTblBody").find("tr:eq('" + $(this).parents("tr").index() + "')").find(".DeleteItem").trigger("click");
    $(this).parents("tr").remove();
    CalcFrightAndDiscount();
    CalcTTable();
    if ($("#itemsBody").find("tr").length == 0) {
        EmptyGlTransaction();
    }
    SetVendoreClientGl();
    CalcNewBalance();
})
$(document).on("click", ".SerialsPoBtn", function () {
    
})
var EditIndex;
var IsUpdate;
$(document).on("click", ".EditPoItem", function () {
    EditIndex = $(this).parents("tr").first().index();
    IsUpdate = true;
    var Tr = $(this).parents("tr");
    var Frm = $("#PoRecItems").find("form");
    $(Frm).find("#Item_id").val($(Tr).find(".ItemId").text());
    $(Frm).find("#Item_id").trigger("change");
    $(Frm).find("#UOM_id").val($(Tr).find(".UOM").attr("data-id"));
    $(Frm).find("#Quantity").val($(Tr).find(".ItemQty").text());
    // $(Frm).find("#Unit_priceMask").val($(Tr).find(".BodyTotal").text()).trigger('mask.maskMoney').trigger("change");
    $(Frm).find("#Unit_priceMask").val(GetMaskNumber($(Tr).find(".ItemPriceMask").text())).trigger('mask.maskMoney').trigger("change");
    $(Frm).find("#Unit_priceMask").trigger("change");
    $(Frm).find("#DiscountMask").val(GetMaskNumber($(Tr).find(".BodyDiscount").text())).trigger('mask.maskMoney').trigger("change");
    $(Frm).find("#Vat_amountMask").val(GetMaskNumber($(Tr).find(".BodyVat").text())).trigger('mask.maskMoney').trigger("change");
    $(Frm).find("#Table_vat_amountMask").val(GetMaskNumber($(Tr).find(".BodyTableVat").text())).trigger('mask.maskMoney').trigger("change");
    $(Frm).find("#Deduct_amountMask").val(GetMaskNumber($(Tr).find(".BodyDeduct").text())).trigger('mask.maskMoney').trigger("change");
    $(Frm).find("#Cost_priceMask").val(GetMaskNumber($(Tr).find(".CostPrice").text())).trigger('mask.maskMoney').trigger("change");
    $(Frm).find("#ItemSite").val($(Tr).find(".ItemSite ").text());

    $(Frm).find("#Vat_amountMask").attr("data-ismodifed", "false")
    $(Frm).find("#Table_vat_amountMask").attr("data-ismodifed", "false")
    $(Frm).find("#Deduct_amountMask").attr("data-ismodifed", "false")

    if ($("#CalcTotalDis").is(":checked")) {
        $(Frm).find("#DiscountMask").attr("disabled", "disabled")
    } else {
        $(Frm).find("#DiscountMask").removeAttr("disabled")
    }

    $("#AddNewItem").removeClass("btn-secondary").addClass("btn-info").text("Update item");
    $("#AddNewItem").attr("id", "UpdateItem");
    if ($("#ShowTax").is(":checked")) {
        $(".ShowOnEdit").removeClass("hide")
    }
    if ($("#Doc_type").val() == Doc_type.Return && getParameterByName("Sales") == "true") {
        $("#Cost_priceMask").removeAttr("disabled")
        $("#Unit_priceMask").removeAttr("disabled")
    }
})
$(document).on("keyup", "#Vat_amountMask,#Table_vat_amountMask,#Deduct_amountMask", function () {
    $(this).attr("data-ismodifed", "true")
})
$(document).on("click", "#UpdateItem", function () {
    if ($("#Doc_type").val() == Doc_type.Return && getParameterByName("Sales") == "true") {
        $("#Cost_priceMask").attr("disabled","disabled")
        $("#Unit_priceMask").attr("disabled","disabled")
    }

    var Tr = $("#itemsBody").find("tr:eq(" + EditIndex + ")");
    var Frm = $("#PoRecItems").find("form");

    $(Tr).find(".ItemId").text($(Frm).find("#Item_id").val());
    $(Tr).find(".UOM").attr("data-id", $(Frm).find("#UOM_id").val());
    $(Tr).find(".ItemQty").text($(Frm).find("#Quantity").val());
    $(Tr).find(".ItemPriceMask").text(MaskThisMoney(GetMaskNumber($(Frm).find("#Unit_priceMask").val())))
    $(Tr).find(".BodyDiscount").text(MaskThisMoney(GetMaskNumber($(Frm).find("#DiscountMask").val())))
    $(Tr).find(".CostPrice").text(MaskThisMoney(GetMaskNumber($(Frm).find("#Cost_priceMask").val())))
    $(Tr).find(".BodyTotal").text(MaskThisMoney(GetMaskNumber(($(Frm).find("#TotalMask").val()))));
    $(Tr).find(".BodyVat").text(MaskThisMoney(GetMaskNumber($(Frm).find("#Vat_amountMask").val())))
    $(Tr).find(".BodyTableVat").text(MaskThisMoney(GetMaskNumber($(Frm).find("#Table_vat_amountMask").val())))
    $(Tr).find(".BodyDeduct").text(MaskThisMoney(GetMaskNumber($(Frm).find("#Deduct_amountMask").val())))

    $(Tr).find(".BodyVat").attr("data-tax-per",GetMaskNumber($(Frm).find("#Vat_amountMask").val()))
    $(Tr).find(".BodyTableVat").attr("data-tax-per",GetMaskNumber($(Frm).find("#Table_vat_amountMask").val()))
    $(Tr).find(".BodyDeduct").attr("data-tax-per",GetMaskNumber($(Frm).find("#Deduct_amountMask").val()))



    if (getParameterByName("Sales") == "true") {
        $(Tr).find(".ItemName").text($(Frm).find("#ItemName").val());
    }
   
    //if (getParameterByName("Sales") != "true") {
    //    SetDC(GetGlAccIndexRClass(GetGlRowClassByAccId($(Tr).find(".BodyTotal").attr("data-CAID"))), GetMaskNumber($(Frm).find("#TotalMask").val()))
    //} else {
    //    SetDC(GetGlAccIndexRClass(GetGlRowClassByAccId($(Tr).find(".BodyTotal").attr("data-CAID"))), GetMaskNumber($(Frm).find("#Cost_priceMask").val()))
    //    SetDC(1, GetMaskNumber($(Frm).find("#Cost_priceMask").val()))
    //}

    var ThisVatTblTxt = $(Frm).find("#Table_vat_amountMask").val();
    var ThisVatTbl = GetMaskNumber(ThisVatTxt);

    var ThisVatTxt = $(Frm).find("#Vat_amountMask").val();
    var ThisVat = GetMaskNumber(ThisVatTxt);

    var ThisDudectTxt = $(Frm).find("#Deduct_amountMask").val();
    var ThisDudect = GetMaskNumber(ThisDudectTxt);

    var TotalTax = RoundNumber(ThisVat + ThisVatTbl - ThisDudect);

    SetToTaxTable(EditIndex, $(Frm).find("#Vat_amountMask").val()
        , $(Frm).find("#Table_vat_amountMask").val(), $(Frm).find("#Deduct_amountMask").val()
        , $(Frm).find("#DiscountMask").val(),$("#Doc_type").val())

    $("#TCT-taxTblBody").find("tr:eq(" + EditIndex + ")").find("td:eq(3)")
        .text($(Frm).find("#Quantity").val());

    $("#TCT-taxTblBody").find("tr:eq(" + EditIndex + ")").find("td:eq(4)")
        .text($(Frm).find("#Unit_priceMask").val());

   
    $("#TCT-taxTblBody").find("tr:eq(" + EditIndex + ")").find("td:eq(5)")
        .text($(Frm).find("#TotalMask").val());
    $("#TCT-taxTblBody").find("tr:eq(" + EditIndex + ")").find("td:eq(6)")
        .text($(Frm).find("#TotalMask").val());
  
    $("#TCT-taxTblBody").find("tr:eq(" + EditIndex + ")").find(".TblTotalVatamount")
        .text(GetMaskNumber($(Frm).find("#Vat_amountMask").val()) + GetMaskNumber($(Frm).find("#Table_vat_amountMask").val()));

    if (!$("#CalcTotalDis").is(":checked")) {
        var Discount = 0;
        $("#itemsBody").find("tr").each(function () {
            Discount += GetMaskNumber($(this).find(".BodyDiscount").text());
        })
        $("#TTradeDiscountMask").val(Discount).trigger('mask.maskMoney').trigger("change").trigger("focusout");
    }

    $(".ShowOnEdit").addClass("hide")
    $("#UpdateItem").attr("id", "AddNewItem");
    $("#AddNewItem").removeClass("btn-info").addClass("btn-secondary").text("Add Item");
    $("#PoRecItems").find("form").find("#reset").trigger("click");
    CalcTTable();
    if (window.location.href.toString().indexOf("/Inventory/Inv_po_adjustment/Index")>0) {
        CalcJvTaxByAccId()
        SetVendoreClientGl();
    } else {
        CalcJvTax();
    }
    if (getParameterByName("Sales") == "true") {
        SetDC(GetGlAccIndex($(Tr).find(".CostPrice").attr("data-caid")),
            GetMaskNumber($(Tr).find(".CostPrice").text()))
    }
    if (getParameterByName("Sales") == "false" && $("#Doc_type").val() == Doc_type.Return) {
        CalcVariance();
    }
    RunAfterAjax(function () {
        setTimeout(function () {
            $("#TTradeDiscountMask").trigger("focusout")
            $("#TFrightMask").trigger("focusout")
        },1000)
    })
    IsUpdate = false;
    CalcNewBalance();

})
$(document).on("focusout", "#Quantity,#Unit_priceMask,#Cost_priceMask", function () {
    CalcUpdateTax();
})
function CalcUpdateTax() {
    if ($("#Table_vat_amountMask").is(":visible")) {
        $("#UpdateItem").attr("disabled", "disabled")


        var Frm = $("#PoRecItems").find("form");

        var Tr = $("#itemsBody").find("tr:eq(" + EditIndex + ")");

        var Qty = parseFloat($(Frm).find("#Quantity").val());
        var UnitPrice = GetMaskNumber($(Frm).find("#Unit_priceMask").val());
        var Discount = GetMaskNumber($(Frm).find("#DiscountMask").val());
        var Total = (Qty * UnitPrice - Discount);

        var TblVatPer = parseFloat($(Tr).find(".BodyTableVat").attr("data-tax-per"))
        var VatPer = parseFloat($(Tr).find(".BodyVat").attr("data-tax-per"))
        var DeductPer = parseFloat($(Tr).find(".BodyDeduct").attr("data-tax-per"))

        var TaxTablVat = RoundNumber(Total * (TblVatPer / 100));
        var TaxVat = RoundNumber((Total + TaxTablVat) * (VatPer / 100));
        var DedcutVat = RoundNumber((Total) * (DeductPer / 100));

        $("#TCT-taxTblBody").find("tr:eq(" + EditIndex + ")").find(".TblVatAccountId")
            .prev("td").text(TaxTablVat);
        $("#TCT-taxTblBody").find("tr:eq(" + EditIndex + ")").find(".TblAccountId")
            .prev("td").text(TaxVat);
        $("#TCT-taxTblBody").find("tr:eq(" + EditIndex + ")").find(".TblDecuttaId")
            .text(DedcutVat);

        $("#TCT-taxTblBody").find("tr:eq(" + EditIndex + ")").find("td:eq(3)")
            .text(Qty);
        $("#TCT-taxTblBody").find("tr:eq(" + EditIndex + ")").find("td:eq(4)")
            .text(UnitPrice);
        $("#TCT-taxTblBody").find("tr:eq(" + EditIndex + ")").find("td:eq(5)")
            .text(setCurrecnyCurrFormate(Qty * UnitPrice));
        $("#TCT-taxTblBody").find("tr:eq(" + EditIndex + ")").find("td:eq(6)")
            .text(setCurrecnyCurrFormate(Qty * UnitPrice));

        $("#TCT-taxTblBody").find("tr:eq(" + EditIndex + ")").find("td:eq(7)")
            .text(setCurrecnyCurrFormate(Discount));

        $("#TCT-taxTblBody").find("tr:eq(" + EditIndex + ")").find(".TblTotalVatamount")
            .text(setCurrecnyCurrFormate(TaxTablVat + TaxVat));

        if ($("#Table_vat_amountMask").attr("data-ismodifed") == "false") {
            $("#Table_vat_amountMask").val(TaxTablVat).trigger('mask.maskMoney').trigger("change");
        }
        if ($("#Vat_amountMask").attr("data-ismodifed") == "false") {
            $("#Vat_amountMask").val(TaxVat).trigger('mask.maskMoney').trigger("change");
        }
        if ($("#Deduct_amountMask").attr("data-ismodifed") == "false") {
            $("#Deduct_amountMask").val(DedcutVat).trigger('mask.maskMoney').trigger("change");
        }
        RunAfterAjax(function () {
            $("#UpdateItem").removeAttr("disabled")
        })

        //var ThisInt = setInterval(function () {
        //    if ($.active <= 0) {

        //        if ($("#Table_vat_amountMask").attr("data-ismodifed") == "false") {
        //            $("#Table_vat_amountMask").val(GetMaskNumber($("#TCT-tableVatAmount").val())).trigger('mask.maskMoney').trigger("change");
        //        }
        //        if ($("#Vat_amountMask").attr("data-ismodifed") == "false") {
        //            $("#Vat_amountMask").val(GetMaskNumber($("#TCT-vatAmount").val())).trigger('mask.maskMoney').trigger("change");
        //        }
        //        if ($("#Deduct_amountMask").attr("data-ismodifed") == "false") {
        //            $("#Deduct_amountMask").val(GetMaskNumber($("#TCT-dacuttaAmount").val())).trigger('mask.maskMoney').trigger("change");
        //        }
        //        clearInterval(ThisInt);
        //        $("#UpdateItem").removeAttr("disabled")
        //    }
        //}, 100)
    }


}

function GetTax(Total, Discount, TblVatPer, VatPer, DeductPer) {
    var TaxTablVat = RoundNumber((GetMaskNumber(Total) - GetMaskNumber(Discount)) * (parseFloat(TblVatPer) / 100));
    var TaxVat = RoundNumber((GetMaskNumber(Total) - GetMaskNumber(Discount) + TaxTablVat) * (parseFloat(VatPer) / 100));
    var DedcutVat = RoundNumber((GetMaskNumber(Total) - GetMaskNumber(Discount)) * (parseFloat(DeductPer) / 100));
    return { TaxTablVat: TaxTablVat, TaxVat: TaxVat, DedcutVat: DedcutVat }
}
function GetTableTax(Tr) {
    var Vat = GetMaskNumber($("#itemsBody").find("tr:eq(" + Tr + ")").find(".BodyVat")
        .text());
    var TblVat = GetMaskNumber($("#itemsBody").find("tr:eq(" + Tr + ")").find(".BodyTableVat")
        .text());
    var Deduct = 0;
    if ($("#itemsBody").find("tr:eq(" + Tr + ")").find(".BodyDeduct").attr("data-tax-accid") != 0){
        Deduct = GetMaskNumber($("#itemsBody").find("tr:eq(" + Tr + ")").find(".BodyDeduct")
            .text());
    }
    return { Vat: Vat, TblVat: TblVat, Deduct: Deduct, Total: RoundNumber(Vat + TblVat - Deduct)}
        
}
function SetTaxToItemTbl(Tr, TaxTablVat, TaxVat, DedcutVat) {
    $("#itemsBody").find("tr:eq(" + Tr + ")").find(".BodyTableVat")
        .text((TaxTablVat));
    $("#itemsBody").find("tr:eq(" + Tr + ")").find(".BodyVat")
        .text((TaxVat));
    $("#itemsBody").find("tr:eq(" + Tr + ")").find(".BodyDeduct")
        .text((DedcutVat));
}
function CalcDiscountJv() {
    if (!CurrencyId) {
        var CurrencyId = "Currency_id";
    }
    if (GetMaskNumber($("#TTradeDiscountMask").val()) > 0) {
        if (getParameterByName("Sales") != "true") {
            Url = "/Payable/Payable_gl_account/GetDiscountJV?Taken_discount=" + GetMaskNumber($("#TTradeDiscountMask").val()) +
                "&VendorId=" + $("#Vendor_idTrans").val()
                + "&ThisCurrIso=" + $("#" + CurrencyId).find("option:selected").text()
                + "&Transaction_rate=1"
                + "&Doc_type=" + $("#Doc_type").val()

            TotalUrl = "/Payable/Payable_gl_account/GetTotalJv?Total=" + GetMaskNumber($("#TTotalMask").val()) +
                "&VendorId=" + $("#Vendor_idTrans").val()
                + "&ThisCurrIso=" + $("#" + CurrencyId).find("option:selected").text()
                + "&Transaction_rate=1"
                + "&Doc_type=" + $("#Doc_type").val();
        } else {
            Url = "/Receivable/Receivable_gl_account/GetDiscountJV?Discount=" + GetMaskNumber($("#TTradeDiscountMask").val()) +
                "&VendorId=" + $("#Vendor_id").val()
                + "&ThisCurrIso=" + $("#" + CurrencyId).find("option:selected").text()
                + "&Transaction_rate=1"
                + "&Doc_type=" + $("#Doc_type").val();

            TotalUrl = "/Receivable/Receivable_gl_account/GetTotalJv?Total=" + GetMaskNumber($("#TTotalMask").val()) +
                "&VendorId=" + $("#Vendor_idTrans").val()
                + "&ThisCurrIso=" + $("#" + CurrencyId).find("option:selected").text()
                + "&Transaction_rate=1"
                + "&Doc_type=" + $("#Doc_type").val();
        }

        $.ajax({
            url: Url,
            method: "POST",
            success: function (data) {
                if (getParameterByName("Sales") != "true") {
                    //$("#TCGE-TTbl").find(".TCGE-TblAccID:contains(" + data.ShowTransactions[1].AID + ")")
                    //    .parents("tr").remove();

                    if ($("#Doc_type").val() == Doc_type.Invoice) {
                        RmDc(data.ShowTransactions[1].AID, false, true);
                        ManyJvAction(data, false);
                        $("#TTradeDiscountMask").attr("data-acid", data.ShowTransactions[1].AID)
                    } else {
                        RmDc(data.ShowTransactions[0].AID, true, false);
                        ManyJvAction(data, false);
                        $("#TTradeDiscountMask").attr("data-acid", data.ShowTransactions[0].AID)
                    }
                } else {
                    //$("#TCGE-TTbl").find(".TCGE-TblAccID:contains(" + + ")")
                    //    .parents("tr").remove();
                    RmDc(data.ShowTransactions[0].AID, true);
                    ManyJvAction(data, false);
                    $("#TTradeDiscountMask").attr("data-acid", data.ShowTransactions[0].AID)

                    if ($("#Doc_type").val() == Doc_type.Invoice) {
                        RmDc(data.ShowTransactions[0].AID, true, false);
                        ManyJvAction(data, false);
                        $("#TTradeDiscountMask").attr("data-acid", data.ShowTransactions[0].AID)

                    } else {

                        RmDc(data.ShowTransactions[1].AID, false, true);
                        ManyJvAction(data, false);
                        $("#TTradeDiscountMask").attr("data-acid", data.ShowTransactions[1].AID)
                    }
                    //SetDC(3, GetMaskNumber($("#TTotalMask").val()) - GetMaskNumber($("#TFrightMask").val()))
                }
            }
        })
    }
  
}
function SameAccounts(First, Second, Amount,CallBack) {
    if (First == Second) {
        return Amount;
    } else {
        return 0;
    }
}
function CalcTableJvValue(CallBack) {

    var TransactionRate = GetMaskNumber($("#TCGE-TransactionRate").val());
    if (!TransactionRate) {
        TransactionRate = 1;
    }

    if (getParameterByName("Sales") == "false") {
        var TotalCostP = 0;
        var TotalBodyT = 0;
        var TotalTax = 0;
        var TotalDiscount = 0;
        var TotalFright = 0;
        var FrightAcc= -1;
        var AcrFrightAcc = -1;
        var TotalAcc = -1;
        var CostAcc = -1;

        var Exist = [];
       
        $("#itemsBody").find("tr").each(function () {
            TotalCostP = GetMaskNumber($(this).find(".CostPrice").text());
            TotalBodyT = GetMaskNumber($(this).find(".BodyTotal").text());
            TotalDiscount = GetMaskNumber($(this).find(".BodyDiscount").text());
            TotalFright = GetMaskNumber($(this).find(".BodyFright").text());
            TotalTax = GetTableTax($(this).index()).Total
            TotalAcc = $(this).find(".BodyTotal").attr("data-caid");
            FrightAcc = $(this).find(".BodyFright").attr("data-acidrow");
            AcrFrightAcc = $(this).find(".BodyFright").attr("data-acidafrow");
            
            if (Exist.find(x => x == TotalAcc)) {
                var BAmount = GetDc(GetGlAccIndex(TotalAcc));
              
                var AddFright = SameAccounts(FrightAcc, TotalAcc, TotalFright);
                var AddAcrualFright = SameAccounts(AcrFrightAcc, TotalAcc,TotalFright);
                var FrightValue = AddFright + AddAcrualFright;

                if ($("#Doc_type").val() == Doc_type.Invoice) {
                    SetDC(GetGlAccIndex(TotalAcc),
                        (TotalBodyT + BAmount + FrightValue) * TransactionRate);
                } else {
                    SetDC(GetGlAccIndex(TotalAcc),
                        (TotalCostP + BAmount + FrightValue) * TransactionRate);
                }

            } else {
              
                if ($("#Doc_type").val() == Doc_type.Invoice) {
                    var AddFright = SameAccounts(FrightAcc, TotalAcc, TotalFright);
                    var AddAcrualFright = SameAccounts(AcrFrightAcc, TotalAcc, TotalFright);
                    var FrightValue = AddFright + AddAcrualFright;

                    SetDC(GetGlAccIndex(TotalAcc),
                        (TotalBodyT + FrightValue) * TransactionRate);
                } else {
                    var AddFright = SameAccounts(FrightAcc, TotalAcc,TotalFright);
                    var AddAcrualFright = SameAccounts(AcrFrightAcc, TotalAcc, TotalFright);
                    var FrightValue = AddFright + AddAcrualFright;

                    SetDC(GetGlAccIndex(TotalAcc),
                        (TotalCostP + FrightValue) * TransactionRate);
                }
            }
            if (Exist.find(x => x == $(this).find(".ItemId").attr("data-vendorecaid"))) {
                var BAmount = GetDc(GetGlAccIndex($(this).find(".ItemId").attr("data-vendorecaid")));
                SetDC(GetGlAccIndex($(this).find(".ItemId").attr("data-vendorecaid")),
                    (TotalBodyT + TotalTax - TotalDiscount + BAmount) * TransactionRate);
            } else {
                SetDC(GetGlAccIndex($(this).find(".ItemId").attr("data-vendorecaid")),
                    (TotalBodyT + TotalTax - TotalDiscount) * TransactionRate);
            }
            Exist.push(TotalAcc)
            Exist.push($(this).find(".ItemId").attr("data-vendorecaid"))
        })

    } else {
        var TotalCostP = 0;
        var TotalBodyT = 0;
        var TotalTax = 0;
        var TotalDiscount = 0;
        var TotalFright = 0;
        var FrightAcc = -1;
        var AcrFrightAcc = -1;
        var TotalAcc = -1;
        var CostAcc = -1;

        var Exist = [];

        $("#itemsBody").find("tr").each(function () {

            TotalCostP = GetMaskNumber($(this).find(".CostPrice").text());
            TotalBodyT = GetMaskNumber($(this).find(".BodyTotal").text());
            TotalTax = GetTableTax($(this).index()).Total
            TotalDiscount = GetMaskNumber($(this).find(".BodyDiscount").text());
            TotalFright = GetMaskNumber($(this).find(".BodyFright").text());
            TotalAcc = $(this).find(".BodyTotal").attr("data-caid");
            CostAcc = $(this).find(".CostPrice").attr("data-caid");
            FrightAcc = $(this).find(".BodyFright").attr("data-acidrow");


            if (Exist.find(x => x == TotalAcc)) {
                var BAmount = GetDc(GetGlAccIndex(TotalAcc));

               

                SetDC(GetGlAccIndex(TotalAcc),
                    (RoundNumber((TotalCostP) + TotalFright + BAmount) * TransactionRate));
            } else {


                SetDC(GetGlAccIndex(TotalAcc),
                    RoundNumber(((TotalCostP) + TotalFright) * TransactionRate) );
            }
            if (Exist.find(x => x == CostAcc)) {
                var BAmount = GetDc(GetGlAccIndex(CostAcc));
                SetDC(GetGlAccIndex(CostAcc),
                    (TotalCostP + BAmount) * TransactionRate);
            } else {
                SetDC(GetGlAccIndex(CostAcc),
                    TotalCostP * TransactionRate);
            }

            if (Exist.find(x => x == $(this).find(".BodyTotal").attr("data-costcaid"))) {
                var BAmount = GetDc(GetGlAccIndex($(this).find(".BodyTotal").attr("data-costcaid")));
                SetDC(GetGlAccIndex($(this).find(".BodyTotal").attr("data-costcaid")),
                    RoundNumber((TotalBodyT + BAmount) * TransactionRate));
            } else {
                SetDC(GetGlAccIndex($(this).find(".BodyTotal").attr("data-costcaid")),
                    RoundNumber(TotalBodyT * TransactionRate));
            }

            if (Exist.find(x => x == $(this).find(".ItemId").attr("data-vendorecaid"))) {
                var BAmount = GetDc(GetGlAccIndex($(this).find(".ItemId").attr("data-vendorecaid")));
                SetDC(GetGlAccIndex($(this).find(".ItemId").attr("data-vendorecaid")),
                    (TotalBodyT + TotalTax - TotalDiscount + BAmount) * TransactionRate);
            } else {
                SetDC(GetGlAccIndex($(this).find(".ItemId").attr("data-vendorecaid")),
                    (TotalBodyT + TotalTax - TotalDiscount) * TransactionRate);
            }
            Exist.push(TotalAcc)
            Exist.push($(this).find(".ItemId").attr("data-vendorecaid"))
            Exist.push(CostAcc)
            Exist.push($(this).find(".BodyTotal").attr("data-costcaid"))
        })
    }
    if (CallBack != null) {
        CallBack();
    }
}
function SetVendoreClientGl(TrIndex = null) {
    if (getParameterByName("FullPay") != "true") {
        CalcDiscountJv();
    }
    CalcTableJvValue();
    CalcJvTax();
}
function CalcJvTax() {
    var SumOfDeduct = 0;
    var SumOfVat = 0;
    var SumOfVatTbl = 0;
    var Sums = [];
    $("#itemsBody").find("tr").each(function () {
        //SumOfDeduct += GetMaskNumber($(this).find(".BodyDeduct").text());
        //SumOfVat += GetMaskNumber($(this).find(".BodyVat").text());
        //SumOfVatTbl += GetMaskNumber($(this).find(".BodyTableVat").text());
        var DeductVat = GetMaskNumber($(this).find(".BodyDeduct").text());
        var Vat = GetMaskNumber($(this).find(".BodyVat").text());
        var VatTbl = GetMaskNumber($(this).find(".BodyTableVat").text());

        Sums.push({ Type: "DeductVat", Val: DeductVat, Row: $(this).find(".BodyDeduct").attr("data-jvindex") })
        Sums.push({ Type: "Vat", Val: Vat, Row: $(this).find(".BodyVat").attr("data-jvindex") })
        Sums.push({ Type: "VatTbl", Val: VatTbl, Row: $(this).find(".BodyTableVat").attr("data-jvindex") })
    })
    var VSums = groupBy(Sums, x => x.Row);

    VSums.forEach(function (i, k) {
        Amount = 0;
        $.each(i, function (kk, ii) {
            Amount += ii.Val;
        })
        var JVIndex = GetGlAccIndexRClass(k);

        if (JVIndex != -1 && Amount!=0) {
            SetDC(JVIndex, Amount);
        }
    })

    
}
function CalcJvTaxByAccId() {
    var SumOfDeduct = 0;
    var SumOfVat = 0;
    var SumOfVatTbl = 0;
    var Sums = [];
    $("#itemsBody").find("tr").each(function () {
        //SumOfDeduct += GetMaskNumber($(this).find(".BodyDeduct").text());
        //SumOfVat += GetMaskNumber($(this).find(".BodyVat").text());
        //SumOfVatTbl += GetMaskNumber($(this).find(".BodyTableVat").text());

        var DeductVat = GetMaskNumber($(this).find(".BodyDeduct").text());
        var Vat = GetMaskNumber($(this).find(".BodyVat").text());
        var VatTbl = GetMaskNumber($(this).find(".BodyTableVat").text());
        Sums.push({ Type: "DeductVat", Val: DeductVat, Row: $(this).find(".BodyDeduct").attr("data-tax-accid") })
        Sums.push({ Type: "Vat", Val: Vat, Row: $(this).find(".BodyVat").attr("data-tax-accid") })
        Sums.push({ Type: "VatTbl", Val: VatTbl, Row: $(this).find(".BodyTableVat").attr("data-tax-accid") })
    })
    var VSums = groupBy(Sums, x => x.Row);

    VSums.forEach(function (i, k) {
        Amount = 0;
        $.each(i, function (kk, ii) {
            Amount += ii.Val;
        })
        var JVIndex = GetGlAccIndexRClass(GetGlRowClassByAccId(k));

        if (JVIndex != -1) {
            SetDC(JVIndex, Amount);
        }
    })

    //if (TblVatRow) {
    //    SetDC(GetGlAccIndexRClass(VatRow), SumOfVat)
    //    SetDC(GetGlAccIndexRClass(TblVatRow), SumOfVatTbl)
    //} else {
    //    SetDC(GetGlAccIndexRClass(VatRow), SumOfVat + SumOfVatTbl)
    //}
    //SetDC(GetGlAccIndexRClass(DeductRow), SumOfDeduct)
}







function CalcTTable() {
    var VatAmount = 0;
    var TblVatAmount = 0;
    var DeductAmount = 0;
    var Discount = GetMaskNumber($("#TTradeDiscountMask").val());
    var Fright = GetMaskNumber($("#TFrightMask").val());
    $("#itemsBody").find("tr").each(function () {
        VatAmount += GetMaskNumber($(this).find(".BodyVat").text())
        TblVatAmount += GetMaskNumber($(this).find(".BodyTableVat").text())
        DeductAmount += GetMaskNumber($(this).find(".BodyDeduct").text())
    })
    var Tax = RoundNumber(VatAmount + TblVatAmount - DeductAmount);
    var Total = 0;
    $("#itemsBody").find("tr").each(function () {
        Total += GetMaskNumber($(this).find(".BodyTotal").text())
    });
    $("#TNetAmountMask").val(Total).trigger('mask.maskMoney').trigger("change");
    $("#TTaxMask").val(Tax).trigger('mask.maskMoney').trigger("change");
    $("#TTotalMask").val(RoundNumber(Total + Tax - Discount + Fright)).trigger('mask.maskMoney').trigger("change");

}
function PushAll(obj, JvData) {
  
    var JvIds = [];
    var LastTmp;
    var Max = 1;
    try {
        LastTmp = LastIndexOfArray(All);
        Max = Math.max.apply(this, LastTmp.JvIds.map(function (o) { return o; }));
    } catch (err) {

    }
    $("#TCGE-TTbl").find("tr").each(function () {
        var TrIndex = $(this).index();
        if (Max < TrIndex || Max == 1) {
            JvIds.push(TrIndex);
        }
    });
    if (All.find(x => x.ItemId == obj.Item_id)) {
        All.find(x => x.ItemId == obj.Item_id).JvIds = All.find(x => x.ItemId == obj.Item_id).JvIds.concat(JvIds);

        obj.Quantity = parseFloat(obj.Quantity)
        obj.Unit_price = parseFloat(obj.Unit_price)
        obj.Total = parseFloat(obj.Total)

        All.find(x => x.ItemId == obj.Item_id).Jv.Quantity += obj.Quantity
        All.find(x => x.ItemId == obj.Item_id).Jv.Unit_price += obj.Unit_price;
        All.find(x => x.ItemId == obj.Item_id).Jv.Unit_priceMask += obj.Unit_priceMask;
        All.find(x => x.ItemId == obj.Item_id).Jv.Total += obj.Total;
        All.find(x => x.ItemId == obj.Item_id).Jv.TotalMask += obj.TotalMask;

    } else {
        obj.Quantity = parseFloat(obj.Quantity)
        obj.Unit_price = parseFloat(obj.Unit_price)
        obj.Total = parseFloat(obj.Total)

        All.push({ JvIds: JvIds, Jv: obj, Index: All.length + 1, ItemId: obj.Item_id, JvData: JvData })
    }
}

function GetNetPlusTaxMinusDis() {
    return GetMaskNumber($("#TNetAmountMask").val()) + GetMaskNumber($("#TTaxMask").val()) - GetMaskNumber($("#TTradeDiscountMask").val())
}
function SetToTaxTable(Tr,Vat=-1,TblVat=-1,Deduct=-1,Discount=-1,DocType=null) {
    if (Vat != -1) {
        if (DocType == Doc_type.Return) {
            $("#TCT-taxTblBody").find("tr:eq(" + Tr + ")").find(".TblAccountId").prev("td").text(-GetMaskNumber(Vat))
            $("#TCT-taxTblBody").find("tr:eq(" + Tr + ")").find(".Vat_amount")
                .text(-GetMaskNumber(Vat));
        } else {
            $("#TCT-taxTblBody").find("tr:eq(" + Tr + ")").find(".TblAccountId").prev("td").text(Vat)
            $("#TCT-taxTblBody").find("tr:eq(" + Tr + ")").find(".Vat_amount")
                .text((Vat));
        }
    
    }
    if (TblVat != -1) {
        if (DocType == Doc_type.Return) {
            $("#TCT-taxTblBody").find("tr:eq(" + Tr + ")").find(".TblVatAccountId").prev("td").text(-GetMaskNumber(TblVat))
            $("#TCT-taxTblBody").find("tr:eq(" + Tr + ")").find(".TblVatAccountId").find(".Table_vat_amount")
                .text(-GetMaskNumber(TblVat));
        } else {
            $("#TCT-taxTblBody").find("tr:eq(" + Tr + ")").find(".TblVatAccountId").prev("td").text(TblVat)
            $("#TCT-taxTblBody").find("tr:eq(" + Tr + ")").find(".TblVatAccountId").find(".Table_vat_amount")
                .text((TblVat));
        }
       
    }

    if (Discount != -1) {
        if (DocType == Doc_type.Return) {
            $("#TCT-taxTblBody").find("tr:eq(" + Tr + ")").find("td:eq(7)").text(-GetMaskNumber(Discount))
            $("#TCT-taxTblBody").find("tr:eq(" + Tr + ")").find(".Discount").text(-GetMaskNumber(Discount))
        } else {
            $("#TCT-taxTblBody").find("tr:eq(" + Tr + ")").find("td:eq(7)").text(Discount)
            $("#TCT-taxTblBody").find("tr:eq(" + Tr + ")").find(".Discount").text(Discount)
        }
       
    }
    if (Deduct != -1) {
        if (DocType == Doc_type.Return) {
            $("#TCT-taxTblBody").find("tr:eq(" + Tr + ")").find(".TblDecuttaId").text(-Deduct)
            $("#TCT-taxTblBody").find("tr:eq(" + Tr + ")").find(".Dacutta_amount").text(-Deduct)
        } else {
            $("#TCT-taxTblBody").find("tr:eq(" + Tr + ")").find(".TblDecuttaId").text(Deduct)
            $("#TCT-taxTblBody").find("tr:eq(" + Tr + ")").find(".Dacutta_amount").text(Deduct)
        }
    } 

    var ThisVat = GetMaskNumber($("#TCT-taxTblBody").find("tr:eq(" + Tr + ")").find(".TblAccountId").prev("td").text())
    var ThisTblVat = GetMaskNumber($("#TCT-taxTblBody").find("tr:eq(" + Tr + ")").find(".TblVatAccountId").prev("td").text())
    var ThisDiscount = GetMaskNumber($("#TCT-taxTblBody").find("tr:eq(" + Tr + ")").find("td:eq(7)").text())
    var ThisDeduct = GetMaskNumber($("#TCT-taxTblBody").find("tr:eq(" + Tr + ")").find(".TblDecuttaId").text())

    $("#TCT-taxTblBody").find("tr:eq(" + Tr + ")").find(".TblAccountId").prev("td").text(ThisVat)
    $("#TCT-taxTblBody").find("tr:eq(" + Tr + ")").find(".TblVatAccountId").prev("td").text(ThisTblVat)
    $("#TCT-taxTblBody").find("tr:eq(" + Tr + ")").find("td:eq(7)").text(ThisDiscount)
    $("#TCT-taxTblBody").find("tr:eq(" + Tr + ")").find(".TblDecuttaId").text(ThisDeduct)
    $("#TCT-taxTblBody").find("tr:eq(" + Tr + ")").find(".TblTotalVatamount").text(ThisVat + ThisTblVat)
    $("#TCT-taxTblBody").find("tr:eq(" + Tr + ")").find(".Discount").text(ThisDiscount)
    $("#TCT-taxTblBody").find("tr:eq(" + Tr + ")").find(".Dacutta_amount").text(ThisDeduct)

}
function SetTaxFinalCost(Tr, FinalCost) {
    $("#TCT-taxTblBody").find("tr:eq(" + Tr + ")")
        .find(".FinalCostPrice").text(FinalCost)
}