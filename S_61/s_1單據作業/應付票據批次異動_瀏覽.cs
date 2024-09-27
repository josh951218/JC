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

namespace S_61.s_1單據作業
{
    public partial class 應付票據批次異動_瀏覽 : S_61.Model.FormT
    {
        public string cono = "";
        public string coname1 = "";
        public string forwork = "";
        public DataTable table = new DataTable();
        public DataTable PrintTable = new DataTable();
        List<DataRow> list = new List<DataRow>();

        public 應付票據批次異動_瀏覽()
        {
            InitializeComponent();
            this.票面金額.DefaultCellStyle.Format = "n" + Common.金額小數;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Dispose();
            this.Close();
        }

        private void 應付票據批次異動_瀏覽_Load(object sender, EventArgs e)
        {
            if (Common.單據異動 == "2") CoNo.Enabled = false;
            dataGridViewT1.SetWidthByPixel();
            dataGridViewT1.AutoGenerateColumns = false;
            dataGridViewT1.ReadOnly = false;
            this.支票號碼.ReadOnly = true;
            this.廠商簡稱.ReadOnly = true;
            this.開票帳戶.ReadOnly = true;
            this.到期日.ReadOnly = true;
            this.預兌日.ReadOnly = true;
            this.票面金額.ReadOnly = true;
            if (Common.User_DateTime == 1) this.異動日期.MaxInputLength = 7;
            else this.異動日期.MaxInputLength = 8;
            CoNo.Text = cono;
            CoName1.Text = coname1;

            switch (forwork)
            {
                case "兌現":
                    ChStatus.Text = "3";
                    ChName.Text = "兌　　現";
                    break;
                case "退票":
                    ChStatus.Text = "4";
                    ChName.Text = "退　　票";
                    break;
                case "作廢":
                    ChStatus.Text = "5";
                    ChName.Text = "作　　廢";
                    break;
                case "其他":
                    ChStatus.Text = "6";
                    ChName.Text = "其    他";
                    break;
            }
            Count.Text = "0";
            table.AsEnumerable().ToList().ForEach(r =>
            {
                r["chdate2"] = Common.User_DateTime == 1 ? Date.AddLine(r["chdate2"].ToString()) : Date.AddLine(r["chdate2_1"].ToString());
                r["chdate3"] = Common.User_DateTime == 1 ? Date.AddLine(r["chdate3"].ToString()) : Date.AddLine(r["chdate3_1"].ToString());
                r["chdate"] = "";
            });
            dataGridViewT1.DataSource = table;
            dataGridViewT1.Rows[0].Selected = true;
            dataGridViewT1.CurrentCell = dataGridViewT1.SelectedRows[0].Cells["異動日期"];
            dataGridViewT1.Focus();
        }

        private void dataGridViewT1_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex > dataGridViewT1.Rows.Count - 1) return;
            if (dataGridViewT1.Columns[e.ColumnIndex].Name != "異動日期" ) return;
            if (dataGridViewT1.EditingControl == null) return;
            if (dataGridViewT1.EditingControl.Text.Trim() != "")
            {
                TextBox tb = new TextBox();
                tb.Text = dataGridViewT1.EditingControl.Text.Trim();
                if (!tb.IsDateTime())
                {
                    MessageBox.Show("日期格式錯誤", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    e.Cancel = true;
                    return;
                }
                if (!CHK.稽核會計年度(tb.Text.Trim()))
                {
                    e.Cancel = true;
                    ((TextBox)dataGridViewT1.EditingControl).SelectAll();
                    return;
                }
            }
            dataGridViewT1.CommitEdit(DataGridViewDataErrorContexts.Commit);
            table.Rows[e.RowIndex]["chdate"] = dataGridViewT1.EditingControl.Text.Trim();
            dataGridViewT1.InvalidateRow(e.RowIndex);
            var temp = table.AsEnumerable().ToList().Where(r => r["chdate"].ToString().Trim() != "");
            Count.Text = temp.Count().ToString();
        }

        private void ChMemo_DoubleClick(object sender, EventArgs e)
        {
            CHK.Memo_OpemFrm(ChMemo);
        }

        private void btnGet_Click(object sender, EventArgs e)
        {
            try
            {
                PrintTable.Clear();
                if (table.AsEnumerable().Count(r => r["chdate"].ToString().Trim() != "") == 0)
                {
                    MessageBox.Show("尚未選擇異動票據，請重新輸入", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                PrintTable = table.AsEnumerable().ToList().Where(r => r["chdate"].ToString().Trim() != "").CopyToDataTable();
                if (Common.User_DateTime == 2)
                {
                    for (int i = 0; i < PrintTable.Rows.Count; i++)
                    {
                        if (PrintTable.Rows[i]["chdate"].ToString().Trim().Length == 7)
                        {
                            MessageBox.Show("請檢查異動日期的日期格式是否為西元年");
                            return;
                        }
                    }
                }
                if (MessageBox.Show("確定是否異動單據", "訊息視窗", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1) == DialogResult.Cancel) return;
                this.Validate();
                dataGridViewT1.DataSource = null;
                using (SqlConnection cn = new SqlConnection(Common.sqlConnString))
                {
                    cn.Open();
                    SqlCommand cmd = cn.CreateCommand();
                    switch (forwork)
                    {
                        case "兌現":
                            for (int i = 0; i < PrintTable.Rows.Count; i++)
                            {
                                SqlCommand cmd1 = cn.CreateCommand();
                                cmd1.CommandText = "update acct set acmny1 -=" + PrintTable.Rows[i]["chmny"].ToDecimal() + " where acno=N'" + PrintTable.Rows[i]["acno"].ToString().Trim() + "'";
                                cmd1.ExecuteNonQuery();
                                cmd.CommandText += "update chko set chstatus=3,chstname=N'" + ChName.Text.Trim() + "',chmemo='" + ChMemo.Text.Trim() + "',chdate='" + Date.ToTWDate(PrintTable.Rows[i]["chdate"].ToString().Trim()) + "',"
                                    + " chdate_1='" + Date.ToUSDate(PrintTable.Rows[i]["chdate"].ToString().Trim()) + "'"
                                    + " where chno='" + PrintTable.Rows[i]["chno"].ToString().Trim() + "';";
                            }
                            break;
                        case "退票":
                            for (int i = 0; i < PrintTable.Rows.Count; i++)
                            {
                                cmd.CommandText += "update chko set chstatus=4,chstname=N'" + ChName.Text.Trim() + "',chmemo='" + ChMemo.Text.Trim() + "',chdate='" + Date.ToTWDate(PrintTable.Rows[i]["chdate"].ToString().Trim()) + "',"
                                    + " chdate_1='" + Date.ToUSDate(PrintTable.Rows[i]["chdate"].ToString().Trim()) + "'"
                                    + " where chno='" + PrintTable.Rows[i]["chno"].ToString().Trim() + "';";
                            }
                            break;
                        case "作廢":
                            for (int i = 0; i < PrintTable.Rows.Count; i++)
                            {
                                cmd.CommandText += "update chko set chstatus=5,chstname=N'" + ChName.Text.Trim() + "',chmemo='" + ChMemo.Text.Trim() + "',chdate='" + Date.ToTWDate(PrintTable.Rows[i]["chdate"].ToString().Trim()) + "',"
                                    + " chdate_1='" + Date.ToUSDate(PrintTable.Rows[i]["chdate"].ToString().Trim()) + "'"
                                    + " where chno='" + PrintTable.Rows[i]["chno"].ToString().Trim() + "';";
                            }
                            break;
                        case "其他":
                            for (int i = 0; i < PrintTable.Rows.Count; i++)
                            {
                                cmd.CommandText += "update chko set chstatus=6,chstname=N'" + ChName.Text.Trim() + "',chmemo='" + ChMemo.Text.Trim() + "',chdate='" + Date.ToTWDate(PrintTable.Rows[i]["chdate"].ToString().Trim()) + "',"
                                    + " chdate_1='" + Date.ToUSDate(PrintTable.Rows[i]["chdate"].ToString().Trim()) + "'"
                                    + " where chno='" + PrintTable.Rows[i]["chno"].ToString().Trim() + "';";
                            }
                            break;
                    }
                    cmd.ExecuteNonQuery();
                }
                if (table.AsEnumerable().ToList().Where(r => r["chdate"].ToString().Trim() == "").Count() > 0)
                {
                    table = table.AsEnumerable().ToList().Where(r => r["chdate"].ToString().Trim() == "").CopyToDataTable();
                }
                else
                {
                    table.Clear();
                }
                dataGridViewT1.DataSource = table;
                Count.Text = "0";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        void Focus異動日期()
        {
            int index = dataGridViewT1.SelectedRows[0].Index;
            if (index <= dataGridViewT1.Rows.Count - 1)
            {
                btnGet.Focus();
                dataGridViewT1.Rows[index].Selected = true;
                dataGridViewT1.CurrentCell = dataGridViewT1.SelectedRows[0].Cells["異動日期"];
                dataGridViewT1.Focus();
            }
            else
            {
                btnGet.Focus();
            }
        }
        private void 應付票據批次異動_瀏覽_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Tab:
                    if (ActiveControl is dataGridViewT)
                        Focus異動日期();
                    break;
                case Keys.F9:
                    btnGet.PerformClick();
                    break;
                case Keys.F11:
                    btnExit.PerformClick();
                    break;
            }
        }

        private void dataGridViewT1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex > dataGridViewT1.Rows.Count - 1) return;
            if (dataGridViewT1.Columns[e.ColumnIndex].Name == "異動日期")
            {
                try
                {
                    using (SqlConnection cn = new SqlConnection(Common.sqlConnString))
                    {
                        cn.Open();
                        SqlCommand cmd = cn.CreateCommand();
                        if (Common.User_DateTime == 1) cmd.CommandText = "select chdate3 from chko where chno='" + dataGridViewT1["支票號碼", e.RowIndex].Value.ToString().Trim() + "'";
                        else cmd.CommandText = "select chdate3_1 from chko where chno='" + dataGridViewT1["支票號碼", e.RowIndex].Value.ToString().Trim() + "'";
                        if (!cmd.ExecuteScalar().IsNullOrEmpty())
                        {
                            dataGridViewT1.EditingControl.Text = cmd.ExecuteScalar().ToString().Trim();
                            table.Rows[e.RowIndex]["chdate"] = cmd.ExecuteScalar().ToString().Trim();
                            dataGridViewT1.InvalidateRow(e.RowIndex);
                        }
                    }
                    dataGridViewT1.CommitEdit(DataGridViewDataErrorContexts.Commit);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
            var temp = table.AsEnumerable().ToList().Where(r => r["chdate"].ToString().Trim() != "");
            Count.Text = temp.Count().ToString();
        }


    }
}
