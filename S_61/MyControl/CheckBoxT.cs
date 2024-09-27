using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace S_61.MyControl
{
    public partial class CheckBoxT : CheckBox
    {
        public CheckBoxT()
        {

        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                this.Checked = true;
                SendKeys.Send("{tab}");
            }
            base.OnKeyDown(e);
        }
    }
}
