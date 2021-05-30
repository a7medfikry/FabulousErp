function BUpdateBtn()
{
    var Check = $("#UpdateWithSearch").text();
    var Check2 = $("#SuccessSubmit").text();

    //C Main Info 
    var Branchname = $("#Branchname").val();

    var Status = "False";
    if ($("#Status").is(":checked")) {
        Status = "True";
    }

    //C Address Info
    var StreetName = $("#StreetName").val();
    var BuldingNo = $("#BuldingNo").val();
    var FloorNo = $("#FloorNo").val();
    var Governorate = $("#Governorate").val();

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

    if (Check.length > 0 || Check2.length > 0) {
        //check first validation of all data by js

        //Main Info Validate
        if (Branchname.length == 0) {
            $("#EBranchname").text("Branch Name Is Required");
            $("#Branchname").focus();
        } else { $("#EBranchname").text(""); }


        //Address Info Validate
        if (StreetName.length == 0) {
            $("#EStreetName").text("Street Name Is Required");
            $("#StreetName").focus();
        } else { $("#EStreetName").text(""); }

        if (BuldingNo.length == 0) {
            $("#EBuldingNo").text("Building Number Is Required");
            $("#BuldingNo").focus();
        } else { $("#EBuldingNo").text(""); }

        if (isNaN(FloorNo)) {
            $("#EFloorNo").text("Floor Number must be numeric");
            $("#FloorNo").focus();
        } else { $("#EFloorNo").text(""); }

        if (Governorate.length == 0) {
            $("#EGovernorate").text("Governorate/State Is Required");
            $("#Governorate").focus();
        } else { $("#EGovernorate").text(""); }


        //Communication Info Validate
        if (isNaN(International1)) {
            $("#EInternational1").text("Numbers Only Is Required");
            $("#International1").focus();
        } else { $("#EInternational1").text(""); }
        if (isNaN(Telephone1)) {
            $("#ETelephone1").text("Numbers Only Is Required");
            $("#Telephone1").focus();
        } else { $("#ETelephone1").text(""); }
        if (isNaN(TelephoneEX1)) {
            $("#ETelephoneEX1").text("Numbers Only Is Required");
            $("#TelephoneEX1").focus();
        } else { $("#ETelephoneEX1").text(""); }

        if (isNaN(International2)) {
            $("#EInternational2").text("Numbers Only Is Required");
            $("#International2").focus();
        } else { $("#EInternational2").text(""); }
        if (isNaN(Telephone2)) {
            $("#ETelephone2").text("Numbers Only Is Required");
            $("#Telephone2").focus();
        } else { $("#ETelephone2").text(""); }
        if (isNaN(TelephoneEX2)) {
            $("#ETelephoneEX2").text("Numbers Only Is Required");
            $("#TelephoneEX2").focus();
        } else { $("#ETelephoneEX2").text(""); }

        if (isNaN(International3)) {
            $("#EInternational3").text("Numbers Only Is Required");
            $("#International3").focus();
        } else { $("#EInternational3").text(""); }
        if (isNaN(Telephone3)) {
            $("#ETelephone3").text("Numbers Only Is Required");
            $("#Telephone3").focus();
        } else { $("#ETelephone3").text(""); }
        if (isNaN(TelephoneEX3)) {
            $("#ETelephoneEX3").text("Numbers Only Is Required");
            $("#TelephoneEX3").focus();
        } else { $("#ETelephoneEX3").text(""); }

        if (isNaN(International4)) {
            $("#EInternational4").text("Numbers Only Is Required");
            $("#International4").focus();
        } else { $("#EInternational4").text(""); }
        if (isNaN(Telephone4)) {
            $("#ETelephone4").text("Numbers Only Is Required");
            $("#Telephone4").focus();
        } else { $("#ETelephone4").text(""); }
        if (isNaN(TelephoneEX4)) {
            $("#ETelephoneEX4").text("Numbers Only Is Required");
            $("#TelephoneEX4").focus();
        } else { $("#ETelephoneEX4").text(""); }

        if (isNaN(International5)) {
            $("#EInternational5").text("Numbers Only Is Required");
            $("#International5").focus();
        } else { $("#EInternational5").text(""); }
        if (isNaN(Telephone5)) {
            $("#ETelephone5").text("Numbers Only Is Required");
            $("#Telephone5").focus();
        } else { $("#ETelephone5").text(""); }
        if (isNaN(TelephoneEX5)) {
            $("#ETelephoneEX5").text("Numbers Only Is Required");
            $("#TelephoneEX5").focus();
        } else { $("#ETelephoneEX5").text(""); }


        if (isNaN(Fax1)) {
            $("#EFax1").text("Numbers Only Is Required");
            $("#Fax1").focus();
        } else { $("#EFax1").text(""); }
        if (isNaN(FaxEX1)) {
            $("#EFaxEX1").text("Numbers Only Is Required");
            $("#FaxEX1").focus();
        } else { $("#EFaxEX1").text(""); }

        if (isNaN(Fax2)) {
            $("#EFax2").text("Numbers Only Is Required");
            $("#Fax2").focus();
        } else { $("#EFax2").text(""); }
        if (isNaN(FaxEX2)) {
            $("#EFaxEX2").text("Numbers Only Is Required");
            $("#FaxEX2").focus();
        } else { $("#EFaxEX2").text(""); }

        if (isNaN(Fax3)) {
            $("#EFax3").text("Numbers Only Is Required");
            $("#Fax3").focus();
        } else { $("#EFax3").text(""); }
        if (isNaN(FaxEX3)) {
            $("#EFaxEX3").text("Numbers Only Is Required");
            $("#FaxEX3").focus();
        } else { $("#EFaxEX3").text(""); }

        if (isNaN(Fax4)) {
            $("#EFax4").text("Numbers Only Is Required");
            $("#Fax4").focus();
        } else { $("#EFax4").text(""); }
        if (isNaN(FaxEX4)) {
            $("#EFaxEX4").text("Numbers Only Is Required");
            $("#FaxEX4").focus();
        } else { $("#EFaxEX4").text(""); }

        if (isNaN(Fax5)) {
            $("#EFax5").text("Numbers Only Is Required");
            $("#Fax5").focus();
        } else { $("#EFax5").text(""); }
        if (isNaN(FaxEX5)) {
            $("#EFaxEX5").text("Numbers Only Is Required");
            $("#FaxEX5").focus();
        } else { $("#EFaxEX5").text(""); }


        //Check All Error text to make update correctly without errors
        var EBranchname = $("#EBranchname").text();

        var EStreetName = $("#EStreetName").text();
        var EBuldingNo = $("#EBuldingNo").text();
        var EFloorNo = $("#EFloorNo").text();
        var EGovernorate = $("#EGovernorate").text();

        var EInternational1 = $("#EInternational1").text();
        var ETelephone1 = $("#ETelephone1").text();
        var ETelephoneEX1 = $("#ETelephoneEX1").text();
        var EInternational2 = $("#EInternational2").text();
        var ETelephone2 = $("#ETelephone2").text();
        var ETelephoneEX2 = $("#ETelephoneEX2").text();
        var EInternational3 = $("#EInternational3").text();
        var ETelephone3 = $("#ETelephone3").text();
        var ETelephoneEX3 = $("#ETelephoneEX3").text();
        var EInternational4 = $("#EInternational4").text();
        var ETelephone4 = $("#ETelephone4").text();
        var ETelephoneEX4 = $("#ETelephoneEX4").text();
        var EInternational5 = $("#EInternational5").text();
        var ETelephone5 = $("#ETelephone5").text();
        var ETelephoneEX5 = $("#ETelephoneEX5").text();

        var EFax1 = $("#EFax1").text();
        var EFaxEX1 = $("#EFaxEX1").text();
        var EFax2 = $("#EFax2").text();
        var EFaxEX2 = $("#EFaxEX2").text();
        var EFax3 = $("#EFax3").text();
        var EFaxEX3 = $("#EFaxEX3").text();
        var EFax4 = $("#EFax4").text();
        var EFaxEX4 = $("#EFaxEX4").text();
        var EFax5 = $("#EFax5").text();
        var EFaxEX5 = $("#EFaxEX5").text();

        if (EBranchname.length == 0 && EStreetName.length == 0 && EBuldingNo.length == 0 && EFloorNo.length == 0 && EGovernorate.length == 0 &&
            EInternational1.length == 0 && ETelephone1.length == 0 && ETelephoneEX1.length == 0 &&
            EInternational2.length == 0 && ETelephone2.length == 0 && ETelephoneEX2.length == 0 &&
            EInternational3.length == 0 && ETelephone3.length == 0 && ETelephoneEX3.length == 0 &&
            EInternational4.length == 0 && ETelephone4.length == 0 && ETelephoneEX4.length == 0 &&
            EInternational5.length == 0 && ETelephone5.length == 0 && ETelephoneEX5.length == 0 &&
            EFax1.length == 0 && EFaxEX1.length == 0 &&
            EFax2.length == 0 && EFaxEX2.length == 0 &&
            EFax3.length == 0 && EFaxEX3.length == 0 &&
            EFax4.length == 0 && EFaxEX4.length == 0 &&
            EFax5.length == 0 && EFaxEX5.length == 0) {

            var BranchID = $("#BranchID").val();
            var BranchAlies = $("#BranchAlies").val();
            var EstablishDate = $("#EstablishDate").val();

            //define her cause not define above in validate js
            //Address
            var Area = $("#Area").val();
            var City = $("#City").val();
            //legal
            var InsuranceID = $("#InsuranceID").val();
            var InsuranceOffice = $("#InsuranceOffice").val();

            //Check If user change of data OR NO
            
            $.ajax({
                type: "GET",
                url: "/CompanyBranch/CheckChanges?BranchID=" + BranchID,
                success: function (data) {

                    var NBranchAlies;
                    var NEstablishDate;
                    var NFloorNo;
                    var NArea;
                    var NCity;
                    var NInsuranceID;
                    var NInsuranceOffice;
                    var NInternational1;
                    var NTelephone1;
                    var NTelephoneEX1;
                    var NInternational2;
                    var NTelephone2;
                    var NTelephoneEX2;
                    var NInternational3;
                    var NTelephone3;
                    var NTelephoneEX3;
                    var NInternational4;
                    var NTelephone4;
                    var NTelephoneEX4;
                    var NInternational5;
                    var NTelephone5;
                    var NTelephoneEX5;
                    var NCode1;
                    var NFax1;
                    var NFaxEX1;
                    var NCode2;
                    var NFax2;
                    var NFaxEX2;
                    var NCode3;
                    var NFax3;
                    var NFaxEX3;
                    var NCode4;
                    var NFax4;
                    var NFaxEX4;
                    var NCode5;
                    var NFax5;
                    var NFaxEX5;

                    if (data.BranchAlies == null) {
                        NBranchAlies = "";
                    } else { NBranchAlies = data.BranchAlies; }

                    if (data.EstablishDate == null) {
                        NEstablishDate = "";
                    } else { NEstablishDate = data.EstablishDate; }

                    if (data.FloorNo == null) {
                        NFloorNo = "";
                    } else { NFloorNo = data.FloorNo; }

                    if (data.Area == null) {
                        NArea = "";
                    } else { NArea = data.Area; }

                    if (data.City == null) {
                        NCity = "";
                    } else { NCity = data.City; }

                    if (data.InsuranceID == null) {
                        NInsuranceID = "";
                    } else { NInsuranceID = data.InsuranceID; }

                    if (data.InsuranceOffice == null) {
                        NInsuranceOffice = "";
                    } else { NInsuranceOffice = data.InsuranceOffice; }

                    if (data.International1 == null) {
                        NInternational1 = "";
                    } else { NInternational1 = data.International1; }

                    if (data.Telephone1 == null) {
                        NTelephone1 = "";
                    } else { NTelephone1 = data.Telephone1; }

                    if (data.TelephoneEX1 == null) {
                        NTelephoneEX1 = "";
                    } else { NTelephoneEX1 = data.TelephoneEX1; }

                    if (data.International2 == null) {
                        NInternational2 = "";
                    } else { NInternational2 = data.International2; }

                    if (data.Telephone2 == null) {
                        NTelephone2 = "";
                    } else { NTelephone2 = data.Telephone2; }

                    if (data.TelephoneEX2 == null) {
                        NTelephoneEX2 = "";
                    } else { NTelephoneEX2 = data.TelephoneEX2; }

                    if (data.International3 == null) {
                        NInternational3 = "";
                    } else { NInternational3 = data.International3; }

                    if (data.Telephone3 == null) {
                        NTelephone3 = "";
                    } else { NTelephone3 = data.Telephone3; }

                    if (data.TelephoneEX3 == null) {
                        NTelephoneEX3 = "";
                    } else { NTelephoneEX3 = data.TelephoneEX3; }

                    if (data.International4 == null) {
                        NInternational4 = "";
                    } else { NInternational4 = data.International4; }

                    if (data.Telephone4 == null) {
                        NTelephone4 = "";
                    } else { NTelephone4 = data.Telephone4; }

                    if (data.TelephoneEX4 == null) {
                        NTelephoneEX4 = "";
                    } else { NTelephoneEX4 = data.TelephoneEX4; }

                    if (data.International5 == null) {
                        NInternational5 = "";
                    } else { NInternational5 = data.International5; }

                    if (data.Telephone5 == null) {
                        NTelephone5 = "";
                    } else { NTelephone5 = data.Telephone5; }

                    if (data.TelephoneEX5 == null) {
                        NTelephoneEX5 = "";
                    } else { NTelephoneEX5 = data.TelephoneEX5; }

                    if (data.Code1 == null) {
                        NCode1 = "";
                    } else { NCode1 = data.Code1; }

                    if (data.Fax1 == null) {
                        NFax1 = "";
                    } else { NFax1 = data.Fax1; }

                    if (data.FaxEX1 == null) {
                        NFaxEX1 = "";
                    } else { NFaxEX1 = data.FaxEX1; }

                    if (data.Code2 == null) {
                        NCode2 = "";
                    } else { NCode2 = data.Code2; }

                    if (data.Fax2 == null) {
                        NFax2 = "";
                    } else { NFax2 = data.Fax2; }

                    if (data.FaxEX2 == null) {
                        NFaxEX2 = "";
                    } else { NFaxEX2 = data.FaxEX2; }

                    if (data.Code3 == null) {
                        NCode3 = "";
                    } else { NCode3 = data.Code3; }

                    if (data.Fax3 == null) {
                        NFax3 = "";
                    } else { NFax3 = data.Fax3; }

                    if (data.FaxEX3 == null) {
                        NFaxEX3 = "";
                    } else { NFaxEX3 = data.FaxEX3; }

                    if (data.Code4 == null) {
                        NCode4 = "";
                    } else { NCode4 = data.Code4; }

                    if (data.Fax4 == null) {
                        NFax4 = "";
                    } else { NFax4 = data.Fax4; }

                    if (data.FaxEX4 == null) {
                        NFaxEX4 = "";
                    } else { NFaxEX4 = data.FaxEX4; }

                    if (data.Code5 == null) {
                        NCode5 = "";
                    } else { NCode5 = data.Code5; }

                    if (data.Fax5 == null) {
                        NFax5 = "";
                    } else { NFax5 = data.Fax5; }

                    if (data.FaxEX5 == null) {
                        NFaxEX5 = "";
                    } else { NFaxEX5 = data.FaxEX5; }

                    if (Branchname == data.Branchname && BranchAlies == NBranchAlies && EstablishDate == NEstablishDate && StreetName == data.StreetName &&
                        BuldingNo == data.BuldingNo && FloorNo == NFloorNo && Area == NArea && City == NCity && Governorate == data.Governorate &&
                        InsuranceID == NInsuranceID && InsuranceOffice == NInsuranceOffice && International1 == NInternational1 && Telephone1 == NTelephone1 &&
                        TelephoneEX1 == NTelephoneEX1 && International2 == NInternational2 && Telephone2 == NTelephone2 &&
                        TelephoneEX2 == NTelephoneEX2 && International3 == NInternational3 && Telephone3 == NTelephone3 &&
                        TelephoneEX3 == NTelephoneEX3 && International4 == NInternational4 && Telephone4 == NTelephone4 &&
                        TelephoneEX4 == NTelephoneEX4 && International5 == NInternational5 && Telephone5 == NTelephone5 &&
                        TelephoneEX5 == NTelephoneEX5 && Code1 == NCode1 && Fax1 == NFax1 && FaxEX1 == NFaxEX1
                        && Code2 == NCode2 && Fax2 == NFax2 && FaxEX2 == NFaxEX2 && Code3 == NCode3 && Fax3 == NFax3 && FaxEX3 == NFaxEX3
                        && Code4 == NCode4 && Fax4 == NFax4 && FaxEX4 == NFaxEX4 && Code5 == NCode5 && Fax5 == NFax5 && FaxEX5 == NFaxEX5 && Status == data.Status) {

                        $("#UpdateWithSearch").show("fast");
                        $("#UpdateWithSearch").text("No Changes To Update");

                    } else
                    {
                        //Update Data Before Check
                        $.ajax({
                            type: "POST",
                            url: "/CompanyBranch/UpdateBranchInfo?BranchID=" + BranchID + "&Branchname=" + Branchname + "&BranchAlies=" + BranchAlies + "&StreetName=" + StreetName + "&BuldingNo=" + BuldingNo + "&FloorNo=" + FloorNo
                                + "&Governorate=" + Governorate + "&Area=" + Area + "&City=" + City + "&InsuranceID=" + InsuranceID + "&InsuranceOffice=" + InsuranceOffice
                                + "&International1=" + International1 + "&Telephone1=" + Telephone1 + "&TelephoneEX1=" + TelephoneEX1 + "&International2=" + International2 + "&Telephone2=" + Telephone2 + "&TelephoneEX2=" + TelephoneEX2
                                + "&International3=" + International3 + "&Telephone3=" + Telephone3 + "&TelephoneEX3=" + TelephoneEX3 + "&International4=" + International4 + "&Telephone4=" + Telephone4 + "&TelephoneEX4=" + TelephoneEX4
                                + "&International5=" + International5 + "&Telephone5=" + Telephone5 + "&TelephoneEX5=" + TelephoneEX5 + "&Fax1=" + Fax1 + "&FaxEX1=" + FaxEX1 + "&Fax2=" + Fax2 + "&FaxEX2=" + FaxEX2
                                + "&Fax3=" + Fax3 + "&FaxEX3=" + FaxEX3 + "&Fax4=" + Fax4 + "&FaxEX4=" + FaxEX4 + "&Fax5=" + Fax5 + "&FaxEX5=" + FaxEX5 + "&EstablishDate=" + EstablishDate + "&Code1=" + Code1 + "&Code2=" + Code2
                                + "&Code3=" + Code3 + "&Code4=" + Code4 + "&Code5=" + Code5 + "&Status=" + Status,
                            success: function (data) {
                                console.log("done");
                                $("#PrintSpan").show("slow");
                                $("#UpdateWithSearch").show("fast");
                                $("#UpdateWithSearch").text("Data Updated Successfully..");
                                $("#UpdateWithSearch").fadeOut(4000);
                                $("#SuccessSubmit").text("");
                            },
                            error: function (req, status, error) {
                                console.log(error);
                            }
                        });
                    }
           
                }
            });                       
        }

    } else {
        $("#UpdateWithSearch").hide();
        $("#UpdateWithoutSearch2").show("fast");
        $("#UpdateWithoutSearch2").text("Pleas Select Company ID Then Fill Branch ID To Search");
        $("#CompanyID").addClass("input-error");
    }
}