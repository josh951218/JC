using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using S_61.Model;
using S_61.MyControl;
using S_61.Basic;

namespace S_61.Basic
{
    public static class myExtension
    {
        public static void SetParaeter(this baseForm frm, ViewMode mode = ViewMode.Big)
        {
            //frm.SuspendLayout();
            //var c = frm.FindControl("tableLayoutPnl1");
            //if (c.IsNotNull()) c.SuspendLayout();

            var p = frm.FindControl("statusStrip1");
            if (p.IsNotNull())
            {
                StatusStrip sp = (StatusStrip)p;
                for (int i = 0; i < sp.Items.Count; i++)
                {
                    sp.Items[i].Font = new Font("微軟正黑體", 12F);
                }
            }
            if (frm.Parent != null)
            {
                //選單與主要表單
                frm.FormBorderStyle = FormBorderStyle.FixedSingle;
                frm.MaximizeBox = false;
                frm.MinimizeBox = false;
                frm.Size = MainForm.FrmMenu.Size;
                frm.Location = new Point(1, 1);
            }
            else
            {
                if (mode == ViewMode.Big)
                {
                    //瀏覽視窗
                    frm.FormBorderStyle = FormBorderStyle.FixedSingle;
                    frm.MaximizeBox = false;
                    frm.MinimizeBox = false;
                    if (MainForm.FrmMenu != null)
                    {
                        frm.Width = MainForm.FrmMenu.ClientSize.Width - 5;
                        frm.Height = MainForm.FrmMenu.ClientSize.Height + 14;
                    }
                }
                else
                {
                    //瀏覽視窗
                    frm.FormBorderStyle = FormBorderStyle.FixedSingle;
                    frm.MaximizeBox = false;
                    frm.MinimizeBox = false;
                    frm.Width = Screen.PrimaryScreen.WorkingArea.Width - 10;
                    frm.Height = Screen.PrimaryScreen.WorkingArea.Height * 7 / 10;
                }
            }
        }
        public static void SetLocation(this baseForm frm)
        {
            if (frm.Parent != null) frm.Location = new Point(1, 1);
            else
            {
                if (frm.ViewStyle == ViewMode.Big)
                {
                    //frm.Location = new Point((Screen.PrimaryScreen.WorkingArea.Width - frm.Width) / 2, 55);
                    frm.StartPosition = FormStartPosition.CenterParent;
                }
                else frm.Location = new Point((Screen.PrimaryScreen.Bounds.Width - 4 - frm.Width) / 2, (Screen.PrimaryScreen.Bounds.Height + 6 - frm.Height) / 2);

            }
            frm.ResumeLayout(false);
            frm.PerformLayout();
            var p = frm.FindControl("tableLayoutPnl1");
            if (p.IsNotNull())
            {
                p.Width = frm.Width - 6;
                p.Height = frm.Height - 28;
                p.ResumeLayout(false);
                p.PerformLayout();
            }
            Application.DoEvents();
        }
        public static void MdiParaeter(this baseForm frm)
        {
            //frm.SuspendLayout();
            //var c = frm.FindControl("tableLayoutPnl1");
            //if (c.IsNotNull()) c.SuspendLayout();

            var p = frm.FindControl("statusStrip1");
            if (p.IsNotNull())
            {
                StatusStrip sp = (StatusStrip)p;
                for (int i = 0; i < sp.Items.Count; i++)
                {
                    sp.Items[i].Font = new Font("微軟正黑體", 12F);
                }
            }
            if (frm.Parent != null)
            {
                //選單與主要表單
                frm.FormBorderStyle = FormBorderStyle.FixedSingle;
                frm.MaximizeBox = false;
                frm.MinimizeBox = false;
                frm.Size = MainForm.FrmMenu.Size;
                frm.Location = new Point(1, 1);
            }
            else
            {
                //瀏覽視窗
                frm.FormBorderStyle = FormBorderStyle.FixedSingle;
                frm.MaximizeBox = false;
                frm.MinimizeBox = false;
                if (MainForm.FrmMenu != null)
                {
                    frm.Width = MainForm.FrmMenu.ClientSize.Width - 5;
                    frm.Height = MainForm.FrmMenu.ClientSize.Height + 14;
                }
            }
        }
        public static void MdiLocation(this baseForm frm)
        {
            if (frm.Parent != null) frm.Location = new Point(1, 1);
            else
            {
                frm.StartPosition = FormStartPosition.CenterParent;
            }
            frm.ResumeLayout(false);
            frm.PerformLayout();
            var p = frm.FindControl("tableLayoutPnl1");
            if (p.IsNotNull())
            {
                p.Width = frm.Width - 6;
                p.Height = frm.Height - 28;
                p.ResumeLayout(false);
                p.PerformLayout();
            }
            Application.DoEvents();
        }
        public static Control FindControl(this Form ctrl, string name)
        {
            Control c = null;
            var p = ctrl.Controls.Find(name, true);
            if (p.Length > 0) c = p[0];
            return c;
        }
        public static bool IsNotNull(this object obj)
        {
            if (obj != null) return true;
            return false;
        }

        public static void AddLine(this TextBox tx)
        {
            if (tx.Text.Trim().Length == 7)
            {
                tx.Text = tx.Text.Substring(0, 3) + "/" + tx.Text.Substring(3, 2) + "/" + tx.Text.Substring(5);
            }
            else if (tx.Text.Trim().Length == 8)
            {
                tx.Text = tx.Text.Substring(0, 4) + "/" + tx.Text.Substring(4, 2) + "/" + tx.Text.Substring(6);
            }
        }
        public static void RemoveLine(this TextBox tx)
        {
            tx.Text = tx.Text.Replace("/", "");
        }
        public static bool IsDateTime(this TextBox tx)
        {
            TaiwanCalendar tw = new TaiwanCalendar();
            bool flag = false;
            string year = "";
            string month = "";
            string day = "";
            DateTime t;
            if (Common.User_DateTime == 1)
            {
                if (tx.Text.Length != 7) return false;
                year = new string(tx.Text.Take(3).ToArray());
                month = new string(tx.Text.Skip(3).Take(2).ToArray()) + "/";
                day = new string(tx.Text.Skip(5).Take(2).ToArray());
                if (DateTime.Now.Year == tw.GetYear(DateTime.Now))
                {
                    //系統為民國年
                    year += "/";
                }
                else
                {
                    //系統為西元年
                    year = year.ToDecimal() + 1911 + "/";
                }
            }
            else
            {
                if (tx.Text.Length != 8) return false;
                year = new string(tx.Text.Take(4).ToArray());
                month = new string(tx.Text.Skip(4).Take(2).ToArray()) + "/";
                day = new string(tx.Text.Skip(6).Take(2).ToArray());
                if (DateTime.Now.Year == tw.GetYear(DateTime.Now))
                {
                    //系統為民國年
                    year = year.ToDecimal() - 1911 + "/";
                }
                else
                {
                    //系統為西元年
                    year += "/";
                }
            }
            flag = DateTime.TryParse(year + month + day, out t);
            return flag;
        }



        public static void Search(this DataGridView grid, string TColumnName, string TValue)
        {
            if (TColumnName.IsNullOrEmpty()) return;
            if (TValue.IsNullOrEmpty()) return;
            if (grid.Rows.Count == 0) return;

            int index = -1;
            List<DataGridViewRow> list = grid.Rows.OfType<DataGridViewRow>().ToList();
            index = list.FindIndex(r => r.Cells[TColumnName].Value.ToString() == TValue);
            if (index == -1)
            {
                index = list.FindLastIndex(r => string.CompareOrdinal(TValue, r.Cells[TColumnName].Value.ToString()) > 0) + 1;
                if (index == 0) index = 0;
                else if (index >= grid.Rows.Count) index = grid.Rows.Count - 1;

            }
            grid.FirstDisplayedScrollingRowIndex = index;
            grid.CurrentCell = grid[0, index];
            grid.Rows[index].Selected = true;
        }
        public static void Search(this DataGridView grid, string Column1, string Value1, string Column2, string Value2)
        {
            if (Value1.Trim().Length > 0 && Value2.Trim().Length > 0)
            {
                List<DataGridViewRow> list = grid.Rows.OfType<DataGridViewRow>().ToList();
                int index = list.FindIndex(r => r.Cells[Column1].Value.ToString() == Value1 && r.Cells[Column2].Value.ToString() == Value2);
                if (index != -1)
                {
                    grid.FirstDisplayedScrollingRowIndex = index;
                    grid.CurrentCell = grid[0, index];
                    grid.Rows[index].Selected = true;
                }
                else
                {
                    index = list.FindIndex(r => r.Cells[Column1].Value.ToString() == Value1);
                    int index2 = list.FindIndex(r => r.Cells[Column2].Value.ToString() == Value2);
                    if (index != -1)
                    {
                        grid.Search(Column1, Value1);
                    }
                    else
                    {
                        if (index2 != -1)
                        {
                            grid.Search(Column2, Value2);
                        }
                        else
                        {
                            grid.Search(Column1, Value1);
                        }
                    }
                }
            }
            else if (Value1.Trim().Length > 0 && Value2.Trim().Length == 0)
            {
                grid.Search(Column1, Value1);
            }
            else if (Value1.Trim().Length == 0 && Value2.Trim().Length > 0)
            {
                grid.Search(Column2, Value2);
            }
            else
            {
                return;
            }
        }
        public static void SearchForDate(this DataGridView grid, string TColumnName, string TValue)
        {
            if (TColumnName.IsNullOrEmpty()) return;
            if (TValue.IsNullOrEmpty()) return;
            if (grid.Rows.Count == 0) return;

            int index = -1;
            List<DataGridViewRow> list = grid.Rows.OfType<DataGridViewRow>().ToList();
            index = list.FindIndex(r => r.Cells[TColumnName].Value.ToString() == TValue);
            if (index == -1)
            {
                index = list.FindLastIndex(r => string.CompareOrdinal(r.Cells[TColumnName].Value.ToString(), TValue) > 0);
                if (index <= 0) index = 0;
                else if (index >= grid.Rows.Count) index = grid.Rows.Count - 1;

            }
            grid.FirstDisplayedScrollingRowIndex = index;
            grid.CurrentCell = grid[0, index];
            grid.Rows[index].Selected = true;
        }
        public static void SearchForDate(this DataGridView grid, string Column1, string Value1, string Column2, string Value2)
        {
            if (Value1.Trim().Length > 0 && Value2.Trim().Length > 0)
            {
                List<DataGridViewRow> list = grid.Rows.OfType<DataGridViewRow>().ToList();
                int index = list.FindIndex(r => r.Cells[Column1].Value.ToString() == Value1 && r.Cells[Column2].Value.ToString() == Value2);
                if (index != -1)
                {
                    grid.FirstDisplayedScrollingRowIndex = index;
                    grid.CurrentCell = grid[0, index];
                    grid.Rows[index].Selected = true;
                }
                else
                {
                    index = list.FindIndex(r => r.Cells[Column1].Value.ToString() == Value1);
                    int index2 = list.FindIndex(r => r.Cells[Column2].Value.ToString() == Value2);
                    if (index != -1)
                    {
                        grid.Search(Column1, Value1);
                    }
                    else
                    {
                        if (index2 != -1)
                        {
                            grid.Search(Column2, Value2);
                        }
                        else
                        {
                            grid.Search(Column1, Value1);
                        }
                    }
                }
            }
            else if (Value1.Trim().Length > 0 && Value2.Trim().Length == 0)
            {
                grid.Search(Column1, Value1);
            }
            else if (Value1.Trim().Length == 0 && Value2.Trim().Length > 0)
            {
                grid.SearchForDate(Column2, Value2);
            }
            else
            {
                return;
            }
        }
        public static void SetWidthByPixel(this DataGridView grid)
        {
            if (grid.IsNullOrEmpty()) return;
            int maxLen;
            dataGridViewT gd = (dataGridViewT)grid;
            for (int i = 0; i < grid.Columns.Count; i++)
            {
                gd.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                maxLen = ((DataGridViewTextBoxColumn)gd.Columns[i]).MaxInputLength;
                //if (5 < maxLen && maxLen < 80)
                //{
                gd.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                //    gd.Columns[i].Width = maxLen * 9;
                //}
                gd.Columns[i].Width = (int)((maxLen * gd.GridWidth) + 7 + (double)gd.GridWidth * ((double)14 / (double)18));
            }
        }
        public static int GetSetWidthByScreen(this Control c)
        {
            int w = 1;
            int ScreenWidth = Screen.PrimaryScreen.Bounds.Width;
            if (ScreenWidth <= 800) w = 6;
            else if (ScreenWidth <= 1024) w = 8;
            //else if (ScreenWidth <= 1152) w = 9;
            //else if (ScreenWidth <= 1280) w = 10;
            //else if (ScreenWidth <= 1360) w = 11;
            //else if (ScreenWidth <= 1480) w = 12;
            //else if (ScreenWidth <= 1600) w = 13;
            //else if (ScreenWidth <= 1720) w = 14;
            //else if (ScreenWidth <= 1840) w = 15;
            else w = 8;
            return w;
        }

        public static void SetFontSizebyScreen(this Control c)
        {
            int ScreenWidth = Screen.PrimaryScreen.Bounds.Width;

            if (ScreenWidth <= 800)
            {
                //StatusBarFont = new Font("細明體", 8F);
                //ControlFont = new Font("細明體", 9F);
                //GridFont = new Font("細明體", 9F);
                c.Font = new Font("細明體", 13F, FontStyle.Bold);
                //CharWidth = 6;
            }
            else if (ScreenWidth <= 1024)
            {
                //StatusBarFont = new Font("細明體", 11F);
                //ControlFont = new Font("細明體", 12F);
                //GridFont = new Font("細明體", 12F);
                c.Font = new Font("細明體", 16F, FontStyle.Bold);
                //CharWidth = 8;
            }
            else if (ScreenWidth <= 1152)
            {
                //StatusBarFont = new Font("細明體", 12F);
                //ControlFont = new Font("細明體", 13F);
                //GridFont = new Font("細明體", 13F);
                c.Font = new Font("細明體", 17F, FontStyle.Bold);
                //CharWidth = 9;
            }
            else if (ScreenWidth <= 1280)
            {
                //StatusBarFont = new Font("細明體", 13F);
                //ControlFont = new Font("細明體", 14F);
                //GridFont = new Font("細明體", 14F);
                c.Font = new Font("細明體", 18F, FontStyle.Bold);
                //CharWidth = 10;
            }
            else if (ScreenWidth <= 1360)
            {
                //StatusBarFont = new Font("細明體", 15F);
                //ControlFont = new Font("細明體", 16F);
                //GridFont = new Font("細明體", 16F);
                c.Font = new Font("細明體", 20F, FontStyle.Bold);
                //CharWidth = 11;
            }
            else if (ScreenWidth <= 1480)
            {
                //StatusBarFont = new Font("細明體", 17F);
                //ControlFont = new Font("細明體", 18F);
                //GridFont = new Font("細明體", 18F);
                c.Font = new Font("細明體", 22F, FontStyle.Bold);
                //CharWidth = 12;
            }
            else if (ScreenWidth <= 1600)
            {
                //StatusBarFont = new Font("細明體", 18F);
                //ControlFont = new Font("細明體", 19F);
                //GridFont = new Font("細明體", 19F);
                c.Font = new Font("細明體", 23F, FontStyle.Bold);
                //CharWidth = 13;
            }
            else if (ScreenWidth <= 1720)
            {
                //StatusBarFont = new Font("細明體", 19F);
                //ControlFont = new Font("細明體", 20F);
                //GridFont = new Font("細明體", 20F);
                c.Font = new Font("細明體", 24F, FontStyle.Bold);
                //CharWidth = 14;
            }
            else if (ScreenWidth <= 1840)
            {
                //StatusBarFont = new Font("細明體", 21F);
                //ControlFont = new Font("細明體", 22F);
                //GridFont = new Font("細明體", 22F);
                c.Font = new Font("細明體", 26F, FontStyle.Bold);
                //CharWidth = 15;
            }
            else
            {
                //StatusBarFont = new Font("細明體", 22F);
                //ControlFont = new Font("細明體", 23F);
                //GridFont = new Font("細明體", 23F);
                c.Font = new Font("細明體", 27F, FontStyle.Bold);
                //CharWidth = 16;
            }
        }


        public static void LoadImg(this PictureBox pic)
        {
            OpenFileDialog ofile = new OpenFileDialog();
            ofile.FileName = "";
            ofile.Filter = "圖片檔 (*.jpg;*.bmp)|*.jpg;*.bmp";

            if (ofile.ShowDialog() == DialogResult.OK)
            {
                using (FileStream filesm = new FileStream(ofile.FileName, FileMode.Open))
                {
                    byte[] buffer = new byte[filesm.Length];
                    filesm.Read(buffer, 0, buffer.Length);
                    using (MemoryStream memorysm = new MemoryStream(buffer))
                    {
                        pic.Image = Bitmap.FromStream(memorysm);
                    }
                }
            }
            else
            {
                pic.Clear();
            }
            pic.Tag = ofile.FileName;
        }
        public static void LoadImg(this PictureBox pic, byte[] buffer)
        {
            if (buffer != null && buffer.Length > 0)
            {
                using (MemoryStream mstream = new MemoryStream(buffer))
                {
                    pic.Image = Bitmap.FromStream(mstream);
                }
            }
            else
            {
                pic.Clear();
            }
        }
        public static void Clear(this PictureBox pic)
        {
            pic.Image = null;
            pic.Tag = "";
        }
        public static byte[] ImageToByte(this PictureBox pic)
        {
            if (pic.Tag.IsNullOrEmpty()) return new byte[0];

            FileStream fs = new FileStream(pic.Tag.ToString(), FileMode.Open);
            byte[] buffer = new byte[fs.Length];
            fs.Read(buffer, 0, buffer.Length);
            fs.Close();
            return buffer;

       
            
        }



        public static bool IsZeroOrEmpty(this string str)
        {
            if (string.IsNullOrEmpty(str)) return true;
            try
            {
                decimal d = Convert.ToDecimal(str);
                if (d == 0)
                    return true;
                else
                    return false;
            }
            catch { return true; }
        }
        public static string GetString(this string str, int Length)
        {
            str = str.Trim();
            if (str.Length > Length)
                return str.Substring(0, Length);
            else
                return str;
        }
        public static string GetUTF8(this string str, int Length)
        {
            if (Length < 0) return "";
            int len = 0;
            string s = "";
            for (int i = 1; i <= str.Length; i++)
            {
                s = new string(str.Take(i).ToArray());
                if (Encoding.GetEncoding(950).GetByteCount(s) > Length)
                {
                    break;
                }
                else if (Encoding.GetEncoding(950).GetByteCount(s) == Length)
                {
                    return s;
                }
                else
                {
                    len = i;
                }
            }
            s = new string(str.Take(len).ToArray());
            return s;
        }
        public static string takeString(this string str, int Length)
        {
            if (str == null) str = "";
            return new string(str.Take(Length).ToArray());
        }
        public static string skipString(this string str, int Length)
        {
            if (str == null) str = "";
            return new string(str.Skip(Length).ToArray());
        }
        public static bool BigThen(this string str, string str1)
        {
            if (str.Trim().Length > 0 && str1.Trim().Length > 0)
            {
                return string.CompareOrdinal(str.Trim(), str1.Trim()) > 0;
            }
            else return false;
        }



        public static bool IsNullOrEmpty(this object obj)
        {
            bool IsNullorEmpty = false;
            if (obj == null)
                IsNullorEmpty = true;
            else
            {
                try
                {
                    if (obj.ToString().Trim() == "")
                    {
                        obj = "";
                        IsNullorEmpty = true;
                    }
                    else
                        IsNullorEmpty = false;
                }
                catch
                {
                    IsNullorEmpty = true;
                }
            }
            return IsNullorEmpty;
        }
        public static decimal ToDecimal(this object obj)
        {
            if (obj == null) return 0;
            decimal d = 0;
            decimal.TryParse(obj.ToString(), out d);
            return d;
        }























    }
}
