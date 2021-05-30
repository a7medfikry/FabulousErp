function CSaveButton() {

    var CompID = $("#CompanyID").val();

    //C Main Info
    var CompanyName = $("#CompanyName").val();
    var CompanyAlies = $("#CompanyAlies").val();
    var CountryName = $("#CountryName").val();
    var Language = $("#Language").val();
    var Currency = $("#Currency").val();
    var CompanyMainActivity = $("#CompanyMainActivity").val();

    var Status = "False";
    if ($("#Status").is(":checked")) {
        Status = "True";
    }

    //C Address Info
    var StreetName = $("#StreetName").val();
    var BuldingNo = $("#BuldingNo").val();
    var FloorNo = $("#FloorNo").val();
    var Governorate = $("#Governorate").val();

    //C Legal INfo
    var CompanyType = $("#CompanyType").val();
    var EstablishDate = $("#EstablishDate").val();
    var CommericalRegister = $("#CommericalRegister").val();
    var CommericalOffice = $("#CommericalOffice").val();
    var TaxFileNo = $("#TaxFileNo").val();
    var TaxOffice = $("#TaxOffice").val();

    //C Communication Info
    var International1 = $("#International1").val();
    var Telephone1 = $("#Telephone1").val();
    var TelephoneEX1 = $("#TelephoneEX1").val();
    var International2 = $("#International2").val();
    var Telephone2 = $("#Telephone2").val();
    var TelephoneEX2 = $("#TelephoneEX2").val();
    var International3 = $("#International3").val();
    var Telephone3 = $("#Telephone3").val();
    var TelephoneEX3 = $("#TelephoneEX3").val();
    var International4 = $("#International4").val();
    var Telephone4 = $("#Telephone4").val();
    var TelephoneEX4 = $("#TelephoneEX4").val();
    var International5 = $("#International5").val();
    var Telephone5 = $("#Telephone5").val();
    var TelephoneEX5 = $("#TelephoneEX5").val();

    var Fax1 = $("#Fax1").val();
    var FaxEX1 = $("#FaxEX1").val();
    var Fax2 = $("#Fax2").val();
    var FaxEX2 = $("#FaxEX2").val();
    var Fax3 = $("#Fax3").val();
    var FaxEX3 = $("#FaxEX3").val();
    var Fax4 = $("#Fax4").val();
    var FaxEX4 = $("#FaxEX4").val();
    var Fax5 = $("#Fax5").val();
    var FaxEX5 = $("#FaxEX5").val();
    var Code1 = $("#Code1").val();
    var Code2 = $("#Code2").val();
    var Code3 = $("#Code3").val();
    var Code4 = $("#Code4").val();
    var Code5 = $("#Code5").val();

    var Area = $("#Area").val();
    var City = $("#City").val();
    //Legal
    var VatID = $("#VatID").val();
    var TaxVaOffice = $("#TaxVaOffice").val();
    var ImporterID = $("#ImporterID").val();
    var ImportOffice = $("#ImportOffice").val();
    var ExportID = $("#ExportID").val();
    var ExportOffice = $("#ExportOffice").val();
    var SocialInsuranceID = $("#SocialInsuranceID").val();
    var SocialInsuranceOffice = $("#SocialInsuranceOffice").val();
    //Communication
    var Website = $("#Website").val();

    var Test = true;

    //Main Info Validate
    if (CompanyName.length == 0) {
        $("#ECompanyName").text("Company Name Is Required");
        GoToTab("CompanyName")
        Test = false;
    } else { $("#ECompanyName").text(""); }
    
    if (CountryName.length == 0) {
        $("#ECountryName").text("Country Name Is Required");
        $("#CountryName").focus();
        GoToTab("CountryName")

        Test = false;
    } else { $("#ECountryName").text(""); }

    if (Language.length == 0) {
        $("#ELanguage").text("Language Is Required");
        GoToTab("Language")
        $("#Language").focus();

        Test = false;
    } else { $("#ELanguage").text(""); }

    if (Currency.length == 0) {
        $("#ECurrency").text("Currency Is Required");
        GoToTab("Currency")
        $("#Currency").focus();

        Test = false;
    } else { $("#ECurrency").text(""); }

    if (CompanyMainActivity.length == 0) {
        $("#ECompanyMainActivity").text("Company Main Activity Is Required");
        GoToTab("CompanyMainActivity")

        $("#CompanyMainActivity").focus();
        Test = false;
    } else { $("#ECompanyMainActivity").text(""); }

    //Address Info Validate
    if (StreetName.length == 0) {
        $("#EStreetName").text("Street Name Is Required");
        GoToTab("StreetName")

        $("#StreetName").focus();
        Test = false;
    } else { $("#EStreetName").text(""); }

    if (BuldingNo.length == 0) {
        $("#EBuldingNo").text("Building Number Is Required");
        GoToTab("BuldingNo")

        $("#BuldingNo").focus();
        Test = false;
    } else { $("#EBuldingNo").text(""); }

    if (isNaN(FloorNo)) {
        $("#EFloorNo").text("Floor Number must be numeric");
        GoToTab("FloorNo")

        $("#FloorNo").focus();
        Test = false;
    } else { $("#EFloorNo").text(""); }

    if (Governorate.length == 0) {
        $("#EGovernorate").text("Governorate/State Is Required");
        GoToTab("Governorate")

        $("#Governorate").focus();
        Test = false;
    } else { $("#EGovernorate").text(""); }

    //Legal Info Validate
    if (CompanyType.length == 0) {
        $("#ECompanyType").text("Company Type Is Required");
        GoToTab("CompanyType")

        $("#CompanyType").focus();
        Test = false;
    } else { $("#ECompanyType").text(""); }
   
    if (CommericalRegister.length == 0) {
        $("#ECommericalRegister").text("Commerical Register Is Required");
        GoToTab("CommericalRegister")

        $("#CommericalRegister").focus();
        Test = false;
    } else { $("#ECommericalRegister").text(""); }
    
    if (TaxFileNo.length == 0) {
        $("#ETaxFileNo").text("Tax File Number Is Required");
        GoToTab("TaxFileNo")

        $("#TaxFileNo").focus();
        Test = false;
    } else { $("#ETaxFileNo").text(""); }

    var pattern = /^(http|https)?:\/\/[a-zA-Z0-9-\.]+\.[a-z]{2,4}/;
    if (!pattern.test(Website) && Website.length > 0) {
        $("#WebsiteError").text("invalid Website");
        GoToTab("WebsiteError")

        $("#WebsiteError").focus();
        Test = false;
    }
    else {
        $("#WebsiteError").text("");
    }
 /****************************************************************/

    var InputLogo = $("#InputLogo").get(0).files;
    var InputCommerical = $("#InputCommerical").get(0).files;
    var InputTax = $("#InputTax").get(0).files;
    var InputVat = $("#InputVat").get(0).files;
    var InputImport = $("#InputImport").get(0).files;
    var InputExport = $("#InputExport").get(0).files;
    var InputInsurance = $("#InputInsurance").get(0).files;

    var dataa = new FormData;
    dataa.append("InputLogo", InputLogo[0]);
    dataa.append("InputCommerical", InputCommerical[0]);
    dataa.append("InputTax", InputTax[0]);
    dataa.append("InputVat", InputVat[0]);
    dataa.append("InputImport", InputImport[0]);
    dataa.append("InputExport", InputExport[0]);
    dataa.append("InputInsurance", InputInsurance[0]);

    if (Test === true) {

        $.ajax({
            type: "POST",
            url: "/MainCompany/CompanyInformations?CompanyID=" + CompID + "&CompanyName=" + CompanyName + "&CompanyAlies=" + CompanyAlies + "&CountryName=" + CountryName
                + "&Language=" + Language + "&CompanyMainActivity=" + CompanyMainActivity + "&StreetName=" + StreetName + "&BuldingNo=" + BuldingNo + "&FloorNo=" + FloorNo
                + "&Governorate=" + Governorate + "&Area=" + Area + "&City=" + City + "&CompanyType=" + CompanyType + "&EstablishDate=" + EstablishDate + "&CommericalRegister=" + CommericalRegister
                + "&CommericalOffice=" + CommericalOffice + "&TaxFileNo=" + TaxFileNo + "&TaxOffice=" + TaxOffice + "&VatID=" + VatID + "&TaxVaOffice=" + TaxVaOffice + "&ImporterID=" + ImporterID
                + "&ImportOffice=" + ImportOffice + "&ExportID=" + ExportID + "&ExportOffice=" + ExportOffice + "&SocialInsuranceID=" + SocialInsuranceID + "&SocialInsuranceOffice=" + SocialInsuranceOffice
                + "&International1=" + International1 + "&Telephone1=" + Telephone1 + "&TelephoneEX1=" + TelephoneEX1 + "&International2=" + International2 + "&Telephone2=" + Telephone2 + "&TelephoneEX2=" + TelephoneEX2
                + "&International3=" + International3 + "&Telephone3=" + Telephone3 + "&TelephoneEX3=" + TelephoneEX3 + "&International4=" + International4 + "&Telephone4=" + Telephone4 + "&TelephoneEX4=" + TelephoneEX4
                + "&International5=" + International5 + "&Telephone5=" + Telephone5 + "&TelephoneEX5=" + TelephoneEX5 + "&Fax1=" + Fax1 + "&FaxEX1=" + FaxEX1 + "&Fax2=" + Fax2 + "&FaxEX2=" + FaxEX2
                + "&Fax3=" + Fax3 + "&FaxEX3=" + FaxEX3 + "&Fax4=" + Fax4 + "&FaxEX4=" + FaxEX4 + "&Fax5=" + Fax5 + "&FaxEX5=" + FaxEX5 + "&Website=" + Website + "&Code1=" + Code1 + "&Code2=" + Code2
                + "&Code3=" + Code3 + "&Code4=" + Code4 + "&Code5=" + Code5 + "&Status=" + Status + "&Currency=" + Currency,
            data: dataa,
            cache: false,
            contentType: false,
            processData: false,
            success: function (result) {
                if (result === "CompanyIDExist") {
                    $("#CompanyID").addClass("input-error");
                    $("#CompanyMain").show();
                    $("#CompanyAddress").hide();
                    $("#CompanyLegal").hide();
                    $("#CompanyCommunication").hide();
                    Talert("There Exist Company Take The Same ID");
                }
                else if (result === "Done") {
                    $("#SuccessSubmit").text("You Can Update Now If You Need...");
                    $("#PrintSpan").show("slow");
                    $("#CompanyID").prop("disabled", true);
                    $("#Currency").prop("disabled", true);
                    $(".fa-lock").show("slow");
                    $("#SaveBtn").prop("disabled", true);

                    $(".DownloadComm").show();
                    $(".DownloadTax").show();
                    $(".DownloadVat").show();
                    $(".DownloadImport").show();
                    $(".DownloadExport").show();
                    $(".DownloadInsurance").show();
                }

            }
        });
    }
}
function GoToTab(InputId) {
    var Id = $("#" + InputId).parents(".container").attr("id")
    var ThisId = '#' + Id + '';
    var asd = $("li[href='" + ThisId + "']");
    $("a[href='" + ThisId + "']").trigger("click");
    $("#" + InputId).focus();
}