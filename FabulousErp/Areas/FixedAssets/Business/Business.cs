using FabulousDB.DB_Context;
using FabulousDB.DB_Tabels.Transaction.Financial.Company.Accounting;
using FabulousDB.Models;
using FabulousErp.Controllers.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FabulousDB.Models;
namespace FixedAssets.Business
{
    public partial class Business
    {
        public static bool GetChangeDeprecationMethod()
        {
            using (DBContext Fdb = new DBContext())
            {
                return Fdb.Deprecation_Setting.FirstOrDefault().Change_deprecation_method;
            }
        }
        public static Deleted_assets GetDeleteAssets(Asset asset)
        {
            return new Deleted_assets
            {
                Acquisation_cost = asset.Acquisation_cost,
                Adjustment_cost = asset.Adjustment_cost,
                Assets_class_id = asset.Assets_class_id,
                Assets_main_id = asset.Assets_main_id,
                Assets_number = asset.Assets_number,
                Assets_transaction_id = asset.Assets_transaction_id,
                Book_id = asset.Book_id,
                Company_id = asset.Company_id,
                Date_of_Adjustmemt = asset.Date_of_Adjustmemt,
                Date_of_orgin = asset.Date_of_orgin,
                Deactive_depraction = asset.Deactive_depraction,
                Deprecation_method = asset.Deprecation_method,
                Deprecation_rate = asset.Deprecation_rate,
                Description = asset.Description,
                Disposal = asset.Disposal,
                Foreign_name = asset.Foreign_name,
                Fully_depraction = asset.Fully_depraction,
                Include_scerap_value = asset.Include_scerap_value,
                Number_of_units = asset.Number_of_units,
                Proparty_type = asset.Proparty_type,
                Scrap_value = asset.Scrap_value,
                Start_derecation_date = asset.Start_derecation_date,
                Start_use = asset.Start_use,
                Type = asset.Type,
                Use_life = asset.Use_life,
                Deleted_date = DateTime.Now
            };
        }
        public static Deleted_assets_class GetDeleteAssetsClass(Assets_class assetclass)
        {
            return new Deleted_assets_class
            {
                Active = assetclass.Active,
                Class_id = assetclass.Class_id,
                Company_id = assetclass.Company_id,
                Deperecation_rate = assetclass.Deperecation_rate,
                Deprecation_method = assetclass.Deprecation_method,
                Description = assetclass.Description,
                Deleted_date = DateTime.Now
            };
        }

        public static Delete_assets_main GetDeleteMainClass(Assets_main assetmain)
        {
            return new Delete_assets_main
            {
                Assets_class_id = assetmain.Assets_class_id,
                Assets_custom_id = assetmain.Assets_custom_id,
                Description = assetmain.Description,
                Assets_number = assetmain.Assets_number,
                Company_id = assetmain.Company_id,
                Inactive = assetmain.Inactive,
                Number_of_parts = assetmain.Number_of_parts,
                Deleted_date=DateTime.Now
            };
        }
        public static Deleted_fixed_assets_renewal GetDeleteRenwal(Fixed_assets_renewal Renw)
        {
            return new Deleted_fixed_assets_renewal
            {
                Assets_id=Renw.Assets_id,
                Company_id=Renw.Company_id,
                Currency_id=Renw.Currency_id,
                Delete_date=DateTime.Now,
                Deprecation_rate=Renw.Deprecation_rate,
                Descroption=Renw.Descroption,
                Gl_transaction_id=Renw.Gl_transaction_id,
                Renewal_amount=Renw.Renewal_amount,
                Renewal_no=Renw.Renewal_no,
                Renwal_date=Renw.Renwal_date,
                Use_life=Renw.Use_life
            };
        }
        public static Delete_fixed_assets_disposel GetDeleteDisposal(Fixed_assets_disposel Dis)
        {
            return new Delete_fixed_assets_disposel
            {
                Assets_id = Dis.Assets_id,
                Company_id = Dis.Company_id,
                Currency_id = Dis.Currency_id,
                Delete_date = DateTime.Now,
                Depreication_up_to_date=Dis.Depreication_up_to_date,
                Disposal_amount=Dis.Disposal_amount,
                Disposal_date=Dis.Disposal_date,
                Disposal_no=Dis.Disposal_no,
                Gl_transaction_id=Dis.Gl_transaction_id,
                Reference=Dis.Reference,
                Transaction_date=Dis.Transaction_date
            };
        }
        public static Delete_fixed_assets_revaluate GetDeleteRevluate(Fixed_assets_revaluate Rev)
        {
            return new Delete_fixed_assets_revaluate
            {
                Assets_id = Rev.Assets_id,
                Company_id = Rev.Company_id,
                Delete_date = DateTime.Now,
                Transaction_date = Rev.Transaction_date,
                Adjustment_cost=Rev.Adjustment_cost,
                Net_profit=Rev.Net_profit,
                Old_cost=Rev.Old_cost,
                Old_use_life=Rev.Old_use_life,
                Revaluate_date=Rev.Revaluate_date,
                Revaluate_no=Rev.Revaluate_no
            };
        }
        public static int PostingToOrThrow()
        {
            return FabulousErp.Business.PostingToOrThrow();
        }
        public static void VoidThisTransaction(int GlTransaction,string companyID,string PostinKey
            ,string TransactionDate,string PostingDate,string Ref)
        {
            using (TransactionApiController Trans = new TransactionApiController())
            {
                using (DBContext DB = new DBContext())
                {
                    C_GeneralJournalEntry_Table Head = DB.C_GeneralJournalEntry_Tables.Where(x => x.C_JournalEntryNumber == GlTransaction).FirstOrDefault();

                    C_GeneralJournalEntry_Table SaveHeader = new C_GeneralJournalEntry_Table
                    {
                        C_PostingDate = PostingDate,
                        C_TransactionDate = TransactionDate,
                        C_Refrence = Ref,
                        CurrencyID = companyID,
                        C_SystemRate = Head.C_SystemRate,
                        C_TransactionRate = Head.C_TransactionRate,
                        C_PostingKey = PostinKey,
                        C_TransactionType = PostinKey,
                        C_PostingNumber= Head.C_PostingNumber
                    };
                    List<C_SaveTransaction_Table> SaveTransaction = DB.C_SaveTransaction_Tables.Where(x => x.C_PostingNumber == Head.C_PostingNumber).ToList();

                    Trans.InsertTransactionData(companyID,Business.PostingToOrThrow(), Head.C_PostingNumber
                        , new FabulousModels.APIModels.TransactionApiData
                        {
                            SaveAnalytic = null,
                            SaveHeader = SaveHeader,
                            SaveTransaction = SaveTransaction.ToArray()
                        }, false);
                }
            }
        }
        public static InsertAssets_main GetInsertAssets(Assets_main assets)
        {
            InsertAssets_main Iassets = new InsertAssets_main
            {
                Assets=assets.Assets,
                Assets_class=assets.Assets_class,
                Assets_class_id=assets.Assets_class_id,
                Assets_custom_id=assets.Assets_custom_id,
                Assets_number=assets.Assets_number,
                Company_id=assets.Company_id,
                Description=assets.Description,
                Inactive=assets.Inactive,
                Number_of_parts=assets.Number_of_parts
            };
            return Iassets;
        }
        public static int GetPotinNumber(int? JornalEntry)
        {
            try
            {
                using (DBContext Mdb = new DBContext())
                {
                    if (JornalEntry.HasValue)
                    {
                        return Mdb.C_GeneralJournalEntry_Tables.FirstOrDefault(x => x.C_JournalEntryNumber == JornalEntry.Value)
                        .C_PostingNumber;
                    }
                    else
                    {
                        return 0;
                    }
                }
            }
            catch
            {
                return 0;
            }
        }
    }

}