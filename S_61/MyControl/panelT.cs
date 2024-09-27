using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace S_61.MyControl
{
    class panelT : Panel
    {
        public panelT()
        {
            this.Anchor = (AnchorStyles)(AnchorStyles.Top | AnchorStyles.Bottom);
        }

        protected override void OnLayout(LayoutEventArgs levent)
        {
            switch (Screen.PrimaryScreen.Bounds.Width)
            {
                case 800:
                    this.Margin = new Padding(0, 1, 0, 1);
                    break;
                case 1024:
                    this.Margin = new Padding(0, 2, 0, 2);
                    break;
                default:
                    this.Margin = new Padding(0, 2, 0, 2);
                    break;
            }
            List<btnT> buttons = this.Controls.Cast<btnT>().Where(b => b is btnT).ToList();
            buttons.Reverse();
            for (int i = 0; i < buttons.Count; i++)
            {
                switch (Screen.PrimaryScreen.Bounds.Width)
                {
                    case 800:
                        buttons[i].Location = new Point(i * 53, 0);
                        break;
                    case 1024:
                        buttons[i].Location = new Point(i * 69, 0);
                        break;
                    default:
                        buttons[i].Location = new Point(i * 69, 0);
                        break;
                }
            }
            switch (Screen.PrimaryScreen.Bounds.Width)
            {
                case 800:
                    this.Width = buttons.Count * 53 + 10;
                    break;
                case 1024:
                    this.Width = buttons.Count * 69 + 10;
                    break;
                default:
                    this.Width = buttons.Count * 69 + 10;
                    break;
            }
            switch (Screen.PrimaryScreen.Bounds.Height)
            {
                case 600:
                    this.Height = 60;
                    break;
                case 768:
                    this.Height = 79;
                    break;
                default:
                    this.Height = 79;
                    break;
            }
            base.OnLayout(levent);
        }

    }
}
