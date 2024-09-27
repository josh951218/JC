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

namespace S_61.s_3基本資料
{
    public partial class 銀行帳號建檔 : FormT
    {
        DataTable tbM = new DataTable();
        List<DataRow> list = new List<DataRow>();
        List<btnT> SC;
        List<btnT> Others;
        List<TextBox> Txt;
        DataRow dr;
        string btnState;
        string BeforeText;
        string CurrentRow = "";
        SqlTransaction tran;
        public 銀行帳號建檔()
        {
            InitializeComponent();
            SC = new List<btnT> { btnSave, btnCancel };
            Others = new List<btnT> { btnTop, btnPrior, btnNext, btnBottom, btnAppend, btnModify, btnDelete, btnExit, btnDuplicate, btnBrow};
            Txt = new List<TextBox> { CoNo, CoName1, AcNo, AcName, AcName1, AcAct, AcActTel, AcTel, AcAddr1, AcWord, AcLia, AcFax, AcR1, AcEmail, AcMemo, Xa1No, Xa1Name, Xa1Par, AcMny, AcMny1 };
            AcMny.NumThousands = AcMny1.NumThousands = true;
            AcMny.NumLast = AcMny1.NumLast = Common.金額小數;
            AcMny.NumFirst = AcMny1.NumFirst = (20-1-Common.金額小數);
            Xa1Par.NumFirst = 4;
            Xa1Par.NumLast = 4;
        }

        private void 銀行帳號建檔_Load(object sender, EventArgs e)
        {
            Common.取得浮動連線字串(Common.使用者預設公司);
            if (Common.單據異動 == "2") CoNo.Enabled = false;
            rd1.BackColor = rd2.BackColor = Color.FromArgb(215, 227, 239);
            SC.ForEach(r => r.Enabled = false);
            Others.ForEach(r => r.Enabled = true);

            AcMny.NumFirst = AcMny1.NumFirst = 15;
            AcMny.NumLast = AcMny1.NumLast =  Convert.ToInt32(Common.金額小數.ToString());

            loadM();
            if (list.Count > 0)
                WriteToTxt(list.First());
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
                    string sql = "";
                    if (Common.單據異動 == "1") sql = "select * from acct order by acno";
                    else sql = "select * from acct where cono='"+Common.使用者預設公司+"' order by acno";
                    SqlDataAdapter dd = new SqlDataAdapter(sql, cn);
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
            return list.Find(r => r["AcNo"].ToString() == AcNo.Text.Trim());
        }

        DataRow GetDataRow(string str)
        {
            return list.Find(r => r["AcNo"].ToString() == str.Trim());
        }

        void WriteToTxt(DataRow dr)
        {
            if (dr == null)
            {
                Txt.ForEach(r => r.Text = "");
                rd1.Checked = rd2.Checked = false;
            }
            else
            {
                Txt.ForEach(r =>
                {
                    if (r is txtNumber)
                    {
                        if (r.Name == "Xa1Par")
                            r.Text = dr[r.Name.ToString()].ToDecimal().ToString("F4");
                        else
                            r.Text = dr[r.Name.ToString()].ToDecimal().ToString("N" + Common.金額小數);
                    }
                    else
                    {
                        r.Text = dr[r.Name.ToString()].ToString().Trim();
                    }
                });
                if (dr["ackind"].ToDecimal() == 1) rd1.Checked = true;
                else rd2.Checked = true;
                CurrentRow = dr["AcNo"].ToString().Trim();
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
            Txt.ForEach(r => r.ReadOnly = false);
            CoName1.ReadOnly = AcMny1.ReadOnly = Xa1Name.ReadOnly = true;
            rd1.Checked = true;
            rd1.Enabled = rd2.Enabled = true;
            Txt.ForEach(r => r.Text = "");
            CoNo.Text = Common.使用者預設公司;
            CHK.GetCoName(CoNo, CoName1);
            SetXa1No();
            decimal d = 0;
            AcMny.Text = AcMny1.Text = d.ToString("N" + Common.金額小數);
            AcNo.Focus();
        }

        private void btnDuplicate_Click(object sender, EventArgs e)
        {
            if (tbM.Rows.Count == 0)
            {
                MessageBox.Show("空資料庫，請先新增", "視窗訊息", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            btnState = "Duplicate";
            SC.ForEach(r => r.Enabled = true);
            Others.ForEach(r => r.Enabled = false);
            Txt.ForEach(r => r.ReadOnly = false);
            CoName1.ReadOnly = AcMny1.ReadOnly = Xa1Name.ReadOnly = true;
            rd1.Enabled = rd2.Enabled = true;
            decimal d = 0;
            AcMny.Text = AcMny1.Text = d.ToString("N" + Common.金額小數);
            AcNo.Text = "";
            AcNo.Focus();
        }

        private void btnModify_Click(object sender, EventArgs e)
        {
            if (tbM.Rows.Count == 0)
            {
                MessageBox.Show("空資料庫，請先新增", "視窗訊息", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            btnState = "Modify";
            SC.ForEach(r => r.Enabled = true);
            Others.ForEach(r => r.Enabled = false);
            Txt.ForEach(r => r.ReadOnly = false);
            CoName1.ReadOnly = AcMny1.ReadOnly = true;
            if(AcMny1.Text.ToDecimal() != 0)Xa1Name.ReadOnly = Xa1No.ReadOnly =  true;
            rd1.Enabled = rd2.Enabled = true;
            AcNo.Focus();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (tbM.Rows.Count == 0)
            {
                MessageBox.Show("空資料庫，請先新增", "視窗訊息", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (AcMny1.Text.ToDecimal() != 0)
            {
                MessageBox.Show("此銀行帳戶尚有餘額，無法刪除", "視窗訊息", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            btnState = "Delete";
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

                        cmd.CommandText = "delete from acct where acno=N'"
                            + AcNo.Text.Trim() + "' COLLATE Chinese_Taiwan_Stroke_BIN";
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

        private void btnBrow_Click(object sender, EventArgs e)
        {
            if (tbM.Rows.Count == 0)
            {
                MessageBox.Show("空資料庫，請先新增", "視窗訊息", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            using (銀行帳號建檔_瀏覽 frm = new 銀行帳號建檔_瀏覽())
            {
                frm.SetParaeter();
                frm.SeekNo = AcNo.Text.Trim();
                frm.CanAppend = false;
                frm.ShowDialog();
                switch (frm.DialogResult)
                {
                    case DialogResult.OK:
                        if (frm.result != "")
                        {
                            loadM();
                            if (list.Count > 0)
                            {
                                dr = GetDataRow(frm.result);
                                WriteToTxt(dr);
                            }
                            else
                            {
                                WriteToTxt(null);
                            }
                        }
                        break;
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (AcNo.Text.Trim() == "")
            {
                MessageBox.Show("『帳戶代碼』不可為空白，請確定後重新輸入", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                AcNo.Focus();
                return;
            }
            if (CoNo.Text.Trim() == "")
            {
                MessageBox.Show("『公司編號』不可為空白，請確定後重新輸入", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                CoNo.Focus();
                return;
            }
            if (btnState == "Append" || btnState == "Duplicate")
            {
                using (SqlConnection cn = new SqlConnection(Common.sqlConnString))
                {
                    cn.Open();
                    SqlCommand cmd = cn.CreateCommand();
                    tran = cn.BeginTransaction();
                    cmd.Transaction = tran;
                    try
                    {
                        cmd.CommandText = "insert into acct (";
                        Txt.ForEach(r =>
                        {
                            if (r.Name.ToString() != "AcMny1")
                                cmd.CommandText += r.Name.ToString() + ",";
                            else
                                cmd.CommandText += r.Name.ToString() + ",ackind)values(";
                        });
                        Txt.ForEach(r =>
                        {
                            if (r.Name.ToString() != "AcMny1")
                            {

                                if(r is TextBoxT)
                                    cmd.CommandText += "N'" + r.Text.ToString() + "',";
                                else
                                    cmd.CommandText += "" + r.Text.ToDecimal() + ",";
                                
                            }
                            else
                            {
                                cmd.CommandText += "'" + AcMny.Text.ToDecimal() + "',";
                                if (rd1.Checked) cmd.CommandText += "'" + 1 + "')";
                                else cmd.CommandText += "'" + 2 + "')";
                            }
                        });
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
                }
                CurrentRow = AcNo.Text.Trim();
                btnAppend_Click(null, null);
            }
            if (btnState == "Modify")
            {
                loadM();
                dr = GetDataRow(CurrentRow);
                int index = list.IndexOf(dr);
                if (index == -1)
                {
                    MessageBox.Show("此筆資料已被刪除", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    CoNo.Focus();
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
                        decimal beforeAcMny = GetDataRow(CurrentRow)["AcMny"].ToDecimal();
                        cmd.CommandText = "delete acct where acno='" + CurrentRow + "'";
                        cmd.ExecuteNonQuery();
                        cmd.CommandText = "";
                        cmd.CommandText = "insert into acct (";
                        Txt.ForEach(r =>
                        {
                            if (r.Name.ToString() != "AcMny1")
                                cmd.CommandText += r.Name.ToString() + ",";
                            else
                                cmd.CommandText += r.Name.ToString() + ",ackind)values(";
                        });
                        Txt.ForEach(r =>
                        {
                            if (r.Name.ToString() != "AcMny1")
                            {
                                if (r is TextBoxT)
                                    cmd.CommandText += "N'" + r.Text.ToString() + "',";
                                else
                                    cmd.CommandText += "'" + r.Text.ToDecimal() + "',";
                            }
                            else
                            {
                                cmd.CommandText += "'" + (AcMny1.Text.ToDecimal()-beforeAcMny+AcMny.Text.ToDecimal()) + "',";
                                if (rd1.Checked) cmd.CommandText += "'" + 1 + "')";
                                else cmd.CommandText += "'" + 2 + "')";
                            }
                        });
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
                }
                CurrentRow = AcNo.Text.Trim();
                Txt.ForEach(r => r.Text = "");
                AcNo.Focus();
                btnState = "Modify";
            }

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            btnState = "";
            SC.ForEach(r => r.Enabled = false);
            Others.ForEach(r => r.Enabled = true);
            Txt.ForEach(r => r.ReadOnly = true);
            rd1.Enabled = rd2.Enabled = false;
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
            CHK.CoNo_OpemFrm(CoNo, CoName1);
            SetXa1No();
        }

        private void CoNo_Validating(object sender, CancelEventArgs e)
        {
            if (btnCancel.Focused) return;
            if (!CHK.CoNo_Validating(CoNo, CoName1))
            {
                e.Cancel = true;
                CHK.CoNo_OpemFrm(CoNo, CoName1);
            }
            else
            {
                SetXa1No();
                Common.取得浮動連線字串(CoNo.Text.Trim());
                if (!CHK.Xa1No_Validating(Common.浮動連線字串, Xa1No, Xa1Name))
                {
                    Xa1No.Text = Xa1Name.Text = "";
                }
            }

        }

        private void AcNo_Validating(object sender, CancelEventArgs e)
        {
            if (AcNo.ReadOnly) return;
            if (btnCancel.Focused) return;

            if (AcNo.Text.Trim() == "")
            {
                e.Cancel = true;
                AcNo.Text = "";
                MessageBox.Show("資料不可為空白，請確定後重新輸入", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (btnState == "Append")
            {
                loadM();
                dr = GetDataRow();
                int i = list.IndexOf(dr);
                if (i != -1)
                {
                    e.Cancel = true;
                    AcNo.Text = "";
                    MessageBox.Show("此帳戶代碼已重複，請重新輸入", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                    AcNo.Text = "";
                    MessageBox.Show("此帳戶代碼已重複，請重新輸入", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }

            if (btnState == "Modify")
            {
                loadM();
                dr = GetDataRow();
                int i = list.IndexOf(dr);
                if (i != -1)
                {
                    if (AcNo.Text.Trim() != BeforeText)
                        WriteToTxt(dr);
                }
                else
                {
                    e.Cancel = true;
                    AcNo.SelectAll();
                    dr = GetDataRow(CurrentRow);
                    //開瀏覽視窗
                    using (銀行帳號建檔_瀏覽 frm = new 銀行帳號建檔_瀏覽())
                    {
                        frm.SetParaeter(ViewMode.Normal);
                        frm.CanAppend = false;
                        frm.SeekNo = AcNo.Text.Trim();
                        frm.開窗模式 = true;
                        frm.ShowDialog();
                        switch (frm.DialogResult)
                        {
                            case DialogResult.OK:
                                dr = GetDataRow(frm.Result["AcNo"].ToString().Trim());
                                WriteToTxt(dr);
                                break;
                        }
                    }
                }
            }
        }

        private void AcNo_DoubleClick(object sender, EventArgs e)
        {
            if (AcNo.ReadOnly != true)
            {
                using (銀行帳號建檔_瀏覽 frm = new 銀行帳號建檔_瀏覽())
                {
                    frm.SetParaeter(ViewMode.Normal);
                    frm.CanAppend = false;
                    frm.SeekNo = AcNo.Text.Trim();
                    frm.開窗模式 = true;
                    frm.ShowDialog();
                    switch (frm.DialogResult)
                    {
                        case DialogResult.OK:
                            if (frm.Result == null) return;
                            dr = GetDataRow(frm.Result["AcNo"].ToString().Trim());
                            WriteToTxt(dr);
                            break;
                    }
                }
            }
        }

        private void 銀行帳號建檔_KeyUp(object sender, KeyEventArgs e)
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

        private void AcAddr1_DoubleClick(object sender, EventArgs e)
        {
            if (AcAddr1.ReadOnly) return;
            FrmSaddr frm = new FrmSaddr();
            frm.SetParaeter(ViewMode.Normal);
            frm.callType = "AcAddr1";
            frm.ShowDialog();
            
        }

        private void AcNo_Enter(object sender, EventArgs e)
        {
            BeforeText = AcNo.Text.Trim();
        }

        void SetXa1No()
        {
            if (btnState == "Modify") return;
            if (CoNo.Text.Trim() != "")
            {
                using (SqlConnection cn = new SqlConnection(Common.sqlConnString))
                {
                    cn.Open();
                    SqlCommand cmd = cn.CreateCommand();
                    cmd.CommandText = "select xa1no,xa1name from comp where cono='" + CoNo.Text.Trim() + "' ";
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows && reader.Read())
                    {
                        Xa1No.Text = reader["Xa1No"].ToString();
                        Xa1Name.Text = reader["Xa1Name"].ToString();
                    }
                    reader.Dispose(); reader.Close();
                    Xa1Par.Text = "1";
                }
            }
            else
            {
                Xa1No.Text = Xa1Name.Text = "";
                Xa1Par.Text = "0";
            }
        }

        private void Xa1No_DoubleClick(object sender, EventArgs e)
        {
            CHK.Xa1No_OpemFrm(Xa1No, Xa1Name);
        }

        private void Xa1No_Validating(object sender, CancelEventArgs e)
        {
            if (Xa1No.ReadOnly) return;
            if (!CHK.Xa1No_Validating(Common.浮動連線字串, Xa1No, Xa1Name))
            {
                e.Cancel = true;
                CHK.Xa1No_OpemFrm(Xa1No, Xa1Name);
            }
        }

        private void AcName_Leave(object sender, EventArgs e)
        {
            if(AcName.ReadOnly)return;
            if (AcName1.Text.Trim() == "")
            {
                AcName1.Text = AcName.Text.GetUTF8(10);
            }
        }








    }
}
