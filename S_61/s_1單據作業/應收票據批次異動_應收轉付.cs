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
    public partial class 應收票據批次異動_應收轉付 : FormT
    {
        public string cono = "";
        public string coname1 = "";
        public DataTable table = new DataTable();
        public DataTable PrintTable = new DataTable();
        List<DataRow> list = new List<DataRow>();
        string path = "";
        string ReportFileName = "";
        string CoAddr1 = "", CoTel1 = "", CoFax1 = "";
        public 應收票據批次異動_應收轉付()
        {
            InitializeComponent();
            this.票面金額.DefaultCellStyle.Format = "N" + Common.金額小數;
            RealMny.NumThousands = TotMny.NumThousands = DMny.NumThousands = ChOMny1.NumThousands = ChOMny2.NumThousands = ChOMny3.NumThousands = true;
            RealMny.NumLast = TotMny.NumLast = DMny.NumLast = ChOMny1.NumLast = ChOMny2.NumLast = ChOMny3.NumLast =  Common.金額小數;
            RealMny.NumFirst = TotMny.NumFirst = DMny.NumFirst = ChOMny1.NumFirst = ChOMny2.NumFirst = ChOMny3.NumFirst = (20 - 1 - Common.金額小數);
            RealMny.ReadOnly = ChOMny1.ReadOnly = ChOMny2.ReadOnly = ChOMny3.ReadOnly = TotMny.ReadOnly = DMny.ReadOnly = false;
            RealMny.Text = TotMny.Text = DMny.Text = ChOMny1.Text = ChOMny2.Text = ChOMny3.Text = "0";
            TotMny.ReadOnly = DMny.ReadOnly = true;
        }

        private void 應收票據批次異動_應收轉付_Load(object sender, EventArgs e)
        {
            ReportFileName = "應收票據批次異動";
            path = Common.reportaddress + "Report\\" + ReportFileName + "_轉付領款簽回單.rpt";
            dataGridViewT1.SetWidthByPixel();
            dataGridViewT1.AutoGenerateColumns = false;
            dataGridViewT1.ReadOnly = false;
            this.支票號碼.ReadOnly = true;
            this.客戶簡稱.ReadOnly = true;
            this.託收帳戶.ReadOnly = true;
            this.到期日.ReadOnly = true;
            this.預兌日.ReadOnly = true;
            this.票面金額.ReadOnly = true;
            if (Common.User_DateTime == 1) this.異動日期.MaxInputLength = 7;
            else this.異動日期.MaxInputLength = 8;
            CoNo.Text = cono;
            CoName1.Text = coname1;
            Common.取得浮動連線字串(CoNo.Text.Trim());
            ChStatus.Text = "5";
            ChName.Text = "應收轉付";
            count.Text = "0";
            using (SqlConnection cn = new SqlConnection(Common.sqlConnString))
            {
                cn.Open();
                SqlCommand cmd = cn.CreateCommand();
                cmd.CommandText = "select * from comp where cono='" + CoNo.Text.Trim() + "'";
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows && reader.Read())
                {
                    CoAddr1 = reader["CoAddr1"].ToString();
                    CoTel1 = reader["CoTel1"].ToString();
                    CoFax1 = reader["CoFax1"].ToString();
                }
                reader.Dispose(); reader.Close();
            }
            table.AsEnumerable().ToList().ForEach(r =>
            {
                r["到期日"] = Common.User_DateTime == 1 ? Date.AddLine(r["chdate2"].ToString()) : Date.AddLine(r["chdate2_1"].ToString());
                r["預兌日"] = Common.User_DateTime == 1 ? Date.AddLine(r["chdate3"].ToString()) : Date.AddLine(r["chdate3_1"].ToString());
            });
            dataGridViewT1.DataSource = table;
            dataGridViewT1.Rows[0].Selected = true;
            dataGridViewT1.CurrentCell = dataGridViewT1.SelectedRows[0].Cells["異動日期"];
            dataGridViewT1.Focus();
        }

        RPT paramsInit()
        {
            RPT rp = new RPT(PrintTable, path);
            rp.office = this.Tag.ToString();
            //公司抬頭
            rp.lobj.Add(new string[] { "txtstart", CHK.GetCoName2(Common.使用者預設公司) });
            //制表日期
            rp.lobj.Add(new string[] { "制表日期", Date.GetDateTime(Common.User_DateTime, true) });
            //廠商名稱
            rp.lobj.Add(new string[] { "廠商名稱", FaNo.Text.Trim()+" "+FaName2.Text.Trim() });
            //應付總額
            rp.lobj.Add(new string[] { "RealMny", RealMny.Text.Trim()});
            //轉付總額
            rp.lobj.Add(new string[] { "DMny", (RealMny.Text.ToDecimal() + ChOMny1.Text.ToDecimal() - ChOMny2.Text.ToDecimal() + ChOMny3.Text.ToDecimal()).ToString("N"+Common.金額小數) });
            //收取現金
            rp.lobj.Add(new string[] { "ChOMny1", ChOMny1.Text.Trim() });
            //支付現金
            rp.lobj.Add(new string[] { "ChOMny2", ChOMny2.Text.Trim() });
            //累入預收
            rp.lobj.Add(new string[] { "ChOMny3", ChOMny3.Text.Trim() });
            //公司地址
            rp.lobj.Add(new string[] { "CoAddr1", CoAddr1 });
            //公司電話
            rp.lobj.Add(new string[] { "CoTel1", CoTel1 });
            //公司傳真
            rp.lobj.Add(new string[] { "CoFax1", CoFax1 });
            return rp;
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (FaNo.Text.Trim() == "")
            {
                MessageBox.Show("『轉付廠商』不可為空白，請確定後重新輸入", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                FaNo.Focus();
                return;
            }
            if (count.Text.Trim() == "0")
            {
                MessageBox.Show("尚未選擇異動票據", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (DMny.Text.ToDecimal() != 0)
            {
                if (MessageBox.Show("轉付金額不正確，請確定是否列印簽回單", "訊息視窗", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1) == DialogResult.Cancel) return;
            }
            PrintTable.Clear();
            var temp = table.AsEnumerable().ToList().Where(r => r["異動日期"].ToString().Trim() != "");
            PrintTable = temp.CopyToDataTable();
            paramsInit().Print();
        }

        private void btnPreview_Click(object sender, EventArgs e)
        {
            if (FaNo.Text.Trim() == "")
            {
                MessageBox.Show("『轉付廠商』不可為空白，請確定後重新輸入", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                FaNo.Focus();
                return;
            }
            if (count.Text.Trim() == "0")
            {
                MessageBox.Show("尚未選擇異動票據", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (DMny.Text.ToDecimal() != 0)
            {
                if (MessageBox.Show("轉付金額不正確，請確定是否列印簽回單", "訊息視窗", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1) == DialogResult.Cancel) return;
            }
            PrintTable.Clear();
            var temp = table.AsEnumerable().ToList().Where(r => r["異動日期"].ToString().Trim() != "");
            PrintTable = temp.CopyToDataTable();
            paramsInit().PreView();
        }

        private void btnGet_Click(object sender, EventArgs e)
        {
            if (DMny.Text.ToDecimal() != 0)
            {
                MessageBox.Show("轉付金額不正確，無法執行轉付作業", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                DMny.Focus();
                return;
            }
            try
            {
                PrintTable.Clear();
                var temp = table.AsEnumerable().ToList().Where(r => r["異動日期"].ToString().Trim() != "");
                if (temp.Count() == 0)
                {
                    MessageBox.Show("尚未選擇異動票據，請選擇", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (MessageBox.Show("確定是否異動票據", "訊息視窗", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1) == DialogResult.Cancel) return;
                PrintTable = temp.CopyToDataTable();
                using (SqlConnection cn = new SqlConnection(Common.sqlConnString))
                {
                    cn.Open();
                    SqlCommand cmd = cn.CreateCommand();
                    for (int i = 0; i < PrintTable.Rows.Count; i++)
                    {
                        if (i == PrintTable.Rows.Count - 1)
                        {
                            cmd.CommandText += "update chki set chstatus=5,chstname=N'" + ChName.Text.Trim() + "',chmemo=N'" + ChMemo.Text.Trim() + "',chdate=N'" + Date.ToTWDate(PrintTable.Rows[i]["異動日期"].ToString().Trim()) + "'"
                                + " ,chdate_1=N'" + Date.ToUSDate(PrintTable.Rows[i]["異動日期"].ToString().Trim()) + "'"
                                + " ,fano=N'"+FaNo.Text.Trim()+"',faname1=N'"+FaName1.Text.Trim()+"',faname2=N'"+FaName2.Text.Trim()+"'"
                                + " ,ChOMny1=" + ChOMny1.Text.ToDecimal().ToString("f" + Common.金額小數) + ""
                                + " ,ChOMny2=" + ChOMny2.Text.ToDecimal().ToString("f" + Common.金額小數) + ""
                                + " ,ChOMny3=" + ChOMny3.Text.ToDecimal().ToString("f" + Common.金額小數) + ""
                                + " where chno=N'" + PrintTable.Rows[i]["chno"].ToString().Trim() + "';";
                        }
                        else
                        {
                            cmd.CommandText += "update chki set chstatus=5,chstname=N'" + ChName.Text.Trim() + "',chmemo=N'" + ChMemo.Text.Trim() + "',chdate=N'" + Date.ToTWDate(PrintTable.Rows[i]["異動日期"].ToString().Trim()) + "'"
                                + " ,chdate_1=N'" + Date.ToUSDate(PrintTable.Rows[i]["異動日期"].ToString().Trim()) +"'"
                                + " ,fano=N'" + FaNo.Text.Trim() + "',faname1=N'" + FaName1.Text.Trim() + "',faname2=N'" + FaName2.Text.Trim() + "'"
                                + " where chno=N'" + PrintTable.Rows[i]["chno"].ToString().Trim() + "';";
                        }
                    }
                    cmd.ExecuteNonQuery();
                }
                using (SqlConnection cn = new SqlConnection(Common.浮動連線字串))
                {
                    cn.Open();
                    SqlCommand cmd = cn.CreateCommand();
                    cmd.CommandText = "update fact set fapayamt+=" + ChOMny3.Text.ToDecimal() + " where fano=N'" + FaNo.Text.Trim() + "'";
                    cmd.ExecuteNonQuery();
                }
                if (table.AsEnumerable().ToList().Where(r => r["異動日期"].ToString().Trim() == "").Count() > 0)
                {
                    table = table.AsEnumerable().ToList().Where(r => r["異動日期"].ToString().Trim() == "").CopyToDataTable();
                }
                else
                {
                    table.Clear();
                }
                dataGridViewT1.DataSource = table;
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

        private void dataGridViewT1_RowStateChanged(object sender, DataGridViewRowStateChangedEventArgs e)
        {
            if (dataGridViewT1.SelectedRows.Count == 0) return;
            BaName.Text = dataGridViewT1.SelectedRows[0].Cells["付款銀行"].Value.ToString();
        }

        private void dataGridViewT1_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (dataGridViewT1.Columns[e.ColumnIndex].Name != "異動日期" || btnExit.Focused) return;
            if (dataGridViewT1.EditingControl == null) return;
            if (dataGridViewT1.EditingControl.Text.Trim() == "")
            {
                dataGridViewT1.CommitEdit(DataGridViewDataErrorContexts.Commit);
                count.Text = table.AsEnumerable().ToList().Where(r => r["異動日期"].ToString().Trim() != "").Count().ToString();
                return;
            }
            if (!((TextBox)dataGridViewT1.EditingControl).IsDateTime())
            {
                MessageBox.Show("日期格式錯誤", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                ((TextBox)dataGridViewT1.EditingControl).SelectAll();
                e.Cancel = true;
                return;
            }
            if (!CHK.稽核會計年度(dataGridViewT1.EditingControl.Text.Trim()))
            {
                e.Cancel = true;
                ((TextBox)dataGridViewT1.EditingControl).SelectAll();
                return;
            }
            dataGridViewT1.CommitEdit(DataGridViewDataErrorContexts.Commit);
            計算錢();
            Focus異動日期();
        }

        private void dataGridViewT1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex > dataGridViewT1.Rows.Count - 1) return;
            if (dataGridViewT1.Columns[e.ColumnIndex].Name != "異動日期") return;
            if (dataGridViewT1.EditingControl == null) return;
            if (dataGridViewT1.EditingControl.Text.ToString() == "")
            {
                dataGridViewT1.EditingControl.Text = Common.User_DateTime == 1 ? table.Rows[e.RowIndex]["chdate3"].ToString() : table.Rows[e.RowIndex]["chdate3_1"].ToString();
                dataGridViewT1.CommitEdit(DataGridViewDataErrorContexts.Commit);
                ((TextBox)dataGridViewT1.EditingControl).SelectAll();
            }
            計算錢();
            Focus異動日期();
        }

        void Focus異動日期()
        {
            int index = dataGridViewT1.SelectedRows[0].Index;
            if (index <= dataGridViewT1.Rows.Count - 1)
            {
                btnGet.Focus();
                dataGridViewT1.Rows[index].Selected = true;
                dataGridViewT1.CurrentCell = dataGridViewT1.SelectedRows[0].Cells["異動日期"];
                dataGridViewT1.Focus();
            }
            else
            {
                btnGet.Focus();
            }
        }

        void 計算錢()
        {
            int c = 0;
            decimal totmny = 0;
            for (int i = 0; i < table.Rows.Count; i++)
            {
                if (table.Rows[i]["異動日期"].ToString().Trim() != "")
                {
                    ++c;
                    totmny += table.Rows[i]["chmny"].ToDecimal();
                }
            }
            count.Text = c.ToString();
            TotMny.Text = totmny.ToString("N" + Common.金額小數);
            DMny.Text = (RealMny.Text.ToDecimal() - TotMny.Text.ToDecimal() + ChOMny1.Text.ToDecimal() - ChOMny2.Text.ToDecimal() + ChOMny3.Text.ToDecimal()).ToString("N" + Common.金額小數);
        }

        private void 應收票據批次異動_應收轉付_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Tab:
                    if (ActiveControl is dataGridViewT)
                        Focus異動日期();
                    break;
                case Keys.F3:
                    btnPrint.PerformClick();
                    break;
                case Keys.F4:
                    btnPreview.PerformClick();
                    break;
                case Keys.F9:
                    btnGet.PerformClick();
                    break;
                case Keys.F11:
                    btnExit.PerformClick();
                    break;
            }
        }

        private void ChMemo_DoubleClick(object sender, EventArgs e)
        {
            CHK.Memo_OpemFrm(ChMemo);
        }

        private void RealMny_Validating(object sender, CancelEventArgs e)
        {
            計算錢();
        }

        private void FaNo_DoubleClick(object sender, EventArgs e)
        {
            CHK.FaNo_OpemFrm(FaNo, FaName1,FaName2);
        }

        private void FaNo_Validating(object sender, CancelEventArgs e)
        {
            if (FaNo.ReadOnly || btnExit.Focused) return;
            if (FaNo.Text.Trim() == "")
            {
                e.Cancel = true;
                FaName1.Text = "";
                FaName2.Text = "";
                MessageBox.Show("『轉付廠商』不可為空，請確定後重新輸入", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                FaNo.Focus();
                return;
            }
            if (!CHK.FaNo_Validating(Common.浮動連線字串, FaNo, FaName1,FaName2))
            {
                e.Cancel = true;
                CHK.FaNo_OpemFrm(FaNo, FaName1, FaName2);
            }
        }


    }
}
