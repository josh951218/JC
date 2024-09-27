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
    public partial class 應收票據批次託收 : FormT
    {
        DataTable tbM = new DataTable();
        DataTable PrintTable = new DataTable();
        List<DataRow> list = new List<DataRow>();
        string path = "";
        string BeforeText;
        string ReportFileName = "";
        string AcName = "";
        string AcAct = "";
        bool 是否驗證 = false;
        public 應收票據批次託收()
        {
            InitializeComponent();
            this.票面金額.DefaultCellStyle.Format = "N" + Common.金額小數;
        }

        private void 應收票據批次託收_Load(object sender, EventArgs e)
        {
            if (Common.單據異動 == "2") CoNo.Enabled = false;
            ReportFileName = "應收票據批次託收";
            if (!System.IO.File.Exists(Common.reportaddress + "Report\\" + ReportFileName + "_自訂一.rpt")) radio2.Enabled = false;
            ToolTip _Tip = new ToolTip();
            _Tip.SetToolTip(lbludf2, ReportFileName + "_自訂一");

            單行註腳.BackColor = groupBoxT1.BackColor = radio1.BackColor = radio2.BackColor = Color.FromArgb(215, 227, 239);
            dataGridViewT1.SetWidthByPixel();
            dataGridViewT1.AutoGenerateColumns = false;
            if (Common.User_DateTime == 1) date.MaxLength = 7;
            else date.MaxLength = 8;
            date.Init();
            CoNo.Text = Common.使用者預設公司;
            CHK.GetCoName(CoNo, CoName1);
            date.Text = Date.GetDateTime(Common.User_DateTime, false);
            this.收票日.DataPropertyName = Common.User_DateTime == 1 ? "chdate1" : "chdate1_1";
            this.到期日.DataPropertyName = Common.User_DateTime == 1 ? "chdate2" : "chdate2_1";
            this.預兌日.DataPropertyName = Common.User_DateTime == 1 ? "chdate3" : "chdate3_1";
            loadM();
            AcNo.Focus();
        }

        RPT paramsInit()
        {
            if (radio1.Checked) path = Common.reportaddress + "Report\\" + ReportFileName + "_標準表.rpt";
            if (radio2.Checked) path = Common.reportaddress + "Report\\" + ReportFileName + "_自訂一.rpt";
            RPT rp = new RPT(PrintTable, path);
            rp.office = this.Tag.ToString();
            //公司抬頭
            rp.lobj.Add(new string[] { "txtstart", CHK.GetCoName2(Common.使用者預設公司) });
            //制表日期
            rp.lobj.Add(new string[] { "制表日期", Date.GetDateTime(Common.User_DateTime,true)});
            //託收日期
            rp.lobj.Add(new string[] { "託收日期", Date.AddLine(date.Text.ToString().Trim())});
            //託收帳戶
            rp.lobj.Add(new string[] { "託收帳戶", AcNo.Text.Trim()+" "+AcName1.Text });
            //銀行帳號
            using (SqlConnection cn  = new SqlConnection(Common.sqlConnString))
            {
                cn.Open();
                SqlCommand cmd = cn.CreateCommand();
                cmd.CommandText = "select AcAct from acct where cono=N'" + CoNo.Text.Trim() + "' and acno=N'" + AcNo.Text.Trim() + "'";
                AcAct = cmd.ExecuteScalar().ToString();
            }
            rp.lobj.Add(new string[] { "銀行帳號", AcAct});
            //託收張數
            rp.lobj.Add(new string[] { "託收張數", Count.Text.Trim() });
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

        void loadM()
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(Common.sqlConnString))
                {
                    cn.Open();
                    tbM.Clear();
                    list.Clear();
                    string sql = "select 託收='',* from chki where ChStatus=1 and cono='"+CoNo.Text.Trim()+"' order by chdate2 ";
                    SqlDataAdapter dd = new SqlDataAdapter(sql, cn);
                    dd.Fill(tbM);
                    if (tbM.Rows.Count > 0)
                    {
                        tbM.AsEnumerable().ToList().ForEach(r =>
                        {
                            r["chdate1"] = Date.AddLine(r["chdate1"].ToString().Trim());
                            r["chdate2"] = Date.AddLine(r["chdate2"].ToString().Trim());
                            r["chdate3"] = Date.AddLine(r["chdate3"].ToString().Trim());
                            r["chdate1_1"] = Date.AddLine(r["chdate1_1"].ToString().Trim());
                            r["chdate2_1"] = Date.AddLine(r["chdate2_1"].ToString().Trim());
                            r["chdate3_1"] = Date.AddLine(r["chdate3_1"].ToString().Trim());
                        });
                        list = tbM.AsEnumerable().ToList();
                    }
                    dataGridViewT1.DataSource = null;
                    dataGridViewT1.DataSource = tbM;
                    Count.Text = "0";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void select2_Click(object sender, EventArgs e)
        {
            if (dataGridViewT1.Rows.Count == 0) return;
            int index = dataGridViewT1.SelectedRows[0].Index;
            decimal d = 0;
            foreach (DataGridViewRow i in dataGridViewT1.Rows)
            {
                if (i.Index >= index)
                {
                    i.Cells["託收"].Value = "V";
                    dataGridViewT1.CommitEdit(DataGridViewDataErrorContexts.Commit);
                }
                if (i.Cells["託收"].Value.ToString().Trim() == "V")
                    ++d;
            }
            Count.Text = d.ToString();
        }

        private void select3_Click(object sender, EventArgs e)
        {
            if (dataGridViewT1.Rows.Count == 0) return;
            int index = dataGridViewT1.SelectedRows[0].Index;
            decimal d = 0;
            foreach (DataGridViewRow i in dataGridViewT1.Rows)
            {
                if (i.Index <= index)
                {
                    i.Cells["託收"].Value = "V";
                    dataGridViewT1.CommitEdit(DataGridViewDataErrorContexts.Commit);
                }
                if (i.Cells["託收"].Value.ToString().Trim() == "V")
                    ++d;
            }
            Count.Text = d.ToString();
            
        }

        private void select4_Click(object sender, EventArgs e)
        {
            if (dataGridViewT1.Rows.Count == 0) return;
            foreach (DataGridViewRow i in dataGridViewT1.Rows)
            {
                 i.Cells["託收"].Value = "";
            }
            Count.Text = "0";
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (AcNo.Text.Trim() == "")
            {
                MessageBox.Show("『託收帳戶』不可為空白，請確定後重新輸入", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                AcNo.Focus();
                return;
            }
            if (Count.Text.Trim() == "0")
            {
                MessageBox.Show("尚未選擇託收", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            PrintTable.Clear();
            var temp = tbM.AsEnumerable().ToList().Where(r => r["託收"].ToString().Trim() == "V");
            PrintTable = temp.CopyToDataTable();
            paramsInit().Print();
        }

        private void btnPreview_Click(object sender, EventArgs e)
        {
            if (AcNo.Text.Trim() == "")
            {
                MessageBox.Show("『託收帳戶』不可為空白，請確定後重新輸入", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                AcNo.Focus();
                return;
            }
            if (Count.Text.Trim() == "0")
            {
                MessageBox.Show("尚未選擇託收", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            PrintTable.Clear();
            var temp = tbM.AsEnumerable().ToList().Where(r => r["託收"].ToString().Trim() == "V");
            PrintTable = temp.CopyToDataTable();
            paramsInit().PreView();
        }

        private void btnGet_Click(object sender, EventArgs e)
        {
            if (AcNo.Text.Trim() == "")
            {
                MessageBox.Show("『託收帳戶』不可為空白，請確定後重新輸入", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                AcNo.Focus();
                return;
            }
            if (Count.Text.Trim() == "0")
            {
                MessageBox.Show("尚未選擇託收", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (MessageBox.Show("確定是否託收票據", "訊息視窗", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1) == DialogResult.Cancel)return;
            var temp = tbM.AsEnumerable().ToList().Where(r => r["託收"].ToString().Trim() == "V");
            DataTable SelectTable = temp.CopyToDataTable();
            try
            {
                using (SqlConnection cn = new SqlConnection(Common.sqlConnString))
                {
                    cn.Open();
                    SqlCommand cmd = cn.CreateCommand();
                    for (int i = 0; i < SelectTable.Rows.Count; i++)
                    {
                        cmd.CommandText += "update chki set"
                                        + " chstatus=2,"
                                        + " chstname=N'託    收',"
                                        + " acno=N'" + AcNo.Text.Trim() + "',"
                                        + " acname1=N'" + AcName1.Text.Trim() + "',"
                                        + " chdate=N'" + Date.ToTWDate(date.Text.Trim()) + "',"
                                        + " chdate_1=N'" + Date.ToUSDate(date.Text.Trim()) + "'"
                                        + " where chno=N'" + SelectTable.Rows[i]["chno"].ToString().Trim() + "';";
                    }
                    cmd.ExecuteNonQuery();
                    dataGridViewT1.DataSource = null;
                    temp = tbM.AsEnumerable().ToList().Where(r => r["託收"].ToString().Trim() == "");
                    if (temp.Count() > 0)
                    {
                        tbM = temp.CopyToDataTable();
                        dataGridViewT1.DataSource = tbM;
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

        private void CoNo_Enter(object sender, EventArgs e)
        {
            if (CoNo.ReadOnly) return;
            BeforeText = CoNo.Text.Trim();
        }

        private void CoNo_DoubleClick(object sender, EventArgs e)
        {
            CHK.CoNo_OpemFrm(CoNo, CoName1);
        }

        private void CoNo_Validating(object sender, CancelEventArgs e)
        {
            if (CoNo.ReadOnly || btnExit.Focused) return;
            if (CoNo.Text.Trim() == "")
            {
                MessageBox.Show("『託收帳戶』不可為空白，請確定後重新輸入", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                e.Cancel = true;
                return;
            }
            if (CoNo.Text.Trim() == BeforeText && !是否驗證) return;
            if (!CHK.CoNo_Validating(CoNo, CoName1))
            {
                e.Cancel = true;
                是否驗證 = true;
                CHK.CoNo_OpemFrm(CoNo, CoName1);
            }
            else
            {
                是否驗證 = false;
                loadM();
                if (!CHK.AcNo_Validating(CoNo.Text.Trim(), AcNo, AcName1, null, true))
                {
                    AcNo.Text = AcName1.Text = "";
                }
            }
        }

        private void AcNo_DoubleClick(object sender, EventArgs e)
        {
            CHK.AcNo_OpemFrm(CoNo.Text.Trim(), AcNo, AcName1, null, true, true);
        }

        private void AcNo_Validating(object sender, CancelEventArgs e)
        {
            if (AcNo.ReadOnly || btnExit.Focused) return;
            if (AcNo.Text.Trim() == "")
            {
                AcName1.Text = "";
                MessageBox.Show("『託收帳戶』不可為空白，請確定後重新輸入", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                e.Cancel = true;
                return ;
            }
            if (!CHK.AcNo_Validating(CoNo.Text.Trim(), AcNo, AcName1, null, true))
            {
                e.Cancel = true;
                CHK.AcNo_OpemFrm(CoNo.Text.Trim(), AcNo, AcName1, null, true, true);
            }
        }

        private void date_Validating(object sender, CancelEventArgs e)
        {
            if (btnExit.Focused) return;
            if (date.ReadOnly) return;
            if (date.Text.Trim() == "")
            {
                MessageBox.Show("『託收日期』不可為空白，請確定後重新輸入", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                e.Cancel = true;
                return;
            }
            if (!date.IsDateTime())
            {
                MessageBox.Show("日期格式錯誤，請確定後重新輸入", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                e.Cancel = true;
                return;
            }
            if (!CHK.稽核會計年度(date.Text.Trim())) e.Cancel = true;
        }

        private void dataGridViewT1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex > dataGridViewT1.Rows.Count - 1) return;
            if (dataGridViewT1.CurrentCell.OwningColumn.Name != "託收") return;
            if (dataGridViewT1["託收", e.RowIndex].Value.ToString().Trim() == "V")
                dataGridViewT1["託收", e.RowIndex].Value = "";
            else
                dataGridViewT1["託收", e.RowIndex].Value = "V";
            dataGridViewT1.CommitEdit(DataGridViewDataErrorContexts.Commit);
            Count.Text = tbM.AsEnumerable().ToList().Where(r => r["託收"].ToString().Trim() == "V").Count().ToString();
        }

        private void lbludf2_Click(object sender, EventArgs e)
        {
            if (radio2.Enabled)
            {
                lblT lbla = sender as lblT;
                if (lbla.Name == lbludf2.Name)
                    radio2.Checked = true;
            }
        }

        private void 應收票據批次託收_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.D0:
                case Keys.F11:
                    btnExit.PerformClick();
                    break;
                case Keys.F2:
                    if (e.Modifiers == Keys.Shift)
                    {
                        select2.PerformClick();
                        break;
                    }
                    btnPrint.PerformClick();
                    break;
                case Keys.F3:
                    if (e.Modifiers == Keys.Shift)
                    {
                        select3.PerformClick();
                        break;
                    }
                    btnPreview.PerformClick();
                    break;
                case Keys.F4:
                    if (e.Modifiers == Keys.Shift)
                    {
                        select4.PerformClick();
                        break;
                    }
                    btnGet.PerformClick();
                    break;
                case Keys.F10:
                    btnExit.PerformClick();
                    break;

            }
        }


    }
}
