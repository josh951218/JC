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
    public partial class 客戶類別建檔_瀏覽 : FormT
    {
        DataTable tbM = new DataTable();
        List<DataRow> list = new List<DataRow>();
        List<btnT> SC;
        List<btnT> Others;
        string CurrentRow = "";
        SqlTransaction tran;
        public string result = "";
        public DataRow Result;
        public bool CanAppend { get; set; }
        public bool 開窗模式 = false;

        public 客戶類別建檔_瀏覽()
        {
            InitializeComponent();
            SC = new List<btnT> { btnSave, btnCancel };
            Others = new List<btnT> { btnModify, btnExit };
        }

        private void 客戶類別建檔_瀏覽_Load(object sender, EventArgs e)
        {
            dataGridViewT1.SetWidthByPixel();
            SC.ForEach(r => r.Enabled = false);
            Others.ForEach(r => r.Enabled = true);

            if (!CanAppend) Append.Visible = false;
            if (開窗模式)
            {
                panelT1.Visible = false;
                X1No.Focus();
            }
            else
            {
                tableLayoutPnl2.Visible = false;
                tableLayoutPnl3.Visible = false;
            }

            loadM();
            if (list.Count > 0)
            {
                dataGridViewT1.Search("類別編號", SeekNo);
            }
        }

        void loadM()
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(Common.浮動連線字串))
                {
                    cn.Open();
                    tbM.Clear();
                    list.Clear();
                    SqlDataAdapter dd = new SqlDataAdapter("select * from xx01 order by x1no", cn);
                    dd.Fill(tbM);
                    if (tbM.Rows.Count > 0) list = tbM.AsEnumerable().ToList();
                    dataGridViewT1.DataSource = tbM;
                    Count.Text = "總共有 " + tbM.Rows.Count + " 筆資料";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void btnModify_Click(object sender, EventArgs e)
        {
            if (dataGridViewT1.Rows.Count == 0) return;
            SC.ForEach(r => r.Enabled = true);
            Others.ForEach(r => r.Enabled = false);
            dataGridViewT1.ReadOnly = false;
            this.類別編號.ReadOnly = true;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (dataGridViewT1.Rows.Count == 0) return;
            using (SqlConnection cn = new SqlConnection(Common.浮動連線字串))
            {
                cn.Open();
                SqlCommand cmd = cn.CreateCommand();
                tran = cn.BeginTransaction();
                cmd.Transaction = tran;
                CurrentRow = dataGridViewT1.SelectedRows[0].Cells["類別編號"].Value.ToString().Trim();
                try
                {
                    foreach (DataGridViewRow i in dataGridViewT1.Rows)
                    {
                        cmd.CommandText += "update xx01 set x1name='" + i.Cells["類別名稱"].Value.ToString().Trim() + "'"
                            + " where x1no='" + i.Cells["類別編號"].Value.ToString().Trim() + "';";
                    }
                    cmd.ExecuteNonQuery();
                    tran.Commit();
                    loadM();
                    if (list.Count > 0)
                    {
                        dataGridViewT1.Search("類別編號", CurrentRow);
                    }
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    MessageBox.Show(ex.ToString());
                }
                btnCancel_Click(null, null);
            }
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
                result = dataGridViewT1.SelectedRows[0].Cells["類別編號"].Value.ToString().Trim();
            this.DialogResult = DialogResult.OK;
            this.Dispose();
            this.Close();
        }

        private void 客戶類別建檔_瀏覽_KeyUp(object sender, KeyEventArgs e)
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
            }
        }

        private void Append_Click(object sender, EventArgs e)
        {
            using (客戶類別建檔 frm = new 客戶類別建檔())
            {
                frm.SetParaeter();
                frm.ShowDialog();
            }
            loadM();
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

        private void X1No_KeyUp(object sender, KeyEventArgs e)
        {
            if (dataGridViewT1.Rows.Count == 0 || X1No.Text.Trim() == "") return;
            dataGridViewT1.Search("類別編號", X1No.Text.Trim());
        }

        private void dataGridViewT1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridViewT1.SelectedRows.Count == 0 || !開窗模式) return;
            Result = tbM.Rows[dataGridViewT1.SelectedRows[0].Index];
            this.DialogResult = DialogResult.OK;
        }



    }
}
