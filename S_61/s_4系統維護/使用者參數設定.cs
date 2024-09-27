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

namespace S_61.s_4系統維護
{
    public partial class 使用者參數設定 : FormT
    {
        DataTable tbM = new DataTable();
        DataTable tbD = new DataTable();
        List<DataRow> list = new List<DataRow>();
        List<btnT> SC;
        List<btnT> Others;
        List<TextBox> control;
        DataRow dr;
        string btnState;
        string CurrentRow = "";
        SqlTransaction tran;

        public 使用者參數設定()
        {
            InitializeComponent();
            SC = new List<btnT> { btnSave, btnCancel };
            Others = new List<btnT> { btnTop, btnPrior, btnNext, btnBottom, btnAppend, btnModify, btnDelete, btnExit, btnDuplicate };
            control = new List<TextBox> { SCName, SCName1, SCPass, CoNo };
        }

        private void 使用者參數設定_Load(object sender, EventArgs e)
        {
            dataGridViewT1.SetWidthByPixel();
            dataGridViewT1.AutoGenerateColumns = false;
            SC.ForEach(r => r.Enabled = false);
            Others.ForEach(r => r.Enabled = true);

            loadM();
            if (list.Count > 0)
                WriteToTxt(list.Find(r => r["scname"].ToString().Trim() == Common.User_Name));
            loadD();
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
                    SqlDataAdapter dd = new SqlDataAdapter("select * from scrit order by scname", cn);
                    dd.Fill(tbM);
                    if (tbM.Rows.Count > 0) list = tbM.AsEnumerable().ToList();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        void loadD()
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(Common.sqlConnString))
                {
                    cn.Open();
                    tbD.Clear();
                    SqlDataAdapter dd = new SqlDataAdapter("select * from scritd where scname='"+SCName.Text.Trim()+"' order by tano", cn);
                    dd.Fill(tbD);
                    dataGridViewT1.DataSource = tbD;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        DataRow GetDataRow()
        {
            return list.Find(r => r["SCName"].ToString() == SCName.Text.Trim());
        }

        DataRow GetDataRow(string str)
        {
            return list.Find(r => r["SCName"].ToString() == str.Trim());
        }

        void WriteToTxt(DataRow dr)
        {
            if (dr == null)
            {
                SCName.Text = SCPass.Text = SCName1.Text = CoNo.Text = CoName1.Text = "";
            }
            else
            {
                SCName.Text = dr["SCName"].ToString().Trim();
                SCName1.Text = dr["SCName1"].ToString().Trim();
                SCPass.Text = dr["SCPass"].ToString().Trim();
                CoNo.Text = dr["CoNo"].ToString().Trim();
                CHK.GetCoName(CoNo, CoName1);
                if (dr["scsuchk"].ToString() == "1")
                    rd1.Checked = true;
                else
                    rd2.Checked = true;
                CurrentRow = SCName.Text.Trim();
            }
        }

        private void btnTop_Click(object sender, EventArgs e)
        {
            loadM();
            if (list.Count > 0)
            {
                dr = list.First();
                WriteToTxt(dr);
                loadD();
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
                        loadD();
                        btnTop.Enabled = btnPrior.Enabled = false;
                        btnNext.Enabled = btnBottom.Enabled = true;
                    }
                    else
                    {
                        WriteToTxt(list[--temp]);
                        loadD();
                        btnTop.Enabled = btnPrior.Enabled = btnNext.Enabled = btnBottom.Enabled = true;
                    }
                }
                if (i > 0)
                {
                    WriteToTxt(list[--i]);
                    loadD();
                    btnTop.Enabled = btnPrior.Enabled = btnNext.Enabled = btnBottom.Enabled = true;
                }
                if (i == 0)
                {
                    MessageBox.Show("已最上一筆", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    WriteToTxt(list.First());
                    loadD();
                    btnTop.Enabled = btnPrior.Enabled = false;
                    btnNext.Enabled = btnBottom.Enabled = true;
                }
            }
            else
            {
                WriteToTxt(null);
                loadD();
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
                        loadD();
                        btnTop.Enabled = btnPrior.Enabled = true;
                        btnNext.Enabled = btnBottom.Enabled = false;
                    }
                    else
                    {
                        WriteToTxt(list[++temp]);
                        loadD();
                        btnTop.Enabled = btnPrior.Enabled = btnNext.Enabled = btnBottom.Enabled = true;
                    }
                }
                if (i < list.Count - 1)
                {
                    WriteToTxt(list[++i]);
                    loadD();
                    btnTop.Enabled = btnPrior.Enabled = btnNext.Enabled = btnBottom.Enabled = true;
                }
                else
                {
                    MessageBox.Show("已最下一筆", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    WriteToTxt(list.Last());
                    loadD();
                    btnTop.Enabled = btnPrior.Enabled = true;
                    btnNext.Enabled = btnBottom.Enabled = false;
                }
            }
            else
            {
                WriteToTxt(null);
                loadD();
            }
        }

        private void btnBottom_Click(object sender, EventArgs e)
        {
            loadM();
            if (list.Count > 0)
            {
                dr = list.Last();
                WriteToTxt(dr);
                loadD();
            }
            btnNext.Enabled = btnBottom.Enabled = false;
            btnTop.Enabled = btnPrior.Enabled = true;
        }

        private void btnAppend_Click(object sender, EventArgs e)
        {
            btnState = "Append";
            SC.ForEach(r => r.Enabled = true);
            Others.ForEach(r => r.Enabled = false);
            control.ForEach(r => r.ReadOnly = false);
            rd1.Enabled = rd2.Enabled = true;
            SCName.Text = SCPass.Text = SCName1.Text = CoNo.Text = CoName1.Text = "";
            CoNo.Text = Common.CoNo;
            CHK.GetCoName(CoNo,CoName1);
            using (SqlConnection cn = new SqlConnection(Common.sqlConnString))
            {
                cn.Open();
                tbD.Clear();
                SqlDataAdapter dd = new SqlDataAdapter("select * from scritd where scname='default' order by tano", cn);
                dd.Fill(tbD);
                dataGridViewT1.DataSource = tbD;
            }
            SCName.Focus();
        }

        private void btnDuplicate_Click(object sender, EventArgs e)
        {
            if (dataGridViewT1.Rows.Count == 0)
            {
                MessageBox.Show("空資料庫，請先新增", "視窗訊息", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            btnState = "Duplicate";
            SC.ForEach(r => r.Enabled = true);
            Others.ForEach(r => r.Enabled = false);
            control.ForEach(r => r.ReadOnly = false);
            rd1.Enabled = rd2.Enabled = true;
            SCName.Text = SCPass.Text = SCName1.Text = "";
            SCName.Focus();
        }

        private void btnModify_Click(object sender, EventArgs e)
        {
            if (dataGridViewT1.Rows.Count == 0)
            {
                MessageBox.Show("空資料庫，請先新增", "視窗訊息", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            dr = GetDataRow(CurrentRow);
            if (dr == null)
            {
                MessageBox.Show("此筆資料已被刪除", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            btnState = "Modify";
            SC.ForEach(r => r.Enabled = true);
            Others.ForEach(r => r.Enabled = false);
            control.ForEach(r => r.ReadOnly = false);
            rd1.Enabled = rd2.Enabled = true;
            SCName.Focus();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (SCName.Text == "") return;
            if (SCName.Text == "BM")
            {
                MessageBox.Show("此帳號為系統管理員，禁止刪除", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (SCName.Text == Common.User_Name)
            {
                MessageBox.Show("不能刪除自己帳號", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

                        cmd.CommandText = "delete from scritd where SCName=N'"
                            + SCName.Text.Trim() + "' COLLATE Chinese_Taiwan_Stroke_BIN";
                        cmd.ExecuteNonQuery();
                        cmd.CommandText = "delete from scrit where SCName=N'"
                            + SCName.Text + "' COLLATE Chinese_Taiwan_Stroke_BIN";
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
                                loadD();
                                MessageBox.Show("已最下一筆", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                            else
                            {
                                WriteToTxt(list[i]);
                                loadD();
                            }
                        }
                        else
                        {
                            WriteToTxt(null);
                            loadD();
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
            if (SCName.Text.Trim() == "" || SCPass.Text.Trim() == "")
            {
                MessageBox.Show("『使用者名稱』、『密碼』不可為空白，請確定後重新輸入", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                SCName.Focus();
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
                        cmd.CommandText = "insert into scrit (scname,scpass,scname1,cono,scsuchk)values("
                        + "'" + SCName.Text.Trim() + "',"
                        + "'" + SCPass.Text.Trim() + "',"
                        + "'" + SCName1.Text.Trim() + "',"
                        + "'" + CoNo.Text.Trim() + "',";
                        if (rd1.Checked)
                            cmd.CommandText += "1)";
                        else
                            cmd.CommandText += "2)";
                        cmd.ExecuteNonQuery();

                        cmd.CommandText = "";
                        for (int i = 0; i < tbD.Rows.Count; i++)
                        {
                            cmd.CommandText += "insert into scritd (scname,tano,taname,taform,sc01,sc02,sc03,sc04,sc05,sc06,sc07,sc08,sc09)values("
                                + "'" + SCName.Text.Trim() + "',"
                                + "'" + tbD.Rows[i]["tano"].ToString() + "',"
                                + "'" + tbD.Rows[i]["taname"].ToString() + "',"
                                + "'" + tbD.Rows[i]["taform"].ToString() + "',"
                                + "'" + tbD.Rows[i]["sc01"].ToString() + "',"
                                + "'" + tbD.Rows[i]["sc02"].ToString() + "',"
                                + "'" + tbD.Rows[i]["sc03"].ToString() + "',"
                                + "'" + tbD.Rows[i]["sc04"].ToString() + "',"
                                + "'" + tbD.Rows[i]["sc05"].ToString() + "',"
                                + "'" + tbD.Rows[i]["sc06"].ToString() + "',"
                                + "'" + tbD.Rows[i]["sc07"].ToString() + "',"
                                + "'" + tbD.Rows[i]["sc08"].ToString() + "',"
                                + "'" + tbD.Rows[i]["sc09"].ToString() + "');";
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
                    SCName.Focus();
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
                        cmd.CommandText = "delete scrit where scname='" + CurrentRow + "'";
                        cmd.ExecuteNonQuery();
                        cmd.CommandText = "insert into scrit (scname,scpass,scname1,cono,scsuchk)values("
                        + "'" + SCName.Text.Trim() + "',"
                        + "'" + SCPass.Text.Trim() + "',"
                        + "'" + SCName1.Text.Trim() + "',"
                        + "'" + CoNo.Text.Trim() + "',";
                        if (rd1.Checked)
                            cmd.CommandText += "1)";
                        else
                            cmd.CommandText += "2)";
                        cmd.ExecuteNonQuery();

                        cmd.CommandText = "delete scritd where scname='" + CurrentRow + "'";
                        cmd.ExecuteNonQuery();
                        cmd.CommandText = "";
                        for (int i = 0; i < tbD.Rows.Count; i++)
                        {
                            cmd.CommandText += "insert into scritd (scname,tano,taname,taform,sc01,sc02,sc03,sc04,sc05,sc06,sc07,sc08,sc09)values("
                                + "'" + SCName.Text.Trim() + "',"
                                + "'" + tbD.Rows[i]["tano"].ToString() + "',"
                                + "'" + tbD.Rows[i]["taname"].ToString() + "',"
                                + "'" + tbD.Rows[i]["taform"].ToString() + "',"
                                + "'" + tbD.Rows[i]["sc01"].ToString() + "',"
                                + "'" + tbD.Rows[i]["sc02"].ToString() + "',"
                                + "'" + tbD.Rows[i]["sc03"].ToString() + "',"
                                + "'" + tbD.Rows[i]["sc04"].ToString() + "',"
                                + "'" + tbD.Rows[i]["sc05"].ToString() + "',"
                                + "'" + tbD.Rows[i]["sc06"].ToString() + "',"
                                + "'" + tbD.Rows[i]["sc07"].ToString() + "',"
                                + "'" + tbD.Rows[i]["sc08"].ToString() + "',"
                                + "'" + tbD.Rows[i]["sc09"].ToString() + "');";
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
                }
            }
            CurrentRow = SCName.Text.Trim();
            btnCancel_Click(null, null);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            btnState = "";
            SC.ForEach(r => r.Enabled = false);
            Others.ForEach(r => r.Enabled = true);
            control.ForEach(r => r.ReadOnly = true);
            rd1.Enabled = rd2.Enabled = false;
            loadM();
            if (list.Count > 0)
            {
                dr = GetDataRow(CurrentRow);
                WriteToTxt(dr);
                loadD();
            }
            else
            {
                WriteToTxt(null);
                loadD();
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(Common.sqlConnString))
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = "select * from scrit where scname=N'"
                            + Common.User_Name + "' COLLATE Chinese_Taiwan_Stroke_BIN";
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                Common.使用者簡稱 = reader["scname1"].ToString();
                                Common.使用者預設公司 = reader["cono"].ToString();
                                Common.單據異動 = reader["scsuchk"].ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            this.Dispose();
            this.Close();
        }

        private void dataGridViewT1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (btnState == "") return;
            if (dataGridViewT1.CurrentCell == null) return;
            if (dataGridViewT1.Columns[e.ColumnIndex].Name.ToString() == "作業名稱") return;
            if (Common.User_Name == "BM" && e.RowIndex != -1 && e.RowIndex < dataGridViewT1.Rows.Count)
            {
                if (dataGridViewT1["作業名稱", e.RowIndex].Value.ToString() == "使用者參數設定作業")
                {
                    MessageBox.Show("此項目為系統管理員權限，無法異動", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
            switch (dataGridViewT1.CurrentCell.OwningColumn.Name)
            {
                case "新增":
                case "修改":
                case "刪除":
                case "查詢":
                case "列印":
                case "複製":
                case "瀏覽":
                    if (dataGridViewT1.CurrentCell.Value.Equals("V"))
                        dataGridViewT1.CurrentCell.Value = "";
                    else
                        dataGridViewT1.CurrentCell.Value = "V";
                    if (dataGridViewT1["新增", dataGridViewT1.CurrentRow.Index].Value.ToString() == "V" &&
                        dataGridViewT1["修改", dataGridViewT1.CurrentRow.Index].Value.ToString() == "V" &&
                        dataGridViewT1["刪除", dataGridViewT1.CurrentRow.Index].Value.ToString() == "V" &&
                        dataGridViewT1["查詢", dataGridViewT1.CurrentRow.Index].Value.ToString() == "V" &&
                        dataGridViewT1["列印", dataGridViewT1.CurrentRow.Index].Value.ToString() == "V" &&
                        dataGridViewT1["複製", dataGridViewT1.CurrentRow.Index].Value.ToString() == "V" &&
                        dataGridViewT1["瀏覽", dataGridViewT1.CurrentRow.Index].Value.ToString() == "V")
                    {
                        dataGridViewT1["全開放", dataGridViewT1.CurrentRow.Index].Value = "V";
                        dataGridViewT1["無權限", dataGridViewT1.CurrentRow.Index].Value = "";
                    }
                    else if (dataGridViewT1["瀏覽", dataGridViewT1.CurrentRow.Index].Value.ToString() == "" &&
                        dataGridViewT1["新增", dataGridViewT1.CurrentRow.Index].Value.ToString() == "" &&
                        dataGridViewT1["修改", dataGridViewT1.CurrentRow.Index].Value.ToString() == "" &&
                        dataGridViewT1["刪除", dataGridViewT1.CurrentRow.Index].Value.ToString() == "" &&
                        dataGridViewT1["查詢", dataGridViewT1.CurrentRow.Index].Value.ToString() == "" &&
                        dataGridViewT1["列印", dataGridViewT1.CurrentRow.Index].Value.ToString() == "" &&
                        dataGridViewT1["複製", dataGridViewT1.CurrentRow.Index].Value.ToString() == "")
                    {
                        dataGridViewT1["全開放", dataGridViewT1.CurrentRow.Index].Value = "";
                        dataGridViewT1["無權限", dataGridViewT1.CurrentRow.Index].Value = "V";
                    }
                    else
                    {
                        dataGridViewT1["全開放", dataGridViewT1.CurrentRow.Index].Value = "";
                        dataGridViewT1["無權限", dataGridViewT1.CurrentRow.Index].Value = "";
                    }
                    break;
                case "全開放":
                    if (dataGridViewT1.CurrentCell.Value.Equals(""))
                    {
                        dataGridViewT1["新增", dataGridViewT1.CurrentRow.Index].Value = "V";
                        dataGridViewT1["修改", dataGridViewT1.CurrentRow.Index].Value = "V";
                        dataGridViewT1["刪除", dataGridViewT1.CurrentRow.Index].Value = "V";
                        dataGridViewT1["查詢", dataGridViewT1.CurrentRow.Index].Value = "V";
                        dataGridViewT1["列印", dataGridViewT1.CurrentRow.Index].Value = "V";
                        dataGridViewT1["複製", dataGridViewT1.CurrentRow.Index].Value = "V";
                        dataGridViewT1["瀏覽", dataGridViewT1.CurrentRow.Index].Value = "V";
                        dataGridViewT1["全開放", dataGridViewT1.CurrentRow.Index].Value = "V";
                        dataGridViewT1["無權限", dataGridViewT1.CurrentRow.Index].Value = "";
                    }
                    break;
                case "無權限":
                    if (dataGridViewT1.CurrentCell.Value.Equals(""))
                    {
                        dataGridViewT1["新增", dataGridViewT1.CurrentRow.Index].Value = "";
                        dataGridViewT1["修改", dataGridViewT1.CurrentRow.Index].Value = "";
                        dataGridViewT1["刪除", dataGridViewT1.CurrentRow.Index].Value = "";
                        dataGridViewT1["查詢", dataGridViewT1.CurrentRow.Index].Value = "";
                        dataGridViewT1["列印", dataGridViewT1.CurrentRow.Index].Value = "";
                        dataGridViewT1["複製", dataGridViewT1.CurrentRow.Index].Value = "";
                        dataGridViewT1["瀏覽", dataGridViewT1.CurrentRow.Index].Value = "";
                        dataGridViewT1["全開放", dataGridViewT1.CurrentRow.Index].Value = "";
                        dataGridViewT1["無權限", dataGridViewT1.CurrentRow.Index].Value = "V";
                    }
                    break;
            }
        }

        private void SCName_Validating(object sender, CancelEventArgs e)
        {
            if (btnCancel.Focused) return;
            if (SCName.ReadOnly) return;
            if (btnState == "Modify" && CurrentRow == SCName.Text.Trim()) return;
            using (SqlConnection cn = new SqlConnection(Common.sqlConnString))
            {
                cn.Open();
                SqlCommand cmd = cn.CreateCommand();
                cmd.CommandText = "select scname from scrit where scname='" + SCName.Text.Trim() + "'";
                if (!cmd.ExecuteScalar().IsNullOrEmpty())
                {
                    e.Cancel = true;
                    MessageBox.Show("此使用者名稱已經重複,請重新輸入", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    SCName.SelectAll();
                    return;
                }
            }
        }

        private void 使用者參數設定_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.D1:
                case Keys.NumPad1:
                    btnAppend.PerformClick();
                    break;
                case Keys.D2:
                case Keys.NumPad2:
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

        private void CoNo_DoubleClick(object sender, EventArgs e)
        {
            CHK.CoNo_OpemFrm(CoNo,CoName1);
        }

        private void CoNo_Validating(object sender, CancelEventArgs e)
        {
            if (!CHK.CoNo_Validating(CoNo, CoName1))
            {
                e.Cancel = true;
                CHK.CoNo_OpemFrm(CoNo, CoName1);
            }
        }


    }
}
