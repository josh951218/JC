using System;
using System.Data;
using System.Windows.Forms;
using S_61.Basic;

namespace S_61.Model
{
    public partial class FrmBuyPriceQuery : Form
    {
        public string ItNo = "";

        public FrmBuyPriceQuery()
        {
            InitializeComponent();
            pVar.FrmBuyPriceQuery = this;
            cn.ConnectionString = Common.sqlConnString;
            this.數量.DefaultCellStyle.Format = "f" + Common.Q;
            this.單價.DefaultCellStyle.Format = "f" + Common.MF;
        }

        private void FrmBuyPriceQuery_Load(object sender, EventArgs e)
        {
            Basic.SetParameter.FormSize(this);
            dataGridViewT1.AutoGenerateColumns = false;

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

            進貨日期.DataPropertyName = (Common.User_DateTime == 1) ? "bsdate" : "bsdate1";
            da.SelectCommand.Parameters["@no"].Value = ItNo;
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridViewT1.DataSource = dt;
        }

        private void FrmBuyPriceQuery_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Escape)
                this.DialogResult = DialogResult.Cancel;
        }
    }
}
