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
    public partial class 銀行存提款作業_瀏覽 : FormT
    {
        public string Result { get; set; }
        DataTable table = new DataTable();
        List<DataRow> list = new List<DataRow>();
        DataRow dr;
        List<Button> query;
        string NO = "";
        public bool 開窗模式 = false;

        public 銀行存提款作業_瀏覽()
        {
            InitializeComponent();
            query = new List<Button> {  query3, query4, query5, query6, query7 };
            this.金額.DefaultCellStyle.Format = "N" + Common.金額小數;
            count.Text = "";
            if (Common.User_DateTime == 1) qDate.MaxLength = 7;
            else qDate.MaxLength = 8;
            qDate.Init();
        }

        private void 銀行存提款作業_瀏覽_Load(object sender, EventArgs e)
        {
            dataGridViewT1.AutoGenerateColumns = false;
            dataGridViewT1.SetWidthByPixel();
            if (開窗模式) tableLayoutPnl2.Visible =  false;
            loadM("LoNo");
            if (list.Count > 0)
            {
                dr = list.Find(r => r["LoNo"].ToString().Trim() == SeekNo);
                if (dr == null) dr = list.Last();
                NO = dr["序號"].ToString().Trim();
            }
            
            SetButtonColor();
            SetSelectRow(NO);
            query3.ForeColor = Color.Red;
            qCoNo.Focus();
        }

        private void loadM(string str)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(Common.sqlConnString))
                {
                    cn.Open();
                    string sql = "select 日期='',序號='',* from lodgm order by " + str;
                    SqlDataAdapter dd = new SqlDataAdapter(sql, cn);
                    table.Clear(); list.Clear();
                    dd.Fill(table);
                    if (table.Rows.Count > 0)
                    {
                        for (int i = 0; i < table.Rows.Count; i++)
                        {
                            table.Rows[i]["序號"] = i;
                            if(Common.User_DateTime == 1)
                                table.Rows[i]["日期"] = Date.AddLine(table.Rows[i]["lodate"].ToString());
                            else
                                table.Rows[i]["日期"] = Date.AddLine(table.Rows[i]["lodate_1"].ToString());
                        }
                        list = table.AsEnumerable().ToList();
                    }
                    dataGridViewT1.DataSource = null;
                    dataGridViewT1.DataSource = table;
                    count.Text = "總共有 " + table.Rows.Count + " 筆資料";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        void SetButtonColor()
        {
            query.ForEach(r => r.ForeColor = SystemColors.ControlText);
        }

        void SetSelectRow(string NO)
        {
            for (int i = 0; i < dataGridViewT1.Rows.Count; i++)
            {
                if (dataGridViewT1["序號", i].Value.ToString() == NO)
                {
                    dataGridViewT1.FirstDisplayedScrollingRowIndex = i;
                    dataGridViewT1.Rows[i].Selected = true;
                    break;
                }
            }
        }

        private void query3_Click(object sender, EventArgs e)
        {
            NO = dataGridViewT1.SelectedRows[0].Cells["序號"].Value.ToString();
            table.DefaultView.Sort = "lono";
            SetButtonColor();
            query3.ForeColor = Color.Red;
            SetSelectRow(NO);
        }

        private void query4_Click(object sender, EventArgs e)
        {
            NO = dataGridViewT1.SelectedRows[0].Cells["序號"].Value.ToString();
            table.DefaultView.Sort = "lodate desc";
            SetButtonColor();
            query4.ForeColor = Color.Red;
            SetSelectRow(NO);
        }

        private void query5_Click(object sender, EventArgs e)
        {
            NO = dataGridViewT1.SelectedRows[0].Cells["序號"].Value.ToString();
            table.DefaultView.Sort = "acno";
            SetButtonColor();
            query5.ForeColor = Color.Red;
            SetSelectRow(NO);
        }

        private void query6_Click(object sender, EventArgs e)
        {
            NO = dataGridViewT1.SelectedRows[0].Cells["序號"].Value.ToString();
            table.DefaultView.Sort = "lomemo";
            SetButtonColor();
            query6.ForeColor = Color.Red;
            SetSelectRow(NO);
        }

        private void query7_Click(object sender, EventArgs e)
        {
            NO = dataGridViewT1.SelectedRows[0].Cells["序號"].Value.ToString();
            table.DefaultView.Sort = "lokind";
            SetButtonColor();
            query7.ForeColor = Color.Red;
            SetSelectRow(NO);
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            btnQuery.Enabled = false;
            if (qCoNo.Text.Trim() != "")
            {
                loadM("CoNo");
                dataGridViewT1.Search("cono", qCoNo.Text);
                SetButtonColor();
                btnQuery.Enabled = true;
                btnQuery.Focus();
                return;
            }
            if (qDate.Text.Trim() != "")
            {
                loadM("lodate desc");
                if(Common.User_DateTime == 1)
                    dataGridViewT1.SearchForDate("lodate", qDate.Text);
                else
                    dataGridViewT1.SearchForDate("lodate_1", qDate.Text);
                SetButtonColor();
                query4.ForeColor = Color.Red;
                btnQuery.Enabled = true;
                btnQuery.Focus();
                return;
            }
            if (qAcNo.Text.Trim() != "")
            {
                loadM("acno");
                dataGridViewT1.Search("acno", qAcNo.Text);
                SetButtonColor();
                query5.ForeColor = Color.Red;
                btnQuery.Enabled = true;
                btnQuery.Focus();
                return;
            }
            if (qLoNo.Text.Trim() != "")
            {
                loadM("LoNo");
                dataGridViewT1.Search("存提證號", qLoNo.Text);
                SetButtonColor();
                query3.ForeColor = Color.Red;
                btnQuery.Enabled = true;
                btnQuery.Focus();
                return;
            }
            if (qMemo.Text.Trim() != "")
            {
                loadM("LoMemo");
                dataGridViewT1.Search("備註", qMemo.Text);
                SetButtonColor();
                query7.ForeColor = Color.Red;
                btnQuery.Enabled = true;
                btnQuery.Focus();
                return;
            }
            btnQuery.Enabled = true;
            btnQuery.Focus();
        }

        private void btnGet_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            if (dataGridViewT1.SelectedRows.Count > 0)
                Result = dataGridViewT1.SelectedRows[0].Cells["存提證號"].Value.ToString().Trim();
            this.Close();
            this.Dispose();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
            this.Dispose();
        }

        private void 銀行存提款作業_瀏覽_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.F3:
                    if (e.Modifiers == Keys.Shift)
                    {
                        query3.Focus();
                        query3.PerformClick();
                        break;
                    }
                    btnQuery.Focus();
                    btnQuery.PerformClick();
                    break;
                case Keys.F4:
                    if (e.Modifiers == Keys.Shift)
                    {
                        query4.Focus();
                        query4.PerformClick();
                        break;
                    }
                    break;
                case Keys.F5:
                    if (e.Modifiers == Keys.Shift)
                    {
                        query5.Focus();
                        query5.PerformClick();
                        break;
                    }
                    btnQuery.Focus();
                    btnQuery.PerformClick();
                    break;
                case Keys.F6:
                    if (e.Modifiers == Keys.Shift)
                    {
                        query6.Focus();
                        query6.PerformClick();
                        break;
                    }
                    break;
                case Keys.F7:
                    if (e.Modifiers == Keys.Shift)
                    {
                        query7.Focus();
                        query7.PerformClick();
                        break;
                    }
                    break;
                case Keys.F9:
                    btnGet.Focus();
                    btnGet.PerformClick();
                    break;
                case Keys.F11:
                    btnExit.Focus();
                    btnExit.PerformClick();
                    break;
            }
        }

        private void dataGridViewT1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (!開窗模式) return;
            this.DialogResult = DialogResult.OK;
            if (dataGridViewT1.SelectedRows.Count > 0)
                Result = dataGridViewT1.SelectedRows[0].Cells["存提證號"].Value.ToString().Trim();
            this.Close();
            this.Dispose();
        }

        private void qCoNo_DoubleClick(object sender, EventArgs e)
        {
            CHK.CoNo_OpemFrm(qCoNo);
        }

        private void qAcNo_DoubleClick(object sender, EventArgs e)
        {
            CHK.AcNo_OpemFrm("",qAcNo,null,null,false,false);
        }

        private void qMemo_DoubleClick(object sender, EventArgs e)
        {
            CHK.Memo_OpemFrm(qMemo);
        }

        private void qDate_Validating(object sender, CancelEventArgs e)
        {
            if (btnExit.Focused) return;
            if (qDate.Text.Trim() == "") return;
            if (!qDate.IsDateTime())
            {
                MessageBox.Show("日期格式錯誤，請確定後重新輸入", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                e.Cancel = true;
                return;
            }
        }




    }
}
