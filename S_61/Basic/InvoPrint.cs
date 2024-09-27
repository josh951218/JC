using System;
using System.Data;
using System.Data.SqlClient;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace S_61.Basic
{
    public class InvoPrint
    {
        DataTable dtTemp = new DataTable();
        RichTextBox TextBox1 = new RichTextBox();
        RichTextBox Detail1 = new RichTextBox();
        RichTextBox Detail2 = new RichTextBox();
        RichTextBox TextBox2 = new RichTextBox();

        SerialPort sp = new SerialPort();
        string 初始化 = ((char)27).ToString() + "@";
        string LeftOnly = ((char)27).ToString() + ((char)99).ToString() + ((char)48).ToString() + ((char)2).ToString();
        string LeftRedStop = ((char)27).ToString() + ((char)99).ToString() + ((char)52).ToString() + ((char)2).ToString();
        string MoveToFront = ((char)13).ToString();
        string 換頁切斷 = ((char)12).ToString();
        string 開錢櫃 = ((char)27).ToString() + "p0" + ((char)50).ToString() + ((char)250).ToString();
        string 跳行 = "";

        int Recorded = 0;
        int PerPage = 0;//明細幾筆
        int PageCode = 0;//頁碼

        //呼叫打印發票時，必要傳入參數
        public DataTable dt = new DataTable();//列印明細
        public decimal Cash = 0;              //現金
        public decimal Change = 0;            //找零  
        public string Machine = "";           //機台號碼
        public string InvNoS = "";            //起始發票號碼
        public string InvNoE = "";            //終止發票號碼
        public bool IsTestPrint = false;      //是否為測試列印

        string itno = "";
        string itname = "";
        decimal qty = 0;
        decimal price = 0;
        decimal taxprice = 0;
        decimal mny = 0;
        decimal total = 0;

        string tempStr;
        string format;
        int len = 0;
        int index = 0;


        public InvoPrint()
        {
            sp.BaudRate = 9600;
            sp.Parity = Parity.None;
            sp.DataBits = 8;
            sp.StopBits = StopBits.One;
            sp.Encoding = Encoding.GetEncoding(950);
            LoadDB();
        }

        void LoadDB()
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(Common.sqlConnString))
                {
                    string sql = "select * from invoiceset where InNo=N'N1'";
                    using (SqlDataAdapter da = new SqlDataAdapter(sql, cn))
                    {
                        dtTemp.Clear();
                        da.Fill(dtTemp);
                    }
                }
                if (dtTemp.Rows.Count > 0)
                {
                    TextBox1.Clear();
                    Detail1.Clear();
                    Detail2.Clear();
                    TextBox2.Clear();

                    TextBox1.Text = dtTemp.Rows[0]["InHead"].ToString();
                    Detail1.Text = dtTemp.Rows[0]["InDital1"].ToString();
                    Detail2.Text = dtTemp.Rows[0]["InDital21"].ToString();
                    TextBox2.Text = dtTemp.Rows[0]["InFoot"].ToString();
                    PerPage = (int)dtTemp.Rows[0]["inditalnum"].ToDecimal();
                    int j = (int)dtTemp.Rows[0]["jump"].ToDecimal();
                    跳行 = ((char)27).ToString() + "d" + ((char)j).ToString();
                }
                else
                {
                    MessageBox.Show("發票設定檔錯誤！");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public void doPrint()
        {
            //try
            //{
            //    if (Common.User_ScInvDev == 2)
            //    {
            //        MessageBox.Show("發票機埠未設定！");
            //        return;
            //    }
            //    else
            //    {
            //        sp.PortName = Common.User_ScInvDev == 1 ? "COM1" : "COM2";
            //    }

            //    if (sp.IsOpen) sp.Close();

            //    sp.Open();
            //    sp.Write(初始化);
            //    sp.Write(LeftOnly);
            //    sp.Write(LeftRedStop);
            //    sp.Write(開錢櫃);

            //    Recorded = 0;
            //    total = 0;
            //    for (int i = 0; i < dt.Rows.Count; i++)
            //    {
            //        //抬頭
            //        if (Recorded % PerPage == 0)
            //        {
            //            PrintTitle();
            //            AddInvNo();
            //        }
            //        //明細
            //        PrintDetail(dt.Rows[i]);
            //        //註腳
            //        if (Recorded >= PerPage)
            //        {
            //            //換頁
            //            if (i == dt.Rows.Count - 1)
            //            {
            //                PrintFloot();
            //                sp.Write(換頁切斷);
            //                break;
            //            }
            //            else
            //            {
            //                sp.Write(換頁切斷);
            //                sp.Write(MoveToFront);
            //            }
            //            Recorded = 0;
            //        }

            //        if (i == (dt.Rows.Count - 1))
            //        {
            //            PrintFloot();
            //            sp.Write(換頁切斷);
            //            break;
            //        }
            //    }
            //    sp.Close();
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.ToString());
            //}
        }

        void PrintTitle()
        {
            PageCode++;
            sp.Write(跳行);
            for (int i = 0; i < TextBox1.Lines.Length; i++)
            {
                tempStr = TextBox1.Lines[i].Trim();
                if (tempStr.Length == 0)
                {
                    sp.WriteLine(sp.NewLine);
                }
                tempStr = CheckFunction(tempStr);
                sp.WriteLine(tempStr);
            }
        }

        void PrintDetail(DataRow row)
        {
            itno = row["itno"].ToString();
            itname = row["itname"].ToString();
            qty = row["qty"].ToDecimal();
            price = row["price"].ToDecimal();
            taxprice = row["taxprice"].ToDecimal();

            string p = price.ToString("f0");
            string q = qty.ToString("f0");
            mny = p.ToDecimal() * q.ToDecimal();
            total += mny;

            //mny = row["mny"].ToDecimal();
            //total += row["totmny"].ToDecimal();

            if (qty == 1)
            {
                for (int i = 0; i < Detail1.Lines.Length; i++)
                {
                    tempStr = Detail1.Lines[i].Trim();
                    if (tempStr.Length == 0)
                    {
                        sp.WriteLine(sp.NewLine);
                    }
                    tempStr = CheckFunction(tempStr);
                    sp.WriteLine(tempStr);
                }
                Recorded += (Detail1.Lines.Length);
            }
            else
            {
                for (int i = 0; i < Detail2.Lines.Length; i++)
                {
                    tempStr = Detail2.Lines[i].Trim();
                    if (tempStr.Length == 0)
                    {
                        sp.WriteLine(sp.NewLine);
                    }
                    tempStr = CheckFunction(tempStr);
                    sp.WriteLine(tempStr);
                }
                Recorded += (Detail2.Lines.Length);
            }
        }

        void PrintFloot()
        {
            for (int i = 0; i < TextBox2.Lines.Length; i++)
            {
                tempStr = TextBox2.Lines[i].Trim();
                if (tempStr.Length == 0)
                {
                    sp.WriteLine(sp.NewLine);
                }
                tempStr = CheckFunction(tempStr);
                sp.WriteLine(tempStr);
            }
        }

        string CheckFunction(string line)
        {
            if (line.Contains("Machine()"))
            {
                line = line.Replace("Machine()", Machine.Trim());
            }
            if (line.Contains("Page()"))
            {
                line = line.Replace("Page()", PageCode.ToString());
            }
            if (line.Contains("GetDateTime(17)"))
            {
                if (Common.User_DateTime == 1)
                    line = line.Replace("GetDateTime(17)", DateTime.Now.ToString("yyyyMMdd  HH:mm:ss"));
                else
                    line = line.Replace("GetDateTime(17)", DateTime.Now.ToString("yyyyMMdd HH:mm:ss"));
            }
            if (line.Contains("ItNo("))
            {
                format = "ItNo(";
                len = GetLen(format, line);
                int i = Encoding.GetEncoding(950).GetByteCount(itno);
                format = itno.PadRight(24, ' ');
                line = line.Replace("ItNo(" + len + ")", format.GetUTF8(len));
            }
            if (line.Contains("ItName("))
            {
                format = "ItName(";
                len = GetLen(format, line);
                int i = Encoding.GetEncoding(950).GetByteCount(itname);
                format = itname.PadRight(24, ' ');
                line = line.Replace("ItName(" + len + ")", format.GetUTF8(len));
            }
            if (line.Contains("TaxPrice("))
            {
                format = "TaxPrice(";
                len = GetLen(format, line);
                format = string.Format("{0," + len + "}", "$" + taxprice.ToString("f0"));
                line = line.Replace("TaxPrice(" + len + ")", format);
            }
            if (line.Contains("Price("))
            {
                format = "Price(";
                len = GetLen(format, line);
                format = string.Format("{0," + len + "}", "$" + price.ToString("f0"));
                line = line.Replace("Price(" + len + ")", format);
            }
            if (line.Contains("Mny("))
            {
                format = "Mny(";
                len = GetLen(format, line);
                format = string.Format("{0," + len + "}", "$" + mny.ToString("f0"));
                line = line.Replace("Mny(" + len + ")", format);
            }
            if (line.Contains("Qty("))
            {
                format = "Qty(";
                len = GetLen(format, line);
                format = string.Format("{0," + len + "}", qty.ToString("f0"));
                line = line.Replace("Qty(" + len + ")", format);
            }
            if (line.Contains("GetCount("))
            {
                format = "GetCount(";
                len = GetLen(format, line);
                format = string.Format("{0," + len + "}", dt.Rows.Count.ToString());
                line = line.Replace("GetCount(" + len + ")", format);
            }
            if (line.Contains("GetTotal("))
            {
                format = "GetTotal(";
                len = GetLen(format, line);
                format = string.Format("{0," + len + "}", "$" + total.ToString("f0"));
                line = line.Replace("GetTotal(" + len + ")", format);
            }
            if (line.Contains("GetCash("))
            {
                format = "GetCash(";
                len = GetLen(format, line);
                format = string.Format("{0," + len + "}", "$" + Cash.ToString("f0"));
                line = line.Replace("GetCash(" + len + ")", format);
            }
            if (line.Contains("GetChange("))
            {
                format = "GetChange(";
                len = GetLen(format, line);
                format = string.Format("{0," + len + "}", "$" + Change.ToString("f0"));
                line = line.Replace("GetChange(" + len + ")", format);
            }
        flag:
            if (line.Contains("Space("))
            {
                format = "Space(";
                len = GetLen(format, line);
                format = string.Format("{0," + len + "}", "");
                line = line.Replace("Space(" + len + ")", format);
                if (line.Contains("Space(")) goto flag;
            }
            if (line.Contains("Line(二聯)"))
            {
                line = line.Replace("Line(二聯)", "------------------------");
            }
            if (line.Contains("Line(三聯)"))
            {
                line = line.Replace("Line(三聯)", "--------------------------------");
            }
            return line;
        }

        int GetLen(string s, string l)
        {
            index = l.IndexOf(s);
            l = new string(l.Skip(index + s.Length).ToArray());
            index = l.IndexOf(")");
            l = new string(l.Take(index).ToArray());
            len = 0;
            int.TryParse(l, out len);
            return len;
        }

        void AddInvNo()
        {
            //if (IsTestPrint) return;
            //try
            //{
            //    using (SqlConnection cn = new SqlConnection(Common.sqlConnString))
            //    {
            //        DataTable t = new DataTable();
            //        string sql = " select * from scrit where ScName=N'" + Common.User_Name + "'";
            //        using (SqlDataAdapter da = new SqlDataAdapter(sql, cn))
            //        {
            //            da.Fill(t);
            //            SqlCommandBuilder builder = new SqlCommandBuilder(da);
            //            da.UpdateCommand = builder.GetUpdateCommand();

            //            if (Common.User_X5No.Trim().ToDecimal() == 3)
            //            {
            //                var p = InvNoS.skipString(2).ToDecimal() + 1;
            //                var s = InvNoS.takeString(2) + p.ToString("f0");
            //                if (s.BigThen(InvNoE)) return;
            //                else
            //                {
            //                    t.AcceptChanges();
            //                    t.Rows[0]["ScInvoicA"] = s;
            //                    da.Update(t);
            //                    InvNoS = s;
            //                }
            //            }
            //            else if (Common.User_X5No.Trim().ToDecimal() == 4)
            //            {
            //                var p = InvNoS.skipString(2).ToDecimal() + 1;
            //                var s = InvNoS.takeString(2) + p.ToString("f0");
            //                if (s.BigThen(InvNoE)) return;
            //                else
            //                {
            //                    t.AcceptChanges();
            //                    t.Rows[0]["ScInvoicA3"] = s;
            //                    da.Update(t);
            //                    InvNoS = s;
            //                }
            //            }
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.ToString());
            //}
        }


































    }
}
