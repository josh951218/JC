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
    public partial class 客戶建檔作業_瀏覽 : FormT
    {
        DataTable dt = new DataTable();
        List<DataRow> list = new List<DataRow>();
        DataRow dr;

        public bool CanAppend { get; set; }
        public DataRow Result { get; set; }

        public 客戶建檔作業_瀏覽()
        {
            InitializeComponent();
        }

        private void FrmCustb_Load(object sender, EventArgs e)
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
                int row = list.FindLastIndex(r => string.CompareOrdinal(SeekNo, r["CuNo"].ToString()) > 0) + 1;
                row = (row >= list.Count) ? list.Count - 1 : row;
                dataGridViewT1.FirstDisplayedScrollingRowIndex = row;
                dataGridViewT1.Rows[row].Selected = true;
            }
            CuNo.Focus();
        }

        void loadDB()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(Common.浮動連線字串))
                {
                    conn.Open();
                    string sql = "select * from Cust order by CuNo COLLATE Chinese_Taiwan_Stroke_BIN";
                    using (SqlDataAdapter da = new SqlDataAdapter(sql, conn))
                    {
                        dt.Clear(); list.Clear();

                        da.Fill(dt);
                        if (dt.Rows.Count > 0)
                        {
                            list = dt.AsEnumerable().ToList();
                            lblT8.Text = "總共有 " + list.Count.ToString() + " 筆資料";
                        }
                        else
                            lblT8.Text = "";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        void writeToTxt()
        {
            if (dt.Rows.Count > 0)
                dataGridViewT1.DataSource = dt;
            else
                dataGridViewT1.DataSource = null;
        }



        //功能按鈕
        private void btnAppend_Click(object sender, EventArgs e)
        {
            客戶建檔作業 frm = new 客戶建檔作業();
            frm.SetParaeter();
            frm.ShowDialog();
            loadDB();
        }

        private void btnGet_Click(object sender, EventArgs e)
        {
            if (dataGridViewT1.Rows.Count > 0)
            {
                string TempID = dataGridViewT1.SelectedRows[0].Cells["客戶編號"].Value.ToString();
                loadDB();
                if (list.Count < 1) return;
                dr = list.Find(r => r["CuNo"].ToString() == TempID);
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

        private void FrmCustb_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F2) btnAppend.PerformClick();
            else if (e.KeyCode == Keys.F6) btnQuery.PerformClick();
            else if (e.KeyCode == Keys.F9) btnGet.PerformClick();
            else if (e.KeyCode == Keys.F11) btnExit.PerformClick();
        }

        private void CuNo_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode.ToString().StartsWith("F")) return;
            if (CuNo.Text.Trim() == ""
                && CuPer1.Text.Trim() == ""
                && CuAtel1.Text.Trim() == ""
                && CuName1.Text.Trim() == ""
                && CuIme.Text.Trim() == ""
                && CuTel1.Text.Trim() == ""
                && CuUdf1.Text.Trim() == "")
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

                string str = "select * from cust where '0'='0' ";
                if (CuNo.Text.Trim() != "") str += " and CuNo like('" + CuNo.Text.Trim() + "%')";
                if (CuPer1.Text.Trim() != "") str += " and CuPer1 like('" + CuPer1.Text.Trim() + "%')";
                if (CuAtel1.Text.Trim() != "") str += " and CuAtel1 like('" + CuAtel1.Text.Trim() + "%')";
                if (CuName1.Text.Trim() != "") str += " and CuName1 like('" + CuName1.Text.Trim() + "%')";
                if (CuIme.Text.Trim() != "") str += " and CuIme like('" + CuIme.Text.Trim() + "%')";
                if (CuTel1.Text.Trim() != "") str += " and CuTel1 like('" + CuTel1.Text.Trim() + "%')";
                if (CuUdf1.Text.Trim() != "") str += " and CuUdf1 like('" + CuUdf1.Text.Trim() + "%')";

                cmd.CommandText = str;
                using (SqlDataReader rd = cmd.ExecuteReader())
                {
                    str = "0";
                    if (rd.HasRows)
                    {
                        if (rd.Read())
                            str = rd["CuNo"].ToString();
                    }
                }
                cmd.Dispose();
                conn.Close(); conn.Dispose();

                if (str != "0")
                {
                    i = list.FindIndex(r => r.Field<string>("CuNo") == str);
                    if (i == -1) return;
                    dataGridViewT1.FirstDisplayedScrollingRowIndex = i;
                    dataGridViewT1.Rows[i].Cells[0].Selected = true;
                }
                else
                {
                    if (CuNo.Text.Trim() != "")
                    {
                        i = list.FindIndex(r => r.Field<string>("CuNo") == CuNo.Text.Trim());
                        if (i != -1)
                        {
                            dataGridViewT1.FirstDisplayedScrollingRowIndex = i;
                            dataGridViewT1.Rows[i].Cells[0].Selected = true;
                        }
                        else
                        {
                            i = list.FindLastIndex(r => string.CompareOrdinal(CuNo.Text.Trim(), r.Field<string>("CuNo")) > 0);
                            if (i == -1) return;
                            dataGridViewT1.FirstDisplayedScrollingRowIndex = i;
                            dataGridViewT1.Rows[i].Cells[0].Selected = true;
                        }
                        return;
                    }
                    if (CuPer1.Text.Trim() != "")
                    {
                        i = list.FindIndex(r => r.Field<string>("CuPer1") == CuPer1.Text.Trim());
                        if (i != -1)
                        {
                            dataGridViewT1.FirstDisplayedScrollingRowIndex = i;
                            dataGridViewT1.Rows[i].Cells[0].Selected = true;
                        }
                        else
                        {
                            i = list.FindLastIndex(r => string.CompareOrdinal(CuPer1.Text.Trim(), r.Field<string>("CuPer1")) > 0);
                            if (i == -1) return;
                            dataGridViewT1.FirstDisplayedScrollingRowIndex = i;
                            dataGridViewT1.Rows[i].Cells[0].Selected = true;
                        }
                        return;
                    }
                    if (CuAtel1.Text.Trim() != "")
                    {
                        i = list.FindIndex(r => r.Field<string>("CuAtel1") == CuAtel1.Text.Trim());
                        if (i != -1)
                        {
                            dataGridViewT1.FirstDisplayedScrollingRowIndex = i;
                            dataGridViewT1.Rows[i].Cells[0].Selected = true;
                        }
                        else
                        {
                            i = list.FindLastIndex(r => string.CompareOrdinal(CuAtel1.Text.Trim(), r.Field<string>("CuAtel1")) > 0);
                            if (i == -1) return;
                            dataGridViewT1.FirstDisplayedScrollingRowIndex = i;
                            dataGridViewT1.Rows[i].Cells[0].Selected = true;
                        }
                        return;
                    }
                    if (CuName1.Text.Trim() != "")
                    {
                        i = list.FindIndex(r => r.Field<string>("CuName1") == CuName1.Text.Trim());
                        if (i != -1)
                        {
                            dataGridViewT1.FirstDisplayedScrollingRowIndex = i;
                            dataGridViewT1.Rows[i].Cells[0].Selected = true;
                        }
                        else
                        {
                            i = list.FindLastIndex(r => string.CompareOrdinal(CuName1.Text.Trim(), r.Field<string>("CuName1")) > 0);
                            if (i == -1) return;
                            dataGridViewT1.FirstDisplayedScrollingRowIndex = i;
                            dataGridViewT1.Rows[i].Cells[0].Selected = true;
                        }
                        return;
                    }
                    if (CuIme.Text.Trim() != "")
                    {
                        i = list.FindIndex(r => r.Field<string>("CuIme") == CuIme.Text.Trim());
                        if (i != -1)
                        {
                            dataGridViewT1.FirstDisplayedScrollingRowIndex = i;
                            dataGridViewT1.Rows[i].Cells[0].Selected = true;
                        }
                        else
                        {
                            i = list.FindLastIndex(r => string.CompareOrdinal(CuIme.Text.Trim(), r.Field<string>("CuIme")) > 0);
                            if (i == -1) return;
                            dataGridViewT1.FirstDisplayedScrollingRowIndex = i;
                            dataGridViewT1.Rows[i].Cells[0].Selected = true;
                        }
                        return;
                    }
                    if (CuTel1.Text.Trim() != "")
                    {
                        i = list.FindIndex(r => r.Field<string>("CuTel1") == CuTel1.Text.Trim());
                        if (i != -1)
                        {
                            dataGridViewT1.FirstDisplayedScrollingRowIndex = i;
                            dataGridViewT1.Rows[i].Cells[0].Selected = true;
                        }
                        else
                        {
                            i = list.FindLastIndex(r => string.CompareOrdinal(CuTel1.Text.Trim(), r.Field<string>("CuTel1")) > 0);
                            if (i == -1) return;
                            dataGridViewT1.FirstDisplayedScrollingRowIndex = i;
                            dataGridViewT1.Rows[i].Cells[0].Selected = true;
                        }
                        return;
                    }

                    if (CuUdf1.Text.Trim() != "")
                    {
                        i = list.FindIndex(r => r.Field<string>("CuUdf1") == CuUdf1.Text.Trim());
                        if (i != -1)
                        {
                            dataGridViewT1.FirstDisplayedScrollingRowIndex = i;
                            dataGridViewT1.Rows[i].Cells[0].Selected = true;
                        }
                        else
                        {
                            i = list.FindLastIndex(r => string.CompareOrdinal(CuUdf1.Text.Trim(), r.Field<string>("CuUdf1")) > 0);
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
            bool b = true;
            foreach (var item in tableLayoutPnl2.Controls)
            {
                if (item is TextBox && (item as TextBox).Text != "") b = false;
            }
            if (b) return;
            try
            {
                using (SqlConnection conn = new SqlConnection(Common.浮動連線字串))
                {
                    string str = "select * from cust";
                    str += " where '0'='0'";
                    foreach (Control tb in tableLayoutPnl2.Controls)
                    {
                        if (tb is TextBox)
                        {
                            if (tb.Text.Trim() != "") str += " and " + tb.Name + " like '%" + tb.Text + "%'";
                            (tb as TextBox).Enter += new EventHandler(Text_OnEnter);
                        }
                    }
                    str += " order by cuno COLLATE Chinese_Taiwan_Stroke_BIN";

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












    }
}
