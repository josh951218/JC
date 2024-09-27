using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using S_61.Basic;
using S_61.Model;
using S_61.MyControl;

namespace S_61.s_2統計圖表
{
    public partial class 應付票據明細_瀏覽 : FormT
    {
        public DataTable table = new DataTable();
        List<DataRow> list;
        List<Button> query;
        string path = "";
        string ReportFileName = "";
        string NO = "";
        public 應付票據明細_瀏覽()
        {
            InitializeComponent();
            query = new List<Button> { query2, query3, query4, query5, query6, query7, query8, query10 };
        }

        private void 應付票據明細_瀏覽_Load(object sender, EventArgs e)
        {
            dataGridViewT1.SetWidthByPixel();
            dataGridViewT1.AutoGenerateColumns = false;
            groupBoxT1.BackColor = 單行註腳.BackColor = panelT1.BackColor = Color.FromArgb(215, 227, 239);
            ReportFileName = "應付票據明細";
            if (!System.IO.File.Exists(Common.reportaddress + "Report\\" + ReportFileName + "_自訂一.rpt")) rd2.Enabled = false;
            if (!System.IO.File.Exists(Common.reportaddress + "Report\\" + ReportFileName + "_自訂二.rpt")) rd3.Enabled = false;
            if (!System.IO.File.Exists(Common.reportaddress + "Report\\" + ReportFileName + "_自訂三.rpt")) rd4.Enabled = false;
            if (Common.User_DateTime == 1) date1.MaxLength = date2.MaxLength = 7;
            else date1.MaxLength = date2.MaxLength = 8;
            date1.Init();
            date2.Init();
            ChMny.NumThousands =  true;
            ChMny.NumLast =  Common.金額小數;
            ChMny.NumFirst =  (20 - 1 - Common.金額小數);
            this.票面金額.DefaultCellStyle.Format = "N" + Common.金額小數;
            if (Common.User_DateTime == 1)
            {
                this.異動日期.DataPropertyName = "chdate";
                this.預兌日.DataPropertyName = "chdate3";
                this.到期日.DataPropertyName = "chdate2";
                this.開票日.DataPropertyName = "chdate1";
            }
            else
            {
                this.異動日期.DataPropertyName = "chdate_1";
                this.預兌日.DataPropertyName = "chdate3_1";
                this.到期日.DataPropertyName = "chdate2_1";
                this.開票日.DataPropertyName = "chdate1_1";
            }
            decimal d = 0, chmny = 0;
            table.AsEnumerable().ToList().ForEach(r =>
            {
                r["序號"] = d++;
                r["chdate"] = Date.AddLine(r["chdate"].ToString());
                r["chdate_1"] = Date.AddLine(r["chdate_1"].ToString());
                r["chdate1"] = Date.AddLine(r["chdate1"].ToString());
                r["chdate1_1"] = Date.AddLine(r["chdate1_1"].ToString());
                r["chdate2"] = Date.AddLine(r["chdate2"].ToString());
                r["chdate2_1"] = Date.AddLine(r["chdate2_1"].ToString());
                r["chdate3"] = Date.AddLine(r["chdate3"].ToString());
                r["chdate3_1"] = Date.AddLine(r["chdate3_1"].ToString());
                chmny += r["ChMny"].ToDecimal();
            });
            list = table.AsEnumerable().ToList();
            TotCount.Text = table.Rows.Count.ToString();
            ChMny.Text = chmny.ToString("N" + Common.金額小數);
            dataGridViewT1.DataSource = table;
            query2.ForeColor = Color.Red;
        }

        void SetButtonColor()
        {
            query.ForEach(r => r.ForeColor = SystemColors.ControlText);
        }

        void SetSelectRow(string NO)
        {
            for (int i = 0; i < dataGridViewT1.Rows.Count; i++)
            {
                if (dataGridViewT1["序號", i].Value.ToString() == NO)
                {
                    dataGridViewT1.FirstDisplayedScrollingRowIndex = i;
                    dataGridViewT1.Rows[i].Selected = true;
                    break;
                }
            }
        }

        RPT paramsInit()
        {
            if (rd1.Checked) path = Common.reportaddress + "Report\\" + ReportFileName + "_內定報表.rpt";
            if (rd2.Checked) path = Common.reportaddress + "Report\\" + ReportFileName + "_自訂一.rpt";
            if (rd3.Checked) path = Common.reportaddress + "Report\\" + ReportFileName + "_自訂二.rpt";
            if (rd4.Checked) path = Common.reportaddress + "Report\\" + ReportFileName + "_自訂三.rpt";
            RPT rp = new RPT(table, path);
            rp.office = this.Tag.ToString();
            //公司抬頭
            rp.lobj.Add(new string[] { "txtstart", CHK.GetCoName2(Common.使用者預設公司) });
            //制表日期
            rp.lobj.Add(new string[] { "制表日期", Date.GetDateTime(Common.User_DateTime, true) });
            //單行註腳
            if (this.FindControl("單行註腳") != null)
            {
                string txtend;
                if (((radio)this.FindControl("單行註腳" + "1")).Checked) txtend = Common.dtEnd.Rows[0]["tamemo"].ToString();
                else if (((radio)this.FindControl("單行註腳" + "2")).Checked) txtend = Common.dtEnd.Rows[1]["tamemo"].ToString();
                else if (((radio)this.FindControl("單行註腳" + "3")).Checked) txtend = Common.dtEnd.Rows[2]["tamemo"].ToString();
                else if (((radio)this.FindControl("單行註腳" + "4")).Checked) txtend = Common.dtEnd.Rows[3]["tamemo"].ToString();
                else if (((radio)this.FindControl("單行註腳" + "5")).Checked) txtend = Common.dtEnd.Rows[4]["tamemo"].ToString();
                else txtend = "";
                rp.lobj.Add(new string[] { "txtend", txtend });
            }
            return rp;
        }

        private void query2_Click(object sender, EventArgs e)
        {
            NO = dataGridViewT1.SelectedRows[0].Cells["序號"].Value.ToString();
            table.DefaultView.Sort = "chno";
            SetButtonColor();
            query2.ForeColor = Color.Red;
            SetSelectRow(NO);
        }

        private void query3_Click(object sender, EventArgs e)
        {
            NO = dataGridViewT1.SelectedRows[0].Cells["序號"].Value.ToString();
            table.DefaultView.Sort = "chstatus";
            SetButtonColor();
            query3.ForeColor = Color.Red;
            SetSelectRow(NO);
        }

        private void query4_Click(object sender, EventArgs e)
        {
            NO = dataGridViewT1.SelectedRows[0].Cells["序號"].Value.ToString();
            table.DefaultView.Sort = "chdate";
            SetButtonColor();
            query4.ForeColor = Color.Red;
            SetSelectRow(NO);
        }

        private void query5_Click(object sender, EventArgs e)
        {
            NO = dataGridViewT1.SelectedRows[0].Cells["序號"].Value.ToString();
            table.DefaultView.Sort = "fano";
            SetButtonColor();
            query5.ForeColor = Color.Red;
            SetSelectRow(NO);
        }

        private void query6_Click(object sender, EventArgs e)
        {
            NO = dataGridViewT1.SelectedRows[0].Cells["序號"].Value.ToString();
            table.DefaultView.Sort = "chdate3";
            SetButtonColor();
            query6.ForeColor = Color.Red;
            SetSelectRow(NO);
        }

        private void query7_Click(object sender, EventArgs e)
        {
            NO = dataGridViewT1.SelectedRows[0].Cells["序號"].Value.ToString();
            table.DefaultView.Sort = "chdate2";
            SetButtonColor();
            query7.ForeColor = Color.Red;
            SetSelectRow(NO);
        }

        private void query8_Click(object sender, EventArgs e)
        {
            NO = dataGridViewT1.SelectedRows[0].Cells["序號"].Value.ToString();
            table.DefaultView.Sort = "chdate1";
            SetButtonColor();
            query8.ForeColor = Color.Red;
            SetSelectRow(NO);
        }

        private void query10_Click(object sender, EventArgs e)
        {
            NO = dataGridViewT1.SelectedRows[0].Cells["序號"].Value.ToString();
            table.DefaultView.Sort = "chmemo";
            SetButtonColor();
            query10.ForeColor = Color.Red;
            SetSelectRow(NO);
        }

        private void 應付票據明細_瀏覽_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.F2:
                    if (e.Modifiers == Keys.Shift)
                    {
                        query2.Focus();
                        query2.PerformClick();
                        break;
                    }
                    break;
                case Keys.F3:
                    if (e.Modifiers == Keys.Shift)
                    {
                        query3.Focus();
                        query3.PerformClick();
                        break;
                    }
                    break;
                case Keys.F4:
                    if (e.Modifiers == Keys.Shift)
                    {
                        query4.Focus();
                        query4.PerformClick();
                        break;
                    }
                    break;
                case Keys.F5:
                    if (e.Modifiers == Keys.Shift)
                    {
                        query5.Focus();
                        query5.PerformClick();
                        break;
                    }
                    break;
                case Keys.F6:
                    if (e.Modifiers == Keys.Shift)
                    {
                        query6.Focus();
                        query6.PerformClick();
                        break;
                    }
                    break;
                case Keys.F7:
                    if (e.Modifiers == Keys.Shift)
                    {
                        query7.Focus();
                        query7.PerformClick();
                        break;
                    }
                    break;
                case Keys.F8:
                    if (e.Modifiers == Keys.Shift)
                    {
                        query8.Focus();
                        query8.PerformClick();
                        break;
                    }
                    break;
                case Keys.F10:
                    if (e.Modifiers == Keys.Shift)
                    {
                        query10.Focus();
                        query10.PerformClick();
                        break;
                    }
                    break;
                case Keys.F11:
                    btnExit.Focus();
                    btnExit.PerformClick();
                    break;
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            paramsInit().Print();
        }

        private void btnPreView_Click(object sender, EventArgs e)
        {
            paramsInit().PreView();
        }

        private void btnWord_Click(object sender, EventArgs e)
        {
            paramsInit().Word();
        }

        private void btnExcel_Click(object sender, EventArgs e)
        {
            paramsInit().Excel();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Dispose();
            this.Close();
        }
    }
}
