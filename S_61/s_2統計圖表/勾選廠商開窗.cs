using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;
using S_61.Basic;
using S_61.Model;
using S_61.MyControl;

namespace S_61.s_2統計圖表
{
    public partial class 勾選廠商開窗 : FormT
    {
        DataTable dt = new DataTable();
        List<DataRow> list = new List<DataRow>();
        DataRow dr;

        string[] arry;
        public string CoNo = "";
        public string result = "";
        public string 勾選字串 = "";

        public 勾選廠商開窗()
        {
            InitializeComponent();
        }

        private void 勾選廠商開窗_Load(object sender, EventArgs e)
        {
            Basic.SetParameter.FontSize(tableLayoutPnl3);
            dataGridViewT1.SetWidthByPixel();
            loadDB();
            //if (list.Count > 0)
            //{
            //    writeToTxt();
            //    int row = list.FindLastIndex(r => string.CompareOrdinal(SeekNo, r["CuNo"].ToString()) > 0) + 1;
            //    row = (row >= list.Count) ? list.Count - 1 : row;
            //    dataGridViewT1.FirstDisplayedScrollingRowIndex = row;
            //    dataGridViewT1.Rows[row].Selected = true;
            //}
            FaNo.Focus();
            if (勾選字串 != "")
            {
                arry = 勾選字串.Substring(0, 勾選字串.Length - 1).Split(',');
                for (int i = 0; i < arry.Length; i++)
                {
                    if (list.Find(r => r["fano"].ToString().Trim() == arry[i].ToString()) != null)
                        list.Find(r => r["fano"].ToString().Trim() == arry[i].ToString())["勾選"] = "V";
                }
            }
            dataGridViewT1.DataSource = dt;
        }

        void loadDB()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(Common.浮動連線字串))
                {
                    conn.Open();
                    string sql = "select 勾選='',* from Fact order by FaNo COLLATE Chinese_Taiwan_Stroke_BIN";
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

        private void btnGet_Click(object sender, EventArgs e)
        {
            勾選字串 = "";
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["勾選"].ToString() == "V")
                    勾選字串 += dt.Rows[i]["fano"].ToString().Trim() + ",";
            }

            this.DialogResult = DialogResult.OK;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Dispose();
            this.Close();
        }

        private void dataGridViewT1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridViewT1.Rows.Count == 0) return;
            if (e.RowIndex < 0 || e.RowIndex > dataGridViewT1.Rows.Count - 1) return;
            if (dataGridViewT1.Columns[e.ColumnIndex].Name != "勾選") return;
            if (dataGridViewT1.Rows[e.RowIndex].Cells["勾選"].Value.ToString().Trim() == "V")
                dt.Rows[e.RowIndex]["勾選"] = "";
            else
                dt.Rows[e.RowIndex]["勾選"] = "V";
        }

        private void 勾選廠商開窗_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.F11:
                    btnExit.PerformClick();
                    break;
                case Keys.F9:
                    btnGet.PerformClick();
                    break;
                case Keys.Escape:
                    btnExit.PerformClick();
                    break;
            }
        }

        private void FaNo_KeyUp(object sender, KeyEventArgs e)
        {
            if (dataGridViewT1.Rows.Count == 0 || FaNo.Text.Trim() == "") return;
            dataGridViewT1.Search("廠商編號", FaNo.Text);
        }

        private void FaPer1_KeyUp(object sender, KeyEventArgs e)
        {
            if (dataGridViewT1.Rows.Count == 0 || FaPer1.Text.Trim() == "") return;
            dataGridViewT1.Search("聯絡人1", FaPer1.Text);
        }

        private void FaAtel1_KeyUp(object sender, KeyEventArgs e)
        {
            if (dataGridViewT1.Rows.Count == 0 || FaAtel1.Text.Trim() == "") return;
            dataGridViewT1.Search("行動電話1", FaAtel1.Text);
        }

        private void FaName1_KeyUp(object sender, KeyEventArgs e)
        {
            if (dataGridViewT1.Rows.Count == 0 || FaName1.Text.Trim() == "") return;
            dataGridViewT1.Search("廠商簡稱", FaName1.Text);
        }

        private void FaIme_KeyUp(object sender, KeyEventArgs e)
        {
            if (dataGridViewT1.Rows.Count == 0 || FaIme.Text.Trim() == "") return;
            dataGridViewT1.Search("注音速查", FaIme.Text);
        }

        private void FaTel1_KeyUp(object sender, KeyEventArgs e)
        {
            if (dataGridViewT1.Rows.Count == 0 || FaTel1.Text.Trim() == "") return;
            dataGridViewT1.Search("電話1", FaTel1.Text);
        }
    }
}
