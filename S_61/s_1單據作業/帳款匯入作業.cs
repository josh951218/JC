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
using S_61.s_3基本資料;

namespace S_61.s_1單據作業
{
    public partial class 帳款匯入作業 : FormT
    {
        DataTable tbM = new DataTable();
        List<DataRow> list = new List<DataRow>();
        List<btnT> SC;
        List<btnT> Others;
        List<TextBox> Txt;
        DataRow dr;
        string btnState = "";
        string CurrentRow = "";
        string BeforeText;
        SqlTransaction tran;

        public 帳款匯入作業()
        {
            InitializeComponent();
            Txt = new List<TextBox> { ReNo, CoNo, CoName1, CoName2, ReDate, AcNo, AcName, AcName1, Xa1Name , Xa1Par, ReMny, ReMemo,CuNo,CuName1,CuName2 };
            SC = new List<btnT> { btnSave, btnCancel };
            Others = new List<btnT> { btnTop, btnPrior, btnNext, btnBottom, btnAppend, btnModify, btnDelete, btnExit, btnDuplicate, btnBrow, btnPrint };
            ReMny.NumThousands = true;
            ReMny.NumLast = Common.金額小數;
            ReMny.NumFirst = (20 - 1 - Common.金額小數);
        }

        private void 帳款匯入作業_Load(object sender, EventArgs e)
        {
            if (Common.單據異動 == "2") CoNo.Enabled = false;
            Common.取得浮動連線字串(Common.使用者預設公司);
            if (Common.User_DateTime == 1) ReDate.MaxLength = 7;
            else ReDate.MaxLength = 8;
            ReDate.Init();
            SC.ForEach(r => r.Enabled = false);
            Others.ForEach(r => r.Enabled = true);
            loadM();
            if (list.Count > 0)
                WriteToTxt(list.Last());

            if (Common.自動產生匯率 == 1) CHK.獲得台灣銀行牌告匯率();
        }

        void loadM()
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(Common.sqlConnString))
                {
                    cn.Open();
                    tbM.Clear();
                    list.Clear();
                    SqlDataAdapter dd = new SqlDataAdapter("select * from remiti order by reno", cn);
                    dd.Fill(tbM);
                    if (tbM.Rows.Count > 0) list = tbM.AsEnumerable().ToList();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        DataRow GetDataRow()
        {
            return list.Find(r => r["ReNo"].ToString() == ReNo.Text.Trim());
        }

        DataRow GetDataRow(string str)
        {
            return list.Find(r => r["ReNo"].ToString() == str.Trim());
        }

        void WriteToTxt(DataRow dr)
        {
            if (dr == null)
            {
                Txt.ForEach(r => r.Text = "");
            }
            else
            {
                Txt.ForEach(r =>
                {
                    if (r is TextBox)
                    {
                        if (r.Name.ToString() == "ReMny")
                            r.Text = dr[r.Name.ToString()].ToDecimal().ToString("N" + Common.金額小數);
                        else
                        {
                            if (r.Name == "ReDate")
                                r.Text = Common.User_DateTime == 1 ? dr["ReDate"].ToString() : dr["ReDate_1"].ToString();
                            else
                                r.Text = dr[r.Name.ToString()].ToString();
                        }
                    }
                });
                CurrentRow = dr["ReNo"].ToString();
            }
        }

        private void btnTop_Click(object sender, EventArgs e)
        {
            loadM();
            if (list.Count > 0)
            {
                dr = list.First();
                WriteToTxt(dr);
            }
            btnTop.Enabled = btnPrior.Enabled = false;
            btnNext.Enabled = btnBottom.Enabled = true;
        }

        private void btnPrior_Click(object sender, EventArgs e)
        {
            dr = GetDataRow();
            int temp = list.IndexOf(dr);
            loadM();
            if (list.Count > 0)
            {
                dr = GetDataRow(CurrentRow);
                int i = list.IndexOf(dr);
                if (i == -1)
                {
                    if (temp == 0)
                    {
                        MessageBox.Show("已最上一筆", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        WriteToTxt(list.First());
                        btnTop.Enabled = btnPrior.Enabled = false;
                        btnNext.Enabled = btnBottom.Enabled = true;
                    }
                    else
                    {
                        WriteToTxt(list[--temp]);
                        btnTop.Enabled = btnPrior.Enabled = btnNext.Enabled = btnBottom.Enabled = true;
                    }
                }
                if (i > 0)
                {
                    WriteToTxt(list[--i]);
                    btnTop.Enabled = btnPrior.Enabled = btnNext.Enabled = btnBottom.Enabled = true;
                }
                if (i == 0)
                {
                    MessageBox.Show("已最上一筆", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    WriteToTxt(list.First());
                    btnTop.Enabled = btnPrior.Enabled = false;
                    btnNext.Enabled = btnBottom.Enabled = true;
                }
            }
            else
            {
                WriteToTxt(null);
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            dr = GetDataRow();
            int temp = list.LastIndexOf(dr);
            loadM();
            if (list.Count > 0)
            {
                dr = GetDataRow(CurrentRow);
                int i = list.LastIndexOf(dr);
                if (i == -1)
                {
                    if (temp >= list.Count - 1)
                    {
                        MessageBox.Show("已最下一筆", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        WriteToTxt(list.Last());
                        btnTop.Enabled = btnPrior.Enabled = true;
                        btnNext.Enabled = btnBottom.Enabled = false;
                    }
                    else
                    {
                        WriteToTxt(list[++temp]);
                        btnTop.Enabled = btnPrior.Enabled = btnNext.Enabled = btnBottom.Enabled = true;
                    }
                }
                if (i < list.Count - 1)
                {
                    WriteToTxt(list[++i]);
                    btnTop.Enabled = btnPrior.Enabled = btnNext.Enabled = btnBottom.Enabled = true;
                }
                else
                {
                    MessageBox.Show("已最下一筆", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    WriteToTxt(list.Last());
                    btnTop.Enabled = btnPrior.Enabled = true;
                    btnNext.Enabled = btnBottom.Enabled = false;
                }
            }
            else
            {
                WriteToTxt(null);
            }
        }

        private void btnBottom_Click(object sender, EventArgs e)
        {
            loadM();
            if (list.Count > 0)
            {
                dr = list.Last();
                WriteToTxt(dr);
            }
            btnNext.Enabled = btnBottom.Enabled = false;
            btnTop.Enabled = btnPrior.Enabled = true;
        }

        private void btnAppend_Click(object sender, EventArgs e)
        {
            btnState = "Append";
            SC.ForEach(r => r.Enabled = true);
            Others.ForEach(r => r.Enabled = false);
            Txt.ForEach(r =>
            {
                if (r is TextBox)
                {
                    r.Text = "";
                    if (r.Name.ToString() != "CoName1")
                        if (r.Name.ToString() != "AcName")
                            if (r.Name.ToString() != "Xa1Name")
                                if (r.Name.ToString() != "CuName2")
                                    ((TextBox)r).ReadOnly = false;
                    if (r is txtNumber) r.Text = "0";
                }
            });
            CoNo.Text = Common.使用者預設公司;
            CHK.CoNo_Validating(CoNo, CoName1, CoName2);
            Common.取得浮動連線字串(CoNo.Text.Trim());
            ReDate.Text = Date.GetDateTime(Common.User_DateTime, false);
            ReNo.Focus();
        }

        private void btnDuplicate_Click(object sender, EventArgs e)
        {
            if (tbM.Rows.Count == 0)
            {
                MessageBox.Show("空資料庫，請先新增", "視窗訊息", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (Common.單據異動 == "2")
            {
                if (CoNo.Text.Trim() != Common.使用者預設公司)
                {
                    MessageBox.Show("無權更改此公司資料", "視窗訊息", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
            Common.取得浮動連線字串(CoNo.Text.Trim());
            btnState = "Duplicate";
            SC.ForEach(r => r.Enabled = true);
            Others.ForEach(r => r.Enabled = false);
            Txt.ForEach(r =>
            {
                if (r is TextBox)
                {
                    if (r.Name.ToString() != "CoName1")
                        if (r.Name.ToString() != "AcName")
                            if (r.Name.ToString() != "Xa1Name")
                                if (r.Name.ToString() != "CuName2")
                                ((TextBox)r).ReadOnly = false;
                }
            });
            ReDate.Text = Date.GetDateTime(Common.User_DateTime);
            ReNo.Text = "";
            ReNo.Focus();
        }

        private void btnModify_Click(object sender, EventArgs e)
        {
            if (tbM.Rows.Count == 0)
            {
                MessageBox.Show("空資料庫，請先新增", "視窗訊息", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            //DataRow ac = list.Find(r => r["ReNo"].ToString().Trim() == ReNo.Text.ToString().Trim());
            DataRow ac = Common.load("Check", "remiti", "reno", ReNo.Text.Trim());
            if (ac != null && ac["reacno"].ToString().Trim() != "")
            {
                MessageBox.Show("此票據已傳輸至會計傳票，無法異動！\n公司編號:" + ac["accono"].ToString().Trim() + " 傳票編號:" + ac["reacno"].ToString().Trim(), "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (Common.單據異動 == "2")
            {
                if (CoNo.Text.Trim() != Common.使用者預設公司)
                {
                    MessageBox.Show("無權更改此公司資料", "視窗訊息", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
            Common.取得浮動連線字串(CoNo.Text.Trim());
            btnState = "Modify";
            SC.ForEach(r => r.Enabled = true);
            Others.ForEach(r => r.Enabled = false);
            Txt.ForEach(r =>
            {
                if (r is TextBox)
                {
                    if (r.Name.ToString() != "CoName1")
                        if (r.Name.ToString() != "AcName")
                            if (r.Name.ToString() != "Xa1Name")
                                if (r.Name.ToString() != "CuName2")
                                ((TextBox)r).ReadOnly = false;
                }
            });
            CoName1.ReadOnly = AcName.ReadOnly = true;
            ReDate.Text = Date.GetDateTime(Common.User_DateTime);
            進銷存轉入匯入();
            ReNo.Focus();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (tbM.Rows.Count == 0)
            {
                MessageBox.Show("空資料庫，請先新增", "視窗訊息", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            //DataRow ac = list.Find(r => r["ReNo"].ToString().Trim() == ReNo.Text.ToString().Trim());
            DataRow ac = Common.load("Check", "remiti", "reno", ReNo.Text.Trim());
            if (ac != null && ac["reacno"].ToString().Trim() != "")
            {
                MessageBox.Show("此票據已傳輸至會計傳票，無法異動！\n公司編號:" + ac["accono"].ToString().Trim() + " 傳票編號:" + ac["reacno"].ToString().Trim(), "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (Common.單據異動 == "2")
            {
                if (CoNo.Text.Trim() != Common.使用者預設公司)
                {
                    MessageBox.Show("無權更改此公司資料", "視窗訊息", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
            DataRow T = list.Find(r => r["reno"].ToString().Trim() == ReNo.Text.Trim());
            if (T["restno"].ToString().Trim() != "")
            {
                MessageBox.Show("此單據由進銷存沖款單轉入，無法刪除!!\n沖款單號:" + T["restno"].ToString(), "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                ReMemo.Focus();
                return;
            }
            if (MessageBox.Show("請確定是否刪除此筆記錄?", "確認視窗", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Cancel)
            {
                return;
            }
            else
            {
                Common.取得浮動連線字串(CoNo.Text.Trim());
                //執行刪除指令前，檢查此筆資料是否已被別人刪除
                loadM();
                dr = GetDataRow();
                if (list.IndexOf(dr) == -1)
                {
                    if (list.Count > 0)
                    {
                        dr = list.Last();
                        WriteToTxt(dr);
                    }
                    else
                    {
                        dr = null;
                        WriteToTxt(dr);
                    }
                    return;//資料已被刪除，以下程式碼不執行
                }
                try
                {
                    using (SqlConnection cn = new SqlConnection(Common.sqlConnString))
                    {
                        cn.Open();
                        SqlCommand cmd = cn.CreateCommand();

                        tran = cn.BeginTransaction();
                        cmd.Transaction = tran;
                        DataRow temp = GetDataRow(ReNo.Text.Trim());
                        cmd.CommandText = "update acct set acmny1-=" + temp["ReMny"].ToDecimal() + " where acno=N'" + temp["AcNo"].ToString().Trim() + "';";

                        cmd.CommandText += "delete from remiti where ReNo=N'"
                            + ReNo.Text.Trim() + "' COLLATE Chinese_Taiwan_Stroke_BIN";
                        cmd.ExecuteNonQuery();

                        tran.Commit(); tran.Dispose();
                        cmd.Dispose();
                        dr = GetDataRow(CurrentRow);
                        int i = list.IndexOf(dr);
                        loadM();
                        if (list.Count > 0)
                        {
                            if (i >= list.Count - 1)
                            {
                                WriteToTxt(list.Last());
                                MessageBox.Show("已最下一筆", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                            else
                            {
                                WriteToTxt(list[i]);
                            }
                        }
                        else
                        {
                            WriteToTxt(null);
                        }
                    }
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    MessageBox.Show("DelError:\n" + ex.ToString());
                }
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (tbM.Rows.Count == 0)
            {
                MessageBox.Show("空資料庫，請先新增", "視窗訊息", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            using (帳款匯入作業_列印 frm = new 帳款匯入作業_列印())
            {
                frm.SetParaeter();
                frm.ShowDialog();
            }
        }

        private void btnBrow_Click(object sender, EventArgs e)
        {
            if (tbM.Rows.Count == 0)
            {
                MessageBox.Show("空資料庫，請先新增", "視窗訊息", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            using (帳款匯入作業_瀏覽 frm = new 帳款匯入作業_瀏覽())
            {
                frm.SetParaeter();
                frm.SeekNo = ReNo.Text.Trim();
                frm.ShowDialog();
                switch (frm.DialogResult)
                {
                    case DialogResult.OK:
                        if (frm.Result != "")
                        {
                            loadM();
                            dr = GetDataRow(frm.Result);
                            WriteToTxt(dr);
                        }
                        break;
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (AcNo.Text.Trim() == "")
            {
                MessageBox.Show("『帳戶編號』不可為空", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                AcNo.Focus();
                return;
            }
            if (CuNo.Text.Trim() == "")
            {
                MessageBox.Show("『客戶編號』不可為空", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                CuNo.Focus();
                return;
            }
            if (ReMny.Text.ToDecimal() == 0)
            {
                MessageBox.Show("『金額』不可為零", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                ReMny.Focus();
                return;
            }
            if (btnState == "Append" || btnState == "Duplicate")
            {
                if (!自動產生編號())
                {
                    MessageBox.Show("『匯入證號』重複", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    ReNo.Focus();
                    return;
                }
                using (SqlConnection cn = new SqlConnection(Common.sqlConnString))
                {
                    cn.Open();
                    SqlCommand cmd = cn.CreateCommand();
                    tran = cn.BeginTransaction();
                    cmd.Transaction = tran;
                    try
                    {
                        cmd.CommandText = "insert into remiti (reno,cono,coname1,coname2,cuno,cuname1,cuname2,redate,redate_1,acno,acname,acname1,remny,rememo,xa1name)values("
                                        + " N'" + ReNo.Text.Trim() + "',"
                                        + " N'" + CoNo.Text.Trim() + "',"
                                        + " N'" + CoName1.Text.Trim() + "',"
                                        + " N'" + CoName2.Text.Trim() + "',"
                                        + " N'" + CuNo.Text.Trim() + "',"
                                        + " N'" + CuName1.Text.Trim() + "',"
                                        + " N'" + CuName2.Text.Trim() + "',"
                                        + " N'" + Date.ToTWDate(ReDate.Text.Trim()) + "',"
                                        + " N'" + Date.ToUSDate(ReDate.Text.Trim()) + "',"
                                        + " N'" + AcNo.Text.Trim() + "',"
                                        + " N'" + AcName.Text.Trim() + "',"
                                        + " N'" + AcName1.Text.Trim() + "',"
                                        + " " + ReMny.Text.ToDecimal() + ","
                                        + " N'" + ReMemo.Text.Trim() + "',"
                                        + " N'" + Xa1Name.Text.Trim() + "');";
                        cmd.CommandText += "update acct set acmny1+=" + ReMny.Text.ToDecimal() + " where acno=N'" + AcNo.Text.Trim() + "';";
                        cmd.ExecuteNonQuery();
                        tran.Commit();
                        tran.Dispose();
                        cmd.Dispose();
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        MessageBox.Show(ex.ToString());
                        return;
                    }
                    CurrentRow = ReNo.Text.Trim();
                    btnAppend_Click(null, null);
                }
            }
            if (btnState == "Modify")
            {
                loadM();
                dr = GetDataRow(CurrentRow);
                int index = list.IndexOf(dr);
                if (index == -1)
                {
                    MessageBox.Show("此筆資料已被刪除", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    ReNo.Focus();
                    return;
                }
                using (SqlConnection cn = new SqlConnection(Common.sqlConnString))
                {
                    cn.Open();
                    SqlCommand cmd = cn.CreateCommand();
                    tran = cn.BeginTransaction();
                    cmd.Transaction = tran;
                    try
                    {
                        DataRow temp = GetDataRow(CurrentRow);
                        cmd.CommandText = "update acct set acmny1-=" + temp["ReMny"].ToDecimal() + " where acno=N'" + temp["AcNo"].ToString().Trim() + "'";
                        cmd.ExecuteNonQuery();
                        cmd.CommandText = "delete remiti where reno=N'" + CurrentRow + "'";
                        cmd.ExecuteNonQuery();
                        cmd.CommandText = "insert into remiti (restno,chstnum,reno,cono,coname1,coname2,cuno,cuname1,cuname2,redate,redate_1,acno,acname,acname1,remny,rememo,xa1name)values("
                                        + " N'" + dr["restno"].ToString().Trim() + "',"
                                        + " N'" + dr["chstnum"].ToString().Trim() + "',"
                                        + " N'" + ReNo.Text.Trim() + "',"
                                        + " N'" + CoNo.Text.Trim() + "',"
                                        + " N'" + CoName1.Text.Trim() + "',"
                                        + " N'" + CoName2.Text.Trim() + "',"
                                        + " N'" + CuNo.Text.Trim() + "',"
                                        + " N'" + CuName1.Text.Trim() + "',"
                                        + " N'" + CuName2.Text.Trim() + "',"
                                        + " N'" + Date.ToTWDate(ReDate.Text.Trim()) + "',"
                                        + " N'" + Date.ToUSDate(ReDate.Text.Trim()) + "',"
                                        + " N'" + AcNo.Text.Trim() + "',"
                                        + " N'" + AcName.Text.Trim() + "',"
                                        + " N'" + AcName1.Text.Trim() + "',"
                                        + " " + ReMny.Text.ToDecimal() + ","
                                        + " N'" + ReMemo.Text.Trim() + "',"
                                        + " N'" + Xa1Name.Text.Trim() + "');";
                        cmd.CommandText += "update acct set acmny1+=" + ReMny.Text.ToDecimal() + " where acno=N'" + AcNo.Text.Trim() + "';";
                        cmd.ExecuteNonQuery();
                        tran.Commit();
                        tran.Dispose();
                        cmd.Dispose();
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        MessageBox.Show(ex.ToString());
                        return;
                    }
                    CurrentRow = ReNo.Text.Trim();
                    Txt.ForEach(r =>
                    {
                        if (r is TextBox) ((TextBox)r).Text = "";
                        if (r is txtNumber) ((TextBox)r).Text = "0";
                    });
                    ReNo.Focus();
                    btnState = "Modify";
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Common.取得浮動連線字串(Common.使用者預設公司);
            btnState = "";
            SC.ForEach(r => r.Enabled = false);
            Others.ForEach(r => r.Enabled = true);
            Txt.ForEach(r =>
            {
                if (r is TextBox) ((TextBox)r).ReadOnly = true;
            });
            loadM();
            if (list.Count > 0)
            {
                dr = GetDataRow(CurrentRow);
                WriteToTxt(dr);
            }
            else
            {
                WriteToTxt(null);
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Dispose();
            this.Close();
        }

        private void CoNo_DoubleClick(object sender, EventArgs e)
        {
            CHK.CoNo_OpemFrm(CoNo, CoName1, CoName2);
        }

        private void CoNo_Validating(object sender, CancelEventArgs e)
        {
            if (CoNo.ReadOnly || btnCancel.Focused) return;
            if (CoNo.Text.Trim() == "")
            {
                CoName1.Text = CoName2.Text = "";
                e.Cancel = true;
                MessageBox.Show("『公司號碼』不可為空白，請確定後重新輸入", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!CHK.CoNo_Validating(CoNo, CoName1, CoName2))
            {
                e.Cancel = true;
                CHK.CoNo_OpemFrm(CoNo, CoName1, CoName2);
            }
            else
            {
                Common.取得浮動連線字串(CoNo.Text.Trim());
                if (CuNo.Text != "")
                {
                    if (!CHK.CuNo_Validating(Common.浮動連線字串, CuNo, CuName1, CuName2))
                        CuNo.Text = CuName1.Text = CuName2.Text = "";
                }
                if (!CHK.AcNo_Validating(CoNo.Text.Trim(), AcNo, AcName1, AcName))
                    AcNo.Text = AcName1.Text = AcName.Text = "";
            }
        }

        private void ReDate_Validating(object sender, CancelEventArgs e)
        {
            if (btnCancel.Focused || ReDate.ReadOnly) return;
            if (ReDate.Text.Trim() == "")
            {
                MessageBox.Show("『匯入日期』不可為空白，請確定後重新輸入", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                e.Cancel = true;
                return;
            }
            if (!ReDate.IsDateTime())
            {
                MessageBox.Show("日期格式錯誤，請確定後重新輸入", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                e.Cancel = true;
                return;
            }
            if (!CHK.稽核會計年度(ReDate.Text.Trim())) e.Cancel = true;
        }

        private void CuNo_DoubleClick(object sender, EventArgs e)
        {
            CHK.CuNo_OpemFrm(CuNo, CuName1, CuName2);
        }

        private void CuNo_Validating(object sender, CancelEventArgs e)
        {
            if (btnCancel.Focused || CuNo.ReadOnly) return;
            if (CuNo.Text.Trim() == "")
            {
                MessageBox.Show("『客戶號碼』不可為空白，請確定後重新輸入", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                e.Cancel = true;
                return;
            }
            if (!CHK.CuNo_Validating(Common.浮動連線字串, CuNo, CuName1, CuName2))
            {
                e.Cancel = true;
                CHK.CuNo_OpemFrm(CuNo, CuName1, CuName2);
            }
        }

        private void AcNo_DoubleClick(object sender, EventArgs e)
        {
            CHK.AcNo_OpemFrm(CoNo.Text.Trim(),AcNo, AcName1, AcName);
            using (SqlConnection cn = new SqlConnection(Common.sqlConnString))
            {
                cn.Open();
                SqlCommand cmd = cn.CreateCommand();
                cmd.CommandText = "select xa1no,xa1name,xa1par from acct where acno='" + AcNo.Text.Trim() + "'";
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows && reader.Read())
                    {
                        Xa1Name.Text = reader["Xa1Name"].ToString().Trim();
                        if (Common.自動產生匯率 == 1)
                        {
                            DataRow temp = CHK.銀行牌告匯率dt.AsEnumerable().ToList().Find(r => r["幣別編號"].ToString().Trim() == reader["xa1no"].ToString().Trim());
                            if (temp != null)
                                Xa1Par.Text = Math.Round(temp["匯率"].ToDecimal(), 4, MidpointRounding.AwayFromZero).ToString("f4");
                            else
                                Xa1Par.Text = reader["xa1par"].ToDecimal().ToString("f4");
                        }
                        else
                        {
                            Xa1Par.Text = reader["xa1par"].ToDecimal().ToString("f4");
                        }
                    }
                }
            }
        }

        private void AcNo_Validating(object sender, CancelEventArgs e)
        {
            if (AcNo.ReadOnly || btnCancel.Focused) return;
            if (AcNo.Text.Trim() == "")
            {
                AcName.Text = AcName1.Text = Xa1Name.Text = "";
                e.Cancel = true;
                MessageBox.Show("『帳戶編號』不可為空白，請確定後重新輸入", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!CHK.AcNo_Validating(CoNo.Text.Trim(), AcNo, AcName1, AcName))
            {
                e.Cancel = true;
                CHK.AcNo_OpemFrm(CoNo.Text.Trim(), AcNo, AcName1, AcName);
            }
            else
            {
                using (SqlConnection cn = new SqlConnection(Common.sqlConnString))
                {
                    cn.Open();
                    SqlCommand cmd = cn.CreateCommand();
                    cmd.CommandText = "select xa1no,xa1name,xa1par from acct where acno='" + AcNo.Text.Trim() + "'";
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows && reader.Read())
                        {
                            Xa1Name.Text = reader["Xa1Name"].ToString().Trim();
                            if (Common.自動產生匯率 == 1)
                            {
                                DataRow temp = CHK.銀行牌告匯率dt.AsEnumerable().ToList().Find(r => r["幣別編號"].ToString().Trim() == reader["xa1no"].ToString().Trim());
                                if (temp != null)
                                    Xa1Par.Text = Math.Round(temp["匯率"].ToDecimal(), 4, MidpointRounding.AwayFromZero).ToString("f4");
                                else
                                    Xa1Par.Text = reader["xa1par"].ToDecimal().ToString("f4");
                            }
                            else
                            {
                                Xa1Par.Text = reader["xa1par"].ToDecimal().ToString("f4");
                            }
                        }
                    }
                }
            }
        }

        private void ReMemo_DoubleClick(object sender, EventArgs e)
        {
            CHK.Memo_OpemFrm(ReMemo);
        }

        private void 帳款匯入作業_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.D1:
                case Keys.NumPad1:
                    btnAppend.Focus();
                    btnAppend.PerformClick();
                    break;
                case Keys.D2:
                case Keys.NumPad2:
                    btnModify.Focus();
                    btnModify.PerformClick();
                    break;
                case Keys.D3:
                case Keys.NumPad3:
                    btnDelete.PerformClick();
                    break;
                case Keys.D0:
                case Keys.NumPad0:
                case Keys.F11:
                    btnExit.PerformClick();
                    break;
                case Keys.Home:
                    btnTop.PerformClick();
                    break;
                case Keys.PageUp:
                    btnPrior.PerformClick();
                    break;
                case Keys.PageDown:
                    btnNext.PerformClick();
                    break;
                case Keys.End:
                    btnBottom.PerformClick();
                    break;
                case Keys.F9:
                    btnSave.PerformClick();
                    break;
                case Keys.F4:
                    btnCancel.Focus();
                    btnCancel.PerformClick();
                    break;
            }
        }

        bool 自動產生編號()
        {
            DataTable table = new DataTable();
            string date = "", count = "1";
            if (Common.傳票編碼方式 == 1)//民國年月日+流水號
            {
                date = Date.ToTWDate(ReDate.Text.Trim());
                count = count.PadLeft(3, '0');
            }
            else if (Common.傳票編碼方式 == 2)//民國年月+流水號
            {
                date = Date.ToTWDate(ReDate.Text.Trim());
                date = date.Substring(0, 5);
                count = count.PadLeft(5, '0');
            }
            else if (Common.傳票編碼方式 == 3)//西元年月日+流水號
            {
                date = Date.ToUSDate(ReDate.Text.Trim());
                count = count.PadLeft(2, '0');
            }
            else
            {
                date = Date.ToUSDate(ReDate.Text.Trim());
                date = date.Substring(0, 6);
                count = count.PadLeft(4, '0');
            }
            decimal No = (date + count).ToDecimal();
            try
            {
                if (ReNo.Text.Trim() == "")
                {
                    using (SqlConnection cn = new SqlConnection(Common.sqlConnString))
                    {
                        cn.Open();
                        string sql = "";
                        if (Common.傳票編碼方式 == 1)
                            sql = "select reno from remiti where left(reno,7)='" + Date.ToTWDate(ReDate.Text.ToString()) + "' order by reno desc";
                        else if (Common.傳票編碼方式 == 2)
                            sql = "select reno from remiti where left(reno,5)='" + Date.ToTWDate(ReDate.Text.ToString()).Substring(0, 5) + "' order by reno desc";
                        else if (Common.傳票編碼方式 == 3)
                            sql = "select reno from remiti where left(reno,8)='" + Date.ToUSDate(ReDate.Text.ToString()) + "' order by reno desc";
                        else
                            sql = "select reno from remiti where left(reno,6)='" + Date.ToUSDate(ReDate.Text.ToString()).Substring(0, 6) + "' order by reno desc";
                        SqlDataAdapter dd = new SqlDataAdapter(sql, cn);
                        dd.Fill(table);
                        if (table.Rows.Count == 0)
                        {
                            ReNo.Text = No.ToString().Trim();
                            return true;
                        }
                        else
                        {
                            No = table.Rows[0]["reno"].ToDecimal() + 1;
                            ReNo.Text = No.ToString().Trim();
                        }
                    }
                }
                else
                {
                    using (SqlConnection cn = new SqlConnection(Common.sqlConnString))
                    {
                        cn.Open();
                        SqlCommand cmd = cn.CreateCommand();
                        cmd.CommandText = "select reno from remiti where reno='" + ReNo.Text.Trim() + "'";
                        if (cmd.ExecuteScalar().IsNullOrEmpty()) return true;
                        else return false;

                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return false;
            }
        }

        private void ReNo_DoubleClick(object sender, EventArgs e)
        {
            if (ReNo.ReadOnly != true)
            {
                using (帳款匯入作業_瀏覽 frm = new 帳款匯入作業_瀏覽())
                {
                    frm.SetParaeter(ViewMode.Normal);
                    frm.SeekNo = ReNo.Text.Trim();
                    frm.開窗模式 = true;
                    frm.ShowDialog();
                    switch (frm.DialogResult)
                    {
                        case DialogResult.OK:
                            if (frm.Result == null) return;
                            dr = GetDataRow(frm.Result.ToString().Trim());
                            WriteToTxt(dr);
                            CoNo.Focus();
                            break;
                    }
                }
            }
        }

        private void ReNo_Validating(object sender, CancelEventArgs e)
        {
            if (ReNo.ReadOnly) return;
            if (btnCancel.Focused) return;

            if (btnState == "Append")
            {
                loadM();
                dr = GetDataRow();
                int i = list.IndexOf(dr);
                if (i != -1)
                {
                    e.Cancel = true;
                    ReNo.Text = "";
                    MessageBox.Show("此匯入證號已經重複，請重新輸入", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }

            if (btnState == "Duplicate")
            {
                loadM();
                dr = GetDataRow();
                int i = list.IndexOf(dr);
                if (i != -1)
                {
                    e.Cancel = true;
                    ReNo.Text = "";
                    MessageBox.Show("此匯入證號已經重複，請重新輸入", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }

            if (btnState == "Modify")
            {
                loadM();
                dr = GetDataRow();
                int i = list.IndexOf(dr);
                if (i != -1)
                {
                    if (ReNo.Text.Trim() != BeforeText)
                    {
                        WriteToTxt(dr);
                        CoNo.Focus();
                    }

                }
                else
                {
                    e.Cancel = true;
                    ReNo.SelectAll();
                    dr = GetDataRow(CurrentRow);
                    //開瀏覽視窗
                    using (帳款匯入作業_瀏覽 frm = new 帳款匯入作業_瀏覽())
                    {
                        frm.SetParaeter(ViewMode.Normal);
                        frm.SeekNo = ReNo.Text.Trim();
                        frm.開窗模式 = true;
                        frm.ShowDialog();
                        switch (frm.DialogResult)
                        {
                            case DialogResult.OK:
                                dr = GetDataRow(frm.Result.ToString().Trim());
                                WriteToTxt(dr);
                                CoNo.Focus();
                                break;
                        }
                    }

                }
            }
        }

        private void ReNo_Enter(object sender, EventArgs e)
        {
            if (ReNo.ReadOnly) return;
            BeforeText = ReNo.Text.Trim();
        }

        void 進銷存轉入匯入()
        {
            dr = list.Find(r => r["reno"].ToString().Trim() == ReNo.Text.Trim());
            if (dr["restno"].ToString().Trim() != "")
            {
                CoNo.ReadOnly = Common.單據異動 == "1" ? false : true;
                CoName1.ReadOnly = true;
                CuNo.ReadOnly = CuName1.ReadOnly = CuName2.ReadOnly = true;
                ReMny.ReadOnly = true;
                ReMemo.ReadOnly = true;
            }
            else
            {
                CoNo.ReadOnly = CoName1.ReadOnly = false;
                CuNo.ReadOnly = CuName1.ReadOnly = CuName2.ReadOnly = false;
                ReMny.ReadOnly = false;
                ReMemo.ReadOnly = false;
            }
        }

    }
}
