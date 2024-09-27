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
    public partial class 客戶郵遞標籤_明細 : FormT
    {
        public DataTable table = new DataTable();
        DataTable dt = new DataTable();
        List<DataRow> list = new List<DataRow>();
        DataView dv = new DataView();
        bool flag = true;


        public 客戶郵遞標籤_明細()
        {
            InitializeComponent();
        }

        private void FrmPrint_Cb_Load(object sender, EventArgs e)
        {
            if (!System.IO.File.Exists(Common.reportaddress + "Report\\客戶郵遞標籤_自訂一.rpt")) Msg3.Enabled = false;
            if (!System.IO.File.Exists(Common.reportaddress + "Report\\客戶郵遞標籤_自訂二.rpt")) Msg4.Enabled = false;
            ToolTip settoolname = new ToolTip();
            settoolname.SetToolTip(Msg, "客戶郵遞標籤_信封");
            settoolname.SetToolTip(lbMsg, "客戶郵遞標籤_信封");
            settoolname.SetToolTip(Msg1, "客戶郵遞標籤_1");
            settoolname.SetToolTip(lbMsg1, "客戶郵遞標籤_1");
            settoolname.SetToolTip(Msg2, "客戶郵遞標籤_1_5");
            settoolname.SetToolTip(lbMsg2, "客戶郵遞標籤_1_5");
            settoolname.SetToolTip(Msg3, "客戶郵遞標籤_自訂一");
            settoolname.SetToolTip(lbMsg3, "客戶郵遞標籤_自訂一");
            settoolname.SetToolTip(Msg4, "客戶郵遞標籤_自訂二");
            settoolname.SetToolTip(lbMsg4, "客戶郵遞標籤_自訂二");

            CuPer1.Checked = true;
            CuAddr1.Checked = true;
            Msg.Checked = true;
            PrintCn.Checked = true;
            Print_c.Text = "1";

            dataGridViewT1.SetWidthByPixel();

            if (table.Rows.Count > 0)
            {
                list = table.AsEnumerable().ToList();
                if (flag)
                {
                    table.Columns.Add();
                    flag = false;
                }
                dataGridViewT1.DataSource = table;
            }
        }

        private void CuPerCheck_CheckedChanged(object sender, EventArgs e)
        {
            S_61.Model.ForControls.ForRadio((RadioButton)sender);
            if (CuPerCheck.Checked)
            {
                CuPerSet.ReadOnly = false;
                CuPerSet.Focus();
                CuPerSet.SelectAll();
            }
            else
                CuPerSet.ReadOnly = true;
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

        private void CuNo_KeyUp(object sender, KeyEventArgs e)
        {
            if (list.Count < 1) return;//如果完全沒有客戶，就不用查了
            table = table.DefaultView.ToTable();
            list.Clear();
            list = table.AsEnumerable().ToList();
            var v = from dr in list
                    where dr["cuno"].ToString().StartsWith(CuNo.Text.Trim())
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
                    table.DefaultView.Sort = "CuNo ASC";
                    dataGridViewT1.DataSource = table;
                    break;
                case "02":
                    table.DefaultView.Sort = "CuName1 ASC";
                    dataGridViewT1.DataSource = table;
                    break;
                case "03":
                    table.DefaultView.Sort = "CuIme ASC";
                    dataGridViewT1.DataSource = table;
                    break;
                case "04":
                    table.DefaultView.Sort = "CuAddr1 ASC";
                    dataGridViewT1.DataSource = table;
                    break;
                case "05":
                    table.DefaultView.Sort = "CuX1No ASC";
                    dataGridViewT1.DataSource = table;
                    break;
            }
        }

        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < table.Rows.Count; i++)
            {
                dataGridViewT1.Rows[i].Cells["Column1"].Value = "Y";
            }
        }

        private void btnSelectD_Click(object sender, EventArgs e)
        {
            for (int i = dataGridViewT1.CurrentCell.OwningRow.Index; i < table.Rows.Count; i++)
            {
                dataGridViewT1.Rows[i].Cells["Column1"].Value = "Y";
            }
        }

        private void btnCancelAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < table.Rows.Count; i++)
            {
                dataGridViewT1.Rows[i].Cells["Column1"].Value = "N";
            }
        }

        private void btnCancelD_Click(object sender, EventArgs e)
        {
            for (int i = dataGridViewT1.CurrentCell.OwningRow.Index; i < table.Rows.Count; i++)
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
                    string str = "select * from cust where cuno=''";

                    for (int i = 0; i < dataGridViewT1.RowCount; i++)
                    {
                        if (dataGridViewT1.Rows[i].Cells["Column1"].Value.ToString() == "Y")
                        {
                            str += " or cuno='" + dataGridViewT1.Rows[i].Cells["客戶編號"].Value.ToString() + "'";
                        }
                    }

                    using (SqlDataAdapter da = new SqlDataAdapter (str,cn))
                    {
                        dt.Clear();
                        da.Fill(dt);
                    }
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
            if (dt.Rows.Count <= 0)
            {
                MessageBox.Show("查無資料", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            Common.FrmReport = new Report.Frmreport();
            ReportDocument oRpt = new ReportDocument();
            if (Msg.Checked) oRpt.Load(Common.reportaddress + "Report\\客戶郵遞標籤_信封.rpt");
            else if (Msg1.Checked) oRpt.Load(Common.reportaddress + "Report\\客戶郵遞標籤_1.rpt");
            else if (Msg2.Checked) oRpt.Load(Common.reportaddress + "Report\\客戶郵遞標籤_1_5.rpt");
            else if (Msg3.Checked) oRpt.Load(Common.reportaddress + "Report\\客戶郵遞標籤_自訂一.rpt");
            else if (Msg4.Checked) oRpt.Load(Common.reportaddress + "Report\\客戶郵遞標籤_自訂二.rpt");
            oRpt.SetDataSource(dt);
            oRpt.Database.Tables[0].ApplyLogOnInfo(Common.logOnInfo);
            if (PrintCn.Checked) oRpt.SetParameterValue("font", "直書");
            else if (PrintRw.Checked) oRpt.SetParameterValue("font", "橫書");
            if (CuPer1.Checked) oRpt.SetParameterValue("people", "聯絡");
            else if (CuPer.Checked) oRpt.SetParameterValue("people", "負責");
            else if (CuPerCheck.Checked) oRpt.SetParameterValue("people", "自訂");
            oRpt.SetParameterValue("people1", CuPerSet.Text);
            if (CuAddr1.Checked) oRpt.SetParameterValue("address", "公司");
            else if (CuAddr2.Checked) oRpt.SetParameterValue("address", "發票");
            else if (CuAddr3.Checked) oRpt.SetParameterValue("address", "送貨");
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
            string filename = "";
            datafill();
            if (dt.Rows.Count <= 0)
            {
                MessageBox.Show("查無資料", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            Common.FrmReport = new Report.Frmreport();
            ReportDocument oRpt = new ReportDocument();
            if (Msg.Checked)
            {
                oRpt.Load(Common.reportaddress + "Report\\客戶郵遞標籤_信封.rpt");
                filename = "客戶郵遞標籤_信封.rpt";
            }
            else if (Msg1.Checked)
            {
                oRpt.Load(Common.reportaddress + "Report\\客戶郵遞標籤_1.rpt");
                filename = "客戶郵遞標籤_1.rpt";
            }
            else if (Msg2.Checked)
            { 
                oRpt.Load(Common.reportaddress + "Report\\客戶郵遞標籤_1_5.rpt");
                filename = "客戶郵遞標籤_1_5.rpt";
            }
            else if (Msg3.Checked)
            {
                oRpt.Load(Common.reportaddress + "Report\\客戶郵遞標籤_自訂一.rpt");
                filename = "客戶郵遞標籤_自訂一.rpt";
            }
            else if (Msg4.Checked)
            {
                oRpt.Load(Common.reportaddress + "Report\\客戶郵遞標籤_自訂二.rpt");
                filename = "客戶郵遞標籤_自訂二.rpt";
            }
            oRpt.SetDataSource(dt);
            oRpt.Database.Tables[0].ApplyLogOnInfo(Common.logOnInfo);
            if (PrintCn.Checked) oRpt.SetParameterValue("font", "直書");
            else if (PrintRw.Checked) oRpt.SetParameterValue("font", "橫書");
            if (CuPer1.Checked) oRpt.SetParameterValue("people", "聯絡");
            else if (CuPer.Checked) oRpt.SetParameterValue("people", "負責");
            else if (CuPerCheck.Checked) oRpt.SetParameterValue("people", "自訂");
            oRpt.SetParameterValue("people1", CuPerSet.Text);
            if (CuAddr1.Checked) oRpt.SetParameterValue("address", "公司");
            else if (CuAddr2.Checked) oRpt.SetParameterValue("address", "發票");
            else if (CuAddr3.Checked) oRpt.SetParameterValue("address", "送貨");
            else if (CuAddrCheck.Checked) oRpt.SetParameterValue("address", "自訂");
            oRpt.SetParameterValue("address1", CuAddrSet.Text);
            Common.FrmReport.cview.ReportSource = oRpt;
            Common.FrmReport.rpt1 = oRpt;
            Common.FrmReport.Text = filename;
            Common.FrmReport.ShowDialog();
            Common.FrmReport.Dispose();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
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
            S_61.Model.ForControls.ForRadio((RadioButton)sender);
        }

        private void lblT1_Click(object sender, EventArgs e)
        {
            S_61.Model.ForControls.ForRadio((Label)sender);
        }

        private void FrmPrint_Cb_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F11) btnExit.PerformClick();
        }
    }
}
