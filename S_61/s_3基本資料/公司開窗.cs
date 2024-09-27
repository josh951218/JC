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
    public partial class 公司開窗 : FormT
    {
        public bool CanAppend { get; set; }
        public DataRow Result { get; set; }
        DataTable tb = new DataTable();
        List<DataRow> list = new List<DataRow>();
        public 公司開窗()
        {
            InitializeComponent();
        }

        private void 公司開窗_Load(object sender, EventArgs e)
        {
            dataGridViewT1.SetWidthByPixel();
            if (!CanAppend) Append.Visible = false;
            load();
            CoNo.Focus();
        }

        void load()
        {
            using (SqlConnection cn = new SqlConnection(Common.sqlConnString))
            {
                cn.Open();
                SqlDataAdapter dd = new SqlDataAdapter("select * from comp where cono not in ('df') order by cono", cn);
                tb.Clear();
                list.Clear();
                dd.Fill(tb);
                if (tb.Rows.Count > 0) list = tb.AsEnumerable().ToList();
                Count.Text = "總共有 " + tb.Rows.Count + " 筆資料";
                dataGridViewT1.DataSource = tb;
            }
            if (list.Count > 0)
            {
                int i = list.FindIndex(r => string.CompareOrdinal(r["cono"].ToString().Trim(), SeekNo) == 0);
                if (i == -1)
                    i = list.FindIndex(r => string.CompareOrdinal(r["cono"].ToString().Trim(), SeekNo) > 0);
                if (i == -1)
                    i = list.Count - 1;
                dataGridViewT1.FirstDisplayedScrollingRowIndex = i;
                dataGridViewT1.Rows[i].Selected = true;
            }
        }

        private void Append_Click(object sender, EventArgs e)
        {
            using (公司建檔作業 frm = new 公司建檔作業())
            {
                frm.SetParaeter();
                frm.ShowDialog();
            }
            load();
        }

        private void Get_Click(object sender, EventArgs e)
        {
            Result = tb.Rows[dataGridViewT1.SelectedRows[0].Index];
            this.DialogResult = DialogResult.OK;
        }

        private void Exit_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void CoNo_KeyUp(object sender, KeyEventArgs e)
        {
            if (dataGridViewT1.Rows.Count == 0 || CoNo.Text.Trim() == "") return;
            int i = list.FindIndex(r => string.CompareOrdinal(r["cono"].ToString().Trim(), CoNo.Text.Trim()) == 0);
            if (i == -1)
                i = list.FindIndex(r => string.CompareOrdinal(r["cono"].ToString().Trim(), CoNo.Text.Trim()) > 0);
            if (i == -1)
                i = list.Count - 1;
            dataGridViewT1.FirstDisplayedScrollingRowIndex = i;
            dataGridViewT1.Rows[i].Selected = true;
        }

        private void 公司開窗_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.F2:
                    Append.PerformClick();
                    break;
                case Keys.F9:
                    Get.PerformClick();
                    break;
                case Keys.F11:
                    Exit.PerformClick();
                    break;
            }
        }

        private void dataGridViewT1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridViewT1.SelectedRows.Count == 0) return;
            Result = tb.Rows[dataGridViewT1.SelectedRows[0].Index];
            this.DialogResult = DialogResult.OK;
        }
    }
}
