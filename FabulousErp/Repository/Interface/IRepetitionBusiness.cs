using FabulousDB.DB_Tabels.Settings.Financial.GeneralSetting.C_B_F_Setup.CompanyBranchInfo;
using FabulousModels.DTOModels.Important;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace FabulousErp.Repository.Interface
{
    public interface IRepetitionBusiness
    {
        string GetAccountChartName(string ChartID);

        string GetUserName(string UserID);

        string GetGroupName(string GroupID);

        string GetCompanyName(string CompanyID);

        string GetBranchName(string BranchID);

        string GetFactoryName(string FactoryID);

        string GetFiscalYearName(string FiscalyearID);

        string GetCompCostCenterName(string CostCenterID);

        string GetBranchCostCenterName(string CostCenterID);

        string GetFactoryCostCenterName(string CostCenterID);

        SelectList RetrieveUserIDList();

        SelectList RetrieveGroupIDList();

        SelectList RetrieveCompIDList();

        SelectList RetrieveCompIDListCond(string companyID);

        SelectList RetrieveBranchIDListCond(string companyID);
        SelectList RetrieveBranchIDListCondByB(string branchID);

        SelectList RetrieveFactoryIDListCond(string companyID);
        SelectList RetrieveFactoryIDListCondByF(string factoryID);
        SelectList RetrieveFactoryIDListCondByB(string branchID);

        SelectList RetrieveBranchIDList();

        SelectList RetrieveFactoryIDList();

        SelectList RetrieveFiscalYearIDList();

        SelectList RetrieveAccountChartIDList();

        List<CompanyBranchInfo_Table> FilterBranchID(string CompanyID);

        List<Get_Small_Data_DTO> FilterCostCenterIDForComp(string CompanyID);

        List<Get_Small_Data_DTO> FilterCostCenterIDForBranch(string BranchID);

        List<Get_Small_Data_DTO> FilterCostCenterIDForFactory(string FactoryID);

        //string AddGroupEmpty();
    }
}
