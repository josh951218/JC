using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using S_61.Basic;
using S_61.Model;
using S_61.MyControl;

namespace S_61.s_3基本資料
{
    public partial class 備註開窗 : FormT
    {
        public string Memo { get; set; }
        DataTable dt = new DataTable();

        public 備註開窗()
        {
            InitializeComponent();
        }

        private void FrmSale_Memo_Load(object sender, EventArgs e)
        {
            //gridView欄位寬度設定
            int maxLen;
            for (int i = 0; i < dataGridViewT1.Columns.Count; i++)
            {
                dataGridViewT1.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                maxLen = ((DataGridViewTextBoxColumn)dataGridViewT1.Columns[i]).MaxInputLength;
                if (5 < maxLen && maxLen < 80)
                {
                    dataGridViewT1.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                    dataGridViewT1.Columns[i].Width = maxLen * 13;
                }
            }

            dataGridViewT1.DataSource = dt;
            LoadDB();
        }

        void LoadDB()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(Common.sqlConnString))
                {
                    string sql = "select * from memo01";
                    using (SqlDataAdapter da = new SqlDataAdapter(sql, conn))
                    {
                        dt.Clear();
                        da.Fill(dt);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void FrmSale_Memo_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.DialogResult = DialogResult.Cancel;
            }
        }

        private void dataGridViewT1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridViewT1.Rows.Count > 0)
            {
                Memo = "";
                if (dataGridViewT1.SelectedRows.Count > 0)
                {
                    if (!dataGridViewT1["備註說明", dataGridViewT1.SelectedRows[0].Index].Value.IsNullOrEmpty())
                    {
                        Memo = dataGridViewT1["備註說明", dataGridViewT1.SelectedRows[0].Index].Value.ToString();
                    }
                }
                this.DialogResult = DialogResult.OK;
            }
        }
    }
}
