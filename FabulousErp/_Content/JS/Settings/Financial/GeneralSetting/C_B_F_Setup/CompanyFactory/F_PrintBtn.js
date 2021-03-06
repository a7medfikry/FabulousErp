function FPrint() {
    //C Main Info 
    var FactoryID = $("#FactoryID").val();
    var FactoryName = $("#FactoryName").val();
    var CompanyID = $("#CompanyID").val();
    var BranchID = $("#BranchID").val();
    var EstablishDate = $("#EstablishDate").val();

    //C Address Info
    var StreetName = $("#StreetName").val();
    var BuldingNo = $("#BuldingNo").val();
    var Governorate = $("#Governorate").val();
    var FloorNo = $("#FloorNo").val();
    var City = $("#City").val();

    //C Legal INfo
    var InsuranceID = $("#InsuranceID").val();

    //C Communication Info
    var Telephone1 = $("#Telephone1").val();
    var TelephoneEX1 = $("#TelephoneEX1").val();
    var Telephone2 = $("#Telephone2").val();
    var TelephoneEX2 = $("#TelephoneEX2").val();
    var Telephone3 = $("#Telephone3").val();
    var TelephoneEX3 = $("#TelephoneEX3").val();
    var Telephone4 = $("#Telephone4").val();
    var TelephoneEX4 = $("#TelephoneEX4").val();
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

    //Check If Mandatory Info Have Data
    if (FactoryID.length > 0 && FactoryName.length > 0 && CompanyID.length > 0 && BranchID.length > 0 && StreetName.length > 0 && BuldingNo.length > 0 && Governorate.length > 0) {
        $("#PFactoryID").show();
        $("#PFacID").text(FactoryID);

        $("#PFactoryName").show();
        $("#PFacName").text(FactoryName);

        $("#PCompanyID").show();
        $("#PCompID").text(CompanyID);

        $("#PBranchID").show();
        $("#PBraID").text(BranchID);


        $("#PStreetName").show();
        $("#PStrName").text(StreetName);

        $("#PBuildingNumber").show();
        $("#PBuildNum").text(BuldingNo);

        $("#PGovernorate").show();
        $("#PGovern").text(Governorate);

        if (EstablishDate.length > 0) {
            $("#PEstablishDate").show();
            $("#PDate").text(EstablishDate);
        }
        if (FloorNo.length > 0) {
            $("#PFloorNumber").show();
            $("#PFloorNum").text(FloorNo);
        }
        if (City.length > 0) {
            $("#PCity").show();
            $("#PCi").text(City);
        }
        if (InsuranceID.length > 0) {
            $("#PSocialInsuranceID").show();
            $("#PInsurance").text(City);
        }
        if (Telephone1.length > 0) {
            $("#PTelephone1").show();
            $("#PT1").text(Telephone1);
            $("#PTX1").text(TelephoneEX1);
        }
        if (Telephone2.length > 0) {
            $("#PTelephone2").show();
            $("#PT2").text(Telephone2);
            $("#PTX2").text(TelephoneEX2);
        }
        if (Telephone3.length > 0) {
            $("#PTelephone3").show();
            $("#PT3").text(Telephone3);
            $("#PTX3").text(TelephoneEX3);
        }
        if (Telephone4.length > 0) {
            $("#PTelephone4").show();
            $("#PT4").text(Telephone4);
            $("#PTX4").text(TelephoneEX4);
        }
        if (Telephone5.length > 0) {
            $("#PTelephone5").show();
            $("#PT5").text(Telephone5);
            $("#PTX5").text(TelephoneEX5);
        }
        if (Fax1.length > 0) {
            $("#PFax1").show();
            $("#PF1").text(Fax1);
            $("#PFX1").text(FaxEX1);
        }
        if (Fax2.length > 0) {
            $("#PFax2").show();
            $("#PF2").text(Fax2);
            $("#PFX2").text(FaxEX2);
        }
        if (Fax3.length > 0) {
            $("#PFax3").show();
            $("#PF3").text(Fax3);
            $("#PFX3").text(FaxEX3);
        }
        if (Fax4.length > 0) {
            $("#PFax4").show();
            $("#PF4").text(Fax4);
            $("#PFX4").text(FaxEX4);
        }
        if (Fax5.length > 0) {
            $("#PFax5").show();
            $("#PF5").text(Fax5);
            $("#PFX5").text(FaxEX5);
        }
        PrintDocument();
    } else {

        Talert("Can't Print Without Fill Mandatory Data..!");
    }


}
function PrintDocument() {
  
    window.print();
    window.location.href = '/CompanyFactory/FactoryInformation/';
}
