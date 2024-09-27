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
    public partial class 公司建檔作業 : FormT
    {
        DataTable tbM = new DataTable();
        List<DataRow> list = new List<DataRow>();
        List<btnT> SC;
        List<btnT> Others;
        List<TextBox> Txt;
        DataRow dr;
        string btnState;
        string CurrentRow = "";
        string BeforeText;
        SqlTransaction tran;

        public 公司建檔作業()
        {
            InitializeComponent();
            SC = new List<btnT> { btnSave, btnCancel };
            Others = new List<btnT> { btnTop, btnPrior, btnNext, btnBottom, btnAppend, btnModify, btnDelete, btnExit, btnDuplicate };
            Txt = new List<TextBox> { CoNo,CoName1,CoName2,CoPer,CoUno,CoTel1,CoFax1,Xa1No,Xa1Name,CoR1,CoAddr1,CoWWW,CoEMail,CoMemo1,CoPaths,CoPatha };
        }

        private void 公司建檔作業_Load(object sender, EventArgs e)
        {
            Common.取得浮動連線字串(Common.使用者預設公司);
            if(Common.單據異動=="2")CoNo.Enabled = false;
            SC.ForEach(r => r.Enabled = false);
            Others.ForEach(r => r.Enabled = true);

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
                    SqlDataAdapter dd = new SqlDataAdapter("select * from comp where cono not in ('df') order by cono", cn);
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
            return list.Find(r => r["CoNo"].ToString() == CoNo.Text.Trim());
        }

        DataRow GetDataRow(string str)
        {
            return list.Find(r => r["CoNo"].ToString() == str.Trim());
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
                    r.Text = dr[r.Name.ToString()].ToString().Trim();
                });
                CurrentRow = dr["CoNo"].ToString().Trim();
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
            Xa1Name.ReadOnly = true;
            Txt.ForEach(r => r.Text = "");
            Xa1No.Text = "TWD";
            Xa1Name.Text = "新臺幣";
            CoNo.Focus();
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
            Xa1Name.ReadOnly = true;
            CoNo.Text = "";
            CoNo.Focus();
        }

        private void btnModify_Click(object sender, EventArgs e)
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
                    MessageBox.Show("無權更改此公司", "視窗訊息", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
            btnState = "Modify";
            SC.ForEach(r => r.Enabled = true);
            Others.ForEach(r => r.Enabled = false);
            Txt.ForEach(r => r.ReadOnly = false);
            Xa1Name.ReadOnly = true;
            CoNo.Focus();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (tbM.Rows.Count == 0)
            {
                MessageBox.Show("空資料庫，請先新增", "視窗訊息", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

                        cmd.CommandText = "delete from comp where CoNo=N'"
                            + CoNo.Text.Trim() + "' COLLATE Chinese_Taiwan_Stroke_BIN";
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

        private void btnSave_Click(object sender, EventArgs e)
        {
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
                    DataTable temp = new DataTable();
                    SqlDataAdapter dd = new SqlDataAdapter("select * from comp where cono='df'", cn);
                    dd.Fill(temp);
                    SqlCommand cmd = cn.CreateCommand();
                    tran = cn.BeginTransaction();
                    cmd.Transaction = tran;
                    Txt.ForEach(r =>
                    {
                        temp.Rows[0][r.Name.ToString()] = r.Text.ToString();

                    });
                    try
                    {
                        cmd.CommandText = "insert into comp (";
                        foreach (DataColumn i in temp.Columns)
                        {
                            if (i.ColumnName.ToString() != "colnk29no")
                                cmd.CommandText += i.ColumnName + ",";
                            else
                                cmd.CommandText += i.ColumnName + ")values(";
                        }
                        foreach (DataColumn i in temp.Columns)
                        {
                            if (i.ColumnName.ToString() != "colnk29no")
                            {
                                if (i.ColumnName.ToString().StartsWith("colnk"))
                                    cmd.CommandText += "N'" + temp.Rows[0][i.ColumnName].ToString().Trim() + "',";
                                else
                                    cmd.CommandText += "'" + temp.Rows[0][i.ColumnName].ToString().Trim() + "',";
                            }
                            else
                                cmd.CommandText += "'" + temp.Rows[0][i.ColumnName].ToString().Trim() + "')";
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
                    CurrentRow = CoNo.Text.Trim();
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
                    CoNo.Focus();
                    return;
                }
                using (SqlConnection cn = new SqlConnection(Common.sqlConnString))
                {
                    cn.Open();
                    DataTable temp = new DataTable();
                    SqlDataAdapter dd = new SqlDataAdapter("select * from comp where cono='"+CurrentRow+"'", cn);
                    dd.Fill(temp);
                    SqlCommand cmd = cn.CreateCommand();
                    tran = cn.BeginTransaction();
                    cmd.Transaction = tran;
                    Txt.ForEach(r =>
                    {
                        temp.Rows[0][r.Name.ToString()] = r.Text.ToString();
                    });
                    try
                    {
                        cmd.CommandText = "delete comp where cono='" + CurrentRow + "'";
                        cmd.ExecuteNonQuery();
                        cmd.CommandText = "";
                        cmd.CommandText = "insert into comp (";
                        foreach (DataColumn i in temp.Columns)
                        {
                            if (i.ColumnName.ToString() != "colnk29no")
                                cmd.CommandText += i.ColumnName + ",";
                            else
                                cmd.CommandText += i.ColumnName + ")values(";
                        }
                        foreach (DataColumn i in temp.Columns)
                        {
                            if (i.ColumnName.ToString() != "colnk29no")
                            {
                                if (i.ColumnName.ToString().StartsWith("colnk"))
                                    cmd.CommandText += "N'" + temp.Rows[0][i.ColumnName].ToString().Trim() + "',";
                                else
                                    cmd.CommandText += "'" + temp.Rows[0][i.ColumnName].ToString().Trim() + "',";
                            }
                            else
                                cmd.CommandText += "'" + temp.Rows[0][i.ColumnName].ToString().Trim() + "')";
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
                    CurrentRow = CoNo.Text.Trim();
                    Txt.ForEach(r => r.Text = "");
                    CoNo.Focus();
                    btnState = "Modify";
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            btnState = "";
            SC.ForEach(r => r.Enabled = false);
            Others.ForEach(r => r.Enabled = true);
            Txt.ForEach(r => r.ReadOnly = true);
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

        private void CoNo_Validating(object sender, CancelEventArgs e)
        {
            if (CoNo.ReadOnly) return;
            if (btnCancel.Focused) return;

            if (CoNo.Text.Trim() == "")
            {
                e.Cancel = true;
                CoNo.Text = "";
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
                    CoNo.Text = "";
                    MessageBox.Show("此公司編號已經重複，請重新輸入", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                    CoNo.Text = "";
                    MessageBox.Show("此公司編號已經重複，請重新輸入", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }

            if (btnState == "Modify")
            {
                loadM();
                dr = GetDataRow();
                int i = list.IndexOf(dr);
                if (i != -1)
                {
                    if (CoNo.Text.Trim() != BeforeText)
                        WriteToTxt(dr);
                }
                else
                {
                    e.Cancel = true;
                    CoNo.SelectAll();
                    dr = GetDataRow(CurrentRow);
                    //開瀏覽視窗
                    using (公司開窗 frm = new 公司開窗())
                    {
                        frm.SetParaeter(ViewMode.Normal);
                        frm.CanAppend = false;
                        frm.SeekNo = CoNo.Text.Trim();
                        frm.ShowDialog();
                        switch (frm.DialogResult)
                        {
                            case DialogResult.OK:
                                dr = GetDataRow(frm.Result["CoNo"].ToString().Trim());
                                WriteToTxt(dr);
                                break;
                        }
                    }

                }
            }
        }

        private void CoNo_DoubleClick(object sender, EventArgs e)
        {
            if (CoNo.ReadOnly != true)
            {
                using (公司開窗 frm = new 公司開窗())
                {
                    frm.SetParaeter(ViewMode.Normal);
                    frm.CanAppend = false;
                    frm.SeekNo = CoNo.Text.Trim();
                    frm.ShowDialog();
                    switch (frm.DialogResult)
                    {
                        case DialogResult.OK:
                            dr = GetDataRow(frm.Result["CoNo"].ToString().Trim());
                            WriteToTxt(dr);
                            break;
                    }
                }
            }
        }

        private void CoNo_Enter(object sender, EventArgs e)
        {
            BeforeText = CoNo.Text.Trim();
        }

        private void 公司建檔作業_KeyUp(object sender, KeyEventArgs e)
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

        private void Xa1No_DoubleClick(object sender, EventArgs e)
        {
            CHK.Xa1No_OpemFrm(Xa1No, Xa1Name);
        }

        private void Xa1No_Validating(object sender, CancelEventArgs e)
        {
            if (!CHK.Xa1No_Validating(Common.浮動連線字串, Xa1No, Xa1Name))
            {
                e.Cancel = true;
                CHK.Xa1No_OpemFrm(Xa1No, Xa1Name);
            }
        }

        private void CoName2_Leave(object sender, EventArgs e)
        {
            if (CoName2.ReadOnly) return;
            if (CoName1.Text.Trim() != "" || CoName2.Text.Trim() == "") return;
            CoName1.Text = CoName2.Text.GetUTF8(10);
        }

        private void CoAddr1_DoubleClick(object sender, EventArgs e)
        {
            if (CoAddr1.ReadOnly) return;
            FrmSaddr frm = new FrmSaddr();
            frm.SetParaeter(ViewMode.Normal);
            frm.callType = "CoAddr1";
            frm.ShowDialog();
        }




    }
}
