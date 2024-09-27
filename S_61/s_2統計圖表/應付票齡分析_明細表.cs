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

namespace S_61.s_2統計圖表
{
    public partial class 應付票齡分析_明細表 : FormT
    {
        public string Rule1_2 = "", Rule2_1 = "", Rule2_2 = "", Rule3_1 = "", Rule3_2 = "", Rule4_1 = "", Rule4_2 = "", Rule5_1 = "";
        public DataTable table = new DataTable();
        DataTable ViewTb = new DataTable();
        List<DataRow> list = new List<DataRow>();
        List<string> fano_list = new List<string>();
        List<txtNumber> Num;

        public 應付票齡分析_明細表()
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
            this.開票日.DataPropertyName = Common.User_DateTime == 1 ? "chdate1" : "chdate1_1";
        }

        private void 應付票齡分析_明細表_Load(object sender, EventArgs e)
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
            list = table.AsEnumerable().ToList();
            fano_list = table.AsEnumerable().Select(r => r["fano"].ToString()).Distinct().ToList();
            WriteToTxt(fano_list.First().ToString().Trim());
        }

        void WriteToTxt(string fano)
        {
            ViewTb.Clear();
            ViewTb = list.Where(r => r["fano"].ToString().Trim() == fano).OrderBy(r => r["chdate1"].ToString()).CopyToDataTable();
            decimal lv1 = 0, lv2 = 0, lv3 = 0, lv4 = 0, lv5 = 0;
            for (int i = 0; i < ViewTb.Rows.Count; i++)
            {
                lv1 += ViewTb.Rows[i]["RuleLv1"].ToDecimal();
                lv2 += ViewTb.Rows[i]["RuleLv2"].ToDecimal();
                lv3 += ViewTb.Rows[i]["RuleLv3"].ToDecimal();
                lv4 += ViewTb.Rows[i]["RuleLv4"].ToDecimal();
                lv5 += ViewTb.Rows[i]["RuleLv5"].ToDecimal();
            }
            Lv1Count.Text = lv1.ToString("N" + Common.金額小數);
            Lv2Count.Text = lv2.ToString("N" + Common.金額小數);
            Lv3Count.Text = lv3.ToString("N" + Common.金額小數);
            Lv4Count.Text = lv4.ToString("N" + Common.金額小數);
            Lv5Count.Text = lv5.ToString("N" + Common.金額小數);
            TotCount.Text = ViewTb.Rows.Count.ToString();
            FaNo.Text = fano;
            FaName1.Text = list.Find(r => r["fano"].ToString().Trim() == fano)["faname1"].ToString();
            dataGridViewT1.DataSource = ViewTb;
        }

        RPT paramsInit()
        {
            string path = Common.reportaddress + "Report\\應付票齡分析_明細表.rpt";
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

        private void btnTop_Click(object sender, EventArgs e)
        {
            WriteToTxt(fano_list.First().Trim());
        }

        private void btnPrior_Click(object sender, EventArgs e)
        {
            int index = fano_list.FindIndex(r => r.ToString().Trim() == FaNo.Text.Trim());
            if (index - 1 < 0)
            {
                MessageBox.Show("已最上一筆", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                WriteToTxt(fano_list[index - 1].ToString().Trim());
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            int index = fano_list.FindIndex(r => r.ToString().Trim() == FaNo.Text.Trim());
            if (index + 1 > fano_list.Count - 1)
            {
                MessageBox.Show("已最下一筆", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                WriteToTxt(fano_list[index + 1].ToString().Trim());
            }
        }

        private void btnBottom_Click(object sender, EventArgs e)
        {
            WriteToTxt(fano_list.Last().Trim());
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
