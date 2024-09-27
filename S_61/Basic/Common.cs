using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using CrystalDecisions.Shared;
using System.Runtime.InteropServices;
using System.Text;


namespace S_61.Basic
{
    class Common
    {
        public static bool EditTime = true;
        public static Control ActiveControl;
        public static Keys keyData;

        public static int Sql_LogMod;//紀錄登入模式：1SQL驗證 2本機驗證
        public static string DatabaseName = "";

        public static FrmRegist FrmRegist;
        public static FrmDataBackUP FrmDataBackUP;
        public static FrmInvoSet FrmInvoSet;
        public static string CompanyName;
        public static string name1;
        public static string name2;
        public static string Series;
        public static string Vers;
        public static string Regist;
        public static System.Timers.Timer ckTime;
        public static string sqlConnString = "";
        public static string 浮動連線字串 = "";
        public static DataTable dtUsSettings = new DataTable();
        public static DataTable dtSysSettings = new DataTable();
        public static List<DataRow> listSysSettings = new List<DataRow>();
        public static List<DataRow> listUsSettings = new List<DataRow>();


        public static DataTable dtEnd = new DataTable();//報表尾
        public static DataTable dtstart = new DataTable();//報表頭
        public static Report.Frmreport FrmReport;
        public static string reportaddress = "..\\..\\";//報表位置
        public static TableLogOnInfo logOnInfo = new TableLogOnInfo();
        public static decimal InvoMode = 2;






        //系統參數
        public static string CoNo;//總公司編號
        public static string 系統西元;
        public static string 系統民國;
        public static int User_DateTime;//日期格式：1民國.2西元.
        public static int 傳票編碼方式;//1民國年月日+流水號 2民國年月+流水號
        public static int 金額小數;
        public static decimal chkline;
        public static decimal chkdis;
        public static string User_Name;
        public static bool 是否顯示千分位;
        public static int 自動產生匯率 ;//1自動產生 2不自動產生

        //使用者參數設定
        public static string 使用者簡稱;
        public static string 使用者預設公司;
        public static string 單據異動;// 1所有公司 2預設公司


        //系統參數
        public static int iFirst = 10;
        public static int nFirst = 12;//數值欄位『整數位數』
        public static int MS = 0;//銷貨單價小數
        public static int MST = 0;//銷貨單據小數
        public static int TS = 0;//銷項稅額小數
        public static int M = 0;//本幣金額小數

        public static int MF = 0;//進貨單價小數
        public static int MFT = 0;//進貨單據小數
        public static int TF = 0;//進項稅額小數
        public static int Q = 0;//庫存數量小數
        public static decimal Sys_Rate = 0.05M;//稅率

        //public static void 取得浮動連線字串()
        //{
        //    try
        //    {
        //        using (SqlConnection cn = new SqlConnection(Common.sqlConnString))
        //        {
        //            cn.Open();
        //            SqlCommand cmd = cn.CreateCommand();
        //            cmd.CommandText = "select copaths from comp where cono='" + Common.使用者預設公司 + "'";
        //            if (!cmd.ExecuteScalar().IsNullOrEmpty())
        //                浮動連線字串 = Common.sqlConnString.Replace("Initial Catalog=" + Common.logOnInfo.ConnectionInfo.DatabaseName, "Initial Catalog=" + cmd.ExecuteScalar().ToString().Trim());
        //            else
        //                浮動連線字串 = Common.sqlConnString;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.ToString());
        //    }
        //}
        public static void 取得浮動連線字串(string cono)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(Common.sqlConnString))
                {
                    cn.Open();
                    SqlCommand cmd = cn.CreateCommand();
                    cmd.CommandText = "select copaths from comp where cono='" + cono + "'";
                    if (!cmd.ExecuteScalar().IsNullOrEmpty())
                        浮動連線字串 = Common.sqlConnString.Replace("Initial Catalog=" + Common.logOnInfo.ConnectionInfo.DatabaseName, "Initial Catalog=" + cmd.ExecuteScalar().ToString().Trim());
                    else
                        浮動連線字串 = Common.sqlConnString;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }


        public static int ToInt32(object obj)
        {
            return (int)obj.ToDecimal();
        }
        public static bool ToBool(object obj)
        {
            bool flag = false;
            if (!obj.IsNullOrEmpty())
            {
                if (obj.ToString() == "1")
                {
                    flag = true;
                }
            }
            return flag;
        }
        public static int 判斷KeyCode(Keys e)
        {
            int 字元 = 0;
            List<Keys> 數字 = new List<Keys>();
            數字.Add(Keys.D0); 數字.Add(Keys.D1); 數字.Add(Keys.D2); 數字.Add(Keys.D3); 數字.Add(Keys.D4); 數字.Add(Keys.D5); 數字.Add(Keys.D6); 數字.Add(Keys.D7); 數字.Add(Keys.D8); 數字.Add(Keys.D9); 數字.Add(Keys.NumPad0); 數字.Add(Keys.NumPad1); 數字.Add(Keys.NumPad2); 數字.Add(Keys.NumPad3); 數字.Add(Keys.NumPad4); 數字.Add(Keys.NumPad5); 數字.Add(Keys.NumPad6); 數字.Add(Keys.NumPad7); 數字.Add(Keys.NumPad8); 數字.Add(Keys.NumPad9);
            List<Keys> 英文 = new List<Keys>();
            英文.Add(Keys.A); 英文.Add(Keys.B); 英文.Add(Keys.C); 英文.Add(Keys.D); 英文.Add(Keys.E); 英文.Add(Keys.F); 英文.Add(Keys.G); 英文.Add(Keys.H); 英文.Add(Keys.I); 英文.Add(Keys.J); 英文.Add(Keys.K); 英文.Add(Keys.L); 英文.Add(Keys.M); 英文.Add(Keys.N); 英文.Add(Keys.O); 英文.Add(Keys.P); 英文.Add(Keys.Q); 英文.Add(Keys.R); 英文.Add(Keys.S); 英文.Add(Keys.T); 英文.Add(Keys.U); 英文.Add(Keys.V); 英文.Add(Keys.W); 英文.Add(Keys.X); 英文.Add(Keys.Y); 英文.Add(Keys.Z);
            List<Keys> 其他字元 = new List<Keys>();
            其他字元.Add((Keys)106); 其他字元.Add((Keys)109); 其他字元.Add((Keys)110); 其他字元.Add((Keys)111); 其他字元.Add((Keys)186); 其他字元.Add((Keys)187); 其他字元.Add((Keys)188); 其他字元.Add((Keys)189); 其他字元.Add((Keys)190); 其他字元.Add((Keys)191); 其他字元.Add((Keys)192); 其他字元.Add((Keys)219); 其他字元.Add((Keys)220); 其他字元.Add((Keys)221); 其他字元.Add((Keys)222);
            foreach (Keys Key值 in 數字)
                if (Key值 == e) 字元 = 1;
            foreach (Keys Key值 in 英文)
                if (Key值 == e) 字元 = 2;
            foreach (Keys Key值 in 其他字元)
                if (Key值 == e) 字元 = 3;
            return 字元;
        }

        //檢查程式版本與資料庫版本是否同步
        public static bool CheckVer(string vers)
        {
            using (SqlConnection conn = new SqlConnection(Common.sqlConnString))
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "select vers from chksys";
                    Common.Vers = cmd.ExecuteScalar().ToDecimal().ToString();
                    if (cmd.ExecuteScalar().ToDecimal() == vers.ToDecimal()) return true;
                }
            }
            return false;
        }

        public static bool CheckWKS()
        {
            decimal 工作站數 = 0;
            decimal 工作站台數 = 0;
            try
            {
                using (SqlConnection conn = new SqlConnection(Common.sqlConnString))
                {
                    Security Sec = new Security();
                    DataTable dtsys = new DataTable();
                    DataTable dtmac = new DataTable();
                    DataTable dtcount = new DataTable();

                    string CT = "";
                    string uomgp = "0";
                    string sql = "select * from chksys";

                    using (SqlDataAdapter da = new SqlDataAdapter(sql, conn))
                    {
                        da.Fill(dtsys);
                        if (dtsys.Rows.Count > 0)
                        {
                            CT = dtsys.Rows[0]["CT"].ToString();//註冊時間
                            uomgp = dtsys.Rows[0]["uomgp"].ToString();//台數
                        }
                        else
                            return false;
                    }
                    ////判斷資料庫是否移動過
                    sql = "select CONVERT(nvarchar(30),create_date,114) CT FROM sys.databases where name ='" + Common.DatabaseName + "'";
                    using (SqlDataAdapter da = new SqlDataAdapter(sql, conn))
                    {
                        da.Fill(dtmac);
                        if (dtmac.Rows.Count > 0)
                            if (dtmac.Rows[0]["CT"].ToString().Trim().Replace(":", "") != CT) return false;
                    }

                    //取得目前工作站數
                    sql = " select hostprocess 主機,net_address 位址 from master..sysprocesses where program_name = 'JBS_C' ";
                    using (SqlDataAdapter da = new SqlDataAdapter(sql, conn))
                    {
                        da.Fill(dtcount);
                    }
                    DataTable temp = dtcount.Copy();
                    temp.Clear();
                    for (int i = 0; i < dtcount.Rows.Count; i++)
                    {
                        var row = temp.AsEnumerable().Where(r => r["主機"].ToString() == dtcount.Rows[i]["主機"].ToString() && r["位址"].ToString() == dtcount.Rows[i]["位址"].ToString());
                        if (row.Count() == 0)
                            temp.ImportRow(dtcount.Rows[i]);
                    }
                    temp.AcceptChanges();
                    工作站數 = temp.Rows.Count.ToDecimal();

                    string 工作站台數temp = "";
                    工作站台數temp = Sec.數字解密(uomgp, CT + "0");

                    if (工作站台數temp.Length != 10) return false;
                    if (工作站台數temp[(int)工作站台數temp[0].ToDecimal() + 2] != '7'
                        || 工作站台數temp[(int)工作站台數temp[0].ToDecimal() + 3] != '8') return false;

                    工作站台數 = (工作站台數temp.Substring((int)工作站台數temp[0].ToDecimal(), 2)).ToDecimal();
                }
                return (工作站台數 >= 工作站數);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return false;
            }
        }

        public static string GetSeries()
        {
            string[] temp = new string[] { "71", "72", "73", "74" };
            Security Sec = new Security();
            string ser = "74";
            try
            {
                DataTable t = new DataTable();
                using (SqlConnection cn = new SqlConnection(Common.sqlConnString))
                {
                    string sql = "select * from chksys where usrno=N'0001'";
                    using (SqlDataAdapter da = new SqlDataAdapter(sql, cn))
                    {
                        t.Clear();
                        da.Fill(t);
                    }
                }
                var vera = t.Rows[0]["vera"].ToString().Trim();
                if (Common.Regist == "[教育版]")
                {
                    if (vera.Length > 0)
                    {
                        //有註冊
                        var ct = t.Rows[0]["CT"].ToString().Trim() + "0";
                        ser = Sec.數字解密(vera, ct);
                        if (ser.Length != 10)
                        {
                            ser = "74";
                            return ser;
                        }
                        if (ser.ToDecimal() == 0)
                        {
                            ser = "74";
                            return ser;
                        }
                        ser = ser[(int)ser[0].ToDecimal()].ToString() + ser[(int)ser[0].ToDecimal() + 1].ToString();
                        if (temp.Count(r => r.ToString() == ser) == 0) ser = "74";
                    }
                    else
                    {
                        //讀設定檔
                        ser = GetPrivateProfileString("JS設定", "Series");
                        if (temp.Count(r => r.ToString() == ser) == 0) ser = "74";
                    }
                }
                else
                {
                    //正式版亂改版
                    var ct = t.Rows[0]["CT"].ToString().Trim() + "0";
                    ser = Sec.數字解密(vera, ct);
                    if (ser.Length != 10)
                    {
                        ser = "74";
                        return ser;
                    }
                    if (ser.ToDecimal() == 0)
                    {
                        ser = "74";
                        return ser;
                    }
                    ser = ser[(int)ser[0].ToDecimal()].ToString() + ser[(int)ser[0].ToDecimal() + 1].ToString();
                    if (temp.Count(r => r.ToString() == ser) == 0) ser = "74";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            return ser;
        }

        public static bool IsOverRegist(string TTableName)
        {
            if (Common.Regist.Contains("正式版")) return true;
            else
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(Common.sqlConnString))
                    {
                        conn.Open();
                        using (SqlCommand cmd = conn.CreateCommand())
                        {
                            cmd.CommandText = "select COUNT(*) from " + TTableName;
                            if (cmd.ExecuteScalar().ToDecimal() < 50) return true;
                        }
                    }
                    return false;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                    return false;
                }
            }
        }


        static string startUpIniFileName = null;
        public static string StartUpIniFileName
        {
            get
            {
                if (string.IsNullOrEmpty(startUpIniFileName))
                {
                    startUpIniFileName = Application.StartupPath + "\\setup.ini";
                }
                return startUpIniFileName;
            }
        }

        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string value, string filePath);

        public static string GetPrivateProfileString(string section, string key)
        {
            int nCapacity = 255;
            StringBuilder temp = new StringBuilder(nCapacity);
            int i = GetPrivateProfileString(section, key, "", temp, nCapacity, StartUpIniFileName);
            if (i < 0)
                return "";
            return temp.ToString();
        }

        public static long WritePrivateProfileString(string section, string key, string value)
        {
            if (section.Trim().Length <= 0 || key.Trim().Length <= 0 || value.Trim().Length <= 0)
                return 0;

            return WritePrivateProfileString(section, key, value, StartUpIniFileName);
        }

        public static DataTable ActionTB = new DataTable();
        public static string ActionSql = "";
        public static DataRow load(string Action, string TBName, string Pk, string str = null)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(Common.sqlConnString))
                {
                    ActionTB.Dispose();
                    ActionSql = "";
                    switch (Action)
                    {
                        case "Top":
                            ActionSql = "select top(1)* from " + TBName + " order by " + Pk + "";
                            break;
                        case "Prior":
                            ActionSql = "select top(1)* from " + TBName + " where " + Pk + " < @str order by " + Pk + " desc";
                            break;
                        case "Next":
                            ActionSql = "select top(1)* from " + TBName + " where " + Pk + " > @str order by " + Pk + "";
                            break;
                        case "Bottom":
                            ActionSql = "select top(1)* from " + TBName + " order by " + Pk + " desc";
                            break;
                        case "Cancel":
                            ActionSql = "select * from " + TBName + " where " + Pk + " = @str";
                            break;

                        case "Check":
                            ActionSql = "select * from " + TBName + " where " + Pk + " = @str";
                            break;
                        case "CPrior":
                            ActionSql = "select top(1)* from " + TBName + " where " + Pk + " >= @str order by " + Pk + "";
                            break;
                        case "CNext":
                            ActionSql = "select top(1)* from " + TBName + " where " + Pk + " <= @str order by " + Pk + " desc";
                            break;
                    }
                    using (SqlCommand cmd = cn.CreateCommand())
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        cmd.Parameters.Clear();
                        if (str != null)
                        {
                            cmd.Parameters.AddWithValue("@str", str);
                        }
                        cmd.CommandText = ActionSql;
                        ActionTB = new DataTable();
                        da.Fill(ActionTB);
                    }
                }
                if (ActionTB.Rows.Count > 0)
                {
                    return ActionTB.Rows[0];
                }
                else if (ActionTB.Rows.Count == 0 && Action == "Cancel")
                {
                    return load("Bottom", TBName, Pk, str);
                }
                return null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return null;
            }
        }




























































        //計時器
        static Stopwatch sw = null;
        public static void SwStart()
        {
            sw = null;
            sw = new Stopwatch();
            sw.Start();
        }
        public static void SwStop()
        {
            if (sw != null) MessageBox.Show(sw.ElapsedMilliseconds.ToString());
        }


























































    }
}
