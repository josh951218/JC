using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Data.SqlClient;
using S_61.Basic;

namespace S_61.JS
{
    public class pos
    {
        public static string ServerConnString = "";
        public static string SIP = "";
        public static string StoreIP = "";
        static string STB = "";

        public static bool IsMutiPoint()
        {
            if (File.Exists(Common.StartUpIniFileName))
            {
                SIP = Common.GetPrivateProfileString("POS設定", "SERVER");
                return SIP.Trim() != "-1";
            }
            else
            {
                MessageBox.Show("總部資訊設定錯誤！", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
        }

        public static bool CheckConn()
        {
            try
            {
                if (File.Exists(Common.StartUpIniFileName))
                {
                    SIP = Common.GetPrivateProfileString("POS設定", "SERVER");
                    STB = Common.GetPrivateProfileString("POS設定", "DATA");
                    SqlConnectionStringBuilder ServerString = new SqlConnectionStringBuilder();
                    ServerString.DataSource = SIP;
                    ServerString.InitialCatalog = STB;
                    ServerString.UserID = "BMIDP";
                    ServerString.Password = "BMIDP";


                    ServerConnString = ServerString.ToString();
                    using (SqlConnection cn = new SqlConnection(ServerConnString))
                    {
                        cn.Open();
                        cn.Close();
                    }
                    return true;
                }
                else
                {
                    MessageBox.Show("總部資訊設定錯誤！", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("總部資訊設定錯誤！", "訊息視窗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                MessageBox.Show(ex.Message.ToString());
                return false;
            }
        }
    }
}
