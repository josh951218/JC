using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using S_61.Basic;
using S_61.Model;
using S_61.MyControl;

namespace S_61.s_3基本資料
{
    public partial class 人員建檔作業 : FormT
    {
        List<Control> CC = new List<Control>();
        DataTable dtdept = new DataTable();//部門檔
        DataTable dtXX06 = new DataTable();//職謂檔
        DataTable dtEmpl = new DataTable();//員工資料檔


        DataTable dtXX02 = new DataTable();//區域建檔
        DataTable dtXX03 = new DataTable();//稅別檔
        DataTable dtXX04 = new DataTable();//結帳類別檔
        DataTable dtXX05 = new DataTable();//發票檔
        DataRow drTemp;

        List<DataRow> list = new List<DataRow>();
        DataTable dt = new DataTable();
        DataRow dr;
        string current;
        int temp = 0;
        string btnState = string.Empty;

        List<MyControl.btnT> SC = new List<MyControl.btnT>();
        List<MyControl.btnT> Others = new List<MyControl.btnT>();
        List<Control> TXTs = new List<Control>();
        List<Control> TXTReadOnly = new List<Control>();
        string BeforeText;

        public 人員建檔作業()
        {
            InitializeComponent();
            //pVar.FrmEmpl = this;

            SC.Add(btnSave);

            SC.Add(btnCancel);

            Others.Add(btnTop);
            Others.Add(btnPrior);
            Others.Add(btnNext);
            Others.Add(btnBottom);
            Others.Add(btnAppend);
            Others.Add(btnDuplicate);
            Others.Add(btnModify);
            Others.Add(btnDelete);
            Others.Add(btnPrint);
            Others.Add(btnBrow);
            Others.Add(btnExit);



            TXTs.Add(EmNo);
            TXTs.Add(EmName);
            TXTs.Add(EmDeno);
            TXTs.Add(EmReg);
            TXTs.Add(EmIdno);
            TXTs.Add(EmX6No);
            TXTs.Add(EmBirth);
            TXTs.Add(EmBlood);
            TXTs.Add(EmTel);
            TXTs.Add(EmPer1);
            TXTs.Add(EmInday);
            TXTs.Add(EmAtel1);
            TXTs.Add(EmTel2);
            TXTs.Add(EmOutday);
            TXTs.Add(EmBbc);
            TXTs.Add(EmAtel2);
            TXTs.Add(Emworkyears);
            TXTs.Add(EmEdu);
            TXTs.Add(EmUnde);
            TXTs.Add(EmSpec);
            TXTs.Add(EmEmail);
            TXTs.Add(EmAddr1);
            TXTs.Add(EmAddr2);
            TXTs.Add(EmR1);
            TXTs.Add(EmR2);
            TXTs.Add(EmEngname);
            TXTs.Add(EmAccno);
            TXTs.Add(EmLinday);
            TXTs.Add(EmLoutday);
            TXTs.Add(EmLamt);
            TXTs.Add(EmLpay);
            TXTs.Add(EmHinday);
            TXTs.Add(EmHoutday);
            TXTs.Add(EmHamt);
            TXTs.Add(EmHpay);
            TXTs.Add(EmSupp);
            TXTs.Add(EmUdf1);
            TXTs.Add(EmUdf2);
            TXTs.Add(EmMemo1);
            TXTs.Add(EmPath);
            TXTs.Add(posPass);
            TXTReadOnly.Add(EmDename);
            TXTReadOnly.Add(EmX6Name);
            TXTReadOnly.Add(EmSex);
            TXTReadOnly.Add(EmMarr);
            TXTReadOnly.Add(EmPath);
            TXTReadOnly.Add(EmIspay);
        }

        private void FrmEmpl_Load(object sender, EventArgs e)
        {
            Common.取得浮動連線字串(Common.使用者預設公司);
            Basic.SetParameter.TabControlItemSize(tabControl1);
            Basic.SetParameter.SetBtnEnable(Others);
            Basic.SetParameter.SetBtnDisable(SC);

            //日期格式
            switch (Common.User_DateTime)
            {
                case 1:
                    EmBirth.MaxLength = 7;
                    Emdate.MaxLength = 7;
                    EmInday.MaxLength = 7;
                    EmLoutday.MaxLength = 7;
                    EmLinday.MaxLength = 7;
                    EmOutday.MaxLength = 7;
                    EmHoutday.MaxLength = 7;
                    EmHinday.MaxLength = 7;

                    break;
                case 2:
                    EmBirth.MaxLength = 8;
                    Emdate.MaxLength = 8;
                    EmInday.MaxLength = 8;
                    EmLoutday.MaxLength = 8;
                    EmLinday.MaxLength = 8;
                    EmOutday.MaxLength = 8;
                    EmHoutday.MaxLength = 8;
                    EmHinday.MaxLength = 8;
                    break;
            }

            //載入資料庫，預設顯示第一筆
            loadDB();
            if (dt.Rows.Count > 0)
            {
                dr = list.First();
                writeToTxt(dr);
            }
            btnAppend.Focus();
            btnPic.Enabled = false;
            btnPicClr.Enabled = false;
        }

        public void loadDB()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(Common.浮動連線字串))
                {
                    string str = "select * from Empl";
                    SqlDataAdapter da = new SqlDataAdapter(str, conn);
                    dt.Clear();
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        list.Clear();
                        list = dt.AsEnumerable().ToList();
                    }
                    else
                    {
                        list.Clear();
                    }

                    da = new SqlDataAdapter("select Deno,Dename1 from Dept", conn);
                    da.Fill(dtdept);
                    da = new SqlDataAdapter("select X6No,X6Name from XX06", conn);
                    da.Fill(dtXX06);
                    da = new SqlDataAdapter("select EmNo,EmName from Empl", conn);
                    da.Fill(dtEmpl);
                    da = new SqlDataAdapter("select X2No,X2Name from XX02", conn);
                    da.Fill(dtXX02);
                    da = new SqlDataAdapter("select X3No,X3Name from XX03", conn);
                    da.Fill(dtXX03);
                    da = new SqlDataAdapter("select X4No,X4Name from XX04", conn);
                    da.Fill(dtXX04);
                    da = new SqlDataAdapter("select X5No,X5Name from XX05", conn);
                    da.Fill(dtXX05);
                    da.Dispose();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public void reLoadDB(string strBrow)
        {
            dr = list.Find(r => r.Field<string>("EmNo") == strBrow);
            if (dr != null)
            {
                writeToTxt(dr);
            }
        }

        private void writeToTxt(DataRow dr)
        {
            if (dr != null)
            {
                //建檔日期
                setEmplDate(Emdate, dr);
                EmNo.Text = dr["EmNo"].ToString();
                setEmplDate(EmBirth, dr);
                EmMarr.Text = dr["EmMarr"].ToString();
                EmName.Text = dr["EmName"].ToString();
                EmReg.Text = dr["EmReg"].ToString();

                EmDeno.Text = dr["EmDeno"].ToString();
                if (EmDeno.Text.Trim() != "")
                {
                    drTemp = dtdept.AsEnumerable().ToList().Find(o => o.Field<string>("Deno") == EmDeno.Text.Trim());
                    if (drTemp != null)
                    {
                        EmDename.Text = drTemp["Dename1"].ToString();
                    }
                }
                else EmDename.Text = "";

                EmIdno.Text = dr["EmIdno"].ToString();
                EmX6No.Text = dr["EmX6no"].ToString();

                if (EmX6No.Text.Trim() != "")
                {
                    drTemp = dtXX06.AsEnumerable().ToList().Find(o => o.Field<string>("X6no") == EmX6No.Text.Trim());
                    if (drTemp != null)
                    {
                        EmX6Name.Text = drTemp["X6name"].ToString();
                    }
                }
                else EmX6No.Text = "";

                //posPass.Text = dr["posPass"].ToString();
                //
                //分頁一
                //
                setEmplDate(EmHinday, dr);
                EmMemo1.Text = dr["EmMemo1"].ToString();
                EmAtel1.Text = dr["EmAtel1"].ToString();
                EmBbc.Text = dr["EmBbc"].ToString();
                EmPer1.Text = dr["EmPer1"].ToString();
                EmAtel2.Text = dr["EmAtel2"].ToString();
                setEmplDate(EmInday, dr);
                setEmplDate(EmOutday, dr);
                EmAccno.Text = dr["EmAccno"].ToString();
                EmAddr1.Text = dr["EmAddr1"].ToString();
                EmR1.Text = dr["EmR1"].ToString();
                EmAddr2.Text = dr["EmAddr2"].ToString();
                EmR2.Text = dr["EmR2"].ToString();
                EmPath.Text = "";
                EmEmail.Text = dr["EmEmail"].ToString();
                EmEdu.Text = dr["EmEdu"].ToString();
                EmSpec.Text = dr["EmSpec"].ToString();
                EmTel.Text = dr["EmTel"].ToString();
                EmTel2.Text = dr["EmTel2"].ToString();
                EmSex.Text = dr["EmSex"].ToString();
                Emworkyears.Text = dr["Emworkyears"].ToString();
                //
                //分頁二
                //
                EmEngname.Text = dr["EmEngname"].ToString();
                EmBlood.Text = dr["EmBlood"].ToString();
                setEmplDate(EmLinday, dr);
                EmLamt.Text = dr["EmLamt"].ToString();
                setEmplDate(EmLoutday, dr);
                EmUnde.Text = dr["EmUnde"].ToString();
                setEmplDate(EmHoutday, dr);
                //區域編號不是空值,帶出區域名稱
                EmHpay.Text = dr["EmHpay"].ToString();

                EmSupp.Text = dr["EmSupp"].ToString();
                EmIspay.Text = dr["EmIspay"].ToString();
                EmUdf1.Text = dr["EmUdf1"].ToString();
                EmUdf2.Text = dr["EmUdf2"].ToString();


                EmAccno.Text = dr["EmAccno"].ToString();


                EmLpay.Text = dr["EmLpay"].ToString();
                EmHamt.Text = dr["EmHamt"].ToString();

                byte[] buffer = dr["Empath"] as byte[];
                pictureBox1.LoadImg(buffer);
            }
            else
            {
                //dr沒有值表示資料庫裡面已經沒有資料
                //清空所有欄位
                Basic.SetParameter.TextBoxClear(TXTs);
                Basic.SetParameter.TextBoxClear(TXTReadOnly);
            }
        }

        private DataRow getCurrentDataRow()
        {
            return list.Find(o => o.Field<string>("EmNo") == (EmNo.Text.Trim()));
        }

        private DataRow getCurrentDataRow(string s)
        {
            return list.Find(o => o.Field<string>("EmNo") == (s));
        }

        private void btnTop_Click(object sender, EventArgs e)
        {
            btnTop.Enabled = false;
            btnPrior.Enabled = false;
            btnNext.Enabled = true;
            btnBottom.Enabled = true;
            loadDB();
            if (list.Count > 0)
            {
                dr = list.First();
                writeToTxt(dr);
            }
        }

        private void btnPrior_Click(object sender, EventArgs e)
        {
            current = EmNo.Text.Trim();
            dr = getCurrentDataRow();
            temp = list.IndexOf(dr);
            loadDB();
            if (list.Count > 0)
            {
                dr = getCurrentDataRow(current);
                int i = list.IndexOf(dr);
                if (i == -1)
                {
                    if (temp == 0)
                    {
                        dr = list.First();
                        writeToTxt(dr);
                        MessageBox.Show("已最上一筆", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        btnTop.Enabled = false;
                        btnPrior.Enabled = false;
                        btnNext.Enabled = true;
                        btnBottom.Enabled = true;
                        return;
                    }
                    else
                    {
                        dr = list[--temp];
                        writeToTxt(dr);
                        btnNext.Enabled = true;
                        btnBottom.Enabled = true;
                        return;
                    }
                }
                if (i > 0)
                {
                    dr = list[--i];
                    writeToTxt(dr);
                    btnNext.Enabled = true;
                    btnBottom.Enabled = true;
                }
                else
                {
                    MessageBox.Show("已最上一筆", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    btnTop.Enabled = false;
                    btnPrior.Enabled = false;
                    btnNext.Enabled = true;
                    btnBottom.Enabled = true;
                }
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            current = EmNo.Text.Trim();
            dr = getCurrentDataRow();
            temp = list.IndexOf(dr);
            loadDB();
            if (list.Count > 0)
            {
                dr = getCurrentDataRow(current);
                int i = list.IndexOf(dr);
                if (i == -1)
                {
                    if (temp >= list.Count)
                    {
                        dr = list.Last();
                        writeToTxt(dr);
                        MessageBox.Show("已至最後一筆", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        btnTop.Enabled = true;
                        btnPrior.Enabled = true;
                        btnNext.Enabled = false;
                        btnBottom.Enabled = false;
                        return;
                    }
                    else
                    {
                        dr = list[++i];
                        writeToTxt(dr);
                        btnTop.Enabled = true;
                        btnPrior.Enabled = true;
                        return;
                    }
                }
                if (i < list.Count - 1)
                {
                    dr = list[++i];
                    writeToTxt(dr);
                    btnTop.Enabled = true;
                    btnPrior.Enabled = true;
                }
                else
                {
                    MessageBox.Show("已至最後一筆", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    btnTop.Enabled = true;
                    btnPrior.Enabled = true;
                    btnNext.Enabled = false;
                    btnBottom.Enabled = false;
                }
            }
        }

        private void btnBottom_Click(object sender, EventArgs e)
        {
            btnTop.Enabled = true;
            btnPrior.Enabled = true;
            btnNext.Enabled = false;
            btnBottom.Enabled = false;
            loadDB();
            if (list.Count > 0)
            {
                dr = list.Last();
                writeToTxt(dr);
            }
        }

        private void btnAppend_Click(object sender, EventArgs e)
        {
            btnState = ((Button)sender).Name.Substring(3);
            current = EmNo.Text.Trim();
            Basic.SetParameter.SetTxtEnable(TXTs);
            Basic.SetParameter.TextBoxClear(TXTReadOnly);
            Basic.SetParameter.SetBtnEnable(SC);
            Basic.SetParameter.SetBtnDisable(Others);
            btnPic.Enabled = true;
            btnPicClr.Enabled = true;
            //新增時，清空欄位,焦點於EmNo
            //載入某些欄位的預設值
            EmNo.ReadOnly = false;
            EmNo.Focus();
        }

        private void btnDuplicate_Click(object sender, EventArgs e)
        {
            btnState = ((Button)sender).Name.Substring(3);
            current = EmNo.Text.Trim();
            Basic.SetParameter.SetTxtEnableNotClear(TXTs);
            EmNo.Text = "";
            //複製時，更新日期
            if (Common.listUsSettings.Count > 0)
            {
                switch (Common.User_DateTime.ToString())
                {
                    case "1":
                        Emdate.Text = Model.Date.GetDateTime(1, false);
                        break;
                    case "2":
                        Emdate.Text = Model.Date.GetDateTime(2, false);
                        break;
                }
            }
            Basic.SetParameter.SetBtnEnable(SC);
            Basic.SetParameter.SetBtnDisable(Others);
            btnPic.Enabled = true;
            btnPicClr.Enabled = true;
            //複製時，清空欄位,焦點於EmNo
            EmNo.ReadOnly = false;
            EmNo.Focus();
        }

        private void btnModify_Click(object sender, EventArgs e)
        {
            btnState = ((Button)sender).Name.Substring(3);
            if (EmNo.Text.Trim() == "") return;//EmNo沒有值，就不執行下面的指令
            current = EmNo.Text.Trim();

            Basic.SetParameter.SetTxtEnableNotClear(TXTs);
            Basic.SetParameter.SetBtnEnable(SC);
            Basic.SetParameter.SetBtnDisable(Others);
            btnPic.Enabled = true;
            btnPicClr.Enabled = true;
            //修改時,焦點於EmNo
            EmNo.ReadOnly = false;
            EmNo.Focus();
            EmNo.SelectAll();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            btnState = ((Button)sender).Name.Substring(3);
            if (EmNo.Text == string.Empty) return;//EmNo沒有值，就不執行下面的指令
            string confirm = "請確定是否刪除此筆記錄?";
            if (MessageBox.Show(confirm, "確認視窗", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Cancel)
            {
                return;
            }
            else
            {
                //執行EmMemo1指令前，檢查此筆資料是否已被別人EmMemo1
                loadDB();
                dr = getCurrentDataRow();
                if (list.IndexOf(dr) == -1)
                {
                    if (list.Count > 0)
                    {
                        dr = list.Last();
                        writeToTxt(dr);
                    }
                    else
                    {
                        dr = null;
                        writeToTxt(dr);
                    }
                    return;
                }
                try
                {
                    using (SqlConnection conn = new SqlConnection(Common.浮動連線字串))
                    {
                        conn.Open();
                        SqlCommand cmd = conn.CreateCommand();
                        cmd.CommandText = "delete from Empl where EmNo=N'"
                            + EmNo.Text + "' COLLATE Chinese_Taiwan_Stroke_BIN";
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();
                    }
                    loadDB();
                    if (list.Count > 0)
                    {
                        dr = list.Last();
                        writeToTxt(dr);
                    }
                    else
                    {
                        dr = null;
                        writeToTxt(dr);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            人員建檔作業_列印 frm = new 人員建檔作業_列印();
            frm.SetParaeter();
            frm.ShowDialog();
        }

        private void btnBrow_Click(object sender, EventArgs e)
        {
            loadDB();
            if (dt.Rows.Count > 0)
            {
                using (人員建檔作業_瀏覽 frm = new 人員建檔作業_瀏覽())
                {
                    frm.SetParaeter();
                    frm.SeekNo = EmNo.Text.Trim();
                    switch (frm.ShowDialog())
                    {
                        case DialogResult.OK:
                            reLoadDB(frm.Result["EmNo"].ToString());
                            break;
                        case DialogResult.Cancel: break;
                    }
                }
            }
            else
            {
                MessageBox.Show("空資料庫，請先新增", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!Common.IsOverRegist("empl"))
            {
                string msg = "目前使用版權為『教育版』，超過筆數限制無法存檔！\n";
                msg += "若要解除筆數限制，請升級為『正式版』。";
                MessageBox.Show(msg, "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (EmNo.Text == string.Empty)//儲存或修改時，EmNo不能為空值
            {
                MessageBox.Show("資料不可為空白，請確定後重新輸入", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                EmNo.Focus();
                return;
            }

            if (btnState == "Append")
            {
                loadDB();
                dr = getCurrentDataRow();
                int i = list.IndexOf(dr);
                if (i != -1)
                {
                    MessageBox.Show("此員工編號已經重複，請重新輸入");
                    EmNo.Text = string.Empty;
                    EmNo.Focus();
                    return;
                }
                try
                {

                    using (SqlConnection conn = new SqlConnection(Common.浮動連線字串))
                    {
                        conn.Open();
                        SqlCommand cmd = conn.CreateCommand();

                        cmd.CommandText = "INSERT INTO Empl"
                            + "(EmNo,EmReg,EmName,EmUnde"
                            + ",Emdeno,EmBlood,EmIdno,EmMarr,EmBirth,EmBirth1"
                            + ",EmX6no,EmMemo1,EmAtel1,EmBbc"
                            + ",EmAtel2,EmInday,EmInday1,EmOutday,EmOutday1"
                            + ",EmAddr1,EmR1,EmAddr2,EmR2"
                            + ",EmEmail,EmPer1,EmLoutday,EmLoutday1,EmSex"
                            + ",EmEdu,EmHpay,EmSpec,EmTel"
                            + ",EmPath"
                            + ",EmEngname,EmLinday,EmLinday1,EmLamt"
                            + ",EmTel2,EmHoutday,EmHoutday1,EmSupp,EmIspay"
                            + ",EmUdf1,EmUdf2,EmAccno,EmHamt"
                            + ",EmLpay,EmHinday,EmHinday1,Emdate,Emdate1,EmWorkYears"
                            + ") VALUES (N'"
                            + EmNo.Text + "',N'" + EmReg.Text + "',N'" + EmName.Text + "',N'" + EmUnde.Text + "',N'"
                            + EmDeno.Text + "',N'" + EmBlood.Text + "',N'" + EmIdno.Text + "',N'" + EmMarr.Text + "',N'" + Date.ToTWDate(EmBirth.Text) + "',N'" + Date.ToUSDate(EmBirth.Text) + "',N'"
                            + EmX6No.Text + "',N'" + EmMemo1.Text + "',N'" + EmAtel1.Text + "',N'" + EmBbc.Text + "',N'"
                            + EmAtel2.Text + "',N'" + Date.ToTWDate(EmInday.Text) + "',N'" + Date.ToUSDate(EmInday.Text) + "',N'" + Date.ToTWDate(EmOutday.Text) + "',N'" + Date.ToUSDate(EmOutday.Text) 
                            +"',N'" + EmAddr1.Text + "',N'" + EmR1.Text + "',N'" + EmAddr2.Text + "',N'" + EmR2.Text + "',N'"
                            + EmEmail.Text + "',N'" + EmPer1.Text + "',N'" + Date.ToTWDate(EmLoutday.Text) + "',N'" + Date.ToUSDate(EmLoutday.Text) + "',N'" + EmSex.Text
                            + "',N'" + EmEdu.Text + "',N'" + EmHpay.Text + "',N'" + EmSpec.Text + "',N'" + EmTel.Text + "',"
                            + "@Empath"
                            + ",N'" + EmEngname.Text + "',N'"+ Date.ToTWDate(EmLinday.Text) + "',N'"+ Date.ToUSDate(EmLinday.Text) + "',N'"+ EmLamt.Text 
                            + "',N'" + EmTel2.Text + "',N'"+ Date.ToTWDate(EmHoutday.Text) + "',N'"+ Date.ToUSDate(EmHoutday.Text) + "',N'"+ EmSupp.Text + "',N'" + EmIspay.Text 
                            + "',N'" + EmUdf1.Text + "',N'" + EmUdf2.Text + "',N'" + EmAccno.Text + "',N'" + EmHamt.Text
                            + "',N'" + EmLpay.Text + "',N'" + Date.ToTWDate(EmHinday.Text) + "',N'" + Date.ToUSDate(EmHinday.Text) + "',N'" + Model.Date.GetDateTime(1, false) + "',N'" + Model.Date.GetDateTime(2, false) + "',N'" + Emworkyears.Text.Trim()
                            + "')";

                        cmd.Parameters.Add("@Empath", SqlDbType.Image);
                        cmd.Parameters["@Empath"].Value = pictureBox1.ImageToByte();

                        cmd.ExecuteNonQuery();
                        cmd.Dispose();
                    }
                    //更新current值
                    current = EmNo.Text.Trim();
                    Basic.SetParameter.TextBoxClear(TXTs);
                    Basic.SetParameter.TextBoxClear(TXTReadOnly);
                    EmNo.Focus();
                    btnPic.Enabled = true;
                    btnPicClr.Enabled = true;
                    pictureBox1.Clear();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }

            if (btnState == "Duplicate")
            {
                loadDB();
                dr = getCurrentDataRow();
                int i = list.IndexOf(dr);
                if (i != -1)
                {
                    MessageBox.Show("此員工編號已經重複，請重新輸入");
                    EmNo.Text = string.Empty;
                    EmNo.Focus();
                    return;
                }
                try
                {
                    using (SqlConnection conn = new SqlConnection(Common.浮動連線字串))
                    {
                        conn.Open();
                        SqlCommand cmd = conn.CreateCommand();

                        cmd.CommandText = "INSERT INTO Empl"
                            + "(EmNo,EmReg,EmName,EmUnde"
                            + ",Emdeno,EmBlood,EmIdno,EmMarr,EmBirth,EmBirth1"
                            + ",EmX6no,EmMemo1,EmAtel1,EmBbc"
                            + ",EmAtel2,EmInday,EmInday1,EmOutday,EmOutday1"
                            + ",EmAddr1,EmR1,EmAddr2,EmR2"
                            + ",EmEmail,EmPer1,EmLoutday,EmLoutday1,EmSex"
                            + ",EmEdu,EmHpay,EmSpec,EmTel"
                            + ",EmPath"
                            + ",EmEngname,EmLinday,EmLinday1,EmLamt"
                            + ",EmTel2,EmHoutday,EmHoutday1,EmSupp,EmIspay"
                            + ",EmUdf1,EmUdf2,EmAccno,EmHamt"
                            + ",EmLpay,EmHinday,EmHinday1,Emdate,Emdate1,EmWorkYears"
                            + ") VALUES (N'"
                            + EmNo.Text + "',N'" + EmReg.Text + "',N'" + EmName.Text + "',N'" + EmUnde.Text + "',N'"
                            + EmDeno.Text + "',N'" + EmBlood.Text + "',N'" + EmIdno.Text + "',N'" + EmMarr.Text + "',N'" + Date.ToTWDate(EmBirth.Text) + "',N'" + Date.ToUSDate(EmBirth.Text) + "',N'"
                            + EmX6No.Text + "',N'" + EmMemo1.Text + "',N'" + EmAtel1.Text + "',N'" + EmBbc.Text + "',N'"
                            + EmAtel2.Text + "',N'" + Date.ToTWDate(EmInday.Text) + "',N'" + Date.ToUSDate(EmInday.Text) + "',N'" + Date.ToTWDate(EmOutday.Text) + "',N'" + Date.ToUSDate(EmOutday.Text)
                            + "',N'" + EmAddr1.Text + "',N'" + EmR1.Text + "',N'" + EmAddr2.Text + "',N'" + EmR2.Text + "',N'"
                            + EmEmail.Text + "',N'" + EmPer1.Text + "',N'" + Date.ToTWDate(EmLoutday.Text) + "',N'" + Date.ToUSDate(EmLoutday.Text) + "',N'" + EmSex.Text
                            + "',N'" + EmEdu.Text + "',N'" + EmHpay.Text + "',N'" + EmSpec.Text + "',N'" + EmTel.Text + "',"
                            + "@Empath"
                            + ",N'" + EmEngname.Text + "',N'" + Date.ToTWDate(EmLinday.Text) + "',N'" + Date.ToUSDate(EmLinday.Text) + "',N'" + EmLamt.Text
                            + "',N'" + EmTel2.Text + "',N'" + Date.ToTWDate(EmHoutday.Text) + "',N'" + Date.ToUSDate(EmHoutday.Text) + "',N'" + EmSupp.Text + "',N'" + EmIspay.Text
                            + "',N'" + EmUdf1.Text + "',N'" + EmUdf2.Text + "',N'" + EmAccno.Text + "',N'" + EmHamt.Text
                            + "',N'" + EmLpay.Text + "',N'" + Date.ToTWDate(EmHinday.Text) + "',N'" + Date.ToUSDate(EmHinday.Text) + "',N'" + Model.Date.GetDateTime(1, false) + "',N'" + Model.Date.GetDateTime(2, false) + "',N'" + Emworkyears.Text.Trim()
                            + "')";

                        cmd.Parameters.Add("@Empath", SqlDbType.Image);
                        cmd.Parameters["@Empath"].Value = pictureBox1.ImageToByte();

                        cmd.ExecuteNonQuery();
                        cmd.Dispose();
                    }
                    //更新current值
                    current = EmNo.Text.Trim();
                    Basic.SetParameter.TextBoxClear(TXTs);
                    Basic.SetParameter.TextBoxClear(TXTReadOnly);
                    EmNo.Focus();
                    btnPic.Enabled = true;
                    btnPicClr.Enabled = true;
                    pictureBox1.Clear();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }

            if (btnState == "Modify")
            {
                loadDB();
                dr = getCurrentDataRow();
                int i = list.IndexOf(dr);
                if (i == -1)
                {
                    MessageBox.Show("此筆資料已被刪除", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    EmNo.Text = string.Empty;
                    EmNo.Focus();
                    return;
                }
                try
                {
                    using (SqlConnection conn = new SqlConnection(Common.浮動連線字串))
                    {
                        conn.Open();
                        SqlCommand cmd = conn.CreateCommand();

                        cmd.CommandText = "Update Empl set "
                          + " EmName =N'" + EmName.Text.Trim() + "',"
                          + " EmUnde =N'" + EmUnde.Text + "',"
                          + " EmBlood =N'" + EmBlood.Text + "',"
                          + " EmDeno =N'" + EmDeno.Text + "',"
                          + " EmBirth =N'" + Date.ToTWDate(EmBirth.Text) + "',"
                          + " EmBirth1 =N'" + Date.ToUSDate(EmBirth.Text) + "',"
                          + " EmMarr =N'" + EmMarr.Text + "',"
                          + " EmIdno =N'" + EmIdno.Text + "',"
                          + " EmX6no =N'" + EmX6No.Text + "',"
                          + " EmAtel1 =N'" + EmAtel1.Text + "',"
                          + " EmBbc =N'" + EmBbc.Text + "',"
                          + " EmPer1 =N'" + EmPer1.Text + "',"
                          + " EmMemo1 =N'" + EmMemo1.Text + "',"
                          + " EmAtel2 =N'" + EmAtel2.Text + "',"
                          + " EmInday =N'" + Date.ToTWDate(EmInday.Text) + "',"
                          + " EmInday1 =N'" + Date.ToUSDate(EmInday.Text) + "',"
                          + " EmOutday =N'" + Date.ToTWDate(EmOutday.Text) + "',"
                          + " EmOutday1 =N'" + Date.ToUSDate(EmOutday.Text) + "',"
                          + " EmAddr1 =N'" + EmAddr1.Text + "',"
                          + " EmR1 =N'" + EmR1.Text + "',"
                          + " EmAddr2 =N'" + EmAddr2.Text + "',"
                          + " EmR2 =N'" + EmR2.Text + "',"
                          + " EmEmail =N'" + EmEmail.Text + "',"
                          + " EmEdu =N'" + EmEdu.Text + "',"
                          + " EmHpay =N'" + EmHpay.Text + "',"
                          + " EmSpec =N'" + EmSpec.Text + "',"
                          + " EmTel =N'" + EmTel.Text + "',"
                          + " EmSex =N'" + EmSex.Text + "',"
                          + " EmPath = @Empath,"
                          + " EmTel2 =N'" + EmTel2.Text + "',"
                          + " EmEngname =N'" + EmEngname.Text + "',"
                          + " EmLinday =N'" + Date.ToTWDate(EmLinday.Text) + "',"
                          + " EmLinday1 =N'" + Date.ToUSDate(EmLinday.Text) + "',"
                          + " EmLamt =N'" + EmLamt.Text + "',"
                          + " EmLoutday =N'" + Date.ToTWDate(EmLoutday.Text) + "',"
                          + " EmLoutday1 =N'" + Date.ToUSDate(EmLoutday.Text) + "',"
                          + " EmHoutday =N'" + EmHoutday.Text + "',"
                          + " EmHoutday1 =N'" + EmHoutday.Text + "',"
                          + " EmSupp =N'" + EmSupp.Text + "',"
                          + " EmIspay =N'" + EmIspay.Text + "',"
                          + " EmUdf1 =N'" + EmUdf1.Text + "',"
                          + " EmLpay =N'" + EmLpay.Text + "',"
                          + " EmHinday =N'" + EmHinday.Text + "',"
                          + " EmHinday1 =N'" + EmHinday.Text + "',"
                          + " EmReg =N'" + EmReg.Text + "',"
                          + " EmUdf2 =N'" + EmUdf2.Text + "',"
                          + " EmHamt =N'" + Model.Date.RemoveLine(EmHamt.Text.Trim()) + "',"
                          + " EmAccno =N'" + EmAccno.Text + "',"
                          + " Emworkyears =N'" + Emworkyears.Text.Trim() + "'"
                          + " where EmNo =N'" + EmNo.Text.Trim() + "' COLLATE Chinese_Taiwan_Stroke_BIN";

                        cmd.Parameters.Add("@Empath", SqlDbType.Image);
                        cmd.Parameters["@Empath"].Value = pictureBox1.ImageToByte();

                        cmd.ExecuteNonQuery();
                        cmd.Dispose();
                    }
                    //更新current值
                    current = EmNo.Text.Trim();
                    Basic.SetParameter.TextBoxClear(TXTs);
                    Basic.SetParameter.TextBoxClear(TXTReadOnly);
                    EmNo.Focus();
                    btnPic.Enabled = true;
                    btnPicClr.Enabled = true;
                    pictureBox1.Clear();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            //取消其它動作時,清空按鈕狀態值
            //載回當前欄位
            btnState = string.Empty;
            Basic.SetParameter.SetTxtDisable(TXTs);
            Basic.SetParameter.SetBtnEnable(Others);
            Basic.SetParameter.SetBtnDisable(SC);
            //取消時，檢查預載回的資料是否已被別人EmMemo1
            //已被EmMemo1，改顯示最後一筆
            //若再沒資料，清空欄位
            //沒被EmMemo1，則載回預存資料
            loadDB();
            dr = getCurrentDataRow(current);
            if (list.IndexOf(dr) == -1)
            {
                if (list.Count > 0)
                {
                    dr = list.Last();
                    writeToTxt(dr);
                }
                else
                {
                    dr = null;
                    writeToTxt(dr);
                }
            }
            else
            {
                writeToTxt(dr);
            }
            btnPic.Enabled = false;
            btnPicClr.Enabled = false;
            btnAppend.Focus();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
        }

        private string getRadioNumber(tableLayoutPnl pnl)
        {
            string str = "";
            foreach (Control rd in pnl.Controls)
            {
                if (rd is RadioButton)
                {
                    if (((RadioButton)rd).Checked)
                        str = ((RadioButton)rd).Name.Substring(5);
                }
            }
            return str;
        }


        //表頭欄位
        private void EmNo_DoubleClick(object sender, EventArgs e)
        {
            if (EmNo.ReadOnly != true)
            {
                using (人員建檔作業_瀏覽 frm = new 人員建檔作業_瀏覽())
                {
                    frm.SetParaeter(ViewMode.Normal);
                    frm.SeekNo = EmNo.Text.Trim();
                    frm.CanAppend = false;
                    switch (frm.ShowDialog())
                    {
                        case DialogResult.OK:
                            reLoadDB(frm.Result["EmNo"].ToString());
                            break;
                        case DialogResult.Cancel: break;
                    }
                }
            }
        }

        private void Deno_DoubleClick(object sender, EventArgs e)
        {
            CHK.DeNo_OpemFrm(EmDeno,EmDename);
        }

        private void EmIdno_DoubleClick(object sender, EventArgs e)
        {
            if (btnState == "Append" || btnState == "Modify")
            {
                if ((sender as TextBox).Text == "男") (sender as TextBox).Text = "女";
                else (sender as TextBox).Text = "男";
            }
        }

        private void X6no_DoubleClick(object sender, EventArgs e)
        {
            CHK.X6No_OpemFrm(EmX6No, EmX6Name);
        }



        //分頁一
        private void EmAddr1_Leave(object sender, EventArgs e)
        {
            if (EmAddr2.Text.Trim() == "") EmAddr2.Text = EmAddr1.Text;
        }

        private void EmAddr1_DoubleClick(object sender, EventArgs e)
        {
            if (EmAddr1.ReadOnly != true)
            {
                FrmSaddr frm = new FrmSaddr();
                frm.SetParaeter(ViewMode.Normal);
                if ((sender as TextBox).Name == "EmAddr1")
                {
                    frm.callType = "EmAddr1";
                }
                if ((sender as TextBox).Name == "EmAddr2")
                {
                    frm.callType = "EmAddr2";
                }
                if ((sender as TextBox).Name == "EmAddr1_1")
                {
                    frm.callType = "EmAddr1_1";
                }
                frm.ShowDialog();
            }
        }

        private void EmR1_Leave(object sender, EventArgs e)
        {
            if (EmR2.Text.Trim() == "") EmR2.Text = EmR1.Text;
        }

        private void EmSex_DoubleClick(object sender, EventArgs e)
        {
            if (btnState == "Append" || btnState == "Modify" || btnState == "Duplicate")
            {
                if ((sender as TextBox).Text == "男") (sender as TextBox).Text = "女";
                else (sender as TextBox).Text = "男";
            }
        }




        //分頁二
        private void FrmEmpl_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.D1:
                case Keys.NumPad1:
                    btnAppend.PerformClick();
                    break;
                case Keys.D2:
                case Keys.NumPad2:
                    btnModify.PerformClick();
                    break;
                case Keys.D3:
                case Keys.NumPad3:
                    btnDelete.PerformClick();
                    break;
                case Keys.D4:
                    btnBrow.PerformClick();
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

        private void EmMarr_DoubleClick(object sender, EventArgs e)
        {
            if (btnState == "Append" || btnState == "Modify" || btnState == "Duplicate")
            {
                if ((sender as TextBox).Text == "已") (sender as TextBox).Text = "未";
                else (sender as TextBox).Text = "已";
            }
        }

        private void btnPic_Click(object sender, EventArgs e)
        {
            pictureBox1.LoadImg();
        }

        private void btnPicClr_Click(object sender, EventArgs e)
        {
            pictureBox1.Clear();
        }

        private void EmIspay_DoubleClick(object sender, EventArgs e)
        {
            if (btnState == "Append" || btnState == "Modify" || btnState == "Duplicate")
            {
                if ((sender as TextBox).Text == "Y") (sender as TextBox).Text = "N";
                else (sender as TextBox).Text = "Y";
            }
        }

        private void EmBirth_Leave(object sender, EventArgs e)
        {
            TextBox datetb = (TextBox)sender;
            if (datetb.Text.Trim() == "") return;
            if (datetb.ReadOnly) return;//唯讀時,不執行欄位改變顏色
            datetb.BackColor = SystemColors.Window;
            if (!datetb.IsDateTime())
            {
                MessageBox.Show("日期格式錯誤", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                if (Common.User_DateTime == 1)
                    datetb.Text = Date.GetDateTime(1, false);
                else
                    datetb.Text = Date.GetDateTime(2, false);
                datetb.SelectAll();
                datetb.Focus();
            }
        }

        void setEmplDate(TextBox tx, DataRow dr)
        {
            if (Common.User_DateTime == 1)
            {
                tx.Text = dr[tx.Name].ToString();
            }
            else
            {
                tx.Text = dr[tx.Name + "1"].ToString();
            }
        }

        private void EmNo_Enter(object sender, EventArgs e)
        {
            BeforeText = EmNo.Text;
        }

        private void EmNo_Validating(object sender, CancelEventArgs e)
        {
            if (EmNo.ReadOnly) return;
            if (btnCancel.Focused) return;

            if (EmNo.Text.Trim() == "")
            {
                e.Cancel = true;
                EmNo.Text = "";
                MessageBox.Show("資料不可為空白，請確定後重新輸入", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (btnState == "Append")
            {
                loadDB();
                dr = getCurrentDataRow();
                int i = list.IndexOf(dr);
                if (i != -1)
                {
                    e.Cancel = true;
                    EmNo.Text = "";
                    MessageBox.Show("此員工編號已經重複，請重新輸入", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }

            if (btnState == "Duplicate")
            {
                loadDB();
                dr = getCurrentDataRow();
                int i = list.IndexOf(dr);
                if (i != -1)
                {
                    e.Cancel = true;
                    EmNo.Text = "";
                    MessageBox.Show("此員工編號已經重複，請重新輸入", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }

            if (btnState == "Modify")
            {
                loadDB();
                dr = getCurrentDataRow();
                int i = list.IndexOf(dr);
                if (i != -1)
                {
                    if (EmNo.Text.Trim() != BeforeText)
                        writeToTxt(dr);
                }
                else
                {
                    e.Cancel = true;
                    EmNo.SelectAll();
                    dr = getCurrentDataRow(current);
                    //開瀏覽視窗mo
                    using (人員建檔作業_瀏覽 frm = new 人員建檔作業_瀏覽())
                    {
                        frm.SetParaeter(ViewMode.Normal);
                        frm.SeekNo = EmNo.Text.Trim();
                        frm.CanAppend = false;
                        switch (frm.ShowDialog())
                        {
                            case DialogResult.OK:
                                reLoadDB(frm.Result["EmNo"].ToString());
                                break;
                            case DialogResult.Cancel: break;
                        }
                    }
                }
            }

        }

        private void EmDeno_Validating(object sender, CancelEventArgs e)
        {
            if (btnCancel.Focused || EmDeno.Text.Trim() == "") return;
            if (!CHK.DeNo_Validating(Common.浮動連線字串,EmDeno, EmDename))
            {
                e.Cancel = true;
                CHK.DeNo_OpemFrm(EmDeno, EmDename);
            }
        }

        private void EmX6no_Validating(object sender, CancelEventArgs e)
        {
            if (btnCancel.Focused || EmX6No.Text.Trim() == "") return;
            if (!CHK.X6No_Validating(Common.浮動連線字串,EmX6No, EmX6Name))
            {
                e.Cancel = true;
                CHK.X6No_OpemFrm(EmX6No, EmX6Name);
            }
        }

        private void ck1_CheckedChanged(object sender, EventArgs e)
        {
            if (ck1.Checked)
                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            else
                pictureBox1.SizeMode = PictureBoxSizeMode.CenterImage;
        }

        private void EmUnde_Validating(object sender, CancelEventArgs e)
        {
            if (EmUnde.ReadOnly) return;
            TextBox tx = (TextBox)sender;
            tx.Text = tx.Text.Trim();
        }








    }
}