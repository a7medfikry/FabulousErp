using FabulousDB.DB_Context;
using FabulousDB.DB_Tabels.Important;
using FabulousDB.DB_Tabels.Settings.Financial.GeneralSetting.C_B_F_Setup.CompanyBranchInfo;
using FabulousDB.DB_Tabels.Settings.Financial.GeneralSetting.UserGroup;
using FabulousErp.Repository.Interface;
using FabulousModels.DTOModels.Important;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FabulousErp;

namespace FabulousErp.Repository.Business
{
    public class RSelectBusiness : IRepetitionBusiness
    {

        DBContext DB = new DBContext();

        //public string AddGroupEmpty()
        //{
        //    var check = DB.CreateGroup_Tables.Where(x => x.GroupID == "EmptyGroup").FirstOrDefault();

        //    if (check == null)
        //    {
        //        CreateGroup_Table createGroup_Table = new CreateGroup_Table()
        //        {
        //            GroupID = "EmptyGroup",

        //            GroupName = "Relation",

        //            GroupDescription = "Exp",

        //            Date = "1",

        //            Deleted = true,

        //            DisActive = true,

        //            FromCBF = "E"
        //        };
        //        DB.CreateGroup_Tables.Add(createGroup_Table);
        //        DB.SaveChanges();
        //    }
        //    return null;
        //}

        public List<CompanyBranchInfo_Table> FilterBranchID(string CompanyID)
        {
            DB.Configuration.ProxyCreationEnabled = false;

            var BranchList = DB.CompanyBranchInfo_Tables.Where(x => x.CompanyID == CompanyID).ToList();

            if (BranchList != null)
            {
                return BranchList;
            }
            else
            {
                return null;
            }
        }

        public List<Get_Small_Data_DTO> FilterCostCenterIDForBranch(string BranchID)
        {
            DB.Configuration.ProxyCreationEnabled = false;

            List<Get_Small_Data_DTO> get_Small_Data_DTO = DB.B_CostCenter_Tables.Where(x => x.BranchID == BranchID).Select(x => new Get_Small_Data_DTO
            {

                CostCenterID = x.B_CostCenterID

            }).ToList();

            return get_Small_Data_DTO;
        }

        public List<Get_Small_Data_DTO> FilterCostCenterIDForComp(string CompanyID)
        {
            DB.Configuration.ProxyCreationEnabled = false;

            List<Get_Small_Data_DTO> get_Small_Data_DTO = DB.C_CostCenter_Tables.Where(x => x.CompanyID == CompanyID).Select(x => new Get_Small_Data_DTO
            {

                CostCenterID = x.C_CostCenterID

            }).ToList();

            return get_Small_Data_DTO;
        }

        public List<Get_Small_Data_DTO> FilterCostCenterIDForFactory(string FactoryID)
        {
            DB.Configuration.ProxyCreationEnabled = false;

            List<Get_Small_Data_DTO> get_Small_Data_DTO = DB.F_CostCenter_Tables.Where(x => x.FactoryID == FactoryID).Select(x => new Get_Small_Data_DTO
            {

                CostCenterID = x.F_CostCenterID

            }).ToList();

            return get_Small_Data_DTO;
        }

        public string GetAccountChartName(string ChartID)
        {
            DB.Configuration.ProxyCreationEnabled = false;

            var getChartName = DB.AccountChart_Table.Where(x => x.AccountChartID == ChartID).FirstOrDefault();

            if (getChartName != null)
            {
                return getChartName.AccountChartName;
            }
            else
            {
                return null;
            }
        }

        public string GetBranchCostCenterName(string CostCenterID)
        {
            DB.Configuration.ProxyCreationEnabled = false;

            var getCostCenterName = DB.B_CostCenter_Tables.Where(x => x.B_CostCenterID == CostCenterID).FirstOrDefault();

            if (getCostCenterName != null)
            {

                return getCostCenterName.B_CostCenterName;
            }
            else
            {
                return null;
            }
        }

        public string GetBranchName(string BranchID)
        {
            DB.Configuration.ProxyCreationEnabled = false;

            var GetBranchName = DB.CompanyBranchInfo_Tables.Where(x => x.BranchID == BranchID).FirstOrDefault();

            if (GetBranchName != null)
            {
                return GetBranchName.BranchName;
            }
            else
            {
                return null;
            }
        }

        public string GetCompanyName(string CompanyID)
        {
            DB.Configuration.ProxyCreationEnabled = false;

            var GetCompName = DB.CompanyMainInfo_Tables.Where(x => x.CompanyID == CompanyID).FirstOrDefault();

            if (GetCompName != null)
            {
                return GetCompName.CompanyName;
            }
            else
            {
                return null;
            }
        }

        public string GetCompCostCenterName(string CostCenterID)
        {
            DB.Configuration.ProxyCreationEnabled = false;

            var getCostCenterName = DB.C_CostCenter_Tables.Where(x => x.C_CostCenterID == CostCenterID).FirstOrDefault();

            if (getCostCenterName != null)
            {
                return getCostCenterName.C_CostCenterName;
            }
            else
            {
                return null;
            }
        }

        public string GetFactoryCostCenterName(string CostCenterID)
        {
            DB.Configuration.ProxyCreationEnabled = false;

            var getCostCenterName = DB.F_CostCenter_Tables.Where(x => x.F_CostCenterID == CostCenterID).FirstOrDefault();

            if (getCostCenterName != null)
            {
                return getCostCenterName.F_CostCenterName;
            }
            else
            {
                return null;
            }
        }

        public string GetFactoryName(string FactoryID)
        {
            DB.Configuration.ProxyCreationEnabled = false;

            var GetName = DB.CompanyFactoryInfo_Tables.Where(x => x.FactoryID == FactoryID).FirstOrDefault();

            if (GetName != null)
            {
                return GetName.FactoryName;
            }
            else
            {
                return null;
            }
        }

        public string GetFiscalYearName(string FiscalyearID)
        {

            DB.Configuration.ProxyCreationEnabled = false;

            var Item = DB.FiscalDefinition_Tables.Where(x => x.Fiscal_Year_ID == FiscalyearID).FirstOrDefault();
            if (Item != null)
            {
                return Item.Fiscal_Year_Name;
            }
            else
            {
                return null;
            }
        }

        public string GetGroupName(string GroupID)
        {
            DB.Configuration.ProxyCreationEnabled = false;

            var GetName = DB.CreateGroup_Tables.Where(x => x.GroupID == GroupID).FirstOrDefault();

            if (GetName != null)
            {
                return GetName.GroupName;
            }
            else
            {
                return null;
            }
        }

        public string GetUserName(string UserID)
        {
            DB.Configuration.ProxyCreationEnabled = false;

            var GetName = DB.CreateAccount_Tables.Where(x => x.UserID == UserID).FirstOrDefault();

            if (GetName != null)
            {
                return GetName.UserName;
            }
            else
            {
                return null;
            }
        }

        public SelectList RetrieveAccountChartIDList()
        {
            DB.Configuration.ProxyCreationEnabled = false;

            var ChartID = DB.AccountChart_Table.ToList();
            SelectList CompanyList = new SelectList(ChartID, "AccountChartID", "AccountChartID");

            return CompanyList;
        }

        public SelectList RetrieveBranchIDList()
        {
            DB.Configuration.ProxyCreationEnabled = false;

            var BranchID = DB.CompanyBranchInfo_Tables.ToList();
            SelectList BranchList = new SelectList(BranchID, "BranchID", "BranchID");

            return BranchList;
        }

        public SelectList RetrieveBranchIDListCond(string companyID)
        {
            DB.Configuration.ProxyCreationEnabled = false;

            var BranchID = DB.CompanyBranchInfo_Tables.Where(x => x.CompanyID == companyID).ToList();
            SelectList BranchList = new SelectList(BranchID, "BranchID", "BranchID");

            return BranchList;
        }

        public SelectList RetrieveBranchIDListCondByB(string branchID)
        {
            DB.Configuration.ProxyCreationEnabled = false;

            var BranchID = DB.CompanyBranchInfo_Tables.Where(x => x.BranchID == branchID).ToList();
            SelectList BranchList = new SelectList(BranchID, "BranchID", "BranchID");

            return BranchList;
        }

        public SelectList RetrieveCompIDList()
        {
            DB.Configuration.ProxyCreationEnabled = false;

            var CompanyID = DB.CompanyMainInfo_Tables.ToList();
            SelectList CompanyList = new SelectList(CompanyID, "CompanyID", "CompanyID",FabulousErp.Business.GetCompanyId());

            return CompanyList;
        }

        public SelectList RetrieveCompIDListCond(string companyID)
        {
            DB.Configuration.ProxyCreationEnabled = false;

            var CompanyID = DB.CompanyMainInfo_Tables.Where(x => x.CompanyID == companyID).ToList();
            SelectList CompanyList = new SelectList(CompanyID, "CompanyID", "CompanyID", FabulousErp.Business.GetCompanyId());

            return CompanyList;
        }

        public SelectList RetrieveFactoryIDList()
        {
            DB.Configuration.ProxyCreationEnabled = false;

            var BranchID = DB.CompanyFactoryInfo_Tables.ToList();
            SelectList FactoryList = new SelectList(BranchID, "FactoryID", "FactoryID");

            return FactoryList;
        }

        public SelectList RetrieveFactoryIDListCond(string companyID)
        {
            DB.Configuration.ProxyCreationEnabled = false;
            var FactoryID = DB.CompanyFactoryInfo_Tables.Where(x => x.CompanyID == companyID).ToList();
            SelectList FactoryList = new SelectList(FactoryID, "FactoryID", "FactoryID");
            return FactoryList;
        }

        public SelectList RetrieveFactoryIDListCondByB(string branchID)
        {
            DB.Configuration.ProxyCreationEnabled = false;
            var FactoryID = DB.CompanyFactoryInfo_Tables.Where(x => x.BranchID == branchID).ToList();
            if (FactoryID != null)
            {
                SelectList FactoryList = new SelectList(FactoryID, "FactoryID", "FactoryID");
                return FactoryList;
            }
            else
            {
                return null;
            }
        }

        public SelectList RetrieveFactoryIDListCondByF(string factoryID)
        {
            DB.Configuration.ProxyCreationEnabled = false;
            var FactoryID = DB.CompanyFactoryInfo_Tables.Where(x => x.FactoryID == factoryID).ToList();
            SelectList FactoryList = new SelectList(FactoryID, "FactoryID", "FactoryID");
            return FactoryList;
        }

        public SelectList RetrieveFiscalYearIDList()
        {
            DB.Configuration.ProxyCreationEnabled = false;

            var Items1 = DB.FiscalDefinition_Tables.ToList();
            SelectList FiscalYearID = new SelectList(Items1, "Fiscal_Year_ID", "Fiscal_Year_ID");

            return FiscalYearID;
        }

        public SelectList RetrieveGroupIDList()
        {
            DB.Configuration.ProxyCreationEnabled = false;

            var GroupID = DB.CreateGroup_Tables.Where(a => a.Deleted == false || a.Deleted == null).ToList();
            SelectList GroupList = new SelectList(GroupID, "GroupID", "GroupID");

            return GroupList;
        }

        public SelectList RetrieveUserIDList()
        {
            DB.Configuration.ProxyCreationEnabled = false;

            var UserID = DB.CreateAccount_Tables.Where(a => a.Deleted == false || a.Deleted == null).ToList();
            SelectList UserList = new SelectList(UserID, "UserID", "UserID");

            return UserList;
        }

        public static DBContext DBC()
        {
            return new DBContext();
        }

        public static void AddError()
        {
            DBContext DB = new DBContext();

            Exception exception = HttpContext.Current.Server.GetLastError();

            if (exception != null && exception.InnerException != null)
            {
                string error = exception.ToString();

                error += "InnerException " + exception.InnerException.ToString();
                Exceptions_Table exceptions_Table = new Exceptions_Table()
                {
                    URL = HttpContext.Current.Request.Url.AbsolutePath,

                    Exception = error,

                    Date = DateTime.Now
                };
                DB.Exceptions_Tables.Add(exceptions_Table);
                DB.SaveChanges();
            }

            //Request.Url.AbsolutePath;
            //to database



        }
    }
}