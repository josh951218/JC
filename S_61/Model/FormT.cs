using System;
using System.Windows.Forms;
using S_61.Basic;
using S_61.MyControl;

namespace S_61.Model
{
    public class FormT : baseForm
    {
        private dataGridViewT grid;

        public FormT() { }

        protected override void OnLoad(EventArgs e)
        {
            IsLoad = true;
            if (!Common.EditTime) this.SetLocation();
            base.OnLoad(e);
        }

        protected override void OnShown(EventArgs e)
        {
            if (grid == null)
            {
                Control[] ctrls = this.Controls.Find("dataGridViewT1", true);
                if (ctrls.Length > 0)
                {
                    grid = (dataGridViewT)ctrls[0];
                }
            }
            base.OnShown(e);
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            #region 原本
            //Common.keyData = keyData;
            //Common.ActiveControl = ActiveControl;
            //if (!Text.Contains("["))
            //    if (keyData == Keys.D2)
            //    {
            //        var p = Controls.Find("btnModify", true);
            //        var btn = p.Length > 0 ? (Button)p[0] : null;
            //        if (btn != null) btn.PerformClick();
            //        return true;
            //    }
            //switch (keyData)
            //{
            //    case Keys.Up:
            //        if (grid.IsNotNull())
            //        {
            //            if (grid.EditingControl.IsNotNull())
            //            {
            //                if (grid.SelectedRows.Count > 0 && grid.SelectedRows[0].Index == 0)
            //                {
            //                    SelectNextControl(ActiveControl, false, true, true, true);
            //                }
            //            }
            //            else
            //            {
            //                if (grid.Focused)
            //                {
            //                    if (grid.SelectedRows.Count > 0 && grid.SelectedRows[0].Index == 0)
            //                    {
            //                        SelectNextControl(ActiveControl, false, true, true, true);
            //                    }
            //                }
            //                else
            //                {
            //                    SelectNextControl(ActiveControl, false, true, true, true);
            //                }
            //            }
            //        }
            //        else
            //            SelectNextControl(ActiveControl, false, true, true, true);
            //        break;
            //    case Keys.Down:
            //        if (grid.IsNotNull())
            //        {
            //            if (grid.EditingControl.IsNotNull())
            //            {
            //                if (grid.SelectedRows.Count > 0 && grid.SelectedRows[0].Index == grid.Rows.Count - 1)
            //                {
            //                    SelectNextControl(ActiveControl, true, true, true, true);
            //                }
            //            }
            //            else
            //            {
            //                if (grid.Focused)
            //                {
            //                    if (grid.SelectedRows.Count > 0 && grid.SelectedRows[0].Index == grid.Rows.Count - 1)
            //                    {
            //                        SelectNextControl(ActiveControl, true, true, true, true);
            //                    }
            //                }
            //                else
            //                {
            //                    SelectNextControl(ActiveControl, true, true, true, true);
            //                }
            //            }
            //        }
            //        else
            //            SelectNextControl(ActiveControl, true, true, true, true);
            //        break;
            //}
            //return base.ProcessCmdKey(ref msg, keyData);
            #endregion
            Common.keyData = keyData;
            if (keyData == Keys.Up)
            {
                var p = ActiveControl.Parent.Parent;
                if (p.IsNotNull() && p is dataGridViewT)
                {
                    dataGridViewT t = ((dataGridViewT)p);
                    var index = t.Rows.GetFirstRow(DataGridViewElementStates.Selected);
                    if (index == 0)
                    {
                        SelectNextControl(t, false, true, true, true);
                        ActiveControl.Focus();
                        if (ActiveControl is TextBox) ((TextBox)ActiveControl).SelectAll();
                    }
                }
                else if (ActiveControl is dataGridViewT)
                {
                    var index = ((dataGridViewT)ActiveControl).Rows.GetFirstRow(DataGridViewElementStates.Selected);
                    if (index == 0)
                    {
                        SelectNextControl(ActiveControl, false, true, true, true);
                        ActiveControl.Focus();
                        if (ActiveControl is TextBox) ((TextBox)ActiveControl).SelectAll();
                    }
                    else if (((dataGridViewT)ActiveControl).Rows.Count == 0)
                    {
                        SelectNextControl(ActiveControl, false, true, true, true);
                        ActiveControl.Focus();
                        if (ActiveControl is TextBox) ((TextBox)ActiveControl).SelectAll();
                    }
                }
                else
                {
                    SelectNextControl(ActiveControl, false, true, true, true);
                }
            }
            else if (keyData == Keys.Down)
            {
                var p = ActiveControl.Parent.Parent;
                if (p.IsNotNull() && p is dataGridViewT)
                {
                    dataGridViewT t = ((dataGridViewT)p);
                    var index = t.Rows.GetFirstRow(DataGridViewElementStates.Selected);
                    if (index == t.Rows.Count - 1)
                    {
                        SelectNextControl(t, true, true, true, true);
                        ActiveControl.Focus();
                        if (ActiveControl is TextBox) ((TextBox)ActiveControl).SelectAll();
                    }
                }
                else if (ActiveControl is dataGridViewT)
                {
                    var index = ((dataGridViewT)ActiveControl).Rows.GetFirstRow(DataGridViewElementStates.Selected);
                    if (index == ((dataGridViewT)ActiveControl).Rows.Count - 1)
                    {
                        SelectNextControl(ActiveControl, true, true, true, true);
                        ActiveControl.Focus();
                        if (ActiveControl is TextBox) ((TextBox)ActiveControl).SelectAll();
                    }
                }
                else
                {
                    SelectNextControl(ActiveControl, true, true, true, true);
                }
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        //protected override void OnKeyUp(KeyEventArgs e)
        //{
        //    if (Common.ActiveControl is Button || Common.ActiveControl is RadioButton)
        //    {
        //        switch (e.KeyCode)
        //        {
        //            case Keys.Up:
        //                SelectNextControl(ActiveControl, false, true, true, true);
        //                break;
        //            case Keys.Down:
        //                SelectNextControl(ActiveControl, true, true, true, true);
        //                break;
        //        }
        //    }
        //    base.OnKeyUp(e);
        //}

        //protected override void CreateHandle()
        //{
        //    if (!pVar.EditTime)
        //    {
        //        oSize = this.Size;
        //        this.Size = new System.Drawing.Size(Screen.PrimaryScreen.WorkingArea.Width - 10, Screen.PrimaryScreen.WorkingArea.Height - 55);
        //    }
        //    base.CreateHandle();
        //}





























    }
}
