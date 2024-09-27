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
    public partial class 勾選客戶開窗 : FormT
    {
        DataTable dt = new DataTable();
        List<DataRow> list = new List<DataRow>();
        DataRow dr;

        string[] arry;
        public string CoNo = "";
        public string result = "";
        public string 勾選字串 = "";

        public 勾選客戶開窗()
        {
            InitializeComponent();
        }

        private void 多選客戶開窗_Load(object sender, EventArgs e)
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
            CuNo.Focus();
            if (勾選字串 != "")
            {
                arry = 勾選字串.Substring(0, 勾選字串.Length - 1).Split(',');
                for (int i = 0; i < arry.Length; i++)
                {
                    if (list.Find(r => r["cuno"].ToString().Trim() == arry[i].ToString()) != null)
                        list.Find(r => r["cuno"].ToString().Trim() == arry[i].ToString())["勾選"] = "V";
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
                    string sql = "select 勾選='',* from Cust order by CuNo COLLATE Chinese_Taiwan_Stroke_BIN";
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
                    勾選字串 += dt.Rows[i]["cuno"].ToString().Trim() + ",";
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

        private void 多選客戶開窗_KeyUp(object sender, KeyEventArgs e)
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

        private void CuNo_KeyUp(object sender, KeyEventArgs e)
        {
            if (dataGridViewT1.Rows.Count == 0 || CuNo.Text.Trim() == "") return;
            dataGridViewT1.Search("客戶編號", CuNo.Text);
        }

        private void CuPer1_KeyUp(object sender, KeyEventArgs e)
        {
            if (dataGridViewT1.Rows.Count == 0 || CuPer1.Text.Trim() == "") return;
            dataGridViewT1.Search("聯絡人一", CuPer1.Text);
        }

        private void CuAtel1_KeyUp(object sender, KeyEventArgs e)
        {
            if (dataGridViewT1.Rows.Count == 0 || CuAtel1.Text.Trim() == "") return;
            dataGridViewT1.Search("行動電話一", CuAtel1.Text);
        }

        private void CuName1_KeyUp(object sender, KeyEventArgs e)
        {
            if (dataGridViewT1.Rows.Count == 0 || CuName1.Text.Trim() == "") return;
            dataGridViewT1.Search("客戶簡稱", CuName1.Text);
        }

        private void CuIme_KeyUp(object sender, KeyEventArgs e)
        {
            if (dataGridViewT1.Rows.Count == 0 || CuIme.Text.Trim() == "") return;
            dataGridViewT1.Search("注音速查", CuIme.Text);
        }

        private void CuTel1_KeyUp(object sender, KeyEventArgs e)
        {
            if (dataGridViewT1.Rows.Count == 0 || CuTel1.Text.Trim() == "") return;
            dataGridViewT1.Search("電話一", CuTel1.Text);
        }

    }
}
