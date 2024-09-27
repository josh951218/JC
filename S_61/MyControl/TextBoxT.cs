using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using S_61.Basic;

namespace S_61.MyControl
{
    public enum TextInputMode { Insert = 0, Recover = 1 }
    public class TextBoxT : TextBox
    {
        public bool CanReSize { get; set; }
        public bool GrayView { get; set; }
        public TextInputMode InputMode { get; set; }
        private ToolTip tip = new ToolTip();
        string tipString = "雙擊滑鼠左鍵二下或按[F12]開窗查詢";
        baseForm frm = null;

        string sKey = "";
        int oLen = 0;
        int sIndex = 0;
        bool AlreadyChanged = false;

        int maxLth;
        int textByteLength;
        int selectTextByteLength;



        public TextBoxT()
        {
            this.CanReSize = true;
            this.Anchor = AnchorStyles.Left;
        }

        protected override void OnValidating(CancelEventArgs e)
        {
            this.Text = this.Text.Trim();
            base.OnValidating(e);
        }

        protected override void OnLayout(LayoutEventArgs levent)
        {
            if (FindForm() is baseForm)
                if (frm != null) if (!frm.IsLoad) return;

            maxLth = MaxLength;
            oLen = MaxLength;

            if (CanReSize)
            {
                if (this.MaxLength < 81)
                {
                    this.Width = (MaxLength * 82 / 10) + 5;
                    if (MaxLength == 1)
                        this.Width += 5;
                    if (MaxLength <= 5)
                        this.Width += 5;
                }
            }

            switch (Screen.PrimaryScreen.Bounds.Width)
            {
                case 800:
                    this.Font = new Font("細明體", 8F); break;
                case 1024:
                default:
                    this.Font = new Font("細明體", 12F); break;
            }

            if (ReadOnly)
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
            if (!ReadOnly)
            {
                AlreadyChanged = false;
                this.BackColor = Color.GreenYellow;
            }
            base.OnEnter(e);
        }

        protected override void OnLeave(EventArgs e)
        {
            if (!ReadOnly)
            {
                this.BackColor = Color.White;
                MaxLength = oLen;
                maxLth = oLen;
                sIndex = 0;
            }

            if (this.Text.Trim() != "")
                if (maxLth > MaxLength)
                    this.Text = this.Text.Substring(0, MaxLength);

            base.OnLeave(e);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (Encoding.GetEncoding(950).GetByteCount(Text) == oLen)
            {
                if (SelectionStart == Text.Length)
                {
                    if (Common.判斷KeyCode(e.KeyCode) > 0)
                    {
                        FindForm().SelectNextControl(this, true, true, true, true);
                        return;
                    }
                }
            }
            if (e.KeyData == Keys.Enter) SendKeys.Send("{tab}");
            base.OnKeyDown(e);

            if (!ReadOnly)
            {
                MaxLength = oLen + 2;
                maxLth = oLen + 2;
                sIndex = SelectionStart;
                sKey = e.KeyData.ToString().ToUpper();
            }
        }

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            if (ReadOnly) return;
            if (maxLth == 0) return;
            if (char.IsControl(e.KeyChar)) return;

            //取得原本字串和新字串相加後的Byte長度
            textByteLength = Encoding.GetEncoding(950).GetByteCount(Text + e.KeyChar.ToString());
            //取得選取字串的Byte長度, 選取字串將會被取代
            selectTextByteLength = Encoding.GetEncoding(950).GetByteCount(SelectedText);
            //相減後長度若大於設定值, 則不送出該字元
            if (textByteLength - selectTextByteLength > maxLth) e.Handled = true;

            base.OnKeyPress(e);
        }

        protected override void OnTextChanged(EventArgs e)
        {
            if (AlreadyChanged)
            {
                AlreadyChanged = false;
                SelectionStart = sIndex;
                return;
            }

            if (ReadOnly) return;
            if (oLen == 0) return;

            base.OnTextChanged(e);

            sIndex = SelectionStart;
            if (Encoding.GetEncoding(950).GetByteCount(Text) <= oLen) return;
            AlreadyChanged = true;
            Text = GetUTF8(Text, oLen);
        }

        string GetUTF8(string str, int Length)
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

        public void Init()
        {
            if (CanReSize)
            {
                if (this.MaxLength < 81)
                {
                    this.Width = (MaxLength * 82 / 10) + 5;
                    if (MaxLength == 1)
                        this.Width += 5;
                    if (MaxLength <= 5)
                        this.Width += 5;
                }
            }
        }

        int WM_PASTEDATA = 0x0302; //貼上資料的訊息
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_PASTEDATA)
            {
                sKey = "CTRLV";
                MaxLength = maxLth = oLen + Encoding.GetEncoding(950).GetByteCount(Clipboard.GetText());
            }
            base.WndProc(ref m);
        }

        int WM_CHAR = 0x0102;
        private void SendCharFromClipboard()
        {
            foreach (char c in Clipboard.GetText())
            {
                Message msg = new Message();
                msg.HWnd = Handle;
                msg.Msg = WM_CHAR;
                msg.WParam = (IntPtr)c;
                msg.LParam = IntPtr.Zero;
                base.WndProc(ref msg);
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
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















































        //protected override void OnCreateControl()
        //{
        //    if (!pVar.EditTime)
        //    {
        //        System.Drawing.Size baseSize = FindForm() is FormT ? ((FormT)FindForm()).oSize : new System.Drawing.Size(1, 1);
        //        //位置
        //        decimal percentX = (decimal)this.Location.X * 100 / baseSize.Width;
        //        decimal percentY = (decimal)this.Location.Y * 100 / baseSize.Height;
        //        System.Drawing.Size newSize = new System.Drawing.Size(Screen.PrimaryScreen.WorkingArea.Width - 10, Screen.PrimaryScreen.WorkingArea.Height - 55);
        //        this.Location = new Point((int)(newSize.Width * percentX / 100), (int)(newSize.Height * percentY / 100));
        //        //大小
        //        decimal basePercent = 0;
        //        basePercent = (decimal)(Screen.PrimaryScreen.WorkingArea.Width - 10) / baseSize.Width;
        //        this.Width = (int)(Width.ToDecimal() * basePercent);
        //        basePercent = (decimal)(Screen.PrimaryScreen.WorkingArea.Height - 55) / baseSize.Height;
        //        this.Height = (int)(Height.ToDecimal() * basePercent);
        //    }
        //    base.OnCreateControl();
        //}
























    }
}
