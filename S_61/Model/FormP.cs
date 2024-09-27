using System;
using System.Linq;
using System.Windows.Forms;
using S_61.Basic;
using S_61.MyControl;
using System.Data;

namespace S_61.Model
{
    public class FormP : baseForm
    {
        public Control UpControl { get; set; }
        public Control DownControl { get; set; }
        dataGridViewT dataGridViewT1;
        Button gridAppend;
        Button gridDelete;
        Button gridInsert;
        Button btnCancel;
        string itno = "";
        int index = 0;

        public FormP() { }

        protected override void OnLoad(EventArgs e)
        {
            IsLoad = true;
            if (!Common.EditTime) this.MdiLocation();
            base.OnLoad(e);
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            if (Common.ActiveControl is Button || Common.ActiveControl is RadioButton)
            {
                switch (e.KeyCode)
                {
                    case Keys.Up:
                        if (Common.ActiveControl.Name == DownControl.Name)
                            dataGridViewT1.Focus();
                        else
                            SelectNextControl(ActiveControl, false, true, true, true);
                        break;
                    case Keys.Down:
                        SelectNextControl(ActiveControl, true, true, true, true);
                        break;
                }
            }
            base.OnKeyUp(e);
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            Common.keyData = keyData;
            Common.ActiveControl = ActiveControl;

            if (keyData == Keys.Up)
            {
                //一般的TextBox，往上一個控制項跳
                if (ActiveControl is TextBox && dataGridViewT1.EditingControl == null)
                {
                    SelectNextControl(ActiveControl, false, true, true, true);
                    return base.ProcessCmdKey(ref msg, keyData);
                }
                //如果DataGridView焦點且列數為0，往UpControl跳
                if (dataGridViewT1.Rows.Count == 0)
                {
                    if (UpControl != null)
                        UpControl.Focus();
                    return true;
                }
                //第0列，往UpControl跳
                if (dataGridViewT1.SelectedRows[0].Index == 0)
                {
                    if (UpControl != null)
                        UpControl.Focus();
                    return true;
                }
                //其它，往上一列跳
                return base.ProcessCmdKey(ref msg, keyData);
            }

            if (keyData == Keys.Down)
            {
                //一般的TextBox，往下一個控制項跳
                if (ActiveControl is TextBox && dataGridViewT1.EditingControl == null)
                {
                    SelectNextControl(ActiveControl, true, true, true, true);
                    return base.ProcessCmdKey(ref msg, keyData);
                }
                //DataGridView 唯讀
                if (dataGridViewT1.ReadOnly)
                    return base.ProcessCmdKey(ref msg, keyData);
                //DataGridView 編輯欄位，往下跳
                if (dataGridViewT1.EditingControl != null)
                {
                    //如果DataGridView焦點且列數為0，往DownControl跳
                    if (dataGridViewT1.Rows.Count == 0)
                    {
                        if (DownControl != null)
                            DownControl.Focus();
                        return true;
                    }
                    //有列數的話，取索引值
                    index = dataGridViewT1.SelectedRows[0].Index;
                    if (dataGridViewT1.Columns["產品編號"] != null)
                    {
                        if (dataGridViewT1.CurrentCell.OwningColumn.Name == "產品編號")
                        {
                            itno = dataGridViewT1.EditingControl.Text;
                            if (itno.Length == 0)
                            {
                                //產品編號空值，直接往下，最後一列則往DownControl跳
                                if (index == dataGridViewT1.Rows.Count - 1)
                                {
                                    if (DownControl != null)
                                        DownControl.Focus();
                                    return true;
                                }
                                else
                                    return base.ProcessCmdKey(ref msg, keyData);
                            }
                            else
                            {
                                if (pVar.ItemValidate(itno))
                                {
                                    //產品編號合法，新增行或是往下跳，最後一列則往DownControl跳
                                    dataGridViewT1.CommitEdit(DataGridViewDataErrorContexts.Commit);
                                    if (index == dataGridViewT1.Rows.Count - 1)
                                    {
                                        var rows = dataGridViewT1.Rows.OfType<DataGridViewRow>().Where(r => r.Cells["產品編號"].Value.IsNullOrEmpty());
                                        if (rows.Count() > 0)
                                        {
                                            if (DownControl != null) DownControl.Focus();
                                            return true;
                                        }
                                        else
                                        {
                                            gridAppend.PerformClick();
                                            return base.ProcessCmdKey(ref msg, keyData);
                                        }
                                    }
                                    else
                                    {
                                        return base.ProcessCmdKey(ref msg, keyData);
                                    }
                                    //var rows = dataGridViewT1.Rows.OfType<DataGridViewRow>().Where(r => r.Cells["產品編號"].Value.IsNullOrEmpty());
                                    //if (rows.Count() > 0)
                                    //{
                                    //    if (index == dataGridViewT1.Rows.Count - 1)
                                    //    {
                                    //        if (DownControl != null)
                                    //            DownControl.Focus();
                                    //        return true;
                                    //    }
                                    //    else
                                    //        return base.ProcessCmdKey(ref msg, keyData);
                                    //}
                                    //else
                                    //{
                                    //    gridAppend.PerformClick();
                                    //    return base.ProcessCmdKey(ref msg, keyData);
                                    //}
                                }
                                else
                                {
                                    //產品編號不合法，啟動驗證事件
                                    SendKeys.Send("{tab}");
                                    return true;
                                }
                            }
                        }
                        else
                        {
                            if (index == dataGridViewT1.Rows.Count - 1)
                            {
                                itno = dataGridViewT1["產品編號", index].EditedFormattedValue.ToString();
                                var rows = dataGridViewT1.Rows.OfType<DataGridViewRow>().Where(r => r.Cells["產品編號"].Value.IsNullOrEmpty());
                                if (rows.Count() > 0)
                                {
                                    if (DownControl != null) DownControl.Focus();
                                    return true;
                                }
                                else
                                {
                                    gridAppend.PerformClick();
                                    return base.ProcessCmdKey(ref msg, keyData);
                                }
                            }
                            else
                            {
                                return base.ProcessCmdKey(ref msg, keyData);
                            }
                        }
                    }
                    else
                    {
                        //有Grid沒產品編號
                        if (dataGridViewT1.SelectedRows.Count > 0)
                        {
                            if (dataGridViewT1.SelectedRows[0].Index == dataGridViewT1.Rows.Count - 1)
                            {
                                if (DownControl != null) DownControl.Focus();
                                return true;
                            }
                        }
                    }
                    return base.ProcessCmdKey(ref msg, keyData);
                }
                else
                {
                    //DataGridView 唯讀欄位 往下跳
                    if (dataGridViewT1.Rows.Count == 0)
                    {
                        if (DownControl != null)
                            DownControl.Focus();
                        return true;
                    }

                    index = dataGridViewT1.SelectedRows[0].Index;
                    if (dataGridViewT1.Columns["產品編號"] != null)
                    {
                        if (index == dataGridViewT1.Rows.Count - 1)
                        {
                            itno = dataGridViewT1["產品編號", index].Value.ToString();
                            var rows = dataGridViewT1.Rows.OfType<DataGridViewRow>().Where(r => r.Cells["產品編號"].Value.IsNullOrEmpty());
                            if (rows.Count() > 0)
                            {
                                if (DownControl != null) DownControl.Focus();
                                return true;
                            }
                            else
                            {
                                gridAppend.PerformClick();
                                return base.ProcessCmdKey(ref msg, keyData);
                            }
                        }
                        else
                        {
                            return base.ProcessCmdKey(ref msg, keyData);
                        }
                    }
                    else
                    {
                        //有Grid沒產品編號
                        if (dataGridViewT1.SelectedRows.Count > 0)
                        {
                            if (dataGridViewT1.SelectedRows[0].Index == dataGridViewT1.Rows.Count - 1)
                            {
                                if (DownControl != null) DownControl.Focus();
                                return true;
                            }
                        }
                    }
                    return base.ProcessCmdKey(ref msg, keyData);
                }
            }

            if (keyData == Keys.F2)
            {
                if (dataGridViewT1.ReadOnly)
                    return base.ProcessCmdKey(ref msg, keyData);
                else
                {
                    if (dataGridViewT1.EditingControl != null)
                    {
                        if (dataGridViewT1.EditingControl.Text.Length > 0)
                        {
                            if (pVar.ItemValidate(dataGridViewT1["產品編號", dataGridViewT1.SelectedRows[0].Index].EditedFormattedValue.ToString()))
                            {
                                gridAppend.PerformClick();
                                return true;
                            }
                            else
                            {
                                //產品編號不合法，啟動驗證事件
                                SendKeys.Send("{tab}");
                                return true;
                            }
                        }
                        else
                            return base.ProcessCmdKey(ref msg, keyData);
                    }
                    gridAppend.PerformClick();
                }
            }

            if (keyData == Keys.F5)
            {
                if (dataGridViewT1.ReadOnly)
                    return base.ProcessCmdKey(ref msg, keyData);
                else
                {
                    if (dataGridViewT1.EditingControl != null)
                    {
                        if (dataGridViewT1.EditingControl.Text.Length > 0)
                        {
                            if (pVar.ItemValidate(dataGridViewT1["產品編號", dataGridViewT1.SelectedRows[0].Index].EditedFormattedValue.ToString()))
                            {
                                gridInsert.PerformClick();
                                return true;
                            }
                            else
                            {
                                //產品編號不合法，啟動驗證事件
                                SendKeys.Send("{tab}");
                                return true;
                            }
                        }
                        else
                            return base.ProcessCmdKey(ref msg, keyData);
                    }
                    gridInsert.PerformClick();
                }
            }

            if (keyData == Keys.F3)
            {
                gridDelete.PerformClick();
                return base.ProcessCmdKey(ref msg, keyData);
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        protected override void OnShown(EventArgs e)
        {
            if (dataGridViewT1 == null)
                dataGridViewT1 = this.FindControl("dataGridViewT1").IsNotNull() ? (dataGridViewT)this.FindControl("dataGridViewT1") : null;
            if (gridAppend == null)
                gridAppend = this.FindControl("gridAppend").IsNotNull() ? (Button)this.FindControl("gridAppend") : null;
            if (gridDelete == null)
                gridDelete = this.FindControl("gridDelete").IsNotNull() ? (Button)this.FindControl("gridDelete") : null;
            if (gridInsert == null)
                gridInsert = this.FindControl("gridInsert").IsNotNull() ? (Button)this.FindControl("gridInsert") : null;
            if (btnCancel == null)
                btnCancel = this.FindControl("btnCancel").IsNotNull() ? (Button)this.FindControl("btnCancel") : null;

            dataGridViewT1.CellValidating -= new System.Windows.Forms.DataGridViewCellValidatingEventHandler(Grid_CellValidating);
            dataGridViewT1.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(Grid_CellValidating);
            base.OnShown(e);
        }

        void Grid_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (dataGridViewT1 != null) if (dataGridViewT1.ReadOnly) return;
            if (gridDelete != null) if (gridDelete.Focused) return;
            if (btnCancel != null) if (btnCancel.Focused) return;

            if (dataGridViewT1.EditingControl != null && dataGridViewT1.Columns[e.ColumnIndex].Name == "產品編號")
            {
                if (dataGridViewT1.EditingControl.Text.Trim().Length == 0)
                {
                    if (Common.keyData == Keys.Left || Common.keyData == Keys.Right || Common.keyData == Keys.Enter || Common.keyData == Keys.Tab) e.Cancel = true;
                    if (ActiveControl.Name == dataGridViewT1.Name) e.Cancel = true;
                }
            }
        }






























    }
}
