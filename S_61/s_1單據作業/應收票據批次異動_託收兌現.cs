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
    public partial class 應收票據批次異動_託收兌現 : FormT
    {
        public string cono = "";
        public string coname1 = "";
        public string forwork = "";
        public DataTable table = new DataTable();
        public DataTable PrintTable = new DataTable();
        List<DataRow> list = new List<DataRow>();
        SqlTransaction tran;

        public 應收票據批次異動_託收兌現()
        {
            InitializeComponent();
            this.票面金額.DefaultCellStyle.Format = "N" + Common.金額小數;
        }

        private void 應收票據批次異動_託收兌現_Load(object sender, EventArgs e)
        {
            dataGridViewT1.SetWidthByPixel();
            dataGridViewT1.AutoGenerateColumns = false;
            dataGridViewT1.ReadOnly = false;
            this.支票號碼.ReadOnly = true;
            this.客戶簡稱.ReadOnly = true;
            this.託收帳戶.ReadOnly = true;
            this.到期日.ReadOnly = true;
            this.預兌日.ReadOnly = true;
            this.票面金額.ReadOnly = true;
            if (Common.User_DateTime == 1) this.異動日期.MaxInputLength = 7;
            else this.異動日期.MaxInputLength = 8;
            CoNo.Text = cono;
            CoName1.Text = coname1;
            switch (forwork)
            {
                case "託收兌現":
                    ChStatus.Text = "3";
                    ChName.Text = "託收兌現";
                    break;
                case "現金兌現":
                    ChStatus.Text = "4";
                    ChName.Text = "現金兌現";
                    break;
                case "退票":
                    ChStatus.Text = "7";
                    ChName.Text = "退    票";
                    break;
                case "其他":
                    ChStatus.Text = "8";
                    ChName.Text = "其    他";
                    break;
            }
            count.Text = "0";
            table.AsEnumerable().ToList().ForEach(r =>
            {
                r["到期日"] = Common.User_DateTime == 1 ? Date.AddLine(r["chdate2"].ToString()) : Date.AddLine(r["chdate2_1"].ToString());
                r["預兌日"] = Common.User_DateTime == 1 ? Date.AddLine(r["chdate3"].ToString()) : Date.AddLine(r["chdate3_1"].ToString());
            });
            dataGridViewT1.DataSource = table;
            dataGridViewT1.Rows[0].Selected = true;
            dataGridViewT1.CurrentCell = dataGridViewT1.SelectedRows[0].Cells["異動日期"];
            dataGridViewT1.Focus();

        }

        private void dataGridViewT1_RowStateChanged(object sender, DataGridViewRowStateChangedEventArgs e)
        {
            if (dataGridViewT1.SelectedRows.Count == 0) return;
            BaName.Text = dataGridViewT1.SelectedRows[0].Cells["付款銀行"].Value.ToString();
        }

        private void dataGridViewT1_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (dataGridViewT1.Columns[e.ColumnIndex].Name != "異動日期" || btnExit.Focused) return;
            if (dataGridViewT1.EditingControl == null) return;
            if (dataGridViewT1.EditingControl.Text.Trim() == "")
            {
                dataGridViewT1.CommitEdit(DataGridViewDataErrorContexts.Commit);
                count.Text = table.AsEnumerable().ToList().Where(r => r["異動日期"].ToString().Trim() != "").Count().ToString();
                return;
            }
            if (!((TextBox)dataGridViewT1.EditingControl).IsDateTime())
            {
                MessageBox.Show("日期格式錯誤", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                ((TextBox)dataGridViewT1.EditingControl).SelectAll();
                e.Cancel = true;
                return;
            }
            if (!CHK.稽核會計年度(dataGridViewT1.EditingControl.Text.Trim()))
            {
                e.Cancel = true;
                ((TextBox)dataGridViewT1.EditingControl).SelectAll();
                return;
            }
            dataGridViewT1.CommitEdit(DataGridViewDataErrorContexts.Commit);
            count.Text = table.AsEnumerable().ToList().Where(r => r["異動日期"].ToString().Trim() != "").Count().ToString();
            Focus異動日期();
        }

        private void dataGridViewT1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex > dataGridViewT1.Rows.Count - 1) return;
            if (dataGridViewT1.Columns[e.ColumnIndex].Name != "異動日期") return;
            if (dataGridViewT1.EditingControl == null) return;
            dataGridViewT1.EditingControl.Text = Common.User_DateTime == 1 ? table.Rows[e.RowIndex]["chdate3"].ToString() : table.Rows[e.RowIndex]["chdate3_1"].ToString();
            dataGridViewT1.CommitEdit(DataGridViewDataErrorContexts.Commit);
            ((TextBox)dataGridViewT1.EditingControl).SelectAll();
            Focus異動日期();
        }

        private void btnGet_Click(object sender, EventArgs e)
        {
            try
            {
                PrintTable.Clear();
                var temp = table.AsEnumerable().ToList().Where(r => r["異動日期"].ToString().Trim() != "");
                if (temp.Count() == 0)
                {
                    MessageBox.Show("尚未選擇異動票據，請重新輸入", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (MessageBox.Show("確定是否異動票據", "訊息視窗", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1) == DialogResult.Cancel) return;
                PrintTable = temp.CopyToDataTable();
                using (SqlConnection cn = new SqlConnection(Common.sqlConnString))
                {
                    cn.Open();
                    tran = cn.BeginTransaction();
                    SqlCommand cmd = cn.CreateCommand();
                    cmd.Transaction = tran;

                    switch (forwork)
                    {
                        case "託收兌現":
                            for (int i = 0; i < PrintTable.Rows.Count; i++)
                            {
                                cmd.CommandText += "update chki set chstatus=3,chstname=N'" + ChName.Text.Trim() +"',chmemo=N'" + ChMemo.Text.Trim() + "',chdate=N'" + Date.ToTWDate(PrintTable.Rows[i]["異動日期"].ToString().Trim()) + "',"
                                       + " chdate_1=N'" + Date.ToUSDate(PrintTable.Rows[i]["異動日期"].ToString().Trim()) + "'"
                                       + " where chno=N'" + PrintTable.Rows[i]["chno"].ToString().Trim() + "';";
                                cmd.CommandText += "update acct set acmny1+=" + PrintTable.Rows[i]["chmny"].ToDecimal() + " where acno=N'" + PrintTable.Rows[i]["acno"].ToString().Trim() + "';";
                            }
                            break;
                        case "現金兌現":
                            for (int i = 0; i < PrintTable.Rows.Count; i++)
                            {
                                cmd.CommandText += "update chki set chstatus=4,chstname=N'" + ChName.Text.Trim() + "',chmemo=N'" + ChMemo.Text.Trim() + "',chdate=N'" + Date.ToTWDate(PrintTable.Rows[i]["異動日期"].ToString().Trim()) + "',"
                                    + " chdate_1=N'" + Date.ToUSDate(PrintTable.Rows[i]["異動日期"].ToString().Trim()) + "'"
                                    + " where chno=N'" + PrintTable.Rows[i]["chno"].ToString().Trim() + "';";
                            }
                            break;
                        case "退票":
                            for (int i = 0; i < PrintTable.Rows.Count; i++)
                            {
                                cmd.CommandText += "update chki set chstatus=7,chstname=N'" + ChName.Text.Trim() + "',chmemo=N'" + ChMemo.Text.Trim() + "',chdate=N'" + Date.ToTWDate(PrintTable.Rows[i]["異動日期"].ToString().Trim()) + "',"
                                    + " chdate_1=N'" + Date.ToUSDate(PrintTable.Rows[i]["異動日期"].ToString().Trim()) + "'"
                                    + " where chno=N'" + PrintTable.Rows[i]["chno"].ToString().Trim() + "';";
                            }
                            break;
                        case "其他":
                            for (int i = 0; i < PrintTable.Rows.Count; i++)
                            {
                                cmd.CommandText += "update chki set chstatus=8,chstname=N'" + ChName.Text.Trim() + "',chmemo=N'" + ChMemo.Text.Trim() + "',chdate=N'" + Date.ToTWDate(PrintTable.Rows[i]["異動日期"].ToString().Trim()) + "',"
                                    + " chdate_1=N'" + Date.ToUSDate(PrintTable.Rows[i]["異動日期"].ToString().Trim()) + "'"
                                    + " where chno=N'" + PrintTable.Rows[i]["chno"].ToString().Trim() + "';";
                            }
                            break;
                    }
                    cmd.ExecuteNonQuery();
                    tran.Commit();
                }
                if (table.AsEnumerable().ToList().Where(r => r["異動日期"].ToString().Trim() == "").Count() > 0)
                {
                    table = table.AsEnumerable().ToList().Where(r => r["異動日期"].ToString().Trim() == "").CopyToDataTable();
                }
                else
                {
                    table.Clear();
                }
                dataGridViewT1.DataSource = table;
            }
            catch (Exception ex)
            {
                
                MessageBox.Show(ex.ToString());
                tran.Rollback();
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Dispose();
            this.Close();
        }

        void Focus異動日期()
        {
            int index = dataGridViewT1.SelectedRows[0].Index;
            if (index  <= dataGridViewT1.Rows.Count - 1)
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

        private void 應收票據批次異動_託收兌現_KeyUp(object sender, KeyEventArgs e)
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

        private void ChMemo_DoubleClick(object sender, EventArgs e)
        {
            CHK.Memo_OpemFrm(ChMemo);
        }



    }
}
