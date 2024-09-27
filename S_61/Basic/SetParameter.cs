using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using S_61.MyControl;

namespace S_61.Basic
{
    class SetParameter
    {
        public static void FormParameters(Form fm)
        {
            if (fm.Parent != null)
            {
                //選單與主要表單
                fm.Width = Screen.PrimaryScreen.WorkingArea.Width - 1;
                fm.Height = Screen.PrimaryScreen.WorkingArea.Height - 45;
                fm.FormBorderStyle = FormBorderStyle.FixedSingle;
                fm.MaximizeBox = false;
                fm.MinimizeBox = false;
                fm.Location = new Point(1, 1);
            }
            else
            {
                //瀏覽視窗
                fm.FormBorderStyle = FormBorderStyle.FixedSingle;
                fm.MaximizeBox = false;
                fm.MinimizeBox = false;
                if (MainForm.FrmMenu != null)
                {
                    fm.Width = MainForm.FrmMenu.Width - 10;
                    fm.Height = MainForm.FrmMenu.Height - 10;
                    fm.Location = new Point((Screen.PrimaryScreen.WorkingArea.Width - fm.Width) / 2, 55);
                }
            }
        }

        public static void RefreshFormLocation(Form fm)
        {
            //子視窗重新定位，傳入表單
            fm.WindowState = FormWindowState.Normal;
            fm.Location = new Point(1, 1);
            fm.BringToFront();
        }

        public static bool CheckMdiChild(string str)
        {
            //檢查子視窗是否存在
            bool result = false;
            for (int i = 0; i < MainForm.main.MdiChildren.Length; i++)
            {
                if (str == MainForm.main.MdiChildren[i].Name)
                {
                    result = true;//子視窗已經存在
                    break;
                }
            }
            return result;
        }

        public static void FormSize(Form fm)
        {
            //瀏覽視窗初始化，傳入表單
            fm.Width = Screen.PrimaryScreen.WorkingArea.Width - 10;
            fm.Height = Screen.PrimaryScreen.WorkingArea.Height * 7 / 10;
            fm.FormBorderStyle = FormBorderStyle.FixedSingle;
            fm.MaximizeBox = false;
            fm.MinimizeBox = false;
            fm.Location = new Point((Screen.PrimaryScreen.Bounds.Width - fm.Width) / 2, (Screen.PrimaryScreen.Bounds.Height - fm.Height) / 2);
        }

        public static void FontSize(Control Ctrl)
        {
            //設定字體大小，傳入控製項
            switch (Screen.PrimaryScreen.Bounds.Width)
            {
                case 800:
                    Ctrl.Font = new Font("細明體", 9);
                    break;
                case 1024:
                    Ctrl.Font = new Font("細明體", 12);
                    break;
                default:
                    Ctrl.Font = new Font("細明體", 12);
                    break;
            }
            if (!Ctrl.HasChildren) return;
            foreach (Control c in Ctrl.Controls)
            {
                switch (Screen.PrimaryScreen.Bounds.Width)
                {
                    case 800:
                        c.Font = new Font("細明體", 9);
                        break;
                    case 1024:
                        c.Font = new Font("細明體", 12);
                        break;
                    default:
                        c.Font = new Font("細明體", 12);
                        break;
                }
            }
        }


        public static void TabControlItemSize(TabControl TabControl1)
        {
            switch (Screen.PrimaryScreen.Bounds.Width)
            {
                case 800:
                    TabControl1.Font = new Font("細明體", 9F);
                    TabControl1.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
                    TabControl1.ItemSize = new Size(160, 20);
                    break;
                case 1024:
                    TabControl1.Font = new Font("細明體", 12F);
                    TabControl1.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
                    TabControl1.ItemSize = new Size(180, 25);
                    break;
                default:
                    TabControl1.Font = new Font("細明體", 12F);
                    TabControl1.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
                    TabControl1.ItemSize = new Size(180, 25);
                    break;
            }
        }

        public static void SetBtnEnable(List<btnT> list)
        {
            //設定按鈕回應點擊，傳入按鈕List
            list.ForEach(o => o.Enabled = true);
        }

        public static void SetBtnDisable(List<btnT> list)
        {
            //設定按鈕不響應點擊，傳入按鈕List
            list.ForEach(btn => btn.Enabled = false);
        }

        public static void SetTxtEnable(Control Ctrl)
        {
            //設定文字框可讀寫，傳入容器
            if (Ctrl.HasChildren)
            {
                foreach (Control c in Ctrl.Controls)
                {
                    if (c is TextBox)
                    {
                        (c as TextBox).ReadOnly = false;
                        c.Text = "";
                    }
                }
            }
        }

        public static void SetTxtDisable(Control Ctrl)
        {
            //設定文字框唯讀，傳入容器
            if (Ctrl.HasChildren)
            {
                foreach (Control c in Ctrl.Controls)
                {
                    if (c is TextBox)
                        ((TextBox)c).ReadOnly = true;
                }
            }
        }

        public static void SetTxtEnableNotClear(Control ctrl)
        {
            foreach (Control c in ctrl.Controls)
            {
                if (c is TextBox)
                    (c as TextBox).ReadOnly = false;
            }
        }

        public static void SetTxtEnableNotClear(List<Control> list)
        {
            //設定文字框可讀寫，傳入文字框List
            list.ForEach(t => ((TextBox)t).ReadOnly = false);
        }

        public static void SetTxtEnable(List<Control> list)
        {
            //設定文字框可讀寫，傳入文字框List
            list.ForEach(t =>
            {
                ((TextBox)t).ReadOnly = false;
                ((TextBox)t).Text = "";
            });
        }

        public static void SetTxtDisable(List<Control> list)
        {
            //設定文字框唯讀，傳入文字框List
            list.ForEach(t => ((TextBox)t).ReadOnly = true);
        }

        public static void TextBoxClear(List<Control> list)
        {
            //清除文字框，傳入文字框List
            list.ForEach(t => t.Text = "");
        }

        public static void SetRadioEnable(Control ctrl)
        {
            foreach (Control c in ctrl.Controls)
            {
                if (c is RadioButton)
                    (c as RadioButton).Enabled = true;
                if (c.HasChildren) SetRadioEnable(c);
            }
        }

        public static void SetRadioDisable(Control ctrl)
        {
            foreach (Control c in ctrl.Controls)
            {
                if (c is RadioButton)
                    (c as RadioButton).Enabled = false;
                if (c.HasChildren) SetRadioDisable(c);
            }
        }

        public static void Drtotext(DataRow dr, Control Ctrl)
        {
            //設定文字框可讀寫，傳入容器
            if (Ctrl.HasChildren)
            {
                foreach (Control c in Ctrl.Controls)
                {
                    if (c is TextBox)
                    {
                        if (dr != null)
                        {
                            c.Text = dr[c.Name].ToString();
                        }
                        else
                        {
                            c.Text = "";
                        }
                    }
                }
            }
        }

        public static void SetTxtClear(Control Ctrl)
        {
            //清空文字框
            if (Ctrl.HasChildren)
            {
                foreach (Control c in Ctrl.Controls)
                {
                    if (c is TextBox)
                    {
                        c.Text = "";
                    }
                }
            }
        }












































    }
}
