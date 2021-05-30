var Doc_type = {
    Invoice: 1,
    Return: 5
};
$(document).on("change", ".Store_id", function () {
    var SiteId = $(document).find("select.Site_id");
    if ($(this).val()) {
        $.ajax({
            url: "/Inventory/Inv_store_site/GetSitesByStoreId?StoreId=" + $(this).val(),
            method: "POST",
            success: function (data) {
                $(SiteId).empty();
                $(SiteId).append("<option value=''></option>");
                $.each(data, function (k, i) {
                    $(SiteId).append("<option value='" + i.Id + "'>" + i.Name + "</option>");
                })
                ForceRefreshPicker();
            }
        })
    } else {
        $(SiteId).empty();
        $(SiteId).append("<option value=''></option>");
        ForceRefreshPicker();
    }
  
})
$(document).on("change", "#Item_id", function () {
    var ThisId = $(this).val();
    var ThisItem = $(this);
    var ST = 0;
    try {
        ST = $("input[name='ItemSearch']:checked").val();
    } catch (err) {
    }
    
    $.ajax({
        url: "/Inventory/Inv_item/ItemHasPassword?Item_id=" + $(this).val(),
        success: function (data) {
            if (data) {
                ModelMsg("<input id='InvItemPassword' type='password'/>", "Enter Password", false,
                    function () {
                        $.ajax({
                            url: "/Inventory/Inv_item/ItemCheckPassword?Item_id=" + ThisId + "&Password="
                                + $(document).find("#InvItemPassword").val(),
                            success: function (data) {
                                if (data) {
                                    GetItemName(ThisId);
                                }
                                else {
                                    $(ThisItem).val("");
                                    $(ThisItem).selectpicker("refresh")
                                    ModelMsg("<h3 class='text-danger text-center'>Wrong Password</h3>", "", true)
                                }
                            },
                            method: "POST"
                        })
                    }, function () {
                        $(ThisItem).val("");
                        $(ThisItem).selectpicker("refresh");
                    })
            } else {
                GetItemName(ThisId)
            }
        },
        method: "POST"
    })
   
})
function GetItemName(ThisId) {
    GetUnitOfMeasure(ThisId);
    $.ajax({
        url: "/Inventory/Inv_item/GetItemName?Item_id=" + ThisId,
        success: function (data) {
            if (window.location.pathname == "/Inventory/Inv_receive_po/Create"
                && getParameterByName("Sales") == "true") {
                $("#ItemName").val(data)
            } else {
                $("#ItemName").text(data)
            }
            ForceRefreshPicker();
        },
        method: "POST"
    });
}
var UOMDropDown = false;
function GetUnitOfMeasure(Id) {
    var ItemId = Id;
    if (!Id) {
        ItemId = $(document).find("#Item_id").val();
    }
    try {
        if (UOMDropDown) {
            GetUnitOfMeasureEq(ItemId);
        } else {
            GetOneUnitOfMeasure(ItemId)
        }
    } catch (err) {
        GetOneUnitOfMeasure(ItemId)
    }
   
}
function GetUnitOfMeasureEq(Id) {
    var ItemId = Id;
    if (!Id) {
        ItemId = $(document).find("#Item_id").val();
    }
    var UOM = "";
    if ($("select#UOM_id").length > 0) {
        UOM = $(document).find("select#UOM_id");
    } else {
        UOM = $(document).find("select#UOM");
    }
    $(UOM).empty();
    $.ajax({
        url: "/Inventory/Inv_item/GetItemUnitMeasureSelect?Item_id=" + ItemId,
        success: function (data) {
            $(UOM).append("<option value='' date-Eq='' data-action=''></option>")
            $.each(data, function (k, i) {
                //data-action='" + i.Action + "'
                $(UOM).append("<option value='" + i.Id + "' date-Eq='" + i.Qty + "'>" + i.Name + "</option>")
            })
            try {
                $(UOM).find("option[value='" + data[0].MainUnit + "']").attr("selected", "selected");
                ForceRefreshThisPicker($(UOM));
            } catch(err){

            }
          
        },
        method: "POST"
    });
}
function GetOneUnitOfMeasure(ItemId) {
    $.ajax({
        url: "/Inventory/Inv_item/GetItemUnitMeasure?Item_id=" + ItemId,
        success: function (data) {
            var UOM = "";
            if ($("#UOM_id").length > 0) {
                UOM = $(document).find("#UOM_id");
            } else {
                UOM = $(document).find("#UOM");
            }
            $(UOM).text(data.Unit)
            $(UOM).val(data.Unit)
        },
        method: "POST"
    });
}
function PoMergeJvTax() {
    RunAfterAjax(function () {
        var FirstTr = $("#itemsBody").find("tr").first();
        var FirstVat = $(FirstTr).find(".BodyVat").attr("data-tax-accid")
        var FirstVatRow = $(FirstTr).find(".BodyVat").attr("data-jvindex")
        var FirstVatTbl = $(FirstTr).find(".BodyTableVat").attr("data-tax-accid")
        var FirstVatTblRow = $(FirstTr).find(".BodyTableVat").attr("data-jvindex")
        var FirstDeduct = $(FirstTr).find(".BodyDeduct").attr("data-tax-accid")
        var FirstDeductRow = $(FirstTr).find(".BodyDeduct").attr("data-jvindex")

        $("#itemsBody").find("tr").not(FirstTr).each(function () {
            var BVAID = $(this).find(".BodyVat").attr("data-tax-accid");
            if ($(this).find(".BodyVat").attr("data-tax-accid") == FirstVat) {
                $(this).find(".BodyVat").attr("data-jvindex", FirstVatRow);
            } else {
                var OtherRow = GetGlRowClassByAccId($(this).find(".BodyVat").attr("data-tax-accid"));
                $(this).find(".BodyVat").attr("data-jvindex", OtherRow);
            }
            if ($(this).find(".BodyTableVat").attr("data-tax-accid") == FirstVatTbl) {
                if (FirstVatTblRow) {
                    $(this).find(".BodyTableVat").attr("data-jvindex", FirstVatTblRow);
                }
            } else {
                var OtherRow = GetGlRowClassByAccId($(this).find(".BodyTableVat").attr("data-tax-accid"));
                $(this).find(".BodyTableVat").attr("data-jvindex", OtherRow);

            }
            if ($(this).find(".BodyDeduct").attr("data-tax-accid") == FirstDeduct) {
                $(this).find(".BodyDeduct").attr("data-jvindex", FirstDeductRow);
            } else {
                var OtherRow = GetGlRowClassByAccId($(this).find(".BodyDeduct").attr("data-tax-accid"));
                $(this).find(".BodyDeduct").attr("data-jvindex", OtherRow);

            }
        })

    })
}
function EmptyRes() {
    $("#Res").empty();
}
function InvMarkTaxJv(TaxJv) {
    if ($("#itemsBody").find("tr").last().find(".BodyDeduct").attr("data-tax-id") != 0) {
        $("#itemsBody").find("tr").last().find(".BodyDeduct").attr("data-jvIndex", $("#TCGE-TTbl").find("tr").last().attr("class"));
        $("#itemsBody").find("tr").last().find(".BodyVat").attr("data-jvIndex", $("#TCGE-TTbl").find("tr").last().prev().attr("class"));

        //if (TaxJv.ShowTransactions.length > 4) {
        //    $("#itemsBody").find("tr").last().find(".BodyTableVat").attr("data-jvIndex", $("#TCGE-TTbl").find("tr").last().prev().prev().attr("class"));
        //} else {
        //    $("#itemsBody").find("tr").last().find(".BodyTableVat").attr("data-jvIndex", $("#TCGE-TTbl").find("tr").last().prev().attr("class"));
        //}

    } else {
        $("#itemsBody").find("tr").last().find(".BodyVat").attr("data-jvIndex", $("#TCGE-TTbl").find("tr").last().attr("class"));
        //if ($("#itemsBody").find("tr").last().find(".BodyTableVat").attr("data-tax-accid") != 0) {
        //    if (TaxJv.ShowTransactions.length > 4) {
        //        $("#itemsBody").find("tr").last().find(".BodyTableVat").attr("data-jvIndex", $("#TCGE-TTbl").find("tr").last().prev().prev().attr("class"));
        //    } else {
        //        $("#itemsBody").find("tr").last().find(".BodyTableVat").attr("data-jvIndex", $("#TCGE-TTbl").find("tr").last().prev().attr("class"));
        //    }
        //}
    }

}
function CalcItemDetils(GetAvaliable = false, ItemId,
    StoreId, Qty, UOM, SoldItems = 0, $Qty, $Cost, $UnitPrice, CallBack = null, NotEnoughInv = "") {

   // var JsonData = { SoldItems: SoldItems }//InvSalesPo Class
    $.ajax({
        url: "/Inventory/Inv_receive_po_items/GetItemDetails?ItemId=" + ItemId + "&GetAvaliable=" + GetAvaliable + "&StoreId=" + StoreId
            + "&Qty=" + Qty + "&UOM=" + UOM + "&SoldItems=" + SoldItems,
       // data: JSON.stringify(JsonData),
        contentType: 'application/json',
        success: function (data) {
            var CostPrice = 0;

            var AvQty = 0;
            $.each(data.POs, function (k, i) {
                AvQty += i.Qty
            })
            if (data.Avaliable) {
                if (GetAvaliable) {
                    $($Qty).val(AvQty)
                }
                $($Cost).val(data.CostPrice).trigger('mask.maskMoney').trigger("change");
                CostPrice = data.CostPrice;
            } else {
                NotValid($($Qty), NotEnoughInv,false)
                $($Qty).val("");
                $($Cost).val("");
                $($UnitPrice).val("");
            }
            if (CallBack != null) {
                CallBack(AvQty, CostPrice, data.EqQty);
            }
        },
        method: "POST"
    })
}

// Item Serials Start
function CheckIfItemHasProp(Item_id, Qty, TrIndex) {
    if (!ModelVisiable()) {
        CheckIfItemHasPropCacl(Item_id, Qty, TrIndex);
    } else {
        setTimeout(function () {
            if (!ModelVisiable()) {
                CheckIfItemHasPropCacl(Item_id, Qty, TrIndex);
            } else {
                CheckIfItemHasProp(Item_id, Qty, TrIndex);
            }
        }, 500)
    }
}

function CheckIfItemHasPropCacl(Item_id, Qty, TrIndex) {
    $.ajax({
        url: "/Inventory/Inv_item/HasProp?ItemId=" + Item_id,
        method: "POST",
        success: function (data) {
            if (data.Has_serial || data.Has_warranty || data.Has_expiry_date) {
                AddItemsSerials(data.Has_serial,
                    data.Has_warranty, data.Has_expiry_date,
                    Qty, TrIndex, Item_id);
                $("#itemsBody").find("tr:eq(" + TrIndex + ")").find(".ItemId").attr("data-hasProp", "true")
                $("#itemsBody").find("tr:eq(" + TrIndex + ")").find(".SerialsPoBtn").removeAttr("disabled");

            } else {
                $("#itemsBody").find("tr:eq(" + TrIndex + ")").find(".ItemId").attr("data-hasProp", "false")
                $("#itemsBody").find("tr:eq(" + TrIndex + ")").find(".SerialsPoBtn").attr("disabled", "disabled")
                RunAfterAjax(function () {
                    $("#InvCreate").removeAttr("disabled");
                })
            }
        }
    })
}
function CheckJustIfItemHasProp(Item_id, TrIndex, CallBack = null) {
    $.ajax({
        url: "/Inventory/Inv_item/HasProp?ItemId=" + Item_id,
        method: "POST",
        success: function (data) {
            if (data.Has_serial || data.Has_warranty || data.Has_expiry_date) {
                $("#itemsBody").find("tr:eq(" + TrIndex + ")").find(".ItemId").attr("data-hasProp", "true")
                $("#itemsBody").find("tr:eq(" + TrIndex + ")").find(".SerialsPoBtn").removeAttr("disabled")
            } else {
                $("#itemsBody").find("tr:eq(" + TrIndex + ")").find(".ItemId").attr("data-hasProp", "false")
                $("#itemsBody").find("tr:eq(" + TrIndex + ")").find(".SerialsPoBtn").attr("disabled", "disabled")
                RunAfterAjax(function () {
                    $("#InvCreate").removeAttr("disabled");
                })
            }

            if (CallBack != null) {
                CallBack(data.Has_serial, data.Has_warranty, data.Has_expiry_date);
            }
        }
    })
}
function CheckAllHasSerial(Tr) {
    var Valid = true;
    if (Tr) {
        $("#itemsBody").find("tr:eq(" + Tr + ")").each(function () {
            var ThisSerial = $(this).find(".ItemSerials").text();
            if ($(this).find(".ItemId").attr("data-hasProp") == "true"
                && ($(this).find(".ItemSerials").text() == "" || $(this).find(".ItemSerials").text() == "[]")) {
                Valid = false;
            }
        })
    } else {
        $("#itemsBody").find("tr").each(function () {
            var ThisSerial = $(this).find(".ItemSerials").text();
            var asd = $(this).find(".ItemId").attr("data-hasProp");
            if ($(this).find(".ItemId").attr("data-hasProp") == "true"
                && ($(this).find(".ItemSerials").text() == "" || $(this).find(".ItemSerials").text() == "[]")) {
                Valid = false;
            }
        })
    }

    return Valid;
}
$(document).on("click", ".SerialsPoBtn", function () {
    var ItemId = $(this).parents("tr").find(".ItemId").text();
    var Index = $(this).parents("tr").index();
    var Qty = parseFloat($(this).parents("tr").find(".ItemQty").text());
    CheckJustIfItemHasProp(ItemId, Index, function (S,W,E) {
        AddItemsSerials(S, W, E, Qty, Index, ItemId, true, true);
    })

})
function ClearSerial(Tr) {
    $("#itemsBody").find("tr:eq(" + Tr + ")").find(".ItemSerials").text("");
}
function SetSerialFromTable(FromExist, Index) {
    if (FromExist) {
        var Serials = GetSerials(Index);

        $.each(Serials, function (k, i) {
            var ThisTr = $(document).find(".SerialTable").find("tbody").find("tr:eq(" + k + ")");
            $(ThisTr).find('.Serial').val(i.Serial);
            $(ThisTr).find('.StartW').val(i.Start_warranty);
            $(ThisTr).find('.EndW').val(i.End_warranty);

            $.each(i.Expiry, function (ek, ei) {
                $(document).find(".ExpieryTable").
                    find("tbody").append(GetExpieryRow(ei.Number,ei.Date))
            })

        })
       // ClearSerial(Index)
        $(document).find(".SerialSelect").trigger("change");
    }
}
function AddItemsSerials(HasSerial, HasWarranty, HasExpiry, Qty, Index, ItemId, FromExist = false,
    HideClose = false) {
    var Table = GetSerialTbl(HasSerial, HasWarranty, HasExpiry)
    var ETable = "";
    if (HasExpiry && getParameterByName("Sales")!="true") {
        ETable = ExpieryTable()
        Table = ETable +"<br/><br/>"+ Table;
    }
    var PoId = null;
    if ($("#Doc_type").val() == Doc_type.Return) {
        PoId = $("#GNums").val();
    }
    var RplData = "";
    if ((getParameterByName("Sales") == "true" && $("#Doc_type").val() == Doc_type.Invoice)
        || (getParameterByName("Sales") != "true" && $("#Doc_type").val() == Doc_type.Return)) {
        var Serials = GetSerials();
        $.ajax({
            url: "/Inventory/Inv_receive_po_items/GetItemsSerials?ItemId=" + ItemId + "&Doc=" + $("#Doc_type").val() + "&PoId=" + PoId + "&Sales=" + getParameterByName("Sales"),
            method: "POST",
            success: function (data) {
                if (data.length > 0) {
                    for (var i = 1; i <= Qty; i++) {
                        var Opt = "<option></option>";
                        var ExDate = "<option></option>";
                        $.each(data, function (k, i) {
                            Opt += "<option value='" + i.Serial + "'>" + i.Serial + "</option>";

                        })
                        if (HasExpiry) {
                            $.each(data[0].ExDate, function (ek, ei) {
                                ExDate += "<option value='" + ei + "'>" + ei + "</option>";
                            })
                        }
                        RplData += "<tr class='TrWS'>";
                        if (HasSerial) {
                            RplData += "<td><select class='form-control  Serial SerialSelect' name='Serial'>" + Opt + "</select ></td> ";
                        }
                        if (HasWarranty) {
                            RplData += "<td><input type='date' class='form-control  StartW' name='StartW' placeholder='Start Warranty' /></td>";
                            RplData += "<td><input type='date' class='form-control  EndW' name='EndW' placeholder='End Warranty' /></td>";
                        }
                        if (HasExpiry) {
                            RplData += "<td><select class='form-control Expiry ExpirySelect' name='Expiry'>" + ExDate + "</select ></td> ";
                        }
                        RplData += "</tr>";
                    }
                    SerialModel(Table, RplData, Index, ItemId, function () {
                        SetSerialFromTable(FromExist, Index)
                    }, HideClose);
                } else {
                    ModelMsg(ItemDHS, $("#ItemName").val())
                    RunAfterAjax(function () {
                        $("#itemsBody").find("tr").last().find(".DeletePoItem").removeAttr("disabled");
                        $("#itemsBody").find("tr").last().find(".DeletePoItem").trigger("click");
                        $("#InvCreate").removeAttr("disabled");
                    })
                  
                }
              
            }
        })
    } else {
        if ((getParameterByName("Sales") == "true" && $("#Doc_type").val() == Doc_type.Return)) {
            $.ajax({
                url: "/Inventory/Inv_receive_po_items/GetItemsSerials?ItemId=" + ItemId + "&Doc=" + $("#Doc_type").val() + "&PoId=" + PoId + "&Sales=" + getParameterByName("Sales"),
                method: "POST",
                success: function (data) {
                    $.each(data, function (k, i) {
                        RplData += "<tr class='TrWS'>";
                        if (HasSerial) {
                            RplData += "<td><input data-itemid='" + ItemId + "' type='text' class='form-control  Serial' value='" + i.Serial + "' name='Serial' placeholder='Serial'></td>";
                        }
                        if (HasWarranty) {
                            RplData += "<td><input type='date' class='form-control  StartW' name='StartW' placeholder='Start Warranty'></td>";
                            RplData += "<td><input type='date' class='form-control  EndW' name='EndW' placeholder='End Warranty'></td>";
                        }
                        if (HasExpiry) {
                            RplData += "<td><input type='date' class='form-control Expiry' name='Expiry' placeholder='Expiry Date' value='" + i.SalesEx +"' /></td>";
                        }
                    })
                    SerialModel(Table, RplData, Index, ItemId, function () {
                        SetSerialFromTable(FromExist, Index)
                    }, HideClose);

                }
            })
        } else {
            for (var i = 1; i <= Qty; i++) {
                RplData += "<tr class='TrWS'>";
                if (HasSerial) {
                    RplData += "<td><input data-itemid='" + ItemId + "' type='text' class='form-control  Serial' name='Serial' placeholder='Serial'></td>";
                }
                if (HasWarranty) {
                    RplData += "<td><input type='date' class='form-control  StartW' name='StartW' placeholder='Start Warranty'></td>";
                    RplData += "<td><input type='date' class='form-control  EndW' name='EndW' placeholder='End Warranty'></td>";
                }
                if (HasExpiry && getParameterByName("Sales")=="true") {
                    RplData += "<td><input type='date' class='form-control Expiry' name='Expiry' placeholder='Expiry Date' value='" + i.SalesEx + "' /></td>";
                }
            }
            SerialModel(Table, RplData, Index, ItemId, function () {
                SetSerialFromTable(FromExist, Index)
            }, HideClose);
        }
    }
}
function SerialModel(Table, RplData, Index, ItemId, CallBack = null, HideClose = false) {
    Table = Table.replace("{{Rep}}", RplData);
    ModelMsg(Table, $("#itemsBody").find("tr:eq(" + Index + ")").find(".ItemName").text() + "<input class='hide ModelItemId' value='" + $("#itemsBody").find("tr:eq(" + Index + ")").find(".ItemId").text()+"'/>", false, function () {
        CheckValdInputs(Index, ItemId)
    }, function () {
            $(document).find(".Serial").each(function () {
                if ($(this).val().trim() == "") {
                    $("#itemsBody").find("tr").last().find(".DeletePoItem").removeAttr("disabled").trigger("click");
                    $("#InvCreate").removeAttr("disabled");
                    $("#AddNewItem").removeAttr("disabled");
                }
            })
    }, "Submit", function () {
        if (getParameterByName("Sales") != "true") {
            $("#Modal").find(".StartW").val($("#Doc_date").val());
            $("#Modal").find(".EndW").val($("#Doc_date").val());
            $("#Modal").find(".Expiry").val($("#Doc_date").val());
        } else {
            $("#Modal").find(".StartW").val($("#TCGE-JEDate").val());
            $("#Modal").find(".EndW").val($("#TCGE-JEDate").val());
            $("#Modal").find(".Expiry").val($("#TCGE-JEDate").val());
        }
        if (CallBack != null) {
            CallBack();
        }
    }, '70vw', HideClose)
}
$(document).on("focusout", ".Serial", function () {
    var ThisSerial = $(this);
    var ItemId = $(this).attr("data-itemid");
    $("#Modal").find(".SerialTable").find("input").removeClass("NotValid")
    var SamePageValid = true;
    if ($(this).val()) {
        var This = $(this);
        $("#Modal").find(".SerialTable").find("input").not($(This)).each(function () {
            if ($(this).val() == $(This).val()) {
                NotValid($(this))
                NotValid($(This))
                SamePageValid = false;
            }
        })
    }
    if (getParameterByName("Sales") != "true" ||
        (getParameterByName("Sales") == "true" && $("#Doc_type").val() == Doc_type.Return)) {
        $(document).find("#ModelSubmit").attr("disabled", "disabled");
        $.ajax({
            url: "/Inventory/Inv_receive_po_items/CheckSerial?Serial=" + $(ThisSerial).val() + "&ItemId=" + ItemId,
            method: "POST",
            success: function (data) {
                var Valid = true;
                var AddSerials = GetSerials();
                if ($("#Doc_type").val() == Doc_type.Invoice) {
                    if (data) {
                        $("#Modal").find(".SerialTable").find("input").each(function () {
                            if (($(this).val() == $(ThisSerial).val())) {
                                NotValid($(this), "Duplicate Serial");
                                Valid = false;
                            }
                        })
                    } else if (AddSerials.length > 0) {
                        $("#Modal").find(".SerialTable").find("input").each(function () {
                            if (AddSerials.find(x => x.Serial == $(this).val() && x.Item_id != $(document).find(".ModelItemId").val())) {
                                NotValid($(this), "Duplicate Serial");
                                Valid = false;
                            }
                        })
                    } else {
                        $(document).find("#ModelSubmit").removeAttr("disabled");
                    }
                    if (Valid || SamePageValid) {
                        $(document).find("#ModelSubmit").removeAttr("disabled");
                    }
                } else {
                    if (!data) {
                        $("#Modal").find(".SerialTable").find("input").each(function () {
                            if (($(this).val() == $(ThisSerial).val())) {
                                NotValid($(this), "Serial Does Not Exist");
                                Valid = false;
                            }
                        })
                    }
                    $("#Modal").find(".SerialTable").find("input").each(function () {
                        if (AddSerials.find(x => x.Serial == $(this).val()) !== undefined) {
                            NotValid($(this), "Duplicate Serial");
                            Valid = false;
                        }
                    })
                    if (Valid || SamePageValid) {
                        $(document).find("#ModelSubmit").removeAttr("disabled");
                    }
                }

            }
        })
    }
})
function CheckValdInputs(Index, ItemId) {
    var Reopen = false;
    var Serials = [];
    $("#Modal").find(".SerialTable").find("input").each(function () {
        if (!$(this).val()) {
            NotValid($(this));
            Reopen = true;
        }
    })
    if (!Reopen) {
        var Expiery = [];
        if (getParameterByName("Sales") == "false") {
            $("#Modal").find(".ExpieryTable tr").each(function () {
                if ($(this).find(".ExpieryNumber").val()
                    && $(this).find(".ExpieryDate").val()) {
                    Expiery.push({
                        Number: $(this).find(".ExpieryNumber").val(),
                        Date: $(this).find(".ExpieryDate").val()
                    })
                }
            })
        }
        $("#Modal").find(".TrWS").each(function () {
            var ThisObj = {
                Serial: $(this).find(".Serial").val(),
                Start_warranty: $(this).find(".StartW").val(),
                End_warranty: $(this).find(".EndW").val(),
              //  Expiry: Expiery,
                Item_id: ItemId
            };
            if (getParameterByName("Sales") == "false") {
                ThisObj.Expiry = Expiery;
            }
            else {
                ThisObj.Expiry_date = $(this).find(".Expiry").val();
            }
            Serials.push(ThisObj)

        });

        $("#itemsBody").find("tr:eq(" + Index + ")").find(".ItemSerials").text(JSON.stringify(Serials));
        ModelCallBack = null;
        $("#InvCreate").removeAttr("disabled");

    } else {
        setTimeout(function () {
            $("#Modal").modal("show")
            ModelCallBack = CheckValdInputs;
        }, 500)
    }
}
$(document).on("change", ".SerialSelect", function () {
    $(document).find(".SerialSelect").find("option").show();
    $("#Modal").find(".modal-body").find(".SerialSelect").each(function () {
        var ThisTxt = $(this).val();
        $("#Modal").find(".modal-body").find(".SerialSelect").not($(this)).each(function () {
            $(this).find("option[value='" + ThisTxt + "']").hide();
        })
    })
})
function GetSerials(Tr = null) {
    var SerialsData = [];
    if (Tr == null) {
        $("#itemsBody").find("tr").each(function () {
            try {
                var ThisItemId = $(this).find(".ItemId").text();
                var SerialsDD = JSON.parse($(this).find(".ItemSerials").text())
                $.each(SerialsDD, function (k, i) {
                    i.Item_id = ThisItemId;
                    SerialsData.push(i);
                })
            } catch (err) {

            }
        });
    } else {
        $("#itemsBody").find("tr:eq(" + Tr + ")").each(function () {
            try {
                var ThisItemId = $(this).find(".ItemId").text();
                var SerialsDD = JSON.parse($(this).find(".ItemSerials").text())
                $.each(SerialsDD, function (k, i) {
                    i.Item_id = ThisItemId;
                    SerialsData.push(i);
                })
            } catch (err) {

            }
        });
    }
    return SerialsData;
}

function GetSerialTbl(HasSerial, HasWarranty, HasExpiry) {
    var Table = "<table class='center m-sm-auto SerialTable'><thead><tr>";
    if (HasSerial) {
        Table += "<td>Serial</td>";
    }
    if (HasWarranty) {
        Table += "<td>Warranty Start</td>";
        Table += "<td>Warranty End</td>";
    }
    if (HasExpiry && getParameterByName("Sales")=="true") {
        Table += "<td>Expiry</td>";
    }
    Table += "</tr></thead><tbody> {{Rep}} </tbody></table>";
    return Table;
}
function ExpieryTable() {
    var Table = "<div class='clearfix'><button class='AddNewExpieryRow btn btn-primary'>Add Expiery Row</button></div>"
        + "<table class='center m-sm-auto ExpieryTable' style='margin-bottom:20px;min-width: 65%;'><thead><tr>" 
        +"<td>Number</td><td>Expiery Date</td><td></td>"
        +"</tr></thead><tbody></tbody></table>";
    return Table;
}

$(document).on("click", ".AddNewExpieryRow", function () {
    $(document).find(".ExpieryTable").find("tbody")
        .append(GetExpieryRow())
})
$(document).on("focusout", ".ExpieryNumber", function () {
    var ExpieryNum = 0;
    $(document).find(".ExpieryNumber").each(function () {
        ExpieryNum += parseFloat($(this).val());
    })
    var ItemId = $(document).find(".ModelItemId").val();
    var TblItemQty = parseFloat($(document).find("#itemsBody").find(".ItemId:textEquals(" + ItemId + ")")
        .parent("tr").find(".ItemQty").text())
    if (ExpieryNum > TblItemQty) {
        $(this).val("");
    }
})
function GetExpieryRow(Number,Date) {
    return "<tr>"
        + "<td><input class='ExpieryNumber' type='number' value='" + Number + "'></td>"
        + "<td><input class='ExpieryDate' type='date' value='" + Date + "'></td>"
        + "<td><i class='fa fa-times DeletThisExpiery' aria-hidden='true'></i></td>"
        + "</tr>"
}
$(document).on("click", ".DeletThisExpiery", function () {
    $(this).parents("tr").remove();
})
// Item Serials End
