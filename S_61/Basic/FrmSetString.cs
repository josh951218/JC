using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Xml.Linq;
using System.Threading;
using System.Configuration;

namespace S_61.Basic
{
    public partial class FrmSetString : Form
    {
        delegate void myDelegate();
        Thread myThread;

        public FrmSetString()
        {
            InitializeComponent();
            radioT1.Checked = true;
        }

        private void FrmSetString_Load(object sender, EventArgs e)
        {
            try
            {
                string sql = ConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
                if (sql.Contains("Application Name=JBS_C")) sql = sql.Replace(";Application Name=JBS_C", "");

                string[] str = sql.Split(';');
                if (str.Length > 3)
                {
                    radioT1.Checked = true;
                    textBoxT1.Text = str[0].Substring(str[0].IndexOf("=") + 1);
                    textBoxT2.Text = str[1].Substring(str[1].IndexOf("=") + 1);
                    textBoxT3.Text = str[2].Substring(str[2].IndexOf("=") + 1);
                    textBoxT4.Text = str[3].Substring(str[3].IndexOf("=") + 1);
                }
                else
                {
                    radioT2.Checked = true;
                    textBoxT1.Text = str[0].Substring(str[0].IndexOf("=") + 1);
                    textBoxT2.Text = str[1].Substring(str[1].IndexOf("=") + 1);
                }

                Common.logOnInfo.ConnectionInfo.ServerName = textBoxT1.Text;
                Common.logOnInfo.ConnectionInfo.DatabaseName = textBoxT2.Text;
                Common.logOnInfo.ConnectionInfo.UserID = textBoxT3.Text;
                Common.logOnInfo.ConnectionInfo.Password = textBoxT4.Text;
            }
            catch { }
        }

        private void radioT1_CheckedChanged(object sender, EventArgs e)
        {
            textBoxT3.Enabled = textBoxT4.Enabled = radioT1.Checked;
        }

        private void btnBrowT1_Click(object sender, EventArgs e)
        {
            if (radioT1.Checked)
            {
                radioT1.Focus();
                Common.sqlConnString = @"Data Source=" + textBoxT1.Text.Trim()
                    + ";Initial Catalog=" + textBoxT2.Text.Trim()
                    + ";User ID=" + textBoxT3.Text.Trim()
                    + ";Password=" + textBoxT4.Text.Trim();
            }
            else
            {
                radioT2.Focus();
                Common.sqlConnString = @"Data Source=" + textBoxT1.Text.Trim()
                + ";Initial Catalog=" + textBoxT2.Text.Trim()
                + ";Integrated Security=True";
            }
            lblResult.Text = "";
            btnBrowT1.Enabled = false;
            picMsg.Visible = true;
            Application.DoEvents();
            myThread = new Thread(new ThreadStart(DoConn));
            myThread.Start();
        }

        void DoConn()
        {
            try
            {
                if (!Common.sqlConnString.Contains("Application Name=JBS_C")) Common.sqlConnString += ";Application Name=JBS_C";
                using (SqlConnection conn = new SqlConnection(Common.sqlConnString))
                {
                    conn.Open();
                }

                string sql = Common.sqlConnString;
                if (sql.Contains("Application Name=JBS_C")) sql = sql.Replace(";Application Name=JBS_C", "");

                XElement element = XElement.Load(Application.ExecutablePath + ".config");
                XElement x = element.Element("connectionStrings").Element("add");
                x.Attribute("connectionString").Value = sql;
                element.Save(Application.ExecutablePath + ".config");
                MainForm.FrmLogin.tryConnOK = true;
            }
            catch (Exception ex)
            {
                MainForm.FrmLogin.tryConnOK = false;
                MessageBox.Show(ex.ToString());
                MessageBox.Show("資料庫連線設定錯誤，請檢查後從新輸入", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                Invoke(new myDelegate(SetBtnBrowT1Enable));
            }
        }

        void SetBtnBrowT1Enable()
        {
            if (InvokeRequired)
            {
                Invoke(new myDelegate(SetBtnBrowT1Enable));
                return;
            }
            lblResult.Text = MainForm.FrmLogin.tryConnOK ? "連線成功" : "連線失敗";
            picMsg.Visible = false;
            btnBrowT1.Enabled = true;
        }

        private void FrmSetString_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (radioT1.Checked)
            {
                radioT1.Focus();
                Common.sqlConnString = @"Data Source=" + textBoxT1.Text.Trim()
                    + ";Initial Catalog=" + textBoxT2.Text.Trim()
                    + ";User ID=" + textBoxT3.Text.Trim()
                    + ";Password=" + textBoxT4.Text.Trim();
            }
            else
            {
                radioT2.Focus();
                Common.sqlConnString = @"Data Source=" + textBoxT1.Text.Trim()
                + ";Initial Catalog=" + textBoxT2.Text.Trim()
                + ";Integrated Security=True";
            }
            if (!Common.sqlConnString.Contains("Application Name=JBS_C")) Common.sqlConnString += ";Application Name=JBS_C";
        }

        private void FrmSetString_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) this.Dispose();
        }
    }
}
