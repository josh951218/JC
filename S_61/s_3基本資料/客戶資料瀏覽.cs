using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;
using S_61.Basic;
using S_61.Model;
using S_61.MyControl;

namespace S_61.s_3基本資料
{
    public partial class 客戶資料瀏覽 : FormT
    {
        public DataTable dt = new DataTable();
        List<DataRow> list = new List<DataRow>();
        public String sqlstr = "";

        public 客戶資料瀏覽()
        {
            InitializeComponent();
            tableLayoutPanel1.BackColor = Color.FromArgb(215, 227, 239);
        }

        private void FrmCustInfo_Load(object sender, EventArgs e)
        {
            CuNo.Focus();
            Common.取得浮動連線字串(Common.使用者預設公司);
        }

        private void btnBrow_Click(object sender, EventArgs e)
        {
            if (CuNo.Text.Trim() != "" && CuNo_1.Text.Trim() != "")
            {
                if (string.CompareOrdinal(CuNo.Text.Trim(), CuNo_1.Text.Trim()) > 0)
                {
                    MessageBox.Show("起始客戶不可大於終止客戶，請確定", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    CuNo.Focus();
                    return;
                }
            }
            if (CuPareNo.Text.Trim() != "" && CuPareNo1.Text.Trim() != "")
            {
                if (string.CompareOrdinal(CuPareNo.Text.Trim(), CuPareNo1.Text.Trim()) > 0)
                {
                    MessageBox.Show("起始經銷商不可大於終止經銷商，請確定", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    CuPareNo.Focus();
                    return;
                }
            }
            if (CuX1No.Text.Trim() != "" && CuX1No_1.Text.Trim() != "")
            {
                if (string.CompareOrdinal(CuX1No.Text.Trim(), CuX1No_1.Text.Trim()) > 0)
                {
                    MessageBox.Show("起始客戶類別編號不可大於終止客戶類別編號，請確定", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    CuX1No.Focus();
                    return;
                }
            }
            if (CuEmNo1.Text.Trim() != "" && CuEmNo1_1.Text.Trim() != "")
            {
                if (string.CompareOrdinal(CuEmNo1.Text.Trim(), CuEmNo1_1.Text.Trim()) > 0)
                {
                    MessageBox.Show("起始業務人員編號不可大於終止業務人員編號，請確定", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    CuEmNo1.Focus();
                    return;
                }
            }
            if (CuX4No.Text.Trim() != "" && CuX4No_1.Text.Trim() != "")
            {
                if (string.CompareOrdinal(CuX4No.Text.Trim(), CuX4No_1.Text.Trim()) > 0)
                {
                    MessageBox.Show("起始結帳類別編號不可大於終止結帳類別編號，請確定", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    CuX4No.Focus();
                    return;
                }
            }
            try
            {
                using (SqlConnection conn = new SqlConnection(Common.浮動連線字串))
                {
                    string str = "select * from cust where 0=0 ";
                    if (CuNo.Text.Trim() != "") str += " and CuNo >= N'" + CuNo.Text + "'";
                    if (CuNo_1.Text.Trim() != "") str += " and CuNo <= N'" + CuNo_1.Text + "'";
                    if (CuX1No.Text.Trim() != "") str += " and CuX1No >= N'" + CuX1No.Text + "'";
                    if (CuX1No_1.Text.Trim() != "") str += " and CuX1No <= N'" + CuX1No_1.Text + "'";
                    if (CuEmNo1.Text.Trim() != "") str += " and CuEmNo1 >= N'" + CuEmNo1.Text + "'";
                    if (CuEmNo1_1.Text.Trim() != "") str += " and CuEmNo1 <= N'" + CuEmNo1_1.Text + "'";
                    if (CuR1.Text.Trim() != "") str += " and CuR1 >= N'" + CuR1.Text + "'";
                    if (CuR1_1.Text.Trim() != "") str += " and CuR1 <= N'" + CuR1_1.Text + "'";
                    if (CuX4No.Text.Trim() != "") str += " and CuX4No >= N'" + CuX4No.Text + "'";
                    if (CuX4No_1.Text.Trim() != "") str += " and CuX4No <= N'" + CuX4No_1.Text + "'";
                    if (CuName1.Text.Trim() != "") str += " and CuName1 like('" + CuName1.Text.Trim() + "%')";
                    if (CuIme.Text.Trim() != "") str += " and CuIme like('" + CuIme.Text.Trim() + "%')";
                    if (CuTel1.Text.Trim() != "") str += " and CuTel1 like('" + CuTel1.Text.Trim() + "%')";
                    if (CuAddr1.Text.Trim() != "") str += " and CuAddr1 like('%" + CuAddr1.Text.Trim() + "%')";
                    if (CuMemo1.Text.Trim() != "") str += " and CuMemo1 like('%" + CuMemo1.Text.Trim() + "%')";
                    if (CuUdf1.Text.Trim() != "") str += " and CuUdf1 like('%" + CuUdf1.Text.Trim() + "%')";

                    sqlstr = str;
                    sqlstr += " and CuAdvamt not in('0')";
                    sqlstr += "order by cuno ";
                    str += "order by cuno ";

                    SqlDataAdapter da = new SqlDataAdapter(str, conn);
                    dt.Clear();
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        客戶資料瀏覽_明細 frm = new 客戶資料瀏覽_明細();
                        frm.SetParaeter();
                        frm.table = dt;
                        frm.ShowDialog();
                    }
                    else
                    {
                        MessageBox.Show("找不到任何資料，請重新輸入", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    da.Dispose();
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
        }



        //其它
        private void CuNo_DoubleClick(object sender, EventArgs e)
        {
            CHK.CuNo_OpemFrm(sender as TextBox);
        }

        private void CuX1No_DoubleClick(object sender, EventArgs e)
        {
            CHK.X1No_OpemFrm(sender as TextBox);
        }

        private void CuEmNo1_DoubleClick(object sender, EventArgs e)
        {
            CHK.EmNo_OpemFrm(sender as TextBox);
        }

        private void CuX4No_DoubleClick(object sender, EventArgs e)
        {
            CHK.X4No_OpemFrm(sender as TextBox);
        }

        private void FrmCustInfo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F11) this.Dispose();
        }


    }
}
