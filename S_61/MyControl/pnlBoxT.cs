using System.Drawing;
using System.Windows.Forms;

namespace S_61.MyControl
{
    class pnlBoxT : Panel
    {
        public pnlBoxT()
        {   
            this.Padding = new Padding(15);
            this.Anchor = AnchorStyles.None;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            //BevelOuter (bvLowered)
            e.Graphics.DrawLine(new Pen(SystemColors.ControlDark), 0, 0, 0, this.Height - 0);
            e.Graphics.DrawLine(new Pen(SystemColors.ControlDark), 0, 0, this.Width - 0, 0);
            e.Graphics.DrawLine(new Pen(SystemColors.ControlLightLight), this.Width - 1, this.Height - 1, 0, this.Height - 1);
            e.Graphics.DrawLine(new Pen(SystemColors.ControlLightLight), this.Width - 1, this.Height - 1, this.Width - 1, 0);

            //BevelInner (bvRaised)
            e.Graphics.DrawLine(new Pen(SystemColors.ControlLightLight), 10, 10, 10, this.Height - 10);
            e.Graphics.DrawLine(new Pen(SystemColors.ControlLightLight), 10, 10, this.Width - 10, 10);
            e.Graphics.DrawLine(new Pen(SystemColors.ControlDark), this.Width - (10 + 1), this.Height - (10 + 1), 10, this.Height - (10 + 1));
            e.Graphics.DrawLine(new Pen(SystemColors.ControlDark), this.Width - (10 + 1), this.Height - (10 + 1), this.Width - (10 + 1), 10);
            base.OnPaint(e);
        }
    }
}
