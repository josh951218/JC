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
    public partial class 銀行對帳報表 : FormT
    {
        DataTable 本期tb = new DataTable();
        DataTable 前期tb = new DataTable();
        DataTable temp = new DataTable();
        List<String> acno_list = new List<string>();
        string 勾選字串 = "";
        public 銀行對帳報表()
        {
            InitializeComponent();
            tableLayoutPnl_for_main6.Dock = DockStyle.None;
            if (Common.User_DateTime == 1) date1.MaxLength = date2.MaxLength = 7;
            else date1.MaxLength = date2.MaxLength = 8;
            date1.Init();
            date2.Init();

        }

        private void 銀行對帳報表_Load(object sender, EventArgs e)
        {
            勾選CB.BackColor = Color.FromArgb(215, 227, 239);
            if (Common.單據異動 == "2") CoNo.Enabled = false;
            CoNo.Text = Common.使用者預設公司;
            CHK.CoNo_Validating(CoNo, CoName1);
            Common.取得浮動連線字串(Common.使用者預設公司);
            if (Common.User_DateTime == 1)
            {
                date1.Text = Date.GetDateTime(Common.User_DateTime).Substring(0, 5) + "01";
                date2.Text = Date.GetDateTime(Common.User_DateTime);
            }
            else
            {
                date1.Text = Date.GetDateTime(Common.User_DateTime).Substring(0, 6) + "01";
                date2.Text = Date.GetDateTime(Common.User_DateTime);
            }
            CoNo.Focus();
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
            if (!compare(date1, date2)) return;
            if (!compare(AcNo1, AcNo2)) return;
            前期tb.Clear();
            本期tb.Clear();
            temp.Clear();
            string 勾選帳戶sql = "";
            if (勾選字串 != "")
            {
                勾選帳戶sql = " and acno in (";
                string[]arry = 勾選字串.Substring(0, 勾選字串.Length - 1).Split(',');
                for (int i = 0; i < arry.Length; i++)
                {
                    if (i == arry.Length - 1)
                        勾選帳戶sql += "N'" + arry[i].ToString() + "'";
                    else
                        勾選帳戶sql += "N'" + arry[i].ToString() + "',";
                }
                勾選帳戶sql += ")";
            }
            try
            {
                using (SqlConnection cn = new SqlConnection(Common.sqlConnString))
                {
                    cn.Open();
                    DataRow dr;
                    SqlDataAdapter dd;
                    string sql = "";
                    #region 先撈帳戶期初金額
                    sql = "select 帳戶編號=acno,帳戶名稱='',帳戶簡稱='',帳號='',交易日期='',交易狀況='',交易憑證='',備註='',存入金額=0.0,領出金額=0.0,餘額=0.0,計算值=acmny from acct where '0'='0'"
                        + " and cono=N'" + CoNo.Text.Trim() + "'" + 勾選帳戶sql;
                    if (AcNo1.Text.Trim() != "")
                        sql += " and acno>=N'" + AcNo1.Text.Trim() + "'";
                    if (AcNo2.Text.Trim() != "")
                        sql += " and acno<=N'" + AcNo2.Text.Trim() + "'";
                    dd = new SqlDataAdapter(sql, cn);
                    dd.Fill(前期tb);
                    dd.Fill(本期tb);//本期取結構
                    本期tb.Clear();
                    if (前期tb.Rows.Count > 0)
                        acno_list = 前期tb.AsEnumerable().Select(r => r["帳戶編號"].ToString().Trim()).ToList();//搜尋的帳號範圍
                    else
                        return;
                    #endregion

                    #region 應收票據建檔-本期
                    sql = "select * from chki where '0'='0'"
                        + " and chdate>='" + Date.ToTWDate(date1.Text.Trim()) + "' and chdate<='" + Date.ToTWDate(date2.Text.Trim()) + "'"
                        + " and cono=N'" + CoNo.Text.Trim() + "'" + 勾選帳戶sql;
                    if (AcNo1.Text.Trim() != "")
                        sql += " and acno>=N'" + AcNo1.Text.Trim() + "'";
                    if (AcNo2.Text.Trim() != "")
                        sql += " and acno<=N'" + AcNo2.Text.Trim() + "'";
                    sql += " and chstatus in (3,6)";
                    dd = new SqlDataAdapter(sql, cn);
                    temp = new DataTable();
                    dd.Fill(temp);
                    for (int i = 0; i < temp.Rows.Count; i++)
                    {
                        if (temp.Rows[i]["chstatus"].ToDecimal() == 3)
                        {
                            dr = 本期tb.NewRow();
                            dr["帳戶編號"] = temp.Rows[i]["acno"].ToString().Trim();
                            dr["交易日期"] = Common.User_DateTime == 1 ? Date.AddLine(temp.Rows[i]["chdate"].ToString().Trim()) : Date.AddLine(temp.Rows[i]["chdate_1"].ToString().Trim());
                            dr["交易狀況"] = "託收兌現";
                            dr["交易憑證"] = temp.Rows[i]["chno"].ToString().Trim();
                            dr["備註"] = temp.Rows[i]["chmemo"].ToString().Trim();
                            dr["存入金額"] = temp.Rows[i]["chmny"].ToDecimal();
                            dr["領出金額"] = 0;
                            dr["餘額"] = 0;
                            dr["計算值"] = temp.Rows[i]["chmny"].ToDecimal();
                            本期tb.Rows.Add(dr);
                            本期tb.AcceptChanges();
                        }
                        else
                        {
                            dr = 本期tb.NewRow();
                            dr["帳戶編號"] = temp.Rows[i]["acno"].ToString().Trim();
                            dr["交易日期"] = Common.User_DateTime == 1 ? Date.AddLine(temp.Rows[i]["chdate"].ToString().Trim()) : Date.AddLine(temp.Rows[i]["chdate_1"].ToString().Trim());
                            dr["交易狀況"] = "票    貼";
                            dr["交易憑證"] = temp.Rows[i]["chno"].ToString().Trim();
                            dr["備註"] = temp.Rows[i]["chmemo"].ToString().Trim();
                            dr["存入金額"] = temp.Rows[i]["chtmny1"].ToDecimal();
                            dr["領出金額"] = 0;
                            dr["餘額"] = 0;
                            dr["計算值"] = temp.Rows[i]["chtmny1"].ToDecimal();
                            本期tb.Rows.Add(dr);
                            本期tb.AcceptChanges();

                            if (temp.Rows[i]["tacno"].ToString().Trim() != "" && temp.Rows[i]["chtmny3"].ToDecimal() != 0)
                            {
                                dr = 本期tb.NewRow();
                                dr["帳戶編號"] = temp.Rows[i]["tacno"].ToString().Trim();
                                if (temp.Rows[i]["chtdate1"].ToString().Trim() != "")
                                    dr["交易日期"] = Common.User_DateTime == 1 ? Date.AddLine(temp.Rows[i]["chtdate1"].ToString().Trim()) : Date.AddLine(temp.Rows[i]["chtdate1_1"].ToString().Trim());
                                else
                                    dr["交易日期"] = Common.User_DateTime == 1 ? "   /  /  " : "    /  /  ";
                                dr["交易狀況"] = "票    貼";
                                dr["交易憑證"] = temp.Rows[i]["chno"].ToString().Trim();
                                dr["備註"] = temp.Rows[i]["chmemo"].ToString().Trim();
                                dr["存入金額"] = temp.Rows[i]["chtmny3"].ToDecimal();
                                dr["領出金額"] = 0;
                                dr["餘額"] = 0;
                                dr["計算值"] = temp.Rows[i]["chtmny3"].ToDecimal();
                                本期tb.Rows.Add(dr);
                                本期tb.AcceptChanges();
                            }
                        }
                    }
                    #endregion

                    #region 應收票據建檔-前期
                    sql = "select * from chki where '0'='0'"
                        + " and chdate < '" + Date.ToTWDate(date1.Text.Trim()) + "' and chdate >='" + Common.系統民國 + "0101" + "' "
                        + " and cono=N'" + CoNo.Text.Trim() + "'" + 勾選帳戶sql;
                    if (AcNo1.Text.Trim() != "")
                        sql += " and acno>=N'" + AcNo1.Text.Trim() + "'";
                    if (AcNo2.Text.Trim() != "")
                        sql += " and acno<=N'" + AcNo2.Text.Trim() + "'";
                    sql += " and chstatus in (3,6)";
                    dd = new SqlDataAdapter(sql, cn);
                    temp = new DataTable();
                    dd.Fill(temp);
                    for (int i = 0; i < temp.Rows.Count; i++)
                    {
                        if (temp.Rows[i]["chstatus"].ToDecimal() == 3)
                        {
                            dr = 前期tb.NewRow();
                            dr["帳戶編號"] = temp.Rows[i]["acno"].ToString().Trim();
                            dr["交易狀況"] = "託收兌現";
                            dr["計算值"] = temp.Rows[i]["chmny"].ToDecimal();
                            前期tb.Rows.Add(dr);
                            前期tb.AcceptChanges();
                        }
                        else
                        {
                            dr = 前期tb.NewRow();
                            dr["帳戶編號"] = temp.Rows[i]["acno"].ToString().Trim();
                            dr["交易狀況"] = "票    貼";
                            dr["計算值"] = temp.Rows[i]["chtmny1"].ToDecimal();
                            前期tb.Rows.Add(dr);
                            前期tb.AcceptChanges();

                            if (temp.Rows[i]["tacno"].ToString().Trim() != "" && temp.Rows[i]["chtmny3"].ToDecimal() != 0)
                            {
                                dr = 前期tb.NewRow();
                                dr["帳戶編號"] = temp.Rows[i]["tacno"].ToString().Trim();
                                dr["交易狀況"] = "票    貼";
                                dr["計算值"] = temp.Rows[i]["chtmny3"].ToDecimal();
                                前期tb.Rows.Add(dr);
                                前期tb.AcceptChanges();
                            }
                        }
                    }
                    #endregion

                    #region 應付票據建檔-本期
                    sql = "select * from chko where '0'='0'"
                        + " and chdate>='" + Date.ToTWDate(date1.Text.Trim()) + "' and chdate<='" + Date.ToTWDate(date2.Text.Trim()) + "'"
                        + " and cono=N'" + CoNo.Text.Trim() + "'" + 勾選帳戶sql;
                    if (AcNo1.Text.Trim() != "")
                        sql += " and acno>=N'" + AcNo1.Text.Trim() + "'";
                    if (AcNo2.Text.Trim() != "")
                        sql += " and acno<=N'" + AcNo2.Text.Trim() + "'";
                    sql += " and chstatus in (3)";
                    dd = new SqlDataAdapter(sql, cn);
                    temp = new DataTable();
                    dd.Fill(temp);
                    for (int i = 0; i < temp.Rows.Count; i++)
                    {
                        dr = 本期tb.NewRow();
                        dr["帳戶編號"] = temp.Rows[i]["acno"].ToString().Trim();
                        dr["交易日期"] = Common.User_DateTime == 1 ? Date.AddLine(temp.Rows[i]["chdate"].ToString().Trim()) : Date.AddLine(temp.Rows[i]["chdate_1"].ToString().Trim());
                        dr["交易狀況"] = "兌    現";
                        dr["交易憑證"] = temp.Rows[i]["chno"].ToString().Trim();
                        dr["備註"] = temp.Rows[i]["chmemo"].ToString().Trim();
                        dr["存入金額"] = 0;
                        dr["領出金額"] = temp.Rows[i]["chmny"].ToDecimal();
                        dr["餘額"] = 0;
                        dr["計算值"] = (-1) * temp.Rows[i]["chmny"].ToDecimal();
                        本期tb.Rows.Add(dr);
                        本期tb.AcceptChanges();
                    }
                    #endregion

                    #region 應付票據建檔-前期
                    sql = "select 帳戶編號=acno,計算值=(-1)*chmny,交易狀況='兌    現' from chko where '0'='0'"
                        + " and chdate < '" + Date.ToTWDate(date1.Text.Trim()) + "' and chdate >='" + Common.系統民國 + "0101" + "' "
                        + " and cono=N'" + CoNo.Text.Trim() + "'" + 勾選帳戶sql;
                    if (AcNo1.Text.Trim() != "")
                        sql += " and acno>=N'" + AcNo1.Text.Trim() + "'";
                    if (AcNo2.Text.Trim() != "")
                        sql += " and acno<=N'" + AcNo2.Text.Trim() + "'";
                    sql += " and chstatus in (3)";
                    dd = new SqlDataAdapter(sql, cn);
                    temp = new DataTable();
                    dd.Fill(temp);
                    前期tb.Merge(temp);
                    前期tb.AcceptChanges();
                    #endregion

                    #region 銀行存提款作業-本期
                    sql = "select * from lodgm where '0'='0'"
                        + " and lodate >= '" + Date.ToTWDate(date1.Text.Trim()) + "' and lodate <='" + Date.ToTWDate(date2.Text.Trim()) + "' "
                        + " and cono=N'" + CoNo.Text.Trim() + "'" + 勾選帳戶sql;
                    if (AcNo1.Text.Trim() != "")
                        sql += " and acno>=N'" + AcNo1.Text.Trim() + "'";
                    if (AcNo2.Text.Trim() != "")
                        sql += " and acno<=N'" + AcNo2.Text.Trim() + "'";
                    dd = new SqlDataAdapter(sql, cn);
                    temp = new DataTable();
                    dd.Fill(temp);
                    for (int i = 0; i < temp.Rows.Count; i++)
                    {
                        if (temp.Rows[i]["lokind"].ToDecimal() == 2)//提款
                        {
                            dr = 本期tb.NewRow();
                            dr["帳戶編號"] = temp.Rows[i]["acno"].ToString().Trim();
                            dr["交易日期"] = Common.User_DateTime == 1 ? Date.AddLine(temp.Rows[i]["lodate"].ToString().Trim()) : Date.AddLine(temp.Rows[i]["lodate_1"].ToString().Trim());
                            dr["交易狀況"] = "提    款";
                            dr["交易憑證"] = temp.Rows[i]["lono"].ToString().Trim();
                            dr["備註"] = temp.Rows[i]["lomemo"].ToString().Trim();
                            dr["存入金額"] = 0;
                            dr["領出金額"] = temp.Rows[i]["lomny"].ToDecimal();
                            dr["餘額"] = 0;
                            dr["計算值"] = (-1) * temp.Rows[i]["lomny"].ToDecimal();
                            本期tb.Rows.Add(dr);
                            本期tb.AcceptChanges();
                        }
                        else// 存款 或 利息
                        {
                            dr = 本期tb.NewRow();
                            dr["帳戶編號"] = temp.Rows[i]["acno"].ToString().Trim();
                            dr["交易日期"] = Common.User_DateTime == 1 ? Date.AddLine(temp.Rows[i]["lodate"].ToString().Trim()) : Date.AddLine(temp.Rows[i]["lodate_1"].ToString().Trim());
                            dr["交易狀況"] = temp.Rows[i]["lokind"].ToDecimal() == 1 ? "存    款" : "利    息";
                            dr["交易憑證"] = temp.Rows[i]["lono"].ToString().Trim();
                            dr["備註"] = temp.Rows[i]["lomemo"].ToString().Trim();
                            dr["存入金額"] = temp.Rows[i]["lomny"].ToDecimal();
                            dr["領出金額"] = 0;
                            dr["餘額"] = 0;
                            dr["計算值"] = temp.Rows[i]["lomny"].ToDecimal();
                            本期tb.Rows.Add(dr);
                            本期tb.AcceptChanges();
                        }
                    }
                    #endregion

                    #region 銀行存提款作業-前期
                    sql = "select acno,lomny,lokind from lodgm where '0'='0'"
                        + " and lodate <  '" + Date.ToTWDate(date1.Text.Trim()) + "' and lodate >='" + Common.系統民國 + "0101" + "' "
                        + " and cono=N'" + CoNo.Text.Trim() + "'" + 勾選帳戶sql;
                    if (AcNo1.Text.Trim() != "")
                        sql += " and acno>=N'" + AcNo1.Text.Trim() + "'";
                    if (AcNo2.Text.Trim() != "")
                        sql += " and acno<=N'" + AcNo2.Text.Trim() + "'";
                    dd = new SqlDataAdapter(sql, cn);
                    temp = new DataTable();
                    dd.Fill(temp);
                    for (int i = 0; i < temp.Rows.Count; i++)
                    {
                        if (temp.Rows[i]["lokind"].ToDecimal() == 2)//提款
                        {
                            dr = 前期tb.NewRow();
                            dr["帳戶編號"] = temp.Rows[i]["acno"].ToString().Trim();
                            dr["交易狀況"] = "提    款";
                            dr["計算值"] = (-1) * temp.Rows[i]["lomny"].ToDecimal();
                            前期tb.Rows.Add(dr);
                            前期tb.AcceptChanges();
                        }
                        else// 存款 或 利息
                        {
                            dr = 前期tb.NewRow();
                            dr["帳戶編號"] = temp.Rows[i]["acno"].ToString().Trim();
                            dr["交易狀況"] = temp.Rows[i]["lokind"].ToDecimal() == 1 ? "存    款" : "利    息";
                            dr["計算值"] = temp.Rows[i]["lomny"].ToDecimal();
                            前期tb.Rows.Add(dr);
                            前期tb.AcceptChanges();
                        }
                    }
                    #endregion

                    #region 銀行轉帳作業-本期(轉出部分)
                    sql = "select * from carry where '0'='0'"
                        + " and cadate >= '" + Date.ToTWDate(date1.Text.Trim()) + "' and cadate <='" + Date.ToTWDate(date2.Text.Trim()) + "' "
                        + " and cono=N'" + CoNo.Text.Trim() + "'" + 勾選帳戶sql;
                    if (AcNo1.Text.Trim() != "")
                        sql += " and acno>=N'" + AcNo1.Text.Trim() + "'";
                    if (AcNo2.Text.Trim() != "")
                        sql += " and acno<=N'" + AcNo2.Text.Trim() + "'";
                    dd = new SqlDataAdapter(sql, cn);
                    temp = new DataTable();
                    dd.Fill(temp);
                    for (int i = 0; i < temp.Rows.Count; i++)
                    {
                        dr = 本期tb.NewRow();
                        dr["帳戶編號"] = temp.Rows[i]["acno"].ToString().Trim();
                        dr["交易日期"] = Common.User_DateTime == 1 ? Date.AddLine(temp.Rows[i]["cadate"].ToString().Trim()) : Date.AddLine(temp.Rows[i]["cadate_1"].ToString().Trim());
                        dr["交易狀況"] = "轉    出";
                        dr["交易憑證"] = temp.Rows[i]["cano"].ToString().Trim();
                        dr["備註"] = temp.Rows[i]["camemo"].ToString().Trim();
                        dr["存入金額"] = 0;
                        dr["領出金額"] = temp.Rows[i]["camny"].ToDecimal();
                        dr["餘額"] = 0;
                        dr["計算值"] = (-1) * temp.Rows[i]["camny"].ToDecimal();
                        本期tb.Rows.Add(dr);
                        本期tb.AcceptChanges();
                    }
                    #endregion

                    #region 銀行轉帳作業-本期(轉入部分)
                    sql = "select * from carry where '0'='0'"
                        + " and cadate >= '" + Date.ToTWDate(date1.Text.Trim()) + "' and cadate <='" + Date.ToTWDate(date2.Text.Trim()) + "' "
                        + " and cono=N'" + CoNo.Text.Trim() + "'" + 勾選帳戶sql.Replace("acno","acnoi");
                    if (AcNo1.Text.Trim() != "")
                        sql += " and acnoi>=N'" + AcNo1.Text.Trim() + "'";
                    if (AcNo2.Text.Trim() != "")
                        sql += " and acnoi<=N'" + AcNo2.Text.Trim() + "'";
                    dd = new SqlDataAdapter(sql, cn);
                    temp = new DataTable();
                    dd.Fill(temp);
                    for (int i = 0; i < temp.Rows.Count; i++)
                    {
                        dr = 本期tb.NewRow();
                        dr["帳戶編號"] = temp.Rows[i]["acnoi"].ToString().Trim();
                        dr["交易日期"] = Common.User_DateTime == 1 ? Date.AddLine(temp.Rows[i]["cadate"].ToString().Trim()) : Date.AddLine(temp.Rows[i]["cadate_1"].ToString().Trim());
                        dr["交易狀況"] = "轉    入";
                        dr["交易憑證"] = temp.Rows[i]["cano"].ToString().Trim();
                        dr["備註"] = temp.Rows[i]["camemo"].ToString().Trim();
                        dr["存入金額"] = temp.Rows[i]["camny1"].ToDecimal();
                        dr["領出金額"] = 0;
                        dr["餘額"] = 0;
                        dr["計算值"] =  temp.Rows[i]["camny1"].ToDecimal();
                        本期tb.Rows.Add(dr);
                        本期tb.AcceptChanges();
                    }
                    #endregion

                    #region 銀行轉帳作業-前期(轉出部分)
                    sql = "select 帳戶編號=acno,計算值=(-1)*camny,交易狀況='轉    出' from carry where '0'='0'"
                        + " and cadate < '" + Date.ToTWDate(date1.Text.Trim()) + "' and cadate >='" + Common.系統民國+"0101" + "' "
                        + " and cono=N'" + CoNo.Text.Trim() + "'" + 勾選帳戶sql;
                    if (AcNo1.Text.Trim() != "")
                        sql += " and acno>=N'" + AcNo1.Text.Trim() + "'";
                    if (AcNo2.Text.Trim() != "")
                        sql += " and acno<=N'" + AcNo2.Text.Trim() + "'";
                    dd = new SqlDataAdapter(sql, cn);
                    temp = new DataTable();
                    dd.Fill(temp);
                    前期tb.Merge(temp);
                    前期tb.AcceptChanges();
                    #endregion

                    #region 銀行轉帳作業-前期(轉入部分)
                    sql = "select 帳戶編號=acnoi,計算值=camny1,交易狀況='轉    入' from carry where '0'='0'"
                        + " and cadate < '" + Date.ToTWDate(date1.Text.Trim()) + "' and cadate >='" + Common.系統民國 + "0101" + "' "
                        + " and cono=N'" + CoNo.Text.Trim() + "'" + 勾選帳戶sql.Replace("acno","acnoi");
                    if (AcNo1.Text.Trim() != "")
                        sql += " and acnoi>=N'" + AcNo1.Text.Trim() + "'";
                    if (AcNo2.Text.Trim() != "")
                        sql += " and acnoi<=N'" + AcNo2.Text.Trim() + "'";
                    dd = new SqlDataAdapter(sql, cn);
                    temp = new DataTable();
                    dd.Fill(temp);
                    前期tb.Merge(temp);
                    前期tb.AcceptChanges();
                    #endregion

                    #region 帳戶匯入作業-本期
                    sql = "select * from remiti where '0'='0'"
                        + " and redate >= '" + Date.ToTWDate(date1.Text.Trim()) + "' and redate <='" + Date.ToTWDate(date2.Text.Trim()) + "' "
                        + " and cono=N'" + CoNo.Text.Trim() + "'" + 勾選帳戶sql;
                    if (AcNo1.Text.Trim() != "")
                        sql += " and acno>=N'" + AcNo1.Text.Trim() + "'";
                    if (AcNo2.Text.Trim() != "")
                        sql += " and acno<=N'" + AcNo2.Text.Trim() + "'";
                    dd = new SqlDataAdapter(sql, cn);
                    temp = new DataTable();
                    dd.Fill(temp);
                    for (int i = 0; i < temp.Rows.Count; i++)
                    {
                        dr = 本期tb.NewRow();
                        dr["帳戶編號"] = temp.Rows[i]["acno"].ToString().Trim();
                        dr["交易日期"] = Common.User_DateTime == 1 ? Date.AddLine(temp.Rows[i]["redate"].ToString().Trim()) : Date.AddLine(temp.Rows[i]["redate_1"].ToString().Trim());
                        dr["交易狀況"] = "匯    入";
                        dr["交易憑證"] = temp.Rows[i]["reno"].ToString().Trim();
                        dr["備註"] = temp.Rows[i]["rememo"].ToString().Trim();
                        dr["存入金額"] = temp.Rows[i]["remny"].ToDecimal();
                        dr["領出金額"] = 0;
                        dr["餘額"] = 0;
                        dr["計算值"] = temp.Rows[i]["remny"].ToDecimal();
                        本期tb.Rows.Add(dr);
                        本期tb.AcceptChanges();
                    }
                    #endregion

                    #region 帳戶匯入作業-前期
                    sql = "select 帳戶編號=acno,計算值=remny,交易狀況='匯    入' from remiti where '0'='0'"
                        + " and redate < '" + Date.ToTWDate(date1.Text.Trim()) + "' and redate >='" + Common.系統民國+"0101" + "' "
                        + " and cono=N'" + CoNo.Text.Trim() + "'" + 勾選帳戶sql;
                    if (AcNo1.Text.Trim() != "")
                        sql += " and acno>=N'" + AcNo1.Text.Trim() + "'";
                    if (AcNo2.Text.Trim() != "")
                        sql += " and acno<=N'" + AcNo2.Text.Trim() + "'";
                    dd = new SqlDataAdapter(sql, cn);
                    temp = new DataTable();
                    dd.Fill(temp);
                    前期tb.Merge(temp);
                    前期tb.AcceptChanges();
                    #endregion

                    #region 帳戶匯出作業-本期
                    sql = "select * from remito where '0'='0'"
                        + " and redate >= '" + Date.ToTWDate(date1.Text.Trim()) + "' and redate <='" + Date.ToTWDate(date2.Text.Trim()) + "' "
                        + " and cono=N'" + CoNo.Text.Trim() + "'" + 勾選帳戶sql;
                    if (AcNo1.Text.Trim() != "")
                        sql += " and acno>=N'" + AcNo1.Text.Trim() + "'";
                    if (AcNo2.Text.Trim() != "")
                        sql += " and acno<=N'" + AcNo2.Text.Trim() + "'";
                    dd = new SqlDataAdapter(sql, cn);
                    temp = new DataTable();
                    dd.Fill(temp);
                    for (int i = 0; i < temp.Rows.Count; i++)
                    {
                        dr = 本期tb.NewRow();
                        dr["帳戶編號"] = temp.Rows[i]["acno"].ToString().Trim();
                        dr["交易日期"] = Common.User_DateTime == 1 ? Date.AddLine(temp.Rows[i]["redate"].ToString().Trim()) : Date.AddLine(temp.Rows[i]["redate_1"].ToString().Trim());
                        dr["交易狀況"] = "匯    出";
                        dr["交易憑證"] = temp.Rows[i]["reno"].ToString().Trim();
                        dr["備註"] = temp.Rows[i]["rememo"].ToString().Trim();
                        dr["存入金額"] = 0;
                        dr["領出金額"] = temp.Rows[i]["remny"].ToDecimal() + temp.Rows[i]["chargemny"].ToDecimal();
                        dr["餘額"] = 0;
                        dr["計算值"] = (-1) * (temp.Rows[i]["remny"].ToDecimal() + temp.Rows[i]["chargemny"].ToDecimal());
                        本期tb.Rows.Add(dr);
                        本期tb.AcceptChanges();
                    }
                    #endregion

                    #region 帳戶匯出作業-前期
                    sql = "select 帳戶編號=acno,計算值=(-1)*(remny+chargemny),交易狀況='匯    出' from remito where '0'='0'"
                        + " and redate < '" + Date.ToTWDate(date1.Text.Trim()) + "' and redate >='" + Common.系統民國 + "0101" + "' "
                        + " and cono=N'" + CoNo.Text.Trim() + "'" + 勾選帳戶sql;
                    if (AcNo1.Text.Trim() != "")
                        sql += " and acno>=N'" + AcNo1.Text.Trim() + "'";
                    if (AcNo2.Text.Trim() != "")
                        sql += " and acno<=N'" + AcNo2.Text.Trim() + "'";
                    dd = new SqlDataAdapter(sql, cn);
                    temp = new DataTable();
                    dd.Fill(temp);
                    前期tb.Merge(temp);
                    前期tb.AcceptChanges();
                    #endregion

                    string acname1 = "", acname = "",acact="";
                    DataTable Sendtb = new DataTable();
                    acno_list.ForEach(r =>
                    {
                        acname1 = ""; acname = ""; acact = "";
                        temp.Clear();
                        temp = 前期tb.AsEnumerable().Where(t => t["帳戶編號"].ToString().Trim() == r).CopyToDataTable();
                        SqlCommand cmd = cn.CreateCommand();
                        cmd.CommandText = "select acname1,acname,acact from acct where acno='" + r + "'";
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.HasRows && reader.Read())
                            {
                                acname1 = reader["acname1"].ToString();
                                acname = reader["acname"].ToString();
                                acact = reader["acact"].ToString();
                            }
                        }
                        if (temp.Rows.Count > 0)
                        {
                            dr = 本期tb.NewRow();
                            dr["帳戶編號"] = temp.Rows[0]["帳戶編號"].ToString().Trim();
                            dr["帳戶名稱"] = acname;
                            dr["帳戶簡稱"] = acname1;
                            dr["帳號"] = acact;
                            dr["交易日期"] = Common.User_DateTime == 1 ? "   /  /  " : "    /  /  ";
                            dr["交易狀況"] = "上期餘額";
                            dr["存入金額"] = 0;
                            dr["領出金額"] = 0;
                            dr["餘額"] = temp.AsEnumerable().Sum(t => t["計算值"].ToDecimal());
                            本期tb.Rows.Add(dr);
                            本期tb.AcceptChanges();
                        }

                        temp = 本期tb.AsEnumerable().Where(t => t["帳戶編號"].ToString().Trim() == r).OrderBy(t => t["交易日期"].ToString()).ThenBy(t => t["交易狀況"].ToString()).ThenBy(t => t["備註"].ToString()).CopyToDataTable();
                        for (int i = 1; i < temp.Rows.Count; i++)
                        {
                             temp.Rows[i]["餘額"] = Math.Round(temp.Rows[i-1]["餘額"].ToDecimal() + temp.Rows[i]["計算值"].ToDecimal(),Common.金額小數,MidpointRounding.AwayFromZero);
                        }
                        Sendtb.Merge(temp);
                        Sendtb.AcceptChanges();
                    });
                    using (銀行對帳報表_瀏覽 frm = new 銀行對帳報表_瀏覽())
                    {
                        frm.SetParaeter();
                        frm.acno_list = acno_list;
                        frm.table = Sendtb;
                        frm.date1.Text = date1.Text.Trim();
                        frm.date2.Text = date2.Text.Trim();
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

        private void date1_Validating(object sender, CancelEventArgs e)
        {
            if (btnExit.Focused) return;
            TextBox tb = sender as TextBox;
            if (tb.Text.Trim() == "")
            {
                MessageBox.Show("日期不可為空", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                e.Cancel = true;
                return;
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
