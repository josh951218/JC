using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace S_61.MyControl
{
    class tableLayoutPnl_for_main : TableLayoutPanel
    {
        public tableLayoutPnl_for_main()
        {
            this.Dock = DockStyle.Fill;
            this.Margin = new Padding(0);
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


        private Color 起始顏色 = Color.FromArgb(215, 227, 239);
        private Color 終止顏色 = Color.FromArgb(215, 227, 239);
        protected override void OnPaint(PaintEventArgs e)
        {
                if (!(起始顏色 == Color.Transparent) && !(終止顏色 == Color.Transparent) &&
                    !起始顏色.IsEmpty && !終止顏色.IsEmpty)
                {
                    Rectangle rowBounds = new Rectangle(
            0,
            0,
            this.Width,
            this.Height);
                    DrawLinearGradient(rowBounds, e.Graphics, 起始顏色, 終止顏色);
                    //e.Paint(e.ClipBounds, (DataGridViewPaintParts.All & ~DataGridViewPaintParts.Background));
                    //e.Handled = true;
                }
                base.OnPaint(e);
        }
        //繪圖用
        private static void DrawLinearGradient(Rectangle Rec, Graphics Grp, Color Color1, Color Color2)
        {
            if (Color1 == Color2)
            {
                Brush backbrush = new SolidBrush(Color1);
                Grp.FillRectangle(backbrush, Rec);
            }
            else
            {
                using (Brush backbrush =
                    new LinearGradientBrush(Rec, Color1, Color2,
                                            LinearGradientMode.
                                                Vertical))
                {
                    Grp.FillRectangle(backbrush, Rec);
                }
            }
        }
    }
}
