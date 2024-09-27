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
    public partial class 銀行資金預估作業_明細表 : FormT
    {
        public List<String> acno_list;
        public DataTable table;
        List<DataRow> list = new List<DataRow>();
        DataTable ViewTb = new DataTable();

        public 銀行資金預估作業_明細表()
        {
            InitializeComponent();
            this.已收未兌現.DefaultCellStyle.Format = "N" + Common.金額小數;
            this.已付未兌現.DefaultCellStyle.Format = "N" + Common.金額小數;
            this.預估餘額.DefaultCellStyle.Format = "N" + Common.金額小數;
            this.計算值.DefaultCellStyle.Format = "N" + Common.金額小數;
        }

        private void 銀行資金預估作業_瀏覽_Load(object sender, EventArgs e)
        {
            groupBoxT1.BackColor = 單行註腳.BackColor = Color.FromArgb(215, 227, 239);
            dataGridViewT1.SetWidthByPixel();
            dataGridViewT1.AutoGenerateColumns = false;
            list = table.AsEnumerable().ToList();
            WriteToTxt(acno_list.First().ToString().Trim());
        }

        void WriteToTxt(string acno)
        {
            ViewTb.Clear();
            ViewTb = list.Where(r => r["帳戶編號"].ToString().Trim() == acno).OrderBy(t => t["預兌日"].ToString()).ThenBy(t => t["票態"].ToString()).ThenBy(t => t["客戶"].ToString()).CopyToDataTable();
            AcNo.Text = ViewTb.Rows[0]["帳戶編號"].ToString().Trim();
            AcName1.Text = ViewTb.Rows[0]["帳戶簡稱"].ToString().Trim();
            AcAct.Text = ViewTb.Rows[0]["帳號"].ToString().Trim();
            dataGridViewT1.DataSource = ViewTb;
        }

        RPT paramsInit()
        {
            string path = "";
            if (radio1.Checked)
                path = Common.reportaddress + "Report\\銀行資金預估作業_不跳頁明細表.rpt";
            else
                path = Common.reportaddress + "Report\\銀行資金預估作業_跳頁明細表.rpt";
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

        private void btnTop_Click(object sender, EventArgs e)
        {
            WriteToTxt(acno_list.First().Trim());
        }

        private void btnPrior_Click(object sender, EventArgs e)
        {
            int index = acno_list.FindIndex(r => r.ToString().Trim() == AcNo.Text.Trim());
            if (index - 1 < 0)
            {
                MessageBox.Show("已最上一筆", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                WriteToTxt(acno_list[index - 1].ToString().Trim());
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            int index = acno_list.FindIndex(r => r.ToString().Trim() == AcNo.Text.Trim());
            if (index + 1 > acno_list.Count - 1)
            {
                MessageBox.Show("已最下一筆", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                WriteToTxt(acno_list[index + 1].ToString().Trim());
            }
        }

        private void btnBottom_Click(object sender, EventArgs e)
        {
            WriteToTxt(acno_list.Last().Trim());
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
