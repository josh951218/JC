using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using S_61.Basic;
using S_61.MyControl;

namespace S_61.Model
{
    public class pVar
    {
        public static Model.FrmBuyPriceQuery FrmBuyPriceQuery;
        //
       
        //營幕
        public static int ScreenWidth = Screen.PrimaryScreen.Bounds.Width;

        public static DataSet reportds = new DataSet();
        public static string TRS = "";
        public static bool ShowTRS = false;




        public static bool Xa01Validate(string TKey, TextBox TxNo, TextBox TxName)
        {
            if (TKey.Trim() == "")
            {
                TxNo.Text = "";
                TxName.Text = "";
                return false;
            }
            bool flag = false;
            try
            {
                using (SqlConnection conn = new SqlConnection(Common.sqlConnString))
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = "select * from xa01 where xa1no =N'" + TKey.Trim() + "' COLLATE Chinese_Taiwan_Stroke_BIN";
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.HasRows && reader.Read())
                            {
                                flag = true;
                                TxNo.Text = reader["xa1no"].ToString();
                                TxName.Text = reader["xa1name"].ToString();
                                return flag;
                            }
                            else
                            {
                                TxNo.Text = TKey;
                                TxName.Text = "";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return flag;
            }
            return flag;
        }
        public static bool XX01Validate(string TKey, TextBox TxNo, TextBox TxName)
        {
            if (TKey.Trim() == "")
            {
                TxNo.Text = "";
                TxName.Text = "";
                return false;
            }
            bool flag = false;
            try
            {
                using (SqlConnection conn = new SqlConnection(Common.sqlConnString))
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = "select * from xx01 where x1no =N'" + TKey.Trim() + "' COLLATE Chinese_Taiwan_Stroke_BIN";
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.HasRows && reader.Read())
                            {
                                flag = true;
                                TxNo.Text = reader["x1no"].ToString();
                                TxName.Text = reader["x1name"].ToString();
                                return flag;
                            }
                            else
                            {
                                TxNo.Text = TKey;
                                TxName.Text = "";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return flag;
            }
            return flag;
        }
        public static bool XX02Validate(string TKey, TextBox TxNo, TextBox TxName)
        {
            if (TKey.Trim() == "")
            {
                TxNo.Text = "";
                TxName.Text = "";
                return false;
            }
            bool flag = false;
            try
            {
                using (SqlConnection conn = new SqlConnection(Common.sqlConnString))
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = "select * from xx02 where x2no =N'" + TKey.Trim() + "' COLLATE Chinese_Taiwan_Stroke_BIN";
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.HasRows && reader.Read())
                            {
                                flag = true;
                                TxNo.Text = reader["x2no"].ToString();
                                TxName.Text = reader["x2name"].ToString();
                                return flag;
                            }
                            else
                            {
                                TxNo.Text = TKey;
                                TxName.Text = "";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return flag;
            }
            return flag;
        }
        public static bool XX03Validate(string TKey, TextBox TxNo, TextBox TxName)
        {
            if (TKey.Trim() == "")
            {
                TxNo.Text = "";
                TxName.Text = "";
                return false;
            }
            bool flag = false;
            try
            {
                using (SqlConnection conn = new SqlConnection(Common.sqlConnString))
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = "select * from xx03 where x3no =N'" + TKey.Trim() + "' COLLATE Chinese_Taiwan_Stroke_BIN";
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.HasRows && reader.Read())
                            {
                                flag = true;
                                TxNo.Text = reader["x3no"].ToString();
                                TxName.Text = reader["x3name"].ToString();
                                return flag;
                            }
                            else
                            {
                                TxNo.Text = TKey;
                                TxName.Text = "";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return flag;
            }
            return flag;
        }
        public static bool XX04Validate(string TKey, TextBox TxNo, TextBox TxName)
        {
            if (TKey.Trim() == "")
            {
                TxNo.Text = "";
                TxName.Text = "";
                return false;
            }
            bool flag = false;
            try
            {
                using (SqlConnection conn = new SqlConnection(Common.sqlConnString))
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = "select * from xx04 where x4no =N'" + TKey.Trim() + "' COLLATE Chinese_Taiwan_Stroke_BIN";
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.HasRows && reader.Read())
                            {
                                flag = true;
                                TxNo.Text = reader["x4no"].ToString();
                                TxName.Text = reader["x4name"].ToString();
                                return flag;
                            }
                            else
                            {
                                TxNo.Text = TKey;
                                TxName.Text = "";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return flag;
            }
            return flag;
        }
        public static bool XX05Validate(string TKey, TextBox TxNo, TextBox TxName)
        {
            if (TKey.Trim() == "")
            {
                TxNo.Text = "";
                TxName.Text = "";
                return false;
            }
            bool flag = false;
            try
            {
                using (SqlConnection conn = new SqlConnection(Common.sqlConnString))
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = "select * from xx05 where x5no =N'" + TKey.Trim() + "' COLLATE Chinese_Taiwan_Stroke_BIN";
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.HasRows && reader.Read())
                            {
                                flag = true;
                                TxNo.Text = reader["x5no"].ToString();
                                TxName.Text = reader["x5name"].ToString();
                                return flag;
                            }
                            else
                            {
                                TxNo.Text = TKey;
                                TxName.Text = "";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return flag;
            }
            return flag;
        }
        public static bool XX06Validate(string TKey, TextBox TxNo, TextBox TxName)
        {
            if (TKey.Trim() == "")
            {
                TxNo.Text = "";
                TxName.Text = "";
                return false;
            }
            bool flag = false;
            try
            {
                using (SqlConnection conn = new SqlConnection(Common.sqlConnString))
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = "select * from xx06 where x6no =N'" + TKey.Trim() + "' COLLATE Chinese_Taiwan_Stroke_BIN";
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.HasRows && reader.Read())
                            {
                                flag = true;
                                TxNo.Text = reader["x6no"].ToString();
                                TxName.Text = reader["x6name"].ToString();
                                return flag;
                            }
                            else
                            {
                                TxNo.Text = TKey;
                                TxName.Text = "";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return flag;
            }
            return flag;
        }
        public static bool XX12Validate(string TKey, TextBox TxNo, TextBox TxName)
        {
            if (TKey.Trim() == "")
            {
                TxNo.Text = "";
                TxName.Text = "";
                return false;
            }
            bool flag = false;
            try
            {
                using (SqlConnection conn = new SqlConnection(Common.sqlConnString))
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = "select * from xx12 where x12no =N'" + TKey.Trim() + "' COLLATE Chinese_Taiwan_Stroke_BIN";
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.HasRows && reader.Read())
                            {
                                flag = true;
                                TxNo.Text = reader["x12no"].ToString();
                                TxName.Text = reader["x12name"].ToString();
                                return flag;
                            }
                            else
                            {
                                TxNo.Text = TKey;
                                TxName.Text = "";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return flag;
            }
            return flag;
        }
        public static bool EmplValidate(string TKey, TextBox TxNo, TextBox TxName)
        {
            if (TKey.Trim() == "")
            {
                TxNo.Text = "";
                TxName.Text = "";
                return false;
            }
            bool flag = false;
            try
            {
                using (SqlConnection conn = new SqlConnection(Common.sqlConnString))
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = "select emno,emname from empl where emno =N'" + TKey.Trim() + "' COLLATE Chinese_Taiwan_Stroke_BIN";
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.HasRows && reader.Read())
                            {
                                flag = true;
                                TxNo.Text = reader["emno"].ToString();
                                TxName.Text = reader["emname"].ToString();
                                return flag;
                            }
                            else
                            {
                                TxNo.Text = TKey;
                                TxName.Text = "";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return flag;
            }
            return flag;
        }
        public static bool CuPareValidate(string TKey, TextBox TxNo, TextBox TxName)
        {
            if (TKey.Trim() == "")
            {
                TxNo.Text = "";
                TxName.Text = "";
                return false;
            }
            bool flag = false;
            try
            {
                using (SqlConnection conn = new SqlConnection(Common.sqlConnString))
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = "select cuno,cuname1 from cust where cuno =N'" + TKey.Trim() + "' COLLATE Chinese_Taiwan_Stroke_BIN";
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.HasRows && reader.Read())
                            {
                                flag = true;
                                TxNo.Text = reader["cuno"].ToString();
                                TxName.Text = reader["cuname1"].ToString();
                                return flag;
                            }
                            else
                            {
                                TxNo.Text = TKey;
                                TxName.Text = "";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return flag;
            }
            return flag;
        }
        public static bool StkValidate(string TKey, TextBox TxNo, TextBox TxName)
        {
            if (TKey.Trim() == "")
            {
                TxNo.Text = "";
                TxName.Text = "";
                return false;
            }
            bool flag = false;
            try
            {
                using (SqlConnection conn = new SqlConnection(Common.sqlConnString))
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = "select stno,stname from stkroom where stno =N'" + TKey.Trim() + "' COLLATE Chinese_Taiwan_Stroke_BIN";
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.HasRows && reader.Read())
                            {
                                flag = true;
                                TxNo.Text = reader["stno"].ToString();
                                TxName.Text = reader["stname"].ToString();
                                return flag;
                            }
                            else
                            {
                                TxNo.Text = TKey;
                                TxName.Text = "";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return flag;
            }
            return flag;
        }
        public static bool SendValidate(string TKey, TextBox TxNo, TextBox TxName)
        {
            if (TKey.Trim() == "")
            {
                TxNo.Text = "";
                TxName.Text = "";
                return false;
            }
            bool flag = false;
            try
            {
                using (SqlConnection conn = new SqlConnection(Common.sqlConnString))
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = "select seno,sename from send where seno =N'" + TKey.Trim() + "' COLLATE Chinese_Taiwan_Stroke_BIN";
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.HasRows && reader.Read())
                            {
                                flag = true;
                                TxNo.Text = reader["seno"].ToString();
                                TxName.Text = reader["sename"].ToString();
                                return flag;
                            }
                            else
                            {
                                TxNo.Text = TKey;
                                TxName.Text = "";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return flag;
            }
            return flag;
        }
        public static bool SpecValidate(string TKey, TextBox TxNo, TextBox TxName)
        {
            if (TKey.Trim() == "")
            {
                TxNo.Text = "";
                TxName.Text = "";
                return false;
            }
            bool flag = false;
            try
            {
                using (SqlConnection conn = new SqlConnection(Common.sqlConnString))
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = "select spno,spname from spec where spno =N'" + TKey.Trim() + "' COLLATE Chinese_Taiwan_Stroke_BIN";
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.HasRows && reader.Read())
                            {
                                flag = true;
                                TxNo.Text = reader["spno"].ToString();
                                TxName.Text = reader["spname"].ToString();
                                return flag;
                            }
                            else
                            {
                                TxNo.Text = TKey;
                                TxName.Text = "";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return flag;
            }
            return flag;
        }
        public static bool DeptValidate(string TKey, TextBox TxNo, TextBox TxName)
        {
            if (TKey.Trim() == "")
            {
                TxNo.Text = "";
                TxName.Text = "";
                return false;
            }
            bool flag = false;
            try
            {
                using (SqlConnection conn = new SqlConnection(Common.sqlConnString))
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = "select deno,dename1 from dept where deno =N'" + TKey.Trim() + "' COLLATE Chinese_Taiwan_Stroke_BIN";
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.HasRows && reader.Read())
                            {
                                flag = true;
                                TxNo.Text = reader["deno"].ToString();
                                TxName.Text = reader["dename1"].ToString();
                                return flag;
                            }
                            else
                            {
                                TxNo.Text = TKey;
                                TxName.Text = "";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return flag;
            }
            return flag;
        }
        public static bool KindValidate(string TKey, TextBox TxNo, TextBox TxName)
        {
            if (TKey.Trim() == "")
            {
                TxNo.Text = "";
                TxName.Text = "";
                return false;
            }
            bool flag = false;
            try
            {
                using (SqlConnection conn = new SqlConnection(Common.sqlConnString))
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = "select kino,kiname from kind where kino =N'" + TKey.Trim() + "' COLLATE Chinese_Taiwan_Stroke_BIN";
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.HasRows && reader.Read())
                            {
                                flag = true;
                                TxNo.Text = reader["kino"].ToString();
                                TxName.Text = reader["kiname"].ToString();
                                return flag;
                            }
                            else
                            {
                                TxNo.Text = TKey;
                                TxName.Text = "";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return flag;
            }
            return flag;
        }
        public static bool ItemValidate(string TKey, TextBox TxNo, TextBox TxName)
        {
            if (TKey.Trim() == "")
            {
                TxNo.Text = "";
                TxName.Text = "";
                return false;
            }
            bool flag = false;
            try
            {
                using (SqlConnection conn = new SqlConnection(Common.sqlConnString))
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = "select itno,itname from item where itno =N'" + TKey.Trim() + "' COLLATE Chinese_Taiwan_Stroke_BIN";
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.HasRows && reader.Read())
                            {
                                flag = true;
                                TxNo.Text = reader["itno"].ToString();
                                TxName.Text = reader["itname"].ToString();
                                return flag;
                            }
                            else
                            {
                                TxNo.Text = TKey;
                                TxName.Text = "";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return flag;
            }
            return flag;
        }
        public static bool ItemValidate(string TKey)
        {
            if (TKey.IsNullOrEmpty()) return false;
            bool flag = false;
            try
            {
                using (SqlConnection conn = new SqlConnection(Common.sqlConnString))
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = "select COUNT(*) from item where itno =N'" + TKey.Trim() + "' COLLATE Chinese_Taiwan_Stroke_BIN";
                        if (cmd.ExecuteScalar().ToDecimal() > 0) flag = true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return flag;
            }
            return flag;
        }


        public static void Xa01OpenForm(TextBox TxNo, TextBox TxName = null, bool append = false, ViewMode mode = ViewMode.Normal)
        {
            //if (mode == ViewMode.Normal) if (TxNo.ReadOnly) return;
            //using (s_3基本資料.貨幣開窗 frm = new s_3基本資料.貨幣開窗())
            //{
            //    frm.SetParaeter(mode);
            //    frm.CanAppend = append;
            //    frm.SeekNo = TxNo.Text.Trim();

            //    switch (frm.ShowDialog())
            //    {
            //        case DialogResult.OK:
            //            TxNo.Text = frm.Result["Xa1No"].ToString();
            //            if (TxName != null)
            //                TxName.Text = frm.Result["Xa1Name"].ToString();
            //            break;
            //        case DialogResult.Cancel: break;
            //    }
            //}
        }
        public static void BomOpenForm(TextBox TxNo, TextBox TxName = null, bool append = false, ViewMode mode = ViewMode.Normal)
        {
            //if (mode == ViewMode.Normal) if (TxNo.ReadOnly) return;
            //using (subMenuFm_5.FrmItemb_Bom frm = new subMenuFm_5.FrmItemb_Bom())
            //{
            //    frm.SetParaeter(mode);
            //    frm.CanAppend = append;
            //    frm.SeekNo = TxNo.Text.Trim();

            //    switch (frm.ShowDialog())
            //    {
            //        case DialogResult.OK:
            //            TxNo.Text = frm.Result["ItNo"].ToString();
            //            if (TxName != null)
            //                TxName.Text = frm.Result["ItName"].ToString();
            //            break;
            //        case DialogResult.Cancel: break;
            //    }
            //}
        }
        public static void XX12OpenForm(TextBox TxNo, TextBox TxName = null, bool append = false, ViewMode mode = ViewMode.Normal)
        {
            //if (mode == ViewMode.Normal) if (TxNo.ReadOnly) return;
            //using (subMenuFm_5.FrmXX12Brow frm = new subMenuFm_5.FrmXX12Brow())
            //{
            //    frm.SetParaeter(mode);
            //    frm.CanAppend = append;
            //    frm.SeekNo = TxNo.Text.Trim();

            //    switch (frm.ShowDialog())
            //    {
            //        case DialogResult.OK:
            //            TxNo.Text = frm.Result["X12No"].ToString();
            //            if (TxName != null)
            //                TxName.Text = frm.Result["X12Name"].ToString();
            //            break;
            //        case DialogResult.Cancel: break;
            //    }
            //}
        }
        public static void XX01OpenForm(TextBox TxNo, TextBox TxName = null, bool append = false, ViewMode mode = ViewMode.Normal)
        {
            //if (mode == ViewMode.Normal) if (TxNo.ReadOnly) return;
            //using (subMenuFm_5.FrmXX01Brow frm = new subMenuFm_5.FrmXX01Brow())
            //{
            //    frm.SetParaeter(mode);
            //    frm.CanAppend = append;
            //    frm.SeekNo = TxNo.Text.Trim();

            //    switch (frm.ShowDialog())
            //    {
            //        case DialogResult.OK:
            //            TxNo.Text = frm.Result["X1No"].ToString();
            //            if (TxName != null)
            //                TxName.Text = frm.Result["X1Name"].ToString();
            //            break;
            //        case DialogResult.Cancel: break;
            //    }
            //}
        }
        public static void XX02OpenForm(TextBox TxNo, TextBox TxName = null, bool append = false, ViewMode mode = ViewMode.Normal)
        {
            //if (mode == ViewMode.Normal) if (TxNo.ReadOnly) return;
            //using (subMenuFm_5.FrmXX02Brow frm = new subMenuFm_5.FrmXX02Brow())
            //{
            //    frm.SetParaeter(mode);
            //    frm.CanAppend = append;
            //    frm.SeekNo = TxNo.Text.Trim();

            //    switch (frm.ShowDialog())
            //    {
            //        case DialogResult.OK:
            //            TxNo.Text = frm.Result["X2No"].ToString();
            //            if (TxName != null)
            //                TxName.Text = frm.Result["X2Name"].ToString();
            //            break;
            //        case DialogResult.Cancel: break;
            //    }
            //}
        }
        public static void XX04OpenForm(TextBox TxNo, TextBox TxName = null, bool append = false, ViewMode mode = ViewMode.Normal)
        {
            //if (mode == ViewMode.Normal) if (TxNo.ReadOnly) return;
            //using (subMenuFm_5.FrmXX04Brow frm = new subMenuFm_5.FrmXX04Brow())
            //{
            //    frm.SetParaeter(mode);
            //    frm.CanAppend = append;
            //    frm.SeekNo = TxNo.Text.Trim();

            //    switch (frm.ShowDialog())
            //    {
            //        case DialogResult.OK:
            //            TxNo.Text = frm.Result["X4No"].ToString();
            //            if (TxName != null)
            //                TxName.Text = frm.Result["X4Name"].ToString();
            //            break;
            //        case DialogResult.Cancel: break;
            //    }
            //}
        }
        public static void XX05OpenForm(TextBox TxNo, TextBox TxName = null, bool append = false, ViewMode mode = ViewMode.Normal)
        {
            //if (mode == ViewMode.Normal) if (TxNo.ReadOnly) return;
            //using (subMenuFm_5.FrmXX05Brow frm = new subMenuFm_5.FrmXX05Brow())
            //{
            //    frm.SetParaeter(mode);
            //    frm.CanAppend = append;
            //    frm.SeekNo = TxNo.Text.Trim();

            //    switch (frm.ShowDialog())
            //    {
            //        case DialogResult.OK:
            //            TxNo.Text = frm.Result["X5No"].ToString();
            //            if (TxName != null)
            //                TxName.Text = frm.Result["X5Name"].ToString();
            //            break;
            //        case DialogResult.Cancel: break;
            //    }
            //}
        }
        public static void XX06OpenForm(TextBox TxNo, TextBox TxName = null, bool append = false, ViewMode mode = ViewMode.Normal)
        {
            //if (mode == ViewMode.Normal) if (TxNo.ReadOnly) return;
            //using (subMenuFm_5.FrmXX06b frm = new subMenuFm_5.FrmXX06b())
            //{
            //    frm.SetParaeter(mode);
            //    frm.CanAppend = append;
            //    frm.SeekNo = TxNo.Text.Trim();

            //    switch (frm.ShowDialog())
            //    {
            //        case DialogResult.OK:
            //            TxNo.Text = frm.Result["X6No"].ToString();
            //            if (TxName != null)
            //                TxName.Text = frm.Result["X6Name"].ToString();
            //            break;
            //        case DialogResult.Cancel: break;
            //    }
            //}
        }
        public static void CustOpenForm(TextBox TxNo, bool append = false, ViewMode mode = ViewMode.Normal)
        {
            //if (mode == ViewMode.Normal) if (TxNo.ReadOnly) return;
            //using (subMenuFm_5.FrmCustb frm = new subMenuFm_5.FrmCustb())
            //{
            //    frm.SetParaeter(mode);
            //    frm.CanAppend = append;
            //    frm.SeekNo = TxNo.Text.Trim();

            //    switch (frm.ShowDialog())
            //    {
            //        case DialogResult.OK:
            //            TxNo.Text = frm.Result["CuNo"].ToString();
            //            break;
            //        case DialogResult.Cancel: break;
            //    }
            //}
        }
        public static void FactOpenForm(TextBox TxNo, bool append = false, ViewMode mode = ViewMode.Normal)
        {
            //if (mode == ViewMode.Normal) if (TxNo.ReadOnly) return;
            //using (subMenuFm_5.FrmFactb frm = new subMenuFm_5.FrmFactb())
            //{
            //    frm.SetParaeter(mode);
            //    frm.CanAppend = append;
            //    frm.SeekNo = TxNo.Text.Trim();

            //    switch (frm.ShowDialog())
            //    {
            //        case DialogResult.OK:
            //            TxNo.Text = frm.Result["FaNo"].ToString();
            //            break;
            //        case DialogResult.Cancel: break;
            //    }
            //}
        }
        public static void ItemOpenForm(TextBox TxNo, bool append = false, ViewMode mode = ViewMode.Normal)
        {
            //if (mode == ViewMode.Normal) if (TxNo.ReadOnly) return;
            //using (subMenuFm_5.FrmItemb frm = new subMenuFm_5.FrmItemb())
            //{
            //    frm.SetParaeter(mode);
            //    frm.CanAppend = append;
            //    frm.SeekNo = TxNo.Text.Trim();

            //    switch (frm.ShowDialog())
            //    {
            //        case DialogResult.OK:
            //            TxNo.Text = frm.Result["itNo"].ToString();
            //            break;
            //        case DialogResult.Cancel: break;
            //    }
            //}
        }
        public static void EmplOpenForm(TextBox TxNo, TextBox TxName = null, bool append = false, ViewMode mode = ViewMode.Normal)
        {
            //if (mode == ViewMode.Normal) if (TxNo.ReadOnly) return;
            //using (subMenuFm_5.FrmEmplb frm = new subMenuFm_5.FrmEmplb())
            //{
            //    frm.SetParaeter(mode);
            //    frm.CanAppend = append;
            //    frm.SeekNo = TxNo.Text.Trim();

            //    switch (frm.ShowDialog())
            //    {
            //        case DialogResult.OK:
            //            TxNo.Text = frm.Result["emNo"].ToString();
            //            if (TxName != null)
            //                TxName.Text = frm.Result["emName"].ToString();
            //            break;
            //        case DialogResult.Cancel: break;
            //    }
            //}
        }
        public static void KindOpenForm(TextBox TxNo, TextBox TxName = null, bool append = false, ViewMode mode = ViewMode.Normal)
        {
            //if (mode == ViewMode.Normal) if (TxNo.ReadOnly) return;
            //using (subMenuFm_5.FrmKindBrow frm = new subMenuFm_5.FrmKindBrow())
            //{
            //    frm.SetParaeter(mode);
            //    frm.CanAppend = append;
            //    frm.SeekNo = TxNo.Text.Trim();

            //    switch (frm.ShowDialog())
            //    {
            //        case DialogResult.OK:
            //            TxNo.Text = frm.Result["kiNo"].ToString();
            //            if (TxName != null)
            //                TxName.Text = frm.Result["kiName"].ToString();
            //            break;
            //        case DialogResult.Cancel: break;
            //    }
            //}
        }
        public static void DeptOpenForm(TextBox TxNo, TextBox TxName = null, bool append = false, ViewMode mode = ViewMode.Normal)
        {
            //if (mode == ViewMode.Normal) if (TxNo.ReadOnly) return;
            //using (subMenuFm_5.FrmDeptb frm = new subMenuFm_5.FrmDeptb())
            //{
            //    frm.SetParaeter(mode);
            //    frm.CanAppend = append;
            //    frm.SeekNo = TxNo.Text.Trim();

            //    switch (frm.ShowDialog())
            //    {
            //        case DialogResult.OK:
            //            TxNo.Text = frm.Result["DeNo"].ToString();
            //            if (TxName != null)
            //                TxName.Text = frm.Result["DeName1"].ToString();
            //            break;
            //        case DialogResult.Cancel: break;
            //    }
            //}
        }
        public static void SendOpenForm(TextBox TxNo, TextBox TxName = null, bool append = false, ViewMode mode = ViewMode.Normal)
        {
            //if (mode == ViewMode.Normal) if (TxNo.ReadOnly) return;
            //using (subMenuFm_5.FrmSendBrow frm = new subMenuFm_5.FrmSendBrow())
            //{
            //    frm.SetParaeter(mode);
            //    frm.CanAppend = append;
            //    frm.SeekNo = TxNo.Text.Trim();

            //    switch (frm.ShowDialog())
            //    {
            //        case DialogResult.OK:
            //            TxNo.Text = frm.Result["SeNo"].ToString();
            //            if (TxName != null)
            //                TxName.Text = frm.Result["SeName"].ToString();
            //            break;
            //        case DialogResult.Cancel: break;
            //    }
            //}
        }
        public static void SpecOpenForm(TextBox TxNo, TextBox TxName = null, bool append = false, ViewMode mode = ViewMode.Normal)
        {
            //if (mode == ViewMode.Normal) if (TxNo.ReadOnly) return;
            //using (subMenuFm_5.FrmSpecBrow frm = new subMenuFm_5.FrmSpecBrow())
            //{
            //    frm.SetParaeter(mode);
            //    frm.CanAppend = append;
            //    frm.SeekNo = TxNo.Text.Trim();

            //    switch (frm.ShowDialog())
            //    {
            //        case DialogResult.OK:
            //            TxNo.Text = frm.Result["SpNo"].ToString();
            //            if (TxName != null)
            //                TxName.Text = frm.Result["SpName"].ToString();
            //            break;
            //        case DialogResult.Cancel: break;
            //    }
            //}
        }
        public static void TradeOpenForm(TextBox TxNo, TextBox TxName = null, bool append = false, ViewMode mode = ViewMode.Normal)
        {
            //if (mode == ViewMode.Normal) if (TxNo.ReadOnly) return;
            //using (subMenuFm_5.FrmTradeb frm = new subMenuFm_5.FrmTradeb())
            //{
            //    frm.SetParaeter(mode);
            //    frm.CanAppend = append;
            //    frm.SeekNo = TxNo.Text.Trim();

            //    switch (frm.ShowDialog())
            //    {
            //        case DialogResult.OK:
            //            TxNo.Text = frm.Result["TrNo"].ToString();
            //            if (TxName != null)
            //                TxName.Text = frm.Result["TrName"].ToString();
            //            break;
            //        case DialogResult.Cancel: break;
            //    }
            //}
        }
        public static void PerNoteOpenForm(TextBox TxNo, ViewMode mode = ViewMode.Normal)
        {
            //if (mode == ViewMode.Normal) if (TxNo.ReadOnly) return;
            //using (subMenuFm_5.FrmPerNote frm = new subMenuFm_5.FrmPerNote())
            //{
            //    frm.SetParaeter(mode);
            //    frm.SeekNo = TxNo.Text.Trim();

            //    switch (frm.ShowDialog())
            //    {
            //        case DialogResult.OK:
            //            TxNo.Text = frm.Result;
            //            break;
            //        case DialogResult.Cancel: break;
            //    }
            //}
        }
        public static void PayNoteOpenForm(TextBox TxNo, ViewMode mode = ViewMode.Normal)
        {
            //if (mode == ViewMode.Normal) if (TxNo.ReadOnly) return;
            //using (subMenuFm_5.FrmPayNoteBrow frm = new subMenuFm_5.FrmPayNoteBrow())
            //{
            //    frm.SetParaeter(mode);
            //    frm.SeekNo = TxNo.Text.Trim();

            //    switch (frm.ShowDialog())
            //    {
            //        case DialogResult.OK:
            //            TxNo.Text = frm.Result;
            //            break;
            //        case DialogResult.Cancel: break;
            //    }
            //}
        }
        public static void StkRoomOpenForm(TextBox TxNo, TextBox TxName = null, bool append = false, ViewMode mode = ViewMode.Normal)
        {
            //if (mode == ViewMode.Normal) if (TxNo.ReadOnly) return;
            //using (subMenuFm_5.FrmStkRoomBrow frm = new subMenuFm_5.FrmStkRoomBrow())
            //{
            //    frm.SetParaeter(mode);
            //    frm.CanAppend = append;
            //    frm.SeekNo = TxNo.Text.Trim();

            //    switch (frm.ShowDialog())
            //    {
            //        case DialogResult.OK:
            //            TxNo.Text = frm.Result["StNo"].ToString();
            //            if (TxName != null)
            //                TxName.Text = frm.Result["StName"].ToString();
            //            break;
            //        case DialogResult.Cancel: break;
            //    }
            //}
        }
        public static void MemoMemoOpenForm(TextBox TxNo, int Len)
        {
            //if (TxNo.ReadOnly) return;
            //using (subMenuFm_2.FrmSale_Memo frm = new subMenuFm_2.FrmSale_Memo())
            //{
            //    frm.SetParaeter(ViewMode.Normal);
            //    switch (frm.ShowDialog())
            //    {
            //        case DialogResult.OK:
            //            TxNo.Text = frm.Memo.GetUTF8(Len);
            //            break;
            //        case DialogResult.Cancel: break;
            //    }
            //}
        }
        public static void PictureOpenForm(string no)
        {
            //using (subMenuFm_2.FrmShowPicture frm = new subMenuFm_2.FrmShowPicture())
            //{
            //    frm.SetParaeter();
            //    frm.ItNo = no;
            //    frm.ShowDialog();
            //}
        }



        //單據
        public static void ShiftOpenForm(TextBox TxNo, TextBox TxName = null, bool append = false, ViewMode mode = ViewMode.Normal)
        {
            //if (mode == ViewMode.Normal) if (TxNo.ReadOnly) return;
            //using (subMenuFm_2.FrmShiftBrow frm = new subMenuFm_2.FrmShiftBrow())
            //{
            //    frm.SetParaeter(mode);
            //    frm.CanAppend = append;
            //    frm.SeekNo = TxNo.Text.Trim();

            //    switch (frm.ShowDialog())
            //    {
            //        case DialogResult.OK:
            //            TxNo.Text = frm.Result["ShNo"].ToString();
            //            if (TxName != null)
            //                TxName.Text = frm.Result["ShName"].ToString();
            //            break;
            //        case DialogResult.Cancel: break;
            //    }
            //}
        }
        
        //public static void WorkStationOpenForm(TextBox TxNo, TextBox TxName = null, bool append = false, ViewMode mode = ViewMode.Normal)
        //{
        //    if (mode == ViewMode.Normal) if (TxNo.ReadOnly) return;
        //    using (subMenuFm_2.FrmWorkStationBrow frm = new subMenuFm_2.FrmWorkStationBrow())
        //    {
        //        frm.SetParaeter(mode);
        //        frm.CanAppend = append;
        //        frm.SeekNo = TxNo.Text.Trim();

        //        switch (frm.ShowDialog())
        //        {
        //            case DialogResult.OK:
        //                TxNo.Text = frm.Result["WoNo"].ToString();
        //                if (TxName != null)
        //                    TxName.Text = frm.Result["WoName"].ToString();
        //                break;
        //            case DialogResult.Cancel: break;
        //        }
        //    }
        //}
        //public static bool WorkStationValidate(TextBox TxNo, TextBox TxName)
        //{
        //    if (TxNo.Text.Trim() == "")
        //    {
        //        TxNo.Text = "";
        //        TxName.Text = "";
        //        return false;
        //    }
        //    bool flag = false;
        //    try
        //    {
        //        using (SqlConnection conn = new SqlConnection(Common.sqlConnString))
        //        {
        //            conn.Open();
        //            using (SqlCommand cmd = conn.CreateCommand())
        //            {
        //                cmd.CommandText = "select * from workstation where WoNo =N'" + TxNo.Text.Trim() + "' COLLATE Chinese_Taiwan_Stroke_BIN";
        //                using (SqlDataReader reader = cmd.ExecuteReader())
        //                {
        //                    if (reader.HasRows && reader.Read())
        //                    {
        //                        flag = true;
        //                        TxNo.Text = reader["WoNo"].ToString();
        //                        TxName.Text = reader["WoName"].ToString();
        //                        return flag;
        //                    }
        //                    else
        //                    {
        //                        TxName.Text = "";
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.ToString());
        //        return flag;
        //    }
        //    return flag;
        //}
        public static bool XX03Validate(string TKey, TextBox TxNo, TextBox TxName, TextBox TxRate)
        {
            if (TKey.Trim() == "")
            {
                TxNo.Text = "";
                TxName.Text = "";
                return false;
            }
            bool flag = false;
            try
            {
                using (SqlConnection conn = new SqlConnection(Common.sqlConnString))
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = "select * from xx03 where x3no =N'" + TKey.Trim() + "' COLLATE Chinese_Taiwan_Stroke_BIN";
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.HasRows && reader.Read())
                            {
                                flag = true;
                                TxNo.Text = reader["x3no"].ToString();
                                TxName.Text = reader["x3name"].ToString();
                                decimal rate = 0;
                                decimal.TryParse(reader["x3rate"].ToString(), out rate);
                                rate = rate * 100;
                                TxRate.Text = rate.ToString("f0");
                                return flag;
                            }
                            else
                            {
                                TxNo.Text = TKey;
                                TxName.Text = "";
                                TxRate.Text = "";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return flag;
            }
            return flag;
        }
        public static bool QuoteValidate(TextBox TxNo)
        {
            if (TxNo.Text.Trim() == "")
            {
                TxNo.Text = "";
                return true;
            }
            try
            {
                using (SqlConnection conn = new SqlConnection(Common.sqlConnString))
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = "select count(*) from quote where quno =N'" + TxNo.Text.Trim() + "' COLLATE Chinese_Taiwan_Stroke_BIN";
                        if (cmd.ExecuteScalar().ToString() == "0") return false;
                        else return true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return false;
            }
        }
        public static DataRow getCustRow(string cuno)
        {
            try
            {
                DataTable dt = new DataTable();
                using (SqlConnection cn = new SqlConnection(Common.sqlConnString))
                {
                    string sql = "select * from cust where cuno=N'" + cuno + "'";
                    using (SqlDataAdapter da = new SqlDataAdapter(sql, cn))
                    {
                        da.Fill(dt);
                    }
                }
                if (dt.Rows.Count > 0) return dt.Rows[0];
                else return null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return null;
            }
        }
        public static DataRow getFactRow(string fano)
        {
            try
            {
                DataTable dt = new DataTable();
                using (SqlConnection cn = new SqlConnection(Common.sqlConnString))
                {
                    string sql = "select * from fact where fano=N'" + fano + "'";
                    using (SqlDataAdapter da = new SqlDataAdapter(sql, cn))
                    {
                        da.Fill(dt);
                    }
                }
                if (dt.Rows.Count > 0) return dt.Rows[0];
                else return null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return null;
            }
        }
        public static DataRow getItemRow(string itno)
        {
            try
            {
                DataTable dt = new DataTable();
                using (SqlConnection cn = new SqlConnection(Common.sqlConnString))
                {
                    string sql = "select * from item where itno=N'" + itno + "'";
                    using (SqlDataAdapter da = new SqlDataAdapter(sql, cn))
                    {
                        da.Fill(dt);
                    }
                }
                if (dt.Rows.Count > 0) return dt.Rows[0];
                else return null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return null;
            }
        }
        public static bool FQuotValidate(TextBox TxNo)
        {
            if (TxNo.Text.Trim() == "")
            {
                TxNo.Text = "";
                return true;
            }
            try
            {
                using (SqlConnection conn = new SqlConnection(Common.sqlConnString))
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = "select count(*) from fquot where fqno =N'" + TxNo.Text.Trim() + "' COLLATE Chinese_Taiwan_Stroke_BIN";
                        if (cmd.ExecuteScalar().ToString() == "0") return false;
                        else return true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return false;
            }
        }
        public static bool SaleValidate(TextBox TxNo)
        {
            if (TxNo.Text.Trim() == "")
            {
                TxNo.Text = "";
                return true;
            }
            try
            {
                using (SqlConnection conn = new SqlConnection(Common.sqlConnString))
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = "select count(*) from sale where sano =N'" + TxNo.Text.Trim() + "' COLLATE Chinese_Taiwan_Stroke_BIN";
                        if (cmd.ExecuteScalar().ToString() == "0") return false;
                        else return true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return false;
            }
        }
        public static bool BShopValidate(TextBox TxNo)
        {
            if (TxNo.Text.Trim() == "")
            {
                TxNo.Text = "";
                return true;
            }
            try
            {
                using (SqlConnection conn = new SqlConnection(Common.sqlConnString))
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = "select count(*) from bshop where bsno =N'" + TxNo.Text.Trim() + "' COLLATE Chinese_Taiwan_Stroke_BIN";
                        if (cmd.ExecuteScalar().ToString() == "0") return false;
                        else return true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return false;
            }
        }

        //其它
        public static void SetRadioUdf(List<Control> pnlist, string TFormName)
        {
            List<DataRow> rdlist = new List<DataRow>();
            try
            {
                using (SqlConnection conn = new SqlConnection(Common.sqlConnString))
                {
                    string sql = "select * from usrcapt where formname ='" + TFormName + "'";
                    using (SqlDataAdapter da = new SqlDataAdapter(sql, conn))
                    {
                        DataTable table = new DataTable();
                        da.Fill(table);
                        if (table.Rows.Count > 0)
                            rdlist = table.AsEnumerable().ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                DataRow row = null;
                RadioButton rd = null;
                if (rdlist.Count > 0)
                {
                    foreach (Control pnl in pnlist)
                    {
                        row = rdlist.Find(r => r["classname"].ToString() == pnl.Name);
                        if (row != null)
                        {
                            rd = pnl.Controls.OfType<RadioButton>().FirstOrDefault(r => r.Name == row["usrvaluen"].ToString());
                            if (rd != null)
                                rd.Checked = true;
                        }
                    }
                }
            }
        }
        public static void SaveRadioUdf(List<Control> pnlist, string TFormName)
        {
            string sudf = "delete from usrcapt where formname ='" + TFormName + "';";
            foreach (Control cl in pnlist)
            {
                string rdname = cl.Controls.OfType<RadioButton>().Where(r => r.Checked).Select(r => r.Name).FirstOrDefault();
                sudf += "insert into usrcapt ([formname],[classname],[usrvaluen]) values ('" + TFormName + "','" + cl.Name + "','" + rdname + "');";
            }
            try
            {
                using (SqlConnection conn = new SqlConnection(Common.sqlConnString))
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = sudf;
                        cmd.ExecuteNonQuery();
                    }
                }
                MessageBox.Show("儲存成功", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        public static void ResetRadioUdf(string TFormName)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(Common.sqlConnString))
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = "delete from usrcapt where formname ='" + TFormName + "';";
                        cmd.ExecuteNonQuery();
                    }
                }
                MessageBox.Show("重置完成", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }


    }
}
