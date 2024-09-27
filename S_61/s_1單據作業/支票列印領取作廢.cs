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
    public partial class 支票列印領取作廢 : FormT
    {
        DataTable  DtM= new DataTable();
        DataTable PrintTable = new DataTable();
        List<DataRow> list = new List<DataRow>();
        string path = "";
        string BeforeText;
        string ReportFileName = "";
        string AcName = "";
        string AcAct = "";
        string txttel="";
        string txtaddr = "";
        string txtfax = "";

        bool 是否驗證 = false;
        public 支票列印領取作廢()
        {
            InitializeComponent();
            this.票面金額.DefaultCellStyle.Format = "n" + Common.金額小數;
        }

        private void 支票列印領取作廢_Load(object sender, EventArgs e)
        {
            date.Text = Date.GetDateTime(Common.User_DateTime, false);
            this.開票日.DataPropertyName = Common.User_DateTime == 1 ? "chdate1" : "chdate1_1";
            this.到期日.DataPropertyName = Common.User_DateTime == 1 ? "chdate2" : "chdate2_1";
            ReportFileName = "支票列印領取作廢";
            if (!System.IO.File.Exists(Common.reportaddress + "Report\\" + ReportFileName + "_自訂一.rpt")) radio2.Enabled = false;
            if (!System.IO.File.Exists(Common.reportaddress + "Report\\" + ReportFileName + "_自訂二.rpt")) radio3.Enabled = false;
            if (!System.IO.File.Exists(Common.reportaddress + "Report\\" + ReportFileName + "_自訂三.rpt")) radio4.Enabled = false;
            if (!System.IO.File.Exists(Common.reportaddress + "Report\\" + ReportFileName + "_自訂四.rpt")) radio5.Enabled = false;
            if (!System.IO.File.Exists(Common.reportaddress + "Report\\" + ReportFileName + "_自訂五.rpt")) radio6.Enabled = false;
            ToolTip _Tip = new ToolTip();
            _Tip.SetToolTip(radio1, ReportFileName + "_支票");
            _Tip.SetToolTip(lbludf2, ReportFileName + "_自訂一");
            _Tip.SetToolTip(lbludf3, ReportFileName + "_自訂二");
            _Tip.SetToolTip(lbludf4, ReportFileName + "_自訂三");
            _Tip.SetToolTip(lbludf5, ReportFileName + "_自訂四");
            _Tip.SetToolTip(lbludf6, ReportFileName + "_自訂五");
            groupBoxT1.BackColor = radio1.BackColor = radio2.BackColor = Color.FromArgb(215, 227, 239);         
            dataGridViewT1.SetWidthByPixel();
            dataGridViewT1.AutoGenerateColumns = false;
            dataGridViewT1.ReadOnly = false;
            dataGridViewT1.Columns["支票號碼"].ReadOnly = true;
            dataGridViewT1.Columns["廠商簡稱"].ReadOnly = true;
            dataGridViewT1.Columns["開票日"].ReadOnly = true;
            dataGridViewT1.Columns["到期日"].ReadOnly = true;
            dataGridViewT1.Columns["票面金額"].ReadOnly = true;
            dataGridViewT1.Columns["選擇"].ReadOnly = true;
            if (Common.單據異動 == "2") CoNo.Enabled = false;
            if (Common.User_DateTime == 1) date.MaxLength = 7;
            else date.MaxLength = 8;
            date.Init();
            CoNo.Text = Common.使用者預設公司;
            CHK.GetCoName(CoNo, CoName1);
            date.Text = Date.GetDateTime(Common.User_DateTime, false);
            loadM();
            AcNo.Focus();
            取得公司地址電話傳真();
        }

        RPT paramsInit()
        {
            path = Common.reportaddress + "Report\\" + ReportFileName + "_領款簽回單.rpt";
            RPT rp = new RPT(PrintTable, path);
            //公司抬頭
            rp.lobj.Add(new string[] { "txtstart", Common.dtstart.Rows[0]["coname2"].ToString() });
            //制表日期
            rp.lobj.Add(new string[] { "制表日期", Date.GetDateTime(Common.User_DateTime, true) });
            //公司地址
            rp.lobj.Add(new string[] { "txtaddr", txtaddr });
            //公司電話
            rp.lobj.Add(new string[] { "txttel", txttel });
            //公司傳真
            rp.lobj.Add(new string[] { "txtfax", txtfax });
            //銀行帳戶
            rp.lobj.Add(new string[] { "txtacact", AcAct });
            return rp;
        }
        RPT chparamsInit()
        {
            if (radio1.Checked) path = Common.reportaddress + "Report\\" + ReportFileName + "_支票.rpt";
            if (radio2.Checked) path = Common.reportaddress + "Report\\" + ReportFileName + "_自訂一.rpt";
            if (radio3.Checked) path = Common.reportaddress + "Report\\" + ReportFileName + "_自訂二.rpt";
            if (radio4.Checked) path = Common.reportaddress + "Report\\" + ReportFileName + "_自訂三.rpt";
            if (radio5.Checked) path = Common.reportaddress + "Report\\" + ReportFileName + "_自訂四.rpt";
            if (radio6.Checked) path = Common.reportaddress + "Report\\" + ReportFileName + "_自訂五.rpt";
            RPT rp = new RPT(PrintTable, path);
            return rp;
        }
        void 取得公司地址電話傳真()
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(Common.sqlConnString))
            {
                conn.Open();
                try
                {
                    SqlDataAdapter da = new SqlDataAdapter("select coaddr1,cofax1,cotel1 from comp where cono =N'" + CoNo.Text.ToString().Trim() + "'", conn);
                    string stt = "select coaddr1,cofax1,cotel1 from comp where cono =N'" + CoNo.Text.ToString().Trim() + "'";
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        txtaddr = dt.Rows[0][0].ToString();
                        txtfax = dt.Rows[0][1].ToString();
                        txttel=dt.Rows[0][2].ToString();
                    }
                }
                catch { }
            }
        }

        void loadM()
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(Common.sqlConnString))
                {
                    cn.Open();
                    DtM.Clear();
                    list.Clear();
                    string sql = "select 選擇='',新支票號碼='',* from chko where ChStatus=1 and cono='" + CoNo.Text.Trim() + "'and acno='" + AcNo.Text.Trim() +"' order by chno ";
                    SqlDataAdapter dd = new SqlDataAdapter(sql, cn);
                    dd.Fill(DtM);
                    
                    if (DtM.Rows.Count > 0)
                    {
                        DtM.AsEnumerable().ToList().ForEach(r =>
                        {
                            r["chdate1"] = Date.AddLine(r["chdate1"].ToString().Trim());
                            r["chdate2"] = Date.AddLine(r["chdate2"].ToString().Trim());
                            r["chdate3"] = Date.AddLine(r["chdate3"].ToString().Trim());
                            r["chdate1_1"] = Date.AddLine(r["chdate1_1"].ToString().Trim());
                            r["chdate2_1"] = Date.AddLine(r["chdate2_1"].ToString().Trim());
                            r["chdate3_1"] = Date.AddLine(r["chdate3_1"].ToString().Trim());
                        });
                        list = DtM.AsEnumerable().ToList();
                    }
                    dataGridViewT1.DataSource = null;
                    dataGridViewT1.DataSource = DtM;
                   // dataGridViewT1["票面金額"].DataFormatString = "{0:N}";
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
                    i.Cells["選擇"].Value = "V";
                    dataGridViewT1.CommitEdit(DataGridViewDataErrorContexts.Commit);
                }
                if (i.Cells["選擇"].Value.ToString().Trim() == "V")
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
                    i.Cells["選擇"].Value = "V";
                    dataGridViewT1.CommitEdit(DataGridViewDataErrorContexts.Commit);
                }
                if (i.Cells["選擇"].Value.ToString().Trim() == "V")
                    ++d;
            }
            Count.Text = d.ToString();
        }

        private void select4_Click(object sender, EventArgs e)
        {
            if (dataGridViewT1.Rows.Count == 0) return;
            foreach (DataGridViewRow i in dataGridViewT1.Rows)
            {
                    i.Cells["選擇"].Value = "";
            }
            Count.Text = "0";
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Dispose();
            this.Close();
        }

        private void btnInvalid_Click(object sender, EventArgs e)
        {
            if (AcNo.Text.Trim() == "")
            {
                MessageBox.Show("『開票帳戶』不可為空白，請確定後重新輸入", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                AcNo.Focus();
                return;
            }
            if (Count.Text.Trim() == "0")
            {
                MessageBox.Show("尚未選擇票據", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (MessageBox.Show("確定是否作廢票據", "訊息視窗", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1) == DialogResult.Cancel) return;
            var temp = DtM.AsEnumerable().ToList().Where(r => r["選擇"].ToString().Trim() == "V");
            DataTable SelectTable = temp.CopyToDataTable();
            try
            {
                using (SqlConnection cn = new SqlConnection(Common.sqlConnString))
                {
                    cn.Open();
                    SqlCommand cmd = cn.CreateCommand();
                    
                    for (int i = 0; i < SelectTable.Rows.Count; i++)
                    {
                        
                        cmd.CommandText +="update chko set"
                                        + " chstatus=5,"
                                        + " chstname=N'作    廢',"
                                        + " acno=N'" + AcNo.Text.Trim() + "',"
                                        + " acname1=N'" + AcName1.Text.Trim() + "',"
                                        + " chdate=N'" + Date.ToTWDate(date.Text.Trim()) + "',"
                                        + " chdate_1=N'" + Date.ToUSDate(date.Text.Trim()) + "'"
                                        + " where chno='" + SelectTable.Rows[i]["chno"].ToString().Trim() + "';";
                    }
                    
                    cmd.ExecuteNonQuery();
                    dataGridViewT1.DataSource = null;
                    temp = DtM.AsEnumerable().ToList().Where(r => r["選擇"].ToString().Trim() == "");
                    if (temp.Count() > 0)
                    {
                        DtM = temp.CopyToDataTable();
                        dataGridViewT1.DataSource = DtM;
                    }
                    Count.Text = "0";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void btnGet_Click(object sender, EventArgs e)
        {
            if (AcNo.Text.Trim() == "")
            {
                MessageBox.Show("『開票帳戶』不可為空白，請確定後重新輸入", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                AcNo.Focus();
                return;
            }
            if (Count.Text.Trim() == "0")
            {
                MessageBox.Show("尚未選擇票據", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (MessageBox.Show("確定是否領取票據", "訊息視窗", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1) == DialogResult.Cancel) return;
            var temp = DtM.AsEnumerable().ToList().Where(r => r["選擇"].ToString().Trim() == "V");
            DataTable SelectTable = temp.CopyToDataTable();
            try
            {
                using (SqlConnection cn = new SqlConnection(Common.sqlConnString))
                {
                    cn.Open();
                    SqlCommand cmd = cn.CreateCommand();
                    for (int i = 0; i < SelectTable.Rows.Count; i++)
                    {
                        cmd.CommandText += "update chko set"
                                        + " chstatus=2,"
                                        + " chstname=N'票已領取',"
                                        + " acno=N'" + AcNo.Text.Trim() + "',"
                                        + " acname1=N'" + AcName1.Text.Trim() + "',"
                                        + " chdate=N'" + Date.ToTWDate(date.Text.Trim()) + "',"
                                        + " chdate_1=N'" + Date.ToUSDate(date.Text.Trim()) + "'"
                                        + " where chno='" + SelectTable.Rows[i]["chno"].ToString().Trim() + "';";
                    }
                    cmd.ExecuteNonQuery();
                    dataGridViewT1.DataSource = null;
                    temp = DtM.AsEnumerable().ToList().Where(r => r["選擇"].ToString().Trim() == "");
                    if (temp.Count() > 0)
                    {
                        DtM = temp.CopyToDataTable();
                        dataGridViewT1.DataSource = DtM;
                    }
                    Count.Text = "0";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void AcNo_DoubleClick(object sender, EventArgs e)
        {
            CHK.AcNo_OpemFrm(CoNo.Text.Trim(), AcNo, AcName1, null, true, true);
            loadM();
        }

        private void CoNo_DoubleClick(object sender, EventArgs e)
        {
            CHK.CoNo_OpemFrm(CoNo, CoName1);
        }

        private void CoNo_Enter(object sender, EventArgs e)
        {
            if (CoNo.ReadOnly) return;
            BeforeText = CoNo.Text.Trim();
        }

        private void CoNo_Validating(object sender, CancelEventArgs e)
        {
            if (CoNo.ReadOnly || btnCancel.Focused) return;
            if (CoNo.Text.Trim() == "")
            {
                MessageBox.Show("『公司編號』不可為空白，請確定後重新輸入", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                if (!CHK.AcNo_Validating(CoNo.Text.Trim(), AcNo, AcName1, null, true))
                    AcNo.Text = AcName1.Text = "";
                loadM();
            }
            取得公司地址電話傳真();
        }

        private void AcNo_Validating(object sender, CancelEventArgs e)
        {
            if (AcNo.ReadOnly || btnCancel.Focused) return;
            if (AcNo.Text.Trim() == "")
            {
                AcName1.Text = "";
                AcName = AcAct = "";
                MessageBox.Show("『開票帳戶』不可為空白，請確定後重新輸入", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                e.Cancel = true;
                return;
            }
            if (!CHK.AcNo_Validating(CoNo.Text.Trim(), AcNo, AcName1, null, true))
            {
                e.Cancel = true;
                CHK.AcNo_OpemFrm(CoNo.Text.Trim(), AcNo, AcName1, null, true, true);
            }
            else
            {
                loadM();
                using (SqlConnection cn = new SqlConnection(Common.sqlConnString))
                {
                    cn.Open();
                    SqlCommand cmd = cn.CreateCommand();
                    cmd.CommandText = "select acact from acct where acno=N'" + AcNo.Text.Trim() + "'";
                    if(!cmd.ExecuteScalar().IsNullOrEmpty())AcAct = cmd.ExecuteScalar().ToString();
                }
            }
        }

        private void date_Validating(object sender, CancelEventArgs e)
        {
            if (btnCancel.Focused) return;
            if (date.ReadOnly) return;
            if (date.Text.Trim() == "")
            {
                MessageBox.Show("『異動日期』不可為空白，請確定後重新輸入", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
            if (dataGridViewT1.CurrentCell.OwningColumn.Name != "選擇") return;

            if (dataGridViewT1["選擇", e.RowIndex].Value.ToString().Trim() == "V")
                dataGridViewT1["選擇", e.RowIndex].Value = "";
            else
                dataGridViewT1["選擇", e.RowIndex].Value = "V";
            dataGridViewT1.CommitEdit(DataGridViewDataErrorContexts.Commit);
            Count.Text = DtM.AsEnumerable().ToList().Where(r => r["選擇"].ToString().Trim() == "V").Count().ToString();
        }

        private void select6_Click(object sender, EventArgs e)
        {
            if (startno.Text.ToString() == "")
            {
                MessageBox.Show("支票開始編號不能為空白，請修正後再執行此功能", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                startno.Focus();
                return;
            }
            if (AcWord.Text.ToString() == "")
            {
                MessageBox.Show("支票字軌不能為空白，請修正後再執行此功能", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                AcWord.Focus();
                return;
            }
            decimal dstart = startno.Text.ToDecimal();
            try
            {
                foreach (DataRow i in DtM.Rows)
                {
                    i["新支票號碼"] = AcWord.Text.Trim() + dstart;
                    ++dstart;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString()); ;
            }
        }

        private void select5_Click(object sender, EventArgs e)
        {
            if (startno.Text.ToString() == "" )
            {
                MessageBox.Show("支票開始編號不能為空白，請修正後再執行此功能", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                startno.Focus();
                return;
            }
            if (AcWord.Text.ToString() == "")
            {
                MessageBox.Show("支票字軌不能為空白，請修正後再執行此功能", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                AcWord.Focus();
                return;
            }
            decimal dstart = startno.Text.ToDecimal();
            try
            {
                foreach (DataRow i in DtM.Rows)
                {
                    if (i["選擇"].ToString().Trim() == "V")
                    {
                        i["新支票號碼"] = AcWord.Text.Trim()+ dstart;
                        ++dstart;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString()); ;
            }
        }

        private void btnChangeNo_Click(object sender, EventArgs e)
        {
            if (AcNo.Text.Trim() == "")
            {
                MessageBox.Show("『開票帳戶』不可為空白，請確定後重新輸入", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                AcNo.Focus();
                return;
            }
            if (MessageBox.Show("請確定是否更變", "訊息視窗", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1) == DialogResult.Cancel) return;
            DataTable SelectTable = new DataTable();
            if (DtM.AsEnumerable().ToList().Count(r => r["新支票號碼"].ToString().Trim() != "") == 0)
            {
                MessageBox.Show("尚未輸入支票號碼", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else
            {
                SelectTable = DtM.AsEnumerable().ToList().Where(r => r["新支票號碼"].ToString().Trim() != "").CopyToDataTable();
            }

            try
            {
                using (SqlConnection cn = new SqlConnection(Common.sqlConnString))
                {
                    cn.Open();
                    SqlCommand cmd = cn.CreateCommand();
                    for (int i = 0; i < SelectTable.Rows.Count; i++)
                    {
                        cmd.CommandText = "select chno from chko where chno=N'" + SelectTable.Rows[i]["新支票號碼"].ToString().Trim() + "'";
                        if (cmd.ExecuteScalar().IsNotNull())
                        {
                            MessageBox.Show("此支票號碼："+SelectTable.Rows[i]["新支票號碼"].ToString().Trim()+"已使用");
                            return;
                        }
                    }
                }
                using (SqlConnection cn = new SqlConnection(Common.sqlConnString))
                {
                    cn.Open();
                    SqlCommand cmd = cn.CreateCommand();
                    for (int i = 0; i < SelectTable.Rows.Count; i++)
                    {
                        cmd.CommandText += "update chko set"
                                        + " chno=N'" + SelectTable.Rows[i]["新支票號碼"].ToString().Trim() + "',"
                                        + " acno=N'" + AcNo.Text.Trim() + "',"
                                        + " acname1=N'" + AcName1.Text.Trim() + "',"
                                        + " chdate=N'" + Date.ToTWDate(date.Text.Trim()) + "',"
                                        + " chdate_1=N'" + Date.ToUSDate(date.Text.Trim()) + "'"
                                        + " where chno='" + SelectTable.Rows[i]["chno"].ToString().Trim() + "';";
                    }
                    cmd.ExecuteNonQuery();
                    //dataGridViewT1.DataSource = null;
                    //temp = DtM.AsEnumerable().ToList().Where(r => r["新支票號碼"].ToString().Trim() == "");
                    //if (temp.Count() > 0)
                    //{
                    //    DtM = temp.CopyToDataTable();
                    //    dataGridViewT1.DataSource = DtM;
                    //}
                    cmd.Dispose();
                    loadM();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void btnSignPreview_Click(object sender, EventArgs e)
        {
            if (AcNo.Text.Trim() == "")
            {
                MessageBox.Show("『開票帳戶』不可為空白，請確定後重新輸入", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                AcNo.Focus();
                return;
            }
            if (Count.Text.Trim() == "0")
            {
                MessageBox.Show("尚未選擇票據", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            PrintTable.Clear();
            var temp = DtM.AsEnumerable().ToList().Where(r => r["選擇"].ToString().Trim() == "V");
            PrintTable = temp.CopyToDataTable();
            paramsInit().PreView();
        }

        private void btnSignPrint_Click(object sender, EventArgs e)
        {
            if (AcNo.Text.Trim() == "")
            {
                MessageBox.Show("『開戶帳戶』不可為空白，請確定後重新輸入", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                AcNo.Focus();
                return;
            }
            if (Count.Text.Trim() == "0")
            {
                MessageBox.Show("尚未選擇票據", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            PrintTable.Clear();
            var temp = DtM.AsEnumerable().ToList().Where(r => r["選擇"].ToString().Trim() == "V");
            PrintTable = temp.CopyToDataTable();
            paramsInit().Print();
        }

        private void btnCheckPreview_Click(object sender, EventArgs e)
        {
            if (AcNo.Text.Trim() == "")
            {
                MessageBox.Show("『開票帳戶』不可為空白，請確定後重新輸入", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                AcNo.Focus();
                return;
            }
            if (Count.Text.Trim() == "0")
            {
                MessageBox.Show("尚未選擇票據", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            PrintTable.Clear();
            var temp = DtM.AsEnumerable().ToList().Where(r => r["選擇"].ToString().Trim() == "V");
            PrintTable = temp.CopyToDataTable();
            chparamsInit().PreView();
        }

        private void btnCheckPrint_Click(object sender, EventArgs e)
        {
            if (AcNo.Text.Trim() == "")
            {
                MessageBox.Show("『開票帳戶』不可為空白，請確定後重新輸入", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                AcNo.Focus();
                return;
            }
            if (Count.Text.Trim() == "0")
            {
                MessageBox.Show("尚未選擇票據", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            PrintTable.Clear();
            var temp = DtM.AsEnumerable().ToList().Where(r => r["選擇"].ToString().Trim() == "V");
            PrintTable = temp.CopyToDataTable();
            chparamsInit().Print();
        }

        private void 支票列印領取作廢_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.D0:
                case Keys.F11:
                    btnCancel.PerformClick();
                    break;
                case Keys.F2:
                    if (e.Modifiers == Keys.Shift)
                    {
                        btnChangeNo.PerformClick();
                        break;
                    }
                    select2.PerformClick();
                    break;
                case Keys.F3:
                    if (e.Modifiers == Keys.Shift)
                    {
                        btnCheckPrint.PerformClick();
                        break;
                    }
                    select3.PerformClick();
                    break;
                case Keys.F4:
                    if (e.Modifiers == Keys.Shift)
                    {
                        btnCheckPreview.PerformClick();
                        break;
                    }
                    select4.PerformClick();
                    break;
                case Keys.F5:
                    if (e.Modifiers == Keys.Shift)
                    {
                        btnSignPrint.PerformClick();
                        break;
                    }
                    select5.PerformClick();
                    break;
                case Keys.F6:
                    if (e.Modifiers == Keys.Shift)
                    {
                        btnSignPreview.PerformClick();
                        break;
                    }
                    select6.PerformClick();
                    break;
                case Keys.F7:
                    if (e.Modifiers == Keys.Shift)
                    {
                        btnInvalid.PerformClick();
                        break;
                    }
                    break;
                case Keys.F9:
                    btnGet.PerformClick();
                    break;
            }
        }

        private void lbludf6_Click(object sender, EventArgs e)
        {
            lblT lb = sender as lblT;
            if (lb.Name == "lbludf2")
                if (radio2.Enabled) radio2.Checked = true;
            if (lb.Name == "lbludf3")
                if (radio3.Enabled) radio3.Checked = true;
            if (lb.Name == "lbludf4")
                if (radio4.Enabled) radio4.Checked = true;
            if (lb.Name == "lbludf5")
                if (radio5.Enabled) radio5.Checked = true;
            if (lb.Name == "lbludf6")
                if (radio6.Enabled) radio6.Checked = true;
        }
    }
}
