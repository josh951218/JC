using System;
using System.Drawing;
using System.Windows.Forms;

namespace S_61.MyControl
{
    class radioT : RadioButton
    {
        public radioT()
        {
            this.Anchor = AnchorStyles.None;
        }

        protected override void OnLayout(LayoutEventArgs levent)
        {
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

        protected override void OnCheckedChanged(EventArgs e)
        {
            if (this.Checked)
                this.BackColor = Color.LightBlue;
            else
                this.BackColor = Color.Transparent;
            base.OnCheckedChanged(e);
        }






































    }
}
