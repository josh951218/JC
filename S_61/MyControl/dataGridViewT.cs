using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using S_61.Basic;



namespace S_61.MyControl
{
    public class dataGridViewT : DataGridView
    {
        public Color 表頭起始顏色 { get; set; }
        public Color 表頭終止顏色 { get; set; }
        public Color 選擇行的顏色 { get; set; }
        public Color 選擇單元格的顏色_唯讀 { get; set; }
        public Color 選擇單元格的顏色_可寫 { get; set; }
        public Color 編輯時單元格的顏色 { get; set; }
        public int GridWidth { get; set; }

        public dataGridViewT()
        {
            GridWidth = this.GetSetWidthByScreen();
            this.Dock = DockStyle.Fill;
            this.AllowUserToAddRows = false;
            this.AllowUserToDeleteRows = false;
            this.AllowUserToOrderColumns = true;
            this.BackgroundColor = Color.White;
            this.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            this.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            this.MultiSelect = false;
            this.ReadOnly = true;
            this.AutoGenerateColumns = false;
            this.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            //Header
            this.EnableHeadersVisualStyles = false;
            this.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.False;
            this.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.RowHeadersWidth = 20;
            //Cell
            this.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            this.DefaultCellStyle.SelectionBackColor = Color.Yellow;
            this.DefaultCellStyle.SelectionForeColor = Color.Black;
            this.ShowCellToolTips = false;
            //Row
            this.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.RowHeadersDefaultCellStyle.BackColor = Color.FromArgb(210, 214, 217);
            this.RowHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            //模式
            EditMode = DataGridViewEditMode.EditOnEnter;
            ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableWithoutHeaderText;

            表頭起始顏色 = Color.FromArgb(244, 246, 247);
            表頭終止顏色 = Color.FromArgb(194, 200, 204);
            選擇行的顏色 = Color.Yellow;
            選擇單元格的顏色_唯讀 = Color.FromArgb(229, 69, 45);
            選擇單元格的顏色_可寫 = Color.Yellow;
            編輯時單元格的顏色 = Color.Yellow;

        }
        //header
        protected override void OnCellPainting(DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex == -1)
            {
                if (!(表頭起始顏色 == Color.Transparent) && !(表頭終止顏色 == Color.Transparent) &&
                    !表頭起始顏色.IsEmpty && !表頭終止顏色.IsEmpty)
                {
                    DrawLinearGradient(e.CellBounds, e.Graphics, 表頭起始顏色, 表頭終止顏色);
                    e.Paint(e.ClipBounds, (DataGridViewPaintParts.All & ~DataGridViewPaintParts.Background));
                    e.Handled = true;
                }
                if (e.ColumnIndex == 0)
                {
                    if (限制輸入_列名_整數_小數_空值 != null)
                    {
                        numColumn.Clear();
                        for (int i = 0; i < 限制輸入_列名_整數_小數_空值.Length; i++)
                        {
                            numColumn.Add(限制輸入_列名_整數_小數_空值[i].Split(',')[0]);
                        }
                    }
                }
            }
            base.OnCellPainting(e);
        }
        //header
        private static void DrawLinearGradient(Rectangle Rec, Graphics Grp, Color Color1, Color Color2)
        {
            if (Color1 == Color2)
            {
                Brush backbrush = new SolidBrush(Color1);
                Grp.FillRectangle(backbrush, Rec);
            }
            else
            {
                using (Brush backbrush =
                    new LinearGradientBrush(Rec, Color1, Color2,
                                            LinearGradientMode.
                                                Vertical))
                {
                    Grp.FillRectangle(backbrush, Rec);
                }
            }
        }

        protected override void OnEditModeChanged(EventArgs e)
        {
            switch (Screen.PrimaryScreen.Bounds.Width)
            {
                case 800:
                    this.Font = new Font("細明體", 9F);
                    this.ColumnHeadersDefaultCellStyle.Font = new Font("細明體", 9F);
                    break;
                case 1024:
                    this.Font = new Font("細明體", 12F);
                    this.ColumnHeadersDefaultCellStyle.Font = new Font("細明體", 12F);
                    break;
                default:
                    this.Font = new Font("細明體", 12F);
                    this.ColumnHeadersDefaultCellStyle.Font = new Font("細明體", 12F);
                    break;
            }
          
            base.OnEditModeChanged(e);
        }

        protected override void OnCellEnter(DataGridViewCellEventArgs e)
        {
            if ((IsBindingOk && e.RowIndex > -1) || (DataSource == null && e.RowIndex > -1))
            {
                if (cell != null)
                {
                    cell.Style.SelectionBackColor = 選擇行的顏色;
                }
                this[0, e.RowIndex].Style.SelectionBackColor = 選擇行的顏色;

                if (this.Columns[e.ColumnIndex].ReadOnly)
                    this[e.ColumnIndex, e.RowIndex].Style.SelectionBackColor = 選擇單元格的顏色_唯讀;
                else
                    this[e.ColumnIndex, e.RowIndex].Style.SelectionBackColor = 選擇單元格的顏色_可寫;

                if (this[e.ColumnIndex, e.RowIndex] != null)
                {
                    if (this[e.ColumnIndex, e.RowIndex].ReadOnly) this[e.ColumnIndex, e.RowIndex].Style.SelectionBackColor = 選擇單元格的顏色_唯讀;
                }
            }
            base.OnCellEnter(e);
        }

        DataGridViewCell cell;
        protected override void OnCellValidated(DataGridViewCellEventArgs e)
        {
            cell = null;
            if (DataSource == null)
            {
                if (this.SelectedRows.Count > 0)
                {
                    cell = this[e.ColumnIndex, e.RowIndex];
                }
            }
            if (IsBindingOk && this.SelectedRows != null && e.RowIndex > -1)
            {
                if (this.SelectedRows.Count > 0)
                {
                    cell = this[e.ColumnIndex, e.RowIndex];
                }
            }
            if (input != null)
            {
                input.Enter -= txtEnter;
                input.KeyPress -= keyPress;
                input.KeyDown -= txtKeyDown;
                input.Leave -= txtLeave;
            }
            if (temptx != null)
            {
                temptx.KeyPress -= tpress;
                temptx.TextChanged -= tchange;
            }
            base.OnCellValidated(e);
        }

        bool IsBindingOk = false;
        protected override void OnDataBindingComplete(DataGridViewBindingCompleteEventArgs e)
        {
            //if (Screen.PrimaryScreen.Bounds.Width <= 800)
            //{
            //    //GridFont = new Font("細明體", 9F);
            //}
            //else if (Screen.PrimaryScreen.Bounds.Width <= 1024)
            //{
            //    //StatusBarFont = new Font("細明體", 11F);
            //    //ControlFont = new Font("細明體", 12F);
            //    //GridFont = new Font("細明體", 12F);
            //    this.Font = new Font("細明體", 16F, FontStyle.Bold);
            //    //CharWidth = 8;
            //}
            //else if (Screen.PrimaryScreen.Bounds.Width <= 1152)
            //{
            //    //StatusBarFont = new Font("細明體", 12F);
            //    //ControlFont = new Font("細明體", 13F);
            //    //GridFont = new Font("細明體", 13F);
            //    this.Font = new Font("細明體", 17F, FontStyle.Bold);
            //    //CharWidth = 9;
            //}
            //else if (Screen.PrimaryScreen.Bounds.Width <= 1280)
            //{
            //    //StatusBarFont = new Font("細明體", 13F);
            //    //ControlFont = new Font("細明體", 14F);
            //    //GridFont = new Font("細明體", 14F);
            //    this.Font = new Font("細明體", 18F, FontStyle.Bold);
            //    //CharWidth = 10;
            //}
            //else if (Screen.PrimaryScreen.Bounds.Width <= 1360)
            //{
            //    //StatusBarFont = new Font("細明體", 15F);
            //    //ControlFont = new Font("細明體", 16F);
            //    //GridFont = new Font("細明體", 16F);
            //    this.Font = new Font("細明體", 20F, FontStyle.Bold);
            //    //CharWidth = 11;
            //}
            //else if (Screen.PrimaryScreen.Bounds.Width <= 1480)
            //{
            //    this.Font = new Font("細明體", 18F);
            //    MessageBox.Show("Test");
            //}
            //else if (Screen.PrimaryScreen.Bounds.Width <= 1600)
            //{
            //    //StatusBarFont = new Font("細明體", 18F);
            //    //ControlFont = new Font("細明體", 19F);
            //    //GridFont = new Font("細明體", 19F);
            //    this.Font = new Font("細明體", 23F, FontStyle.Bold);
            //    //CharWidth = 13;
            //}
            //else if (Screen.PrimaryScreen.Bounds.Width <= 1720)
            //{
            //    //StatusBarFont = new Font("細明體", 19F);
            //    //ControlFont = new Font("細明體", 20F);
            //    //GridFont = new Font("細明體", 20F);
            //    this.Font = new Font("細明體", 24F, FontStyle.Bold);
            //    //CharWidth = 14;
            //}
            //else if (Screen.PrimaryScreen.Bounds.Width <= 1840)
            //{
            //    //StatusBarFont = new Font("細明體", 21F);
            //    //ControlFont = new Font("細明體", 22F);
            //    //GridFont = new Font("細明體", 22F);
            //    this.Font = new Font("細明體", 26F, FontStyle.Bold);
            //    //CharWidth = 15;
            //}
            //else
            //{
            //    //StatusBarFont = new Font("細明體", 22F);
            //    //ControlFont = new Font("細明體", 23F);
            //    //GridFont = new Font("細明體", 23F);
            //    this.Font = new Font("細明體", 27F, FontStyle.Bold);
            //    //CharWidth = 16;
            //}
            if (!IsBindingOk) IsBindingOk = true;
            base.OnDataBindingComplete(e);
        }





        private TextBox input;
        private TextBox TempTb;
        private List<string> numColumn = new List<string>();
        public string[] 限制輸入_列名_整數_小數_空值 { get; set; }
        private string[] thenum = new string[6];
        private string ColumnName { get; set; }
        private int numFirst;
        private int NumFirst
        {
            get { return numFirst; }
            set
            {
                numFirst = value;
                if (numFirst <= 0) numFirst = 1;
            }
        }
        private int NumLast { get; set; }
        private string NumChar { get; set; }//初始值
        private Boolean NumNull { get; set; }//空值
        private Boolean NumNegative { get; set; }//負數
        private Boolean NumThousands { get; set; }//千分位
        private TextBox temptx;
        ContextMenu CM = new System.Windows.Forms.ContextMenu();
        protected override void OnEditingControlShowing(DataGridViewEditingControlShowingEventArgs e)
        {
            e.CellStyle.BackColor = 編輯時單元格的顏色;
            TempTb = (TextBox)e.Control;
            if (限制輸入_列名_整數_小數_空值 == null) return;
            if (限制輸入_列名_整數_小數_空值.ToList().Find(s => s.StartsWith(CurrentCell.OwningColumn.Name)) != null)
            {
                ForNumbuerLoop();
                input = (TextBox)e.Control;
                input.Enter += txtEnter;
                input.KeyPress += keyPress;
                input.KeyDown += txtKeyDown;
                input.Leave += txtLeave;
            }
            else
            {
                maxLth = ((DataGridViewTextBoxColumn)CurrentCell.OwningColumn).MaxInputLength;
                temptx = (TextBox)e.Control;
                temptx.Validating -= new CancelEventHandler(tvalidate);
                temptx.Validating += new CancelEventHandler(tvalidate);
            }
            base.OnEditingControlShowing(e);
        }
        void txtEnter(object sender, EventArgs e)
        {
            if (input.ReadOnly)
            {
                return;
            }
            if (input.Text != "")
                if (NumThousands)
                {
                    if (ColumnName != "")
                        if (input.Text.IndexOf(',') >= 0)
                            input.Text = string.Format("{0:" + "F" + NumLast.ToString() + "}", decimal.Parse(input.Text));
                }
        }
        void txtLeave(object sender, EventArgs e)
        {
            if (input != null)
            {
                if (限制輸入_列名_整數_小數_空值.ToList().Find(s => s.StartsWith(CurrentCell.OwningColumn.Name)) != null)
                {
                    if (NumThousands)
                        if (input.Text.IndexOf(',') >= 0) return;

                    if (!NumNull)
                        if (input.Text == "") input.Text = NumChar;
                    if (!IsNumOK(input.Text, null)) input.Text = NumChar;
                    if (input.Text == "-") input.Text = NumChar;
                    else if (input.Text == ".") input.Text = NumChar;

                    if (input.Text != "")
                    {
                        if (NumThousands)
                            input.Text = string.Format("{0:" + "N" + NumLast.ToString() + "}", decimal.Parse(input.Text));
                        else
                            input.Text = string.Format("{0:" + "F" + NumLast.ToString() + "}", decimal.Parse(input.Text));
                    }
                }
                //100年10月14異動，改成驗證完成才拿掉事件
                //input.Enter -= txtEnter;
                //input.KeyPress -= keyPress;
                //input.KeyDown -= txtKeyDown;
                //input.Leave -= txtLeave;
            }
        }
        void keyPress(object sender, KeyPressEventArgs e)
        {
            if (this.ReadOnly)
            {
                base.OnKeyPress(e);
                return;
            }
            if (char.IsDigit(e.KeyChar) || e.KeyChar == '-' || e.KeyChar == (char)46)
            {
                //負數的處理
                if (!NumNegative)
                    if (e.KeyChar == '-')
                    {
                        e.Handled = true;
                        return;
                    }
                //不在是最前面輸入負號
                if (e.KeyChar == '-')
                    if (input.SelectionStart != 0)
                    {
                        e.Handled = true;
                        return;
                    }

                //小數點的處理，NumLast=0，不可輸入小數點
                if (e.KeyChar == (char)46)
                    if (NumLast == 0)
                    {
                        e.Handled = true;
                        return;
                    }
                //測試把字元插入後是否還是數值型態
                string tChar = "";
                string tStr = input.Text;
                tChar = e.KeyChar.ToString();
                //沒有選取範圍的時候
                if (input.SelectionLength == 0)
                {
                    tStr = input.Text.Insert(input.SelectionStart, tChar);
                }
                else if (input.SelectionLength > 0)
                {
                    tStr = input.Text.Substring(0, input.SelectionStart);
                    tStr += input.Text.Substring(input.SelectionStart + input.SelectionLength, input.TextLength - input.SelectionStart - input.SelectionLength);
                    tStr = tStr.Insert(input.SelectionStart, tChar);

                }
                if (!IsNumOK(tStr, null))
                {
                    e.Handled = true;
                    return;
                }
            }
            else if (e.KeyChar == (char)Keys.Back)
            {
                string TempNumStr = "";
                if (input.SelectionLength != input.TextLength && input.SelectionStart > 0)
                {
                    if (input.SelectionLength > 0)
                    {
                        TempNumStr = input.Text.Substring(0, input.SelectionStart);
                        TempNumStr += input.Text.Substring(input.SelectionStart + input.SelectionLength, input.TextLength - TempNumStr.Length - input.SelectionLength);
                    }
                    else
                    {
                        TempNumStr = input.Text.Substring(0, input.SelectionStart - 1);
                        TempNumStr += input.Text.Substring(input.SelectionStart, input.TextLength - input.SelectionStart);
                    }
                }
                else if (input.SelectionLength != input.TextLength && input.SelectionStart == 0)
                {
                    if (input.SelectionLength > 0)
                    {
                        TempNumStr = input.Text.Substring(input.SelectionLength, input.TextLength - input.SelectionLength);
                    }
                }
                if (!IsNumOK(TempNumStr, null))
                {
                    e.Handled = true;
                    return;
                }
            }
            else
            {
                e.Handled = true;
            }
        }
        void txtKeyDown(object sender, KeyEventArgs e)
        {
            string str = input.Text;
            if (e.KeyData == Keys.Delete)
            {
                string TempNumStr = "";
                if (input.SelectionLength != input.TextLength && input.SelectionStart < input.TextLength)
                {
                    if (input.SelectionLength > 0)
                    {
                        TempNumStr = input.Text.Substring(0, input.SelectionStart);
                        TempNumStr += input.Text.Substring(input.SelectionStart + input.SelectionLength, input.TextLength - TempNumStr.Length - input.SelectionLength);
                    }
                    else
                    {
                        TempNumStr = input.Text.Substring(0, input.SelectionStart);
                        TempNumStr += input.Text.Substring(input.SelectionStart + 1, input.TextLength - input.SelectionStart - 1);
                    }
                }
                else if (input.SelectionLength != input.TextLength && input.SelectionStart == input.TextLength)
                {
                    if (input.SelectionLength > 0)
                    {
                        TempNumStr = input.Text.Substring(0, input.SelectionStart);
                    }
                }
                if (!IsNumOK(TempNumStr, null))
                {
                    e.Handled = true;
                    return;
                }
            }
        }
        void ForNumbuerLoop()
        {
            for (int i = 0; i < 限制輸入_列名_整數_小數_空值.Length; i++)
            {
                thenum = 限制輸入_列名_整數_小數_空值[i].Split(',');
                if (thenum[0] == CurrentCell.OwningColumn.Name)
                {
                    ColumnName = "";
                    NumFirst = 1;
                    NumLast = 0;
                    NumChar = "0";
                    NumNull = false;
                    NumNegative = false;
                    NumThousands = false;
                    for (int j = 0; j < thenum.Length; j++)
                    {
                        if (j == 0)
                        {
                            ColumnName = thenum[j];
                        }
                        else if (j == 1)
                        {
                            numFirst = Int32.Parse(thenum[j]);
                        }
                        else if (j == 2)
                        {
                            NumLast = Int32.Parse(thenum[j]);
                        }
                        else if (j == 3)
                        {
                            if (IsNumeric(thenum[j])) NumChar = thenum[j];
                        }
                        else if (j == 4)
                        {
                            try
                            {
                                if (thenum[j] == "0") thenum[j] = "False";
                                else if (thenum[j] == "1") thenum[j] = "True";
                                thenum[j] = Boolean.Parse(thenum[j]).ToString();
                            }
                            catch { }
                            if (thenum[j] == "True")
                                NumNull = true;
                            else
                                NumNull = false;
                        }
                        else if (j == 5)
                        {
                            try
                            {
                                if (thenum[j] == "0") thenum[j] = "False";
                                else if (thenum[j] == "1") thenum[j] = "True";
                                thenum[j] = Boolean.Parse(thenum[j]).ToString();
                            }
                            catch { }
                            if (thenum[j] == "True")
                                NumNegative = true;
                            else
                                NumNegative = false;
                        }
                        else if (j == 6)
                        {
                            try
                            {
                                if (thenum[j] == "0") thenum[j] = "False";
                                else if (thenum[j] == "1") thenum[j] = "True";
                                thenum[j] = Boolean.Parse(thenum[j]).ToString();
                            }
                            catch { }
                            if (thenum[j] == "True")
                                NumThousands = true;
                            else
                                NumThousands = false;
                        }
                    }
                    return;
                }
            }
        }
        bool IsNumeric(object Expression)
        {
            if (Expression is string)
            {
                if ((Expression as string) == "") return true;
                else if ((Expression as string) == "-") return true;
                else if ((Expression as string) == ".") return true;
            }
            bool isNum;
            double retNum;
            isNum = Double.TryParse(Convert.ToString(Expression), System.Globalization.NumberStyles.Any, System.Globalization.NumberFormatInfo.InvariantInfo, out retNum);
            return isNum;
        }
        bool IsNumOK(string str, string tChar)
        {
            if (str == "") return true;
            else if (str == "-") return true;
            else if (str == ".") return true;
            if (!IsNumeric(str)) return false;
            string tempstr;
            string tempFirst;
            string tempLast;
            Boolean boolchar;
            boolchar = false;
            if (tChar == "-") boolchar = true;
            if (str.IndexOf('-') == 0)
            {
                tempstr = str.Substring(1, str.Length - 1);
                boolchar = true;
                //負號的後面是小數點
                if (str.IndexOf('.') == 0) return false;
            }
            else
                tempstr = str;
            //最前面的數字是零，第二個數字又不是小數點
            if (tempstr.Length >= 2)
                if (tempstr.Substring(0, 1) == "0")
                    if (tempstr.Substring(1, 1) != ".")
                        return false;

            if (tempstr.IndexOf('.') >= 0)
            {
                //小數點在第一位
                if (tempstr.IndexOf('.') == 0)
                {
                    tempstr = tempstr.Substring(tempstr.IndexOf('.') + 1, tempstr.Length - tempstr.IndexOf('.') - 1);
                    if (tempstr.Length > NumLast)
                        return false;
                }
                //小數點在最後一位
                else if (tempstr.IndexOf('.') + 1 == tempstr.Length)
                {
                    tempstr = tempstr.Substring(0, tempstr.Length - 1);
                    //當輸入負號的時候，允許小數點前面位數再多一位
                    if (boolchar)
                        if (tempstr.Length > NumFirst + 1) return false;
                        else
                            if (tempstr.Length > NumFirst) return false;
                }
                //小數點在中間
                else
                {

                    tempFirst = tempstr.Substring(0, tempstr.IndexOf('.'));
                    tempLast = tempstr.Substring(tempstr.IndexOf('.') + 1, tempstr.Length - tempstr.IndexOf('.') - 1);

                    //當輸入負號的時候，允許小數點前面位數再多一位
                    if (boolchar)
                    {
                        if (tempFirst.Length > NumFirst + 1) return false;
                    }
                    else
                    {
                        if (tempFirst.Length > NumFirst) return false;
                    }
                    if (tempLast.Length > NumLast)
                        return false;
                }
            }
            else
            {
                if (tempstr.Length > NumFirst)
                    return false;
            }
            return true;
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (VisibleColumnsCount == 0)
            {
                foreach (DataGridViewColumn column in this.Columns)
                {
                    if (column.Visible) VisibleColumnsCount++;
                }
            }
            if (keyData == Keys.Enter)
            {
                if (CurrentCell != null)
                {
                    //if (VisibleColumnsCount == 0)
                    //{
                    //    foreach (DataGridViewColumn column in this.Columns)
                    //    {
                    //        if (column.Visible) VisibleColumnsCount++;
                    //    }
                    //}
                    if (this.Columns[VisibleColumnsCount - 1].ReadOnly)
                    {
                        if (CurrentCell.ColumnIndex == VisibleColumnsCount && CurrentCell.RowIndex == Rows.Count - 1)
                        {
                            Button bt = (Button)FindForm().Controls.Find("gridAppend", true).FirstOrDefault();
                            if (bt != null)
                            {
                                CurrentCell = this[0, CurrentCell.RowIndex];
                                bt.Focus();
                                bt.PerformClick();
                            }
                            return true;
                        }
                    }
                    else
                    {
                        if (CurrentCell.ColumnIndex == (VisibleColumnsCount - 1) && CurrentCell.RowIndex == Rows.Count - 1)
                        {
                            Button bt = (Button)FindForm().Controls.Find("gridAppend", true).FirstOrDefault();
                            if (bt != null)
                            {
                                CurrentCell = this[0, CurrentCell.RowIndex];
                                bt.Focus();
                                bt.PerformClick();
                            }
                            return true;
                        }
                    }
                }
                SendKeys.Send("{TAB}");
                if (限制輸入_列名_整數_小數_空值 != null)
                    if (CurrentCell != null)
                        if (CurrentCell.OwningColumn != null)
                            for (int i = 0; i < 限制輸入_列名_整數_小數_空值.Length; i++)
                            {
                                thenum = 限制輸入_列名_整數_小數_空值[i].Split(',');
                                if (thenum[0] == CurrentCell.OwningColumn.Name)
                                    txtLeave(null, null);
                            }
                return true;
            }
            else if (keyData == Keys.Tab)
            {
                if (限制輸入_列名_整數_小數_空值 != null)
                    if (CurrentCell != null)
                        if (CurrentCell.OwningColumn != null)
                            for (int i = 0; i < 限制輸入_列名_整數_小數_空值.Length; i++)
                            {
                                thenum = 限制輸入_列名_整數_小數_空值[i].Split(',');
                                if (thenum[0] == CurrentCell.OwningColumn.Name)
                                    txtLeave(null, null);
                            }

            }
            else if (keyData == Keys.Up)
            {
                if (限制輸入_列名_整數_小數_空值 != null)
                {
                    if (CurrentCell != null)
                        if (CurrentCell.OwningColumn != null)
                            if (numColumn.Find(s => s == CurrentCell.OwningColumn.Name) != null)
                            {
                                if (CurrentCell.RowIndex != 0)
                                {
                                    txtLeave(null, null);
                                }
                            }
                }
                return base.ProcessCmdKey(ref msg, keyData);
            }
            else if (keyData == Keys.Down)
            {
                if (限制輸入_列名_整數_小數_空值 != null)
                {
                    if (CurrentCell != null)
                        if (CurrentCell.OwningColumn != null)
                            if (numColumn.Find(s => s == CurrentCell.OwningColumn.Name) != null)
                            {
                                if (CurrentCell.RowIndex != RowCount - 1)
                                {
                                    txtLeave(null, null);
                                }
                            }
                }
                return base.ProcessCmdKey(ref msg, keyData);
            }
            else if (keyData == Keys.Left)
            {
                if (CurrentCell != null && CurrentCell.ColumnIndex > 0)
                {
                    if (EditingControl != null)
                    {
                        ((TextBox)EditingControl).SelectionLength = 0;
                        ((TextBox)EditingControl).SelectionStart = 0;
                        return base.ProcessCmdKey(ref msg, keyData);
                    }
                }
            }
            else if (keyData == Keys.Right)
            {
                if (CurrentCell != null && CurrentCell.ColumnIndex < VisibleColumnsCount)
                {
                    if (EditingControl != null)
                    {
                        ((TextBox)EditingControl).SelectionLength = 0;
                        ((TextBox)EditingControl).SelectionStart = ((TextBox)EditingControl).Text.Length + 1;
                        return base.ProcessCmdKey(ref msg, keyData);
                    }
                }
            }
            else
            {
                if (限制輸入_列名_整數_小數_空值 != null)
                {
                    if (CurrentCell != null)
                        if (CurrentCell.OwningColumn != null)
                            if (numColumn.Find(s => s == CurrentCell.OwningColumn.Name) != null)
                            {
                                if (keyData == (Keys.Control | Keys.V)) return true;
                                if (keyData == (Keys.Control | Keys.X)) return true;
                                if (keyData == (Keys.Control | Keys.H)) return true;

                                //if (keyData == Keys.Left) { }
                                //if (CurrentCell.ColumnIndex != 0)
                                //    if (input != null && EditingControl != null)
                                //        if (input == EditingControl)
                                //            if (input.SelectionStart == 0)
                                //                if ((input.SelectionLength != input.TextLength) || input.TextLength == 0)
                                //                {
                                //                    txtLeave(null, null);
                                //                    input.SelectionStart = 0;
                                //                }
                                //if (keyData == Keys.Right) { }
                                //if (CurrentCell.ColumnIndex != ColumnCount - 1)
                                //    if (input != null && EditingControl != null)
                                //        if (input == EditingControl)
                                //            if (input.TextLength == input.SelectionStart)
                                //                if ((input.SelectionLength != input.TextLength) || input.TextLength == 0)
                                //                {
                                //                    txtLeave(null, null);
                                //                    input.SelectionStart = input.TextLength;
                                //                }
                            }
                }
                //1010106年新增
                if (!keyData.ToString().Contains("ControlKey") && keyData.ToString().StartsWith("C") && keyData.ToString().EndsWith("Control"))
                {
                    Clipboard.Clear();
                    if (this.ReadOnly)
                    {
                        if (CurrentCell != null)
                        {
                            if (CurrentCell.EditedFormattedValue.IsNullOrEmpty()) Clipboard.Clear();
                            else Clipboard.SetText(CurrentCell.EditedFormattedValue.ToString());
                        }
                    }
                    else if (temptx != null)
                    {
                        if (temptx.SelectionLength > 0)
                            Clipboard.SetText(temptx.SelectedText);
                    }
                    return true;
                }
                //1010106年新增
                if (keyData.ToString().StartsWith("V") && keyData.ToString().EndsWith("Control"))
                {
                    ftemp = true;
                    return base.ProcessCmdKey(ref msg, keyData);
                }
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        //1010106 複製貼上/中文算二碼/支援滑鼠鍵盤
        bool ftemp = false;
        int VisibleColumnsCount;
        int maxLth;
        int textByteLength;
        int selectTextByteLength;
        void tvalidate(object sender, CancelEventArgs e)
        {
            temptx.Text = temptx.Text.GetUTF8(temptx.MaxLength);
        }
        void tpress(object sender, KeyPressEventArgs e)
        {
            if (char.IsControl(e.KeyChar)) return;
            //取得原本字串和新字串相加後的Byte長度
            textByteLength = Encoding.GetEncoding(950).GetByteCount(temptx.Text + e.KeyChar.ToString());
            //取得選取字串的Byte長度, 選取字串將會被取代
            selectTextByteLength = Encoding.GetEncoding(950).GetByteCount(temptx.SelectedText);
            //相減後長度若大於設定值, 則不送出該字元
            if (textByteLength - selectTextByteLength > maxLth) e.Handled = true;
        }
        void tchange(object sender, EventArgs e)
        {
            if (ftemp)
            {
                string stemp = temptx.Text;
                if (Encoding.GetEncoding(950).GetByteCount(stemp) > maxLth)
                {
                    for (int i = 0; i < stemp.Length; i++)
                    {
                        if (Encoding.GetEncoding(950).GetByteCount(stemp.GetString(i + 1)) > maxLth)
                        {
                            temptx.Text = stemp.GetString(i);
                            break;
                        }
                    }
                }
                ftemp = false;
            }
        }
        void ctrlc(object sender, EventArgs e)
        {
            SendKeys.Send("^C");
        }
        void ctrlv(object sender, EventArgs e)
        {
            SendKeys.Send("^V");
        }















    }
}
