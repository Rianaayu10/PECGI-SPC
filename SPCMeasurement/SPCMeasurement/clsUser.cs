using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPCMeasurement
{
    class clsUser
    {
        public string UserID { get; set; }
        public string FullName { get; set; }
        public string Password { get; set; }
        public string FactoryCode { get; set; }

    }

    class clsUserDB
    {
        private static clsConfig cfg = new clsConfig();
        private static clsConfigData cfd = cfg.uf_ReadConfig();
        private static string constr = cfd.ConnectionString;

        public static clsUser GetData(string UserID)
        {
            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("select * from spc_UserSetup where UserID = @UserID", con);
                cmd.Parameters.AddWithValue("UserID", UserID);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                if (dt.Rows.Count == 0)
                {
                    return null;
                } else
                {
                    SqlCommand cmd2 = new SqlCommand("select * from sys.objects where name = 'spc_UserInfo' and type = 'U'", con);
                    SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
                    DataTable dt2 = new DataTable();
                    da2.Fill(dt2);
                    clsDESEncryption clsDESEncryption = new clsDESEncryption("TOS");
                    clsUser User = new clsUser();
                    User.UserID = dt.Rows[0]["UserID"].ToString();
                    User.FullName = dt.Rows[0]["FullName"].ToString();
                    if (dt2.Rows.Count == 0)
                    {
                        User.Password = clsDESEncryption.DecryptData(dt.Rows[0]["Password"].ToString());
                    } else
                    {
                        User.Password = clsDESEncryption.Decrypt(dt.Rows[0]["Password"].ToString(), dt.Rows[0]["UserID"].ToString().ToUpper().Trim());
                    }                                        
                    User.FactoryCode = dt.Rows[0]["FactoryCode"].ToString().Trim();
                    return User;
                }
            }
        }

        public static bool AllowUpdate(string pUserID, string pMenuID)
        {            
            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                string q = "SELECT AllowUpdate FROM dbo.spc_UserPrivilege WHERE AppID = 'SPC' AND UserID = @UserID AND MenuID = @MenuID ";
                SqlCommand cmd = new SqlCommand(q, con);
                cmd.Parameters.AddWithValue("UserID", pUserID);
                cmd.Parameters.AddWithValue("MenuID", pMenuID);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                if(dt.Rows.Count > 0 && dt.Rows[0][0].ToString() == "1")
                {
                    return true;
                } 
                else
                {
                    return false;
                }
            }
        }
    }
}
