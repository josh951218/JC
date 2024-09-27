using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using System.Xml.Linq;
using S_61.Basic;

namespace S_61.Basic
{
    public partial class LoginForm : Form
    {
        public bool tryConnOK = false;
        delegate void myDelegate();
        Thread myThread, Thread2;
        System.Timers.Timer t = null;

        public LoginForm()
        {
            InitializeComponent();
            MainForm.FrmLogin = this;

            ScName.Clear();
            ScPass.Clear();
            //ScName.Text = "BM";
            //ScPass.Text = "BM";
        }

        //第一步，按enter登入        // 1 未設定連線就登入        // 2 曾經設定連線登入
        void UserLogin()
        {
            if (tryConnOK)
            {
                try
                {
                    if (!Common.sqlConnString.Contains("Application Name=JBS_C")) Common.sqlConnString += ";Application Name=JBS_C";

                    using (SqlConnection conn = new SqlConnection(Common.sqlConnString))
                    {
                        string str = "select * from scrit where scname=@ID COLLATE Chinese_Taiwan_Stroke_BIN and scpass=@PW COLLATE Chinese_Taiwan_Stroke_BIN";
                        SqlCommand cmd = new SqlCommand(str, conn);
                        cmd.Parameters.AddWithValue("@ID", ScName.Text);
                        cmd.Parameters.AddWithValue("@PW", ScPass.Text);
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(Common.dtUsSettings);
                        da.Dispose();
                        cmd.Dispose();
                    }
                    if (Common.dtUsSettings.Rows.Count > 0)
                    {
                        Common.User_Name = Common.dtUsSettings.Rows[0]["scname"].ToString();
                        Common.listUsSettings = Common.dtUsSettings.AsEnumerable().ToList();
                        SaveLogonInfo();
                        //登入成功，關窗
                        Invoke(new myDelegate(closeForm));
                    }
                    else
                    {
                        MessageBox.Show("沒有此用戶", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                catch (Exception ex) { MessageBox.Show(ex.ToString()); }
            }
            else
            {
                //未設定連線，直接登入
                if (ScName.Text.Trim() == "" | ScPass.Text.Trim() == "")
                {
                    t.Enabled = false;
                    t.Dispose();
                    MessageBox.Show("請先輸入『帳號』、『密碼』", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                try
                {
                    string sql = ConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
                    if (!sql.Contains("Application Name=JBS_C")) sql += ";Application Name=JBS_C";

                    using (SqlConnection conn = new SqlConnection(sql))
                    {
                        conn.Open();
                        Common.sqlConnString = sql;
                        string str = "select * from scrit where scname=@ID COLLATE Chinese_Taiwan_Stroke_BIN and scpass=@PW COLLATE Chinese_Taiwan_Stroke_BIN";
                        SqlCommand cmd = new SqlCommand(str, conn);
                        cmd.Parameters.AddWithValue("@ID", ScName.Text);
                        cmd.Parameters.AddWithValue("@PW", ScPass.Text);
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(Common.dtUsSettings);
                        da.Dispose();
                        cmd.Dispose();
                    }
                    if (Common.dtUsSettings.Rows.Count > 0)
                    {
                        Common.User_Name = Common.dtUsSettings.Rows[0]["scname"].ToString();
                        Common.listUsSettings = Common.dtUsSettings.AsEnumerable().ToList();
                        SaveLogonInfo();
                        //登入成功，關窗
                        Invoke(new myDelegate(closeForm));
                    }
                    else
                    {
                        MessageBox.Show("沒有此用戶！", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                catch
                {
                    //登入失敗，開啟連線設定視窗
                    //if (pnlOption.Visible == false)
                    //    Invoke(new myDelegate(ShowLoginBox));
                }
            }
        }

        void Open1()
        {
            if (InvokeRequired)
            {
                Invoke(new myDelegate(Open1));
                return;
            }
            using (FrmSetString sfrm = new FrmSetString())
            {
                sfrm.ShowDialog();
                if (Thread2 != null && Thread2.IsAlive)
                {
                    Thread2.Abort();
                    Thread2.Join();
                }
            }
        }

        void closeForm()
        {
            if (InvokeRequired)
            {
                Invoke(new myDelegate(closeForm));
                return;
            }
            if (Common.sqlConnString.Contains("Integrated")) Common.Sql_LogMod = 2;
            else Common.Sql_LogMod = 1;
            this.Close();
        }

        void SaveLogonInfo()
        {
            string sql = Common.sqlConnString;
            if (sql.Contains("Application Name=JBS_C"))
                sql = sql.Replace(";Application Name=JBS_C", "");

            string[] str = sql.Split(';');

            Common.logOnInfo.ConnectionInfo.ServerName = str[0].Substring(str[0].IndexOf("=") + 1);
            Common.logOnInfo.ConnectionInfo.DatabaseName = str[1].Substring(str[1].IndexOf("=") + 1);
            Common.DatabaseName = Common.logOnInfo.ConnectionInfo.DatabaseName;

            if (Common.sqlConnString.Contains("Password"))
            {
                Common.logOnInfo.ConnectionInfo.UserID = str[2].Substring(str[2].IndexOf("=") + 1);
                Common.logOnInfo.ConnectionInfo.Password = str[3].Substring(str[3].IndexOf("=") + 1);
            }
        }

        int count = 0;
        private void ScName_KeyDown(object sender, KeyEventArgs e)
        {
            TextBox tb = ((TextBox)sender);
            if (tb.Name == ScName.Name)
            {
                if (e.KeyCode == Keys.Enter)
                {
                    ScPass.Focus();
                    ScPass.Focus();
                }
                else if (e.KeyCode == Keys.Down)
                {
                    ScPass.Focus();
                    ScPass.SelectAll();
                }
            }
            else if (tb.Name == ScPass.Name)
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (Thread2 != null && Thread2.IsAlive) return;
                    Thread2 = new Thread(new ThreadStart(UserLogin));
                    Thread2.Start();
                    t = new System.Timers.Timer();
                    t.Elapsed -= new System.Timers.ElapsedEventHandler(t_Elapsed);
                    t.Elapsed += new System.Timers.ElapsedEventHandler(t_Elapsed);
                    t.Interval = 1000;
                    t.Enabled = true;
                }
                else if (e.KeyCode == Keys.Up)
                {
                    ScName.Focus();
                    ScName.SelectAll();
                }
            }
        }

        void t_Elapsed(object source, System.Timers.ElapsedEventArgs e)
        {
            if (++count == 2)
            {
                if (Thread2 != null && Thread2.IsAlive)
                {
                    count = 0;
                    t.Dispose();
                    Thread2.Abort();
                    Thread2.Join();
                    Invoke(new myDelegate(Open1));
                }
            }
        }

        private void LoginForm_Resize(object sender, EventArgs e)
        {
            ScName.Location = new Point(this.Width * 47 / 100, this.Height * 7 / 10);
            ScPass.Location = new Point(this.Width * 47 / 100, this.Height * 75 / 100);
        }

        private void ScName_Enter(object sender, EventArgs e)
        {
            (sender as TextBox).BackColor = Color.GreenYellow;
        }

        private void ScName_Leave(object sender, EventArgs e)
        {
            (sender as TextBox).BackColor = Color.White;
        }

        private void tableLayoutPnl1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Location.X > (this.Width - 250) && e.Location.Y < 200)
            {
                if (Thread2 != null && Thread2.IsAlive)
                {
                    t.Dispose();
                    Thread2.Abort();
                    Thread2.Join();
                }
                using (FrmSetString sfrm = new FrmSetString())
                {
                    sfrm.ShowDialog();
                }
            }
            else if (e.Location.X > (this.Width - 250) && e.Location.Y > (this.Height - 140))
            {
                ApplicationExit_Click(null, null);
            }
        }

        //ESC離開系統
        private void LoginForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) ApplicationExit_Click(null, null);
        }

        //離開系統
        private void ApplicationExit_Click(object sender, EventArgs e)
        {
            if (MainForm.main.tempThread.IsAlive)
            {
                MainForm.main.tempThread.Abort();
                MainForm.main.tempThread.Join();
            }
            if (t.IsNotNull())
            {
                t.Enabled = false;
                t.Dispose();
            }
            this.Close();
            this.Dispose();
            Application.Exit();
        }
    }
}
