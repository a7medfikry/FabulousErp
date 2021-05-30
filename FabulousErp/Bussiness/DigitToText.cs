using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FabulousErp.Bussiness
{
    public class ConvertNumberToText
    {
        string NumberAr;
        public ConvertNumberToText(string Number,string Unit,string SmallUnit)
        {
            NumberAr = ConvertNumberToAlpha(Number,  Unit,  SmallUnit);
        }
        public static string ConvertNumberToAlpha(string Number, string Unit, string SmallUnit)
        {
            Number = Number.Replace("-", "");

            bool FloatNotZero = false;
            if (Number.Contains('.'))
            {
                try
                {
                    FloatNotZero = Convert.ToInt32(Number.Split('.')[1]) == 0 ? false  : true;
                    //Number = Number.Split('.')[0];
                }
                catch
                {
                    FloatNotZero = false;
                }
            }
            if (FloatNotZero)
            {
                if (Number.Split('.')[0].ToCharArray().Length > 6)
                {
                    return "No Number";
                }
                else
                {
                    switch (Number.Split('.')[0].ToCharArray().Length)
                    {
                        case 1: return convertOneDigits(Number.Split('.')[0].ToString())+" " + Unit + " و " + convertTwoDigits(Number.Split('.')[1])+" " + SmallUnit;
                        case 2: return convertTwoDigits(Number.Split('.')[0].ToString()) + " " + Unit + " و " + convertTwoDigits(Number.Split('.')[1]) + " " + SmallUnit;
                        case 3: return convertThreeDigits(Number.Split('.')[0].ToString()) + " " + Unit + " و " + convertTwoDigits(Number.Split('.')[1]) + " " + SmallUnit;
                        case 4: return convertFourDigits(Number.Split('.')[0].ToString()) + " " + Unit + " و " + convertTwoDigits(Number.Split('.')[1]) + " " + SmallUnit;
                        case 5: return convertFiveDigits(Number.Split('.')[0].ToString()) + " " + Unit + " و " + convertTwoDigits(Number.Split('.')[1]) + " " + SmallUnit;
                        case 6: return convertSixDigits(Number.Split('.')[0].ToString()) + " " + Unit + " و " + convertTwoDigits(Number.Split('.')[1]) + " " + SmallUnit;
                        default: return "";
                    }
                }
            }
            else
            {
                Number = Number.Split('.')[0];
                if (Number.ToCharArray().Length > 6)
                {
                    return "No Number";
                }
                else
                {
                    switch (Number.ToCharArray().Length)
                    {
                        case 1: return convertOneDigits(Number.ToString())+" " + Unit;
                        case 2: return convertTwoDigits(Number.ToString()) + " " + Unit;
                        case 3: return convertThreeDigits(Number.ToString()) + " " + Unit;
                        case 4: return convertFourDigits(Number.ToString()) + " " + Unit;
                        case 5: return convertFiveDigits(Number.ToString()) + " " + Unit;
                        case 6: return convertSixDigits(Number.ToString()) + " " + Unit;
                        default: return "";
                    }

                }
            }
        }
        private static string convertTwoDigits(string TwoDigits)
        {
            string returnAlpha = "00";
           
            if (TwoDigits.ToCharArray()[0] == '0' && TwoDigits.ToCharArray()[1] != '0')
            {
                return convertOneDigits(TwoDigits.ToCharArray()[1].ToString());
            }
            else
            {
                switch (int.Parse(TwoDigits.ToCharArray()[0].ToString()))
                {
                    case 1:
                        {
                            if (int.Parse(TwoDigits.ToCharArray()[1].ToString()) == 1)
                            {
                                return "إحدى عشر";
                            }
                            else if (int.Parse(TwoDigits.ToCharArray()[1].ToString()) == 2)
                            {
                                return "إثنى عشر";
                            }
                            else
                            {
                                returnAlpha = "عشر";
                                return convertOneDigits(TwoDigits.ToCharArray()[1].ToString()) + " " + returnAlpha;
                            }
                        }
                    case 2: returnAlpha = "عشرون"; break;
                    case 3: returnAlpha = "ثلاثون"; break;
                    case 4: returnAlpha = "أريعون"; break;
                    case 5: returnAlpha = "خمسون"; break;
                    case 6: returnAlpha = "ستون"; break;
                    case 7: returnAlpha = "سبعون"; break;
                    case 8: returnAlpha = "ثمانون"; break;
                    case 9: returnAlpha = "تسعون"; break;
                    default: returnAlpha = ""; break;
                }
            }
            if (convertOneDigits(TwoDigits.ToCharArray()[1].ToString()).Length == 0)
            { return returnAlpha; }
            else
            {
                return convertOneDigits(TwoDigits.ToCharArray()[1].ToString()) + " و " + returnAlpha;
            }
        }
        private static string convertOneDigits(string OneDigits)
        {
            switch (int.Parse(OneDigits))
            {
                case 1: return "واحد";
                case 2: return "إثنان";
                case 3: return "ثلاثه";
                case 4: return "أربعه";
                case 5: return "خمسه";
                case 6: return "سته";
                case 7: return "سبعه";
                case 8: return "ثمانيه";
                case 9: return "تسعه";
                default: return "";
            }
        }
        private static string convertThreeDigits(string ThreeDigits)
        {
            switch (int.Parse(ThreeDigits.ToCharArray()[0].ToString()))
            {
                case 1:
                    {
                        if (int.Parse(ThreeDigits.ToCharArray()[1].ToString()) == 0)
                        {
                            if (int.Parse(ThreeDigits.ToCharArray()[2].ToString()) == 0)
                            {
                                return "مائه";
                            }
                            return "مائه" + " و " + convertOneDigits(ThreeDigits.ToCharArray()[2].ToString());
                        }
                        else
                        {
                            return "مائه" + " و " + convertTwoDigits(ThreeDigits.Substring(1, 2));
                        }
                    }
                case 2:
                    {
                        if (int.Parse(ThreeDigits.ToCharArray()[1].ToString()) == 0)
                        {
                            if (int.Parse(ThreeDigits.ToCharArray()[2].ToString()) == 0)
                            {
                                return "مائتين";
                            }
                            return "مائتين" + " و " + convertOneDigits(ThreeDigits.ToCharArray()[2].ToString());
                        }
                        else
                        {
                            return "مائتين" + " و " + convertTwoDigits(ThreeDigits.Substring(1, 2));
                        }
                    }
                case 3:
                    {
                        if (int.Parse(ThreeDigits.ToCharArray()[1].ToString()) == 0)
                        {
                            if (int.Parse(ThreeDigits.ToCharArray()[2].ToString()) == 0)
                            {
                                return convertOneDigits(ThreeDigits.ToCharArray()[0].ToString()).Split('ه')[0] + "مائه";
                            }
                            return convertOneDigits(ThreeDigits.ToCharArray()[0].ToString()).Split('ه')[0] + "مائه" + " و " + convertOneDigits(ThreeDigits.ToCharArray()[2].ToString());
                        }
                        else
                        {
                            return convertOneDigits(ThreeDigits.ToCharArray()[0].ToString()).Split('ه')[0] + "مائه" + " و " + convertTwoDigits(ThreeDigits.Substring(1, 2));
                        }
                    }
                case 4:
                    {
                        goto case 3;
                    }
                case 5:
                    {
                        goto case 3;
                    }
                case 6:
                    {
                        goto case 3;
                    }
                case 7:
                    {
                        goto case 3;
                    }
                case 8:
                    {
                        goto case 3;
                    }
                case 9:
                    {
                        goto case 3;
                    }
                case 0:
                    {
                        if (ThreeDigits.ToCharArray()[1] == '0')
                        {
                            if (ThreeDigits.ToCharArray()[2] == '0')
                            {
                                return "";
                            }
                            else
                            {
                                return convertOneDigits(ThreeDigits.ToCharArray()[2].ToString());
                            }
                        }
                        else
                        {
                            return convertTwoDigits(ThreeDigits.Substring(1, 2));
                        }
                    }
                default: return "";
            }
        }
        private static string convertFourDigits(string FourDigits)
        {
            switch (int.Parse(FourDigits.ToCharArray()[0].ToString()))
            {
                case 1:
                    {
                        if (int.Parse(FourDigits.ToCharArray()[1].ToString()) == 0)
                        {
                            if (int.Parse(FourDigits.ToCharArray()[2].ToString()) == 0)
                            {
                                if (int.Parse(FourDigits.ToCharArray()[3].ToString()) == 0)
                                    return "ألف";
                                else
                                {
                                    return "ألف" + " و " + convertOneDigits(FourDigits.ToCharArray()[3].ToString());
                                }
                            }
                            return "ألف" + " و " + convertTwoDigits(FourDigits.Substring(2, 2));
                        }
                        else
                        {
                            return "ألف" + " و " + convertThreeDigits(FourDigits.Substring(1, 3));
                        }
                    }
                case 2:
                    {
                        if (int.Parse(FourDigits.ToCharArray()[1].ToString()) == 0)
                        {
                            if (int.Parse(FourDigits.ToCharArray()[2].ToString()) == 0)
                            {
                                if (int.Parse(FourDigits.ToCharArray()[3].ToString()) == 0)
                                    return "ألفين";
                                else
                                {
                                    return "ألفين" + " و " + convertOneDigits(FourDigits.ToCharArray()[3].ToString());
                                }
                            }
                            return "ألفين" + " و " + convertTwoDigits(FourDigits.Substring(2, 2));
                        }
                        else
                        {
                            return "ألفين" + " و " + convertThreeDigits(FourDigits.Substring(1, 3));
                        }
                    }
                case 3:
                    {
                        if (int.Parse(FourDigits.ToCharArray()[1].ToString()) == 0)
                        {
                            if (int.Parse(FourDigits.ToCharArray()[2].ToString()) == 0)
                            {
                                if (int.Parse(FourDigits.ToCharArray()[3].ToString()) == 0)
                                    return convertOneDigits(FourDigits.ToCharArray()[0].ToString()) + " ألاف";
                                else
                                {
                                    return convertOneDigits(FourDigits.ToCharArray()[0].ToString()) + " ألاف" + " و " + convertOneDigits(FourDigits.ToCharArray()[3].ToString());
                                }
                            }
                            return convertOneDigits(FourDigits.ToCharArray()[0].ToString()) + " ألاف" + " و " + convertTwoDigits(FourDigits.Substring(2, 2));
                        }
                        else
                        {
                            return convertOneDigits(FourDigits.ToCharArray()[0].ToString()) + " ألاف" + " و " + convertThreeDigits(FourDigits.Substring(1, 3));
                        }
                    }
                case 4:
                    {
                        goto case 3;
                    }
                case 5:
                    {
                        if (int.Parse(FourDigits.ToCharArray()[1].ToString()) == 0)
                        {
                            if (int.Parse(FourDigits.ToCharArray()[2].ToString()) == 0)
                            {
                                if (int.Parse(FourDigits.ToCharArray()[3].ToString()) == 0)
                                    return convertOneDigits(FourDigits.ToCharArray()[0].ToString()) + " ألاف";
                                else
                                {
                                    return convertOneDigits(FourDigits.ToCharArray()[0].ToString()) + " ألاف " + " و " + convertOneDigits(FourDigits.ToCharArray()[3].ToString());
                                }
                            }
                            return convertOneDigits(FourDigits.ToCharArray()[0].ToString()) + " ألاف " + " و " + convertTwoDigits(FourDigits.Substring(2, 2));
                        }
                        else
                        {
                            return "ألاف" + " و " + convertThreeDigits(FourDigits.Substring(1, 3));
                        }
                    }
                case 6:
                    {
                        goto case 3;
                    }
                case 7:
                    {
                        goto case 3;
                    }
                case 8:
                    {
                        goto case 3;
                    }
                case 9:
                    {
                        goto case 3;
                    }
                default: return "";
            }
        }
        private static string convertFiveDigits(string FiveDigits)
        {
            if (convertThreeDigits(FiveDigits.Substring(2, 3)).Length == 0)
            {
                return convertTwoDigits(FiveDigits.Substring(0, 2)) + " الاف ";
            }
            else
            {
                return convertTwoDigits(FiveDigits.Substring(0, 2)) + " ألفا " + " و " + convertThreeDigits(FiveDigits.Substring(2, 3));
            }
        }
        private static string convertSixDigits(string SixDigits)
        {
            if (convertThreeDigits(SixDigits.Substring(2, 3)).Length == 0)
            {
                return convertThreeDigits(SixDigits.Substring(0, 3)) + " ألف ";
            }
            else
            {
                return convertThreeDigits(SixDigits.Substring(0, 3)) + " ألفا " + " و " + convertThreeDigits(SixDigits.Substring(3, 3));
            }
        }

        public string GetNumberAr()
        {

            return NumberAr;
        }


    }
}