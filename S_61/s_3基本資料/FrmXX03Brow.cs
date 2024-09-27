using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;
using S_61.Basic;
using S_61.Model;

namespace S_61.s_3基本資料
{
    public partial class FrmXX03Brow : FormT
    {
        DataTable dt = new DataTable();
        List<DataRow> list = new List<DataRow>();
        DataRow dr;
        string TempID;

        //public string SeekNo { get; set; }
        //public bool CanAppend { get; set; }
        //public ViewMode ViewStyle { get; set; }
        public enum ViewMode : int { Normal = 0, Big = 1 }
        public DataRow Result { get; set; }

        public FrmXX03Brow()
        {
            InitializeComponent();
        }

        private void FrmXX03Brow_Load(object sender, EventArgs e)
        {
            //if (ViewStyle == ViewMode.Normal)
            //    Basic.SetParameter.FormSize(this);
            //if (ViewStyle == ViewMode.Big)
            //    Basic.SetParameter.FormParameters(this);

            //if (CanAppend)
            //{
            //    btnAppend.Visible = true;
            //    tableLayoutPnl3.ColumnStyles[1].SizeType = SizeType.Percent;
            //}
            //else
            //{
            //    btnAppend.Visible = false;
            //    tableLayoutPnl3.ColumnStyles[1].SizeType = SizeType.AutoSize;
            //}

            Basic.SetParameter.FontSize(tableLayoutPnl3);
            dataGridViewT1.AutoGenerateColumns = false;
            //gridView欄位寬度設定
            for (int i = 0; i < dataGridViewT1.Columns.Count; i++)
            {
                dataGridViewT1.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                dataGridViewT1.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                dataGridViewT1.Columns[i].Width = pVar.ScreenWidth / 3 - 18;
            }

            loadDB();
            if (dt.Rows.Count > 0)
            {
                writeToTxt();
                int row = list.FindLastIndex(r => string.CompareOrdinal(SeekNo, r["X3No"].ToString()) > 0) + 1;
                row = (row >= list.Count) ? list.Count - 1 : row;
                dataGridViewT1.FirstDisplayedScrollingRowIndex = row;
                dataGridViewT1.Rows[row].Selected = true;
            }
            X3No.Focus();
        }

        private void loadDB()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(Common.sqlConnString))
                {
                    string sql = "select * from XX03 order by X3No COLLATE Chinese_Taiwan_Stroke_BIN";
                    using (SqlDataAdapter da = new SqlDataAdapter(sql, conn))
                    {
                        dt.Clear(); list.Clear();

                        da.Fill(dt);
                        if (dt.Rows.Count > 0)
                        {
                            list = dt.AsEnumerable().ToList();
                            lblT1.Text = "總共有 " + list.Count.ToString() + " 筆資料";
                        }
                        else
                            lblT1.Text = "";
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
                TempID = dataGridViewT1.SelectedRows[0].Cells["稅別編號"].Value.ToString();
                loadDB();
                if (list.Count < 1) return;
                dr = list.Find(r => r.Field<string>("X3No") == TempID);
                if (dr == null)
                {
                    writeToTxt();
                    MessageBox.Show("您選取的資料已被刪除", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                Result = dr;
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

        private void FrmXX03Brow_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F6) btnQuery.PerformClick();
            if (e.KeyCode == Keys.F9) btnGet.PerformClick();
            if (e.KeyCode == Keys.F11) btnExit.PerformClick();
        }

        private void X3No_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode.ToString().StartsWith("F")) return;
            dataGridViewT1.Search("稅別編號", X3No.Text, "稅別名稱", X3Name.Text);
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            if (X3No.Text.Trim() == "" && X3Name.Text.Trim() == "") return;

            try
            {
                using (SqlConnection conn = new SqlConnection(Common.sqlConnString))
                {
                    string str = "select * from XX03";
                    str += " where '0'='0'";
                    foreach (Control tb in tableLayoutPnl2.Controls)
                    {
                        if (tb is TextBox)
                        {
                            if (tb.Text.Trim() != "") str += " and " + tb.Name + " like '%" + tb.Text + "%'";
                            (tb as TextBox).Enter += new EventHandler(Text_OnEnter);
                        }
                    }
                    str += " order by X3No COLLATE Chinese_Taiwan_Stroke_BIN";
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