function CSearchBtn() {
    var CompID = $("#CompanyID").val();

    if (CompID == "") {
        $("#ErrorInSearch").text("Fill Company ID to Search");
    }
    else if (CompID.length < 5) {
        $("#ErrorInSearch").text("Company ID required 5 characters");
    }
    else if (CompID.length > 5) {
        $("#ErrorInSearch").text("Company ID required 5 characters");
    }
    else {
        $("#ErrorInSearch").text("");
        $.ajax({
            type: "GET",
            url: "/MainCompany/GetCompanyInfo?CompanyID=" + CompID,
            success: function (data) {

                if (data == "NoCompanyExist") {
                    $("#ErrorInSearch").text("Company ID Not Exist");
                }
                else {

                    var empty = "";

                    //Empty All Error text If Exist
                    $("#ECompanyID").text(empty);
                    $("#ECompanyName").text(empty);
                    //$("#ECompanyAlies").text(empty);
                    $("#ECountryName").text(empty);
                    $("#ELanguage").text(empty);
                    $("#ECurrency").text(empty);
                    $("#ECompanyMainActivity").text(empty);

                    $("#EStreetName").text(empty);
                    $("#EBuldingNo").text(empty);
                    $("#EFloorNo").text(empty);
                    $("#EGovernorate").text(empty);
                    $("#BarValid").text("True");


                    $("#ECompanyType").text(empty);
                    // var EEstablishDate = $("#EEstablishDate").text();
                    $("#ECommericalRegister").text(empty);
                    // var ECommericalOffice = $("#ECommericalOffice").text();
                    $("#ETaxFileNo").text(empty);
                    // var ETaxOffice = $("#ETaxOffice").text();

                    $("#EInternational1").text(empty);
                    $("#ETelephone1").text(empty);
                    $("#ETelephoneEX1").text(empty);
                    $("#EInternational2").text(empty);
                    $("#ETelephone2").text(empty);
                    $("#ETelephoneEX2").text(empty);
                    $("#EInternational3").text(empty);
                    $("#ETelephone3").text(empty);
                    $("#ETelephoneEX3").text(empty);
                    $("#EInternational4").text(empty);
                    $("#ETelephone4").text(empty);
                    $("#ETelephoneEX4").text(empty);
                    $("#EInternational5").text(empty);
                    $("#ETelephone5").text(empty);
                    $("#ETelephoneEX5").text(empty);


                    $("#EFax1").text(empty);
                    $("#EFaxEX1").text(empty);
                    $("#EFax2").text(empty);
                    $("#EFaxEX2").text(empty);
                    $("#EFax3").text(empty);
                    $("#EFaxEX3").text(empty);
                    $("#EFax4").text(empty);
                    $("#EFaxEX4").text(empty);
                    $("#EFax5").text(empty);
                    $("#EFaxEX5").text(empty);

                    //new validation
                    $("#CNError").text(empty);
                    $("#CAError").text(empty);
                    $("#CONError").text(empty);
                    $("#LError").text(empty);
                    $("#CuError").text(empty);
                    $("#ActivityError").text(empty);
                    $("#StreetNameError").text(empty);
                    $("#BuildingNoError").text(empty);
                    $("#GovError").text(empty);
                    $("#CTError").text(empty);
                    $("#CRError").text(empty);
                    $("#TFError").text(empty);
                    //delete border of error
                    $("#CompanyID").removeClass('input-error');
                    $("#CompanyName").removeClass('input-error');
                    $("#CountryName").removeClass('input-error');
                    $("#Language").removeClass('input-error');
                    $("#Currency").removeClass('input-error');
                    $("#CompanyMainActivity").removeClass('input-error');
                    $("#StreetName").removeClass('input-error');
                    $("#BuldingNo").removeClass('input-error');
                    $("#Governorate").removeClass('input-error');
                    $("#CompanyType").removeClass('input-error');
                    $("#CommericalRegister").removeClass('input-error');
                    $("#TaxFileNo").removeClass('input-error');


                    $("#UpdateWithSearch").show("fast");
                    $("#UpdateWithoutSearch2").hide();
                    $("#UpdateWithSearch").text("You Can Update Now..");
                    $("#CompanyID").prop("disabled", true);
                    $("#Currency").prop("disabled", true);
                    $("#SaveBtn").prop("disabled", true);

                    $(".fa-lock").show("slow");

                    if (data.CommericalRegisterPath.length > 0) {
                        $(".DownloadComm").addClass("input-success");
                    } else {
                        $(".DownloadComm").addClass("input-error");
                    }

                    if (data.ExportIDPath.length > 0) {
                        $(".DownloadExport").addClass("input-success");
                    } else {
                        $(".DownloadExport").addClass("input-error");
                    }

                    if (data.ImportIDPath.length > 0) {
                        $(".DownloadImport").addClass("input-success");
                    } else {
                        $(".DownloadImport").addClass("input-error");
                    }

                    if (data.TaxFilePath.length > 0) {
                        $(".DownloadTax").addClass("input-success");
                    } else {
                        $(".DownloadTax").addClass("input-error");
                    }

                    if (data.VatIDPath.length > 0) {
                        $(".DownloadVat").addClass("input-success");
                    } else {
                        $(".DownloadVat").addClass("input-error");
                    }

                    if (data.InsuranceIDPath.length > 0) {
                        $(".DownloadInsurance").addClass("input-success");
                    } else {
                        $(".DownloadInsurance").addClass("input-error");
                    }

                    $(".DownloadComm").show();
                    $(".DownloadTax").show();
                    $(".DownloadVat").show();
                    $(".DownloadImport").show();
                    $(".DownloadExport").show();
                    $(".DownloadInsurance").show();

                    if (data.Status === "True") {
                        $("#Status").prop("checked", true);
                    }
                    else {
                        $("#Status").prop("checked", false);
                    }

                    //$("#InputLogo").hide();
                    //$("#InputCommerical").hide();
                    //$("#InputTax").hide();
                    //$("#InputVat").hide();
                    //$("#InputImport").hide();
                    //$("#InputExport").hide();
                    //$("#InputInsurance").hide();

                    $("#ImageLogo").attr('src', data.Logo);
                    /*
                    $("#CommericalRegisterImg").attr('src', data.CommericalRegisterImg);
                    $("#TaxFileImg").attr('src', data.TaxFileImg);
                    $("#VatIDImg").attr('src', data.VatIDImg);
                    $("#ImportIDImg").attr('src', data.ImportIDImg);
                    $("#ExportIDImg").attr('src', data.ExportIDImg);
                    */
                    $("#CompanyName").val(data.CompanyName);
                    $("#CompanyAlies").val(data.CompanyAlies);
                    $("#CountryName").val(data.CountryName);
                    $("#Language").val(data.Language);
                    $("#Currency").val(data.Currency);
                    $("#CompanyMainActivity").val(data.CompanyMainActivity);
                    $("#StreetName").val(data.StreetName);
                    $("#BuldingNo").val(data.BuldingNo);
                    $("#FloorNo").val(data.FloorNo);
                    $("#Area").val(data.Area);
                    $("#City").val(data.City);
                    $("#Governorate").val(data.Governorate);
                    $("#CompanyType").val(data.CompanyType);
                    $("#EstablishDate").val(data.EstablishDate);
                    $("#CommericalRegister").val(data.CommericalRegister);
                    $("#CommericalOffice").val(data.CommericalOffice);
                    $("#TaxFileNo").val(data.TaxFileNo);
                    $("#TaxOffice").val(data.TaxOffice);
                    $("#VatID").val(data.VatID);
                    $("#TaxVaOffice").val(data.TaxVaOffice);
                    $("#ImporterID").val(data.ImporterID);
                    $("#ImportOffice").val(data.ImportOffice);
                    $("#ExportID").val(data.ExportID);
                    $("#ExportOffice").val(data.ExportOffice);
                    $("#SocialInsuranceID").val(data.SocialInsuranceID);
                    $("#SocialInsuranceOffice").val(data.SocialInsuranceOffice);
                    $("#International1").val(data.International1);
                    $("#Telephone1").val(data.Telephone1);
                    $("#TelephoneEX1").val(data.TelephoneEX1);
                    //to not hide dives of commubication that have data in search               

                    if (data.International2 != null || data.Telephone2 != null) {
                        if (data.International2.length != 0 || data.Telephone2.length != 0) {
                            $("#T2").show("fast");
                            $("#International2").val(data.International2);
                            $("#Telephone2").val(data.Telephone2);
                            $("#TelephoneEX2").val(data.TelephoneEX2);
                            $("#Tbtn1").hide();
                        }
                    }
                    else { $("#T2").hide("fast"); }

                    if (data.International3 != null || data.Telephone3 != null) {
                        if (data.International3.length != 0 || data.Telephone3.length != 0) {
                            $("#T3").show("fast");
                            $("#International3").val(data.International3);
                            $("#Telephone3").val(data.Telephone3);
                            $("#TelephoneEX3").val(data.TelephoneEX3);
                            $("#Tbtn2").hide();
                        }
                    }
                    else { $("#T3").hide("fast"); }

                    if (data.International4 != null || data.Telephone4 != null) {
                        if (data.International4.length != 0 || data.Telephone4.length != 0) {
                            $("#T4").show("fast");
                            $("#International4").val(data.International4);
                            $("#Telephone4").val(data.Telephone4);
                            $("#TelephoneEX4").val(data.TelephoneEX4);
                            $("#Tbtn3").hide();
                        }
                    }
                    else { $("#T4").hide("fast"); }

                    if (data.International5 != null || data.Telephone5 != null) {
                        if (data.International5.length != 0 || data.Telephone5.length != 0) {
                            $("#T5").show("fast");
                            $("#International5").val(data.International5);
                            $("#Telephone5").val(data.Telephone5);
                            $("#TelephoneEX5").val(data.TelephoneEX5);
                            $("#Tbtn4").hide();
                        }
                    }
                    else { $("#T5").hide("fast"); }

                    $("#Code1").val(data.Code1);
                    $("#Fax1").val(data.Fax1);
                    $("#FaxEX1").val(data.FaxEX1);

                    if (data.Fax2 != null) {
                        if (data.Fax2.length != 0) {
                            $("#F2").show("fast");
                            $("#Code2").val(data.Code2);
                            $("#Fax2").val(data.Fax2);
                            $("#FaxEX2").val(data.FaxEX2);
                            $("#Fbtn1").hide();
                        }
                    } else { $("#F2").hide("fast"); }

                    if (data.Fax3 != null) {
                        if (data.Fax3.length != 0) {
                            $("#F3").show("fast");
                            $("#Code3").val(data.Code3);
                            $("#Fax3").val(data.Fax3);
                            $("#FaxEX3").val(data.FaxEX3);
                            $("#Fbtn2").hide();
                        }
                    } else { $("#F3").hide("fast"); }

                    if (data.Fax4 != null) {
                        if (data.Fax4.length != 0) {
                            $("#F4").show("fast");
                            $("#Code4").val(data.Code4);
                            $("#Fax4").val(data.Fax4);
                            $("#FaxEX4").val(data.FaxEX4);
                            $("#Fbtn3").hide();
                        }
                    } else { $("#F4").hide("fast"); }

                    if (data.Fax5 != null) {
                        if (data.Fax5.length != 0) {
                            $("#F5").show("fast");
                            $("#Code5").val(data.Code5);
                            $("#Fax5").val(data.Fax5);
                            $("#FaxEX5").val(data.FaxEX5);
                            $("#Fbtn4").hide();
                        }
                    } else { $("#F5").hide("fast"); }

                    $("#Website").val(data.Website);
                }
            },
            error: function (req, status, error) {
                $("#ErrorInSearch").text("Company ID Not Exist");
            }
        });
    }
}

/*
function ViewCommerical() {
    if ($("#CommericalRegisterImg").attr('src') == '') {
        $("#errorInLoad").show("slow");
        $("#TaxFileImg").hide();
        $("#VatIDImg").hide();
        $("#ImportIDImg").hide();
        $("#ExportIDImg").hide();
    }
    else {
        $("#CommericalRegisterImg").show("slow");
        $("#TaxFileImg").hide();
        $("#VatIDImg").hide();
        $("#ImportIDImg").hide();
        $("#ExportIDImg").hide();
        $("#errorInLoad").hide();
    }
}
function ViewTax() {
    if ($("#TaxFileImg").attr('src') == '') {
        $("#errorInLoad").show("slow");
        $("#CommericalRegisterImg").hide();
        $("#VatIDImg").hide();
        $("#ImportIDImg").hide();
        $("#ExportIDImg").hide();
    }
    else {
        $("#TaxFileImg").show("slow");
        $("#CommericalRegisterImg").hide();
        $("#VatIDImg").hide();
        $("#ImportIDImg").hide();
        $("#ExportIDImg").hide();
        $("#errorInLoad").hide();
    }
}
function ViewVat() {
    if ($("#VatIDImg").attr('src') == '') {
        $("#errorInLoad").show("slow");
        $("#CommericalRegisterImg").hide();
        $("#ImportIDImg").hide();
        $("#ExportIDImg").hide();
        $("#TaxFileImg").hide();
    } else {
        $("#VatIDImg").show("slow");
        $("#CommericalRegisterImg").hide();
        $("#ImportIDImg").hide();
        $("#ExportIDImg").hide();
        $("#TaxFileImg").hide();
        $("#errorInLoad").hide();
    }
}
function ViewImport() {
    if ($("#ImportIDImg").attr('src') == '') {
        $("#errorInLoad").show("slow");
        $("#CommericalRegisterImg").hide();
        $("#VatIDImg").hide();
        $("#ExportIDImg").hide();
        $("#TaxFileImg").hide();
    } else {
        $("#ImportIDImg").show("slow");
        $("#CommericalRegisterImg").hide();
        $("#VatIDImg").hide();
        $("#ExportIDImg").hide();
        $("#TaxFileImg").hide();
        $("#errorInLoad").hide();
    }
}
function ViewExport() {
    if ($("#ImportIDImg").attr('src') == '') {
        $("#errorInLoad").show("slow");
        $("#CommericalRegisterImg").hide();
        $("#VatIDImg").hide();
        $("#ImportIDImg").hide();
        $("#TaxFileImg").hide();
    }
    else {
        $("#ExportIDImg").show("slow");
        $("#CommericalRegisterImg").hide();
        $("#VatIDImg").hide();
        $("#ImportIDImg").hide();
        $("#TaxFileImg").hide();
        $("#errorInLoad").hide();
    }
}
*/