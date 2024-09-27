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
    public partial class 應付票齡分析 : FormT
    {
        List<txtNumber> Rule;
        DataTable table = new DataTable();
        string 勾選字串 = "";

        public 應付票齡分析()
        {
            InitializeComponent();
            Rule = new List<txtNumber>() { Rule1_1, Rule1_2, Rule2_1, Rule2_2, Rule3_1, Rule3_2, Rule4_1, Rule4_2, Rule5_1 };
            Rule.ForEach(r =>
            {
                if (r.Name != "Rule1_1")
                    r.ReadOnly = false;
            });
            if (Common.User_DateTime == 1) ChDate1_1.MaxLength = ChDate1_2.MaxLength = 7;
            else ChDate1_1.MaxLength = ChDate1_2.MaxLength = 8;
            ChDate1_1.Init();
            ChDate1_2.Init();
        }

        private void 應付票齡分析_Load(object sender, EventArgs e)
        {
            groupBoxT3.BackColor = groupBoxT1.BackColor = Color.FromArgb(215, 227, 239);
            明細表.BackColor = 總額表.BackColor = 勾選CB.BackColor = Color.FromArgb(215, 227, 239);
            if (Common.單據異動 == "2") CoNo.Enabled = false;
            CoNo.Text = Common.使用者預設公司;
            CHK.CoNo_Validating(CoNo, CoName1);
            Common.取得浮動連線字串(Common.使用者預設公司);
            if (Common.User_DateTime == 1)
            {
                ChDate1_1.Text = Date.GetDateTime(Common.User_DateTime).Substring(0, 5) + "01";
                ChDate1_2.Text = Date.GetDateTime(Common.User_DateTime);
            }
            else
            {
                ChDate1_1.Text = Date.GetDateTime(Common.User_DateTime).Substring(0, 6) + "01";
                ChDate1_2.Text = Date.GetDateTime(Common.User_DateTime);
            }
        }

        bool compare(TextBox tb1, TextBox tb2)
        {
            if (string.Compare(tb1.Text, tb2.Text) > 0)
            {
                MessageBox.Show("起始" + tb1.Tag + "不可大於終止" + tb2.Tag, "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                tb1.Focus();
                return false;
            }
            return true;
        }

        private void btnBrow_Click(object sender, EventArgs e)
        {
            if (!CB1.Checked && !CB2.Checked && !CB3.Checked && !CB4.Checked && !CB5.Checked && !CB6.Checked )
            {
                MessageBox.Show("票態尚未勾選", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                CB1.Focus();
                return;
            }
            if (!compare(ChDate1_1, ChDate1_2)) return;
            if (!compare(FaNo1, FaNo2)) return;
            try
            {
                using (SqlConnection cn = new SqlConnection(Common.sqlConnString))
                {
                    cn.Open();
                    string sql = "";
                    sql = "select RuleLv1=0.0,RuleLv2=0.0,RuleLv3=0.0,RuleLv4=0.0,RuleLv5=0.0,筆數='',fano,faname2,faname1,chno,chstname,chdate1,chdate1_1,chdate2,chdate2_1,chmny from chko where '0'='0' ";
                    sql += " and cono='" + CoNo.Text.Trim() + "' and chdate1 >='" + Date.ToTWDate(ChDate1_1.Text.Trim()) + "' and chdate1 <= '" + Date.ToTWDate(ChDate1_2.Text.Trim()) + "'";
                    if (FaNo1.Text.Trim() != "") sql += " and fano >= '" + FaNo1.Text.Trim() + "'";
                    if (FaNo2.Text.Trim() != "") sql += " and fano <= '" + FaNo2.Text.Trim() + "'";
                    if (勾選字串 != "")
                    {
                        sql += " and fano in (";
                        string[] arry = 勾選字串.Substring(0, 勾選字串.Length - 1).Split(',');
                        for (int i = 0; i < arry.Length; i++)
                        {
                            if (i == arry.Length - 1)
                                sql += "'" + arry[i].ToString() + "'";
                            else
                                sql += "'" + arry[i].ToString() + "',";
                        }
                        sql += ")";
                    }
                    sql += " and chstatus in (";
                    if (CB1.Checked) sql += "1,";
                    if (CB2.Checked) sql += "2,";
                    if (CB3.Checked) sql += "3,";
                    if (CB4.Checked) sql += "4,";
                    if (CB5.Checked) sql += "5,";
                    if (CB6.Checked) sql += "6,";
                    sql = sql.Substring(0, sql.Length - 1);
                    sql += ") order by fano";
                    SqlDataAdapter dd = new SqlDataAdapter(sql, cn);
                    table.Clear();
                    dd.Fill(table);
                }
                if (table.Rows.Count == 0)
                {
                    MessageBox.Show("找不到任何資料，請重新輸入", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                DateTime t1, t2;
                TimeSpan ts;
                using (SqlConnection cn = new SqlConnection(Common.浮動連線字串))
                {
                    cn.Open();
                    SqlCommand cmd = cn.CreateCommand();
                    SqlDataReader reader;
                    for (int i = 0; i < table.Rows.Count; i++)
                    {
                        cmd.CommandText = "select faname1,faname2 from fact where fano='" + table.Rows[i]["fano"].ToString().Trim() + "'";
                        using (reader = cmd.ExecuteReader())
                        {
                            if (reader.HasRows && reader.Read())
                            {
                                table.Rows[i]["faname1"] = reader["faname1"].ToString().Trim();
                                table.Rows[i]["faname2"] = reader["faname2"].ToString().Trim();
                            }
                        }
                        table.Rows[i]["chdate1"] = Date.AddLine(table.Rows[i]["chdate1"].ToString());
                        table.Rows[i]["chdate1_1"] = Date.AddLine(table.Rows[i]["chdate1_1"].ToString());
                        table.Rows[i]["chdate2"] = Date.AddLine(table.Rows[i]["chdate2"].ToString());
                        table.Rows[i]["chdate2_1"] = Date.AddLine(table.Rows[i]["chdate2_1"].ToString());
                        t1 = Convert.ToDateTime(table.Rows[i]["chdate1_1"].ToString().Trim());//收票日 西元年
                        t2 = Convert.ToDateTime(table.Rows[i]["chdate2_1"].ToString().Trim());//到期日 西元年
                        ts = t2 - t1;//到期日 - 收票日 相差的天數
                        if (ts.Days.ToDecimal() < 0)
                            table.Rows[i]["RuleLv1"] = table.Rows[i]["chmny"].ToDecimal();//到期日 小於 收票日
                        else if (ts.Days.ToDecimal() >= Rule1_1.Text.ToDecimal() && ts.Days.ToDecimal() <= Rule1_2.Text.ToDecimal())
                            table.Rows[i]["RuleLv1"] = table.Rows[i]["chmny"].ToDecimal();
                        else if (ts.Days.ToDecimal() >= Rule2_1.Text.ToDecimal() && ts.Days.ToDecimal() <= Rule2_2.Text.ToDecimal())
                            table.Rows[i]["RuleLv2"] = table.Rows[i]["chmny"].ToDecimal();
                        else if (ts.Days.ToDecimal() >= Rule3_1.Text.ToDecimal() && ts.Days.ToDecimal() <= Rule3_2.Text.ToDecimal())
                            table.Rows[i]["RuleLv3"] = table.Rows[i]["chmny"].ToDecimal();
                        else if (ts.Days.ToDecimal() >= Rule4_1.Text.ToDecimal() && ts.Days.ToDecimal() <= Rule4_2.Text.ToDecimal())
                            table.Rows[i]["RuleLv4"] = table.Rows[i]["chmny"].ToDecimal();
                        else
                            table.Rows[i]["RuleLv5"] = table.Rows[i]["chmny"].ToDecimal();
                    }
                }
                if (明細表.Checked)
                {
                    應付票齡分析_明細表 frm = new 應付票齡分析_明細表();
                    frm.SetParaeter();
                    frm.table = table;
                    frm.Rule1_2 = Rule1_2.Text.Trim();
                    frm.Rule2_1 = Rule2_1.Text.Trim();
                    frm.Rule2_2 = Rule2_2.Text.Trim();
                    frm.Rule3_1 = Rule3_1.Text.Trim();
                    frm.Rule3_2 = Rule3_2.Text.Trim();
                    frm.Rule4_1 = Rule4_1.Text.Trim();
                    frm.Rule4_2 = Rule4_2.Text.Trim();
                    frm.Rule5_1 = Rule5_1.Text.Trim();
                    frm.date1.Text = ChDate1_1.Text;
                    frm.date2.Text = ChDate1_2.Text;
                    frm.ShowDialog();
                }
                else
                {
                    DataTable 總額表 = table.Copy();
                    總額表.Clear();
                    DataRow dr;
                    List<string> cuno_list = table.AsEnumerable().Select(r => r["fano"].ToString()).Distinct().ToList();
                    DataTable temp;
                    cuno_list.ForEach(r =>
                    {
                        decimal lv1 = 0, lv2 = 0, lv3 = 0, lv4 = 0, lv5 = 0;
                        dr = 總額表.NewRow();
                        temp = table.AsEnumerable().Where(t => t["fano"].ToString().Trim() == r.Trim()).CopyToDataTable();
                        dr["fano"] = temp.Rows[0]["fano"].ToString();
                        dr["faname1"] = temp.Rows[0]["faname1"].ToString();
                        dr["筆數"] = temp.Rows.Count.ToString();
                        for (int i = 0; i < temp.Rows.Count; i++)
                        {
                            lv1 += temp.Rows[i]["RuleLv1"].ToDecimal();
                            lv2 += temp.Rows[i]["RuleLv2"].ToDecimal();
                            lv3 += temp.Rows[i]["RuleLv3"].ToDecimal();
                            lv4 += temp.Rows[i]["RuleLv4"].ToDecimal();
                            lv5 += temp.Rows[i]["RuleLv5"].ToDecimal();
                        }
                        dr["RuleLv1"] = lv1;
                        dr["RuleLv2"] = lv2;
                        dr["RuleLv3"] = lv3;
                        dr["RuleLv4"] = lv4;
                        dr["RuleLv5"] = lv5;
                        總額表.Rows.Add(dr);
                    });
                    應付票齡分析_總額表 frm = new 應付票齡分析_總額表();
                    frm.SetParaeter();
                    frm.table = 總額表;
                    frm.Rule1_2 = Rule1_2.Text.Trim();
                    frm.Rule2_1 = Rule2_1.Text.Trim();
                    frm.Rule2_2 = Rule2_2.Text.Trim();
                    frm.Rule3_1 = Rule3_1.Text.Trim();
                    frm.Rule3_2 = Rule3_2.Text.Trim();
                    frm.Rule4_1 = Rule4_1.Text.Trim();
                    frm.Rule4_2 = Rule4_2.Text.Trim();
                    frm.Rule5_1 = Rule5_1.Text.Trim();
                    frm.date1.Text = ChDate1_1.Text;
                    frm.date2.Text = ChDate1_2.Text;
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
            CHK.CoNo_OpemFrm(CoNo, CoName1, null, false);
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
                if (FaNo1.Text.Trim() != "")
                    if (!CHK.FaNo_Validating(Common.浮動連線字串, FaNo1)) FaNo1.Text = "";
                if (FaNo2.Text.Trim() != "")
                    if (!CHK.FaNo_Validating(Common.浮動連線字串, FaNo2)) FaNo2.Text = "";
            }
        }

        private void ChDate1_1_Validating(object sender, CancelEventArgs e)
        {
            if (btnExit.Focused) return;
            TextBox tb = sender as TextBox;
            if (tb.Name == "ChDate1_1" || tb.Name == "ChDate1_2")
            {
                if (tb.Text.Trim() == "")
                {
                    MessageBox.Show("收票日期不可為空", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

        private void FaNo1_DoubleClick(object sender, EventArgs e)
        {
            TextBox tb = sender as TextBox;
            CHK.FaNo_OpemFrm(tb);
        }

        private void FaNo1_Validating(object sender, CancelEventArgs e)
        {
            if (btnExit.Focused) return;
            TextBox tb = sender as TextBox;
            if (tb.Text.Trim() != "")
            {
                if (!CHK.FaNo_Validating(Common.浮動連線字串, tb))
                {
                    e.Cancel = true;
                    CHK.FaNo_OpemFrm(tb);
                }
            }
        }

        private void 勾選開窗_Click(object sender, EventArgs e)
        {
            using (勾選廠商開窗 frm = new 勾選廠商開窗())
            {
                frm.SetParaeter(ViewMode.Normal);
                frm.勾選字串 = 勾選字串;
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
                FaNo1.Text = FaNo2.Text = "";
                FaNo1.Enabled = FaNo2.Enabled = false;
                勾選CB.Checked = true;
            }
            else
            {
                FaNo1.Enabled = FaNo2.Enabled = true;
                勾選CB.Checked = false;
            }
        }

        private void 勾選清除_Click(object sender, EventArgs e)
        {
            勾選字串 = "";
            FaNo1.Enabled = FaNo2.Enabled = true;
            勾選CB.Checked = false;
        }
    }
}
