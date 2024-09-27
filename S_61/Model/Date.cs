using System;
using System.Globalization;
using S_61.Basic;

namespace S_61.Model
{
    class Date
    {
        public static string GetDateTime(int i, bool showLine = false)
        {
            TaiwanCalendar tw = new TaiwanCalendar();
            switch (i)
            {
                case 1:
                    if (showLine)
                        return tw.GetYear(DateTime.Now) + "/" + DateTime.Now.ToString("MM/dd");
                    else
                        return tw.GetYear(DateTime.Now) + DateTime.Now.ToString("MMdd");
                case 2:
                    if (showLine)
                        return tw.GetYear(DateTime.Now) + 1911 + "/" + DateTime.Now.ToString("MM/dd");
                    else
                        return tw.GetYear(DateTime.Now) + 1911 + DateTime.Now.ToString("MMdd");
                default:
                    return "";
            }
        }

        public static string ChangeDateForSN(string d)
        {
            string strDate = "";
            if (d.Trim().Length == 7)
            {
                switch (Common.傳票編碼方式)
                {
                    case 1:
                        strDate = d;
                        break;
                    case 2:
                        strDate = d.Substring(0, 5) + "00";
                        break;
                }
                return strDate;
            }
            else
            {
                switch (Common.傳票編碼方式)
                {
                    case 1:
                        strDate = (Int32.Parse(d.Substring(0, 4)) - 1911) + d.Substring(4);
                        break;
                    case 2:
                        strDate = (Int32.Parse(d.Substring(0, 4)) - 1911) + d.Substring(4, 2) + "00";
                        break;
                }
                return strDate;
            }
        }

        public static string AddLine(string d)
        {
            if (d.Trim().Length == 7)
            {
                d = d.Substring(0, 3) + "/" + d.Substring(3, 2) + "/" + d.Substring(5);
                return d;
            }
            else
            {
                d = d.Substring(0, 4) + "/" + d.Substring(4, 2) + "/" + d.Substring(6);
                return d;
            }
        }

        public static string RemoveLine(string d)
        {
            return d.Replace("/", "");
        }

        public static string ToTWDate(string d)
        {
            int Year = 0;
            if (Common.User_DateTime == 2)
            {
                if (d.Trim() == "") return d;
                int.TryParse(d.Substring(0, 4), out Year);
                Year -= 1911;
                return Year + d.Substring(4);
            }
            else return d;
        }

        public static string ToUSDate(string d)
        {
            int Year = 0;
            if (Common.User_DateTime == 1)
            {
                if (d.Trim() == "") return d;
                int.TryParse(d.Substring(0, 3), out Year);
                Year += 1911;
                return Year + d.Substring(3);
            }
            else return d;
        }
    }
}
