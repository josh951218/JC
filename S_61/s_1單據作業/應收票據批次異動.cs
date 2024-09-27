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
using S_61.s_3基本資料;

namespace S_61.s_1單據作業
{
    public partial class 應收票據批次異動 : FormT
    {
        DataTable table = new DataTable();
        string forwork = "";
        public 應收票據批次異動()
        {
            InitializeComponent();
        }

        private void 應收票據批次異動_Load(object sender, EventArgs e)
        {
            if (Common.單據異動 == "2") CoNo.Enabled = false;
            ToolTip _Tip = new ToolTip();
            _Tip.SetToolTip(rd1, "票態:託收");
            _Tip.SetToolTip(rd2, "票態:未處理");
            _Tip.SetToolTip(rd3, "票態:未處理");
            _Tip.SetToolTip(rd4, "票態:未處理,託收");
            _Tip.SetToolTip(rd5, "票態:託收");

            groupBoxT1.BackColor = Color.FromArgb(215, 227, 239);
            if (Common.User_DateTime == 1) ChDate3.MaxLength = 7;
            else ChDate3.MaxLength = 8;
            ChDate3.Init();

            CoNo.Text = Common.使用者預設公司;
            CHK.GetCoName(CoNo, CoName1);
            ChDate3.Focus();
        }

        private void btnBrow_Click(object sender, EventArgs e)
        {
            try
            {
                string str = " and cono=N'" + CoNo.Text.Trim() + "'";
                    //+" and chdate >= '"+Common.系統民國+"0101"+"'";//結轉後 上個年度的未處理&託收 還是可以撈出來
                if (ChDate3.Text.Trim() != "")
                {
                    if(Common.User_DateTime == 1)str += " and chdate3 <= '" + ChDate3.Text.Trim() + "'";
                    else str += " and chdate3_1 <= '" + ChDate3.Text.Trim() + "'";
                }
                if(AcNo.Text.Trim() != "")str+=" and acno = '"+AcNo.Text.Trim()+"'";
                str += " order by chdate2";
                string sql = "";
                if (rd1.Checked) sql = "select *,到期日='',預兌日='',異動日期='' from chki where '0'='0' and chstatus=2" + str;
                if (rd2.Checked) sql = "select *,到期日='',預兌日='',異動日期='' from chki where '0'='0' and chstatus=1" + str;
                if (rd3.Checked) sql = "select *,到期日='',預兌日='',異動日期='' from chki where '0'='0' and chstatus=1" + str;
                if (rd4.Checked) sql = "select *,到期日='',預兌日='',異動日期='' from chki where '0'='0' and chstatus in (1,2)" + str;
                if (rd5.Checked) sql = "select *,到期日='',預兌日='',異動日期='' from chki where '0'='0' and chstatus=1" + str;
                using (SqlConnection cn = new SqlConnection(Common.sqlConnString))
                {
                    cn.Open();
                    SqlDataAdapter dd = new SqlDataAdapter(sql, cn);
                    table.Clear();
                    dd.Fill(table);
                    if (table.Rows.Count == 0)
                    {
                        MessageBox.Show("找不到任何資料，請重新輸入", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }
                if (rd1.Checked || rd2.Checked || rd4.Checked || rd5.Checked)
                {
                    forwork = "";
                    if (rd1.Checked) forwork = "託收兌現";
                    else if (rd2.Checked) forwork = "現金兌現";
                    else if (rd3.Checked) forwork = "應收轉付";
                    else if (rd4.Checked) forwork = "退票";
                    else if (rd5.Checked) forwork = "其他";
                    using (應收票據批次異動_託收兌現 frm = new 應收票據批次異動_託收兌現())
                    {
                        frm.SetParaeter();
                        frm.forwork = forwork;
                        frm.cono = CoNo.Text.Trim();
                        frm.coname1 = CoName1.Text.Trim();
                        frm.table = table;
                        frm.ShowDialog();
                    }
                }
                else
                {
                    using (應收票據批次異動_應收轉付 frm = new 應收票據批次異動_應收轉付())
                    {
                        frm.SetParaeter();
                        frm.cono = CoNo.Text.Trim();
                        frm.coname1 = CoName1.Text.Trim();
                        frm.table = table;
                        frm.ShowDialog();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Dispose();
            this.Close();
        }

        private void CoNo_DoubleClick(object sender, EventArgs e)
        {
            CHK.CoNo_OpemFrm(CoNo, CoName1);
        }

        private void CoNo_Validating(object sender, CancelEventArgs e)
        {
            if (btnExit.Focused) return;
            if(CoNo.Text.Trim() == "")
            {
                MessageBox.Show("『公司編號』不可為空白，請確定後重新輸入", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                e.Cancel = true;
                return;
            }
            if (!CHK.CoNo_Validating(CoNo, CoName1))
            {
                e.Cancel = true;
                CHK.CoNo_OpemFrm(CoNo, CoName1);
            }
            else
            {
                Common.取得浮動連線字串(CoNo.Text.Trim());
                if (!CHK.AcNo_Validating(CoNo.Text.Trim(), AcNo, AcName1, null, true))
                    AcNo.Text = AcName1.Text = "";
            }
        }

        private void ChDate3_Validating(object sender, CancelEventArgs e)
        {
            if (btnExit.Focused || ChDate3.Text.Trim()=="") return;
            if (!ChDate3.IsDateTime())
            {
                MessageBox.Show("日期格式錯誤，請確定後重新輸入", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                e.Cancel = true;
                return;
            }
            if (!CHK.稽核會計年度(ChDate3.Text.Trim())) e.Cancel = true;
        }

        private void AcNo_DoubleClick(object sender, EventArgs e)
        {
            CHK.AcNo_OpemFrm(CoNo.Text.Trim(),AcNo, AcName1, null, true, true);
        }

        private void AcNo_Validating(object sender, CancelEventArgs e)
        {
            if (btnExit.Focused) return;
            if (AcNo.Text.Trim() == "") return;
            if (!CHK.AcNo_Validating(CoNo.Text.Trim(), AcNo, AcName1, null, true))
            {
                e.Cancel = true;
                CHK.AcNo_OpemFrm(CoNo.Text.Trim(), AcNo, AcName1, null, true, true);
            }
        }
    }
}
