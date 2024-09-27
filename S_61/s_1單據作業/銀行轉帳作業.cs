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
using System.Net;
using System.IO;

namespace S_61.s_1單據作業
{
    public partial class 銀行轉帳作業 : FormT
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

        public 銀行轉帳作業()
        {
            InitializeComponent();
            Txt = new List<TextBox> { CaNo, CoNo, CoName1, CoName2, CaDate, AcNo, AcName, AcName1, Xa1Name, Xa1Par, AcNoi, AcNamei, AcName1i, Xa1Namei, Xa1Pari, CaMny, CaMny1, CaMemo };
            SC = new List<btnT> { btnSave, btnCancel };
            Others = new List<btnT> { btnTop, btnPrior, btnNext, btnBottom, btnAppend, btnModify, btnDelete, btnExit, btnDuplicate, btnBrow, btnPrint };
            CaMny.NumThousands = CaMny1.NumThousands = true;
            CaMny.NumLast = CaMny1.NumLast = Common.金額小數;
            CaMny.NumFirst = CaMny1.NumFirst = (20 - 1 - Common.金額小數);
            lb提示.Text = "";
        }

        private void 銀行轉帳作業_Load(object sender, EventArgs e)
        {
            if (Common.單據異動 == "2") CoNo.Enabled = false;

            if (Common.User_DateTime == 1) CaDate.MaxLength = 7;
            else CaDate.MaxLength = 8;
            CaDate.Init();
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
                    SqlDataAdapter dd = new SqlDataAdapter("select * from carry order by cano", cn);
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
            return list.Find(r => r["CaNo"].ToString() == CaNo.Text.Trim());
        }

        DataRow GetDataRow(string str)
        {
            return list.Find(r => r["CaNo"].ToString() == str.Trim());
        }

        void WriteToTxt(DataRow dr)
        {
            if (dr == null)
            {
                Txt.ForEach(r =>r.Text = "");
                Xa1Par.Visible = Xa1Pari.Visible = false;
                lb提示.Text = "";
            }
            else
            {
                Txt.ForEach(r =>{
                    if (r is txtNumber)
                    {
                        if (r.Name == "Xa1Par" || r.Name == "Xa1Pari") r.Text = dr[r.Name.ToString()].ToDecimal().ToString("f4");
                        else r.Text = dr[r.Name.ToString()].ToDecimal().ToString("N"+Common.金額小數);
                    }
                    else
                    {
                        r.Text = dr[r.Name.ToString()].ToString().Trim();
                    }
                });
                CaDate.Text = Common.User_DateTime == 1 ? dr["CaDate"].ToString() : dr["CaDate_1"].ToString();
                Xa1Par.Visible = dr["Xa1Par"].ToDecimal() == 1 ? false : true;
                Xa1Pari.Visible = dr["Xa1Pari"].ToDecimal() == 1 ? false : true;
                lb提示.Text = "請輸入『" + dr["Xa1Name"].ToString().Trim() + "』金額";
                CurrentRow = dr["CaNo"].ToString();
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
                r.Text = "";
                if (r.Name != "CoName1")
                    if (r.Name != "AcName")
                        if (r.Name != "AcNamei")
                            if (r.Name != "Xa1Name")
                                if (r.Name != "Xa1Namei")
                                    r.ReadOnly = false;
            });
            CoNo.Text = Common.使用者預設公司;
            CHK.CoNo_Validating(CoNo, CoName1, CoName2);
            CaDate.Text = Date.GetDateTime(Common.User_DateTime, false);
            lb提示.Text = "";
            Xa1Par.Visible = Xa1Pari.Visible = false;
            CaNo.Focus();
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
                if (r.Name != "CoName1")
                    if (r.Name != "AcName")
                        if (r.Name != "AcNamei")
                            if (r.Name != "Xa1Name")
                                if (r.Name != "Xa1Namei")
                                    r.ReadOnly = false;
            });
            CaDate.Text = Date.GetDateTime(Common.User_DateTime, false);
            CaNo.Text = "";
            CaNo.Focus();
        }

        private void btnModify_Click(object sender, EventArgs e)
        {
            if (tbM.Rows.Count == 0)
            {
                MessageBox.Show("空資料庫，請先新增", "視窗訊息", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            //DataRow ac = list.Find(r => r["CaNo"].ToString().Trim() == CaNo.Text.ToString().Trim());
            DataRow ac = Common.load("Check", "carry", "Cano", CaNo.Text.Trim());
            if (ac != null && ac["caacno"].ToString().Trim() != "")
            {
                MessageBox.Show("此票據已傳輸至會計傳票，無法異動！\n公司編號:" + ac["accono"].ToString().Trim() + " 傳票編號:" + ac["caacno"].ToString().Trim(), "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                if (r.Name != "CoName1")
                    if (r.Name != "AcName")
                        if (r.Name != "AcNamei")
                            if (r.Name != "Xa1Name")
                                if (r.Name != "Xa1Namei")
                                    r.ReadOnly = false;
            });
            CaNo.Focus();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (tbM.Rows.Count == 0)
            {
                MessageBox.Show("空資料庫，請先新增", "視窗訊息", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            //DataRow ac = list.Find(r => r["CaNo"].ToString().Trim() == CaNo.Text.ToString().Trim());
            DataRow ac = Common.load("Check", "carry", "Cano", CaNo.Text.Trim());
            if (ac != null && ac["caacno"].ToString().Trim() != "")
            {
                MessageBox.Show("此票據已傳輸至會計傳票，無法異動！\n公司編號:" + ac["accono"].ToString().Trim() + " 傳票編號:" + ac["caacno"].ToString().Trim(), "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                if (!刪除帳戶金額加減(GetDataRow())) return;
                try
                {
                    using (SqlConnection cn = new SqlConnection(Common.sqlConnString))
                    {
                        cn.Open();
                        SqlCommand cmd = cn.CreateCommand();

                        tran = cn.BeginTransaction();
                        cmd.Transaction = tran;
                       
                        cmd.CommandText = "delete from carry where CaNo=N'"
                            + CaNo.Text.Trim() + "' COLLATE Chinese_Taiwan_Stroke_BIN";
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
            using (銀行轉帳作業_列印 frm = new 銀行轉帳作業_列印())
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
            using (銀行轉帳作業_瀏覽 frm = new 銀行轉帳作業_瀏覽())
            {
                frm.SetParaeter();
                frm.SeekNo = CaNo.Text.Trim();
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
                MessageBox.Show("『轉出帳號』不可為空白，請確定後重新輸入", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                CaNo.Focus();
                return;
            }
            if (AcNoi.Text.Trim() == "")
            {
                MessageBox.Show("『轉入帳號』不可為空白，請確定後重新輸入", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                CaNo.Focus();
                return;
            }
            if (CaMny.Text.ToDecimal() == 0)
            {
                MessageBox.Show("『金額』不可為零，請確定後重新輸入", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                CaMny.Focus();
                return;
            }
            if (btnState == "Append" || btnState == "Duplicate")
            {
                if (!自動產生編號())
                {
                    MessageBox.Show("『轉帳證號』重複", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    CaNo.Focus();
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
                        匯兌計算();
                        cmd.CommandText = "insert into carry(cano,cono,coname1,coname2,cadate,cadate_1,acno,acname,acname1,xa1name,xa1par,"
                                        + "acnoi,acnamei,acname1i,xa1namei,xa1pari,camny,camny1,camemo)values("
                                        + "N'" + CaNo.Text.Trim() + "',"
                                        + "N'" + CoNo.Text.Trim() + "',"
                                        + "N'" + CoName1.Text.Trim() + "',"
                                        + "N'" + CoName2.Text.Trim() + "',"
                                        + "N'" + Date.ToTWDate(CaDate.Text.ToString().Trim()) + "',"
                                        + "N'" + Date.ToUSDate(CaDate.Text.ToString().Trim()) + "',"
                                        + "N'" + AcNo.Text.Trim() + "',"
                                        + "N'" + AcName.Text.Trim() + "',"
                                        + "N'" + AcName1.Text.Trim() + "',"
                                        + "N'" + Xa1Name.Text.Trim() + "',"
                                        + "" + Xa1Par.Text.ToDecimal() + ","
                                        + "N'" + AcNoi.Text.Trim() + "',"
                                        + "N'" + AcNamei.Text.Trim() + "',"
                                        + "N'" + AcName1i.Text.Trim() + "',"
                                        + "N'" + Xa1Namei.Text.Trim() + "',"
                                        + "" + Xa1Pari.Text.ToDecimal() + ","
                                        + "" + CaMny.Text.ToDecimal() + ","
                                        + "" + CaMny1.Text.ToDecimal() + ","
                                        + "N'" + CaMemo.Text.Trim() + "')";
                        cmd.ExecuteNonQuery();
                        if (帳戶金額加減())
                            tran.Commit();
                        else
                            tran.Rollback();
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        MessageBox.Show(ex.ToString());
                        return;
                    }
                    CurrentRow = CaNo.Text.Trim();
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
                    CaNo.Focus();
                    return;
                }
                if (!刪除帳戶金額加減(GetDataRow(CurrentRow))) return;
                using (SqlConnection cn = new SqlConnection(Common.sqlConnString))
                {
                    cn.Open();
                    SqlCommand cmd = cn.CreateCommand();
                    tran = cn.BeginTransaction();
                    cmd.Transaction = tran;
                    try
                    {
                        匯兌計算();
                        cmd.CommandText = "delete carry where cano='" + CurrentRow + "';";
                        cmd.CommandText += "insert into carry(cano,cono,coname1,coname2,cadate,cadate_1,acno,acname,acname1,xa1name,xa1par,"
                                         + "acnoi,acnamei,acname1i,xa1namei,xa1pari,camny,camny1,camemo)values("
                                         + "N'" + CaNo.Text.Trim() + "',"
                                         + "N'" + CoNo.Text.Trim() + "',"
                                         + "N'" + CoName1.Text.Trim() + "',"
                                         + "N'" + CoName2.Text.Trim() + "',"
                                         + "N'" + Date.ToTWDate(CaDate.Text.ToString().Trim()) + "',"
                                         + "N'" + Date.ToUSDate(CaDate.Text.ToString().Trim()) + "',"
                                         + "N'" + AcNo.Text.Trim() + "',"
                                         + "N'" + AcName.Text.Trim() + "',"
                                         + "N'" + AcName1.Text.Trim() + "',"
                                         + "N'" + Xa1Name.Text.Trim() + "',"
                                         + "" + Xa1Par.Text.ToDecimal() + ","
                                         + "N'" + AcNoi.Text.Trim() + "',"
                                         + "N'" + AcNamei.Text.Trim() + "',"
                                         + "N'" + AcName1i.Text.Trim() + "',"
                                         + "N'" + Xa1Namei.Text.Trim() + "',"
                                         + "" + Xa1Pari.Text.ToDecimal() + ","
                                         + "" + CaMny.Text.ToDecimal() + ","
                                         + "" + CaMny1.Text.ToDecimal() + ","
                                         + "N'" + CaMemo.Text.Trim() + "')";
                        cmd.ExecuteNonQuery();
                        if (帳戶金額加減())
                            tran.Commit();
                        else
                            tran.Rollback();
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        MessageBox.Show(ex.ToString());
                        return;
                    }
                    CurrentRow = CaNo.Text.Trim();
                    Txt.ForEach(r =>r.Text = "");
                    lb提示.Text = "";
                    Xa1Par.Visible = Xa1Pari.Visible = false;
                    CaNo.Focus();
                    btnState = "Modify";
                }
            } 
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            btnState = "";
            SC.ForEach(r => r.Enabled = false);
            Others.ForEach(r => r.Enabled = true);
            Txt.ForEach(r =>r.ReadOnly = true);
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
                {
                    AcNo.Text = AcName1.Text = AcName.Text = "";
                }
            }
        }

        private void CaDate_Validating(object sender, CancelEventArgs e)
        {
            if (btnCancel.Focused || CaDate.ReadOnly) return;
            if (CaDate.Text.Trim() == "")
            {
                MessageBox.Show("『提款日期』不可為空白，請確定後重新輸入", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                e.Cancel = true;
                return;
            }
            if (!CaDate.IsDateTime())
            {
                MessageBox.Show("日期格式錯誤，請確定後重新輸入", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                e.Cancel = true;
                return;
            }
            if (!CHK.稽核會計年度(CaDate.Text.Trim())) e.Cancel = true;
        }

        private void AcNo_DoubleClick(object sender, EventArgs e)
        {
            TextBox tb = sender as TextBox;
            if (tb.ReadOnly) return;
            if (tb.Name == "AcNo") CHK.AcNo_OpemFrm(CoNo.Text.Trim(),tb, AcName1, AcName);
            if (tb.Name == "AcNoi") CHK.AcNo_OpemFrm(CoNo.Text.Trim(),tb, AcName1i, AcNamei);
            using (SqlConnection cn = new SqlConnection(Common.sqlConnString))
            {
                cn.Open();
                SqlCommand cmd = cn.CreateCommand();
                cmd.CommandText = "select ackind,xa1par,xa1name,xa1no from acct where acno='" + tb.Text.Trim() + "'";
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows && reader.Read())
                {
                    if (tb.Name == "AcNo")
                    {
                        Xa1Name.Text = reader["xa1name"].ToString();
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
                        Xa1Par.Visible = reader["ackind"].ToDecimal() == 2 ? true : false;
                        lb提示.Text = "請輸入『" + Xa1Name.Text.Trim() + "』金額";
                    }
                    else
                    {
                        Xa1Namei.Text = reader["xa1name"].ToString();
                        if (Common.自動產生匯率 == 1)
                        {
                            DataRow temp = CHK.銀行牌告匯率dt.AsEnumerable().ToList().Find(r => r["幣別編號"].ToString().Trim() == reader["xa1no"].ToString().Trim());
                            if (temp != null)
                                Xa1Pari.Text = Math.Round(temp["匯率"].ToDecimal(), 4, MidpointRounding.AwayFromZero).ToString("f4");
                            else
                                Xa1Pari.Text = reader["xa1par"].ToDecimal().ToString("f4");
                        }
                        else
                        {
                            Xa1Pari.Text = reader["xa1par"].ToDecimal().ToString("f4");
                        }
                        Xa1Pari.Visible = reader["ackind"].ToDecimal() == 2 ? true : false;
                    }
                }
            }
        }

        private void AcNo_Validating(object sender, CancelEventArgs e)
        {
            TextBox tb = sender as TextBox;
            if (tb.ReadOnly || btnCancel.Focused) return;
            if (tb.Name == "AcNo" ? !CHK.AcNo_Validating(CoNo.Text.Trim(),tb, AcName1, AcName) : !CHK.AcNo_Validating(CoNo.Text.Trim(),tb, AcName1i, AcNamei))
            {
                e.Cancel = true;
                if (tb.Name == "AcNo") CHK.AcNo_OpemFrm(CoNo.Text.Trim(),tb, AcName1, AcName);
                if (tb.Name == "AcNoi") CHK.AcNo_OpemFrm(CoNo.Text.Trim(),tb, AcName1i, AcNamei);
            }
            else
            {
                using (SqlConnection cn = new SqlConnection(Common.sqlConnString))
                {
                    cn.Open();
                    SqlCommand cmd = cn.CreateCommand();
                    cmd.CommandText = "select ackind,xa1par,xa1name,xa1no from acct where acno='" + tb.Text.Trim() + "'";
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows && reader.Read())
                    {
                        if (tb.Name == "AcNo")
                        {
                            Xa1Name.Text = reader["xa1name"].ToString();
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
                            Xa1Par.Visible = reader["ackind"].ToDecimal() == 2 ? true : false;
                            lb提示.Text = "請輸入『" + Xa1Name.Text.Trim() + "』金額";
                        }
                        else
                        {
                            Xa1Namei.Text = reader["xa1name"].ToString();
                            if (Common.自動產生匯率 == 1)
                            {
                                DataRow temp = CHK.銀行牌告匯率dt.AsEnumerable().ToList().Find(r => r["幣別編號"].ToString().Trim() == reader["xa1no"].ToString().Trim());
                                if (temp != null)
                                    Xa1Pari.Text = Math.Round(temp["匯率"].ToDecimal(), 4, MidpointRounding.AwayFromZero).ToString("f4");
                                else
                                    Xa1Pari.Text = reader["xa1par"].ToDecimal().ToString("f4");
                            }
                            else
                            {
                                Xa1Pari.Text = reader["xa1par"].ToDecimal().ToString("f4");
                            }
                            Xa1Pari.Visible = reader["ackind"].ToDecimal() == 2 ? true : false;
                        }
                    }
                }
            }
        }

        private void CaMemo_DoubleClick(object sender, EventArgs e)
        {
            CHK.Memo_OpemFrm(CaMemo);
        }

        bool 自動產生編號()
        {
            DataTable table = new DataTable();
            string date = "", count = "1";
            if (Common.傳票編碼方式 == 1)//民國年月日+流水號
            {
                date = Date.ToTWDate(CaDate.Text.Trim());
                count = count.PadLeft(3, '0');
            }
            else if (Common.傳票編碼方式 == 2)//民國年月+流水號
            {
                date = Date.ToTWDate(CaDate.Text.Trim());
                date = date.Substring(0, 5);
                count = count.PadLeft(5, '0');
            }
            else if (Common.傳票編碼方式 == 3)//西元年月日+流水號
            {
                date = Date.ToUSDate(CaDate.Text.Trim());
                count = count.PadLeft(2, '0');
            }
            else
            {
                date = Date.ToUSDate(CaDate.Text.Trim());
                date = date.Substring(0,6);
                count = count.PadLeft(4, '0');
            }
            decimal No = (date + count).ToDecimal();
            try
            {
                if (CaNo.Text.Trim() == "")
                {
                    using (SqlConnection cn = new SqlConnection(Common.sqlConnString))
                    {
                        cn.Open();
                        string sql = "";
                        if(Common.傳票編碼方式 == 1)
                            sql = "select cano from carry where left(cano,7)='" + Date.ToTWDate(CaDate.Text.ToString()) + "' order by cano desc";
                        else if (Common.傳票編碼方式 == 2)
                            sql = "select cano from carry where left(cano,5)='" + Date.ToTWDate(CaDate.Text.ToString()).Substring(0, 5) + "' order by cano desc";
                        else if (Common.傳票編碼方式 == 3)
                            sql = "select cano from carry where left(cano,8)='" + Date.ToUSDate(CaDate.Text.ToString()) + "' order by cano desc";
                        else
                            sql = "select cano from carry where left(cano,6)='" + Date.ToUSDate(CaDate.Text.ToString()).Substring(0, 6) + "' order by cano desc";
                        SqlDataAdapter dd = new SqlDataAdapter(sql,cn);
                        dd.Fill(table);
                        if (table.Rows.Count == 0)
                        {
                            CaNo.Text = No.ToString().Trim();
                            return true;
                        }
                        else
                        {
                            No = table.Rows[0]["cano"].ToDecimal() + 1;
                            CaNo.Text = No.ToString().Trim();
                        }
                    }
                }
                else
                {
                    using (SqlConnection cn = new SqlConnection(Common.sqlConnString))
                    {
                        cn.Open();
                        SqlCommand cmd = cn.CreateCommand();
                        cmd.CommandText = "select cano from carry where cano='" + CaNo.Text.Trim() + "'";
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

        void 匯兌計算()
        {
            CaMny1.Text = Math.Round((CaMny.Text.ToDecimal() * Xa1Par.Text.ToDecimal()) / Xa1Pari.Text.ToDecimal(), Common.金額小數, MidpointRounding.AwayFromZero).ToString("f"+Common.金額小數);
        }

        bool 帳戶金額加減()
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(Common.sqlConnString))
                {
                    cn.Open();
                    SqlCommand cmd = cn.CreateCommand();
                    cmd.CommandText = "update acct set acmny1-="+CaMny.Text.ToDecimal()+" where acno=N'"+AcNo.Text.Trim()+"';";
                    //decimal 轉換後金額 = Math.Round((CaMny.Text.ToDecimal() * Xa1Par.Text.ToDecimal()) / Xa1Pari.Text.ToDecimal(), Common.金額小數, MidpointRounding.AwayFromZero);
                    cmd.CommandText += "update acct set acmny1+=" + CaMny1.Text.ToDecimal() + " where acno=N'" + AcNoi.Text.Trim() + "';";
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return false;
            }
            return true;
        }

        bool 刪除帳戶金額加減(DataRow dr)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(Common.sqlConnString))
                {
                    cn.Open();
                    SqlCommand cmd = cn.CreateCommand();
                    cmd.CommandText = "update acct set acmny1+=" + dr["CaMny"].ToDecimal() + " where acno=N'" + dr["AcNo"].ToString().Trim() + "';";
                    //decimal 轉換後金額 = Math.Round((dr["CaMny"].ToDecimal() * dr["Xa1Par"].ToDecimal()) / dr["Xa1Pari"].ToDecimal(), Common.金額小數, MidpointRounding.AwayFromZero);
                    cmd.CommandText += "update acct set acmny1-=" + dr["CaMny1"].ToDecimal() + " where acno=N'" + dr["AcNoi"].ToString().Trim() + "';";
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return false;
            }
            return true;
        }

        private void 銀行轉帳作業_KeyUp(object sender, KeyEventArgs e)
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

        private void CaNo_Enter(object sender, EventArgs e)
        {
            if (CaNo.ReadOnly) return;
            BeforeText = CaNo.Text.Trim();
        }

        private void CaNo_DoubleClick(object sender, EventArgs e)
        {
            if (CaNo.ReadOnly != true)
            {
                using (銀行轉帳作業_瀏覽 frm = new 銀行轉帳作業_瀏覽())
                {
                    frm.SetParaeter(ViewMode.Normal);
                    frm.SeekNo = CaNo.Text.Trim();
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

        private void CaNo_Validating(object sender, CancelEventArgs e)
        {
            if (CaNo.ReadOnly) return;
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
                    CaNo.Text = "";
                    MessageBox.Show("此『轉帳證號』已經重複，請重新輸入", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                    CaNo.Text = "";
                    MessageBox.Show("此『轉帳證號』已經重複，請重新輸入", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }

            if (btnState == "Modify")
            {
                loadM();
                dr = GetDataRow();
                int i = list.IndexOf(dr);
                if (i != -1)
                {
                    if (CaNo.Text.Trim() != BeforeText)
                    {
                        WriteToTxt(dr);
                        CaNo.Focus();
                    }

                }
                else
                {
                    e.Cancel = true;
                    CaNo.SelectAll();
                    dr = GetDataRow(CurrentRow);
                    //開瀏覽視窗
                    using (銀行轉帳作業_瀏覽 frm = new 銀行轉帳作業_瀏覽())
                    {
                        frm.SetParaeter(ViewMode.Normal);
                        frm.SeekNo = CaNo.Text.Trim();
                        frm.開窗模式 = true;
                        frm.ShowDialog();
                        switch (frm.DialogResult)
                        {
                            case DialogResult.OK:
                                dr = GetDataRow(frm.Result.ToString().Trim());
                                WriteToTxt(dr);
                                CaNo.Focus();
                                break;
                        }
                    }

                }
            }
        }
    }


}
