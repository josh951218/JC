using System.Drawing;
using System.Windows.Forms;

namespace S_61.MyControl.textbox
{
    public class StatusBar1 : Label
    {
        public StatusBar1()
        {
            this.Anchor = (AnchorStyles)(AnchorStyles.Left | AnchorStyles.Right);
        }
        protected override void OnLayout(LayoutEventArgs levent)
        {
            this.AutoSize = false;
            switch (Screen.PrimaryScreen.Bounds.Width)
            {
                case 800:
                    if (this.Parent != null)
                        this.Parent.Height = 20;
                    break;
                case 1024:
                    if (this.Parent != null)
                        this.Parent.Height = 25;
                    break;
                default:
                    if (this.Parent != null)
                        this.Parent.Height = 25;
                    break;
            }


            switch (Screen.PrimaryScreen.Bounds.Width)
            {
                case 800:
                    this.Font = new Font("細明體", 8F);
        
                    this.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
                    this.Height = 20;
                    break;
                case 1024:
                    this.Font = new Font("細明體", 11F);
      
                    this.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
                    this.Height = 25;
                    break;
                default:
                    this.Font = new Font("細明體", 11F);
            
                    this.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
                    this.Height = 25;
                    break;
            }
            this.BackColor = Color.Transparent;
            this.TextAlign = ContentAlignment.MiddleLeft;
            //this.ForeColor = Color.FromArgb(9, 32, 97);
            this.ForeColor = Color.FromArgb(0, 0, 0);
            this.Dock = DockStyle.Bottom;
            this.ImageAlign = ContentAlignment.MiddleCenter;
            base.OnLayout(levent);

        }
    }
}
