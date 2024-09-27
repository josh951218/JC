using System;
using System.Linq;
using System.Text;

namespace S_61.Basic
{
    public class Security
    {
        public bool DecHexStr(string autostr, string code)//解密
        {
            return (DoHex(autostr)) == code ? true : false;
        }

        public string DoHex(string str)//加密
        {
            var p = Math.Abs(str.GetHashCode()).ToString();
            return GetUTF8(p, str.Length);
        }
        public static string GetUTF8(string str, int Length)
        {
            if (Length < 0) return "";
            int len = 0;
            string s = "";
            for (int i = 1; i <= str.Length; i++)
            {
                s = new string(str.Take(i).ToArray());
                if (Encoding.GetEncoding(950).GetByteCount(s) > Length)
                {
                    break;
                }
                else if (Encoding.GetEncoding(950).GetByteCount(s) == Length)
                {
                    return s;
                }
                else
                {
                    len = i;
                }
            }
            s = new string(str.Take(len).ToArray());
            return s;
        }
        public string UomgpToX(string str)
        {
            string X = "";
            string ABC = "HA9B8C7D6E";
            for (int i = 0; i < str.Length; i++)
            {
                int j;
                int.TryParse(str[i].ToString(), out j);
                X += ABC.ElementAt(j);
            }
            Random rd = new Random();
            return rd.Next(11, 99) + X;
        }

        public string 加密(string str, char 特徵位移 = (char)0)
        {
            string X = "";
            string 特徵 = "8257910346";
            string ABC = "";
            for (int i = 0; i < 特徵.Length; i++)
            {
                int 字元 = (((int)特徵[i] + (int)特徵位移) % 10) + 48;
                ABC += ((char)字元).ToString();
            }
            for (int i = 0; i < str.Length; i++)
            {
                int j;
                int.TryParse(str[i].ToString(), out j);
                X += ABC.ElementAt(j);
            }
            Random rd = new Random();
            return X;
        }
        public string 解密(string str, char 特徵位移 = (char)0)
        {
            string X = "";
            string 特徵 = "8257910346";
            string ABC = "";
            for (int i = 0; i < 特徵.Length; i++)
            {
                int 字元 = (((int)特徵[i] + (int)特徵位移) % 10) + 48;
                ABC += ((char)字元).ToString();
            }
            for (int i = 0; i < str.Length; i++)
            {
                int j = ABC.IndexOf(str[i]);
                X += j.ToString();
            }
            return X;
        }

        public decimal XToUomgp(string str)
        {
            string X = "";
            string ABC = "HA9B8C7D6E";
            string xS = new string(str.Skip(2).ToArray());
            for (int i = 0; i < xS.Length; i++)
            {
                int j = ABC.IndexOf(xS[i]);
                X += j.ToString();
            }
            return X.ToDecimal();
        }

        public string 數字加密(string 字串, string 特徵)
        {
            string 編碼key = "7815940236";
            string 處理過的特徵 = "";
            string 結果 = "";

            //讓字串和特徵長度相同
            if (特徵.Length < 字串.Length) 特徵.PadRight((char)0);
            特徵 = 特徵.Substring(0, 字串.Length);

            foreach (char c in 特徵)
            {
                處理過的特徵 += (char)((int)c % 10);
            }
            for (int i = 0; i < 字串.Length; i++)
            {
                char 編碼 = 編碼key.IndexOf(字串[i]).ToString()[0];
                char 編碼_加上特徵 = (char)(((int)編碼 - 48) + (int)處理過的特徵[i]);
                char 編碼_加上特徵_處理 = (char)(編碼_加上特徵 % 10 + 48);
                結果 += 編碼_加上特徵_處理;
            }
            return new string(結果.ToCharArray().Reverse().ToArray());
        }
        public string 數字解密(string 字串, string 特徵)
        {
            string 編碼key = "7815940236";
            string 處理過的特徵 = "";
            string 結果 = "";
            字串 = new string(字串.ToCharArray().Reverse().ToArray());
            //讓字串和特徵長度相同
            if (特徵.Length < 字串.Length) 特徵.PadRight((char)0);
            特徵 = 特徵.Substring(0, 字串.Length);

            foreach (char c in 特徵)
            {
                處理過的特徵 += (char)((int)c % 10);
            }
            for (int i = 0; i < 字串.Length; i++)
            {
                char 編碼_加上特徵_處理 = (char)((int)字串[i] - 48);
                if (編碼_加上特徵_處理 - (int)處理過的特徵[i] < 0) 編碼_加上特徵_處理 = (char)((int)編碼_加上特徵_處理 + 10);
                char 編碼_加上特徵 = (char)(((int)編碼_加上特徵_處理 + 48) - (int)處理過的特徵[i]);
                char 編碼 = 編碼key[編碼_加上特徵 - 48];
                結果 += 編碼;
            }
            return 結果;
        }
        public static string 數字加密為英數(string 字串, int 長度 = 8)
        {
            long 數字 = long.Parse(字串);
            string 加密字串 = "";
            while (數字 != 0)
            {
                long 當前值 = 數字 % 36;
                if (當前值 < 10) 加密字串 += (char)(當前值 + 48);
                else 加密字串 += (char)(當前值 + 55);
                數字 = 數字 / 36;
            }
            while (加密字串.Length < 長度)
            {
                char 最小值 = 加密字串.ToCharArray().Min();
                char 最大值 = 加密字串.ToCharArray().Max();
                int 插入的字符_1 = (int)最小值 + ((int)最大值 - (int)最小值) / (長度 - 加密字串.Length);
                if (插入的字符_1 < 65 && 插入的字符_1 > 48) 插入的字符_1 += 8;
                char 插入的字符_2 = (char)插入的字符_1;
                加密字串 = 加密字串.Insert(加密字串.IndexOf(最大值), 插入的字符_2.ToString());
            }
            return 加密字串;
        }


















    }
}