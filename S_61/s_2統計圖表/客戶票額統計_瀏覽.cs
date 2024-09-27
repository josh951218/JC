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
    public partial class 客戶票額統計_瀏覽 : FormT
    {
        public DataTable table = new DataTable();
        List<txtNumber> Num;
        public 客戶票額統計_瀏覽()
        {
            InitializeComponent();
            Num = new List<txtNumber>(){Tot, Tot_1, Tot_2, Tot_3, Tot_4, Tot_5, Tot_6, Tot_7, Tot_8};
            Num.ForEach(r =>
            {
                r.NumThousands = true;
                r.NumLast = Common.金額小數;
                r.NumFirst = (20 - 1 - Common.金額小數);
            });
            if (Common.User_DateTime == 1) date1.MaxLength = date2.MaxLength = 7;
            else date1.MaxLength = date2.MaxLength = 8;
            date1.Init();
            date2.Init();
        }

        private void 客戶票額統計_瀏覽_Load(object sender, EventArgs e)
        {
            單行註腳.BackColor = panelT1.BackColor = Color.FromArgb(215, 227, 239);
            dataGridViewT1.SetWidthByPixel();
            dataGridViewT1.AutoGenerateColumns = false;
            dataGridViewT1.DataSource = table;
            Count.Text = table.Rows.Count.ToString();
            this.未處理.DefaultCellStyle.Format = "N" + Common.金額小數;
            this.託收.DefaultCellStyle.Format = "N" + Common.金額小數;
            this.託收兌現.DefaultCellStyle.Format = "N" + Common.金額小數;
            this.現金兌現.DefaultCellStyle.Format = "N" + Common.金額小數;
            this.應收轉付.DefaultCellStyle.Format = "N" + Common.金額小數;
            this.票貼.DefaultCellStyle.Format = "N" + Common.金額小數;
            this.退票.DefaultCellStyle.Format = "N" + Common.金額小數;
            this.其他.DefaultCellStyle.Format = "N" + Common.金額小數;
            this.小計.DefaultCellStyle.Format = "N" + Common.金額小數;
            decimal tot = 0, tot_1 = 0, tot_2 = 0, tot_3 = 0, tot_4 = 0, tot_5 = 0, tot_6 = 0, tot_7 = 0,tot_8 = 0;
            table.AsEnumerable().ToList().ForEach(r =>
            {
                tot += r["小計"].ToDecimal();
                tot_1 += r["未處理"].ToDecimal();
                tot_2 += r["託收"].ToDecimal();
                tot_3 += r["託收兌現"].ToDecimal();
                tot_4 += r["現金兌現"].ToDecimal();
                tot_5 += r["應收轉付"].ToDecimal();
                tot_6 += r["票貼"].ToDecimal();
                tot_7 += r["退票"].ToDecimal();
                tot_8 += r["其他"].ToDecimal();
            });
            Tot.Text = tot.ToString("N" + Common.金額小數);
            Tot_1.Text = tot_1.ToString("N" + Common.金額小數);
            Tot_2.Text = tot_2.ToString("N" + Common.金額小數);
            Tot_3.Text = tot_3.ToString("N" + Common.金額小數);
            Tot_4.Text = tot_4.ToString("N" + Common.金額小數);
            Tot_5.Text = tot_5.ToString("N" + Common.金額小數);
            Tot_6.Text = tot_6.ToString("N" + Common.金額小數);
            Tot_7.Text = tot_7.ToString("N" + Common.金額小數);
            Tot_8.Text = tot_8.ToString("N" + Common.金額小數);
        }

        RPT paramsInit()
        {
            string path = Common.reportaddress + "Report\\客戶票額統計_標準報表.rpt";
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
