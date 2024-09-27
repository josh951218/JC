using System;
using System.Data.SqlClient;
using System.Windows.Forms;
using S_61.Basic;

namespace S_61.Model
{
    public class PowerOfPage
    {
        public static bool IsPowerful(object TKey)
        {
            if (TKey.IsNullOrEmpty()) return true;
            bool ispower = false;
            try
            {
                using (SqlConnection conn = new SqlConnection(Common.sqlConnString))
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = "select m.scname,d.* from scrit as m,scritd as d where '0'='0' "
                            + " and d.taname='" + TKey.ToString().Trim() + "'"
                            + " and m.scname=d.scname "
                            + " and m.scname='" + Common.User_Name + "' COLLATE Chinese_Taiwan_Stroke_BIN";

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.HasRows && reader.Read())
                            {
                                //檢查『無權限』是否有打v，沒打v則可以通行
                                //MessageBox.Show((reader["sc09"].ToString().Trim() == "").ToString());
                                if (reader["sc09"].ToString().Trim() == "")
                                    ispower = true;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            return ispower;
        }
    }
}
