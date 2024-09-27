using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using S_61.Basic;
using S_61.menu;

namespace S_61
{
    public partial class MainForm : Form
    {
        public Thread tempThread;
        public static MainForm main;
        public static LoginForm FrmLogin;
        public static MenuForm FrmMenu;
        public int stripHeight = 1;

        string name1 = "票據管理系統";
        string name2 = "JC";
        //string version = "3.0";
        string version = "6.0";
        string subver = "g";

        public MainForm()
        {
            InitializeComponent();
            main = this;
            ReportAddress();
            tempThread = new Thread(new ThreadStart(BackGroundRpt));
            tempThread.Start();
            Common.name1 = name1;
            Common.name2 = name2;

            using (LoginForm loginForm = new LoginForm())
            {
                Application.DoEvents();
                loginForm.ShowDialog();
                if (!Common.sqlConnString.Contains("Application Name=JBS_C")) Common.sqlConnString += ";Application Name=JBS_C";
            }

            JS.JSStrip jsstrip = new JS.JSStrip();
            this.Controls.Add(jsstrip.menuStrip1);
            this.MainMenuStrip = jsstrip.menuStrip1;
            stripHeight = jsstrip.menuStrip1.Height;
        }

        private void Form1_Load(object sender, EventArgs e)
        {


            if (Common.dtUsSettings.Rows.Count > 0)  //登入成功
            {
                if (!Common.CheckVer(version))//版本不符
                {
                    MessageBox.Show("版本不符！" + Environment.NewLine + "資料庫  版本 V " + Common.Vers + " 版" + Environment.NewLine + "JBS系統版本 V " + version + " 版", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    this.Dispose();
                    Application.Exit();
                    return;
                }

                //是否超出工作台數
                Common.Regist = Common.CheckWKS() ? "[正式版]" : "[教育版]";
                Common.Series = Common.GetSeries();
                //載入片語/系統參數/使用者參數
                doConnection();
                loadUserSetting();
                loadSystemSetting();
                loadPhrase();
                SetTitle();
                Application.DoEvents();
                OpenMenuForm();
            }
            else
            {
                Application.Exit();
            }
        }

        public void loadPhrase()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(Common.sqlConnString))
                {
                    //讀報表尾
                    string str = "select * from tail";
                    using (SqlDataAdapter da = new SqlDataAdapter(str, conn))
                    {
                        Common.dtEnd.Clear();
                        da.Fill(Common.dtEnd);
                    }
                    //讀報表頭
                    str = "select * from comp where cono='"+Common.使用者預設公司+"'";
                    using (SqlDataAdapter da = new SqlDataAdapter(str, conn))
                    {
                        Common.dtstart.Clear();
                        da.Fill(Common.dtstart);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public void loadUserSetting()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(Common.sqlConnString))
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = "select * from scrit where scname=N'"
                            + Common.User_Name + "' COLLATE Chinese_Taiwan_Stroke_BIN";
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                Common.使用者簡稱 = reader["scname1"].ToString();
                                Common.使用者預設公司 = reader["cono"].ToString();
                                Common.單據異動 = reader["scsuchk"].ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public void loadSystemSetting()
        {
            using (SqlConnection conn = new SqlConnection(Common.sqlConnString))
            {
                //讀系統參數設定
                string str = "select * from chksys";
                using (SqlDataAdapter da = new SqlDataAdapter(str, conn))
                {
                    Common.dtSysSettings.Clear();
                    da.Fill(Common.dtSysSettings);
                    if (Common.dtSysSettings.Rows.Count > 0)
                    {
                        Common.listSysSettings.Clear();
                        Common.listSysSettings = Common.dtSysSettings.AsEnumerable().ToList();

                        Common.CoNo = Common.dtSysSettings.Rows[0]["CoNo"].ToString().Trim();
                        Common.系統民國 = Common.dtSysSettings.Rows[0]["chkyear1"].ToString().Trim();
                        Common.系統西元 = Common.dtSysSettings.Rows[0]["chkyear2"].ToString().Trim();
                        Common.User_DateTime = Convert.ToInt32(Common.dtSysSettings.Rows[0]["sysdate"].ToString());
                        Common.傳票編碼方式 = Convert.ToInt32(Common.dtSysSettings.Rows[0]["noadd"].ToString());
                        Common.金額小數 = Convert.ToInt32( Common.dtSysSettings.Rows[0]["deci"].ToDecimal());
                        Common.chkline = Common.dtSysSettings.Rows[0]["chkline"].ToDecimal();
                        Common.chkdis = Common.dtSysSettings.Rows[0]["chkdis"].ToDecimal();
                        Common.自動產生匯率 = Convert.ToInt32(Common.dtSysSettings.Rows[0]["autoget"].ToDecimal());
                        Common.是否顯示千分位 = true;

                        SetTitle();
                    }
                    else
                    {
                        Common.listSysSettings.Clear();
                    }
                }
            }
        }

        void SetTitle()
        {
            this.Text = Common.CompanyName + name1 + "[ " + name2 + Common.Series + " ][ 軟體版本:V" + version + subver + " ]" + Common.Regist;
        }

        void OpenMenuForm()
        {
            if (Basic.SetParameter.CheckMdiChild("FrmMenu"))
            {
                Basic.SetParameter.RefreshFormLocation(FrmMenu);
            }
            else
            {
                FrmMenu = new menu.MenuForm();
                FrmMenu.Name = "FrmMenu";
                FrmMenu.MdiParent = this;
                FrmMenu.Show();
            }
        }

        void ReportAddress()
        {
            if (Directory.Exists(Application.StartupPath + "\\Report"))
            {
                Common.reportaddress = "";
            }
            else
            {
                Common.reportaddress = "..\\..\\";
            }
            using (Report.Frmreport frm = new Report.Frmreport()) { }
        }

        void BackGroundRpt()
        {
            //try
            //{
            //    Common.Frmreport = new Report.Frmreport();
            //    using (Common.Frmreport.reportDocument1 = new ReportDocument())
            //    {
            //        Common.Frmreport.reportDocument1.Load(Common.reportaddress + "Report\\空報表.rpt");
            //        Common.Frmreport.crystalReportViewer1.ReportSource = Common.Frmreport.reportDocument1;

            //        if (!System.IO.Directory.Exists(Application.StartupPath + "\\temp"))
            //        {
            //            System.IO.Directory.CreateDirectory(Application.StartupPath + "\\temp");
            //        }
            //        Common.Frmreport.reportDocument1.ExportToDisk(ExportFormatType.WordForWindows, Application.StartupPath + "\\temp\\temp.doc");
            //        Common.Frmreport.reportDocument1.ExportToDisk(ExportFormatType.Excel, Application.StartupPath + "\\temp\\temp.xls");

            //        Common.Frmreport.reportDocument1.Close();
            //    }
            //    Common.Frmreport.Dispose();
            //}
            //catch
            //{
            //}
        }

        void doConnection()
        {
            Common.ckTime = new System.Timers.Timer();
            Common.ckTime.Elapsed += new System.Timers.ElapsedEventHandler(tick);
            Common.ckTime.Interval = 180000;
            Common.ckTime.Enabled = true;
        }

        public void tick(object source, System.Timers.ElapsedEventArgs e)
        {
            //唯持連線用。
            using (SqlConnection conn = new SqlConnection(Common.sqlConnString))
            {
                conn.Open();
                conn.Close();
            }
            //POS傳輸用/每小時一次
            //if (DateTime.Now.Minute <= 3 && Common.Sys_StockKind == 2)
            //if (DateTime.Now.Minute > 0 && Common.Sys_StockKind == 2)
            //{
            //    Thread t = new Thread(new ThreadStart(JS.transmission.transCust));
            //    t.Start();
            //}
        }

        void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Common.ckTime != null)
            {
                Common.ckTime.Enabled = false;
                Common.ckTime.Dispose();
            }
        }

        const int HTCAPTION = 2;
        const int HTSYSMENU = 3;
        const int WM_NCMOUSEMOVE = 0x00A0;
        const int WM_NCLBUTTONDOWN = 0x00A1;
        const int WM_NCLBUTTONUP = 0x00A2;
        const int WM_NCLBUTTONDBLCLK = 0x00A3;
        const int WM_NCRBUTTONDOWN = 0x00A4;
        const int WM_NCRBUTTONUP = 0x00A5;
        const int WM_NCRBUTTONDBLCLK = 0x00A6;
        const int WM_NCMBUTTONDOWN = 0x00A7;
        const int WM_NCMBUTTONUP = 0x00A8;
        const int WM_NCMBUTTONDBLCLK = 0x00A9;

        protected override void WndProc(ref Message m)
        {
            try
            {
                if (m.Msg == WM_NCLBUTTONDBLCLK)//左雙擊
            {
                m.WParam = System.IntPtr.Zero;
                return;
            }
            else if (m.Msg == WM_NCRBUTTONDOWN)//右選單
            {
                m.WParam = System.IntPtr.Zero;
                return;
            }
            else if (m.Msg == WM_NCLBUTTONDOWN && m.WParam.ToInt32() == HTCAPTION)//左拖曳
            {
                return;
            }
            else if (m.Msg == WM_NCLBUTTONDOWN && m.WParam.ToInt32() == HTSYSMENU)//左選單
            {
                return;
            }
            else if (m.WParam.ToInt32() == 9)
            {
                return;
            }
            base.WndProc(ref m);
            }
            catch 
            {
            }
            
        }
    }
}
