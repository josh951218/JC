using System;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Transactions;
using System.Windows.Forms;
using System.Drawing;

namespace S_61.Basic
{
    public partial class FrmRegist : baseForm
    {
        public FrmRegist()
        {
            InitializeComponent();

            //選單與主要表單
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Size = MainForm.FrmMenu.Size;
        }

        private void FrmRegist_Load(object sender, EventArgs e)
        {
            this.Location = new Point(1, 1);
            LoadTime();
        }

        void LoadTime()
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(Common.sqlConnString))
                {
                    cn.Open();
                    using (SqlCommand cmd = cn.CreateCommand())
                    {
                        cmd.CommandText = "select time from chksys where usrno='T01'";
                        textBoxT1.Text = cmd.ExecuteScalar().ToDecimal().ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void bookNo_Validating(object sender, CancelEventArgs e)
        {
            if (BookNo.ReadOnly) return;
            if (btnExit.Focused) return;

            if (BookNo.Text.Trim().Length != 17)
            {
                e.Cancel = true;
                MessageBox.Show("序號輸入錯誤！", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else if (Common.name2 != BookNo.Text.Substring(9, 2).ToUpper())
            {
                e.Cancel = true;
                MessageBox.Show("序號輸入錯誤！", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            //else if (!BookNo.Text.Remove(13, 1).Remove(9, 2).IsNumic())
            //{
            //    e.Cancel = true;
            //    MessageBox.Show("序號輸入錯誤！", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    return;
            //}
            //else if (!BookNo.Text.Remove(11, 2).NotIn(new string[] { "71", "72" }))
            //{
            //    e.Cancel = true;
            //    MessageBox.Show("序號輸入錯誤！", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    return;
            //}
            else
            {
                Code.Focus();
                //產生AutoX1
                AutoX1.Text = "";
                Wks.Text = new string(BookNo.Text.Skip(15).ToArray());

                Random rd = new Random((int)DateTime.Now.Ticks);
                for (int i = 0; i < 4; i++)
                {
                    AutoX1.Text += rd.Next(1, 9).ToString();
                }

                Security Sec = new Security();
                AutoX1.Text += Sec.數字加密(BookNo.Text.Substring(11, 2), AutoX1.Text.Substring(0, 2));
                AutoX1.Text += Sec.數字加密(textBoxT1.Text.PadLeft(2, '0').Substring(0, 2), AutoX1.Text.Substring(0, 2));
                AutoX1.Text += Sec.數字加密(BookNo.Text.Substring(15, 2), AutoX1.Text.Substring(2, 2));
            }
            BookNo.ReadOnly = true;

            if (BookNo.ReadOnly) return;
            if (btnExit.Focused) return;

            if (BookNo.Text.Trim().Length != 17)
            {
                e.Cancel = true;
                MessageBox.Show("序號輸入錯誤！", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else
            {
                string temp = new string(BookNo.Text.Skip(11).Take(2).ToArray());
                if (temp.ToDecimal() != Common.Series.ToDecimal())
                {
                    e.Cancel = true;
                    MessageBox.Show("序號輸入錯誤！", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                BookNo.ReadOnly = true;
                Wks.Focus();
                //產生AutoX1
                AutoX1.Text = "";
                Wks.Text = new string(BookNo.Text.Skip(15).ToArray());

                Random rd = new Random((int)DateTime.Now.Ticks);
                for (int i = 0; i < 3; i++)
                {
                    AutoX1.Text += rd.Next(1, 9).ToString();
                }
                AutoX1.Text += Wks.Text.Trim();
                for (int i = 0; i < 3; i++)
                {
                    AutoX1.Text += rd.Next(1, 9).ToString();
                }
            }
        }

        private void Wks_Validating(object sender, CancelEventArgs e)
        {
            if (Wks.Text.ToDecimal() == 0)
            {
                e.Cancel = true;
                MessageBox.Show("工作台數輸入錯誤！", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
        }

        private void btnRegist_Click(object sender, EventArgs e)
        {
            Security Sec = new Security();

            string 特徵 = new string(AutoX1.Text.ToCharArray().Reverse().ToArray());
            bool 驗證 = true;

            string textBoxT1temp = "0";
            try
            {
                string 解密 = Sec.數字解密(Code.Text, 特徵);
                if (解密.Substring(0, 4) != AutoX1.Text.Substring(0, 4)) 驗證 = false;
                if (解密.Substring(6, 2) != AutoX1.Text.Substring(0, 2)) 驗證 = false;
                textBoxT1temp = 解密.Substring(4, 2);
            }
            catch
            {
                驗證 = false;
            }
            if (驗證)
            {
                string 版本次 = "";
                string 版本次temp = BookNo.Text.Substring(11, 2);
                Random rd = new Random((int)DateTime.Now.Ticks);
                版本次 += rd.Next(1, 8).ToString();
                for (int i = 1; i < 10; i++)
                {
                    if (i == 版本次[0].ToDecimal())
                        版本次 += 版本次temp;
                    if (i != 版本次[0].ToDecimal() && i != 版本次[0].ToDecimal() + 1)
                        版本次 += rd.Next(0, 9).ToString();
                }

                string 註冊台數 = "";
                string 註冊台數temp = BookNo.Text.Substring(15, 2) + "78";

                註冊台數 += rd.Next(1, 6).ToString();
                for (int i = 1; i < 10; i++)
                {
                    if (i == 註冊台數[0].ToDecimal())
                        註冊台數 += 註冊台數temp;
                    if (i < 註冊台數[0].ToDecimal() || i > 註冊台數[0].ToDecimal() + 3)
                        註冊台數 += rd.Next(0, 9).ToString();
                }

                using (TransactionScope ts = new TransactionScope())
                {
                    try
                    {
                        using (SqlConnection conn = new SqlConnection(Common.sqlConnString))
                        {
                            conn.Open();
                            using (SqlCommand cmd = conn.CreateCommand())
                            {
                                cmd.CommandText = "SELECT CONVERT(nvarchar(30),create_date,114) FROM sys.databases where name ='" + Common.DatabaseName + "'";
                                string str = "";
                                str = cmd.ExecuteScalar().ToString().Trim().Replace(":", "");

                                cmd.CommandText = "update chksys set uomgp='" + Sec.數字加密(註冊台數, str + "0")
                                    + "' ,CT ='" + str
                                    + "' ,vera ='" + Sec.數字加密(版本次, str + "0")
                                    + "' ,time =" + textBoxT1temp;
                                cmd.ExecuteNonQuery();
                            }
                        }
                        ts.Complete();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                        return;
                    }
                }
                Wks.ReadOnly = true;
                Code.ReadOnly = true;
                textBoxT1.Text = textBoxT1temp;
                MessageBox.Show("註冊成功！", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                MessageBox.Show("Code錯誤！", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Code.Focus();
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
