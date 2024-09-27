using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using S_61.Basic;
using S_61.Model;
using S_61.MyControl;
using S_61.s_3基本資料;

namespace S_61.s_1單據作業
{
    public partial class 應付票據建檔 : FormT
    {
        DataTable tbM = new DataTable();
        List<DataRow> list = new List<DataRow>();
        List<btnT> SC;
        List<btnT> Others;
        List<Control> Txt;
        List<Label> Lb;
        DataRow dr;
        bool 是否驗證 = false;
        string btnState = "";
        string CurrentRow = "";
        string BeforeText;
        SqlTransaction tran;

        public 應付票據建檔()
        {
            InitializeComponent();
            SC = new List<btnT> { btnSave, btnCancel };
            Others = new List<btnT> { btnTop, btnPrior, btnNext, btnBottom, btnAppend, btnModify, btnDelete, btnExit, btnDuplicate, btnBrow };
            Lb = new List<Label> { Lb1, Lb2, Lb3, Lb4, Lb5, Lb6 };
            Txt = new List<Control> { ChNo, CoNo, CoName1, FaNo, FaName1, FaName2, AcNo,AcName, AcName1, ChLine, ChDis, ChDate,ChDate1,ChDate2,ChDate3,ChMny,ChStatus,ChMemo};
            ChMny.NumThousands = true;
            ChMny.NumLast = Common.金額小數;
            ChMny.NumFirst = (20 - 1 - Common.金額小數);
            if (Common.User_DateTime == 1) ChDate.MaxLength = ChDate1.MaxLength = ChDate2.MaxLength = ChDate3.MaxLength = 7;
            else ChDate.MaxLength = ChDate1.MaxLength = ChDate2.MaxLength = ChDate3.MaxLength = 8;
            ChDate.Init();
            ChDate1.Init();
            ChDate2.Init();
            ChDate3.Init();
        }

        private void 應付票據建檔_Load(object sender, EventArgs e)
        {
            if (Common.單據異動 == "2") CoNo.Enabled = false;
            Common.取得浮動連線字串(Common.使用者預設公司);
            ChLine.BackColor = ChDis.BackColor = Color.FromArgb(215, 227, 239);
            SC.ForEach(r => r.Enabled = false);
            Others.ForEach(r => r.Enabled = true);
            Txt.ForEach(r =>
            {
                if (r is TextBox) ((TextBox)r).ReadOnly = true;
                if (r is CheckBox) ((CheckBox)r).Enabled = false;
            });

            loadM();
            if (list.Count > 0)
                WriteToTxt(list.Last());
        }

        void loadM()
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(Common.sqlConnString))
                {
                    cn.Open();
                    tbM.Clear();
                    list.Clear();
                    SqlDataAdapter dd = new SqlDataAdapter("select * from chko order by chno", cn);
                    dd.Fill(tbM);
                    if (tbM.Rows.Count > 0) list = tbM.AsEnumerable().ToList();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        DataRow GetDataRow()
        {
            return list.Find(r => r["ChNo"].ToString() == ChNo.Text.Trim());
        }

        DataRow GetDataRow(string str)
        {
            return list.Find(r => r["ChNo"].ToString() == str.Trim());
        }

        void WriteToTxt(DataRow dr)
        {
            if (dr == null)
            {
                Txt.ForEach(r =>
                {
                    if (r is TextBox) ((TextBox)r).Text = "";
                    if (r is CheckBox) ((CheckBox)r).CheckState = CheckState.Unchecked;
                });
            }
            else
            {
                ChNo.Text = dr["ChNo"].ToString();
                CoNo.Text = dr["CoNo"].ToString();
                CoName1.Text = dr["CoName1"].ToString();
                ChMny.Text = dr["ChMny"].ToDecimal().ToString("N" + Common.金額小數);
                ChStatus.Text = dr["ChStatus"].ToString();
                AcNo.Text = dr["AcNo"].ToString();
                AcName1.Text = dr["AcName1"].ToString();
                AcName.Text = dr["AcName"].ToString();
                FaNo.Text = dr["FaNo"].ToString();
                FaName1.Text = dr["FaName1"].ToString();
                FaName2.Text = dr["FaName2"].ToString();
                ChMemo.Text = dr["ChMemo"].ToString();
                if (Common.User_DateTime == 1)
                {
                    ChDate.Text = dr["ChDate"].ToString();
                    ChDate1.Text = dr["ChDate1"].ToString();
                    ChDate2.Text = dr["ChDate2"].ToString();
                    ChDate3.Text = dr["ChDate3"].ToString();
                }
                else
                {
                    ChDate.Text = dr["ChDate_1"].ToString();
                    ChDate1.Text = dr["ChDate1_1"].ToString();
                    ChDate2.Text = dr["ChDate2_1"].ToString();
                    ChDate3.Text = dr["ChDate3_1"].ToString();
                }
                ChLine.Checked = dr["ChLine"].ToDecimal() == 1 ? true : false;
                ChDis.Checked = dr["ChDis"].ToDecimal() == 1 ? true : false;

                CurrentRow = dr["ChNo"].ToString();
            }
            SetLbColor();
        }

        private void btnTop_Click(object sender, EventArgs e)
        {
            loadM();
            if (list.Count > 0)
            {
                dr = list.First();
                WriteToTxt(dr);
            }
            btnTop.Enabled = btnPrior.Enabled = false;
            btnNext.Enabled = btnBottom.Enabled = true;
        }

        private void btnPrior_Click(object sender, EventArgs e)
        {
            dr = GetDataRow();
            int temp = list.IndexOf(dr);
            loadM();
            if (list.Count > 0)
            {
                dr = GetDataRow(CurrentRow);
                int i = list.IndexOf(dr);
                if (i == -1)
                {
                    if (temp == 0)
                    {
                        MessageBox.Show("已最上一筆", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        WriteToTxt(list.First());
                        btnTop.Enabled = btnPrior.Enabled = false;
                        btnNext.Enabled = btnBottom.Enabled = true;
                    }
                    else
                    {
                        WriteToTxt(list[--temp]);
                        btnTop.Enabled = btnPrior.Enabled = btnNext.Enabled = btnBottom.Enabled = true;
                    }
                }
                if (i > 0)
                {
                    WriteToTxt(list[--i]);
                    btnTop.Enabled = btnPrior.Enabled = btnNext.Enabled = btnBottom.Enabled = true;
                }
                if (i == 0)
                {
                    MessageBox.Show("已最上一筆", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    WriteToTxt(list.First());
                    btnTop.Enabled = btnPrior.Enabled = false;
                    btnNext.Enabled = btnBottom.Enabled = true;
                }
            }
            else
            {
                WriteToTxt(null);
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            dr = GetDataRow();
            int temp = list.LastIndexOf(dr);
            loadM();
            if (list.Count > 0)
            {
                dr = GetDataRow(CurrentRow);
                int i = list.LastIndexOf(dr);
                if (i == -1)
                {
                    if (temp >= list.Count - 1)
                    {
                        MessageBox.Show("已最下一筆", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        WriteToTxt(list.Last());
                        btnTop.Enabled = btnPrior.Enabled = true;
                        btnNext.Enabled = btnBottom.Enabled = false;
                    }
                    else
                    {
                        WriteToTxt(list[++temp]);
                        btnTop.Enabled = btnPrior.Enabled = btnNext.Enabled = btnBottom.Enabled = true;
                    }
                }
                if (i < list.Count - 1)
                {
                    WriteToTxt(list[++i]);
                    btnTop.Enabled = btnPrior.Enabled = btnNext.Enabled = btnBottom.Enabled = true;
                }
                else
                {
                    MessageBox.Show("已最下一筆", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    WriteToTxt(list.Last());
                    btnTop.Enabled = btnPrior.Enabled = true;
                    btnNext.Enabled = btnBottom.Enabled = false;
                }
            }
            else
            {
                WriteToTxt(null);
            }
        }

        private void btnBottom_Click(object sender, EventArgs e)
        {
            loadM();
            if (list.Count > 0)
            {
                dr = list.Last();
                WriteToTxt(dr);
            }
            btnNext.Enabled = btnBottom.Enabled = false;
            btnTop.Enabled = btnPrior.Enabled = true;
        }

        private void btnAppend_Click(object sender, EventArgs e)
        {
            btnState = "Append";
            SC.ForEach(r => r.Enabled = true);
            Others.ForEach(r => r.Enabled = false);
            Txt.ForEach(r =>
            {
                if (r is TextBox) ((TextBox)r).ReadOnly = false;
                if (r is TextBox) ((TextBox)r).Text = "";
                if (r is CheckBox)
                {
                    ((CheckBox)r).Enabled = true;
                    if (r.Name == "ChLine")
                    {
                        if (Common.chkline == 0) ((CheckBox)r).CheckState = CheckState.Unchecked;
                        else ((CheckBox)r).CheckState = CheckState.Checked;
                    }
                    if (r.Name == "ChDis")
                    {
                        if (Common.chkdis == 0) ((CheckBox)r).CheckState = CheckState.Unchecked;
                        else ((CheckBox)r).CheckState = CheckState.Checked;
                    }
                }
            });
            CoName1.ReadOnly = AcName1.ReadOnly = true;
            CoNo.Text = Common.使用者預設公司;
            CHK.CoNo_Validating(CoNo, CoName1);
            Common.取得浮動連線字串(CoNo.Text.Trim());
            ChDate.Text = ChDate1.Text = ChDate2.Text = ChDate3.Text = Date.GetDateTime(Common.User_DateTime, false);
            ChStatus.Text = "1";
            SetLbColor();
            decimal d = 0;
            ChMny.Text = d.ToString("N" + Common.金額小數);
            ChNo.Focus();
        }

        private void btnDuplicate_Click(object sender, EventArgs e)
        {
            if (tbM.Rows.Count == 0)
            {
                MessageBox.Show("空資料庫，請先新增", "視窗訊息", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (Common.單據異動 == "2")
            {
                if (CoNo.Text.Trim() != Common.使用者預設公司)
                {
                    MessageBox.Show("無權更改此公司資料", "視窗訊息", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
            Common.取得浮動連線字串(CoNo.Text.Trim());
            btnState = "Duplicate";
            SC.ForEach(r => r.Enabled = true);
            Others.ForEach(r => r.Enabled = false);
            Txt.ForEach(r =>
            {
                if (r is TextBox) ((TextBox)r).ReadOnly = false;
                if (r is CheckBox) ((CheckBox)r).Enabled = true;
            });
            CoName1.ReadOnly = AcName1.ReadOnly = true;
            ChDate.Text = Date.GetDateTime(Common.User_DateTime);
            SetLbColor();
            ChNo.Text = "";
            ChNo.Focus();
        }

        private void btnModify_Click(object sender, EventArgs e)
        {
            if (tbM.Rows.Count == 0)
            {
                MessageBox.Show("空資料庫，請先新增", "視窗訊息", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            // DataRow ac = list.Find(r => r["ChNo"].ToString().Trim() == ChNo.Text.ToString().Trim());
            DataRow ac = Common.load("Check", "chko", "chno", ChNo.Text.Trim());
            if (ac != null && ac["chflg1"].ToString() == "True")
            {
                MessageBox.Show("此票據已結帳，無法修改與刪除！", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (ac != null && ac["chacno"].ToString().Trim() != "")
            {
                MessageBox.Show("此票據已傳輸至會計傳票，無法異動！\n公司編號:" + ac["accono"].ToString().Trim() + " 傳票編號:" + ac["chacno"].ToString().Trim(), "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (Common.單據異動 == "2")
            {
                if (CoNo.Text.Trim() != Common.使用者預設公司)
                {
                    MessageBox.Show("無權更改此公司資料", "視窗訊息", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
            Common.取得浮動連線字串(CoNo.Text.Trim());
            btnState = "Modify";
            SC.ForEach(r => r.Enabled = true);
            Others.ForEach(r => r.Enabled = false);
            Txt.ForEach(r =>
            {
                if (r is TextBox) ((TextBox)r).ReadOnly = false;
                if (r is CheckBox) ((CheckBox)r).Enabled = true;
            });
            CoName1.ReadOnly = AcName1.ReadOnly = true;
            ChDate.Text = Date.GetDateTime(Common.User_DateTime);
            SetLbColor();
            進銷存轉入支票();
            ChNo.Focus();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (tbM.Rows.Count == 0)
            {
                MessageBox.Show("空資料庫，請先新增", "視窗訊息", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            //DataRow ac = list.Find(r => r["ChNo"].ToString().Trim() == ChNo.Text.ToString().Trim());
            DataRow ac = Common.load("Check", "chko", "chno", ChNo.Text.Trim());
            if (ac != null && ac["chflg1"].ToString() == "True")
            {
                MessageBox.Show("此票據已結帳，無法修改與刪除！", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (ac != null && ac["chacno"].ToString().Trim() != "")
            {
                MessageBox.Show("此票據已傳輸至會計傳票，無法異動！\n公司編號:" + ac["accono"].ToString().Trim() + " 傳票編號:" + ac["chacno"].ToString().Trim(), "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (Common.單據異動 == "2")
            {
                if (CoNo.Text.Trim() != Common.使用者預設公司)
                {
                    MessageBox.Show("無權更改此公司資料", "視窗訊息", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
            DataRow T = list.Find(r => r["chno"].ToString().Trim() == ChNo.Text.Trim());
            if (T["chstno"].ToString().Trim() != "")
            {
                MessageBox.Show("此支票由進銷存沖款單轉入，無法刪除!!\n沖款單號:" + T["chstno"].ToString(), "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                ChMemo.Focus();
                return;
            }
            if (MessageBox.Show("請確定是否刪除此筆記錄?", "確認視窗", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Cancel)
            {
                return;
            }
            else
            {
                Common.取得浮動連線字串(CoNo.Text.Trim());
                //執行刪除指令前，檢查此筆資料是否已被別人刪除
                loadM();
                dr = GetDataRow();
                if (list.IndexOf(dr) == -1)
                {
                    if (list.Count > 0)
                    {
                        dr = list.Last();
                        WriteToTxt(dr);
                    }
                    else
                    {
                        dr = null;
                        WriteToTxt(dr);
                    }
                    return;//資料已被刪除，以下程式碼不執行
                }
                try
                {
                    using (SqlConnection cn = new SqlConnection(Common.sqlConnString))
                    {
                        cn.Open();
                        SqlCommand cmd = cn.CreateCommand();

                        tran = cn.BeginTransaction();
                        cmd.Transaction = tran;

                        cmd.CommandText = "delete from chko where ChNo=N'"
                            + ChNo.Text.Trim() + "' COLLATE Chinese_Taiwan_Stroke_BIN";
                        cmd.ExecuteNonQuery();
                        if (刪除之前帳戶資料(GetDataRow(ChNo.Text.Trim())))
                            tran.Commit();
                        else
                            tran.Rollback();
                        tran.Dispose();
                        cmd.Dispose();
                        dr = GetDataRow(CurrentRow);
                        int i = list.IndexOf(dr);
                        loadM();
                        if (list.Count > 0)
                        {
                            if (i >= list.Count - 1)
                            {
                                WriteToTxt(list.Last());
                                MessageBox.Show("已最下一筆", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                            else
                            {
                                WriteToTxt(list[i]);
                            }
                        }
                        else
                        {
                            WriteToTxt(null);
                        }
                    }
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    MessageBox.Show("DelError:\n" + ex.ToString());
                }
            }
        }

        private void btnBrow_Click(object sender, EventArgs e)
        {
            if (tbM.Rows.Count == 0)
            {
                MessageBox.Show("空資料庫，請先新增", "視窗訊息", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            using (應付票據建檔_瀏覽 frm = new 應付票據建檔_瀏覽())
            {
                frm.SetParaeter();
                frm.SeekNo = ChNo.Text.Trim();
                frm.ShowDialog();
                switch (frm.DialogResult)
                {
                    case DialogResult.OK:
                        if (frm.Result != "")
                        {
                            loadM();
                            dr = GetDataRow(frm.Result);
                            WriteToTxt(dr);
                        }
                        break;
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            //if (ChNo.Text.Trim() == "")
            //{
            //    MessageBox.Show("『支票號碼』不可為空白，請確定後重新輸入", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    ChNo.Focus();
            //    return;
            //}
            if (AcNo.Text.Trim() == "")
            {
                if (ChStatus.Text.ToDecimal() == 2 || ChStatus.Text.ToDecimal() == 3 || ChStatus.Text.ToDecimal() == 6)
                {
                    MessageBox.Show("『開票帳戶』不可為空，請確定後重新輸入", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    AcNo.Focus();
                    return;
                }
            }
            if (ChMny.Text.ToDecimal() == 0)
            {
                MessageBox.Show("『票面金額』不可為零，請確定後重新輸入", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                ChMny.Focus();
                return;
            }
            if (btnState == "Append" || btnState == "Duplicate")
            {
                if (!自動產生編號())
                {
                    MessageBox.Show("『支票號碼』重複", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    ChNo.Focus();
                    return;
                }
                using (SqlConnection cn = new SqlConnection(Common.sqlConnString))
                {
                    cn.Open();
                    SqlCommand cmd = cn.CreateCommand();
                    tran = cn.BeginTransaction();
                    cmd.Transaction = tran;
                    try
                    {
                        cmd.CommandText = "insert into chko (chno,cono,coname1,chline,chdis,chdate1,chdate1_1,chdate2,chdate2_1,chdate3,chdate3_1,"
                                        + " chmny,chstatus,acno,acname,acname1,fano,faname1,faname2,chdate,chdate_1,chmemo,chstname)values("
                                        + " N'" + ChNo.Text.Trim() + "',"
                                        + " N'" + CoNo.Text.Trim() + "',"
                                        + " N'" + CoName1.Text.Trim() + "',";
                        if (ChLine.Checked)
                            cmd.CommandText += " 1,";
                        else
                            cmd.CommandText += " 0,";
                        if (ChDis.Checked)
                            cmd.CommandText += " 1,";
                        else
                            cmd.CommandText += " 0,";
                        cmd.CommandText += " N'" + Date.ToTWDate(ChDate1.Text.Trim()) + "',"
                                        + " N'" + Date.ToUSDate(ChDate1.Text.Trim()) + "',"
                                        + " N'" + Date.ToTWDate(ChDate2.Text.Trim()) + "',"
                                        + " N'" + Date.ToUSDate(ChDate2.Text.Trim()) + "',"
                                        + " N'" + Date.ToTWDate(ChDate3.Text.Trim()) + "',"
                                        + " N'" + Date.ToUSDate(ChDate3.Text.Trim()) + "',"
                                        + " '" + ChMny.Text.ToDecimal() + "',"
                                        + " '" + ChStatus.Text + "',"
                                        + " N'" + AcNo.Text.Trim() + "',"
                                        + " N'" + AcName.Text.Trim() + "',"
                                        + " N'" + AcName1.Text.Trim() + "',"
                                        + " N'" + FaNo.Text.Trim() + "',"
                                        + " N'" + FaName1.Text.Trim() + "',"
                                        + " N'" + FaName2.Text.Trim() + "',"
                                        + " N'" + Date.ToTWDate(ChDate.Text.Trim()) + "',"
                                        + " N'" + Date.ToUSDate(ChDate.Text.Trim()) + "',"
                                        + " N'" + ChMemo.Text.Trim() + "',";
                        if (ChStatus.Text == "1") cmd.CommandText += "'未 處 理')";
                        if (ChStatus.Text == "2") cmd.CommandText += "'票已領取')";
                        if (ChStatus.Text == "3") cmd.CommandText += "'兌    現')";
                        if (ChStatus.Text == "4") cmd.CommandText += "'退    票')";
                        if (ChStatus.Text == "5") cmd.CommandText += "'作    廢')";
                        if (ChStatus.Text == "6") cmd.CommandText += "'其    他')";
                        cmd.ExecuteNonQuery();
                        if (新增目前帳戶資料())
                            tran.Commit();
                        else
                            tran.Rollback();
                        tran.Dispose();
                        cmd.Dispose();
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        MessageBox.Show(ex.ToString());
                        return;
                    }
                    CurrentRow = ChNo.Text.Trim();
                    btnAppend_Click(null, null);
                }
            }
            if (btnState == "Modify")
            {
                loadM();
                dr = GetDataRow(CurrentRow);
                int index = list.IndexOf(dr);
                if (index == -1)
                {
                    MessageBox.Show("此筆資料已被刪除", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    ChNo.Focus();
                    return;
                }
                if (!刪除之前帳戶資料(GetDataRow(CurrentRow))) return;
                using (SqlConnection cn = new SqlConnection(Common.sqlConnString))
                {
                    cn.Open();
                    SqlCommand cmd = cn.CreateCommand();
                    tran = cn.BeginTransaction();
                    cmd.Transaction = tran;
                    try
                    {
                        cmd.CommandText = "delete chko where chno='" + CurrentRow + "'";
                        cmd.ExecuteNonQuery();
                        cmd.CommandText = "insert into chko (chstno,chstnum,chno,cono,coname1,chline,chdis,chdate1,chdate1_1,chdate2,chdate2_1,chdate3,chdate3_1,"
                                        + " chmny,chstatus,acno,acname,acname1,fano,faname1,faname2,chdate,chdate_1,chmemo,chstname)values("
                                        + " N'" + dr["chstno"].ToString().Trim() + "',"
                                        + " N'" + dr["chstnum"].ToString().Trim() + "',"
                                        + " N'" + ChNo.Text.Trim() + "',"
                                        + " N'" + CoNo.Text.Trim() + "',"
                                        + " N'" + CoName1.Text.Trim() + "',";
                        if (ChLine.Checked)
                            cmd.CommandText += " 1,";
                        else
                            cmd.CommandText += " 0,";
                        if (ChDis.Checked)
                            cmd.CommandText += " 1,";
                        else
                            cmd.CommandText += " 0,";
                        cmd.CommandText += " N'" + Date.ToTWDate(ChDate1.Text.Trim()) + "',"
                                        + " N'" + Date.ToUSDate(ChDate1.Text.Trim()) + "',"
                                        + " N'" + Date.ToTWDate(ChDate2.Text.Trim()) + "',"
                                        + " N'" + Date.ToUSDate(ChDate2.Text.Trim()) + "',"
                                        + " N'" + Date.ToTWDate(ChDate3.Text.Trim()) + "',"
                                        + " N'" + Date.ToUSDate(ChDate3.Text.Trim()) + "',"
                                        + " '" + ChMny.Text.ToDecimal() + "',"
                                        + " '" + ChStatus.Text + "',"
                                        + " N'" + AcNo.Text.Trim() + "',"
                                        + " N'" + AcName.Text.Trim() + "',"
                                        + " N'" + AcName1.Text.Trim() + "',"
                                        + " N'" + FaNo.Text.Trim() + "',"
                                        + " N'" + FaName1.Text.Trim() + "',"
                                        + " N'" + FaName2.Text.Trim() + "',"
                                        + " N'" + Date.ToTWDate(ChDate.Text.Trim()) + "',"
                                        + " N'" + Date.ToUSDate(ChDate.Text.Trim()) + "',"
                                        + " N'" + ChMemo.Text.Trim() + "',";
                        if (ChStatus.Text == "1") cmd.CommandText += "'未 處 理')";
                        if (ChStatus.Text == "2") cmd.CommandText += "'票已領取')";
                        if (ChStatus.Text == "3") cmd.CommandText += "'兌    現')";
                        if (ChStatus.Text == "4") cmd.CommandText += "'退    票')";
                        if (ChStatus.Text == "5") cmd.CommandText += "'作    廢')";
                        if (ChStatus.Text == "6") cmd.CommandText += "'其    他')";
                        cmd.ExecuteNonQuery();
                        if (新增目前帳戶資料())
                            tran.Commit();
                        else
                            tran.Rollback();
                        tran.Dispose();
                        cmd.Dispose();
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        MessageBox.Show(ex.ToString());
                        return;
                    }
                    CurrentRow = ChNo.Text.Trim();
                    Txt.ForEach(r =>
                    {
                        if (r is TextBox) ((TextBox)r).Text = "";
                        if (r is CheckBox) ((CheckBox)r).CheckState = CheckState.Unchecked;
                    });
                    ChNo.Focus();
                    SetLbColor();
                    btnState = "Modify";
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Common.取得浮動連線字串(Common.使用者預設公司);
            btnState = "";
            SC.ForEach(r => r.Enabled = false);
            Others.ForEach(r => r.Enabled = true);
            Txt.ForEach(r =>
            {
                if (r is TextBox) ((TextBox)r).ReadOnly = true;
                if (r is CheckBox) ((CheckBox)r).Enabled = false;
            });
            loadM();
            if (list.Count > 0)
            {
                dr = GetDataRow(CurrentRow);
                WriteToTxt(dr);
            }
            else
            {
                WriteToTxt(null);
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Dispose();
            this.Close();
        }

        void SetLbColor()
        {
            Lb.ForEach(R => R.BackColor = Color.Transparent);
            switch (ChStatus.Text.ToString().Trim())
            {
                case "1":
                    Lb1.BackColor = Color.MistyRose;
                    break;
                case "2":
                    Lb2.BackColor = Color.MistyRose;
                    break;
                case "3":
                    Lb3.BackColor = Color.MistyRose;
                    break;
                case "4":
                    Lb4.BackColor = Color.MistyRose;
                    break;
                case "5":
                    Lb5.BackColor = Color.MistyRose;
                    break;
                case "6":
                    Lb6.BackColor = Color.MistyRose;
                    break;
            }
        }

        private void ChStatus_Validating(object sender, CancelEventArgs e)
        {
            if (btnCancel.Focused || ChStatus.ReadOnly) return;
            if (ChStatus.Text.Trim() == "")
            {
                MessageBox.Show("欄位不可為空", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                e.Cancel = true;
                return;
            }
            Char[] c = ChStatus.Text.ToCharArray();
            if (c[0].ToString().StartsWith("F")) return;
            if (!Char.IsNumber(c[0]) || c[0].ToDecimal() < 1 || c[0].ToDecimal() > 8)
            {
                MessageBox.Show("請輸入１～８數字", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                ChStatus.Text = "";
                e.Cancel = true;
                return;
            }
            SetLbColor();
        }

        private void 應付票據建檔_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.D1:
                case Keys.NumPad1:
                    btnAppend.Focus();
                    btnAppend.PerformClick();
                    break;
                case Keys.D2:
                case Keys.NumPad2:
                    btnModify.Focus();
                    btnModify.PerformClick();
                    break;
                case Keys.D3:
                case Keys.NumPad3:
                    btnDelete.PerformClick();
                    break;
                case Keys.D0:
                case Keys.NumPad0:
                case Keys.F11:
                    btnExit.PerformClick();
                    break;
                case Keys.Home:
                    btnTop.PerformClick();
                    break;
                case Keys.PageUp:
                    btnPrior.PerformClick();
                    break;
                case Keys.PageDown:
                    btnNext.PerformClick();
                    break;
                case Keys.End:
                    btnBottom.PerformClick();
                    break;
                case Keys.F9:
                    btnSave.PerformClick();
                    break;
                case Keys.F4:
                    btnCancel.Focus();
                    btnCancel.PerformClick();
                    break;
            }
        }

        private void CoNo_DoubleClick(object sender, EventArgs e)
        {
            CHK.CoNo_OpemFrm(CoNo, CoName1);
        }

        private void CoNo_Validating(object sender, CancelEventArgs e)
        {
            if (btnCancel.Focused || CoNo.ReadOnly) return;
            if (CoNo.Text.Trim() == "")
            {
                MessageBox.Show("『公司號碼』不可為空白，請確定後重新輸入", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                e.Cancel = true;
                return;
            }
            if (!CHK.CoNo_Validating(CoNo, CoName1))
            {
                e.Cancel = true;
                CHK.CoNo_OpemFrm(CoNo, CoName1);
            }
            else
            {
                Common.取得浮動連線字串(CoNo.Text.Trim());
                if (!CHK.FaNo_Validating(Common.浮動連線字串, FaNo, FaName1, FaName2))
                {
                    FaNo.Text = FaName1.Text = FaName2.Text = "";
                }
                if (!CHK.AcNo_Validating(CoNo.Text.Trim(), AcNo, AcName1, AcName, true))
                    AcNo.Text = AcName.Text = AcName1.Text = "";
            }
        }

        private void FaNo_DoubleClick(object sender, EventArgs e)
        {
            CHK.FaNo_OpemFrm(FaNo, FaName1, FaName2);
        }

        private void FaNo_Validating(object sender, CancelEventArgs e)
        {
            if (btnCancel.Focused || FaNo.ReadOnly) return;
            if (FaNo.Text.Trim() == "")
            {
                MessageBox.Show("『廠商編號』不可為空白，請確定後重新輸入", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                e.Cancel = true;
                return;
            }
            if (FaNo.Text.Trim() == BeforeText && !是否驗證) return;
            if (!CHK.FaNo_Validating(Common.浮動連線字串, FaNo, FaName1, FaName2))
            {
                是否驗證 = true;
                e.Cancel = true;
                CHK.FaNo_OpemFrm(FaNo, FaName1, FaName2);
            }
            else
            {
                是否驗證 = false;
                帶出上次交易帳戶();
            }
        }

        private void AcNo_DoubleClick(object sender, EventArgs e)
        {
            CHK.AcNo_OpemFrm(CoNo.Text.Trim(),AcNo, AcName1, AcName,true,true);
        }

        private void AcNo_Validating(object sender, CancelEventArgs e)
        {
            if (AcNo.ReadOnly || btnCancel.Focused) return;
            if (AcNo.Text.Trim() != "")
            {
                if (!CHK.AcNo_Validating(CoNo.Text.Trim(), AcNo, AcName1, AcName, true))
                {
                    e.Cancel = true;
                    CHK.AcNo_OpemFrm(CoNo.Text.Trim(), AcNo, AcName1, AcName, true, true);
                }
            }
            else
            {
                AcNo.Text = AcName1.Text = "";
            }
        }

        private void ChDate1_Validating(object sender, CancelEventArgs e)
        {
            if (btnCancel.Focused) return;
            TextBox tb = (TextBox)sender;
            if (tb.ReadOnly) return;
            if (tb.Text.Trim() == "")
            {
                MessageBox.Show("『日期』不可為空白，請確定後重新輸入", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                e.Cancel = true;
                return;
            }
            if (!tb.IsDateTime())
            {
                MessageBox.Show("日期格式錯誤，請確定後重新輸入", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                e.Cancel = true;
                return;
            }
            if (!CHK.稽核會計年度(tb.Text.Trim()))
            {
                e.Cancel = true;
                return;
            }
            if (tb.Name == "ChDate1")
            {
                if (ChDate1.Text.ToDecimal() > ChDate2.Text.ToDecimal())
                    ChDate2.Text = ChDate1.Text;
                if (ChDate1.Text.ToDecimal() > ChDate3.Text.ToDecimal())
                    ChDate3.Text = ChDate1.Text;
            }
            if (tb.Name == "ChDate2")
            {
                if (ChDate2.Text.ToDecimal() > ChDate3.Text.ToDecimal())
                    ChDate3.Text = ChDate2.Text;
            }
        }

        private void ChMemo_DoubleClick(object sender, EventArgs e)
        {
            CHK.Memo_OpemFrm(ChMemo);
        }

        private void ChNo_DoubleClick(object sender, EventArgs e)
        {
            if (ChNo.ReadOnly != true)
            {
                using (應付票據建檔_瀏覽 frm = new 應付票據建檔_瀏覽())
                {
                    frm.SetParaeter(ViewMode.Normal);
                    frm.SeekNo = ChNo.Text.Trim();
                    frm.開窗模式 = true;
                    frm.ShowDialog();
                    switch (frm.DialogResult)
                    {
                        case DialogResult.OK:
                            if (frm.Result == null) return;
                            dr = GetDataRow(frm.Result.ToString().Trim());
                            WriteToTxt(dr);
                            CoNo.Focus();
                            break;
                    }
                }
            }
        }

        private void ChNo_Validating(object sender, CancelEventArgs e)
        {
            if (ChNo.ReadOnly) return;
            if (btnCancel.Focused) return;

            //if (ChNo.Text.Trim() == "")
            //{
            //    e.Cancel = true;
            //    ChNo.Text = "";
            //    MessageBox.Show("資料不可為空白，請確定後重新輸入", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    return;
            //}

            if (btnState == "Append")
            {
                loadM();
                dr = GetDataRow();
                int i = list.IndexOf(dr);
                if (i != -1)
                {
                    e.Cancel = true;
                    ChNo.Text = "";
                    MessageBox.Show("此支票編號已經重複，請重新輸入", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }

            if (btnState == "Duplicate")
            {
                loadM();
                dr = GetDataRow();
                int i = list.IndexOf(dr);
                if (i != -1)
                {
                    e.Cancel = true;
                    ChNo.Text = "";
                    MessageBox.Show("此支票編號已經重複，請重新輸入", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }

            if (btnState == "Modify")
            {
                loadM();
                dr = GetDataRow();
                int i = list.IndexOf(dr);
                if (i != -1)
                {
                    if (ChNo.Text.Trim() != BeforeText)
                    {
                        WriteToTxt(dr);
                        ChDate.Text = Date.GetDateTime(Common.User_DateTime);
                        CoNo.Focus();
                    }

                }
                else
                {
                    e.Cancel = true;
                    ChNo.SelectAll();
                    dr = GetDataRow(CurrentRow);
                    //開瀏覽視窗
                    using (應付票據建檔_瀏覽 frm = new 應付票據建檔_瀏覽())
                    {
                        frm.SetParaeter(ViewMode.Normal);
                        frm.SeekNo = ChNo.Text.Trim();
                        frm.開窗模式 = true;
                        frm.ShowDialog();
                        switch (frm.DialogResult)
                        {
                            case DialogResult.OK:
                                dr = GetDataRow(frm.Result.ToString().Trim());
                                WriteToTxt(dr);
                                ChDate.Text = Date.GetDateTime(Common.User_DateTime);
                                CoNo.Focus();
                                break;
                        }
                    }

                }
            }
        }

        private void ChNo_Enter(object sender, EventArgs e)
        {
            TextBox tb = (sender as TextBox);
            if (tb.ReadOnly) return;
            BeforeText = tb.Text.Trim();
        }

        void 帶出上次交易帳戶()
        {
            if (FaNo.Text.Trim() != "")
            {
                try
                {
                    using (SqlConnection cn = new SqlConnection(Common.sqlConnString))
                    {
                        cn.Open();
                        SqlCommand cmd = cn.CreateCommand();
                        cmd.CommandText = "select acno,acname1,acname from chko where fano='" + FaNo.Text.Trim() + "' order by ChDate1 desc";
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.HasRows && reader.Read())
                        {
                            AcNo.Text = reader["AcNo"].ToString();
                            AcName.Text = reader["AcName"].ToString();
                            AcName1.Text = reader["AcName1"].ToString();
                        }
                        reader.Dispose(); reader.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }

        bool 刪除之前帳戶資料(DataRow Row)
        {
            try
            {
                if (dr["chstatus"].ToDecimal() == 3)
                {
                    using (SqlConnection cn = new SqlConnection(Common.sqlConnString))
                    {
                        cn.Open();
                        SqlCommand cmd = cn.CreateCommand();
                        cmd.CommandText = "update acct set acmny1+=" + dr["ChMny"].ToDecimal() + " where acno=N'" + dr["AcNo"].ToString().Trim() + "'";
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return false;
            }
            return true;
        }

        bool 新增目前帳戶資料()
        {
            try
            {
                if (ChStatus.Text.Trim() == "3")
                {
                    using (SqlConnection cn = new SqlConnection(Common.sqlConnString))
                    {
                        cn.Open();
                        SqlCommand cmd = cn.CreateCommand();
                        cmd.CommandText = "update acct set acmny1-=" + ChMny.Text.ToDecimal() + " where acno=N'" + AcNo.Text.ToString().Trim() + "'";
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return false;
            }
            return true;
        }

        bool 自動產生編號()
        {
            DataTable table = new DataTable();
            string date = "", count = "1";
            if (Common.傳票編碼方式 == 1)//民國年月日+流水號
            {
                date = Date.GetDateTime(1);
                count = count.PadLeft(3, '0');
            }
            else if (Common.傳票編碼方式 == 2)//民國年月+流水號
            {
                date = Date.GetDateTime(1);
                date = date.Substring(0, 5);
                count = count.PadLeft(5, '0');
            }
            else if (Common.傳票編碼方式 == 3)//西元年月日+流水號
            {
                date = Date.GetDateTime(2);
                count = count.PadLeft(2, '0');
            }
            else
            {
                date = Date.GetDateTime(2);
                date = date.Substring(0, 6);
                count = count.PadLeft(4, '0');
            }
            decimal No = (date + count).ToDecimal();
            try
            {
                if (ChNo.Text.Trim() == "")
                {
                    using (SqlConnection cn = new SqlConnection(Common.sqlConnString))
                    {
                        cn.Open();
                        string sql = "";
                        if (Common.傳票編碼方式 == 1)
                            sql = "select chno from chko where left(chno,7)='" + Date.GetDateTime(1) + "' order by chno desc";
                        else if (Common.傳票編碼方式 == 2)
                            sql = "select chno from chko where left(chno,5)='" + Date.GetDateTime(1).Substring(0, 5) + "' order by chno desc";
                        else if (Common.傳票編碼方式 == 3)
                            sql = "select chno from chko where left(chno,8)='" + Date.GetDateTime(2) + "' order by chno desc";
                        else
                            sql = "select chno from chko where left(chno,6)='" + Date.GetDateTime(2).Substring(0, 6) + "' order by chno desc";
                        SqlDataAdapter dd = new SqlDataAdapter(sql, cn);
                        dd.Fill(table);
                        if (table.Rows.Count == 0)
                        {
                            ChNo.Text = No.ToString().Trim();
                            return true;
                        }
                        else
                        {
                            No = table.Rows[0]["chno"].ToDecimal() + 1;
                            ChNo.Text = No.ToString().Trim();
                        }
                    }
                }
                else
                {
                    using (SqlConnection cn = new SqlConnection(Common.sqlConnString))
                    {
                        cn.Open();
                        SqlCommand cmd = cn.CreateCommand();
                        cmd.CommandText = "select chno from chko where chno='" + ChNo.Text.Trim() + "'";
                        if (cmd.ExecuteScalar().IsNullOrEmpty()) return true;
                        else return false;

                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return false;
            }
        }

        void 進銷存轉入支票()
        {
            dr = list.Find(r => r["chno"].ToString().Trim() == ChNo.Text.Trim());
            if (dr["chstno"].ToString().Trim() != "")
            {
                CoNo.ReadOnly = Common.單據異動 == "1" ? false : true;
                FaNo.ReadOnly = FaName1.ReadOnly = FaName2.ReadOnly = true;
                ChMny.ReadOnly = true;
                ChMemo.ReadOnly = true;
            }
            else
            {
                CoNo.ReadOnly = Common.單據異動 == "1" ? false : true;
                FaNo.ReadOnly = FaName1.ReadOnly = FaName2.ReadOnly = false;
                ChMny.ReadOnly = false;
                ChMemo.ReadOnly = false;
            }
        }
    }
}
