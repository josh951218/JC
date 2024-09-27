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
    public partial class 銀行資金預估作業_總額明細表 : FormT
    {
        public List<String> acno_list;
        public DataTable table;


        public 銀行資金預估作業_總額明細表()
        {
            InitializeComponent();
            this.已收未兌現.DefaultCellStyle.Format = "N" + Common.金額小數;
            this.已付未兌現.DefaultCellStyle.Format = "N" + Common.金額小數;
            this.預估餘額.DefaultCellStyle.Format = "N" + Common.金額小數;
            this.計算值.DefaultCellStyle.Format = "N" + Common.金額小數;
        }

        private void 銀行資金預估作業_總額明細表_Load(object sender, EventArgs e)
        {
            單行註腳.BackColor = Color.FromArgb(215, 227, 239);
            dataGridViewT1.SetWidthByPixel();
            dataGridViewT1.AutoGenerateColumns = false;
            dataGridViewT1.DataSource = table;
            預估總額.Text = dataGridViewT1["預估餘額", dataGridViewT1.Rows.Count - 1].Value.ToDecimal().ToString("N" + Common.金額小數);
            已收未兌總額.Text = table.AsEnumerable().Sum(r => r["已收未兌"].ToDecimal()).ToString("N" + Common.金額小數);
            已付未兌總額.Text = table.AsEnumerable().Sum(r => r["已付未兌"].ToDecimal()).ToString("N" + Common.金額小數);
        }

        RPT paramsInit()
        {
            string path = Common.reportaddress + "Report\\銀行資金預估作業_總額明細表.rpt";
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
