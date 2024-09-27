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
    public partial class 應收票據建檔 : FormT
    {
        DataTable tbM = new DataTable();
        List<DataRow> list = new List<DataRow>();
        List<btnT> SC;
        List<btnT> Others;
        List<Control> UnderTxt;
        List<TextBox> DwonTxt;
        List<Label> Lb;
        List<TextBox> ChStatusSet;
        List<txtNumber> NumTxt;
        DataRow dr;
        bool 是否驗證 = false;
        string btnState="";
        string CurrentRow = "";
        string BeforeText;
        SqlTransaction tran;

        public 應收票據建檔()
        {
            InitializeComponent();
            SC = new List<btnT> { btnSave, btnCancel };
            Others = new List<btnT> { btnTop, btnPrior, btnNext, btnBottom, btnAppend, btnModify, btnDelete, btnExit, btnDuplicate, btnBrow };
            UnderTxt = new List<Control> { ChNo, CoNo, CoName1, CuNo, CuName1, CuName2, BaNo, BaName, ChAct, ChLine, ChDis, ChDate1, ChDate2, ChDate3, ChMny, ChStatus, ChDate,ChMemo };
            DwonTxt = new List<TextBox> { AcNo,AcName1,FaNo,FaName1,FaName2,ChOMny1,ChOMny2,ChOMny3,ChTMny1,ChTMny2,ChTMny3,ChTDate1,TacNo,TacName1};
            Lb = new List<Label> { Lb1,Lb2,Lb3,Lb4,Lb5,Lb6,Lb7,Lb8};
            ChStatusSet = new List<TextBox> { AcNo,AcName1,FaNo,FaName1,FaName2,ChOMny1,ChOMny2,ChOMny3,ChTMny1,ChTMny2,ChTMny3,ChTDate1,TacNo,TacName1};
            NumTxt = new List<txtNumber> { ChMny, ChOMny1, ChOMny2, ChOMny3, ChTMny1, ChTMny2, ChTMny3 };
            NumTxt.ForEach(r =>
            {
                r.NumThousands = true;
                r.NumLast = Common.金額小數;
                r.NumFirst = (20 - 1 - Common.金額小數);
            });
            if (Common.User_DateTime == 1) ChDate.MaxLength = ChDate1.MaxLength = ChDate2.MaxLength = ChDate3.MaxLength = ChTDate1.MaxLength =  7;
            else ChDate.MaxLength = ChDate1.MaxLength = ChDate2.MaxLength = ChDate3.MaxLength =  ChTDate1.MaxLength =  8;
            ChDate.Init();
            ChDate1.Init();
            ChDate2.Init();
            ChDate3.Init();
            ChTDate1.Init();
        }

        private void 應收票據建檔_Load(object sender, EventArgs e)
        {
            if (Common.單據異動 == "2") CoNo.Enabled = false;
            Common.取得浮動連線字串(Common.使用者預設公司);
            groupBoxT1.BackColor = groupBoxT2.BackColor = Color.FromArgb(215, 227, 239);
            ChLine.BackColor = ChDis.BackColor = Color.FromArgb(215, 227, 239);
            SC.ForEach(r => r.Enabled = false);
            Others.ForEach(r => r.Enabled = true);
            UnderTxt.ForEach(r =>
            {
                if (r is TextBox) ((TextBox)r).ReadOnly = true;
                if (r is CheckBox) ((CheckBox)r).Enabled= false;
            });
            DwonTxt.ForEach(r => r.ReadOnly = true);

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
                    SqlDataAdapter dd = new SqlDataAdapter("select * from chki order by chno", cn);
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
                UnderTxt.ForEach(r =>
                {
                    if (r is TextBox) ((TextBox)r).Text = "";
                    if (r is CheckBox) ((CheckBox)r).CheckState = CheckState.Unchecked;
                });
                DwonTxt.ForEach(r => r.Text = "");
            }
            else
            {
                ChNo.Text = dr["ChNo"].ToString();
                CoNo.Text = dr["CoNo"].ToString();
                CoName1.Text = dr["CoName1"].ToString();
                CuNo.Text = dr["CuNo"].ToString();
                CuName1.Text = dr["CuName1"].ToString();
                CuName2.Text = dr["CuName2"].ToString();
                BaNo.Text = dr["BaNo"].ToString();
                BaName.Text = dr["BaName"].ToString();
                ChAct.Text = dr["ChAct"].ToString();
                ChMny.Text = dr["ChMny"].ToDecimal().ToString("N" + Common.金額小數);
                ChStatus.Text = dr["ChStatus"].ToString();
                AcNo.Text = dr["AcNo"].ToString();
                AcName1.Text = dr["AcName1"].ToString();
                FaNo.Text = dr["FaNo"].ToString();
                FaName1.Text = dr["FaName1"].ToString();
                FaName2.Text = dr["FaName2"].ToString();
                ChOMny1.Text = dr["ChOMny1"].ToDecimal().ToString("N" + Common.金額小數);
                ChOMny2.Text = dr["ChOMny2"].ToDecimal().ToString("N" + Common.金額小數);
                ChOMny3.Text = dr["ChOMny3"].ToDecimal().ToString("N" + Common.金額小數);
                ChTMny1.Text = dr["ChTMny1"].ToDecimal().ToString("N" + Common.金額小數);
                ChTMny2.Text = dr["ChTMny2"].ToDecimal().ToString("N" + Common.金額小數);
                ChTMny3.Text = dr["ChTMny3"].ToDecimal().ToString("N" + Common.金額小數);
                TacNo.Text = dr["TacNo"].ToString();
                TacName1.Text = dr["TacName1"].ToString();
                ChMemo.Text = dr["ChMemo"].ToString();
                if (Common.User_DateTime == 1)
                {
                    ChDate.Text = dr["ChDate"].ToString();
                    ChDate1.Text = dr["ChDate1"].ToString();
                    ChDate2.Text = dr["ChDate2"].ToString();
                    ChDate3.Text = dr["ChDate3"].ToString();
                    ChTDate1.Text = dr["ChTDate1"].ToString();
                }
                else
                {
                    ChDate.Text = dr["ChDate_1"].ToString();
                    ChDate1.Text = dr["ChDate1_1"].ToString();
                    ChDate2.Text = dr["ChDate2_1"].ToString();
                    ChDate3.Text = dr["ChDate3_1"].ToString();
                    ChTDate1.Text = dr["ChTDate1_1"].ToString();
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
            UnderTxt.ForEach(r =>
            {
                if (r is TextBoxT) ((TextBoxT)r).GrayView = true;
                if (r is txtNumber) ((txtNumber)r).GrayView = true;
                if (r is TextBox) ((TextBox)r).ReadOnly = false;
                if (r is TextBox) ((TextBox)r).Text = "";
                if (r is CheckBox) ((CheckBox)r).Enabled = true;
                if (r is CheckBox) ((CheckBox)r).CheckState = CheckState.Unchecked;
            });
            CoName1.ReadOnly = true;
            DwonTxt.ForEach(r => {
                r.Text = "";
                r.ReadOnly = false;
                if (r is TextBoxT) ((TextBoxT)r).GrayView = true;
                if (r is txtNumber) ((txtNumber)r).GrayView = true;
                r.ReadOnly = true;
            });
            CoNo.Text = Common.使用者預設公司;
            CHK.CoNo_Validating(CoNo, CoName1);
            Common.取得浮動連線字串(CoNo.Text.Trim());
            ChDate.Text = ChDate1.Text = ChDate2.Text = ChDate3.Text = Date.GetDateTime(Common.User_DateTime, false);
            ChStatus.Text = "1";
            SetLbColor();
            decimal d = 0;
            ChMny.Text = ChOMny1.Text = ChOMny2.Text = ChOMny3.Text = ChTMny1.Text = ChTMny2.Text = ChTMny3.Text = d.ToString("N" + Common.金額小數);
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
            UnderTxt.ForEach(r =>
            {
                if (r is TextBoxT) ((TextBoxT)r).GrayView = true;
                if (r is txtNumber) ((txtNumber)r).GrayView = true;
                if (r is TextBox) ((TextBox)r).ReadOnly = false;
                if (r is CheckBox) ((CheckBox)r).Enabled = true;
            });
            DwonTxt.ForEach(r =>
            {
                r.ReadOnly = false;
                if (r is TextBoxT) ((TextBoxT)r).GrayView = true;
                if (r is txtNumber) ((txtNumber)r).GrayView = true;
                r.ReadOnly = true;
            });
            CoName1.ReadOnly = true;
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
            //DataRow ac = list.Find(r => r["ChNo"].ToString().Trim() == ChNo.Text.ToString().Trim());
            DataRow ac = Common.load("Check", "chki", "chno", ChNo.Text.Trim());
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
            UnderTxt.ForEach(r =>
            {
                if (r is TextBoxT) ((TextBoxT)r).GrayView = true;
                if (r is txtNumber) ((txtNumber)r).GrayView = true;
                if (r is TextBox) ((TextBox)r).ReadOnly = false;
                if (r is CheckBox) ((CheckBox)r).Enabled = true;
            });
            DwonTxt.ForEach(r =>
            {
                r.ReadOnly = false;
                if (r is TextBoxT) ((TextBoxT)r).GrayView = true;
                if (r is txtNumber) ((txtNumber)r).GrayView = true;
                r.ReadOnly = true;
            });
            CoName1.ReadOnly = true;
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
            DataRow ac = Common.load("Check", "chki", "chno", ChNo.Text.Trim());
            if (ac != null && ac["chflg1"].ToString() == "True")
            {
                MessageBox.Show("此票據已結帳，無法修改與刪除！","訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
            DataRow T = list.Find(r=>r["chno"].ToString().Trim() == ChNo.Text.Trim());
            if(T["chstno"].ToString().Trim() != "")
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
                        刪除之前帳戶資料(GetDataRow(ChNo.Text.Trim()));
                        cmd.CommandText = "delete from chki where ChNo=N'"
                            + ChNo.Text.Trim() + "' COLLATE Chinese_Taiwan_Stroke_BIN";
                        cmd.ExecuteNonQuery();

                        tran.Commit(); tran.Dispose();
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
            using (應收票據建檔_瀏覽 frm = new 應收票據建檔_瀏覽())
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
            if (ChNo.Text.Trim() == "")
            {
                MessageBox.Show("『支票號碼』不可為空白，請確定後重新輸入", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                ChNo.Focus();
                return;
            }
            if (BaNo.Text.Trim() == "")
            {
                MessageBox.Show("『付款銀行』不可為空白，請確定後重新輸入", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                BaNo.Focus();
                return;
            }
            if (ChMny.Text.ToDecimal() == 0)
            {
                MessageBox.Show("『票面金額』不可為零，請確定後重新輸入", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                ChMny.Focus();
                return;
            }
            if (btnState == "Append" || btnState == "Duplicate")
            {
                using (SqlConnection cn = new SqlConnection(Common.sqlConnString))
                {
                    cn.Open();
                    SqlCommand cmd = cn.CreateCommand();
                    tran = cn.BeginTransaction();
                    cmd.Transaction = tran;
                    try
                    {
                        cmd.CommandText = "insert into chki (chno,cono,coname1,cuno,cuname1,cuname2,bano,baname,chact,chline,chdis,chdate1,chdate1_1,chdate2,chdate2_1,chdate3,chdate3_1,"
                                        + " chmny,chstatus,acno,acname1,fano,faname1,faname2,chomny1,chomny2,chomny3,chtmny1,chtmny2,chtmny3,chtdate1,chtdate1_1,tacno,tacname1,chdate,chdate_1,chmemo,chstname)values("
                                        + " N'" + ChNo.Text.Trim() + "',"
                                        + " N'" + CoNo.Text.Trim() + "',"
                                        + " N'" + CoName1.Text.Trim() + "',"
                                        + " N'" + CuNo.Text.Trim() + "',"
                                        + " N'" + CuName1.Text.Trim() + "',"
                                        + " N'" + CuName2.Text.Trim() + "',"
                                        + " N'" + BaNo.Text.Trim() + "',"
                                        + " N'" + BaName.Text.Trim() + "',"
                                        + " N'" + ChAct.Text.Trim() + "',";
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
                                        + " N'" + AcName1.Text.Trim() + "',"
                                        + " N'" + FaNo.Text.Trim() + "',"
                                        + " N'" + FaName1.Text.Trim() + "',"
                                        + " N'" + FaName2.Text.Trim() + "',"
                                        + " '" + ChOMny1.Text.ToDecimal() + "',"
                                        + " '" + ChOMny2.Text.ToDecimal() + "',"
                                        + " '" + ChOMny3.Text.ToDecimal() + "',"
                                        + " '" + ChTMny1.Text.ToDecimal() + "',"
                                        + " '" + ChTMny2.Text.ToDecimal() + "',"
                                        + " '" + ChTMny3.Text.ToDecimal() + "',"
                                        + " N'" + Date.ToTWDate(ChTDate1.Text.Trim()) + "',"
                                        + " N'" + Date.ToUSDate(ChTDate1.Text.Trim()) + "',"
                                        + " N'" + TacNo.Text.Trim() + "',"
                                        + " N'" + TacName1.Text.Trim() + "',"
                                        + " N'" + Date.ToTWDate(ChDate.Text.Trim()) + "',"
                                        + " N'" + Date.ToUSDate(ChDate.Text.Trim()) + "',"
                                        + " N'" + ChMemo.Text.Trim() + "',";
                        if (ChStatus.Text == "1") cmd.CommandText += "'未 處 理')";
                        if (ChStatus.Text == "2") cmd.CommandText += "'託    收')";
                        if (ChStatus.Text == "3") cmd.CommandText += "'託收兌現')";
                        if (ChStatus.Text == "4") cmd.CommandText += "'現金兌現')";
                        if (ChStatus.Text == "5") cmd.CommandText += "'應收轉付')";
                        if (ChStatus.Text == "6") cmd.CommandText += "'票    貼')";
                        if (ChStatus.Text == "7") cmd.CommandText += "'退    票')";
                        if (ChStatus.Text == "8") cmd.CommandText += "'其    他')";
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
                DataRow T;
                T = GetDataRow(CurrentRow);
                int index = list.IndexOf(T);
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
                        cmd.CommandText = "delete chki where chno='" + CurrentRow + "'";
                        cmd.ExecuteNonQuery();
                        cmd.CommandText = "insert into chki (chstno,chstnum,chno,cono,coname1,cuno,cuname1,cuname2,bano,baname,chact,chline,chdis,chdate1,chdate1_1,chdate2,chdate2_1,chdate3,chdate3_1,"
                                        + " chmny,chstatus,acno,acname1,fano,faname1,faname2,chomny1,chomny2,chomny3,chtmny1,chtmny2,chtmny3,chtdate1,chtdate1_1,tacno,tacname1,chdate,chdate_1,chmemo,chstname)values("
                                        + " N'" + T["chstno"].ToString().Trim() + "',"
                                        + " N'" + T["chstnum"].ToString().Trim() + "',"
                                        + " N'" + ChNo.Text.Trim() + "',"
                                        + " N'" + CoNo.Text.Trim() + "',"
                                        + " N'" + CoName1.Text.Trim() + "',"
                                        + " N'" + CuNo.Text.Trim() + "',"
                                        + " N'" + CuName1.Text.Trim() + "',"
                                        + " N'" + CuName2.Text.Trim() + "',"
                                        + " N'" + BaNo.Text.Trim() + "',"
                                        + " N'" + BaName.Text.Trim() + "',"
                                        + " N'" + ChAct.Text.Trim() + "',";
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
                                        + " N'" + AcName1.Text.Trim() + "',"
                                        + " N'" + FaNo.Text.Trim() + "',"
                                        + " N'" + FaName1.Text.Trim() + "',"
                                        + " N'" + FaName2.Text.Trim() + "',"
                                        + " '" + ChOMny1.Text.ToDecimal() + "',"
                                        + " '" + ChOMny2.Text.ToDecimal() + "',"
                                        + " '" + ChOMny3.Text.ToDecimal() + "',"
                                        + " '" + ChTMny1.Text.ToDecimal() + "',"
                                        + " '" + ChTMny2.Text.ToDecimal() + "',"
                                        + " '" + ChTMny3.Text.ToDecimal() + "',"
                                        + " N'" + Date.ToTWDate(ChTDate1.Text.Trim()) + "',"
                                        + " N'" + Date.ToUSDate(ChTDate1.Text.Trim()) + "',"
                                        + " N'" + TacNo.Text.Trim() + "',"
                                        + " N'" + TacName1.Text.Trim() + "',"
                                        + " N'" + Date.ToTWDate(ChDate.Text.Trim()) + "',"
                                        + " N'" + Date.ToUSDate(ChDate.Text.Trim()) + "',"
                                        + " N'" + ChMemo.Text.Trim() + "',";
                        if (ChStatus.Text == "1") cmd.CommandText += "'未 處 理')";
                        if (ChStatus.Text == "2") cmd.CommandText += "'託    收')";
                        if (ChStatus.Text == "3") cmd.CommandText += "'託收兌現')";
                        if (ChStatus.Text == "4") cmd.CommandText += "'現金兌現')";
                        if (ChStatus.Text == "5") cmd.CommandText += "'應收轉付')";
                        if (ChStatus.Text == "6") cmd.CommandText += "'票    貼')";
                        if (ChStatus.Text == "7") cmd.CommandText += "'退    票')";
                        if (ChStatus.Text == "8") cmd.CommandText += "'其    他')";
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
                    UnderTxt.ForEach(r =>
                    {
                        if (r is TextBox) ((TextBox)r).ReadOnly = false;
                        if (r is TextBox) ((TextBox)r).Text = "";
                        if (r is CheckBox) ((CheckBox)r).CheckState = CheckState.Unchecked;
                    });
                    CoName1.ReadOnly = true;
                    DwonTxt.ForEach(r => r.Text = "");
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
            UnderTxt.ForEach(r =>
            {
                if (r is TextBox) ((TextBox)r).ReadOnly = false;
                if (r is TextBoxT) ((TextBoxT)r).GrayView = false;
                if (r is txtNumber) ((txtNumber)r).GrayView = false;
                if (r is TextBox) ((TextBox)r).ReadOnly = true;
                if (r is CheckBox) ((CheckBox)r).Enabled = false;
            });

            DwonTxt.ForEach(r => {
                r.ReadOnly = false;
                if (r is TextBoxT) ((TextBoxT)r).GrayView = false;
                if (r is txtNumber) ((txtNumber)r).GrayView = false;
                r.ReadOnly = true;
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
            ChStatusSet.ForEach(r => r.ReadOnly = true);
            switch (ChStatus.Text.ToString().Trim())
            {
                case "1":
                    Lb1.BackColor = Color.MistyRose;
                    break;
                case "2":
                    Lb2.BackColor = Color.MistyRose;
                    if (btnState != "")
                    {
                        AcNo.ReadOnly = false;
                        AcNo.Focus();
                    }
                    break;
                case "3":
                    Lb3.BackColor = Color.MistyRose;
                    if (btnState != "")
                    {
                        AcNo.ReadOnly = false;
                        AcNo.Focus();
                    }
                    break;
                case "4":
                    Lb4.BackColor = Color.MistyRose;
                    break;
                case "5":
                    Lb5.BackColor = Color.MistyRose;
                    if (btnState != "")
                    {
                        FaNo.ReadOnly = ChOMny1.ReadOnly = ChOMny2.ReadOnly = ChOMny3.ReadOnly = false;
                        FaNo.Focus();
                    }
                    break;
                case "6":
                    Lb6.BackColor = Color.MistyRose;
                    if (btnState != "")
                    {
                        AcNo.ReadOnly = ChTMny1.ReadOnly = ChTMny2.ReadOnly = ChTDate1.ReadOnly = TacNo.ReadOnly = false;
                        if (ChTMny1.Text.ToDecimal()+ChTMny2.Text.ToDecimal()+ChTMny3.Text.ToDecimal() != ChMny.Text.ToDecimal())
                            ChTMny3.Text = ChMny.Text;
                        AcNo.Focus();
                    }
                    break;
                case "7":
                    Lb7.BackColor = Color.MistyRose;
                    break;
                case "8":
                    Lb8.BackColor = Color.MistyRose;
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
            if (ChStatus.Text.Trim() == BeforeText) return;
            if (btnState != "")
            {
                decimal d = 0;
                ChStatusSet.ForEach(r =>
                {
                    if (r.Name.ToString() == "ChOMny1" ||
                        r.Name.ToString() == "ChOMny2" ||
                        r.Name.ToString() == "ChOMny3" ||
                        r.Name.ToString() == "ChTMny1" ||
                        r.Name.ToString() == "ChTMny2" ||
                        r.Name.ToString() == "ChTMny3"
                       ) r.Text = d.ToString("N" + Common.金額小數);
                    else r.Text = "";

                });
            }
            SetLbColor();
        }

        private void 應收票據建檔_KeyUp(object sender, KeyEventArgs e)
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
                if (CuNo.Text != "")
                {
                    if (!CHK.CuNo_Validating(Common.浮動連線字串, CuNo, CuName1, CuName2))
                        CuNo.Text = CuName1.Text = CuName2.Text = "";
                }
                if (FaNo.Text != "")
                {
                    if (!CHK.FaNo_Validating(Common.浮動連線字串, FaNo, FaName1, FaName2))
                        FaNo.Text = FaName1.Text = FaName2.Text = "";
                }
                if (AcNo.Text != "")
                {
                    if(!CHK.AcNo_Validating(CoNo.Text.Trim(),AcNo,AcName1,null,true))
                        AcNo.Text = AcName1.Text = "";
                }
                if (TacNo.Text != "")
                {
                    if(!CHK.AcNo_Validating(CoNo.Text.Trim(),TacNo,TacName1,null,true))
                        TacNo.Text = TacName1.Text = "";
                }
            }
        }

        private void CuNo_DoubleClick(object sender, EventArgs e)
        {
            CHK.CuNo_OpemFrm(CuNo, CuName1, CuName2);
        }

        private void CuNo_Validating(object sender, CancelEventArgs e)
        {
            if (btnCancel.Focused || CuNo.ReadOnly) return;
            if (CuNo.Text.Trim() == "")
            {
                MessageBox.Show("『客戶號碼』不可為空白，請確定後重新輸入", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                e.Cancel = true;
                return;
            }
            if (CuNo.Text.Trim() == BeforeText && !是否驗證) return;
            if (!CHK.CuNo_Validating(Common.浮動連線字串, CuNo, CuName1, CuName2))
            {
                是否驗證 = true;
                e.Cancel = true;
                CHK.CuNo_OpemFrm(CuNo, CuName1, CuName2);
            }
            else
            {
                是否驗證 = false;
                帶出上次交易帳戶();
            }
        }

        private void CuNo_Enter(object sender, EventArgs e)
        {
            TextBox tb = (sender as TextBox);
            if (tb.ReadOnly) return;
            BeforeText = tb.Text.Trim();
        }

        private void BaNo_DoubleClick(object sender, EventArgs e)
        {
            CHK.BaNo_OpemFrm(BaNo, BaName);
        }

        private void BaNo_Validating(object sender, CancelEventArgs e)
        {
            if (btnCancel.Focused || BaNo.ReadOnly) return;
            //if (BaNo.Text.Trim() == "")
            //{
            //    MessageBox.Show("『付款銀行』不可為空白，請確定後重新輸入", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    e.Cancel = true;
            //    return;
            //}
            if (BaNo.Text.Trim() == BeforeText && !是否驗證) return;
            if (!CHK.BaNo_Validating(BaNo, BaName))
            {
                是否驗證 = true;
                e.Cancel = true;
                CHK.BaNo_OpemFrm(BaNo, BaName);
            }
            else
            {
                是否驗證 = false;
            }
        }

        private void ChDate1_Validating(object sender, CancelEventArgs e)
        {
            if (btnCancel.Focused) return;
            TextBox tb = (TextBox)sender;
            if (tb.ReadOnly) return;
            if (tb.Text.Trim() == "")
            {
                if (tb.Name == "ChTDate1") return;
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
            if (!CHK.稽核會計年度(tb.Text.Trim())) e.Cancel = true;
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

        private void ChNo_DoubleClick(object sender, EventArgs e)
        {
            if (ChNo.ReadOnly != true)
            {
                using (應收票據建檔_瀏覽 frm = new 應收票據建檔_瀏覽())
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

            if (ChNo.Text.Trim() == "")
            {
                e.Cancel = true;
                ChNo.Text = "";
                MessageBox.Show("資料不可為空白，請確定後重新輸入", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

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
                        進銷存轉入支票();
                        CoNo.Focus();
                    }

                }
                else
                {
                    e.Cancel = true;
                    ChNo.SelectAll();
                    dr = GetDataRow(CurrentRow);
                    //開瀏覽視窗
                    using (應收票據建檔_瀏覽 frm = new 應收票據建檔_瀏覽())
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
                                進銷存轉入支票();
                                ChDate.Text = Date.GetDateTime(Common.User_DateTime);
                                CoNo.Focus();
                                break;
                        }
                    }

                }
            }
        }

        private void ChMemo_DoubleClick(object sender, EventArgs e)
        {
            CHK.Memo_OpemFrm(ChMemo);
        }

        private void AcNo_DoubleClick(object sender, EventArgs e)
        {
            CHK.AcNo_OpemFrm(CoNo.Text.Trim(),AcNo, AcName1,null,true,true);
        }

        private void AcNo_Validating(object sender, CancelEventArgs e)
        {
            if (AcNo.ReadOnly || btnCancel.Focused) return;
            if (AcNo.Text.Trim() == "")
            {
                MessageBox.Show("『票態帳戶』不可為空白，請確定後重新輸入", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                e.Cancel = true;
                return;
            }
            if (!CHK.AcNo_Validating(CoNo.Text.Trim(), AcNo, AcName1,null,true))
            {
                e.Cancel = true;
                CHK.AcNo_OpemFrm(CoNo.Text.Trim(), AcNo, AcName1, null, true, true);
            }
        }

        private void FaNo_DoubleClick(object sender, EventArgs e)
        {
            CHK.FaNo_OpemFrm(FaNo, FaName1, FaName2);
        }

        private void FaNo_Validating(object sender, CancelEventArgs e)
        {
            if (FaNo.ReadOnly || btnCancel.Focused) return;
            if (FaNo.Text.Trim() == "")
            {
                MessageBox.Show("『轉付廠商』不可為空白，請確定後重新輸入", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                e.Cancel = true;
                return;
            }
            if (!CHK.FaNo_Validating(Common.浮動連線字串, FaNo, FaName1, FaName2))
            {
                e.Cancel = true;
                CHK.FaNo_OpemFrm(FaNo, FaName1, FaName2);
            }
        }

        private void ChTMny1_Validating(object sender, CancelEventArgs e)
        {
            if ((sender as TextBox).ReadOnly) return;
            ChTMny3.Text = (ChMny.Text.ToDecimal() - ChTMny1.Text.ToDecimal() - ChTMny2.Text.ToDecimal()).ToString("N" + Common.金額小數);
        }

        private void TacNo_DoubleClick(object sender, EventArgs e)
        {
            CHK.AcNo_OpemFrm(CoNo.Text.Trim(), TacNo, TacName1,null,true,true);
        }

        private void TacNo_Validating(object sender, CancelEventArgs e)
        {
            if (TacNo.ReadOnly || btnCancel.Focused) return;
            if (TacNo.Text.Trim() == "")
            {
                TacName1.Text = "";
                return;
            }
            else
            {
                if (!CHK.AcNo_Validating(CoNo.Text.Trim(), TacNo, TacName1,null,true))
                {
                    e.Cancel = true;
                    CHK.AcNo_OpemFrm(CoNo.Text.Trim(), TacNo, TacName1,null,true,true);
                }
            }
        }

        void 帶出上次交易帳戶()
        {
            if (CuNo.Text.Trim() != "")
            {
                try
                {
                    using (SqlConnection cn = new SqlConnection(Common.sqlConnString))
                    {
                        cn.Open();
                        SqlCommand cmd = cn.CreateCommand();
                        cmd.CommandText = "select BaNo,BaName,ChAct from chki where cuno='" + CuNo.Text.Trim() + "' order by ChDate1 desc";
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.HasRows && reader.Read())
                        {
                            BaNo.Text = reader["BaNo"].ToString();
                            BaName.Text = reader["BaName"].ToString();
                            ChAct.Text = reader["ChAct"].ToString();
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
                if (Row["chstatus"].ToDecimal() == 3)
                {
                    using (SqlConnection cn = new SqlConnection(Common.sqlConnString))
                    {
                        cn.Open();
                        SqlCommand cmd = cn.CreateCommand();
                        cmd.CommandText = "update acct set acmny1-=" + Row["ChMny"].ToDecimal() + " where acno=N'" + Row["AcNo"].ToString().Trim() + "'";
                        cmd.ExecuteNonQuery();
                    }
                }
                if (Row["chstatus"].ToDecimal() == 5)
                {
                    using (SqlConnection cn = new SqlConnection(Common.浮動連線字串))
                    {
                        cn.Open();
                        SqlCommand cmd = cn.CreateCommand();
                        cmd.CommandText = "update fact set FaPayAmt-=" + Row["ChOMny3"].ToDecimal() + " where FaNo=N'" + Row["FaNo"].ToString().Trim() + "'";
                        cmd.ExecuteNonQuery();
                    }
                }
                if (Row["chstatus"].ToDecimal() == 6)
                {
                    using (SqlConnection cn = new SqlConnection(Common.sqlConnString))
                    {
                        cn.Open();
                        SqlCommand cmd = cn.CreateCommand();
                        cmd.CommandText = "update acct set acmny1-=" + Row["ChTMny1"].ToDecimal() + " where acno=N'" + Row["AcNo"].ToString().Trim() + "';";
                        cmd.CommandText += "update acct set acmny1-=" + Row["ChTMny3"].ToDecimal() + " where acno=N'" + Row["TacNo"].ToString().Trim() + "';";
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
                        cmd.CommandText = "update acct set acmny1+=" + ChMny.Text.ToDecimal() + " where acno=N'" + AcNo.Text.ToString().Trim() + "'";
                        cmd.ExecuteNonQuery();
                    }
                }
                if (ChStatus.Text.Trim() == "5")
                {
                    using (SqlConnection cn = new SqlConnection(Common.浮動連線字串))
                    {
                        cn.Open();
                        SqlCommand cmd = cn.CreateCommand();
                        cmd.CommandText = "update fact set FaPayAmt+=" + ChOMny3.Text.ToDecimal() + " where FaNo=N'" + FaNo.Text.ToString().Trim() + "'";
                        cmd.ExecuteNonQuery();
                    }
                }
                if (ChStatus.Text.Trim() == "6")
                {
                    using (SqlConnection cn = new SqlConnection(Common.sqlConnString))
                    {
                        cn.Open();
                        SqlCommand cmd = cn.CreateCommand();
                        cmd.CommandText = "update acct set acmny1+=" + ChTMny1.Text.ToDecimal() + " where acno=N'" + AcNo.Text.ToString().Trim() + "';";
                        cmd.CommandText += "update acct set acmny1+=" + ChTMny3.Text.ToDecimal() + " where acno=N'" + TacNo.Text.ToString().Trim() + "';";
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

        void 進銷存轉入支票()
        {
            dr = list.Find(r => r["chno"].ToString().Trim() == ChNo.Text.Trim());
            if (dr["chstno"].ToString().Trim() != "")
            {
                CoNo.ReadOnly = Common.單據異動 == "1" ? false : true;
                CuNo.ReadOnly = CuName1.ReadOnly = CuName2.ReadOnly = true;
                ChMny.ReadOnly = true;
                ChMemo.ReadOnly = true;
            }
            else
            {
                CoNo.ReadOnly = Common.單據異動 == "1" ? false : true;
                CuNo.ReadOnly = CuName1.ReadOnly = CuName2.ReadOnly = false;
                ChMny.ReadOnly = false;
                ChMemo.ReadOnly = false;
            }
        }

    }
}
