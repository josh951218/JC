﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.Data;
using S_61.Basic;
using S_61.Model;
using S_61.MyControl;

namespace S_61.s_3基本資料
{
    public partial class 廠商資料瀏覽_明細 : FormT
    {
        public DataTable table = new DataTable();
        string path = "";
        public 廠商資料瀏覽_明細()
        {
            InitializeComponent();
        }

        private void FrmFactInfob_Load(object sender, EventArgs e)
        {
            if (!System.IO.File.Exists(Common.reportaddress + "Report\\廠商資料瀏覽_自訂一.rpt")) radio2.Enabled = false;
            if (!System.IO.File.Exists(Common.reportaddress + "Report\\廠商資料瀏覽_自訂二.rpt")) radio3.Enabled = false;
            if (!System.IO.File.Exists(Common.reportaddress + "Report\\廠商資料瀏覽_自訂三.rpt")) radio4.Enabled = false;
            if (!System.IO.File.Exists(Common.reportaddress + "Report\\廠商資料瀏覽_自訂四.rpt")) radio5.Enabled = false;
            ToolTip settoolname = new ToolTip();
            settoolname.SetToolTip(radio1, "廠商資料瀏覽_內定報表");
            settoolname.SetToolTip(lbradio1, "廠商資料瀏覽_內定報表");
            settoolname.SetToolTip(radio2, "廠商資料瀏覽_自訂一");
            settoolname.SetToolTip(lbradio2, "廠商資料瀏覽_自訂一");
            settoolname.SetToolTip(radio3, "廠商資料瀏覽_自訂二");
            settoolname.SetToolTip(lbradio3, "廠商資料瀏覽_自訂二");
            settoolname.SetToolTip(radio4, "廠商資料瀏覽_自訂三");
            settoolname.SetToolTip(lbradio4, "廠商資料瀏覽_自訂三");
            settoolname.SetToolTip(radio5, "廠商資料瀏覽_自訂四");
            settoolname.SetToolTip(lbradio5, "廠商資料瀏覽_自訂四");

            Basic.SetParameter.FontSize(groupBox1);
            Basic.SetParameter.FontSize(單行註腳);
            radio1.Checked = true;
            radio6.Checked = true;

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
            if (table.Rows.Count > 0)
            {
                dataGridViewT1.DataSource = table;
            }
            switch (Screen.PrimaryScreen.Bounds.Width)
            {
                case 800:
                    tableLayoutPnl2.Height = 59;
                    break;
                case 1024:
                    tableLayoutPnl2.Height = 80;
                    break;
                default:
                    tableLayoutPnl2.Height = 80;
                    break;
            }
        }

        RPT paramsInit()
        {
            if (radio1.Checked)
                path = Common.reportaddress + "Report\\廠商資料瀏覽_內定報表.rpt";
            else if (radio2.Checked)
                path = Common.reportaddress + "Report\\廠商資料瀏覽_自訂一.rpt";
            else if (radio3.Checked)
                path = Common.reportaddress + "Report\\廠商資料瀏覽_自訂二.rpt";
            else if (radio4.Checked)
                path = Common.reportaddress + "Report\\廠商資料瀏覽_自訂三.rpt";
            else if (radio4.Checked)
                path = Common.reportaddress + "Report\\廠商資料瀏覽_自訂四.rpt";

            RPT rp = new RPT(table, path);
            rp.office = this.Tag.ToString();
            //公司抬頭
            rp.lobj.Add(new string[] { "txtstart", Common.dtstart.Rows[0]["coname2"].ToString() });
            //制表日期
            rp.lobj.Add(new string[] { "制表日期", Date.GetDateTime(Common.User_DateTime, true) });
            //單行註腳
            if (this.FindControl("單行註腳") != null)
            {
                string txtend;
                if (radio6.Checked) txtend = Common.dtEnd.Rows[0]["tamemo"].ToString();
                else if (radio7.Checked) txtend = Common.dtEnd.Rows[1]["tamemo"].ToString();
                else if (radio8.Checked) txtend = Common.dtEnd.Rows[2]["tamemo"].ToString();
                else if (radio9.Checked) txtend = Common.dtEnd.Rows[3]["tamemo"].ToString();
                else if (radio10.Checked) txtend = Common.dtEnd.Rows[4]["tamemo"].ToString();
                else txtend = "";
                rp.lobj.Add(new string[] { "txtend", txtend });
            }
            return rp;
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            paramsInit().Print();
        }

        private void btnPreview_Click(object sender, EventArgs e)
        {
            paramsInit().PreView();
        }

        private void bteExit_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
        }

        private void btnWord_Click(object sender, EventArgs e)
        {
            paramsInit().Word();
        }

        private void btnExcel_Click(object sender, EventArgs e)
        {
            paramsInit().Excel();
        }

        private void radio1_CheckedChanged(object sender, EventArgs e)
        {
            S_61.Model.ForControls.ForRadio((RadioButton)sender);
        }

        private void lbradio1_Click(object sender, EventArgs e)
        {
            S_61.Model.ForControls.ForRadio((Label)sender);
        }

        private void FrmFactInfob_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                bteExit.PerformClick();
            if (e.KeyCode == Keys.F11)
                bteExit.PerformClick();
        }
    }
}
