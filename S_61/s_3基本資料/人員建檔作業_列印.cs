using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using S_61.Basic;
using S_61.Model;

namespace S_61.s_3基本資料
{
    public partial class 人員建檔作業_列印 : FormT
    {
        DataTable dt = new DataTable();
        string path = "";

        public 人員建檔作業_列印()
        {
            InitializeComponent();
        }

        private void FrmEmplPrint_Load(object sender, EventArgs e)
        {
            loadDB();
            EmNo.Focus();
        }

        void loadDB()
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(Common.浮動連線字串))
                {
                    cn.Open();
                    string str = "select * from empl left join dept on empl.emdeno=dept.deno where EmNo >=N'" + EmNo.Text + "'";
                    if (EmNo_1.Text.Trim() != "")
                        str += " and EmNo <= N'" + EmNo_1.Text + "'";

                    using (SqlDataAdapter da = new SqlDataAdapter(str, cn))
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

        RPT paramsInit()
        {
            path = Common.reportaddress + "Report\\人員基本資料.rpt";
            RPT rp = new RPT(dt, path);
            rp.office = this.Tag.ToString();
            //公司抬頭
            rp.lobj.Add(new string[] { "txtstart", Common.dtstart.Rows[0]["coname2"].ToString() });
            //制表日期
            rp.lobj.Add(new string[] { "制表日期", Date.GetDateTime(Common.User_DateTime, true) });
            //單行註腳
            if (this.FindControl("單行註腳") != null)
            {
                string txtend;
                if (radioButton1.Checked) txtend = Common.dtEnd.Rows[0]["tamemo"].ToString();
                else if (radioButton2.Checked) txtend = Common.dtEnd.Rows[1]["tamemo"].ToString();
                else if (radioButton3.Checked) txtend = Common.dtEnd.Rows[2]["tamemo"].ToString();
                else if (radioButton4.Checked) txtend = Common.dtEnd.Rows[3]["tamemo"].ToString();
                else if (radioButton5.Checked) txtend = Common.dtEnd.Rows[4]["tamemo"].ToString();
                else txtend = "";
                rp.lobj.Add(new string[] { "txtend", txtend });
            }
            return rp;
        }

        private void btnprint_Click(object sender, EventArgs e)
        {
            paramsInit().Print();
        }

        private void btnpreview_Click(object sender, EventArgs e)
        {
            paramsInit().PreView();
        }

        private void btnword_Click(object sender, EventArgs e)
        {
            paramsInit().Word();
        }

        private void btnexcel_Click(object sender, EventArgs e)
        {
            paramsInit().Excel();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void EmNo_DoubleClick(object sender, EventArgs e)
        {
            pVar.EmplOpenForm(sender as TextBox);
        }
    }
}
