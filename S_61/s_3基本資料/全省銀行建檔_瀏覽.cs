using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using S_61.Model;
using S_61.Basic;
using S_61.MyControl;

namespace S_61.s_3基本資料
{
    public partial class 全省銀行建檔_瀏覽 : FormT
    {
        DataTable tbM = new DataTable();
        List<DataRow> list = new List<DataRow>();
        List<btnT> SC;
        List<btnT> Others;
        string CurrentRow = "";
        public string result = "";
        public DataRow Result;
        public bool CanAppend { get; set; }
        public bool 開窗模式 = false;
        public 全省銀行建檔_瀏覽()
        {
            InitializeComponent();
            CN.ConnectionString = Common.sqlConnString;
            SC = new List<btnT> { btnSave, btnCancel };
            Others = new List<btnT> { btnModify, btnExit };
        }

        private void 全省銀行建檔_瀏覽_Load(object sender, EventArgs e)
        {
            dataGridViewT1.SetWidthByPixel();
            SC.ForEach(r => r.Enabled = false);
            Others.ForEach(r => r.Enabled = true);

            if (!CanAppend) Append.Visible = false;
            if (開窗模式)
            {
                tableLayoutPnl4.Visible = false;
                qBaNo.Focus();
            }
            else
            {
                tableLayoutPnl2.Visible = false;
                tableLayoutPnl3.Visible = false;
                BaNo.Focus();
            }
            loadM();
            SelectRow(SeekNo, "");
        }

        void loadM()
        {
            try
            {
                dataGridViewT1.DataSource = null;
                tbM.Clear();
                list.Clear();
                Bank.Fill(tbM);
                if (tbM.Rows.Count > 0) list = tbM.AsEnumerable().ToList();
                dataGridViewT1.DataSource = tbM;
                Count.Text = "總共有 " + tbM.Rows.Count + " 筆資料";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        void SelectRow(string BaNo,string BaName)
        {
            if (list.Count > 0)
            {
                if (BaNo != "" && BaName == "")
                {
                    dataGridViewT1.Search("銀行編號", BaNo);
                }
                else if (BaNo == "" && BaName != "")
                {
                    int i = list.FindIndex(r => r["baname"].ToString().Contains(BaName));
                    if (i == -1) return;
                    dataGridViewT1.FirstDisplayedScrollingRowIndex = i;
                    dataGridViewT1.Rows[i].Selected = true;
                }
                else if (BaNo != "" && BaName != "")
                {
                    int i = list.FindIndex(r => string.CompareOrdinal(r["bano"].ToString(), BaNo) == 0);
                    if (i != -1)
                    {
                        dataGridViewT1.FirstDisplayedScrollingRowIndex = i;
                        dataGridViewT1.Rows[i].Selected = true;
                    }
                    else
                    {
                        i = list.FindIndex(r => string.CompareOrdinal(r["baname"].ToString(), BaName) == 0);
                        if(i != -1)
                        {
                            dataGridViewT1.FirstDisplayedScrollingRowIndex = i;
                            dataGridViewT1.Rows[i].Selected = true;
                            return;
                        }
                        var temp = tbM.AsEnumerable().Where(r => r["bano"].ToString().StartsWith(BaNo));
                        foreach (var t in temp)
                        {
                            if (t["baname"].ToString().Contains(BaName))
                            {
                                i = list.FindIndex(r => r["bano"].ToString() == t["bano"].ToString());
                                dataGridViewT1.FirstDisplayedScrollingRowIndex = i;
                                dataGridViewT1.Rows[i].Selected = true;
                                break;
                            }
                        }
                    }
                }
            }
        }

        private void btnModify_Click(object sender, EventArgs e)
        {
            if (dataGridViewT1.Rows.Count == 0) return;
            SC.ForEach(r => r.Enabled = true);
            Others.ForEach(r => r.Enabled = false);
            dataGridViewT1.ReadOnly = false;
            this.銀行編號.ReadOnly = true;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (dataGridViewT1.Rows.Count == 0) return;
            CurrentRow = dataGridViewT1.SelectedRows[0].Cells["銀行編號"].Value.ToString().Trim();
            try
            {
                Bank.Update(tbM);
                loadM();
                SelectRow(CurrentRow, "");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            btnCancel_Click(null, null);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            dataGridViewT1.ReadOnly = true;
            SC.ForEach(r => r.Enabled = false);
            Others.ForEach(r => r.Enabled = true);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            if (dataGridViewT1.SelectedRows.Count != 0)
                result = dataGridViewT1.SelectedRows[0].Cells["銀行編號"].Value.ToString().Trim();
            this.DialogResult = DialogResult.OK;
            this.Dispose();
            this.Close();
        }

        private void qBaNo_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode.ToString().StartsWith("F")) return;
            SelectRow(qBaNo.Text,qBaName.Text);
        }

        private void qBaName_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode.ToString().StartsWith("F")) return;
            SelectRow(qBaNo.Text, qBaName.Text);
        }

        private void 全省銀行建檔_瀏覽_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.F11:
                    if (開窗模式)
                        Exit.PerformClick();
                    else
                        btnExit.PerformClick();
                    break;
                case Keys.F9:
                    if (開窗模式)
                        Get.PerformClick();
                    else
                        btnSave.PerformClick();
                    break;
                case Keys.F4:
                    btnCancel.Focus();
                    btnCancel.PerformClick();
                    break;
                case Keys.F2:
                    if (開窗模式)
                        Append.PerformClick();
                    break;
                case Keys.Escape:
                    if (開窗模式)
                        Exit.PerformClick();
                    else
                        btnExit.PerformClick();
                    break;
                case Keys.F6:
                    if (開窗模式)
                        Query.PerformClick();
                    break;
            }
        }

        private void BaNo_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode.ToString().StartsWith("F")) return;
            SelectRow(BaNo.Text, BaName.Text);
        }

        private void BaName_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode.ToString().StartsWith("F")) return;
            SelectRow(BaNo.Text, BaName.Text);
        }


        private void Append_Click(object sender, EventArgs e)
        {
            using (全省銀行建檔 frm = new 全省銀行建檔())
            {
                frm.SetParaeter();
                frm.ShowDialog();
            }
            loadM();
        }

        private void btnBrowT1_Click(object sender, EventArgs e)
        {

        }

        private void Query_Click(object sender, EventArgs e)
        {
            if (BaNo.Text == "" && BaName.Text == "") return;
            using (SqlConnection cn = new SqlConnection(Common.sqlConnString))
            {
                cn.Open();
                string sql = "select * from bank where '0'='0'";
                if (BaNo.Text != "")
                    sql += " and bano like '%"+BaNo.Text+"%'";
                if(BaName.Text != "")
                    sql += " and baname like '%" + BaName.Text + "%'";
                sql += " order by bano";
                SqlDataAdapter dd = new SqlDataAdapter(sql, cn);
                tbM.Clear();
                dataGridViewT1.DataSource = null;
                dd.Fill(tbM);
                dataGridViewT1.DataSource = tbM;
                Count.Text = "總共有 " + tbM.Rows.Count + " 筆資料";
                BaNo.Enter += new EventHandler(Text_OnEnter);
                BaName.Enter += new EventHandler(Text_OnEnter);
            }
        }

        private void Text_OnEnter(object sender, EventArgs e)
        {
            loadM();
            BaNo.Text = BaName.Text = "";
            BaNo.Enter -= Text_OnEnter;
            BaName.Enter -= Text_OnEnter;
        }

        private void Get_Click(object sender, EventArgs e)
        {
            Result = tbM.Rows[dataGridViewT1.SelectedRows[0].Index];
            this.DialogResult = DialogResult.OK;
        }

        private void Exit_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void dataGridViewT1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridViewT1.SelectedRows.Count == 0 || !開窗模式) return;
            Result = tbM.Rows[dataGridViewT1.SelectedRows[0].Index];
            this.DialogResult = DialogResult.OK;
        }




    }
}
