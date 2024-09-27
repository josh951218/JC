using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using S_61.Basic;
using S_61.Model;
using S_61.MyControl;

namespace S_61.subMenuFm_5
{
    public partial class FrmPhrase : FormT
    {
        public FrmPhrase()
        {
            InitializeComponent();
            //pVar.FrmPhrase = this;
        }

        DataSet ds = new DataSet();
        BindingSource bs = new BindingSource();
        SqlDataAdapter da = new SqlDataAdapter();
        DataGridView[] dtview = new DataGridView[6];


        private void FrmPhrase_Load(object sender, EventArgs e)
        {
            //Basic.SetParameter.TabControlItemSize(tabControl1);
       
            //dtview[0] = dataGridViewT1;
            //dtview[1] = dataGridViewT2;
            //dtview[2] = dataGridViewT3;
            //dtview[3] = dataGridViewT4;
            //dtview[4] = dataGridViewT5;
            //dtview[5] = dataGridViewT6;
            //dataGridViewT1.ReadOnly = false;
            //dataGridViewT2.ReadOnly = false;
            //dataGridViewT3.ReadOnly = false;
            //dataGridViewT4.ReadOnly = false;
            //dataGridViewT5.ReadOnly = false;
            //dataGridViewT6.ReadOnly = false;
            //dataGridViewT3.Columns["taname"].ReadOnly = true;
            //dataGridViewT4.Columns["Pnnote"].ReadOnly = true;
            ////gridView欄位寬度設定
            //int maxLen;
            //for (int i = 0; i < dataGridViewT1.Columns.Count; i++)
            //{
            //    dataGridViewT1.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            //    maxLen = ((DataGridViewTextBoxColumn)dataGridViewT1.Columns[i]).MaxInputLength;
            //    if (5 < maxLen && maxLen < 80)
            //    {
            //        dataGridViewT1.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            //        dataGridViewT1.Columns[i].Width = maxLen * 9;
            //    }
            //}
            ////gridView欄位寬度設定
            //for (int i = 0; i < dataGridViewT2.Columns.Count; i++)
            //{
            //    dataGridViewT2.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            //    maxLen = ((DataGridViewTextBoxColumn)dataGridViewT2.Columns[i]).MaxInputLength;
            //    if (5 < maxLen && maxLen < 80)
            //    {
            //        dataGridViewT2.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            //        dataGridViewT2.Columns[i].Width = maxLen * 9;
            //    }
            //}
            ////gridView欄位寬度設定
            //for (int i = 0; i < dataGridViewT3.Columns.Count; i++)
            //{
            //    dataGridViewT3.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            //    maxLen = ((DataGridViewTextBoxColumn)dataGridViewT3.Columns[i]).MaxInputLength;
            //    if (5 < maxLen && maxLen < 80)
            //    {
            //        dataGridViewT3.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            //        dataGridViewT3.Columns[i].Width = maxLen * 9;
            //    }
            //}
            ////gridView欄位寬度設定
            //for (int i = 0; i < dataGridViewT4.Columns.Count; i++)
            //{
            //    dataGridViewT4.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            //    maxLen = ((DataGridViewTextBoxColumn)dataGridViewT4.Columns[i]).MaxInputLength;
            //    if (5 < maxLen && maxLen < 80)
            //    {
            //        dataGridViewT4.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            //        dataGridViewT4.Columns[i].Width = maxLen * 9;
            //    }
            //}
            ////gridView欄位寬度設定
            //for (int i = 0; i < dataGridViewT5.Columns.Count; i++)
            //{
            //    dataGridViewT5.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            //    maxLen = ((DataGridViewTextBoxColumn)dataGridViewT5.Columns[i]).MaxInputLength;
            //    if (5 < maxLen && maxLen < 80)
            //    {
            //        dataGridViewT5.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            //        dataGridViewT5.Columns[i].Width = maxLen * 9;
            //    }
            //}
            ////gridView欄位寬度設定
            //for (int i = 0; i < dataGridViewT6.Columns.Count; i++)
            //{
            //    dataGridViewT6.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            //    maxLen = ((DataGridViewTextBoxColumn)dataGridViewT6.Columns[i]).MaxInputLength;
            //    if (5 < maxLen && maxLen < 80)
            //    {
            //        dataGridViewT6.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            //        dataGridViewT6.Columns[i].Width = maxLen * 9;
            //    }
            //}

            //loadDB();
            //bs.DataMember = tabControl1.SelectedTab.Name;
        }

        public void loadDB()
        {
            //try
            //{
            //    using (SqlConnection conn = new SqlConnection(Common.sqlConnString))
            //    {
            //        ds.Clear();
            //        da = new SqlDataAdapter("select * from memo01", conn);
            //        da.Fill(ds, "備註");
            //        da = new SqlDataAdapter("select AdMemo as 地址,Adr1 as 地址1,pk as 主鍵 from Addr", conn);
            //        da.Fill(ds, "常用地址");
            //        da = new SqlDataAdapter("select * from tail", conn);
            //        da.Fill(ds, "表尾註腳");
            //        da = new SqlDataAdapter("select * from pnthead", conn);
            //        da.Fill(ds, "公司印表抬頭");
            //        da = new SqlDataAdapter("select * from paynote", conn);
            //        da.Fill(ds, "付款條件");
            //        da = new SqlDataAdapter("select * from pernote", conn);
            //        da.Fill(ds, "有效期限");

            //        for (int i = 0; i < 6; i++)
            //        {
            //            dtview[i].AutoGenerateColumns = false;
            //        }
            //        dataGridViewT1.DataSource = ds.Tables["備註"];
            //        dataGridViewT2.DataSource = ds.Tables["常用地址"];
            //        dataGridViewT3.DataSource = ds.Tables["表尾註腳"];
            //        dataGridViewT4.DataSource = ds.Tables["公司印表抬頭"];
            //        dataGridViewT5.DataSource = ds.Tables["付款條件"];
            //        dataGridViewT6.DataSource = ds.Tables["有效期限"];

            //        bs.DataSource = ds;
            //    }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.ToString());
            //}
        }

        private void bt1Append_Click(object sender, EventArgs e)
        {
            //bs.DataMember = tabControl1.SelectedTab.Name;
            //bool check = true;
            //for (int i = 0; i < dtview[tabControl1.SelectedIndex].ColumnCount; i++)
            //{
            //    if (dtview[tabControl1.SelectedIndex].Rows.Count > 0)
            //    {
            //        if (tabControl1.SelectedIndex == 0)
            //            if (dtview[0]["mememo", dtview[0].Rows.Count - 1].Value.ToString() == "") check = false;
            //        if (tabControl1.SelectedIndex == 1)
            //            if (dtview[1]["地址", dtview[1].Rows.Count - 1].Value.ToString() == "") check = false;
            //        if (tabControl1.SelectedIndex == 4)
            //            if (dtview[4]["Paymemo", dtview[4].Rows.Count - 1].Value.ToString() == "") check = false;
            //        if (tabControl1.SelectedIndex == 5)
            //            if (dtview[5]["Permemo", dtview[5].Rows.Count - 1].Value.ToString() == "") check = false;
            //    }
            //}
            //if (check)
            //{
            //    try
            //    {

            //        if (tabControl1.SelectedIndex == 0)
            //            ds.Tables["備註"].Rows.Add(1);
            //        else if (tabControl1.SelectedIndex == 1)
            //            ds.Tables["常用地址"].Rows.Add(1);
            //        else if (tabControl1.SelectedIndex == 4)
            //            ds.Tables["付款條件"].Rows.Add(1);
            //        else if (tabControl1.SelectedIndex == 5)
            //            ds.Tables["有效期限"].Rows.Add(1);
            //    }
            //    catch (Exception ex)
            //    {
            //        MessageBox.Show(ex.ToString());
            //    }

            //    if (tabControl1.SelectedIndex == 1)
            //        dtview[1]["地址", dtview[1].Rows.Count - 1].Value = "";
            //    dtview[tabControl1.SelectedIndex].Focus();
            //    //dtview[tabControl1.SelectedIndex].Rows[dtview[tabControl1.SelectedIndex].Rows.Count - 1].Selected = true;

            //    if (tabControl1.SelectedIndex == 0)
            //        dtview[tabControl1.SelectedIndex].CurrentCell = dtview[tabControl1.SelectedIndex].Rows[dtview[tabControl1.SelectedIndex].Rows.Count - 1].Cells["mememo"];
            //    else if (tabControl1.SelectedIndex == 1)
            //        dtview[tabControl1.SelectedIndex].CurrentCell = dtview[tabControl1.SelectedIndex].Rows[dtview[tabControl1.SelectedIndex].Rows.Count - 1].Cells["地址"];
            //    else if (tabControl1.SelectedIndex == 4)
            //        dtview[tabControl1.SelectedIndex].CurrentCell = dtview[tabControl1.SelectedIndex].Rows[dtview[tabControl1.SelectedIndex].Rows.Count - 1].Cells["Paymemo"];
            //    else if (tabControl1.SelectedIndex == 5)
            //        dtview[tabControl1.SelectedIndex].CurrentCell = dtview[tabControl1.SelectedIndex].Rows[dtview[tabControl1.SelectedIndex].Rows.Count - 1].Cells["Permemo"];

            //    dtview[tabControl1.SelectedIndex].BeginEdit(true);


            //}
        }

        private void bt1Delete_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    if (dtview[tabControl1.SelectedIndex].SelectedRows.Count > 0)
            //    {
            //        bs.DataMember = tabControl1.SelectedTab.Name;
            //        bs.RemoveAt(dtview[tabControl1.SelectedIndex].SelectedRows[0].Index);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.ToString());
            //}
        }

        private void bt1Save_Click(object sender, EventArgs e)
        {
            //bs.DataMember = tabControl1.SelectedTab.Name;
            //bs.EndEdit();
            //using (SqlConnection conn = new SqlConnection(Common.sqlConnString))
            //{
            //    conn.Open();
            //    using (SqlCommand cmd = conn.CreateCommand())
            //    {
            //        if (tabControl1.SelectedIndex == 0)
            //        {
            //            cmd.CommandText = "delete from memo01";
            //            cmd.ExecuteNonQuery();
            //            for (int i = 0; i < dtview[0].Rows.Count; i++)
            //            {

            //                if (dtview[0]["mememo", i].Value.ToString() != "")
            //                {
            //                    cmd.CommandText = "INSERT INTO memo01"
            //                    + "(mememo) VALUES ('" + dtview[0]["mememo", i].Value.ToString() + "')";
            //                    cmd.ExecuteNonQuery();
            //                }
            //            }

            //        }
            //        else if (tabControl1.SelectedIndex == 1)
            //        {
            //            cmd.CommandText = "delete from addr";
            //            cmd.ExecuteNonQuery();
            //            for (int i = 0; i < dtview[1].Rows.Count; i++)
            //            {

            //                if (dtview[1]["地址", i].Value.ToString() != "")
            //                {
            //                    cmd.CommandText = "INSERT INTO addr"
            //                    + "(AdMemo) VALUES ('" + dtview[1]["地址", i].Value.ToString() + "')";
            //                    cmd.ExecuteNonQuery();
            //                }
            //            }
            //        }
            //        else if (tabControl1.SelectedIndex == 2)
            //        {
            //            string[] str = { "111", "121", "131", "141", "151", "311", "312", "313", "321", "322", "323", "331", "332", "333", "341", "342", "343", "351", "352", "353" };
            //            for (int i = 0; i < 20; i++)
            //            {
            //                cmd.CommandText = "Update tail set "
            //                  + "taname =N'" + dtview[2]["taname", i].Value.ToString() + "',"
            //                  + "tamemo =N'" + dtview[2]["tamemo", i].Value.ToString() + "' where tano =N'"
            //                  + str[i] + "'";
            //                cmd.ExecuteNonQuery();
            //            }
            //        }
            //        else if (tabControl1.SelectedIndex == 3)
            //        {
            //            string[] str = { "1", "2", "3", "4", "5" };
            //            for (int i = 0; i < 5; i++)
            //            {
            //                cmd.CommandText = "Update pnthead set "
            //                  + "PnNote =N'" + dtview[3]["PnNote", i].Value.ToString() + "',"
            //                   + "pntel =N'" + dtview[3]["pntel", i].Value.ToString() + "',"
            //                    + "pnaddr =N'" + dtview[3]["pnaddr", i].Value.ToString() + "',"
            //                  + "PnName =N'" + dtview[3]["PnName", i].Value.ToString() + "' where pnno =N'"
            //                  + str[i] + "'";
            //                cmd.ExecuteNonQuery();
            //            }
            //        }
            //        else if (tabControl1.SelectedIndex == 4)
            //        {
            //            cmd.CommandText = "delete from paynote";
            //            cmd.ExecuteNonQuery();
            //            for (int i = 0; i < dtview[4].Rows.Count; i++)
            //            {

            //                if (dtview[4]["Paymemo", i].Value.ToString() != "")
            //                {
            //                    cmd.CommandText = "INSERT INTO paynote"
            //                    + "(Paymemo) VALUES ('" + dtview[4]["Paymemo", i].Value.ToString() + "')";
            //                    cmd.ExecuteNonQuery();
            //                }
            //            }
            //        }
            //        else if (tabControl1.SelectedIndex == 5)
            //        {
            //            cmd.CommandText = "delete from pernote";
            //            cmd.ExecuteNonQuery();
            //            for (int i = 0; i < dtview[5].Rows.Count; i++)
            //            {

            //                if (dtview[5]["Permemo", i].Value.ToString() != "")
            //                {
            //                    cmd.CommandText = "INSERT INTO pernote"
            //                    + "(Permemo) VALUES ('" + dtview[5]["Permemo", i].Value.ToString() + "')";
            //                    cmd.ExecuteNonQuery();
            //                }
            //            }
            //        }
            //    }
            //}
            //MainForm.main.loadPhrase();
            //MessageBox.Show("儲存成功", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void bt1Cancel_Click(object sender, EventArgs e)
        {
            //loadDB();
        }

        private void bt1Exit_Click(object sender, EventArgs e)
        {
            //this.Close();
            //this.Dispose();
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //bs.DataMember = tabControl1.SelectedTab.Name;
            //loadDB();
        }

        private void FrmPhrase_KeyUp(object sender, KeyEventArgs e)
        {
            //if (e.KeyCode == Keys.F11)
            //    bt1Exit.PerformClick();
            //if (e.KeyCode == Keys.F4)
            //{
            //    if (tabControl1.SelectedIndex == 0)
            //        bt1Cancel.PerformClick();
            //    if (tabControl1.SelectedIndex == 1)
            //        bt2Cancel.PerformClick();
            //    if (tabControl1.SelectedIndex == 2)
            //        bt3Cancel.PerformClick();
            //    if (tabControl1.SelectedIndex == 3)
            //        bt4Cancel.PerformClick();
            //    if (tabControl1.SelectedIndex == 4)
            //        bt5Cancel.PerformClick();
            //}
            //if (e.KeyCode == Keys.F9)
            //{
            //    if (tabControl1.SelectedIndex == 0)
            //        bt1Save.PerformClick();
            //    if (tabControl1.SelectedIndex == 1)
            //        bt2Save.PerformClick();
            //    if (tabControl1.SelectedIndex == 2)
            //        bt3Save.PerformClick();
            //    if (tabControl1.SelectedIndex == 3)
            //        bt4Save.PerformClick();
            //    if (tabControl1.SelectedIndex == 4)
            //        bt5Save.PerformClick();
            //}
        }





    }
}
