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
    public partial class 銀行存提款作業 : FormT
    {
        DataTable tbM = new DataTable();
        List<DataRow> list = new List<DataRow>();
        List<btnT> SC;
        List<btnT> Others;
        List<Control> Txt;
        DataRow dr;
        string btnState = "";
        string CurrentRow = "";
        string BeforeText;
        SqlTransaction tran;

        public 銀行存提款作業()
        {
            InitializeComponent();
            Txt = new List<Control> { LoNo, CoNo, CoName1,CoName2, LoDate, AcNo, AcName, AcName1, Xa1Name, Xa1Par, LoMny,LoMemo,rd1,rd2,rd3 };
            SC = new List<btnT> { btnSave, btnCancel };
            Others = new List<btnT> { btnTop, btnPrior, btnNext, btnBottom, btnAppend, btnModify, btnDelete, btnExit, btnDuplicate, btnBrow, btnPrint };
            LoMny.NumThousands = true;
            LoMny.NumLast = Common.金額小數;
            LoMny.NumFirst = (20 - 1 - Common.金額小數);
        }

        private void 銀行存提款作業_Load(object sender, EventArgs e)
        {
            if (Common.單據異動 == "2") CoNo.Enabled = false;
            rd1.BackColor = rd2.BackColor = rd3.BackColor = Color.FromArgb(215, 227, 239);
            if (Common.User_DateTime == 1) LoDate.MaxLength = 7;
            else LoDate.MaxLength = 8;
            LoDate.Init();
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
                    SqlDataAdapter dd = new SqlDataAdapter("select * from lodgm order by lono", cn);
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
            return list.Find(r => r["LoNo"].ToString() == LoNo.Text.Trim());
        }

        DataRow GetDataRow(string str)
        {
            return list.Find(r => r["LoNo"].ToString() == str.Trim());
        }

        void WriteToTxt(DataRow dr)
        {
            if (dr == null)
            {
                Txt.ForEach(r =>
                {
                    if (r is TextBox) ((TextBox)r).Text = "";
                    if (r is radio) ((radio)r).Checked = false;
                });
            }
            else
            {
                Txt.ForEach(r =>
                {
                    if (r is TextBox)
                    {
                        if (r.Name.ToString() == "LoMny")
                            r.Text = dr[r.Name.ToString()].ToDecimal().ToString("N" + Common.金額小數);
                        else
                        {
                            if (r.Name == "LoDate")
                                r.Text = Common.User_DateTime == 1 ? dr["LoDate"].ToString() : dr["LoDate_1"].ToString();
                            else
                                r.Text = dr[r.Name.ToString()].ToString();
                        }
                    }
                    if (r is radio)
                    {
                        if (dr["LoKind"].ToDecimal() == 1) rd1.Checked = true;
                        if (dr["LoKind"].ToDecimal() == 2) rd2.Checked = true;
                        if (dr["LoKind"].ToDecimal() == 3) rd3.Checked = true;
                    }
                });
                CurrentRow = dr["LoNo"].ToString();
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
                                ((TextBox)r).ReadOnly = false;
                    if (r is txtNumber) r.Text = "0";
                }
                if (r is radio)
                {
                    ((radio)r).Enabled = true;
                }
            });
            rd1.Checked = true;
            CoNo.Text = Common.使用者預設公司;
            CHK.CoNo_Validating(CoNo, CoName1,CoName2);
            LoDate.Text = Date.GetDateTime(Common.User_DateTime, false);
            LoNo.Focus();
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
                                ((TextBox)r).ReadOnly = false;
                }
                if (r is radio)
                {
                    ((radio)r).Enabled = true;
                }
            });
            LoDate.Text = Date.GetDateTime(Common.User_DateTime);
            LoNo.Text = "";
            LoNo.Focus();
        }

        private void btnModify_Click(object sender, EventArgs e)
        {
            if (tbM.Rows.Count == 0)
            {
                MessageBox.Show("空資料庫，請先新增", "視窗訊息", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            //DataRow ac = list.Find(r => r["LoNo"].ToString().Trim() == LoNo.Text.ToString().Trim());
            DataRow ac = Common.load("Check", "lodgm", "lono", LoNo.Text.Trim());
            if (ac != null && ac["chacno"].ToString().Trim() != "")
            {
                MessageBox.Show("此票據已傳輸至會計傳票，無法異動！\n公司編號:" + ac["accono"].ToString().Trim() + " 傳票編號:" + ac["chacno"].ToString().Trim(), "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                                ((TextBox)r).ReadOnly = false;
                }
                if (r is radio)
                {
                    ((radio)r).Enabled = true;
                }
            });
            CoName1.ReadOnly = AcName.ReadOnly = true;
            LoDate.Text = Date.GetDateTime(Common.User_DateTime);
            LoNo.Focus();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (tbM.Rows.Count == 0)
            {
                MessageBox.Show("空資料庫，請先新增", "視窗訊息", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            //DataRow ac = list.Find(r => r["LoNo"].ToString().Trim() == LoNo.Text.ToString().Trim());
            DataRow ac = Common.load("Check", "lodgm", "lono", LoNo.Text.Trim());
            if (ac != null && ac["chacno"].ToString().Trim() != "")
            {
                MessageBox.Show("此票據已傳輸至會計傳票，無法異動！\n公司編號:" + ac["accono"].ToString().Trim() + " 傳票編號:" + ac["chacno"].ToString().Trim(), "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
            if (MessageBox.Show("請確定是否刪除此筆記錄?", "確認視窗", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Cancel)
            {
                return;
            }
            else
            {
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
                        DataRow temp = GetDataRow(LoNo.Text.Trim());
                        if (temp["lokind"].ToDecimal() == 1)
                            cmd.CommandText = "update acct set acmny1-=" + temp["LoMny"].ToDecimal() + " where acno=N'" + temp["AcNo"].ToString().Trim() + "';";
                        else if (temp["lokind"].ToDecimal() == 2)
                            cmd.CommandText = "update acct set acmny1+=" + temp["LoMny"].ToDecimal() + " where acno=N'" + temp["AcNo"].ToString().Trim() + "';";
                        else if (temp["lokind"].ToDecimal() == 3)
                            cmd.CommandText = "update acct set acmny1-=" + temp["LoMny"].ToDecimal() + " where acno=N'" + temp["AcNo"].ToString().Trim() + "';";

                        cmd.CommandText += "delete from lodgm where LoNo=N'"
                            + LoNo.Text.Trim() + "' COLLATE Chinese_Taiwan_Stroke_BIN";
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
            using (銀行存提款作業_列印 frm = new 銀行存提款作業_列印())
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
            using (銀行存提款作業_瀏覽 frm = new 銀行存提款作業_瀏覽())
            {
                frm.SetParaeter();
                frm.SeekNo = LoNo.Text.Trim();
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
            if (LoMny.Text.ToDecimal() == 0)
            {
                MessageBox.Show("『金額』不可為零", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                LoMny.Focus();
                return;
            }
            if (btnState == "Append" || btnState == "Duplicate")
            {
                if (!自動產生編號())
                {
                    MessageBox.Show("『存提證號』重複", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    LoNo.Focus();
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
                        cmd.CommandText = "insert into lodgm (lono,cono,coname1,coname2,lodate,lodate_1,acno,acname,acname1,lomny,lomemo,xa1name,xa1par,lokind,lokiname )values("
                                        + " N'" + LoNo.Text.Trim() + "',"
                                        + " N'" + CoNo.Text.Trim() + "',"
                                        + " N'" + CoName1.Text.Trim() + "',"
                                        + " N'" + CoName2.Text.Trim() + "',"
                                        + " N'" + Date.ToTWDate(LoDate.Text.Trim()) + "',"
                                        + " N'" + Date.ToUSDate(LoDate.Text.Trim()) + "',"
                                        + " N'" + AcNo.Text.Trim() + "',"
                                        + " N'" + AcName.Text.Trim() + "',"
                                        + " N'" + AcName1.Text.Trim() + "',"
                                        + " " + LoMny.Text.ToDecimal() + ","
                                        + " N'" + LoMemo.Text.Trim() + "',"
                                        + " N'" + Xa1Name.Text.Trim() + "',"
                                        + "  '" + Xa1Par.Text.Trim() + "',";
                        if(rd1.Checked)
                        {
                            cmd.CommandText+="1,'存款');";
                            cmd.CommandText += "update acct set acmny1+=" + LoMny.Text.ToDecimal() + " where acno=N'" + AcNo.Text.Trim() + "';";
                        }
                        else if (rd2.Checked)
                        {
                            cmd.CommandText += "2,'提款');";
                            cmd.CommandText += "update acct set acmny1-=" + LoMny.Text.ToDecimal() + " where acno=N'" + AcNo.Text.Trim() + "';";
                        }
                        else
                        {
                            cmd.CommandText += "3,'利息');";
                            cmd.CommandText += "update acct set acmny1+=" + LoMny.Text.ToDecimal() + " where acno=N'" + AcNo.Text.Trim() + "';";
                        }
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
                    CurrentRow = LoNo.Text.Trim();
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
                    LoNo.Focus();
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
                        if (temp["lokind"].ToDecimal() == 1)
                            cmd.CommandText = "update acct set acmny1-=" + temp["LoMny"].ToDecimal() + " where acno=N'" + temp["AcNo"].ToString().Trim() + "'";
                        else if (temp["lokind"].ToDecimal() == 2)
                            cmd.CommandText = "update acct set acmny1+=" + temp["LoMny"].ToDecimal() + " where acno=N'" + temp["AcNo"].ToString().Trim() + "'";
                        else if (temp["lokind"].ToDecimal() == 3)
                            cmd.CommandText = "update acct set acmny1-=" + temp["LoMny"].ToDecimal() + " where acno=N'" + temp["AcNo"].ToString().Trim() + "'";
                        cmd.ExecuteNonQuery();
                        cmd.CommandText = "delete lodgm where lono=N'" + CurrentRow + "'";
                        cmd.ExecuteNonQuery();
                        cmd.CommandText = "insert into lodgm (lono,cono,coname1,coname2,lodate,lodate_1,acno,acname,acname1,lomny,lomemo,xa1name,xa1par,lokind,lokiname )values("
                                        + " N'" + LoNo.Text.Trim() + "',"
                                        + " N'" + CoNo.Text.Trim() + "',"
                                        + " N'" + CoName1.Text.Trim() + "',"
                                        + " N'" + CoName2.Text.Trim() + "',"
                                        + " N'" + Date.ToTWDate(LoDate.Text.Trim()) + "',"
                                        + " N'" + Date.ToUSDate(LoDate.Text.Trim()) + "',"
                                        + " N'" + AcNo.Text.Trim() + "',"
                                        + " N'" + AcName.Text.Trim() + "',"
                                        + " N'" + AcName1.Text.Trim() + "',"
                                        + " " + LoMny.Text.ToDecimal() + ","
                                        + " N'" + LoMemo.Text.Trim() + "',"
                                        + " N'" + Xa1Name.Text.Trim() + "',"
                                        + "  '" + Xa1Par.Text.Trim() + "',";
                        if (rd1.Checked)
                        {
                            cmd.CommandText += "1,'存款');";
                            cmd.CommandText += "update acct set acmny1+=" + LoMny.Text.ToDecimal() + " where acno=N'" + AcNo.Text.Trim() + "';";
                        }
                        else if (rd2.Checked)
                        {
                            cmd.CommandText += "2,'提款');";
                            cmd.CommandText += "update acct set acmny1-=" + LoMny.Text.ToDecimal() + " where acno=N'" + AcNo.Text.Trim() + "';";
                        }
                        else
                        {
                            cmd.CommandText += "3,'利息');";
                            cmd.CommandText += "update acct set acmny1+=" + LoMny.Text.ToDecimal() + " where acno=N'" + AcNo.Text.Trim() + "';";
                        }
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
                    CurrentRow = LoNo.Text.Trim();
                    Txt.ForEach(r =>
                    {
                        if (r is TextBox) ((TextBox)r).Text = "";
                        if (r is txtNumber) ((TextBox)r).Text = "0";
                        rd1.Checked = true;
                    });
                    LoNo.Focus();
                    btnState = "Modify";
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            btnState = "";
            SC.ForEach(r => r.Enabled = false);
            Others.ForEach(r => r.Enabled = true);
            Txt.ForEach(r =>
            {
                if (r is TextBox) ((TextBox)r).ReadOnly = true;
                if (r is radio) ((radio)r).Enabled = false;
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
                if (!CHK.AcNo_Validating(CoNo.Text.Trim(), AcNo, AcName1, AcName))
                    AcNo.Text = AcName1.Text = AcName.Text = "";
            }
        }

        private void LoDate_Validating(object sender, CancelEventArgs e)
        {
            if (btnCancel.Focused || LoDate.ReadOnly) return;
            if (LoDate.Text.Trim() == "")
            {
                MessageBox.Show("『提款日期』不可為空白，請確定後重新輸入", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                e.Cancel = true;
                return;
            }
            if (!LoDate.IsDateTime())
            {
                MessageBox.Show("日期格式錯誤，請確定後重新輸入", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                e.Cancel = true;
                return;
            }
            if (!CHK.稽核會計年度(LoDate.Text.Trim())) e.Cancel = true;
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
            if(AcNo.Text.Trim() == "")
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

        private void LoMemo_DoubleClick(object sender, EventArgs e)
        {
            CHK.Memo_OpemFrm(LoMemo);
        }

        private void 銀行存提款作業_KeyUp(object sender, KeyEventArgs e)
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
                date = Date.ToTWDate(LoDate.Text.Trim());
                count = count.PadLeft(3, '0');
            }
            else if (Common.傳票編碼方式 == 2)//民國年月+流水號
            {
                date = Date.ToTWDate(LoDate.Text.Trim());
                date = date.Substring(0, 5);
                count = count.PadLeft(5, '0');
            }
            else if (Common.傳票編碼方式 == 3)//西元年月日+流水號
            {
                date = Date.ToUSDate(LoDate.Text.Trim());
                count = count.PadLeft(2, '0');
            }
            else
            {
                date = Date.ToUSDate(LoDate.Text.Trim());
                date = date.Substring(0, 6);
                count = count.PadLeft(4, '0');
            }
            decimal No = (date + count).ToDecimal();
            try
            {
                if (LoNo.Text.Trim() == "")
                {
                    using (SqlConnection cn = new SqlConnection(Common.sqlConnString))
                    {
                        cn.Open();
                        string sql = "";
                        if (Common.傳票編碼方式 == 1)
                            sql = "select lono from lodgm where left(lono,7)='" + Date.ToTWDate(LoDate.Text.ToString()) + "' order by lono desc";
                        else if (Common.傳票編碼方式 == 2)
                            sql = "select lono from lodgm where left(lono,5)='" + Date.ToTWDate(LoDate.Text.ToString()).Substring(0, 5) + "' order by lono desc";
                        else if (Common.傳票編碼方式 == 3)
                            sql = "select lono from lodgm where left(lono,8)='" + Date.ToUSDate(LoDate.Text.ToString()) + "' order by lono desc";
                        else
                            sql = "select lono from lodgm where left(lono,6)='" + Date.ToUSDate(LoDate.Text.ToString()).Substring(0, 6) + "' order by lono desc";
                        SqlDataAdapter dd = new SqlDataAdapter(sql, cn);
                        dd.Fill(table);
                        if (table.Rows.Count == 0)
                        {
                            LoNo.Text = No.ToString().Trim();
                            return true;
                        }
                        else
                        {
                            No = table.Rows[0]["lono"].ToDecimal() + 1;
                            LoNo.Text = No.ToString().Trim();
                        }
                    }
                }
                else
                {
                    using (SqlConnection cn = new SqlConnection(Common.sqlConnString))
                    {
                        cn.Open();
                        SqlCommand cmd = cn.CreateCommand();
                        cmd.CommandText = "select lono from lodgm where lono='" + LoNo.Text.Trim() + "'";
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

        private void LoNo_DoubleClick(object sender, EventArgs e)
        {
            if (LoNo.ReadOnly != true)
            {
                using (銀行存提款作業_瀏覽 frm = new 銀行存提款作業_瀏覽())
                {
                    frm.SetParaeter(ViewMode.Normal);
                    frm.SeekNo = LoNo.Text.Trim();
                    frm.開窗模式 = true;
                    frm.ShowDialog();
                    switch (frm.DialogResult)
                    {
                        case DialogResult.OK:
                            if (frm.Result == null) return;
                            dr = GetDataRow(frm.Result.ToString().Trim());
                            WriteToTxt(dr);
                            break;
                    }
                }
            }
        }

        private void LoNo_Validating(object sender, CancelEventArgs e)
        {
            if (LoNo.ReadOnly) return;
            if (btnCancel.Focused) return;

            //if (LoNo.Text.Trim() == "")
            //{
            //    e.Cancel = true;
            //    LoNo.Text = "";
            //    MessageBox.Show("資料不可為空白，請確定後重新輸入", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    return;
            //}

            if (btnState == "Append")
            {
                loadM();
                dr = GetDataRow();
                int i = list.IndexOf(dr);
                if (i != -1)
                {
                    e.Cancel = true;
                    LoNo.Text = "";
                    MessageBox.Show("此『存提證號』已經重複，請重新輸入", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                    LoNo.Text = "";
                    MessageBox.Show("此『存提證號』已經重複，請重新輸入", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }

            if (btnState == "Modify")
            {
                loadM();
                dr = GetDataRow();
                int i = list.IndexOf(dr);
                if (i != -1)
                {
                    if (LoNo.Text.Trim() != BeforeText)
                    {
                        WriteToTxt(dr);
                        LoNo.Focus();
                    }

                }
                else
                {
                    e.Cancel = true;
                    LoNo.SelectAll();
                    dr = GetDataRow(CurrentRow);
                    //開瀏覽視窗
                    using (銀行存提款作業_瀏覽 frm = new 銀行存提款作業_瀏覽())
                    {
                        frm.SetParaeter(ViewMode.Normal);
                        frm.SeekNo = LoNo.Text.Trim();
                        frm.開窗模式 = true;
                        frm.ShowDialog();
                        switch (frm.DialogResult)
                        {
                            case DialogResult.OK:
                                dr = GetDataRow(frm.Result.ToString().Trim());
                                WriteToTxt(dr);
                                LoNo.Focus();
                                break;
                        }
                    }

                }
            }
        }

        private void LoNo_Enter(object sender, EventArgs e)
        {
            if (LoNo.ReadOnly) return;
            BeforeText = LoNo.Text.Trim();
        }


    }
}
