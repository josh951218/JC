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
    public partial class 銀行轉帳作業_列印 : FormT
    {
        DataTable PrintTable = new DataTable();
        string path = "";
        public 銀行轉帳作業_列印()
        {
            InitializeComponent();
            if (Common.User_DateTime == 1) date1.MaxLength = date2.MaxLength =  7;
            else date1.MaxLength = date2.MaxLength = 8;
            date1.Init(); date2.Init();
        }

        private void 銀行轉帳作業_列印_Load(object sender, EventArgs e)
        {
            
            if (Common.User_DateTime == 1)
            {
                date1.Text = Date.GetDateTime(1, false).Substring(0, 5) + "01";
                date2.Text = Date.GetDateTime(1, false);
            }
            else
            {
                date1.Text = Date.GetDateTime(2, false).Substring(0, 6) + "01";
                date2.Text = Date.GetDateTime(2, false);
            }
            date1.Focus();
        }

        RPT paramsInit()
        {
            if (radio1.Checked) path = Common.reportaddress + "Report\\銀行轉帳作業_標準報表.rpt";
            else path = Common.reportaddress + "Report\\銀行轉帳作業_幣別表.rpt";
            RPT rp = new RPT(PrintTable, path);
            rp.office = this.Tag.ToString();
            //公司抬頭
            rp.lobj.Add(new string[] { "txtstart", CHK.GetCoName2(Common.使用者預設公司) });
            //制表日期
            rp.lobj.Add(new string[] { "制表日期", Date.GetDateTime(Common.User_DateTime, true) });
            //單行註腳
            if (this.FindControl("單行註腳") != null)
            {
                string txtend;
                if (((radio)this.FindControl("單行註腳"+"1")).Checked) txtend = Common.dtEnd.Rows[0]["tamemo"].ToString();
                else if (((radio)this.FindControl("單行註腳"+"2")).Checked) txtend = Common.dtEnd.Rows[1]["tamemo"].ToString();
                else if (((radio)this.FindControl("單行註腳"+"3")).Checked) txtend = Common.dtEnd.Rows[2]["tamemo"].ToString();
                else if (((radio)this.FindControl("單行註腳"+"4")).Checked) txtend = Common.dtEnd.Rows[3]["tamemo"].ToString();
                else if (((radio)this.FindControl("單行註腳"+"5")).Checked) txtend = Common.dtEnd.Rows[4]["tamemo"].ToString();
                else txtend = "";
                rp.lobj.Add(new string[] { "txtend", txtend });
            }
            return rp;
        }

        bool compare()
        {
            if (string.Compare(date1.Text, date2.Text) > 0) return true;
            else return false;

        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (compare())
            {
                MessageBox.Show("起始日期不可大於終止日期", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                date1.Focus();
                return;
            }
            setsql();
            paramsInit().Print();
        }

        private void btnPreView_Click(object sender, EventArgs e)
        {
            if (compare())
            {
                MessageBox.Show("起始日期不可大於終止日期", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            setsql();
            paramsInit().PreView();
        }

        private void btnWord_Click(object sender, EventArgs e)
        {
            if (compare())
            {
                MessageBox.Show("起始日期不可大於終止日期", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            setsql();
            paramsInit().Word();
        }

        private void btnExcel_Click(object sender, EventArgs e)
        {
            if (compare())
            {
                MessageBox.Show("起始日期不可大於終止日期", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            setsql();
            paramsInit().Excel();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Dispose();
            this.Close();
        }

        private void date2_Validating(object sender, CancelEventArgs e)
        {
            TextBox tb = sender as TextBox;
            if (tb.Text.Trim() == "")
            {
                MessageBox.Show("日期不可為空", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                e.Cancel = true;
            }
            if (!tb.IsDateTime())
            {
                MessageBox.Show("日期格式錯誤","訊息視窗",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                e.Cancel = true;
            }
        }

        void setsql()
        {
            string sql = "";
            PrintTable.Clear();
            using (SqlConnection cn = new SqlConnection(Common.sqlConnString))
            {
                cn.Open();
                sql = "select * from carry where cadate >= '" + Date.ToTWDate(date1.Text) + "' and cadate <= '" + Date.ToTWDate(date2.Text) + "' order by cadate,cano";
                SqlDataAdapter dd = new SqlDataAdapter(sql, cn);
                dd.Fill(PrintTable);
            }
        }


    }
}
