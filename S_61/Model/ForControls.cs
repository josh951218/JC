using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using S_61.MyControl;

namespace S_61.Model
{
    class ForControls
    {
        public static void ForRadio(Label LB)
        {
            foreach (Control a in LB.Parent.Controls)
            {
                if (a is RadioButton)
                    if ("lb" + a.Name == LB.Name)
                    {
                        if ((a as RadioButton).Enabled == false) return;
                    }
            }
            foreach (Control a in LB.Parent.Controls)
            {
                if (a is RadioButton)
                    if ("lb" + a.Name == LB.Name)
                    {
                        (a as RadioButton).Checked = true;
                    }
                if (a is Label)
                {
                    a.BackColor = System.Drawing.Color.Transparent;
                }
            }
            LB.BackColor = System.Drawing.Color.LightBlue;

        }

        public static void ForRadio(RadioButton RB)
        {
            foreach (Control lb in RB.Parent.Controls)
            {
                if (lb is Label)
                    if ("lb" + RB.Name == lb.Name)
                    {
                        lb.BackColor = System.Drawing.Color.LightBlue;
                    }
                    else
                    {
                        lb.BackColor = System.Drawing.Color.Transparent;
                    }
            }
        }

    }
}
