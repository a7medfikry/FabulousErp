using FabulousDB.DB_Context;
using FabulousDB.DB_Tabels.Settings.Financial.GeneralSetting.Post;
using FabulousDB.Migrations;
using FabulousDB.Models;
using FabulousErp.Bussiness;
using FabulousErp.Payable.Models;
using FabulousErp.Receivable.Models;
using FabulousModels.DTOModels.Settings.Financial.Accounting.CurrenciesDefinition;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Caching;
using System.Web.Routing;

namespace FabulousErp
{
    public class Business
    {
        public enum FinancialForms
        {
            Company_Journal_Entry_Transaction = 1,
            Company_Cash_Reciept = 2,
            Company_Cash_Withdraw = 3,
            Company_Bank_Checkout = 4,
            Company_Bank_CheckReceived = 5,
            Checkbook_Transfer = 6,
            Payable_transaction = 7,
            Payable_payment = 8,
            Receivable_transaction = 9,
            Receivable_payment = 10,
            FixedAssets_Assets = 11,
            FixedAssets_Renewal = 12,
            FixedAssets_Disposal = 13,
        }
        public enum CheckBookKey
        {
            TCCW = 1,
            TCBC = 2,
            TCCR = 3,
            TCBR = 4,
        }
        public static List<T> GetEnumValues<T>() where T : struct
        {
            if (!typeof(T).IsEnum)
            {
                throw new ArgumentException("GetValues<T> can only be called for types derived from System.Enum", "T");
            }
            return ((T[])Enum.GetValues(typeof(T))).ToList();
        }
        //  static DBContext db = new DBContext();
        public static int GetPotingNumber(int? JornalEntry)
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
        public static int GetJournalEntry(int? PostingNumber)
        {
            try
            {
                using (DBContext Mdb = new DBContext())
                {
                    if (PostingNumber.HasValue)
                    {
                        return Mdb.C_GeneralJournalEntry_Tables.FirstOrDefault(x => x.C_PostingNumber == PostingNumber.Value)
                        .C_JournalEntryNumber;
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
        public static int GetCheckBookNumber(CheckBookKey PostingKey, string companyID, int CheckbookID)
        {
            DBContext DB = new DBContext();
            int documentNumber = 0;
            if (PostingKey == CheckBookKey.TCCW ||
                PostingKey == CheckBookKey.TCBC)
            {
                var nextWithdrawNumber = DB.C_CheckBookSetting_Tables.Where(x => x.CompanyID == companyID && x.C_CBSID == CheckbookID).FirstOrDefault();
                documentNumber = nextWithdrawNumber.C_NextWithdrawNumber;
                nextWithdrawNumber.C_NextWithdrawNumber = nextWithdrawNumber.C_NextWithdrawNumber + 1;
                DB.SaveChanges();
                return documentNumber;
            }
            else if (PostingKey == CheckBookKey.TCBR ||
                PostingKey == CheckBookKey.TCCR)
            {
                var nextDepositeNumber = DB.C_CheckBookSetting_Tables.Where(x => x.CompanyID == companyID && x.C_CBSID == CheckbookID).FirstOrDefault();
                documentNumber = nextDepositeNumber.C_NextDepositNumber;
                nextDepositeNumber.C_NextDepositNumber = nextDepositeNumber.C_NextDepositNumber + 1;
                DB.SaveChanges();
                return documentNumber;
            }
            else
            {
                return 0;
            }

        }
        public static int PostingToOrThrow()
        {
            DBContext DB = new DBContext();
            try
            {
                string companyID = GetCompanyId();
                string AreaName = GetCurrentAreaName();
                var detectJEPer = DB.PostingSetup_Tables.Where(x =>
                x.CompanyID == companyID &&
                x.Module == AreaName).FirstOrDefault();

                int PostingToOrThrow = 0;
                if (detectJEPer.PostingType == "A1")
                {
                    PostingToOrThrow = 1;
                }
                else
                {
                    PostingToOrThrow = 2;
                }
                return PostingToOrThrow;
            }
            catch
            {
                return 0;
            }

        }
        public static bool AllowEditPostingDate()
        {
            DBContext DB = new DBContext();
            try
            {
                string companyID = GetCompanyId();
                string AreaName = GetCurrentAreaName();
                return DB.PostingSetup_Tables.Where(x =>
                x.CompanyID == companyID &&
                x.Module == AreaName).FirstOrDefault().EditPostingDate == "F1" ? true : false;
            }
            catch
            {
                return true;
            }

        }
        public static double PostiveSubtract(DateTime First, DateTime Second)
        {
            double asd = 0;
            if (First > Second)
            {
                asd = First.Subtract(Second).TotalDays;
            }
            else
            {
                asd = Second.Subtract(First).TotalDays;
            }
            return asd;
        }
        public static decimal PostiveSubtract(decimal First, decimal Second)
        {
            decimal Res = 0;
            if (First > Second)
            {

                Res = ToPostive(First) - ToPostive(Second);
            }
            else
            {
                Res = ToPostive(Second) - ToPostive(First);
            }
            return Res;
        }
        public static float PostiveSubtract(float First, float Second)
        {
            float Res = 0;
            if (First > Second)
            {

                Res = ToPostive(First) - ToPostive(Second);
            }
            else
            {
                Res = ToPostive(Second) - ToPostive(First);
            }
            return Res;
        }
        public static decimal ToPostive(decimal Number)
        {
            if (Number < 0)
            {
                Number = -Number;
            }
            return Number;
        }
        public static float ToPostive(float Number)
        {
            if (Number < 0)
            {
                Number = -Number;
            }
            return Number;
        }
        public static string GetDecimalNumber()
        {
            string Decimal = "0.";
            for (int i = 0; i < GetDecimalPointNumber(); i++)
            {
                Decimal += "0";
            }
            return Decimal;
        }
        public static decimal RoundNumber(decimal Number)
        {
            return Math.Round(Number, GetDecimalPointNumber());
        }
        public static int GetDecimalPointNumber()
        {
            try
            {
                using (DBContext db = new DBContext())
                {
                    return Convert.ToInt32(db.FormateSetting_Tables.FirstOrDefault().Decimal);
                }
            }
            catch
            {
                return 2;
            }

        }
        public static bool GetPageTransactionSetting(FinancialForms P)
        {
            try
            {
                using (DBContext DB = new DBContext())
                {
                    return DB.C_EditTRates.FirstOrDefault(x => x.TransactionFormName == P.ToString().Replace("_", " ")).AllowUserE;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public static string GetUserId()
        {
            try
            {
                HttpCookie LoginCookie = HttpContext.Current.Request.Cookies["Login"];
                if (LoginCookie != null)
                {
                    return LoginCookie["UserId"];
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                return null;
            }
        }
        public static string GetUserName()
        {
            try
            {
                HttpCookie LoginCookie = HttpContext.Current.Request.Cookies["Login"];
                if (LoginCookie != null)
                {
                    return LoginCookie["UserName"];
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                return null;
            }
        }
        public static string GetCompanyId()
        {
            try
            {
                HttpCookie LoginCookie = HttpContext.Current.Request.Cookies["Login"];
                //HttpCookie LoginCookie2 = HttpContext.Current.Response.Cookies["Login"];
                if (LoginCookie != null)
                {
                    return LoginCookie["CompanyID"];
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                return null;
            }
        }
        public static string GetMainIso()
        {
            try
            {
                string CompId = GetCompanyId();
                using (DBContext db = new DBContext())
                {
                  return  db.CurrenciesDefinition_Tables.Where(x => x.CompanyID == CompId).FirstOrDefault().ISOCode;
                }
            }
            catch
            {
                return "EGP";
            }
        }
        public static string GetLanguage()
        {
            try
            {
                HttpCookie LoginCookie = HttpContext.Current.Request.Cookies["Lang"];
                if (LoginCookie != null)
                {
                    return LoginCookie["Lang"];
                }
                else
                {
                    return FabulousDB.Models.Langs.English.ToString();
                }
            }
            catch
            {
                return FabulousDB.Models.Langs.English.ToString();
            }
        }
        public static string GetROL()
        {
            if (GetLanguage() == FabulousDB.Models.Langs.English.ToString())
            {
                return "left";
            }
            else
            {
                return "right";
            }
        }
        public static string GetCompLogo()
        {
            try
            {
                using (DBContext db = new DBContext())
                {
                    string CompId = GetCompanyId();
                    var base64 = Convert.ToBase64String(db.CompanyMainInfo_Tables.FirstOrDefault(x => x.CompanyID == CompId).LogoByte);
                    string Logo = String.Format("data:image/gif;base64,{0}", base64);
                    return Logo;
                }
            }
            catch
            {
                return "#";
            }

        }
        public static string GetCleanLink(string Link)
        {
            Link = new String(Link.Where(c => (c < '0' || c > '9')).ToArray());
            if (Link[Link.Length - 1] == '/')
            {
                Link = Link.Remove(Link.Length - 1, 1);
            }
            return Link;
        }

        public static void SetPrefix(int Id, string Prefix)
        {
            using (DBContext db = new DBContext())
            {
                db.C_CreateAccount_Tables.FirstOrDefault(x => x.C_AID == Id).C_Prefix = Prefix;
                db.SaveChanges();
            }

        }

        // This size of the IV (in bytes) must = (keysize / 8).  Default keysize is 256, so the IV must be
        // 32 bytes long.  Using a 16 character string here gives us 32 bytes when converted to a byte array.
        private const string initVector = "Fabulouspgzl88Cd";
        // This constant is used to determine the keysize of the encryption algorithm
        private const int keysize = 256;
        //Encrypt
        public static string EncryptString(string plainText, string passPhrase = "ErpSystemPas@&&*sKeyForFa)buol(ousErpSystem123!@P@ssWordEncyrption")
        {
            byte[] initVectorBytes = Encoding.UTF8.GetBytes(initVector);
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            PasswordDeriveBytes password = new PasswordDeriveBytes(passPhrase, null);
            byte[] keyBytes = password.GetBytes(keysize / 8);
            RijndaelManaged symmetricKey = new RijndaelManaged();
            symmetricKey.Mode = CipherMode.CBC;
            ICryptoTransform encryptor = symmetricKey.CreateEncryptor(keyBytes, initVectorBytes);
            MemoryStream memoryStream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
            cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
            cryptoStream.FlushFinalBlock();
            byte[] cipherTextBytes = memoryStream.ToArray();
            memoryStream.Close();
            cryptoStream.Close();
            return Convert.ToBase64String(cipherTextBytes);
        }
        //Decrypt
        public static string DecryptString(string cipherText, string passPhrase = "ErpSystemPas@&&*sKeyForFa)buol(ousErpSystem123!@P@ssWordEncyrption")
        {
            byte[] initVectorBytes = Encoding.UTF8.GetBytes(initVector);
            byte[] cipherTextBytes = Convert.FromBase64String(cipherText);
            PasswordDeriveBytes password = new PasswordDeriveBytes(passPhrase, null);
            byte[] keyBytes = password.GetBytes(keysize / 8);
            RijndaelManaged symmetricKey = new RijndaelManaged();
            symmetricKey.Mode = CipherMode.CBC;
            ICryptoTransform decryptor = symmetricKey.CreateDecryptor(keyBytes, initVectorBytes);
            MemoryStream memoryStream = new MemoryStream(cipherTextBytes);
            CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
            byte[] plainTextBytes = new byte[cipherTextBytes.Length];
            int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
            memoryStream.Close();
            cryptoStream.Close();
            return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
        }
        public static string GetCompCurrIso()
        {
            using (DBContext db = new DBContext())
            {
                string companyID = GetCompanyId();
                return db.CurrenciesDefinition_Tables.Where(x => x.CurrencyID == companyID).ToList().DefaultIfEmpty(new FabulousDB.DB_Tabels.Settings.Financial.Accounting.CurrenciesDefinition.CurrenciesDefinition_Table { ISOCode = "" }).FirstOrDefault().ISOCode;
            }
        }


        public static string GetItemBySt(int ST, Inv_item Item)
        {
            if (ST == 0)
            {
                return Item.Item_id;
            }
            else if (ST == 1)
            {
                return Item.Short_description;
            }
            else
            {
                return Item.Description;

            }
        }
        public static string GetVendoreByDV(int SV, Payable_creditor_setting Vendore)
        {
            if (SV == 0)
            {
                return Vendore.Vendor_id;
            }
            else
            {
                return Vendore.Vendor_name;
            }
        }
        public static string GetCustomerByDV(int SV, Receivable_vendore_setting Customer)
        {
            if (SV == 0)
            {
                return Customer.Vendor_id;
            }
            else
            {
                return Customer.Vendor_name;
            }
        }
        public static string Translate(string Word, bool RemoveDigit = false)
        {
            return BusController.Translate(Word, RemoveDigit);
        }
        public static List<string> GetAreasNames()
        {
            List<string> areaNames = RouteTable.Routes.OfType<Route>()
   .Where(d => d.DataTokens != null && d.DataTokens.ContainsKey("area"))
   .Select(r => r.DataTokens["area"]).Cast<string>().ToList<string>();
            areaNames.Add("Financial");
            return areaNames;
        }
        public static string GetCurrentAreaName()
        {
            try
            {
                return HttpContext.Current.Request.RequestContext.RouteData.DataTokens["area"].ToString();
            }
            catch
            {
                return "Financial";
            }
        }
        public static void GetUrlsNames()
        {

            Assembly axsm = Assembly.GetAssembly(typeof(FabulousErp.MvcApplication));

            var controlleractionlist = axsm.GetTypes()
                    .Where(type => typeof(System.Web.Mvc.Controller).IsAssignableFrom(type))
                    .SelectMany(type => type.GetMethods(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public))
                    .Where(m => !m.GetCustomAttributes(typeof(System.Runtime.CompilerServices.CompilerGeneratedAttribute), true).Any())
                    .Select(x => new { Controller = x.DeclaringType.Name, Action = x.Name, ReturnType = x.ReturnType.Name, Attributes = String.Join(",", x.GetCustomAttributes().Select(a => a.GetType().Name.Replace("Attribute", ""))) })
                    .OrderBy(x => x.Controller).ThenBy(x => x.Action).ToList();

            var areaNames = RouteTable.Routes.OfType<Route>()
    .Where(d => d.DataTokens != null && d.DataTokens.ContainsKey("area"))
    .Select(r => r.DataTokens["area"]).ToArray();

        }
        public static bool HasPostringSetup()
        {
            string companyID = GetCompanyId();
            string AreaName = GetCurrentAreaName();
            return db().PostingSetup_Tables.Any(x =>
               x.CompanyID == companyID &&
               x.Module == AreaName && !string.IsNullOrEmpty(x.PostingType));
        }
        public static PostingSetup_Table GetPostingSetup()
        {
            string companyID = GetCompanyId();
            string AreaName = GetCurrentAreaName();
            try
            {
                return db().PostingSetup_Tables.FirstOrDefault(x =>
                   x.CompanyID == companyID &&
                   x.Module == AreaName);
            }
            catch
            {
                return db().PostingSetup_Tables.Where(x =>
                  x.CompanyID == companyID &&
                  x.Module == "Financial").ToList().DefaultIfEmpty(null).FirstOrDefault();
            }
       
        }
        public static int GetDigitsIgnoreChars(string NextNumber)
        {
            int NextInt = 0;
            if (int.TryParse(NextNumber, out NextInt))
            {
                NextInt = Convert.ToInt32(NextNumber);
            }
            return NextInt;
        }
        public static int GetDigits(string NextNumber,bool AddOne=false)
        {
            int NextInt;
            if (int.TryParse(NextNumber, out NextInt))
            {
                NextInt = Convert.ToInt32(NextNumber);
                if (AddOne)
                {
                    NextInt += 1;
                }
            }
            else
            {
                try
                {
                    NextInt = Convert.ToInt32(System.Text.RegularExpressions
                        .Regex.Match(NextNumber, @"\d+").Value);
                    if (AddOne)
                    {
                        NextInt += 1;
                    }
                }
                catch
                {
                    NextInt = 1;
                }
            }
          
            return NextInt;
        }

        public static void SetCompanyCookie(string CompanyId)
        {

            HttpCookie Login;
            if (HttpContext.Current.Request.Cookies["Login"] != null)
            {
                Login = HttpContext.Current.Request.Cookies["Login"];
            }
            else
            {
                Login = new HttpCookie("Login");
            }

            Login["CompanyID"] = CompanyId;
            Login.Expires = DateTime.Now.AddHours(8);
            HttpContext.Current.Response.Cookies.Add(Login);
            
        }
        public static void SetUserCookie(string UserId, string UserName)
        {
            HttpCookie Login;
            if (HttpContext.Current.Request.Cookies["Login"] != null)
            {
                Login = HttpContext.Current.Request.Cookies["Login"];
            }
            else
            {
                Login = new HttpCookie("Login");
            }

            Login["UserId"] = UserId;
            Login["UserName"] = UserName;
            Login.Expires = DateTime.Now.AddHours(8);
            HttpContext.Current.Response.Cookies.Add(Login);
        }
        public static void DelCookies()
        {
            HttpCookie Login = new HttpCookie("Login");
            //HttpCookie Login = //HttpContext.Current.Request.Cookies["Login"];
            //HttpContext.Current.Request.Cookies["Login"].Expires = DateTime.Now.AddDays(-1);
            Login.Expires = DateTime.Now.AddDays(-1d);

            //Login["UserId"] = null;
            //Login["UserName"] = null;
            //Login["CompanyID"] = null;
            //HttpContext.Current.Response.Cookies.Remove("Login");
            HttpContext.Current.Response.Cookies.Add(Login);

            //HttpContext.Current.Request.Cookies.Remove("Login");
            //HttpContext.Current.Request.Cookies.Set(Login);
        }
        public static DBContext db()
        {
            return new DBContext();
        }

        public static Formate_Setting_DTO GetCurrencyFormate()
        {
            try
            {
                using (DBContext db = new DBContext())
                {
                    string companyID = FabulousErp.Business.GetCompanyId();
                    FabulousDB.DB_Tabels.Settings.Financial.Accounting.CurrenciesDefinition
                        .FormateSetting_Table getFormatSetting = db.FormateSetting_Tables.Where(x => x.CompanyID == companyID).FirstOrDefault();

                    if (getFormatSetting != null)
                    {
                        if (string.IsNullOrEmpty(getFormatSetting.Suffix))
                        {
                            getFormatSetting.Suffix = " EGP";
                        }
                        Formate_Setting_DTO formate_Setting_DTO = new Formate_Setting_DTO()
                        {
                            DecimalNumber = getFormatSetting.DecimalNumber,

                            Prefix = getFormatSetting.Prefix,

                            Suffix = getFormatSetting.Suffix,

                            Decimal = getFormatSetting.Decimal,

                            Thousands = getFormatSetting.Thousands,
                        };
                        return formate_Setting_DTO;
                    }
                    return new Formate_Setting_DTO
                    {
                        DecimalNumber = "2",

                        Prefix = "",

                        Suffix = "EGP",

                        Decimal = ".",

                        Thousands = ",",
                    };
                }
             
            }
            catch
            {
                return new Formate_Setting_DTO
                {
                    DecimalNumber = "2",

                    Prefix = "",

                    Suffix = "EGP",

                    Decimal = ".",

                    Thousands = ",",
                };
            }
            
            
        }
        public static void AddToCache(string name, string value)
        {
            Cache C = new Cache();
            if (C[name] == null)
            {
                C.Add(name, value, null, DateTime.Now.AddHours(1), TimeSpan.Zero, CacheItemPriority.High, null);
            }
        }
        public static string GetFromCache(string name)
        {
            Cache C = new Cache();
            string Res = Newtonsoft.Json.JsonConvert.SerializeObject(C[name]);
            return Res;
        }
    }
   
    public static class Extensions
    {
        /// <summary>
        ///     A generic extension method that aids in reflecting 
        ///     and retrieving any attribute that is applied to an `Enum`.
        /// </summary>
        public static TAttribute GetAttribute<TAttribute>(this Enum enumValue)
                where TAttribute : Attribute
        {
            return enumValue.GetType()
                            .GetMember(enumValue.ToString())
                            .First()
                            .GetCustomAttribute<TAttribute>();
        }

        public static TAttribute RemoveZeroFromProp<TAttribute>(this Enum enumValue)
          where TAttribute : Attribute
        {
            return enumValue.GetType()
                            .GetMember(enumValue.ToString())
                            .First()
                            .GetCustomAttribute<TAttribute>();
        }
        public static T Clone<T>(T Source)
        {
            //if (!typeof(T).IsSerializable)
            //{
            //    throw new ArgumentException("Not Serializable");
            //}
            if (Object.ReferenceEquals(Source, null))
            {
                return default(T);
            }
            System.Runtime.Serialization.IFormatter Formate = new System.Runtime.Serialization.Formatters.
                Binary.BinaryFormatter();
            Stream S = new MemoryStream();
            using (S)
            {
                Formate.Serialize(S, Source);
                S.Seek(0, SeekOrigin.Begin);
                return (T)Formate.Deserialize(S);
            }
        }
        public static void AddToFront<T>(this List<T> list, T item)
        {
            list.Insert(0, item);
        }
    }
    
    public class JvManyAccountFormate
    {
        public int AID { get; set; }
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
        public decimal Orginal_debit { get; set; }
        public decimal Orginal_credit { get; set; }
        public string AccountID { get; set; }
        public string AccountName { get; set; }
        public bool ShowBtn { get; set; } = true;
        public string Orginal_curr { get; set; }
        public string Mark { get; set; }

    }
    public class JvHead
    {
        public string ISO { get; set; }
        public string Describtion { get; set; }
        public string DocType { get; set; }

    }
    public class JvHeaderDet
    {
        public JvHead ShowHeader { get; set; }
        public List<JvManyAccountFormate> ShowTransactions { get; set; }
    }

}