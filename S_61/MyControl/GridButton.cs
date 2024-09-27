using System.Drawing;
using System.Windows.Forms;
using S_61.Basic;

namespace S_61.MyControl
{
    class GridButton : Button
    {
        public GridButton()
        {
            this.Dock = DockStyle.Fill;
        }

        protected override void OnLayout(LayoutEventArgs levent)
        {
            this.FlatStyle = FlatStyle.Standard;
            switch (Screen.PrimaryScreen.Bounds.Width)
            {
                case 800:
                    this.Font = new Font("細明體", 8F);
                    break;
                case 1024:
                    this.Font = new Font("細明體", 9F);
                    break;
                default:
                    this.Font = new Font("細明體", 9F);
                    break;
            }
            base.OnLayout(levent);
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
