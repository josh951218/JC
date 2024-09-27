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
    public partial class 銀行帳號建檔_瀏覽 : FormT
    {
        DataTable tbM = new DataTable();
        List<DataRow> list = new List<DataRow>();
        public string result = "";
        public DataRow Result;
        public bool CanAppend { get; set; }
        public bool 開窗模式 = false;
        public bool 去除外幣帳戶 = false;
        public string CoNo = "";

        public 銀行帳號建檔_瀏覽()
        {
            InitializeComponent();
            this.現行餘額.DefaultCellStyle.Format = "N" + Common.金額小數;
        }

        private void 銀行帳號建檔_瀏覽_Load(object sender, EventArgs e)
        {
            dataGridViewT1.SetWidthByPixel();
            dataGridViewT1.AutoGenerateColumns = false;
            if (!CanAppend) Append.Visible = false;
            loadM();
            if (list.Count > 0)
            {
                dataGridViewT1.Search("帳戶編號", SeekNo);
            }
            AcNo.Focus();
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
                    string sql = "select 帳戶類別=case when ackind=1 then '本幣帳戶'  when ackind=2 then '外幣帳戶' end,* from acct where '0'='0'";
                    if (去除外幣帳戶) sql += " and ackind=1 ";
                    if(CoNo != "")
                        sql += " and cono='"+CoNo+"' ";
                    else if(Common.單據異動 == "2")
                        sql += " and cono='" + Common.使用者預設公司 + "' ";

                    sql += " order by acno";
                    SqlDataAdapter dd = new SqlDataAdapter(sql, cn);
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

        private void 銀行帳號建檔_瀏覽_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.F11:
                        Exit.PerformClick();
                    break;
                case Keys.F2:
                        Append.PerformClick();
                    break;
                case Keys.F9:
                        Get.PerformClick();
                    break;
                case Keys.Escape:
                        Exit.PerformClick();
                    break;
            }
        }

        private void Append_Click(object sender, EventArgs e)
        {
            using (銀行帳號建檔 frm = new 銀行帳號建檔())
            {
                frm.SetParaeter();
                frm.ShowDialog();
            }
            loadM();
        }

        private void Get_Click(object sender, EventArgs e)
        {
            result = dataGridViewT1.SelectedRows[0].Cells["帳戶編號"].Value.ToString().Trim();
            Result = tbM.Rows[dataGridViewT1.SelectedRows[0].Index];
            this.DialogResult = DialogResult.OK;
        }

        private void Exit_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            result = dataGridViewT1.SelectedRows[0].Cells["帳戶編號"].Value.ToString().Trim();
            this.Dispose();
            this.Close();
        }

        private void AcNo_KeyUp(object sender, KeyEventArgs e)
        {
            if (dataGridViewT1.Rows.Count == 0 || AcNo.Text.Trim() == "") return;
            dataGridViewT1.Search("帳戶編號", AcNo.Text);
        }

        private void dataGridViewT1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridViewT1.SelectedRows.Count == 0 || !開窗模式) return;
            Result = tbM.Rows[dataGridViewT1.SelectedRows[0].Index];
            this.DialogResult = DialogResult.OK;
        }


    }
}
