using System.Windows.Forms;

namespace S_61.MyControl
{
    public class tableLayoutPnl : TableLayoutPanel
    {
        public tableLayoutPnl()
        {
            this.Dock = DockStyle.Fill;
            this.Margin = new Padding(0);

            //双缓冲
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true); // 禁止擦除背景. 
            SetStyle(ControlStyles.DoubleBuffer, true); // 双缓冲 
            this.UpdateStyles();
            //Row的比例
            //15
            //67
            //14
            //4

            /// <summary>
            /// 
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            /// tableLayoutPnl1.RowStyles[1].Height = 0;
            /// tableLayoutPnl1.RowStyles[1].SizeType = SizeType.Percent;
            /// tableLayoutPnl1.RowStyles[1].Height = 20F;
            /// tableLayoutPnl1.RowStyles[2].Height = 10F;
        }
    }
}
