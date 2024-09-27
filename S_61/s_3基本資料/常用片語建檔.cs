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
    public partial class 常用片語建檔 : FormT
    {
        DataTable memo01 = new DataTable();
        DataTable addr = new DataTable();
        DataTable tail = new DataTable();
        public 常用片語建檔()
        {
            InitializeComponent();
            cn.ConnectionString = Common.sqlConnString;
        }

        private void 常用片語建檔_Load(object sender, EventArgs e)
        {
            Basic.SetParameter.TabControlItemSize(tabControl1);
            dataGridViewT1.ReadOnly = false;
            dataGridViewT2.ReadOnly = false;
            dataGridViewT3.ReadOnly = false;
            dataGridViewT3.Columns["組別編號"].ReadOnly = true;
            dataGridViewT1.SetWidthByPixel();
            dataGridViewT2.SetWidthByPixel();
            dataGridViewT3.SetWidthByPixel();
            dataGridViewT1.AutoGenerateColumns = false;
            dataGridViewT2.AutoGenerateColumns = false;
            dataGridViewT3.AutoGenerateColumns = false;
            load();
        }

        void load()
        {
            dataGridViewT1.DataSource = null;
            dataGridViewT2.DataSource = null;
            dataGridViewT3.DataSource = null;
            memo01.Clear();
            Memo01.Fill(memo01);
            addr.Clear();
            Addr.Fill(addr);
            tail.Clear();
            Tail.Fill(tail);
            dataGridViewT1.DataSource = memo01;
            dataGridViewT2.DataSource = addr;
            dataGridViewT3.DataSource = tail;
        }

        private void bt1Append_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 0)
            {
                if (dataGridViewT1.Rows.Count > 0)
                {
                    if (dataGridViewT1["mememo", dataGridViewT1.Rows.Count-1].Value.ToString() == "") return;
                }
                DataRow dr = memo01.NewRow();
                memo01.Rows.Add(dr);
            }
            else if (tabControl1.SelectedIndex == 1)
            {
                if (dataGridViewT2.Rows.Count > 0)
                {
                    if (dataGridViewT2["地址", dataGridViewT2.Rows.Count - 1].Value.ToString() == "") return;
                }
                DataRow dr = addr.NewRow();
                addr.Rows.Add(dr);
            }
        }

        private void bt1Delete_Click(object sender, EventArgs e)
        {
            try
            {
                if (tabControl1.SelectedIndex == 0)
                {
                    var r = dataGridViewT1.SelectedRows.OfType<DataGridViewRow>();
                    int index = r.Count() > 0 ? r.FirstOrDefault().Index : -1;
                    if (index == -1) return;
                    var row = memo01.Rows[index];
                    row.Delete();
                    Memo01.Update(memo01);
                }
                else if (tabControl1.SelectedIndex == 1)
                {
                    var r = dataGridViewT2.SelectedRows.OfType<DataGridViewRow>();
                    int index = r.Count() > 0 ? r.FirstOrDefault().Index : -1;
                    if (index == -1) return;
                    var row = addr.Rows[index];
                    row.Delete();
                    Addr.Update(addr);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void bt1Save_Click(object sender, EventArgs e)
        {
            try
            {
                if (tabControl1.SelectedIndex == 0)
                    Memo01.Update(memo01);
                else if (tabControl1.SelectedIndex == 1)
                    Addr.Update(addr);
                else
                    Tail.Update(tail);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return;
            }
            MessageBox.Show("儲存成功", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void bt1Cancel_Click(object sender, EventArgs e)
        {
            load();
        }

        private void bt1Exit_Click(object sender, EventArgs e)
        {
            this.Dispose();
            this.Close();
        }

        private void 常用片語建檔_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.D1:
                case Keys.NumPad1:
                    if(tabControl1.SelectedIndex == 0)
                        bt1Append.PerformClick();
                    if (tabControl1.SelectedIndex == 1)
                        bt2Append.PerformClick();
                    break;
                case Keys.D3:
                case Keys.NumPad3:
                    if (tabControl1.SelectedIndex == 0)
                        bt1Delete.PerformClick();
                    if (tabControl1.SelectedIndex == 1)
                        bt2Delete.PerformClick();
                    break;
                case Keys.D0:
                case Keys.NumPad0:
                case Keys.F11:
                    if (tabControl1.SelectedIndex == 0)
                        bt1Exit.PerformClick();
                    if (tabControl1.SelectedIndex == 1)
                        bt2Exit.PerformClick();
                    if (tabControl1.SelectedIndex == 2)
                        bt3Exit.PerformClick();
                    break;
                case Keys.F9:
                    if (tabControl1.SelectedIndex == 0)
                        bt1Save.PerformClick();
                    if (tabControl1.SelectedIndex == 1)
                        bt2Save.PerformClick();
                    if (tabControl1.SelectedIndex == 2)
                        bt3Save.PerformClick();
                    break;
                case Keys.F4:
                    if (tabControl1.SelectedIndex == 0)
                    {
                        bt1Cancel.Focus();
                        bt1Cancel.PerformClick();
                    }
                    if (tabControl1.SelectedIndex == 1)
                    {
                        bt2Cancel.Focus();
                        bt2Cancel.PerformClick();
                    }
                    if (tabControl1.SelectedIndex == 2)
                    {
                        bt3Cancel.Focus();
                        bt3Cancel.PerformClick();
                    }
                    break;
                case Keys.Escape:
                    if (tabControl1.SelectedIndex == 0)
                        bt1Exit.PerformClick();
                    if (tabControl1.SelectedIndex == 1)
                        bt2Exit.PerformClick();
                    if (tabControl1.SelectedIndex == 2)
                        bt3Exit.PerformClick();
                    break;
            }
        }




    }
}
