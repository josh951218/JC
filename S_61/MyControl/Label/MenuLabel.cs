using System;
using System.Drawing;
using System.Windows.Forms;
using S_61.Basic;

namespace S_61.MyControl
{
    public class MenuLabel : Label
    {
        public MenuLabel()
        {
        }

        protected override void OnLayout(LayoutEventArgs levent)
        {
            this.SetFontSizebyScreen();
            this.Margin = new Padding(3, 3, 3, 3);
            this.TextAlign = ContentAlignment.MiddleCenter;
            this.BackColor = Color.Transparent;
            this.ForeColor = Color.FromArgb(0, 0, 0);
            this.Dock = DockStyle.None;
            this.AutoSize = true;
            this.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            base.OnLayout(levent);
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            this.ForeColor = Color.Blue;
            this.Cursor = Cursors.Hand;
            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            this.ForeColor = Color.FromArgb(0, 0, 0);
            this.Cursor = Cursors.Default;
            this.BorderStyle = System.Windows.Forms.BorderStyle.None;
            base.OnMouseLeave(e);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            this.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.ForeColor = Color.Blue;
            base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            this.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.ForeColor = Color.FromArgb(0, 0, 0);
            this.Cursor = Cursors.Default;
            base.OnMouseUp(e);
        }

        protected override void OnMouseHover(EventArgs e)
        {
            foreach (Control helppic in FindForm().Controls)
            {
                if (helppic is PictureBox)
                {
                    helppic.Visible = true;
                    helppic.Location = new Point(this.Location.X + this.Width + 8, this.Location.Y + 60 + (this.Height - helppic.Height) / 2);
                    helppic.BringToFront();
                    helppic.Tag = "";
                    helppic.Cursor = Cursors.Hand;
                    helppic.Tag = this.Parent.Tag + @"\";
                    foreach (char a in this.Text)
                    {
                        if (!(a == ' '))
                            helppic.Tag = helppic.Tag + a.ToString();
                    }
                }
            }
            base.OnMouseHover(e);
        }

    }
}
