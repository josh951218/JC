using System.Drawing;
using System.Windows.Forms;

namespace S_61.MyControl
{
    class GroupBoxT : GroupBox
    {
        public GroupBoxT()
        {
            this.Dock = DockStyle.Fill;
        }

        protected override void OnLayout(LayoutEventArgs levent)
        {
            this.Margin = new Padding(10, 5, 10, 5);
            this.Padding = new Padding(3);
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
            base.OnLayout(levent);
        }
    }
}
