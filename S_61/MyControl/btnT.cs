using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using S_61.Basic;
using S_61.Properties;

namespace S_61.MyControl
{
    public class btnT : Button
    {
        string btnState;
        private bool mcheck;
        private int mcount;
        List<bool> mchecklist = new List<bool>();
        List<Control> aaa = new List<Control>();
        List<string> bbb = new List<string>();

        public btnT()
        {
            this.FlatStyle = FlatStyle.Popup;
            this.BackgroundImageLayout = ImageLayout.Center;
            this.BackColor = SystemColors.Control;
        }

        protected override void OnCreateControl()
        {
            switch (Screen.PrimaryScreen.Bounds.Width)
            {
                case 800:
                    this.Size = new Size(53, 60);
                    this.BackgroundImage = (Bitmap)Resources.ResourceManager.GetObject(new string(this.Name.Skip(3).ToArray()).ToLower() + "800"); break;
                case 1024:
                default:
                    this.Size = new Size(69, 79);
                    this.BackgroundImage = (Bitmap)Resources.ResourceManager.GetObject(new string(this.Name.Skip(3).ToArray()).ToLower() + "1024"); break;
            }
            base.OnCreateControl();
        }

        protected override void OnEnabledChanged(EventArgs e)
        {
            try
            {
                if (this.Enabled)
                {
                    switch (Screen.PrimaryScreen.Bounds.Width)
                    {
                        case 800:
                            this.BackgroundImage = (Bitmap)Resources.ResourceManager.GetObject(new string(this.Name.Skip(3).ToArray()).ToLower() + "800"); break;
                        case 1024:
                        default:
                            this.BackgroundImage = (Bitmap)Resources.ResourceManager.GetObject(new string(this.Name.Skip(3).ToArray()).ToLower() + "1024"); break;
                    }
                }
                else
                {
                    switch (Screen.PrimaryScreen.Bounds.Width)
                    {
                        case 800:
                            this.BackgroundImage = (Bitmap)Resources.ResourceManager.GetObject(new string(this.Name.Skip(3).ToArray()).ToLower() + "800d"); break;
                        case 1024:
                        default:
                            this.BackgroundImage = (Bitmap)Resources.ResourceManager.GetObject(new string(this.Name.Skip(3).ToArray()).ToLower() + "1024d"); break;
                    }
                }
            }
            catch { }
            base.OnEnabledChanged(e);
        }

        protected override void OnClick(EventArgs e)
        {
            Form fm = FindForm();
            if (fm != null)
            {
                if (!fm.Tag.IsNullOrEmpty())
                {
                    if (!IsPowerful(fm.Tag.ToString()))
                    {
                        MessageBox.Show("無使用權限", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }

                switch (Name)
                {
                    case "btnAppend":
                        btnState = "-[新增]";
                        if (!fm.Text.Contains("-["))
                            fm.Text += btnState;
                        else
                        {
                            fm.Text = fm.Text.Remove(fm.Text.IndexOf("-["));
                            fm.Text += btnState;
                        }
                        break;
                    case "btnDuplicate":
                        btnState = "-[複製]";
                        if (!fm.Text.Contains("-["))
                            fm.Text += btnState;
                        else
                        {
                            fm.Text = fm.Text.Remove(fm.Text.IndexOf("-["));
                            fm.Text += btnState;
                        }
                        break;
                    case "btnModify":
                        mcheck = true;
                        mcount = 0;
                        mchecklist.Clear();
                        loopformodify(fm);
                        foreach (bool a in mchecklist)
                        {
                            if (a == false)
                                mcount++;
                        }
                        if (mcount == mchecklist.Count)
                            mcheck = false;
                        if (mcheck)
                        {
                            btnState = "-[修改]";
                            if (!fm.Text.Contains("-["))
                                fm.Text += btnState;
                            else
                            {
                                fm.Text = fm.Text.Remove(fm.Text.IndexOf("-["));
                                fm.Text += btnState;
                            }
                        }
                        break;
                    case "btnSave":
                        if (fm.Text.Contains("-[複製]") || fm.Text.Contains("-[修改]"))
                        {
                            btnState = "-[新增]";
                            fm.Text = fm.Text.Remove(fm.Text.IndexOf("-["));
                            fm.Text += btnState;
                        }
                        break;
                    case "btnCancel":
                        btnState = "";
                        if (fm.Text.Contains("-["))
                            fm.Text = fm.Text.Remove(fm.Text.IndexOf("-["));
                        break;
                    case "btnExit":
                        break;
                }
            }
            base.OnClick(e);
        }

        void loopformodify(Control a)
        {
            foreach (Control x in a.Controls)
            {
                if (x is TextBox)
                    if (x.Name.ToUpper().EndsWith("NO"))
                    {
                        if (x.Text == "")
                        {
                            mchecklist.Add(false);
                        }
                        else
                        {
                            mchecklist.Add(true);
                        }
                    }
                loopformodify(x);
            }
        }

        bool IsPowerful(string Tkey)
        {
            bool ispower = false;
            string name = Name.Substring(3);
            string scno = "";
            if (name == "Append") scno = "sc01";        //新增
            else if (name == "Modify") scno = "sc02";   //修變
            else if (name == "Delete") scno = "sc04";   //刪除
            else if (name == "Print") scno = "sc05";    //列印
            else if (name == "Duplicate") scno = "sc06";//複製
            else if (name == "Brow") scno = "sc07";     //瀏覽
            else if (name == "***") scno = "sc08";      //全開放
            else if (name == "****") scno = "sc03";     //查詢
            else return true;
            try
            {
                using (SqlConnection conn = new SqlConnection(Common.sqlConnString))
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = "select m.scname,d.* from scrit as m,scritd as d where '0'='0' "
                            + " and d.taname='" + Tkey.Trim() + "'"
                            + " and m.scname=d.scname "
                            + " and m.scname='" + Common.User_Name + "' COLLATE Chinese_Taiwan_Stroke_BIN";

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.HasRows && reader.Read())
                            {
                                //有打V才有權限
                                if (reader[scno].ToString() != "")
                                    ispower = true;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            return ispower;
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            Common.ActiveControl = this;
            if (keyData == Keys.Up) return true;
            if (keyData == Keys.Down) return true;
            return base.ProcessCmdKey(ref msg, keyData);
        }



















    }
}
