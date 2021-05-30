var Credit_limit_enum = {
    No_credit: 1,
    Unlimited: 2,
    Amount: 3
};
var Doc_type = {
    Invoice: 1,
    Credit_Memo: 2,
    Debit_Memo: 3,
    Payment: 4,
    Return: 5
};
var HasNoPayment = "hide";
function GetVendoreData() {
    VendoreValid = false;

    var Vendor_id = $("#Vendor_id").val()
    $.ajax({
        url: "/Receivable/Creditor_setting/HasPassword?id=" + Vendor_id,
        method: "POST",
        success: function (HasPassword) {
            if (HasPassword) {
                ModelMsg("<div class='col-sm-12'><label>Enter Password</label><input type='password' class='GroupSettingPas form-control'  /></div>", "Enter Password"
                    , false,
                    function () {
                        $.ajax({
                            url: "/Receivable/Creditor_setting/CheckPass?Id=" + Vendor_id + "&Password=" + $(document).find(".GroupSettingPas").val(),
                            method: "POST",
                            success: function (data) {
                                if (data) {
                                    GetCreditorName(Vendor_id)
                                    VendoreValid = true;

                                } else {
                                    ModelMsg("<h4>Wrong Password</h4>", "Wrong Password", true)
                                    $("#Vendor_id").val("");
                                    VendoreValid = false;
                                }
                            }
                        })
                    }, function () {
                        if (!VendoreValid) {
                            $("#Vendor_id").val("");
                        }
                    })
            } else {
                GetCreditorName(Vendor_id)
            }
        }
    });
    try {
        //GetCustomerBalance(Vendor_id, function (VeAmount) {
        //    CalcCustomerBalance()
        //});
    } catch (err) {

    }
   
}
function GetCustomerBalance(Customer_id, CallBack = null) {
    $.ajax({
        url: "/Receivable/Creditor_setting/CheckCreditorLimit?id=" + Customer_id,
        method: "POST",
        success: function (CL) {
            CLType = CL.status;
            CLAmount = CL.amount
            if (CL.status == Credit_limit_enum.Amount) {
                VeAmount = CL.veAmount;
            }
            if (HasNoPayment == "hide") {
                if (CL.status == Credit_limit_enum.No_credit || CL.status == Credit_limit_enum.Amount) {
                    $(".ReceivablePayment").removeClass("hide");
                    VZeroSum = true;
                    CLAmount = CL.amount
                } else {
                    $(".ReceivablePayment").addClass("hide");
                    VZeroSum = false;
                }
            } else {
                VZeroSum = true;
            }
            MinAmount = CL.MinAmount;
            MaxAmount = CL.MaxAmount;
            if (CallBack != null) {
                CallBack(CL.amount, MinAmount, MaxAmount)
            }
            try {
                if (getParameterByName("Piece") == "true") {
                    CalcNewBalance();
                }
            } catch (err) {

            }
        }
    })
}
function GetCreditorName(Id) {
    $.ajax({
        url: "/Receivable/Creditor_setting/GetNameAndCBookById?id=" + Id,
        success: function (data) {
            $("#VendorNamelbl").text(data.Name);
            $(".ReceivablePayment").find("#Check_book_id").val(data.CBook);
            $("#TransDiv").find("#Payment_terms_id").val(data.PTI);
            $("#TransDiv").find("#Shipping_method_id").val(data.SI);
        },
        method: "POST"
    })
}