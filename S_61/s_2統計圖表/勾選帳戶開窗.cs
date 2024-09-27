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


namespace S_61.s_2統計圖表
{
    public partial class 勾選帳戶開窗 : FormT
    {
        DataTable tbM = new DataTable();
        List<DataRow> list = new List<DataRow>();
        string[] arry;
        public string CoNo = "";
        public string result = "";
        public bool 去除外幣帳戶 = false;
        public string 勾選字串 = "";

        public 勾選帳戶開窗()
        {
            InitializeComponent();
            this.現行餘額.DefaultCellStyle.Format = "N" + Common.金額小數;
        }

        private void 多選帳戶開窗_Load(object sender, EventArgs e)
        {
            dataGridViewT1.SetWidthByPixel();
            dataGridViewT1.AutoGenerateColumns = false;
            loadM();
            //if (list.Count > 0)
            //{
            //    dataGridViewT1.Search("帳戶編號", SeekNo);
            //}
            AcNo.Focus();
            if (勾選字串 != "")
            {
                arry = 勾選字串.Substring(0, 勾選字串.Length-1).Split(',');
                for (int i = 0; i < arry.Length; i++)
                {
                    if (list.Find(r => r["acno"].ToString().Trim() == arry[i].ToString()) != null)
                        list.Find(r => r["acno"].ToString().Trim() == arry[i].ToString())["勾選"] = "V";
                }
            }
            dataGridViewT1.DataSource = tbM;
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
                    string sql = "select 帳戶類別=case when ackind=1 then '本幣帳戶'  when ackind=2 then '外幣帳戶' end,勾選='',* from acct where '0'='0'";
                    if (去除外幣帳戶) sql += " and ackind=1 ";
                    if (CoNo != "")
                        sql += " and cono='" + CoNo + "' ";
                    else if (Common.單據異動 == "2")
                        sql += " and cono='" + Common.使用者預設公司 + "' ";
                    sql += " order by acno";
                    SqlDataAdapter dd = new SqlDataAdapter(sql, cn);
                    dd.Fill(tbM);
                    if (tbM.Rows.Count > 0) list = tbM.AsEnumerable().ToList();
                    Count.Text = "總共有 " + tbM.Rows.Count + " 筆資料";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void 多選帳戶開窗_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.F11:
                    Exit.PerformClick();
                    break;
                case Keys.F9:
                    Get.PerformClick();
                    break;
                case Keys.Escape:
                    Exit.PerformClick();
                    break;
            }
        }

        private void Get_Click(object sender, EventArgs e)
        {
            勾選字串 = "";
            for (int i = 0; i < tbM.Rows.Count; i++)
            {
                if (tbM.Rows[i]["勾選"].ToString() == "V")
                    勾選字串 += tbM.Rows[i]["acno"].ToString().Trim() + ",";
            }
            
            this.DialogResult = DialogResult.OK;
        }

        private void Exit_Click(object sender, EventArgs e)
        {
            //result = dataGridViewT1.SelectedRows[0].Cells["帳戶編號"].Value.ToString().Trim();
            this.DialogResult = DialogResult.Cancel;
            this.Dispose();
            this.Close();
        }

        private void AcNo_KeyUp(object sender, KeyEventArgs e)
        {
            if (dataGridViewT1.Rows.Count == 0 || AcNo.Text.Trim() == "") return;
            dataGridViewT1.Search("帳戶編號", AcNo.Text);
        }

        private void dataGridViewT1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridViewT1.Rows.Count == 0) return;
            if (e.RowIndex < 0 || e.RowIndex > dataGridViewT1.Rows.Count - 1) return;
            if (dataGridViewT1.Columns[e.ColumnIndex].Name != "勾選") return;
            if (dataGridViewT1.Rows[e.RowIndex].Cells["勾選"].Value.ToString().Trim() == "V")
                tbM.Rows[e.RowIndex]["勾選"] = "";
            else
                tbM.Rows[e.RowIndex]["勾選"] = "V";
        }
    }
}
