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
    public partial class 廠商郵遞標籤 : FormT
    {
        public DataTable dt = new DataTable();
        public List<DataRow> list = new List<DataRow>();

        public 廠商郵遞標籤()
        {
            InitializeComponent();
            tableLayoutPanel1.BackColor = Color.FromArgb(215, 227, 239);
        }

        private void FrmPrint_F_Load(object sender, EventArgs e)
        {
            Common.取得浮動連線字串(Common.使用者預設公司);
            FaNo.Focus();
        }

        private void btnBrow_Click(object sender, EventArgs e)
        {
            if (FaNo.Text.Trim() != "" && FaNo_1.Text.Trim() != "")
            {
                if (FaNo.Text.Trim()[0] > FaNo_1.Text.Trim()[0])
                {
                    MessageBox.Show("起始廠商不可大於終止廠商，請確定", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    FaNo.Focus();
                    return;
                }
            }
            if (FaX12no.Text.Trim() != "" && FaX12no_1.Text.Trim() != "")
            {
                if (FaX12no.Text.Trim()[0] > FaX12no_1.Text.Trim()[0])
                {
                    MessageBox.Show("起始廠商類別編號不可大於終止廠商類別編號，請確定", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    FaX12no.Focus();
                    return;
                }
            }
            if (FaEmno1.Text.Trim() != "" && FaEmno1_1.Text.Trim() != "")
            {
                if (FaEmno1.Text.Trim()[0] > FaEmno1_1.Text.Trim()[0])
                {
                    MessageBox.Show("起始業務人員編號不可大於終止業務人員編號，請確定", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    FaEmno1.Focus();
                    return;
                }
            }
            if (FaX4no.Text.Trim() != "" && FaX4no_1.Text.Trim() != "")
            {
                if (FaX4no.Text.Trim()[0] > FaX4no_1.Text.Trim()[0])
                {
                    MessageBox.Show("起始結帳類別編號不可大於終止結帳類別編號，請確定", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    FaX4no.Focus();
                    return;
                }
            }
            try
            {
                using (SqlConnection conn = new SqlConnection(Common.浮動連線字串))
                {
                    string str = "select 列印='Y',* from Fact where FaNo >=N'" + FaNo.Text + "'";
                    if (FaNo_1.Text.Trim() != "")
                        str += " and FaNo <= '" + FaNo_1.Text + "'";
                    if (FaX12no.Text.Trim() != "")
                        str += " and FaX12no >= '" + FaX12no.Text + "'";
                    if (FaX12no_1.Text.Trim() != "")
                        str += " and FaX12no <= '" + FaX12no_1.Text + "'";
                    if (FaEmno1.Text.Trim() != "")
                        str += " and FaEmno1 >= '" + FaEmno1.Text + "'";
                    if (FaEmno1_1.Text.Trim() != "")
                        str += " and FaEmno1 <= '" + FaEmno1_1.Text + "'";
                    if (FaR1.Text.Trim() != "")
                        str += " and FaR1 >= '" + FaR1.Text + "'";
                    if (FaR1_1.Text.Trim() != "")
                        str += " and FaR1 <= '" + FaR1_1.Text + "'";
                    if (FaX4no.Text.Trim() != "")
                        str += " and FaX4no >= '" + FaX4no.Text + "'";
                    if (FaX4no_1.Text.Trim() != "")
                        str += " and FaX4no <= '" + FaX4no_1.Text + "'";
                    if (FaName1.Text.Trim() != "")
                        str += " and FaName1 like('" + FaName1.Text.Trim() + "%')";
                    if (FaIme.Text.Trim() != "")
                        str += " and FaIme like('" + FaIme.Text.Trim() + "%')";
                    if (FaTel1.Text.Trim() != "")
                        str += " and FaTel1 like('" + FaTel1.Text.Trim() + "%')";
                    if (FaAddr1.Text.Trim() != "")
                        str += " and FaAddr1 like('%" + FaAddr1.Text.Trim() + "%')";
                    if (FaMemo1.Text.Trim() != "")
                        str += " and FaMemo1 like('%" + FaMemo1.Text.Trim() + "%')";
                    if (FaUdf1.Text.Trim() != "")
                        str += " and FaUdf1 like('%" + FaUdf1.Text.Trim() + "%')";
                    SqlDataAdapter da = new SqlDataAdapter(str, conn);
                    dt.Clear();
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        list = dt.AsEnumerable().ToList();
                        廠商郵遞標籤_明細 frm = new 廠商郵遞標籤_明細();
                        frm.dt = dt;
                        frm.SetParaeter();
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

        private void FaNo_DoubleClick(object sender, EventArgs e)
        {
            CHK.FaNo_OpemFrm(sender as TextBox);
        }

        private void FaX12no_DoubleClick(object sender, EventArgs e)
        {
            CHK.X12No_OpemFrm(sender as TextBox);
        }

        private void FaEmno1_DoubleClick(object sender, EventArgs e)
        {
            CHK.EmNo_OpemFrm(sender as TextBox);
        }

        private void FaX4no_DoubleClick(object sender, EventArgs e)
        {
            CHK.X4No_OpemFrm(sender as TextBox);
        }

        private void FrmPrint_F_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F11) this.Dispose();
        }




    }
}
