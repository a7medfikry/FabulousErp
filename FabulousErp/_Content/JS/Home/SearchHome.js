$("#searchinput").autocomplete({
    source: function (request, response) {
        $.ajax({

            type: "GET",
            url: "/Home/GetSearchValue?SearchValue=" + $("#searchinput").val().toUpperCase(),
            success: function (data) {
                response($.map(data, function (item) {
                    return { label: item.FormName, value: item.FormCode };
                }));
            },
            error: function (xhr, status, error) {

            }
        });
    }
});

$("#SearchFormsBtn").click(function () {
    var searchinput = $("#searchinput").val().toUpperCase();

    if (searchinput === "SCL") {
        window.location.href = "/MainCompany/CompanyInformation";
    }
    else if (searchinput === "SCBI") {
        window.location.href = "/CompanyBranch/BranchInformation";
    }
    else if (searchinput === "SCPI") {
        window.location.href = "/CompanyFactory/FactoryInformation";
    }
    else if (searchinput === "SCNA") {
        window.location.href = "/CreateNewAccount/NewAccountInfo";
    }
    else if (searchinput === "SLOU") {
        window.location.href = "/ListOfUser/GetUsers";
    }
    else if (searchinput === "SCNG") {
        window.location.href = "/CreateNewGroup/NewGroupInfo";
    }
    else if (searchinput === "SLOG") {
        window.location.href = "/ListOfGroup/GetGroups";
    }
    else if (searchinput === "SAGI") {
        window.location.href = "/AccountGroupInfo/AddUserToGroup";
    }
    else if (searchinput === "SUACP") {
        window.location.href = "/CompanyPremissionAccess/UACompPremission";
    }
    else if (searchinput === "SUABP") {
        window.location.href = "/BranchPremissionAccess/UABranchPremission";
    }
    else if (searchinput === "SUAFP") {
        window.location.href = "/FactoryPremissionAccess/UAFactoryPremission";
    }
    else if (searchinput === "SUAP") {
        window.location.href = "/UGFormsAccess/FormsAccess";
    }
    else if (searchinput === "SFYD") {
        window.location.href = "/FiscalDef/FiscalYearDef";
    }
    else if (searchinput === "SCFY") {
        window.location.href = "/CompanyFiscalYear/AddCompanyFiscalYear";
    }
    else if (searchinput === "SCNY") {
        window.location.href = "/CreateNewYear/NewYear";
    }
    else if (searchinput === "SCF") {
        window.location.href = "/CurrencyFormateSetting/FormateSetting";
    }
    else if (searchinput === "SCD") {
        window.location.href = "/CurrenciesDefinition/CurrenciesTypes";
    }
    else if (searchinput === "SCET") {
        window.location.href = "/ExchangeCurrenciesSetup/ExchangeCurrencies";
    }
    else if (searchinput === "ILOU") {
        window.location.href = "/Inquiry_UserData/ShowListofUsers";
    }
    else if (searchinput === "SCCOA") {
        window.location.href = "/CreateAccountChart/AccountChart";
    }
    else if (searchinput === "ICI") {
        window.location.href = "/Inquiry_CompanySetup/CompanyInformation";
    }
    else if (searchinput === "IUP") {
        window.location.href = "/Inquiry_UserProfile/Accountprofile";
    }
    else if (searchinput === "ISFP") {
        window.location.href = "/AllFiscalPeriods/ShowFiscalPeriods";
    }
    else if (searchinput === "SACTC") {
        window.location.href = "/CompanyChartAccount/AddChartToCompany";
    }
    else if (searchinput === "ILCOA") {
        window.location.href = "/Inquiry_AccountChart/ListAccountChart";
    }
    else if (searchinput === "SCAA") {
        window.location.href = "/C_CreateAnalyticAccounts/CompanyAnalyticAccounts";
    }
    else if (searchinput === "SBAA") {
        window.location.href = "/B_CreateAnalyticAccounts/BranchAnalyticAccounts";
    }
    else if (searchinput === "SFAA") {
        window.location.href = "/F_CreateAnalyticAccounts/FactoryAnalyticAccounts";
    }
    else if (searchinput === "SCCC") {
        window.location.href = "/C_CreateCostCenter/CompanyCostCenter";
    }
    else if (searchinput === "SBCC") {
        window.location.href = "/B_CreateCostCenter/BranchCostCenter";
    }
    else if (searchinput === "SFCC") {
        window.location.href = "/F_CreateCostCenter/FactoryCostCenter";
    }
    else if (searchinput === "SCMCC") {
        window.location.href = "/C_CreateMainCostCenter/CompMainCostCenter";
    }
    else if (searchinput === "SUMCC") {
        window.location.href = "/C_UpdateMainCostCenter/CompUpdateGroupCC";
    }
    else if (searchinput === "SCCCA") {
        window.location.href = "/C_CreateCostCenterAccounts/CompCCAccounts";
    }
    else if (searchinput === "SCAG") {
        window.location.href = "/CreateAccountGroup/AccountGroup";
    }
    else if (searchinput === "SCCA") {
        window.location.href = "/C_CreateAccount/CompanyAccount";
    }
    else if (searchinput === "SCY") {
        window.location.href = "/CloseFiscalYear/CloseYear";
    }
    else if (searchinput === "SCAAD") {
        window.location.href = "/C_AnalyticAccountDistribution/CompanyAnalyticDistribution";
    }
    else if (searchinput === "SBAAD") {
        window.location.href = "/B_AnalyticAccountDistribution/BranchAnalyticDistribution";
    }
    else if (searchinput === "SFAAD") {
        window.location.href = "/F_AnalyticAccountDistribution/FactoryAnalyticDistribution";
    }
    else if (searchinput === "SBMCC") {
        window.location.href = "/B_CreateMainCostCenter/BranchMainCostCenter";
    }
    else if (searchinput === "SFMCC") {
        window.location.href = "/F_CreateMainCostCenter/FactoryMainCostCenter";
    }
    else if (searchinput === "SUBMCC") {
        window.location.href = "/B_UpdateMainCostCenter/BranchUpdateGroupCC";
    }
    else if (searchinput === "SUFMCC") {
        window.location.href = "/F_UpdateMainCostCenter/FactoryUpdateGroupCC";
    }
    else if (searchinput === "SBCCA") {
        window.location.href = "/B_CreateCostCenterAccounts/BranchCCAccounts";
    }
    else if (searchinput === "SFCCA") {
        window.location.href = "/F_CreateCostCenterAccounts/FactoryCCAccounts";
    }
    else if (searchinput === "SBUAP") {
        window.location.href = "/UGFormsAccess/BranchFormsAccess";
    }
    else if (searchinput === "SFUAP") {
        window.location.href = "/UGFormsAccess/FactoryFormsAccess";
    }
    else if (searchinput === "IUFA") {
        window.location.href = "/Inquiry_FormsAccess/FormsAccess";
    }
    else if (searchinput === "IBA") {
        window.location.href = "/Inquiry_BranchAccess/BranchAccess";
    }
    else if (searchinput === "ICA") {
        window.location.href = "/Inquiry_CompanyAccess/CompanyAccess";
    }
    else if (searchinput === "IFA") {
        window.location.href = "/Inquiry_FactoryAccess/FactoryAccess";
    }
    else if (searchinput === "IGA") {
        window.location.href = "/Inquiry_GroupAccess/GroupAccess";
    }
    else if (searchinput === "SDATCA") {
        window.location.href = "/C_AddDistToAccount/DistToCompAccount";
    }
    else if (searchinput === "SCATCA") {
        window.location.href = "/C_AddCostAccountToAccount/CostAccountToCompAccount";
    }
    else if (searchinput === "IBAA") {
        window.location.href = "/Inquiry_BranchAnalytic/BranchAnalyticAccount";
    }
    else if (searchinput === "IBCC") {
        window.location.href = "/Inquiry_BranchCostCenter/BranchCostCenter";
    }
    else if (searchinput === "ICAA") {
        window.location.href = "/Inquiry_CompanyAnalytic/CompanyAnalyticAccount";
    }
    else if (searchinput === "ICCC") {
        window.location.href = "/Inquiry_CompanyCostCenter/CompanyCostCenter";
    }
    else if (searchinput === "SBCA") {
        window.location.href = "/B_CreateAccount/BranchAccount";
    }
    else if (searchinput === "SDATBA") {
        window.location.href = "/B_AddDistToAccount/DistToBranchAccount";
    }
    else if (searchinput === "SCATBA") {
        window.location.href = "/B_AddCostAccountToAccount/CostAccountToBranchAccount";
    }
    else if (searchinput === "SFCA") {
        window.location.href = "/F_CreateAccount/FactoryAccount";
    }
    else if (searchinput === "SDATFA") {
        window.location.href = "/F_AddDistToAccount/DistToFactoryAccount";
    }
    else if (searchinput === "SCATFA") {
        window.location.href = "/F_AddCostAccountToAccount/CostAccountToFactoryAccount";
    }
    else if (searchinput === "SOCP") {
        window.location.href = "/OpenAndClosFiscalPeriods/OpenClosePeriods";
    }
    else if (searchinput === "SUP") {
        window.location.href = "/AccessOfUserPost/UserPost";
    }
    else if (searchinput === "SPS") {
        window.location.href = "/CreatePostingSetup/PostingSetup";
    }
    else if (searchinput === "IBADA") {
        window.location.href = "/Inquiry_BranchAnalyticDistribution/BranchAnalyticDistribution";
    }
    else if (searchinput === "IBCCA") {
        window.location.href = "/Inquiry_BranchCostCenterAccounts/BranchCenterAccounts";
    }
    else if (searchinput === "ICADA") {
        window.location.href = "/Inquiry_CompanyAnalyticDistribution/CompanyAnalyticDistribution";
    }
    else if (searchinput === "SPD") {
        window.location.href = "/PrintDocumentSetting/PrintDocument";
    }
    else if (searchinput === "ICCCA") {
        window.location.href = "/Inquiry_CompanyCostCenterAccounts/CompanyCenterAccounts";
    }
    else if (searchinput === "IFADA") {
        window.location.href = "/Inquiry_FactoryAnalyticDistribution/FactoryAnalyticDistribution";
    }
    else if (searchinput === "IFCCA") {
        window.location.href = "/Inquiry_FactoryCostCenterAccounts/FactoryCenterAccounts";
    }
    else if (searchinput === "TCCB") {
        window.location.href = "/C_CreateBatch/CompanyCreateBatch";
    }
    else if (searchinput === "TBCB") {
        window.location.href = "/B_CreateBatch/BranchCreateBatch";
    }
    else if (searchinput === "TFCB") {
        window.location.href = "/F_CreateBatch/FactoryCreateBatch";
    }
    else if (searchinput === "SUETR") {
        window.location.href = "/UserEditTRate/UEditRate";
    }
    else if (searchinput === "TCGE") {
        window.location.href = "/C_GeneralEntryTransaction/CompanyGETransaction";
    }
    else if (searchinput === "SCLAA") {
        window.location.href = "/C_LinkAnalyticToAccount/CompanyLinkAnalyticToAccount";
    }
    else if (searchinput === "SBLAA") {
        window.location.href = "/B_LinkAnalyticToAccount/BranchLinkAnalyticToAccount";
    }
    else if (searchinput === "SFLAA") {
        window.location.href = "/F_LinkAnalyticToAccount/FactoryLinkAnalyticToAccount";
    }
    else if (searchinput === "CDTBA") {
        window.location.href = "/B_LinkCDtoBA/CurrencyDefinitionToBranchAccount";
    }
    else if (searchinput === "CDTFA") {
        window.location.href = "/F_LinkCDtoFA/CurrencyDefinitionToFactoryAccount";
    }
    else if (searchinput === "TCBA") {
        window.location.href = "/C_CreateBatch/CompanyBatchApproval";
    }
    else if (searchinput === "TCS") {
        window.location.href = "/C_GeneralEntryTransaction/CompanyShowTransactions";
    }
    else if (searchinput === "TCMBP") {
        window.location.href = "/C_CreateBatch/CompanyMultiBatchesPost";
    }
    else if (searchinput === "IADI") {
        window.location.href = "/Inquiry_CompanyJETransactionsDetails/AccountDetails";
    }
    else if (searchinput === "IASI") {
        window.location.href = "/Inquiry_CompanyJETransactionsDetails/AccountSummary";
    }
    else if (searchinput === "ITDI") {
        window.location.href = "/Inquiry_CompanyJETransactionsDetails/TransactionDetails";
    }
    else if (searchinput === "ICBSI") {
        window.location.href = "/Inquiry_CompanyJETransactionsDetails/BatchSecurity";
    }
    else if (searchinput === "IHASI") {
        window.location.href = "/Inquiry_CompanyJETransactionsDetails/HistoricalAccountSummary";
    }
    else if (searchinput === "IHADI") {
        window.location.href = "/Inquiry_CompanyJETransactionsDetails/HistoricalAccountDetails";
    }
    else if (searchinput === "TCGV") {
        window.location.href = "/C_GeneralEntryTransaction/CompanyVoidTransactions";
    }

});

$('#seachForm').submit(function (e) {
    e.preventDefault();
    $("#searchinput").keyup(function (event) {
        event.preventDefault();
        if (event.keyCode === 13) {
            $("#SearchFormsBtn").click();
        }
    });
    /*
    $('#searchinput').on('autocompleteselect', function (e, ui) {
        searchinput = ui.item.value;
        $("#SearchFormsBtn").click();
    });

    $('#searchinput').on('change', function () {
        searchinput = this.value;
        $("#SearchFormsBtn").click();
    }).change();
    */

});


$("#searchinput").keyup(function (event) {
    event.preventDefault();
    if (event.keyCode === 13) {

        $("#SearchFormsBtn").click();
    }

});
/*
$('#searchinput').on('change', function () {
    $("#SearchFormsBtn").click();

}).change();

$('#searchinput').on('autocompleteselect', function (e, ui) {
    searchinput = ui.item.value;
    $("#SearchFormsBtn").click();
});
*/