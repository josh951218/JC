using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using S_61.Basic;
using S_61.Model;
using S_61.MyControl;
using S_61.s_3基本資料;
using System.Linq;


namespace S_61.s_1單據作業
{
    public partial class 應付票據批次異動 : S_61.Model.FormT
    {
        DataTable Dt = new DataTable();
        string forwork = "";
        public 應付票據批次異動()
        {
            InitializeComponent();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Dispose();
            this.Close();
        }

        private void 應付票據批次異動_Load(object sender, EventArgs e)
        {
            if (Common.單據異動 == "2") CoNo.Enabled = false;
            ToolTip _Tip = new ToolTip();
            _Tip.SetToolTip(radio1, "票態:票已領取");
            _Tip.SetToolTip(radio2, "票態:票已領取");
            _Tip.SetToolTip(radio3, "票態:未處理,票已領取");
            _Tip.SetToolTip(radio4, "票態:未處理");

            groupBoxT1.BackColor = Color.FromArgb(215, 227, 239);
            if (Common.User_DateTime == 1) ChDate3.MaxLength = 7;
            else ChDate3.MaxLength = 8;
            ChDate3.Init();

            CoNo.Text = Common.使用者預設公司;
            CHK.GetCoName(CoNo, CoName1);
            ChDate3.Focus();
            CoNo.Focus();
        }

        private void CoNo_DoubleClick(object sender, EventArgs e)
        {
            CHK.CoNo_OpemFrm(CoNo, CoName1);
        }

        private void CoNo_Validating(object sender, CancelEventArgs e)
        {
            if (btnExit.Focused) return;
            if (CoNo.Text.Trim() == "")
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
                if (!CHK.AcNo_Validating(CoNo.Text.Trim(), AcNo, AcName1, null, true))
                    AcNo.Text = AcName1.Text = "";
            }
        }

        private void ChDate3_Validating(object sender, CancelEventArgs e)
        {
            if (btnExit.Focused || ChDate3.Text.Trim() == "") return;
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

        private void btnBrow_Click(object sender, EventArgs e)
        {
            try
            {
                string str = " and cono='" + CoNo.Text.Trim() + "'";
                //+"and chdate >='"+Common.系統民國+"0101"+"'";//結轉後 上個年度的未處理&託收 還是可以撈出來
                if (ChDate3.Text.Trim() != "") str += " and chdate3 <= '" + Date.ToTWDate(ChDate3.Text.Trim()) + "'";
                if (AcNo.Text.Trim() != "") str += " and acno = '" + AcNo.Text.Trim() + "'";
                str += " order by chdate2";
                string sql = "";
                if (radio1.Checked) sql = "select * from chko where '0'='0' and chstatus=2" + str;
                if (radio2.Checked) sql = "select * from chko where '0'='0' and chstatus=2" + str;
                if (radio3.Checked) sql = "select * from chko where '0'='0' and chstatus in (1,2)" + str;
                if (radio4.Checked) sql = "select * from chko where '0'='0' and chstatus=1" + str;
                using (SqlConnection cn = new SqlConnection(Common.sqlConnString))
                {
                    cn.Open();
                    SqlDataAdapter dd = new SqlDataAdapter(sql, cn);
                    Dt.Clear();
                    dd.Fill(Dt);
                    if (Dt.Rows.Count == 0)
                    {
                        MessageBox.Show("找不到任何資料，請重新輸入", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }
                forwork = "";
                if (radio1.Checked) forwork = "兌現";
                else if (radio2.Checked) forwork = "退票";
                else if (radio3.Checked) forwork = "作廢";
                else if (radio4.Checked) forwork = "其他";
                using (應付票據批次異動_瀏覽 frm = new 應付票據批次異動_瀏覽())
                {
                   frm.SetParaeter(ViewMode.Big);
                   frm.forwork = forwork;
                   frm.cono = CoNo.Text.Trim();
                   frm.coname1 = CoName1.Text.Trim();
                   frm.table = Dt;
                   frm.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
