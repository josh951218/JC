using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using S_61.Basic;
using S_61.Model;
using S_61.MyControl;

namespace S_61.s_2統計圖表
{
    public partial class 應收票據明細 : FormT
    {
        DataTable table = new DataTable();
        string 勾選字串 = "";
        public 應收票據明細()
        {
            InitializeComponent();
            if (Common.User_DateTime == 1) ChDate_1.MaxLength = ChDate_2.MaxLength = ChDate1_1.MaxLength = ChDate1_2.MaxLength = ChDate2_1.MaxLength = ChDate2_2.MaxLength = ChDate3_1.MaxLength = ChDate3_2.MaxLength = 7;
            else ChDate_1.MaxLength = ChDate_2.MaxLength = ChDate1_1.MaxLength = ChDate1_2.MaxLength = ChDate2_1.MaxLength = ChDate2_2.MaxLength = ChDate3_1.MaxLength = ChDate3_2.MaxLength = 8;
            ChDate_1.Init();
            ChDate_2.Init();
            ChDate1_1.Init();
            ChDate1_2.Init();
            ChDate2_1.Init();
            ChDate2_2.Init();
            ChDate3_1.Init();
            ChDate3_2.Init();
        }

        private void 應收票據明細_Load(object sender, EventArgs e)
        {
            groupBoxT1.BackColor  = 勾選CB.BackColor = Color.FromArgb(215, 227, 239);
            if (Common.單據異動 == "2") CoNo.Enabled = false;
            CoNo.Text = Common.使用者預設公司;
            CHK.CoNo_Validating(CoNo, CoName1);
            Common.取得浮動連線字串(Common.使用者預設公司);
            if (Common.User_DateTime == 1)
            {
                ChDate_1.Text = Date.GetDateTime(Common.User_DateTime).Substring(0, 5) + "01";
                ChDate_2.Text = Date.GetDateTime(Common.User_DateTime);
            }
            else
            {
                ChDate_1.Text = Date.GetDateTime(Common.User_DateTime).Substring(0, 6) + "01";
                ChDate_2.Text = Date.GetDateTime(Common.User_DateTime);
            }

        }

        bool compare(TextBox tb1,TextBox tb2)
        {
            if (string.Compare(tb1.Text, tb2.Text) > 0)
            {
                MessageBox.Show("起始"+tb1.Tag+"不可大於終止"+tb2.Tag,"訊息視窗",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                tb1.Focus();
                return false;
            }
            return true;
        }

        private void btnBrow_Click(object sender, EventArgs e)
        {
            if (!CB1.Checked && !CB2.Checked && !CB3.Checked && !CB4.Checked && !CB5.Checked && !CB6.Checked && !CB7.Checked && !CB8.Checked)
            {
                MessageBox.Show("票態尚未勾選", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                CB1.Focus();
                return;
            }
            if (!compare(ChDate_1,ChDate_2)) return;
            if (!compare(ChNo1, ChNo2)) return;
            if (!compare(CuNo1, CuNo2)) return;
            if (!compare(AcNo1, AcNo2)) return;
            if (!compare(ChDate1_1, ChDate1_2)) return;
            if (!compare(ChDate2_1, ChDate2_2)) return;
            if (!compare(ChDate3_1, ChDate3_2)) return;
            try
            {
                using (SqlConnection cn = new SqlConnection(Common.sqlConnString))
                {
                    cn.Open();
                    string sql = "select 序號='',* from chki where '0'='0'"
                               + " and cono='" + CoNo.Text.Trim() + "' and chdate >='" + Date.ToTWDate(ChDate_1.Text.Trim()) + "' and chdate <= '" + Date.ToTWDate(ChDate_2.Text.Trim()) + "'";
                    if (ChNo1.Text.Trim() != "") sql += " and chno >='" + ChNo1.Text.Trim() + "'";
                    if (ChNo2.Text.Trim() != "") sql += " and chno <='" + ChNo2.Text.Trim() + "'";
                    if (AcNo1.Text.Trim() != "") sql += " and acno >= '" + AcNo1.Text.Trim() + "'";
                    if (AcNo2.Text.Trim() != "") sql += " and acno <= '" + AcNo2.Text.Trim() + "'";
                    if (CuNo1.Text.Trim() != "") sql += " and cuno >= '" + CuNo1.Text.Trim() + "'";
                    if (CuNo2.Text.Trim() != "") sql += " and cuno <= '" + CuNo2.Text.Trim() + "'";
                    if (ChDate1_1.Text.Trim() != "") sql += " and chdate1 >= '" + Date.ToTWDate(ChDate1_1.Text.Trim()) + "'";
                    if (ChDate1_2.Text.Trim() != "") sql += " and chdate1 <= '" + Date.ToTWDate(ChDate1_2.Text.Trim()) + "'";
                    if (ChDate2_1.Text.Trim() != "") sql += " and chdate2 >= '" + Date.ToTWDate(ChDate2_1.Text.Trim()) + "'";
                    if (ChDate2_2.Text.Trim() != "") sql += " and chdate2 <= '" + Date.ToTWDate(ChDate2_2.Text.Trim()) + "'";
                    if (ChDate3_1.Text.Trim() != "") sql += " and chdate3 >= '" + Date.ToTWDate(ChDate3_1.Text.Trim()) + "'";
                    if (ChDate3_2.Text.Trim() != "") sql += " and chdate3 <= '" + Date.ToTWDate(ChDate3_2.Text.Trim()) + "'";
                    sql += " and chstatus in (";
                    if (CB1.Checked) sql += "1,";
                    if (CB2.Checked) sql += "2,";
                    if (CB3.Checked) sql += "3,";
                    if (CB4.Checked) sql += "4,";
                    if (CB5.Checked) sql += "5,";
                    if (CB6.Checked) sql += "6,";
                    if (CB7.Checked) sql += "7,";
                    if (CB8.Checked) sql += "8,";
                    sql = sql.Substring(0, sql.Length - 1);
                    sql += ")";
                    if (勾選字串 != "")
                    {
                        sql += " and acno in (";
                        string[]arry = 勾選字串.Substring(0, 勾選字串.Length - 1).Split(',');
                        for (int i = 0; i < arry.Length; i++)
                        {
                            if (i == arry.Length - 1)
                                sql += "'" + arry[i].ToString() + "'";
                            else
                                sql += "'" + arry[i].ToString() + "',";
                        }
                        sql += ")";
                    }
                    sql += " order by chno";
                    SqlDataAdapter dd = new SqlDataAdapter(sql, cn);
                    table.Clear();
                    dd.Fill(table);
                }
                if (table.Rows.Count == 0)
                {
                    MessageBox.Show("找不到任何資料，請重新輸入", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                using (應收票據明細_瀏覽 frm = new 應收票據明細_瀏覽())
                {
                    frm.SetParaeter();
                    frm.table = table;
                    frm.date1.Text = ChDate_1.Text;
                    frm.date2.Text = ChDate_2.Text;
                    frm.ShowDialog();
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Dispose();
            this.Close();
        }

        private void CoNo_DoubleClick(object sender, EventArgs e)
        {
            CHK.CoNo_OpemFrm(CoNo, CoName1,null,false);
        }

        private void CoNo_Validating(object sender, CancelEventArgs e)
        {
            if (btnExit.Focused) return;
            if (CoNo.Text.Trim() == "")
            {
                MessageBox.Show("公司編號不可為空", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                e.Cancel = true;
                return;
            }
            if (!CHK.CoNo_Validating(CoNo, CoName1))
            {
                e.Cancel = true;
                CHK.CoNo_OpemFrm(CoNo, CoName1, null, false);
            }
            else
            {
                Common.取得浮動連線字串(CoNo.Text.Trim());
                if (CuNo1.Text.Trim() != "")
                    if (!CHK.CuNo_Validating(Common.浮動連線字串, CuNo1)) CuNo1.Text = "";
                if (CuNo2.Text.Trim() != "")
                    if (!CHK.CuNo_Validating(Common.浮動連線字串, CuNo2)) CuNo2.Text = "";
            }
        }

        private void ChDate_2_Validating(object sender, CancelEventArgs e)
        {
            if (btnExit.Focused) return;
            TextBox tb = sender as TextBox;
            if (tb.Name == "ChDate_1" || tb.Name == "ChDate_2")
            {
                if (tb.Text.Trim() == "")
                {
                    MessageBox.Show("異動日期不可為空", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    e.Cancel = true;
                    return;
                }
            }
            if (tb.Text.Trim() != "")
            {
                if (!tb.IsDateTime())
                {
                    MessageBox.Show("日期格式錯誤", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    e.Cancel = true;
                }
            }
            if (!CHK.稽核會計年度(tb.Text.Trim())) e.Cancel = true;
        }

        private void CuNo1_DoubleClick(object sender, EventArgs e)
        {
            TextBox tb  = sender as TextBox;
            CHK.CuNo_OpemFrm(tb);
        }

        private void CuNo1_Validating(object sender, CancelEventArgs e)
        {
            if (btnExit.Focused) return;
            TextBox tb = sender as TextBox;
            if (tb.Text.Trim() != "")
            {
                if (!CHK.CuNo_Validating(Common.浮動連線字串, tb))
                {
                    e.Cancel = true;
                    CHK.CuNo_OpemFrm(tb);
                }
            }
        }

        private void AcNo1_DoubleClick(object sender, EventArgs e)
        {
            TextBox tb = sender as TextBox;
            CHK.AcNo_OpemFrm(CoNo.Text.Trim(),tb,null,null,false,true);
        }

        private void AcNo1_Validating(object sender, CancelEventArgs e)
        {
            if (btnExit.Focused) return;
            TextBox tb = sender as TextBox;
            if (tb.Text.Trim() != "")
            {
                if (!CHK.AcNo_Validating(CoNo.Text.Trim(), tb, null, null, true))
                {
                    e.Cancel = true;
                    CHK.AcNo_OpemFrm(CoNo.Text.Trim(), tb, null, null, false, true);
                }
            }
        }

        private void 勾選開窗_Click(object sender, EventArgs e)
        {
            using (勾選帳戶開窗 frm = new 勾選帳戶開窗())
            {
                frm.SetParaeter(ViewMode.Normal);
                frm.勾選字串 = 勾選字串;
                frm.去除外幣帳戶 = true;
                frm.CoNo = CoNo.Text.Trim();
                frm.ShowDialog();
                switch (frm.DialogResult)
                {
                    case DialogResult.OK:
                        勾選字串 = frm.勾選字串;
                        break;
                }
            }
            if (勾選字串 != "")
            {
                AcNo1.Text = AcNo2.Text = "";
                AcNo1.Enabled = AcNo2.Enabled = false;
                勾選CB.Checked = true;
            }
            else
            {
                AcNo1.Enabled = AcNo2.Enabled = true;
                勾選CB.Checked = false;
            }
        }

        private void 勾選清除_Click(object sender, EventArgs e)
        {
            勾選字串 = "";
            AcNo1.Enabled = AcNo2.Enabled = true;
            勾選CB.Checked = false;
        }


    }
}
