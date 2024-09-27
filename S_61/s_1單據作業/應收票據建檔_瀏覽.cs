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
    public partial class 應收票據建檔_瀏覽 : FormT
    {
        public string Result { get; set; }
        DataTable table = new DataTable();
        List<DataRow> list = new List<DataRow>();
        DataRow dr;
        List<Button> query;
        string NO = "";
        public bool 開窗模式 = false;

        public 應收票據建檔_瀏覽()
        {
            InitializeComponent();
            query = new List<Button> { query2, query3, query4, query5, query6, query7, query8, query9, query10 };
            this.票面金額.DefaultCellStyle.Format = "N" + Common.金額小數;
            if (Common.User_DateTime == 1) qChDate.MaxLength = qChDate1.MaxLength = qChDate2.MaxLength = qChDate3.MaxLength = 7;
            else qChDate.MaxLength = qChDate1.MaxLength = qChDate2.MaxLength = qChDate3.MaxLength = 8;
            qChDate.Init();
            qChDate1.Init();
            qChDate2.Init();
            qChDate3.Init();
        }

        private void 應收票據建檔_瀏覽_Load(object sender, EventArgs e)
        {
            dataGridViewT1.AutoGenerateColumns = false;
            dataGridViewT1.SetWidthByPixel();
            if (開窗模式) tableLayoutPnl2.Visible = tableLayoutPnl4.Visible = false;
            loadM("ChNo");
            if (list.Count > 0)
            {
                dr = list.Find(r => r["ChNo"].ToString().Trim() == SeekNo);
                if (dr == null) dr = list.Last();
                WriteToTxt(dr);
                NO = dr["序號"].ToString().Trim();
            }
            else
            {
                WriteToTxt(null);
            }
            SetButtonColor();
            SetSelectRow(NO);
            qCoNo.Focus();
        }

        private void loadM(string str)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(Common.sqlConnString))
                {
                    cn.Open();
                    string sql = "select 異動日期='',預兌日='',到期日='',收票日='',序號='',* from chki order by "+str;
                    SqlDataAdapter dd = new SqlDataAdapter(sql, cn);
                    table.Clear(); list.Clear();
                    dd.Fill(table);
                    if (table.Rows.Count > 0)
                    {
                        for (int i = 0; i < table.Rows.Count; i++)
                        {
                            table.Rows[i]["序號"] = i;
                            table.Rows[i]["異動日期"] = Common.User_DateTime == 1 ? Date.AddLine(table.Rows[i]["ChDate"].ToString()) : Date.AddLine(table.Rows[i]["ChDate_1"].ToString());
                            table.Rows[i]["預兌日"] = Common.User_DateTime == 1 ? Date.AddLine(table.Rows[i]["ChDate3"].ToString()) : Date.AddLine(table.Rows[i]["ChDate3_1"].ToString());
                            table.Rows[i]["到期日"] = Common.User_DateTime == 1 ? Date.AddLine(table.Rows[i]["ChDate2"].ToString()) : Date.AddLine(table.Rows[i]["ChDate2_1"].ToString());
                            table.Rows[i]["收票日"] = Common.User_DateTime == 1 ? Date.AddLine(table.Rows[i]["ChDate1"].ToString()) : Date.AddLine(table.Rows[i]["ChDate1_1"].ToString());
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

        private void WriteToTxt(DataRow dr)
        {
            if (dr == null) return;
            CuNo.Text = dr["CuNo"].ToString();
            AcNo.Text = dr["AcNo"].ToString();
            FaNo.Text = dr["FaNo"].ToString();
            ChMemo.Text = dr["ChMemo"].ToString();
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

        private void query2_Click(object sender, EventArgs e)
        {
            NO = dataGridViewT1.SelectedRows[0].Cells["序號"].Value.ToString();
            table.DefaultView.Sort = "ChNo";
            SetButtonColor();
            query2.ForeColor = Color.Red;
            SetSelectRow(NO);
        }

        private void query3_Click(object sender, EventArgs e)
        {
            NO = dataGridViewT1.SelectedRows[0].Cells["序號"].Value.ToString();
            table.DefaultView.Sort = "ChStatus,chdate2";
            SetButtonColor();
            query3.ForeColor = Color.Red;
            SetSelectRow(NO);
        }

        private void query4_Click(object sender, EventArgs e)
        {
            NO = dataGridViewT1.SelectedRows[0].Cells["序號"].Value.ToString();
            table.DefaultView.Sort = "ChDate desc,chno";
            SetButtonColor();
            query4.ForeColor = Color.Red;
            SetSelectRow(NO);
        }

        private void query5_Click(object sender, EventArgs e)
        {
            NO = dataGridViewT1.SelectedRows[0].Cells["序號"].Value.ToString();
            table.DefaultView.Sort = "CuNo,chdate2 desc";
            SetButtonColor();
            query5.ForeColor = Color.Red;
            SetSelectRow(NO);
        }

        private void query6_Click(object sender, EventArgs e)
        {
            NO = dataGridViewT1.SelectedRows[0].Cells["序號"].Value.ToString();
            table.DefaultView.Sort = "ChDate3 desc,chno";
            SetButtonColor();
            query6.ForeColor = Color.Red;
            SetSelectRow(NO);
        }

        private void query7_Click(object sender, EventArgs e)
        {
            NO = dataGridViewT1.SelectedRows[0].Cells["序號"].Value.ToString();
            table.DefaultView.Sort = "ChDate2 desc,chno";
            SetButtonColor();
            query7.ForeColor = Color.Red;
            SetSelectRow(NO);
        }

        private void query8_Click(object sender, EventArgs e)
        {
            NO = dataGridViewT1.SelectedRows[0].Cells["序號"].Value.ToString();
            table.DefaultView.Sort = "ChDate1 desc,chno";
            SetButtonColor();
            query8.ForeColor = Color.Red;
            SetSelectRow(NO);
        }

        private void query9_Click(object sender, EventArgs e)
        {
            NO = dataGridViewT1.SelectedRows[0].Cells["序號"].Value.ToString();
            table.DefaultView.Sort = "FaNo,chdate2 desc";
            SetButtonColor();
            query9.ForeColor = Color.Red;
            SetSelectRow(NO);
        }

        private void query10_Click(object sender, EventArgs e)
        {
            NO = dataGridViewT1.SelectedRows[0].Cells["序號"].Value.ToString();
            table.DefaultView.Sort = "ChMemo,chno";
            SetButtonColor();
            query10.ForeColor = Color.Red;
            SetSelectRow(NO);
        }

        private void btnGet_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            if (dataGridViewT1.SelectedRows.Count > 0)
                Result = dataGridViewT1.SelectedRows[0].Cells["支票編號"].Value.ToString().Trim();
            this.Close();
            this.Dispose();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
            this.Dispose();
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            btnQuery.Enabled = false;
            if (qCoNo.Text.Trim() != "")
            {
                loadM("CoNo,ChNo");
                //table.DefaultView.Sort = "CoNo";
                dataGridViewT1.Search("cono", qCoNo.Text);
                dr = list.Find(r => r["序號"].ToString() == dataGridViewT1.SelectedRows[0].Cells["序號"].ToString());
                SetButtonColor();
                WriteToTxt(dr);
                btnQuery.Enabled = true;
                btnQuery.Focus();
                return;
            }
            if (qChNo.Text.Trim() != "")
            {
                loadM("ChNo");
                //table.DefaultView.Sort = "ChNo";
                dataGridViewT1.Search("支票編號", qChNo.Text);
                dr = list.Find(r => r["序號"].ToString() == dataGridViewT1.SelectedRows[0].Cells["序號"].ToString());
                SetButtonColor();
                WriteToTxt(dr);
                query2.ForeColor = Color.Red;
                btnQuery.Enabled = true;
                btnQuery.Focus();
                return;
            }
            if (qChStatus.Text.Trim() != "")
            {
                loadM("ChStatus,chdate2 desc");
                //table.DefaultView.Sort = "ChStatus";
                dataGridViewT1.Search("chstatus", qChStatus.Text);
                dr = list.Find(r => r["序號"].ToString() == dataGridViewT1.SelectedRows[0].Cells["序號"].ToString());
                SetButtonColor();
                WriteToTxt(dr);
                query3.ForeColor = Color.Red;
                btnQuery.Enabled = true;
                btnQuery.Focus();
                return;
            }
            if (qCuNo.Text.Trim() != "")
            {
                loadM("CuNo,chdate2 desc");
                //table.DefaultView.Sort = "CuNo";
                dataGridViewT1.Search("客戶編號", qCuNo.Text);
                dr = list.Find(r => r["序號"].ToString() == dataGridViewT1.SelectedRows[0].Cells["序號"].ToString());
                SetButtonColor();
                WriteToTxt(dr);
                query5.ForeColor = Color.Red;
                btnQuery.Enabled = true;
                btnQuery.Focus();
                return;
            }
            if (qFaNo.Text.Trim() != "")
            {
                loadM("FaNo,chdate2 desc");
                //table.DefaultView.Sort = "FaNo";
                dataGridViewT1.Search("廠商編號", qFaNo.Text);
                dr = list.Find(r => r["序號"].ToString() == dataGridViewT1.SelectedRows[0].Cells["序號"].ToString());
                SetButtonColor();
                WriteToTxt(dr);
                query9.ForeColor = Color.Red;
                btnQuery.Enabled = true;
                btnQuery.Focus();
                return;
            }
            if (qChDate.Text.Trim() != "")
            {
                loadM("ChDate desc,chno");
                //table.DefaultView.Sort = "ChDate";
                if(Common.User_DateTime == 1)
                    dataGridViewT1.SearchForDate("chdate", qChDate.Text);
                else
                    dataGridViewT1.SearchForDate("chdate_1", qChDate.Text);
                dr = list.Find(r => r["序號"].ToString() == dataGridViewT1.SelectedRows[0].Cells["序號"].ToString());
                SetButtonColor();
                WriteToTxt(dr);
                query4.ForeColor = Color.Red;
                btnQuery.Enabled = true;
                btnQuery.Focus();
                return;
            }
            if (qChDate3.Text.Trim() != "")
            {
                loadM("ChDate3 desc,chno");
                //table.DefaultView.Sort = "ChDate3";
                if (Common.User_DateTime == 1)
                    dataGridViewT1.SearchForDate("chdate3", qChDate3.Text);
                else
                    dataGridViewT1.SearchForDate("chdate3_1", qChDate3.Text);
                dr = list.Find(r => r["序號"].ToString() == dataGridViewT1.SelectedRows[0].Cells["序號"].ToString());
                SetButtonColor();
                WriteToTxt(dr);
                query6.ForeColor = Color.Red;
                btnQuery.Enabled = true;
                btnQuery.Focus();
                return;
            }
            if (qChDate2.Text.Trim() != "")
            {
                loadM("ChDate2 desc,chno");
                //table.DefaultView.Sort = "ChDate2";
                if(Common.User_DateTime == 1)
                    dataGridViewT1.SearchForDate("chdate2", qChDate2.Text);
                else
                    dataGridViewT1.SearchForDate("chdate2_1", qChDate2.Text);
                dr = list.Find(r => r["序號"].ToString() == dataGridViewT1.SelectedRows[0].Cells["序號"].ToString());
                SetButtonColor();
                WriteToTxt(dr);
                query7.ForeColor = Color.Red;
                btnQuery.Enabled = true;
                btnQuery.Focus();
                return;
            }
            if (qChDate1.Text.Trim() != "")
            {
                loadM("ChDate1 desc,chno");
                //table.DefaultView.Sort = "ChDate1";
                if(Common.User_DateTime == 1)
                    dataGridViewT1.SearchForDate("chdate1", qChDate1.Text);
                else
                    dataGridViewT1.SearchForDate("chdate1_1", qChDate1.Text);
                dr = list.Find(r => r["序號"].ToString() == dataGridViewT1.SelectedRows[0].Cells["序號"].ToString());
                SetButtonColor();
                WriteToTxt(dr);
                query8.ForeColor = Color.Red;
                btnQuery.Enabled = true;
                btnQuery.Focus();
                return;
            }
            if (qChMemo.Text.Trim() != "")
            {
                loadM("ChMemo,chno");
                //table.DefaultView.Sort = "ChMemo";
                dataGridViewT1.Search("備註", qChMemo.Text);
                dr = list.Find(r => r["序號"].ToString() == dataGridViewT1.SelectedRows[0].Cells["序號"].ToString());
                SetButtonColor();
                WriteToTxt(dr);
                query10.ForeColor = Color.Red;
                btnQuery.Enabled = true;
                btnQuery.Focus();
                return;
            }

            btnQuery.Enabled = true;
            btnQuery.Focus();
        }

        private void 應收票據建檔_瀏覽_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.F2:
                    if (e.Modifiers == Keys.Shift)
                    {
                        query2.Focus();
                        query2.PerformClick();
                    }
                    break;
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
                case Keys.F8:
                    if (e.Modifiers == Keys.Shift)
                    {
                        query8.Focus();
                        query8.PerformClick();
                    }
                    break;
                case Keys.F9:
                    if (e.Modifiers == Keys.Shift)
                    {
                        query9.Focus();
                        query9.PerformClick();
                    }
                    btnGet.Focus();
                    btnGet.PerformClick();
                    break;
                case Keys.F10:
                    if (e.Modifiers == Keys.Shift)
                    {
                        query10.Focus();
                        query10.PerformClick();
                    }
                    break;
                case Keys.F11:
                    btnExit.Focus();
                    btnExit.PerformClick();
                    break;
            }
        }

        private void dataGridViewT1_RowStateChanged(object sender, DataGridViewRowStateChangedEventArgs e)
        {
            if (dataGridViewT1.SelectedRows.Count == 0) return;
            WriteToTxt(list.Find(r => r["chno"].ToString().Trim() == dataGridViewT1.SelectedRows[0].Cells["支票編號"].Value.ToString().Trim()));
        }

        private void qCoNo_DoubleClick(object sender, EventArgs e)
        {
            CHK.CoNo_OpemFrm(qCoNo);
            if (qCoNo.Text != "")
                Common.取得浮動連線字串(qCoNo.Text.Trim());
        }

        private void qCuNo_DoubleClick(object sender, EventArgs e)
        {
            CHK.CuNo_OpemFrm(qCuNo);
        }

        private void qFaNo_DoubleClick(object sender, EventArgs e)
        {
            CHK.FaNo_OpemFrm(qFaNo);
        }

        private void qChDate_Validating(object sender, CancelEventArgs e)
        {
            if (btnExit.Focused) return;
            TextBox tb = sender as TextBox;
            if (tb.Text.Trim() == "") return;
            if (!tb.IsDateTime())
            {
                MessageBox.Show("日期格式錯誤，請確定後重新輸入", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                e.Cancel = true;
                return;
            }
        }

        private void qChMemo_DoubleClick(object sender, EventArgs e)
        {
            CHK.Memo_OpemFrm(qChMemo);
        }

        private void qChStatus_Validating(object sender, CancelEventArgs e)
        {
            if (btnExit.Focused || qChStatus.Text.Trim() == "") return;
            Char[] c = qChStatus.Text.ToCharArray();
            if (c[0].ToString().StartsWith("F")) return;
            if (!Char.IsNumber(c[0]) || c[0].ToDecimal() < 1 || c[0].ToDecimal() > 8)
            {
                MessageBox.Show("請輸入１～８數字", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                qChStatus.Text = "";
                e.Cancel = true;
                return;
            }
        }

        private void dataGridViewT1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (!開窗模式) return;
            this.DialogResult = DialogResult.OK;
            if (dataGridViewT1.SelectedRows.Count > 0)
                Result = dataGridViewT1.SelectedRows[0].Cells["支票編號"].Value.ToString().Trim();
            this.Close();
            this.Dispose();
        }



    }
}
