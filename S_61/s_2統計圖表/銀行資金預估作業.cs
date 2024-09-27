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
    public partial class 銀行資金預估作業 : FormT
    {
        DataTable table = new DataTable();
        DataTable temp = new DataTable();
        List<String> acno_list ;
        string 勾選字串 = "";
        string sql = "";
        SqlDataAdapter dd;
        DataTable Sendtb = new DataTable();
        DataRow dr;

        public 銀行資金預估作業()
        {
            InitializeComponent();
            tableLayoutPnl_for_main6.Dock = DockStyle.None;
            if (Common.User_DateTime == 1) ChDate3.MaxLength = 7;
            else ChDate3.MaxLength = 8;
            ChDate3.Init();
        }

        private void 銀行資金_預估作業_Load(object sender, EventArgs e)
        {
            groupBoxT2.BackColor = groupBoxT1.BackColor = 勾選CB.BackColor = Color.FromArgb(215, 227, 239);
            if (Common.單據異動 == "2") CoNo.Enabled = false;
            CoNo.Text = Common.使用者預設公司;
            CHK.CoNo_Validating(CoNo, CoName1);
            Common.取得浮動連線字串(Common.使用者預設公司);
            ChDate3.Focus();
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
            if (!compare(AcNo1, AcNo2)) return;
            table.Clear();
            temp.Clear();
            Sendtb.Clear();
            string 搜尋條件 = " and cono=N'" + CoNo.Text.Trim() + "'";
            if (ChDate3.Text.Trim() != "")
                搜尋條件 += " and chdate3 <='" + Date.ToTWDate(ChDate3.Text.Trim()) + "'";
            if(AcNo1.Text.Trim() != "")
                搜尋條件 += " and acno >=N'" + AcNo1.Text.Trim() + "'";
            if (AcNo2.Text.Trim() != "")
                搜尋條件 += " and acno <=N'" + AcNo2.Text.Trim() + "'";
            if (勾選字串 != "")
            {
                搜尋條件 += " and acno in (";
                string[] arry = 勾選字串.Substring(0, 勾選字串.Length - 1).Split(',');
                for (int i = 0; i < arry.Length; i++)
                {
                    if (i == arry.Length - 1)
                        搜尋條件 += "N'" + arry[i].ToString() + "'";
                    else
                        搜尋條件 += "N'" + arry[i].ToString() + "',";
                }
                搜尋條件 += ")";
            }
            搜尋條件 += " and acno!='' order by chdate3 ";
            try
            {
                using (SqlConnection cn = new SqlConnection(Common.sqlConnString))
                {
                    cn.Open();
                    #region 先撈銀行帳戶現有餘額
                    sql = "select 帳戶編號=acno,帳戶名稱=acname,帳戶簡稱=acname1,帳號=acact,預兌日='',"
                               + " 單據='',票態='現有餘額',支票號碼='',客戶='',已收未兌=0.0,已付未兌=0.0,預估餘額=acmny1,計算值=0.0"
                               + " from acct where cono='" + CoNo.Text.Trim() + "'";
                    if (AcNo1.Text.Trim() != "")
                        sql += " and acno >=N'" + AcNo1.Text.Trim() + "'";
                    if (AcNo2.Text.Trim() != "")
                        sql += " and acno <=N'" + AcNo2.Text.Trim() + "'";
                    if (勾選字串 != "")
                    {
                        sql += " and acno in (";
                        string[] arry = 勾選字串.Substring(0, 勾選字串.Length - 1).Split(',');
                        for (int i = 0; i < arry.Length; i++)
                        {
                            if (i == arry.Length - 1)
                                sql += "N'" + arry[i].ToString() + "'";
                            else
                                sql += "N'" + arry[i].ToString() + "',";
                        }
                        sql += ")";
                    }
                    dd = new SqlDataAdapter(sql, cn);
                    dd.Fill(table);
                    if (table.Rows.Count == 0) return;
                    #endregion

                    #region 應收票據作業
                    //應收票據作業-不包含空帳戶
                    sql = "select 帳戶編號=acno,帳戶名稱='',帳戶簡稱='',帳號='',預兌日="
                        + " case"
                        + " when " + Common.User_DateTime + "=1 then chdate3"
                        + " when " + Common.User_DateTime + "=2 then chdate3_1"
                        + " end,"
                        + " 單據='應收',票態=chstname,支票號碼=chno,客戶=cuname1,已收未兌=chmny,已付未兌=0.0,預估餘額=0.0,計算值=chmny"
                        + " from chki where chstatus in (1,2) " + 搜尋條件;
                    dd = new SqlDataAdapter(sql, cn);
                    temp.Clear();
                    dd.Fill(temp);
                    table.Merge(temp);

                    if (計入.Checked)
                    {
                        //應收票據作業-含空帳戶
                        sql = "select 帳戶編號='',帳戶名稱='',帳戶簡稱='',帳號='',預兌日="
                            + " case"
                            + " when " + Common.User_DateTime + "=1 then chdate3"
                            + " when " + Common.User_DateTime + "=2 then chdate3_1"
                            + " end,"
                            + " 單據='應收',票態=chstname,支票號碼=chno,客戶=cuname1,已收未兌=chmny,已付未兌=0.0,預估餘額=0.0,計算值=chmny"
                            + " from chki where chstatus in (1) ";
                        sql += " and cono=N'" + CoNo.Text.Trim() + "'";
                        if (ChDate3.Text.Trim() != "")
                            sql += " and chdate3 <='" + Date.ToTWDate(ChDate3.Text.Trim()) + "'";
                        sql += " and acno='' order by chdate3";
                        dd = new SqlDataAdapter(sql, cn);
                        temp.Clear();
                        dd.Fill(temp);
                        table.Merge(temp);
                    }
                    #endregion

                    #region 應付票據作業
                    //應付票據作業-不包含空帳戶
                    sql = "select 帳戶編號=acno,帳戶名稱='',帳戶簡稱='',帳號='',預兌日="
                        + " case"
                        + " when " + Common.User_DateTime + "=1 then chdate3"
                        + " when " + Common.User_DateTime + "=2 then chdate3_1"
                        + " end,"
                        + " 單據='應付',票態=chstname,支票號碼=chno,客戶=faname1,已收未兌=0.0,已付未兌=chmny,預估餘額=0.0,計算值=(-1)*chmny"
                        + " from chko where chstatus in (1,2) " + 搜尋條件;
                    dd = new SqlDataAdapter(sql, cn);
                    temp.Clear();
                    dd.Fill(temp);
                    table.Merge(temp);

                    if (計入.Checked)
                    {
                        //應收票據作業-含空帳戶
                        sql = "select 帳戶編號=acno,帳戶名稱='',帳戶簡稱='',帳號='',預兌日="
                            + " case"
                            + " when " + Common.User_DateTime + "=1 then chdate3"
                            + " when " + Common.User_DateTime + "=2 then chdate3_1"
                            + " end,"
                            + " 單據='應付',票態=chstname,支票號碼=chno,客戶=faname1,已收未兌=0.0,已付未兌=chmny,預估餘額=0.0,計算值=(-1)*chmny"
                            + " from chko where chstatus in (1) ";
                        sql += " and cono=N'" + CoNo.Text.Trim() + "'";
                        if (ChDate3.Text.Trim() != "")
                            sql += " and chdate3 <='" + Date.ToTWDate(ChDate3.Text.Trim()) + "'";
                        sql += " and acno='' order by chdate3";
                        dd = new SqlDataAdapter(sql, cn);
                        temp.Clear();
                        dd.Fill(temp);
                        table.Merge(temp);
                    }
                    #endregion
                }

                //是否需要空帳戶
                if (計入.Checked)
                {
                    DataRow dr = table.NewRow();
                    dr["帳戶編號"] = "";
                    dr["帳戶簡稱"] = "空帳戶";
                    dr["帳戶名稱"] = "空帳戶";
                    dr["票態"] = "現有餘額";
                    dr["預兌日"] = "";
                    dr["已收未兌"] = 0;
                    dr["已付未兌"] = 0;
                    dr["預估餘額"] = 0;
                    dr["計算值"] = 0;
                    table.Rows.Add(dr);
                    table.AcceptChanges();
                }
                else
                {
                    table = table.AsEnumerable().Where(r => r["帳戶編號"].ToString().Trim() != "").CopyToDataTable();
                }
                acno_list = table.AsEnumerable().OrderBy(r => r["帳戶編號"].ToString()).Select(r => r["帳戶編號"].ToString()).Distinct().ToList();

                if (明細表.Checked)
                {
                    acno_list.ForEach(r =>
                    {
                        temp.Clear();
                        temp = table.AsEnumerable().Where(t => t["帳戶編號"].ToString() == r).OrderBy(t => t["預兌日"].ToString()).ThenBy(t => t["票態"].ToString()).ThenBy(t => t["客戶"].ToString()).CopyToDataTable();
                        for (int i = 1; i < temp.Rows.Count; i++)
                        {
                            if (temp.Rows[i]["預兌日"].ToString() == "")
                                temp.Rows[i]["預兌日"] = Common.User_DateTime == 1 ? "   /  /  " : "    /  /  ";
                            else
                                temp.Rows[i]["預兌日"] = Date.AddLine(temp.Rows[i]["預兌日"].ToString());
                            temp.Rows[i]["預估餘額"] = Math.Round(temp.Rows[i - 1]["預估餘額"].ToDecimal() + temp.Rows[i]["計算值"].ToDecimal(), Common.金額小數, MidpointRounding.AwayFromZero);
                        }
                        Sendtb.Merge(temp);
                        Sendtb.AcceptChanges();
                    });
                    using (銀行資金預估作業_明細表 frm = new 銀行資金預估作業_明細表())
                    {
                        frm.SetParaeter();
                        frm.acno_list = acno_list;
                        frm.table = Sendtb;
                        frm.ShowDialog();
                    }
                }
                else if (日記表.Checked)
                {
                    Sendtb = table.Copy();
                    Sendtb.Clear();
                    Sendtb.Columns.Add("已收未兌筆數",typeof(decimal));
                    Sendtb.Columns.Add("已付未兌筆數", typeof(decimal));
                    Sendtb.Columns.Add("報表結餘金額", typeof(decimal));
                    int index;
                    //挑出同一帳號
                    acno_list.ForEach(r =>
                    {
                        temp.Clear();
                        temp = table.AsEnumerable().Where(t => t["帳戶編號"].ToString() == r).OrderBy(t => t["預兌日"].ToString()).ThenBy(t => t["票態"].ToString()).ThenBy(t => t["客戶"].ToString()).CopyToDataTable();
                        //挑出同一天日期
                        var date = temp.AsEnumerable().Select(t => t["預兌日"].ToString()).Distinct().ToList();
                        for (int i = 0; i < date.Count(); i++)
                        {
                            dr = Sendtb.NewRow();
                            dr["帳戶編號"] = r;
                            dr["帳戶名稱"] = temp.Rows[0]["帳戶名稱"].ToString();
                            dr["帳戶簡稱"] = temp.Rows[0]["帳戶簡稱"].ToString();
                            dr["帳號"] = temp.Rows[0]["帳號"].ToString();
                            dr["已收未兌筆數"] = temp.AsEnumerable().Where(t => t["單據"].ToString() == "應收" && t["預兌日"].ToString() == date[i]).Count();
                            dr["已收未兌"] = temp.AsEnumerable().Where(t => t["單據"].ToString() == "應收" && t["預兌日"].ToString() == date[i]).Sum(t => t["已收未兌"].ToDecimal());
                            dr["已付未兌筆數"] = temp.AsEnumerable().Where(t => t["單據"].ToString() == "應付" && t["預兌日"].ToString() == date[i]).Count();
                            dr["已付未兌"] = temp.AsEnumerable().Where(t => t["單據"].ToString() == "應付" && t["預兌日"].ToString() == date[i]).Sum(t => t["已付未兌"].ToDecimal());
                            dr["預估餘額"] = temp.Rows[0]["預估餘額"].ToDecimal();
                            if (date[i].ToString().Trim() == "")
                                dr["預兌日"] = Common.User_DateTime == 1 ? "   /  /  " : "    /  /  ";
                            else
                                dr["預兌日"] = Date.AddLine(date[i]);
                            Sendtb.Rows.Add(dr);
                            Sendtb.AcceptChanges();
                        }
                        for (int i = 1; i < Sendtb.Rows.Count; i++)
                        {
                            if (Sendtb.Rows[i]["帳戶編號"].ToString() == r && Sendtb.Rows[i - 1]["帳戶編號"].ToString() == r)
                                Sendtb.Rows[i]["預估餘額"] = Math.Round(Sendtb.Rows[i - 1]["預估餘額"].ToDecimal() + Sendtb.Rows[i]["已收未兌"].ToDecimal() - Sendtb.Rows[i]["已付未兌"].ToDecimal(), Common.金額小數, MidpointRounding.AwayFromZero);
                        }
                        index = Sendtb.AsEnumerable().ToList().FindLastIndex(t => t["帳戶編號"].ToString() == r);
                        Sendtb.AsEnumerable().Where(t => t["帳戶編號"].ToString() == r).ToList().ForEach(t => t["報表結餘金額"] = Sendtb.Rows[index]["預估餘額"].ToDecimal());
                    });
                    using (銀行資金預估作業_日記表 frm = new 銀行資金預估作業_日記表())
                    {
                        frm.SetParaeter();
                        frm.acno_list = acno_list;
                        frm.table = Sendtb;
                        frm.ShowDialog();
                    }
                }
                else if (總額明細表.Checked)
                {
                    Sendtb = table.Copy();
                    Sendtb.Clear();
                    dr = Sendtb.NewRow();
                    dr["預兌日"] = Common.User_DateTime == 1 ? "   /  /  " : "    /  /  ";
                    dr["票態"] = "現有餘額";
                    dr["已收未兌"] = 0;
                    dr["已付未兌"] = 0;
                    dr["預估餘額"] = table.AsEnumerable().Where(r => r["預兌日"].ToString() == "").Sum(r => r["預估餘額"].ToDecimal());
                    Sendtb.Rows.Add(dr);
                    Sendtb.AcceptChanges();
                    string acname="",acname1="";
                    acno_list.ForEach(r =>
                    {
                        temp.Clear();
                        temp = table.AsEnumerable().Where(t => t["帳戶編號"].ToString() == r).OrderBy(t => t["預兌日"].ToString()).ThenBy(t => t["票態"].ToString()).ThenBy(t => t["客戶"].ToString()).CopyToDataTable();
                        for (int i = 0; i < temp.Rows.Count; i++)
                        {
                            if (i == 0)
                            {
                                acname = temp.Rows[0]["帳戶名稱"].ToString();
                                acname1 = temp.Rows[0]["帳戶簡稱"].ToString();
                                continue;
                            }
                            temp.Rows[i]["預兌日"] = Date.AddLine(temp.Rows[i]["預兌日"].ToString());
                            temp.Rows[i]["帳戶名稱"] = acname;
                            temp.Rows[i]["帳戶簡稱"] = acname1;
                        }
                        Sendtb.Merge(temp);
                        Sendtb.AcceptChanges();
                    });
                    Sendtb = Sendtb.AsEnumerable().Where(t => t["預兌日"].ToString()!="").OrderBy(t => t["預兌日"].ToString()).ThenBy(t => t["票態"].ToString()).ThenBy(t => t["客戶"].ToString()).CopyToDataTable();
                    for (int i = 1; i < Sendtb.Rows.Count; i++)
                    {
                        Sendtb.Rows[i]["預估餘額"] = Math.Round(Sendtb.Rows[i - 1]["預估餘額"].ToDecimal() + Sendtb.Rows[i]["計算值"].ToDecimal(), Common.金額小數, MidpointRounding.AwayFromZero);
                    }
                    using (銀行資金預估作業_總額明細表 frm = new 銀行資金預估作業_總額明細表())
                    {
                        frm.SetParaeter();
                        frm.acno_list = acno_list;
                        frm.table = Sendtb;
                        frm.AcNo.Text = table.AsEnumerable().Where(r => r["帳戶編號"].ToString() != "").Select(r => r["帳戶編號"].ToString()).Min();
                        frm.AcNo1.Text = table.AsEnumerable().Select(r => r["帳戶編號"].ToString()).Max();
                        frm.ShowDialog();
                    }
                }
                else
                {
                    Sendtb = table.Copy();
                    Sendtb.Clear();
                    Sendtb.Columns.Add("已收未兌筆數", typeof(decimal));
                    Sendtb.Columns.Add("已付未兌筆數", typeof(decimal));
                    Sendtb.Columns.Add("報表結餘金額", typeof(decimal));

                    dr = Sendtb.NewRow();
                    dr["預兌日"] = Common.User_DateTime == 1 ? "   /  /  " : "    /  /  ";
                    dr["票態"] = "現有餘額";
                    dr["已收未兌"] = 0;
                    dr["已付未兌"] = 0;
                    dr["預估餘額"] = table.AsEnumerable().Where(r => r["預兌日"].ToString() == "").Sum(r => r["預估餘額"].ToDecimal());
                    Sendtb.Rows.Add(dr);
                    Sendtb.AcceptChanges();
                    var date = table.AsEnumerable().Select(t => t["預兌日"].ToString()).Distinct().ToList();
                    for (int i = 0; i < date.Count(); i++)
                    {
                        if (date[i] == "") continue;
                        dr = Sendtb.NewRow();
                        dr["已收未兌筆數"] = table.AsEnumerable().Where(t => t["單據"].ToString() == "應收" && t["預兌日"].ToString() == date[i]).Count();
                        dr["已收未兌"] = table.AsEnumerable().Where(t => t["單據"].ToString() == "應收" && t["預兌日"].ToString() == date[i]).Sum(t => t["已收未兌"].ToDecimal());
                        dr["已付未兌筆數"] = table.AsEnumerable().Where(t => t["單據"].ToString() == "應付" && t["預兌日"].ToString() == date[i]).Count();
                        dr["已付未兌"] = table.AsEnumerable().Where(t => t["單據"].ToString() == "應付" && t["預兌日"].ToString() == date[i]).Sum(t => t["已付未兌"].ToDecimal());
                        dr["預估餘額"] = table.Rows[0]["預估餘額"].ToDecimal();
                        dr["預兌日"] = Date.AddLine(date[i]);
                        Sendtb.Rows.Add(dr);
                        Sendtb.AcceptChanges();
                    }
                    Sendtb = Sendtb.AsEnumerable().OrderBy(t => t["預兌日"].ToString()).CopyToDataTable();
                    for (int i = 1; i < Sendtb.Rows.Count; i++)
                    {
                        Sendtb.Rows[i]["預估餘額"] = Math.Round(Sendtb.Rows[i - 1]["預估餘額"].ToDecimal() + Sendtb.Rows[i]["已收未兌"].ToDecimal() - Sendtb.Rows[i]["已付未兌"].ToDecimal(), Common.金額小數, MidpointRounding.AwayFromZero);
                    }
                    Sendtb.AsEnumerable().ToList().ForEach(t => t["報表結餘金額"] = Sendtb.Rows[Sendtb.Rows.Count-1]["預估餘額"].ToDecimal());
                    using (銀行資金預估作業_總額日記表 frm = new 銀行資金預估作業_總額日記表())
                    {
                        frm.SetParaeter();
                        frm.acno_list = acno_list;
                        frm.table = Sendtb;
                        frm.AcNo.Text = table.AsEnumerable().Where(r => r["帳戶編號"].ToString() != "").Select(r => r["帳戶編號"].ToString()).Min();
                        frm.AcNo1.Text = table.AsEnumerable().Select(r => r["帳戶編號"].ToString()).Max();
                        frm.ShowDialog();
                    }
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
                if (AcNo1.Text.Trim() != "")
                    if (!CHK.AcNo_Validating(CoNo.Text.Trim(), AcNo1)) AcNo1.Text = "";
                if (AcNo2.Text.Trim() != "")
                    if (!CHK.AcNo_Validating(CoNo.Text.Trim(), AcNo2)) AcNo2.Text = "";
            }
        }

        private void ChDate3_Validating(object sender, CancelEventArgs e)
        {
            if (btnExit.Focused) return;
            TextBox tb = sender as TextBox;
            if (tb.Text.Trim() != "")
            {
                if (!tb.IsDateTime())
                {
                    MessageBox.Show("日期格式錯誤", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    e.Cancel = true;
                    return;
                }
                if (!CHK.稽核會計年度(tb.Text.Trim())) e.Cancel = true;
            }
        }

        private void AcNo1_DoubleClick(object sender, EventArgs e)
        {
            TextBox tb = sender as TextBox;
            CHK.AcNo_OpemFrm(CoNo.Text.Trim(), tb, null, null, false, false);
        }

        private void AcNo1_Validating(object sender, CancelEventArgs e)
        {
            if (btnExit.Focused) return;
            TextBox tb = sender as TextBox;
            if (tb.Text.Trim() != "")
            {
                if (!CHK.AcNo_Validating(CoNo.Text.Trim(), tb, null, null, false))
                {
                    e.Cancel = true;
                    CHK.AcNo_OpemFrm(CoNo.Text.Trim(), tb, null, null, false, false);
                }
            }
        }

        private void 勾選開窗_Click(object sender, EventArgs e)
        {
            using (勾選帳戶開窗 frm = new 勾選帳戶開窗())
            {
                frm.SetParaeter(ViewMode.Normal);
                frm.勾選字串 = 勾選字串;
                frm.去除外幣帳戶 = false;
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
