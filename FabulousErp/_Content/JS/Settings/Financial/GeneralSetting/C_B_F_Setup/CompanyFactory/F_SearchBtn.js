function FSearchBtn() {
    var CompanyID = $("#CompanyID").val();
    var FactoryID = $("#FactoryID").val();

    if (FactoryID == "" || CompanyID == "") {
        $("#ErrorInSearch").text("You Must Choose Company ID & Fill Factory ID to Search");
    }
    else if (FactoryID.length < 9) {
        $("#ErrorInSearch").text("Factory ID required 9 characters");
    }
    else if (FactoryID.length > 9) {
        $("#ErrorInSearch").text("Factory ID required 9 characters");
    }
    else {
        $("#ErrorInSearch").text("");
        $.ajax({
            type: "GET",
            url: "/CompanyFactory/GetFactoryInfo?FactoryID=" + FactoryID + "&CompanyID=" + CompanyID,
            success: function (data) {

                if (data == "False") {
                    $("#ErrorInSearch").text("Factory ID Not matching With Company ID");
                }
                else {
                    //new validation
                    var empty = "";
                    $("#FIDError").text(empty);
                    $("#FNError").text(empty);
                    $("#BIDError").text(empty);
                    $("#CIDError").text(empty);
                    $("#StreetError").text(empty);
                    $("#BuildingError").text(empty);
                    $("#GovError").text(empty);

                    //remove border error
                    $("#FactoryID").removeClass('input-error');
                    $("#FactoryName").removeClass('input-error');
                    $("#CompanyID").removeClass('input-error');
                    $("#StreetName").removeClass('input-error');
                    $("#BuldingNo").removeClass('input-error');
                    $("#Governorate").removeClass('input-error');


                    $("#UpdateWithSearch").show("fast");
                    $("#UpdateWithoutSearch2").hide();
                    $("#UpdateWithSearch").text("You Can Update Now..");
                    $("#FactoryID").prop("disabled", true);
                    $("#BranchID").prop("disabled", true);
                    $("#CompanyID").prop("disabled", true);
                    $("#FactoryName").prop("disabled", false);
                    $("#EstablishDate").prop("disabled", false);
                    $("#FactoryAlies").prop("disabled", false);
                    //$("#EstablishDate").prop("disabled", true);
                    $("#SaveBtn").prop("disabled", true);
                    $("#BarValid").text("True");

                    $(".glyphicon-lock").show("slow");

                    if (data.Status === "True") {
                        $("#Status").prop("Checked", true);
                    } else {
                        $("#Status").prop("Checked", false);
                    }

                    $("#FactoryName").val(data.FactoryName);
                    $("#FactoryAlies").val(data.FactoryAlies);

                    if (data.BranchID === null) {
                        $("#BranchID").val("-1");
                        $("#BranchName").text("");
                    } else {
                        $("#BranchID").val(data.BranchID);
                        //to show Branch name in search
                        var BranchID = $("#BranchID").val();
                        if (BranchID.length > 0) {
                            $.ajax({
                                type: "GET",
                                url: "/CompanyFactory/GetBranchName?BranchID=" + BranchID,
                                success: function (data) {
                                    $("#BranchName").show("fast");
                                    $("#BranchName").text(data);
                                }
                            });
                        }
                    }
                    
                    $("#CompanyID").val(data.CompanyID);
                    $("#EstablishDate").val(data.EstablishDate);
                    $("#StreetName").val(data.StreetName);
                    $("#BuldingNo").val(data.BuldingNo);
                    $("#FloorNo").val(data.FloorNo);
                    $("#Area").val(data.Area);
                    $("#City").val(data.City);
                    $("#Governorate").val(data.Governorate);
                    $("#InsuranceID").val(data.InsuranceID);
                    $("#InsuranceOffice").val(data.InsuranceOffice);
                    $("#International1").val(data.International1);
                    $("#Telephone1").val(data.Telephone1);
                    $("#TelephoneEX1").val(data.TelephoneEX1);


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

                    //to show Branch name in search
                    /*
                    var BranchID = $("#BranchID").val();
                    if (BranchID.length > 0) {
                        $.ajax({
                            type: "GET",
                            url: "/CompanyFactory/GetBranchName?BranchID=" + BranchID,
                            success: function (data) {
                                $("#BranchName").show("fast");
                                $("#BranchName").text(data);
                            }
                        });
                    }
                    */
                }
                /*
                error: function (req, status, error) {
                    $("#ErrorInSearch").text("Factory ID Not Exist");
                }
                */
            }
        });
    }
}