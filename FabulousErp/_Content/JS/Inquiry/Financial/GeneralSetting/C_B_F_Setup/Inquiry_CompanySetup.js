$("#InquiryCompanyID").change(function () {

    var InquiryCompanyID = $(this).val();

    $.ajax({
        type: "GET",
        url: "/Inquiry_CompanySetup/CompanyDetails?InquiryCompanyID=" + InquiryCompanyID,
        success: function (result) {
            CompanyDetails(result);
            BranchDetails(result);
            FactoryDetails(result);
        }
    });
});

function FactoryDetails() {
    var selected = $('#InquiryCompanyID option:selected');
    var selected2 = $('#InquiryCompanyName option:selected');
    var SetData = $("#factory-append-data");
    SetData.html("");
    $.ajax({
        type: "GET",
        url: "/Inquiry_CompanySetup/FactoryDetails?Selected=" + selected.html(),
        contentType: "html",
        success: function (result) {
            if (result.length == 0) {
                SetData.append('<tr style="color:red"><td> </td></tr>')
            }
            else {
                for (var i = 0; i < result.length; i++) {
                    var Data = "<tr>" +
                        "<td>" + result[i].FactoryID + "</td>" +
                        "<td>" + result[i].FactoryName + "</td>" +
                        "<td>" + result[i].FactoryAlies + "</td>" +
                        "<td>" + result[i].StreetName + "<br/>" + result[i].BuldingNo + "<br/>" + result[i].FloorNo + "<br/>" + result[i].Area + "<br/>" + result[i].City + "<br/>" + result[i].Governorate + "</td>" +
                        "<td>" + result[i].SocialInsuranceID + "</td>" +
                        "<td>" + result[i].SocialInsuranceOffice + "</td>" +
                        "<td>" + result[i].International1 + "<br/>" + result[i].Telephone1 + "<br/>" + result[i].TelephoneEX1 + "</td>" +
                        "<td>" + result[i].International2 + "<br/>" + result[i].Telephone2 + "<br/>" + result[i].TelephoneEX2 + "</td>" +
                        "<td>" + result[i].Code1 + "<br/>" + result[i].Fax1 + "<br/>" + result[i].FaxEX1 + "</td>" +
                        "<td>" + result[i].Code2 + "<br/>" + result[i].Fax2 + "<br/>" + result[i].FaxEX2 + "</td>" +
                        "</tr>";
                    SetData.append(Data);
                }
            }
        },
        error: function (req, status, error) {
        }
    });
}

function CompanyDetails(result) {
    for (var i = 0; i < result.length; i++) {
        $("#company-id").val(result[i].CompanyID);
        $("#company-name").val(result[i].CompanyName);
        $("#company-alies").val(result[i].CompanyAlies);
        $("#company-countryname").val(result[i].CountryName);
        $("#company-language").val(result[i].Language);
        $("#company-currency").val(result[i].Currency);
        $("#company-activity").val(result[i].CompanyMainActivity);
        $("#company-type").val(result[i].CompanyType);
        $("#company-establishdate").val(result[i].EstablishDate);
        $("#company-commregister").val(result[i].CommericalRegister);
        $("#company-commoffice").val(result[i].CommericalOffice);
        $("#company-taxfile").val(result[i].TaxFileNo);
        $("#company-taxoffice").val(result[i].TaxOffice);
        $("#company-vatid").val(result[i].VatID);
        $("#company-taxvatoffice").val(result[i].TaxVaOffice);
        $("#company-importid").val(result[i].ImporterID);
        $("#company-importoffice").val(result[i].ImportOffice);
        $("#company-exportid").val(result[i].ExportID);
        $("#company-exportoffice").val(result[i].ExportOffice);
        $("#company-socialid").val(result[i].SocialInsuranceID);
        $("#company-socialoffice").val(result[i].SocialInsuranceOffice);
        $("#company-streetname").val(result[i].StreetName);
        $("#company-buildingno").val(result[i].BuldingNo);
        $("#company-floorno").val(result[i].FloorNo);
        $("#company-area").val(result[i].Area);
        $("#company-city").val(result[i].City);
        $("#company-state").val(result[i].Governorate);
        $("#company-telecode1").val(result[i].International1);
        $("#company-tele1").val(result[i].Telephone1);
        $("#company-teleext1").val(result[i].TelephoneEX1);
        $("#company-faxcode").val(result[i].Code1);
        $("#company-fax1").val(result[i].Fax1);
        $("#company-faxext1").val(result[i].FaxEX1);
    }
}

function BranchDetails() {
    var selected = $('#InquiryCompanyID option:selected');
    var selected2 = $('#InquiryCompanyName option:selected');
    var SetData = $("#branch-append-data");
    SetData.html("");
    $.ajax({
        type: "GET",
        url: "/Inquiry_CompanySetup/BranchDetails?Selected=" + selected.html(),
        contentType: "html",
        success: function (result) {
            if (result.length == 0) {
                SetData.append('<tr style="color:red"><td> </td></tr>')
            }
            else {
                for (var i = 0; i < result.length; i++) {
                    var Data = "<tr>" +
                        "<td>" + result[i].BranchID + "</td>" +
                        "<td>" + result[i].BranchName + "</td>" +
                        "<td>" + result[i].BranchAlies + "</td>" +
                        "<td>" + result[i].StreetName + "<br/>" + result[i].BuldingNo + "<br/>" + result[i].FloorNo + "<br/>" + result[i].Area + "<br/>" + result[i].City + "<br/>" + result[i].Governorate + "</td>" +
                        "<td>" + result[i].SocialInsuranceID + "</td>" +
                        "<td>" + result[i].SocialInsuranceOffice + "</td>" +
                        "<td>" + result[i].International1 + "<br/>" + result[i].Telephone1 + "<br/>" + result[i].TelephoneEX1 + "</td>" +
                        "<td>" + result[i].International2 + "<br/>" + result[i].Telephone2 + "<br/>" + result[i].TelephoneEX2 + "</td>" +
                        "<td>" + result[i].Code1 + "<br/>" + result[i].Fax1 + "<br/>" + result[i].FaxEX1 + "</td>" +
                        "<td>" + result[i].Code2 + "<br/>" + result[i].Fax2 + "<br/>" + result[i].FaxEX2 + "</td>" +
                        "</tr>";
                    SetData.append(Data);
                }
            }
        },
        error: function (req, status, error) {
        }
    });
}