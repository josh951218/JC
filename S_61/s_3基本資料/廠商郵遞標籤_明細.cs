using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;
using CrystalDecisions.CrystalReports.Engine;
using S_61.Basic;
using S_61.Model;
using S_61.MyControl;

namespace S_61.s_3基本資料
{
    public partial class 廠商郵遞標籤_明細 : FormT
    {
        public DataTable dt = new DataTable();
        List<DataRow> list = new List<DataRow>();
        DataView dv = new DataView();
        bool flag = true;

        public 廠商郵遞標籤_明細()
        {
            InitializeComponent();
        }

        private void FrmPrint_Fb_Load(object sender, EventArgs e)
        {
            if (!System.IO.File.Exists(Common.reportaddress + "Report\\廠商郵遞標籤_自訂一.rpt")) Msg3.Enabled = false;
            if (!System.IO.File.Exists(Common.reportaddress + "Report\\廠商郵遞標籤_自訂二.rpt")) Msg4.Enabled = false;
            ToolTip settoolname = new ToolTip();
            settoolname.SetToolTip(Msg, "廠商郵遞標籤_信封");
            settoolname.SetToolTip(lbMsg, "廠商郵遞標籤_信封");
            settoolname.SetToolTip(Msg1, "廠商郵遞標籤_1");
            settoolname.SetToolTip(lbMsg1, "廠商郵遞標籤_1");
            settoolname.SetToolTip(Msg2, "廠商郵遞標籤_1_5");
            settoolname.SetToolTip(lbMsg2, "廠商郵遞標籤_1_5");
            settoolname.SetToolTip(Msg3, "廠商郵遞標籤_自訂一");
            settoolname.SetToolTip(lbMsg3, "廠商郵遞標籤_自訂一");
            settoolname.SetToolTip(Msg4, "廠商郵遞標籤_自訂二");
            settoolname.SetToolTip(lbMsg4, "廠商郵遞標籤_自訂二");
            FaPer1.Checked = true;
            FaAddr1.Checked = true;
            Msg.Checked = true;
            PrintCn.Checked = true;
            Print_c.Text = "1";

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

            if (dt.Rows.Count > 0)
            {
                list = dt.AsEnumerable().ToList();
                if (flag)
                {
                    dt.Columns.Add();
                    flag = false;
                }
                dataGridViewT1.DataSource = dt;
            }
        }

        private void FaPerCheck_CheckedChanged(object sender, EventArgs e)
        {
            S_61.Model.ForControls.ForRadio((RadioButton)sender);
            if (FaPerCheck.Checked)
            {
                FaPerSet.ReadOnly = false;
                FaPerSet.Focus();
                FaPerSet.SelectAll();
            }
            else
                FaPerSet.ReadOnly = true;
        }

        private void CuAddrCheck_CheckedChanged(object sender, EventArgs e)
        {
            S_61.Model.ForControls.ForRadio((RadioButton)sender);
            if (CuAddrCheck.Checked)
            {
                CuAddrSet.ReadOnly = false;
                CuAddrSet.Focus();
                CuAddrSet.SelectAll();
            }
            else
                CuAddrSet.ReadOnly = true;
        }

        private void FaNo_KeyUp(object sender, KeyEventArgs e)
        {
            if (list.Count < 1) return;//如果完全沒有廠商，就不用查了
            dt = dt.DefaultView.ToTable();
            list.Clear();
            list = dt.AsEnumerable().ToList();
            var v = from dr in list
                    where dr["FaNo"].ToString().StartsWith(FaNo.Text.Trim())
                    select dr;
            if (v.Count() > 0)
            {
                int i = list.IndexOf(v.First());
                if (i != -1)
                {
                    dataGridViewT1.FirstDisplayedScrollingRowIndex = i;
                    dataGridViewT1.Rows[i].Cells[0].Selected = true;
                }
            }
            else
            {
                dataGridViewT1.Rows[list.Count - 1].Cells[0].Selected = true;
            }
        }

        private void cbSort_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cbSort.SelectedItem.ToString().Substring(0, 2))
            {
                case "01":
                    dt.DefaultView.Sort = "FaNo ASC";
                    dataGridViewT1.DataSource = dt;
                    break;
                case "02":
                    dt.DefaultView.Sort = "FaName1 ASC";
                    dataGridViewT1.DataSource = dt;
                    break;
                case "03":
                    dt.DefaultView.Sort = "FaIme ASC";
                    dataGridViewT1.DataSource = dt;
                    break;
                case "04":
                    dt.DefaultView.Sort = "FaAddr1 ASC";
                    dataGridViewT1.DataSource = dt;
                    break;
                case "05":
                    dt.DefaultView.Sort = "FaX12no ASC";
                    dataGridViewT1.DataSource = dt;
                    break;
            }
        }

        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dataGridViewT1.Rows[i].Cells["Column1"].Value = "Y";
            }
        }

        private void btnSelectD_Click(object sender, EventArgs e)
        {
            for (int i = dataGridViewT1.CurrentCell.OwningRow.Index; i < dt.Rows.Count; i++)
            {
                dataGridViewT1.Rows[i].Cells["Column1"].Value = "Y";
            }
        }

        private void btnCancelAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dataGridViewT1.Rows[i].Cells["Column1"].Value = "N";
            }
        }

        private void btnCancelD_Click(object sender, EventArgs e)
        {
            for (int i = dataGridViewT1.CurrentCell.OwningRow.Index; i < dt.Rows.Count; i++)
            {
                dataGridViewT1.Rows[i].Cells["Column1"].Value = "N";
            }
        }
        void datafill()
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(Common.浮動連線字串))
                {
                    cn.Open();
                    string str = "select * from fact where fano=''";

                    for (int i = 0; i < dataGridViewT1.RowCount; i++)
                    {
                        if (dataGridViewT1.Rows[i].Cells["Column1"].Value.ToString() == "Y")
                        {
                            str += " or fano='" + dataGridViewT1.Rows[i].Cells["廠商編號"].Value.ToString() + "'";
                        }
                    }

                    SqlDataAdapter da = new SqlDataAdapter(str, cn);
                    pVar.reportds.Clear();
                    da.Fill(pVar.reportds);
                    da.Dispose();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("LoadDBError:\n" + ex.ToString());
            }

        }
        private void btnPrint_Click(object sender, EventArgs e)
        {
            datafill();
            if (pVar.reportds.Tables[0].Rows.Count <= 0)
            {
                MessageBox.Show("查無資料", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                return;
            }
            Common.FrmReport = new Report.Frmreport();
            ReportDocument oRpt = new ReportDocument();
            if (Msg.Checked) oRpt.Load(Common.reportaddress + "Report\\廠商郵遞標籤_信封.rpt");
            else if (Msg1.Checked) oRpt.Load(Common.reportaddress + "Report\\廠商郵遞標籤_1.rpt");
            else if (Msg2.Checked) oRpt.Load(Common.reportaddress + "Report\\廠商郵遞標籤_1_5.rpt");
            else if (Msg3.Checked) oRpt.Load(Common.reportaddress + "Report\\廠商郵遞標籤_自訂一.rpt");
            else if (Msg4.Checked) oRpt.Load(Common.reportaddress + "Report\\廠商郵遞標籤_自訂二.rpt");
            oRpt.SetDataSource(pVar.reportds.Tables[0]);
            oRpt.Database.Tables[0].ApplyLogOnInfo(Common.logOnInfo);

            if (PrintCn.Checked) oRpt.SetParameterValue("font", "直書");
            else if (PrintRw.Checked) oRpt.SetParameterValue("font", "橫書");
            if (FaPer1.Checked) oRpt.SetParameterValue("people", "聯絡");
            else if (FaPer.Checked) oRpt.SetParameterValue("people", "負責");
            else if (FaPerCheck.Checked) oRpt.SetParameterValue("people", "自訂");
            oRpt.SetParameterValue("people1", FaPerSet.Text);
            if (FaAddr1.Checked) oRpt.SetParameterValue("address", "公司");
            else if (FaAddr2.Checked) oRpt.SetParameterValue("address", "發票");
            else if (FaAddr3.Checked) oRpt.SetParameterValue("address", "送貨");
            else if (CuAddrCheck.Checked) oRpt.SetParameterValue("address", "自訂");
            oRpt.SetParameterValue("address1", CuAddrSet.Text);
            Common.FrmReport.rpt1 = oRpt;
            try
            {
                printDialog1.PrinterSettings.Copies = short.Parse(Print_c.Text);
            }
            catch { }
            if (printDialog1.ShowDialog() == DialogResult.OK)
            {
                Common.FrmReport.rpt1.PrintOptions.PrinterName = printDialog1.PrinterSettings.PrinterName;
                Common.FrmReport.rpt1.PrintToPrinter(printDialog1.PrinterSettings.Copies, true, 0, 0);
            }
            printDialog1.Dispose();
            Common.FrmReport.Dispose();
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            datafill();
            if (pVar.reportds.Tables[0].Rows.Count <= 0)
            {
                MessageBox.Show("查無資料", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                return;
            }
            Common.FrmReport = new Report.Frmreport();
            ReportDocument oRpt = new ReportDocument();
            if (Msg.Checked) oRpt.Load(Common.reportaddress + "Report\\廠商郵遞標籤_信封.rpt");
            else if (Msg1.Checked) oRpt.Load(Common.reportaddress + "Report\\廠商郵遞標籤_1.rpt");
            else if (Msg2.Checked) oRpt.Load(Common.reportaddress + "Report\\廠商郵遞標籤_1_5.rpt");
            else if (Msg3.Checked) oRpt.Load(Common.reportaddress + "Report\\廠商郵遞標籤_自訂一.rpt");
            else if (Msg4.Checked) oRpt.Load(Common.reportaddress + "Report\\廠商郵遞標籤_自訂二.rpt");
            oRpt.SetDataSource(pVar.reportds.Tables[0]);
            oRpt.Database.Tables[0].ApplyLogOnInfo(Common.logOnInfo);

            if (PrintCn.Checked) oRpt.SetParameterValue("font", "直書");
            else if (PrintRw.Checked) oRpt.SetParameterValue("font", "橫書");
            if (FaPer1.Checked) oRpt.SetParameterValue("people", "聯絡");
            else if (FaPer.Checked) oRpt.SetParameterValue("people", "負責");
            else if (FaPerCheck.Checked) oRpt.SetParameterValue("people", "自訂");
            oRpt.SetParameterValue("people1", FaPerSet.Text);
            if (FaAddr1.Checked) oRpt.SetParameterValue("address", "公司");
            else if (FaAddr2.Checked) oRpt.SetParameterValue("address", "發票");
            else if (FaAddr3.Checked) oRpt.SetParameterValue("address", "送貨");
            else if (CuAddrCheck.Checked) oRpt.SetParameterValue("address", "自訂");
            oRpt.SetParameterValue("address1", CuAddrSet.Text);
            Common.FrmReport.cview.ReportSource = oRpt;
            Common.FrmReport.ShowDialog();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
        }

        private void dataGridViewT1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridViewT1.Columns[e.ColumnIndex].Name != "Column1") return;
            if (dataGridViewT1["Column1", e.RowIndex].Value.ToString() == "Y")
            {
                dataGridViewT1["Column1", e.RowIndex].Value = "N";
            }
            else
            {
                dataGridViewT1["Column1", e.RowIndex].Value = "Y";
            }
        }

        private void Msg_CheckedChanged(object sender, EventArgs e)
        {
            if (sender is RadioButton)
                S_61.Model.ForControls.ForRadio((RadioButton)sender);
            else if (sender is Label)
                S_61.Model.ForControls.ForRadio((Label)sender);

        }

        private void FrmPrint_Fb_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F11)
                btnExit.PerformClick();
        }


    }
}
