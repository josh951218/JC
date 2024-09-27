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
using System.Windows.Forms.DataVisualization.Charting;


namespace S_61.s_2統計圖表
{
    public partial class 應收票齡分析_總額表 : FormT
    {
        public string Rule1_2 = "", Rule2_1 = "", Rule2_2 = "", Rule3_1 = "", Rule3_2 = "", Rule4_1 = "", Rule4_2 = "", Rule5_1 = "";
        public DataTable table = new DataTable();
        List<txtNumber> Num;

        public 應收票齡分析_總額表()
        {
            InitializeComponent();
            Num = new List<txtNumber>() { Lv1Count, Lv2Count, Lv3Count, Lv4Count, Lv5Count };
            Num.ForEach(r =>
            {
                r.NumThousands = true;
                r.NumLast = Common.金額小數;
                r.NumFirst = (20 - 1 - Common.金額小數);
            });
            if (Common.User_DateTime == 1) date1.MaxLength = date2.MaxLength = 7;
            else date1.MaxLength = date2.MaxLength = 8;
            date1.Init(); date2.Init();
            this.RuleLv1.DefaultCellStyle.Format = "N" + Common.金額小數;
            this.RuleLv2.DefaultCellStyle.Format = "N" + Common.金額小數;
            this.RuleLv3.DefaultCellStyle.Format = "N" + Common.金額小數;
            this.RuleLv4.DefaultCellStyle.Format = "N" + Common.金額小數;
            this.RuleLv5.DefaultCellStyle.Format = "N" + Common.金額小數;
        }

        private void 應收票齡分析_總額表_Load(object sender, EventArgs e)
        {
            單行註腳.BackColor = Color.FromArgb(215, 227, 239);
            this.RuleLv1.HeaderText = Rule1_2.PadLeft(3, ' ') + "日以下";
            this.RuleLv2.HeaderText = Rule2_2.PadLeft(3, ' ') + "日 ~ " + Rule2_1.PadLeft(3, ' ') + "日";
            this.RuleLv3.HeaderText = Rule3_2.PadLeft(3, ' ') + "日 ~ " + Rule3_1.PadLeft(3, ' ') + "日";
            this.RuleLv4.HeaderText = Rule4_2.PadLeft(3, ' ') + "日 ~ " + Rule4_1.PadLeft(3, ' ') + "日";
            this.RuleLv5.HeaderText = Rule5_1.PadLeft(3, ' ') + "日以上";
            lbLv5.Text = Rule5_1.PadLeft(3, ' ') + "日以上";
            lbLv4.Text = Rule4_2.PadLeft(3, ' ') + "日 ~ " + Rule4_1.PadLeft(3, ' ') + "日";
            lbLv3.Text = Rule3_2.PadLeft(3, ' ') + "日 ~ " + Rule3_1.PadLeft(3, ' ') + "日";
            lbLv2.Text = Rule2_2.PadLeft(3, ' ') + "日 ~ " + Rule2_1.PadLeft(3, ' ') + "日";
            lbLv1.Text = Rule1_2.PadLeft(3, ' ') + "日以下";

            dataGridViewT1.SetWidthByPixel();
            dataGridViewT1.AutoGenerateColumns = false;
            dataGridViewT1.DataSource = table;
            decimal lv1 = 0, lv2 = 0, lv3 = 0, lv4 = 0, lv5 = 0;
            for (int i = 0; i < table.Rows.Count; i++)
            {
                lv1 += table.Rows[i]["RuleLv1"].ToDecimal();
                lv2 += table.Rows[i]["RuleLv2"].ToDecimal();
                lv3 += table.Rows[i]["RuleLv3"].ToDecimal();
                lv4 += table.Rows[i]["RuleLv4"].ToDecimal();
                lv5 += table.Rows[i]["RuleLv5"].ToDecimal();
            }
            Lv1Count.Text = lv1.ToString("N" + Common.金額小數);
            Lv2Count.Text = lv2.ToString("N" + Common.金額小數);
            Lv3Count.Text = lv3.ToString("N" + Common.金額小數);
            Lv4Count.Text = lv4.ToString("N" + Common.金額小數);
            Lv5Count.Text = lv5.ToString("N" + Common.金額小數);
            Count.Text = table.Rows.Count.ToString();
            圖表設定();
        }

        RPT paramsInit()
        {
            string path = Common.reportaddress + "Report\\應收票齡分析_總額表.rpt";
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
            //lbLv1
            rp.lobj.Add(new string[] { "lbLv1", lbLv1.Text.Trim() });
            //lbLv2
            rp.lobj.Add(new string[] { "lbLv2", lbLv2.Text.Trim() });
            //lbLv3
            rp.lobj.Add(new string[] { "lbLv3", lbLv3.Text.Trim() });
            //lbLv4
            rp.lobj.Add(new string[] { "lbLv4", lbLv4.Text.Trim() });
            //lbLv5
            rp.lobj.Add(new string[] { "lbLv5", lbLv5.Text.Trim() });
            return rp;
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

        void 圖表設定()
        {
            string[] xValues = { lbLv5.Text + "    ", lbLv4.Text, lbLv3.Text, lbLv2.Text, lbLv1.Text + "    " };
            decimal[] yValues = { Lv5Count.Text.ToDecimal(), Lv4Count.Text.ToDecimal(), Lv3Count.Text.ToDecimal(), Lv2Count.Text.ToDecimal(), Lv1Count.Text.ToDecimal(),};
            //設定 ChartArea1
            chart1.ChartAreas["ChartArea1"].Area3DStyle.Enable3D = true;
            chart1.ChartAreas["ChartArea1"].Area3DStyle.Inclination = 50;
            chart1.ChartAreas[0].AxisX.Interval = 1;

            //設定 Legends
            chart1.Legends["Legend1"].BackColor = Color.FromArgb(224, 224, 224);
            chart1.Legends["Legend1"].BackHatchStyle = ChartHatchStyle.DarkDownwardDiagonal;//斜線背景
            chart1.Legends["Legend1"].Font = new Font("細明體", 10);
            //設定 Series1
            chart1.Series["Series1"].ChartType = SeriesChartType.Pie;
            chart1.Series["Series1"].Points.DataBindXY(xValues, yValues);
            chart1.Series["Series1"].LegendText = "#VALX:    [ #PERCENT{P1} ]"; //X軸 + 百分比
            chart1.Series["Series1"].Label = "#VALX\n#PERCENT{P1}"; //X軸 + 百分比
            chart1.Series["Series1"]["PieLabelStyle"] = "Outside"; //數值顯示在圓餅外
            //chart1.Series["Series1"]["PieLabelStyle"] = "Inside"; //數值顯示在圓餅內
            //Chart1.Series["Series1"]["PieLabelStyle"] = "Disabled"; //不顯示數值
            chart1.Series["Series1"]["PieDrawingStyle"] = "Default";
            chart1.Series["Series1"].Font = new Font("細明體", 10);
            chart1.Series["Series1"].Points.FindMaxByValue().LabelForeColor = Color.Red;//最多顯示紅字
        }
    }
}
