using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace SPCMeasurement
{
    public class clsSPCResultDB
    {
        private static clsConfig cfg = new clsConfig();
        private static clsConfigData cfd = cfg.uf_ReadConfig();
        private static string constr = cfd.ConnectionString;

        public static IEnumerable<clsSPCResult> SPCResults
        {
            get
            {
                List<clsSPCResult> result = new List<clsSPCResult>();
                using (SqlConnection con = new SqlConnection(constr))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("sp_SPCResult", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataReader rd = cmd.ExecuteReader();
                    while (rd.Read())
                    {
                        clsSPCResult obj = new clsSPCResult();
                        obj.SPCResultID = Convert.ToInt32(rd["SPCResultID"]);
                        obj.FactoryCode = rd["FactoryCode"].ToString().Trim();
                        obj.ItemTypeCode = rd["ItemTypeCode"].ToString().Trim();
                        obj.LineCode = rd["LineCode"].ToString().Trim();
                        obj.ItemCheckCode = rd["ItemCheckCode"].ToString().Trim();
                        obj.ProdDate = rd["ProdDate"].ToString().Trim();
                        obj.ShiftCode = rd["ShiftCode"].ToString().Trim();
                        result.Add(obj);
                    }
                    rd.Close();
                    cmd.Dispose();
                    return result;
                }
            }
        }

        public static int InsertPrevValue(clsSPCResult Result, string ItemCheckCodeFrom)
        {
            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_SPCResult_InsPrevValue", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("FactoryCode", Result.FactoryCode);
                cmd.Parameters.AddWithValue("ItemTypeCode", Result.ItemTypeCode);
                cmd.Parameters.AddWithValue("Line", Result.LineCode);
                cmd.Parameters.AddWithValue("ItemCheckCodeFrom", ItemCheckCodeFrom);
                cmd.Parameters.AddWithValue("ItemCheckCode", Result.ItemCheckCode);
                cmd.Parameters.AddWithValue("ProdDate", Result.ProdDate);
                cmd.Parameters.AddWithValue("ShiftCode", Result.ShiftCode);
                cmd.Parameters.AddWithValue("SequenceNo", Result.SequenceNo);
                cmd.Parameters.AddWithValue("RegisterUser", Result.RegisterUser);
                cmd.Parameters.AddWithValue("RegisterNo", Result.RegisterNo);

                cmd.Parameters.Add("SPCResultID", SqlDbType.Int);
                cmd.Parameters["SPCResultID"].Direction = ParameterDirection.Output;
                int i = cmd.ExecuteNonQuery();
                Result.SPCResultID = Convert.ToInt32(cmd.Parameters["SPCResultID"].Value);
                cmd.Dispose();
                return i;
            }
        }
        public static int Insert(clsSPCResult Result)
        {
            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_SPCResult_Ins", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("FactoryCode", Result.FactoryCode);
                cmd.Parameters.AddWithValue("ItemTypeCode", Result.ItemTypeCode);
                cmd.Parameters.AddWithValue("LineCode", Result.LineCode);
                cmd.Parameters.AddWithValue("ItemCheckCode", Result.ItemCheckCode);
                cmd.Parameters.AddWithValue("ProdDate", Result.ProdDate);
                cmd.Parameters.AddWithValue("ShiftCode", Result.ShiftCode);
                cmd.Parameters.AddWithValue("SequenceNo", Result.SequenceNo);
                cmd.Parameters.AddWithValue("SubLotNo", Result.SubLotNo);
                cmd.Parameters.AddWithValue("Remark", Result.Remark);
                cmd.Parameters.AddWithValue("RegisterUser", Result.RegisterUser);
                cmd.Parameters.AddWithValue("RegisterNo", Result.RegisterNo);
                cmd.Parameters.AddWithValue("Measure2nd", Result.Measure2nd);

                cmd.Parameters.Add("SPCResultID", SqlDbType.Int);
                cmd.Parameters["SPCResultID"].Direction = ParameterDirection.Output;                
                int i = cmd.ExecuteNonQuery();
                Result.SPCResultID = Convert.ToInt32(cmd.Parameters["SPCResultID"].Value);
                cmd.Dispose();
                return i;
            }            
        }

        public static clsSPCResultSample GetSampleSize(int SPCResultID)
        {
            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_SPCResult_GetComplete", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("SPCResultID", SPCResultID);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                if(dt.Rows.Count == 0)
                {
                    return null;
                } else
                {
                    clsSPCResultSample result = new clsSPCResultSample();
                    result.SPCResultID = SPCResultID;
                    result.SampleSize = Convert.ToInt16(dt.Rows[0]["SampleSize"]);
                    result.ActualSample = Convert.ToInt16(dt.Rows[0]["ActualSample"]);                    
                    return result;
                }                
            }
        }

        public static bool AllowSkill(string UserID, string FactoryCode, string LineCode, string ItemTypeCode)
        {
            using (SqlConnection con = new SqlConnection(constr))
            {
                string q =
                    "SELECT MES.EmployeeID FROM MS_Skill MS \n" +
                    "Join MS_MachineSkillSetting MM ON MS.SkillCode = MM.SkillCode \n" +
                    "Join MS_EmployeeSkill MES ON MM.SkillCode = MES.SkillCode \n" +
                    "Join MS_Employee ME ON ME.EmployeeID = MES.EmployeeID \n" +
                    "Join spc_UserSetup U ON ME.EmployeeID = U.EmployeeID \n" +
                    "WHERE MM.FactoryCode = @FactoryCode AND MM.LineCode = @LineCode AND MS.SkillCode = @SkillCode AND U.UserID = @UserID \n" +
                    "And CAST(GETDATE() As Date) BETWEEN CAST(MES.StartDate AS DATE) And CAST(MES.EndDate AS DATE) \n";
                con.Open();
                SqlCommand cmd = new SqlCommand(q, con);
                cmd.Parameters.AddWithValue("FactoryCode", FactoryCode);
                cmd.Parameters.AddWithValue("LineCode", LineCode);
                cmd.Parameters.AddWithValue("UserID", UserID);
                cmd.Parameters.AddWithValue("SkillCode", "SPC001");
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt.Rows.Count > 0;
            }
        }

        public static int Update(clsSPCResult Result)
        {
            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_SPCResult_Upd", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("FactoryCode", Result.FactoryCode);
                cmd.Parameters.AddWithValue("ItemTypeCode", Result.ItemTypeCode);
                cmd.Parameters.AddWithValue("LineCode", Result.LineCode);
                cmd.Parameters.AddWithValue("ItemCheckCode", Result.ItemCheckCode);
                cmd.Parameters.AddWithValue("ProdDate", Result.ProdDate);
                cmd.Parameters.AddWithValue("ShiftCode", Result.ShiftCode);
                cmd.Parameters.AddWithValue("SequenceNo", Result.SequenceNo);
                cmd.Parameters.AddWithValue("SubLotNo", Result.SubLotNo);
                cmd.Parameters.AddWithValue("Remark", Result.Remark);
                cmd.Parameters.AddWithValue("RegisterUser", Result.RegisterUser);
                cmd.Parameters.AddWithValue("RegisterNo", Result.RegisterNo);
                int i = cmd.ExecuteNonQuery();
                cmd.Dispose();
                return i;
            }
        }

        public static bool ChartSetupExists(string FactoryCode, string ItemTypeCode, string LineCode, string ItemCheckCode, string ProdDate)
        {
            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_SPC_ChartSetup", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("FactoryCode", FactoryCode);
                cmd.Parameters.AddWithValue("ItemTypeCode", ItemTypeCode);
                cmd.Parameters.AddWithValue("LineCode", LineCode);
                cmd.Parameters.AddWithValue("ItemCheckCode", ItemCheckCode);
                cmd.Parameters.AddWithValue("ProdDate", ProdDate);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt.Rows.Count > 0;
            }
        }        
    }
}
