using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;
using S_61.Basic;
using S_61.Model;
using S_61.MyControl;

namespace S_61.s_3基本資料
{
    public partial class 人員建檔作業_瀏覽 : FormT
    {
        DataTable dt = new DataTable();
        List<DataRow> list = new List<DataRow>();
        DataRow dr;

        public bool CanAppend { get; set; }
        public DataRow Result { get; set; }


        public 人員建檔作業_瀏覽()
        {
            InitializeComponent();
        }

        private void FrmEmplb_Load(object sender, EventArgs e)
        {
            if (CanAppend)
            {
                btnAppend.Visible = true;
                tableLayoutPnl3.ColumnStyles[1].SizeType = SizeType.Percent;
            }
            else
            {
                btnAppend.Visible = false;
                tableLayoutPnl3.ColumnStyles[1].SizeType = SizeType.AutoSize;
            }

            Basic.SetParameter.FontSize(tableLayoutPnl3);
            //gridView欄位寬度設定
            int maxLen;
            for (int i = 0; i < dataGridViewT1.Columns.Count; i++)
            {
                dataGridViewT1.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                maxLen = ((DataGridViewTextBoxColumn)dataGridViewT1.Columns[i]).MaxInputLength;
                if (5 < maxLen && maxLen < 80)
                {
                    dataGridViewT1.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                    dataGridViewT1.Columns[i].Width = maxLen * 9;
                }
            }
            loadDB();
            if (dt.Rows.Count > 0)
            {
                writeToTxt();
                int row = list.FindLastIndex(r => string.CompareOrdinal(SeekNo, r["EmNo"].ToString()) > 0) + 1;
                row = (row >= list.Count) ? list.Count - 1 : row;
                dataGridViewT1.FirstDisplayedScrollingRowIndex = row;
                dataGridViewT1.Rows[row].Selected = true;
            }
            EmNo.Focus();
        }

        private void loadDB()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(Common.浮動連線字串))
                {
                    string sql = "select * from empl order by emno COLLATE Chinese_Taiwan_Stroke_BIN";
                    using (SqlDataAdapter da = new SqlDataAdapter(sql, conn))
                    {
                        dt.Clear(); list.Clear();

                        da.Fill(dt);
                        if (dt.Rows.Count > 0)
                        {
                            list = dt.AsEnumerable().ToList();
                            lblT3.Text = "總共有 " + list.Count.ToString() + " 筆資料";
                        }
                        else
                            lblT3.Text = "";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void writeToTxt()
        {
            if (dt.Rows.Count > 0)
                dataGridViewT1.DataSource = dt;
            else
                dataGridViewT1.DataSource = null;
        }



        //功能按鈕

        private void btnGet_Click(object sender, EventArgs e)
        {
            if (dataGridViewT1.Rows.Count > 0)
            {
                string TempID = dataGridViewT1.SelectedRows[0].Cells["員工編號"].Value.ToString();
                loadDB();
                if (list.Count < 1) return;
                dr = list.Find(r => r.Field<string>("EmNo") == TempID);
                if (dr == null)
                {
                    writeToTxt();
                    MessageBox.Show("您選取的資料已被刪除", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                this.Result = dr;
                this.DialogResult = DialogResult.OK;
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }



        //其它
        private void dataGridViewT1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            btnGet_Click(null, null);
        }


        private void EmNo_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode.ToString().StartsWith("F")) return;
            if (EmNo.Text.Trim() == ""
                && EmName.Text.Trim() == ""
                && EmSex.Text.Trim() == ""
                && EmReg.Text.Trim() == ""
                && EmIdno.Text.Trim() == ""
                && EmTel.Text.Trim() == ""
                && EmAtel1.Text.Trim() == "")
                return;
            QueryFunction(0);
        }

        void QueryFunction(int i)
        {
            try
            {
                SqlConnection conn = new SqlConnection();
                conn.ConnectionString = Common.浮動連線字串;
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();

                string str = "select * from empl where EmNo like('" + EmNo.Text.Trim() + "%')";
                if (EmName.Text.Trim() != "") str += " and EmName like('" + EmName.Text.Trim() + "%')";
                if (EmSex.Text.Trim() != "") str += " and EmSex like('" + EmSex.Text.Trim() + "%')";
                if (EmReg.Text.Trim() != "") str += " and EmReg like('" + EmReg.Text.Trim() + "%')";
                if (EmIdno.Text.Trim() != "") str += " and EmIdno like('" + EmIdno.Text.Trim() + "%')";
                if (EmTel.Text.Trim() != "") str += " and EmTel like('" + EmTel.Text.Trim() + "%')";
                if (EmAtel1.Text.Trim() != "") str += " and EmAtel1 like('" + EmAtel1.Text.Trim() + "%')";

                str += " COLLATE Chinese_Taiwan_Stroke_BIN";

                cmd.CommandText = str;
                using (SqlDataReader rd = cmd.ExecuteReader())
                {
                    str = "0";
                    if (rd.HasRows)
                    {
                        if (rd.Read())
                            str = rd["EmNo"].ToString();
                    }
                }
                cmd.Dispose();
                conn.Close(); conn.Dispose();

                if (str != "0")
                {
                    i = list.FindIndex(r => r.Field<string>("EmNo") == str);
                    if (i == -1) return;
                    dataGridViewT1.FirstDisplayedScrollingRowIndex = i;
                    dataGridViewT1.Rows[i].Cells[0].Selected = true;
                }
                else
                {
                    if (EmNo.Text.Trim() != "")
                    {
                        i = list.FindIndex(r => r.Field<string>("EmNo") == EmNo.Text.Trim());
                        if (i != -1)
                        {
                            dataGridViewT1.FirstDisplayedScrollingRowIndex = i;
                            dataGridViewT1.Rows[i].Cells[0].Selected = true;
                        }
                        else
                        {
                            i = list.FindLastIndex(r => string.CompareOrdinal(EmNo.Text.Trim(), r.Field<string>("EmNo")) > 0);
                            if (i == -1) return;
                            dataGridViewT1.FirstDisplayedScrollingRowIndex = i;
                            dataGridViewT1.Rows[i].Cells[0].Selected = true;
                        }
                        return;
                    }
                    if (EmName.Text.Trim() != "")
                    {
                        i = list.FindIndex(r => r.Field<string>("EmName") == EmName.Text.Trim());
                        if (i != -1)
                        {
                            dataGridViewT1.FirstDisplayedScrollingRowIndex = i;
                            dataGridViewT1.Rows[i].Cells[0].Selected = true;
                        }
                        else
                        {
                            i = list.FindLastIndex(r => string.CompareOrdinal(EmName.Text.Trim(), r.Field<string>("EmName")) > 0);
                            if (i == -1) return;
                            dataGridViewT1.FirstDisplayedScrollingRowIndex = i;
                            dataGridViewT1.Rows[i].Cells[0].Selected = true;
                        }
                        return;
                    }
                    if (EmSex.Text.Trim() != "")
                    {
                        i = list.FindIndex(r => r.Field<string>("EmSex") == EmSex.Text.Trim());
                        if (i != -1)
                        {
                            dataGridViewT1.FirstDisplayedScrollingRowIndex = i;
                            dataGridViewT1.Rows[i].Cells[0].Selected = true;
                        }
                        else
                        {
                            i = list.FindLastIndex(r => string.CompareOrdinal(EmSex.Text.Trim(), r.Field<string>("EmSex")) > 0);
                            if (i == -1) return;
                            dataGridViewT1.FirstDisplayedScrollingRowIndex = i;
                            dataGridViewT1.Rows[i].Cells[0].Selected = true;
                        }
                        return;
                    }
                    if (EmReg.Text.Trim() != "")
                    {
                        i = list.FindIndex(r => r.Field<string>("EmReg") == EmReg.Text.Trim());
                        if (i != -1)
                        {
                            dataGridViewT1.FirstDisplayedScrollingRowIndex = i;
                            dataGridViewT1.Rows[i].Cells[0].Selected = true;
                        }
                        else
                        {
                            i = list.FindLastIndex(r => string.CompareOrdinal(EmReg.Text.Trim(), r.Field<string>("EmReg")) > 0);
                            if (i == -1) return;
                            dataGridViewT1.FirstDisplayedScrollingRowIndex = i;
                            dataGridViewT1.Rows[i].Cells[0].Selected = true;
                        }
                        return;
                    }
                    if (EmIdno.Text.Trim() != "")
                    {
                        i = list.FindIndex(r => r.Field<string>("EmIdno") == EmIdno.Text.Trim());
                        if (i != -1)
                        {
                            dataGridViewT1.FirstDisplayedScrollingRowIndex = i;
                            dataGridViewT1.Rows[i].Cells[0].Selected = true;
                        }
                        else
                        {
                            i = list.FindLastIndex(r => string.CompareOrdinal(EmIdno.Text.Trim(), r.Field<string>("EmIdno")) > 0);
                            if (i == -1) return;
                            dataGridViewT1.FirstDisplayedScrollingRowIndex = i;
                            dataGridViewT1.Rows[i].Cells[0].Selected = true;
                        }
                        return;
                    }
                    if (EmTel.Text.Trim() != "")
                    {
                        i = list.FindIndex(r => r.Field<string>("EmTel") == EmTel.Text.Trim());
                        if (i != -1)
                        {
                            dataGridViewT1.FirstDisplayedScrollingRowIndex = i;
                            dataGridViewT1.Rows[i].Cells[0].Selected = true;
                        }
                        else
                        {
                            i = list.FindLastIndex(r => string.CompareOrdinal(EmTel.Text.Trim(), r.Field<string>("EmTel")) > 0);
                            if (i == -1) return;
                            dataGridViewT1.FirstDisplayedScrollingRowIndex = i;
                            dataGridViewT1.Rows[i].Cells[0].Selected = true;
                        }
                        return;
                    }

                    if (EmAtel1.Text.Trim() != "")
                    {
                        i = list.FindIndex(r => r.Field<string>("EmAtel1") == EmAtel1.Text.Trim());
                        if (i != -1)
                        {
                            dataGridViewT1.FirstDisplayedScrollingRowIndex = i;
                            dataGridViewT1.Rows[i].Cells[0].Selected = true;
                        }
                        else
                        {
                            i = list.FindLastIndex(r => string.CompareOrdinal(EmAtel1.Text.Trim(), r.Field<string>("EmAtel1")) > 0);
                            if (i == -1) return;
                            dataGridViewT1.FirstDisplayedScrollingRowIndex = i;
                            dataGridViewT1.Rows[i].Cells[0].Selected = true;
                        }
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            if (EmNo.Text.Trim() == ""
                && EmName.Text.Trim() == ""
                && EmSex.Text.Trim() == ""
                && EmReg.Text.Trim() == ""
                && EmIdno.Text.Trim() == ""
                && EmTel.Text.Trim() == ""
                && EmAtel1.Text.Trim() == "")
                return;
            try
            {
                using (SqlConnection conn = new SqlConnection(Common.浮動連線字串))
                {
                    string str = "select * from Empl";
                    str += " where '0'='0'";
                    foreach (Control tb in tableLayoutPnl2.Controls)
                    {
                        if (tb is TextBox)
                        {
                            if (tb.Text.Trim() != "") str += " and " + tb.Name + " like '%" + tb.Text + "%'";
                            (tb as TextBox).Enter += new EventHandler(Text_OnEnter);
                        }
                    }
                    str += " order by emno COLLATE Chinese_Taiwan_Stroke_BIN";
                    SqlDataAdapter da = new SqlDataAdapter(str, conn);
                    dt.Clear();
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        list.Clear();
                        list = dt.AsEnumerable().ToList();
                    }
                    else
                    {
                        list.Clear();
                    }
                    da.Dispose();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void Text_OnEnter(object sender, EventArgs e)
        {
            loadDB();
            foreach (Control tb in (sender as Control).Parent.Controls)
            {
                if (tb is TextBox)
                {
                    (tb as TextBox).Text = "";
                    (tb as TextBox).Enter -= Text_OnEnter;
                }
            }
        }

        private void btnAppend_Click(object sender, EventArgs e)
        {
            人員建檔作業 frm = new 人員建檔作業();
            frm.SetParaeter();
            frm.ShowDialog();
            loadDB();
            if (dt.Rows.Count > 0)
                dataGridViewT1.DataSource = dt;
        }

        private void FrmEmplb_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F2) btnAppend.PerformClick();
            else if (e.KeyCode == Keys.F6) btnQuery.PerformClick();
            else if (e.KeyCode == Keys.F9) btnGet.PerformClick();
            else if (e.KeyCode == Keys.F11) btnExit.PerformClick();
        }














    }
}
