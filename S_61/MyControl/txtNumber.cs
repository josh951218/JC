using System;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;


namespace S_61.MyControl
{
    public class txtNumber : TextBox
    {
        private int numFirst;
        public int NumFirst
        {
            get
            {
                return numFirst;
            }
            set
            {
                numFirst = value;
                if (numFirst <= 0) numFirst = 1;
            }
        }
        public int NumLast { get; set; }
        public Boolean NumNegative { get; set; }
        public Boolean NumNull { get; set; }
        public Boolean NumThousands { get; set; }
        public bool GrayView { get; set; }
        public bool CanReSize { get; set; }
        private ToolTip tip = new ToolTip();
        string tipString = "雙擊滑鼠左鍵二下或按[F12]開窗查詢";

        public txtNumber()
        {
            Anchor = (AnchorStyles)(AnchorStyles.Left | AnchorStyles.Right);
            ReadOnly = true;
            TextAlign = HorizontalAlignment.Right;
            CanReSize = true;

            ContextMenu screenMenu = new ContextMenu();
            this.ContextMenu = screenMenu;
        }

        protected override void OnLayout(LayoutEventArgs levent)
        {
            MaxLength = 0;
            int L = NumFirst + NumLast + 1;

            if (CanReSize)
            {
                if (L < 81)
                {
                    this.Width = (L * 82 / 10) + 5;
                    if (L <= 5)
                        this.Width += 5;
                }
            }

            switch (Screen.PrimaryScreen.Bounds.Width)
            {
                case 800:
                    this.Font = new Font("細明體", 8F);
                    break;
                case 1024:
                    this.Font = new Font("細明體", 12F);
                    break;
                default:
                    this.Font = new Font("細明體", 12F);
                    break;
            }

            //設定唯讀
            if (this.ReadOnly)
            {
                this.TabStop = false;
                if (GrayView)
                    this.BackColor = Color.Silver;
                else
                    this.BackColor = Color.FromArgb(243, 243, 243);
            }
            base.OnLayout(levent);
        }

        protected override void OnEnter(EventArgs e)
        {
            if (this.ReadOnly)
            {
                base.OnEnter(e);
                return;
            }
            if (Text != "")
                if (NumThousands)
                    this.Text = string.Format("{0:" + "F" + NumLast.ToString() + "}", decimal.Parse(Text));

            this.BackColor = Color.GreenYellow;
            base.OnEnter(e);
        }

        protected override void OnLeave(EventArgs e)
        {
            if (this.ReadOnly)
            {
                base.OnLeave(e);
                return;
            }
            this.BackColor = Color.White;

            if (NumThousands)
                if (Text.IndexOf(',') >= 0) return;

            if (!NumNull)
                if (Text == "") Text = "0";
            if (!IsNumOK(Text, null)) Text = "0";
            if (Text == "-") Text = "0";
            else if (Text == ".") Text = "0";

            if (Text != "")
            {
                if (NumThousands)
                    this.Text = string.Format("{0:" + "N" + NumLast.ToString() + "}", decimal.Parse(Text));
                else
                    this.Text = string.Format("{0:" + "F" + NumLast.ToString() + "}", decimal.Parse(Text));
            }

            base.OnLeave(e);
        }

        protected override void OnKeyDown(System.Windows.Forms.KeyEventArgs e)
        {
            string str = Text;
            if (e.KeyData == Keys.Delete)
            {
                string TempNumStr = "";
                if (SelectionLength != TextLength && SelectionStart < TextLength)
                {
                    if (SelectionLength > 0)
                    {
                        TempNumStr = Text.Substring(0, SelectionStart);
                        TempNumStr += Text.Substring(SelectionStart + SelectionLength, TextLength - TempNumStr.Length - SelectionLength);
                    }
                    else
                    {
                        TempNumStr = Text.Substring(0, SelectionStart);
                        TempNumStr += Text.Substring(SelectionStart + 1, TextLength - SelectionStart - 1);
                    }
                }
                else if (SelectionLength != TextLength && SelectionStart == TextLength)
                {
                    if (SelectionLength > 0)
                    {
                        TempNumStr = Text.Substring(0, SelectionStart);
                    }
                }
                if (!IsNumOK(TempNumStr, null))
                {
                    e.Handled = true;
                    return;
                }
            }
            if (e.KeyData == Keys.Enter)
                SendKeys.Send("{tab}");
            base.OnKeyDown(e);
        }

        protected override void OnKeyPress(System.Windows.Forms.KeyPressEventArgs e)
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
                    if (SelectionStart != 0)
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
                string tStr = Text;
                tChar = e.KeyChar.ToString();
                //沒有選取範圍的時候
                if (SelectionLength == 0)
                {
                    tStr = Text.Insert(SelectionStart, tChar);
                }
                else if (SelectionLength > 0)
                {
                    tStr = Text.Substring(0, SelectionStart);
                    tStr += Text.Substring(SelectionStart + SelectionLength, TextLength - SelectionStart - SelectionLength);
                    tStr = tStr.Insert(SelectionStart, tChar);

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
                if (SelectionLength != TextLength && SelectionStart > 0)
                {
                    if (SelectionLength > 0)
                    {
                        TempNumStr = Text.Substring(0, SelectionStart);
                        TempNumStr += Text.Substring(SelectionStart + SelectionLength, TextLength - TempNumStr.Length - SelectionLength);
                    }
                    else
                    {
                        TempNumStr = Text.Substring(0, SelectionStart - 1);
                        TempNumStr += Text.Substring(SelectionStart, TextLength - SelectionStart);
                    }
                }
                else if (SelectionLength != TextLength && SelectionStart == 0)
                {
                    if (SelectionLength > 0)
                    {
                        TempNumStr = Text.Substring(SelectionLength, TextLength - SelectionLength);
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
            base.OnKeyPress(e);
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


        //唯讀和非唯讀時的顏色切換
        protected override void OnReadOnlyChanged(EventArgs e)
        {
            if (!this.ReadOnly)//可讀寫
            {
                this.TabStop = true;
                this.BackColor = Color.White;
            }
            if (this.ReadOnly) //唯讀
            {
                this.TabStop = false;
                if (GrayView)
                    this.BackColor = Color.Silver;
                else
                    this.BackColor = Color.FromArgb(243, 243, 243);
            }
            base.OnReadOnlyChanged(e);
        }

        protected override void OnTextChanged(EventArgs e)
        {
            if (ReadOnly == true) return;
            try
            {
                if (this.Focused == false)
                {
                    if (NumThousands)
                        if (Text.IndexOf(',') >= 0) return;
                    if (!NumNull)
                        if (Text == "") Text = "0";
         
                    if (Text == "-") Text = "0";
                    else if (Text == ".") Text = "0";

                    if (Text != "")
                    {
                        if (NumThousands)
                            this.Text = string.Format("{0:" + "N" + NumLast.ToString() + "}", decimal.Parse(Text));
                        else
                            this.Text = string.Format("{0:" + "F" + NumLast.ToString() + "}", decimal.Parse(Text));
                    }
                }
            }
            catch { }
            base.OnTextChanged(e);
        }

        protected override void OnMouseUp(MouseEventArgs mevent)
        {
            if (!this.ReadOnly)
                if (this.SelectionLength == 0)
                    this.SelectAll();
            base.OnMouseUp(mevent);
        }

        //快捷鍵屏蔽
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Control | Keys.V)) return true;
            if (keyData == (Keys.Control | Keys.X)) return true;
            if (keyData == (Keys.Control | Keys.H)) return true;
            if (keyData == Keys.F12)
            {
                if (IsAnyDoubleClickandNotReadOnly()) this.OnDoubleClick(null);
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        protected override void OnDoubleClick(EventArgs e)
        {
            base.OnDoubleClick(e);
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            if (IsAnyDoubleClickandNotReadOnly()) tip.SetToolTip(this, tipString);
            base.OnMouseEnter(e);
        }

        bool IsAnyDoubleClickandNotReadOnly()
        {
            if (ReadOnly) return false;
            BindingFlags mPropertyFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static | BindingFlags.NonPublic;
            Type t = typeof(System.Windows.Forms.Control);
            PropertyInfo propertyInfo = t.GetProperty("Events", mPropertyFlags);
            EventHandlerList eventHandlerList = (EventHandlerList)propertyInfo.GetValue(this, null);

            BindingFlags mFieldFlags = BindingFlags.Static | BindingFlags.NonPublic;
            FieldInfo fieldInfo = (typeof(Control)).GetField("EventDoubleClick", mFieldFlags);

            Delegate d = eventHandlerList[fieldInfo.GetValue(this)];
            if (d != null && d.GetInvocationList().Length > 0) return true;
            return false;
        }

    }
}