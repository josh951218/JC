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
    public partial class 廠商建檔作業_瀏覽 : FormT
    {
        DataTable dt = new DataTable();
        List<DataRow> list = new List<DataRow>();
        DataRow dr;

        public bool CanAppend { get; set; }
        public DataRow Result { get; set; }

        public 廠商建檔作業_瀏覽()
        {
            InitializeComponent();
        }

        private void FrmFactb_Load(object sender, EventArgs e)
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
                int row = list.FindLastIndex(r => string.CompareOrdinal(SeekNo, r["FaNo"].ToString()) > 0) + 1;
                row = (row >= list.Count) ? list.Count - 1 : row;
                dataGridViewT1.FirstDisplayedScrollingRowIndex = row;
                dataGridViewT1.Rows[row].Selected = true;
            }
            FaNo.Focus();
        }

        private void loadDB()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(Common.浮動連線字串))
                {
                    string sql = "select * from Fact order by fano COLLATE Chinese_Taiwan_Stroke_BIN";
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
                string TempID = dataGridViewT1.CurrentCell.OwningRow.Cells["廠商編號"].Value.ToString();
                loadDB();
                if (list.Count < 1) return;
                dr = list.Find(r => r.Field<string>("FaNo") == TempID);
                if (dr == null)
                {
                    writeToTxt();
                    MessageBox.Show("您選取的資料已被刪除");
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

        private void FrmFactb_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F2) btnAppend.PerformClick();
            else if (e.KeyCode == Keys.F6) btnQuery.PerformClick();
            else if (e.KeyCode == Keys.F9) btnGet.PerformClick();
            else if (e.KeyCode == Keys.F11) btnExit.PerformClick();
        }

        void QueryFunction(int i)
        {
            try
            {
                SqlConnection conn = new SqlConnection();
                conn.ConnectionString = Common.浮動連線字串;
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();

                string str = "select * from fact where FaNo like('" + FaNo.Text.Trim() + "%')";
                if (FaPer1.Text.Trim() != "") str += " and FaPer1 like('" + FaPer1.Text.Trim() + "%')";
                if (FaAtel1.Text.Trim() != "") str += " and FaAtel1 like('" + FaAtel1.Text.Trim() + "%')";
                if (FaName1.Text.Trim() != "") str += " and FaName1 like('" + FaName1.Text.Trim() + "%')";
                if (FaIme.Text.Trim() != "") str += " and FaIme like('" + FaIme.Text.Trim() + "%')";
                if (FaTel1.Text.Trim() != "") str += " and FaTel1 like('" + FaTel1.Text.Trim() + "%')";
                if (FaUdf1.Text.Trim() != "") str += " and FaUdf1 like('" + FaUdf1.Text.Trim() + "%')";

                str += "COLLATE Chinese_Taiwan_Stroke_BIN";

                cmd.CommandText = str;
                using (SqlDataReader rd = cmd.ExecuteReader())
                {
                    str = "0";
                    if (rd.HasRows)
                    {
                        if (rd.Read())
                            str = rd["FaNo"].ToString();
                    }
                }
                cmd.Dispose();
                conn.Close(); conn.Dispose();

                if (str != "0")
                {
                    i = list.FindIndex(r => r.Field<string>("FaNo") == str);
                    if (i == -1) return;
                    dataGridViewT1.FirstDisplayedScrollingRowIndex = i;
                    dataGridViewT1.Rows[i].Cells[0].Selected = true;
                }
                else
                {
                    if (FaNo.Text.Trim() != "")
                    {
                        i = list.FindIndex(r => r.Field<string>("FaNo") == FaNo.Text.Trim());
                        if (i != -1)
                        {
                            dataGridViewT1.FirstDisplayedScrollingRowIndex = i;
                            dataGridViewT1.Rows[i].Cells[0].Selected = true;
                        }
                        else
                        {
                            i = list.FindLastIndex(r => string.CompareOrdinal(FaNo.Text.Trim(), r.Field<string>("FaNo")) > 0);
                            if (i == -1) return;
                            dataGridViewT1.FirstDisplayedScrollingRowIndex = i;
                            dataGridViewT1.Rows[i].Cells[0].Selected = true;
                        }
                        return;
                    }
                    if (FaPer1.Text.Trim() != "")
                    {
                        i = list.FindIndex(r => r.Field<string>("FaPer1") == FaPer1.Text.Trim());
                        if (i != -1)
                        {
                            dataGridViewT1.FirstDisplayedScrollingRowIndex = i;
                            dataGridViewT1.Rows[i].Cells[0].Selected = true;
                        }
                        else
                        {
                            i = list.FindLastIndex(r => string.CompareOrdinal(FaPer1.Text.Trim(), r.Field<string>("FaPer1")) > 0);
                            if (i == -1) return;
                            dataGridViewT1.FirstDisplayedScrollingRowIndex = i;
                            dataGridViewT1.Rows[i].Cells[0].Selected = true;
                        }
                        return;
                    }
                    if (FaAtel1.Text.Trim() != "")
                    {
                        i = list.FindIndex(r => r.Field<string>("FaAtel1") == FaAtel1.Text.Trim());
                        if (i != -1)
                        {
                            dataGridViewT1.FirstDisplayedScrollingRowIndex = i;
                            dataGridViewT1.Rows[i].Cells[0].Selected = true;
                        }
                        else
                        {
                            i = list.FindLastIndex(r => string.CompareOrdinal(FaAtel1.Text.Trim(), r.Field<string>("FaAtel1")) > 0);
                            if (i == -1) return;
                            dataGridViewT1.FirstDisplayedScrollingRowIndex = i;
                            dataGridViewT1.Rows[i].Cells[0].Selected = true;
                        }
                        return;
                    }
                    if (FaName1.Text.Trim() != "")
                    {
                        i = list.FindIndex(r => r.Field<string>("FaName1") == FaName1.Text.Trim());
                        if (i != -1)
                        {
                            dataGridViewT1.FirstDisplayedScrollingRowIndex = i;
                            dataGridViewT1.Rows[i].Cells[0].Selected = true;
                        }
                        else
                        {
                            i = list.FindLastIndex(r => string.CompareOrdinal(FaName1.Text.Trim(), r.Field<string>("FaName1")) > 0);
                            if (i == -1) return;
                            dataGridViewT1.FirstDisplayedScrollingRowIndex = i;
                            dataGridViewT1.Rows[i].Cells[0].Selected = true;
                        }
                        return;
                    }
                    if (FaIme.Text.Trim() != "")
                    {
                        i = list.FindIndex(r => r.Field<string>("FaIme") == FaIme.Text.Trim());
                        if (i != -1)
                        {
                            dataGridViewT1.FirstDisplayedScrollingRowIndex = i;
                            dataGridViewT1.Rows[i].Cells[0].Selected = true;
                        }
                        else
                        {
                            i = list.FindLastIndex(r => string.CompareOrdinal(FaIme.Text.Trim(), r.Field<string>("FaIme")) > 0);
                            if (i == -1) return;
                            dataGridViewT1.FirstDisplayedScrollingRowIndex = i;
                            dataGridViewT1.Rows[i].Cells[0].Selected = true;
                        }
                        return;
                    }
                    if (FaTel1.Text.Trim() != "")
                    {
                        i = list.FindIndex(r => r.Field<string>("FaTel1") == FaTel1.Text.Trim());
                        if (i != -1)
                        {
                            dataGridViewT1.FirstDisplayedScrollingRowIndex = i;
                            dataGridViewT1.Rows[i].Cells[0].Selected = true;
                        }
                        else
                        {
                            i = list.FindLastIndex(r => string.CompareOrdinal(FaTel1.Text.Trim(), r.Field<string>("FaTel1")) > 0);
                            if (i == -1) return;
                            dataGridViewT1.FirstDisplayedScrollingRowIndex = i;
                            dataGridViewT1.Rows[i].Cells[0].Selected = true;
                        }
                        return;
                    }

                    if (FaUdf1.Text.Trim() != "")
                    {
                        i = list.FindIndex(r => r.Field<string>("FaUdf1") == FaUdf1.Text.Trim());
                        if (i != -1)
                        {
                            dataGridViewT1.FirstDisplayedScrollingRowIndex = i;
                            dataGridViewT1.Rows[i].Cells[0].Selected = true;
                        }
                        else
                        {
                            i = list.FindLastIndex(r => string.CompareOrdinal(FaUdf1.Text.Trim(), r.Field<string>("FaUdf1")) > 0);
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

        private void FaNo_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode.ToString().StartsWith("F")) return;
            if (FaNo.Text.Trim() == ""
                && FaPer1.Text.Trim() == ""
                && FaAtel1.Text.Trim() == ""
                && FaName1.Text.Trim() == ""
                && FaIme.Text.Trim() == ""
                && FaTel1.Text.Trim() == ""
                && FaUdf1.Text.Trim() == "")
                return;
            QueryFunction(0);
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            if (FaNo.Text.Trim() == ""
                && FaPer1.Text.Trim() == ""
                && FaAtel1.Text.Trim() == ""
                && FaName1.Text.Trim() == ""
                && FaIme.Text.Trim() == ""
                && FaTel1.Text.Trim() == ""
                && FaUdf1.Text.Trim() == "")
                return;
            try
            {
                using (SqlConnection conn = new SqlConnection(Common.浮動連線字串))
                {
                    string str = "select FaNo,FaName1,FaPer1,FaIme,FaTel1,FaAtel1,FaUdf1,FaFax1,FaPer2,FaTel2,FaAtel2,FaTel3,FaName2,FaPer,FaEmno1,FaUno,FaX12no,FaAddr1,FaAddr2,FaAddr3,FaUdf2 from Fact";
                    str += " where '0'='0'";
                    foreach (Control tb in tableLayoutPnl2.Controls)
                    {
                        if (tb is TextBox)
                        {
                            if (tb.Text.Trim() != "") str += " and " + tb.Name + " like '%" + tb.Text + "%'";
                            (tb as TextBox).Enter += new EventHandler(Text_OnEnter);
                        }
                    }
                    str += " order by fano COLLATE Chinese_Taiwan_Stroke_BIN";
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
            廠商建檔作業 frm = new 廠商建檔作業();
            frm.SetParaeter();
            frm.ShowDialog();
            loadDB();
            if (dt.Rows.Count > 0)
                dataGridViewT1.DataSource = dt;
        }
















    }
}
